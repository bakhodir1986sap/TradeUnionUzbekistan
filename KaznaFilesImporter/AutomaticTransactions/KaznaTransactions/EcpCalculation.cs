using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using KasabaKIAT.ViewModel.Buxgalteriya;
using KaznaFilesImporter;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki.AutomaticTransactions.KaznaTransactions
{
    public class EcpCalculation
    {
        public void CalcProcess()
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                db.sp_im_KaznaNewOrg();

                var procList = db.sp_pr_GetNewTransactions();

                var enterList = new List<EcpCalcViewModel>();

                if (procList != null)
                {
                    var model = new EcpCalcViewModel();

                    foreach (var payment in procList)
                    {
                        //Clear Model
                        model.federation_amt = 0;
                        model.id = 0;
                        model.oblsovprof_amt = 0;
                        model.obl_department_base = string.Empty;
                        model.payment_id = 0;
                        model.profsouz_amt = 0;
                        model.provodka_id = 0;


                        model.payment_id = (long)payment.ID;

                        //Inn Parsing
                        #region Inn Parsing
                        if (payment.CLINN.Contains("000000000"))
                        {
                            string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                            //First 9 digit number As INN
                            var Inn = numbers.FirstOrDefault(x => x.Length == 9);

                            if (Inn != null)
                            {
                                payment.CLINN = Inn;
                            }
                        }
                        else if (payment.CLINN.Equals(payment.COINN))
                        {
                            string[] numbers = Regex.Split(payment.PURPOSE, @"\D+");

                            //First 9 digit number As INN
                            var Inn = numbers.FirstOrDefault(x => x.Length == 9);

                            if (Inn != null)
                            {
                                payment.CLINN = Inn;
                            }
                        }
                        else
                        {

                        }

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
                        var egrpo = db.t_egrpo.Where(z => z.inn == payment.CLINN);

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

                        #endregion

                        EcpCalcDataManager.Create(ref model);

                    }
                }

               var provodki = new Provodki();

               var trans = new ProvodkiViewModel();

               var NotProcList = db.tbbu_ecp_calc.Where(z => z.PROVODKA_ID == null && z.OBL_DEPARTMENT_BASE != null);

               if (NotProcList.Any())
               {
                   foreach(var pp in NotProcList)
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
    }
}