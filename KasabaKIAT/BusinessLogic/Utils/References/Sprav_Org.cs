using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Utils.References
{
    public class Sprav_Org
    {
        public static void Create(ref Sprav_OrgViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tblSprav_Org org = new tblSprav_Org();

                org.ID_Ot       = model.ID_Ot       ;
                org.Name_Org    = model.Name_Org    ;
                org.INN         = model.INN         ;
                org.RS          = model.RS          ;
                org.MFO         = model.MFO         ;
                org.Bank        = model.Bank        ;
                org.OKONX       = model.OKONX       ;
                org.Adress      = model.Adress      ;
                org.Telefon     = model.Telefon     ;
                org.Region      = model.Region      ;
                org.DT_OST      = model.DT_OST      ;
                org.KT_OST      = model.KT_OST      ;
                org.E_Mail      = model.E_Mail      ;
                org.UserId = model.UserId;

                var orgIns = db.tblSprav_Org.Add(org);

                model.ID = orgIns.ID;

                db.SaveChanges();
            }
        }

        public static void Modify(Sprav_OrgViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var org = db.tblSprav_Org.Where(z => z.ID == model.ID);

                if (org.Any())
                {
                    var org1 = org.FirstOrDefault();

                    if (org1 != null)
                    {
                        org1.ID_Ot = model.ID_Ot;
                        org1.Name_Org = model.Name_Org;
                        org1.INN = model.INN;
                        org1.RS = model.RS;
                        org1.MFO = model.MFO;
                        org1.Bank = model.Bank;
                        org1.OKONX = model.OKONX;
                        org1.Adress = model.Adress;
                        org1.Telefon = model.Telefon;
                        org1.Region = model.Region;
                        org1.DT_OST = model.DT_OST;
                        org1.KT_OST = model.KT_OST;
                        org1.E_Mail = model.E_Mail;
                        org1.UserId = model.UserId;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(Sprav_OrgViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var org = db.tblSprav_Org.Where(z => z.ID == model.ID);

                if (org.Any())
                {
                    var org1 = org.FirstOrDefault();

                    if (org1 != null)
                    {
                        db.tblSprav_Org.Remove(org1);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<Sprav_OrgViewModel> Get(Sprav_OrgViewModel model)
        {
            List<Sprav_OrgViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var queryRes = from org in db.tblSprav_Org
                               select new Sprav_OrgViewModel
                               {
                                   ID_Ot    = org.ID_Ot,
                                   Name_Org = org.Name_Org,
                                   INN      = org.INN,
                                   RS       = org.RS,
                                   MFO      = org.MFO,
                                   Bank     = org.Bank,
                                   OKONX    = org.OKONX,
                                   Adress   = org.Adress,
                                   Telefon  = org.Telefon,
                                   Region   = org.Region,
                                   DT_OST   = org.DT_OST,
                                   KT_OST   = org.KT_OST,
                                   E_Mail   = org.E_Mail,
                                   UserId   = org.UserId
                            }  ;

                list = queryRes.ToList();

            }

            return list;
        }





    }
}