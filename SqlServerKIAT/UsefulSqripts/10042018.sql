USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_Postup_OtraslGroup]    Script Date: 04/10/2018 18:42:15 ******/
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
, COUNT(TT2.[SUMPAY]/100) As Kolichestvo 
, ROUND(SUM(TT2.[FEDERATIONSUM])/100,2)   AS TOTALSUM
, ROUND(SUM(TT2.[OBLASTSUM])/100,2) As OBLASTSUM
FROM
(SELECT [K_TYPE]
      ,[OPERDAY]
      ,TT1.[ID]
      ,[BANKID]
      ,[KLSDOX]
      ,[DOCNUMB]
      ,[DOCDATE]
      ,[DOCTYPE]
      ,[SUMPAY]
      ,[CLMFO]
      ,[CLACC]
      ,[CLINN]
      ,[CLNAME]
      ,[COMFO]
      ,[COACC]
      ,[COINN]
      ,[CONAME]
      ,[PURPOSE]
      ,[CHAPTER]
      ,[OKONH]
      ,[FILE_ID]
      ,[OTCHISLEN]
      ,SUMMATOTAL
      , SUMMATOTAL * 0.3 AS FEDERATIONSUM
      , SUMMATOTAL * 0.7 AS OBLASTSUM
      , CASE WHEN ss.[name_ru] IS NULL THEN 'Ne Opredelen' ELSE ss.[name_ru] END  As OblastName
      , CASE WHEN oo.name_ru  IS NULL THEN 'Ne Opredelen' ELSE oo.name_ru END   as OkonxName
      , CASE WHEN bb.name_ru  IS NULL THEN 'Ne Opredelen' ELSE bb.name_ru END  as otraslName
 FROM 
  (SELECT a.[K_TYPE]
      ,a.[OPERDAY]
      ,a.[ID]
      ,a.[BANKID]
      ,a.[KLSDOX]
      ,a.[DOCNUMB]
      ,a.[DOCDATE]
      ,a.[DOCTYPE]
      ,a.[SUMPAY]
      ,a.[CLMFO]
      ,a.[CLACC]
      ,a.[CLINN]
      ,a.[CLNAME]
      ,a.[COMFO]
      ,a.[COACC]
      ,a.[COINN]
      ,a.[CONAME]
      ,a.[PURPOSE]
      ,a.[CHAPTER]
      ,a.[OKONH]
      ,a.[FILE_ID]
      ,a.[OTCHISLEN]
       ,case when a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212110%' THEN a.SUMPAY * 0.4/100  
            when  a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212120%' THEN a.SUMPAY * 0.666666667/100  
            when  a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212130%' THEN a.SUMPAY * 0.666666667/100 
            ELSE SUMPAY * 0.4 /100  END AS SUMMATOTAL
  FROM [KasabaKIAT_N1].[dbo].[KaznaPaymentFile] a
  INNER JOIN [KasabaKIAT_N1].dbo.tbbu_ecp_calc cc ON a.ID = cc.PAYMENT_ID
  WHERE cc.OBL_DEPARTMENT_BASE LIKE @DEPARTMENT_BASE + '%'
  ) TT1
  LEFT OUTER JOIN (select EE1.INN , MAX(EE1.SOATO_ID) AS SOATO ,  MAX(EE1.OKONX_ID) AS OKONX
					from dbo.t_egrpo EE1
					GROUP BY EE1.INN ) EE on EE.INN = TT1.[CLINN]
  LEFT OUTER JOIN dbo.sp_soato ss on ss.code = SUBSTRING(EE.SOATO , 1 , 4)
  LEFT OUTER JOIN dbo.sp_okonx oo on oo.code = EE.OKONX
  LEFT OUTER JOIN dbo.sp_branch bb  on bb.code = oo.branch_code
  WHERE SUBSTRING(ISNULL(EE.SOATO , '00000') , 1 , 4) = CASE WHEN @SOATO = '0' THEN SUBSTRING(ISNULL(EE.SOATO , '00000') , 1 , 4) ELSE @SOATO END 
  AND  CONVERT(DATE,TT1.[OPERDAY]) BETWEEN CONVERT(DATE,@BEGIN_DATE)  AND CONVERT(DATE,@END_DATE)
  ) TT2
  GROUP BY TT2.OblastName, TT2.otraslName WITH ROLLUP;
  
