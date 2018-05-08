INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionarytype]
           ([CODE]
           ,[NAME_UZ]
           ,[NAME_RU]
           ,[NAME_LT]
           ,[USER_ID])
     VALUES
           ('TipOplati'
           ,'Тип оплаты'
           ,'Тип оплаты'
           ,'Тип оплаты'
           ,1);


INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary]
           ([CODE]
           ,[NAME_RU]
           ,[NAME_UZ]
           ,[NAME_LT]
           ,[DICTIONARY_TYPE_ID]
           ,[STATUS]
           ,[USER_ID])
     VALUES
           ('01'
           ,'Взаиморасчеты'
           ,'Взаиморасчеты'
           ,'Взаиморасчеты'
           ,7
           ,1
           ,1);
GO


INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary]
           ([CODE]
           ,[NAME_RU]
           ,[NAME_UZ]
           ,[NAME_LT]
           ,[DICTIONARY_TYPE_ID]
           ,[STATUS]
           ,[USER_ID])
     VALUES
           ('02'
           ,'Оплата Путевок'
           ,'Оплата Путевок'
           ,'Оплата Путевок'
           ,7
           ,1
           ,1);
GO

INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionarytype]
           ([CODE]
           ,[NAME_UZ]
           ,[NAME_RU]
           ,[NAME_LT]
           ,[USER_ID])
     VALUES
           ('VidOplati'
           ,'Вид оплаты'
           ,'Вид оплаты'
           ,'Вид оплаты'
           ,1);
           
           
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary]
           ([CODE]
           ,[NAME_RU]
           ,[NAME_UZ]
           ,[NAME_LT]
           ,[DICTIONARY_TYPE_ID]
           ,[STATUS]
           ,[USER_ID])
     VALUES
           ('01'
           ,'Определен'
           ,'Определен'
           ,'Определен'
           ,8
           ,1
           ,1);
GO


INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary]
           ([CODE]
           ,[NAME_RU]
           ,[NAME_UZ]
           ,[NAME_LT]
           ,[DICTIONARY_TYPE_ID]
           ,[STATUS]
           ,[USER_ID])
     VALUES
           ('02'
           ,'Не определен'
           ,'Не определен'
           ,'Не определен'
           ,8
           ,1
           ,1);
GO           
           

----------------------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Zapros_Putyovki]    Script Date: 05/06/2018 22:39:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_rt_Zapros_Putyovki]
@beginDate Datetime,
@endDate  Datetime,
@ID_ORG bigint,
@ID_SANAT bigint,
@LASTNAME nvarchar(255),
@NUM_PUT varchar(10),
@POL int,
@DepartmentBase varchar(255)
as 
begin 
set nocount on;

/****** Script for SelectTopNRows command from SSMS  ******/
  select 
  postMain.Vid							as StatusPutyovki,
  Org1.Name_Org							as Otpravitel_Postavshik,
  postMain.Data_Post					as DataPrixoda,
  postMain.Num_Doc						as NomerSchetFakturi,
  postMain.Name_Dov						as PrixodDoverennost,
  Kv1.Name_Kv + ' ' + postMain.SmFil	as PeriodPrixoda,
  San1.Name_San							as NazvaniyaSanatoriya,
  postDtl.Num_Put						as N_Putyovki,
  postDtl.DataSan						as SrokPutyovki,
  postDtl.Symma_Put						as Stoimost,
  postDtl.ID_PolVoz						as StatusPrioda,
  ViplMain.Data_Post					as DataVidachi,
  ViplMain.Num_Doc						as NomerNakladnoy,
  Org2.Name_Org							as PoluchatelPokupatel,
  ViplMain.Name_Dov						as RasxodDoverennost,
  Kv2.Name_Kv + ' ' + ViplMain.SmFil	as PeriodVidachi,
  ViplPut.ID_PolVoz						as StatusVidachi,
  face.LastName						    as FamiliyaVladelsa,
  face.Name								as ImyaVladelsa,
  face.FirstName						as OtchstvoVladelsa,
  face.Adress							as MestoJitelstva,
  face.Rabota							as MestoRaboti,
  face.Dolj								as Doljnost,
  Procent.Name_Proc						as Procent,
  face.Pas_ser						    as SeriyaPassporta,
  face.Pas_Num							as NomerPassporta,
  face.Pas_KD							as KemVidan,
  face.Pas_Data							as DataVidachi,
  sprvface.Name_Os						as TipPutyovki,
  Osnov.Name_Os							as OsnovaniyeVidachi,
  face.Pol								as Pol,
  FMed.Med_Doc                          as MedisinaNomer,
  FMed.Med_Data							as MedisinaDataVidachi,
  San2.Name_San							as MedReqSanatory,
  FMed.Opl_Doc							as UplataNomer,
  FMed.Opl_Data							as UplataDataDoc,
  FMed.Opl_Symma						as UplataSumma,
  FMed.Tal_Doc							as ObratniyTalon,
  FMed.Tal_Data							as ObratniyTalonData,
  Oplata.Data_Post						as DataOplati,
  Oplata.Num_Doc						as NomerDocumenta,
  OplRas.Symma							as SummaViplati,
  Oplata.Osnov							as OsnovaniyeOplati,
  vozvrat.Tip_Voz						as TipVozvrata,
  vozvrat.Data_Post						as DataVozvrata,
  vozvrat.Num_Doc						as NomerDocumentaVozvrat,
  Org3.Name_Org							as IzyataPutyovka,
  Org4.Name_Org							as OtdanaPutyovka,
  vozvrat.Dover							as DovrennostVozvrat,
  vozvrat.SymmaP						as SummaVozvrata
  from 
  --PrixodPutyovka
  [KasabaKIAT_N1].dbo.tblPost postMain
  inner join [KasabaKIAT_N1].dbo.tblPos_Put postDtl  on  postMain.ID = postDtl.ID_Post
  inner join [KasabaKIAT_N1].dbo.tblSprav_Org Org1   on  postMain.ID_Pos = Org1.ID
  inner join [KasabaKIAT_N1].dbo.tblSprav_Kv Kv1     on  postMain.ID_KV =  Kv1.ID
  inner join [KasabaKIAT_N1].dbo.tblSprav_San San1   on  postDtl.ID_Sanat = San1.ID
  --RasxodPutyovka
  inner join [KasabaKIAT_N1].dbo.tblVipl_Put ViplPut on  postDtl.Num_Put = ViplPut.Num_Put AND postDtl.ID_Sanat = ViplPut.ID_Sanat
  inner join [KasabaKIAT_N1].dbo.tblVipl ViplMain	 on  ViplPut.ID_Vipl = ViplMain.ID
  inner join [KasabaKIAT_N1].dbo.tblSprav_Org Org2   on  ViplMain.ID_Org = Org2.ID
  inner join [KasabaKIAT_N1].dbo.tblSprav_Kv Kv2     on  ViplMain.ID_KV =  Kv2.ID
  --DanniyeVladelsev
  inner join [KasabaKIAT_N1].dbo.tblFase face		 on  ViplPut.ID = face.ID_Put
  inner join [KasabaKIAT_N1].dbo.tblFace_Med FMed	 on  face.ID = FMed.ID_Face
  inner join [KasabaKIAT_N1].dbo.tblSprav_Pr Procent on  face.Gost_Pr = Procent.ID
  inner join [KasabaKIAT_N1].dbo.tblSprav_Face sprvface  on  face.ID_Osnov = sprvface.ID
  inner join [KasabaKIAT_N1].dbo.tblSprav_Os Osnov   on  face.Stav_Vid = Osnov.ID
  inner join [KasabaKIAT_N1].dbo.tblSprav_San San2   on  FMed.Med_San = San2.ID
  --OplataPutyovok
  inner join [KasabaKIAT_N1].dbo.tblOplata_Ras OplRas on postDtl.Num_Put = OplRas.Num_Put AND postDtl.ID_Sanat = OplRas.ID_Sanat
  inner join [KasabaKIAT_N1].dbo.tblOplata Oplata	 on  OplRas.ID_Opl = Oplata.ID
  --Vozvrat
  left outer join [KasabaKIAT_N1].dbo.tblVozVrat  vozvrat   on postDtl.Num_Put = vozvrat.Num_Put AND postDtl.ID_Sanat = vozvrat.ID_Sanat
  inner join [KasabaKIAT_N1].dbo.tblSprav_Org Org3   on  vozvrat.ID_Pol  = Org3.ID
  inner join [KasabaKIAT_N1].dbo.tblSprav_Org Org4   on  vozvrat.ID_Ot = Org4.ID
  
  WHERE postMain.Data_Post between @beginDate AND @endDate
  AND   Org1.ID = CASE WHEN @ID_ORG = 0 THEN Org1.ID ELSE @ID_ORG END
  AND   San1.ID = CASE WHEN @ID_SANAT = 0 THEN San1.ID ELSE @ID_SANAT END
  AND   face.LastName LIKE '%' + @LASTNAME + '%'
  AND   postDtl.Num_Put LIKE '%' + @NUM_PUT + '%'
  AND   face.Pol = CASE WHEN @POL = -1 THEN face.Pol ELSE @POL END
  AND   postMain.DepartmentBase = @DepartmentBase
  AND   ViplMain.DepartmentBase = @DepartmentBase
  AND   Org1.DepartmentBase = @DepartmentBase
  AND   Oplata.DepartmentBase = @DepartmentBase
  AND   vozvrat.DepartmentBase = @DepartmentBase
  ;
 
