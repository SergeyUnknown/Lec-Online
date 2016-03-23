CREATE TABLE [dbo].[AspNetUserRoles]
(
    [UserId] NVARCHAR (128) NOT NULL,
    [RoleId] NVARCHAR (128) NOT NULL,

    CONSTRAINT [PK_AspNetUserRoles_UserID_RoleID] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_AspNetUserRoles_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].AspNetUsers (Id) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_UserRole] FOREIGN KEY ([RoleId]) REFERENCES [dbo].AspNetRoles (Id) ON DELETE CASCADE
);

GO
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_UserID]
    ON [dbo].[AspNetUserRoles] ([UserId] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleID]
    ON [dbo].[AspNetUserRoles] ([RoleId] ASC);