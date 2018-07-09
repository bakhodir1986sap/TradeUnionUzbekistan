ALTER TABLE dbo.KaznaBankFile 
ADD PROVODKA_ID bigint;

  ALTER TABLE [KasabaKIAT_N1].[dbo].[tblSprav_Org]
  ADD TIMESTAMP_CH datetime default GETDATE(); 


-------------------------------------------------------------------------------------------------------------------

  USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_pr_GetNewTransactions]    Script Date: 07/08/2018 15:08:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_pr_GetNewBankProvodki]
as 
begin 
set nocount on;

SELECT TOP 10000
       [OPERDAY]
      ,[ID]
      ,[GRPID]
      ,[KLSDOX]
      ,ROUND([SUMPAY]/100 , 2) as SUMPAY
      ,[COINN]
      , pr.Departmentbase
  FROM [KasabaKIAT_N1].[dbo].[KaznaBankFile] kb
  left outer join [KasabaKIAT_N1].[dbo].tblPred pr
  on kb.COACC = pr.KaznaAccount
  WHERE   kb.PROVODKA_ID IS NULL
 ORDER BY kb.ID;
;
  
end

-------------------------------------------------------------------------


USE [KasabaKIAT_N1]
GO

/****** Object:  Table [dbo].[roles]    Script Date: 07/09/2018 00:03:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TimeShedulers](
	[id] [bigint]  NOT NULL,
	[LastChanged] datetime NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



  INSERT INTO [KasabaKIAT_N1].[dbo].[TimeShedulers]
           ([id]
           ,[LastChanged])
     VALUES
           (1
           ,GETDATE())