end

           
---------------------------------------------------------------------------------------------------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_ZayavkiList]    Script Date: 05/07/2018 16:23:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_OperativkaPutyovki]
@beginDate datetime,
@endDate   datetime,
@DepartmentBase varchar(255)
as 
begin 
set nocount on;



select nachaloPrixod.SanatID,  
	   nachaloPrixod.SanatoryName,
	   nachaloPrixod.Kolichestvo - nachaloRasxod.Kolichestvo as OstatokKol,
	   nachaloPrixod.Symma - nachaloRasxod.Symma as OstatokSymma,
	   prixodPeriod.Kolichestvo as PrixodKol,
	   prixodPeriod.Symma       as PrixodSumma,
	   rasxodPeriod.Kolichestvo as RasxodKol,
	   rasxodPeriod.Symma		as RasxodSumma,
	   vozvratPeriod.Kolichestvo as VozvratKol,
	   vozvratPeriod.Symma       as VozvRatSumma,
	   konesPrixod.Kolichestvo - konesRasxod.Kolichestvo as OstatokKolKones,
	   konesPrixod.Symma - konesRasxod.Symma as  OstatokSymmaKones      
from 
(select San.ID as SanatID, San.Name_San as SanatoryName,
	   ISNULL(COUNT(post.ID_Sanat), 0) as Kolichestvo,
	   ISNULL(SUM(post.Symma_Put), 0)  as Symma
from KasabaKIAT_N1.dbo.tblSprav_San San left outer join
KasabaKIAT_N1.dbo.tblPos_Put post on San.ID = post.ID_Sanat
where CAST(post.Data_Post as DATE) <= CAST(@beginDate as DATE)
AND   post.DepartmentBase = @DepartmentBase
group by San.ID , San.Name_San) nachaloPrixod
inner join 
(select San.ID as SanatID, San.Name_San as SanatoryName,
	   ISNULL(COUNT(vipl.ID_Sanat), 0) as Kolichestvo,
	   ISNULL(SUM(vipl.Symma), 0)  as Symma
from KasabaKIAT_N1.dbo.tblSprav_San San left outer join
KasabaKIAT_N1.dbo.tblVipl_Put vipl on San.ID = vipl.ID_Sanat
where CAST(vipl.Data_Post as DATE) <= CAST(@beginDate as DATE)
AND   vipl.DepartmentBase = @DepartmentBase
group by San.ID , San.Name_San) nachaloRasxod 
on nachaloPrixod.SanatID = nachaloRasxod.SanatID
inner join 
(select San.ID as SanatID, San.Name_San as SanatoryName,
	   COUNT(post.ID_Sanat) as Kolichestvo,
	   SUM(ISNULL(post.Symma_Put, 0))  as Symma
from KasabaKIAT_N1.dbo.tblSprav_San San left outer join
KasabaKIAT_N1.dbo.tblPos_Put post on San.ID = post.ID_Sanat
where CAST(post.Data_Post as DATE) 
between CAST(@beginDate as DATE) AND  CAST(@endDate as DATE)
AND   post.DepartmentBase = @DepartmentBase
group by San.ID , San.Name_San) prixodPeriod
on nachaloPrixod.SanatID = prixodPeriod.SanatID 
inner join 
(select San.ID as SanatID, San.Name_San as SanatoryName,
	   COUNT(vipl.ID_Sanat) as Kolichestvo,
	   SUM(vipl.Symma)  as Symma
from KasabaKIAT_N1.dbo.tblSprav_San San left outer join
KasabaKIAT_N1.dbo.tblVipl_Put vipl on San.ID = vipl.ID_Sanat
where CAST(vipl.Data_Post as DATE) 
between CAST(@beginDate as DATE) AND  CAST(@endDate as DATE)
AND   vipl.DepartmentBase = @DepartmentBase
group by San.ID , San.Name_San) rasxodPeriod
on nachaloPrixod.SanatID = rasxodPeriod.SanatID
inner join 
(select San.ID as SanatID, San.Name_San as SanatoryName,
	   COUNT(vozv.ID_Sanat) as Kolichestvo,
	   SUM(vozv.SymmaP)  as Symma
from KasabaKIAT_N1.dbo.tblSprav_San San left outer join
KasabaKIAT_N1.dbo.tblVozVrat vozv on San.ID = vozv.ID_Sanat
where CAST(vozv.Data_Post as DATE) between CAST(@beginDate as DATE) 
AND  CAST(@endDate as DATE)
AND   vozv.DepartmentBase = @DepartmentBase
group by San.ID , San.Name_San) vozvratPeriod
on nachaloPrixod.SanatID = vozvratPeriod.SanatID
inner join 
(select San.ID as SanatID, San.Name_San as SanatoryName,
	   COUNT(post.ID_Sanat) as Kolichestvo,
	   SUM(post.Symma_Put)  as Symma
from KasabaKIAT_N1.dbo.tblSprav_San San left outer join
KasabaKIAT_N1.dbo.tblPos_Put post on San.ID = post.ID_Sanat
where CAST(post.Data_Post as DATE) <= CAST(@endDate as DATE)
AND   post.DepartmentBase = @DepartmentBase
group by San.ID , San.Name_San) konesPrixod
on nachaloPrixod.SanatID = konesPrixod.SanatID
inner join 
(select San.ID as SanatID, San.Name_San as SanatoryName,
	   COUNT(vipl.ID_Sanat) as Kolichestvo,
	   SUM(vipl.Symma)  as Symma
from KasabaKIAT_N1.dbo.tblSprav_San San left outer join
KasabaKIAT_N1.dbo.tblVipl_Put vipl on San.ID = vipl.ID_Sanat
where CAST(vipl.Data_Post as DATE) <= CAST(@endDate as DATE)
AND   vipl.DepartmentBase = @DepartmentBase
group by San.ID , San.Name_San) konesRasxod
on nachaloPrixod.SanatID = konesPrixod.SanatID;  
  
