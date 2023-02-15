-- Syntax to add to two tables with one transaction.
-- https://msdn.microsoft.com/en-us/library/ms188792.aspx
SET XACT_ABORT ON;

BEGIN TRANSACTION
   DECLARE @DataID int;
   INSERT INTO DataTable (Column1 ...) VALUES (....);
   SELECT @DataID = scope_identity();
   INSERT INTO LinkTable VALUES (@ObjectID, @DataID);
COMMIT

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
      -- Can add values here, just used NULL here because I was setting the auto increment UserId
      -- in Users table and then inserting UserId into UserJobInfo table.
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
