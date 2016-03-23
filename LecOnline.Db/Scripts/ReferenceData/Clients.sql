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