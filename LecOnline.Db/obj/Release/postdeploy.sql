:setvar TestData "0"
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

PRINT 'Synchronizing roles'

MERGE INTO [dbo].AspNetRoles AS Target
USING (VALUES
	('d9317a83-834b-46c6-8df2-2c1438adf390', N'Administrator'),
	('d9317a83-834b-46c6-8df2-2c1438adf391', N'Manager'),
	('d9317a83-834b-46c6-8df2-2c1438adf392', N'EthicalCommitteeMember'),
	('d9317a83-834b-46c6-8df2-2c1438adf393', N'MedicalCenter')) 
	AS Source (Id, [Name])
	ON Target.Id = Source.Id
WHEN MATCHED 
THEN
	UPDATE 
	SET	[Name] = Source.[Name]
WHEN NOT MATCHED BY TARGET 
THEN
	INSERT (Id, [Name])
	VALUES (Id, [Name])
WHEN NOT MATCHED BY SOURCE 
THEN
	DELETE;

PRINT 'Synchronizing research types'

MERGE INTO [dbo].StudyTypes AS Target
USING (VALUES
	(1, N'Phase1'),
	(2, N'Phase2'),
	(3, N'Phase3'),
	(4, N'Phase4'),
	(5, N'Bioequivalence'),
	(6, N'Observational')) 
	AS Source (Id, [Name])
	ON Target.Id = Source.Id
WHEN MATCHED 
THEN
	UPDATE 
	SET	[Name] = Source.[Name]
WHEN NOT MATCHED BY TARGET 
THEN
	INSERT (Id, [Name])
	VALUES (Id, [Name])
WHEN NOT MATCHED BY SOURCE 
THEN
	DELETE;

PRINT 'Synchronizing request statuses'

MERGE INTO [dbo].RequestStatuses AS Target
USING (VALUES
	(0, N'Created'),
	(10, N'Submitted'),
	(20, N'InvalidSubmission'),
	(30, N'Accepted'),
	(40, N'MeetingSet'),
	(50, N'Processing'),
	(60, N'NeedMoreInformation'),
	(70, N'Resolved'))
	AS Source (Id, [Name])
	ON Target.Id = Source.Id
WHEN MATCHED 
THEN
	UPDATE 
	SET	[Name] = Source.[Name]
WHEN NOT MATCHED BY TARGET 
THEN
	INSERT (Id, [Name])
	VALUES (Id, [Name])
WHEN NOT MATCHED BY SOURCE 
THEN
	DELETE;

PRINT 'Synchronizing research types'

MERGE INTO [dbo].StudyTypes AS Target
USING (VALUES
	(1, N'Phase1'),
	(2, N'Phase2'),
	(3, N'Phase3'),
	(4, N'Phase4'),
	(5, N'Bioequivalence'),
	(6, N'Observational')) 
	AS Source (Id, [Name])
	ON Target.Id = Source.Id
WHEN MATCHED 
THEN
	UPDATE 
	SET	[Name] = Source.[Name]
WHEN NOT MATCHED BY TARGET 
THEN
	INSERT (Id, [Name])
	VALUES (Id, [Name])
WHEN NOT MATCHED BY SOURCE 
THEN
	DELETE;

PRINT 'Synchronizing attendance statuses'

MERGE INTO [dbo].AttendanceStatuses AS Target
USING (VALUES
	(0, N'Pending'),
	(10, N'InvitationAccepted'),
	(20, N'InvitationDeclined'))
	AS Source (Id, [Name])
	ON Target.Id = Source.Id
WHEN MATCHED 
THEN
	UPDATE 
	SET	[Name] = Source.[Name]
WHEN NOT MATCHED BY TARGET 
THEN
	INSERT (Id, [Name])
	VALUES (Id, [Name])
WHEN NOT MATCHED BY SOURCE 
THEN
	DELETE;

PRINT 'Synchronizing object types'

MERGE INTO [dbo].[ObjectTypes] AS Target
USING (VALUES
	(0, N'SystemSettings'),
	(1, N'User'),
	(2, N'Client'),
	(3, N'Committee'),
	(4, N'Request'))
	AS Source (Id, [Name])
	ON Target.Id = Source.Id
WHEN MATCHED 
THEN
	UPDATE 
	SET	[Name] = Source.[Name]
