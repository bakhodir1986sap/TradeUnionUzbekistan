using KasabaKiat.BusinessLogic.Putyovki.OplataPutyovok;
using KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok;
using KasabaKiat.BusinessLogic.Putyovki.RealizasiyaPutyovok;
using KasabaKiat.BusinessLogic.VozvratPutyovki;
using KasabaKiat.ViewModel;
using KasabaKiat.ViewModel.Putyovki;
using KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki;
using KasabaKIAT.BusinessLogic.Putyovki.VozvratPutyovki;
using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Buxgalteriya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace KasabaKiat.BusinessLogic.Putyovki.VozvratPutyovki
{
    public class Vozvrat
    {
        public async Task EnterVozvrat(VozvratEnter model)
        {
            if (model == null)
            {
                throw new ApplicationException("model is null");
            }

            //Additional functional
            //Add to Prixod putyovok as original ID_Org
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {

                var prixodMain = new PostupleniyaViewModel
                {
                    Id_Kv = model.id_kv,
                    Data_Post = model.data_post,
                    Num_Doc = model.num_doc,
                    ID_Pos = model.id_ot,
                    Num_Kol = model.detailPutyovka.Count,
                    Symma = model.detailPutyovka.Sum(p => p.symma),
                    Name_Dov = model.dover,
                    Vid = false,
                    SmFil = model.smfil,
                    UserId = model.userid,
                    DepartmentBase = model.departmentbase
                };

                tblPost pp = null;

                PostMainDataManager.Create(ref prixodMain,db, out pp);

                //Add to Oplata as Vozvrat 
                var oplata = new OplataViewModel
                {
                    Data_Post = model.data_post,
                    Departmentbase = model.departmentbase,
                    ID_KV = model.id_kv,
                    ID_Otras = model.id_ot,
                    Num_Doc = model.num_doc,
                    PrizOpl = (int)OplataRasxPriz.VozvratPutyovki,
                    SmFil = model.smfil,
                    Symma_DT = 0,
                    Symma_KT = model.detailPutyovka.Sum(p => p.symma),
                    Tip_Org = true,
                    UserId = model.userid
                };

                tblOplata opp = null;
                OplataDataManager.Create(ref oplata,db, out opp);

                foreach (var mm in model.detailPutyovka)
                {
                    //Vozvrat dtl
                    var mmodel = new VozvratViewModel
                    {
                        data_post = model.data_post,
                        num_doc = model.num_doc,
                        id_sanat = mm.id_sanat,
                        num_put = mm.num_put,
                        id_ot = model.id_ot,
                        dover = model.dover,
                        symmap = mm.symma,
                        id_kv = model.id_kv,
                        smfil = model.smfil,
                        tip_voz = false,
                        departmentbase = model.departmentbase,
                        userid = model.userid
                    };

                    tblVozVrat vv = null;

                    VozvratDataManager.Create(ref mmodel, db, out vv);

                    vv.tblPost = pp;

                    //Prixod Dtl
                    var prixodDetail = new PostPutyovkaViewModel
                    {
                        IdPost = prixodMain.Id,
                        IdSanat = mm.id_sanat,
                        Num_Put = mm.num_put,
                        Symma_Put = mm.symma,
                        ID_PolVoz = false,
                        Data_Post = mm.data_post,
                        UserId = mm.userid,
                        Departmentbase = mm.departmentbase
                    };

                    PostPutDetailDataManager.Create(ref prixodDetail, db, pp);

                    var trans = new ProvodkiViewModel
                    {
                        comment = "Возврат в Облсовпроф",
                        department_base = mm.departmentbase,
                        inoutflg = false,
                        platelrsch = "03",
                        poluchrsch = "04",
                        summa = mm.symma,
                        user_id = mm.userid,
                        trdt = DateTime.Now,
                        trref = "VPP",
                        trrefno = string.Format("{0}|{1}", pp.Num_Doc, pp.Data_Post.Value.ToShortDateString())
                    };

                    tbbu_provodki provodka = null;
                    ProvodkiDataManager.Create(ref trans, db, out provodka);

                    vv.tbbu_provodki = provodka;
                    pp.tbbu_provodki = provodka;

                    //Oplata Dtl
                    var oplatDtl = new OplataRasxodiViewModel
                    {
                        Departmentbase = mm.departmentbase,
                        ID_Opl = oplata.ID,
                        ID_Pr = 1, //45%
                        ID_Sanat = mm.id_sanat.Value,
                        Num_Put = mm.num_put,
                        Priz = (int)OplataRasxPriz.VozvratPutyovki,
                        Symma = mm.symma.Value,
                        UserId = mm.userid.Value
                    };

                    OplataRasxodDataManager.Create(ref oplatDtl,db, opp);

                    //Remove or change status in Viplata
                    Vipl_PutDataManager.RemoveByPutyovka(mm.id_sanat.Value, mm.num_put, mm.departmentbase, db, vv);

                }

                await db.SaveChangesAsync();
            }
        }
    }
}