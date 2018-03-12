USE [KasabaKIAT]
GO

/****** Object:  Table [dbo].[KaznaSverkaFile]    Script Date: 03/09/2018 00:40:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[KaznaSverkaFile](
	[REC_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[K_TYPE] [varchar](3) NULL,
	[K_OPERDAY] [date] NULL,
	[FILE_NAME] [varchar](50) NULL,
	[CREATE_DATETIME] [varchar](50) NULL,
	[FILE_ID] [bigint] NULL,
 CONSTRAINT [PK_KaznaSverkaFile] PRIMARY KEY CLUSTERED 
(
	[REC_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[KaznaSverkaFile]  WITH CHECK ADD  CONSTRAINT [FK_KaznaSverkaFile_KaznaFiles] FOREIGN KEY([FILE_ID])
REFERENCES [dbo].[KaznaFiles] ([ID])
GO

ALTER TABLE [dbo].[KaznaSverkaFile] CHECK CONSTRAINT [FK_KaznaSverkaFile_KaznaFiles]
GO

-------------------------------------------------------------------

USE [KasabaKIAT]
GO

/****** Object:  Index [index_inn]    Script Date: 03/09/2018 00:47:41 ******/
CREATE NONCLUSTERED INDEX [index_inn] ON [dbo].[t_egrpo] 
(
	[inn] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

-----------------------------------------------------------------------

USE [KasabaKIAT]
GO

/****** Object:  Index [UniqueID]    Script Date: 03/09/2018 00:57:06 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UniqueID] ON [dbo].[KaznaPaymentFile] 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

-----------------------------------------------------------------------------

USE [KasabaKIAT]
GO

/****** Object:  Index [Unique_FileName]    Script Date: 03/09/2018 01:08:11 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Unique_FileName] ON [dbo].[KaznaFiles] 
(
	[FileName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

-------------------------------------------------

USE [KasabaKIAT]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_ZayavkiList]    Script Date: 03/09/2018 01:49:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_im_KaznaNewOrg]
as 
begin 
set nocount on;

insert into dbo.t_egrpo(version, s_create, name_full, inn)
select 0 , GETDATE() , CLNAME, CLINN from dbo.KaznaPaymentFile
where NOT EXISTS (select inn from dbo.t_egrpo where inn = CLINN)
group by CLNAME, CLINN;  
  
end
