CREATE TABLE [dbo].[Configuration]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(40) NOT NULL, 
    [Value] NVARCHAR(MAX) NULL
)
