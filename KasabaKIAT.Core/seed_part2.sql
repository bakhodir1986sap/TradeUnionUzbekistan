USE [KasabaKIAT];
GO

-- ============================================================
-- TIER 2: Tables that depend on Tier 1
-- ============================================================

-- users (depends on roles)
-- Keep user id=1 (admin), add 9 more
DELETE FROM [users] WHERE id > 1;
INSERT INTO [users] (id, version, account_expired, account_locked, user_name, password, email,
                     is_super_admin, role_id, bkut_id, BRANCH_ID, state, s_status, s_create,
                     REGION_ID, dep_id, NATIVE_ID)
VALUES
(2,  0,0,0,N'buxgalter1', N'password1',N'bux1@kasabakiat.uz',  0,2,1,1,N'ACTIVE',N'ACTIVE','2024-01-15',N'1703',NULL,NULL),
(3,  0,0,0,N'operator1',  N'password2',N'oper1@kasabakiat.uz', 0,3,2,2,N'ACTIVE',N'ACTIVE','2024-01-20',N'1702',NULL,NULL),
(4,  0,0,0,N'kassir1',    N'password3',N'kass1@kasabakiat.uz', 0,4,3,3,N'ACTIVE',N'ACTIVE','2024-02-01',N'1704',NULL,NULL),
(5,  0,0,0,N'rukov1',     N'password4',N'rukov1@kasabakiat.uz',0,5,4,4,N'ACTIVE',N'ACTIVE','2024-02-10',N'1701',NULL,NULL),
(6,  0,0,0,N'revizor1',   N'password5',N'rev1@kasabakiat.uz',  0,6,5,5,N'ACTIVE',N'ACTIVE','2024-03-01',N'1706',NULL,NULL),
(7,  0,0,0,N'dispetcher1',N'password6',N'disp1@kasabakiat.uz', 0,7,6,6,N'ACTIVE',N'ACTIVE','2024-03-15',N'1708',NULL,NULL),
(8,  0,0,0,N'auditor1',   N'password7',N'audit1@kasabakiat.uz',0,8,7,7,N'ACTIVE',N'ACTIVE','2024-04-01',N'1712',NULL,NULL),
(9,  0,0,0,N'metodist1',  N'password8',N'met1@kasabakiat.uz',  0,9,8,8,N'ACTIVE',N'ACTIVE','2024-04-15',N'1714',NULL,NULL),
(10, 0,0,0,N'chitatel1',  N'password9',N'read1@kasabakiat.uz', 0,10,9,9,N'ACTIVE',N'ACTIVE','2024-05-01',N'1715',NULL,NULL);
GO

-- role_permission
DELETE FROM [role_permission];
INSERT INTO [role_permission] (id, role_id, functionality_id, action) VALUES
(1,  1,1,N'READ,WRITE,DELETE'),
(2,  1,2,N'READ,WRITE,DELETE'),
(3,  1,11,N'READ,WRITE,DELETE'),
(4,  1,12,N'READ,WRITE,DELETE'),
(5,  2,9,N'READ,WRITE'),
(6,  2,7,N'READ,WRITE'),
(7,  3,2,N'READ,WRITE'),
(8,  3,5,N'READ,WRITE'),
(9,  3,6,N'READ,WRITE'),
(10, 4,7,N'READ,WRITE'),
(11, 5,1,N'READ'),
(12, 5,10,N'READ'),
(13, 6,10,N'READ'),
(14, 7,8,N'READ,WRITE'),
(15, 8,10,N'READ');
GO

