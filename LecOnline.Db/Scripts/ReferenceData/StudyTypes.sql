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
