





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
    @UserId INT
AS
BEGIN
    SELECT [Users].[UserId],
        [Users].[FirstName],
        [Users].[LastName],
        [Users].[Email],
        [Users].[Gender],
        [Users].[Active]
    FROM TutorialAppSchema.Users AS Users
    WHERE Users.UserId = @UserId
END

-- Don't put anything after END, it will be picked up by stored procedure. 
-- "END" is "metaphorical"/documentary