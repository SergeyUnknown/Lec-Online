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