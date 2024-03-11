CREATE TABLE [dbo].[tbl_Users] (
    [UserId]     INT            NOT NULL,
    [Username]   VARCHAR (255)  NOT NULL,
    [Email]      VARCHAR (255)  NULL,
    [ProfilePic] VARCHAR (255)  NULL,
    [JoinDate]   DATETIME       NULL,
    [Role]       NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC),
    CHECK ([Role]='user' OR [Role]='admin')
);

