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

/****** Object:  Table [dbo].[tblCloseDayHistory]    Script Date: 04/28/2018 21:18:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblCloseDayHistory](
	[REC_ID] [bigint] IDENTITY(1,1) NOT NULL,
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

--------------------------------------

DROP TABLE [KasabaKIAT_N1].[dbo].[tblPred]

USE [KasabaKIAT_N1]
GO
/****** Object:  Table [dbo].[tblPred]    Script Date: 04/29/2018 18:35:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblPred](
	[ID_Prof] [bigint] NULL,
	[ID_Name] [nvarchar](50) NULL,
	[ID_Pred] [nvarchar](90) NULL,
	[ID_Pass] [nvarchar](10) NULL,
	[Adress] [nvarchar](50) NULL,
	[RS] [nvarchar](20) NULL,
	[Name_bank] [nvarchar](50) NULL,
	[MFO] [nvarchar](5) NULL,
	[INN] [nvarchar](9) NULL,
	[OKONX] [nvarchar](6) NULL,
	[Name_Dir] [nvarchar](25) NULL,
	[PutDay] [int] NULL,
	[VIO] [bit] NOT NULL,
	[SeverName] [nvarchar](60) NULL,
	[UserName] [nvarchar](50) NULL,
	[PasName] [nvarchar](50) NULL,
	[TipName] [nvarchar](50) NULL,
	[Vid_SSL] [bit] NOT NULL,
	[UserId] [bigint] NULL,
	[Departmentbase] [nvarchar](255) NOT NULL,
	[KaznaAccount] [varchar](50) NULL
 CONSTRAINT [PK_tblPred] PRIMARY KEY CLUSTERED 
(
	[Departmentbase] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO
INSERT [dbo].[tblPred] ([ID_Prof], [ID_Name], [ID_Pred], [ID_Pass], [Adress], [RS], [Name_bank], [MFO], [INN], [OKONX], [Name_Dir], [PutDay], [VIO], [SeverName], [UserName], [PasName], [TipName], [Vid_SSL], [UserId], [Departmentbase], [KaznaAccount]) VALUES (100, N'Тошкент шахар касаба уюшма ташкилотлари бирлашмаси', N'252-34-09,256-76-97', N'0', N'Бухора кучаси-24', N'20212000500136154006', N'Ипотека банк Шайхантохур филиали', N'00425', N'201273304', N'98300', N'', 730, 0, NULL, NULL, NULL, NULL, 0, 1, '1', NULL)
INSERT [dbo].[tblPred] ([ID_Prof], [ID_Name], [ID_Pred], [ID_Pass], [Adress], [RS], [Name_bank], [MFO], [INN], [OKONX], [Name_Dir], [PutDay], [VIO], [SeverName], [UserName], [PasName], [TipName], [Vid_SSL], [UserId], [Departmentbase], [KaznaAccount]) VALUES (100, N'АНДИЖАНСКИЙ ОБЛСОВПРОФ', NULL, N'0', NULL, NULL, NULL, N'00059', NULL, N'98300', N'', 730, 0, NULL, NULL, NULL, NULL, 0, 1, N'17203', N'20212000900184956019')
INSERT [dbo].[tblPred] ([ID_Prof], [ID_Name], [ID_Pred], [ID_Pass], [Adress], [RS], [Name_bank], [MFO], [INN], [OKONX], [Name_Dir], [PutDay], [VIO], [SeverName], [UserName], [PasName], [TipName], [Vid_SSL], [UserId], [Departmentbase], [KaznaAccount]) VALUES (100, N'БУХАРСКИЙ ОБЛСОВПРОФ', NULL, N'0', NULL, NULL, NULL, N'00088', NULL, N'98300', N'', 730, 0, NULL, NULL, NULL, NULL, 0, 1, N'17206', N'20212000600487320009')
INSERT [dbo].[tblPred] ([ID_Prof], [ID_Name], [ID_Pred], [ID_Pass], [Adress], [RS], [Name_bank], [MFO], [INN], [OKONX], [Name_Dir], [PutDay], [VIO], [SeverName], [UserName], [PasName], [TipName], [Vid_SSL], [UserId], [Departmentbase], [KaznaAccount]) VALUES (100, N'ДЖИЗАКСКИЙ ОБЛСОВПРОФ', NULL, N'0', NULL, NULL, NULL, N'01117', NULL, N'98300', N'', 730, 0, NULL, NULL, NULL, NULL, 0, 1, N'17208', N'20212000200400852012')
INSERT [dbo].[tblPred] ([ID_Prof], [ID_Name], [ID_Pred], [ID_Pass], [Adress], [RS], [Name_bank], [MFO], [INN], [OKONX], [Name_Dir], [PutDay], [VIO], [SeverName], [UserName], [PasName], [TipName], [Vid_SSL], [UserId], [Departmentbase], [KaznaAccount]) VALUES (100, N'КАРАКАЛПАКСКИЙ РЕССОВПРОФ', NULL, N'0', NULL, NULL, NULL, N'00621', NULL, N'98300', N'', 730, 0, NULL, NULL, NULL, NULL, 0, 1, N'17235', N'20212000700538352008')
INSERT [dbo].[tblPred] ([ID_Prof], [ID_Name], [ID_Pred], [ID_Pass], [Adress], [RS], [Name_bank], [MFO], [INN], [OKONX], [Name_Dir], [PutDay], [VIO], [SeverName], [UserName], [PasName], [TipName], [Vid_SSL], [UserId], [Departmentbase], [KaznaAccount]) VALUES (100, N'КАШКАДАРЬИНСКИЙ ОБЛСОВПРОФ', NULL, N'0', NULL, NULL, NULL, N'00190', NULL, N'98300', N'', 730, 0, NULL, NULL, NULL, NULL, 0, 1, N'17210', N'20212000100305583002')
INSERT [dbo].[tblPred] ([ID_Prof], [ID_Name], [ID_Pred], [ID_Pass], [Adress], [RS], [Name_bank], [MFO], [INN], [OKONX], [Name_Dir], [PutDay], [VIO], [SeverName], [UserName], [PasName], [TipName], [Vid_SSL], [UserId], [Departmentbase], [KaznaAccount]) VALUES (100, N'НАВОИЙСКИЙ ОБЛСОВПРОФ', NULL, N'0', NULL, NULL, NULL, N'00205', NULL, N'98300', N'', 730, 0, NULL, NULL, NULL, NULL, 0, 1, N'17212', N'20212000400506482001')
INSERT [dbo].[tblPred] ([ID_Prof], [ID_Name], [ID_Pred], [ID_Pass], [Adress], [RS], [Name_bank], [MFO], [INN], [OKONX], [Name_Dir], [PutDay], [VIO], [SeverName], [UserName], [PasName], [TipName], [Vid_SSL], [UserId], [Departmentbase], [KaznaAccount]) VALUES (100, N'НАМАНГАНСКИЙ ОБЛСОВПРОФ', NULL, N'0', NULL, NULL, NULL, N'00223', NULL, N'98300', N'', 730, 0, NULL, NULL, NULL, NULL, 0, 1, N'17214', N'20212000900467922002')
INSERT [dbo].[tblPred] ([ID_Prof], [ID_Name], [ID_Pred], [ID_Pass], [Adress], [RS], [Name_bank], [MFO], [INN], [OKONX], [Name_Dir], [PutDay], [VIO], [SeverName], [UserName], [PasName], [TipName], [Vid_SSL], [UserId], [Departmentbase], [KaznaAccount]) VALUES (100, N'ПК АООТ АГМК (Профсоюз соц.) (Алмалыкский ГМК)', NULL, N'0', NULL, NULL, NULL, NULL, NULL, N'98300', N'', 730, 0, NULL, NULL, NULL, NULL, 0, 1, N'173О1', NULL)
INSERT [dbo].[tblPred] ([ID_Prof], [ID_Name], [ID_Pred], [ID_Pass], [Adress], [RS], [Name_bank], [MFO], [INN], [OKONX], [Name_Dir], [PutDay], [VIO], [SeverName], [UserName], [PasName], [TipName], [Vid_SSL], [UserId], [Departmentbase], [KaznaAccount]) VALUES (100, N'Профсоюз работников НГМК', NULL, N'0', NULL, NULL, NULL, NULL, NULL, N'98300', N'', 730, 0, NULL, NULL, NULL, NULL, 0, 1, N'173Н2', NULL)
INSERT [dbo].[tblPred] ([ID_Prof], [ID_Name], [ID_Pred], [ID_Pass], [Adress], [RS], [Name_bank], [MFO], [INN], [OKONX], [Name_Dir], [PutDay], [VIO], [SeverName], [UserName], [PasName], [TipName], [Vid_SSL], [UserId], [Departmentbase], [KaznaAccount]) VALUES (100, N'РС Профсоюза ЖЕЛЕЗНОДОРОЖНИКОВ', NULL, N'0', NULL, NULL, NULL, NULL, NULL, N'98300', N'', 730, 0, NULL, NULL, NULL, NULL, 0, 1, N'173Т2', NULL)
INSERT [dbo].[tblPred] ([ID_Prof], [ID_Name], [ID_Pred], [ID_Pass], [Adress], [RS], [Name_bank], [MFO], [INN], [OKONX], [Name_Dir], [PutDay], [VIO], [SeverName], [UserName], [PasName], [TipName], [Vid_SSL], [UserId], [Departmentbase], [KaznaAccount]) VALUES (100, N'САМАРКАНДСКИЙ ОБЛСОВПРОФ', NULL, N'0', NULL, NULL, NULL, N'00985', NULL, N'98300', N'', 730, 0, NULL, NULL, NULL, NULL, 0, 1, N'17218', N'20212000900510123017')
INSERT [dbo].[tblPred] ([ID_Prof], [ID_Name], [ID_Pred], [ID_Pass], [Adress], [RS], [Name_bank], [MFO], [INN], [OKONX], [Name_Dir], [PutDay], [VIO], [SeverName], [UserName], [PasName], [TipName], [Vid_SSL], [UserId], [Departmentbase], [KaznaAccount]) VALUES (100, N'СУРХАНДАРЬИНСКИЙ ОБЛСОВПРОФ', NULL, N'0', NULL, NULL, NULL, N'01001', NULL, N'98300', N'', 730, 0, NULL, NULL, NULL, NULL, 0, 1, N'17222', N'20212000500361799005')
INSERT [dbo].[tblPred] ([ID_Prof], [ID_Name], [ID_Pred], [ID_Pass], [Adress], [RS], [Name_bank], [MFO], [INN], [OKONX], [Name_Dir], [PutDay], [VIO], [SeverName], [UserName], [PasName], [TipName], [Vid_SSL], [UserId], [Departmentbase], [KaznaAccount]) VALUES (100, N'СФП УЗБЕКИСТАНА,  ТАШКЕНТ', NULL, N'0', NULL, NULL, NULL, NULL, N'201685882', N'98300', N'', 730, 0, NULL, NULL, NULL, NULL, 0, 1, N'17', N'20212000100431324011')
INSERT [dbo].[tblPred] ([ID_Prof], [ID_Name], [ID_Pred], [ID_Pass], [Adress], [RS], [Name_bank], [MFO], [INN], [OKONX], [Name_Dir], [PutDay], [VIO], [SeverName], [UserName], [PasName], [TipName], [Vid_SSL], [UserId], [Departmentbase], [KaznaAccount]) VALUES (100, N'СЫРДАРЬИНСКИЙ ОБЛСОВПРОФ', NULL, N'0', NULL, NULL, NULL, N'01118', NULL, N'98300', N'', 730, 0, NULL, NULL, NULL, NULL, 0, 1, N'17224', N'20212000400134927009')
INSERT [dbo].[tblPred] ([ID_Prof], [ID_Name], [ID_Pred], [ID_Pass], [Adress], [RS], [Name_bank], [MFO], [INN], [OKONX], [Name_Dir], [PutDay], [VIO], [SeverName], [UserName], [PasName], [TipName], [Vid_SSL], [UserId], [Departmentbase], [KaznaAccount]) VALUES (100, N'ТАШГОРСОВЕТ Объед.профсоюзов', NULL, N'0', NULL, NULL, NULL, N'00425', NULL, N'98300', N'', 730, 0, NULL, NULL, NULL, NULL, 0, 1, N'17226', N'20212000800136154002')
INSERT [dbo].[tblPred] ([ID_Prof], [ID_Name], [ID_Pred], [ID_Pass], [Adress], [RS], [Name_bank], [MFO], [INN], [OKONX], [Name_Dir], [PutDay], [VIO], [SeverName], [UserName], [PasName], [TipName], [Vid_SSL], [UserId], [Departmentbase], [KaznaAccount]) VALUES (100, N'ТАШКЕНТСКИЙ ОБЛСОВПРОФ', NULL, N'0', NULL, NULL, NULL, N'00425', NULL, N'98300', N'', 730, 0, NULL, NULL, NULL, NULL, 0, 1, N'17227', N'20212000200431318004')
INSERT [dbo].[tblPred] ([ID_Prof], [ID_Name], [ID_Pred], [ID_Pass], [Adress], [RS], [Name_bank], [MFO], [INN], [OKONX], [Name_Dir], [PutDay], [VIO], [SeverName], [UserName], [PasName], [TipName], [Vid_SSL], [UserId], [Departmentbase], [KaznaAccount]) VALUES (100, N'ФЕРГАНСКИЙ ОБЛСОВПРОФ', NULL, N'0', NULL, NULL, NULL, N'00965', NULL, N'98300', N'', 730, 0, NULL, NULL, NULL, NULL, 0, 1, N'17230', N'20212000101947929017')
INSERT [dbo].[tblPred] ([ID_Prof], [ID_Name], [ID_Pred], [ID_Pass], [Adress], [RS], [Name_bank], [MFO], [INN], [OKONX], [Name_Dir], [PutDay], [VIO], [SeverName], [UserName], [PasName], [TipName], [Vid_SSL], [UserId], [Departmentbase], [KaznaAccount]) VALUES (100, N'ХОРЕЗМСКИЙ ОБЛСОВПРОФ', NULL, N'0', NULL, NULL, NULL, N'00579', NULL, N'98300', N'', 730, 0, NULL, NULL, NULL, NULL, 0, 1, N'17233', N'20212000200431876011')
/****** Object:  Default [DF__tblPred__ID_Prof__6E565CE8]    Script Date: 04/29/2018 18:35:27 ******/
ALTER TABLE [dbo].[tblPred] ADD  DEFAULT ((0)) FOR [ID_Prof]
GO
/****** Object:  Default [DF__tblPred__PutDay__6F4A8121]    Script Date: 04/29/2018 18:35:27 ******/
ALTER TABLE [dbo].[tblPred] ADD  DEFAULT ((0)) FOR [PutDay]
GO
/****** Object:  ForeignKey [FK_tblPred_t_departmentbase]    Script Date: 04/29/2018 18:35:27 ******/
--ALTER TABLE [dbo].[tblPred]  WITH CHECK ADD  CONSTRAINT [FK_tblPred_t_departmentbase] FOREIGN KEY([Departmentbase])
--REFERENCES [dbo].[t_departmentbase] ([code])
--GO
--ALTER TABLE [dbo].[tblPred] CHECK CONSTRAINT [FK_tblPred_t_departmentbase]
--GO
/****** Object:  ForeignKey [FK_tblPred_Users]    Script Date: 04/29/2018 18:35:27 ******/
ALTER TABLE [dbo].[tblPred]  WITH CHECK ADD  CONSTRAINT [FK_tblPred_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[tblPred] CHECK CONSTRAINT [FK_tblPred_Users]
GO
