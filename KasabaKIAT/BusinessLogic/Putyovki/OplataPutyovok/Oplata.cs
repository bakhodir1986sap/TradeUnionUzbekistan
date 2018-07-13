using KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok;
using KasabaKiat.BusinessLogic.Putyovki.RealizasiyaPutyovok;
using KasabaKiat.ViewModel;
using KasabaKiat.ViewModel.Putyovki;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using KasabaKIAT.ViewModel.Buxgalteriya;
using KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki;

namespace KasabaKiat.BusinessLogic.Putyovki.OplataPutyovok
{
    public enum OplataRasxPriz
    {
        KurortPrixod = 1,
        OrganizasiyaRasxod = 2,
        OrganizasiyaOplata = 3,
        KurortOplata = 4,
        VozvratPutyovki = 5,
        VozvratViplata = 6
    }

    public enum TipOpr
    {
        Opredelen,
        NeOpredelen
    }

    public enum PrizOpl
    {
        Vzaimorascheti,
        OplataPutyovok
    }


    public class Oplata
    {
        /*
         * 1) Viplata qilib nakladnoy berilgandan keyin 
         * Oplataga o'tilsa u yerda Viplata Summasi debet tomonda ko'rinadi
         * 
         * 2) Oplata modulida Raznoska qilinadi Qaysi tashkilot necha pul o'tkazganligi
         * ko'rsatiladi va qaysi putyovkalar uchun to'lamganligi ko'rsatiladi
         * 
         * 3) Xar keyingi raznoskada qoldiq keyingi safarga summaga qo'shiladi
         * 
         * 4) Oplata kreditga o'tiradi
         * 
         * 5) Tanlangan putyovkalar Oplata_Rasxodiga o'tiradi
         */