end

----------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_Postup_OkonxGroup]    Script Date: 04/10/2018 18:42:08 ******/
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

SELECT CASE WHEN (GROUPING(TT2.OblastName) = 1) THEN 'Итого '
            ELSE ISNULL(TT2.OblastName, 'Неопределен')
       END AS OblastName ,
       CASE WHEN (GROUPING(TT2.OkonxName) = 1) THEN 'Итого '
            ELSE ISNULL(TT2.OkonxName, 'Неопределен')
       END AS OkonxName 
, COUNT(TT2.[SUMPAY]/100) As Kolichestvo 
, ROUND(SUM(TT2.[FEDERATIONSUM])/100,2)   AS TOTALSUM
, ROUND(SUM(TT2.[OBLASTSUM])/100,2) As OBLASTSUM
FROM
(SELECT [K_TYPE]
      ,[OPERDAY]
      ,TT1.[ID]
      ,[BANKID]
      ,[KLSDOX]
      ,[DOCNUMB]
      ,[DOCDATE]
      ,[DOCTYPE]
      ,[SUMPAY]
      ,[CLMFO]
      ,[CLACC]
      ,[CLINN]
      ,[CLNAME]
      ,[COMFO]
      ,[COACC]
      ,[COINN]
      ,[CONAME]
      ,[PURPOSE]
      ,[CHAPTER]
      ,[OKONH]
      ,[FILE_ID]
      ,[OTCHISLEN]
      ,SUMMATOTAL
      , SUMMATOTAL * 0.3 AS FEDERATIONSUM
      , SUMMATOTAL * 0.7 AS OBLASTSUM
      , CASE WHEN ss.[name_ru] IS NULL THEN 'Ne Opredelen' ELSE ss.[name_ru] END  As OblastName
      , CASE WHEN oo.name_ru  IS NULL THEN 'Ne Opredelen' ELSE oo.name_ru END   as OkonxName
      , CASE WHEN bb.name_ru  IS NULL THEN 'Ne Opredelen' ELSE bb.name_ru END  as otraslName
 FROM 
  (SELECT a.[K_TYPE]
      ,a.[OPERDAY]
      ,a.[ID]
      ,a.[BANKID]
      ,a.[KLSDOX]
      ,a.[DOCNUMB]
      ,a.[DOCDATE]
      ,a.[DOCTYPE]
      ,a.[SUMPAY]
      ,a.[CLMFO]
      ,a.[CLACC]
      ,a.[CLINN]
      ,a.[CLNAME]
      ,a.[COMFO]
      ,a.[COACC]
      ,a.[COINN]
      ,a.[CONAME]
      ,a.[PURPOSE]
      ,a.[CHAPTER]
      ,a.[OKONH]
      ,a.[FILE_ID]
      ,a.[OTCHISLEN]
       ,case when a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212110%' THEN a.SUMPAY * 0.4 /100
            when  a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212120%' THEN a.SUMPAY * 0.666666667/100  
            when  a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212130%' THEN a.SUMPAY * 0.666666667/100 
            ELSE SUMPAY * 0.4 /100  END AS SUMMATOTAL
  FROM [KasabaKIAT_N1].[dbo].[KaznaPaymentFile] a
  INNER JOIN [KasabaKIAT_N1].dbo.tbbu_ecp_calc cc ON a.ID = cc.PAYMENT_ID
  WHERE cc.OBL_DEPARTMENT_BASE LIKE @DEPARTMENT_BASE + '%') TT1
  LEFT OUTER JOIN (select EE1.INN , MAX(EE1.SOATO_ID) AS SOATO ,  MAX(EE1.OKONX_ID) AS OKONX
					from dbo.t_egrpo EE1
					GROUP BY EE1.INN ) EE on EE.INN = TT1.[CLINN]
  LEFT OUTER JOIN dbo.sp_soato ss on ss.code = SUBSTRING(EE.SOATO , 1 , 4)
  LEFT OUTER JOIN dbo.sp_okonx oo on oo.code = EE.OKONX
  LEFT OUTER JOIN dbo.sp_branch bb  on bb.code = oo.branch_code
  WHERE TT1.[SUMPAY] > 0 AND TT1.[CLINN] != '000000000'
  AND SUBSTRING(ISNULL(EE.SOATO , '00000') , 1 , 4) = CASE WHEN @SOATO = '0' THEN SUBSTRING(ISNULL(EE.SOATO , '00000') , 1 , 4) ELSE @SOATO END 
  AND  CONVERT(DATE,TT1.[OPERDAY]) BETWEEN CONVERT(DATE,@BEGIN_DATE)  AND CONVERT(DATE,@END_DATE)
  ) TT2
  GROUP BY TT2.OblastName,TT2.OkonxName WITH ROLLUP;
  