end

-----------------------------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_ZayavkiList]    Script Date: 05/07/2018 16:23:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_OperativkaPutyovkiPrixod]
@beginDate datetime,
@endDate   datetime,
@SanatID   int,
@DepartmentBase varchar(255)
as 
begin 
set nocount on;

select  postMain.Data_Post,
		postMain.Num_Doc,
	    post.Num_Put,
	    Org.Name_Org,
	    post.Symma_Put
from KasabaKIAT_N1.dbo.tblSprav_San San inner join
KasabaKIAT_N1.dbo.tblPos_Put post on San.ID = post.ID_Sanat
inner join KasabaKIAT_N1.dbo.tblPost postMain on post.ID_Post = postMain.ID
inner join KasabaKIAT_N1.dbo.tblSprav_Org Org on postMain.ID_Pos = Org.ID
where CAST(post.Data_Post as DATE) 
between CAST(@beginDate as DATE) AND  CAST(@endDate as DATE)
AND post.ID_Sanat = @SanatID
AND postMain.DepartmentBase = @DepartmentBase; 
  
end
----------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_ZayavkiList]    Script Date: 05/07/2018 16:23:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_OperativkaPutyovkiRasxod]
@beginDate datetime,
@endDate   datetime,
@SanatID   int,
@DepartmentBase varchar(255)
as 
begin 
set nocount on;

select  postMain.Data_Post,
		postMain.Num_Doc,
	    post.Num_Put,
	    Org.Name_Org,
	    post.Symma
from KasabaKIAT_N1.dbo.tblSprav_San San inner join
KasabaKIAT_N1.dbo.tblVipl_Put post on San.ID = post.ID_Sanat
inner join KasabaKIAT_N1.dbo.tblVipl postMain on post.ID_Vipl = postMain.ID
inner join KasabaKIAT_N1.dbo.tblSprav_Org Org on postMain.ID_Org = Org.ID
where CAST(post.Data_Post as DATE) 
between CAST(@beginDate as DATE) AND  CAST(@endDate as DATE)
AND post.ID_Sanat = @SanatID
AND postMain.DepartmentBase = @DepartmentBase; 
  
end

------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_ZayavkiList]    Script Date: 05/07/2018 16:23:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_OperativkaPutyovkiVozvrat]
@beginDate datetime,
@endDate   datetime,
@SanatID   int,
@DepartmentBase varchar(255)
as 
begin 
set nocount on;

select  post.Data_Post,
		post.Num_Doc,
	    post.Num_Put,
	    Org.Name_Org,
	    post.SymmaP
from KasabaKIAT_N1.dbo.tblSprav_San San inner join
KasabaKIAT_N1.dbo.tblVozVrat post on San.ID = post.ID_Sanat
inner join KasabaKIAT_N1.dbo.tblSprav_Org Org on post.ID_Ot = Org.ID
where CAST(post.Data_Post as DATE) 
between CAST(@beginDate as DATE) AND  CAST(@endDate as DATE)
AND post.ID_Sanat = @SanatID
AND post.DepartmentBase = @DepartmentBase;
  
