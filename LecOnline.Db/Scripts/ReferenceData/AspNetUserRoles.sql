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
