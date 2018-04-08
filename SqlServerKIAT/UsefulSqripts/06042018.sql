USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_Postup_OkonxGroup]    Script Date: 04/06/2018 15:55:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_rt_Razbivka_Postup_OkonxGroup]
@SOATO nvarchar(4),
@BEGIN_DATE Datetime,
@END_DATE Datetime

as 
begin 
set nocount on;

/****** Script for SelectTopNRows command from SSMS  ******/

SELECT TT2.OblastName,TT2.OkonxName 
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
  FROM [KasabaKIAT_N1].[dbo].[KaznaPaymentFile] a) TT1
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
  GROUP BY TT2.OblastName,TT2.OkonxName;
  
end
---------------------------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_Postup_OtraslGroup]    Script Date: 04/06/2018 15:55:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_rt_Razbivka_Postup_OtraslGroup]
@SOATO nvarchar(4),
@BEGIN_DATE Datetime,
@END_DATE Datetime

as 
begin 
set nocount on;

/****** Script for SelectTopNRows command from SSMS  ******/

SELECT TT2.OblastName, TT2.otraslName
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
  FROM [KasabaKIAT_N1].[dbo].[KaznaPaymentFile] a) TT1
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
  GROUP BY TT2.OblastName, TT2.otraslName;
  
end



---------------------------



  update [KasabaKIAT_N1].[dbo].[tbku_StrokaProfsoyza]
  set MATCH_DEPARTMENTBASE = '173О1'
  where profsoyzKod = '000000057';


  update dbo.users 
set role_id = 13
where user_name = 'olmaliq.komb@kasaba.uz';



----------------------------------------------


insert into [KasabaKIAT_N1].[dbo].[users]
SELECT [version]
      ,[account_expired]
      ,[account_locked]
      ,[changed]
      ,[created]
      ,'navoiy.komb@kasaba.uz'
      ,[last_logged]
      ,[last_logged_id]
      ,[state]
      ,hashbytes('md5', 'navoiy.komb@kasaba.uz')
      ,[salt]
      ,[language]
      ,[is_super_admin]
      ,'navoiy.komb@kasaba.uz'
      ,[s_comment]
      ,[s_create]
      ,[s_status]
      ,[s_user_id]
      ,[usertype]
      ,[bkut_id]
      ,[dep_id]
      ,[role_id]
      ,[bkuq_id]
      ,[REGION_ID]
      ,[BRANCH_ID]
      ,null
  FROM [KasabaKIAT_N1].[dbo].[users]
  where user_name = 'olmaliq.komb@kasaba.uz';



  insert into [KasabaKIAT_N1].[dbo].[sp_branch]
SELECT [version]
      ,'Н2'
      ,[s_comment]
      ,GETDATE()
      ,[s_status]
      ,N'“Навоий кон-металлургия комбинати” очиқ акционерлик жамияти ходимлари касаба уюшмаси'
      ,N'“Навоий кон-металлургия комбинати” очиқ акционерлик жамияти ходимлари касаба уюшмаси'
      ,N'“Навоий кон-металлургия комбинати” очиқ акционерлик жамияти ходимлари касаба уюшмаси'
      ,[s_user_id]
      ,[NATIVE_ID]
  FROM [KasabaKIAT_N1].[dbo].[sp_branch]
  where id = 14;



  /****** Script for SelectTopNRows command from SSMS  ******/
insert into [KasabaKIAT_N1].[dbo].[t_departmentbase]
SELECT [dtype]
      ,[version]
      ,[keys]
      ,[childseq]
      ,'173Н2'
      ,[s_comment]
      ,[s_create]
      ,[s_status]
      ,[isleaf]
      ,[s_level]
      ,'Совет профсоюза АО «Навоинский горно-металлургический комбинат»'
      ,'«Навоий кон-металлургия комбинати» АЖ касаба уюшмаси кенгаши'
      ,'«Навоий кон-металлургия комбинати» АЖ касаба уюшмаси кенгаши'
      ,[address]
      ,[alohida_xona_bormi]
      ,[computer_bormi]
      ,[count_of_members]
      ,[davlat_ruyxatidan_otganmi]
      ,[email]
      ,[inn]
      ,[internet_bormi]
      ,[jamoa_shartnoma_bormi]
      ,[kasaba_uyushmasi_xaq_tulanadi]
      ,[bkut_name]
      ,[phone]
      ,[rais_kollegia_azomi]
      ,[rais_ozodmi]
      ,[spec_number]
      ,[type]
      ,[xisob_raqam]
      ,[yuqori_shartnomadan_foydalanadimi]
      ,[parent_code]
      ,'Н2'
      ,[deptype_code]
      ,[s_user_id]
      ,[soato_code]
      ,[employee_id]
      ,[okonx]
      ,[camps_address]
      ,[camps_type1]
      ,[camps_type2]
      ,[camps_organization_address]
      ,[camps_organization_cheef]
      ,[camps_organization_cheef_tel]
      ,[camps_organization_name]
      ,[camps_union]
      ,[camps_union_tel]
      ,[camps_cheef]
      ,[camps_cheef_tel]
      ,[camps_email]
      ,[camps_name]
      ,[camps_site]
      ,[camps_tel]
      ,[soato_organization_id]
      ,[department_code]
      ,[sanatorium_name]
      ,[site]
      ,[telephone]
      ,[sanatorium_profil_id]
      ,[sanatorium_type_id]
      ,[employeefullname]
      ,[dep_name]
      ,[le_inn]
      ,[group_2ti]
      ,null
  FROM [KasabaKIAT_N1].[dbo].[t_departmentbase]
  where native_id = '16675';


  update [KasabaKIAT_N1].dbo.users 
set role_id = 13, 
dep_id = 19628
where user_name = 'navoiy.komb@kasaba.uz';
  

  update [KasabaKIAT_N1].[dbo].[tbku_StrokaProfsoyza]
  set MATCH_DEPARTMENTBASE = '173Н2'
  where profsoyzKod = '000000055';

  --temiryol@kasaba.uz
  update [KasabaKIAT_N1].dbo.[users]
  set dep_id = 2643,
  role_id = 13
  where dep_id = 699;


      update [KasabaKIAT_N1].[dbo].[tbku_StrokaProfsoyza]
  set MATCH_DEPARTMENTBASE = '173Т2'
  where profsoyzKod = '000000056';


  --temiryol@kasaba.uz
  --navoiy.komb@kasaba.uz
  --olmaliq.komb@kasaba.uz
  -----------------------------------------------


USE [KasabaKIAT_N1]
GO
/** Object:  StoredProcedure [dbo].[uspLoginChange]    Script Date: 04/06/2018 17:45:02 **/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[uspLoginChange] 
@user_id bigint,
@parol nvarchar(50)

as 
begin 
set nocount on;

update [KasabaKIAT_N1].[dbo].[users]
set  [password] = hashbytes('md5', [user_name])
where id = @user_id;

end