CREATE PROCEDURE [dbo].[usp_CreateCandidate]
	@name nvarchar(255),
	@recruiter_id int = NULL
AS
	BEGIN TRANSACTION
		Insert INTO dbo.Candidate (Name,Recruiter_Id)
		VALUES
		(@name,@recruiter_id)

		SELECT SCOPE_IDENTITY() as ID

	COMMIT