WHEN NOT MATCHED BY TARGET 
THEN
	INSERT (Id, [Name])
	VALUES (Id, [Name])
WHEN NOT MATCHED BY SOURCE 
THEN
	DELETE;


/* Default admin user */
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

if '$(TestData)' = '1'
begin
	/* Create clients and committees first, since users relate to these data*/
PRINT 'Synchronizing clients'

set identity_insert [dbo].Clients on

MERGE INTO [dbo].Clients AS Target
USING (VALUES
	(1, N'Hoffmann–La Roche', '', '', ''),
	(2, N'Johnson & Johnson', '', '', ''),
	(3, N'Pfizer', '', '', ''),
	(4, N'Roche', '', '', '')) 
	AS Source (Id, CompanyName, ContactPerson, ContactEmail, ContactPhone)
	ON Target.Id = Source.Id
WHEN MATCHED 
THEN
	UPDATE 
	SET	CompanyName = Source.CompanyName,
		ContactPerson = Source.ContactPerson,
		ContactEmail = Source.ContactEmail,
		ContactPhone = Source.ContactPhone
WHEN NOT MATCHED BY TARGET 
THEN
	INSERT (Id, CompanyName, ContactPerson, ContactEmail, ContactPhone)
	VALUES (Id, CompanyName, ContactPerson, ContactEmail, ContactPhone)
-- WHEN NOT MATCHED BY SOURCE 
-- THEN
--	DELETE
;

set identity_insert [dbo].Clients OFF
PRINT 'Synchronizing committees'

set identity_insert [dbo].Committees on

MERGE INTO [dbo].Committees AS Target
USING (VALUES
	(1, N'Nuffield Council Bioethics', N'г. Санкт-Петербург'),
	(2, N'Corec', N'г. Санкт-Петербург')) 
	AS Source (Id, Name, City)
	ON Target.Id = Source.Id
WHEN MATCHED 
THEN
	UPDATE 
	SET	Name			= Source.Name,
		City			= Source.City
WHEN NOT MATCHED BY TARGET 
THEN
	INSERT (Id, Name, City)
	VALUES (Id, Name, City)
-- WHEN NOT MATCHED BY SOURCE 
-- THEN
--	DELETE
;

set identity_insert [dbo].Committees OFF
	
	/* Users data */
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

PRINT 'Synchronizing users roles'

MERGE INTO [dbo].AspNetUserRoles AS Target
USING (VALUES
	/* Adding administrators */
	('d9317a83-834b-46c6-8df2-2c1438adf390', N'd9317a83-834b-46c6-8df2-2c1438adf3af'), 
	/* Client 1 */
	/* Adding managers */
	('d9317a83-834b-46c6-8df2-2c1438adf391', N'd9317a83-834b-46c6-8df2-2c1438adf3b0'), 
	/* Adding medical centers */
	('d9317a83-834b-46c6-8df2-2c1438adf393', N'd9317a83-834b-46c6-8df2-2c1438adf3b2'),
	('d9317a83-834b-46c6-8df2-2c1438adf393', N'd9317a83-834b-46c6-8df2-2c1438adf3b3'),  
	/* Client 2 */
	/* Adding managers */
	('d9317a83-834b-46c6-8df2-2c1438adf391', N'd9317a83-834b-46c6-8df2-2c1438adf3c2'), 
	/* Adding medical centers */ 
	/* Adding cometee members */
	/* Committee 1 */
	('d9317a83-834b-46c6-8df2-2c1438adf392', N'd9317a83-834b-46c6-8df2-2c1438adf3b1'),
	('d9317a83-834b-46c6-8df2-2c1438adf392', N'd9317a83-834b-46c6-8df2-2c1438adf3b4'),
	('d9317a83-834b-46c6-8df2-2c1438adf392', N'd9317a83-834b-46c6-8df2-2c1438adf3b5'),
	('d9317a83-834b-46c6-8df2-2c1438adf392', N'd9317a83-834b-46c6-8df2-2c1438adf3b6'),
	('d9317a83-834b-46c6-8df2-2c1438adf392', N'd9317a83-834b-46c6-8df2-2c1438adf3b7'),
	('d9317a83-834b-46c6-8df2-2c1438adf392', N'd9317a83-834b-46c6-8df2-2c1438adf3b8'),
	('d9317a83-834b-46c6-8df2-2c1438adf392', N'd9317a83-834b-46c6-8df2-2c1438adf3b9'),
	('d9317a83-834b-46c6-8df2-2c1438adf392', N'd9317a83-834b-46c6-8df2-2c1438adf3ba'),
	/* Pending ethical committee member */
	('d9317a83-834b-46c6-8df2-2c1438adf392', N'e9317a83-834b-46c6-8df2-2c1438adf3bb'),
	/* Committee 2 */
	('d9317a83-834b-46c6-8df2-2c1438adf392', N'd9317a83-834b-46c6-8df2-2c1438adf3bb'),
	('d9317a83-834b-46c6-8df2-2c1438adf392', N'd9317a83-834b-46c6-8df2-2c1438adf3bc'),
	('d9317a83-834b-46c6-8df2-2c1438adf392', N'd9317a83-834b-46c6-8df2-2c1438adf3bd'),
	('d9317a83-834b-46c6-8df2-2c1438adf392', N'd9317a83-834b-46c6-8df2-2c1438adf3be'),
	('d9317a83-834b-46c6-8df2-2c1438adf392', N'd9317a83-834b-46c6-8df2-2c1438adf3bf'),
	('d9317a83-834b-46c6-8df2-2c1438adf392', N'd9317a83-834b-46c6-8df2-2c1438adf3c0'),
	('d9317a83-834b-46c6-8df2-2c1438adf392', N'd9317a83-834b-46c6-8df2-2c1438adf3c1')
	)
	AS Source (RoleId, UserId)
	ON	Target.RoleId = Source.RoleId 
	AND Target.UserId = Source.UserId
