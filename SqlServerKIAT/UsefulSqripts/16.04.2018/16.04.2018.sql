USE [KasabaKIAT_N1]
GO

/****** Object:  Table [dbo].[tbcs_sisconf]    Script Date: 04/16/2018 09:06:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tbcs_sisconf](
	[ID] [smallint] IDENTITY(1,1) NOT NULL,
	[CONF_CODE] [varchar](50) NULL,
	[DESCRIPTION] [nvarchar](200) NULL,
	[VALUE] [varchar](50) NULL
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

-----------------------

  ALTER TABLE [KasabaKIAT_N1].[dbo].[tblPred]
  ADD KaznaAccount varchar(50);

  --------------------------------------------------------

  ALTER TABLE [KasabaKIAT_N1].[dbo].[tbbu_ecp_calc]
  ADD INN VARCHAR(20);
  
  ALTER TABLE [KasabaKIAT_N1].[dbo].[tbbu_ecp_calc]
  ADD SOATO VARCHAR(20);
  
  ALTER TABLE [KasabaKIAT_N1].[dbo].[tbbu_ecp_calc]
  ADD SOATO_NAME NVARCHAR(500);

  -------------------------------------------------------------


UPDATE [KasabaKIAT_N1].[dbo].[tblPred] SET [KaznaAccount] = '20212000900184956019', [MFO] = '00059' WHERE [Departmentbase] ='17203'; 
UPDATE [KasabaKIAT_N1].[dbo].[tblPred] SET [KaznaAccount] = '20212000600487320009', [MFO] = '00088' WHERE [Departmentbase] ='17206';
UPDATE [KasabaKIAT_N1].[dbo].[tblPred] SET [KaznaAccount] = '20212000200400852012', [MFO] = '01117' WHERE [Departmentbase] ='17208';
UPDATE [KasabaKIAT_N1].[dbo].[tblPred] SET [KaznaAccount] = '20212000700538352008', [MFO] = '00621' WHERE [Departmentbase] ='17235';
UPDATE [KasabaKIAT_N1].[dbo].[tblPred] SET [KaznaAccount] = '20212000100305583002', [MFO] = '00190' WHERE [Departmentbase] ='17210';  
UPDATE [KasabaKIAT_N1].[dbo].[tblPred] SET [KaznaAccount] = '20212000400506482001', [MFO] = '00205' WHERE [Departmentbase] ='17212';
UPDATE [KasabaKIAT_N1].[dbo].[tblPred] SET [KaznaAccount] = '20212000900467922002', [MFO] = '00223' WHERE [Departmentbase] ='17214';
UPDATE [KasabaKIAT_N1].[dbo].[tblPred] SET [KaznaAccount] = '20212000900510123017', [MFO] = '00985' WHERE [Departmentbase] ='17218';
UPDATE [KasabaKIAT_N1].[dbo].[tblPred] SET [KaznaAccount] = '20212000500361799005', [MFO] = '01001' WHERE [Departmentbase] ='17222';
UPDATE [KasabaKIAT_N1].[dbo].[tblPred] SET [KaznaAccount] = '20212000400134927009', [MFO] = '01118' WHERE [Departmentbase] ='17224';
UPDATE [KasabaKIAT_N1].[dbo].[tblPred] SET [KaznaAccount] = '20212000800136154002', [MFO] = '00425' WHERE [Departmentbase] ='17226';
UPDATE [KasabaKIAT_N1].[dbo].[tblPred] SET [KaznaAccount] = '20212000200431318004', [MFO] = '00425' WHERE [Departmentbase] ='17227';
UPDATE [KasabaKIAT_N1].[dbo].[tblPred] SET [KaznaAccount] = '20212000101947929017', [MFO] = '00965' WHERE [Departmentbase] ='17230';
UPDATE [KasabaKIAT_N1].[dbo].[tblPred] SET [KaznaAccount] = '20212000200431876011', [MFO] = '00579' WHERE [Departmentbase] ='17233';
UPDATE [KasabaKIAT_N1].[dbo].[tblPred] SET [KaznaAccount] = '20212000100431324011', [INN] = '201685882' WHERE [Departmentbase] ='17';


------------------------------------------------------

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





