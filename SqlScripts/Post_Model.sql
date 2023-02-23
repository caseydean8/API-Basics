






USE DotNetCourseDatabase
GO







CREATE TABLE TutorialAppSchema.Posts
(
    -- (starts at 1, increments by 1)
    PostId INT IDENTITY(1,1),
    UserId INT,
    PostTitle NVARCHAR(255),
    PostContent NVARCHAR(MAX),
    PostCreated DATETIME,
    PostUpdated DATETIME
)
-- Sort by UserId first and then PostId
CREATE CLUSTERED INDEX cix_Posts_UserId_PostId ON TutorialAppSchema.Posts(UserId, PostId)

SELECT *
FROM TutorialAppSchema.Posts

UPDATE TutorialAppSchema.Posts 
    SET UserId = 1002
    WHERE PostId = 1

UPDATE TutorialAppSchema.Posts 
    SET PostContent = '', PostTitle = '', PostUpdated = GETDATE()
    WHERE PostId = 4

DELETE FROM TutorialAppSchema.Posts WHERE PostId = ''

SELECT *
FROM TutorialAppSchema.Posts
WHERE PostTitle LIKE '%search%'
    OR PostContent LIKE '%search%'