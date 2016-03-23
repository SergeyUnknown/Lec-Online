CREATE TABLE [dbo].[AspNetUserLogins]
(
    [UserId]        NVARCHAR (128) NOT NULL,
    [LoginProvider] NVARCHAR (128) NOT NULL,
    [ProviderKey]   NVARCHAR (128) NOT NULL,

    CONSTRAINT [PK_AspNetUserLogins_UserID_LoginProvider_ProviderKey] PRIMARY KEY CLUSTERED ([UserId] ASC, [LoginProvider] ASC, [ProviderKey] ASC),
    CONSTRAINT [FK_AspNetUserLogins_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].AspNetUsers (Id) ON DELETE CASCADE
);

GO
CREATE NONCLUSTERED INDEX [IX_UserLogin_UserID]
    ON [dbo].[AspNetUserLogins] ([UserId] ASC);