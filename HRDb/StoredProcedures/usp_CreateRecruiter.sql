CREATE PROCEDURE [dbo].[usp_CreateRecruiter]
	@name nvarchar(255)
AS
	BEGIN TRANSACTION
		Insert INTO dbo.Recruiter (Name)
		VALUES
		(@name)

		SELECT SCOPE_IDENTITY() as ID
	COMMIT