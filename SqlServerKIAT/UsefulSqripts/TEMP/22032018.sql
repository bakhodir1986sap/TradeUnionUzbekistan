USE [KasabaKIAT]
GO
/****** Object:  StoredProcedure [dbo].[sp_get_RealizasiyaPutyovokDetailN]    Script Date: 03/22/2018 22:53:47 ******/
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
  LEFT OUTER JOIN [KasabaKIAT].dbo.sp_okonx oo         on oo.code   = ee.okonx_id
  LEFT OUTER JOIN [KasabaKIAT].dbo.tbpt_zayavki zz     on zz.REC_ID = p.ZAYAVKA_ID
  WHERE p.REC_ID = @REC_ID
  AND p.STATUS_CODE = 1;
  
end