end

-------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_Postup_OblastTumanOtraslGroup]    Script Date: 04/10/2018 18:42:02 ******/
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
      , CASE WHEN bb.name_ru  IS NULL THEN 'Ne Opredelen' ELSE bb.name_ru END  as otraslName
      , CASE WHEN ss.KODI IS NULL THEN ss1.NAME_LN ELSE ss2.NAME_LN END AS OBLAST_NAME
      , CASE WHEN ss.KODI IS NULL THEN ss1.CENTRE_NAME_LN ELSE ss.NAME_LN END AS TUMAN_NAME
  from 
(select CASE WHEN REMARK like '%SOATO%' THEN 
                                       CASE WHEN REMARK like '%PROBLEM%' THEN SUBSTRING(REMARK, 19, 7)
                                           WHEN REMARK not like '%PROBLEM%' THEN SUBSTRING(REMARK, 21, 7) END 
            ELSE CASE WHEN OBL_DEPARTMENT_BASE = '' then '17' else OBL_DEPARTMENT_BASE end END   AS SOATO_CODE,
        CASE WHEN REMARK like '%INN%' THEN CASE WHEN REMARK like '%INN:PROBLEM%' THEN '000000000'
                                           WHEN REMARK not like '%PROBLEM%' THEN SUBSTRING(REMARK, 5, 9) END  
             ELSE '000000000' END INN  ,                                
            [ID]
      ,[PAYMENT_ID]
      ,[PROFSOUZ_AMT]
      ,ABS([FEDERATION_AMT]) AS FEDERATION_AMT
      ,ABS([OBLSOVPROF_AMT]) AS OBLSOVPROF_AMT
      ,[OBL_DEPARTMENT_BASE]
      ,[PROVODKA_ID]
      ,[REMARK]                          
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
inner join dbo.KaznaPaymentFile kk on kk.ID  = aa1.PAYMENT_ID 
where 
CONVERT(DATE,kk.[OPERDAY]) BETWEEN CONVERT(DATE,@BEGIN_DATE)  AND CONVERT(DATE,@END_DATE)
AND SUBSTRING(ISNULL(aa1.KODI , '00000') , 1 , 4) = CASE WHEN @SOATO = '0' THEN SUBSTRING(ISNULL(aa1.KODI , '00000') , 1 , 4) ELSE @SOATO END 
group by aa1.OBLAST_NAME , aa1.TUMAN_NAME , aa1.otraslName WITH ROLLUP
--order by aa1.OBLAST_NAME , aa1.TUMAN_NAME , aa1.otraslName
;
  
  
  
end


------------
USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_Postup_OblastTumanGroup]    Script Date: 04/10/2018 18:41:55 ******/
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
      , CASE WHEN ss.KODI IS NULL THEN ss1.NAME_LN ELSE ss2.NAME_LN END AS OBLAST_NAME
      , CASE WHEN ss.KODI IS NULL THEN ss1.CENTRE_NAME_LN ELSE ss.NAME_LN END AS TUMAN_NAME
      
  from 
