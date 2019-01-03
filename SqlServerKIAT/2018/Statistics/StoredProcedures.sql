SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION dbo.IsMyDependent
(@Parent nvarchar(255), @Child nvarchar(255))
RETURNS bit
AS
BEGIN

	IF ((@Parent IS null) OR (@Parent = '')) RETURN 0; 
	IF ((@Child IS null) OR (@Child = '')) RETURN 0; 

	IF (@Parent = '17') RETURN 1; 
	
	IF ((LEN(@Parent) = LEN(@Child)) AND (@Parent != @Child)) RETURN 0; 
	
	IF ((LEN(@Parent) = LEN(@Child)) AND (@Parent = @Child)) RETURN 1; 
	
	IF ((SUBSTRING(@Parent, 1 , 3) = '172') AND (SUBSTRING(@Child, 1 , 3) = '173') 
	AND (SUBSTRING(@Parent, 4 , 2) = SUBSTRING(@Child, 6 , 2))) RETURN 1; 

		
	IF ((SUBSTRING(@Parent, 1 , 3) = '173') AND (SUBSTRING(@Child, 1 , 3) = '173') 
	AND (SUBSTRING(@Parent, 4 , 7) = SUBSTRING(@Child, 4 , 7) AND (LEN(@Parent) = 10))) RETURN 1;
			
	RETURN 0;

END
GO

----------------------------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_ApprovedZayavka]    Script Date: 08/11/2018 11:27:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_stat_ApprovedZayavka]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

 --Single Amount
 SELECT COUNT(REC_ID)  as ApprovedZayavkaCount
 FROM KasabaKIAT_N1.dbo.tbpt_zayavki
 WHERE [STATUS] = '4' 
 AND dbo.IsMyDependent(@DEPARTMENT_BASE, DEPARTMENT_CODE) = 1
 AND DATEPART ( q , [TIMESTAMP] ) = @KVARTAL
 AND   DATEPART ( YEAR , [TIMESTAMP] ) = @YEAR;
  
end

---------------------------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_InProgressZayavka]    Script Date: 08/11/2018 13:42:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_stat_InProgressZayavka]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

--Single Amount
 SELECT COUNT(REC_ID)  as InProgressZayavkaCount
 FROM KasabaKIAT_N1.dbo.tbpt_zayavki
 WHERE [STATUS] = '1' 
 AND dbo.IsMyDependent(@DEPARTMENT_BASE, DEPARTMENT_CODE) = 1
 AND DATEPART ( q , [TIMESTAMP] ) = @KVARTAL
 AND   DATEPART ( YEAR , [TIMESTAMP] ) = @YEAR;
  
end

--------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_RejectedZayavka]    Script Date: 08/11/2018 13:42:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_stat_RejectedZayavka]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

--Single Amount
 SELECT COUNT(REC_ID)  as RejectedZayavkaCount
 FROM KasabaKIAT_N1.dbo.tbpt_zayavki
 WHERE [STATUS] = '3' 
 AND dbo.IsMyDependent(@DEPARTMENT_BASE, DEPARTMENT_CODE) = 1
 AND DATEPART ( q , [TIMESTAMP] ) = @KVARTAL
 AND   DATEPART ( YEAR , [TIMESTAMP] ) = @YEAR;
  
end

------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_BronCount]    Script Date: 08/16/2018 12:15:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_BronCount]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

SELECT COUNT(*) as BronCount 
FROM KasabaKIAT_N1.dbo.tblBron Br
inner join KasabaKIAT_N1.dbo.tblPos_Put Pt
on Br.ID_San = Pt.ID_Sanat AND Br.Num_Put = Pt.Num_Put
inner join KasabaKIAT_N1.dbo.tblPost Pm
on Pt.ID_Post = Pm.ID
WHERE dbo.IsMyDependent(@DEPARTMENT_BASE, Pm.DepartmentBase) = 1
 AND Pm.ID_KV = @KVARTAL
 AND Pm.SmFil = @YEAR;

  
end

