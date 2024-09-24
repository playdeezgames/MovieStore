namespace MovieStore
{
    internal class TitleListItem
    {
		public int TitleId { get; set; }
		public required string TitleName { get; set; }
		public int? TitleYear { get; set; }
		public int MediaTypeId { get; set; }
		public required string MediaTypeName { get; set; }
		public string? IMDBID { get; set; }
		public int GenreCount { get; set; }
    }
}
