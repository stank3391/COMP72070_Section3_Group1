CREATE TABLE [dbo].[tbl_Users] (
    [UserId]     INT            IDENTITY (1, 1) NOT NULL,
    [Username]   VARCHAR (255)  NOT NULL,
    [Email]      VARCHAR (255)  NULL,
    [ProfilePic] VARCHAR (255)  NULL,
    [JoinDate]   DATETIME       NULL,
    [Role]       NVARCHAR (255) NULL,
    CONSTRAINT [PK_dbo.tbl_Users] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

