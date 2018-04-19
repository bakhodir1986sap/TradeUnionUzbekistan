using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using KasabaKIAT.ViewModel.Buxgalteriya;
using KaznaFilesImporter;
using Epigov.Log;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki.AutomaticTransactions.KaznaTransactions
{
    public class EcpCalculation
    {
        public void CalcProcess()
        {
            var _logService = new FileLogService(typeof(Program));

            _logService.Info("Begin Calc======");

            try
            {
                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    db.sp_im_KaznaNewOrg();

                    List<string> blackListInn = new List<string>();

                    var procList = db.sp_pr_GetNewTransactions();

                    var enterList = new List<EcpCalcViewModel>();

                    if (procList != null)
                    {
                        var model = new EcpCalcViewModel();

                        string innFromDtl = string.Empty;

                        foreach (var payment in procList)
                        {
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

                            model.payment_id = (long)payment.ID;

                            //Inn Parsing
                            #region Inn Parsing

                            var bankAccountBalance = payment.CLACC.Substring(0, 5);

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
                            else if (payment.PURPOSE.ToUpper().Contains("ИНН"))
                            {
                                string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                                //First 9 digit number As INN
                                var Inns = numbers.Where(x => x.Length == 9);

                                var posOfInn = payment.PURPOSE.ToUpper().IndexOf("ИНН");

                                foreach (var srt in Inns)
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
                            else if (payment.PURPOSE.ToUpper().Contains("INN"))
                            {
                                string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                                //First 9 digit number As INN
                                var Inns = numbers.Where(x => x.Length == 9);

                                var posOfInn = payment.PURPOSE.ToUpper().IndexOf("INN");

                                foreach (var srt in Inns)
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
                            else if (payment.PURPOSE.ToUpper().Contains("СТИР"))
                            {
                                string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                                //First 9 digit number As INN
                                var Inns = numbers.Where(x => x.Length == 9);

                                var posOfInn = payment.PURPOSE.ToUpper().IndexOf("СТИР");

                                foreach (var srt in Inns)
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
                            else if (payment.PURPOSE.ToUpper().Contains("STIR"))
                            {
                                string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                                //First 9 digit number As INN
                                var Inns = numbers.Where(x => x.Length == 9);

                                var posOfInn = payment.PURPOSE.ToUpper().IndexOf("STIR");

                                foreach (var srt in Inns)
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
                                model.inn = "PROBLEM";
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

                            //if (payment.CLINN.Contains("000000000"))
                            //{
                            //    string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                            //    //First 9 digit number As INN
                            //    var Inn = numbers.FirstOrDefault(x => x.Length == 9);

                            //    if (Inn != null)
                            //    {
                            //        payment.CLINN = Inn;
                            //    }
                            //}
                            //else if (payment.CLINN.Equals(payment.COINN))
                            //{
                            //    string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                            //    //First 9 digit number As INN
                            //    var Inn = numbers.FirstOrDefault(x => x.Length == 9);

                            //    if (Inn != null)
                            //    {
                            //        payment.CLINN = Inn;
                            //    }
                            //}
                            //else
                            //{

                            //}

                            #endregion

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

                            var str = payment.PURPOSE.Substring(6, 25).Substring(10, 5);

                            var prepsoato = string.Format("{0}{1}", "17", str);

                            var soatoExists = db.tbsp_soato_new.Where(x => x.KODI == prepsoato);

                            if (soatoExists.Any())
                            {
                                var soato = soatoExists.First().KODI;
                                var soato_nm = soatoExists.First().NAME_RU;
                                model.remark += string.Format("SOATO:{0};SOATO_NAME:{1};", soato, soato_nm);
                                model.obl_department_base = string.Format("{0}{1}{2}", prepsoato.Substring(0, 2), "2", prepsoato.Substring(2, 2));
                                model.soato = soato;
                                model.soato_name = soato_nm;
                            }
                            else
                            {
                                string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                                //First 9 digit number As INN
                                var Inns = numbers.Where(x => x.Length == 27);

                                if (Inns.Any())
                                {
                                    var str2 = payment.PURPOSE.Substring(6, 27).Substring(9, 5);

                                    var prepsoato2 = string.Format("{0}{1}", "17", str2);

                                    var soatoExists2 = db.tbsp_soato_new.Where(x => x.KODI == prepsoato2);

                                    if (soatoExists2.Any())
                                    {
                                        //model.remark += string.Format("SOATO:{0};SOATO_NAME:{1};", soatoExists2.First().KODI, soatoExists2.First().NAME_RU);
                                        //model.obl_department_base = string.Format("{0}{1}{2}", prepsoato2.Substring(0, 2), "2", prepsoato2.Substring(2, 2));

                                        var soato = soatoExists2.First().KODI;
                                        var soato_nm = soatoExists2.First().NAME_RU;
                                        model.remark += string.Format("SOATO:{0};SOATO_NAME:{1};", soato, soato_nm);
                                        model.obl_department_base = string.Format("{0}{1}{2}", prepsoato.Substring(0, 2), "2", prepsoato.Substring(2, 2));
                                        model.soato = soato;
                                        model.soato_name = soato_nm;
                                    }
                                }
                            }

                            if (payment.CLINN.Equals(innFromDtl))
                            {
                                var egrpo = db.t_egrpo.Where(z => z.inn == payment.CLINN);

                                var nibddNumber1 = payment.CLACC.Substring(9, 8);

                                var fermer1 = db.tbbFermers.Where(z => z.INN == nibddNumber1);

                                if (fermer1.Any())
                                {
                                    model.remark += string.Format("{0}-{1}", fermer1.First().ID, "TASHKENT");
                                }


                                if (egrpo.Any())
                                {
                                    var egrpo1 = egrpo.First();

                                    if (!string.IsNullOrEmpty(egrpo1.soato_id))
                                    {
                                        if (egrpo1.soato_id.Length >= 4)
                                        {

                                            var soatoTmp = string.Format("{0}{1}{2}", egrpo1.soato_id.Substring(0, 2), "2", egrpo1.soato_id.Substring(2, 2));
                                            model.obl_department_base = soatoTmp;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(payment.CLACC))
                                    {
                                        var nibddNumber = payment.CLACC.Substring(9, 8);

                                        var fermer = db.tbbFermers.Where(z => z.INN == nibddNumber);

                                        //This is Tashkent
                                        if (fermer.Any())
                                        {
                                            model.obl_department_base = "17226";
                                            //
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var egrpo = db.t_egrpo.Where(z => z.inn == innFromDtl);

                                var nibddNumber1 = payment.CLACC.Substring(9, 8);

                                var fermer1 = db.tbbFermers.Where(z => z.INN == nibddNumber1);

                                if (fermer1.Any())
                                {
                                    model.remark += string.Format("{0}-{1}", fermer1.First().ID, "TASHKENT");
                                }


                                if (egrpo.Any())
                                {
                                    var egrpo1 = egrpo.First();

                                    if (!string.IsNullOrEmpty(egrpo1.soato_id))
                                    {
                                        if (egrpo1.soato_id.Length >= 4)
                                        {

                                            var soatoTmp = string.Format("{0}{1}{2}", egrpo1.soato_id.Substring(0, 2), "2", egrpo1.soato_id.Substring(2, 2));
                                            model.obl_department_base = soatoTmp;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(payment.CLACC))
                                    {
                                        var nibddNumber = payment.CLACC.Substring(9, 8);

                                        var fermer = db.tbbFermers.Where(z => z.INN == nibddNumber);

                                        //This is Tashkent
                                        if (fermer.Any())
                                        {
                                            model.obl_department_base = "17226";
                                            //
                                        }
                                    }
                                }
                            }


                            #endregion

                            EcpCalcDataManager.Create(ref model);

                        }
                    }

                    var provodki = new Provodki();

                    var trans = new ProvodkiViewModel();

                    var NotProcList = db.tbbu_ecp_calc.Where(z => z.PROVODKA_ID == 0 && z.OBL_DEPARTMENT_BASE != null);

                    if (NotProcList.Any())
                    {
                        foreach (var pp in NotProcList)
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

                            //Obshaya provodka
                            trans.comment = "Postupleniya po ECP vsego";
                            trans.department_base = pp.OBL_DEPARTMENT_BASE;
                            trans.inoutflg = false;
                            trans.platelrsch = "004";
                            trans.poluchrsch = "18";
                            trans.summa = pp.PROFSOUZ_AMT;
                            trans.user_id = 1;
                            trans.trdt = DateTime.Now;

                            ProvodkiDataManager.Create(ref trans);

                            pp.PROVODKA_ID = trans.rec_id;

                            var ppmodel = new EcpCalcViewModel();
                            ppmodel.id = pp.ID;
                            ppmodel.provodka_id = pp.PROVODKA_ID;

                            EcpCalcDataManager.Modify(ppmodel);

                            //----------------------------------

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

                            //Obshaya provodka
                            trans.comment = "Postupleniya po ECP federasiya";
                            trans.department_base = pp.OBL_DEPARTMENT_BASE;
                            trans.inoutflg = false;
                            trans.platelrsch = "22";
                            trans.poluchrsch = "004";
                            trans.summa = pp.FEDERATION_AMT;
                            trans.user_id = 1;
                            trans.trdt = DateTime.Now;

                            ProvodkiDataManager.Create(ref trans);

                            pp.PROVODKA_ID = trans.rec_id;

                            var ppmodel2 = new EcpCalcViewModel();
                            ppmodel2.id = pp.ID;
                            ppmodel2.provodka_id = pp.PROVODKA_ID;

                            EcpCalcDataManager.Modify(ppmodel2);

                            //----------------------------------

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

                            //Obshaya provodka
                            trans.comment = "Postupleniya po ECP oblsovprof";
                            trans.department_base = pp.OBL_DEPARTMENT_BASE;
                            trans.inoutflg = false;
                            trans.platelrsch = "20";
                            trans.poluchrsch = "004";
                            trans.summa = pp.OBLSOVPROF_AMT;
                            trans.user_id = 1;
                            trans.trdt = DateTime.Now;

                            ProvodkiDataManager.Create(ref trans);

                            pp.PROVODKA_ID = trans.rec_id;

                            var ppmodel3 = new EcpCalcViewModel();
                            ppmodel3.id = pp.ID;
                            ppmodel3.provodka_id = pp.PROVODKA_ID;

                            EcpCalcDataManager.Modify(ppmodel3);

                        }
                    }

                }
            }
            catch(Exception ex)
            {
                _logService.Error(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
        }
    }
}