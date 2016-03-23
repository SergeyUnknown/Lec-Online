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