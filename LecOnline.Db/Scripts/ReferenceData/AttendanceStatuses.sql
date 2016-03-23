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
