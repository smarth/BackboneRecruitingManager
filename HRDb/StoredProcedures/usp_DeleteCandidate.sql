CREATE PROCEDURE [dbo].[usp_DeleteCandidate]
	@id int
AS
	DELETE from dbo.Candidate
	where id=@id

