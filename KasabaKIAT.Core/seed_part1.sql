USE [KasabaKIAT];
GO

-- ============================================================
-- CLEANUP: Delete all data in reverse FK order
-- ============================================================
DELETE FROM [api_refresh_tokens];
DELETE FROM [tblPos_Put];
DELETE FROM [tblVozvrat];
DELETE FROM [tblVipl_Put];
DELETE FROM [tblOplata_Rasxod];
DELETE FROM [tblOplata];
DELETE FROM [tblZakritiye];
DELETE FROM [tblViplF];
DELETE FROM [tblVipl];
DELETE FROM [tblPost];
DELETE FROM [tblBrons];
DELETE FROM [tbpt_avarageprice];
DELETE FROM [tbpt_yearlyplanotrasl];
DELETE FROM [tbpt_yearlyplanregion];
DELETE FROM [tbpt_zaezdi];
DELETE FROM [tbpt_putyovki];
DELETE FROM [useractions];
DELETE FROM [tbbu_platejka];
DELETE FROM [tbbu_provodki];
DELETE FROM [tbbu_finOtchetSetting];
DELETE FROM [tbku_StrokaProfsoyza];
DELETE FROM [tbku_GrafikZaezdovSanatoriev];
DELETE FROM [tbku_SpisokRaspredeleniya];
DELETE FROM [t_egrpo];
DELETE FROM [tbcs_dictionary];
DELETE FROM [t_sanatorium];
DELETE FROM [t_camp];
DELETE FROM [t_departmentbase];
DELETE FROM [role_permission];
DELETE FROM [users] WHERE id > 1;
DELETE FROM [roles] WHERE id > 1;
DELETE FROM [tblSprav_Org];
DELETE FROM [tblSprav_Otras];
DELETE FROM [tblSprav_Kv];
DELETE FROM [tblSprav_Reg];
DELETE FROM [tblSprav_San];
DELETE FROM [tblSprav_Vil];
DELETE FROM [tblSprav_MFO];
DELETE FROM [tblSprav_Face];
DELETE FROM [tblSprav_Os];
DELETE FROM [tblSprav_Pr];
DELETE FROM [tbcs_dictionarytype];
DELETE FROM [t_normativdoc];
DELETE FROM [sp_employees];
DELETE FROM [sp_soogu];
DELETE FROM [sp_okonx];
DELETE FROM [sp_opf];
DELETE FROM [sp_section];
DELETE FROM [sp_departmenttype];
DELETE FROM [sp_contracttype];
DELETE FROM [sp_degree];
DELETE FROM [sp_nation];
DELETE FROM [sp_position];
DELETE FROM [sp_country];
DELETE FROM [sp_branch];
DELETE FROM [sp_sanatorium_profil];
DELETE FROM [sp_sanatorium_type];
DELETE FROM [sp_soato];
DELETE FROM [functionality];
GO

-- ============================================================
-- TIER 1: No FK dependencies
-- ============================================================

-- functionality
DELETE FROM [functionality];
INSERT INTO [functionality] (id, name, description) VALUES
(1, N'dashboard', N'Главная страница'),
(2, N'putyovki', N'Управление путёвками'),
(3, N'godovoy_plan', N'Годовой план'),
(4, N'grafik_zaezdov', N'График заездов'),
(5, N'prixod_putevok', N'Приход путёвок'),
(6, N'rasxod_putevok', N'Расход путёвок'),
(7, N'oplata', N'Оплата и выплаты'),
(8, N'zayavki', N'Заявки'),
(9, N'buxgalter', N'Бухгалтерия'),
(10, N'otchot', N'Отчёты'),
(11, N'admin_users', N'Пользователи'),
(12, N'admin_roles', N'Роли');
GO

-- roles
DELETE FROM [roles] WHERE id > 1;
INSERT INTO [roles] (id, version, name, s_status, s_create) VALUES
(2, 0, N'Бухгалтер',  N'ACTIVE', '2024-01-01'),
(3, 0, N'Оператор',   N'ACTIVE', '2024-01-01'),
(4, 0, N'Кассир',     N'ACTIVE', '2024-01-01'),
(5, 0, N'Руководитель',N'ACTIVE','2024-01-01'),
(6, 0, N'Ревизор',    N'ACTIVE', '2024-01-01'),
(7, 0, N'Диспетчер',  N'ACTIVE', '2024-01-01'),
(8, 0, N'Аудитор',    N'ACTIVE', '2024-01-01'),
(9, 0, N'Методист',   N'ACTIVE', '2024-01-01'),
(10,0, N'Читатель',   N'ACTIVE', '2024-01-01');
GO

