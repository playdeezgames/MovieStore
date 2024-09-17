CREATE TABLE [dbo].[GenreTitles]
(
	[GenreTitleId] INT NOT NULL IDENTITY(1,1) CONSTRAINT PK_GenreTitles PRIMARY KEY,
	[GenreId] INT NOT NULL CONSTRAINT FK_GenreTitles_Genres FOREIGN KEY REFERENCES [Genres]([GenreId]),
	[TitleId] INT NOT NULL CONSTRAINT FK_GenreTitles_Titles FOREIGN KEY REFERENCES [Titles]([TitleId]),
	CONSTRAINT AK_GenreTitles_GenreId_TitleId UNIQUE([GenreId],[TitleId])
)