-------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_EcpIncome]    Script Date: 08/16/2018 13:14:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_EcpIncome]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

SELECT SUM(ecp.OBLSOVPROF_AMT) as  EcpIncomeAmount
FROM 
KasabaKIAT_N1.dbo.tbbu_ecp_calc ecp
WHERE ([dbo].IsMyDependent(@DEPARTMENT_BASE , ecp.OBL_DEPARTMENT_BASE) = 1 OR
	[dbo].IsMyDependent(@DEPARTMENT_BASE , ecp.ADEP_CODE) = 1  )
	AND DATEPART ( q , ecp.OPERDATE ) = @KVARTAL
	AND   DATEPART ( YEAR , ecp.OPERDATE ) = @YEAR;

  
end

--------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_EcpIncome]    Script Date: 08/16/2018 14:20:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_stat_EcpIncomeFed]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

SELECT SUM(ecp.FEDERATION_AMT) as  EcpIncomeAmount
FROM 
KasabaKIAT_N1.dbo.tbbu_ecp_calc ecp
WHERE ([dbo].IsMyDependent(@DEPARTMENT_BASE , ecp.OBL_DEPARTMENT_BASE) = 1 OR
	[dbo].IsMyDependent(@DEPARTMENT_BASE , ecp.ADEP_CODE) = 1  )
	AND DATEPART ( q , ecp.OPERDATE ) = @KVARTAL
	AND   DATEPART ( YEAR , ecp.OPERDATE ) = @YEAR;

  
end

------------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_PartialIncome]    Script Date: 08/16/2018 20:31:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_PartialIncome]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

	SELECT ISNULL(SUM(ecp.AMOUNT) ,  0) as CHASTICHKA
	FROM KasabaKIAT_N1.dbo.tbgl_bankpayments ecp
	inner join KasabaKIAT_N1.dbo.tbku_StrokaProfsoyza pp
	on ecp.DEPARTMENT_BASE = pp.MATCH_DEPARTMENTBASE
	WHERE DATEPART ( YEAR , ecp.TRAN_DATE ) = @YEAR
	AND DATEPART ( q , ecp.TRAN_DATE ) = @KVARTAL
	AND dbo.IsMyDependent(@DEPARTMENT_BASE, ecp.DEPARTMENT_BASE) = 1
  
end


----------------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_OtherIncome]    Script Date: 08/16/2018 20:32:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_OtherIncome]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

SELECT  ISNULL(SUM(pr.SUMMA), 0) AS OTHERSAMOUNT
 FROM KasabaKIAT_N1.dbo.tbbu_provodki pr
inner join KasabaKIAT_N1.dbo.tbku_StrokaProfsoyza pp
on pr.DEPARTMENT_BASE = pp.MATCH_DEPARTMENTBASE
WHERE DATEPART ( YEAR , pr.TRDT ) = @YEAR
AND DATEPART ( q , pr.TRDT ) = @KVARTAL
AND dbo.IsMyDependent(@DEPARTMENT_BASE, pr.DEPARTMENT_BASE) = 1
AND pr.PLATELRSCH = '19' 
AND pr.POLUCHRSCH = '22'

  
end

---------------------------------------------------------------------------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_IncomePerRegionList_Fed]    Script Date: 08/16/2018 16:23:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_IncomePerRegionList_Fed]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

	SELECT	AAA.ProfsoyzNaimenovanie , 
	    ISNULL(SUM(AAA.OBLSOVPROF_AMT) , 0) + ISNULL(SUM(AAA.CHASTICHKA), 0) + ISNULL(SUM(AAA.OTHERS), 0) as TOTAL_AMT,
		SUM(AAA.OBLSOVPROF_AMT) as OBLSOVPROF_AMT,
		SUM(AAA.CHASTICHKA) as  CHASTICHKA,
		SUM(AAA.OTHERS) as OTHERS