-- sp_sanatorium_type
DELETE FROM [sp_sanatorium_type];
INSERT INTO [sp_sanatorium_type] (id, version, name_ru, name_uz, name_uzl, s_status, s_create) VALUES
(1, 0, N'Санаторий',         N'Sanatoriy',         N'Sanatoriy',         N'ACTIVE', '2024-01-01'),
(2, 0, N'Пансионат',         N'Pansionat',         N'Pansionat',         N'ACTIVE', '2024-01-01'),
(3, 0, N'Детский лагерь',    N'Bolalar lageri',    N'Bolalar lageri',    N'ACTIVE', '2024-01-01'),
(4, 0, N'Дом отдыха',        N'Dam olish uyi',     N'Dam olish uyi',     N'ACTIVE', '2024-01-01'),
(5, 0, N'Профилакторий',     N'Profilaktory',      N'Profilaktory',      N'ACTIVE', '2024-01-01'),
(6, 0, N'Курорт',            N'Kurort',            N'Kurort',            N'ACTIVE', '2024-01-01'),
(7, 0, N'Туристическая база',N'Sayyohlik bazasi',  N'Sayyohlik bazasi',  N'ACTIVE', '2024-01-01'),
(8, 0, N'Водолечебница',     N'Balneologik klinik',N'Balneologik klinik',N'ACTIVE', '2024-01-01'),
(9, 0, N'Климатолечебница',  N'Iqlim kurort',      N'Iqlim kurort',      N'ACTIVE', '2024-01-01'),
(10,0, N'Грязелечебница',    N'Balchiq kurort',    N'Balchiq kurort',    N'ACTIVE', '2024-01-01');
GO

-- sp_sanatorium_profil
DELETE FROM [sp_sanatorium_profil];
INSERT INTO [sp_sanatorium_profil] (id, code, name_ru, name_uz, name_uzl, s_create) VALUES
(1, N'CARD',  N'Кардиологический', N'Kardiologik',   N'Kardiologik',   '2024-01-01'),
(2, N'NEUR',  N'Неврологический',  N'Nevrologik',    N'Nevrologik',    '2024-01-01'),
(3, N'GASTR', N'Гастроэнтерологический',N'Gastroenterologik',N'Gastroenterologik','2024-01-01'),
(4, N'ORTH',  N'Ортопедический',   N'Ortopedik',     N'Ortopedik',     '2024-01-01'),
(5, N'PULM',  N'Пульмонологический',N'Pulmonologik', N'Pulmonologik',  '2024-01-01'),
(6, N'URO',   N'Урологический',    N'Urologik',      N'Urologik',      '2024-01-01'),
(7, N'DERM',  N'Дерматологический',N'Dermatologik',  N'Dermatologik',  '2024-01-01'),
(8, N'OPH',   N'Офтальмологический',N'Oftalmologik', N'Oftalmologik',  '2024-01-01'),
(9, N'REHAB', N'Реабилитационный', N'Reabilitatsiya',N'Reabilitatsiya','2024-01-01'),
(10,N'GEN',   N'Общеоздоровительный',N'Umumsoglomlash',N'Umumsoglomlash','2024-01-01');
GO

-- sp_soato
DELETE FROM [sp_soato];
INSERT INTO [sp_soato] (id, code, name_ru, name_uz, name_uzl) VALUES
(1,  N'1703',N'г. Ташкент',          N'Toshkent shahri',    N'Toshkent shahri'),
(2,  N'1706',N'Ташкентская область', N'Toshkent viloyati',  N'Toshkent viloyati'),
(3,  N'1702',N'Самаркандская область',N'Samarqand viloyati',N'Samarqand viloyati'),
(4,  N'1704',N'Ферганская область',  N'Farg''ona viloyati', N'Farg''ona viloyati'),
(5,  N'1701',N'Андижанская область', N'Andijon viloyati',   N'Andijon viloyati'),
(6,  N'1708',N'Наманганская область',N'Namangan viloyati',  N'Namangan viloyati'),
(7,  N'1705',N'Кашкадарьинская обл.',N'Qashqadaryo viloyati',N'Qashqadaryo viloyati'),
(8,  N'1710',N'Сурхандарьинская обл.',N'Surxandaryo viloyati',N'Surxandaryo viloyati'),
(9,  N'1712',N'Бухарская область',   N'Buxoro viloyati',    N'Buxoro viloyati'),
(10, N'1714',N'Хорезмская область',  N'Xorazm viloyati',    N'Xorazm viloyati'),
(11, N'1715',N'Навоийская область',  N'Navoiy viloyati',    N'Navoiy viloyati'),
(12, N'1718',N'Джизакская область',  N'Jizzax viloyati',    N'Jizzax viloyati');
GO

