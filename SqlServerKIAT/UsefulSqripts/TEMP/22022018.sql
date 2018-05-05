ALTER TABLE dbo.buildingfixedasset		ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.eco_platej				ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.eco_xs					ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.fill_rate				ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.role_permission			ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.roles					ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_branch				ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_classname			ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_contracttype			ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_country				ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_degree				ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_departmenttype		ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_employees			ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_feedback				ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_files				ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_fixedassettype		ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_fs					ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_measure				ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_nation				ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_okonx				ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_opf					ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_organization			ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_position				ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_position1			ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_sanatorium_profil	ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_sanatorium_type		ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_section				ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_soato				ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_soogu				ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sp_soogu_copy			ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.sps_translate			ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.t_agreement				ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.t_bkut_statistics		ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.t_camp					ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.t_camp_statistics		ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.t_collectiveagreement	ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.t_contract				ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.t_contractappendix		ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.t_departmentbase		ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.t_egrpo					ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.t_egrpo_about			ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.t_egrpotest				ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.t_fixedassets			ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.t_generalagreement		ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.t_ku_report				ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.t_normativdoc			ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.useractions				ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.useractivity			ADD NATIVE_ID BIGINT;
ALTER TABLE dbo.users					ADD NATIVE_ID BIGINT;




ALTER TABLE [KasabaKIAT].[dbo].[tbpt_sanatory_info]
  ADD SANATORY_NAME NVARCHAR(255);


ALTER TABLE [KasabaKIAT].[dbo].[tbpt_sanatory_info]
  ADD INN VARCHAR(9);


USE [KasabaKIAT]
GO
/** Object:  StoredProcedure [dbo].[sp_get_RealizasiyaPutyovokDetail]    Script Date: 02/22/2018 20:12:14 **/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_get_RealizasiyaPutyovokDetail]
@DEP_BASE varchar(255),
@SANID int
as 
begin 
set nocount on;

SELECT B.ID,
       B.ID_SANAT , 
     SAN.Name_San ,
     B.NUM_PUT , 
     B.DATASAN,
     CASE WHEN  C.ID IS NULL or c.Uchet = 0 THEN 0 ELSE 1 END BRON_FLG,
     CASE WHEN  DATEDIFF(DAY, GETDATE() , B.DATASAN) <= 14 THEN 1 ELSE 0 END URGENT_FLG
FROM KASABAKIAT.dbo.tblPost A
INNER JOIN KASABAKIAT.dbo.tblPos_Put B 
ON A.ID = B.ID_POST
INNER JOIN KASABAKIAT.dbo.tblSprav_San SAN
ON SAN.ID = B.ID_SANAT
LEFT JOIN  KASABAKIAT.dbo.tblBron C    
ON C.ID_San = B.ID_SANAT AND C.NUM_PUT = B.Num_Put
WHERE B.ID_SANAT = @SANID
AND B.ID_POLVOZ = 0 
AND A.DEPARTMENTBASE = @DEP_BASE;
  
end