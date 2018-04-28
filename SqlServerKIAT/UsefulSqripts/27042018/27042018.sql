USE [KasabaKIAT_N1]
GO

/****** Object:  Table [dbo].[tblHolidays]    Script Date: 04/28/2018 10:45:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblHolidays](
	[REC_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SET_DATE] [datetime] NULL,
	[DATE_TYPE] [smallint] NULL,
	[STATUS] [smallint] NULL,
 CONSTRAINT [PK_tblHolidays] PRIMARY KEY CLUSTERED 
(
	[REC_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 - Holiday; 2 - Changed WeekEnd to Workday' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblHolidays', @level2type=N'COLUMN',@level2name=N'DATE_TYPE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 - Aktive; 2 - Not Aktive' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblHolidays', @level2type=N'COLUMN',@level2name=N'STATUS'
GO

USE [KasabaKIAT_N1]
GO

/****** Object:  Index [UI_Holidate]    Script Date: 04/28/2018 10:46:18 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UI_Holidate] ON [dbo].[tblHolidays] 
(
	[SET_DATE] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

----------------------------------------------------------------


USE [KasabaKIAT_N1]
GO

/****** Object:  Table [dbo].[tblCloseDay]    Script Date: 04/28/2018 12:37:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblCloseDay](
	[REC_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DEPARTMENT_CODE] [varchar](255) NULL,
	[PREVDATE] [datetime] NULL,
	[CURDAY] [datetime] NULL,
	[NEXTDAY] [datetime] NULL,
 CONSTRAINT [PK_tblCloseDay] PRIMARY KEY CLUSTERED 
(
	[REC_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


USE [KasabaKIAT_N1]
GO

/****** Object:  Index [UI_DepartmentCode]    Script Date: 04/28/2018 12:38:06 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UI_DepartmentCode] ON [dbo].[tblCloseDay] 
(
	[DEPARTMENT_CODE] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO




INSERT INTO [KasabaKIAT_N1].dbo.tblCloseDay
SELECT [MATCH_DEPARTMENTBASE],
       GETDATE() -1 as PREVDATE,
       GETDATE() as CURDAY,
       GETDATE() + 1 AS NEXTDAY
  FROM [KasabaKIAT_N1].[dbo].[tbku_StrokaProfsoyza]
  where [MATCH_DEPARTMENTBASE] IS NOT NULL;

----------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO

/****** Object:  Table [dbo].[tblCloseDayHistory]    Script Date: 04/28/2018 12:59:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblCloseDayHistory](
	[REC_ID] [bigint] NOT NULL,
	[DEPARTMENT_CODE] [varchar](255) NULL,
	[CLOSED_DATE] [date] NULL,
	[TIMESTAMP] [datetime] NULL,
	[USER_ID] [bigint] NULL,
	[REMARK] [nvarchar](4000) NULL,
 CONSTRAINT [PK_tblCloseDayHistory] PRIMARY KEY CLUSTERED 
(
	[REC_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


