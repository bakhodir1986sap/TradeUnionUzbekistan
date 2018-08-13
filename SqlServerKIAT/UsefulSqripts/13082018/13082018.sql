  ALTER TABLE [KasabaKIAT_N1].[dbo].[tbku_StrokaProfsoyza]
  ADD PRIVFLG BIT default(0);
  
  UPDATE [KasabaKIAT_N1].[dbo].[tbku_StrokaProfsoyza]
  SET PRIVFLG = 0
  WHERE MATCH_DEPARTMENTBASE NOT IN ('173Т2','173О1','173Н2');
  
  
  UPDATE [KasabaKIAT_N1].[dbo].[tbku_StrokaProfsoyza]
  SET PRIVFLG = 1
  WHERE MATCH_DEPARTMENTBASE IN ('173Т2','173О1','173Н2');


  ALTER TABLE [KasabaKIAT_N1].[dbo].[tbbu_ecp_calc]
  ADD ADEP_CODE nvarchar(255);


    UPDATE [KasabaKIAT_N1].[dbo].[tbbu_ecp_calc]
  SET BRANCH_CODE = 'О1'
  WHERE INN in (  SELECT dd.NAME_RU FROM KasabaKIAT_N1.dbo.tbcs_dictionary dd
  where dd.CODE = '173О1')  AND (BRANCH_CODE IS NULL OR  BRANCH_CODE = '')
  
  
  UPDATE [KasabaKIAT_N1].[dbo].[tbbu_ecp_calc]
  SET BRANCH_CODE = 'Т2'
  WHERE INN in (  SELECT dd.NAME_RU FROM KasabaKIAT_N1.dbo.tbcs_dictionary dd
  where dd.CODE = '173Т2') AND (BRANCH_CODE IS NULL OR  BRANCH_CODE = '')
  
  
  UPDATE [KasabaKIAT_N1].[dbo].[tbbu_ecp_calc]
  SET ADEP_CODE = CASE WHEN BRANCH_CODE = 'Н2' THEN '173Н2'
                       WHEN BRANCH_CODE = 'Т2' THEN '173Т2'
                       WHEN BRANCH_CODE = 'О1' THEN '173О1'
                       ELSE '173'+ BRANCH_CODE + SUBSTRING([SOATO] , 3 , 5) END
  WHERE BRANCH_CODE IS NOT NULL;


  -------------------------------------******************************************-----------------------------------


    ALTER TABLE [KasabaKIAT_N1].[dbo].[tbku_StrokaProfsoyza]
  ADD PRIVFLG BIT default(0);
  
  UPDATE [KasabaKIAT_N1].[dbo].[tbku_StrokaProfsoyza]
  SET PRIVFLG = 0
  WHERE MATCH_DEPARTMENTBASE NOT IN ('173Т2','173О1','173Н2');
  
  
  UPDATE [KasabaKIAT_N1].[dbo].[tbku_StrokaProfsoyza]
  SET PRIVFLG = 1
  WHERE MATCH_DEPARTMENTBASE IN ('173Т2','173О1','173Н2');


  ALTER TABLE [KasabaKIAT_N1].[dbo].[tbbu_ecp_calc]
  ADD ADEP_CODE nvarchar(255);


    UPDATE [KasabaKIAT_N1].[dbo].[tbbu_ecp_calc]
  SET BRANCH_CODE = 'О1'
  WHERE INN in (  SELECT dd.NAME_RU FROM KasabaKIAT_N1.dbo.tbcs_dictionary dd
  where dd.CODE = '173О1')  AND (BRANCH_CODE IS NULL OR  BRANCH_CODE = '')
  
  SELECT * FROM [KasabaKIAT_N1].[dbo].[tbbu_ecp_calc]
  WHERE INN in (  SELECT dd.NAME_RU FROM KasabaKIAT_N1.dbo.tbcs_dictionary dd
  where dd.CODE = '173О1')
  
  
  UPDATE [KasabaKIAT_N1].[dbo].[tbbu_ecp_calc]
  SET BRANCH_CODE = 'Т2'
  WHERE INN in (  SELECT dd.NAME_RU FROM KasabaKIAT_N1.dbo.tbcs_dictionary dd
  where dd.CODE = '173Т2') AND (BRANCH_CODE IS NULL OR  BRANCH_CODE = '')
  
  
  UPDATE [KasabaKIAT_N1].[dbo].[tbbu_ecp_calc]
  SET ADEP_CODE = CASE WHEN BRANCH_CODE = 'Н2' THEN '173Н2'
                       WHEN BRANCH_CODE = 'Т2' THEN '173Т2'
                       WHEN BRANCH_CODE = 'О1' THEN '173О1'
                       ELSE '173'+ BRANCH_CODE + SUBSTRING([SOATO] , 3 , 5) END
  WHERE BRANCH_CODE IS NOT NULL AND BRANCH_CODE != '';
  
  SELECT * FROM [KasabaKIAT_N1].[dbo].[tbbu_ecp_calc] WHERE BRANCH_CODE = ''
  
  
    UPDATE [KasabaKIAT_N1].[dbo].[tbbu_ecp_calc]
  SET ADEP_CODE = null
  WHERE BRANCH_CODE = ''
  
  
  SELECT * from KasabaKIAT_N1.dbo.tblSprav_Org
WHERE INN in (  SELECT dd.NAME_RU FROM KasabaKIAT_N1.dbo.tbcs_dictionary dd
  where dd.CODE = '173Т2')
  
  15
  
  UPDATE KasabaKIAT_N1.dbo.tblSprav_Org
  SET ID_Ot = 10
  WHERE INN in (  SELECT dd.NAME_RU FROM KasabaKIAT_N1.dbo.tbcs_dictionary dd
  where dd.CODE = '173Т2')
  
  
    SELECT * from KasabaKIAT_N1.dbo.tblSprav_Otras
  
  
  select  TOP 1000 * from [KasabaKIAT_N1].[dbo].[tbbu_ecp_calc]
  where INN = '200541746'