  ALTER TABLE [KasabaKIAT_N1].dbo.tbbu_ecp_calc
  ADD OPERDATE DATETIME;
  
  ALTER TABLE [KasabaKIAT_N1].dbo.tbbu_ecp_calc
  ADD OKONX_ID VARCHAR(255);
  
   ALTER TABLE [KasabaKIAT_N1].dbo.tbbu_ecp_calc
  ADD OKONX_NAME NVARCHAR(500); 
  
  ALTER TABLE [KasabaKIAT_N1].dbo.tbbu_ecp_calc
  ADD BRANCH_CODE NVARCHAR(2); 
  
    ALTER TABLE [KasabaKIAT_N1].dbo.tbbu_ecp_calc
  ADD BRANCH_NAME NVARCHAR(200); 


UPDATE [KasabaKIAT_N1].dbo.tbbu_ecp_calc 
SET OPERDATE = (SELECT OPERDAY
               FROM [KasabaKIAT_N1].[dbo].[KaznaPaymentFile] k
               WHERE k.ID = PAYMENT_ID)
WHERE OPERDATE IS NULL;


-------------------------------------------------------------

UPDATE [KasabaKIAT_N1].dbo.tbbu_ecp_calc 
SET [KasabaKIAT_N1].dbo.tbbu_ecp_calc.OKONX_ID = EE1.okonx_id
FROM [KasabaKIAT_N1].dbo.tbbu_ecp_calc c
INNER JOIN [KasabaKIAT_N1].dbo.t_egrpo EE1 
ON (c.INN = EE1.inn)
WHERE c.OKONX_ID IS NULL;

-------------------------------------------------------------


UPDATE [KasabaKIAT_N1].dbo.tbbu_ecp_calc 
SET [KasabaKIAT_N1].dbo.tbbu_ecp_calc.OKONX_NAME = EE1.name_ru
FROM [KasabaKIAT_N1].dbo.tbbu_ecp_calc c
INNER JOIN [KasabaKIAT_N1].dbo.sp_okonx EE1 
ON (c.OKONX_ID = EE1.code)
WHERE c.OKONX_NAME IS NULL;



UPDATE [KasabaKIAT_N1].dbo.tbbu_ecp_calc 
SET [KasabaKIAT_N1].dbo.tbbu_ecp_calc.BRANCH_CODE = EE1.branch_code
FROM [KasabaKIAT_N1].dbo.tbbu_ecp_calc c
INNER JOIN [KasabaKIAT_N1].dbo.sp_okonx EE1 
ON (c.OKONX_ID = EE1.code)
WHERE c.BRANCH_CODE IS NULL;


UPDATE [KasabaKIAT_N1].dbo.tbbu_ecp_calc 
SET [KasabaKIAT_N1].dbo.tbbu_ecp_calc.BRANCH_NAME = EE1.name_ru
FROM [KasabaKIAT_N1].dbo.tbbu_ecp_calc c
INNER JOIN [KasabaKIAT_N1].dbo.sp_branch EE1 
ON (c.BRANCH_CODE = EE1.code)
WHERE c.BRANCH_NAME IS NULL;

----------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_im_KaznaNewOrg]    Script Date: 05/30/2018 12:54:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_im_KaznaNewOrg]
as 
begin 
set nocount on;

insert into dbo.t_egrpo(version, s_create, name_full, inn)
select 0 , GETDATE() , MAX(CLNAME), CLINN from dbo.KaznaPaymentFile
where NOT EXISTS (select inn from dbo.t_egrpo where inn = CLINN)
group by CLINN; 


UPDATE [KasabaKIAT_N1].dbo.tbbu_ecp_calc 
SET OPERDATE = (SELECT OPERDAY
               FROM [KasabaKIAT_N1].[dbo].[KaznaPaymentFile] k
               WHERE k.ID = PAYMENT_ID)
WHERE OPERDATE IS NULL;


UPDATE [KasabaKIAT_N1].dbo.tbbu_ecp_calc 
SET [KasabaKIAT_N1].dbo.tbbu_ecp_calc.OKONX_ID = EE1.okonx_id
FROM [KasabaKIAT_N1].dbo.tbbu_ecp_calc c
INNER JOIN [KasabaKIAT_N1].dbo.t_egrpo EE1 
ON (c.INN = EE1.inn)
WHERE c.OKONX_ID IS NULL;


