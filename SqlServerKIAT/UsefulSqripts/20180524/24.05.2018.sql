 ALTER TABLE [KasabaKIAT_N1].[dbo].[tblSprav_Org]
 ADD ORIG_INN VARCHAR(20);

 ALTER TABLE [KasabaKIAT_N1].[dbo].[tblSprav_Org]
 ADD OWNER_CODE VARCHAR(255);

 --Fill not exists regions for Organization 
  INSERT INTO [KasabaKIAT_N1].[dbo].[tblSprav_Reg]
  SELECT [name_ru] as NameReg,
      2000 +  v.ID_Code  as CodeReg,
      1 as  UserId,
      '17226' as    DepartmentBase,
      v.ID as  VilId,
      d.soato_code as  SOATO_CODE
  FROM [KasabaKIAT_N1].[dbo].[t_departmentbase] d
  inner join [KasabaKIAT_N1].[dbo].tblSprav_Vil v
  on substring(d.soato_code , 1 , 4) = v.SOATO_CODE
  where parent_code LIKE '172%'
  AND LEN(CODE) = 8 
  AND NOT EXISTS (SELECT [ID]
        FROM [KasabaKIAT_N1].[dbo].[tblSprav_Reg] r
        WHERE r.SOATO_CODE = d.soato_code);



		  INSERT INTO [KasabaKIAT_N1].[dbo].[tblSprav_Reg]
           ([NameReg]
           ,[CodeReg]
           ,[UserId]
           ,[DepartmentBase]
           ,[VilId]
           ,[SOATO_CODE])
     VALUES
           ('Пустой'
           ,-1
           ,1
           ,null
           ,null
           ,null);

