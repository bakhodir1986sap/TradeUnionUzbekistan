USE [KasabaKIAT_N1]
GO

/****** Object:  Table [dbo].[KaznaPaymentFile]    Script Date: 04/12/2018 19:36:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[KaznaBankFile](
	[K_TYPE] [char](1) NULL,
	[OPERDAY] [datetime] NULL,
	[ID] [numeric](12, 0) NOT NULL,
	[GRPID] [numeric](12, 0) NOT NULL,
	[KLSDOX] [nvarchar](25) NULL,
	[DOCNUMB] [nvarchar](10) NULL,
	[DOCDATE] [datetime] NULL,
	[DOCTYPE] [char](2) NULL,
	[SUMPAY] [numeric](20, 0) NULL,
	[CLMFO] [nvarchar](5) NULL,
	[CLACC] [nvarchar](20) NULL,
	[CLINN] [nvarchar](9) NULL,
	[CLNAME] [nvarchar](160) NULL,
	[COMFO] [nvarchar](5) NULL,
	[COACC] [nvarchar](20) NULL,
	[COINN] [nvarchar](9) NULL,
	[CONAME] [nvarchar](160) NULL,
	[PURPOSE] [nvarchar](500) NULL,
	[FILE_ID] [bigint] NULL,
 CONSTRAINT [PK_KaznaBankFile] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[KaznaBankFile]  WITH CHECK ADD  CONSTRAINT [FK_KaznaBankFile_KaznaFiles1] FOREIGN KEY([FILE_ID])
REFERENCES [dbo].[KaznaFiles] ([ID])
GO

ALTER TABLE [dbo].[KaznaBankFile] CHECK CONSTRAINT [FK_KaznaBankFile_KaznaFiles1]
GO

-------------------------------------------

ALTER TABLE dbo.KaznaPaymentFile
ADD [GRPID] [numeric](12, 0) ;