UPDATE [KasabaKIAT_N1].dbo.tbbu_ecp_calc 
SET [KasabaKIAT_N1].dbo.tbbu_ecp_calc.OKONX_NAME = EE1.name_ru
FROM [KasabaKIAT_N1].dbo.tbbu_ecp_calc c
INNER JOIN [KasabaKIAT_N1].dbo.sp_okonx EE1 
ON (c.OKONX_ID = EE1.code)
WHERE c.OKONX_NAME IS NULL;


UPDATE [KasabaKIAT_N1].dbo.tbbu_ecp_calc 
SET [KasabaKIAT_N1].dbo.tbbu_ecp_calc.BRANCH_CODE = EE1.branch_code
FROM [KasabaKIAT_N1].dbo.tbbu_ecp_calc c
INNER JOIN [KasabaKIAT_N1].dbo.sp_okonx EE1 
ON (c.OKONX_ID = EE1.code)
WHERE c.BRANCH_CODE IS NULL;


UPDATE [KasabaKIAT_N1].dbo.tbbu_ecp_calc 
SET [KasabaKIAT_N1].dbo.tbbu_ecp_calc.BRANCH_NAME = EE1.name_ru
FROM [KasabaKIAT_N1].dbo.tbbu_ecp_calc c
INNER JOIN [KasabaKIAT_N1].dbo.sp_branch EE1 
ON (c.BRANCH_CODE = EE1.code)
WHERE c.BRANCH_NAME IS NULL; 
  
end


-------------------------------------------------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_Postup]    Script Date: 05/30/2018 13:06:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_rt_Razbivka_Postup]
@SOATO nvarchar(4),
@BRANCH  nvarchar(10),
@BEGIN_DATE Datetime,
@END_DATE Datetime,
@DEPARTMENT_BASE nvarchar(255)

as 
begin 
set nocount on;

