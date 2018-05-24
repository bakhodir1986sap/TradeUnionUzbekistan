  UPDATE [KasabaKIAT_N1].[dbo].[tbcs_dictionary]
  SET CODE = 4
  WHERE [DICTIONARY_TYPE_ID] = 12 AND CODE = 1;
  
  UPDATE [KasabaKIAT_N1].[dbo].[tbcs_dictionary]
  SET CODE = 6
  WHERE [DICTIONARY_TYPE_ID] = 12 AND CODE = 3;
    
  UPDATE [KasabaKIAT_N1].[dbo].[tbcs_dictionary]
  SET CODE = 3
  WHERE [DICTIONARY_TYPE_ID] = 12 AND CODE = 2;

    ALTER TABLE [KasabaKIAT_N1].[dbo].[tblSprav_Org]
  ADD PRAVORGLEVEL INT;
  
  ALTER TABLE [KasabaKIAT_N1].[dbo].[tblSprav_Org]
  ADD DIRECTOR NVARCHAR(200);
  
  ALTER TABLE [KasabaKIAT_N1].[dbo].[tblSprav_Org]
  ADD BUXGALTER NVARCHAR(200);
  
  ALTER TABLE [KasabaKIAT_N1].[dbo].[tblSprav_Org]
  ADD DOVERENNOE_LISO NVARCHAR(200);