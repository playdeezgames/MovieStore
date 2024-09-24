using System.Data;

namespace MovieStore
{
    internal static class Titles
    {
        internal static int Count(IDbConnection connection)
        {
            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT COUNT(1) FROM [{Constants.TitlesTable}];";
            var result = command.ExecuteScalar();
            if (result==null)
            {
                return 0;
            }
            return (int)result;
        }
        internal static IEnumerable<TitleListItem> Filter(IDbConnection connection, string filter)
        {
            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT [{Constants.TitleIdColumn}],[{Constants.TitleNameColumn}],[{Constants.TitleYearColumn}],[{Constants.MediaTypeIdColumn}],[{Constants.MediaTypeNameColumn}],[{Constants.IMDBIDColumn}],[{Constants.GenreCountColumn}] FROM [{Constants.TitleListItemsView}] WHERE [{Constants.TitleNameColumn}] LIKE @{Constants.TitleNameColumn} ORDER BY [{Constants.TitleNameColumn}];";
            var parameter = command.CreateParameter();
            parameter.ParameterName = $"@{Constants.TitleNameColumn}";
            parameter.Value = filter;
            command.Parameters.Add(parameter);
            using var reader = command.ExecuteReader();
            List<TitleListItem> result = [];
            while (reader.Read())
            {
                result.Add(new TitleListItem 
                {
                    TitleId=reader.GetInt32(0),
                    TitleName=reader.GetString(1),
                    TitleYear= reader.IsDBNull(2) ? null : reader.GetInt32(2),
                    MediaTypeId=reader.GetInt32(3),
                    MediaTypeName=reader.GetString(4),
                    IMDBID=reader.GetString(5),
                    GenreCount=reader.GetInt32(6)
                });
            }
            return result;
        }
    }
}