-- sp_branch
DELETE FROM [sp_branch];
INSERT INTO [sp_branch] (id, version, name_ru, name_uz, name_uzl, s_status, s_create) VALUES
(1, 0, N'Ташкентский филиал',      N'Toshkent filiali',      N'Toshkent filiali',      N'ACTIVE','2024-01-01'),
(2, 0, N'Самаркандский филиал',    N'Samarqand filiali',     N'Samarqand filiali',     N'ACTIVE','2024-01-01'),
(3, 0, N'Ферганский филиал',       N'Farg''ona filiali',     N'Farg''ona filiali',     N'ACTIVE','2024-01-01'),
(4, 0, N'Андижанский филиал',      N'Andijon filiali',       N'Andijon filiali',       N'ACTIVE','2024-01-01'),
(5, 0, N'Наманганский филиал',     N'Namangan filiali',      N'Namangan filiali',      N'ACTIVE','2024-01-01'),
(6, 0, N'Кашкадарьинский филиал',  N'Qashqadaryo filiali',  N'Qashqadaryo filiali',   N'ACTIVE','2024-01-01'),
(7, 0, N'Бухарский филиал',        N'Buxoro filiali',        N'Buxoro filiali',        N'ACTIVE','2024-01-01'),
(8, 0, N'Хорезмский филиал',       N'Xorazm filiali',        N'Xorazm filiali',        N'ACTIVE','2024-01-01'),
(9, 0, N'Навоийский филиал',       N'Navoiy filiali',        N'Navoiy filiali',        N'ACTIVE','2024-01-01'),
(10,0, N'Джизакский филиал',       N'Jizzax filiali',        N'Jizzax filiali',        N'ACTIVE','2024-01-01');
GO

-- sp_country
DELETE FROM [sp_country];
INSERT INTO [sp_country] (id, version, name_ru, name_uz, name_uzl, s_status, s_create) VALUES
(1, 0,N'Узбекистан',   N'O''zbekiston', N'O''zbekiston', N'ACTIVE','2024-01-01'),
(2, 0,N'Россия',       N'Rossiya',      N'Rossiya',      N'ACTIVE','2024-01-01'),
(3, 0,N'Казахстан',    N'Qozog''iston', N'Qozog''iston', N'ACTIVE','2024-01-01'),
(4, 0,N'Кыргызстан',   N'Qirg''iziston',N'Qirg''iziston',N'ACTIVE','2024-01-01'),
(5, 0,N'Таджикистан',  N'Tojikiston',   N'Tojikiston',   N'ACTIVE','2024-01-01'),
(6, 0,N'Туркменистан', N'Turkmaniston', N'Turkmaniston', N'ACTIVE','2024-01-01'),
(7, 0,N'Германия',     N'Germaniya',    N'Germaniya',    N'ACTIVE','2024-01-01'),
(8, 0,N'Китай',        N'Xitoy',        N'Xitoy',        N'ACTIVE','2024-01-01'),
(9, 0,N'Турция',       N'Turkiya',      N'Turkiya',      N'ACTIVE','2024-01-01'),
(10,0,N'США',          N'AQSH',         N'AQSH',         N'ACTIVE','2024-01-01');
GO

-- sp_position
DELETE FROM [sp_position];
INSERT INTO [sp_position] (id, version, name_ru, name_uz, name_uzl, s_status, s_create) VALUES
(1, 0,N'Директор',           N'Direktor',           N'Direktor',           N'ACTIVE','2024-01-01'),
(2, 0,N'Главный бухгалтер',  N'Bosh buxgalter',     N'Bosh buxgalter',     N'ACTIVE','2024-01-01'),
(3, 0,N'Бухгалтер',          N'Buxgalter',          N'Buxgalter',          N'ACTIVE','2024-01-01'),
(4, 0,N'Экономист',          N'Iqtisodchi',         N'Iqtisodchi',         N'ACTIVE','2024-01-01'),
(5, 0,N'Юрист',              N'Yurist',             N'Yurist',             N'ACTIVE','2024-01-01'),
(6, 0,N'Кассир',             N'Kassir',             N'Kassir',             N'ACTIVE','2024-01-01'),
(7, 0,N'Инспектор',          N'Inspektor',          N'Inspektor',          N'ACTIVE','2024-01-01'),
(8, 0,N'Специалист',         N'Mutaxassis',         N'Mutaxassis',         N'ACTIVE','2024-01-01'),
(9, 0,N'Секретарь',          N'Kotib',              N'Kotib',              N'ACTIVE','2024-01-01'),
(10,0,N'Заместитель',        N'O''rinbosar',        N'O''rinbosar',        N'ACTIVE','2024-01-01');
GO

-- sp_nation
DELETE FROM [sp_nation];
INSERT INTO [sp_nation] (id, code, name_ru, name_uz, name_uzl, s_create) VALUES
(1, N'UZB',N'Узбек',      N'O''zbek',    N'O''zbek',    '2024-01-01'),
(2, N'RUS',N'Русский',    N'Rus',        N'Rus',        '2024-01-01'),
(3, N'TAJ',N'Таджик',     N'Tojik',      N'Tojik',      '2024-01-01'),
(4, N'KAZ',N'Казах',      N'Qozoq',      N'Qozoq',      '2024-01-01'),
(5, N'KYR',N'Кыргыз',     N'Qirg''iz',  N'Qirg''iz',  '2024-01-01'),
(6, N'TUR',N'Туркмен',    N'Turkman',    N'Turkman',    '2024-01-01'),
(7, N'KOR',N'Кореец',     N'Koreys',     N'Koreys',     '2024-01-01'),
(8, N'TAT',N'Татарин',    N'Tatar',      N'Tatar',      '2024-01-01'),
(9, N'ARM',N'Армянин',    N'Arman',      N'Arman',      '2024-01-01'),
(10,N'AZE',N'Азербайджанец',N'Ozarbayjonli',N'Ozarbayjonli','2024-01-01');
GO

