USE [KasabaKIAT]
GO

/****** Object:  Table [dbo].[tbbu_provodki]    Script Date: 02/27/2018 15:13:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tbbu_provodki](
	[REC_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DEPARTMENT_BASE] [varchar](255) NULL,
	[USER_ID] [bigint] NULL,
	[INOUTFLG] [bit] NULL,
	[TRDT] [datetime] NULL,
	[NUMBER] [nvarchar](50) NULL,
	[TIPSCHETA] [bit] NULL,
	[TIPOBOROTA] [bit] NULL,
	[PLATELNAME] [nvarchar](500) NULL,
	[PLATELINN] [varchar](9) NULL,
	[PLATELMFO] [varchar](5) NULL,
	[POLUCHNAME] [nvarchar](500) NULL,
	[POLUCHINN] [varchar](9) NULL,
	[POLUCHMFO] [varchar](5) NULL,
	[PLATELRSCH] [varchar](30) NULL,
	[POLUCHRSCH] [varchar](30) NULL,
	[SUMMA] [money] NULL,
	[COMMENT] [nvarchar](1000) NULL,
 CONSTRAINT [PK_tbbu_provodki] PRIMARY KEY CLUSTERED 
(
	[REC_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

----------------------------------------

USE [KasabaKIAT]
GO

/****** Object:  Table [dbo].[tbbu_platejka]    Script Date: 02/27/2018 15:17:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tbbu_platejka](
	[REC_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DEPARTMENT_CODE] [varchar](255) NULL,
	[USERID] [bigint] NULL,
	[PLAT_DATE] [datetime] NULL,
	[PLAT_NUM] [varchar](10) NULL,
	[PLATELNAME] [nvarchar](500) NULL,
	[PLATELINN] [varchar](9) NULL,
	[PLATELRASCHET] [varchar](30) NULL,
	[PLATELBANKNAME] [nvarchar](500) NULL,
	[PLATELBANKMFO] [varchar](5) NULL,
	[POLUCHNAME] [nvarchar](500) NULL,
	[POLUCHINN] [varchar](9) NULL,
	[POLUCHRASCHET] [varchar](30) NULL,
	[POLUCHBANKNAME] [nvarchar](500) NULL,
	[POLUCHBANKMFO] [varchar](5) NULL,
	[SUMMA] [money] NULL,
	[COMMENT] [nvarchar](500) NULL,
	[RUKOVODITEL] [nvarchar](100) NULL,
	[GLAVBUX] [nvarchar](100) NULL,
 CONSTRAINT [PK_tbbu_platejka] PRIMARY KEY CLUSTERED 
(
	[REC_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


-------------------------------------------------------------


USE [KasabaKIAT]
GO

/****** Object:  Table [dbo].[tbbu_planbyudjetregion]    Script Date: 02/27/2018 18:39:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tbbu_planbyudjetregion](
	[REC_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DEPARTMENT_BASE] [varchar](255) NULL,
	[USERID] [bigint] NULL,
	[YEAR] [int] NULL,
	[SOATO_CODE] [varchar](10) NULL,
	[ITEM_CODE] [varchar](254) NULL,
	[ITEM_NAME] [nvarchar](254) NULL,
	[GODOVOY] [money] NULL,
	[QUARTER1] [money] NULL,
	[HALFYEAR] [money] NULL,
	[MONTH9] [money] NULL,
 CONSTRAINT [PK_tbbu_planbyudjetregion] PRIMARY KEY CLUSTERED 
(
	[REC_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

