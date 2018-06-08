  DELETE [KasabaKIAT_N1].[dbo].[tbbu_provodki]
  
  ALTER TABLE [KasabaKIAT_N1].[dbo].[tbbu_provodki]
  ADD TRREF VARCHAR(10);
  
  ALTER TABLE [KasabaKIAT_N1].[dbo].[tbbu_provodki]
  ADD TRREFNO VARCHAR(20);
  
  
  USE [KasabaKIAT_N1]
GO

/****** Object:  Table [dbo].[tbbu_provodki]    Script Date: 06/08/2018 15:51:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tbbu_trref](
	[REC_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CODE] [varchar](10) NULL,
	[DESCRIPTION] [nvarchar](200) NULL,
	[USER_ID] [bigint] NULL,
 CONSTRAINT [PK_tbbu_provodki] PRIMARY KEY CLUSTERED 
(
	[REC_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


  ALTER TABLE [KasabaKIAT_N1].[dbo].[tblPost]
  ADD PROVODKA_ID bigint NULL;

  ALTER TABLE [KasabaKIAT_N1].[dbo].[tblVipl]
  ADD PROVODKA_ID bigint NULL;
  
  -------------------------------------------------
  ALTER TABLE [KasabaKIAT_N1].dbo.tblVozVrat
  ADD PRIXOD_ID bigint;
  
  ALTER TABLE [KasabaKIAT_N1].dbo.tblVozVrat
  ADD RASXOD_ID bigint;
  
  ALTER TABLE [KasabaKIAT_N1].dbo.tblVozVrat
  ADD PRIXOD_PROVODKA_ID bigint;
  
  ALTER TABLE [KasabaKIAT_N1].dbo.tblVozVrat
  ADD RASXOD_PROVODKA_ID bigint;
  
  --------------------------------------------------

  ALTER TABLE [KasabaKIAT_N1].dbo.tblOplata
  ADD PROVODKA_ID bigint NULL;
  