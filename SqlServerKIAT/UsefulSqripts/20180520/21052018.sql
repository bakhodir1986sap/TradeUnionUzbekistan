USE [KasabaKIAT_N1]
GO

/****** Object:  Table [dbo].[tblSanatDataCompare]    Script Date: 05/21/2018 21:23:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblSanatDataCompare](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[YEAR] [int] NULL,
	[OWNER_TP] [int] NULL,
	[SANAT_ID] [int] NULL,
	[NUM_PUT] [nvarchar](50) NULL,
	[DATE_SAN] [nvarchar](100) NULL,
	[PRICE] [nvarchar](100) NULL,
	[FIO_OTDIX] [nvarchar](200) NULL,
	[ADRESS] [nvarchar](500) NULL,
	[WORK_PLACE] [nvarchar](500) NULL,
	[OTRASL] [nvarchar](500) NULL,
	[OWNER_CODE] [varchar](255) NULL,
 CONSTRAINT [PK_tblSanatDataCompare] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