(select CASE WHEN REMARK like '%SOATO%' THEN 
                                       CASE WHEN REMARK like '%PROBLEM%' THEN SUBSTRING(REMARK, 19, 7)
                                           WHEN REMARK not like '%PROBLEM%' THEN SUBSTRING(REMARK, 21, 7) END 
            ELSE CASE WHEN OBL_DEPARTMENT_BASE = '' then '17' else OBL_DEPARTMENT_BASE end END   AS SOATO_CODE,
            [ID]
      ,[PAYMENT_ID]
      ,[PROFSOUZ_AMT]
      ,ABS([FEDERATION_AMT]) AS FEDERATION_AMT
      ,ABS([OBLSOVPROF_AMT]) AS OBLSOVPROF_AMT
      ,[OBL_DEPARTMENT_BASE]
      ,[PROVODKA_ID]
      ,[REMARK]                          
from dbo.tbbu_ecp_calc 
where [OBL_DEPARTMENT_BASE] LIKE @DEPARTMENT_BASE + '%') AA
left outer  join dbo.tbsp_soato_new ss on aa.SOATO_CODE = ss.KODI
left outer  join dbo.tbsp_soato_new ss1 on substring(aa.SOATO_CODE, 1 ,2) + substring(aa.SOATO_CODE, 4 ,2) = ss1.KODI
left outer  join dbo.tbsp_soato_new ss2 on substring(ss.KODI, 1 ,4)  = ss2.KODI) aa1
inner join dbo.KaznaPaymentFile kk on kk.ID  = aa1.PAYMENT_ID 
where --aa1.OBLSOVPROF_AMT > 0 AND 
CONVERT(DATE,kk.[OPERDAY]) 
BETWEEN CONVERT(DATE,@BEGIN_DATE)  AND CONVERT(DATE,@END_DATE)
group by aa1.OBLAST_NAME , aa1.TUMAN_NAME WITH ROLLUP
--order by aa1.OBLAST_NAME , aa1.TUMAN_NAME

--UNION 

--select 'Итого: ' as OBLAST_NAME , '' as TUMAN_NAME, SUM(JJ1.KOLICHESTVO) as KOLICHESTVO ,SUM(JJ1.summa) as summa , '2' as order_tp  from 
--(select aa1.OBLAST_NAME , aa1.TUMAN_NAME , COUNT(aa1.TUMAN_NAME) as KOLICHESTVO, SUM(aa1.OBLSOVPROF_AMT) as summa 
--from 
--(
--select AA.[ID]
--      ,AA.[PAYMENT_ID]
--      ,AA.[PROFSOUZ_AMT]
--      ,AA.[FEDERATION_AMT]
--      ,AA.[OBLSOVPROF_AMT]
--      ,AA.[OBL_DEPARTMENT_BASE]
--      ,AA.[PROVODKA_ID]
--      ,AA.[REMARK]     
--      , CASE WHEN ss.KODI IS NULL THEN ss1.NAME_LN ELSE ss2.NAME_LN END AS OBLAST_NAME
--      , CASE WHEN ss.KODI IS NULL THEN ss1.CENTRE_NAME_LN ELSE ss.NAME_LN END AS TUMAN_NAME
--  from 
--(select CASE WHEN REMARK like '%SOATO%' THEN 
--                                       CASE WHEN REMARK like '%PROBLEM%' THEN SUBSTRING(REMARK, 19, 7)
--                                           WHEN REMARK not like '%PROBLEM%' THEN SUBSTRING(REMARK, 21, 7) END 
--            ELSE CASE WHEN OBL_DEPARTMENT_BASE = '' then '17' else OBL_DEPARTMENT_BASE end END   AS SOATO_CODE,
--            [ID]
--      ,[PAYMENT_ID]
--      ,[PROFSOUZ_AMT]
--      ,ABS([FEDERATION_AMT]) AS FEDERATION_AMT
--      ,ABS([OBLSOVPROF_AMT]) AS OBLSOVPROF_AMT
--      ,[OBL_DEPARTMENT_BASE]
--      ,[PROVODKA_ID]
--      ,[REMARK]                          
--from dbo.tbbu_ecp_calc ) AA
--left outer  join dbo.tbsp_soato_new ss on aa.SOATO_CODE = ss.KODI
--left outer  join dbo.tbsp_soato_new ss1 on substring(aa.SOATO_CODE, 1 ,2) + substring(aa.SOATO_CODE, 4 ,2) = ss1.KODI
--left outer  join dbo.tbsp_soato_new ss2 on substring(ss.KODI, 1 ,4)  = ss2.KODI) aa1
--inner join dbo.KaznaPaymentFile kk on kk.ID  = aa1.PAYMENT_ID 
--where aa1.OBLSOVPROF_AMT > 0 AND CONVERT(DATE,kk.[OPERDAY]) 
--BETWEEN CONVERT(DATE,@BEGIN_DATE)  AND CONVERT(DATE,@END_DATE)
--group by aa1.OBLAST_NAME , aa1.TUMAN_NAME) JJ1
) AAA
order by AAA.order_tp

