CREATE TABLE [dbo].[tbl_Token] (
    [TokenId]        INT            NOT NULL,
    [UserId]         INT            NOT NULL,
    [Provider]       NVARCHAR (255) NULL,
    [AccessToken]    CHAR (255)     NULL,
    [RefreshToken]   CHAR (255)     NULL,
    [CreationDate]   DATETIME       NULL,
    [ExpirationDate] DATETIME       NULL,
    CONSTRAINT [PK_dbo.tbl_Token] PRIMARY KEY CLUSTERED ([TokenId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[tbl_Token]([UserId] ASC);