/****** Script for SelectTopNRows command from SSMS  ******/
 SELECT TOP 1000 c.[K_TYPE]
      ,c.[OPERDAY]
      ,c.[ID]
      ,c.[BANKID]
      ,c.[KLSDOX]
      ,c.[DOCNUMB]
      ,c.[DOCDATE]
      ,c.[DOCTYPE]
      ,ROUND(c.[SUMPAY]/100,2) AS SUMPAY
      ,c.[CLMFO]
      ,c.[CLACC]
      ,c.[CLINN]
      ,c.[CLNAME]
      ,c.[COMFO]
      ,c.[COACC]
      ,c.[COINN]
      ,c.[CONAME]
      ,c.[PURPOSE]
      ,c.[CHAPTER]
      ,c.[OKONH]
      ,c.[FILE_ID]
      ,c.[OTCHISLEN]
      , a.PROFSOUZ_AMT AS SUMMATOTAL
      , a.FEDERATION_AMT  AS FEDERATIONSUM
      , a.OBLSOVPROF_AMT AS OBLASTSUM
      , a.[SOATO_NAME] As OblastName
      , a.[OKONX_NAME] as OkonxName
      , a.[BRANCH_NAME] as otraslName
    from  dbo.tbbu_ecp_calc a
          inner join dbo.KaznaPaymentFile c 
          on a.PAYMENT_ID  = c.ID
          left outer join dbo.tbku_StrokaProfsoyza b 
          on a.OBL_DEPARTMENT_BASE = b.MATCH_DEPARTMENTBASE 
    WHERE SUBSTRING(ISNULL(a.[SOATO] , '00000') , 1 , 4) = CASE WHEN @SOATO = '0' THEN SUBSTRING(ISNULL(a.[SOATO] , '00000') , 1 , 4) ELSE @SOATO END 
  AND ISNULL(a.[BRANCH_CODE] , '00') = CASE WHEN @BRANCH = '0' THEN ISNULL(a.[BRANCH_CODE] , '00') ELSE @BRANCH END
  AND  CONVERT(DATE,c.[OPERDAY]) BETWEEN CONVERT(DATE,@BEGIN_DATE)  AND CONVERT(DATE,@END_DATE)
  AND  [OBL_DEPARTMENT_BASE] LIKE @DEPARTMENT_BASE + '%';
 --SELECT TOP 1000 c.[K_TYPE]
 --     ,c.[OPERDAY]
 --     ,c.[ID]
 --     ,c.[BANKID]
 --     ,c.[KLSDOX]
 --     ,c.[DOCNUMB]
 --     ,c.[DOCDATE]
 --     ,c.[DOCTYPE]
 --     ,ROUND(c.[SUMPAY]/100,2) AS SUMPAY
 --     ,c.[CLMFO]
 --     ,c.[CLACC]
 --     ,c.[CLINN]
 --     ,c.[CLNAME]
 --     ,c.[COMFO]
 --     ,c.[COACC]
 --     ,c.[COINN]
 --     ,c.[CONAME]
 --     ,c.[PURPOSE]
 --     ,c.[CHAPTER]
 --     ,c.[OKONH]
 --     ,c.[FILE_ID]
 --     ,c.[OTCHISLEN]
 --     , a.PROFSOUZ_AMT AS SUMMATOTAL
 --     , a.FEDERATION_AMT  AS FEDERATIONSUM
 --     , a.OBLSOVPROF_AMT AS OBLASTSUM
 --     , ISNULL(b.ProfsoyzNaimenovanie , 'NE OPREDELEN') + '  '  +  a.REMARK  As OblastName
 --     , CASE WHEN oo.name_ru  IS NULL THEN 'Ne Opredelen' ELSE oo.name_ru END   as OkonxName
 --     , CASE WHEN bb.name_ru  IS NULL THEN 'Ne Opredelen' ELSE bb.name_ru END  as otraslName
 --   from  dbo.tbbu_ecp_calc a
 --         inner join dbo.KaznaPaymentFile c 
 --         on a.PAYMENT_ID  = c.ID
 --         left outer join dbo.tbku_StrokaProfsoyza b 
 --         on a.OBL_DEPARTMENT_BASE = b.MATCH_DEPARTMENTBASE 
 --     LEFT OUTER JOIN (select EE1.INN , MAX(EE1.SOATO_ID) AS SOATO ,  MAX(EE1.OKONX_ID) AS OKONX
	--				from dbo.t_egrpo EE1
	--				GROUP BY EE1.INN ) EE on EE.INN = SUBSTRING(a.REMARK , 5, 9)
 -- LEFT OUTER JOIN dbo.sp_okonx oo on oo.code = EE.OKONX
 -- LEFT OUTER JOIN dbo.sp_branch bb  on bb.code = oo.branch_code
 --   WHERE SUBSTRING(ISNULL(EE.SOATO , '00000') , 1 , 4) = CASE WHEN @SOATO = '0' THEN SUBSTRING(ISNULL(EE.SOATO , '00000') , 1 , 4) ELSE @SOATO END 
 -- AND ISNULL(bb.code , '00') = CASE WHEN @BRANCH = '0' THEN ISNULL(bb.code , '00') ELSE @BRANCH END
 -- AND  CONVERT(DATE,c.[OPERDAY]) BETWEEN CONVERT(DATE,@BEGIN_DATE)  AND CONVERT(DATE,@END_DATE)
 -- AND  [OBL_DEPARTMENT_BASE] LIKE @DEPARTMENT_BASE + '%';

  
end

------------------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_Postup_OtraslGroup]    Script Date: 05/30/2018 13:20:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_rt_Razbivka_Postup_OtraslGroup]
@SOATO nvarchar(4),
@BEGIN_DATE Datetime,
@END_DATE Datetime,
@DEPARTMENT_BASE nvarchar(255)

as 
begin 
set nocount on;

/****** Script for SelectTopNRows command from SSMS  ******/

SELECT 
CASE WHEN (GROUPING(TT2.OblastName) = 1) THEN 'Итого '
            ELSE ISNULL(TT2.OblastName, 'Неопределен')
       END AS OblastName ,
       CASE WHEN (GROUPING(TT2.otraslName) = 1) THEN 'Итого '
            ELSE ISNULL(TT2.otraslName, 'Неопределен')
       END AS otraslName 
