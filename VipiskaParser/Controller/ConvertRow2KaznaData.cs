using System;
using System.Collections.Generic;
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


    public class ConvertRow2KaznaData
    {
        public static List<KaznaModel> resList { get; set; }

        public static List<KaznaModel> resListGrouped { get; set; }

        public static List<KaznaModel> chastichka { get; set; }

        public static List<KaznaModel> chastichkaGrouped { get; set; }

        public static List<KaznaModel> other { get; set; }

        public static List<KaznaModel> debitOborot { get; set; }

        public static int totalAborot { get; set; }

        public static decimal totalAmount { get; set; }

        public static decimal totalAmountDebit { get; set; }
        public static decimal KaznaAmount { get; set; }
        public static decimal chatichka { get; set; }
        public static decimal OtherAmount { get; set; }

        public static List<KaznaModel> Process(List<InputRow> lData)
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
