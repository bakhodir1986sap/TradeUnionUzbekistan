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
 SELECT COUNT(REC_ID) 
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
 SELECT COUNT(REC_ID) 
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
 SELECT COUNT(REC_ID) 
 FROM KasabaKIAT_N1.dbo.tbpt_zayavki
 WHERE [STATUS] = '3' 
 AND dbo.IsMyDependent(@DEPARTMENT_BASE, DEPARTMENT_CODE) = 1
 AND DATEPART ( q , [TIMESTAMP] ) = @KVARTAL
 AND   DATEPART ( YEAR , [TIMESTAMP] ) = @YEAR;
  
end

------------------------------------------------------------------------------------

