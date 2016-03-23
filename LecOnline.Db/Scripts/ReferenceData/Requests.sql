PRINT 'Synchronizing requests'

set identity_insert [dbo].Requests on

declare @defaultCompany nvarchar(500)
declare @defaultResearchBase nvarchar(500)
declare @defaultCompanyAddress nvarchar(500)
set @defaultCompany = N'ОАО «Фармацевтическая фабрика Санкт-Петербурга»'
set @defaultCompanyAddress = N'дом 24-а, ул, Моисеенко. Санкт-Петербург. 191144, Россия'
set @defaultResearchBase = N'СПб ГБУЗ «Городская поликлнпика №107»'
MERGE INTO [dbo].Requests AS Target
USING (VALUES
	(1, 1, 1, 'Transcranial brain stimulation', N'',
		'01022', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 1, 3, 100, 150, 10, 0 /* Created */),
	(2, 1, 1, 'Transcranial brain stimulation', N'',
		'01022', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 3, 3, 100, 150, 0, 10 /* Submitted */),
	(3, 1, 1, 'Transcranial brain invalid', N'',
		'01022', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 1, 3, 100, 150, 0, 20 /* InvalidSubmission */),
	(4, 1, 1, 'Transcranial brain stimulation', N'',
		'01022', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 2, 3, 100, 150, 0, 30 /* Accepted */),
	(5, 1, 1, 'Transcranial brain stimulation', N'',
		'01022', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 1, 3, 100, 150, 0, 40 /* MeetingSet */),
	(6, 1, 1, 'Transcranial brain stimulation', N'',
		'01022', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 1, 3, 100, 150, 0, 50 /* Processing */),
	(7, 1, 1, 'Transcranial brain stimulation', N'',
		'01022', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 1, 3, 100, 150, 0, 60 /* MeetingFinished */),
	(8, 1, 1, 'Transcranial brain not enoght data', N'',
		'01022', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 4, 3, 100, 150, 0, 70 /* NeedMoreInformation */),
	(9, 1, 1, 'Transcranial brain stimulation', N'',
		'01022', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 1, 3, 100, 150, 0, 80 /* Resolved */),
	(10, 1, 2, 'Electronic cigrars safety', N'',
		'AB1234', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 4, 1, 365, 100, 0, 30 /* Accepted */),
	(11, 2, 1, 'Safe start Programme', N'',
		'01021', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		6 /* Observational */, 1, 1, 365, 100, 0, 0 /* Created */),
	(12, 2, 2, 'Another research', N'',
		'AB1235', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'',
		5 /* Observational */, 1, 1, 365, 100, 0, 0 /* Created */),
	(13, 1, 1, N'Международное рандомизированное исследование 3 фазы иммунотерапии AGS-003 в сочетании со стандартной терапией распространенной почечно-клеточной карциномы (ADAPT)', N'',
		N'AGS-003-007', @defaultResearchBase, @defaultCompany, @defaultCompany, N'ООО "Смуз Драг Девелопмент"', N'Санкт-Петербург, Красногвардейский пер., д.23, лит.Ж. Санкт-Петербурга, 197342, Россия', N'',
		5 /* Observational */, 2, 1, 365, 100, 0, 0 /* Created */),
	(14, 1, 1, N'Долгосрочное, многоцентровое, открытое исследование без контрольной группы, являющееся продолжением исследования МЕРИТ-1, для оценки безопасности, переносимости и эффективности Мацитентана у пациентов с неоперабельной хронической тромбоэмболической легочной гипертензией (ХТЛГ)', N'',
		N'АС-055Е202', @defaultResearchBase, @defaultCompany, @defaultCompany, N'ООО "МБ Квест", Россия', N'3, Barabanny pereulok, fl. 4, Moscow, 107023, Russia', N'',
		5 /* Observational */, 1, 1, 365, 100, 0, 10 /* Submitted */),
	(15, 1, 1, N'Рандомизированное открытое исследование с активным препаратом сравнения и изменяемыми дозами для оценки безопасности и переносимости монотерапии топираматом по сравнению с монотерапией леветирацетамом у детей и подростков со случаями впервые или недавно возникшей эпилепсии', N'',
		N'TOPMAT EPY4067', @defaultResearchBase, @defaultCompany, @defaultCompany, N'ООО "Парексель Интернэшнл (РУС)", Россия', N'121609, Г МОСКВА, БУЛЬВ ОСЕННИЙ, Д 23', N'',
		5 /* Observational */, 1, 1, 365, 100, 0, 10 /* Submitted */),
	(16, 1, 1, N'52-недельное, двойное слепое, рандомизированное, многоцентровое исследование III фазы в параллельных группах у пациентов с бронхиальной астмой в восзрасте от 12 лет и старше по оценке эффективности и безопасности Симбикорта® (будесонид + формотерол) Турбухалер® 160/4.5 мкг "по требованию" в сравнении с Пульмикортом® (будесонид) Турбухалером® 200 мкг два раза в сутки плюс Тербуталин Турбухалер® 0.4 мг "по требованию"', N'',
		N'D589SC00003', @defaultResearchBase, @defaultCompany, @defaultCompany, N'ООО "АстраЗенека Фармасьютикалз", Россия', N'БЦ "Nordstar Tower", 23 этаж, Беговая улица, 3с1, Москва, Россия, 125284', N'',
		5 /* Observational */, 2, 1, 365, 100, 23, 10 /* Submitted */),
	(17, 1, 1, N'МНОГОЦЕНТРОВОЕ ОТКРЫТОЕ РАНДОМИЗИРОВАННОЕ СРАВНИТЕЛЬНОЕ КЛИНИЧЕСКОЕ ИССЛЕДОВАНИЕ ЭФФЕКТИВНОСТИ И БЕЗОПАСНОСТИ ПРИМЕНЕНИЯ ПРЕПАРАТОВ МОМЕСПИР® ЛАНГХАЛЕР®, ПОРОШОК ДЛЯ ИНГАЛЯЦИЙ ДОЗИРОВАННЫЙ, И АСМАНЕКС® ТВИСТХЕЙЛЕР®, ПОРОШОК ДЛЯ ИНГАЛЯЦИЙ ДОЗИРОВАННЫЙ, В ТЕРАПИИ ПАЦИЕНТОВ С ЛЕГКОЙ ИЛИ СРЕДНЕЙ ТЯЖЕСТИ ПЕРСИСТИРУЮЩЕЙ БРОНХИАЛЬНОЙ АСТМОЙ', N'l. Оценка не меньшей эффективнOсти лекарствеI{ного препарата МомеспирФ ЛангхатерФ,
шорошок .&{я ингаляций лозирOванltыйо в ходе 3-месячного куреs лечения пациентов с
легкоЙ или средней тяжесТн персвý,I,ирl.юrшеЙ чаýтичн0 контрлшруемой бронхиа"qыtой
аетмой, в сраЕнеIIии с препаратом АсманексФ ТвистхейлерФ. порошок jIля иttгаляциЙ
лознроваlIный. ЭффекIнвностъ булет 0цениваться по первичной и вторичным кOнеч}lым
точкаь! (локазателям эффективttости)
2. оценка безопасносfll лекарственноr0 препарата МомеепирФ ЛангхалерФ, поропlOк дjlя
инга:lяций дозироваlIный, R ходе З-месячIIогtr курса jIечения пацвентов с легкой или
срелней тяжести персистируюцей частично кOнтролируе:иой бронхиалыlой астltой, в
сравнени}п с препаратом АсманексФ ТвистхейлерФ, пороruок д,тIя иигачяций
дозированный. БезопаснQстЬ булет оцениваться шо сOответствующим конеч}Iым точкам
(показателям безопасности},',
		N'8МПИ', @defaultResearchBase, @defaultCompany, @defaultCompany, @defaultCompany, @defaultCompanyAddress, N'МЗ РФ от 01.12.2014 № 20-2-461427/Р/КИР',
		5 /* Observational */, 3, 1, 365, 100, 23, 10 /* Submitted */)) 
	AS Source (Id, ClientId, CommitteeId, Title, Description, 
		StudyCode, StudyBase, StudySponsor, StudyProducer, StudyPerformer, StudyPerformerAddress, StudyApprovedBy,
		StudyType, StudyPhase, CentersQty, PlannedDuration, PatientsCount, RandomizedPatientsCount, Status)
	ON Target.Id = Source.Id
