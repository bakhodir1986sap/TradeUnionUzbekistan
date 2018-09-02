USE [KasabaKIAT_N1]
GO

/****** Object:  Table [dbo].[tbbu_provodki]    Script Date: 08/31/2018 10:59:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tbbu_nastroykiProvodok](
	[REC_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DEPARTMENT_BASE] [nvarchar](255) NULL,
	[USER_ID] [bigint] NULL,
	[CODE] [varchar](50) NOT NULL,
	[DEBIT_ACC] [nvarchar](30) NULL,
	[CREADIT_ACC] [nvarchar](30) NULL,
 CONSTRAINT [PK_tbbu_nastroykiProvodok] PRIMARY KEY CLUSTERED 
(
	[REC_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [KasabaKIAT_N1]
GO

ALTER TABLE [dbo].[KaznaBankFile]  WITH CHECK ADD  CONSTRAINT [FK_KaznaBankFile_tbbu_provodki] FOREIGN KEY([PROVODKA_ID])
REFERENCES [dbo].[tbbu_provodki] ([REC_ID])
GO

ALTER TABLE [dbo].[KaznaBankFile] CHECK CONSTRAINT [FK_KaznaBankFile_tbbu_provodki]
GO

