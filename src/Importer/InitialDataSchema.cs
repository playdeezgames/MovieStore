using CsvHelper.Configuration.Attributes;

namespace Importer
{
    [CultureInfo("en-US")]
    public class InitialDataSchema
    {
        public DateTimeOffset Timestamp { get; set; }
        [Name("Media Type")]
        public required string MediaType { get; set; }
        public required string Title { get; set; }
        public int? Year { get; set; }
        public string? IMDBID { get; set; }
        public required string Genre { get; set; }
    }
}
