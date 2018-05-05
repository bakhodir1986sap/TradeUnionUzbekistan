DROP TABLE [dbo].[tbgl_balance]


USE [KasabaKIAT]
GO

/****** Object:  Table [dbo].[tbgl_balance]    Script Date: 02/25/2018 03:00:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tbgl_balance](
	[TRDT] [date] NOT NULL,
	[ACCOUNT] [varchar](10) NULL,
	[DEPARTMENT_CODE] [varchar](255) NULL,
	[PRE_DEBIT_BAL] [money] NULL,
	[PRE_CREDIT_BAL] [money] NULL,
	[DEBIT_COUNT] [int] NULL,
	[DEBIT_AMOUNT] [money] NULL,
	[CREDIT_COUNT] [int] NULL,
	[CREDIT_AMOUNT] [money] NULL,
	[TDBAL_DEBIT] [money] NULL,
	[TD_BAL_CREDIT] [money] NULL,
	[CCY] [char](3) NULL,
	[TIMESTAMP] [date] NULL,
	[USER_ID] [bigint] NULL,
	[REC_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[INN] [varchar](9) NULL,
 CONSTRAINT [PK_tbgl_balance] PRIMARY KEY CLUSTERED 
(
	[REC_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tbgl_balance]  WITH NOCHECK ADD  CONSTRAINT [FK_tbgl_balance_t_departmentbase] FOREIGN KEY([DEPARTMENT_CODE])
REFERENCES [dbo].[t_departmentbase] ([code])
GO

ALTER TABLE [dbo].[tbgl_balance] NOCHECK CONSTRAINT [FK_tbgl_balance_t_departmentbase]
GO

ALTER TABLE [dbo].[tbgl_balance]  WITH NOCHECK ADD  CONSTRAINT [FK_tbgl_balance_users] FOREIGN KEY([USER_ID])
REFERENCES [dbo].[users] ([id])
GO

ALTER TABLE [dbo].[tbgl_balance] NOCHECK CONSTRAINT [FK_tbgl_balance_users]
GO

-------------------------------------------------------------------------------

DROP TABLE [dbo].[tbgl_jnlmst]


USE [KasabaKIAT]
GO

/****** Object:  Table [dbo].[tbgl_jnlmst]    Script Date: 02/25/2018 20:10:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tbgl_jnlmst](
	[TRDT] [datetime] NULL,
	[DEPARTMENT_BASE] [varchar](255) NULL,
	[DYSEQ] [int] NULL,
	[OPER_ID] [int] NULL,
	[TRTP] [char](1) NULL,
	[ORGTRDT] [char](8) NULL,
	[ORGORG_ID] [bigint] NULL,
	[ORGCRT_USR_ID] [bigint] NULL,
	[ORGDYSEQ] [int] NULL,
	[REMARK] [varchar](500) NULL,
	[CRTUSER_ID] [bigint] NULL,
	[REC_ID] [bigint] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_tbgl_jnlmst] PRIMARY KEY CLUSTERED 
(
	[REC_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tbgl_jnlmst]  WITH NOCHECK ADD  CONSTRAINT [tbgl_jnlmst_1] FOREIGN KEY([CRTUSER_ID])
REFERENCES [dbo].[users] ([id])
GO

ALTER TABLE [dbo].[tbgl_jnlmst] NOCHECK CONSTRAINT [tbgl_jnlmst_1]
GO

ALTER TABLE [dbo].[tbgl_jnlmst]  WITH NOCHECK ADD  CONSTRAINT [tbgl_jnlmst_2] FOREIGN KEY([DEPARTMENT_BASE])
REFERENCES [dbo].[t_departmentbase] ([code])
GO

ALTER TABLE [dbo].[tbgl_jnlmst] NOCHECK CONSTRAINT [tbgl_jnlmst_2]
GO

-------------------------------------------------------------------------------

DROP TABLE [dbo].[tbgl_jnldd]



USE [KasabaKIAT]
GO

/****** Object:  Table [dbo].[tbgl_jnldd]    Script Date: 02/25/2018 21:15:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tbgl_jnldd](
	[TRDT] [datetime] NULL,
	[DEPARTMENT_CODE] [varchar](255) NULL,
	[DYSEQ] [int] NULL,
	[DYTRSEQ] [int] NULL,
	[ACCOUNT] [varchar](50) NULL,
	[DRCR] [char](1) NULL,
	[CCY] [char](3) NULL,
	[AMOUNT] [money] NULL,
	[REMARK] [varchar](500) NULL,
	[CRTUSER_ID] [bigint] NULL,
	[REC_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[INN] [varchar](9) NULL,
 CONSTRAINT [PK_tbgl_jnldd] PRIMARY KEY CLUSTERED 
(
	[REC_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tbgl_jnldd]  WITH NOCHECK ADD  CONSTRAINT [tbgl_jnldd_1] FOREIGN KEY([CRTUSER_ID])
REFERENCES [dbo].[users] ([id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[tbgl_jnldd] NOCHECK CONSTRAINT [tbgl_jnldd_1]
GO

ALTER TABLE [dbo].[tbgl_jnldd]  WITH NOCHECK ADD  CONSTRAINT [tbgl_jnldd_2] FOREIGN KEY([DEPARTMENT_CODE])
REFERENCES [dbo].[t_departmentbase] ([code])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[tbgl_jnldd] NOCHECK CONSTRAINT [tbgl_jnldd_2]
GO

