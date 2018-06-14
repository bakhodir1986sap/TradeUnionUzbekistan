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

--------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO

ALTER TABLE [dbo].[tblPost]  WITH CHECK ADD  CONSTRAINT [FK_tblPost_tbbu_provodki] FOREIGN KEY([PROVODKA_ID])
REFERENCES [dbo].[tbbu_provodki] ([REC_ID])
GO

ALTER TABLE [dbo].[tblPost] CHECK CONSTRAINT [FK_tblPost_tbbu_provodki]
GO

---------------------------------------------------------------------------

  ALTER TABLE [KasabaKIAT_N1].[dbo].[tblVipl]
  ADD PROVODKA_ID bigint NULL;


USE [KasabaKIAT_N1]
GO

ALTER TABLE [dbo].[tblVipl]  WITH CHECK ADD  CONSTRAINT [FK_tblVipl_tbbu_provodki] FOREIGN KEY([PROVODKA_ID])
REFERENCES [dbo].[tbbu_provodki] ([REC_ID])
GO

ALTER TABLE [dbo].[tblVipl] CHECK CONSTRAINT [FK_tblVipl_tbbu_provodki]
GO
  
  -------------------------------------------------
  ALTER TABLE [KasabaKIAT_N1].dbo.tblVozVrat
  ADD PRIXOD_ID bigint;

	USE [KasabaKIAT_N1]
	GO

	ALTER TABLE [dbo].[tblVozVrat]  WITH CHECK ADD  CONSTRAINT [FK_tblVozVrat_tblPost] FOREIGN KEY([PRIXOD_ID])
	REFERENCES [dbo].[tblPost] ([ID])
	GO

	ALTER TABLE [dbo].[tblVozVrat] CHECK CONSTRAINT [FK_tblVozVrat_tblPost]
	GO


  
  ALTER TABLE [KasabaKIAT_N1].dbo.tblVozVrat
  ADD RASXOD_ID bigint;


	USE [KasabaKIAT_N1]
	GO

	ALTER TABLE [dbo].[tblVozVrat]  WITH CHECK ADD  CONSTRAINT [FK_tblVozVrat_tblVipl] FOREIGN KEY([RASXOD_ID])
	REFERENCES [dbo].[tblVipl] ([ID])
	GO

	ALTER TABLE [dbo].[tblVozVrat] CHECK CONSTRAINT [FK_tblVozVrat_tblVipl]
	GO


  
  ALTER TABLE [KasabaKIAT_N1].dbo.tblVozVrat
  ADD PRIXOD_PROVODKA_ID bigint;

	USE [KasabaKIAT_N1]
	GO

	ALTER TABLE [dbo].[tblVozVrat]  WITH CHECK ADD  CONSTRAINT [FK_tblVozVrat_tbbu_provodki] FOREIGN KEY([PRIXOD_PROVODKA_ID])
	REFERENCES [dbo].[tbbu_provodki] ([REC_ID])
	GO

	ALTER TABLE [dbo].[tblVozVrat] CHECK CONSTRAINT [FK_tblVozVrat_tbbu_provodki]
	GO
	

  
  ALTER TABLE [KasabaKIAT_N1].dbo.tblVozVrat
  ADD RASXOD_PROVODKA_ID bigint;

	USE [KasabaKIAT_N1]
	GO

	ALTER TABLE [dbo].[tblVozVrat]  WITH CHECK ADD  CONSTRAINT [FK_tblVozVrat_tbbu_provodki1] FOREIGN KEY([RASXOD_PROVODKA_ID])
	REFERENCES [dbo].[tbbu_provodki] ([REC_ID])
	GO

	ALTER TABLE [dbo].[tblVozVrat] CHECK CONSTRAINT [FK_tblVozVrat_tbbu_provodki1]
	GO


  
  --------------------------------------------------

  ALTER TABLE [KasabaKIAT_N1].dbo.tblOplata
  ADD PROVODKA_ID bigint NULL;
  

  USE [KasabaKIAT_N1]
GO

ALTER TABLE [dbo].[tblOplata]  WITH CHECK ADD  CONSTRAINT [FK_tblOplata_tbbu_provodki] FOREIGN KEY([PROVODKA_ID])
REFERENCES [dbo].[tbbu_provodki] ([REC_ID])
GO

ALTER TABLE [dbo].[tblOplata] CHECK CONSTRAINT [FK_tblOplata_tbbu_provodki]
GO

