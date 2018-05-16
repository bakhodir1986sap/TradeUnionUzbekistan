using KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok;
using KasabaKiat.BusinessLogic.Putyovki.RealizasiyaPutyovok;
using KasabaKiat.ViewModel.Putyovki;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public void ClosePrixodPutyovok(long IdMain)
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
                    oplata.PrizOpl = (int)PrizOpl.Vzaimorascheti;
                    oplata.SmFil = post1.SmFil;
                    oplata.Symma_DT = 0;
                    oplata.Symma_KT = post1.Symma;
                    oplata.Tip_Org = (int)TipOpr.Opredelen == 0 ? false : true;
                    oplata.UserId = post1.UserId;

                    OplataDataManager.Create(ref oplata);

                    var prixodDtl = PostPutDetailDataManager.GetFiltered(new PostPutyovkaViewModel()
                    {
                        Departmentbase = post1.DepartmentBase,
                        IdPost = IdMain
                    });

                    if (prixodDtl != null)
                    {
                        foreach(var pDtl in prixodDtl)
                        {
                            var oplatDtl = new OplataRasxodiViewModel();
                            oplatDtl.Departmentbase = pDtl.Departmentbase;
                            oplatDtl.ID_Opl = oplata.ID;
                            oplatDtl.ID_Pr = 1; //45%
                            oplatDtl.ID_Sanat = pDtl.IdSanat.Value;
                            oplatDtl.Num_Put = pDtl.Num_Put;
                            oplatDtl.Priz = (int)OplataRasxPriz.KurortPrixod;
                            oplatDtl.Symma = pDtl.Symma_Put.Value;
                            oplatDtl.UserId = pDtl.UserId.Value;

                            OplataRasxodDataManager.Create(ref oplatDtl);
                        }
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdMain"></param>
        public void CloseRasxodPutyovok(long IdMain)
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
                        PrizOpl = (int)PrizOpl.OplataPutyovok,
                        SmFil = post1.SmFil,
                        Symma_DT = post1.Symma,
                        Symma_KT = 0,
                        Tip_Org = (int)TipOpr.Opredelen == 0 ? false : true,
                        UserId = post1.UserId
                    };

                    OplataDataManager.Create(ref oplata);

                    var prixodDtl = Vipl_PutDataManager.Get(IdMain);

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

                            OplataRasxodDataManager.Create(ref oplatDtl);
                        }
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdOrg"></param>
        /// <returns></returns>
        public List<OplataRasxodiViewModel> GetRaznoskaList(long IdOrg)
        {
            var list = new List<OplataRasxodiViewModel>(); 

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from oplMst in db.tblOplatas
                            join oplDtl in db.tblOplata_Ras on oplMst.ID equals oplDtl.ID_Opl
                            join sanat  in db.tblSprav_San  on oplDtl.ID_Sanat equals sanat.ID
                            join proc   in db.tblSprav_Pr   on oplDtl.ID_Pr equals proc.ID
                            where (oplMst.ID_Otras == IdOrg && oplDtl.Priz == (int)OplataRasxPriz.OrganizasiyaRasxod &&
                            oplMst.PrizOpl == (int)PrizOpl.OplataPutyovok)
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
                            };

                foreach(var putyovka in query)
                {
                    var find = db.tblOplata_Ras.Where(z => z.Priz == (int)OplataRasxPriz.OrganizasiyaOplata
                                                        && z.ID_Sanat == putyovka.IdSanat
                                                        && z.Num_Put == putyovka.Num_Put
                                                        && z.Symma == putyovka.Price);

                    if (find.Any())
                    {
                        var oplataCount = find.Count();

                        var find2 = db.tblOplata_Ras.Where(z => z.Priz == (int)OplataRasxPriz.VozvratPutyovki
                                    && z.ID_Sanat == putyovka.IdSanat
                                    && z.Num_Put == putyovka.Num_Put
                                    && z.Symma == putyovka.Price);

                        var vozvratCount = find.Count();

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
        public decimal GetGeneralRemainingByOrg(long IdOrg)
        {
            decimal totalRemain = 0;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from oplMst in db.tblOplatas
                            join oplDtl in db.tblOplata_Ras on oplMst.ID equals oplDtl.ID_Opl
                            join sanat in db.tblSprav_San on oplDtl.ID_Sanat equals sanat.ID
                            join proc in db.tblSprav_Pr on oplDtl.ID_Pr equals proc.ID
                            where (oplMst.ID_Otras == IdOrg && oplDtl.Priz == (int)OplataRasxPriz.OrganizasiyaOplata &&
                            oplMst.PrizOpl == (int)PrizOpl.OplataPutyovok)
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
                            };

                var detailTotal = query.Sum(p => p.Price);

                if (!detailTotal.HasValue)
                {
                    detailTotal = 0;
                }

                var query1 = from oplMst in db.tblOplatas
                            where (oplMst.ID_Otras == IdOrg && 
                            oplMst.PrizOpl == (int)PrizOpl.OplataPutyovok)
                            orderby oplMst.Data_Post
                            select new
                            {
                                Price = oplMst.Symma_KT
                            };

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
        public decimal GetTotalDebtByOrg(long IdOrg)
        {
            decimal totalRemain = 0;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from oplMst in db.tblOplatas
                            where (oplMst.ID_Otras == IdOrg &&
                            oplMst.PrizOpl == (int)PrizOpl.OplataPutyovok)
                            orderby oplMst.Data_Post
                            select new
                            {
                                Price = oplMst.Symma_DT
                            };

                var DebitAmount = query.Sum(p => p.Price);

                if (!DebitAmount.HasValue)
                {
                    DebitAmount = 0;
                }

                var query1 = from oplMst in db.tblOplatas
                             where (oplMst.ID_Otras == IdOrg &&
                             oplMst.PrizOpl == (int)PrizOpl.OplataPutyovok)
                             orderby oplMst.Data_Post
                             select new
                             {
                                 Price = oplMst.Symma_KT
                             };

                var KreditAmount = query1.Sum(p => p.Price);

                if (!KreditAmount.HasValue)
                {
                    KreditAmount = 0;
                }

                totalRemain = KreditAmount.Value - DebitAmount.Value;
            }

            return totalRemain;
        }

    }
}