;
  
  
  
end

--------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_Postup_OblastGroup]    Script Date: 04/10/2018 18:41:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_rt_Razbivka_Postup_OblastGroup]
@BEGIN_DATE Datetime,
@END_DATE Datetime,
@DEPARTMENT_BASE nvarchar(255)

as 
begin 
set nocount on;

/****** Script for SelectTopNRows command from SSMS  ******/

SELECT TT4.OblastName , TT4.Kolichestvo, TT4.TOTALSUM , TT4.OBLASTSUM FROM 
(
SELECT	CASE WHEN (GROUPING(AA.OBLAST_NAME) = 1) THEN 'Итого'
            ELSE ISNULL(AA.OBLAST_NAME, 'Неопределен')
       END AS OblastName,
        COUNT(AA.[FEDERATION_AMT]) AS Kolichestvo,
        ROUND(SUM(AA.[FEDERATION_AMT]),2)   AS TOTALSUM,
        ROUND(SUM(AA.[OBLSOVPROF_AMT]),2)   AS OBLASTSUM,
        '1' AS ORDERR
FROM 
(
select a.[ID]
      ,a.[PAYMENT_ID]
      ,a.[PROFSOUZ_AMT]
      ,a.[FEDERATION_AMT] AS FEDERATION_AMT
      ,a.[OBLSOVPROF_AMT] AS OBLSOVPROF_AMT
      ,a.[OBL_DEPARTMENT_BASE]
      ,a.[PROVODKA_ID]
      ,a.[REMARK] 
      ,ISNULL(b.ProfsoyzNaimenovanie , 'Не определен:') as OBLAST_NAME
from dbo.tbbu_ecp_calc a
inner join dbo.KaznaPaymentFile c 
on a.PAYMENT_ID  = c.ID
left outer join dbo.tbku_StrokaProfsoyza b 
on a.OBL_DEPARTMENT_BASE = b.MATCH_DEPARTMENTBASE
WHERE CONVERT(DATE,c.[OPERDAY]) 
BETWEEN CONVERT(DATE,@BEGIN_DATE)  AND CONVERT(DATE,@END_DATE)
AND [OBL_DEPARTMENT_BASE] LIKE @DEPARTMENT_BASE + '%') AA
GROUP BY AA.[OBL_DEPARTMENT_BASE], AA.OBLAST_NAME WITH ROLLUP

--UNION 

--SELECT	'Итого:' AS OblastName,
--        COUNT(AA.[FEDERATION_AMT]) AS Kolichestvo,
--        ROUND(SUM(AA.[FEDERATION_AMT]),2)   AS TOTALSUM,
--        ROUND(SUM(AA.[OBLSOVPROF_AMT]),2)   AS OBLASTSUM,
--        '2' AS ORDERR
--FROM 
--(
--select a.[ID]
--      ,a.[PAYMENT_ID]
--      ,a.[PROFSOUZ_AMT]
--      ,a.[FEDERATION_AMT] AS FEDERATION_AMT
--      ,a.[OBLSOVPROF_AMT] AS OBLSOVPROF_AMT
--      ,a.[OBL_DEPARTMENT_BASE]
--      ,a.[PROVODKA_ID]
--      ,a.[REMARK] 
--      ,ISNULL(b.ProfsoyzNaimenovanie , 'Не определен:') as OBLAST_NAME
--from dbo.tbbu_ecp_calc a
--inner join dbo.KaznaPaymentFile c 
--on a.PAYMENT_ID  = c.ID
--left outer join dbo.tbku_StrokaProfsoyza b 
--on a.OBL_DEPARTMENT_BASE = b.MATCH_DEPARTMENTBASE
--WHERE CONVERT(DATE,c.[OPERDAY]) 
--BETWEEN CONVERT(DATE,@BEGIN_DATE)  AND CONVERT(DATE,@END_DATE)) AA
) TT4
ORDER BY TT4.ORDERR
--SELECT TT4.OblastName , TT4.Kolichestvo, TT4.TOTALSUM , TT4.OBLASTSUM
--FROM 
--(
-- SELECT TT2.OblastName 
--, COUNT(TT2.[SUMPAY]/100) As Kolichestvo 
--, ROUND(SUM(TT2.[FEDERATIONSUM])/100,2)   AS TOTALSUM
--, ROUND(SUM(TT2.[OBLASTSUM])/100,2) As OBLASTSUM
--, '1' AS ORDERR
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
--      ,ABS(a.SUMPAY) AS SUMPAY
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
--       ,case when a.PURPOSE LIKE '%3212110%' THEN ABS(a.SUMPAY) * 0.4 /100
--            when  a.PURPOSE LIKE '%3212120%' THEN ABS(a.SUMPAY) * 0.666666667 /100 
--            when  a.PURPOSE LIKE '%3212130%' THEN ABS(a.SUMPAY) * 0.666666667 /100
--            ELSE ABS(a.SUMPAY) * 0.4 /100  END AS SUMMATOTAL
--  FROM [KasabaKIAT_N1].[dbo].[KaznaPaymentFile] a) TT1
--  LEFT OUTER JOIN (select EE1.INN , MAX(EE1.SOATO_ID) AS SOATO ,  MAX(EE1.OKONX_ID) AS OKONX
--					from dbo.t_egrpo EE1
--					GROUP BY EE1.INN ) EE on EE.INN = TT1.[CLINN]
--  LEFT OUTER JOIN dbo.sp_soato ss on ss.code = SUBSTRING(EE.SOATO , 1 , 4)
--  LEFT OUTER JOIN dbo.sp_okonx oo on oo.code = EE.OKONX
--  LEFT OUTER JOIN dbo.sp_branch bb  on bb.code = oo.branch_code
--  WHERE CONVERT(DATE,TT1.[OPERDAY]) BETWEEN CONVERT(DATE,@BEGIN_DATE)  AND CONVERT(DATE,@END_DATE)
--  ) TT2
--  GROUP BY TT2.OblastName
  