, COUNT(TT2.[FEDERATION_AMT]) As Kolichestvo 
, ROUND(SUM(TT2.[FEDERATION_AMT]),2)   AS TOTALSUM
, ROUND(SUM(TT2.OBLSOVPROF_AMT),2) As OBLASTSUM
FROM
(  SELECT [ID]
      ,[PAYMENT_ID]
      ,[PROFSOUZ_AMT]
      ,[FEDERATION_AMT]
      ,[OBLSOVPROF_AMT]
      ,[OBL_DEPARTMENT_BASE]
      ,[PROVODKA_ID]
      ,[REMARK]
      ,[INN]
      ,[SOATO]
      ,[SOATO_NAME] as OblastName
      ,[OPERDATE]
      ,[OKONX_ID]
      ,[OKONX_NAME]
      ,[BRANCH_CODE]
      ,[BRANCH_NAME]  as otraslName
  FROM [KasabaKIAT_N1].dbo.tbbu_ecp_calc cc
  WHERE cc.OBL_DEPARTMENT_BASE LIKE @DEPARTMENT_BASE + '%'
  AND SUBSTRING(ISNULL(cc.SOATO , '00000') , 1 , 4) = CASE WHEN @SOATO = '0' THEN SUBSTRING(ISNULL(cc.SOATO , '00000') , 1 , 4) ELSE @SOATO END 
  AND  CONVERT(DATE,cc.OPERDATE) BETWEEN CONVERT(DATE,@BEGIN_DATE)  AND CONVERT(DATE,@END_DATE)
  ) TT2
  GROUP BY TT2.OblastName, TT2.otraslName WITH ROLLUP;
--SELECT 
--CASE WHEN (GROUPING(TT2.OblastName) = 1) THEN 'Итого '
--            ELSE ISNULL(TT2.OblastName, 'Неопределен')
--       END AS OblastName ,
--       CASE WHEN (GROUPING(TT2.otraslName) = 1) THEN 'Итого '
--            ELSE ISNULL(TT2.otraslName, 'Неопределен')
--       END AS otraslName 
--, COUNT(TT2.[SUMPAY]/100) As Kolichestvo 
--, ROUND(SUM(TT2.[FEDERATIONSUM])/100,2)   AS TOTALSUM
--, ROUND(SUM(TT2.[OBLASTSUM])/100,2) As OBLASTSUM
--FROM
--(SELECT [K_TYPE]
--      ,[OPERDAY]
--      ,TT1.[ID]
--      ,[BANKID]
--      ,[KLSDOX]
--      ,[DOCNUMB]
--      ,[DOCDATE]
--      ,[DOCTYPE]
--      ,[SUMPAY]
--      ,[CLMFO]
--      ,[CLACC]
--      ,[CLINN]
--      ,[CLNAME]
--      ,[COMFO]
--      ,[COACC]
--      ,[COINN]
--      ,[CONAME]
--      ,[PURPOSE]
--      ,[CHAPTER]
--      ,[OKONH]
--      ,[FILE_ID]
--      ,[OTCHISLEN]
--      ,SUMMATOTAL
--      , SUMMATOTAL * 0.3 AS FEDERATIONSUM
--      , SUMMATOTAL * 0.7 AS OBLASTSUM
--      , CASE WHEN ss.[name_ru] IS NULL THEN 'Ne Opredelen' ELSE ss.[name_ru] END  As OblastName
--      , CASE WHEN oo.name_ru  IS NULL THEN 'Ne Opredelen' ELSE oo.name_ru END   as OkonxName
--      , CASE WHEN bb.name_ru  IS NULL THEN 'Ne Opredelen' ELSE bb.name_ru END  as otraslName
-- FROM 
--  (SELECT a.[K_TYPE]
--      ,a.[OPERDAY]
--      ,a.[ID]
--      ,a.[BANKID]
--      ,a.[KLSDOX]
--      ,a.[DOCNUMB]
--      ,a.[DOCDATE]
--      ,a.[DOCTYPE]
--      ,a.[SUMPAY]
--      ,a.[CLMFO]
--      ,a.[CLACC]
--      ,a.[CLINN]
--      ,a.[CLNAME]
--      ,a.[COMFO]
--      ,a.[COACC]
--      ,a.[COINN]
--      ,a.[CONAME]
--      ,a.[PURPOSE]
--      ,a.[CHAPTER]
--      ,a.[OKONH]
--      ,a.[FILE_ID]
--      ,a.[OTCHISLEN]
--       ,case when a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212110%' THEN a.SUMPAY * 0.4/100  
--            when  a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212120%' THEN a.SUMPAY * 0.666666667/100  
--            when  a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212130%' THEN a.SUMPAY * 0.666666667/100 
--            ELSE SUMPAY * 0.4 /100  END AS SUMMATOTAL
--  FROM [KasabaKIAT_N1].[dbo].[KaznaPaymentFile] a
--  INNER JOIN [KasabaKIAT_N1].dbo.tbbu_ecp_calc cc ON a.ID = cc.PAYMENT_ID
--  WHERE cc.OBL_DEPARTMENT_BASE LIKE @DEPARTMENT_BASE + '%'
--  ) TT1
--  LEFT OUTER JOIN (select EE1.INN , MAX(EE1.SOATO_ID) AS SOATO ,  MAX(EE1.OKONX_ID) AS OKONX
--					from dbo.t_egrpo EE1
--					GROUP BY EE1.INN ) EE on EE.INN = TT1.[CLINN]
--  LEFT OUTER JOIN dbo.sp_soato ss on ss.code = SUBSTRING(EE.SOATO , 1 , 4)
--  LEFT OUTER JOIN dbo.sp_okonx oo on oo.code = EE.OKONX
--  LEFT OUTER JOIN dbo.sp_branch bb  on bb.code = oo.branch_code
--  WHERE SUBSTRING(ISNULL(EE.SOATO , '00000') , 1 , 4) = CASE WHEN @SOATO = '0' THEN SUBSTRING(ISNULL(EE.SOATO , '00000') , 1 , 4) ELSE @SOATO END 
--  AND  CONVERT(DATE,TT1.[OPERDAY]) BETWEEN CONVERT(DATE,@BEGIN_DATE)  AND CONVERT(DATE,@END_DATE)
--  ) TT2
--  GROUP BY TT2.OblastName, TT2.otraslName WITH ROLLUP;
  