end

---------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_ZayavkiList]    Script Date: 05/07/2018 16:23:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_OperativkaPutyovkiOstatok]
@beginDate datetime,
@endDate   datetime,
@SanatID   int,
@DepartmentBase varchar(255)
as 
begin 
set nocount on;

select  postMain.Data_Post,
		postMain.Num_Doc,
	    post.Num_Put,
	    Org.Name_Org,
	    post.Symma_Put
from KasabaKIAT_N1.dbo.tblSprav_San San inner join
KasabaKIAT_N1.dbo.tblPos_Put post on San.ID = post.ID_Sanat
inner join KasabaKIAT_N1.dbo.tblPost postMain on post.ID_Post = postMain.ID
inner join KasabaKIAT_N1.dbo.tblSprav_Org Org on postMain.ID_Pos = Org.ID
where CAST(post.Data_Post as DATE) 
between CAST(@beginDate as DATE) AND  CAST(@endDate as DATE)
AND post.ID_Sanat = @SanatID
AND postMain.DepartmentBase = @DepartmentBase
AND NOT EXISTS (
select  1
from KasabaKIAT_N1.dbo.tblSprav_San San1 inner join
KasabaKIAT_N1.dbo.tblVipl_Put post1 on San.ID = post.ID_Sanat
inner join KasabaKIAT_N1.dbo.tblVipl postMain1 on post1.ID_Vipl = postMain1.ID
inner join KasabaKIAT_N1.dbo.tblSprav_Org Org1 on postMain1.ID_Org = Org1.ID
where 
post.Num_Put = post1.Num_Put
AND post.ID_Sanat = post1.ID_Sanat
AND CAST(post.Data_Post as DATE) 
between CAST(@beginDate as DATE) AND  CAST(@endDate as DATE)
AND post.ID_Sanat = @SanatID);
  
end

------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_SvodniyPoPutyovkam]    Script Date: 05/07/2018 20:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_SvodniyPoPutyovkam]
@Year int,
@Kvartal   int,
@Otras   int,
@DepartmentBase varchar(255)
as 
begin 
set nocount on;

select San.Name_San,
	   Otras.Name_Ot,
	   Org.Name_Org,
	   Ras.Symma,
	   Ras.Priz
from KasabaKIAT_N1.dbo.tblOplata OplMain
inner join KasabaKIAT_N1.dbo.tblOplata_Ras Ras
on OplMain.ID = Ras.ID_Opl
inner join KasabaKIAT_N1.dbo.tblSprav_Org Org
on OplMain.ID_Otras = Org.ID
inner join KasabaKIAT_N1.dbo.tblSprav_Otras Otras
on Org.ID_Ot = Otras.ID_Ot
inner join KasabaKIAT_N1.dbo.tblSprav_San San
on Ras.ID_Sanat = San.ID
where OplMain.SmFil = @Year
AND   OplMain.ID_KV = @Kvartal
AND   Otras.ID_Ot = @Otras
AND   OplMain.DepartmentBase = @DepartmentBase;
  
end

-----------------------------------------------------------

---Kartochka Organizasii

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_OperativkaPutyovkiOstatok]    Script Date: 05/08/2018 08:54:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_KartochkaOrganizasii]
@OrgInn bigint,
@DepartmentBase varchar(255)
as 
begin 
set nocount on;

select Org.Name_Org,
	   Reg.NameReg,
	   Org.MFO,
	   Org.Adress,
	   Org.Telefon,
	   Org.RS,
	   Org.DT_OST,
	   Org.KT_OST
from KasabaKIAT_N1.dbo.tblSprav_Org Org
inner join KasabaKIAT_N1.dbo.tblSprav_Reg Reg
on Org.Region = Reg.CodeReg
where  Org.INN LIKE '%' + @OrgInn
AND Org.DepartmentBase = @DepartmentBase;
  
end


--------------------------------------------------------
--Rasxod

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_OperativkaPutyovkiOstatok]    Script Date: 05/08/2018 08:54:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_KartochkaOrganizasiiRasxod]
@IdOrg bigint,
@beginDate datetime,
@endDate datetime,
@DepartmentBase varchar(255)
as 
begin 
set nocount on;

select 
		vipl.ID_Org, 
		COUNT(vipl.ID_Org) as KolVo,  
		SUM(vipl.Symma) as NaSummu
from  KasabaKIAT_N1.dbo.tblVipl vipl
where CAST(vipl.Data_Post as DATE) BETWEEN 
 CAST(@beginDate as DATE) AND CAST(@endDate as DATE)
AND  vipl.ID_Org = @IdOrg
AND  vipl.DepartmentBase = @DepartmentBase
GROUP BY vipl.ID_Org;

end
---------------------------------------------------------
--Oplata

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_OperativkaPutyovkiOstatok]    Script Date: 05/08/2018 08:54:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_KartochkaOrganizasiiOplata]
@IdOrg bigint,
@beginDate datetime,
@endDate datetime,
@DepartmentBase varchar(255)
as 
begin 
set nocount on;

select opl.ID_Otras,
       COUNT(opl.ID_Otras) as KolVo, 
       SUM(opl.Symma_KT) as NaSummu  
from KasabaKIAT_N1.dbo.tblOplata opl
where CAST(opl.Data_Post as DATE) BETWEEN 
 CAST(@beginDate as DATE) AND CAST(@endDate as DATE)
AND  opl.ID_Otras = @IdOrg
AND  opl.PrizOpl = 1
AND  opl.DepartmentBase = @DepartmentBase
GROUP BY opl.ID_Otras;

end
------------------------------------------------------------
--Vozvrat
USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_OperativkaPutyovkiOstatok]    Script Date: 05/08/2018 08:54:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_KartochkaOrganizasiiVozvrat]
@IdOrg bigint,
@beginDate datetime,
@endDate datetime,
@DepartmentBase varchar(255)
as 
begin 
set nocount on;

select vozv.ID_Ot,  
       COUNT(vozv.ID_Ot) as KolVo, 
       SUM(vozv.SymmaP) as NaSummu
