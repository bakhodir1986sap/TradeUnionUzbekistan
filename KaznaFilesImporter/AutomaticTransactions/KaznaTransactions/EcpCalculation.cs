using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using KasabaKIAT.ViewModel.Buxgalteriya;
using KaznaFilesImporter;
using Epigov.Log;
using System.Data.Entity.Validation;
using System.Threading;
using KasabaKIAT.BusinessLogic.BankOplati.Kaznacheystvo;
using KasabaKIAT.BusinessLogic.Structure;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki.AutomaticTransactions.KaznaTransactions
{
    public class EcpCalculation
    {
        FileLogService _logService = new FileLogService(typeof(EcpCalculation));
        KaznaController kk { get; set; }
        /// <summary>
        /// Old one
        /// </summary>
        //public void CalcProcess()
        //{
        //    var _logService = new FileLogService(typeof(Program));

        //    _logService.Info("Begin Calc======");

        //    try
        //    {
        //        List<string> egrpoInnEnter = new List<string>();
        //        List<string> orgInnEnter = new List<string>();

        //        using (KasabaKIATEntities db = new KasabaKIATEntities())
        //        {
        //            //db.sp_im_KaznaNewOrg();

        //            List<string> blackListInn = new List<string>();

        //            var procList = db.sp_pr_GetNewTransactions();

        //            var enterList = new List<EcpCalcViewModel>();

        //            if (procList != null)
        //            {
        //                var model = new EcpCalcViewModel();

        //                string innFromDtl = string.Empty;

        //                foreach (var payment in procList.ToList())
        //                {
        //                    Console.WriteLine(DateTime.Now.ToLongTimeString() +  payment.ID);
        //                    Console.WriteLine(DateTime.Now.ToLongTimeString() + payment.PURPOSE);
                            
        //                    //Clear Model
        //                    innFromDtl = string.Empty;
        //                    model.federation_amt = 0;
        //                    model.id = 0;
        //                    model.oblsovprof_amt = 0;
        //                    model.obl_department_base = string.Empty;
        //                    model.payment_id = 0;
        //                    model.profsouz_amt = 0;
        //                    model.provodka_id = 0;
        //                    model.remark = string.Empty;
        //                    model.inn = string.Empty;
        //                    model.soato = string.Empty;
        //                    model.soato_name = string.Empty;
        //                    model.operdate = null;
        //                    model.okonx_id = string.Empty;
        //                    model.okonx_name = string.Empty;
        //                    model.branch_code = string.Empty;
        //                    model.branch_name = string.Empty;

        //                    //Filling
        //                    model.payment_id = (long)payment.ID;
        //                    model.operdate = payment.OPERDAY;

        //                    //Inn Parsing
        //                    #region Inn Parsing
        //                    string bankAccountBalance = string.Empty;

        //                    if (!string.IsNullOrEmpty(payment.CLACC))
        //                    {
        //                        if (payment.CLACC.Length >= 5)
        //                        {
        //                            bankAccountBalance = payment.CLACC.Substring(0, 5);
        //                        }
        //                    }

        //                    if (bankAccountBalance.Equals("20208") ||
        //                        bankAccountBalance.Equals("14301") ||
        //                        bankAccountBalance.Equals("13301") ||
        //                        bankAccountBalance.Equals("20210") ||
        //                        bankAccountBalance.Equals("20212") ||
        //                        bankAccountBalance.Equals("20214") ||
        //                        bankAccountBalance.Equals("20216") ||
        //                        bankAccountBalance.Equals("20218") ||
        //                        bankAccountBalance.Equals("20296") ||
        //                        bankAccountBalance.Equals("21506"))
        //                    {
        //                        model.remark += string.Format("INN:{0};", payment.CLINN);
        //                        innFromDtl = payment.CLINN;
        //                    }
        //                    else if (payment.CLINN.Equals("000000000"))
        //                    {
        //                        string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

        //                        //other case try to get INN 
        //                        var Inn = numbers.FirstOrDefault(x => x.Length == 9);

        //                        if ((Inn != null) && (Inn.StartsWith("2") || (Inn.StartsWith("3"))))
        //                        {
        //                            model.remark += string.Format("INN:{0};", Inn);
        //                            innFromDtl = Inn;
        //                        }
        //                        else
        //                        {
        //                            model.remark += string.Format("INN:{0};", payment.CLINN);
        //                            innFromDtl = payment.CLINN;
        //                        }
        //                    }
        //                    else if (payment.PURPOSE.ToUpper().Contains("ИНН "))
        //                    {
        //                        string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

        //                        //First 9 digit number As INN
        //                        var Inns = numbers.Where(x => x.Length == 9);

        //                        var posOfInn = payment.PURPOSE.ToUpper().IndexOf("ИНН ");

        //                        foreach (var srt in Inns.ToList())
        //                        {
        //                            if (!(srt.StartsWith("2") || (srt.StartsWith("3"))))
        //                                continue;

        //                            var posOfInn2 = payment.PURPOSE.ToUpper().IndexOf(srt);

        //                            if (posOfInn < posOfInn2)
        //                            {
        //                                model.remark += string.Format("INN:{0};", srt);
        //                                innFromDtl = srt;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    else if (payment.PURPOSE.ToUpper().Contains("INN "))
        //                    {
        //                        string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

        //                        //First 9 digit number As INN
        //                        var Inns = numbers.Where(x => x.Length == 9);

        //                        var posOfInn = payment.PURPOSE.ToUpper().IndexOf("INN");

        //                        foreach (var srt in Inns.ToList())
        //                        {
        //                            if (!(srt.StartsWith("2") || (srt.StartsWith("3"))))
        //                                continue;

        //                            var posOfInn2 = payment.PURPOSE.ToUpper().IndexOf(srt);

        //                            if (posOfInn < posOfInn2)
        //                            {
        //                                model.remark += string.Format("INN:{0};", srt);
        //                                innFromDtl = srt;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    else if (payment.PURPOSE.ToUpper().Contains("СТИР "))
        //                    {
        //                        string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

        //                        //First 9 digit number As INN
        //                        var Inns = numbers.Where(x => x.Length == 9);

        //                        var posOfInn = payment.PURPOSE.ToUpper().IndexOf("СТИР");

        //                        foreach (var srt in Inns.ToList())
        //                        {
        //                            if (!(srt.StartsWith("2") || (srt.StartsWith("3"))))
        //                                continue;

        //                            var posOfInn2 = payment.PURPOSE.ToUpper().IndexOf(srt);

        //                            if (posOfInn < posOfInn2)
        //                            {
        //                                model.remark += string.Format("INN:{0};", srt);
        //                                innFromDtl = srt;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    else if (payment.PURPOSE.ToUpper().Contains("STIR "))
        //                    {
        //                        string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

        //                        //First 9 digit number As INN
        //                        var Inns = numbers.Where(x => x.Length == 9);

        //                        var posOfInn = payment.PURPOSE.ToUpper().IndexOf("STIR");

        //                        foreach (var srt in Inns.ToList())
        //                        {
        //                            if (!(srt.StartsWith("2") || (srt.StartsWith("3"))))
        //                                continue;

        //                            var posOfInn2 = payment.PURPOSE.ToUpper().IndexOf(srt);

        //                            if (posOfInn < posOfInn2)
        //                            {
        //                                model.remark += string.Format("INN:{0};", srt);
        //                                innFromDtl = srt;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

        //                        //other case try to get INN 
        //                        var Inn = numbers.FirstOrDefault(x => x.Length == 9);

        //                        if ((Inn != null) && (Inn.StartsWith("2") || (Inn.StartsWith("3"))))
        //                        {
        //                            model.remark += string.Format("INN:{0};", Inn);
        //                            innFromDtl = Inn;
        //                        }
        //                        else
        //                        {
        //                            model.remark += string.Format("INN:{0};", payment.CLINN);
        //                            innFromDtl = payment.CLINN;
        //                        }
        //                    }

        //                    if (string.IsNullOrEmpty(innFromDtl))
        //                    {
        //                        //Problem with geting INN
        //                        model.remark += string.Format("INN:{0};", "PROBLEM");
        //                        model.inn = payment.CLINN; 
        //                    }
        //                    else if (payment.CLINN.Equals(innFromDtl))
        //                    {
        //                        model.inn = innFromDtl;
        //                        //Good
        //                    }
        //                    else
        //                    {
        //                        model.inn = innFromDtl;
        //                        blackListInn.Add(payment.CLINN);
        //                    }

        //                    #endregion

        //                    Console.WriteLine("Formula Calculation");
        //                    //Formula Calculation
        //                    #region Formula Calculation

        //                    decimal sumAmount = ((payment.SUMPAY.HasValue ? payment.SUMPAY.Value : 0) / 100);
        //                    decimal profsouzAmount = 0;
        //                    decimal federationAmount = 0;
        //                    decimal oblsovprofAmount = 0;

        //                    if (payment.PURPOSE.Contains("3212110"))
        //                    {
        //                        profsouzAmount = (decimal)((double)(sumAmount) * 0.4 / 100);
        //                    }
        //                    else if (payment.PURPOSE.Contains("3212120"))
        //                    {
        //                        profsouzAmount = (decimal)((double)(sumAmount) * 0.666666667 / 100);
        //                    }
        //                    else if (payment.PURPOSE.Contains("3212130"))
        //                    {
        //                        profsouzAmount = (decimal)((double)(sumAmount) * 0.666666667 / 100);
        //                    }
        //                    else
        //                    {
        //                        profsouzAmount = (decimal)((double)(sumAmount) * 0.4 / 100);
        //                    }

        //                    federationAmount = Math.Round(profsouzAmount * 30 / 100, 2);

        //                    oblsovprofAmount = Math.Round(profsouzAmount * 70 / 100, 2);

        //                    model.profsouz_amt = Math.Round(profsouzAmount, 2);
        //                    model.federation_amt = federationAmount;
        //                    model.oblsovprof_amt = oblsovprofAmount;

        //                    #endregion

        //                    //Region and Branch Difinition
        //                    #region Region and Branch Difinition

        //                    //Define region 1 stage in detail we mostly have Kaznacheystvo Account which contains soato

        //                    #region Oblast and Tuman Difinition
        //                    Console.WriteLine(DateTime.Now.ToLongTimeString() + " Oblast and Tuman Difinition");
        //                    var ppp = db.tblPreds.Where(p => p.Departmentbase == "17");

        //                    string federationAccount = string.Empty;

        //                    if (ppp.Any())
        //                    {
        //                        federationAccount = ppp.First().KaznaAccount;
        //                    }

        //                    var bankDatas = db.KaznaBankFiles.Where(k => k.GRPID == payment.GRPID);
        //                    Console.WriteLine(DateTime.Now.ToLongTimeString() +  " Oblast and Tuman Difinition2222");
        //                    if (bankDatas.Any())
        //                    {
        //                        //Har bitta GRPID buyicha 2 ta qator topiladi 70% ga 30%
        //                        var KKK = bankDatas.Where(ll => ll.COACC != federationAccount);

        //                        //Federasiya bumagani topilmasa kerak emas
        //                        if (!KKK.Any()) continue;

        //                        //Federasiya bumaganini ixtiyoriy birinchisi
        //                        var bank = KKK.First();

        //                        //
        //                        var preds = db.tblPreds.Where(p => p.KaznaAccount == bank.COACC);

        //                        if (preds.Any())
        //                        {
                                    
        //                            //Shunaqa yo'l bilan departmentBase aniqlanadi
        //                            model.obl_department_base = preds.First().Departmentbase;

        //                            Console.WriteLine("DepartmentBase" + model.obl_department_base);
        //                        }
        //                        else
        //                        {
        //                            Console.WriteLine("Topa Olmadi Depbase");
        //                        }
        //                    }

        //                    if (string.IsNullOrEmpty(model.obl_department_base))
        //                    {
        //                        _logService.Error("Empty Department Code ID = " + payment.ID + " GRPID = " + payment.GRPID);
        //                    }

        //                    Console.WriteLine(DateTime.Now.ToLongTimeString() +  " Oblast and Tuman Difinition333333");
        //                    var egrpos = db.t_egrpo.Where(e => e.inn == innFromDtl);

        //                    if (egrpos.Any())
        //                    {
        //                        var org = egrpos.First();

        //                        model.soato = org.soato_id;

        //                        if (!string.IsNullOrEmpty(model.soato))
        //                        {
        //                            var lll = db.tbsp_soato_new.Where(s => s.KODI == model.soato).FirstOrDefault();

        //                            if (lll != null)
        //                            {
        //                                model.soato_name = lll.NAME_RU;
        //                            }
        //                            else
        //                            {
        //                                model.soato_name = string.Empty;
        //                            }
        //                        }
        //                        Console.WriteLine(DateTime.Now.ToLongTimeString() + " Oblast and Tuman Difinition4444444");
        //                        if (!string.IsNullOrEmpty(org.okonx_id))
        //                        {
        //                            model.okonx_id = org.okonx_id;

        //                            var okonx = db.sp_okonx.Where(o => o.code == org.okonx_id).First();

        //                            model.okonx_name = okonx.name_ru;
        //                            Console.WriteLine(DateTime.Now.ToLongTimeString() +  " Oblast and Tuman Difinition5555555");
        //                            if (!string.IsNullOrEmpty(okonx.branch_code))
        //                            {
        //                                model.branch_code = okonx.branch_code;
        //                                Console.WriteLine(DateTime.Now.ToLongTimeString() + " Oblast and Tuman Difinition66666666");
        //                                model.branch_name = db.sp_branch.Where(b => b.code == okonx.branch_code).First().name_ru;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (egrpoInnEnter.Where(z=>z== innFromDtl).Any())
        //                        {
        //                            //skip
        //                        }
        //                        else
        //                        {
        //                            //add to egrpo
        //                            var ee = new t_egrpo();
        //                            Console.WriteLine(DateTime.Now.ToLongTimeString() + " Oblast and Tuman Difinition777777777");
        //                            ee.version = 0;
        //                            ee.inn = innFromDtl;
        //                            ee.name_full = payment.CLNAME;

        //                            db.t_egrpo.Add(ee);

        //                            egrpoInnEnter.Add(innFromDtl);
        //                        }
        //                    }

        //                    //Search in tblSprav_Org
        //                    var orgsList = db.tblSprav_Org.Where(z => z.INN == innFromDtl);

        //                    if (orgsList.Any())
        //                    {
        //                        var org1 = orgsList.First();

        //                        if (org1.ID_Ot.HasValue)
        //                        {
        //                            var currentOtrasl = db.tblSprav_Otras.Where(z => z.ID_Ot == org1.ID_Ot);

        //                            if (currentOtrasl.Any())
        //                            {
        //                                var branchCode = currentOtrasl.First().BRANCH_CODE;

        //                                var branchNameRow = db.sp_branch.Where(z => z.code == branchCode);

        //                                if (branchNameRow.Any())
        //                                {
        //                                    var branchName = branchNameRow.First().name_ru;

        //                                    model.branch_code = branchCode;
        //                                    Console.WriteLine(DateTime.Now.ToLongTimeString() + " Oblast and Tuman Difinition888888");
        //                                    model.branch_name = branchName;
        //                                }
        //                            }
        //                        }

        //                    }
        //                    else
        //                    {

        //                        if (orgInnEnter.Where(z => z == innFromDtl).Any())
        //                        {
        //                            //skip
        //                        }
        //                        else
        //                        {
        //                            var orrg = new tblSprav_Org();
        //                            orrg.INN = innFromDtl;

        //                            if (payment.CLNAME.Length > 100)
        //                            {
        //                                orrg.Name_Org = payment.CLNAME.Substring(0, 100);
        //                            }
        //                            else
        //                            {
        //                                orrg.Name_Org = payment.CLNAME;
        //                            }

        //                            if (string.IsNullOrEmpty(model.obl_department_base))
        //                            {
        //                                orrg.DepartmentBase = null;
        //                            }
        //                            else
        //                            {
        //                                orrg.DepartmentBase = model.obl_department_base;
        //                            }

        //                            orrg.ORIG_INN = innFromDtl;
        //                            orrg.OKONX = model.okonx_id;


        //                            db.tblSprav_Org.Add(orrg);

        //                            orgInnEnter.Add(innFromDtl);
        //                        }
        //                    }

        //                    #endregion

        //                    #endregion
        //                    Console.WriteLine(DateTime.Now.ToLongTimeString() + " Oblast and Tuman Difinition88888888");
        //                    EcpCalcDataManager.Create(ref model, db);

        //                }

        //                try
        //                {
        //                    db.SaveChanges();
        //                    orgInnEnter.Clear();
        //                    egrpoInnEnter.Clear();

        //                }
        //                catch (DbEntityValidationException e)
        //                {
        //                    foreach (var eve in e.EntityValidationErrors)
        //                    {
        //                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //                        foreach (var ve in eve.ValidationErrors)
        //                        {
        //                            _logService.Error(string.Format("- Property: \"{0}\", Error: \"{1}\"",  ve.PropertyName, ve.ErrorMessage));
        //                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                                ve.PropertyName, ve.ErrorMessage);
        //                        }
        //                    }
        //                    throw;
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.ToString());
        //                    _logService.Error(ex.ToString());
        //                }
                        
        //            }

        //            //return;

        //            #region Provodki
        //            var provodki = new Provodki();

        //            var trans = new ProvodkiViewModel();

        //            var NotProcList1 = db.sp_pr_GetNewBankProvodki();

        //            var NotProcList = NotProcList1.ToList();

        //            if (NotProcList.Any())
        //            {
        //                foreach (var pp in NotProcList)
        //                {
        //                    Console.WriteLine(pp.OPERDAY);
        //                    Console.WriteLine(pp.COINN);
        //                    Console.WriteLine("========================================");

        //                    trans.comment = string.Empty;
        //                    trans.department_base = string.Empty;
        //                    trans.inoutflg = false;
        //                    trans.number = string.Empty;
        //                    trans.platelinn = string.Empty;
        //                    trans.platelmfo = string.Empty;
        //                    trans.platelname = string.Empty;
        //                    trans.platelrsch = string.Empty;
        //                    trans.poluchinn = string.Empty;
        //                    trans.poluchmfo = string.Empty;
        //                    trans.poluchname = string.Empty;
        //                    trans.poluchrsch = string.Empty;
        //                    trans.rec_id = 0;
        //                    trans.summa = 0;
        //                    trans.tipoborota = false;
        //                    trans.tipscheta = false;
        //                    trans.trdt = DateTime.MinValue;
        //                    trans.user_id = 0;
        //                    trans.trref = "ECP";
        //                    trans.trrefno = pp.ID.ToString();

        //                    //Obshaya provodka
        //                    trans.comment = "Postupleniya po ECP";
        //                    trans.department_base = pp.Departmentbase;
        //                    trans.inoutflg = false;
        //                    trans.platelrsch = pp.Departmentbase == "17" ? "22" : "20";
        //                    trans.poluchrsch = "004";
        //                    trans.summa = pp.SUMPAY;

        //                    trans.user_id = 1;
        //                    trans.trdt = pp.OPERDAY;

        //                    ProvodkiDataManager.Create(ref trans);

        //                    EcpCalcDataManager.ModifyBank(pp.ID , trans.rec_id);

        //                }
        //            }

        //            #endregion


        //            #region Otrasl_Past change

        //            Console.WriteLine("Otrasl Late Update");

        //            var lastTimeRow = db.TimeShedulers.Where(z => z.id == 1).FirstOrDefault();

        //            if (lastTimeRow != null)
        //            {
        //                var lastTime = lastTimeRow.LastChanged;

        //                Console.WriteLine("lastTime: " + lastTime);

        //                var newOrgChangedList = db.tblSprav_Org.Where(z => z.TIMESTAMP_CH >= lastTime);

        //                if (newOrgChangedList.Any())
        //                {
        //                    foreach(var org in newOrgChangedList)
        //                    {
        //                        if (org.ID_Ot.HasValue)
        //                        {
        //                            var currentOtrasl = db.tblSprav_Otras.Where(z => z.ID_Ot == org.ID_Ot);

        //                            if (currentOtrasl.Any())
        //                            {
        //                                var branchCode = currentOtrasl.First().BRANCH_CODE;

        //                                Console.WriteLine("branchCode: " + branchCode);

        //                                var branchNameRow = db.sp_branch.Where(z => z.code == branchCode);

        //                                if (branchNameRow.Any())
        //                                {
        //                                    var branchName = branchNameRow.First().name_ru;

        //                                    Console.WriteLine("branchName: " + branchName);

        //                                    var ecpChangeList = db.tbbu_ecp_calc.Where(z => z.INN == org.INN);

        //                                    if (ecpChangeList.Any())
        //                                    {
        //                                        foreach(var ecp in ecpChangeList)
        //                                        {
        //                                            ecp.BRANCH_CODE = branchCode;
        //                                            ecp.BRANCH_NAME = branchName;
        //                                        }

                                                
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }

        //                    lastTimeRow.LastChanged = DateTime.Now;

        //                    db.SaveChanges();
        //                }
        //            }

        //            #endregion

        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        _logService.Error(ex.ToString());
        //        Console.WriteLine(ex.ToString());
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        public void CalcProcessN()
        {
            Console.WriteLine("CalcProcessN");
            Thread t1 = new Thread(new ThreadStart(EcpCalcController));
            Thread t2 = new Thread(new ThreadStart(MakeProvodkiController));
            Thread t3 = new Thread(new ThreadStart(BranchCalcController));
            Thread t4 = new Thread(new ThreadStart(KaznaFileController));
            Thread t5 = new Thread(new ThreadStart(OwnerCodeController));
            Thread t6 = new Thread(new ThreadStart(BronController));

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();
            t6.Start();

            Console.WriteLine("Threads Started");

            Thread.CurrentThread.Join();
        }

        public EcpCalculation()
        {
            Console.WriteLine("EcpCalculation Constructor");
            EcpCalcStart = true;
            MakeProvodkiStart = true;
            BranchCalcStart = true;
            KaznaFileStart = true;
            OwnerCodeStart = true;
            BronStart = true;

            EcpCalcSheduler = 3600000;
            MakeProvodkiSheduler = 3600000;
            BranchCalcSheduler = 3600000;
            KaznaFileSheduler = 3600000;
            OwnerCodeSheduler = 3600000;
            BronSheduler = 3600000;

            kk = new KaznaController();
        }

        public bool EcpCalcStart { get; set; }
        public bool MakeProvodkiStart { get; set; }
        public bool BranchCalcStart { get; set; }
        public bool KaznaFileStart { get; set; }
        public bool OwnerCodeStart { get; set; }
        public bool BronStart { get; set; }

        public int EcpCalcSheduler { get; set; }
        public int MakeProvodkiSheduler { get; set; }
        public int BranchCalcSheduler { get; set; }
        public int KaznaFileSheduler { get; set; }
        public int OwnerCodeSheduler { get; set; }
        public int BronSheduler { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private void EcpCalcController()
        {
            while(true)
            {
                Console.WriteLine("EcpCalcController...");
                if (!EcpCalcStart) continue;

                EcpCalc();

                Thread.Sleep(EcpCalcSheduler);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void MakeProvodkiController()
        {
            while (true)
            {
                Console.WriteLine("MakeProvodkiController...");

                if (!MakeProvodkiStart) continue;

                MakeProvodki();

                Thread.Sleep(MakeProvodkiSheduler);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BranchCalcController()
        {
            while (true)
            {
                Console.WriteLine("BranchCalcController...");

                if (!BranchCalcStart) continue;

                BranchCalc();

                Thread.Sleep(BranchCalcSheduler);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void KaznaFileController()
        {
            while (true)
            {
                Console.WriteLine("KaznaFileController...");

                if (!KaznaFileStart) continue;

                kk.Process();

                Thread.Sleep(KaznaFileSheduler);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void OwnerCodeController()
        {
            while (true)
            {
                Console.WriteLine("KaznaFileController...");

                if (!OwnerCodeStart) continue;

                MakeOwnerCode();

                Thread.Sleep(OwnerCodeSheduler);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void BronController()
        {
            while (true)
            {
                Console.WriteLine("BronController...");

                if (!BronStart) continue;

                BronExpiredClear();

                Thread.Sleep(BronSheduler);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void EcpCalc()
        {
            Console.WriteLine("EcpCalc...");
            _logService.Info("EcpCalc ====******");

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

                                model.payment_id = (long)payment.PAYMENT_ID;
                                model.profsouz_amt = payment.PROFSOUZ_AMT;
                                model.federation_amt = payment.FEDERATION_AMT;
                                model.oblsovprof_amt = payment.OBLSOVPROF_AMT;
                                model.soato = payment.SOATO;
                                model.soato_name = payment.SOATO_NAME;
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

                                if (bankAccountBalance.Equals("20208") //||
                                                                       //bankAccountBalance.Equals("14301") ||
                                                                       //bankAccountBalance.Equals("13301") ||
                                                                       //bankAccountBalance.Equals("20210") ||
                                                                       //bankAccountBalance.Equals("20212") ||
                                                                       //bankAccountBalance.Equals("20214") ||
                                                                       //bankAccountBalance.Equals("20216") ||
                                                                       ////bankAccountBalance.Equals("20218") || //Ibroxim Namangan
                                                                       //bankAccountBalance.Equals("20296") ||
                                                                       //bankAccountBalance.Equals("21506")
                                    )
                                {
                                    model.remark += string.Format("INN:{0};", payment.INN);
                                    innFromDtl = payment.INN;
                                }
                                else
                                {
                                    //other case try to get INN 
                                    var Inn = GetInnFromDetail(payment.PURPOSE);

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

                                            if (model.branch_code.Equals("Н2"))
                                            {
                                                model.adep_code = "173Н2";
                                            }
                                            else if (model.branch_code.Equals("Т2"))
                                            {
                                                model.adep_code = "173Т2";
                                            }
                                            else if (model.branch_code.Equals("О1"))
                                            {
                                                model.adep_code = "173О1";
                                            }
                                            else
                                            {
                                                model.adep_code = "173" + model.branch_code + payment.SOATO.Substring(2, 5);
                                            }
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
                                var orgsList = db.tblSprav_Org.Where(z => z.INN == innFromDtl &&
                                z.DepartmentBase == payment.OBL_DEPARTMENT_BASE);

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

                                                if (model.branch_code.Equals("Н2"))
                                                {
                                                    model.adep_code = "173Н2";
                                                }
                                                else if (model.branch_code.Equals("Т2"))
                                                {
                                                    model.adep_code = "173Т2";
                                                }
                                                else if (model.branch_code.Equals("О1"))
                                                {
                                                    model.adep_code = "173О1";
                                                }
                                                else
                                                {
                                                    model.adep_code = "173" + model.branch_code + payment.SOATO.Substring(2, 5);
                                                }

                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    string combine = string.Format("{0}{1}", innFromDtl, payment.OBL_DEPARTMENT_BASE);

                                    if (orgInnEnter.Where(z => z == combine).Any())
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

                                        orrg.FACE_TYPE = 0;

                                        db.tblSprav_Org.Add(orrg);

                                        orgInnEnter.Add(combine);
                                    }
                                }

                                #endregion

                                Console.WriteLine(DateTime.Now.ToLongTimeString() + "EcpCalcDataManager.Create ");
                                EcpCalcDataManager.Create(ref model, db);
                            }

                            try
                            {
                                Console.WriteLine(DateTime.Now.ToLongTimeString() + "SaveChanges!!!!!================================== ");
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

                    }
                }
                catch (Exception ex)
                {
                    _logService.Info(ex.ToString());
                }

        }



        /// <summary>
        /// 
        /// </summary>
        private void MakeProvodki()
        {
            Console.WriteLine("MakeProvodki...");
            _logService.Info("MakeProvodki ====******");

            try
            {
                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    #region Provodki
                    var provodki = new Provodki();

                    var trans = new ProvodkiViewModel();

                    #region GetNastroyki
                    //Get provodki Nastroyki
                    var first = new ProvodkaSettingViewModel();
                    var seconda = new ProvodkaSettingViewModel();
                    var secondb = new ProvodkaSettingViewModel();
                    var nast1 = db.tbbu_nastroykiProvodok.Where(z => z.CODE == "ECP_TOTAL");

                    if (nast1.Any())
                    {
                        var fp = nast1.First();
                        first.code = fp.CODE;
                        first.debit_acc = fp.DEBIT_ACC;
                        first.creadit_acc = fp.CREADIT_ACC;
                    }
                    else
                    {
                        Console.WriteLine("DIQQAT!!!DIQQAT!!!DIQQAT!!!");
                        Console.WriteLine("Nastroyka did not exists ECP_TOTAL!!!!!");
                        return;
                    }


                    //Provodkani 2 qismi
                    var nast2 = db.tbbu_nastroykiProvodok.Where(z => z.CODE == "ECP_RASP" && z.DEPARTMENT_BASE == "17");

                    if (nast2.Any())
                    {
                        var fp = nast2.First();
                        seconda.code = fp.CODE;
                        seconda.debit_acc = fp.DEBIT_ACC;
                        seconda.creadit_acc = fp.CREADIT_ACC;
                    }
                    else
                    {
                        Console.WriteLine("DIQQAT!!!DIQQAT!!!DIQQAT!!!");
                        Console.WriteLine("Nastroyka did not exists ECP_RASP 17 !!!!!");
                        return;
                    }


                    var nast3 = db.tbbu_nastroykiProvodok.Where(z => z.CODE == "ECP_RASP" && z.DEPARTMENT_BASE != "17");

                    if (nast3.Any())
                    {
                        var fp = nast3.First();
                        secondb.code = fp.CODE;
                        secondb.debit_acc = fp.DEBIT_ACC;
                        secondb.creadit_acc = fp.CREADIT_ACC;
                    }
                    else
                    {
                        Console.WriteLine("DIQQAT!!!DIQQAT!!!DIQQAT!!!");
                        Console.WriteLine("Nastroyka did not exists ECP_RASP not equal 17 !!!!!");
                        return;
                    }


                    //////////////////////
                    ///
                    var railway_prov = new ProvodkaSettingViewModel();
                    var agmk_prov = new ProvodkaSettingViewModel();
                    var ngmk_prov = new ProvodkaSettingViewModel();

                    var todepcode = new ProvodkaSettingViewModel();

                    var nast4 = db.tbbu_nastroykiProvodok.Where(z => z.CODE == "RAILWAY_PROV" );

                    if (nast4.Any())
                    {
                        var fp = nast4.First();
                        railway_prov.code = fp.CODE;
                        railway_prov.debit_acc = fp.DEBIT_ACC;
                        railway_prov.creadit_acc = fp.CREADIT_ACC;
                    }
                    else
                    {
                        Console.WriteLine("DIQQAT!!!DIQQAT!!!DIQQAT!!!");
                        Console.WriteLine("Nastroyka did not exists RAILWAY_PROV");
                        return;
                    }


                    var nast5 = db.tbbu_nastroykiProvodok.Where(z => z.CODE == "AGMK_PROV" );

                    if (nast5.Any())
                    {
                        var fp = nast5.First();
                        agmk_prov.code = fp.CODE;
                        agmk_prov.debit_acc = fp.DEBIT_ACC;
                        agmk_prov.creadit_acc = fp.CREADIT_ACC;
                    }
                    else
                    {
                        Console.WriteLine("DIQQAT!!!DIQQAT!!!DIQQAT!!!");
                        Console.WriteLine("Nastroyka did not exists AGMK_PROV");
                        return;
                    }


                    var nast6 = db.tbbu_nastroykiProvodok.Where(z => z.CODE == "NGMK_PROV");

                    if (nast6.Any())
                    {
                        var fp = nast6.First();
                        ngmk_prov.code = fp.CODE;
                        ngmk_prov.debit_acc = fp.DEBIT_ACC;
                        ngmk_prov.creadit_acc = fp.CREADIT_ACC;
                    }
                    else
                    {
                        Console.WriteLine("DIQQAT!!!DIQQAT!!!DIQQAT!!!");
                        Console.WriteLine("Nastroyka did not exists NGMK_PROV !!!!!");
                        return;
                    }


                    var nast7 = db.tbbu_nastroykiProvodok.Where(z => z.CODE == "TO_DEPCODE");

                    if (nast7.Any())
                    {
                        var fp = nast7.First();
                        todepcode.code = fp.CODE;
                        todepcode.debit_acc = fp.DEBIT_ACC;
                        todepcode.creadit_acc = fp.CREADIT_ACC;
                    }
                    else
                    {
                        Console.WriteLine("DIQQAT!!!DIQQAT!!!DIQQAT!!!");
                        Console.WriteLine("Nastroyka did not exists ECP_RASP not equal 17 !!!!!");
                        return;
                    }

                    /////

                    #endregion

                    var NotProcList1 = db.sp_pr_GetNewBankProvodki();

                    var NotProcList = NotProcList1.ToList();

                    if (NotProcList.Any())
                    {
                        foreach (var pp in NotProcList)
                        {
                            Console.WriteLine(pp.OPERDAY);
                            Console.WriteLine(pp.COINN);
                            Console.WriteLine("========================================");


                            #region Clear
                            {
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
                                trans.trref = "ECO";
                                trans.trrefno = pp.ID.ToString();
                            }
                            #endregion

                            //Obshaya provodka
                            trans.comment = "Postupleniya po ECP Vsego";
                            trans.department_base = pp.Departmentbase;
                            trans.inoutflg = false;
                            trans.platelrsch = first.debit_acc;
                            trans.poluchrsch = first.creadit_acc;
                            trans.summa = pp.SUMPAY;

                            trans.user_id = 1;
                            trans.trdt = pp.OPERDAY;

                            ProvodkiDataManager.Create(ref trans, db, out tbbu_provodki firstp);

                            var trans1 = new ProvodkiViewModel();
                            #region Clear
                            {
                                trans1.comment = string.Empty;
                                trans1.department_base = string.Empty;
                                trans1.inoutflg = false;
                                trans1.number = string.Empty;
                                trans1.platelinn = string.Empty;
                                trans1.platelmfo = string.Empty;
                                trans1.platelname = string.Empty;
                                trans1.platelrsch = string.Empty;
                                trans1.poluchinn = string.Empty;
                                trans1.poluchmfo = string.Empty;
                                trans1.poluchname = string.Empty;
                                trans1.poluchrsch = string.Empty;
                                trans1.rec_id = 0;
                                trans1.summa = 0;
                                trans1.tipoborota = false;
                                trans1.tipscheta = false;
                                trans1.trdt = DateTime.MinValue;
                                trans1.user_id = 0;
                                trans1.trref = "ECP";
                                trans1.trrefno = pp.ID.ToString();
                            }
                            #endregion

                            //Obshaya provodka
                            trans1.comment = "Raspredeleniye po Obl i Federasiya";
                            trans1.department_base = pp.Departmentbase;
                            trans1.inoutflg = false;
                            trans1.platelrsch = pp.Departmentbase == "17" ? seconda.debit_acc : secondb.debit_acc;
                            trans1.poluchrsch = pp.Departmentbase == "17" ? seconda.creadit_acc : secondb.creadit_acc;
                            trans1.summa = pp.SUMPAY;
                            trans1.user_id = 1;
                            trans1.trdt = pp.OPERDAY;

                            ProvodkiDataManager.Create(ref trans1, db, out tbbu_provodki second);

                            EcpCalcDataManager.ModifyBank(pp.ID, db , firstp);

                            ///////////////////////////////////////////////////////////////////////////////////////////
                            ///
                            /// By GRPID Get All row in KaznaPayment and using tblSprav_Org Define if it is NGMK, AGMK or Railways
                            /// if such make some transaction. for Finotchet
                            /// 
                            ////////////////////////////////////////////////////////////////////////////////////////////

                            #region Raspredeleniye po AGMK NGMK i TemirYo'q

                            var kaznaList = db.KaznaPaymentFiles.Where(z => z.GRPID == pp.GRPID);

                            if (kaznaList.Any())
                            {
                                var summ = kaznaList.Sum(z => z.SUMPAY);

                                if (summ != pp.SUMPAY)
                                {
                                    Console.WriteLine("Summas in Bank and KaznaPayment by GRPID are not equal");
                                }
                                else
                                {
                                    Console.WriteLine("Summas in Bank and KaznaPayment by GRPID EQUAL!!!");
                                }

                                foreach(var kazna in kaznaList)
                                {
                                    var ecpCalcs = db.tbbu_ecp_calc.Where(z => z.PAYMENT_ID == kazna.ID);

                                    if (!ecpCalcs.Any())
                                    {
                                        //ECP Calc Thread not finished
                                        Thread.Sleep(60000);
                                        return;
                                    }

                                    var ecpCalc = ecpCalcs.First();

                                    var orgs = db.tblSprav_Org.Where(z => z.INN == ecpCalc.INN);

                                    //ECP Calc Thread not finished
                                    if (!orgs.Any())
                                    {
                                        Thread.Sleep(60000);
                                        return;
                                    }

                                    // 10 15 16
                                    var org = orgs.First();

                                    if (org.ID_Ot == 10)
                                    {
                                        #region Reilway Temir Yo'l
                                        var trans2 = new ProvodkiViewModel();
                                        #region Clear
                                        {
                                            trans2.comment = string.Empty;
                                            trans2.department_base = string.Empty;
                                            trans2.inoutflg = false;
                                            trans2.number = string.Empty;
                                            trans2.platelinn = string.Empty;
                                            trans2.platelmfo = string.Empty;
                                            trans2.platelname = string.Empty;
                                            trans2.platelrsch = string.Empty;
                                            trans2.poluchinn = string.Empty;
                                            trans2.poluchmfo = string.Empty;
                                            trans2.poluchname = string.Empty;
                                            trans2.poluchrsch = string.Empty;
                                            trans2.rec_id = 0;
                                            trans2.summa = 0;
                                            trans2.tipoborota = false;
                                            trans2.tipscheta = false;
                                            trans2.trdt = DateTime.MinValue;
                                            trans2.user_id = 0;
                                            trans2.trref = "ECO";
                                            trans2.trrefno = pp.ID.ToString();
                                        }
                                        #endregion

                                        //Obshaya provodka
                                        trans2.comment = "Temiq yo'l";
                                        trans2.department_base = pp.Departmentbase;
                                        trans2.inoutflg = false;
                                        trans2.platelrsch = railway_prov.debit_acc;
                                        trans2.poluchrsch = railway_prov.creadit_acc;
                                        trans2.summa = pp.SUMPAY;
                                        trans2.user_id = 1;
                                        trans2.trdt = pp.OPERDAY;

                                        ProvodkiDataManager.Create(ref trans2, db, out tbbu_provodki firstp1);

                                        #endregion

                                    }
                                    else if (org.ID_Ot == 15)
                                    {
                                        #region Olmaliq
                                        var trans3 = new ProvodkiViewModel();
                                        #region Clear
                                        {
                                            trans3.comment = string.Empty;
                                            trans3.department_base = string.Empty;
                                            trans3.inoutflg = false;
                                            trans3.number = string.Empty;
                                            trans3.platelinn = string.Empty;
                                            trans3.platelmfo = string.Empty;
                                            trans3.platelname = string.Empty;
                                            trans3.platelrsch = string.Empty;
                                            trans3.poluchinn = string.Empty;
                                            trans3.poluchmfo = string.Empty;
                                            trans3.poluchname = string.Empty;
                                            trans3.poluchrsch = string.Empty;
                                            trans3.rec_id = 0;
                                            trans3.summa = 0;
                                            trans3.tipoborota = false;
                                            trans3.tipscheta = false;
                                            trans3.trdt = DateTime.MinValue;
                                            trans3.user_id = 0;
                                            trans3.trref = "ECO";
                                            trans3.trrefno = pp.ID.ToString();
                                        }
                                        #endregion

                                        //Obshaya provodka
                                        trans3.comment = "AGMK";
                                        trans3.department_base = pp.Departmentbase;
                                        trans3.inoutflg = false;
                                        trans3.platelrsch = agmk_prov.debit_acc;
                                        trans3.poluchrsch = agmk_prov.creadit_acc;
                                        trans3.summa = pp.SUMPAY;
                                        trans3.user_id = 1;
                                        trans3.trdt = pp.OPERDAY;

                                        ProvodkiDataManager.Create(ref trans3, db, out tbbu_provodki firstp2);

                                        #endregion

                                    }
                                    else if (org.ID_Ot == 16)
                                    {
                                        #region NGMK
                                        var trans4 = new ProvodkiViewModel();
                                        #region Clear
                                        {
                                            trans4.comment = string.Empty;
                                            trans4.department_base = string.Empty;
                                            trans4.inoutflg = false;
                                            trans4.number = string.Empty;
                                            trans4.platelinn = string.Empty;
                                            trans4.platelmfo = string.Empty;
                                            trans4.platelname = string.Empty;
                                            trans4.platelrsch = string.Empty;
                                            trans4.poluchinn = string.Empty;
                                            trans4.poluchmfo = string.Empty;
                                            trans4.poluchname = string.Empty;
                                            trans4.poluchrsch = string.Empty;
                                            trans4.rec_id = 0;
                                            trans4.summa = 0;
                                            trans4.tipoborota = false;
                                            trans4.tipscheta = false;
                                            trans4.trdt = DateTime.MinValue;
                                            trans4.user_id = 0;
                                            trans4.trref = "ECO";
                                            trans4.trrefno = pp.ID.ToString();
                                        }
                                        #endregion

                                        //Obshaya provodka
                                        trans4.comment = "NGMK";
                                        trans4.department_base = pp.Departmentbase;
                                        trans4.inoutflg = false;
                                        trans4.platelrsch = ngmk_prov.debit_acc;
                                        trans4.poluchrsch = ngmk_prov.creadit_acc;
                                        trans4.summa = pp.SUMPAY;
                                        trans4.user_id = 1;
                                        trans4.trdt = pp.OPERDAY;

                                        ProvodkiDataManager.Create(ref trans4, db, out tbbu_provodki firstp3);

                                        #endregion
                                    }

                                    if ((org.TO_DEPCODE != null) && (!string.IsNullOrEmpty(org.TO_DEPCODE)))
                                    {
                                        #region TO DEP CODE
                                        var trans5 = new ProvodkiViewModel();
                                        #region Clear
                                        {
                                            trans5.comment = string.Empty;
                                            trans5.department_base = string.Empty;
                                            trans5.inoutflg = false;
                                            trans5.number = string.Empty;
                                            trans5.platelinn = string.Empty;
                                            trans5.platelmfo = string.Empty;
                                            trans5.platelname = string.Empty;
                                            trans5.platelrsch = string.Empty;
                                            trans5.poluchinn = string.Empty;
                                            trans5.poluchmfo = string.Empty;
                                            trans5.poluchname = string.Empty;
                                            trans5.poluchrsch = string.Empty;
                                            trans5.rec_id = 0;
                                            trans5.summa = 0;
                                            trans5.tipoborota = false;
                                            trans5.tipscheta = false;
                                            trans5.trdt = DateTime.MinValue;
                                            trans5.user_id = 0;
                                            trans5.trref = "ECO";
                                            trans5.trrefno = pp.ID.ToString();
                                        }
                                        #endregion

                                        //Obshaya provodka
                                        trans5.comment = "TO DEPCODE";
                                        trans5.department_base = org.TO_DEPCODE;
                                        trans5.inoutflg = false;
                                        trans5.platelrsch = todepcode.debit_acc;
                                        trans5.poluchrsch = todepcode.creadit_acc;
                                        trans5.summa = pp.SUMPAY;
                                        trans5.user_id = 1;
                                        trans5.trdt = pp.OPERDAY;

                                        ProvodkiDataManager.Create(ref trans5, db, out tbbu_provodki firstp4);

                                        #endregion
                                    }
                                }
                            }
                            else
                            {
                                //ECP Calc Thread not finished
                                Thread.Sleep(60000);
                                return;
                            }

                            #endregion
                        }

                        db.SaveChanges();
                    }

                    #endregion
                }
            }
            catch(Exception ex)
            {
                _logService.Info(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BranchCalc()
        {
            Console.WriteLine("BranchCalc...");
            _logService.Info("BranchCalc ====******");

            try
            {
                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
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

                                                    if (branchCode.Equals("Н2"))
                                                    {
                                                        ecp.ADEP_CODE = "173Н2";
                                                    }
                                                    else if (branchCode.Equals("Т2"))
                                                    {
                                                        ecp.ADEP_CODE = "173Т2";
                                                    }
                                                    else if (branchCode.Equals("О1"))
                                                    {
                                                        ecp.ADEP_CODE = "173О1";
                                                    }
                                                    else
                                                    {
                                                        ecp.ADEP_CODE = "173" + branchCode + ecp.SOATO.Substring(2, 5);
                                                    }
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

        /// <summary>
        /// 
        /// </summary>
        private void MakeOwnerCode()
        {
            Console.WriteLine("MakeOwnerCode");

            try
            {
                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    var newOrgChangedList = db.tblSprav_Org.Where(z => z.OWNER_CODE == null 
                    && z.ID_Ot != null 
                    && z.ID_Ot != -1 
                    && z.Region != -1 
                    && z.Region != null 
                    && z.PRAVORGLEVEL != null).Take(1000);

                    if (newOrgChangedList.Any())
                    {
                        foreach(var org in newOrgChangedList)
                        {
                            try
                            {
                                Console.WriteLine("org.ID_Ot.Value " + org.ID_Ot.Value);
                                Console.WriteLine("org.Region.Value " + org.Region.Value);
                                Console.WriteLine("org.PRAVORGLEVEL.Value " + org.PRAVORGLEVEL.Value);
                                Console.WriteLine("org.ID " + org.ID);

                                org.OWNER_CODE = GenerateDepartmentCode(org.ID_Ot.Value
                                            , org.Region.Value,
                                            org.PRAVORGLEVEL.Value,
                                            org.ID, db);

                                if (!string.IsNullOrEmpty(org.OWNER_CODE) && org.ID_Ot.Value != -111)
                                {
                                    var depbase = new t_departmentbase
                                    {
                                        code = org.OWNER_CODE,
                                        name_ru = org.Name_Org,
                                        name_uz = org.Name_Org,
                                        name_uzl = org.Name_Org,
                                        dtype = "Bkut"
                                    };

                                    db.t_departmentbase.Add(depbase);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.ToString());
                                continue;
                            }

                            Console.Write("ID = " + org.ID);
                            Console.WriteLine("  OwnerCode = " + org.OWNER_CODE);
                        }

                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbEntityValidationException e)
                        {
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                        ve.PropertyName, ve.ErrorMessage);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
                _logService.Info(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BronExpiredClear()
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryList = db.tblBrons.Where(z => z.Uchet == true);

                if (queryList.Any())
                {
                    foreach (var bron in queryList)
                    {
                        if (bron.ExpireDate <= DateTime.Now)
                        {
                            bron.Uchet = false;
                        }
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otrasl"></param>
        /// <param name="region"></param>
        /// <param name="pravsoyuzLevel"></param>
        /// <param name="Id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        private string GenerateDepartmentCode(int otrasl = 0, int region = 0, int pravsoyuzLevel = 0, long Id = 0, KasabaKIATEntities db = null)
        {
                if (region == -1)
                {
                    //Pustoy
                    return null;
                }

                var structureKiat = new StructureOfKiat();

                if (Id == 0)
                {
                    Id = db.tblSprav_Org.Max(z => z.ID) + 1;
                }

                var branches = db.tblSprav_Otras.Where(z => z.ID_Ot == otrasl);

                if (branches.Any())
                {
                    var branch = branches.First().BRANCH_CODE;

                    KIAT_STRUCT str = KIAT_STRUCT.PPO;

                    if (pravsoyuzLevel == (int)KIAT_STRUCT.TumanBranch)
                    {
                        str = KIAT_STRUCT.TumanBranch;
                    }
                    else if (pravsoyuzLevel == (int)KIAT_STRUCT.PPO)
                    {
                        str = KIAT_STRUCT.PPO;
                    }

                    var regions = db.tblSprav_Reg.Where(z => z.CodeReg == region);

                    string soatoCode = string.Empty;

                    if (regions.Any())
                    {
                        soatoCode = regions.First().SOATO_CODE;
                    }
                    else
                    {
                        //throw new ApplicationException("Region is invalid");
                        return null;
                    }

                    return structureKiat.GenerateDepartmentCode(str, branch, soatoCode, Id);
                }
                else
                {
                    return null;
                }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private string GetInnFromDetail(string detail)
        {
            if (string.IsNullOrEmpty(detail)) return null;

            string[] numbers = Regex.Matches(detail, @"([-:,.\s*/A-Za-zА-Яа-я](\d\s?){8}\d[-:,.\s)*~/A-Za-zА-Яа-я]|\s(\d\s?){8}\d$)")
                            .Cast<Match>()
                            .Select(m => m.Value)
                            .ToArray();

            List<string> newArray = new List<string>();
            foreach (var str in numbers)
            {
                var str1 = Regex.Matches(str, @"\d+").Cast<Match>()
                                        .Select(m => m.Value)
                                        .ToArray();

                var sssdd = string.Join("", str1.Select(x => x.ToString()));

                newArray.Add(sssdd);

                Console.WriteLine("sssdd:{0} ", sssdd);
            }
            
            if (newArray.Count > 1)
            {
                return  newArray.First();
            }
            else if (newArray.Count == 1)
            {
                return newArray.First();
            }

             return null;
        }

    }
}