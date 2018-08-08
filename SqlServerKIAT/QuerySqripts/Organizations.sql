SELECT [ID_Ot]
      ,[Name_Org]
      ,TTT.[INN]
      ,[RS]
      ,[MFO]
      ,[Bank]
      ,[OKONX]
      ,[Adress]
      ,[Telefon]
      ,[Region]
      ,[DT_OST]
      ,[KT_OST]
      ,[E_Mail]
      ,[UserId]
      ,TTT.OBL_DEPARTMENT_BASE as [DepartmentBase]
      ,[IS_PRAVSOYUZ]
      ,[ParentID]
      ,[PRAVORGLEVEL]
      ,[DIRECTOR]
      ,[BUXGALTER]
      ,[DOVERENNOE_LISO]
      ,[ORIG_INN]
      ,[OWNER_CODE]
      ,[TIMESTAMP_CH]
      ,[DB_BAL]
      ,[CT_BAL] 
FROM KasabaKIAT_N1.dbo.tblSprav_Org org
inner join
(select INN, OBL_DEPARTMENT_BASE 
from KasabaKIAT_N1.dbo.tbbu_ecp_calc  ecp
where INN IN (SELECT TT.INN FROM 
(select INN, OBL_DEPARTMENT_BASE 
from KasabaKIAT_N1.dbo.tbbu_ecp_calc 
group by  INN, OBL_DEPARTMENT_BASE) TT
GROUP BY TT.INN
HAVING COUNT(TT.INN) > 1)
AND NOT EXISTS (SELECT * FROM KasabaKIAT_N1.dbo.tblSprav_Org org
			    WHERE org.INN = ecp.INN AND org.DepartmentBase = ecp.OBL_DEPARTMENT_BASE)
GROUP BY INN, OBL_DEPARTMENT_BASE ) TTT
on org.INN = TTT.INN;


-------------------------------------------------------------------

--Working organizations list
SELECT 
CASE WHEN  TTT.ID_Ot IS NULL THEN N'Не определен' ELSE TTT.Name_Ot END as OTRASL,
org.inn,
org.Name_Org,
reg.NameReg
,[RS]
      ,[MFO]
      ,[Bank]
      ,[OKONX]
      ,[Adress]
      ,[Telefon]
      ,[Region]
      ,[DB_BAL]
      ,[CT_BAL] 
FROM KasabaKIAT_N1.dbo.tblSprav_Org org
left outer join (SELECT * FROM KasabaKIAT_N1.dbo.tblSprav_Otras ot where ot.DepartmentBase = '17235') TTT
on org.ID_Ot =TTT.ID_Ot 
left outer join KasabaKIAT_N1.dbo.tblSprav_Reg Reg on org.Region = reg.CodeReg
WHERE org.INN in (select ecp.INN from KasabaKIAT_N1.dbo.tbbu_ecp_calc ecp
				  where ecp.OBL_DEPARTMENT_BASE = '17235')
AND org.DepartmentBase = '17235'