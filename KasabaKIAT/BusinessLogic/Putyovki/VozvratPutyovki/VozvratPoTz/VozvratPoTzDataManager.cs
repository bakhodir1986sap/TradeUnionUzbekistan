using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.VozvratPutyovki.VozvratPoTz
{
    public class VozvratPoTzDataManager
    {
        public static void Create(ref OtkazPutyovkiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbpt_otkazputyovka vozv = new tbpt_otkazputyovka();

                //vozv.REC_ID            = model.rec_id           ;
                vozv.USER_ID           = model.user_id          ;
                vozv.DEPARTMENT_CODE   = model.department_code  ;
                vozv.OTKAZ_DATE        = model.otkaz_date       ;
                vozv.REALIZATION_ID    = model.realization_id   ;
                vozv.REMARK = model.remark;

                var vozz = db.tbpt_otkazputyovka.Add(vozv);

                db.SaveChanges();

                model.rec_id = vozz.REC_ID;
            }
        }

        //public static void Modify(OtkazPutyovkiViewModel model)
        //{
        //    using (KasabaKIATEntities db = new KasabaKIATEntities())
        //    {
        //        var vozv = db.tblVozVrat.Where(z => z.ID == model.id);

        //        if (vozv.Any())
        //        {
        //            var vozv1 = vozv.FirstOrDefault();

        //            if (vozv1 != null)
        //            {
        //                vozv1.Data_Post = model.data_post;
        //                vozv1.Num_Doc = model.num_doc;
        //                vozv1.ID_Sanat = model.id_sanat;
        //                vozv1.Num_Put = model.num_put;
        //                vozv1.ID_Ot = model.id_ot;
        //                vozv1.Dover = model.dover;
        //                vozv1.ID_Pol = model.id_pol;
        //                vozv1.SymmaP = model.symmap;
        //                vozv1.ID_KV = model.id_kv;
        //                vozv1.SmFil = model.smfil;
        //                vozv1.Tip_Voz = model.tip_voz;
        //                vozv1.UserId = model.userid;
        //                vozv1.DepartmentBase = model.departmentbase;
        //            }
        //        }

        //        db.SaveChanges();
        //    }
        //}

        public static void Remove(OtkazPutyovkiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var sanInfo = db.tbpt_otkazputyovka.Where(z => z.REC_ID == model.rec_id);

                if (sanInfo.Any())
                {
                    var reg = sanInfo.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tbpt_otkazputyovka.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

    }
}