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
