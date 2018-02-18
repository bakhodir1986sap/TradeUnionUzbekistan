 --update [KasabaKIAT].[dbo].[tblSprav_Otras] set = 'К1' where id =	Кимё саноати ходимлари касаба уюшмаси
 --update [KasabaKIAT].[dbo].[tblSprav_Otras] set = 'Н1' where id =	Нефт-газ ва геология ходимлари касаба уюшмаси
 --update [KasabaKIAT].[dbo].[tblSprav_Otras] set = 'О1' where id =	“Олмали? кон-металлургия комбинати” очи? акционерлик жамияти ходимлари касаба уюшмаси
 
 
  ALTER TABLE [KasabaKIAT].[dbo].[tblSprav_Otras]
  ADD BRANCH_CODE VARCHAR(10);


update [KasabaKIAT].[dbo].[tblSprav_Otras] set BRANCH_CODE= 'А1' where ID_Ot =	6;
update [KasabaKIAT].[dbo].[tblSprav_Otras] set BRANCH_CODE= 'Д1' where ID_Ot =	3;
update [KasabaKIAT].[dbo].[tblSprav_Otras] set BRANCH_CODE= 'И1' where ID_Ot =	4;
update [KasabaKIAT].[dbo].[tblSprav_Otras] set BRANCH_CODE= 'М1' where ID_Ot =	11;
update [KasabaKIAT].[dbo].[tblSprav_Otras] set BRANCH_CODE= 'С1' where ID_Ot =	8;
update [KasabaKIAT].[dbo].[tblSprav_Otras] set BRANCH_CODE= 'Т1' where ID_Ot =	5;
update [KasabaKIAT].[dbo].[tblSprav_Otras] set BRANCH_CODE= 'Т2' where ID_Ot =	10;
update [KasabaKIAT].[dbo].[tblSprav_Otras] set BRANCH_CODE= 'Т3' where ID_Ot =	1;
update [KasabaKIAT].[dbo].[tblSprav_Otras] set BRANCH_CODE= 'Э1' where ID_Ot =	9;
update [KasabaKIAT].[dbo].[tblSprav_Otras] set BRANCH_CODE= 'А2' where ID_Ot =	2;
update [KasabaKIAT].[dbo].[tblSprav_Otras] set BRANCH_CODE= 'А3' where ID_Ot =	7;


INSERT INTO [KasabaKIAT].[dbo].[tblSprav_Otras]
SELECT [ID_Ot]
      ,[Name_Ot]
      ,[Priz]
      ,[UserId]
      ,MATCH_DEPARTMENTBASE
      ,[BRANCH_CODE] 
FROM [KasabaKIAT].[dbo].[tblSprav_Otras]
CROSS JOIN [KasabaKIAT].dbo.tbku_StrokaProfsoyza
WHERE MATCH_DEPARTMENTBASE IS NOT NULL;




USE [KasabaKIAT]
GO

ALTER TABLE [dbo].[tblSprav_Vil]  WITH CHECK ADD  CONSTRAINT [FK_tblSprav_Vil_sp_soato] FOREIGN KEY([SOATO_CODE])
REFERENCES [dbo].[sp_soato] ([code])
GO

ALTER TABLE [dbo].[tblSprav_Vil] CHECK CONSTRAINT [FK_tblSprav_Vil_sp_soato]
GO

USE [KasabaKIAT]
GO

ALTER TABLE [dbo].[tblSprav_Reg]  WITH CHECK ADD  CONSTRAINT [FK_tblSprav_Reg_sp_soato] FOREIGN KEY([SOATO_CODE])
REFERENCES [dbo].[sp_soato] ([code])
GO

ALTER TABLE [dbo].[tblSprav_Reg] CHECK CONSTRAINT [FK_tblSprav_Reg_sp_soato]
GO

USE [KasabaKIAT]
GO

ALTER TABLE [dbo].[tblSprav_Otras]  WITH CHECK ADD  CONSTRAINT [FK_tblSprav_Otras_sp_branch] FOREIGN KEY([BRANCH_CODE])
REFERENCES [dbo].[sp_branch] ([code])
GO

ALTER TABLE [dbo].[tblSprav_Otras] CHECK CONSTRAINT [FK_tblSprav_Otras_sp_branch]
GO

