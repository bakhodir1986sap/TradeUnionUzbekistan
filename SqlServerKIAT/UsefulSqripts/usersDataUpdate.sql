--navoi@kasaba.uz		--56		--98
--andijon@kasaba.uz		--17089		--80
--buxoro@kasaba.uz		--1362		--88
--fargona@kasaba.uz		--17107		--89
--jizzax@kasaba.uz		--3987		--82
--namangan@kasaba.uz	--17099		--99
--qashqadaryo@kasaba.uz --17091		--103
--qoraqalpoq@kasaba.uz   --408		--90
--samarqand@kasaba.uz   --17100		--79
--sirdaryo@kasaba.uz    --17103		--95
--surxondaryo@kasaba.uz --17102		--62
--toshkent@kasaba.uz    --14990		--94
--toshvil@kasaba.uz		--17105		--91
--xorazm@kasaba.uz      --17108		--21



UPDATE KasabaKIAT.dbo.users SET dep_id = 56		where   id = 98	;
UPDATE KasabaKIAT.dbo.users SET dep_id = 17089	where   id = 80	;
UPDATE KasabaKIAT.dbo.users SET dep_id = 1362	where   id = 88	;
UPDATE KasabaKIAT.dbo.users SET dep_id = 17107	where   id = 89	;
UPDATE KasabaKIAT.dbo.users SET dep_id = 3987	where   id = 82	;
UPDATE KasabaKIAT.dbo.users SET dep_id = 17099	where   id = 99	;
UPDATE KasabaKIAT.dbo.users SET dep_id = 17091	where   id = 103;
UPDATE KasabaKIAT.dbo.users SET dep_id = 408	where   id = 90	;
UPDATE KasabaKIAT.dbo.users SET dep_id = 17100	where   id = 79	;
UPDATE KasabaKIAT.dbo.users SET dep_id = 17103	where   id = 95	;
UPDATE KasabaKIAT.dbo.users SET dep_id = 17102	where   id = 62	;
UPDATE KasabaKIAT.dbo.users SET dep_id = 14990	where   id = 94	;
UPDATE KasabaKIAT.dbo.users SET dep_id = 17105	where   id = 91	;
UPDATE KasabaKIAT.dbo.users SET dep_id = 17108	where   id = 21	;
update KasabaKIAT.dbo.tbku_StrokaProfsoyza set MATCH_DEPARTMENTBASE = '17218' WHERE ProfsoyzKod = '000000032';
UPDATE KasabaKIAT.dbo.users SET dep_id = null	where   id = 232;

---------------------------------------------------------------------------

UPDATE KasabaKIAT.dbo.users SET user_name = 'navoi@kasaba.uz'	where   id = 98	;
UPDATE KasabaKIAT.dbo.users SET user_name = 'andijon@kasaba.uz'	where   id = 80	;
UPDATE KasabaKIAT.dbo.users SET user_name = 'buxoro@kasaba.uz'	where   id = 88	;
UPDATE KasabaKIAT.dbo.users SET user_name = 'fargona@kasaba.uz'	where   id = 89	;
UPDATE KasabaKIAT.dbo.users SET user_name = 'jizzax@kasaba.uz'	where   id = 82	;
UPDATE KasabaKIAT.dbo.users SET user_name = 'namangan@kasaba.uz'	where   id = 99	;
UPDATE KasabaKIAT.dbo.users SET user_name = 'qashqadaryo@kasaba.uz'	where   id = 103;
UPDATE KasabaKIAT.dbo.users SET user_name = 'qoraqalpoq@kasaba.uz'   where   id = 90	;
UPDATE KasabaKIAT.dbo.users SET user_name = 'samarqand@kasaba.uz'	where   id = 79	;
UPDATE KasabaKIAT.dbo.users SET user_name = 'sirdaryo@kasaba.uz'	where   id = 95	;
UPDATE KasabaKIAT.dbo.users SET user_name = 'surxondaryo@kasaba.uz'	where   id = 62	;
UPDATE KasabaKIAT.dbo.users SET user_name = 'toshkent@kasaba.uz'	where   id = 94	;
UPDATE KasabaKIAT.dbo.users SET user_name = 'toshvil@kasaba.uz'	where   id = 91	;
UPDATE KasabaKIAT.dbo.users SET user_name = 'xorazm@kasaba.uz'	where   id = 21	;

