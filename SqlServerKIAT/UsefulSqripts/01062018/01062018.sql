USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_pr_GetNewTransactions]    Script Date: 06/04/2018 23:32:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_pr_GetNewTransactions]
as 
begin 
set nocount on;

SELECT TOP 10000 K.ID, K.SUMPAY , K.CLINN , COINN , K.PURPOSE , K.CLACC , K.OPERDAY , K.GRPID, K.CLNAME
FROM dbo.KaznaPaymentFile K
WHERE   NOT EXISTS (SELECT EE.[PAYMENT_ID] FROM dbo.tbbu_ecp_calc EE WHERE K.ID = EE.[PAYMENT_ID])
;
  
end

------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_pr_GetNewECPMOIFY]    Script Date: 06/05/2018 00:32:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_pr_GetNewECPMOIFY]
as 
begin 
set nocount on;

SELECT TOP 10000 [ID]
      ,[PAYMENT_ID]
      ,[PROFSOUZ_AMT]
      ,[FEDERATION_AMT]
      ,[OBLSOVPROF_AMT]
      ,[OBL_DEPARTMENT_BASE]
      ,[PROVODKA_ID]
      ,[REMARK]
      ,[INN]
      ,[SOATO]
      ,[SOATO_NAME]
      ,[OPERDATE]
      ,[OKONX_ID]
      ,[OKONX_NAME]
      ,[BRANCH_CODE]
      ,[BRANCH_NAME]
  FROM [KasabaKIAT_N1].[dbo].[tbbu_ecp_calc]
  WHERE [OBL_DEPARTMENT_BASE] IS NULL;

end
