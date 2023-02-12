


SELECT [UserId],
    [JobTitle],
    [Department]
FROM TutorialAppSchema.UserJobInfo

UPDATE TutorialAppSchema.Users
            SET
                [JobTitle] = 'Rip off guy'
                , [Department] = 'Stealing'
                 WHERE UserId = 3
INSERT INTO TutorialAppSchema.UserJobInfo
    (
    [JobTitle],
    [Department])
VALUES
    (
        'programmer', 'righteous')

SELECT *
FROM TutorialAppSchema.UserJobInfo
ORDER BY UserId DESC

USE DotNetCourseDatabase
Go

CREATE TABLE [TutorialAppSchema].[UserJobInfo](
	[UserId] [int] NOT NULL,
	[JobTitle] [nvarchar](50) NULL,
	[Department] [nvarchar](50) NULL
)

ALTER TABLE TutorialAppSchema.UserJobInfo 
alter COLUMN UserId INT NULL
-- ADD PRIMARY KEY (UserId)

DELETE FROM TutorialAppSchema.UserJobInfo
    WHERE UserId IS NULL

ALTER TABLE TutorialAppSchema.UserJobInfo DROP COLUMN UserId 
ALTER TABLE TutorialAppSchema.UserJobInfo Add UserId INT IDENTITY(1,1)

ALTER TABLE TutorialAppSchema.UserJobInfo MODIFY UserId INT NOT NULL

ALTER TABLE TutorialAppSchema.UserJobInfo DROP PRIMARY KEY

ALTER TABLE TutorialAppSchema.UserJobInfo DROP CONSTRAINT PK_Tutorial


SELECT * FROM TutorialAppSchema.UserJobInfo
    -- WHERE UserId = 112
    WHERE UserId (105, 115)