--------------------------------------------------------------------------------14:02
UPDATE [KasabaKIAT].[dbo].[tblPost] 
SET 
    ID_KV = DATEPART(QUARTER, [Data_Post]) ,
	[SmFil] = YEAR([Data_Post])
WHERE ID_KV IS NULL OR [SmFil] IS NULL;


  ALTER TABLE [KasabaKIAT].[dbo].[tblSprav_Vil]
  ADD SOATO_CODE varchar(10);

  	UPDATE [KasabaKIAT].[dbo].[tblSprav_Vil] SET SOATO_CODE = '1703' where ID=4		;
	UPDATE [KasabaKIAT].[dbo].[tblSprav_Vil] SET SOATO_CODE = '1706' where ID=5		;
	UPDATE [KasabaKIAT].[dbo].[tblSprav_Vil] SET SOATO_CODE = '1708' where ID=6		;
	UPDATE [KasabaKIAT].[dbo].[tblSprav_Vil] SET SOATO_CODE = '1710' where ID=7		;
	UPDATE [KasabaKIAT].[dbo].[tblSprav_Vil] SET SOATO_CODE = '1712' where ID=9		;
	UPDATE [KasabaKIAT].[dbo].[tblSprav_Vil] SET SOATO_CODE = '1714' where ID=8		;
	UPDATE [KasabaKIAT].[dbo].[tblSprav_Vil] SET SOATO_CODE = '1735' where ID=15	;
	UPDATE [KasabaKIAT].[dbo].[tblSprav_Vil] SET SOATO_CODE = '1718' where ID=10	;
	UPDATE [KasabaKIAT].[dbo].[tblSprav_Vil] SET SOATO_CODE = '1722' where ID=11	;
	UPDATE [KasabaKIAT].[dbo].[tblSprav_Vil] SET SOATO_CODE = '1724' where ID=12	;
	UPDATE [KasabaKIAT].[dbo].[tblSprav_Vil] SET SOATO_CODE = '1726' where ID=2		;
	UPDATE [KasabaKIAT].[dbo].[tblSprav_Vil] SET SOATO_CODE = '1727' where ID=3		;
	UPDATE [KasabaKIAT].[dbo].[tblSprav_Vil] SET SOATO_CODE = '1730' where ID=13	;
	UPDATE [KasabaKIAT].[dbo].[tblSprav_Vil] SET SOATO_CODE = '1733' where ID=14	;


	  ALTER TABLE [KasabaKIAT].[dbo].[tblSprav_Reg]
      ADD VilId bigint;

	  ALTER TABLE [KasabaKIAT].[dbo].[tblSprav_Reg]
	  ADD SOATO_CODE varchar(10);


UPDATE [KasabaKIAT].[dbo].[tblSprav_Reg] SET SOATO_CODE = '1726264' WHERE ID 	= 2	;     
UPDATE [KasabaKIAT].[dbo].[tblSprav_Reg] SET SOATO_CODE = '1726269' WHERE ID 	= 3	;     
UPDATE [KasabaKIAT].[dbo].[tblSprav_Reg] SET SOATO_CODE = '1726273' WHERE ID 	= 4	;     
UPDATE [KasabaKIAT].[dbo].[tblSprav_Reg] SET SOATO_CODE = '1726280' WHERE ID 	= 8	;     
UPDATE [KasabaKIAT].[dbo].[tblSprav_Reg] SET SOATO_CODE = '1726283' WHERE ID 	= 5	;     
UPDATE [KasabaKIAT].[dbo].[tblSprav_Reg] SET SOATO_CODE = '1726262' WHERE ID 	= 7	;     
UPDATE [KasabaKIAT].[dbo].[tblSprav_Reg] SET SOATO_CODE = '1726294' WHERE ID 	= 10;     
UPDATE [KasabaKIAT].[dbo].[tblSprav_Reg] SET SOATO_CODE = '1726277' WHERE ID 	= 11;     
UPDATE [KasabaKIAT].[dbo].[tblSprav_Reg] SET SOATO_CODE = '1726266' WHERE ID 	= 12;     
UPDATE [KasabaKIAT].[dbo].[tblSprav_Reg] SET SOATO_CODE = '1726287' WHERE ID 	= 1	;     
UPDATE [KasabaKIAT].[dbo].[tblSprav_Reg] SET SOATO_CODE = '1726290' WHERE ID 	= 9	;     