WHEN NOT MATCHED BY TARGET 
THEN
	INSERT (RoleId, UserId)
	VALUES (RoleId, UserId)
WHEN NOT MATCHED BY SOURCE 
THEN
	DELETE;

	
	/* Domain data */
PRINT 'Synchronizing requests'

set identity_insert [dbo].Requests on

declare @defaultCompany nvarchar(500)
declare @defaultResearchBase nvarchar(500)
declare @defaultCompanyAddress nvarchar(500)
set @defaultCompany = N'ОАО «Фармацевтическая фабрика Санкт-Петербурга»'
set @defaultCompanyAddress = N'дом 24-а, ул, Моисеенко. Санкт-Петербург. 191144, Россия'
set @defaultResearchBase = N'СПб ГБУЗ «Городская поликлнпика №107»'
MERGE INTO [dbo].Requests AS Target
USING (VALUES
	(1, 1, 1, 'Transcranial brain stimulation', N'',
		'01022', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 1, 3, 100, 150, 10, 0 /* Created */),
	(2, 1, 1, 'Transcranial brain stimulation', N'',
		'01022', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 3, 3, 100, 150, 0, 10 /* Submitted */),
	(3, 1, 1, 'Transcranial brain invalid', N'',
		'01022', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 1, 3, 100, 150, 0, 20 /* InvalidSubmission */),
	(4, 1, 1, 'Transcranial brain stimulation', N'',
		'01022', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 2, 3, 100, 150, 0, 30 /* Accepted */),
	(5, 1, 1, 'Transcranial brain stimulation', N'',
		'01022', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 1, 3, 100, 150, 0, 40 /* MeetingSet */),
	(6, 1, 1, 'Transcranial brain stimulation', N'',
		'01022', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 1, 3, 100, 150, 0, 50 /* Processing */),
	(7, 1, 1, 'Transcranial brain stimulation', N'',
		'01022', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 1, 3, 100, 150, 0, 60 /* MeetingFinished */),
	(8, 1, 1, 'Transcranial brain not enoght data', N'',
		'01022', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 4, 3, 100, 150, 0, 70 /* NeedMoreInformation */),
	(9, 1, 1, 'Transcranial brain stimulation', N'',
		'01022', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 1, 3, 100, 150, 0, 80 /* Resolved */),
	(10, 1, 2, 'Electronic cigrars safety', N'',
		'AB1234', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 4, 1, 365, 100, 0, 30 /* Accepted */),
	(11, 2, 1, 'Safe start Programme', N'',
		'01021', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 1, 1, 365, 100, 0, 0 /* Created */),
	(12, 2, 2, 'Another research', N'',
		'AB1235', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		5 /* Observational */, 1, 1, 365, 100, 0, 0 /* Created */),
	(13, 1, 1, N'Международное рандомизированное исследование 3 фазы иммунотерапии AGS-003 в сочетании со стандартной терапией распространенной почечно-клеточной карциномы (ADAPT)', N'',
		N'AGS-003-007', @defaultResearchBase, @defaultCompany, @defaultCompany, N'ООО "Смуз Драг Девелопмент"', N'Санкт-Петербург, Красногвардейский пер., д.23, лит.Ж. Санкт-Петербурга, 197342, Россия', N'',
		5 /* Observational */, 2, 1, 365, 100, 0, 0 /* Created */),
	(14, 1, 1, N'Долгосрочное, многоцентровое, открытое исследование без контрольной группы, являющееся продолжением исследования МЕРИТ-1, для оценки безопасности, переносимости и эффективности Мацитентана у пациентов с неоперабельной хронической тромбоэмболической легочной гипертензией (ХТЛГ)', N'',
		N'АС-055Е202', @defaultResearchBase, @defaultCompany, @defaultCompany, N'ООО "МБ Квест", Россия', N'3, Barabanny pereulok, fl. 4, Moscow, 107023, Russia', N'',
		5 /* Observational */, 1, 1, 365, 100, 0, 10 /* Submitted */),
	(15, 1, 1, N'Рандомизированное открытое исследование с активным препаратом сравнения и изменяемыми дозами для оценки безопасности и переносимости монотерапии топираматом по сравнению с монотерапией леветирацетамом у детей и подростков со случаями впервые или недавно возникшей эпилепсии', N'',
		N'TOPMAT EPY4067', @defaultResearchBase, @defaultCompany, @defaultCompany, N'ООО "Парексель Интернэшнл (РУС)", Россия', N'121609, Г МОСКВА, БУЛЬВ ОСЕННИЙ, Д 23', N'',
		5 /* Observational */, 1, 1, 365, 100, 0, 10 /* Submitted */),
	(16, 1, 1, N'52-недельное, двойное слепое, рандомизированное, многоцентровое исследование III фазы в параллельных группах у пациентов с бронхиальной астмой в восзрасте от 12 лет и старше по оценке эффективности и безопасности Симбикорта® (будесонид + формотерол) Турбухалер® 160/4.5 мкг "по требованию" в сравнении с Пульмикортом® (будесонид) Турбухалером® 200 мкг два раза в сутки плюс Тербуталин Турбухалер® 0.4 мг "по требованию"', N'',
		N'D589SC00003', @defaultResearchBase, @defaultCompany, @defaultCompany, N'ООО "АстраЗенека Фармасьютикалз", Россия', N'БЦ "Nordstar Tower", 23 этаж, Беговая улица, 3с1, Москва, Россия, 125284', N'',
		5 /* Observational */, 2, 1, 365, 100, 23, 10 /* Submitted */),
	(17, 1, 1, N'МНОГОЦЕНТРОВОЕ ОТКРЫТОЕ РАНДОМИЗИРОВАННОЕ СРАВНИТЕЛЬНОЕ КЛИНИЧЕСКОЕ ИССЛЕДОВАНИЕ ЭФФЕКТИВНОСТИ И БЕЗОПАСНОСТИ ПРИМЕНЕНИЯ ПРЕПАРАТОВ МОМЕСПИР® ЛАНГХАЛЕР®, ПОРОШОК ДЛЯ ИНГАЛЯЦИЙ ДОЗИРОВАННЫЙ, И АСМАНЕКС® ТВИСТХЕЙЛЕР®, ПОРОШОК ДЛЯ ИНГАЛЯЦИЙ ДОЗИРОВАННЫЙ, В ТЕРАПИИ ПАЦИЕНТОВ С ЛЕГКОЙ ИЛИ СРЕДНЕЙ ТЯЖЕСТИ ПЕРСИСТИРУЮЩЕЙ БРОНХИАЛЬНОЙ АСТМОЙ', N'l. Оценка не меньшей эффективнOсти лекарствеI{ного препарата МомеспирФ ЛангхатерФ,
шорошок .&{я ингаляций лозирOванltыйо в ходе 3-месячного куреs лечения пациентов с
легкоЙ или средней тяжесТн персвý,I,ирl.юrшеЙ чаýтичн0 контрлшруемой бронхиа"qыtой
аетмой, в сраЕнеIIии с препаратом АсманексФ ТвистхейлерФ. порошок jIля иttгаляциЙ
лознроваlIный. ЭффекIнвностъ булет 0цениваться по первичной и вторичным кOнеч}lым
точкаь! (локазателям эффективttости)
2. оценка безопасносfll лекарственноr0 препарата МомеепирФ ЛангхалерФ, поропlOк дjlя
инга:lяций дозироваlIный, R ходе З-месячIIогtr курса jIечения пацвентов с легкой или
срелней тяжести персистируюцей частично кOнтролируе:иой бронхиалыlой астltой, в
сравнени}п с препаратом АсманексФ ТвистхейлерФ, пороruок д,тIя иигачяций
дозированный. БезопаснQстЬ булет оцениваться шо сOответствующим конеч}Iым точкам
(показателям безопасности},',
		N'8МПИ', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'МЗ РФ от 01.12.2014 № 20-2-461427/Р/КИР',
		5 /* Observational */, 3, 1, 365, 100, 23, 10 /* Submitted */)) 
	AS Source (Id, ClientId, CommitteeId, Title, Description, 
		StudyCode, StudyBase, StudySponsor, StudyProducer, StudyPerformer, StudyPerformerAddress, StudyApprovedBy,
		StudyType, StudyPhase, CentersQty, PlannedDuration, PatientsCount, RandomizedPatientsCount, Status)
	ON Target.Id = Source.Id