-- t_departmentbase
DELETE FROM [t_departmentbase];
INSERT INTO [t_departmentbase] (id, DEPARTMENT_CODE, name_ru, name_uz, name_uzl, s_status, s_user_id, s_create, PARENT_ID, MATCH_DEPARTMENTBASE, bkut_id) VALUES
(1, N'00-000',N'ФПТУ',                  N'FHTU',                  N'FHTU',                  N'ACTIVE',1,'2024-01-01',NULL,N'00-000',0),
(2, N'01-001',N'Ташкентский ОК',        N'Toshkent OK',           N'Toshkent OK',           N'ACTIVE',1,'2024-01-01',1,   N'01-001',1),
(3, N'01-002',N'Самаркандский ОК',      N'Samarqand OK',          N'Samarqand OK',          N'ACTIVE',1,'2024-01-01',1,   N'01-002',2),
(4, N'01-003',N'Ферганский ОК',         N'Farg''ona OK',          N'Farg''ona OK',          N'ACTIVE',1,'2024-01-01',1,   N'01-003',3),
(5, N'01-004',N'Андижанский ОК',        N'Andijon OK',            N'Andijon OK',            N'ACTIVE',1,'2024-01-01',1,   N'01-004',4),
(6, N'02-001',N'ПК завода ТашЭл',       N'ToshEl PK',             N'ToshEl PK',             N'ACTIVE',1,'2024-01-01',2,   N'02-001',1),
(7, N'02-002',N'ПК СК',                 N'SK PK',                 N'SK PK',                 N'ACTIVE',1,'2024-01-01',2,   N'02-002',1),
(8, N'02-003',N'ПК ТрансКо',            N'TransKo PK',            N'TransKo PK',            N'ACTIVE',1,'2024-01-01',3,   N'02-003',2),
(9, N'02-004',N'ПК МедЦентра',          N'MedMark PK',            N'MedMark PK',            N'ACTIVE',1,'2024-01-01',3,   N'02-004',2),
(10,N'02-005',N'ПК ТГТУ',               N'TDTU PK',               N'TDTU PK',               N'ACTIVE',1,'2024-01-01',2,   N'02-005',1);
GO

-- t_sanatorium (depends on sp_sanatorium_type)
DELETE FROM [t_sanatorium];
INSERT INTO [t_sanatorium] (id, name_uz, name_ru, code, s_create, sanatorium_type_id) VALUES
(1, N'Zarafshon sanatoriysi',   N'Санаторий Зарафшан',     N'ZAR', '2024-01-01', 1),
(2, N'Do''stlik sanatoriysi',   N'Санаторий Дустлик',      N'DST', '2024-01-01', 1),
(3, N'Navro''z sanatoriysi',    N'Санаторий Навруз',       N'NAV', '2024-01-01', 1),
(4, N'Bahor sanatoriysi',       N'Санаторий Бахор',        N'BAH', '2024-01-01', 2),
(5, N'Chinar sanatoriysi',      N'Санаторий Чинар',        N'CHN', '2024-01-01', 1),
(6, N'Bolalar lageri',          N'Детский лагерь',         N'LAG', '2024-01-01', 3),
(7, N'Yangier dam olish uyi',   N'Дом отдыха Янгиер',      N'YNG', '2024-01-01', 4),
(8, N'Toshkent sanatoriysi',    N'Санаторий Ташкент',      N'TSH', '2024-01-01', 1),
(9, N'Samarqand pansionati',    N'Пансионат Самарканд',    N'SAM', '2024-01-01', 2),
(10,N'Farg''ona sanatoriysi',   N'Санаторий Фергана',      N'FRG', '2024-01-01', 1),
(11,N'Andijon sanatoriysi',     N'Санаторий Андижан',      N'AND', '2024-01-01', 1),
(12,N'Chimyon kurortiyi',       N'Курорт Чимган',          N'CHM', '2024-01-01', 6);
GO

-- t_camp (no FK)
DELETE FROM [t_camp];
INSERT INTO [t_camp] (id, camps_name, s_create, s_status) VALUES
(1, N'Лагерь Артек-УЗ',       '2024-01-01',N'ACTIVE'),
(2, N'Лагерь Янгиер',         '2024-01-01',N'ACTIVE'),
(3, N'Лагерь Чимган',         '2024-01-01',N'ACTIVE'),
(4, N'Лагерь Бухара',         '2024-01-01',N'ACTIVE'),
(5, N'Лагерь Самарканд',      '2024-01-01',N'ACTIVE'),
(6, N'Лагерь Паркент',        '2024-01-01',N'ACTIVE'),
(7, N'Лагерь Ферганский',     '2024-01-01',N'ACTIVE'),
(8, N'Лагерь Андижанский',    '2024-01-01',N'ACTIVE'),
(9, N'Лагерь Наманганский',   '2024-01-01',N'ACTIVE'),
(10,N'Лагерь Навоийский',     '2024-01-01',N'ACTIVE');
GO