UPDATE  [KasabaKIAT].[dbo].[tblSprav_Reg] 
SET VilId = 2
WHERE ID != 6;

INSERT INTO [KasabaKIAT].[dbo].[tblSprav_Reg]
  SELECT 
		AA.[name_ru] ,
		AA.RowNUmber + 100, 
		1,
		null,
		15,
		AA.[code]
  FROM 
  (SELECT ROW_NUMBER() OVER(ORDER BY name_ru ASC) AS RowNUmber
      ,[name_ru]
      ,code
      FROM [KasabaKIAT].[dbo].[sp_soato]
  WHERE [parent_code] = 1735200) AA;


 INSERT INTO [KasabaKIAT].[dbo].[tblSprav_Reg]
    SELECT 
		AA.[name_ru] ,
		AA.RowNUmber + 200, 
		1,
		null,
		3,
		AA.[code]
  FROM 
  (SELECT ROW_NUMBER() OVER(ORDER BY name_ru ASC) AS RowNUmber
      ,[name_ru]
      ,code
      FROM [KasabaKIAT].[dbo].[sp_soato]
  WHERE [parent_code] = 1727200) AA;

 INSERT INTO [KasabaKIAT].[dbo].[tblSprav_Reg]
    SELECT 
		AA.[name_ru] ,
		AA.RowNUmber + 300, 
		1,
		null,
		4,
		AA.[code]
  FROM 
  (SELECT ROW_NUMBER() OVER(ORDER BY name_ru ASC) AS RowNUmber
      ,[name_ru]
      ,code
      FROM [KasabaKIAT].[dbo].[sp_soato]
  WHERE [parent_code] = 1703200) AA;

INSERT INTO [KasabaKIAT].[dbo].[tblSprav_Reg] 
    SELECT 
		AA.[name_ru] ,
		AA.RowNUmber + 500, 
		1,
		null,
		5,
		AA.[code]
  FROM 
  (SELECT ROW_NUMBER() OVER(ORDER BY name_ru ASC) AS RowNUmber
      ,[name_ru]
      ,code
      FROM [KasabaKIAT].[dbo].[sp_soato]
  WHERE [parent_code] = 1706200) AA;


INSERT INTO [KasabaKIAT].[dbo].[tblSprav_Reg]  
    SELECT 
		AA.[name_ru] ,
		AA.RowNUmber + 600, 
		1,
		null,
		6,
		AA.[code]
  FROM 
  (SELECT ROW_NUMBER() OVER(ORDER BY name_ru ASC) AS RowNUmber
      ,[name_ru]
      ,code
      FROM [KasabaKIAT].[dbo].[sp_soato]
  WHERE [parent_code] = 1708200) AA;



  INSERT INTO [KasabaKIAT].[dbo].[tblSprav_Reg]   
    SELECT 
		AA.[name_ru] ,
		AA.RowNUmber + 700, 
		1,
		null,
		7,
		AA.[code]
  FROM 
  (SELECT ROW_NUMBER() OVER(ORDER BY name_ru ASC) AS RowNUmber
      ,[name_ru]
      ,code
      FROM [KasabaKIAT].[dbo].[sp_soato]
  WHERE [parent_code] = 1710200) AA;


 INSERT INTO [KasabaKIAT].[dbo].[tblSprav_Reg]  
    SELECT 
		AA.[name_ru] ,
		AA.RowNUmber + 800, 
		1,
		null,
		8,
		AA.[code]
  FROM 
  (SELECT ROW_NUMBER() OVER(ORDER BY name_ru ASC) AS RowNUmber
      ,[name_ru]
      ,code
      FROM [KasabaKIAT].[dbo].[sp_soato]
  WHERE [parent_code] = 1714200) AA;


   INSERT INTO [KasabaKIAT].[dbo].[tblSprav_Reg]  
    SELECT 
		AA.[name_ru] ,
		AA.RowNUmber + 900, 
		1,
		null,
		9,
		AA.[code]
  FROM 
  (SELECT ROW_NUMBER() OVER(ORDER BY name_ru ASC) AS RowNUmber
      ,[name_ru]
      ,code
      FROM [KasabaKIAT].[dbo].[sp_soato]
  WHERE [parent_code] = 1712200) AA;


    