WHEN MATCHED 
THEN
	UPDATE 
	SET	ClientId = Source.ClientId,
		CommitteeId = Source.CommitteeId,
		Title = Source.Title,
		Description = Source.Description,
		StudyCode = Source.StudyCode,
		StudyType = Source.StudyType,
		StudyPhase = Source.StudyPhase,
		CentersQty = Source.CentersQty,
		PlannedDuration = Source.PlannedDuration,
		PatientsCount = Source.PatientsCount,
		RandomizedPatientsCount = Source.RandomizedPatientsCount,
		StudyBase = Source.StudyBase,
		StudySponsor = Source.StudySponsor,
		StudyProducer = Source.StudyProducer,
		StudyPerformer = Source.StudyPerformer,
		StudyPerformerStatutoryAddress = Source.StudyPerformerAddress,
		StudyPerformerRegisteredAddress = Source.StudyPerformerAddress,
		StudyApprovedBy = Source.StudyApprovedBy,
		Status = Source.Status
WHEN NOT MATCHED BY TARGET 
THEN
	INSERT (Id, 
		ClientId, 
		CommitteeId, 
		Title, 
		StudyCode, 
		StudyType, 
		CentersQty, 
		PlannedDuration, 
		PatientsCount, 
		ContactPerson,
		ContactPhone,
		ContactFax,
		ContactEmail,
		CreatedBy,
		Status)
	VALUES (Id, 
		ClientId, 
		CommitteeId, 
		Title, 
		StudyCode, 
		StudyType, 
		CentersQty, 
		PlannedDuration, 
		PatientsCount, 
		'Somebody A.V.',
		'1234567891',
		'1234567891',
		'test@test.org',
		'manager@lec-online.ru',
		Status)
