CREATE TABLE [dbo].[Clients]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	CompanyName nvarchar(100) not null,
	ContactPerson nvarchar(200),
	ContactEmail nvarchar(100),
	ContactPhone nvarchar(100),
	Notes nvarchar(max),
)
