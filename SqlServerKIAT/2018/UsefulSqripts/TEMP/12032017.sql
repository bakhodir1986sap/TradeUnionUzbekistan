USE [KasabaKIAT]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_FormaN13]    Script Date: 03/12/2018 19:29:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_Postupleniya]
@INN  varchar(9)

as 
begin 
set nocount on;

SELECT TOP 1000 [K_TYPE]
      ,[OPERDAY]
      ,k1.[ID]
      ,[BANKID]
      ,[KLSDOX]
      ,[DOCNUMB]
      ,[DOCDATE]
      ,[DOCTYPE]
      ,[SUMPAY]
      ,[CLMFO]
      ,[CLACC]
      ,[CLINN]
      ,[CLNAME]
      ,[COMFO]
      ,[COACC]
      ,[COINN]
      ,[CONAME]
      ,[PURPOSE]
      ,[CHAPTER]
      ,[OKONH]
      ,[FILE_ID]
      ,[OTCHISLEN]
      , k2.FileName
      , k2.CreateDate
  FROM [KasabaKIAT].[dbo].[KaznaPaymentFile] k1
  INNER JOIN dbo.KaznaFiles k2 on k1.FILE_ID = k2.ID
  WHERE CLINN = @INN;
  
end
