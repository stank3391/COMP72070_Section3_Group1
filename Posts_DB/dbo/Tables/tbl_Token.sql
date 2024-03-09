CREATE TABLE [dbo].[tbl_Token] (
    [TokenId]        INT            NOT NULL,
    [UserId]         INT            NOT NULL,
    [Provider]       NVARCHAR (255) NULL,
    [AccessToken]    CHAR (255)     NULL,
    [RefreshToken]   CHAR (255)     NULL,
    [CreationDate]   DATETIME       NULL,
    [ExpirationDate] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([TokenId] ASC),
    CONSTRAINT [TokenUserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[tbl_Users] ([UserId])
);

