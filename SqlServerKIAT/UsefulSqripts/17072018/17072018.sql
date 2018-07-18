UPDATE KasabaKIAT_N1.dbo.users 
SET dep_id = 3521
WHERE user_name = 'olmaliq.komb@kasaba.uz';


INSERT INTO [KasabaKIAT_N1].[dbo].[tblSprav_Otras]
SELECT [ID_Ot]
      ,[Name_Ot]
      ,[Priz]
      ,[UserId]
      ,'173О1'
      ,[BRANCH_CODE]
  FROM [KasabaKIAT_N1].[dbo].[tblSprav_Otras]
  WHERE DepartmentBase = '17226';


  /****** Script for SelectTopNRows command from SSMS  ******/
INSERT INTO [KasabaKIAT_N1].[dbo].tblSprav_Org
SELECT 11 as [ID_Ot]
      ,org.name_full as [Name_Org]
      ,[INN]
      ,null as [RS]
      ,null as [MFO]
      ,null as [Bank]
      ,null as [OKONX]
      ,null as [Adress]
      ,null as [Telefon]
      ,null as [Region]
      ,null as [DT_OST]
      ,null as [KT_OST]
      ,null as [E_Mail]
      ,1 as [UserId]
      ,'173О1' as [DepartmentBase]
      ,1 as [IS_PRAVSOYUZ]
      ,null as [ParentID]
      ,null as [PRAVORGLEVEL]
      ,null as [DIRECTOR]
      ,null as [BUXGALTER]
      ,null as [DOVERENNOE_LISO]
      ,null as [ORIG_INN]
      ,null as [OWNER_CODE]
      ,GETDATE() as [TIMESTAMP_CH]
  FROM KasabaKIAT_N1.dbo.t_egrpo org
where org.INN IN 
(SELECT [NAME_RU]
  FROM [KasabaKIAT_N1].[dbo].[tbcs_dictionary]
  WHERE [CODE] = '173О1')



  --173Т2

  INSERT INTO [KasabaKIAT_N1].[dbo].[tblSprav_Otras]
SELECT [ID_Ot]
      ,[Name_Ot]
      ,[Priz]
      ,[UserId]
      ,'173Т2'
      ,[BRANCH_CODE]
  FROM [KasabaKIAT_N1].[dbo].[tblSprav_Otras]
  WHERE DepartmentBase = '17226';


  /****** Script for SelectTopNRows command from SSMS  ******/
INSERT INTO [KasabaKIAT_N1].[dbo].tblSprav_Org
SELECT 11 as [ID_Ot]
      ,org.name_full as [Name_Org]
      ,[INN]
      ,null as [RS]
      ,null as [MFO]
      ,null as [Bank]
      ,null as [OKONX]
      ,null as [Adress]
      ,null as [Telefon]
      ,null as [Region]
      ,null as [DT_OST]
      ,null as [KT_OST]
      ,null as [E_Mail]
      ,1 as [UserId]
      ,'173Т2' as [DepartmentBase]
      ,1 as [IS_PRAVSOYUZ]
      ,null as [ParentID]
      ,null as [PRAVORGLEVEL]
      ,null as [DIRECTOR]
      ,null as [BUXGALTER]
      ,null as [DOVERENNOE_LISO]
      ,null as [ORIG_INN]
      ,null as [OWNER_CODE]
      ,GETDATE() as [TIMESTAMP_CH]
  FROM KasabaKIAT_N1.dbo.t_egrpo org
where org.INN IN 
(SELECT [NAME_RU]
  FROM [KasabaKIAT_N1].[dbo].[tbcs_dictionary]
  WHERE [CODE] = '173Т2');


----***************-*-*****************************************************

  INSERT INTO [KasabaKIAT_N1].[dbo].[tblSprav_Otras]
SELECT [ID_Ot]
      ,[Name_Ot]
      ,[Priz]
      ,[UserId]
      ,N'173Н2'
      ,[BRANCH_CODE]
  FROM [KasabaKIAT_N1].[dbo].[tblSprav_Otras]
  WHERE DepartmentBase = '17226';



  USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_PostupleniyaN]    Script Date: 07/17/2018 22:10:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_rt_PostupleniyaN]
@INN  nvarchar(9),
@DEPARTMENT_BASE nvarchar(255)

as 
begin 
set nocount on;


IF (@DEPARTMENT_BASE = N'173О1' OR @DEPARTMENT_BASE = N'173Т2' OR @DEPARTMENT_BASE = N'173Н2')

SELECT AAA.OPERDAY,
	   AAA.CLINN as INN,
	   AAA.CLNAME,
	   AAA.POSTUPLENIYA,
	   AAA.Obshiy01,
	   AAA.FEDERATION_AMT,
	   AAA.OBLSOVPROF_AMT,
	   AAA.PURPOSE	