from KasabaKIAT_N1.dbo.tblVozVrat vozv
where CAST(vozv.Data_Post as DATE) BETWEEN 
 CAST(@beginDate as DATE) AND CAST(@endDate as DATE)
AND  vozv.ID_Ot = @IdOrg
AND  vozv.DepartmentBase = @DepartmentBase
GROUP BY vozv.ID_Ot, vozv.Num_Doc , vozv.Data_Post;

end
------------------------------------------------------------------
--Razbivka po Procentam oplat za putyovki

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_OperativkaPutyovkiOstatok]    Script Date: 05/08/2018 08:54:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_KartochkaOrganizasiiProcentRazbivka]
@IdOrg bigint,
@beginDate datetime,
@endDate datetime,
@DepartmentBase varchar(255)
as 
begin 
set nocount on;

select Pr.Name_Proc , 
	   COUNT(Pr.ID) as KolVo, 
	   SUM(vipl.Symma) as NaSummu
from KasabaKIAT.dbo.tblSprav_Pr Pr
left outer join  KasabaKIAT.dbo.tblVipl_Put vipl
on vipl.Num_Proc = Pr.ID
inner join KasabaKIAT.dbo.tblVipl ViplMain
on ViplMain.ID = vipl.ID_Vipl
where CAST(ViplMain.Data_Post as DATE) BETWEEN 
 CAST(@beginDate as DATE) AND CAST(@endDate as DATE)
AND  ViplMain.ID_Org = @IdOrg
AND  ViplMain.DepartmentBase = @DepartmentBase
GROUP BY Pr.Name_Proc

end
-----------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_OperativkaPutyovkiOstatok]    Script Date: 05/08/2018 08:54:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_KartochkaOrganizasiiHistory]
@IdOrg bigint,
@beginDate datetime,
@endDate datetime,
@DepartmentBase varchar(255)
as 
begin 
set nocount on;

SELECT AA.TipDokumenta,
	   AA.DataDocumenta,
	   AA.DocumentNumber,
	   AA.Doverennost,
	   AA.Summa,
	   AA.UniQueId
FROM 
(select 'Проход' as TipDokumenta,
	   post.Data_Post as DataDocumenta,
	   post.Num_Doc   as DocumentNumber,
	   post.Name_Dov  as Doverennost,
	   post.Symma     as Summa,
	   post.ID        as UniQueId
from KasabaKIAT.dbo.tblPost post
inner join KasabaKIAT.dbo.tblSprav_Org Org
on post.ID_Pos = Org.ID
where post.ID_Pos = @IdOrg
AND post.DepartmentBase = @DepartmentBase
AND CAST(post.Data_Post as DATE) BETWEEN 
CAST(@beginDate as DATE) AND CAST(@endDate as DATE)

UNION 

select 'Накладная' as TipDokumenta,
	   post.Data_Post as DataDocumenta,
	   post.Num_Doc   as DocumentNumber,
	   post.Name_Dov  as Doverennost,
	   post.Symma     as Summa,
	   post.ID        as UniQueId
from KasabaKIAT.dbo.tblVipl post
inner join KasabaKIAT.dbo.tblSprav_Org Org
on post.ID_Org = Org.ID
where post.ID_Org = @IdOrg
AND post.DepartmentBase = @DepartmentBase
AND CAST(post.Data_Post as DATE) BETWEEN 
CAST(@beginDate as DATE) AND CAST(@endDate as DATE)

UNION


select 'Оплата' as TipDokumenta,
	   post.Data_Post as DataDocumenta,
	   post.Num_Doc   as DocumentNumber,
	   post.Osnov  as Doverennost,
	   post.Symma_KT     as Summa,
	   post.ID        as UniQueId
from KasabaKIAT.dbo.tblOplata post
inner join KasabaKIAT.dbo.tblSprav_Org Org
on post.ID_Otras = Org.ID
where post.ID_Otras = @IdOrg
AND post.DepartmentBase = @DepartmentBase
AND CAST(post.Data_Post as DATE) BETWEEN 
CAST(@beginDate as DATE) AND CAST(@endDate as DATE)
AND  post.PrizOpl = 1

UNION


select 'Возврат' as TipDokumenta,
	   post.Data_Post as DataDocumenta,
	   post.Num_Doc   as DocumentNumber,
	   post.Dover  as Doverennost,
	   post.SymmaP     as Summa,
	   post.ID        as UniQueId
from KasabaKIAT.dbo.tblVozVrat post
inner join KasabaKIAT.dbo.tblSprav_Org Org
on post.ID_Ot = Org.ID
where post.ID_Ot = @IdOrg
AND post.DepartmentBase = @DepartmentBase
AND CAST(post.Data_Post as DATE) BETWEEN 
CAST(@beginDate as DATE) AND CAST(@endDate as DATE)) AA


end

-------------------------------------------------------------


--Prixod
USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_OperativkaPutyovkiOstatok]    Script Date: 05/08/2018 08:54:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_KartochkaOrganizasiiHistoriDtlPrixod]
@IdUnique bigint,
@DepartmentBase varchar(255)
as 
begin 
set nocount on;

select put.Data_Post , 
       San.Name_San ,
	   put.Num_Put
from KasabaKIAT.dbo.tblPos_Put put
inner join KasabaKIAT.dbo.tblSprav_San San
on put.ID_Sanat = San.ID
where put.ID_Post = @IdUnique
AND put.DepartmentBase = @DepartmentBase;
end

-----------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_OperativkaPutyovkiOstatok]    Script Date: 05/08/2018 08:54:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_KartochkaOrganizasiiHistoriDtlRasxod]
@IdUnique bigint,
@DepartmentBase varchar(255)
as 
begin 
set nocount on;

--Rasxod
select put.Data_Post , 
       San.Name_San ,
	   put.Num_Put
from KasabaKIAT.dbo.tblVipl_Put put
inner join KasabaKIAT.dbo.tblSprav_San San
on put.ID_Sanat = San.ID
where put.ID_Vipl = @IdUnique
AND put.DepartmentBase = @DepartmentBase;
end


