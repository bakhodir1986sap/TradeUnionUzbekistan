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