WHEN MATCHED 
THEN
	UPDATE 
	SET	ClientId = Source.ClientId,
		CommitteeId = Source.CommitteeId,
		Title = Source.Title,
		Description = Source.Description,
		StudyCode = Source.StudyCode,
		StudyType = Source.StudyType,
		StudyPhase = Source.StudyPhase,
		CentersQty = Source.CentersQty,
		PlannedDuration = Source.PlannedDuration,
		PatientsCount = Source.PatientsCount,
		RandomizedPatientsCount = Source.RandomizedPatientsCount,
		StudyBase = Source.StudyBase,
		StudySponsor = Source.StudySponsor,
		StudyProducer = Source.StudyProducer,
		StudyPerformer = Source.StudyPerformer,
		StudyPerformerStatutoryAddress = Source.StudyPerformerAddress,
		StudyPerformerRegisteredAddress = Source.StudyPerformerAddress,
		StudyApprovedBy = Source.StudyApprovedBy,
		Status = Source.Status
WHEN NOT MATCHED BY TARGET 
THEN
	INSERT (Id, 
		ClientId, 
		CommitteeId, 
		Title, 
		StudyCode, 
		StudyType, 
		CentersQty, 
		PlannedDuration, 
		PatientsCount, 
		ContactPerson,
		ContactPhone,
		ContactFax,
		ContactEmail,
		CreatedBy,
		Status)
	VALUES (Id, 
		ClientId, 
		CommitteeId, 
		Title, 
		StudyCode, 
		StudyType, 
		CentersQty, 
		PlannedDuration, 
		PatientsCount, 
		'Somebody A.V.',
		'1234567891',
		'1234567891',
		'test@test.org',
		'manager@lec-online.ru',
		Status)
-- WHEN NOT MATCHED BY SOURCE 
-- THEN
--	DELETE
;

set identity_insert [dbo].Requests OFF

UPDATE Requests
SET CommitteeId = NULL
WHERE Id = 1;