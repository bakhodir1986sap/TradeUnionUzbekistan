USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_Postup_OblastGroup]    Script Date: 07/07/2018 12:32:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_rt_Razbivka_Postup_OblastGroup]
@BEGIN_DATE Datetime,
@END_DATE Datetime,
@DEPARTMENT_BASE nvarchar(255)

as 
begin 
set nocount on;

/****** Script for SelectTopNRows command from SSMS  ******/

SELECT TT4.OblastName , TT4.Kolichestvo, TT4.TOTALSUM , TT4.OBLASTSUM FROM 
(
SELECT	CASE WHEN (GROUPING(AA.OBLAST_NAME) = 1) THEN 'Итого'
            ELSE ISNULL(AA.OBLAST_NAME, 'Неопределен')
       END AS OblastName,
        COUNT(AA.[FEDERATION_AMT]) AS Kolichestvo,
        ROUND(SUM(AA.[FEDERATION_AMT])/100,2)   AS TOTALSUM,
        ROUND(SUM(AA.[OBLSOVPROF_AMT])/100,2)   AS OBLASTSUM,
        '1' AS ORDERR
FROM 
(
SELECT P.ID_Name as OBLAST_NAME,
       B.SUMPAY * 30 / 70 FEDERATION_AMT,
       B.SUMPAY OBLSOVPROF_AMT,
       P.Departmentbase as OBL_DEPARTMENT_BASE
FROM KasabaKIAT_N1.dbo.KaznaBankFile B
inner join KasabaKIAT_N1.dbo.tblPred P
on B.COACC = P.KaznaAccount
WHERE CONVERT(DATE,B.[OPERDAY]) 
BETWEEN CONVERT(DATE,@BEGIN_DATE)  AND CONVERT(DATE,@END_DATE)
AND P.Departmentbase LIKE @DEPARTMENT_BASE + '%'
AND P.Departmentbase not like '17') AA
GROUP BY AA.OBLAST_NAME WITH ROLLUP

) TT4
ORDER BY TT4.ORDERR
  
end
