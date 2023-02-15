USE DotNetCourseDatabase
GO

SELECT [Users].[UserId],
    -- AS FullName field alias, same syntax as table alias below
    -- [Users].[FirstName] + ' ' + [Users].[LastName] AS FullName,
    [Users].[FirstName],
    [Users].[LastName],
    [Users].[Email],
    [Users].[Gender],
    [Users].[Active]
-- AS Users TABLE alias creates a qualifed list of fields so you can use
-- other tables in query
FROM TutorialAppSchema.Users AS Users
-- WHERE clause must precede ORDER BY clause
WHERE Users.Active = 1
ORDER BY Users.UserId DESC

-- TABLE Name Users, UserJobInfo, UserSalary

SELECT * ,
    UserJobInfo. *
FROM TutorialAppSchema.Users AS Users
    JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
    ON UserJobInfo.UserId = Users.UserId

SELECT *
FROM TutorialAppSchema.UserJobInfo
ORDER BY UserId DESC

DELETE FROM TutorialAppSchema.UserJobInfo
    WHERE UserId IS NULL

SELECT IDENT_CURRENT('TutorialAppSchema.Users')

INSERT INTO TutorialAppSchema.Users
    (
    [FirstName],
    [LastName],
    [Email],
    [Gender],
    [Active])
VALUES
    (
        'test', 'tester', 't@tester.com', 'dude', 'True')



INSERT INTO TutorialAppSchema.Users
    (
    [FirstName],
    [LastName],
    [Email],
    [Gender],
    [Active])
VALUES
    (
        NULL,
        NULL,
        NULL,
        NULL,
        0
    )

SELECT *
FROM TutorialAppSchema.Users
ORDER BY UserId DESC

-- Add User row (with UserId) in Users table and then add UserJobInfo
-- fields with same UserId to UserJobInfo table
SET XACT_ABORT ON;

BEGIN TRANSACTION
DECLARE @UserID int;
INSERT INTO TutorialAppSchema.Users
    (
    [FirstName],
    [LastName],
    [Email],
    [Gender],
    [Active])
VALUES
    (
        NULL,
        NULL,
        NULL,
        NULL,
        0
    )
SELECT @UserID = scope_identity();
INSERT INTO TutorialAppSchema.UserJobInfo
    (
    [UserId],
    [JobTitle],
    [Department])
VALUES
    (
        @UserID,
        'Tester',
        'Unit Testing'
    )
COMMIT

SELECT *
FROM TutorialAppSchema.UserJobInfo
ORDER BY UserId DESC



