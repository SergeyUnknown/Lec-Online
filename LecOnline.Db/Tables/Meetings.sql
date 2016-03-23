CREATE TABLE [dbo].[Meetings]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	RequestId int not null,
	MeetingDate datetime not null,
	Status int not null default(0),
	Protocol nvarchar(max),
	MeetingLog nvarchar(max), 
	Resolution int not null default(0),

    CONSTRAINT [FK_Meetings_ToRequests] FOREIGN KEY (RequestId) REFERENCES Requests(Id),
)
