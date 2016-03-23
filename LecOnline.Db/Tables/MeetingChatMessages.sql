CREATE TABLE [dbo].[MeetingChatMessages]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	MeetingId int not null,
	UserId nvarchar(128) not null,
	SentDate datetime not null,
	Message nvarchar(1000),

    CONSTRAINT [FK_MeetingChatMessages_ToMeetings] FOREIGN KEY (MeetingId) REFERENCES Meetings(Id),
	CONSTRAINT [FK_MeetingChatMessages_ToAspNetUsers] FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id),
)