--  UNION
  
--  SELECT 'Obshee', SUM(TT3.Kolichestvo) as Kolichestvo,
--        SUM(TT3.TOTALSUM) AS TOTALSUM, SUM(TT3.OBLASTSUM) AS OBLASTSUM, '2' AS ORDERR
--  FROM 
--  (SELECT TT2.OblastName 
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
--       ,case when a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212110%' THEN ABS(a.SUMPAY) * 0.4 /100
--            when  a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212120%' THEN ABS(a.SUMPAY) * 0.666666667 /100 
--            when  a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212130%' THEN ABS(a.SUMPAY) * 0.666666667 /100
--            ELSE ABS(a.SUMPAY) * 0.4 /100  END AS SUMMATOTAL
--  FROM [KasabaKIAT_N1].[dbo].[KaznaPaymentFile] a) TT1
--  LEFT OUTER JOIN (select EE1.INN , MAX(EE1.SOATO_ID) AS SOATO ,  MAX(EE1.OKONX_ID) AS OKONX
--					from dbo.t_egrpo EE1
--					GROUP BY EE1.INN ) EE on EE.INN = TT1.[CLINN]
--  LEFT OUTER JOIN dbo.sp_soato ss on ss.code = SUBSTRING(EE.SOATO , 1 , 4)
--  LEFT OUTER JOIN dbo.sp_okonx oo on oo.code = EE.OKONX
--  LEFT OUTER JOIN dbo.sp_branch bb  on bb.code = oo.branch_code
--  WHERE CONVERT(DATE,TT1.[OPERDAY]) BETWEEN CONVERT(DATE,@BEGIN_DATE)  AND CONVERT(DATE,@END_DATE)
--  ) TT2
--  GROUP BY TT2.OblastName) TT3) TT4
--  ORDER BY TT4.ORDERR;
  
  
  