FROM
(select 
	'Всего: ' as OPERDAY,
	pp.CLINN ,
	MAX(pp.CLNAME) as CLNAME, 
	SUM(ROUND(pp.SUMPAY/100,2)) as POSTUPLENIYA, 
	SUM(ecp.FEDERATION_AMT + ecp.OBLSOVPROF_AMT) as Obshiy01 ,  
	SUM(ecp.FEDERATION_AMT) as FEDERATION_AMT , 
	SUM(ecp.OBLSOVPROF_AMT) as OBLSOVPROF_AMT ,
	'' as PURPOSE,
	GETDATE() as OPERDAY11
from 
KasabaKIAT_N1.dbo.KaznaBankFile bb
	inner join KasabaKIAT_N1.dbo.tblPred pr on bb.COACC = pr.KaznaAccount
	left outer join KasabaKIAT_N1.dbo.KaznaPaymentFile pp on bb.GRPID = pp.GRPID
	left outer join KasabaKIAT_N1.dbo.tbbu_ecp_calc ecp on ecp.PAYMENT_ID = pp.ID
	left outer join KasabaKIAT_N1.dbo.tbcs_dictionary dic on dic.NAME_RU =ecp.INN 
	where dic.CODE = @DEPARTMENT_BASE
	--ecp.INN = @INN
	AND pp.CLINN = @INN
	Group by pp.CLINN --,pp.CLNAME

UNION

select 
	REPLACE(CONVERT(NVARCHAR, bb.OPERDAY, 103), '/', '.') as  OPERDAY, 
	pp.CLINN , 
	pp.CLNAME , 
	ROUND(pp.SUMPAY/100,2) as POSTUPLENIYA, 
	ecp.FEDERATION_AMT + ecp.OBLSOVPROF_AMT as Obshiy01 ,  
	ecp.FEDERATION_AMT , 
	ecp.OBLSOVPROF_AMT , 
	pp.PURPOSE ,
	bb.OPERDAY as OPERDAY11
from 
	KasabaKIAT_N1.dbo.KaznaBankFile bb
	inner join KasabaKIAT_N1.dbo.tblPred pr on bb.COACC = pr.KaznaAccount
	left outer join KasabaKIAT_N1.dbo.KaznaPaymentFile pp on bb.GRPID = pp.GRPID
	left outer join KasabaKIAT_N1.dbo.tbbu_ecp_calc ecp on ecp.PAYMENT_ID = pp.ID
	left outer join KasabaKIAT_N1.dbo.tbcs_dictionary dic on dic.NAME_RU =ecp.INN 
where dic.CODE = @DEPARTMENT_BASE
    --AND ecp.INN = @INN
     AND	pp.CLINN = @INN
    ) AAA
 ORDER BY    AAA.OPERDAY11 DESC
 ;
 
 ELSE ---------------------------------------------------------------------------------------------
	SELECT AAA.OPERDAY,
	   AAA.CLINN as INN,
	   AAA.CLNAME,
	   AAA.POSTUPLENIYA,
	   AAA.Obshiy01,
	   AAA.FEDERATION_AMT,
	   AAA.OBLSOVPROF_AMT,
	   AAA.PURPOSE	
FROM
(select 
	'Всего: ' as OPERDAY,
	pp.CLINN ,
	MAX(pp.CLNAME) as CLNAME, 
	SUM(ROUND(pp.SUMPAY/100,2)) as POSTUPLENIYA, 
	SUM(ecp.FEDERATION_AMT + ecp.OBLSOVPROF_AMT) as Obshiy01 ,  
	SUM(ecp.FEDERATION_AMT) as FEDERATION_AMT , 
	SUM(ecp.OBLSOVPROF_AMT) as OBLSOVPROF_AMT ,
	'' as PURPOSE,
	GETDATE() as OPERDAY11
from 
KasabaKIAT_N1.dbo.KaznaBankFile bb
	inner join KasabaKIAT_N1.dbo.tblPred pr on bb.COACC = pr.KaznaAccount
	left outer join KasabaKIAT_N1.dbo.KaznaPaymentFile pp on bb.GRPID = pp.GRPID
	left outer join KasabaKIAT_N1.dbo.tbbu_ecp_calc ecp on ecp.PAYMENT_ID = pp.ID
	where pr.Departmentbase LIKE @DEPARTMENT_BASE + '%' AND 
	--ecp.INN = @INN
	pp.CLINN = @INN
	Group by pp.CLINN --,pp.CLNAME

UNION

select 
	REPLACE(CONVERT(NVARCHAR, bb.OPERDAY, 103), '/', '.') as  OPERDAY, 
	pp.CLINN , 
	pp.CLNAME , 
	ROUND(pp.SUMPAY/100,2) as POSTUPLENIYA, 
	ecp.FEDERATION_AMT + ecp.OBLSOVPROF_AMT as Obshiy01 ,  
	ecp.FEDERATION_AMT , 
	ecp.OBLSOVPROF_AMT , 
	pp.PURPOSE ,
	bb.OPERDAY as OPERDAY11
from 
	KasabaKIAT_N1.dbo.KaznaBankFile bb
	inner join KasabaKIAT_N1.dbo.tblPred pr on bb.COACC = pr.KaznaAccount
	left outer join KasabaKIAT_N1.dbo.KaznaPaymentFile pp on bb.GRPID = pp.GRPID
	left outer join KasabaKIAT_N1.dbo.tbbu_ecp_calc ecp on ecp.PAYMENT_ID = pp.ID
where pr.Departmentbase LIKE @DEPARTMENT_BASE + '%'
    --AND ecp.INN = @INN
     AND	pp.CLINN = @INN
    ) AAA
 ORDER BY    AAA.OPERDAY11 DESC
 ;
  