end

---------------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_Postup_OkonxGroup]    Script Date: 05/30/2018 13:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_rt_Razbivka_Postup_OkonxGroup]
@SOATO nvarchar(4),
@BEGIN_DATE Datetime,
@END_DATE Datetime,
@DEPARTMENT_BASE nvarchar(255)

as 
begin 
set nocount on;

/****** Script for SelectTopNRows command from SSMS  ******/
SELECT 
CASE WHEN (GROUPING(TT2.OblastName) = 1) THEN 'Итого '
            ELSE ISNULL(TT2.OblastName, 'Неопределен')
       END AS OblastName ,
       CASE WHEN (GROUPING(TT2.OkonxName) = 1) THEN 'Итого '
            ELSE ISNULL(TT2.OkonxName, 'Неопределен')
       END AS OkonxName 
, COUNT(TT2.[FEDERATION_AMT]) As Kolichestvo 
, ROUND(SUM(TT2.[FEDERATION_AMT]),2)   AS TOTALSUM
, ROUND(SUM(TT2.OBLSOVPROF_AMT),2) As OBLASTSUM
FROM
(  SELECT [ID]
      ,[PAYMENT_ID]
      ,[PROFSOUZ_AMT]
      ,[FEDERATION_AMT]
      ,[OBLSOVPROF_AMT]
      ,[OBL_DEPARTMENT_BASE]
      ,[PROVODKA_ID]
      ,[REMARK]
      ,[INN]
      ,[SOATO]
      ,[SOATO_NAME] as OblastName
      ,[OPERDATE]
      ,[OKONX_ID]
      ,[OKONX_NAME] as OkonxName
      ,[BRANCH_CODE]
      ,[BRANCH_NAME]  as otraslName
  FROM [KasabaKIAT_N1].dbo.tbbu_ecp_calc cc
  WHERE cc.OBL_DEPARTMENT_BASE LIKE @DEPARTMENT_BASE + '%'
  AND SUBSTRING(ISNULL(cc.SOATO , '00000') , 1 , 4) = CASE WHEN @SOATO = '0' THEN SUBSTRING(ISNULL(cc.SOATO , '00000') , 1 , 4) ELSE @SOATO END 
  AND  CONVERT(DATE,cc.OPERDATE) BETWEEN CONVERT(DATE,@BEGIN_DATE)  AND CONVERT(DATE,@END_DATE)
  ) TT2
  GROUP BY TT2.OblastName, TT2.OkonxName WITH ROLLUP;
