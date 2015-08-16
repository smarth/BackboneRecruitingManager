﻿/*
Deployment script for HRDB

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "HRDB"
:setvar DefaultFilePrefix "HRDB"
:setvar DefaultDataPath ""
:setvar DefaultLogPath ""

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Altering [dbo].[usp_ReadCandidates]...';


GO
ALTER PROCEDURE [dbo].[usp_ReadCandidates]
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
GO
PRINT N'Update complete.';


GO