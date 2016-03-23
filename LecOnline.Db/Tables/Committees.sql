CREATE TABLE [dbo].[Committees]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	Name nvarchar(100) not null,
	Notes nvarchar(max),
	Chairman NVARCHAR (128) NULL,
	ViceChairman NVARCHAR (128) NULL,
	Secretary NVARCHAR (128) NULL, 
	City NVARCHAR (128) NULL, 

    CONSTRAINT [FK_Committees_ToAspNetUsers_Chariman] FOREIGN KEY (Chairman) REFERENCES AspNetUsers(Id),
	CONSTRAINT [FK_Committees_ToAspNetUsers_ViceChariman] FOREIGN KEY (ViceChairman) REFERENCES AspNetUsers(Id),
	CONSTRAINT [FK_Committees_ToAspNetUsers_Secretary] FOREIGN KEY (Secretary) REFERENCES AspNetUsers(Id),
)