FROM 
(SELECT pp.ProfsoyzNaimenovanie , ecp.OBLSOVPROF_AMT , 0 as CHASTICHKA, 0 AS OTHERS FROM 
KasabaKIAT_N1.dbo.tbbu_ecp_calc ecp
inner join KasabaKIAT_N1.dbo.tbku_StrokaProfsoyza pp
on ecp.OBL_DEPARTMENT_BASE = pp.MATCH_DEPARTMENTBASE
WHERE DATEPART ( YEAR , ecp.OPERDATE ) = @YEAR
AND DATEPART ( q , ecp.OPERDATE ) = @KVARTAL

UNION

SELECT pp.ProfsoyzNaimenovanie , 0 as  OBLSOVPROF_AMT , ecp.AMOUNT as CHASTICHKA, 0 AS OTHERS 
FROM KasabaKIAT_N1.dbo.tbgl_bankpayments ecp
inner join KasabaKIAT_N1.dbo.tbku_StrokaProfsoyza pp
on ecp.DEPARTMENT_BASE = pp.MATCH_DEPARTMENTBASE
WHERE DATEPART ( YEAR , ecp.TRAN_DATE ) = @YEAR
AND DATEPART ( q , ecp.TRAN_DATE ) = @KVARTAL
AND LEN(ecp.DEPARTMENT_BASE) = 5

UNION

SELECT pp.ProfsoyzNaimenovanie , 0 as  OBLSOVPROF_AMT , 0 as CHASTICHKA, pr.SUMMA AS OTHERS
 FROM KasabaKIAT_N1.dbo.tbbu_provodki pr
inner join KasabaKIAT_N1.dbo.tbku_StrokaProfsoyza pp
on pr.DEPARTMENT_BASE = pp.MATCH_DEPARTMENTBASE
WHERE DATEPART ( YEAR , pr.TRDT ) = @YEAR
AND DATEPART ( q , pr.TRDT ) = @KVARTAL
AND LEN(pr.DEPARTMENT_BASE) = 5
AND pr.PLATELRSCH = '19' 
AND pr.POLUCHRSCH = '22') AAA
GROUP BY AAA.ProfsoyzNaimenovanie

  
end
-----------------------------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_IncomePerRegionList_Obl]    Script Date: 08/16/2018 16:25:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_IncomePerRegionList_Obl]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

	SELECT	AAA.ProfsoyzNaimenovanie , 
		ISNULL(SUM(AAA.OBLSOVPROF_AMT) , 0) + ISNULL(SUM(AAA.CHASTICHKA), 0) + ISNULL(SUM(AAA.OTHERS), 0) as TOTAL_AMT,
		SUM(AAA.OBLSOVPROF_AMT) as OBLSOVPROF_AMT,
		SUM(AAA.CHASTICHKA) as  CHASTICHKA,
		SUM(AAA.OTHERS) as OTHERS
	FROM 
	(
		SELECT Reg.NameReg as ProfsoyzNaimenovanie , ecp.OBLSOVPROF_AMT , 0 as CHASTICHKA, 0 AS OTHERS FROM 
		KasabaKIAT_N1.dbo.tbbu_ecp_calc ecp
		inner join KasabaKIAT_N1.dbo.tblSprav_Reg Reg
		on '17' + SUBSTRING(ecp.ADEP_CODE, 6 , 5) = Reg.SOATO_CODE
		WHERE dbo.IsMyDependent(@DEPARTMENT_BASE, ecp.ADEP_CODE) = 1 
		AND DATEPART ( YEAR , ecp.OPERDATE ) = @YEAR
		AND DATEPART ( q , ecp.OPERDATE ) = @KVARTAL

		UNION

		SELECT null as ProfsoyzNaimenovanie , 0 as  OBLSOVPROF_AMT , 0 as CHASTICHKA, 0 AS OTHERS 

		UNION

		SELECT null as ProfsoyzNaimenovanie , 0 as  OBLSOVPROF_AMT , 0 as CHASTICHKA, 0 AS OTHERS 

) AAA
WHERE (AAA.OBLSOVPROF_AMT > 0 OR AAA.CHASTICHKA > 0 OR AAA.OTHERS > 0)
GROUP BY AAA.ProfsoyzNaimenovanie;

  
end
-------------------------------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_UrgentPutyovkas]    Script Date: 08/16/2018 20:25:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_UrgentPutyovkas]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;


