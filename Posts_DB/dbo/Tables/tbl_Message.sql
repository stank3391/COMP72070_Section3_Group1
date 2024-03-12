CREATE TABLE [dbo].[tbl_Message] (
    [MessageId]  INT           NOT NULL,
    [SenderId]   INT           NOT NULL,
    [ReceiverId] INT           NOT NULL,
    [Content]    VARCHAR (255) NULL,
    [Date]       DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([MessageId] ASC),
    CONSTRAINT [ReceiverId] FOREIGN KEY ([ReceiverId]) REFERENCES [dbo].[tbl_Users] ([UserId]),
    CONSTRAINT [SenderId] FOREIGN KEY ([SenderId]) REFERENCES [dbo].[tbl_Users] ([UserId])
);

