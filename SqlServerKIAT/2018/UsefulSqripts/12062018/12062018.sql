 UPDATE [KasabaKIAT_N1].[dbo].[tblSprav_Reg] SET CodeReg = CodeReg + ID WHERE CodeReg IN
  (SELECT [CodeReg]
  FROM [KasabaKIAT_N1].[dbo].[tblSprav_Reg]
GROUP BY [CodeReg]
HAVING COUNT(*) > 1);
  DELETE [KasabaKIAT_N1].[dbo].[tblSprav_Reg] WHERE ID =6;
  DELETE [KasabaKIAT_N1].[dbo].[tblSprav_Reg] WHERE ID =174;
  DELETE [KasabaKIAT_N1].[dbo].[tblSprav_Reg] WHERE ID =173;
  DELETE [KasabaKIAT_N1].[dbo].[tblSprav_Reg] WHERE ID =164;
  DELETE [KasabaKIAT_N1].[dbo].[tblSprav_Reg] WHERE ID =172;
  DELETE [KasabaKIAT_N1].[dbo].[tblSprav_Reg] WHERE ID =170;
  DELETE [KasabaKIAT_N1].[dbo].[tblSprav_Reg] WHERE ID =169;
  DELETE [KasabaKIAT_N1].[dbo].[tblSprav_Reg] WHERE ID =168;
  DELETE [KasabaKIAT_N1].[dbo].[tblSprav_Reg] WHERE ID =167;
  DELETE [KasabaKIAT_N1].[dbo].[tblSprav_Reg] WHERE ID =171;
  DELETE [KasabaKIAT_N1].[dbo].[tblSprav_Reg] WHERE ID =165;
  DELETE [KasabaKIAT_N1].[dbo].[tblSprav_Reg] WHERE ID =162;
  DELETE [KasabaKIAT_N1].[dbo].[tblSprav_Reg] WHERE ID =163;
  DELETE [KasabaKIAT_N1].[dbo].[tblSprav_Reg] WHERE ID =161;
  DELETE [KasabaKIAT_N1].[dbo].[tblSprav_Reg] WHERE ID = 166;
  DELETE [KasabaKIAT_N1].[dbo].[tblSprav_Reg] WHERE ID = 160;



  insert into [KasabaKIAT_N1].[dbo].[sp_soato]
SELECT 0 as [version]
      ,ss.KODI as [code]
      ,null as [s_comment]
      ,GETDATE() as [s_create]
      ,null as [s_status]
      ,ss.NAME_RU as [name_ru]
      ,ss.NAME_RU as [name_uz]
      ,ss.NAME_RU as [name_uzl]
      ,ss.CENTRE_NAME_RU as [name_center_ru]
      ,ss.CENTRE_NAME_RU as [name_center_uz]
      ,ss.CENTRE_NAME_RU as  [name_center_uzl]
      ,1 as [s_user_id]
      ,null as [parent_code]
      ,null as [keys]
      ,null as  [NATIVE_ID] 
FROM KasabaKIAT_N1.dbo.tbsp_soato_new ss
where ss.KODI not in 
(SELECT [code]
      FROM [KasabaKIAT_N1].[dbo].[sp_soato]);


  UPDATE [KasabaKIAT_N1].[dbo].[tblSprav_Reg]
  SET [SOATO_CODE] = '1727401'
  WHERE [ID] = 213