CREATE TABLE [dbo].[RequestActions]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	RequestId int not null,
	ActionType tinyint not null,
	Description nvarchar(200) not null,
	UserName NVARCHAR (256) not null,
	ActionDate datetime not null default(getutcdate())

	CONSTRAINT [FK_RequestActions_ToRequest] FOREIGN KEY (RequestId) REFERENCES Requests(Id),
)