--SELECT CASE WHEN (GROUPING(TT2.OblastName) = 1) THEN 'Итого '
--            ELSE ISNULL(TT2.OblastName, 'Неопределен')
--       END AS OblastName ,
--       CASE WHEN (GROUPING(TT2.OkonxName) = 1) THEN 'Итого '
--            ELSE ISNULL(TT2.OkonxName, 'Неопределен')
--       END AS OkonxName 
--, COUNT(TT2.[SUMPAY]/100) As Kolichestvo 
--, ROUND(SUM(TT2.[FEDERATIONSUM])/100,2)   AS TOTALSUM
--, ROUND(SUM(TT2.[OBLASTSUM])/100,2) As OBLASTSUM
--FROM
--(SELECT [K_TYPE]
--      ,[OPERDAY]
--      ,TT1.[ID]
--      ,[BANKID]
--      ,[KLSDOX]
--      ,[DOCNUMB]
--      ,[DOCDATE]
--      ,[DOCTYPE]
--      ,[SUMPAY]
--      ,[CLMFO]
--      ,[CLACC]
--      ,[CLINN]
--      ,[CLNAME]
--      ,[COMFO]
--      ,[COACC]
--      ,[COINN]
--      ,[CONAME]
--      ,[PURPOSE]
--      ,[CHAPTER]
--      ,[OKONH]
--      ,[FILE_ID]
--      ,[OTCHISLEN]
--      ,SUMMATOTAL
--      , SUMMATOTAL * 0.3 AS FEDERATIONSUM
--      , SUMMATOTAL * 0.7 AS OBLASTSUM
--      , CASE WHEN ss.[name_ru] IS NULL THEN 'Ne Opredelen' ELSE ss.[name_ru] END  As OblastName
--      , CASE WHEN oo.name_ru  IS NULL THEN 'Ne Opredelen' ELSE oo.name_ru END   as OkonxName
--      , CASE WHEN bb.name_ru  IS NULL THEN 'Ne Opredelen' ELSE bb.name_ru END  as otraslName
-- FROM 
--  (SELECT a.[K_TYPE]
--      ,a.[OPERDAY]
--      ,a.[ID]
--      ,a.[BANKID]
--      ,a.[KLSDOX]
--      ,a.[DOCNUMB]
--      ,a.[DOCDATE]
--      ,a.[DOCTYPE]
--      ,a.[SUMPAY]
--      ,a.[CLMFO]
--      ,a.[CLACC]
--      ,a.[CLINN]
--      ,a.[CLNAME]
--      ,a.[COMFO]
--      ,a.[COACC]
--      ,a.[COINN]
--      ,a.[CONAME]
--      ,a.[PURPOSE]
--      ,a.[CHAPTER]
--      ,a.[OKONH]
--      ,a.[FILE_ID]
--      ,a.[OTCHISLEN]
--       ,case when a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212110%' THEN a.SUMPAY * 0.4 /100
--            when  a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212120%' THEN a.SUMPAY * 0.666666667/100  
--            when  a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212130%' THEN a.SUMPAY * 0.666666667/100 
--            ELSE SUMPAY * 0.4 /100  END AS SUMMATOTAL
--  FROM [KasabaKIAT_N1].[dbo].[KaznaPaymentFile] a
--  INNER JOIN [KasabaKIAT_N1].dbo.tbbu_ecp_calc cc ON a.ID = cc.PAYMENT_ID
--  WHERE cc.OBL_DEPARTMENT_BASE LIKE @DEPARTMENT_BASE + '%') TT1
--  LEFT OUTER JOIN (select EE1.INN , MAX(EE1.SOATO_ID) AS SOATO ,  MAX(EE1.OKONX_ID) AS OKONX
--					from dbo.t_egrpo EE1
--					GROUP BY EE1.INN ) EE on EE.INN = TT1.[CLINN]
--  LEFT OUTER JOIN dbo.sp_soato ss on ss.code = SUBSTRING(EE.SOATO , 1 , 4)
--  LEFT OUTER JOIN dbo.sp_okonx oo on oo.code = EE.OKONX
--  LEFT OUTER JOIN dbo.sp_branch bb  on bb.code = oo.branch_code
--  WHERE TT1.[SUMPAY] > 0 AND TT1.[CLINN] != '000000000'
--  AND SUBSTRING(ISNULL(EE.SOATO , '00000') , 1 , 4) = CASE WHEN @SOATO = '0' THEN SUBSTRING(ISNULL(EE.SOATO , '00000') , 1 , 4) ELSE @SOATO END 
--  AND  CONVERT(DATE,TT1.[OPERDAY]) BETWEEN CONVERT(DATE,@BEGIN_DATE)  AND CONVERT(DATE,@END_DATE)
--  ) TT2
--  GROUP BY TT2.OblastName,TT2.OkonxName WITH ROLLUP;
  