-- sp_degree
DELETE FROM [sp_degree];
INSERT INTO [sp_degree] (id, code, name_ru, name_uz, name_uzl, s_create) VALUES
(1, N'01',N'Без степени',        N'Darajasiz',          N'Darajasiz',          '2024-01-01'),
(2, N'02',N'Бакалавр',           N'Bakalavr',           N'Bakalavr',           '2024-01-01'),
(3, N'03',N'Магистр',            N'Magistr',            N'Magistr',            '2024-01-01'),
(4, N'04',N'Кандидат наук',      N'Fan nomzodi',        N'Fan nomzodi',        '2024-01-01'),
(5, N'05',N'Доктор наук',        N'Fan doktori',        N'Fan doktori',        '2024-01-01'),
(6, N'06',N'PhD',                N'PhD',                N'PhD',                '2024-01-01'),
(7, N'07',N'Академик',           N'Akademik',           N'Akademik',           '2024-01-01'),
(8, N'08',N'Среднее специальное',N'O''rta maxsus',      N'O''rta maxsus',      '2024-01-01'),
(9, N'09',N'Среднее',            N'O''rta',             N'O''rta',             '2024-01-01'),
(10,N'10',N'Высшее',             N'Oliy',               N'Oliy',               '2024-01-01');
GO

-- sp_contracttype
DELETE FROM [sp_contracttype];
INSERT INTO [sp_contracttype] (id, code, name_ru, name_uz, name_uzl, s_create) VALUES
(1, N'01',N'Трудовой договор',      N'Mehnat shartnomasi',   N'Mehnat shartnomasi',   '2024-01-01'),
(2, N'02',N'Гражданско-правовой',   N'Fuqarolik-huquqiy',    N'Fuqarolik-huquqiy',    '2024-01-01'),
(3, N'03',N'Совместительство',      N'Yarim stavka',         N'Yarim stavka',         '2024-01-01'),
(4, N'04',N'Срочный',               N'Muddatli',             N'Muddatli',             '2024-01-01'),
(5, N'05',N'Бессрочный',            N'Muddatsiz',            N'Muddatsiz',            '2024-01-01'),
(6, N'06',N'Сезонный',              N'Mavsumiy',             N'Mavsumiy',             '2024-01-01'),
(7, N'07',N'Временный',             N'Vaqtinchalik',         N'Vaqtinchalik',         '2024-01-01'),
(8, N'08',N'Подрядный',             N'Pudrat',               N'Pudrat',               '2024-01-01'),
(9, N'09',N'Стажировка',            N'Amaliyot',             N'Amaliyot',             '2024-01-01'),
(10,N'10',N'Договор услуг',         N'Xizmat shartnomasi',   N'Xizmat shartnomasi',   '2024-01-01');
GO

-- sp_departmenttype
DELETE FROM [sp_departmenttype];
INSERT INTO [sp_departmenttype] (id, code, name_ru, name_uz, name_uzl, s_create) VALUES
(1, N'01',N'Отраслевой профсоюз',   N'Tarmoq kasaba uyushmasi',N'Tarmoq kasaba uyushmasi','2024-01-01'),
(2, N'02',N'Первичная организация',  N'Birlamchi tashkilot',    N'Birlamchi tashkilot',    '2024-01-01'),
(3, N'03',N'Районный комитет',       N'Tuman qo''mitasi',       N'Tuman qo''mitasi',       '2024-01-01'),
(4, N'04',N'Областной комитет',      N'Viloyat qo''mitasi',     N'Viloyat qo''mitasi',     '2024-01-01'),
(5, N'05',N'Республиканский комитет',N'Respublika qo''mitasi',  N'Respublika qo''mitasi',  '2024-01-01'),
(6, N'06',N'Цеховой комитет',        N'Sex qo''mitasi',         N'Sex qo''mitasi',         '2024-01-01'),
(7, N'07',N'Совет трудового коллектива',N'Mehnat jamoasi kengashi',N'Mehnat jamoasi kengashi','2024-01-01'),
(8, N'08',N'Межрегиональный',        N'Mintaqalararo',          N'Mintaqalararo',          '2024-01-01'),
(9, N'09',N'Международный',          N'Xalqaro',                N'Xalqaro',                '2024-01-01'),
(10,N'10',N'Прочие',                 N'Boshqalar',              N'Boshqalar',              '2024-01-01');
GO

