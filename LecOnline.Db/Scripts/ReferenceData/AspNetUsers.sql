PRINT 'Creating test users'

/*
Master email account which is reserve email address for all other accounts is
leconlinemaster@gmail.com which could be recovered on the kant2002@gmail.com
*/

/* Default password for all users - !23QweAsd */
MERGE INTO [dbo].AspNetUsers AS Target
USING (VALUES
	('d9317a83-834b-46c6-8df2-2c1438adf3b0', 1, NULL, N'manager@lec-online.ru', N'Иван', N'Васильевич', N'Валукаев'),
	('d9317a83-834b-46c6-8df2-2c1438adf3b1', NULL, 1, N'secretary@lec-online.ru', N'Иван', N'Алексеевич', N'Мартынов'),
	('d9317a83-834b-46c6-8df2-2c1438adf3b2', 1, NULL, N'medicalcenter1@lec-online.ru', N'Алексей', N'Иванович', N'Полежаев'),
	('d9317a83-834b-46c6-8df2-2c1438adf3b3', 1, NULL, N'medicalcenter2@lec-online.ru', N'Артем', N'Владимирович', N'Админенко'),
	('d9317a83-834b-46c6-8df2-2c1438adf3b4', NULL, 1, N'chairman@lec-online.ru', N'Таисия', N'Трифоновна', N'Полевая'),
	('d9317a83-834b-46c6-8df2-2c1438adf3b5', NULL, 1, N'member1@lec-online.ru', N'Кирилл', N'Афанасьевич', N'Салтыков-Бунякин'),
	('d9317a83-834b-46c6-8df2-2c1438adf3b6', NULL, 1, N'member2@lec-online.ru', N'Василиса', N'Мироновна', N'Онищенко'),
	('d9317a83-834b-46c6-8df2-2c1438adf3b7', NULL, 1, N'member3@lec-online.ru', N'Админ', N'Админович', N'Печегорский'),
	('d9317a83-834b-46c6-8df2-2c1438adf3b8', NULL, 1, N'member4@lec-online.ru', N'Петр', N'Андреевич', N'Марусенко'),
	('d9317a83-834b-46c6-8df2-2c1438adf3b9', NULL, 1, N'member5@lec-online.ru', N'Владимир', N'Амиржанович', N'Ситов'),
	('d9317a83-834b-46c6-8df2-2c1438adf3ba', NULL, 1, N'member6@lec-online.ru', N'Юрий', N'Викторович', N'Ким'),
	('d9317a83-834b-46c6-8df2-2c1438adf3bb', NULL, 2, N'chariman2@lec-online.ru', N'Юрий', N'Викторович', N'Ким'),
	('d9317a83-834b-46c6-8df2-2c1438adf3bc', NULL, 2, N'secretary2@lec-online.ru', N'Юрий', N'Викторович', N'Ким'),
	('d9317a83-834b-46c6-8df2-2c1438adf3bd', NULL, 2, N'member7@lec-online.ru', N'Кирилл', N'Афанасьевич', N'Салтыков-Бунякин'),
	('d9317a83-834b-46c6-8df2-2c1438adf3be', NULL, 2, N'member8@lec-online.ru', N'Кирилл', N'Афанасьевич', N'Салтыков-Бунякин'),
	('d9317a83-834b-46c6-8df2-2c1438adf3bf', NULL, 2, N'member9@lec-online.ru', N'Кирилл', N'Афанасьевич', N'Салтыков-Бунякин'),
	('d9317a83-834b-46c6-8df2-2c1438adf3c0', NULL, 2, N'member10@lec-online.ru', N'Кирилл', N'Афанасьевич', N'Салтыков-Бунякин'),
	('d9317a83-834b-46c6-8df2-2c1438adf3c1', NULL, 2, N'member11@lec-online.ru', N'Кирилл', N'Афанасьевич', N'Салтыков-Бунякин'),
	('d9317a83-834b-46c6-8df2-2c1438adf3c2', 2, NULL, N'manager2@lec-online.ru', N'Мустафа', N'Аполлонович', N'Пархоменко')) 
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


MERGE INTO [dbo].AspNetUsers AS Target
USING (VALUES
	('e9317a83-834b-46c6-8df2-2c1438adf3bb', N'invalid@lec-online.ru', N'Адольф', N'Эрихович', N'Маргулис') )
	AS Source (Id, [Email], FirstName, PatronymicName, LastName)
	ON Target.Id = Source.Id
WHEN MATCHED 
THEN
	UPDATE 
	SET	[UserName] = Source.Email,
		Email = Source.Email,
		FirstName = Source.FirstName,
		PatronymicName = Source.PatronymicName,
		LastName = Source.LastName
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
		FirstName,
		PatronymicName,
		LastName)
	VALUES (Id, 
		[Email], 
		Email, 
		0, 
		'AKG5StHZy2X2GxHPFWXI0A439bhO5CPsBgluXCSxwEL22TUt+GvA9/c6WONi99BKsQ==',
		'7e8fe0f8-811c-4dc4-89cc-531efc3b555b',
		1,
		0,
		1,
		0,
		FirstName,
		PatronymicName,
		LastName);
