USE [KasabaKIAT_N1]
GO

/****** Object:  Table [dbo].[tblPutHist]    Script Date: 08/05/2018 12:57:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblPutHist](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EVENT_TP] [int] NOT NULL,
	[Num_Put] [nvarchar](10) NULL,
	[ID_Sanat] [int] NULL,
	[REMARK] [nvarchar](1000) NULL,
	[UserId] [bigint] NULL,
	[DepartmentBase] [nvarchar](255) NULL,
 CONSTRAINT [PK_tblPutHist] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tblPutHist]  WITH CHECK ADD  CONSTRAINT [FK_tblPutHist_t_departmentbase] FOREIGN KEY([DepartmentBase])
REFERENCES [dbo].[t_departmentbase] ([code])
GO

ALTER TABLE [dbo].[tblPutHist] CHECK CONSTRAINT [FK_tblPutHist_t_departmentbase]
GO

ALTER TABLE [dbo].[tblPutHist]  WITH CHECK ADD  CONSTRAINT [FK_tblPutHist_tblSprav_San] FOREIGN KEY([ID_Sanat])
REFERENCES [dbo].[tblSprav_San] ([ID])
GO

ALTER TABLE [dbo].[tblPutHist] CHECK CONSTRAINT [FK_tblPutHist_tblSprav_San]
GO

ALTER TABLE [dbo].[tblPutHist]  WITH CHECK ADD  CONSTRAINT [FK_tblPutHist_users] FOREIGN KEY([UserId])
REFERENCES [dbo].[users] ([id])
GO

ALTER TABLE [dbo].[tblPutHist] CHECK CONSTRAINT [FK_tblPutHist_users]
GO

  ALTER TABLE [KasabaKIAT_N1].[dbo].[tblPutHist]
  ADD TIMESTAMP_T datetime default GETDATE(); 

  ALTER TABLE [KasabaKIAT_N1].dbo.tblFace_Med
  ADD TIMESTAMP_T datetime default GETDATE(); 

  ALTER TABLE [KasabaKIAT_N1].dbo.tblFace_Two
  ADD TIMESTAMP_T datetime default GETDATE(); 

  ALTER TABLE [KasabaKIAT_N1].dbo.tblFase
  ADD TIMESTAMP_T datetime default GETDATE(); 
  
  
  ALTER TABLE [KasabaKIAT_N1].dbo.[tblFile]
  ADD TIMESTAMP_T datetime default GETDATE(); 
  

  ALTER TABLE [KasabaKIAT_N1].dbo.tblOplata
  ADD TIMESTAMP_T datetime default GETDATE(); 
  

  ALTER TABLE [KasabaKIAT_N1].dbo.tblOplata_Ras
  ADD TIMESTAMP_T datetime default GETDATE(); 
  
  ALTER TABLE [KasabaKIAT_N1].dbo.tblPos_Put
  ADD TIMESTAMP_T datetime default GETDATE(); 
  
  ALTER TABLE [KasabaKIAT_N1].dbo.tblPost
  ADD TIMESTAMP_T datetime default GETDATE(); 
  
  ALTER TABLE [KasabaKIAT_N1].dbo.tblVipl
  ADD TIMESTAMP_T datetime default GETDATE(); 
  

  ALTER TABLE [KasabaKIAT_N1].dbo.tblVipl_Put
  ADD TIMESTAMP_T datetime default GETDATE(); 
  
  ALTER TABLE [KasabaKIAT_N1].dbo.tblVozVrat
  ADD TIMESTAMP_T datetime default GETDATE(); 
  
  ALTER TABLE [KasabaKIAT_N1].dbo.tblZayavkiLager
  ADD TIMESTAMP_T datetime default GETDATE(); 