------------------------------------------------------------------
USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_OperativkaPutyovkiOstatok]    Script Date: 05/08/2018 08:54:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_KartochkaOrganizasiiHistoriDtlVozvrat]
@IdUnique bigint,
@DepartmentBase varchar(255)
as 
begin 
set nocount on;

--Oplata
select GETDATE() as Data_Post , 
       San.Name_San ,
	   put.Num_Put
from KasabaKIAT.dbo.tblOplata_Ras put
inner join KasabaKIAT.dbo.tblSprav_San San
on put.ID_Sanat = San.ID
where put.ID_Opl = @IdUnique
AND put.DepartmentBase = @DepartmentBase;

end
--Kones Kartochki organizasii

----------------------------------------------------------------------------------
--Balanci Predpriyatiy


USE [KasabaKIAT_N1]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_BalanciPredpriyatiy]
@beginDate datetime,
@endDate datetime,
@DepartmentBase varchar(255)
as 
begin 
set nocount on;

select Nachalo.Name_Org ,  
       Nachalo.DebitNachalo,
       Nachalo.KreditNachalo,
       Oborot.DebitOborot,
       Oborot.KreditOborot,
       Kones.DebitKones,
       Kones.KreditKones
from 
(select Org.ID , Org.Name_Org,
	   case when ISNULL(SUM(Opl.Symma_DT), 0) > ISNULL(SUM(Opl.Symma_KT), 0) 
	   then ISNULL(SUM(Opl.Symma_DT), 0) - ISNULL(SUM(Opl.Symma_KT), 0)  else 0 end DebitNachalo,
	   case when ISNULL(SUM(Opl.Symma_DT), 0) < ISNULL(SUM(Opl.Symma_KT), 0) 
	   then ISNULL(SUM(Opl.Symma_KT), 0) - ISNULL(SUM(Opl.Symma_DT), 0)   else 0 end KreditNachalo	   
from KasabaKIAT_N1.dbo.tblOplata Opl
inner join  KasabaKIAT_N1.dbo.tblSprav_Org Org
on Opl.ID_Otras = Org.ID
where CAST(Opl.Data_Post as DATE) <  CAST(@beginDate as DATE)
AND   Opl.DepartmentBase = @DepartmentBase
group by Org.ID , Org.Name_Org) Nachalo
inner join 
(select Org.ID , Org.Name_Org,
	   ISNULL(SUM(Opl.Symma_DT), 0) as DebitOborot,
	   ISNULL(SUM(Opl.Symma_KT), 0) as KreditOborot
from KasabaKIAT_N1.dbo.tblOplata Opl
inner join  KasabaKIAT_N1.dbo.tblSprav_Org Org
on Opl.ID_Otras = Org.ID
where CAST(Opl.Data_Post as DATE) between  CAST(@beginDate as DATE) AND CAST(@endDate as DATE)
AND   Opl.DepartmentBase = @DepartmentBase
group by Org.ID , Org.Name_Org) Oborot on Nachalo.ID = Oborot.ID
inner join 
(select Org.ID , Org.Name_Org,
	   case when ISNULL(SUM(Opl.Symma_DT), 0) > ISNULL(SUM(Opl.Symma_KT), 0) 
	   then ISNULL(SUM(Opl.Symma_DT), 0) - ISNULL(SUM(Opl.Symma_KT), 0)  else 0 end DebitKones,
	   case when ISNULL(SUM(Opl.Symma_DT), 0) < ISNULL(SUM(Opl.Symma_KT), 0) 
	   then ISNULL(SUM(Opl.Symma_KT), 0) - ISNULL(SUM(Opl.Symma_DT), 0)   else 0 end KreditKones	   
from KasabaKIAT_N1.dbo.tblOplata Opl
inner join  KasabaKIAT_N1.dbo.tblSprav_Org Org
on Opl.ID_Otras = Org.ID
where CAST(Opl.Data_Post as DATE) <=  CAST(@endDate as DATE)
AND   Opl.DepartmentBase = @DepartmentBase
group by Org.ID , Org.Name_Org) Kones on Nachalo.ID = Kones.ID;

end

--------------------------------------------------------------------------------
-- Otchet
--------------------------------------------------------------------------------
--
--Uchet Medisinskoy Registrasii Rabotnikov
---------------------------------------------------------------------------------
USE [KasabaKIAT_N1]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_OtchetMedRegRabotnikov]
@IdOrg     bigint,
@beginDate datetime,
@endDate datetime,
@DepartmentBase varchar(255)
as 
begin 
set nocount on;

select  face.FirstName 
      , face.Name 
      , face.LastName 
      , face.Adress 
      , face.Pas_ser 
      , face.Pas_Num
      , face.Pas_Data
      , face.Pas_KD
      , fMed.Med_Doc
      , fMed.Med_Data
      , Sanat.Name_San
from KasabaKIAT_N1.dbo.tblFace_Med fMed
inner join KasabaKIAT_N1.dbo.tblFase face 
on fMed.ID_Face = face.ID
inner join KasabaKIAT_N1.dbo.tblVipl_Put ViplPut
on face.ID_Put = ViplPut.ID
inner join KasabaKIAT_N1.dbo.tblVipl ViplMain
on ViplPut.ID_Vipl = ViplMain.ID
inner join KasabaKIAT_N1.dbo.tblSprav_San Sanat
on fMed.Med_San = Sanat.ID
where CAST(ViplMain.Data_Post as DATE) 
between  CAST(@beginDate as DATE) AND CAST(@endDate as DATE)
AND ViplMain.ID_Org = @IdOrg
AND ViplMain.DepartmentBase = @DepartmentBase;

end

------*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
----- Uchet Postupleniya Stoimosti Putyovok
------*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

USE [KasabaKIAT_N1]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_OtchetUchetStoimostiVidachi]
@beginDate datetime,
@endDate datetime,
@DepartmentBase varchar(255)
as 
begin 
set nocount on;

SELECT CC.Num_Doc,
       CC.Data_Post,
       CC.VidanoPutyovokKolVo,
       CC.VidanoPutyovokSumma,
       DD.PrBPKolVo,
       DD.PrBPSumma,
       DD.Pr45KolVo,
       DD.Pr100KolVo,
       DD.Pr100Summa,
       DD.DoljnoPostupit