-- sp_section
DELETE FROM [sp_section];
INSERT INTO [sp_section] (id, code, name_ru, name_uz, name_uzl, s_create) VALUES
(1, N'A', N'Сельское хозяйство',       N'Qishloq xo''jaligi',     N'Qishloq xo''jaligi',     '2024-01-01'),
(2, N'B', N'Горнодобывающая',          N'Tog'' qazib olish',       N'Tog'' qazib olish',       '2024-01-01'),
(3, N'C', N'Обрабатывающая',           N'Qayta ishlash',           N'Qayta ishlash',           '2024-01-01'),
(4, N'D', N'Энергетика',               N'Energetika',              N'Energetika',              '2024-01-01'),
(5, N'E', N'Строительство',            N'Qurilish',                N'Qurilish',                '2024-01-01'),
(6, N'F', N'Торговля',                 N'Savdo',                   N'Savdo',                   '2024-01-01'),
(7, N'G', N'Транспорт',                N'Transport',               N'Transport',               '2024-01-01'),
(8, N'H', N'Здравоохранение',          N'Sog''liqni saqlash',      N'Sog''liqni saqlash',      '2024-01-01'),
(9, N'I', N'Образование',              N'Ta''lim',                 N'Ta''lim',                 '2024-01-01'),
(10,N'J', N'Культура',                 N'Madaniyat',               N'Madaniyat',               '2024-01-01');
GO

-- sp_okonx
DELETE FROM [sp_okonx];
INSERT INTO [sp_okonx] (id, code, name_ru, name_uz, name_uzl, s_create) VALUES
(1, N'01110',N'Растениеводство',      N'O''simlikchilik',   N'O''simlikchilik',   '2024-01-01'),
(2, N'01210',N'Животноводство',       N'Chorvachilik',      N'Chorvachilik',      '2024-01-01'),
(3, N'10100',N'Добыча угля',          N'Ko''mir qazish',    N'Ko''mir qazish',    '2024-01-01'),
(4, N'13100',N'Металлургия',          N'Metallurgiya',      N'Metallurgiya',      '2024-01-01'),
(5, N'15110',N'Производство мяса',    N'Go''sht ishlab chiqarish',N'Go''sht ishlab chiqarish','2024-01-01'),
(6, N'20100',N'Лесозаготовка',        N'O''rmon kesish',    N'O''rmon kesish',    '2024-01-01'),
(7, N'35110',N'Производство эл. энергии',N'Elektr energiya',N'Elektr energiya',  '2024-01-01'),
(8, N'41000',N'Строительство',        N'Qurilish',          N'Qurilish',          '2024-01-01'),
(9, N'45110',N'Автомобильная торговля',N'Avtomobil savdo',  N'Avtomobil savdo',   '2024-01-01'),
(10,N'49100',N'Железнодорожный транспорт',N'Temir yo''l',   N'Temir yo''l',       '2024-01-01');
GO

-- sp_opf
DELETE FROM [sp_opf];
INSERT INTO [sp_opf] (id, code, name_ru, name_uz, name_uzl, s_create) VALUES
(1, N'01',N'ООО',                        N'MChJ',          N'MChJ',          '2024-01-01'),
(2, N'02',N'АО',                         N'AJ',            N'AJ',            '2024-01-01'),
(3, N'03',N'ЧП',                         N'XK',            N'XK',            '2024-01-01'),
(4, N'04',N'Госпредприятие',             N'Davlat korxona',N'Davlat korxona','2024-01-01'),
(5, N'05',N'Бюджетная организация',      N'Byudjet tashkilot',N'Byudjet tashkilot','2024-01-01'),
(6, N'06',N'Некоммерческая организация', N'Notijorat tashkilot',N'Notijorat tashkilot','2024-01-01'),
(7, N'07',N'Кооператив',                 N'Kooperativ',    N'Kooperativ',    '2024-01-01'),
(8, N'08',N'Унитарное предприятие',      N'Unitar korxona',N'Unitar korxona','2024-01-01'),
(9, N'09',N'Холдинг',                    N'Xolding',       N'Xolding',       '2024-01-01'),
(10,N'10',N'Филиал',                     N'Filial',        N'Filial',        '2024-01-01');
GO

-- sp_soogu
DELETE FROM [sp_soogu];
INSERT INTO [sp_soogu] (id, version, code, name_ru, name_uz, name_uzl, s_status, s_create) VALUES
(1, 0,N'10000',N'Промышленность',       N'Sanoat',              N'Sanoat',              N'ACTIVE','2024-01-01'),
(2, 0,N'20000',N'Сельское хозяйство',   N'Qishloq xo''jaligi', N'Qishloq xo''jaligi', N'ACTIVE','2024-01-01'),
(3, 0,N'30000',N'Строительство',        N'Qurilish',            N'Qurilish',            N'ACTIVE','2024-01-01'),
(4, 0,N'40000',N'Транспорт',            N'Transport',           N'Transport',           N'ACTIVE','2024-01-01'),
(5, 0,N'50000',N'Торговля',             N'Savdo',               N'Savdo',               N'ACTIVE','2024-01-01'),
(6, 0,N'60000',N'Связь',                N'Aloqa',               N'Aloqa',               N'ACTIVE','2024-01-01'),
(7, 0,N'70000',N'Финансы',              N'Moliya',              N'Moliya',              N'ACTIVE','2024-01-01'),
(8, 0,N'80000',N'Образование',          N'Ta''lim',             N'Ta''lim',             N'ACTIVE','2024-01-01'),
(9, 0,N'90000',N'Здравоохранение',      N'Sog''liqni saqlash',  N'Sog''liqni saqlash',  N'ACTIVE','2024-01-01'),
(10,0,N'95000',N'Культура и спорт',     N'Madaniyat va sport',  N'Madaniyat va sport',  N'ACTIVE','2024-01-01');
GO