end

-----------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_PostupN]    Script Date: 07/17/2018 22:14:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_rt_Razbivka_PostupN]
@PAY_TYPE  varchar(3),
@DEPARTMENT_BASE nvarchar(255),
@BEGIN_DATE Datetime

as 
begin 
set nocount on;

IF (@DEPARTMENT_BASE = N'173О1' OR @DEPARTMENT_BASE = N'173Т2' OR @DEPARTMENT_BASE = N'173Н2')
SELECT 
	aa.TUMAN_SHAXAR  , 
	REPLACE(CONVERT(NVARCHAR, bb.OPERDAY, 103), '/', '.') as  OPERDAY , 
	pp.CLACC , 
	ecp.INN, 
	pp.CLMFO, 
	pp.CLNAME , 
	ROUND(pp.SUMPAY/100,2) as POSTUPLENIYA, 
	ecp.FEDERATION_AMT + ecp.OBLSOVPROF_AMT as Obshiy01 ,  
	ecp.FEDERATION_AMT , 
	ecp.OBLSOVPROF_AMT ,  
	pp.PURPOSE,
	ecp.OKONX_NAME,
	ecp.BRANCH_NAME 
FROM KasabaKIAT_N1.dbo.KaznaBankFile bb
	inner join KasabaKIAT_N1.dbo.tblPred pr 
		on bb.COACC = pr.KaznaAccount
	left outer join KasabaKIAT_N1.dbo.Kazna_Account aa
		on bb.KLSDOX = aa.XISOB_RAQAM
	left outer join KasabaKIAT_N1.dbo.KaznaPaymentFile pp
		on bb.GRPID = pp.GRPID
	left outer join KasabaKIAT_N1.dbo.tbbu_ecp_calc ecp 
		on ecp.PAYMENT_ID = pp.ID
	left outer join KasabaKIAT_N1.dbo.tbcs_dictionary dic 
		on dic.NAME_RU =ecp.INN 
WHERE CONVERT(DATE, bb.OPERDAY) = CONVERT(DATE, @BEGIN_DATE) 
	--AND pr.Departmentbase LIKE @DEPARTMENT_BASE + '%'
	AND dic.CODE = @DEPARTMENT_BASE
	AND aa.SHARTLI_RAQAM = @PAY_TYPE
ORDER BY aa.TUMAN_SHAXAR  ,bb.OPERDAY;
ELSE
SELECT 
	aa.TUMAN_SHAXAR  , 
	REPLACE(CONVERT(NVARCHAR, bb.OPERDAY, 103), '/', '.') as  OPERDAY , 
	pp.CLACC , 
	ecp.INN, 
	pp.CLMFO, 
	pp.CLNAME , 
	ROUND(pp.SUMPAY/100,2) as POSTUPLENIYA, 
	ecp.FEDERATION_AMT + ecp.OBLSOVPROF_AMT as Obshiy01 ,  
	ecp.FEDERATION_AMT , 
	ecp.OBLSOVPROF_AMT ,  
	pp.PURPOSE,
	ecp.OKONX_NAME,
	ecp.BRANCH_NAME 
FROM KasabaKIAT_N1.dbo.KaznaBankFile bb
	inner join KasabaKIAT_N1.dbo.tblPred pr 
		on bb.COACC = pr.KaznaAccount
	left outer join KasabaKIAT_N1.dbo.Kazna_Account aa
		on bb.KLSDOX = aa.XISOB_RAQAM
	left outer join KasabaKIAT_N1.dbo.KaznaPaymentFile pp
		on bb.GRPID = pp.GRPID
	left outer join KasabaKIAT_N1.dbo.tbbu_ecp_calc ecp 
		on ecp.PAYMENT_ID = pp.ID
WHERE CONVERT(DATE, bb.OPERDAY) = CONVERT(DATE, @BEGIN_DATE) 
	AND pr.Departmentbase LIKE @DEPARTMENT_BASE + '%'
	AND aa.SHARTLI_RAQAM = @PAY_TYPE
ORDER BY aa.TUMAN_SHAXAR  ,bb.OPERDAY;
  
end
