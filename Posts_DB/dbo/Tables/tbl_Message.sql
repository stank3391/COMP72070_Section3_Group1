CREATE TABLE [dbo].[tbl_Message] (
    [MessageId]  INT           NOT NULL,
    [SenderId]   INT           NOT NULL,
    [ReceiverId] INT           NOT NULL,
    [Content]    VARCHAR (255) NULL,
    [Date]       DATETIME      NULL,
    CONSTRAINT [PK_dbo.tbl_Message] PRIMARY KEY CLUSTERED ([MessageId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_SenderId]
    ON [dbo].[tbl_Message]([SenderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ReceiverId]
    ON [dbo].[tbl_Message]([ReceiverId] ASC);

