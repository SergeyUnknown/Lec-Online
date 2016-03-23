CREATE TABLE [dbo].[ChangesLog]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	ObjectType int not null,
	ObjectId int not null,
	Changed datetime not null,
	ChangedBy nvarchar(128) not null,
	ClientId int null,
	CommitteeId int null,
	ChangeDescription nvarchar(max) not null,
)
