CREATE TABLE [dbo].[tbl_Post] (
    [PostId]  INT            NOT NULL,
    [UserId]  INT            NOT NULL,
    [Type]    NVARCHAR (255) NULL,
    [Content] CHAR (255)     NULL,
    [Date]    DATETIME       NULL,
    CONSTRAINT [PK_dbo.tbl_Post] PRIMARY KEY CLUSTERED ([PostId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[tbl_Post]([UserId] ASC);

