  ALTER TABLE [KasabaKIAT_N1].[dbo].[tbgl_bankpayments]
  ADD PROVODKA_ID bigint;


USE [KasabaKIAT_N1]
GO

ALTER TABLE [dbo].[tbgl_bankpayments]  WITH CHECK ADD  CONSTRAINT [FK_tbgl_bankpayments_tbbu_provodki] FOREIGN KEY([PROVODKA_ID])
REFERENCES [dbo].[tbbu_provodki] ([REC_ID])
GO

ALTER TABLE [dbo].[tbgl_bankpayments] CHECK CONSTRAINT [FK_tbgl_bankpayments_tbbu_provodki]
GO

----------------------------------------------------------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Provodka_Details]    Script Date: 07/11/2018 18:11:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_rt_Provodka_Details]
@DEPARTMENT_BASE nvarchar(255),
@BEGIN_DATE Datetime,
@END_DATE Datetime

as 
begin 
set nocount on;

select REPLACE(CONVERT(NVARCHAR, pro.TRDT, 103), '/', '.') as TRDT,
       pro.PLATELRSCH as DEBIT_ACCOUNT,
       pro.POLUCHRSCH as CREDIT_ACCOUNT,
       pro.SUMMA      as AMOUNT,
       pro.COMMENT	  as DETALI,
       pro.TRREFNO    as ADDITIONAL,
       pp.description as OPISANIYE,
       dd.NAME_RU
from KasabaKIAT_N1.dbo.tbbu_provodki pro
left outer join KasabaKIAT_N1.dbo.p_provodok pp
on pro.PLATELRSCH = pp.score_begin AND pro.POLUCHRSCH = pp.score_end
left outer join KasabaKIAT_N1.dbo.tbcs_dictionary dd
on pp.[order] = dd.REC_ID
where pro.DEPARTMENT_BASE LIKE @DEPARTMENT_BASE + '%' 
and   CONVERT(DATE , pro.TRDT) between  CONVERT(DATE , @BEGIN_DATE) 
AND CONVERT(DATE , @END_DATE);
  
end



-------------------------------------------------------------------------------


USE [KasabaKIAT_N1]
GO
/****** Object:  StoredProcedure [dbo].[sp_rt_Razbivka_PostupN]    Script Date: 07/11/2018 12:31:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_rt_Provodka_Summary]
@DEPARTMENT_BASE nvarchar(255),
@BEGIN_DATE Datetime,
@END_DATE Datetime

as 
begin 
set nocount on;

select pro.PLATELRSCH as DEBIT_ACCOUNT,
       pro.POLUCHRSCH as CREDIT_ACCOUNT,
       SUM(pro.SUMMA)      as AMOUNT
from KasabaKIAT_N1.dbo.tbbu_provodki pro
where pro.DEPARTMENT_BASE = @DEPARTMENT_BASE
and   CONVERT(DATE , pro.TRDT) between  CONVERT(DATE , @BEGIN_DATE) 
AND CONVERT(DATE , @END_DATE)
GROUP BY pro.PLATELRSCH, pro.POLUCHRSCH;;
  
end