SELECT COUNT(postp.ID) as  UrgentPutyovkaCount
FROM KasabaKIAT_N1.dbo.tblPos_Put postp
inner join KasabaKIAT_N1.dbo.tblPost post
on postp.ID_Post = post.ID
where postp.ID_PolVoz = 0
AND [dbo].IsMyDependent(@DEPARTMENT_BASE , post.Departmentbase) = 1
AND  postp.DataSan <= CAST(DATEADD(day,14,GETDATE()) as DATE)
AND post.SmFil = @YEAR
AND post.ID_KV = @KVARTAL;
  
end


--------------------------------------------------------------------------------------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_UrgentPutyovkasRegionListFed]    Script Date: 08/16/2018 21:06:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_UrgentPutyovkasRegionListFed]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

SELECT pp.ProfsoyzNaimenovanie , COUNT(postp.ID) as PutCount
FROM KasabaKIAT_N1.dbo.tblPos_Put postp
inner join KasabaKIAT_N1.dbo.tblPost post
on postp.ID_Post = post.ID
inner join KasabaKIAT_N1.dbo.tbku_StrokaProfsoyza pp
on post.DepartmentBase = pp.MATCH_DEPARTMENTBASE
where postp.ID_PolVoz = 0
AND [dbo].IsMyDependent(@DEPARTMENT_BASE , post.Departmentbase) = 1
AND  postp.DataSan <= CAST(DATEADD(day,14,GETDATE()) as DATE)
AND post.SmFil = @YEAR
AND post.ID_KV = @KVARTAL
GROUP BY pp.ProfsoyzNaimenovanie;

  
end

------------------------------------------------------------------------------------------



USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_UrgentPutyovkasRegionListObl]    Script Date: 08/16/2018 21:06:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_UrgentPutyovkasRegionListObl]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

SELECT postp.Num_Put , San.Name_San , postp.DataSan
FROM KasabaKIAT_N1.dbo.tblPos_Put postp
inner join KasabaKIAT_N1.dbo.tblPost post
on postp.ID_Post = post.ID
inner join KasabaKIAT_N1.dbo.tblSprav_San San
on postp.ID_Sanat = San.ID
where postp.ID_PolVoz = 0
AND [dbo].IsMyDependent(@DEPARTMENT_BASE , post.Departmentbase) = 1
AND  postp.DataSan <= CAST(DATEADD(day,14,GETDATE()) as DATE)
AND post.SmFil = @YEAR
AND post.ID_KV = @KVARTAL
;

  
end


------------------------------------------------------------------------------------------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_PlanPutyovkaCount]    Script Date: 08/16/2018 22:29:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_PlanPutyovkaCount]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

SELECT SUM(AA.PutKol) as PLANCount 
FROM 
(SELECT CASE WHEN @KVARTAL = 1 THEN Putevok1kv
			WHEN @KVARTAL = 2 THEN Putevok2kv
			WHEN @KVARTAL = 3 THEN Putevok3kv
			WHEN @KVARTAL = 4 THEN Putevok4kv
			END PutKol
FROM KasabaKIAT_N1.dbo.tbku_SpisokRaspredeleniya p
inner join KasabaKIAT_N1.dbo.tbku_StrokaProfsoyza pp
on p.ProfsoyzKod = pp.ProfsoyzKod
WHERE p.GodZaezdov = @YEAR
AND [dbo].IsMyDependent(@DEPARTMENT_BASE , pp.MATCH_DEPARTMENTBASE) = 1) AA

  
end


---------------------------------------------------------------------------------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_PlanPutyovkaAmount]    Script Date: 08/16/2018 22:58:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_PlanPutyovkaAmount]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

SELECT SUM(AA.PutKol) * 
(SELECT TOP 1 AMOUNT 
	FROM KasabaKIAT_N1.dbo.tbpt_avarageprice 
	WHERE YEAR(SET_DATE) = @YEAR) as PLANAmount
