using KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok;
using KasabaKIAT.BusinessLogic.Putyovki.RealizasiyaPutyovok.RealizasitaPoTz;
using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Putyovki;
using System;
using System.Linq;

namespace KasabaKIAT.BusinessLogic.Putyovki.VozvratPutyovki.VozvratPoTz
{
    public class VozvratPutyovki
    {
        public void VozvratPutyovkiProcess(long realizRecId, string remark)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var realiz = db.tbpt_putyovkarealizasiya.Where(z => z.REC_ID == realizRecId);

                if (realiz.Any())
                {
                    var real = realiz.First();

                    var otkaz = new OtkazPutyovkiViewModel();

                    otkaz.department_code = real.DEPARTMENT_CODE;
                    otkaz.otkaz_date = DateTime.Now;
                    otkaz.realization_id = real.REC_ID;
                    otkaz.remark = remark;
                    otkaz.user_id = real.USER_ID;

                    VozvratPoTzDataManager.Create(ref otkaz);

                    RealizasiyaDataManager.VozvratReal(new RealizasiyaByTzViewModel() { rec_id = real.REC_ID });

                    PostPutDetailDataManager.OtmenPolzov((int)real.SANATORIUM_ID.Value, real.SERAIL_NUMBER);
                }
                else
                {
                    throw new ApplicationException("Invalid REC_ID");
                }
            }
        }
    }
}