-- tbcs_dictionary (depends on tbcs_dictionarytype)
DELETE FROM [tbcs_dictionary];
INSERT INTO [tbcs_dictionary] (REC_ID, NAME_RU, NAME_UZ, NAME_LT, CODE, STATUS, USER_ID, DICTIONARY_TYPE_ID) VALUES
(1, N'Одноместный',      N'Bir kishilik',    N'Yakka',     N'SGL', 1, 1, 5),
(2, N'Двухместный',      N'Ikki kishilik',   N'Juft',      N'DBL', 1, 1, 5),
(3, N'Полулюкс',         N'Yarim lyuks',     N'Polulyuks', N'SLX', 1, 1, 5),
(4, N'Люкс',             N'Lyuks',           N'Lyuks',     N'LUX', 1, 1, 5),
(5, N'Трёхместный',      N'Uch kishilik',    N'Uch kishi', N'TRP', 1, 1, 5),
(6, N'Полное питание',   N'To''liq ovqat',   N'Full',      N'FB',  1, 1, 4),
(7, N'Полупансион',      N'Yarim pansion',   N'Half',      N'HB',  1, 1, 4),
(8, N'Завтрак',          N'Nonushta',        N'BB',        N'BB',  1, 1, 4),
(9, N'Взрослая',         N'Kattalar',        N'Adult',     N'ADL', 1, 1, 6),
(10,N'Детская',          N'Bolalar',         N'Child',     N'CHD', 1, 1, 6),
(11,N'Льготная',         N'Imtiyozli',       N'Lgot',      N'LGT', 1, 1, 6),
(12,N'Реабилитационная', N'Reabilitatsiya',  N'Rehab',     N'RHB', 1, 1, 6);
GO

-- t_egrpo (depends on sp_okonx, sp_opf)
DELETE FROM [t_egrpo];
INSERT INTO [t_egrpo] (id, inn, name_full, name_short, address, soato_id, okonx_id, opf_id, director_name, director_inn) VALUES
(1, N'202010001',N'Федерация профсоюзов Узбекистана',      N'ФПТУ',       N'г. Ташкент, ул. Шота Руставели 16',N'1703',8,6,N'Ахмедов А.А.',N'306010001'),
(2, N'202010002',N'Ташкентский областной комитет',         N'ТашОК',      N'г. Ташкент, ул. Навои 5',          N'1703',8,6,N'Юсупов Б.Б.',  N'306010002'),
(3, N'202010003',N'Самаркандский областной комитет',       N'СамОК',      N'г. Самарканд, ул. Регистан 1',      N'1702',8,6,N'Каримов В.В.', N'306010003'),
(4, N'202010004',N'Профсоюзный комитет ТашЭлектрик',      N'ПК ТашЭл',   N'г. Ташкент, ул. Файзулла 3',       N'1703',1,4,N'Расулов Г.Г.', N'306010004'),
(5, N'202010005',N'Профсоюзный комитет Стройкомпании',    N'ПК СК',      N'г. Ташкент, ул. Буюк Ипак Йули',   N'1706',8,2,N'Тошев Д.Д.',   N'306010005'),
(6, N'202010006',N'Профсоюзный комитет Транспорт',        N'ПК Транспорт',N'г. Ташкент, ул. Амир Темур 12',   N'1703',10,4,N'Назаров Е.Е.',N'306010006'),
(7, N'202010007',N'Профсоюзный комитет Медцентра',        N'ПК МедЦентр',N'г. Ташкент, ул. Паркент 5',        N'1703',8,5,N'Содиков Ж.Ж.', N'306010007'),
(8, N'202010008',N'Профсоюзный комитет ТГТУ',             N'ПК ТГТУ',    N'г. Ташкент, Вузгородок',           N'1703',8,5,N'Омонов З.З.',  N'306010008'),
(9, N'202010009',N'Ферганский областной комитет',          N'ФергОК',     N'г. Фергана, ул. Мустакиллик 7',    N'1704',8,6,N'Алиев И.И.',   N'306010009'),
(10,N'202010010',N'Андижанский областной комитет',         N'АндОК',      N'г. Андижан, ул. Бобур 3',          N'1701',8,6,N'Бобоев К.К.',  N'306010010');
GO

PRINT 'Part 2 complete.';
GO