end

---------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_Postup]    Script Date: 04/10/2018 18:41:12 ******/
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
      , ISNULL(b.ProfsoyzNaimenovanie , 'NE OPREDELEN') + '  '  +  a.REMARK  As OblastName
      , CASE WHEN oo.name_ru  IS NULL THEN 'Ne Opredelen' ELSE oo.name_ru END   as OkonxName
      , CASE WHEN bb.name_ru  IS NULL THEN 'Ne Opredelen' ELSE bb.name_ru END  as otraslName
    from  dbo.tbbu_ecp_calc a
          inner join dbo.KaznaPaymentFile c 
          on a.PAYMENT_ID  = c.ID
          left outer join dbo.tbku_StrokaProfsoyza b 
          on a.OBL_DEPARTMENT_BASE = b.MATCH_DEPARTMENTBASE 
      LEFT OUTER JOIN (select EE1.INN , MAX(EE1.SOATO_ID) AS SOATO ,  MAX(EE1.OKONX_ID) AS OKONX
					from dbo.t_egrpo EE1
					GROUP BY EE1.INN ) EE on EE.INN = SUBSTRING(a.REMARK , 5, 9)
  LEFT OUTER JOIN dbo.sp_okonx oo on oo.code = EE.OKONX
  LEFT OUTER JOIN dbo.sp_branch bb  on bb.code = oo.branch_code
    WHERE SUBSTRING(ISNULL(EE.SOATO , '00000') , 1 , 4) = CASE WHEN @SOATO = '0' THEN SUBSTRING(ISNULL(EE.SOATO , '00000') , 1 , 4) ELSE @SOATO END 
  AND ISNULL(bb.code , '00') = CASE WHEN @BRANCH = '0' THEN ISNULL(bb.code , '00') ELSE @BRANCH END
  AND  CONVERT(DATE,c.[OPERDAY]) BETWEEN CONVERT(DATE,@BEGIN_DATE)  AND CONVERT(DATE,@END_DATE)
  AND  [OBL_DEPARTMENT_BASE] LIKE @DEPARTMENT_BASE + '%';
 --SELECT TOP 2500 [K_TYPE]
 --     ,[OPERDAY]
 --     ,TT1.[ID]
 --     ,[BANKID]
 --     ,[KLSDOX]
 --     ,[DOCNUMB]
 --     ,[DOCDATE]
 --     ,[DOCTYPE]
 --     ,ROUND([SUMPAY]/100,2) AS SUMPAY
 --     ,[CLMFO]
 --     ,[CLACC]
 --     ,[CLINN]
 --     ,[CLNAME]
 --     ,[COMFO]
 --     ,[COACC]
 --     ,[COINN]
 --     ,[CONAME]
 --     ,[PURPOSE]
 --     ,[CHAPTER]
 --     ,[OKONH]
 --     ,[FILE_ID]
 --     ,[OTCHISLEN]
 --     , ROUND(SUMMATOTAL/100,2) AS SUMMATOTAL
 --     , ROUND(SUMMATOTAL * 0.3/100,2) AS FEDERATIONSUM
 --     , ROUND(SUMMATOTAL * 0.7/100,2) AS OBLASTSUM
 --     , CASE WHEN ss.[name_ru] IS NULL THEN aa.VILOYAT + ' ' + aa.TUMAN_SHAXAR ELSE ss.[name_ru] END  As OblastName
 --     , CASE WHEN oo.name_ru  IS NULL THEN 'Ne Opredelen' ELSE oo.name_ru END   as OkonxName
 --     , CASE WHEN bb.name_ru  IS NULL THEN 'Ne Opredelen' ELSE bb.name_ru END  as otraslName
 --FROM 
 -- (SELECT a.[K_TYPE]
 --     ,a.[OPERDAY]
 --     ,a.[ID]
 --     ,a.[BANKID]
 --     ,a.[KLSDOX]
 --     ,a.[DOCNUMB]
 --     ,a.[DOCDATE]
 --     ,a.[DOCTYPE]
 --     ,ABS(a.SUMPAY) AS SUMPAY
 --     ,a.[CLMFO]
 --     ,a.[CLACC]
 --     ,a.[CLINN]
 --     ,a.[CLNAME]
 --     ,a.[COMFO]
 --     ,a.[COACC]
 --     ,a.[COINN]
 --     ,a.[CONAME]
 --     ,a.[PURPOSE]
 --     ,a.[CHAPTER]
 --     ,a.[OKONH]
 --     ,a.[FILE_ID]
 --     ,a.[OTCHISLEN]
 --      ,case when a.PURPOSE LIKE '%3212110%' THEN ABS(a.SUMPAY) * 0.4 /100
 --           when  a.PURPOSE LIKE '%3212120%' THEN ABS(a.SUMPAY) * 0.666666667 /100 
 --           when  a.PURPOSE LIKE '%3212130%' THEN ABS(a.SUMPAY) * 0.666666667 /100
 --           ELSE ABS(a.SUMPAY) * 0.4 /100  END AS SUMMATOTAL
 -- FROM [KasabaKIAT_N1].[dbo].[KaznaPaymentFile] a) TT1
 -- LEFT OUTER JOIN (select EE1.INN , MAX(EE1.SOATO_ID) AS SOATO ,  MAX(EE1.OKONX_ID) AS OKONX
	--				from dbo.t_egrpo EE1
	--				GROUP BY EE1.INN ) EE on EE.INN = TT1.[CLINN]
 -- LEFT OUTER JOIN dbo.sp_soato ss on ss.code = SUBSTRING(EE.SOATO , 1 , 4)
 -- LEFT OUTER JOIN dbo.sp_okonx oo on oo.code = EE.OKONX
 -- LEFT OUTER JOIN dbo.sp_branch bb  on bb.code = oo.branch_code
 -- LEFT OUTER JOIN dbo.Kazna_Account aa on aa.XISOB_RAQAM = SUBSTRING(TT1.[PURPOSE] , 7 , 25)
 -- WHERE SUBSTRING(ISNULL(EE.SOATO , '00000') , 1 , 4) = CASE WHEN @SOATO = '0' THEN SUBSTRING(ISNULL(EE.SOATO , '00000') , 1 , 4) ELSE @SOATO END 
 -- AND ISNULL(bb.code , '00') = CASE WHEN @BRANCH = '0' THEN ISNULL(bb.code , '00') ELSE @BRANCH END
 -- AND  CONVERT(DATE,TT1.[OPERDAY]) BETWEEN CONVERT(DATE,@BEGIN_DATE)  AND CONVERT(DATE,@END_DATE);
  
