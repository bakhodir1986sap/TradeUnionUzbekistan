using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using KasabaKIAT.ViewModel.Buxgalteriya;
using KaznaFilesImporter;
using Epigov.Log;
using System.Data.Entity.Validation;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki.AutomaticTransactions.KaznaTransactions
{
    public class EcpCalculation
    {
        /// <summary>
        /// 
        /// </summary>
        public void CalcProcess()
        {
            var _logService = new FileLogService(typeof(Program));

            _logService.Info("Begin Calc======");

            try
            {
                List<string> egrpoInnEnter = new List<string>();
                List<string> orgInnEnter = new List<string>();

                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    //db.sp_im_KaznaNewOrg();

                    List<string> blackListInn = new List<string>();

                    var procList = db.sp_pr_GetNewTransactions();

                    var enterList = new List<EcpCalcViewModel>();

                    if (procList != null)
                    {
                        var model = new EcpCalcViewModel();

                        string innFromDtl = string.Empty;

                        foreach (var payment in procList.ToList())
                        {
                            Console.WriteLine(DateTime.Now.ToLongTimeString() +  payment.ID);
                            Console.WriteLine(DateTime.Now.ToLongTimeString() + payment.PURPOSE);
                            
                            //Clear Model
                            innFromDtl = string.Empty;
                            model.federation_amt = 0;
                            model.id = 0;
                            model.oblsovprof_amt = 0;
                            model.obl_department_base = string.Empty;
                            model.payment_id = 0;
                            model.profsouz_amt = 0;
                            model.provodka_id = 0;
                            model.remark = string.Empty;
                            model.inn = string.Empty;
                            model.soato = string.Empty;
                            model.soato_name = string.Empty;
                            model.operdate = null;
                            model.okonx_id = string.Empty;
                            model.okonx_name = string.Empty;
                            model.branch_code = string.Empty;
                            model.branch_name = string.Empty;

                            //Filling
                            model.payment_id = (long)payment.ID;
                            model.operdate = payment.OPERDAY;

                            //Inn Parsing
                            #region Inn Parsing
                            string bankAccountBalance = string.Empty;

                            if (!string.IsNullOrEmpty(payment.CLACC))
                            {
                                if (payment.CLACC.Length >= 5)
                                {
                                    bankAccountBalance = payment.CLACC.Substring(0, 5);
                                }
                            }

                            if (bankAccountBalance.Equals("20208") ||
                                bankAccountBalance.Equals("14301") ||
                                bankAccountBalance.Equals("13301") ||
                                bankAccountBalance.Equals("20210") ||
                                bankAccountBalance.Equals("20212") ||
                                bankAccountBalance.Equals("20214") ||
                                bankAccountBalance.Equals("20216") ||
                                bankAccountBalance.Equals("20218") ||
                                bankAccountBalance.Equals("20296") ||
                                bankAccountBalance.Equals("21506"))
                            {
                                model.remark += string.Format("INN:{0};", payment.CLINN);
                                innFromDtl = payment.CLINN;
                            }
                            else if (payment.CLINN.Equals("000000000"))
                            {
                                string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                                //other case try to get INN 
                                var Inn = numbers.FirstOrDefault(x => x.Length == 9);

                                if ((Inn != null) && (Inn.StartsWith("2") || (Inn.StartsWith("3"))))
                                {
                                    model.remark += string.Format("INN:{0};", Inn);
                                    innFromDtl = Inn;
                                }
                                else
                                {
                                    model.remark += string.Format("INN:{0};", payment.CLINN);
                                    innFromDtl = payment.CLINN;
                                }
                            }
                            else if (payment.PURPOSE.ToUpper().Contains("ИНН "))
                            {
                                string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                                //First 9 digit number As INN
                                var Inns = numbers.Where(x => x.Length == 9);

                                var posOfInn = payment.PURPOSE.ToUpper().IndexOf("ИНН ");

                                foreach (var srt in Inns.ToList())
                                {
                                    if (!(srt.StartsWith("2") || (srt.StartsWith("3"))))
                                        continue;

                                    var posOfInn2 = payment.PURPOSE.ToUpper().IndexOf(srt);

                                    if (posOfInn < posOfInn2)
                                    {
                                        model.remark += string.Format("INN:{0};", srt);
                                        innFromDtl = srt;
                                        break;
                                    }
                                }
                            }
                            else if (payment.PURPOSE.ToUpper().Contains("INN "))
                            {
                                string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                                //First 9 digit number As INN
                                var Inns = numbers.Where(x => x.Length == 9);

                                var posOfInn = payment.PURPOSE.ToUpper().IndexOf("INN");

                                foreach (var srt in Inns.ToList())
                                {
                                    if (!(srt.StartsWith("2") || (srt.StartsWith("3"))))
                                        continue;

                                    var posOfInn2 = payment.PURPOSE.ToUpper().IndexOf(srt);

                                    if (posOfInn < posOfInn2)
                                    {
                                        model.remark += string.Format("INN:{0};", srt);
                                        innFromDtl = srt;
                                        break;
                                    }
                                }
                            }
                            else if (payment.PURPOSE.ToUpper().Contains("СТИР "))
                            {
                                string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                                //First 9 digit number As INN
                                var Inns = numbers.Where(x => x.Length == 9);

                                var posOfInn = payment.PURPOSE.ToUpper().IndexOf("СТИР");

                                foreach (var srt in Inns.ToList())
                                {
                                    if (!(srt.StartsWith("2") || (srt.StartsWith("3"))))
                                        continue;

                                    var posOfInn2 = payment.PURPOSE.ToUpper().IndexOf(srt);

                                    if (posOfInn < posOfInn2)
                                    {
                                        model.remark += string.Format("INN:{0};", srt);
                                        innFromDtl = srt;
                                        break;
                                    }
                                }
                            }
                            else if (payment.PURPOSE.ToUpper().Contains("STIR "))
                            {
                                string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                                //First 9 digit number As INN
                                var Inns = numbers.Where(x => x.Length == 9);

                                var posOfInn = payment.PURPOSE.ToUpper().IndexOf("STIR");

                                foreach (var srt in Inns.ToList())
                                {
                                    if (!(srt.StartsWith("2") || (srt.StartsWith("3"))))
                                        continue;

                                    var posOfInn2 = payment.PURPOSE.ToUpper().IndexOf(srt);

                                    if (posOfInn < posOfInn2)
                                    {
                                        model.remark += string.Format("INN:{0};", srt);
                                        innFromDtl = srt;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                                //other case try to get INN 
                                var Inn = numbers.FirstOrDefault(x => x.Length == 9);

                                if ((Inn != null) && (Inn.StartsWith("2") || (Inn.StartsWith("3"))))
                                {
                                    model.remark += string.Format("INN:{0};", Inn);
                                    innFromDtl = Inn;
                                }
                                else
                                {
                                    model.remark += string.Format("INN:{0};", payment.CLINN);
                                    innFromDtl = payment.CLINN;
                                }
                            }

                            if (string.IsNullOrEmpty(innFromDtl))
                            {
                                //Problem with geting INN
                                model.remark += string.Format("INN:{0};", "PROBLEM");
                                model.inn = payment.CLINN; 
                            }
                            else if (payment.CLINN.Equals(innFromDtl))
                            {
                                model.inn = innFromDtl;
                                //Good
                            }
                            else
                            {
                                model.inn = innFromDtl;
                                blackListInn.Add(payment.CLINN);
                            }

                            #endregion

                            Console.WriteLine("Formula Calculation");
                            //Formula Calculation
                            #region Formula Calculation

                            decimal sumAmount = ((payment.SUMPAY.HasValue ? payment.SUMPAY.Value : 0) / 100);
                            decimal profsouzAmount = 0;
                            decimal federationAmount = 0;
                            decimal oblsovprofAmount = 0;

                            if (payment.PURPOSE.Contains("3212110"))
                            {
                                profsouzAmount = (decimal)((double)(sumAmount) * 0.4 / 100);
                            }
                            else if (payment.PURPOSE.Contains("3212120"))
                            {
                                profsouzAmount = (decimal)((double)(sumAmount) * 0.666666667 / 100);
                            }
                            else if (payment.PURPOSE.Contains("3212130"))
                            {
                                profsouzAmount = (decimal)((double)(sumAmount) * 0.666666667 / 100);
                            }
                            else
                            {
                                profsouzAmount = (decimal)((double)(sumAmount) * 0.4 / 100);
                            }

                            federationAmount = Math.Round(profsouzAmount * 30 / 100, 2);

                            oblsovprofAmount = Math.Round(profsouzAmount * 70 / 100, 2);

                            model.profsouz_amt = Math.Round(profsouzAmount, 2);
                            model.federation_amt = federationAmount;
                            model.oblsovprof_amt = oblsovprofAmount;

                            #endregion

                            //Region and Branch Difinition
                            #region Region and Branch Difinition

                            //Define region 1 stage in detail we mostly have Kaznacheystvo Account which contains soato

                            #region Oblast and Tuman Difinition
                            Console.WriteLine(DateTime.Now.ToLongTimeString() + " Oblast and Tuman Difinition");
                            var ppp = db.tblPreds.Where(p => p.Departmentbase == "17");

                            string federationAccount = string.Empty;

                            if (ppp.Any())
                            {
                                federationAccount = ppp.First().KaznaAccount;
                            }

                            var bankDatas = db.KaznaBankFiles.Where(k => k.GRPID == payment.GRPID);
                            Console.WriteLine(DateTime.Now.ToLongTimeString() +  " Oblast and Tuman Difinition2222");
                            if (bankDatas.Any())
                            {
                                //Har bitta GRPID buyicha 2 ta qator topiladi 70% ga 30%
                                var KKK = bankDatas.Where(ll => ll.COACC != federationAccount);

                                //Federasiya bumagani topilmasa kerak emas
                                if (!KKK.Any()) continue;

                                //Federasiya bumaganini ixtiyoriy birinchisi
                                var bank = KKK.First();

                                //
                                var preds = db.tblPreds.Where(p => p.KaznaAccount == bank.COACC);

                                if (preds.Any())
                                {
                                    
                                    //Shunaqa yo'l bilan departmentBase aniqlanadi
                                    model.obl_department_base = preds.First().Departmentbase;

                                    Console.WriteLine("DepartmentBase" + model.obl_department_base);
                                }
                                else
                                {
                                    Console.WriteLine("Topa Olmadi Depbase");
                                }
                            }

                            if (string.IsNullOrEmpty(model.obl_department_base))
                            {
                                _logService.Error("Empty Department Code ID = " + payment.ID + " GRPID = " + payment.GRPID);
                            }

                            Console.WriteLine(DateTime.Now.ToLongTimeString() +  " Oblast and Tuman Difinition333333");
                            var egrpos = db.t_egrpo.Where(e => e.inn == innFromDtl);

                            if (egrpos.Any())
                            {
                                var org = egrpos.First();

                                model.soato = org.soato_id;

                                if (!string.IsNullOrEmpty(model.soato))
                                {
                                    var lll = db.tbsp_soato_new.Where(s => s.KODI == model.soato).FirstOrDefault();

                                    if (lll != null)
                                    {
                                        model.soato_name = lll.NAME_RU;
                                    }
                                    else
                                    {
                                        model.soato_name = string.Empty;
                                    }
                                }
                                Console.WriteLine(DateTime.Now.ToLongTimeString() + " Oblast and Tuman Difinition4444444");
                                if (!string.IsNullOrEmpty(org.okonx_id))
                                {
                                    model.okonx_id = org.okonx_id;

                                    var okonx = db.sp_okonx.Where(o => o.code == org.okonx_id).First();

                                    model.okonx_name = okonx.name_ru;
                                    Console.WriteLine(DateTime.Now.ToLongTimeString() +  " Oblast and Tuman Difinition5555555");
                                    if (!string.IsNullOrEmpty(okonx.branch_code))
                                    {
                                        model.branch_code = okonx.branch_code;
                                        Console.WriteLine(DateTime.Now.ToLongTimeString() + " Oblast and Tuman Difinition66666666");
                                        model.branch_name = db.sp_branch.Where(b => b.code == okonx.branch_code).First().name_ru;
                                    }
                                }
                            }
                            else
                            {
                                if (egrpoInnEnter.Where(z=>z== innFromDtl).Any())
                                {
                                    //skip
                                }
                                else
                                {
                                    //add to egrpo
                                    var ee = new t_egrpo();
                                    Console.WriteLine(DateTime.Now.ToLongTimeString() + " Oblast and Tuman Difinition777777777");
                                    ee.version = 0;
                                    ee.inn = innFromDtl;
                                    ee.name_full = payment.CLNAME;

                                    db.t_egrpo.Add(ee);

                                    egrpoInnEnter.Add(innFromDtl);
                                }
                            }

                            //Search in tblSprav_Org
                            var orgsList = db.tblSprav_Org.Where(z => z.INN == innFromDtl);

                            if (orgsList.Any())
                            {
                                var org1 = orgsList.First();

                                if (org1.ID_Ot.HasValue)
                                {
                                    var currentOtrasl = db.tblSprav_Otras.Where(z => z.ID_Ot == org1.ID_Ot);

                                    if (currentOtrasl.Any())
                                    {
                                        var branchCode = currentOtrasl.First().BRANCH_CODE;

                                        var branchNameRow = db.sp_branch.Where(z => z.code == branchCode);

                                        if (branchNameRow.Any())
                                        {
                                            var branchName = branchNameRow.First().name_ru;

                                            model.branch_code = branchCode;
                                            Console.WriteLine(DateTime.Now.ToLongTimeString() + " Oblast and Tuman Difinition888888");
                                            model.branch_name = branchName;
                                        }
                                    }
                                }

                            }
                            else
                            {

                                if (orgInnEnter.Where(z => z == innFromDtl).Any())
                                {
                                    //skip
                                }
                                else
                                {
                                    var orrg = new tblSprav_Org();
                                    orrg.INN = innFromDtl;

                                    if (payment.CLNAME.Length > 100)
                                    {
                                        orrg.Name_Org = payment.CLNAME.Substring(0, 100);
                                    }
                                    else
                                    {
                                        orrg.Name_Org = payment.CLNAME;
                                    }

                                    if (string.IsNullOrEmpty(model.obl_department_base))
                                    {
                                        orrg.DepartmentBase = null;
                                    }
                                    else
                                    {
                                        orrg.DepartmentBase = model.obl_department_base;
                                    }

                                    orrg.ORIG_INN = innFromDtl;
                                    orrg.OKONX = model.okonx_id;


                                    db.tblSprav_Org.Add(orrg);

                                    orgInnEnter.Add(innFromDtl);
                                }
                            }

                            #endregion

                            #endregion
                            Console.WriteLine(DateTime.Now.ToLongTimeString() + " Oblast and Tuman Difinition88888888");
                            EcpCalcDataManager.Create(ref model, db);

                        }

                        try
                        {
                            db.SaveChanges();
                            orgInnEnter.Clear();
                            egrpoInnEnter.Clear();

                        }
                        catch (DbEntityValidationException e)
                        {
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    _logService.Error(string.Format("- Property: \"{0}\", Error: \"{1}\"",  ve.PropertyName, ve.ErrorMessage));
                                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                        ve.PropertyName, ve.ErrorMessage);
                                }
                            }
                            throw;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            _logService.Error(ex.ToString());
                        }
                        
                    }

                    //return;

                    #region Provodki
                    var provodki = new Provodki();

                    var trans = new ProvodkiViewModel();

                    var NotProcList1 = db.sp_pr_GetNewBankProvodki();

                    var NotProcList = NotProcList1.ToList();

                    if (NotProcList.Any())
                    {
                        foreach (var pp in NotProcList)
                        {
                            Console.WriteLine(pp.OPERDAY);
                            Console.WriteLine(pp.COINN);
                            Console.WriteLine("========================================");

                            trans.comment = string.Empty;
                            trans.department_base = string.Empty;
                            trans.inoutflg = false;
                            trans.number = string.Empty;
                            trans.platelinn = string.Empty;
                            trans.platelmfo = string.Empty;
                            trans.platelname = string.Empty;
                            trans.platelrsch = string.Empty;
                            trans.poluchinn = string.Empty;
                            trans.poluchmfo = string.Empty;
                            trans.poluchname = string.Empty;
                            trans.poluchrsch = string.Empty;
                            trans.rec_id = 0;
                            trans.summa = 0;
                            trans.tipoborota = false;
                            trans.tipscheta = false;
                            trans.trdt = DateTime.MinValue;
                            trans.user_id = 0;
                            trans.trref = "ECP";
                            trans.trrefno = pp.ID.ToString();

                            //Obshaya provodka
                            trans.comment = "Postupleniya po ECP";
                            trans.department_base = pp.Departmentbase;
                            trans.inoutflg = false;
                            trans.platelrsch = pp.Departmentbase == "17" ? "22" : "20";
                            trans.poluchrsch = "004";
                            trans.summa = pp.SUMPAY;

                            trans.user_id = 1;
                            trans.trdt = pp.OPERDAY;

                            ProvodkiDataManager.Create(ref trans);

                            EcpCalcDataManager.ModifyBank(pp.ID , trans.rec_id);

                        }
                    }

                    #endregion


                    #region Otrasl_Past change

                    Console.WriteLine("Otrasl Late Update");

                    var lastTimeRow = db.TimeShedulers.Where(z => z.id == 1).FirstOrDefault();

                    if (lastTimeRow != null)
                    {
                        var lastTime = lastTimeRow.LastChanged;

                        Console.WriteLine("lastTime: " + lastTime);

                        var newOrgChangedList = db.tblSprav_Org.Where(z => z.TIMESTAMP_CH >= lastTime);

                        if (newOrgChangedList.Any())
                        {
                            foreach(var org in newOrgChangedList)
                            {
                                if (org.ID_Ot.HasValue)
                                {
                                    var currentOtrasl = db.tblSprav_Otras.Where(z => z.ID_Ot == org.ID_Ot);

                                    if (currentOtrasl.Any())
                                    {
                                        var branchCode = currentOtrasl.First().BRANCH_CODE;

                                        Console.WriteLine("branchCode: " + branchCode);

                                        var branchNameRow = db.sp_branch.Where(z => z.code == branchCode);

                                        if (branchNameRow.Any())
                                        {
                                            var branchName = branchNameRow.First().name_ru;

                                            Console.WriteLine("branchName: " + branchName);

                                            var ecpChangeList = db.tbbu_ecp_calc.Where(z => z.INN == org.INN);

                                            if (ecpChangeList.Any())
                                            {
                                                foreach(var ecp in ecpChangeList)
                                                {
                                                    ecp.BRANCH_CODE = branchCode;
                                                    ecp.BRANCH_NAME = branchName;
                                                }

                                                
                                            }
                                        }
                                    }
                                }
                            }

                            lastTimeRow.LastChanged = DateTime.Now;

                            db.SaveChanges();
                        }
                    }

                    #endregion

                }
            }
            catch(Exception ex)
            {
                _logService.Error(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalcProcessN()
        {
            var _logService = new FileLogService(typeof(EcpCalculation));

            _logService.Info("CalcProcessN ====******");

            try
            {
                List<string> egrpoInnEnter = new List<string>();
                List<string> orgInnEnter = new List<string>();

                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    var procList = db.sp_pr_GetNewTransactionsN1();

                    if (procList != null)
                    {
                        var model = new EcpCalcViewModel();

                        string innFromDtl = string.Empty;

                        foreach (var payment in procList.ToList())
                        {

                            #region Clear Model
                            innFromDtl = string.Empty;
                            model.federation_amt = 0;
                            model.id = 0;
                            model.oblsovprof_amt = 0;
                            model.obl_department_base = string.Empty;
                            model.payment_id = 0;
                            model.profsouz_amt = 0;
                            model.provodka_id = 0;
                            model.remark = string.Empty;
                            model.inn = string.Empty;
                            model.soato = string.Empty;
                            model.soato_name = string.Empty;
                            model.operdate = null;
                            model.okonx_id = string.Empty;
                            model.okonx_name = string.Empty;
                            model.branch_code = string.Empty;
                            model.branch_name = string.Empty;
                            #endregion

                            #region FillData

                            model.payment_id        = (long)payment.PAYMENT_ID;
                            model.profsouz_amt      = payment.PROFSOUZ_AMT;
                            model.federation_amt    = payment.FEDERATION_AMT;
                            model.oblsovprof_amt    = payment.OBLSOVPROF_AMT;
                            model.soato             = payment.SOATO;
                            model.soato_name        = payment.SOATO_NAME;
                            model.obl_department_base = payment.OBL_DEPARTMENT_BASE;

                            model.operdate = payment.OPERDAY;

                            #endregion

                            #region Inn Parsing

                            string bankAccountBalance = string.Empty;

                            if (!string.IsNullOrEmpty(payment.CLACC))
                            {
                                if (payment.CLACC.Length >= 5)
                                {
                                    bankAccountBalance = payment.CLACC.Substring(0, 5);
                                }
                            }

                            if (bankAccountBalance.Equals("20208") ||
                                bankAccountBalance.Equals("14301") ||
                                bankAccountBalance.Equals("13301") ||
                                bankAccountBalance.Equals("20210") ||
                                bankAccountBalance.Equals("20212") ||
                                bankAccountBalance.Equals("20214") ||
                                bankAccountBalance.Equals("20216") ||
                                bankAccountBalance.Equals("20218") ||
                                bankAccountBalance.Equals("20296") ||
                                bankAccountBalance.Equals("21506"))
                            {
                                model.remark += string.Format("INN:{0};", payment.INN);
                                innFromDtl = payment.INN;
                            }
                            else if (payment.INN.Equals("000000000"))
                            {
                                string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                                //other case try to get INN 
                                var Inn = numbers.FirstOrDefault(x => x.Length == 9);

                                if ((Inn != null) && (Inn.StartsWith("2") || (Inn.StartsWith("3"))))
                                {
                                    model.remark += string.Format("INN:{0};", Inn);
                                    innFromDtl = Inn;
                                }
                                else
                                {
                                    model.remark += string.Format("INN:{0};", payment.INN);
                                    innFromDtl = payment.INN;
                                }
                            }
                            else if (payment.PURPOSE.ToUpper().Contains("ИНН "))
                            {
                                string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                                //First 9 digit number As INN
                                var Inns = numbers.Where(x => x.Length == 9);

                                var posOfInn = payment.PURPOSE.ToUpper().IndexOf("ИНН ");

                                foreach (var srt in Inns.ToList())
                                {
                                    if (!(srt.StartsWith("2") || (srt.StartsWith("3"))))
                                        continue;

                                    var posOfInn2 = payment.PURPOSE.ToUpper().IndexOf(srt);

                                    if (posOfInn < posOfInn2)
                                    {
                                        model.remark += string.Format("INN:{0};", srt);
                                        innFromDtl = srt;
                                        break;
                                    }
                                }
                            }
                            else if (payment.PURPOSE.ToUpper().Contains("INN "))
                            {
                                string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                                //First 9 digit number As INN
                                var Inns = numbers.Where(x => x.Length == 9);

                                var posOfInn = payment.PURPOSE.ToUpper().IndexOf("INN");

                                foreach (var srt in Inns.ToList())
                                {
                                    if (!(srt.StartsWith("2") || (srt.StartsWith("3"))))
                                        continue;

                                    var posOfInn2 = payment.PURPOSE.ToUpper().IndexOf(srt);

                                    if (posOfInn < posOfInn2)
                                    {
                                        model.remark += string.Format("INN:{0};", srt);
                                        innFromDtl = srt;
                                        break;
                                    }
                                }
                            }
                            else if (payment.PURPOSE.ToUpper().Contains("СТИР "))
                            {
                                string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                                //First 9 digit number As INN
                                var Inns = numbers.Where(x => x.Length == 9);

                                var posOfInn = payment.PURPOSE.ToUpper().IndexOf("СТИР");

                                foreach (var srt in Inns.ToList())
                                {
                                    if (!(srt.StartsWith("2") || (srt.StartsWith("3"))))
                                        continue;

                                    var posOfInn2 = payment.PURPOSE.ToUpper().IndexOf(srt);

                                    if (posOfInn < posOfInn2)
                                    {
                                        model.remark += string.Format("INN:{0};", srt);
                                        innFromDtl = srt;
                                        break;
                                    }
                                }
                            }
                            else if (payment.PURPOSE.ToUpper().Contains("STIR "))
                            {
                                string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                                //First 9 digit number As INN
                                var Inns = numbers.Where(x => x.Length == 9);

                                var posOfInn = payment.PURPOSE.ToUpper().IndexOf("STIR");

                                foreach (var srt in Inns.ToList())
                                {
                                    if (!(srt.StartsWith("2") || (srt.StartsWith("3"))))
                                        continue;

                                    var posOfInn2 = payment.PURPOSE.ToUpper().IndexOf(srt);

                                    if (posOfInn < posOfInn2)
                                    {
                                        model.remark += string.Format("INN:{0};", srt);
                                        innFromDtl = srt;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                                //other case try to get INN 
                                var Inn = numbers.FirstOrDefault(x => x.Length == 9);

                                if ((Inn != null) && (Inn.StartsWith("2") || (Inn.StartsWith("3"))))
                                {
                                    model.remark += string.Format("INN:{0};", Inn);
                                    innFromDtl = Inn;
                                }
                                else
                                {
                                    model.remark += string.Format("INN:{0};", payment.INN);
                                    innFromDtl = payment.INN;
                                }
                            }

                            if (string.IsNullOrEmpty(innFromDtl))
                            {
                                //Problem with geting INN
                                model.remark += string.Format("INN:{0};", "PROBLEM");
                                model.inn = payment.INN;
                            }
                            else if (payment.INN.Equals(innFromDtl))
                            {
                                model.inn = innFromDtl;
                                //Good
                            }
                            else
                            {
                                model.inn = innFromDtl;
                            }

                            #endregion

                            #region Branch Difinition

                            Console.WriteLine(DateTime.Now.ToLongTimeString() + " Oblast and Tuman Difinition333333");
                            var egrpos = db.t_egrpo.Where(e => e.inn == innFromDtl);

                            if (egrpos.Any())
                            {
                                var org = egrpos.First();

                                Console.WriteLine(DateTime.Now.ToLongTimeString() + " Oblast and Tuman Difinition4444444");
                                if (!string.IsNullOrEmpty(org.okonx_id))
                                {
                                    model.okonx_id = org.okonx_id;

                                    var okonx = db.sp_okonx.Where(o => o.code == org.okonx_id).First();

                                    model.okonx_name = okonx.name_ru;
                                    Console.WriteLine(DateTime.Now.ToLongTimeString() + " Oblast and Tuman Difinition5555555");
                                    if (!string.IsNullOrEmpty(okonx.branch_code))
                                    {
                                        model.branch_code = okonx.branch_code;
                                        Console.WriteLine(DateTime.Now.ToLongTimeString() + " Oblast and Tuman Difinition66666666");
                                        model.branch_name = db.sp_branch.Where(b => b.code == okonx.branch_code).First().name_ru;
                                    }
                                }
                            }
                            else
                            {
                                if (egrpoInnEnter.Where(z => z == innFromDtl).Any())
                                {
                                    //skip
                                }
                                else
                                {
                                    //add to egrpo
                                    var ee = new t_egrpo();
                                    Console.WriteLine(DateTime.Now.ToLongTimeString() + " Oblast and Tuman Difinition777777777");
                                    ee.version = 0;
                                    ee.inn = innFromDtl;
                                    ee.name_full = payment.CLNAME;
                                    ee.soato_id = model.soato;

                                    db.t_egrpo.Add(ee);

                                    egrpoInnEnter.Add(innFromDtl);
                                }
                            }

                            //Search in tblSprav_Org
                            var orgsList = db.tblSprav_Org.Where(z => z.INN == innFromDtl);

                            if (orgsList.Any())
                            {
                                var org1 = orgsList.First();

                                if (org1.ID_Ot.HasValue)
                                {
                                    var currentOtrasl = db.tblSprav_Otras.Where(z => z.ID_Ot == org1.ID_Ot);

                                    if (currentOtrasl.Any())
                                    {
                                        var branchCode = currentOtrasl.First().BRANCH_CODE;

                                        var branchNameRow = db.sp_branch.Where(z => z.code == branchCode);

                                        if (branchNameRow.Any())
                                        {
                                            var branchName = branchNameRow.First().name_ru;

                                            model.branch_code = branchCode;
                                            Console.WriteLine(DateTime.Now.ToLongTimeString() + " Oblast and Tuman Difinition888888");
                                            model.branch_name = branchName;
                                        }
                                    }
                                }

                            }
                            else
                            {

                                if (orgInnEnter.Where(z => z == innFromDtl).Any())
                                {
                                    //skip
                                }
                                else
                                {
                                    var orrg = new tblSprav_Org();
                                    orrg.INN = innFromDtl;

                                    if (payment.CLNAME.Length > 100)
                                    {
                                        orrg.Name_Org = payment.CLNAME.Substring(0, 100);
                                    }
                                    else
                                    {
                                        orrg.Name_Org = payment.CLNAME;
                                    }

                                    if (string.IsNullOrEmpty(model.obl_department_base))
                                    {
                                        orrg.DepartmentBase = null;
                                    }
                                    else
                                    {
                                        orrg.DepartmentBase = model.obl_department_base;
                                    }

                                    orrg.ORIG_INN = innFromDtl;
                                    orrg.OKONX = model.okonx_id;

                                    var region = db.tblSprav_Reg.Where(z => z.SOATO_CODE == model.soato);

                                    if (region.Any())
                                    {
                                        orrg.Region = region.First().CodeReg;
                                    }

                                    db.tblSprav_Org.Add(orrg);

                                    orgInnEnter.Add(innFromDtl);
                                }
                            }

                            #endregion

                            Console.WriteLine(DateTime.Now.ToLongTimeString() + "EcpCalcDataManager.Create ");
                            EcpCalcDataManager.Create(ref model, db);
                        }

                        try
                        {
                            db.SaveChanges();
                            orgInnEnter.Clear();
                            egrpoInnEnter.Clear();

                        }
                        catch (DbEntityValidationException e)
                        {
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    _logService.Error(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                        ve.PropertyName, ve.ErrorMessage);
                                }
                            }
                            throw;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            _logService.Error(ex.ToString());
                        }
                    }


                    #region Provodki
                    var provodki = new Provodki();

                    var trans = new ProvodkiViewModel();

                    var NotProcList1 = db.sp_pr_GetNewBankProvodki();

                    var NotProcList = NotProcList1.ToList();

                    if (NotProcList.Any())
                    {
                        foreach (var pp in NotProcList)
                        {
                            Console.WriteLine(pp.OPERDAY);
                            Console.WriteLine(pp.COINN);
                            Console.WriteLine("========================================");

                            trans.comment = string.Empty;
                            trans.department_base = string.Empty;
                            trans.inoutflg = false;
                            trans.number = string.Empty;
                            trans.platelinn = string.Empty;
                            trans.platelmfo = string.Empty;
                            trans.platelname = string.Empty;
                            trans.platelrsch = string.Empty;
                            trans.poluchinn = string.Empty;
                            trans.poluchmfo = string.Empty;
                            trans.poluchname = string.Empty;
                            trans.poluchrsch = string.Empty;
                            trans.rec_id = 0;
                            trans.summa = 0;
                            trans.tipoborota = false;
                            trans.tipscheta = false;
                            trans.trdt = DateTime.MinValue;
                            trans.user_id = 0;
                            trans.trref = "ECP";
                            trans.trrefno = pp.ID.ToString();

                            //Obshaya provodka
                            trans.comment = "Postupleniya po ECP";
                            trans.department_base = pp.Departmentbase;
                            trans.inoutflg = false;
                            trans.platelrsch = pp.Departmentbase == "17" ? "22" : "20";
                            trans.poluchrsch = "004";
                            trans.summa = pp.SUMPAY;

                            trans.user_id = 1;
                            trans.trdt = pp.OPERDAY;

                            ProvodkiDataManager.Create(ref trans);

                            EcpCalcDataManager.ModifyBank(pp.ID, trans.rec_id);

                        }
                    }

                    #endregion
                    
                    #region Otrasl_Past change

                    Console.WriteLine("Otrasl Late Update");

                    var lastTimeRow = db.TimeShedulers.Where(z => z.id == 1).FirstOrDefault();

                    if (lastTimeRow != null)
                    {
                        var lastTime = lastTimeRow.LastChanged;

                        Console.WriteLine("lastTime: " + lastTime);

                        var newOrgChangedList = db.tblSprav_Org.Where(z => z.TIMESTAMP_CH >= lastTime);

                        if (newOrgChangedList.Any())
                        {
                            foreach (var org in newOrgChangedList)
                            {
                                if (org.ID_Ot.HasValue)
                                {
                                    var currentOtrasl = db.tblSprav_Otras.Where(z => z.ID_Ot == org.ID_Ot);

                                    if (currentOtrasl.Any())
                                    {
                                        var branchCode = currentOtrasl.First().BRANCH_CODE;

                                        Console.WriteLine("branchCode: " + branchCode);

                                        var branchNameRow = db.sp_branch.Where(z => z.code == branchCode);

                                        if (branchNameRow.Any())
                                        {
                                            var branchName = branchNameRow.First().name_ru;

                                            Console.WriteLine("branchName: " + branchName);

                                            var ecpChangeList = db.tbbu_ecp_calc.Where(z => z.INN == org.INN);

                                            if (ecpChangeList.Any())
                                            {
                                                foreach (var ecp in ecpChangeList)
                                                {
                                                    ecp.BRANCH_CODE = branchCode;
                                                    ecp.BRANCH_NAME = branchName;
                                                }


                                            }
                                        }
                                    }
                                }
                            }

                            lastTimeRow.LastChanged = DateTime.Now;

                            db.SaveChanges();
                        }
                    }

                    #endregion
                }
            }
            catch(Exception ex)
            {
                _logService.Info(ex.ToString());
            }





        }

    }
}