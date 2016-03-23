CREATE TABLE [dbo].[ErrorLog]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	UserName nvarchar(32) null,
	Created datetime not null default(getutcdate()),
	ErrorMessage nvarchar(max),
)
