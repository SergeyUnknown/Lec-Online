CREATE TABLE [dbo].[MeetingAttendees]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	MeetingId int not null,
	UserId nvarchar(128) not null,
	Status tinyint NULL, 
	Vote tinyint null,

    CONSTRAINT [FK_MeetingAttendees_ToMeetings] FOREIGN KEY (MeetingId) REFERENCES Meetings(Id),
	CONSTRAINT [FK_MeetingAttendees_ToAspNetUsers] FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id),
)
