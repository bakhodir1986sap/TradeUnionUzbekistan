select * 
into KasabaKIAT_N1.dbo.tblPos_Put_t2
from KasabaKIAT_N1.dbo.tblPos_Put p;


select * 
into KasabaKIAT_N1.dbo.tblVozVrat_t2
from KasabaKIAT_N1.dbo.tblVozVrat;


select * 
into KasabaKIAT_N1.dbo.tblPost_t2
from KasabaKIAT_N1.dbo.tblPost p;



select * 
into KasabaKIAT_N1.dbo.tblVipl_Put_t2
from KasabaKIAT_N1.dbo.tblVipl_Put;


select * 
into KasabaKIAT_N1.dbo.tblVipl_t2
from KasabaKIAT_N1.dbo.tblVipl;


select * 
into KasabaKIAT_N1.dbo.tblOplata_Ras_t2
from KasabaKIAT_N1.dbo.tblOplata_Ras;


select * 
into KasabaKIAT_N1.dbo.tblOplata_t2
from KasabaKIAT_N1.dbo.tblOplata;


select * 
into KasabaKIAT_N1.dbo.tblFace_Med_t2
from KasabaKIAT_N1.dbo.tblFace_Med;


select * 
into KasabaKIAT_N1.dbo.tblFase_t2
from KasabaKIAT_N1.dbo.tblFase;

select * 
into KasabaKIAT_N1.dbo.tbgl_bankpayments_t2
from KasabaKIAT_N1.dbo.tbgl_bankpayments;


select * 
into KasabaKIAT_N1.dbo.tbbu_provodki_t2
from KasabaKIAT_N1.dbo.tbbu_provodki;


select * 
into KasabaKIAT_N1.dbo.tbbu_balance_t2
from KasabaKIAT_N1.dbo.tbbu_balance ;


-------------------------------------------------------------------------------------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_im_KaznaNewOrg]    Script Date: 07/26/2018 20:05:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_str_Clearing]

@department nvarchar(255)
as 
begin 
set nocount on;

	DELETE from KasabaKIAT_N1.dbo.tblPos_Put  
	where Departmentbase = @department;

-----------------------------------------------
	DELETE from KasabaKIAT_N1.dbo.tblVozVrat
	where  DepartmentBase = @department;

------------------------------------------------------
	DELETE from KasabaKIAT_N1.dbo.tblPost 
	where Departmentbase = @department;
  
  
  --------------------------------------------------------
	DELETE from KasabaKIAT_N1.dbo.tblVipl_Put 
	where DepartmentBase = @department;


---------------------------------------------------------
	DELETE from KasabaKIAT_N1.dbo.tblVipl
	where DepartmentBase = @department;

------------------------------------------------------------
	DELETE from KasabaKIAT_N1.dbo.tblOplata_Ras 
	where Departmentbase = @department;


-----------------------------------------------------------
	DELETE from KasabaKIAT_N1.dbo.tblOplata
	where Departmentbase = @department;


---------------------------------------------------------
	DELETE  from KasabaKIAT_N1.dbo.tblFace_Med
	where Departmentbase = @department;


-------------------------------------------------------------
	DELETE from KasabaKIAT_N1.dbo.tblFase 
	where Departmentbase = @department;

------------------------------------------------------------------
	delete from KasabaKIAT_N1.dbo.tbgl_bankpayments
	where DEPARTMENT_BASE = @department;

	delete KasabaKIAT_N1.dbo.tbbu_provodki
	where DEPARTMENT_BASE = @department;


--------------------------------------------------------------
	UPDATE KasabaKIAT_N1.dbo.tbbu_balance 
	SET DEBIT_BAL = 0,
		DEBIT_COUNT = 0,
		DEBIT_TRANS = 0,
		CREDIT_BAL = 0,
		CREDIT_COUNT = 0,
		CREDIT_TRANS = 0
	WHERE DEPARTMENT_BASE = @department;
  
end
