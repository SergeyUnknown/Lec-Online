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