FROM 
(select OplMain.Num_Doc ,
	   OplMain.Data_Post,
	   COUNT(OplDtl.Num_Put) as VidanoPutyovokKolVo,
	   SUM(OplDtl.Symma)     as VidanoPutyovokSumma
from 
KasabaKIAT_N1.dbo.tblOplata OplMain
inner join KasabaKIAT_N1.dbo.tblOplata_Ras OplDtl
on OplMain.ID = OplDtl.ID_Opl
where OplMain.Departmentbase = @DepartmentBase
AND CAST(OplMain.Data_Post AS DATE) BETWEEN CAST(@beginDate AS DATE) AND CAST(@endDate AS DATE)
AND OplDtl.Priz = 2
group by OplMain.Num_Doc ,
	   OplMain.Data_Post) CC
inner join 
(SELECT BB.Num_Doc , 
       BB.Data_Post,
       SUM(BB.PrBPKolVo) as  PrBPKolVo,
       SUM(BB.PrBPSumma) as  PrBPSumma,
       
       SUM(BB.Pr45KolVo) as  Pr45KolVo,
       SUM(BB.Pr45Summa) as  Pr45Summa,
       
       SUM(BB.Pr100KolVo) as  Pr100KolVo,
       SUM(BB.Pr100Summa) as  Pr100Summa,
       
       (SUM(BB.Pr45Summa) * 45 /100) + SUM(BB.Pr100Summa) As DoljnoPostupit
FROM
(select AA.Num_Doc,
       AA.Data_Post,
       CASE WHEN  AA.ID_Pr = 1 THEN AA.KolVo ELSE 0 END As Pr45KolVo,
       CASE WHEN  AA.ID_Pr = 1 THEN AA.Summa ELSE 0 END As Pr45Summa,

       CASE WHEN  AA.ID_Pr = 2 THEN AA.KolVo ELSE 0 END As Pr100KolVo,
       CASE WHEN  AA.ID_Pr = 2 THEN AA.Summa ELSE 0 END As Pr100Summa,
       
       CASE WHEN  AA.ID_Pr = 3 THEN AA.KolVo ELSE 0 END As PrBPKolVo,
       CASE WHEN  AA.ID_Pr = 3 THEN AA.Summa ELSE 0 END As PrBPSumma     
from 
(select OplMain.Num_Doc ,
	   OplMain.Data_Post,
	    OplDtl.ID_Pr,
	    ISNULL(COUNT(OplDtl.ID_Pr),0) as KolVo,
	    ISNULL(SUM(OplDtl.Symma),0)   as Summa
from KasabaKIAT_N1.dbo.tblOplata OplMain
inner join KasabaKIAT_N1.dbo.tblOplata_Ras OplDtl
on OplMain.ID = OplDtl.ID_Opl
where OplMain.Departmentbase = @DepartmentBase
AND OplDtl.Priz = 2
AND CAST(OplMain.Data_Post AS DATE) BETWEEN CAST(@beginDate AS DATE) AND CAST(@endDate AS DATE)
group by OplMain.Num_Doc ,
	   OplMain.Data_Post,
	    OplDtl.ID_Pr
) AA) BB
 GROUP BY BB.Num_Doc , 
       BB.Data_Post) DD
  on CC.Num_Doc = DD.Num_Doc AND CC.Data_Post = DD.Data_Post;

end



--------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_FormaN13]    Script Date: 05/08/2018 22:40:26 ******/
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
AND   YEAR(a.Data_Post) = @aYear
AND   a.DepartmentBase = @DepartmentBase
AND   d.DepartmentBase = @DepartmentBase;
  
end


-------------------------------------------------------------------------------------
USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_FormaN13]    Script Date: 05/08/2018 22:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_FormaN14]
@beginDate datetime,
@endDate  datetime,
@DepartmentBase varchar(255)

as 
begin 
set nocount on;

select nachaloPrixod.SanatID as Col_0,  
	   nachaloPrixod.SanatoryName as Col_1,
	   nachaloPrixod.Kolichestvo - nachaloRasxod.Kolichestvo as OstatokKol_2,
	   nachaloPrixod.Symma - nachaloRasxod.Symma as OstatokSymma_3,
	   prixodPeriod.Kolichestvo as PrixodKol_4,
	   prixodPeriod.Symma       as PrixodSumma_5,
	   0 as Col_6,
	   0 as Col_7,
	   rasxodPeriod.Kolichestvo as RasxodKol_8,
	   rasxodPeriod.Symma		as RasxodSumma_9,
	   rasxodPeriod.Kolichestvo as RasxodKol_10,
	   rasxodPeriod.Symma		as RasxodSumma_11,	   
	   0 as Col_12,
	   0 as Col_13,
	   0 as Col_14,
	   0 as Col_15,   
	   0 as Col_16,
	   0 as Col_17,  
	   0 as Col_18,
	   0 as Col_19,
	   rasxodPeriod.Kolichestvo as RasxodKol_20,
	   rasxodPeriod.Symma		as RasxodSumma_21,	
	   0 as Col_22,
	   0 as Col_23,
	   0 as Col_24,
	   0 as Col_25,
	   vozvratPeriod.Kolichestvo as VozvratKol_26,
	   vozvratPeriod.Symma       as VozvRatSumma_27,
	   konesPrixod.Kolichestvo - konesRasxod.Kolichestvo as OstatokKolKones_28,
	   konesPrixod.Symma - konesRasxod.Symma as  OstatokSymmaKones_29      
