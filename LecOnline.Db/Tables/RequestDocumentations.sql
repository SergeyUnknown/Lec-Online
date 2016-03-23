CREATE TABLE [dbo].[RequestDocumentations]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	RequestId int NOT NULL,
	Name nvarchar(256) not null,
	FileType tinyint not null default(0),
	Content varbinary(max) not null,
	[Created] datetime not null,
	[CreatedBy] NVARCHAR (32) not null,
	Signed datetime,
	SignedBy NVARCHAR (32),

	CONSTRAINT [FK_RequestDocumentations_ToRequests] FOREIGN KEY (RequestId) REFERENCES Requests(Id), 
    CONSTRAINT [FK_RequestDocumentations_ToUsers_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [AspNetUsers](UserName),
	CONSTRAINT [FK_RequestDocumentations_ToUsers_SignedBy] FOREIGN KEY (SignedBy) REFERENCES [AspNetUsers](UserName),
)
