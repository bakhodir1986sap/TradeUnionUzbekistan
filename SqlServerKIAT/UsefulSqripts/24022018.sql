DROP TABLE [dbo].[tbgl_bankpayments]

USE [KasabaKIAT]
GO

/****** Object:  Table [dbo].[tbgl_bankpayments]    Script Date: 02/24/2018 20:05:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tbgl_bankpayments](
	[REC_ID] [bigint] NOT NULL IDENTITY(1,1),
	[NUM_DOC] [varchar](10) NULL,
	[PAYER_NAME] [varchar](500) NULL,
	[PAYER_INN] [varchar](9) NOT NULL,
	[PAYER_MFO] [varchar](5) NULL,
	[PAYER_ACCOUNT] [varchar](30) NOT NULL,
	[TRAN_DATE] [date] NOT NULL,
	[PAYMENT_DATAIL] [varchar](500) NULL,
	[AMOUNT] [money] NOT NULL,
	[BENEF_NAME] [varchar](500) NULL,
	[BENEF_INN] [varchar](9) NULL,
	[BENEF_MFO] [varchar](5) NULL,
	[BENEF_ACCOUNT] [varchar](30) NULL,
	[DBCRFLG] [varchar](10) NULL,
	[PARENT_ID] [bigint] NULL,
	[DEPARTMENT_BASE] [varchar](255) NULL,
	[USER_ID] [bigint] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


ALTER TABLE [dbo].[tbgl_bankpayments]
ADD CONSTRAINT tbgl_bankpayment_pk PRIMARY KEY(REC_ID)