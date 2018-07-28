using KasabaKIAT.BusinessLogic.StartProcess.LoaderViewModels;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.StartProcess.LoaderDataManagers
{
    public class PutyovkaOstatokDataManager
    {
        public static void Enter(string DepartmentBase, long UserId, List<PutyovkiOstatokViewModel> models, KasabaKIATEntities db)
        {
            var naklad = new tblPost(); 

            var orgs = db.tblSprav_Org.Where(z => z.INN == DepartmentBase.PadLeft(9, '9'));

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
            naklad.Data_Post = DateTime.Now;
            naklad.DepartmentBase = DepartmentBase;
            naklad.UserId = UserId;
            naklad.SmFil = 2017;
            naklad.ID_KV = 4;

            db.tblPost.Add(naklad);

            //Putyovka

            foreach(var model in models)
            {
                var putyovka = new tblPos_Put();

                putyovka.tblPost = naklad;
                putyovka.ID_Sanat = model.SanatoryCode;
                putyovka.Num_Put = model.NumPut;
                putyovka.DataSan = model.DataSan;
                putyovka.Symma_Put = model.Summa;
                putyovka.Data_Post = DateTime.Now;

                db.tblPos_Put.Add(putyovka);
            }
        }
    }
}