-- Add Primary key
CREATE TABLE [TutorialAppSchema].[UserJobInfo] (
    [UserId]     INT           NOT NULL,
    [JobTitle]   NVARCHAR (50) NULL,
    [Department] NVARCHAR (50) NULL,
    CONSTRAINT [PK_UserJobInfo] PRIMARY KEY CLUSTERED ([UserId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_UserJobInfo_JobTitle]
    ON [TutorialAppSchema].[UserJobInfo]([JobTitle] ASC)
    INCLUDE([Department]);