-- sp_employees
DELETE FROM [sp_employees];
INSERT INTO [sp_employees] (id, code, name_ru, name_uz, name_uzl, s_create) VALUES
(1, N'EMP001',N'Иванов Иван',      N'Ivanov Ivan',      N'Ivanov Ivan',      '2024-01-01'),
(2, N'EMP002',N'Петров Пётр',      N'Petrov Pyotr',     N'Petrov Pyotr',     '2024-01-01'),
(3, N'EMP003',N'Сидоров Сидор',    N'Sidorov Sidor',    N'Sidorov Sidor',    '2024-01-01'),
(4, N'EMP004',N'Каримов Акбар',    N'Karimov Akbar',    N'Karimov Akbar',    '2024-01-01'),
(5, N'EMP005',N'Рахимова Дилнора', N'Rahimova Dilnora', N'Rahimova Dilnora', '2024-01-01'),
(6, N'EMP006',N'Усмонов Бехзод',   N'Usmonov Behzod',   N'Usmonov Behzod',   '2024-01-01'),
(7, N'EMP007',N'Юсупова Гулнора',  N'Yusupova Gulnora', N'Yusupova Gulnora', '2024-01-01'),
(8, N'EMP008',N'Холматов Улугбек', N'Xolmatov Ulugbek', N'Xolmatov Ulugbek', '2024-01-01'),
(9, N'EMP009',N'Азимова Шахло',    N'Azimova Shahlo',   N'Azimova Shahlo',   '2024-01-01'),
(10,N'EMP010',N'Турсунов Баходир', N'Tursunov Bahodir', N'Tursunov Bahodir', '2024-01-01');
GO

-- tbcs_dictionarytype
DELETE FROM [tbcs_dictionarytype];
INSERT INTO [tbcs_dictionarytype] (REC_ID, CODE, NAME_RU, NAME_UZ) VALUES
(1, N'PLACE_TYPE',  N'Тип места',               N'Joy turi'),
(2, N'STATUS',      N'Статус',                  N'Holat'),
(3, N'GENDER',      N'Пол',                     N'Jins'),
(4, N'MEAL_TYPE',   N'Тип питания',             N'Ovqatlanish turi'),
(5, N'ROOM_TYPE',   N'Тип номера',              N'Xona turi'),
(6, N'TRIP_TYPE',   N'Тип путёвки',             N'Yo''llanma turi'),
(7, N'PAY_TYPE',    N'Тип оплаты',              N'To''lov turi'),
(8, N'CLAIM_TYPE',  N'Тип заявки',              N'Ariza turi'),
(9, N'TRANSPORT',   N'Вид транспорта',          N'Transport turi'),
(10,N'DOC_TYPE',    N'Тип документа',           N'Hujjat turi');
GO

-- tblSprav_San
DELETE FROM [tblSprav_San];
INSERT INTO [tblSprav_San] (ID, Name_San, Num_Kod, UserId, KurortSanID) VALUES
(1,  N'Санаторий Зарафшан',     N'SAN001', 1, 1),
(2,  N'Санаторий Дустлик',      N'SAN002', 1, 2),
(3,  N'Санаторий Навруз',       N'SAN003', 1, 1),
(4,  N'Санаторий Бахор',        N'SAN004', 1, 3),
(5,  N'Санаторий Чинар',        N'SAN005', 1, 2),
(6,  N'Детский лагерь Артек-UZ',N'LAG001', 1, 3),
(7,  N'Дом отдыха Янгиер',      N'DOM001', 1, 4),
(8,  N'Санаторий Ташкент',      N'SAN006', 1, 1),
(9,  N'Пансионат Самарканд',    N'PAN001', 1, 2),
(10, N'Санаторий Фергана',      N'SAN007', 1, 1),
(11, N'Санаторий Андижан',      N'SAN008', 1, 3),
(12, N'Курорт Чимган',          N'KUR001', 1, 4);
GO

-- tblSprav_Kv
DELETE FROM [tblSprav_Kv];
INSERT INTO [tblSprav_Kv] (ID, Name_Kv, UserId) VALUES
(1, N'1 квартал', 1),(2, N'2 квартал', 1),(3, N'3 квартал', 1),(4, N'4 квартал', 1),
(5, N'Январь', 1),(6, N'Февраль', 1),(7, N'Март', 1),(8, N'Апрель', 1),
(9, N'Май', 1),(10, N'Июнь', 1),(11, N'Июль', 1),(12, N'Август', 1);
GO

