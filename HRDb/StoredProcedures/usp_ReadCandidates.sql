CREATE PROCEDURE [dbo].[usp_ReadCandidates]
	@recruiter_id int = NULL,
	@candidate_id int = NULL
AS
	IF @recruiter_id IS NULL
	BEGIN
		IF @candidate_id IS NULL
		BEGIN
			SELECT C.Id as CandidateID,C.Name as CandidateName,R.ID as RecruiterID,R.Name as RecruiterName
			FROM Candidate C
			LEFT OUTER JOIN Recruiter R ON R.Id = C.Recruiter_Id
		END
		ELSE
		BEGIN
			SELECT C.Id as CandidateID,C.Name as CandidateName,R.ID as RecruiterID,R.Name as RecruiterName
			FROM Candidate C
			LEFT OUTER JOIN Recruiter R ON R.Id = C.Recruiter_Id
			WHERE C.Id = @candidate_id
		END
	END
	ELSE
	BEGIN
			SELECT C.Id as CandidateID,C.Name as CandidateName,R.ID as RecruiterID,R.Name as RecruiterName
			FROM Candidate C
			LEFT OUTER JOIN Recruiter R ON R.Id = C.Recruiter_Id
			WHERE
			C.Recruiter_Id=R.Id	
	END



