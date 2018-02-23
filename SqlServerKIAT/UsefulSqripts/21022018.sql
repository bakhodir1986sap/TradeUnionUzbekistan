ALTER TABLE [KasabaKIAT].[dbo].[tbpt_putyovkarealizasiya]
ADD [NAKLADNOY_NOMER] VARCHAR(20);

 ALTER TABLE [KasabaKIAT].dbo.tbpt_zayavki
 ADD SOTR_INN VARCHAR(9);

 ALTER TABLE [KasabaKIAT].[dbo].[tbpt_putyovkarealizasiya] 
 ADD TALON_NUMBER VARCHAR(20);

   ALTER TABLE [KasabaKIAT].[dbo].[tbpt_putyovkarealizasiya]
  ADD STATUS_CODE bit;

USE [KasabaKIAT]
GO
/** Object:  StoredProcedure [dbo].[sp_get_ProsmotrRealizovannixPutyovokDetali]    Script Date: 02/21/2018 19:06:29 **/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_get_ProsmotrRealizovannixPutyovokDetali]
@REC_ID bigint
as 
begin 
set nocount on;

SELECT p.[REC_ID]
      ,p.[DEPARTMENT_CODE]
      ,d.name_ru as MakeOrgName
      ,p.[TO_DEPARTMENT_CODE]
      ,d2.bkut_name
      ,p.[USER_ID]
      ,p.[SANATORIUM_ID]
      ,p.[CAMP_ID]
      ,p.[SERAIL_NUMBER]
      ,p.[REST_BEGIN_DATE]
      ,p.[REST_END_DATE]
      ,p.[FULL_NAME]
      ,p.[PRICE]
      ,p.[PAYMENT_PERSENT]
      ,p.[RECIEVER]
      ,p.[DOVERENNOST_NUMBER]
      ,p.[DOVER_ISSUE_DATE]
      ,p.[DOVER_EXPIRE_DATE]
      ,p.[QUANTITY]
      ,p.[ZAYAVKA_ID]
      ,p.[PLATEJ_DATE]
      ,p.[PLATEJ_AMOUNT]
      ,p.[PLATEJ_NUMBER]
      ,p.[TALON_BACK_DATE]
      ,p.[REMARK]
      ,p.[ORG_INN]
      , a.Name_San
      ,p.[NAKLADNOY_NOMER]
  FROM [KasabaKIAT].[dbo].[tbpt_putyovkarealizasiya] p
  INNER JOIN [KasabaKIAT].dbo.tblSprav_San a      on  p.SANATORIUM_ID = a.ID
  INNER JOIN [KasabaKIAT].dbo.t_departmentbase d  on d.code = p.[DEPARTMENT_CODE]
  INNER JOIN [KasabaKIAT].dbo.t_departmentbase d2 on d2.code = p.[TO_DEPARTMENT_CODE]
  WHERE [REC_ID] = @REC_ID
  AND p.STATUS_CODE = 1;
  
end



--USE [KasabaKIAT]
--GO
/****** Object:  StoredProcedure [dbo].[sp_get_RealizasiyaPutyovokDetailN]    Script Date: 02/21/2018 22:30:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_get_RealizasiyaPutyovokDetailN]
@REC_ID bigint
as 
begin 
set nocount on;

SELECT p.[REC_ID]
      ,p.[TO_DEPARTMENT_CODE]
      ,d2.bkut_name
      ,EE.ID AS KOD
      ,EE.INN AS INN
      ,EE.name_full
      ,zz.FULL_NAME
      ,zz.POSITION
      ,zz.SOTR_INN
      ,oo.name_ru as OKONX_NAME
      ,p.REMARK
      ,p.TALON_NUMBER
      ,p.[RECIEVER]
      ,p.[DOVERENNOST_NUMBER]
      ,p.[DOVER_ISSUE_DATE]
      ,p.[DOVER_EXPIRE_DATE]
  FROM [KasabaKIAT].[dbo].[tbpt_putyovkarealizasiya] p
  INNER JOIN [KasabaKIAT].dbo.tblSprav_San a      on p.SANATORIUM_ID = a.ID
  INNER JOIN [KasabaKIAT].dbo.t_departmentbase d2 on d2.code   = p.[TO_DEPARTMENT_CODE]
  INNER JOIN [KasabaKIAT].dbo.t_egrpo ee      on ee.INN    = d2.LE_INN
  INNER JOIN [KasabaKIAT].dbo.sp_okonx oo         on oo.code   = ee.okonx_id
  INNER JOIN [KasabaKIAT].dbo.tbpt_zayavki zz     on zz.REC_ID = p.ZAYAVKA_ID
  WHERE p.REC_ID = @REC_ID
  AND p.STATUS_CODE = 1;
  
end




--USE [KasabaKIAT]
--GO
/****** Object:  StoredProcedure [dbo].[sp_get_ProsmotrRealizovannixPutyovok]    Script Date: 02/21/2018 22:32:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_get_ProsmotrRealizovannixPutyovok]
@DEP_BASE varchar(255)
as 
begin 
set nocount on;

SELECT [REC_ID]
      ,[DEPARTMENT_CODE]
      ,[TO_DEPARTMENT_CODE]
      ,[USER_ID]
      ,[SANATORIUM_ID]
      ,[CAMP_ID]
      ,[SERAIL_NUMBER]
      ,[REST_BEGIN_DATE]
      ,[REST_END_DATE]
      ,[FULL_NAME]
      ,[PRICE]
      ,[PAYMENT_PERSENT]
      ,[RECIEVER]
      ,[DOVERENNOST_NUMBER]
      ,[DOVER_ISSUE_DATE]
      ,[DOVER_EXPIRE_DATE]
      ,[QUANTITY]
      ,[ZAYAVKA_ID]
      ,[PLATEJ_DATE]
      ,[PLATEJ_AMOUNT]
      ,[PLATEJ_NUMBER]
      ,[TALON_BACK_DATE]
      ,[REMARK]
      ,[ORG_INN]
      , a.Name_San
  FROM [KasabaKIAT].[dbo].[tbpt_putyovkarealizasiya] p
  INNER JOIN [KasabaKIAT].dbo.tblSprav_San a on  p.SANATORIUM_ID = a.ID
  WHERE [DEPARTMENT_CODE] = @DEP_BASE
  AND p.STATUS_CODE = 1;
  
end


--------------------------------------------------------------------

USE [KasabaKIAT]
GO

/****** Object:  Table [dbo].[tbpt_putyovka_statechange]    Script Date: 02/22/2018 01:02:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tbpt_putyovka_statechange](
	[REC_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ID_SANAT] [int] NULL,
	[NUM_PUT] [varchar](10) NULL,
	[CHANGE_DT] [datetime] NULL,
	[STATE] [int] NULL,
	[DEPARTMENT_BASE] [varchar](255) NULL,
	[USER_ID] [bigint] NULL,
 CONSTRAINT [PK_tbpt_putyovka_statechange] PRIMARY KEY CLUSTERED 
(
	[REC_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

