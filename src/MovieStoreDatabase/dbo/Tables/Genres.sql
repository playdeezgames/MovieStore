﻿CREATE TABLE [dbo].[Genres]
(
	[GenreId] INT NOT NULL IDENTITY(1,1) CONSTRAINT PK_Genres PRIMARY KEY,
	[GenreName] NVARCHAR(50) NOT NULL CONSTRAINT AK_Genres_GenreName UNIQUE
)
