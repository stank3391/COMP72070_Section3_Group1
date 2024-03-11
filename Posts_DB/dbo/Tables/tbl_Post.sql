CREATE TABLE [dbo].[tbl_Post] (
    [PostId]  INT            NOT NULL,
    [UserId]  INT            NOT NULL,
    [Type]    NVARCHAR (255) NULL,
    [Content] CHAR (255)     NULL,
    [Date]    DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([PostId] ASC),
    CHECK ([Type]='image' OR [Type]='text'),
    CONSTRAINT [FK_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[tbl_Users] ([UserId])
);