from 
(select San.ID as SanatID, San.Name_San as SanatoryName,
	   ISNULL(COUNT(post.ID_Sanat), 0) as Kolichestvo,
	   ISNULL(SUM(post.Symma_Put), 0)  as Symma
from KasabaKIAT_N1.dbo.tblSprav_San San left outer join
KasabaKIAT_N1.dbo.tblPos_Put post on San.ID = post.ID_Sanat
where CAST(post.Data_Post as DATE) <= CAST(@beginDate as DATE)
AND   post.DepartmentBase = @DepartmentBase
group by San.ID , San.Name_San) nachaloPrixod
inner join 
(select San.ID as SanatID, San.Name_San as SanatoryName,
	   ISNULL(COUNT(vipl.ID_Sanat), 0) as Kolichestvo,
	   ISNULL(SUM(vipl.Symma), 0)  as Symma
from KasabaKIAT_N1.dbo.tblSprav_San San left outer join
KasabaKIAT_N1.dbo.tblVipl_Put vipl on San.ID = vipl.ID_Sanat
where CAST(vipl.Data_Post as DATE) <= CAST(@beginDate as DATE)
AND   vipl.DepartmentBase = @DepartmentBase
group by San.ID , San.Name_San) nachaloRasxod 
on nachaloPrixod.SanatID = nachaloRasxod.SanatID
inner join 
(select San.ID as SanatID, San.Name_San as SanatoryName,
	   COUNT(post.ID_Sanat) as Kolichestvo,
	   SUM(ISNULL(post.Symma_Put, 0))  as Symma
from KasabaKIAT_N1.dbo.tblSprav_San San left outer join
KasabaKIAT_N1.dbo.tblPos_Put post on San.ID = post.ID_Sanat
where CAST(post.Data_Post as DATE) 
between CAST(@beginDate as DATE) AND  CAST(@endDate as DATE)
AND   post.DepartmentBase = @DepartmentBase
group by San.ID , San.Name_San) prixodPeriod
on nachaloPrixod.SanatID = prixodPeriod.SanatID 
inner join 
(select San.ID as SanatID, San.Name_San as SanatoryName,
	   COUNT(vipl.ID_Sanat) as Kolichestvo,
	   SUM(vipl.Symma)  as Symma
from KasabaKIAT_N1.dbo.tblSprav_San San left outer join
KasabaKIAT_N1.dbo.tblVipl_Put vipl on San.ID = vipl.ID_Sanat
where CAST(vipl.Data_Post as DATE) 
between CAST(@beginDate as DATE) AND  CAST(@endDate as DATE)
AND   vipl.DepartmentBase = @DepartmentBase
group by San.ID , San.Name_San) rasxodPeriod
on nachaloPrixod.SanatID = rasxodPeriod.SanatID
inner join 
(select San.ID as SanatID, San.Name_San as SanatoryName,
	   COUNT(vozv.ID_Sanat) as Kolichestvo,
	   SUM(vozv.SymmaP)  as Symma
from KasabaKIAT_N1.dbo.tblSprav_San San left outer join
KasabaKIAT_N1.dbo.tblVozVrat vozv on San.ID = vozv.ID_Sanat
where CAST(vozv.Data_Post as DATE) between CAST(@beginDate as DATE) 
AND  CAST(@endDate as DATE)
AND   vozv.DepartmentBase = @DepartmentBase
group by San.ID , San.Name_San) vozvratPeriod
on nachaloPrixod.SanatID = vozvratPeriod.SanatID
inner join 
(select San.ID as SanatID, San.Name_San as SanatoryName,
	   COUNT(post.ID_Sanat) as Kolichestvo,
	   SUM(post.Symma_Put)  as Symma
from KasabaKIAT_N1.dbo.tblSprav_San San left outer join
KasabaKIAT_N1.dbo.tblPos_Put post on San.ID = post.ID_Sanat
where CAST(post.Data_Post as DATE) <= CAST(@endDate as DATE)
AND   post.DepartmentBase = @DepartmentBase
group by San.ID , San.Name_San) konesPrixod
on nachaloPrixod.SanatID = konesPrixod.SanatID
inner join 
(select San.ID as SanatID, San.Name_San as SanatoryName,
	   COUNT(vipl.ID_Sanat) as Kolichestvo,
	   SUM(vipl.Symma)  as Symma
from KasabaKIAT_N1.dbo.tblSprav_San San left outer join
KasabaKIAT_N1.dbo.tblVipl_Put vipl on San.ID = vipl.ID_Sanat
where CAST(vipl.Data_Post as DATE) <= CAST(@endDate as DATE)
AND   vipl.DepartmentBase = @DepartmentBase
group by San.ID , San.Name_San) konesRasxod
on nachaloPrixod.SanatID = konesPrixod.SanatID; 

end


-----------------------------------------------------------------------------------
USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_FormaN13]    Script Date: 05/08/2018 22:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_UchetVladelsevPutyovok]
@beginDate datetime,
@endDate  datetime,
@DepartmentBase varchar(255)

as 
begin 
set nocount on;

select	San.ID,
		San.Name_San,
		viplDtl.Num_Put,
		viplDtl.Symma,
		face.LastName						    as FamiliyaVladelsa,
		face.Name								as ImyaVladelsa,
		face.FirstName						as OtchstvoVladelsa,
		face.Adress							as MestoJitelstva,
		face.Rabota							as MestoRaboti,
		face.Dolj								as Doljnost,
		face.Pas_ser						    as SeriyaPassporta,
		face.Pas_Num							as NomerPassporta,
		face.Pas_KD							as KemVidan,
		face.Pas_Data							as DataVidachi,
		face.Pol								as Pol,
		FMed.Med_Doc                          as MedisinaNomer,
		FMed.Med_Data							as MedisinaDataVidachi,
		FMed.Opl_Doc							as UplataNomer,
		FMed.Opl_Data							as UplataDataDoc,
		FMed.Opl_Symma						as UplataSumma,
		FMed.Tal_Doc							as ObratniyTalon,
		FMed.Tal_Data							as ObratniyTalonData
from KasabaKIAT_N1.dbo.tblVipl viplMain
inner join KasabaKIAT_N1.dbo.tblVipl_Put viplDtl
on viplMain.ID = viplDtl.ID_Vipl
inner join KasabaKIAT_N1.dbo.tblFase face
on face.ID_Put = viplDtl.ID
inner join KasabaKIAT_N1.dbo.tblFace_Med fMed
on face.ID = fMed.ID_Face
inner join KasabaKIAT_N1.dbo.tblSprav_San San
on viplDtl.ID_Sanat = San.ID
WHERE CAST(viplMain.Data_Post as DATE) BETWEEN CAST(@beginDate as DATE) AND CAST(@endDate as DATE)
AND viplMain.DepartmentBase = @departmentBase;

end