end
------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Postupleniya]    Script Date: 04/10/2018 19:17:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_rt_Postupleniya]
@INN  nvarchar(9),
@DEPARTMENT_BASE nvarchar(255)

as 
begin 
set nocount on;

SELECT TOP 1000 [K_TYPE]
      ,[OPERDAY]
      ,k1.[ID]
      ,[BANKID]
      ,[KLSDOX]
      ,[DOCNUMB]
      ,[DOCDATE]
      ,[DOCTYPE]
      ,ROUND(ABS([SUMPAY])/100,2) AS SUMPAY
      ,[CLMFO]
      ,[CLACC]
      ,[CLINN]
      ,[CLNAME]
      ,[COMFO]
      ,[COACC]
      ,[COINN]
      ,[CONAME]
      ,[PURPOSE]
      ,[CHAPTER]
      ,[OKONH]
      ,[FILE_ID]
      ,[OTCHISLEN]
      , k2.FileName
      , k2.CreateDate
  FROM [KasabaKIAT_N1].[dbo].[KaznaPaymentFile] k1
  INNER JOIN dbo.KaznaFiles k2 on k1.FILE_ID = k2.ID
  INNER JOIN dbo.tbbu_ecp_calc cc on k1.ID = cc.PAYMENT_ID
  WHERE CLINN = @INN
  AND [OBL_DEPARTMENT_BASE] LIKE @DEPARTMENT_BASE + '%';
  
end



