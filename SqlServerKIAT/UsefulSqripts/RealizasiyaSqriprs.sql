  ALTER TABLE [KasabaKIAT].[dbo].[tbpt_putyovkarealizasiya]
  ADD ORG_INN VARCHAR(9);

USE [KasabaKIAT]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[tbpt_putyovkarealizasiya_3]') AND parent_object_id = OBJECT_ID(N'[dbo].[tbpt_putyovkarealizasiya]'))
ALTER TABLE [dbo].[tbpt_putyovkarealizasiya] DROP CONSTRAINT [tbpt_putyovkarealizasiya_3]
GO

USE [KasabaKIAT]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[tbpt_putyovkarealizasiya_5]') AND parent_object_id = OBJECT_ID(N'[dbo].[tbpt_putyovkarealizasiya]'))
ALTER TABLE [dbo].[tbpt_putyovkarealizasiya] DROP CONSTRAINT [tbpt_putyovkarealizasiya_5]
GO

USE [KasabaKIAT]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[tbpt_zayavki_2]') AND parent_object_id = OBJECT_ID(N'[dbo].[tbpt_zayavki]'))
ALTER TABLE [dbo].[tbpt_zayavki] DROP CONSTRAINT [tbpt_zayavki_2]
GO

USE [KasabaKIAT]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tbpt_yearlyplanotrasl_t_sanatorium]') AND parent_object_id = OBJECT_ID(N'[dbo].[tbpt_yearlyplanotrasl]'))
ALTER TABLE [dbo].[tbpt_yearlyplanotrasl] DROP CONSTRAINT [FK_tbpt_yearlyplanotrasl_t_sanatorium]
GO