end

--------------------------------------------------------------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_Postup_OblastTumanGroup]    Script Date: 05/30/2018 14:03:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_rt_Razbivka_Postup_OblastTumanGroup]
@BEGIN_DATE Datetime,
@END_DATE Datetime,
@DEPARTMENT_BASE nvarchar(255)

as 
begin 
set nocount on;

/****** Script for SelectTopNRows command from SSMS  ******/

select AAA.OBLAST_NAME, AAA.TUMAN_NAME , AAA.KOLICHESTVO, AAA.summa from 
(
select CASE WHEN (GROUPING(aa1.OBLAST_NAME) = 1) THEN 'Итого '
            ELSE ISNULL(aa1.OBLAST_NAME, 'Неопределен')
       END AS OBLAST_NAME , 
       CASE WHEN (GROUPING(aa1.TUMAN_NAME) = 1) THEN 'Итого '
            ELSE ISNULL(aa1.TUMAN_NAME, 'Неопределен')
       END AS TUMAN_NAME , 
      COUNT(aa1.TUMAN_NAME) as KOLICHESTVO, 
      SUM(aa1.OBLSOVPROF_AMT) as summa, '1' as order_tp
from 
(
select AA.[ID]
      ,AA.[PAYMENT_ID]
      ,AA.[PROFSOUZ_AMT]
      ,AA.[FEDERATION_AMT]
      ,AA.[OBLSOVPROF_AMT]
      ,AA.[OBL_DEPARTMENT_BASE]
      ,AA.[PROVODKA_ID]
      ,AA.[REMARK] 
      , AA.[OPERDATE]       
      , CASE WHEN ss.KODI IS NULL THEN ss1.NAME_LN ELSE ss2.NAME_LN END AS OBLAST_NAME
      , CASE WHEN ss.KODI IS NULL THEN ss1.CENTRE_NAME_LN ELSE ss.NAME_LN END AS TUMAN_NAME
      
  from 
(select 
      SOATO AS SOATO_CODE ,
            [ID]
      ,[PAYMENT_ID]
      ,[PROFSOUZ_AMT]
      ,ABS([FEDERATION_AMT]) AS FEDERATION_AMT
      ,ABS([OBLSOVPROF_AMT]) AS OBLSOVPROF_AMT
      ,[OBL_DEPARTMENT_BASE]
      ,[PROVODKA_ID]
      ,[REMARK]  
      , [OPERDATE]                        
from dbo.tbbu_ecp_calc 
 ) AA
left outer  join dbo.tbsp_soato_new ss on aa.SOATO_CODE = ss.KODI
left outer  join dbo.tbsp_soato_new ss1 on substring(aa.SOATO_CODE, 1 ,2) + substring(aa.SOATO_CODE, 4 ,2) = ss1.KODI
left outer  join dbo.tbsp_soato_new ss2 on substring(ss.KODI, 1 ,4)  = ss2.KODI) aa1
where 
CONVERT(DATE,aa1.[OPERDATE]) 
BETWEEN CONVERT(DATE,@BEGIN_DATE)  AND CONVERT(DATE,@END_DATE)
AND aa1.[OBL_DEPARTMENT_BASE] LIKE @DEPARTMENT_BASE + '%'
group by aa1.OBLAST_NAME , aa1.TUMAN_NAME WITH ROLLUP
) AAA
order by AAA.order_tp

;

  
  
  
end

