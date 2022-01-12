using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using KasabaKIAT.ViewModel.Buxgalteriya;
using KaznaFilesImporter;
using Epigov.Log;
using System.Data.Entity.Validation;
using System.Threading;
using KasabaKIAT.BusinessLogic.BankOplati.Kaznacheystvo;
using System.Configuration;
using System.Threading.Tasks;
using KasabaKIAT.BusinessLogic.Structure;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki.AutomaticTransactions.KaznaTransactions
{
    public class EcpCalculation
    {
        readonly FileLogService _logService = new FileLogService(typeof(EcpCalculation));
        KaznaController kaznaController { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public void CalcProcessN()
        {
            Console.WriteLine("CalcProcessN");

            var tasks= new List<Task>();

            if (EcpCalcStart) tasks.Add(Task.Run(EcpCalcController));
            if (MakeProvodkiStart) tasks.Add(Task.Run(MakeProvodkiController));
            if (BranchCalcStart) tasks.Add(Task.Run(BranchCalcController));
            if (KaznaFileStart) tasks.Add(Task.Run(KaznaFileController));
            if (OwnerCodeStart) tasks.Add(Task.Run(OwnerCodeController));
            if (BronStart) tasks.Add(Task.Run(BronController));

            Console.WriteLine("Threads Started");

            Task.WaitAll(tasks.ToArray());
        }

        public EcpCalculation()
        {
            Console.WriteLine("EcpCalculation Constructor");

            try
            {
                EcpCalcStart = Boolean.Parse(ConfigurationManager.AppSettings["EcpCalcStart"]);
                MakeProvodkiStart = Boolean.Parse(ConfigurationManager.AppSettings["MakeProvodkiStart"]);
                BranchCalcStart = Boolean.Parse(ConfigurationManager.AppSettings["BranchCalcStart"]);
                KaznaFileStart = Boolean.Parse(ConfigurationManager.AppSettings["KaznaFileStart"]);
                OwnerCodeStart = Boolean.Parse(ConfigurationManager.AppSettings["OwnerCodeStart"]);
                BronStart = Boolean.Parse(ConfigurationManager.AppSettings["BronStart"]);

                EcpCalcSheduler = int.Parse(ConfigurationManager.AppSettings["EcpCalcSheduler"]);
                MakeProvodkiSheduler = int.Parse(ConfigurationManager.AppSettings["MakeProvodkiSheduler"]);
                BranchCalcSheduler = int.Parse(ConfigurationManager.AppSettings["BranchCalcSheduler"]);
                KaznaFileSheduler = int.Parse(ConfigurationManager.AppSettings["KaznaFileSheduler"]);
                OwnerCodeSheduler = int.Parse(ConfigurationManager.AppSettings["OwnerCodeSheduler"]);
                BronSheduler = int.Parse(ConfigurationManager.AppSettings["BronSheduler"]);
            }
            catch
            {
                Console.WriteLine("Error in Configuration when converting value from app.config");
                throw;
            }

            kaznaController = new KaznaController();
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

                EcpCalc();

                var hourTime = int.Parse(DateTime.Now.ToString("HH"));

                if (hourTime >= 20 || hourTime <= 7)
                {
                    Thread.Sleep(600000); // 10 min run in no working hours
                }
                else
                {
                    Thread.Sleep(EcpCalcSheduler);
                }
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

                kaznaController.Process();

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
                    var orgInnEnter = new List<string>();

                    using (KasabaKIATEntities db = new KasabaKIATEntities())
                    {
                        var procList = db.sp_pr_GetNewTransactionsN1();

                        Console.WriteLine("Got data to process");

                        foreach (var payment in procList.ToList())
                        {
                            Console.WriteLine("New Iteration=============================");
                            MakeEcpRow(db, payment, orgInnEnter);
                        }

                        try
                        {
                            Console.WriteLine(DateTime.Now.ToLongTimeString() + "SaveChanges!!!!!================================== ");
                            db.SaveChanges();
                            orgInnEnter.Clear();
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
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    _logService.Info(ex.ToString());
                }

        }

        private void AssignDataToModel(EcpCalcViewModel model, sp_pr_GetNewTransactionsN1_Result1 payment)
        {
            Console.WriteLine("AssignDataToModel Called");
            model.payment_id = (long)payment.PAYMENT_ID;
            Console.WriteLine("payment_id = " + model.payment_id);
            model.profsouz_amt = payment.PROFSOUZ_AMT;
            model.federation_amt = payment.FEDERATION_AMT;
            model.oblsovprof_amt = payment.OBLSOVPROF_AMT;
            model.soato = payment.SOATO;
            model.soato_name = payment.SOATO_NAME;
            model.obl_department_base = payment.OBL_DEPARTMENT_BASE;
            Console.WriteLine("DEPARTMENT_BASE = " + payment.OBL_DEPARTMENT_BASE);

            model.operdate = payment.OPERDAY;
        }

        private string GetFinalINN(sp_pr_GetNewTransactionsN1_Result1 payment)
        {
            Console.WriteLine("GetFinalINN Called");

            var Inn = GetInnFromDetail(payment.PURPOSE, payment.INN);

            Console.WriteLine("Inn found from GetInnFromDetail = " + Inn);

            if (!string.IsNullOrEmpty(Inn) && (Inn.StartsWith("2") || Inn.StartsWith("3")))
            {
                Console.WriteLine("First Case = " + Inn);

                return Inn;
            }
            else
            {
                Console.WriteLine("Second Case = " + payment.INN);

                return payment.INN;
            }
        }

        private void AssignBranchInfo(int ID_Ot, string DepartmentBase, KasabaKIATEntities db, 
            EcpCalcViewModel model, sp_pr_GetNewTransactionsN1_Result1 payment)
        {
            Console.WriteLine("AssignBranchInfo Called");

            if (ID_Ot <= 0 || string.IsNullOrEmpty(DepartmentBase))
            {
                Console.WriteLine("ID_Ot = " + ID_Ot);
                Console.WriteLine("DepartmentBase = " + DepartmentBase);

                throw new ArgumentNullException("ID_Ot or DepartmentBase is invalid");
            }

            var currentOtrasl = db.tblSprav_Otras.Where(z => z.ID_Ot == ID_Ot
                                                    && z.DepartmentBase == DepartmentBase);

            if (currentOtrasl.Any())
            {
                var branchCode = currentOtrasl.First().BRANCH_CODE;

                var branchNameRow = db.sp_branch.Where(z => z.code == branchCode);

                if (branchNameRow.Any())
                {
                    var branchName = branchNameRow.First().name_ru;

                    model.branch_code = branchCode;
                    Console.WriteLine(DateTime.Now.ToLongTimeString() + " " + branchName);
                    model.branch_name = branchName;

                    switch (model.branch_code)
                    {
                        case "Н2":
                            model.adep_code = "173Н2";
                            break;
                        case "Т2":
                            model.adep_code = "173Т2";
                            break;
                        case "О1":
                            model.adep_code = "173О1";
                            break;
                        default:
                            model.adep_code = "173" + model.branch_code + payment.SOATO.Substring(2, 5);
                            break;
                    }
                }
                else
                {
                    throw new ApplicationException("See to sp_branch table branch is missing!!!");
                }
            }
            else
            {
                throw new ApplicationException("See to tblSprav_Otras table branch is missing!!!");
            }
        }

        private void CreateOrganization(KasabaKIATEntities db,
            EcpCalcViewModel model, sp_pr_GetNewTransactionsN1_Result1 payment)
        {
            Console.WriteLine(DateTime.Now.ToLongTimeString() + "CreateOrganization ");

            var orrg = new tblSprav_Org
            {
                INN = model.inn,
                IS_PRAVSOYUZ = false,
                ORIG_INN = model.inn,
                OKONX = model.okonx_id,
                FACE_TYPE = 0,
                UserId = 1,
                Name_Org = payment.CLNAME.Length > 100 ? payment.CLNAME.Substring(0, 100) : payment.CLNAME,
                DepartmentBase = string.IsNullOrEmpty(model.obl_department_base) ? null : model.obl_department_base
            };

            var region = db.tblSprav_Reg.Where(z => z.SOATO_CODE == model.soato);

            if (region.Any())
            {
                orrg.Region = region.First().CodeReg;
            }

            db.tblSprav_Org.Add(orrg);

            model.name_org = orrg.Name_Org;
        }


        private void AssignBranchOrCreateOrg(KasabaKIATEntities db,
            EcpCalcViewModel model, sp_pr_GetNewTransactionsN1_Result1 payment, List<string> orgInnEnter)
        {
            Console.WriteLine("AssignBranchOrCreateOrg called");
            var orgsList = db.tblSprav_Org.Where(z => z.INN == model.inn &&
            z.DepartmentBase == payment.OBL_DEPARTMENT_BASE);

            if (orgsList.Any())
            {
                Console.WriteLine("Organization Found!");

                var org1 = orgsList.First();

                Console.WriteLine("Name Org = ");
                Console.Write(org1.Name_Org);
                Console.WriteLine("Otrasl = ");
                Console.Write(org1.ID_Ot);

                if (org1.ID_Ot.HasValue)
                {
                    AssignBranchInfo(org1.ID_Ot.Value, org1.DepartmentBase, db, model, payment);
                }

                model.name_org = org1.Name_Org;
            }
            else
            {
                Console.WriteLine("Organization NOT Found!");
                string combine = string.Format("{0}{1}", model.inn, payment.OBL_DEPARTMENT_BASE);

                if (!orgInnEnter.Any(z => z == combine))
                {
                    CreateOrganization(db, model, payment);

                    orgInnEnter.Add(combine);
                }
            }
        }

        private void MakeEcpRow(KasabaKIATEntities db,
            sp_pr_GetNewTransactionsN1_Result1 payment, List<string> orgInnEnter)
        {
            Console.WriteLine("MakeEcpRow Called");

            var model = new EcpCalcViewModel();

            AssignDataToModel(model, payment);

            model.inn = GetFinalINN(payment);

            AssignBranchOrCreateOrg(db, model, payment, orgInnEnter);

            Console.WriteLine("EcpCalcDataManager.Create Call");
            EcpCalcDataManager.Create(ref model, db);
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
        /// Improvements: 
        /// 1) Reference tables which has less than 50 rows can be loaded to buffer. It will decrease query count
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

                        while(true)
                        {
                            var newOrgChangedList = db.tblSprav_Org.Where(z => z.TIMESTAMP_CH >= lastTime && z.ID_Ot.HasValue).Take(100);

                            if (newOrgChangedList.Any())
                            {

                                foreach (var org in newOrgChangedList)
                                {
                                    try
                                    {
                                        if (org.ID_Ot.HasValue)
                                        {
                                            var currentOtrasl = db.tblSprav_Otras.Where(z => z.ID_Ot == org.ID_Ot 
                                            && z.DepartmentBase == org.DepartmentBase);

                                            if (currentOtrasl.Any())
                                            {
                                                var branchCode = currentOtrasl.First().BRANCH_CODE;

                                                Console.WriteLine("update branch INN: " + org.INN);
                                                Console.WriteLine("update branch branchCode: " + branchCode);

                                                var branchNameRow = db.sp_branch.Where(z => z.code == branchCode);

                                                if (branchNameRow.Any())
                                                {
                                                    var branchName = branchNameRow.First().name_ru;

                                                    Console.WriteLine("update branch branchName: " + branchName);

                                                    var ecpChangeList = db.tbbu_ecp_calc.Where(z => z.INN == org.INN && z.OBL_DEPARTMENT_BASE == org.DepartmentBase);

                                                    if (ecpChangeList.Any())
                                                    {
                                                        foreach (var ecp in ecpChangeList)
                                                        {
                                                            ecp.BRANCH_CODE = branchCode;
                                                            ecp.BRANCH_NAME = branchName;
                                                            ecp.NAME_ORG = org.Name_Org;

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
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Exception in FOR==============");
                                        Console.WriteLine(ex.ToString());
                                    }

                                    org.TIMESTAMP_CH = DateTime.Now.AddDays(-365);

                                }

                                Console.WriteLine("=======BRANCH CHANGE ITERATION=========SaveChanges==================");

                                db.SaveChanges();

                            }
                            else
                            {
                                lastTimeRow.LastChanged = DateTime.Now;

                                Console.WriteLine("=======BRANCH CHANGE LAST=========SaveChanges==================");

                                db.SaveChanges();

                                break;
                            }
                        }


                        Console.WriteLine("DoubleBranch Begin*********** " + DateTime.Now.ToLongTimeString());

                        var doubleBranchs = db.sp_pr_DoubleBranchCheck().ToList();

                        if (doubleBranchs.Any())
                        {
                            foreach (var org in doubleBranchs)
                            {
                                Console.WriteLine("DoubleBranch Branch*********** " + DateTime.Now.ToLongTimeString());
                                Console.WriteLine("INN = " + org.INN + " " + DateTime.Now.ToLongTimeString());

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

                                            var ecpChangeList = db.tbbu_ecp_calc.Where(z => z.INN == org.INN && z.OBL_DEPARTMENT_BASE == org.DepartmentBase);

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
                                else
                                {
                                    var ecpChangeList = db.tbbu_ecp_calc.Where(z => z.INN == org.INN);

                                    if (ecpChangeList.Any())
                                    {
                                        foreach (var ecp in ecpChangeList)
                                        {
                                            ecp.BRANCH_CODE = null;
                                            ecp.BRANCH_NAME = null;
                                            ecp.ADEP_CODE = null;
                                        }
                                    }
                                }
                            }

                            db.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine("DoubleBranch Empty*********** " + DateTime.Now.ToLongTimeString());
                        }

                        Console.WriteLine("DoubleBranch !!!!!!*********** " + DateTime.Now.ToLongTimeString());
                    }

                    #endregion
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("General try for branch");
                Console.WriteLine(ex + DateTime.Now.ToLongTimeString());
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
        private string GetInnFromDetail(string detail, string clinn)
        {
            if (string.IsNullOrEmpty(detail)) return null;

            string[] numbers = Regex.Matches(detail, @"([-:,\~.\s*/A-Za-zА-Яа-я](\d\s?){8}\d[-:,.\s)*~/A-Za-zА-Яа-я]|\s(\d\s?){8}\d$)") //([-:,.\s*/A-Za-zА-Яа-я](\d\s?){8}\d[-:,.\s)*~/A-Za-zА-Яа-я]|\s(\d\s?){8}\d$)
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

                if (SkipFromDetail(sssdd))
                {
                    if (!clinn.Equals("000000000"))
                    {
                        continue;
                    }
                }
                else if (!sssdd.Substring(1, 1).Equals("0"))
                {
                    continue;
                }

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

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <returns></returns>
        private bool SkipFromDetail(string inn)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var isExist = db.tbbu_ecp_inn_setting.Where(z => z.INN == inn && 
                (z.PLACE_ESC == (int)EscapePlaces.DETAIL || z.PLACE_ESC == (int)EscapePlaces.ALL));

                if (isExist.Any())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inn"></param>
        /// <returns></returns>
        private bool SkipFromCLL(string inn)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var isExist = db.tbbu_ecp_inn_setting.Where(z => z.INN == inn && 
                (z.PLACE_ESC == (int)EscapePlaces.CLL || z.PLACE_ESC == (int)EscapePlaces.ALL));

                if (isExist.Any())
                {
                    return true;
                }
            }

            return false;
        }

    }

    public enum EscapePlaces
    {
        CLL = 1,
        DETAIL ,
        ALL = 9
    }

    public enum Where2Takes
    {
        CLL = 1,
        DETAIL,
        ANY = 9
    }
}