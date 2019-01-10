using KasabaKiat.BusinessLogic.Putyovki.OplataPutyovok;
using KasabaKIAT.BusinessLogic.StartProcess.LoaderViewModels;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.StartProcess.LoaderDataManagers
{
    public class PutyovkaOstatokDataManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DepartmentBase"></param>
        /// <param name="UserId"></param>
        /// <param name="models"></param>
        /// <param name="db"></param>
        public static void Enter(string DepartmentBase, long UserId, List<PutyovkiOstatokViewModel> models, KasabaKIATEntities db)
        {
            var naklad = new tblPost();

            var departmentxxx = DepartmentBase.PadLeft(9, '9');

            var orgs = db.tblSprav_Org.Where(z => z.INN == departmentxxx);

            if (orgs.Any())
            {
                naklad.tblSprav_Org = orgs.First();
            }
            else
            {
                var org = new tblSprav_Org();
                org.INN = DepartmentBase.PadLeft(9, '9');
                org.Name_Org = "Бошлангич Организация";

                db.tblSprav_Org.Add(org);

                naklad.tblSprav_Org = org;
            }


            //Nakladnaya 
            naklad.Data_Post = DateTime.ParseExact("31.12.2018", "dd.MM.yyyy", CultureInfo.InvariantCulture);
            naklad.DepartmentBase = DepartmentBase;
            naklad.UserId = UserId;
            naklad.SmFil = 2018;
            naklad.ID_KV = 4;

            db.tblPost.Add(naklad);

            //Putyovka

            foreach(var model in models)
            {
                var putyovka = new tblPos_Put
                {
                    tblPost = naklad,
                    ID_Sanat = model.SanatoryCode,
                    Num_Put = model.NumPut,
                    DataSan = model.DataSan,
                    Symma_Put = model.Summa,
                    Data_Post = DateTime.Now,
                    Departmentbase = DepartmentBase,
                    UserId = UserId,
                    TIMESTAMP_T = DateTime.Now
                };

                db.tblPos_Put.Add(putyovka);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DepartmentBase"></param>
        /// <param name="UserId"></param>
        /// <param name="models"></param>
        /// <param name="db"></param>
        public static void EnterKengash(string DepartmentBase, long UserId, List<PutyovkiOstatokViewModel> models, KasabaKIATEntities db)
        {

            List<string> distinctInns = models
              .GroupBy(p => p.Inn)
              .Select(g => g.First().Inn)
              .ToList();

            foreach(var inn in distinctInns)
            {
                //Kngashu Nakladnaya
                #region Nakladnaya
                var naklad = new tblPost();
                var rasxodNaklad = new tblVipl();
                var oplata = new tblOplata();

                string owner_code = string.Empty;

                var orgs = db.tblSprav_Org.Where(z => z.INN == inn && z.DepartmentBase == DepartmentBase);

                if (orgs.Any())
                {
                    var organization = orgs.First();
                    owner_code = organization.OWNER_CODE;
                    naklad.tblSprav_Org = organization;
                    rasxodNaklad.tblSprav_Org = organization;
                    oplata.tblSprav_Org = organization;
                }
                else
                {
                    continue;
                }

                //Nakladnaya Prixod
                naklad.Data_Post = DateTime.ParseExact("31.12.2018", "dd.MM.yyyy", CultureInfo.InvariantCulture);
                naklad.DepartmentBase = orgs.First().OWNER_CODE;
                naklad.UserId = UserId;
                naklad.SmFil = 2018;
                naklad.ID_KV = 4;

                db.tblPost.Add(naklad);

                rasxodNaklad.Data_Post = DateTime.ParseExact("31.12.2018", "dd.MM.yyyy", CultureInfo.InvariantCulture);
                rasxodNaklad.DepartmentBase = DepartmentBase;
                rasxodNaklad.UserId = UserId;
                rasxodNaklad.SmFil = 2018;
                rasxodNaklad.ID_Kv = 4;

                db.tblVipl.Add(rasxodNaklad);

                oplata.Data_Post = DateTime.ParseExact("31.12.2018", "dd.MM.yyyy", CultureInfo.InvariantCulture);
                oplata.Num_Doc = "1";
                oplata.Symma_DT = 0;
                oplata.Symma_KT = 0;
                oplata.Osnov = "";
                oplata.ID_KV =4;
                oplata.SmFil = 2018;
                oplata.Name_org ="";
                oplata.Tip_Org = true;
                oplata.PrizOpl = (int)OplataRasxPriz.OrganizasiyaRasxod;
                oplata.UserId = UserId;
                oplata.Departmentbase = DepartmentBase;

                db.tblOplatas.Add(oplata);
                #endregion

                //Putyovka
                #region Putyovkas
                var modelsL = models.Where(z => z.Inn == inn);

                foreach (var model in modelsL)
                {
                    var putyovka = new tblPos_Put
                    {
                        tblPost = naklad,
                        ID_Sanat = model.SanatoryCode,
                        Num_Put = model.NumPut,
                        DataSan = model.DataSan,
                        Symma_Put = model.Summa,
                        Data_Post = DateTime.Now,
                        Departmentbase = owner_code,
                        UserId = UserId,
                        ID_PolVoz = false
                    };

                    db.tblPos_Put.Add(putyovka);


                    var putyovkaras = new tblVipl_Put
                    {
                        tblVipl = rasxodNaklad,
                        ID_Sanat = model.SanatoryCode,
                        Num_Put = model.NumPut,
                        Num_Proc = 1,
                        Symma = model.Summa,
                        Data_Post = DateTime.Now,
                        DepartmentBase = DepartmentBase,
                        UserId = UserId,
                        ID_PolVoz = false
                    };

                    db.tblVipl_Put.Add(putyovkaras);

                    var opl = new tblOplata_Ras
                    {
                        tblOplata = oplata,
                        ID_Sanat = model.SanatoryCode,
                        Num_Put = model.NumPut,
                        Symma = Decimal.Multiply(model.Summa, 0.45M),
                        ID_Pr = 1,
                        Priz = (int)OplataRasxPriz.OrganizasiyaRasxod,
                        UserId = UserId,
                        Departmentbase = DepartmentBase
                    };

                    db.tblOplata_Ras.Add(opl);
                }

                #endregion
            }
        }
    }
}