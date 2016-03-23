PRINT 'Creating admin user'

/* Default password for user - !23QweAsd */
MERGE INTO [dbo].AspNetUsers AS Target
USING (VALUES
	('d9317a83-834b-46c6-8df2-2c1438adf3af', NULL, NULL, N'admin@lec-online.ru', N'Админ', N'Админович', N'Админенко'))
	AS Source (Id, ClientId, CommitteeId, [Email], FirstName, PatronymicName, LastName)
	ON Target.Id = Source.Id
WHEN MATCHED 
THEN
	UPDATE 
	SET	[UserName] = Source.Email,
		Email = Source.Email,
		FirstName = Source.FirstName,
		PatronymicName = Source.PatronymicName,
		LastName = Source.LastName,
		ClientId = Source.ClientId,
		CommitteeId = Source.CommitteeId
WHEN NOT MATCHED BY TARGET 
THEN
	INSERT (Id, 
		[UserName], 
		Email, 
		EmailConfirmed, 
		PasswordHash, 
		SecurityStamp,
		PhoneNumberConfirmed,
		TwoFactorEnabled,
		LockoutEnabled,
		AccessFailedCount,
		ClientId,
		CommitteeId,
		FirstName,
		PatronymicName,
		LastName)
	VALUES (Id, 
		[Email], 
		Email, 
		1, 
		'AKG5StHZy2X2GxHPFWXI0A439bhO5CPsBgluXCSxwEL22TUt+GvA9/c6WONi99BKsQ==',
		'7e8fe0f8-811c-4dc4-89cc-531efc3b555b',
		1,
		0,
		1,
		0,
		ClientId,
		CommitteeId,
		FirstName,
		PatronymicName,
		LastName);

MERGE INTO [dbo].AspNetUserRoles AS Target
USING (VALUES
 ('d9317a83-834b-46c6-8df2-2c1438adf390', N'd9317a83-834b-46c6-8df2-2c1438adf3af'))
 AS Source (RoleId, UserId)
 ON Target.RoleId = Source.RoleId 
 AND Target.UserId = Source.UserId
WHEN NOT MATCHED BY TARGET 
THEN
 INSERT (RoleId, UserId)
 VALUES (RoleId, UserId);