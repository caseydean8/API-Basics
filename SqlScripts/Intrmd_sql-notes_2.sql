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

SELECT * FROM TutorialAppSchema.UserJobInfo
    ORDER BY UserId DESC

DELETE FROM TutorialAppSchema.UserJobInfo
    WHERE UserId IS NULL 


SELECT * FROM TutorialAppSchema.Users
    ORDER BY UserId DESC









