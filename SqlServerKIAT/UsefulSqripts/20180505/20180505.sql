INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionarytype]
           ([CODE]
           ,[NAME_UZ]
           ,[NAME_RU]
           ,[NAME_LT]
           ,[USER_ID])
     VALUES
           ('TipOplati'
           ,'Тип оплаты'
           ,'Тип оплаты'
           ,'Тип оплаты'
           ,1);


INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary]
           ([CODE]
           ,[NAME_RU]
           ,[NAME_UZ]
           ,[NAME_LT]
           ,[DICTIONARY_TYPE_ID]
           ,[STATUS]
           ,[USER_ID])
     VALUES
           ('01'
           ,'Взаиморасчеты'
           ,'Взаиморасчеты'
           ,'Взаиморасчеты'
           ,7
           ,1
           ,1);
GO


INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary]
           ([CODE]
           ,[NAME_RU]
           ,[NAME_UZ]
           ,[NAME_LT]
           ,[DICTIONARY_TYPE_ID]
           ,[STATUS]
           ,[USER_ID])
     VALUES
           ('02'
           ,'Оплата Путевок'
           ,'Оплата Путевок'
           ,'Оплата Путевок'
           ,7
           ,1
           ,1);
GO

INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionarytype]
           ([CODE]
           ,[NAME_UZ]
           ,[NAME_RU]
           ,[NAME_LT]
           ,[USER_ID])
     VALUES
           ('VidOplati'
           ,'Вид оплаты'
           ,'Вид оплаты'
           ,'Вид оплаты'
           ,1);
           
           
INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary]
           ([CODE]
           ,[NAME_RU]
           ,[NAME_UZ]
           ,[NAME_LT]
           ,[DICTIONARY_TYPE_ID]
           ,[STATUS]
           ,[USER_ID])
     VALUES
           ('01'
           ,'Определен'
           ,'Определен'
           ,'Определен'
           ,8
           ,1
           ,1);
GO


INSERT INTO [KasabaKIAT_N1].[dbo].[tbcs_dictionary]
           ([CODE]
           ,[NAME_RU]
           ,[NAME_UZ]
           ,[NAME_LT]
           ,[DICTIONARY_TYPE_ID]
           ,[STATUS]
           ,[USER_ID])
     VALUES
           ('02'
           ,'Не определен'
           ,'Не определен'
           ,'Не определен'
           ,8
           ,1
           ,1);
GO           
           


           
