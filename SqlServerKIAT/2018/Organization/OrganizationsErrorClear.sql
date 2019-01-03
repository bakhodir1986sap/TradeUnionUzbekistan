/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [REC_ID]
      ,[DEPARTMENT_BASE]
      ,[USER_ID]
      ,[CODE]
      ,[DEBIT_ACC]
      ,[CREADIT_ACC]
  FROM [KasabaKIAT_N1].[dbo].[tbbu_nastroykiProvodok]
  
  
  SELECT * 
  INTO KasabaKIAT_N1.dbo.tblSprav_Org_musor
  FROM 
  KasabaKIAT_N1.dbo.tblSprav_Org 
  where SUBSTRING(INN , 2 , 1) != '0' and FACE_TYPE = 0 
  and SUBSTRING(INN , 1 , 1) = '2'
  ORDER BY Name_Org
  
  
  SELECT * FROM  KasabaKIAT_N1.dbo.tblOplata 
  WHERE ID_Otras in 
  (SELECT ID FROM 
  KasabaKIAT_N1.dbo.tblSprav_Org 
  where SUBSTRING(INN , 2 , 1) != '0' and FACE_TYPE = 0 
  and SUBSTRING(INN , 1 , 1) = '2')
  
  
    SELECT * FROM  KasabaKIAT_N1.dbo.tblVipl 
  WHERE ID_Org in 
  (SELECT ID FROM 
  KasabaKIAT_N1.dbo.tblSprav_Org 
  where SUBSTRING(INN , 2 , 1) != '0' and FACE_TYPE = 0 
  and SUBSTRING(INN , 1 , 1) = '2')



  SELECT * FROM  KasabaKIAT_N1.dbo.tblOplata 
  WHERE ID_Otras in 
  (SELECT ID FROM 
  KasabaKIAT_N1.dbo.tblSprav_Org 
  where SUBSTRING(INN , 2 , 1) != '0' and FACE_TYPE = 0 
  and SUBSTRING(INN , 1 , 1) = '2')
  
  
  DELETE FROM 
  KasabaKIAT_N1.dbo.tblSprav_Org 
  where SUBSTRING(INN , 2 , 1) != '0' and FACE_TYPE = 0 
  and SUBSTRING(INN , 1 , 1) = '2'
  AND ID NOT IN (  SELECT ID_Org FROM  KasabaKIAT_N1.dbo.tblVipl 
  WHERE ID_Org in 
  (SELECT ID FROM 
  KasabaKIAT_N1.dbo.tblSprav_Org 
  where SUBSTRING(INN , 2 , 1) != '0' and FACE_TYPE = 0 
  and SUBSTRING(INN , 1 , 1) = '2'))