using KasabaKIAT.ViewModel.Buxgalteriya;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VipiskaParser.Model;

namespace VipiskaParser.Controller
{
    //RegularExpressions
    //[0-9]{20} - details1 to get 20 digit account number
    //if from details1 20 digit equals to 23402000300100001010 it is from Kaznacheystvo
    //then Apply pattern below
    //\"(.*?)\" and get 2 matchet and write it to separete fields
    //

    public struct Dlt01
    {
        public string Mfo;
        public string Account;
        public string Inn;
    }

    public class ConvertRow2KaznaData
    {
        public static List<KaznaModel> resList { get; set; }

        public static List<KaznaModel> resListGrouped { get; set; }

        public static List<KaznaModel> chastichka { get; set; }

        public static List<KaznaModel> chastichkaGrouped { get; set; }

        public static List<KaznaModel> other { get; set; }

        public static List<KaznaModel> debitOborot { get; set; }

        public static List<BankpaymentsViewModel> platList { get; set; }

        public static int totalAborot { get; set; }

        public static decimal totalAmount { get; set; }

        public static decimal totalAmountDebit { get; set; }
        public static decimal KaznaAmount { get; set; }
        public static decimal chatichka { get; set; }
        public static decimal OtherAmount { get; set; }

        public static List<KaznaModel> ProcessOld(List<InputRow> lData)
        {
            totalAmount = 0;
            KaznaAmount = 0;
            chatichka = 0;
            OtherAmount = 0;
            resList = new List<KaznaModel>();
            chastichka = new List<KaznaModel>();
            other = new List<KaznaModel>();
            debitOborot = new List<KaznaModel>();
            Regex regexDtl1 = new Regex(@"[0-9]{20}");
            Regex regexDtl2 = new Regex(@"\""(.*?)\""");
            var predSettings = new PredContainer();
            var debitSettings = new DebitPredefined();

            if (lData == null) return null;

            totalAborot = lData.Count;

            foreach (var obj in lData)
            {
                KaznaModel model = new KaznaModel(); 
                MatchCollection mtch1 = regexDtl1.Matches(obj.detailsRow1);

                if (mtch1.Count > 0)
                {
                    foreach(Match objmtch in mtch1)
                    {
                        if (objmtch.Value.Length == 20)
                        {
                            if (objmtch.Value.Equals("23402000300100001010")) //Kaznacheystvo account
                            {
                                MatchCollection mtch2 = regexDtl2.Matches(obj.detailsRow3);

                                if (mtch2.Count > 0)
                                {
                                    int counter = 0;
                                    foreach (Match objmtch2 in mtch2)
                                    {
                                        if (counter == 0)
                                        {
                                            model.Oblast = objmtch2.Value;
                                        }
                                        if (counter == 1)
                                        {
                                            model.Rayon = objmtch2.Value;
                                        }

                                        counter++;
                                    }

                                    if (counter > 0 )
                                    {
                                        resList.Add(model);
                                    }
                                    
                                }

                                decimal outValue;
                                if (decimal.TryParse(CorrectAmount(obj.Credit), out outValue))
                                {
                                    model.Amount = outValue;
                                    KaznaAmount = KaznaAmount + outValue;
                                }
                                else
                                {
                                    model.Amount = 0;
                                }
                            }
                            else
                            {
                                var isInProfAccounts1 = predSettings.listPred.Where(z => z.RaschetniySchet1.Equals(objmtch.Value));
                                var isInProfAccounts2 = predSettings.listPred.Where(z => z.RaschetniySchet2.Equals(objmtch.Value));

                                if (isInProfAccounts1.Any() || isInProfAccounts2.Any())
                                {
                                    string obls = string.Empty;

                                    var isInProfAccounts1N = predSettings.listPred.Where(z => z.RaschetniySchet1.Equals(objmtch.Value));
                                    if (isInProfAccounts1N.Any())
                                    {
                                        obls = isInProfAccounts1N.First().Naimenovaniye;
                                    }

                                    var isInProfAccounts2N = predSettings.listPred.Where(z => z.RaschetniySchet2.Equals(objmtch.Value));
                                    if (isInProfAccounts2N.Any())
                                    {
                                        obls = isInProfAccounts2N.First().Naimenovaniye;
                                    }

                                    decimal outValue;
                                    if (decimal.TryParse(CorrectAmount(obj.Credit), out outValue))
                                    {
                                        chatichka = chatichka + outValue;

                                        chastichka.Add(new KaznaModel() { Amount = outValue, Oblast = obls, Rayon = obj.detailsRow3 });
                                    }
                                }
                                else
                                {
                                    //other
                                    decimal outValue;
                                    if (decimal.TryParse(CorrectAmount(obj.Credit), out outValue))
                                    {
                                        OtherAmount = OtherAmount + outValue;

                                        other.Add(new KaznaModel() { Amount = outValue, Oblast = obj.detailsRow2, Rayon = obj.detailsRow3 });
                                    }
                                }

                            }
                        }


                    }
                }

                //DebitAnalizator
                
                decimal outValue2;
                if (decimal.TryParse(CorrectAmount(obj.Credit), out outValue2))
                {
                    totalAmount = totalAmount + outValue2;
                }

                decimal outValue2Debit;
                if (decimal.TryParse(CorrectAmount(obj.Debit), out outValue2Debit))
                {
                    totalAmountDebit = totalAmountDebit + outValue2Debit;

                    if (outValue2Debit > 0)
                    {
                        debitOborot.Add(new KaznaModel() { Amount = outValue2Debit, Oblast = obj.detailsRow2, Rayon = obj.detailsRow3 });
                    }
                }

            }

            Console.WriteLine("Количество оборотов = ", lData.Count);
            Console.WriteLine(string.Format("Общая сумма по Дебиту = {0}", totalAmountDebit));
            Console.WriteLine(string.Format("Общая сумма по Кредиту = {0}", totalAmount));
            Console.WriteLine(string.Format("Сумма от Казначейства = {0}", KaznaAmount));
            Console.WriteLine(string.Format("Сумма по Частичным оплатам = {0}", chatichka));
            Console.WriteLine(string.Format("Другие суммы = {0}", OtherAmount));

            return resList;
        }

        public static List<BankpaymentsViewModel> Process(List<InputRow> lData)
        {
            platList = new List<BankpaymentsViewModel>();
            Regex regexDtl1 = new Regex(@"[0-9]{20}");
            Regex regexDtl2 = new Regex(@"\""(.*?)\""");
            var predSettings = new PredContainer();
            var debitSettings = new DebitPredefined();

            if (lData == null) return null;

            totalAborot = lData.Count;

            foreach (var obj in lData)
            {
                KaznaModel model = new KaznaModel();
                MatchCollection mtch1 = regexDtl1.Matches(obj.detailsRow1);

                if (mtch1.Count > 0)
                {
                    foreach (Match objmtch in mtch1)
                    {
                        if (objmtch.Value.Length == 20)
                        {
                            if (objmtch.Value.Equals("23402000300100001010")) //Kaznacheystvo account
                            {
                                continue;
                            }
                            else
                            {
                                decimal outValue;
                                if (decimal.TryParse(CorrectAmount(obj.Credit), out outValue))
                                {
                                    if (outValue > 0)
                                    {
                                        var isInProfAccounts1 = predSettings.listPred.Where(z => z.RaschetniySchet1.Equals(objmtch.Value));
                                        var isInProfAccounts2 = predSettings.listPred.Where(z => z.RaschetniySchet2.Equals(objmtch.Value));

                                        if (isInProfAccounts1.Any() || isInProfAccounts2.Any())
                                        {
                                            var modelb = new BankpaymentsViewModel();

                                            modelb.num_doc = obj.DocumentNumber;
                                            modelb.payer_name = obj.detailsRow2;
                                            var St = ParseDtl1(obj.detailsRow1);

                                            modelb.payer_inn        = St.Inn;
                                            modelb.payer_mfo        = St.Mfo;
                                            modelb.payer_account    = St.Account;

                                            DateTime trandate;
                                            if (DateTime.TryParseExact(obj.DateStr, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out trandate))
                                            {
                                                modelb.tran_date = trandate;
                                            }
                                            else
                                            {
                                                modelb.tran_date = DateTime.Now;

                                            }

                                            modelb.payment_datail = obj.detailsRow3;

                                            modelb.amount = outValue;

                                            modelb.benef_name   = predSettings.listPred[0].Naimenovaniye;
                                            modelb.benef_inn    = predSettings.listPred[0].Inn;
                                            modelb.benef_mfo    = "00000";
                                            modelb.benef_account= predSettings.listPred[0].RaschetniySchet1;
                                            modelb.dbcrflg      = "C";

                                            platList.Add(modelb);
                                        }
                                    }
                                }

                            }
                        }


                    }
                }

            }

            return platList;
        }

        public static string CorrectAmount(string inputAmount)
        {
            if (!inputAmount.Any(z=>z == '.'))
            {
                if (inputAmount.Any(z => z == ','))
                {
                    return inputAmount;
                }
            }

            return inputAmount.Replace(",", "").Replace(".", ",");
        }

        public static Dlt01 ParseDtl1(string input)
        {
            Dlt01 model = new Dlt01();
            Regex regexDtl1 = new Regex(@"МФО\:[0-9]{5}");
            Regex regexDtl2 = new Regex(@"Счет\:[0-9]{20}");
            Regex regexDtl3 = new Regex(@"ИНН\:[0-9]{9}");

            Regex regexDtl11 = new Regex(@"[0-9]{5}");
            Regex regexDtl21 = new Regex(@"[0-9]{20}");
            Regex regexDtl31 = new Regex(@"[0-9]{9}");

            MatchCollection mtch1 = regexDtl1.Matches(input);
            MatchCollection mtch2 = regexDtl2.Matches(input);
            MatchCollection mtch3 = regexDtl3.Matches(input);

            if (mtch1.Count > 0 && mtch2.Count > 0 && mtch3.Count > 0)
            {
                foreach(Match mm in mtch1)
                {
                    var text1 = mm.Value;
                    MatchCollection mtch11 = regexDtl11.Matches(text1);

                    foreach (Match mm1 in mtch11)
                    {
                        model.Mfo = mm1.Value;
                    }
                }

                foreach (Match mm in mtch2)
                {
                    var text2 = mm.Value;
                    MatchCollection mtch21 = regexDtl21.Matches(text2);

                    foreach (Match mm1 in mtch21)
                    {
                        model.Account = mm1.Value;
                    }

                }

                foreach (Match mm in mtch3)
                {
                    var text3 = mm.Value;
                    MatchCollection mtch31 = regexDtl31.Matches(text3);

                    foreach (Match mm1 in mtch31)
                    {
                        model.Inn = mm1.Value;
                    }
                }
            }

            return model;
        }


        public static void GroupKaznaAndChastichka()
        {
            if (KaznaAmount > 0)
            {
                resListGrouped = resList
                .GroupBy(l => l.Oblast)
                .Select(cl => new KaznaModel
                {
                    Oblast = cl.First().Oblast,
                    Amount = cl.Sum(c => c.Amount),
                }).ToList();

                Console.WriteLine("==============================================");
                Console.WriteLine("===Поступление по Областьям от Казначейства=====");
                Console.WriteLine("==============================================");

                foreach (var obj in resListGrouped)
                {
                    Console.WriteLine(string.Format(" {0} {1} ", obj.Oblast, obj.Amount)); 
                }
            }


            if (chatichka > 0)
            {
                chastichkaGrouped = chastichka
                .GroupBy(l => l.Oblast)
                .Select(cl => new KaznaModel
                {
                    Oblast = cl.First().Oblast,
                    Amount = cl.Sum(c => c.Amount),
                }).ToList();

                Console.WriteLine("==============================================");
                Console.WriteLine("===Поступление по Областьям по Частичным оплатам=====");
                Console.WriteLine("==============================================");

                foreach (var obj in chastichkaGrouped)
                {
                    Console.WriteLine(string.Format(" {0} {1} ", obj.Oblast, obj.Amount));
                }
            }

            if (OtherAmount > 0)
            {
                Console.WriteLine("==============================================");
                Console.WriteLine("===Поступление по Прочим оплатам=====");
                Console.WriteLine("==============================================");

                foreach (var obj in other)
                {
                    Console.WriteLine(string.Format(" {0} {1} {2}", obj.Oblast, obj.Amount, obj.Rayon));
                }
            }

            if (totalAmountDebit > 0 )
            {
                Console.WriteLine("==============================================");
                Console.WriteLine("===Дебитовой оборот=====");
                Console.WriteLine("==============================================");

                foreach (var obj in debitOborot)
                {
                    Console.WriteLine(string.Format(" {0} {1} {2}", obj.Oblast, obj.Amount, obj.Rayon));
                }
            }


        }
    }
}
