USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Postupleniya]    Script Date: 07/06/2018 10:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_PostupleniyaN]
@INN  nvarchar(9),
@DEPARTMENT_BASE nvarchar(255)

as 
begin 
set nocount on;

SELECT AAA.OPERDAY,
	   AAA.INN,
	   AAA.CLNAME,
	   AAA.POSTUPLENIYA,
	   AAA.Obshiy01,
	   AAA.FEDERATION_AMT,
	   AAA.OBLSOVPROF_AMT,
	   AAA.PURPOSE	
FROM
(select 
	'Всего: ' as OPERDAY,
	ecp.INN ,
	pp.CLNAME, 
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
	ecp.INN = @INN
	Group by ecp.INN ,pp.CLNAME

UNION

select 
	REPLACE(CONVERT(NVARCHAR, bb.OPERDAY, 103), '/', '.') as  OPERDAY, 
	ecp.INN , 
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
    AND ecp.INN = @INN) AAA
 ORDER BY    AAA.OPERDAY11 DESC
 ;
  
end

----------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_PostupleniyaN]    Script Date: 07/06/2018 13:44:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_Razbivka_PostupN]
@PAY_TYPE  varchar(3),
@DEPARTMENT_BASE nvarchar(255),
@BEGIN_DATE Datetime

as 
begin 
set nocount on;


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
