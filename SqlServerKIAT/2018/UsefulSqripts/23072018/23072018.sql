USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_pr_GetNewTransactions]    Script Date: 07/23/2018 10:17:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_pr_GetNewTransactionsN1]
as 
begin 
set nocount on;

select TOP 10000 p.ID as PAYMENT_ID,
case when SUBSTRING(p.KLSDOX, 16, 6) = '321211' 
then ROUND(((SUMPAY /100) * 0.4 / 100) , 2)  

when SUBSTRING(p.KLSDOX, 16, 6) = '321212' 
then ROUND(((SUMPAY /100) * 0.666666667 / 100) , 2) 

when SUBSTRING(p.KLSDOX, 16, 6) = '321213' 
then ROUND(((SUMPAY /100) * 0.666666667 / 100) , 2)   end as PROFSOUZ_AMT,

case when SUBSTRING(p.KLSDOX, 16, 6) = '321211' 
then ROUND(((SUMPAY /100) * 0.4 / 100) * 30 / 100, 2)  

when SUBSTRING(p.KLSDOX, 16, 6) = '321212' 
then ROUND(((SUMPAY /100) * 0.666666667 / 100) * 30 / 100, 2) 

when SUBSTRING(p.KLSDOX, 16, 6) = '321213' 
then ROUND(((SUMPAY /100) * 0.666666667 / 100) * 30 / 100, 2)   end as FEDERATION_AMT,

case when SUBSTRING(p.KLSDOX, 16, 6) = '321211' 
then ROUND(((SUMPAY /100) * 0.4 / 100) * 70 / 100, 2)  

when SUBSTRING(p.KLSDOX, 16, 6) = '321212' 
then ROUND(((SUMPAY /100) * 0.666666667 / 100) * 70 / 100, 2) 

when SUBSTRING(p.KLSDOX, 16, 6) = '321213' 
then ROUND(((SUMPAY /100) * 0.666666667 / 100) * 70 / 100, 2)   end as OBLSOVPROF_AMT,

'172' +  SUBSTRING(KLSDOX, 11, 2) as OBL_DEPARTMENT_BASE,
null as  PROVODKA_ID,
p.PURPOSE ,
p.CLNAME as CLNAME,
CLINN as INN,
'17' + SUBSTRING(p.KLSDOX, 11, 5) as SOATO,
ss.NAME_RU as SOATO_NAME,
p.CLACC , p.OPERDAY , p.GRPID, p.CLNAME
from KaznaPaymentFile  p
left outer join dbo.tbsp_soato_new ss on '17' + SUBSTRING(p.KLSDOX, 11, 5) = ss.KODI
WHERE   P.ID > (SELECT MAX(PAYMENT_ID) FROM KasabaKIAT_N1.dbo.tbbu_ecp_calc)
ORDER BY P.ID
;

  
end

--------------------------------------------------------------------------------------------


ALTER TABLE KasabaKIAT_N1.dbo.tblSprav_Org
ADD DB_BAL money NULL;

ALTER TABLE KasabaKIAT_N1.dbo.tblSprav_Org
ADD CT_BAL money NULL;