INSERT INTO [KasabaKIAT].[dbo].[tblSprav_Reg]    
    SELECT 
		AA.[name_ru] ,
		AA.RowNUmber + 1000, 
		1,
		null,
		10,
		AA.[code]
  FROM 
  (SELECT ROW_NUMBER() OVER(ORDER BY name_ru ASC) AS RowNUmber
      ,[name_ru]
      ,code
      FROM [KasabaKIAT].[dbo].[sp_soato]
  WHERE [parent_code] = 1718200) AA;



INSERT INTO [KasabaKIAT].[dbo].[tblSprav_Reg]   
    SELECT 
		AA.[name_ru] ,
		AA.RowNUmber + 1100, 
		1,
		null,
		11,
		AA.[code]
  FROM 
  (SELECT ROW_NUMBER() OVER(ORDER BY name_ru ASC) AS RowNUmber
      ,[name_ru]
      ,code
      FROM [KasabaKIAT].[dbo].[sp_soato]
  WHERE [parent_code] = 1722200) AA;



   INSERT INTO [KasabaKIAT].[dbo].[tblSprav_Reg]   
    SELECT 
		AA.[name_ru] ,
		AA.RowNUmber + 1200, 
		1,
		null,
		12,
		AA.[code]
  FROM 
  (SELECT ROW_NUMBER() OVER(ORDER BY name_ru ASC) AS RowNUmber
      ,[name_ru]
      ,code
      FROM [KasabaKIAT].[dbo].[sp_soato]
  WHERE [parent_code] = 1724200) AA;
  
  
  
  
   INSERT INTO [KasabaKIAT].[dbo].[tblSprav_Reg]   
    SELECT 
		AA.[name_ru] ,
		AA.RowNUmber + 1300, 
		1,
		null,
		13,
		AA.[code]
  FROM 
  (SELECT ROW_NUMBER() OVER(ORDER BY name_ru ASC) AS RowNUmber
      ,[name_ru]
      ,code
      FROM [KasabaKIAT].[dbo].[sp_soato]
  WHERE [parent_code] = 1730200) AA;
  
 INSERT INTO [KasabaKIAT].[dbo].[tblSprav_Reg]   
    SELECT 
		AA.[name_ru] ,
		AA.RowNUmber + 1400, 
		1,
		null,
		14,
		AA.[code]
  FROM 
  (SELECT ROW_NUMBER() OVER(ORDER BY name_ru ASC) AS RowNUmber
      ,[name_ru]
      ,code
      FROM [KasabaKIAT].[dbo].[sp_soato]
  WHERE [parent_code] = 1733200) AA
  ---------------------------------------------------

  USE [KasabaKIAT]
