USE [KasabaKIAT_N1]
GO

/****** Object:  Table [dbo].[tbbu_balance]    Script Date: 07/15/2018 23:06:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbbu_finOtchetSetting](
	[REC_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[OTCHET_PART] [int] NOT NULL, --FIN OTCHET, BALANCE
	[CODE] [nvarchar](10) NOT NULL,
	[DEBIT_SIDE] [nvarchar](30)   NULL,
	[CREDIT_SIDE] [nvarchar](30)  NULL,
	[ACCOUNT_CODE] [nvarchar](30) NULL,
	[DATA_TYPE] [int] NULL, --OBOROT, OSTATOK, EXPRESSION, VIBORKA 
	[DATA_DESC] [nvarchar](1000)   NULL, 
 CONSTRAINT [PK_finOtchetSetting] PRIMARY KEY CLUSTERED 
(
	[REC_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-------------------------------------------------

USE [KasabaKIAT_N1]
GO

/****** Object:  Table [dbo].[tblSprav_Otras]    Script Date: 07/16/2018 09:24:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblZayavkiLager](
	[REC_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ID_Otras] [int] NOT NULL,
	[ID_Camp] [bigint] NOT NULL, 
    [Quantuty]  [int]  NOT NULL, 
	[TotalAmount] [money] NOT NULL, 
	[TIMESTAMP] datetime NOT NULL, 
	[STATUS] int NOT NULL,
	[DEPARTMENT_CODE] [nvarchar](255)  NOT NULL,
	[UserId] [bigint] NOT NULL,
 CONSTRAINT [PK_ZayavkiLager] PRIMARY KEY CLUSTERED 
(
	[REC_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


----------------------------------------

USE [KasabaKIAT_N1]
GO

/****** Object:  Table [dbo].[tblSprav_Otras]    Script Date: 07/16/2018 09:24:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblOtchetiLager](
	[REC_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ID_ORG] [bigint] NOT NULL,
    [Quantuty]  [int]  NOT NULL, 
	[TotalAmount] [money] NOT NULL, 
	[TIMESTAMP] datetime NOT NULL, 
	[STATUS] int NOT NULL,
	[DEPARTMENT_CODE] [nvarchar](255)  NOT NULL,
	[UserId] [bigint] NOT NULL,
 CONSTRAINT [PK_OtchetiLager] PRIMARY KEY CLUSTERED 
(
	[REC_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO