USE [KasabaKIAT]
GO
/****** Object:  StoredProcedure [dbo].[sp_im_KaznaNewOrg]    Script Date: 03/13/2018 15:03:39 ******/
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
  
end
-----------------------------------------------------

USE [KasabaKIAT]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_Postup]    Script Date: 03/13/2018 16:03:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_rt_Razbivka_Postup]
@SOATO varchar(4),
@BRANCH  varchar(10),
@BEGIN_DATE Datetime,
@END_DATE Datetime

as 
begin 
set nocount on;

/****** Script for SelectTopNRows command from SSMS  ******/

 SELECT [K_TYPE]
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
            when  a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212120%' THEN a.SUMPAY * 0.6666 /100 
            when  a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212130%' THEN a.SUMPAY * 0.6666 /100
            ELSE SUMPAY * 0.4 /100  END AS SUMMATOTAL
  FROM [KasabaKIAT].[dbo].[KaznaPaymentFile] a) TT1
  LEFT OUTER JOIN (select EE1.INN , MAX(EE1.SOATO_ID) AS SOATO ,  MAX(EE1.OKONX_ID) AS OKONX
					from dbo.t_egrpo EE1
					GROUP BY EE1.INN ) EE on EE.INN = TT1.[CLINN]
  LEFT OUTER JOIN dbo.sp_soato ss on ss.code = SUBSTRING(EE.SOATO , 1 , 4)
  LEFT OUTER JOIN dbo.sp_okonx oo on oo.code = EE.OKONX
  LEFT OUTER JOIN dbo.sp_branch bb  on bb.code = oo.branch_code
  WHERE TT1.[SUMPAY] > 0 AND TT1.[CLINN] != TT1.[COINN] AND TT1.[CLINN] != '000000000'
  AND SUBSTRING(ISNULL(EE.SOATO , '00000') , 1 , 4) = CASE WHEN @SOATO = '0' THEN SUBSTRING(ISNULL(EE.SOATO , '00000') , 1 , 4) ELSE @SOATO END 
  AND ISNULL(bb.code , '00') = CASE WHEN @BRANCH = '0' THEN ISNULL(bb.code , '00') ELSE @BRANCH END
  AND  TT1.[OPERDAY] BETWEEN @BEGIN_DATE AND @END_DATE;
  
end

----------------------------------------

USE [KasabaKIAT]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_Postup]    Script Date: 03/13/2018 17:30:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_Razbivka_Postup_OblastGroup]
@BEGIN_DATE Datetime,
@END_DATE Datetime

as 
begin 
set nocount on;

/****** Script for SelectTopNRows command from SSMS  ******/

 SELECT TT2.OblastName 
, COUNT(TT2.[SUMPAY]) As Kolichestvo 
, ROUND(SUM(TT2.[SUMPAY]),2)   AS TOTALSUM
, ROUND(SUM(TT2.[OBLASTSUM]),2) As OBLASTSUM
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
            when  a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212120%' THEN a.SUMPAY * 0.6666 /100 
            when  a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212130%' THEN a.SUMPAY * 0.6666 /100
            ELSE SUMPAY * 0.4 /100  END AS SUMMATOTAL
  FROM [KasabaKIAT].[dbo].[KaznaPaymentFile] a) TT1
  LEFT OUTER JOIN (select EE1.INN , MAX(EE1.SOATO_ID) AS SOATO ,  MAX(EE1.OKONX_ID) AS OKONX
					from dbo.t_egrpo EE1
					GROUP BY EE1.INN ) EE on EE.INN = TT1.[CLINN]
  LEFT OUTER JOIN dbo.sp_soato ss on ss.code = SUBSTRING(EE.SOATO , 1 , 4)
  LEFT OUTER JOIN dbo.sp_okonx oo on oo.code = EE.OKONX
  LEFT OUTER JOIN dbo.sp_branch bb  on bb.code = oo.branch_code
  WHERE TT1.[SUMPAY] > 0 AND TT1.[CLINN] != TT1.[COINN] AND TT1.[CLINN] != '000000000'
  AND  TT1.[OPERDAY] BETWEEN @BEGIN_DATE AND @END_DATE
  ) TT2
  GROUP BY TT2.OblastName;
  
end
-------------------------------

USE [KasabaKIAT]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_Postup]    Script Date: 03/13/2018 17:30:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_Razbivka_Postup_OtraslGroup]
@SOATO varchar(4),
@BEGIN_DATE Datetime,
@END_DATE Datetime

as 
begin 
set nocount on;

/****** Script for SelectTopNRows command from SSMS  ******/

SELECT TT2.OblastName, TT2.otraslName
, COUNT(TT2.[SUMPAY]) As Kolichestvo 
, ROUND(SUM(TT2.[SUMPAY]),2)   AS TOTALSUM
, ROUND(SUM(TT2.[OBLASTSUM]),2) As OBLASTSUM
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
            when  a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212120%' THEN a.SUMPAY * 0.6666 /100 
            when  a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212130%' THEN a.SUMPAY * 0.6666 /100
            ELSE SUMPAY * 0.4 /100  END AS SUMMATOTAL
  FROM [KasabaKIAT].[dbo].[KaznaPaymentFile] a) TT1
  LEFT OUTER JOIN (select EE1.INN , MAX(EE1.SOATO_ID) AS SOATO ,  MAX(EE1.OKONX_ID) AS OKONX
					from dbo.t_egrpo EE1
					GROUP BY EE1.INN ) EE on EE.INN = TT1.[CLINN]
  LEFT OUTER JOIN dbo.sp_soato ss on ss.code = SUBSTRING(EE.SOATO , 1 , 4)
  LEFT OUTER JOIN dbo.sp_okonx oo on oo.code = EE.OKONX
  LEFT OUTER JOIN dbo.sp_branch bb  on bb.code = oo.branch_code
  WHERE TT1.[SUMPAY] > 0 AND TT1.[CLINN] != TT1.[COINN] AND TT1.[CLINN] != '000000000'
  AND SUBSTRING(ISNULL(EE.SOATO , '00000') , 1 , 4) = CASE WHEN @SOATO = '0' THEN SUBSTRING(ISNULL(EE.SOATO , '00000') , 1 , 4) ELSE @SOATO END 
  AND  TT1.[OPERDAY] BETWEEN @BEGIN_DATE AND @END_DATE
  ) TT2
  GROUP BY TT2.OblastName, TT2.otraslName;
  
end

-------------------------------

USE [KasabaKIAT]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_Postup_OtraslGroup]    Script Date: 03/13/2018 17:36:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_Razbivka_Postup_OkonxGroup]
@SOATO varchar(4),
@BEGIN_DATE Datetime,
@END_DATE Datetime

as 
begin 
set nocount on;

/****** Script for SelectTopNRows command from SSMS  ******/

SELECT TT2.OblastName,TT2.OkonxName 
, COUNT(TT2.[SUMPAY]) As Kolichestvo 
, ROUND(SUM(TT2.[SUMPAY]),2)   AS TOTALSUM
, ROUND(SUM(TT2.[OBLASTSUM]),2) As OBLASTSUM
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
            when  a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212120%' THEN a.SUMPAY * 0.6666 /100 
            when  a.SUMPAY > 0 AND a.PURPOSE LIKE '%3212130%' THEN a.SUMPAY * 0.6666 /100
            ELSE SUMPAY * 0.4 /100  END AS SUMMATOTAL
  FROM [KasabaKIAT].[dbo].[KaznaPaymentFile] a) TT1
  LEFT OUTER JOIN (select EE1.INN , MAX(EE1.SOATO_ID) AS SOATO ,  MAX(EE1.OKONX_ID) AS OKONX
					from dbo.t_egrpo EE1
					GROUP BY EE1.INN ) EE on EE.INN = TT1.[CLINN]
  LEFT OUTER JOIN dbo.sp_soato ss on ss.code = SUBSTRING(EE.SOATO , 1 , 4)
  LEFT OUTER JOIN dbo.sp_okonx oo on oo.code = EE.OKONX
  LEFT OUTER JOIN dbo.sp_branch bb  on bb.code = oo.branch_code
  WHERE TT1.[SUMPAY] > 0 AND TT1.[CLINN] != TT1.[COINN] AND TT1.[CLINN] != '000000000'
  AND SUBSTRING(ISNULL(EE.SOATO , '00000') , 1 , 4) = CASE WHEN @SOATO = '0' THEN SUBSTRING(ISNULL(EE.SOATO , '00000') , 1 , 4) ELSE @SOATO END 
  AND  TT1.[OPERDAY] BETWEEN @BEGIN_DATE AND @END_DATE
  ) TT2
  GROUP BY TT2.OblastName,TT2.OkonxName;
  
end
