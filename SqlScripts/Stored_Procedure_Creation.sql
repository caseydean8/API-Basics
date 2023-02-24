





USE DotNetCourseDatabase
GO
-- use of GO as a separator so that the USE statement isn't added to next query

-- CREATE PROCEDURE TutorialAppSchema.sp_ -- don't use underscore, sql will think it's a system stored procedure

-- naming system (not necessarily best practice) Object_Action
-- CREATE PROCEDURE TutorialAppSchema.spUsers_Get -- CREATE PROCEDURE can only be run once for procedure name.
-- To change PROCEDURE use "ALTER"
ALTER PROCEDURE TutorialAppSchema.spUsers_Get
-- EXEC TutorialAppSchema.spUsers_Get -- used to call procedure
AS
BEGIN
    SELECT [Users].[UserId],
        [Users].[FirstName],
        [Users].[LastName],
        [Users].[Email],
        [Users].[Gender],
        [Users].[Active]
    FROM TutorialAppSchema.Users AS Users
END

-- Don't put anything after END, it will be picked up by stored procedure. 
-- "END" is "metaphorical"/documentary