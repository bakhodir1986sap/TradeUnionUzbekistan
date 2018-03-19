USE [KasabaKIAT]
GO

/****** Object:  Table [dbo].[PrivyazkaProvodok]    Script Date: 03/18/2018 13:01:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PrivyazkaProvodok](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[JO_REF] [bigint] NULL,
	[OWNER_DEPARTMENTBASE] [varchar](10) NULL,
	[PROVODOK_ID] [int] NULL,
	[USER_ID] [bigint] NULL,
 CONSTRAINT [PK_PrivyazkaProvodok] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[PrivyazkaProvodok]  WITH CHECK ADD  CONSTRAINT [FK_PrivyazkaProvodok_provodok] FOREIGN KEY([PROVODOK_ID])
REFERENCES [dbo].[provodok] ([id])
GO

ALTER TABLE [dbo].[PrivyazkaProvodok] CHECK CONSTRAINT [FK_PrivyazkaProvodok_provodok]
GO

ALTER TABLE [dbo].[PrivyazkaProvodok]  WITH CHECK ADD  CONSTRAINT [FK_PrivyazkaProvodok_sp_soato] FOREIGN KEY([OWNER_DEPARTMENTBASE])
REFERENCES [dbo].[sp_soato] ([code])
GO

ALTER TABLE [dbo].[PrivyazkaProvodok] CHECK CONSTRAINT [FK_PrivyazkaProvodok_sp_soato]
GO

ALTER TABLE [dbo].[PrivyazkaProvodok]  WITH CHECK ADD  CONSTRAINT [FK_PrivyazkaProvodok_tbcs_dictionary] FOREIGN KEY([JO_REF])
REFERENCES [dbo].[tbcs_dictionary] ([REC_ID])
GO

ALTER TABLE [dbo].[PrivyazkaProvodok] CHECK CONSTRAINT [FK_PrivyazkaProvodok_tbcs_dictionary]
GO

----------------------------------

USE [KasabaKIAT]
GO

/****** Object:  Table [dbo].[tbbu_ecp_calc]    Script Date: 03/18/2018 13:29:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tbbu_ecp_calc](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[PAYMENT_ID] [bigint] NULL,
	[PROFSOUZ_AMT] [money] NULL,
	[FEDERATION_AMT] [money] NULL,
	[OBLSOVPROF_AMT] [money] NULL,
	[OBL_DEPARTMENT_BASE] [varchar](255) NULL,
	[PROVODKA_ID] [bigint] NULL,
 CONSTRAINT [PK_tbbu_ecp_calc] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

--------------------------

USE [KasabaKIAT]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_pr_GetNewTransactions]
as 
begin 
set nocount on;

SELECT K.ID, K.SUMPAY , K.CLINN , COINN , K.PURPOSE 
FROM dbo.KaznaPaymentFile K
WHERE  K.ID NOT IN (SELECT EE.[PAYMENT_ID] FROM dbo.tbbu_ecp_calc EE);
  
end

---------------------
USE [KasabaKIAT]
GO
/****** Object:  StoredProcedure [dbo].[sp_pr_GetNewTransactions]    Script Date: 03/18/2018 21:03:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_pr_GetNewTransactions]
as 
begin 
set nocount on;

SELECT K.ID, K.SUMPAY , K.CLINN , COINN , K.PURPOSE , K.CLACC
FROM dbo.KaznaPaymentFile K
WHERE  K.ID NOT IN (SELECT EE.[PAYMENT_ID] FROM dbo.tbbu_ecp_calc EE);
  
end
