CREATE PROCEDURE [dbo].[usp_UpdateRecruiter]
	@name nvarchar(255),
	@id int
AS
	UPDATE dbo.Recruiter
	SET name=@name
	WHERE 
	id=@id