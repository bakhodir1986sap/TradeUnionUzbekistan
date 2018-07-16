USE [KasabaKIAT_N1]
GO

/****** Object:  Table [dbo].[tblCloseDay]    Script Date: 07/13/2018 12:37:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblCloseKvartal](
	[REC_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DEPARTMENT_CODE] [varchar](255) NULL,
	[PREVKV_YEAR] [int] NULL,
	[PREVKV] [int] NULL,
	[CURKV_YEAR] [int] NULL,
	[CURKV] [int] NULL,
	[NEXTKV_YEAR] [int] NULL,
	[NEXTKV] [int] NULL,
 CONSTRAINT [PK_tblCloseKvartal] PRIMARY KEY CLUSTERED 
(
	[REC_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

--------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO

/****** Object:  Table [dbo].[tblCloseDayHistory]    Script Date: 07/13/2018 12:44:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblCloseKvartalHistory](
	[REC_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DEPARTMENT_CODE] [varchar](255) NULL,
	[CLOSED_KVYEAR] [int] NULL,
	[CLOSED_KV] [int] NULL,
	[TIMESTAMP] [datetime] NULL,
	[USER_ID] [bigint] NULL,
	[REMARK] [nvarchar](4000) NULL,
 CONSTRAINT [PK_tblCloseKvartalHistory] PRIMARY KEY CLUSTERED 
(
	[REC_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

----------------------------------------------------------------------------

  ALTER TABLE [KasabaKIAT_N1].[dbo].[tbbu_balance]
  ADD DEBIT_COUNT int;
  
  
  ALTER TABLE [KasabaKIAT_N1].[dbo].[tbbu_balance]
  ADD CREDIT_COUNT int;
  
  
  ALTER TABLE [KasabaKIAT_N1].[dbo].[tbbu_balance]
  ADD DEBIT_TRANS money;
  
    ALTER TABLE [KasabaKIAT_N1].[dbo].[tbbu_balance]
  ADD CREDIT_TRANS money;




  USE [KasabaKIAT_N1]
GO


/****** Object:  Index [UX_DEPARTMENT_CODE]    Script Date: 07/13/2018 14:40:17 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_DEPARTMENT_CODE] ON [dbo].[tblCloseKvartal] 
(
	[DEPARTMENT_CODE] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


INSERT INTO [KasabaKIAT_N1].[dbo].[tblCloseKvartal]
SELECT [DEPARTMENT_CODE]
      ,2017 as [PREVKV_YEAR]
      ,4 as [PREVKV]
      ,2018 as [CURKV_YEAR]
      ,1 as [CURKV]
      ,2018 as [NEXTKV_YEAR]
      ,2 as [NEXTKV]
  FROM [KasabaKIAT_N1].[dbo].[tblCloseDay]




  USE [KasabaKIAT_N1]
GO

/****** Object:  Index [UX_FILDS]    Script Date: 07/13/2018 15:38:19 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_FILDS] ON [dbo].[tbbu_balance] 
(
	[DEPARTMENT_BASE] ASC,
	[CUR_YEAR] ASC,
	[CUR_KV] ASC,
	[ACCOUNT_CODE] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO




----------------------------------------------

  insert into [KasabaKIAT_N1].[dbo].[tbbu_balance]
  select cc.DEPARTMENT_CODE as [DEPARTMENT_BASE]
      ,2018 as [CUR_YEAR]
      ,1 as [CUR_KV]
      ,1 as [USER_ID]
      ,dd.NAME_RU as [ACCOUNT_CODE]
      ,0 as [DEBIT_BAL]
      ,0 as [CREDIT_BAL]
      ,0 as [DEBIT_COUNT]
      ,0 as [CREDIT_COUNT]
      ,0 as [DEBIT_TRANS]
      ,0 as [CREDIT_TRANS] 
  from [KasabaKIAT_N1].[dbo].tbcs_dictionary dd
  cross join [KasabaKIAT_N1].[dbo].tblCloseDay cc
  where dd.DICTIONARY_TYPE_ID = 14
  order by cc.DEPARTMENT_CODE;

  ------------------------------------------------------------------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_Postup_OblastGroup]    Script Date: 07/14/2018 19:10:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_GlavnayaKniga_Debit]
@YEAR		int,
@kvartal	int,
@DEPARTMENT_CODE nvarchar(255)

as 
begin 
set nocount on;

/****** Script for SelectTopNRows command from SSMS  ******/

SELECT prov.score_begin as DebitAccount , 
       dict.NAME_RU as JO , 
       SUM(prod.SUMMA) as SUMMA 
  FROM [KasabaKIAT_N1].[dbo].[p_provodok] prov
  inner join [KasabaKIAT_N1].[dbo].tbcs_dictionary dict 
  on  prov.[order] = dict.REC_ID
  inner join [KasabaKIAT_N1].[dbo].tbbu_provodki prod
  on prov.score_begin = prod.PLATELRSCH AND prov.score_end = prod.POLUCHRSCH
 WHERE prod.DEPARTMENT_BASE = @DEPARTMENT_CODE
 AND   YEAR(prod.TRDT) = @YEAR
 AND  (((@kvartal = 1) AND (MONTH(prod.TRDT) >= 1	AND MONTH(prod.TRDT) <= 3)) OR
       ((@kvartal = 2) AND (MONTH(prod.TRDT) >= 4	AND MONTH(prod.TRDT) <= 6)) OR
       ((@kvartal = 3) AND (MONTH(prod.TRDT) >= 7	AND MONTH(prod.TRDT) <= 9)) OR
       ((@kvartal = 4) AND (MONTH(prod.TRDT) >= 10	AND MONTH(prod.TRDT) <= 12)))
 group by prov.score_begin , dict.NAME_RU;
 
  
end



-----------------------------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_Postup_OblastGroup]    Script Date: 07/14/2018 19:10:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_GlavnayaKniga_Credit]
@YEAR		int,
@kvartal	int,
@DEPARTMENT_CODE nvarchar(255)

as 
begin 
set nocount on;

/****** Script for SelectTopNRows command from SSMS  ******/

SELECT prov.score_end  as CreditAccount , 
       dict.NAME_RU as JO , 
       SUM(prod.SUMMA) as SUMMA 
  FROM [KasabaKIAT_N1].[dbo].[p_provodok] prov
  inner join [KasabaKIAT_N1].[dbo].tbcs_dictionary dict 
  on  prov.[order] = dict.REC_ID
  inner join [KasabaKIAT_N1].[dbo].tbbu_provodki prod
  on prov.score_begin = prod.PLATELRSCH AND prov.score_end = prod.POLUCHRSCH
   WHERE prod.DEPARTMENT_BASE = @DEPARTMENT_CODE
    AND   YEAR(prod.TRDT) = @YEAR
	AND  (((@kvartal = 1) AND (MONTH(prod.TRDT) >= 1  AND MONTH(prod.TRDT) <= 3)) OR
       ((@kvartal = 2) AND (MONTH(prod.TRDT) >= 4  AND MONTH(prod.TRDT) <= 6)) OR
       ((@kvartal = 3) AND (MONTH(prod.TRDT) >= 7  AND MONTH(prod.TRDT) <= 9)) OR
       ((@kvartal = 4) AND (MONTH(prod.TRDT) >= 10 AND MONTH(prod.TRDT) <= 12)))
 group by prov.score_end , dict.NAME_RU;
 
  
end
