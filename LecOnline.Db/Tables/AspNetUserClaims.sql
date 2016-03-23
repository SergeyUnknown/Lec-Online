CREATE TABLE [dbo].[AspNetUserClaims]
(
    [Id]    INT IDENTITY (1000,1) NOT NULL,
    [UserId]     NVARCHAR (128)        NOT NULL,
    [ClaimType]  NVARCHAR (MAX)        NULL,
    [ClaimValue] NVARCHAR (MAX)        NULL,

    CONSTRAINT [PK_AspNetUserClaims_ClaimID] PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT [FK_AspNetUserClaims_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].AspNetUsers (Id) ON DELETE CASCADE
);

GO
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserID]
    ON [dbo].AspNetUserClaims ([UserId] ASC);