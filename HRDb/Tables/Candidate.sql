CREATE TABLE [dbo].[Candidate]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] nvarchar(255) NOT NULL,
	[Recruiter_Id] INT FOREIGN KEY REFERENCES Recruiter(Id)
)