FROM 
(SELECT CASE WHEN @KVARTAL = 1 THEN Putevok1kv
			WHEN @KVARTAL = 2 THEN Putevok2kv
			WHEN @KVARTAL = 3 THEN Putevok3kv
			WHEN @KVARTAL = 4 THEN Putevok4kv
			END PutKol
FROM KasabaKIAT_N1.dbo.tbku_SpisokRaspredeleniya p
inner join KasabaKIAT_N1.dbo.tbku_StrokaProfsoyza pp
on p.ProfsoyzKod = pp.ProfsoyzKod
WHERE p.GodZaezdov = @YEAR
AND [dbo].IsMyDependent(@DEPARTMENT_BASE , pp.MATCH_DEPARTMENTBASE) = 1) AA

  
end

----------------------------------------------------------------------------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_RealizedPutyovkas]    Script Date: 08/16/2018 23:06:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_RealizedPutyovkas]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

SELECT COUNT(Vp.ID) as RealizasiyaCount 
FROM KasabaKIAT_N1.dbo.tblVipl_Put Vp
inner join KasabaKIAT_N1.dbo.tblVipl Vm
on Vp.ID_Vipl = Vm.ID
WHERE [dbo].IsMyDependent(@DEPARTMENT_BASE , Vm.DepartmentBase) = 1
AND Vm.SmFil = @YEAR
AND Vm.ID_Kv = @KVARTAL

  
end

-----------------------------------------------------------------------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_RealizPutyovkaCount]    Script Date: 08/16/2018 23:08:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_RealizPutyovkaCount]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

SELECT COUNT(Vp.ID) as RealizasiyaCount 
FROM KasabaKIAT_N1.dbo.tblVipl_Put Vp
inner join KasabaKIAT_N1.dbo.tblVipl Vm
on Vp.ID_Vipl = Vm.ID
WHERE [dbo].IsMyDependent(@DEPARTMENT_BASE , Vm.DepartmentBase) = 1
AND Vm.SmFil = @YEAR
AND Vm.ID_Kv = @KVARTAL

  
end

------------------------------------------------------------------------------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_RealizPutyovkaAmount]    Script Date: 08/16/2018 23:08:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_RealizPutyovkaAmount]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

SELECT SUM(Vp.Symma) as RealizasiyaAmount
FROM KasabaKIAT_N1.dbo.tblVipl_Put Vp
inner join KasabaKIAT_N1.dbo.tblVipl Vm
on Vp.ID_Vipl = Vm.ID
WHERE [dbo].IsMyDependent(@DEPARTMENT_BASE , Vm.DepartmentBase) = 1
AND Vm.SmFil = @YEAR
AND Vm.ID_Kv = @KVARTAL

  
end

-----------------------------------------------------------------------------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_OstatokPutyovkaCount]    Script Date: 08/16/2018 23:56:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_OstatokPutyovkaCount]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;


SELECT COUNT(Vp.Symma_Put) as OstatokCount
FROM KasabaKIAT_N1.dbo.tblPos_Put Vp
inner join KasabaKIAT_N1.dbo.tblPost Vm
on Vp.ID_Post = Vm.ID
WHERE [dbo].IsMyDependent(@DEPARTMENT_BASE , Vm.DepartmentBase) = 1
AND Vm.SmFil = @YEAR
AND Vm.ID_Kv = @KVARTAL
AND Vp.ID_PolVoz = 0;
  
end

-------------------------------------------------------------------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_OstatokPutyovkaAmount]    Script Date: 08/16/2018 23:56:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_OstatokPutyovkaAmount]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

SELECT SUM(Vp.Symma_Put) as OstatokAmount
FROM KasabaKIAT_N1.dbo.tblPos_Put Vp
inner join KasabaKIAT_N1.dbo.tblPost Vm
on Vp.ID_Post = Vm.ID
WHERE [dbo].IsMyDependent(@DEPARTMENT_BASE , Vm.DepartmentBase) = 1
AND Vm.SmFil = @YEAR
AND Vm.ID_Kv = @KVARTAL
AND Vp.ID_PolVoz = 0;

  
end

