using KasabaKiat.BusinessLogic.Putyovki.OplataPutyovok;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.OplataPutyovok.Template
{
    public class OplataImporter
    {
        public static void EnterOplata(string DepartmentBase, 
                                        long UserId, 
                                        int year, 
                                        int kvartal, 
                                        List<OplataViplataTemplateViewModel> models, 
                                        KasabaKIATEntities db)
        {
            List<string> distinctInns = models
            .GroupBy(p => p.inn)
            .Select(g => g.First().inn)
            .ToList();

            foreach (var inn in distinctInns)
            {
                var oplata = new tblOplata();

                var orgs = db.tblSprav_Org.Where(z => z.INN == inn && z.DepartmentBase == DepartmentBase);

                if (orgs.Any())
                {
                    var organization = orgs.First();
                    oplata.tblSprav_Org = organization;
                }
                else
                {
                    continue;
                }

                oplata.Data_Post = DateTime.Now;
                oplata.Num_Doc = models.Where(z => z.inn == inn).Max(z => z.num_scetfak);
                oplata.Symma_DT = 0;
                oplata.Symma_KT = models.Where(z=>z.inn == inn).Sum(z=>z.oplacheno);
                oplata.Osnov = "";
                oplata.ID_KV = kvartal;
                oplata.SmFil = year;
                oplata.Name_org = "";
                oplata.Tip_Org = true;
                oplata.PrizOpl = (int)OplataRasxPriz.OrganizasiyaRasxod;
                oplata.UserId = UserId;
                oplata.Departmentbase = DepartmentBase;

                db.tblOplatas.Add(oplata);

                var modelsL = models.Where(z => z.inn == inn);

                foreach (var model in modelsL)
                {
                    var opl = new tblOplata_Ras
                    {
                        tblOplata = oplata,
                        ID_Sanat = model.sanatoriy_code,
                        Num_Put = model.nomer_putyovki,
                        Symma = model.oplacheno,
                        ID_Pr = model.procent,
                        Priz = (int)OplataRasxPriz.OrganizasiyaRasxod,
                        UserId = UserId,
                        Departmentbase = DepartmentBase
                    };

                    db.tblOplata_Ras.Add(opl);
                }
            }
        }
    }
}