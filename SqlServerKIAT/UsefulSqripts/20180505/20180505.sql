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
@POL int
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
  AND   face.Pol = CASE WHEN @POL = -1 THEN face.Pol ELSE @POL END;
  
  
 
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
@endDate   datetime
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
group by San.ID , San.Name_San) nachaloPrixod
inner join 
(select San.ID as SanatID, San.Name_San as SanatoryName,
	   ISNULL(COUNT(vipl.ID_Sanat), 0) as Kolichestvo,
	   ISNULL(SUM(vipl.Symma), 0)  as Symma
from KasabaKIAT_N1.dbo.tblSprav_San San left outer join
KasabaKIAT_N1.dbo.tblVipl_Put vipl on San.ID = vipl.ID_Sanat
where CAST(vipl.Data_Post as DATE) <= CAST(@beginDate as DATE)
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
group by San.ID , San.Name_San) vozvratPeriod
on nachaloPrixod.SanatID = vozvratPeriod.SanatID
inner join 
(select San.ID as SanatID, San.Name_San as SanatoryName,
	   COUNT(post.ID_Sanat) as Kolichestvo,
	   SUM(post.Symma_Put)  as Symma
from KasabaKIAT_N1.dbo.tblSprav_San San left outer join
KasabaKIAT_N1.dbo.tblPos_Put post on San.ID = post.ID_Sanat
where CAST(post.Data_Post as DATE) <= CAST(@endDate as DATE)
group by San.ID , San.Name_San) konesPrixod
on nachaloPrixod.SanatID = konesPrixod.SanatID
inner join 
(select San.ID as SanatID, San.Name_San as SanatoryName,
	   COUNT(vipl.ID_Sanat) as Kolichestvo,
	   SUM(vipl.Symma)  as Symma
from KasabaKIAT_N1.dbo.tblSprav_San San left outer join
KasabaKIAT_N1.dbo.tblVipl_Put vipl on San.ID = vipl.ID_Sanat
where CAST(vipl.Data_Post as DATE) <= CAST(@endDate as DATE)
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
@SanatID   int
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
AND post.ID_Sanat = @SanatID; 
  
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
@SanatID   int
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
AND post.ID_Sanat = @SanatID; 
  
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
@SanatID   int
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
AND post.ID_Sanat = @SanatID;
  
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
@SanatID   int
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
AND NOT EXISTS (
select  1
from KasabaKIAT_N1.dbo.tblSprav_San San1 inner join
KasabaKIAT_N1.dbo.tblVipl_Put post1 on San.ID = post.ID_Sanat
inner join KasabaKIAT_N1.dbo.tblVipl postMain1 on post1.ID_Vipl = postMain.ID
inner join KasabaKIAT_N1.dbo.tblSprav_Org Org1 on postMain1.ID_Org = Org.ID
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
ALTER procedure [dbo].[sp_rt_SvodniyPoPutyovkam]
@Year int,
@Kvartal   int,
@Otras   int
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
AND   Otras.ID_Ot = @Otras;
  
end

-----------------------------------------------------------

---Kartochka Organizasii

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
where  Org.INN LIKE '%' + @OrgInn;

--------------------------------------------------------
--Rasxod
select 
		vipl.ID_Org, 
		COUNT(vipl.ID_Org) as KolVo,  
		SUM(vipl.Symma) as NaSummu
from  KasabaKIAT_N1.dbo.tblVipl vipl
where CAST(vipl.Data_Post as DATE) BETWEEN 
 CAST(@beginDate as DATE) AND CAST(@endDate as DATE)
AND  vipl.ID_Org = @IdOrg
GROUP BY vipl.ID_Org;

---------------------------------------------------------
--Oplata

select opl.ID_Otras,
       COUNT(opl.ID_Otras) as KolVo, 
       SUM(opl.Symma_KT) as NaSummu  
from KasabaKIAT_N1.dbo.tblOplata opl
where CAST(opl.Data_Post as DATE) BETWEEN 
 CAST(@beginDate as DATE) AND CAST(@endDate as DATE)
AND  opl.ID_Otras = @IdOrg
AND  opl.PrizOpl = 1
GROUP BY opl.ID_Otras;

------------------------------------------------------------
--Vozvrat

select vozv.ID_Ot,  
       COUNT(vozv.ID_Ot) as KolVo, 
       SUM(vozv.SymmaP) as NaSummu
from KasabaKIAT_N1.dbo.tblVozVrat vozv
where CAST(vozv.Data_Post as DATE) BETWEEN 
 CAST(@beginDate as DATE) AND CAST(@endDate as DATE)
AND  vozv.ID_Ot = @IdOrg
GROUP BY vozv.ID_Ot, vozv.Num_Doc , vozv.Data_Post;

------------------------------------------------------------------
--Razbivka po Procentam oplat za putyovki
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
GROUP BY Pr.Name_Proc











--Kones Kartochki organizasii