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



UPDATE [KasabaKIAT_N1].[dbo].[users]
SET s_status = 'INACTIVE';

  

UPDATE [KasabaKIAT_N1].[dbo].[users]
SET s_status = 'ACTIVE'
WHERE [user_name] in
('navoi@kasaba.uz'    
,'andijon@kasaba.uz'    
,'buxoro@kasaba.uz'    
,'fargona@kasaba.uz'    
,'jizzax@kasaba.uz'    
,'namangan@kasaba.uz'  
,'qashqadaryo@kasaba.uz' 
,'qoraqalpoq@kasaba.uz'  
,'samarqand@kasaba.uz'   
,'sirdaryo@kasaba.uz'    
,'surxondaryo@kasaba.uz' 
,'toshkent@kasaba.uz'
,'toshvil@kasaba.uz'
,'xorazm@kasaba.uz'
,'fargona_bkut_test1@kasaba.uz'
,'fargona_bkut_test2@kasaba.uz'
,'andijon_bkut_test1@kasaba.uz'
,'andijon_bkut_test2@kasaba.uz'
,'buxoro_bkut_test1@kasaba.uz'
,'buxoro_bkut_test2@kasaba.uz'
,'jizzax_bkut_test1@kasaba.uz'
,'jizzax_bkut_test2@kasaba.uz'
,'qashqadaryo_bkut_test1@kasaba.uz'
,'qashqadaryo_bkut_test2@kasaba.uz'
,'sirdaryo_bkut_test1@kasaba.uz'
,'sirdaryo_bkut_test2@kasaba.uz'
,'tashvil_bkut_test1@kasaba.uz'
,'tashvil_bkut_test2@kasaba.uz'
,'tashkent_bkut_test1@kasaba.uz'
,'tashkent_bkut_test2@kasaba.uz'
,'xorazm_bkut_test1@kasaba.uz'
,'xorazm_bkut_test2@kasaba.uz'
,'federasiya@kasaba.uz'
,'mehnat@kasaba.uz'
,'temiryol@kasaba.uz'
,'navoiy.komb@kasaba.uz'
,'olmaliq.komb@kasaba.uz'
,'azamat_tashvil@kasaba.uz');

--------------------------------------------------
USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[uspLogin]    Script Date: 05/26/2018 21:13:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[uspLogin] 
@login nvarchar(50),
@parol nvarchar(50)
as 
select id 
from [dbo].[users]  
where 
USER_NAME=@login 
AND PASSWORD=hashbytes('md5', @parol)
AND s_status = 'ACTIVE'


---------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[uspLoginChange]    Script Date: 05/26/2018 22:18:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[userAdd] 
@login nvarchar(255),
@parol nvarchar(50),
@bkit_id bigint,
@role_id bigint

as 
begin 
set nocount on;

  INSERT INTO [KasabaKIAT_N1].[dbo].[users]
           ([version]
           ,[account_expired]
           ,[account_locked]
           ,[changed]
           ,[created]
           ,[email]
           ,[last_logged]
           ,[last_logged_id]
           ,[state]
           ,[password]
           ,[salt]
           ,[language]
           ,[is_super_admin]
           ,[user_name]
           ,[s_comment]
           ,[s_create]
           ,[s_status]
           ,[s_user_id]
           ,[usertype]
           ,[bkut_id]
           ,[dep_id]
           ,[role_id]
           ,[bkuq_id]
           ,[REGION_ID]
           ,[BRANCH_ID]
           ,[NATIVE_ID])
     VALUES
           (0
           ,0
           ,0
           ,GETDATE()
           ,GETDATE()
           ,@login
           ,GETDATE()
           ,1
           ,'ACTIVE'
           ,hashbytes('md5', @parol)
           ,''
           ,'RU'
           ,0
           ,@login
           ,''
           ,GETDATE()
           ,'ACTIVE'
           ,12
           ,'BKUT'
           ,@bkit_id
           ,null
           ,@role_id
           ,null
           ,null
           ,null
           ,null
           );

end


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[uspLoginChange]    Script Date: 05/26/2018 22:18:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[userUpdate] 
@userId bigint,
@login nvarchar(255),
@parol nvarchar(50),
@bkit_id bigint,
@role_id bigint

as 
begin 
set nocount on;

  UPDATE [KasabaKIAT_N1].[dbo].[users]
  SET [user_name] = @login,
  [password] = hashbytes('md5', @parol),
  [bkut_id] = @bkit_id,
  [role_id] = @role_id
  WHERE id = @userId;
 
end


----------------------------------------------------------

USE [KasabaKIAT_N1]
GO

/****** Object:  Table [dbo].[tblFile]    Script Date: 05/27/2018 14:49:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblFile](
	[ID] [bigint] NOT NULL IDENTITY(1,1),
	[EXTERNAL_ID] [bigint] NULL,
	[OP_TYPE] [int] NULL,
	[NAME] [nvarchar](100) NULL,
	[CONTENTTYPE] [nvarchar](200) NULL,
	[DATA] [varbinary](max) NULL,
	[OPNAME] [nvarchar](500) NULL,
 CONSTRAINT [PK_tblFile] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

