CREATE PROCEDURE [dbo].[sp_rt_FormaN13_1]
@aYear int,
@departmentBase  varchar(9)
AS
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
		 
		FROM KasabaKIAT.dbo.tblPost a
		INNER JOIN KasabaKIAT.dbo.tblPos_Put b
		on a.[ID] = b.[ID_Post]
		LEFT OUTER JOIN KasabaKIAT.dbo.tblVipl_Put c
		on b.ID_Sanat = c.ID_Sanat AND b.Num_Put = c.Num_Put
		INNER JOIN KasabaKIAT.dbo.tblVipl d
		on c.[ID_Vipl] = d.ID
		INNER JOIN KasabaKIAT.dbo.tblSprav_Org org
		on d.ID_Org = org.ID
		INNER JOIN dbo.tblSprav_San san
		on c.ID_Sanat = san.ID
		LEFT OUTER JOIN [KasabaKIAT].[dbo].[tblFase] face
		on b.ID_Sanat = face.ID_Sanat AND b.Num_Put = face.Num_Put
		INNER JOIN [KasabaKIAT].dbo.tblSprav_Face spface
		on face.ID_Osnov  = spface.[ID]
		LEFT OUTER JOIN [KasabaKIAT].[dbo].[tblFace_Med] fm
		on face.[ID] = fm.ID_Face
		WHERE a.DepartmentBase = @departmentBase
		AND   YEAR(a.Data_Post) = @aYear;

RETURN 0
