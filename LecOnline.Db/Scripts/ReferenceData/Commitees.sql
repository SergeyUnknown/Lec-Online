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