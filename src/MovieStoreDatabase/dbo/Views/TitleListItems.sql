CREATE VIEW TitleListItems AS
SELECT
	t.[TitleId],
	t.[TitleName],
	t.[TitleYear],
	t.[MediaTypeId],
	mt.[MediaTypeName],
	t.[IMDBID],
	COUNT(gt.[GenreTitleId]) GenreCount
FROM
	[Titles] t JOIN
	[MediaTypes] mt ON mt.MediaTypeId = t.MediaTypeId JOIN
	[GenreTitles] gt ON gt.TitleId = t.TitleId
GROUP BY
	t.[TitleId],
	t.[TitleName],
	t.[TitleYear],
	t.[MediaTypeId],
	mt.[MediaTypeName],
	t.[IMDBID];