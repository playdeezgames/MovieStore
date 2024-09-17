using CsvHelper;
using Importer;
using Microsoft.Data.SqlClient;
using System.Globalization;

const string InitialDataFilename = "InitialData.csv";
const string DatabaseConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=MovieStore;Integrated Security=True;Trust Server Certificate=True;";

var initialData = ReadInitialDataFromCsv();
using var connection = new SqlConnection(DatabaseConnectionString);
connection.Open();
ClearGenres(connection);
ClearTitles(connection);
ClearMediaTypes(connection);
using SqlCommand command = ClearTitles(connection);
WriteMediaTypes(connection, initialData);
WriteGenres(connection, initialData);
WriteToDatabase(initialData, connection, ReadGenreTable(connection), ReadMediaTypeTable(connection));
connection.Close();

static List<InitialDataSchema> ReadInitialDataFromCsv()
{
    var result = new List<InitialDataSchema>();
    using var reader = new StreamReader(InitialDataFilename);
    using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
    var records = csv.GetRecords<InitialDataSchema>();
    foreach (var record in records)
    {
        result.Add(record);
    }
    return result;
}

static void WriteToDatabase(List<InitialDataSchema> initialData, SqlConnection connection,IReadOnlyDictionary<string, int> genreTable, IReadOnlyDictionary<string, int> mediaTypeTable)
{
    foreach (var row in initialData)
    {
        var titleId = WriteToTitles(connection, row, mediaTypeTable);
        WriteToGenreTitles(connection, row, titleId, genreTable);
    }
}

static void WriteToGenreTitles(SqlConnection connection, InitialDataSchema row, int titleId, IReadOnlyDictionary<string, int> genreTable)
{
    var tokens = row.Genre.Split(", ");
    foreach(var token in tokens)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO [GenreTitles]([TitleId],[GenreId]) VALUES(@TitleId,@GenreId);";
        command.Parameters.AddWithValue("@TitleId", titleId);
        command.Parameters.AddWithValue("@GenreId", genreTable[token]);
    }
}

static int WriteToTitles(SqlConnection connection, InitialDataSchema row, IReadOnlyDictionary<string, int> mediaTypeTable)
{
    using var command = connection.CreateCommand();
    command.CommandText = "INSERT INTO [Titles]([TitleName],[TitleYear],[IMDBID],[MediaTypeId]) OUTPUT INSERTED.TitleId VALUES(@TitleName,@TitleYear,@IMDBID,@MediaTypeId);";
    command.Parameters.AddWithValue("@TitleName", row.Title);
    command.Parameters.AddWithValue("@TitleYear", row.Year.HasValue ? row.Year.Value : DBNull.Value);
    command.Parameters.AddWithValue("@IMDBID", row.IMDBID);
    command.Parameters.AddWithValue("@MediaTypeId", mediaTypeTable[row.MediaType]);
    return (int)command.ExecuteScalar();
}

static SqlCommand ClearTitles(SqlConnection connection)
{
    var command = connection.CreateCommand();
    command.CommandText = "DELETE FROM [Titles];";
    command.ExecuteNonQuery();
    return command;
}

static SqlCommand ClearMediaTypes(SqlConnection connection)
{
    var command = connection.CreateCommand();
    command.CommandText = "DELETE FROM [MediaTypes];";
    command.ExecuteNonQuery();
    return command;
}


static SqlCommand ClearGenreTitles(SqlConnection connection)
{
    var command = connection.CreateCommand();
    command.CommandText = "DELETE FROM [GenreTitles];";
    command.ExecuteNonQuery();
    return command;
}
static SqlCommand ClearGenres(SqlConnection connection)
{
    ClearGenreTitles(connection);
    var command = connection.CreateCommand();
    command.CommandText = "DELETE FROM [Genres];";
    command.ExecuteNonQuery();
    return command;
}

static void WriteMediaTypes(SqlConnection connection, List<InitialDataSchema> initialData)
{
    var mediaTypes = initialData.GroupBy(x => x.MediaType);
    foreach (var mediaType in mediaTypes)
    {
        var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO [MediaTypes]([MediaTypeName]) VALUES(@MediaTypeName);";
        command.Parameters.AddWithValue("@MediaTypeName", mediaType.Key);
        command.ExecuteNonQuery();
    }
}

static void WriteGenres(SqlConnection connection, List<InitialDataSchema> initialData)
{
    var genres = new HashSet<string>();
    foreach(var item in initialData)
    {
        var tokens = item.Genre.Split(", ");
        foreach (var token in tokens)
        {
            genres.Add(token);
        }
    }
    foreach (var genre in genres)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO [Genres]([GenreName]) VALUES(@GenreName);";
        command.Parameters.AddWithValue("@GenreName", genre);
        command.ExecuteNonQuery();
    }
}

static IReadOnlyDictionary<string, int> ReadGenreTable(SqlConnection connection)
{
    using var command = connection.CreateCommand();
    command.CommandText = "SELECT [GenreId],[GenreName] FROM [Genres];";
    using var reader = command.ExecuteReader();
    Dictionary<string, int> result = [];
    while (reader.Read())
    {
        result[reader.GetString(1)] = reader.GetInt32(0);
    }
    return result;
}

static IReadOnlyDictionary<string, int> ReadMediaTypeTable(SqlConnection connection)
{
    using var command = connection.CreateCommand();
    command.CommandText = "SELECT [MediaTypeId],[MediaTypeName] FROM [MediaTypes];";
    using var reader = command.ExecuteReader();
    Dictionary<string, int> result = [];
    while (reader.Read())
    {
        result[reader.GetString(1)] = reader.GetInt32(0);
    }
    return result;
}