         /// <summary>
         /// 
         /// </summary>
        public Oplata()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="main1"></param>
        /// <param name="putList"></param>
        public void ViplataProizoshla(RealizasiyaViplMainViewModel main1, 
            List<RealizasiyaViplPutViewModel> putList)
        {
            //Soditsya po debetu
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void AddOplataCredit(OplataViewModel model)
        {

        }


        public void AddOplataRasxod(OplataRasxodiViewModel putModel)
        {

        }

        public List<RealizasiyaViplPutViewModel> GetViplataPutList()
        {
            return new List<RealizasiyaViplPutViewModel>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdMain"></param>
        public async Task ClosePrixodPutyovok(long IdMain)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var post = db.tblPost.Where(z => z.ID == IdMain);

                if (post.Any())
                {
                    var post1 = post.FirstOrDefault();

                    var oplata = new OplataViewModel();

                    oplata.Data_Post = post1.Data_Post;
                    oplata.Departmentbase = post1.DepartmentBase;
                    oplata.ID_KV = post1.ID_KV;
                    oplata.ID_Otras = post1.ID_Pos;
                    oplata.Num_Doc = post1.Num_Doc;
                    oplata.PrizOpl = (int)OplataRasxPriz.KurortPrixod;
                    oplata.SmFil = post1.SmFil;
                    oplata.Symma_DT = 0;
                    oplata.Symma_KT = post1.Symma;
                    oplata.Tip_Org = true;
                    oplata.UserId = post1.UserId;

                    tblOplata opp = null;

                    OplataDataManager.Create(ref oplata, db, out opp);

                    var prixodDtl = await PostPutDetailDataManager.GetFiltered(new PostPutyovkaViewModel()
                    {
                        Departmentbase = post1.DepartmentBase,
                        IdPost = IdMain
                    });

                    if (prixodDtl != null)
                    {
                        foreach(var pDtl in prixodDtl)
                        {
                            var oplatDtl = new OplataRasxodiViewModel
                            {
                                Departmentbase = pDtl.Departmentbase,
                                ID_Opl = oplata.ID,
                                ID_Pr = 1, //45%
                                ID_Sanat = pDtl.IdSanat.Value,
                                Num_Put = pDtl.Num_Put,
                                Priz = (int)OplataRasxPriz.KurortPrixod,
                                Symma = pDtl.Symma_Put.Value,
                                UserId = pDtl.UserId.Value
                            };

                            OplataRasxodDataManager.Create(ref oplatDtl, db, opp);
                        }
                    }

                    var trans = new ProvodkiViewModel
                    {
                        comment = "Приход путевок ",
                        department_base = post1.DepartmentBase,
                        inoutflg = false,
                        platelrsch = "03",
                        poluchrsch = "07",
                        summa = post1.Symma,
                        user_id = post1.UserId,
                        trdt = post1.Data_Post,
                        trref = "PPR",
                        trrefno = string.Format("{0}|{1}", post1.Num_Doc, post1.Data_Post.Value.ToShortDateString())
                    };

                    tbbu_provodki provodka = null;
                    ProvodkiDataManager.Create(ref trans, db, out provodka);

                    post1.tbbu_provodki = provodka;
                }

                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdMain"></param>
        public async Task CloseRasxodPutyovok(long IdMain)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var post = db.tblVipl.Where(z => z.ID == IdMain);

                if (post.Any())
                {
                    var post1 = post.FirstOrDefault();

                    var oplata = new OplataViewModel
                    {
                        Data_Post = post1.Data_Post,
                        Departmentbase = post1.DepartmentBase,
                        ID_KV = post1.ID_Kv,
                        ID_Otras = post1.ID_Org,
                        Num_Doc = post1.Num_Doc,
                        PrizOpl = (int)OplataRasxPriz.OrganizasiyaRasxod,
                        SmFil = post1.SmFil,
                        Symma_DT = post1.Symma,
                        Symma_KT = 0,
                        Tip_Org = true,
                        UserId = post1.UserId
                    };

                    tblOplata opp = null;

                    OplataDataManager.Create(ref oplata, db, out opp);

                    var prixodDtl = await Vipl_PutDataManager.Get(IdMain);

                    if (prixodDtl != null)
                    {
                        foreach (var pDtl in prixodDtl)
                        {
                            var oplatDtl = new OplataRasxodiViewModel
                            {
                                Departmentbase = pDtl.departmentbase,
                                ID_Opl = oplata.ID,
                                ID_Pr = 1, //45%
                                ID_Sanat = pDtl.id_sanat.Value,
                                Num_Put = pDtl.num_put,
                                Priz = (int)OplataRasxPriz.OrganizasiyaRasxod,
                                Symma = pDtl.symma.Value,
                                UserId = pDtl.userid.Value
                            };

                            OplataRasxodDataManager.Create(ref oplatDtl,db, opp);
                        }
                    }

                    //If Organization IsPravsoyuz Flag and Not PPO Rasxod should go as Prixod

                    var orgInfo = db.tblSprav_Org.Where(z => z.IS_PRAVSOYUZ == true 
                                                            && z.PRAVORGLEVEL < 3
                                                            && z.ID == post1.ID_Org);
                    
                    if (orgInfo.Any())
                    {
                        var departmentCode = orgInfo.First().OWNER_CODE;

                        if (!string.IsNullOrEmpty(departmentCode))
                        {
                            var prixodMain = new PostupleniyaViewModel
                            {
                                Id_Kv = post1.ID_Kv,
                                Data_Post = DateTime.Now,
                                Num_Doc = post1.Num_Doc,
                                ID_Pos = post1.ID_Org,
                                Num_Kol =null,
                                Symma = null,
                                Name_Dov = post1.Name_Dov,
                                Vid = false,
                                SmFil = post1.SmFil,
                                UserId = post1.UserId,
                                DepartmentBase = departmentCode
                            };

                            tblPost pp = null;

                            PostMainDataManager.Create(ref prixodMain,db, out pp);

                            var prixodDtl2 = await Vipl_PutDataManager.Get(IdMain);

                            if (prixodDtl2 != null)
                            {
                                foreach (var pDtl in prixodDtl2)
                                {
                                    //Prixod Dtl
                                    var prixodDetail1 = new PostPutyovkaViewModel
                                    {
                                        IdPost = prixodMain.Id,
                                        IdSanat = pDtl.id_sanat,
                                        Num_Put = pDtl.num_put,
                                        Symma_Put = pDtl.symma,
                                        ID_PolVoz = false,
                                        Data_Post = pDtl.data_post,
                                        UserId = pDtl.userid,
                                        Departmentbase = departmentCode
                                    };

                                    PostPutDetailDataManager.Create(ref prixodDetail1,db, pp);
                                }
                            }


                        }
                        else
                        {
                            throw new ApplicationException("Owner code in organization list is invalid");
                        }
                    }

                    var trans = new ProvodkiViewModel
                    {
                        comment = "Расход путевок ",
                        department_base = post1.DepartmentBase,
                        inoutflg = false,
                        platelrsch = "04",
                        poluchrsch = "03",
                        summa = post1.Symma,
                        user_id = post1.UserId,
                        trdt = DateTime.Now,
                        trref = "RSP",
                        trrefno = string.Format("{0}|{1}", post1.Num_Doc, post1.Data_Post.Value.ToShortDateString())
                    };

                    tbbu_provodki provodka = null;
                    ProvodkiDataManager.Create(ref trans, db, out provodka);

                    post1.tbbu_provodki = provodka;

                }

                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdOrg"></param>
        /// <returns></returns>
        public async Task<List<OplataRasxodiViewModel>> GetRaznoskaList(long IdOrg)
        {
            var list = new List<OplataRasxodiViewModel>(); 

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = await (from oplMst in db.tblOplatas
                            join oplDtl in db.tblOplata_Ras on oplMst.ID equals oplDtl.ID_Opl
                            join sanat  in db.tblSprav_San  on oplDtl.ID_Sanat equals sanat.ID
                            join proc   in db.tblSprav_Pr   on oplDtl.ID_Pr equals proc.ID
                            where (oplMst.ID_Otras == IdOrg && oplDtl.Priz == (int)OplataRasxPriz.OrganizasiyaRasxod &&
                            oplMst.PrizOpl == (int)OplataRasxPriz.OrganizasiyaRasxod)
                            orderby oplMst.Data_Post
                            select new
                            {
                                IdDtl       = oplDtl.ID,
                                IdSanat     = sanat.ID,
                                SanatName   = sanat.Name_San,
                                Num_Put     = oplDtl.Num_Put,
                                ProcName    = proc.Name_Proc,
                                Price       = oplDtl.Symma,
                                Priznak     = oplDtl.Priz
                            }).ToArrayAsync();

                foreach(var putyovka in query)
                {
                    var find = await (db.tblOplata_Ras.Where(z => z.Priz == (int)OplataRasxPriz.OrganizasiyaOplata
                                                        && z.ID_Sanat == putyovka.IdSanat
                                                        && z.Num_Put == putyovka.Num_Put
                                                        && z.Symma == putyovka.Price)).ToArrayAsync();

                    if (find.Any())
                    {
                        var oplataCount = find.Count();

                        var find2 = await  (db.tblOplata_Ras.Where(z => z.Priz == (int)OplataRasxPriz.VozvratPutyovki
                                    && z.ID_Sanat == putyovka.IdSanat
                                    && z.Num_Put == putyovka.Num_Put
                                    && z.Symma == putyovka.Price)).ToArrayAsync();

                        var vozvratCount = find2.Count();

                        if (oplataCount > vozvratCount)
                        {
                            //Putyovka fully covred
                            continue;
                        }
                        else
                        {
                            //Not Covered putyovka
                            var obj = new OplataRasxodiViewModel
                            {
                                ID = putyovka.IdDtl,
                                ID_Sanat = putyovka.IdSanat,
                                SanatName = putyovka.SanatName,
                                Num_Put = putyovka.Num_Put,
                                Name_Proc = putyovka.ProcName,
                                Symma = putyovka.Price.Value,
                                Priz = putyovka.Priznak
                            };

                            list.Add(obj);
                        }
                    }
                    else
                    {
                        var obj = new OplataRasxodiViewModel
                        {
                            ID = putyovka.IdDtl,
                            ID_Sanat = putyovka.IdSanat,
                            SanatName = putyovka.SanatName,
                            Num_Put = putyovka.Num_Put,
                            Name_Proc = putyovka.ProcName,
                            Symma = putyovka.Price.Value,
                            Priz = putyovka.Priznak
                        };

                        list.Add(obj);
                    }
                }

            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdOrg"></param>
        /// <returns></returns>
        public async Task<decimal> GetGeneralRemainingByOrg(long IdOrg)
        {
            decimal totalRemain = 0;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = await (from oplMst in db.tblOplatas
                            join oplDtl in db.tblOplata_Ras on oplMst.ID equals oplDtl.ID_Opl
                            join sanat in db.tblSprav_San on oplDtl.ID_Sanat equals sanat.ID
                            join proc in db.tblSprav_Pr on oplDtl.ID_Pr equals proc.ID
                            where (oplMst.ID_Otras == IdOrg && oplDtl.Priz == (int)OplataRasxPriz.OrganizasiyaOplata &&
                            oplMst.PrizOpl == (int)OplataRasxPriz.OrganizasiyaOplata)
                            orderby oplMst.Data_Post
                            select new
                            {
                                IdDtl = oplDtl.ID,
                                IdSanat = sanat.ID,
                                SanatName = sanat.Name_San,
                                Num_Put = oplDtl.Num_Put,
                                ProcName = proc.Name_Proc,
                                Price = oplDtl.Symma,
                                Priznak = oplDtl.Priz
                            }).ToArrayAsync();

                var detailTotal = query.Sum(p => p.Price);

                if (!detailTotal.HasValue)
                {
                    detailTotal = 0;
                }

                var query1 = await (from oplMst in db.tblOplatas
                            where (oplMst.ID_Otras == IdOrg && 
                            oplMst.PrizOpl == (int)OplataRasxPriz.OrganizasiyaRasxod)
                            orderby oplMst.Data_Post
                            select new
                            {
                                Price = oplMst.Symma_DT
                            }).ToArrayAsync();

                var masterTotal = query1.Sum(p => p.Price);

                if (!masterTotal.HasValue)
                {
                    masterTotal = 0;
                }

                totalRemain = masterTotal.Value - detailTotal.Value;
            }

            return totalRemain;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdOrg"></param>
        /// <returns></returns>
        public async Task<decimal> GetTotalDebtByOrg(long IdOrg)
        {
            decimal totalRemain = 0;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = await (from oplMst in db.tblOplatas
                            where (oplMst.ID_Otras == IdOrg &&
                            oplMst.PrizOpl == (int)OplataRasxPriz.OrganizasiyaRasxod)
                            orderby oplMst.Data_Post
                            select new
                            {
                                Price = oplMst.Symma_DT
                            }).ToArrayAsync();

                var DebitAmount = query.Sum(p => p.Price);

                if (!DebitAmount.HasValue)
                {
                    DebitAmount = 0;
                }

                var query1 = await (from oplMst in db.tblOplatas
                             where (oplMst.ID_Otras == IdOrg &&
                             oplMst.PrizOpl == (int)OplataRasxPriz.OrganizasiyaOplata)
                             orderby oplMst.Data_Post
                             select new
                             {
                                 Price = oplMst.Symma_KT
                             }).ToArrayAsync();

                var KreditAmount = query1.Sum(p => p.Price);

                if (!KreditAmount.HasValue)
                {
                    KreditAmount = 0;
                }

                totalRemain = KreditAmount.Value - DebitAmount.Value;
            }

            return totalRemain;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdOpl"></param>
        /// <returns></returns>
        public async Task<decimal> GetOplataSaldo(long IdOpl)
        {
            decimal totalRemain = 0;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = await (from oplMst in db.tblOplatas
                            where (oplMst.ID == IdOpl &&
                            oplMst.PrizOpl == (int)OplataRasxPriz.OrganizasiyaOplata)
                            select new
                            {
                                Price = oplMst.Symma_KT
                            }).ToArrayAsync();

                var OplataAmount = query.Sum(p => p.Price);

                if (!OplataAmount.HasValue)
                {
                    OplataAmount = 0;
                }

                var PutyovkaOplAmt = db.tblOplata_Ras.Where(z => z.ID_Opl == IdOpl).Sum(z => z.Symma);

                if (!PutyovkaOplAmt.HasValue)
                {
                    PutyovkaOplAmt = 0;
                }

                totalRemain = OplataAmount.Value - PutyovkaOplAmt.Value;
            }

            return totalRemain;
        }

    }
}