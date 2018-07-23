---------
--Prixod,
--Rasxod
--Oplata
--Vozvrat
--Raznoska
--==================================
--Do not delete TashVil


select * 
into KasabaKIAT_N1.dbo.tblPos_Put_t
from KasabaKIAT_N1.dbo.tblPos_Put p 
where p.Departmentbase != '17227';

DELETE from KasabaKIAT_N1.dbo.tblPos_Put  
where Departmentbase != '17227';

-----------------------------------------------

select * 
into KasabaKIAT_N1.dbo.tblVozVrat_t
from KasabaKIAT_N1.dbo.tblVozVrat
where  DepartmentBase != '17227';

DELETE from KasabaKIAT_N1.dbo.tblVozVrat
where  DepartmentBase != '17227';

------------------------------------------------------

select * 
into KasabaKIAT_N1.dbo.tblPost_t
from KasabaKIAT_N1.dbo.tblPost p 
where p.Departmentbase != '17227';

DELETE from KasabaKIAT_N1.dbo.tblPost 
where Departmentbase != '17227';
  
  
  --------------------------------------------------------


select * 
into KasabaKIAT_N1.dbo.tblVipl_Put_t
from KasabaKIAT_N1.dbo.tblVipl_Put 
where DepartmentBase != '17227';

DELETE from KasabaKIAT_N1.dbo.tblVipl_Put 
where DepartmentBase != '17227';


---------------------------------------------------------

select * 
into KasabaKIAT_N1.dbo.tblVipl_t
from KasabaKIAT_N1.dbo.tblVipl
where DepartmentBase != '17227';

DELETE from KasabaKIAT_N1.dbo.tblVipl
where DepartmentBase != '17227';

------------------------------------------------------------

select * 
into KasabaKIAT_N1.dbo.tblOplata_Ras_t
from KasabaKIAT_N1.dbo.tblOplata_Ras 
where Departmentbase != '17227';



DELETE from KasabaKIAT_N1.dbo.tblOplata_Ras 
where Departmentbase != '17227';


-----------------------------------------------------------


select * 
into KasabaKIAT_N1.dbo.tblOplata_t
from KasabaKIAT_N1.dbo.tblOplata 
where Departmentbase != '17227';



DELETE from KasabaKIAT_N1.dbo.tblOplata
where Departmentbase != '17227';


---------------------------------------------------------


select * 
into KasabaKIAT_N1.dbo.tblFace_Med_t
from KasabaKIAT_N1.dbo.tblFace_Med
where Departmentbase != '17227';


DELETE  from KasabaKIAT_N1.dbo.tblFace_Med
where Departmentbase != '17227';


-------------------------------------------------------------


select * 
into KasabaKIAT_N1.dbo.tblFase_t
from KasabaKIAT_N1.dbo.tblFase 
where Departmentbase != '17227';


DELETE from KasabaKIAT_N1.dbo.tblFase 
where Departmentbase != '17227';

------------------------------------------------------------------
select * 
into KasabaKIAT_N1.dbo.tbgl_bankpayments_t
from KasabaKIAT_N1.dbo.tbgl_bankpayments
where DEPARTMENT_BASE != '17227';


delete from KasabaKIAT_N1.dbo.tbgl_bankpayments
where DEPARTMENT_BASE != '17227';


select * 
into KasabaKIAT_N1.dbo.tbbu_provodki_t
from KasabaKIAT_N1.dbo.tbbu_provodki
where DEPARTMENT_BASE != '17227' AND TRREF != 'ECP';


delete KasabaKIAT_N1.dbo.tbbu_provodki
where DEPARTMENT_BASE != '17227' AND TRREF != 'ECP';


--------------------------------------------------------------

select * 
into KasabaKIAT_N1.dbo.tbbu_balance_t
from KasabaKIAT_N1.dbo.tbbu_balance ;

UPDATE KasabaKIAT_N1.dbo.tbbu_balance 
SET DEBIT_BAL = 0,
DEBIT_COUNT = 0,
DEBIT_TRANS = 0,
CREDIT_BAL = 0,
CREDIT_COUNT = 0,
CREDIT_TRANS = 0
WHERE DEPARTMENT_BASE != '17227';
------------------------------------Provodki vse krome TashVil i ECP


/****** Script for SelectTopNRows command from SSMS  ******/
insert into [KasabaKIAT_N1].[dbo].[tblSprav_Otras]
SELECT 13
      ,N'Кимё саноати ходимлари касаба уюшмаси'
      ,[Priz]
      ,1
      ,[DepartmentBase]
      ,N'К1'
  FROM [KasabaKIAT_N1].[dbo].[tblSprav_Otras]
  where BRANCH_CODE = 'Т3';
  
  
  
insert into [KasabaKIAT_N1].[dbo].[tblSprav_Otras]
SELECT 14
      ,N'Нефт-газ ва геология ходимлари касаба уюшмаси'
      ,[Priz]
      ,1
      ,[DepartmentBase]
      ,N'Н1'
  FROM [KasabaKIAT_N1].[dbo].[tblSprav_Otras]
  where BRANCH_CODE = 'Т3';
  

  insert into [KasabaKIAT_N1].[dbo].[tblSprav_Otras]
SELECT 15
      ,N'Олмалиқ кон-металлургия комбинати ОАЖ ходимлари касаба уюшмаси'
      ,[Priz]
      ,1
      ,[DepartmentBase]
      ,N'О1'
  FROM [KasabaKIAT_N1].[dbo].[tblSprav_Otras]
  where BRANCH_CODE = 'Т3';
  
  
  
  
    insert into [KasabaKIAT_N1].[dbo].[tblSprav_Otras]
SELECT 16
      ,N'“Навоий кон-металлургия комбинати” ОАЖ ходимлари касаба уюшмаси'
      ,[Priz]
      ,1
      ,[DepartmentBase]
      ,N'Н2'
  FROM [KasabaKIAT_N1].[dbo].[tblSprav_Otras]
  where BRANCH_CODE = 'Т3';



insert into [KasabaKIAT_N1].[dbo].[tblSprav_Otras]
SELECT -1
      ,N'Не Определен'
      ,[Priz]
      ,1
      ,[DepartmentBase]
      ,null 
  FROM [KasabaKIAT_N1].[dbo].[tblSprav_Otras]
  where BRANCH_CODE = 'Т3';


  --------------------------------------------------------------------

USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Provodka_Summary]    Script Date: 07/20/2018 02:35:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_Organization_Summary]
@DEPARTMENT_BASE nvarchar(255)

as 
begin 
set nocount on;

SELECT case when Otr.Name_Ot IS null then 'Не определен' else Otr.Name_Ot end  as Branch, 
COUNT(*)  as colVo
from KasabaKIAT_N1.dbo.tblSprav_Org Org
left outer join
(SELECT * FROM KasabaKIAT_N1.dbo.tblSprav_Otras Otr1
WHERE Otr1.DepartmentBase = @DEPARTMENT_BASE AND Otr1.BRANCH_CODE is not null) Otr
on Org.ID_Ot = Otr.ID_Ot
WHERE Org.DepartmentBase = @DEPARTMENT_BASE
GROUP BY Otr.Name_Ot
  
end