------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_Postup_OblastTumanOtraslGroup]    Script Date: 05/30/2018 14:10:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_rt_Razbivka_Postup_OblastTumanOtraslGroup]
@BEGIN_DATE Datetime,
@END_DATE Datetime,
@SOATO varchar(4),
@DEPARTMENT_BASE nvarchar(255)

as 
begin 
set nocount on;

/****** Script for SelectTopNRows command from SSMS  ******/

select CASE WHEN (GROUPING(aa1.OBLAST_NAME) = 1) THEN 'Итого '
            ELSE ISNULL(aa1.OBLAST_NAME, 'Неопределен')
       END AS OBLAST_NAME  , 
       CASE WHEN (GROUPING(aa1.TUMAN_NAME) = 1) THEN 'Итого '
            ELSE ISNULL(aa1.TUMAN_NAME, 'Неопределен')
       END AS TUMAN_NAME , 
       CASE WHEN (GROUPING(aa1.otraslName) = 1) THEN 'Итого '
            ELSE ISNULL(aa1.otraslName, 'Неопределен')
       END AS otraslName , 
       COUNT(aa1.TUMAN_NAME) as KOLICHESTVO, 
       SUM(aa1.OBLSOVPROF_AMT) as summa
from 
(
select AA.[ID]
      ,AA.[PAYMENT_ID]
      ,AA.[PROFSOUZ_AMT]
      ,AA.[FEDERATION_AMT]
      ,AA.[OBLSOVPROF_AMT]
      ,AA.[OBL_DEPARTMENT_BASE]
      ,AA.[PROVODKA_ID]
      ,AA.[REMARK]   
      ,AA.INN  
      ,ss.KODI
      ,AA.[OPERDATE]  
      , CASE WHEN bb.name_ru  IS NULL THEN 'Ne Opredelen' ELSE bb.name_ru END  as otraslName
      , CASE WHEN ss.KODI IS NULL THEN ss1.NAME_LN ELSE ss2.NAME_LN END AS OBLAST_NAME
      , CASE WHEN ss.KODI IS NULL THEN ss1.CENTRE_NAME_LN ELSE ss.NAME_LN END AS TUMAN_NAME
  from 
(select 
        SOATO as SOATO_CODE,
        INN as INN,
            [ID]
      ,[PAYMENT_ID]
      ,[PROFSOUZ_AMT]
      ,ABS([FEDERATION_AMT]) AS FEDERATION_AMT
      ,ABS([OBLSOVPROF_AMT]) AS OBLSOVPROF_AMT
      ,[OBL_DEPARTMENT_BASE]
      ,[PROVODKA_ID]
      ,[REMARK]      
      ,[OPERDATE]                   
from dbo.tbbu_ecp_calc 
where [OBL_DEPARTMENT_BASE] LIKE @DEPARTMENT_BASE + '%') AA
left outer  join dbo.tbsp_soato_new ss on aa.SOATO_CODE = ss.KODI
left outer  join dbo.tbsp_soato_new ss1 on substring(aa.SOATO_CODE, 1 ,2) + substring(aa.SOATO_CODE, 4 ,2) = ss1.KODI
left outer  join dbo.tbsp_soato_new ss2 on substring(ss.KODI, 1 ,4)  = ss2.KODI
LEFT OUTER JOIN (select EE1.INN ,  MAX(EE1.OKONX_ID) AS OKONX
					from dbo.t_egrpo EE1
					GROUP BY EE1.INN ) EE on EE.INN = AA.[INN]
LEFT OUTER JOIN dbo.sp_okonx oo on oo.code = EE.OKONX
LEFT OUTER JOIN dbo.sp_branch bb  on bb.code = oo.branch_code ) aa1
where 
CONVERT(DATE,aa1.[OPERDATE] ) BETWEEN CONVERT(DATE,@BEGIN_DATE)  AND CONVERT(DATE,@END_DATE)
AND SUBSTRING(ISNULL(aa1.KODI , '00000') , 1 , 4) = CASE WHEN @SOATO = '0' THEN SUBSTRING(ISNULL(aa1.KODI , '00000') , 1 , 4) ELSE @SOATO END 
group by aa1.OBLAST_NAME , aa1.TUMAN_NAME , aa1.otraslName WITH ROLLUP
--order by aa1.OBLAST_NAME , aa1.TUMAN_NAME , aa1.otraslName
;
  
  
  
end