------------------------------------------------------------------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_S45PutyovkaCount]    Script Date: 08/17/2018 00:54:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_S45PutyovkaCount]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;


SELECT COUNT(Put.ID) as S45PutyovkaCount 
FROM KasabaKIAT_N1.dbo.tblOplata_Ras Put
inner join KasabaKIAT_N1.dbo.tblOplata Opl
on Put.ID_Opl = Opl.ID
WHERE Opl.PrizOpl = 3 --OplataPutyovka
AND Put.ID_Pr = 1     --45%
AND Opl.SmFil =@YEAR
AND Opl.ID_KV =  @KVARTAL
AND [dbo].IsMyDependent(@DEPARTMENT_BASE , Opl.DepartmentBase) = 1;
  
end

------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_S45PutyovkaAmount]    Script Date: 08/17/2018 00:57:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_S45PutyovkaAmount]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

SELECT SUM(Put.Symma) as S45PutyovkaAmoun
FROM KasabaKIAT_N1.dbo.tblOplata_Ras Put
inner join KasabaKIAT_N1.dbo.tblOplata Opl
on Put.ID_Opl = Opl.ID
WHERE Opl.PrizOpl = 3 --OplataPutyovka
AND Put.ID_Pr = 1     --45%
AND Opl.SmFil =@YEAR
AND Opl.ID_KV =  @KVARTAL
AND [dbo].IsMyDependent(@DEPARTMENT_BASE , Opl.DepartmentBase) = 1;

  
end


---------------------------------------------------------------------------------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_S45PutyovkaCount]    Script Date: 08/17/2018 00:54:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_S100PutyovkaCount]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;


SELECT COUNT(Put.ID) as S45PutyovkaCount 
FROM KasabaKIAT_N1.dbo.tblOplata_Ras Put
inner join KasabaKIAT_N1.dbo.tblOplata Opl
on Put.ID_Opl = Opl.ID
WHERE Opl.PrizOpl = 3 --OplataPutyovka
AND Put.ID_Pr = 2     --45%
AND Opl.SmFil =@YEAR
AND Opl.ID_KV =  @KVARTAL
AND [dbo].IsMyDependent(@DEPARTMENT_BASE , Opl.DepartmentBase) = 1;
  
end

------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_S45PutyovkaAmount]    Script Date: 08/17/2018 00:57:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_S100PutyovkaAmount]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

SELECT SUM(Put.Symma) as S45PutyovkaAmoun
FROM KasabaKIAT_N1.dbo.tblOplata_Ras Put
inner join KasabaKIAT_N1.dbo.tblOplata Opl
on Put.ID_Opl = Opl.ID
WHERE Opl.PrizOpl = 3 --OplataPutyovka
AND Put.ID_Pr = 2     --45%
AND Opl.SmFil =@YEAR
AND Opl.ID_KV =  @KVARTAL
AND [dbo].IsMyDependent(@DEPARTMENT_BASE , Opl.DepartmentBase) = 1;

  
end

----------------------------------------------------------------------------------



USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_S45PutyovkaCount]    Script Date: 08/17/2018 00:54:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_S0PutyovkaCount]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;


SELECT COUNT(Put.ID) as S45PutyovkaCount 
FROM KasabaKIAT_N1.dbo.tblOplata_Ras Put
inner join KasabaKIAT_N1.dbo.tblOplata Opl
on Put.ID_Opl = Opl.ID
WHERE Opl.PrizOpl = 3 --OplataPutyovka
AND Put.ID_Pr = 3     --45%
AND Opl.SmFil =@YEAR
AND Opl.ID_KV =  @KVARTAL
AND [dbo].IsMyDependent(@DEPARTMENT_BASE , Opl.DepartmentBase) = 1;
  
end

------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_S45PutyovkaAmount]    Script Date: 08/17/2018 00:57:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_S0PutyovkaAmount]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

