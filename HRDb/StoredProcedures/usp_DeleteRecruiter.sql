CREATE PROCEDURE [dbo].[usp_DeleteRecruiter]
	@id int
AS
	BEGIN TRANSACTION
		UPDATE dbo.Candidate
		set Recruiter_Id=NULL
		where Recruiter_Id=@id

		DELETE from dbo.Recruiter
		where id=@id

	COMMIT
