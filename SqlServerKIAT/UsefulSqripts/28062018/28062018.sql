--Executed


USE [KasabaKIAT_N1]
GO

/****** Object:  Table [dbo].[tbbu_provodki]    Script Date: 06/28/2018 17:52:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tbbu_balance](
	[REC_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DEPARTMENT_BASE] [nvarchar](255) NOT NULL,
	[CUR_YEAR] int NOT NULL,
	[CUR_KV]   int NOT NULL,
	[USER_ID] [bigint] NOT NULL,
	[ACCOUNT_CODE] [nvarchar](30) NOT NULL,
	[DEBIT_BAL] [money] NOT NULL,
	[CREDIT_BAL] [money] NOT NULL,
 CONSTRAINT [PK_tbbu_balance] PRIMARY KEY CLUSTERED 
(
	[REC_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

  INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionarytype]
           ([CODE]
           ,[NAME_UZ]
           ,[NAME_RU]
           ,[NAME_LT]
           ,[USER_ID])
     VALUES
           ('GL_ROWS'
           ,'Строки Главной книги'
           ,'Строки Главной книги'
           ,'Строки Главной книги'
           ,1)
GO




  INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionarytype]
           ([CODE]
           ,[NAME_UZ]
           ,[NAME_RU]
           ,[NAME_LT]
           ,[USER_ID])
     VALUES
           ('GL_COLS'
           ,'Колонки Главной книги'
           ,'Колонки Главной книги'
           ,'Колонки Главной книги'
           ,1)
GO


---------------------------------------------------------------------



INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('1' ,'001'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('2' ,'002'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('3' ,'003'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('4' ,'004'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('5' ,'005'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('6' ,'03'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('7' ,'0310'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('8' ,'04'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('9' ,'0410'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('10' ,'06'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('11' ,'061'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('12' ,'062'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('13' ,'063'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('14' ,'07'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('15' ,'09'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('16' ,'10'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('17' ,'11'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('18' ,'12'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('19' ,'18'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('20' ,'19'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('21' ,'20'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('22' ,'21'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('23' ,'22'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('24' ,'2210'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('25' ,'2220'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('26' ,'2230'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('27' ,'23'	,'','',14 ,1,1);
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary] ([CODE] ,[NAME_RU] ,[NAME_UZ] ,[NAME_LT] ,[DICTIONARY_TYPE_ID] ,[STATUS] ,[USER_ID]) VALUES ('28' ,'24'	,'','',14 ,1,1);

--------------------------------------------

INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary]
SELECT [REC_ID] as CODE
      ,[NAME_RU]
      ,[NAME_UZ]
      ,[NAME_LT]
      ,15
      ,[STATUS]
      ,[USER_ID]
  FROM [KasabaKIAT_N1].[dbo].[tbcs_dictionary]
  where DICTIONARY_TYPE_ID = 6;


--------------------------------------------------------------------------