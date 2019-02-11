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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
                #region PostupleniyeNakladnaya
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

                #endregion

                #region OplataMain
                //Add to Oplata as Vozvrat 
                var oplata = new OplataViewModel
                {
                    Data_Post = model.data_post,
                    Departmentbase = model.departmentbase,
                    ID_KV = model.id_kv,
                    ID_Otras = model.id_ot,
                    Num_Doc = model.num_doc,
                    PrizOpl = (int)OplataRasxPriz.OrganizasiyaOplata,
                    SmFil = model.smfil,
                    Symma_DT = model.detailPutyovka.Sum(p => p.symma) * (-1),
                    Symma_KT = 0,
                    Tip_Org = true,
                    UserId = model.userid
                };

                tblOplata opp = null;
                OplataDataManager.Create(ref oplata,db, out opp);

                #endregion

                #region SatelliteOrg
                string owner_Code = string.Empty;
                var orgs = db.tblSprav_Org.Where(z => z.ID == model.id_ot && z.DepartmentBase == model.departmentbase);

                if (orgs.Any())
                {

                    owner_Code = orgs.First().OWNER_CODE;
                }
                #endregion

                foreach (var mm in model.detailPutyovka)
                {
                    //Vozvrat dtl
                    #region Vozvrat dtl
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

                    #endregion

                    //Prixod Dtl
                    #region Prixod Dtl
                    var prixodDetail = new PostPutyovkaViewModel
                    {
                        IdPost = prixodMain.Id,
                        IdSanat = mm.id_sanat,
                        Num_Put = mm.num_put,
                        Symma_Put = mm.symma,
                        ID_PolVoz = false,
                        Data_Post = mm.data_post,
                        DataSan = mm.data_post,
                        UserId = mm.userid,
                        Departmentbase = mm.departmentbase
                    };

                    PostPutDetailDataManager.Create(ref prixodDetail, db, pp);

                    #endregion

                    #region PostPut Satteleit Org Putyovka Remove
                    var PostPuts = from PosPut in db.tblPos_Put
                                   join PosMain in db.tblPost on PosPut.ID_Post equals PosMain.ID
                                   where PosMain.DepartmentBase == owner_Code && PosPut.Num_Put == mm.num_put
                                   && PosPut.ID_Sanat == mm.id_sanat
                                   && PosPut.ID_PolVoz == false
                                   select PosPut;

                    if (PostPuts.Any())
                    {
                        foreach(var put in PostPuts)
                        {
                            db.tblPos_Put.Remove(put);
                        }
                    }

                    #endregion

                    #region Provodki
                    var trans = new ProvodkiViewModel
                    {
                        comment = "Возврат в Облсовпроф",
                        department_base = mm.departmentbase,
                        inoutflg = false,
                        platelrsch = "03",
                        poluchrsch = "04",
                        summa = mm.symma,
                        user_id = mm.userid,
                        trdt = model.data_post,
                        trref = "VPP",
                        trrefno = string.Format("{0}|{1}", pp.Num_Doc, pp.Data_Post.Value.ToShortDateString())
                    };

                    tbbu_provodki provodka = null;
                    ProvodkiDataManager.Create(ref trans, db, out provodka);

                    vv.tbbu_provodki = provodka;
                    pp.tbbu_provodki = provodka;

                    #endregion

                    //Oplata Dtl
                    #region Oplata Dtl
                    var oplatDtl = new OplataRasxodiViewModel
                    {
                        Departmentbase = mm.departmentbase,
                        ID_Opl = oplata.ID,
                        ID_Pr = 1, //45%
                        ID_Sanat = mm.id_sanat.Value,
                        Num_Put = mm.num_put,
                        Priz = (int)OplataRasxPriz.VozvratPutyovki,
                        Symma = mm.symma.Value * (-1),
                        UserId = mm.userid.Value
                    };

                    OplataRasxodDataManager.Create(ref oplatDtl,db, opp);

                    #endregion

                    //Remove or change status in Viplata
                    #region Remove or change status in Viplata
                    Vipl_PutDataManager.RemoveByPutyovka(mm.id_sanat.Value, mm.num_put, mm.departmentbase, model.id_ot, db, vv);
                    #endregion

                }

                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CancelVozvrat(VozvratViewModel model)
        {
            if (model == null)
            {
                throw new ApplicationException("model is null");
            }

            //Additional functional
            //Add to Prixod putyovok as original ID_Org
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                //get vozvrat links
                var vozvrat = db.tblVozVrats.Where(z => z.ID == model.id 
                && z.DepartmentBase == model.departmentbase).FirstOrDefault();

                if (vozvrat == null) return;

                //Delete Prixod
                var postPuts = db.tblPos_Put.Where(z => z.ID_Post == vozvrat.PRIXOD_ID
                && z.Num_Put == vozvrat.Num_Put && z.ID_Sanat == vozvrat.ID_Sanat);

                if (postPuts.Any())
                {
                    var reg = postPuts.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblPos_Put.Remove(reg);
                    }
                }

                //Reset Rasxod
                var rasxodPut = new tblVipl_Put
                {
                    ID_Vipl = vozvrat.RASXOD_ID,
                    Num_Put = vozvrat.Num_Put,
                    ID_Sanat = vozvrat.ID_Sanat,
                    Data_Post = vozvrat.Data_Post,
                    Symma = vozvrat.SymmaP,
                    Num_Proc = 100,
                    DepartmentBase = model.departmentbase,
                    TIMESTAMP_T = DateTime.Now,
                    UserId = model.userid
                };

                db.tblVipl_Put.Add(rasxodPut);

                //insert into oplata

                var prixod_id = vozvrat.PRIXOD_ID;
                //delete from vozvrat
                var sanInfo = db.tblVozVrats.Where(z => z.ID == model.id);

                if (sanInfo.Any())
                {
                    var reg = sanInfo.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tblVozVrats.Remove(reg);
                    }
                }


                var countPostPut = db.tblPos_Put.Where(z => z.ID_Post == prixod_id).Count();

                if (countPostPut == 1)
                {
                    var posts = db.tblPost.Where(z => z.ID == prixod_id);

                    if (posts.Any())
                    {
                        var reg = posts.FirstOrDefault();

                        if (reg != null)
                        {
                            db.tblPost.Remove(reg);
                        }
                    }
                }

                try
                {
                    await db.SaveChangesAsync();
                }
                catch(Exception ex)
                {

                }
            }
        }
    }
}