GO
/****** Object:  StoredProcedure [dbo].[sp_get_RealizasiyaPutyovokDetail]    Script Date: 02/17/2018 17:22:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_get_RealizasiyaPutyovokDetail]
@DEP_BASE varchar(255),
@SANID int
as 
begin 
set nocount on;

SELECT B.ID_SANAT , 
	   SAN.Name_San ,
	   B.NUM_PUT , 
	   B.DATASAN,
	   CASE WHEN  C.ID IS NULL or c.Uchet = 0 THEN 0 ELSE 1 END BRON_FLG,
	   CASE WHEN  DATEDIFF(DAY, GETDATE() , B.DATASAN) <= 14 THEN 1 ELSE 0 END URGENT_FLG
FROM KASABAKIAT.dbo.tblPost A
INNER JOIN KASABAKIAT.dbo.tblPos_Put B 
ON A.ID = B.ID_POST
INNER JOIN KASABAKIAT.dbo.tblSprav_San SAN
ON SAN.ID = B.ID_SANAT
LEFT JOIN  KASABAKIAT.dbo.tblBron C    
ON C.ID_San = B.ID_SANAT AND C.NUM_PUT = B.Num_Put
WHERE B.ID_SANAT = @SANID
AND B.ID_POLVOZ = 0 
AND A.DEPARTMENTBASE = @DEP_BASE;
  
end

----------------------------------------------------------------

--USE [KasabaKIAT]
--GO
/****** Object:  StoredProcedure [dbo].[sp_get_RealizasiyaPutyovokGroup]    Script Date: 02/17/2018 17:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_get_RealizasiyaPutyovokGroup]
@DEP_BASE varchar(255)
as 
begin 
set nocount on;

SELECT AA.SANID,
     AA.SAN_NAME,
     MIN(AA.NUM_PUT) AS MIN_NUM,
     MAX(AA.NUM_PUT) AS MAX_NUM,
     MIN(AA.DATA_SAN) AS MIN_DATE,
     MAX(AA.DATA_SAN) AS MAX_DATE,   
     SUM(AA.BRON_FLG) AS BRON,
     SUM(AA.URGENT_FLG) AS URGENT
FROM 
(SELECT B.ID_SANAT AS SANID, 
     SAN.Name_San AS SAN_NAME ,
     B.NUM_PUT AS NUM_PUT, 
     B.DATASAN AS DATA_SAN,
     CASE WHEN  C.ID IS NULL or c.Uchet = 0 THEN 0 ELSE 1 END BRON_FLG,
     CASE WHEN  DATEDIFF(DAY, GETDATE() , B.DATASAN) <= 14 THEN 1 ELSE 0 END URGENT_FLG
FROM KASABAKIAT.dbo.tblPost A
INNER JOIN KASABAKIAT.dbo.tblPos_Put B 
ON A.ID = B.ID_POST
INNER JOIN KASABAKIAT.dbo.tblSprav_San SAN
ON SAN.ID = B.ID_SANAT
LEFT JOIN  KASABAKIAT.dbo.tblBron C    
ON C.ID_San = B.ID_SANAT AND C.NUM_PUT = B.Num_Put
WHERE B.ID_POLVOZ = 0 
AND A.DEPARTMENTBASE = @DEP_BASE) AA
GROUP BY AA.SANID,
     AA.SAN_NAME;
  
end



--code	name_uz
--1726264	Бектемир тумани			--2	Бектемир
--1726269	Мирзо Улу?бек тумани    --3	Мирзо Улугбек
--1726273	Миробод тумани			--4	Миробод
--1726280	Олмазор тумани			--8	Олмазор
--1726283	Сир?али тумани			--5	Сергелий
--1726262	Учтепа тумани			--7	Уч Тепа
--1726294	Чилонзор тумани			--10	Чилонзор
--1726277	Шайхонтохур тумани		--11	Шайхантахур
--1726266	Юнусобод тумани			--12	Юнусобод
--1726287	Яккасарой тумани		--1	Яккасарой
--1726290	Яшнобод тумани			--9	Хамза

--ID	NameReg
--2	Бектемир
--3	Мирзо Улугбек
--4	Миробод
--8	Олмазор
--6	Олмалик
--5	Сергелий
--7	Уч Тепа
--9	Хамза
--10	Чилонзор
--11	Шайхантахур
--12	Юнусобод
--1	Яккасарой

--code	name_ru
--1703	Андижанская область          
--1706	Бухарская область
--1726	город Ташкент
--1708	Джизакская область
--1710	Кашкадарьинская область
--1712	Навоийская область
--1714	Наманганская область
--1735	Республика Каракалпакстан
--1718	Самаркандская область
--1722	Сурхандарьинская область
--1724	Сырдарьинская область
--1727	Ташкентская область
--1730	Ферганская область
--1733	Хорезмская область

--ID	FullName


--4		Андижанская область						--1703	Андижанская область   	
--5		Бухарская область						--1706	Бухарская область
--6		Джизакская область						--1708	Джизакская область
--7		Кашкадарьинская область					--1710	Кашкадарьинская область
--9		Навоинская область						--1712	Навоийская область
--8		Наманганская область					--1714	Наманганская область
--15	Автономнас Республика Каракалпакстан	--1735	Республика Каракалпакстан
--10	Самаркандская область					--1718	Самаркандская область
--11	Сурхандарьинская область				--1722	Сурхандарьинская область
--12	Сырдарьинская область					--1724	Сырдарьинская область
--2		Ташкент город							--1726	город Ташкент
--3		Ташкент область							--1727	Ташкентская область
--13	Ферганская область						--1730	Ферганская область
--14	Хорезмская область						--1733	Хорезмская область

--*********************************
--17	Алмалыкская ГМК
--16	Навоинская ГМК