-- tblSprav_Reg
DELETE FROM [tblSprav_Reg];
INSERT INTO [tblSprav_Reg] (ID, name_ru, name_uz, name_uzl, code, UserId, s_create) VALUES
(1, N'г. Ташкент',             N'Toshkent shahri',      N'Toshkent shahri',      N'TAS',1,'2024-01-01'),
(2, N'Ташкентская обл.',       N'Toshkent viloyati',    N'Toshkent viloyati',    N'TV', 1,'2024-01-01'),
(3, N'Самаркандская обл.',     N'Samarqand viloyati',   N'Samarqand viloyati',   N'SV', 1,'2024-01-01'),
(4, N'Ферганская обл.',        N'Farg''ona viloyati',   N'Farg''ona viloyati',   N'FV', 1,'2024-01-01'),
(5, N'Андижанская обл.',       N'Andijon viloyati',     N'Andijon viloyati',     N'AV', 1,'2024-01-01'),
(6, N'Наманганская обл.',      N'Namangan viloyati',    N'Namangan viloyati',    N'NV', 1,'2024-01-01'),
(7, N'Кашкадарьинская обл.',   N'Qashqadaryo viloyati', N'Qashqadaryo viloyati',N'QV', 1,'2024-01-01'),
(8, N'Сурхандарьинская обл.',  N'Surxandaryo viloyati', N'Surxandaryo viloyati',N'SRV',1,'2024-01-01'),
(9, N'Бухарская обл.',         N'Buxoro viloyati',      N'Buxoro viloyati',      N'BV', 1,'2024-01-01'),
(10,N'Хорезмская обл.',        N'Xorazm viloyati',      N'Xorazm viloyati',      N'XV', 1,'2024-01-01'),
(11,N'Навоийская обл.',        N'Navoiy viloyati',      N'Navoiy viloyati',      N'NAV',1,'2024-01-01'),
(12,N'Джизакская обл.',        N'Jizzax viloyati',      N'Jizzax viloyati',      N'JV', 1,'2024-01-01');
GO

-- tblSprav_Otras
DELETE FROM [tblSprav_Otras];
INSERT INTO [tblSprav_Otras] (ID, Name_Ot, DepartmentBase, UserId) VALUES
(1, N'Профсоюз работников образования',      N'01-001', 1),
(2, N'Профсоюз медицинских работников',      N'01-002', 1),
(3, N'Профсоюз работников культуры',         N'01-003', 1),
(4, N'Профсоюз работников транспорта',       N'01-004', 1),
(5, N'Профсоюз работников торговли',         N'01-005', 1),
(6, N'Профсоюз работников промышленности',   N'01-006', 1),
(7, N'Профсоюз работников сельского хозяйства',N'01-007',1),
(8, N'Профсоюз работников строительства',    N'01-008', 1),
(9, N'Профсоюз работников связи',            N'01-009', 1),
(10,N'Профсоюз работников финансов',         N'01-010', 1);
GO

-- tblSprav_Vil
DELETE FROM [tblSprav_Vil];
INSERT INTO [tblSprav_Vil] (ID, Name_Vil, CodeVil, RegID) VALUES
(1, N'Юнусабадский р-н',  N'1703201',1),(2, N'Мирабадский р-н',   N'1703203',1),
(3, N'Алмазарский р-н',   N'1703204',1),(4, N'Бекабад',           N'1706201',2),
(5, N'Ангрен',            N'1706202',2),(6, N'Самарканд',         N'1702201',3),
(7, N'Каттакурган',       N'1702202',3),(8, N'Фергана',           N'1704201',4),
(9, N'Маргилан',          N'1704202',4),(10,N'Андижан',           N'1701201',5);
GO

-- tblSprav_MFO
DELETE FROM [tblSprav_MFO];
INSERT INTO [tblSprav_MFO] (ID, Code_MFO, Name_MFO) VALUES
(1, N'00014',N'НБУ'),(2, N'00873',N'Узпромстройбанк'),
(3, N'00440',N'Асакабанк'),(4, N'00460',N'Ипотека-банк'),
(5, N'00883',N'Микрокредитбанк'),(6, N'01018',N'Агробанк'),
(7, N'00891',N'Капиталбанк'),(8, N'01060',N'Туронбанк'),
(9, N'00926',N'Народный банк'),(10,N'01029',N'Ориёнбанк');
GO

-- tblSprav_Face
DELETE FROM [tblSprav_Face];
INSERT INTO [tblSprav_Face] (ID, Name_Face, UserId) VALUES
(1, N'Ахмедов Камол',    1),(2, N'Бекова Нилуфар',    1),
(3, N'Гулямов Санжар',   1),(4, N'Давлатова Мадина',  1),
(5, N'Эргашев Жасур',    1),(6, N'Зокиров Фаррух',    1),
(7, N'Иброхимова Шахло', 1),(8, N'Каримов Мирзо',     1),
(9, N'Латипов Бехзод',   1),(10,N'Мирзаев Отабек',    1);
GO

-- tblSprav_Os
DELETE FROM [tblSprav_Os];
INSERT INTO [tblSprav_Os] (ID, Name_Os, UserId) VALUES
(1,N'Наличные',1),(2,N'Безналичные',1),(3,N'Перечисление',1),(4,N'Карточка',1),
(5,N'Чек',1),(6,N'Аккредитив',1),(7,N'Вексель',1),(8,N'Бартер',1),
(9,N'Субсидия',1),(10,N'Дотация',1);
GO

