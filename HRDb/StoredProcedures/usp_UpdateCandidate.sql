CREATE PROCEDURE [dbo].[usp_UpdateCandidate]
	@name nvarchar(255),
	@id int,
	@recruiter_id int = NULL
AS
	UPDATE dbo.Candidate
	SET name=@name,
	recruiter_id=@recruiter_id
	WHERE 
	id=@id
