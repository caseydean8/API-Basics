





USE DotNetCourseDatabase
GO
-- use of GO as a separator so that the USE statement isn't added to next query

-- CREATE PROCEDURE TutorialAppSchema.sp_ -- don't use underscore, sql will think it's a system stored procedure

-- naming system (not necessarily best practice) Object_Action
-- CREATE PROCEDURE TutorialAppSchema.spUsers_Get -- CREATE PROCEDURE can only be run once for procedure name.
-- To change PROCEDURE use "ALTER"
ALTER PROCEDURE TutorialAppSchema.spUsers_Get
    -- EXEC TutorialAppSchema.spUsers_Get -- used to call procedure
    -- EXEC TutorialAppSchema.spUsers_Get 3 -- works but becomes confusing with multiple variables
    -- EXEC TutorialAppSchema.spUsers_Get @UserId=3 -- same as above but more clear
    -- @ creates a variable in Sql. Make sure to declare type or sql will implicitly convert and slow down query.
    @UserId INT = NULL
AS
BEGIN
    -- SELECT UserJobInfo.Department
    --     , AVG(UserSalary.Salary)
    -- FROM TutorialAppSchema.Users AS Users
    --     LEFT JOIN TutorialAppSchema.UserSalary AS UserSalary
    --     ON UserSalary.UserId = Users.UserId
    --     LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
    --     ON UserJobInfo.UserId = Users.UserId
    -- GROUP BY UserJobInfo.Department

    SELECT [Users].[UserId],
        [Users].[FirstName],
        [Users].[LastName],
        [Users].[Email],
        [Users].[Gender],
        [Users].[Active],
        UserSalary.Salary,
        UserJobInfo.Department,
        UserJobInfo.JobTitle,
        AvgSalary.AvgSalary
    FROM TutorialAppSchema.Users AS Users
        LEFT JOIN TutorialAppSchema.UserSalary AS UserSalary
        ON UserSalary.UserId = Users.UserId
        LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
        ON UserJobInfo.UserId = Users.UserId
        OUTER APPLY (
            SELECT AVG(UserSalary2.Salary) AvgSalary
        FROM TutorialAppSchema.Users AS Users
            LEFT JOIN TutorialAppSchema.UserSalary AS UserSalary2
            ON UserSalary2.UserId = Users.UserId
            LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo2
            ON UserJobInfo2.UserId = Users.UserId
            WHERE UserJobInfo2.Department = UserJobInfo.Department
        GROUP BY UserJobInfo2.Department
        ) AS AvgSalary
    WHERE Users.UserId = ISNULL (@UserId, Users.UserId)
END

-- Don't put anything after END, it will be picked up by stored procedure. 
-- "END" is "metaphorical"/documentary