-- WHEN NOT MATCHED BY SOURCE 
-- THEN
--	DELETE
;

set identity_insert [dbo].Requests OFF

UPDATE Requests
SET CommitteeId = NULL
WHERE Id = 1;
PRINT 'Synchronizing meetings'

set identity_insert [dbo].Meetings on

MERGE INTO [dbo].Meetings AS Target
USING (VALUES
	(1, 5, '2015/01/01 00:00:00', 0, N'Protocol for request #5', 'Log for request #5:', 0),
	(2, 6, '2015/01/01 00:00:00', 20, N'Protocol for request #6', 'Log for request #6:', 0),
	(3, 7, '2015/01/01 00:00:00', 40, N'Protocol for request #7', 'Log for request #7:', 0),
	(4, 8, '2015/01/01 00:00:00', 40, N'Protocol for request #8', 'Log for request #8:', 0),
	(5, 9, '2015/01/01 00:00:00', 40, 
		N'1. Одобрить международное клиническое исследование препарата по протоколу Ne 8МПИ:
<<Мноеоценmровое оmкрыmое ронOомuзuрованное сравнumельное клuнчческое uсслеOованuе эффекmuвнасmu u безопоснасmч прuмененt)я препараmов МомеспuрО
Ланzхалере, парощок dля uнzаляцuй dозuрованньlil, tJ дсманексФ ТвuсmхеалерФ, порошок
0ля uнеаляцut dозuраванньtй, в mерапuч пацuенmоа с леекой uлч среоней mяжесmч
персuсmчруюu,lей бронхчальной acmMailll фаза lll на базе спб гБуз (городская
поликлиника N9107 (главный исследователь Един А.с.).
2. Одобрение комитёта по этl4ке вступает в силу после одобрения протокола Мз.
з. Плановый пересмотр t4 рассмотрение отчета о ходе исследования запланировать на
декабрь 2015 rода.', 
		N'Член этического комитета 3ахарова М.С.: с информацией о том, что ознакомилась с
представленными документами и пред/пожением об одобрении проведения международного
клинического исследования препарата по протоколу Ne 8МПИ: кМноzоценmровое omqpblmoe
ранdомuзuровонное сравнumельное клuнчческое чселеdовонuе эффекmuвносmч ч беsопоаносmч
прuмененuя препораmов Момеспuр@ Ланехалер@, порошок 0ля uнzалiцuit iозuрованньtй, u
Дсманекс@ Твuсmхейлер@, пороtдок dля uнеаляцuй iозuрованньtй, в mерапuч пацuенmов с леекой
uлч среOней mяжесmч персuсmuрующей бронхuольной асmмой> фаза lll

Член комитета Маликов А.Я.
С поддержкой преддожения 3ахаровой М.С.', 1)) 
	AS Source (Id, RequestId, MeetingDate, Status, Protocol, MeetingLog, Resolution)
	ON Target.Id = Source.Id
WHEN MATCHED 
THEN
	UPDATE 
	SET	RequestId = Source.RequestId,
		MeetingDate = Source.MeetingDate,
		Status = Source.Status,
		Protocol = Source.Protocol,
		MeetingLog = Source.MeetingLog,
		Resolution = Source.Resolution
WHEN NOT MATCHED BY TARGET 
THEN
	INSERT (Id, RequestId, MeetingDate, Status, Protocol, MeetingLog, Resolution)
	VALUES (Id, RequestId, MeetingDate, Status, Protocol, MeetingLog, Resolution)
-- WHEN NOT MATCHED BY SOURCE 
-- THEN
--	DELETE
;

set identity_insert [dbo].Meetings OFF
PRINT 'Synchronizing meeting attendees'

set identity_insert [dbo].MeetingAttendees on

MERGE INTO [dbo].MeetingAttendees AS Target
USING (VALUES
	(1, 1, 'd9317a83-834b-46c6-8df2-2c1438adf3b1', 0, NULL),
	(2, 1, 'd9317a83-834b-46c6-8df2-2c1438adf3b4', 0, NULL),
	(3, 1, 'd9317a83-834b-46c6-8df2-2c1438adf3b5', 0, NULL),
	(4, 1, 'd9317a83-834b-46c6-8df2-2c1438adf3b6', 0, NULL),
	(5, 1, 'd9317a83-834b-46c6-8df2-2c1438adf3b7', 0, NULL),
	(6, 1, 'd9317a83-834b-46c6-8df2-2c1438adf3b8', 0, NULL),
	(7, 1, 'd9317a83-834b-46c6-8df2-2c1438adf3b9', 0, NULL),
	(8, 1, 'd9317a83-834b-46c6-8df2-2c1438adf3ba', 0, NULL),

	(9, 2, 'd9317a83-834b-46c6-8df2-2c1438adf3b1', 10, NULL),
	(10, 2, 'd9317a83-834b-46c6-8df2-2c1438adf3b4', 10, NULL),
	(11, 2, 'd9317a83-834b-46c6-8df2-2c1438adf3b5', 10, NULL),
	(12, 2, 'd9317a83-834b-46c6-8df2-2c1438adf3b6', 10, NULL),
	(13, 2, 'd9317a83-834b-46c6-8df2-2c1438adf3b7', 20, NULL),
	(14, 2, 'd9317a83-834b-46c6-8df2-2c1438adf3b8', 10, NULL),
	(15, 2, 'd9317a83-834b-46c6-8df2-2c1438adf3b9', 10, NULL),
	(16, 2, 'd9317a83-834b-46c6-8df2-2c1438adf3ba', 10, NULL),

	(17, 3, 'd9317a83-834b-46c6-8df2-2c1438adf3b1', 10, NULL),
	(18, 3, 'd9317a83-834b-46c6-8df2-2c1438adf3b4', 10, NULL),
	(19, 3, 'd9317a83-834b-46c6-8df2-2c1438adf3b5', 10, NULL),
	(20, 3, 'd9317a83-834b-46c6-8df2-2c1438adf3b6', 10, NULL),
	(21, 3, 'd9317a83-834b-46c6-8df2-2c1438adf3b7', 20, NULL),
	(22, 3, 'd9317a83-834b-46c6-8df2-2c1438adf3b8', 10, NULL),
	(23, 3, 'd9317a83-834b-46c6-8df2-2c1438adf3b9', 10, NULL),
	(24, 3, 'd9317a83-834b-46c6-8df2-2c1438adf3ba', 10, NULL),

	(25, 4, 'd9317a83-834b-46c6-8df2-2c1438adf3b1', 10, NULL),
	(26, 4, 'd9317a83-834b-46c6-8df2-2c1438adf3b4', 10, NULL),
	(27, 4, 'd9317a83-834b-46c6-8df2-2c1438adf3b5', 10, NULL),
	(28, 4, 'd9317a83-834b-46c6-8df2-2c1438adf3b6', 10, NULL),
	(29, 4, 'd9317a83-834b-46c6-8df2-2c1438adf3b7', 20, NULL),
	(30, 4, 'd9317a83-834b-46c6-8df2-2c1438adf3b8', 10, NULL),
	(31, 4, 'd9317a83-834b-46c6-8df2-2c1438adf3b9', 10, NULL),
	(32, 4, 'd9317a83-834b-46c6-8df2-2c1438adf3ba', 10, NULL),

	(33, 5, 'd9317a83-834b-46c6-8df2-2c1438adf3b1', 10, 1),
	(34, 5, 'd9317a83-834b-46c6-8df2-2c1438adf3b4', 10, 1),
	(35, 5, 'd9317a83-834b-46c6-8df2-2c1438adf3b5', 10, 1),
	(36, 5, 'd9317a83-834b-46c6-8df2-2c1438adf3b6', 10, 1),
	(37, 5, 'd9317a83-834b-46c6-8df2-2c1438adf3b7', 20, NULL),
	(38, 5, 'd9317a83-834b-46c6-8df2-2c1438adf3b8', 10, 2),
	(39, 5, 'd9317a83-834b-46c6-8df2-2c1438adf3b9', 10, 3),
	(40, 5, 'd9317a83-834b-46c6-8df2-2c1438adf3ba', 10, 1)) 
	AS Source (Id, MeetingId, UserId, Status, Vote)
	ON Target.Id = Source.Id
WHEN MATCHED 
THEN
	UPDATE 
	SET	MeetingId = Source.MeetingId,
		UserId = Source.UserId,
		Status = Source.Status,
		Vote = Source.Vote
WHEN NOT MATCHED BY TARGET 
THEN
	INSERT (Id, MeetingId, UserId, Status, Vote)
	VALUES (Id, MeetingId, UserId, Status, Vote)
-- WHEN NOT MATCHED BY SOURCE 
-- THEN
--	DELETE
;

set identity_insert [dbo].MeetingAttendees OFF

	/* Committee membership */
PRINT 'Set committees membership'

MERGE INTO [dbo].Committees AS Target
USING (VALUES
	(1, 'd9317a83-834b-46c6-8df2-2c1438adf3b4', NULL, 'd9317a83-834b-46c6-8df2-2c1438adf3b1'),
	(2, 'd9317a83-834b-46c6-8df2-2c1438adf3bb', NULL, 'd9317a83-834b-46c6-8df2-2c1438adf3bc')) 
	AS Source (Id, Chairman, ViceChairman, Secretary)
	ON Target.Id = Source.Id
WHEN MATCHED 
THEN
	UPDATE 
	SET	Chairman		= Source.Chairman,
		ViceChairman	= Source.ViceChairman,
		Secretary		= Source.Secretary
;
end
GO
