USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_FormaN13]    Script Date: 08/03/2018 10:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_rt_FormaN13]
@aYear int,
@orgInn  nvarchar(9),
@DepartmentBase varchar(255)

as 
begin 
set nocount on;

SELECT a.Data_Post as Data_Postupleniya,
	   d.Num_Doc as Nomer_SchetFacturi,
	   org.Name_Org as NameimenovaniyeOrg,
	   c.Num_Put as NomerPutyovki,
	   b.DataSan as DataZaezda,
	   b.[Symma_Put] as SenaPutyovki,
	   d.[Data_Post] as DataPokupkiPutyovki,
	   spface.Name_Os as NomerResheniyaOVidache,
	   face.[LastName] + ' ' + face.[Name] + ' ' + face.[FirstName] as FIO,
	   face.Rabota + ' ' + face.Dolj as MestoRaboti,
	   fm.[Opl_Data] as DataPlateja,
	   fm.[Opl_Symma] as SymmaPlateja,
	   fm.[Opl_Doc] as NomerPlatejnogoPorucheniya,
	   fm.[Tal_Doc] as NomerTalona,
       fm.[Tal_Data] as DataTalona
 
FROM KasabaKIAT_N1.dbo.tblPost a
INNER JOIN KasabaKIAT_N1.dbo.tblPos_Put b
on a.[ID] = b.[ID_Post]
LEFT OUTER JOIN KasabaKIAT_N1.dbo.tblVipl_Put c
on b.ID_Sanat = c.ID_Sanat AND b.Num_Put = c.Num_Put
INNER JOIN KasabaKIAT_N1.dbo.tblVipl d
on c.[ID_Vipl] = d.ID
INNER JOIN KasabaKIAT_N1.dbo.tblSprav_Org org
on d.ID_Org = org.ID
INNER JOIN KasabaKIAT_N1.dbo.tblSprav_San san
on c.ID_Sanat = san.ID
LEFT OUTER JOIN [KasabaKIAT_N1].[dbo].[tblFase] face
on b.ID_Sanat = face.ID_Sanat AND b.Num_Put = face.Num_Put
INNER JOIN [KasabaKIAT_N1].dbo.tblSprav_Face spface
on face.ID_Osnov  = spface.[ID]
LEFT OUTER JOIN [KasabaKIAT_N1].[dbo].[tblFace_Med] fm
on face.[ID] = fm.ID_Face
WHERE org.INN LIKE @orgInn + '%'
AND   org.DepartmentBase = @DepartmentBase
AND   YEAR(a.Data_Post) = @aYear
AND   a.DepartmentBase = @DepartmentBase
AND   d.DepartmentBase = @DepartmentBase;
  
end