-- tblSprav_Pr
DELETE FROM [tblSprav_Pr];
INSERT INTO [tblSprav_Pr] (ID, Name_Pr, Value, UserId) VALUES
(1,N'НДС 12%',     12.00,1),(2,N'НДС 15%',     15.00,1),
(3,N'Льгота 50%',  50.00,1),(4,N'Льгота 30%',  30.00,1),
(5,N'Льгота 20%',  20.00,1),(6,N'Льгота 10%',  10.00,1),
(7,N'Наценка 5%',   5.00,1),(8,N'Наценка 10%', 10.00,1),
(9,N'Скидка 15%',  15.00,1),(10,N'Скидка 25%', 25.00,1);
GO

-- tblSprav_Org
DELETE FROM [tblSprav_Org];
INSERT INTO [tblSprav_Org] (ID, Name_Org, INN, OWNER_CODE, ID_Ot, DepartmentBase, UserId, Address, Name_Short, Name_Full, SOATO_ID) VALUES
(1, N'ФПТУ Узбекистана',             N'202010001',N'OWN001',1,N'00-000',1,N'г. Ташкент, ул. Шота Руставели 16',N'ФПТУ',      N'Федерация профсоюзов Узбекистана', N'1703'),
(2, N'Ташкентский ОК профсоюзов',    N'202010002',N'OWN002',1,N'01-001',1,N'г. Ташкент, ул. Навои 5',          N'ТашОК',     N'Ташкентский областной комитет профсоюзов',N'1703'),
(3, N'Самаркандский ОК профсоюзов',  N'202010003',N'OWN003',2,N'01-002',1,N'г. Самарканд, ул. Регистан 1',     N'СамОК',     N'Самаркандский областной комитет',  N'1702'),
(4, N'ПК завода Тошкентэлектрик',    N'202010004',N'OWN004',6,N'02-001',1,N'г. Ташкент, ул. Файзулла 3',       N'ПК ТашЭл',  N'Профсоюзный комитет ТашЭл',        N'1703'),
(5, N'ПК строительной компании',     N'202010005',N'OWN005',8,N'02-002',1,N'г. Ташкент, ул. Буюк Ипак Йули 2',N'ПК СК',     N'Профсоюзный комитет СК',           N'1706'),
(6, N'ПК транспортной компании',     N'202010006',N'OWN006',4,N'02-003',1,N'г. Ташкент, ул. Амир Темур 12',    N'ПК ТрансКо',N'Профсоюзный комитет ТрансКо',      N'1703'),
(7, N'ПК медицинского центра',       N'202010007',N'OWN007',2,N'02-004',1,N'г. Ташкент, ул. Паркент 5',        N'ПК МедЦ',   N'Профсоюзный комитет МедЦентра',    N'1703'),
(8, N'ПК университета',              N'202010008',N'OWN008',1,N'02-005',1,N'г. Ташкент, Вузгородок',           N'ПК ТГТУ',   N'Профсоюзный комитет ТГТУ',         N'1703'),
(9, N'Ферганский ОК профсоюзов',     N'202010009',N'OWN009',4,N'01-003',1,N'г. Фергана, ул. Мустакиллик 7',    N'ФергОК',    N'Ферганский областной комитет',      N'1704'),
(10,N'Андижанский ОК профсоюзов',    N'202010010',N'OWN010',5,N'01-004',1,N'г. Андижан, ул. Бобур 3',          N'АндОК',     N'Андижанский областной комитет',     N'1701');
GO

-- t_normativdoc
DELETE FROM [t_normativdoc];
INSERT INTO [t_normativdoc] (id, name_uz, name_ru, description, s_create) VALUES
(1, N'Mehnat kodeksi',              N'Трудовой кодекс',                N'Основной документ трудового права','2024-01-01'),
(2, N'Kasaba uyushmalar to''g''risida qonun',N'Закон о профсоюзах',   N'Регулирует деятельность профсоюзов','2024-01-01'),
(3, N'Ijtimoiy sug''urta to''g''risida',     N'О социальном страховании',N'Обязательное соцстрахование',    '2024-01-01'),
(4, N'Dam olish va sanatoriy muolajasi',     N'Об оздоровлении',        N'Путёвки и санаторное лечение',    '2024-01-01'),
(5, N'Pensiya ta''minoti to''g''risida',     N'О пенсионном обеспечении',N'Пенсионные права работников',   '2024-01-01'),
(6, N'Bolalar nafaqasi to''g''risida',       N'О детских пособиях',     N'Выплаты на детей',                '2024-01-01'),
(7, N'Kolektiv shartnoma to''g''risida',     N'О коллективном договоре',N'Коллективные договоры',           '2024-01-01'),
(8, N'Ish vaqti to''g''risida',              N'О рабочем времени',      N'Режим и норма рабочего времени',  '2024-01-01'),
(9, N'Mehnat muhofazasi to''g''risida',      N'Об охране труда',        N'Правила охраны труда',            '2024-01-01'),
(10,N'Ish haqi to''g''risida',               N'Об оплате труда',        N'Порядок оплаты труда',            '2024-01-01');
GO

PRINT 'Part 1 complete.';
GO
