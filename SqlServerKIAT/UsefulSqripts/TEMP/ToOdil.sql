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
