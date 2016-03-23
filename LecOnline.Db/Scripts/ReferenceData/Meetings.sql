PRINT 'Synchronizing meetings'

set identity_insert [dbo].Meetings on

MERGE INTO [dbo].Meetings AS Target
USING (VALUES
	(1, 5, '2015/01/01 00:00:00', 0, N'Protocol for request #5', 'Log for request #5:', 0),
	(2, 6, '2015/01/01 00:00:00', 20, N'Protocol for request #6', 'Log for request #6:', 0),
	(3, 7, '2015/01/01 00:00:00', 40, N'Protocol for request #7', 'Log for request #7:', 0),
	(4, 8, '2015/01/01 00:00:00', 40, N'Protocol for request #8', 'Log for request #8:', 0),
	(5, 9, '2015/01/01 00:00:00', 40, 
		N'1. Одобрить международное клиническое исследование препарата по протоколу Ne 8МПИ:
<<Мноеоценmровое оmкрыmое ронOомuзuрованное сравнumельное клuнчческое uсслеOованuе эффекmuвнасmu u безопоснасmч прuмененt)я препараmов МомеспuрО
Ланzхалере, парощок dля uнzаляцuй dозuрованньlil, tJ дсманексФ ТвuсmхеалерФ, порошок
0ля uнеаляцut dозuраванньtй, в mерапuч пацuенmоа с леекой uлч среоней mяжесmч
персuсmчруюu,lей бронхчальной acmMailll фаза lll на базе спб гБуз (городская
поликлиника N9107 (главный исследователь Един А.с.).
2. Одобрение комитёта по этl4ке вступает в силу после одобрения протокола Мз.
з. Плановый пересмотр t4 рассмотрение отчета о ходе исследования запланировать на
декабрь 2015 rода.', 
		N'Член этического комитета 3ахарова М.С.: с информацией о том, что ознакомилась с
представленными документами и пред/пожением об одобрении проведения международного
клинического исследования препарата по протоколу Ne 8МПИ: кМноzоценmровое omqpblmoe
ранdомuзuровонное сравнumельное клuнчческое чселеdовонuе эффекmuвносmч ч беsопоаносmч
прuмененuя препораmов Момеспuр@ Ланехалер@, порошок 0ля uнzалiцuit iозuрованньtй, u
Дсманекс@ Твuсmхейлер@, пороtдок dля uнеаляцuй iозuрованньtй, в mерапuч пацuенmов с леекой
uлч среOней mяжесmч персuсmuрующей бронхuольной асmмой> фаза lll

Член комитета Маликов А.Я.
С поддержкой преддожения 3ахаровой М.С.', 1)) 
	AS Source (Id, RequestId, MeetingDate, Status, Protocol, MeetingLog, Resolution)
	ON Target.Id = Source.Id
WHEN MATCHED 
THEN
	UPDATE 
	SET	RequestId = Source.RequestId,
		MeetingDate = Source.MeetingDate,
		Status = Source.Status,
		Protocol = Source.Protocol,
		MeetingLog = Source.MeetingLog,
		Resolution = Source.Resolution
WHEN NOT MATCHED BY TARGET 
THEN
	INSERT (Id, RequestId, MeetingDate, Status, Protocol, MeetingLog, Resolution)
	VALUES (Id, RequestId, MeetingDate, Status, Protocol, MeetingLog, Resolution)
-- WHEN NOT MATCHED BY SOURCE 
-- THEN
--	DELETE
;

set identity_insert [dbo].Meetings OFF