  ALTER TABLE [KasabaKIAT_N1].[dbo].[tblSprav_Org]
  ADD FACE_TYPE int default(0);
  
  ALTER TABLE [KasabaKIAT_N1].[dbo].[tblSprav_Org]
  ADD Pas_ser nvarchar(50);
  
  ALTER TABLE [KasabaKIAT_N1].[dbo].[tblSprav_Org]
  ADD Pas_Num nvarchar(50);
  
  ALTER TABLE [KasabaKIAT_N1].[dbo].[tblSprav_Org]
  ADD Pas_KD nvarchar(50);
  
  
  ALTER TABLE [KasabaKIAT_N1].[dbo].[tblSprav_Org]
  ADD Pas_Data datetime;
  
  UPDATE [KasabaKIAT_N1].[dbo].[tblSprav_Org]
  SET FACE_TYPE = 0;