SELECT SUM(Put.Symma) as S45PutyovkaAmoun
FROM KasabaKIAT_N1.dbo.tblOplata_Ras Put
inner join KasabaKIAT_N1.dbo.tblOplata Opl
on Put.ID_Opl = Opl.ID
WHERE Opl.PrizOpl = 3 --OplataPutyovka
AND Put.ID_Pr = 3     --45%
AND Opl.SmFil =@YEAR
AND Opl.ID_KV =  @KVARTAL
AND [dbo].IsMyDependent(@DEPARTMENT_BASE , Opl.DepartmentBase) = 1;

  
end

------------------------------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_TotalZadoljennostAmount]    Script Date: 08/17/2018 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_TotalZadoljennostAmount]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

SELECT ISNULL(SUM(Org.DB_BAL), 0) as ZadolAmount 
FROM KasabaKIAT_N1.dbo.tblSprav_Org Org
WHERE  (Org.DB_BAL > 0 OR Org.CT_BAL > 0)
AND [dbo].IsMyDependent(@DEPARTMENT_BASE , Org.OWNER_CODE) = 1

  
end


----------------------------------------------------------------------------------------------------------------------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_ZadoljennostListFed]    Script Date: 08/17/2018 15:58:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_ZadoljennostListFed]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

SELECT Pr.ProfsoyzNaimenovanie , ISNULL(SUM(Org.DB_BAL), 0) as ZadolAmount 
FROM KasabaKIAT_N1.dbo.tblSprav_Org Org
inner join KasabaKIAT_N1.dbo.tblSprav_Reg Reg
on Org.Region = Reg.CodeReg
inner join KasabaKIAT_N1.dbo.tbku_StrokaProfsoyza Pr
on '172' + SUBSTRING(Reg.SOATO_CODE , 3 , 2) = Pr.MATCH_DEPARTMENTBASE
WHERE  (Org.DB_BAL > 0 OR Org.CT_BAL > 0)
AND [dbo].IsMyDependent(@DEPARTMENT_BASE , Org.OWNER_CODE) = 1
GROUP BY Pr.ProfsoyzNaimenovanie


  
end

---------------------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_ZadoljennostListObl]    Script Date: 08/17/2018 16:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_ZadoljennostListObl]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

SELECT Reg.NameReg , ISNULL(SUM(Org.DB_BAL), 0) as ZadolAmount 
FROM KasabaKIAT_N1.dbo.tblSprav_Org Org
inner join KasabaKIAT_N1.dbo.tblSprav_Reg Reg
on Org.Region = Reg.CodeReg
WHERE  (Org.DB_BAL > 0 OR Org.CT_BAL > 0)
AND [dbo].IsMyDependent(@DEPARTMENT_BASE , Org.OWNER_CODE) = 1
GROUP BY Reg.NameReg

  
end

-----------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_ProfComExists]    Script Date: 08/17/2018 20:19:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_ProfComExists]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

SELECT COUNT(ID) as  ProfComExistsCount
FROM KasabaKIAT_N1.dbo.tblSprav_Org Org
WHERE [dbo].IsMyDependent(@DEPARTMENT_BASE , Org.DepartmentBase) = 1
AND  Org.IS_PRAVSOYUZ = 1
GROUP BY Org.IS_PRAVSOYUZ;

  
end


---------------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_stat_ProfComNotExists]    Script Date: 08/17/2018 20:20:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_stat_ProfComNotExists]
@DEPARTMENT_BASE nvarchar(255),
@YEAR int,
@KVARTAL int

as 
begin 
set nocount on;

SELECT COUNT(ID) as  ProfComExistsCount
FROM KasabaKIAT_N1.dbo.tblSprav_Org Org
WHERE [dbo].IsMyDependent(@DEPARTMENT_BASE , Org.DepartmentBase) = 1
AND  Org.IS_PRAVSOYUZ = 0
GROUP BY Org.IS_PRAVSOYUZ;

  
end


----------------------------------------------------------------------------------------------------


