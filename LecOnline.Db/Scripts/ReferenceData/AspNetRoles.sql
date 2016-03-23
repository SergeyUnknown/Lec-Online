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
