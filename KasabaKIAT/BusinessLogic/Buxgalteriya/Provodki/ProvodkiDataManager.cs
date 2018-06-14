using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Buxgalteriya;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.Provodki
{
    public class ProvodkiDataManager
    {
        public static void Create(ref ProvodkiViewModel model, KasabaKIATEntities db, out tbbu_provodki provod )
        {
                tbbu_provodki reg = new tbbu_provodki();

                reg.DEPARTMENT_BASE = model.department_base ;
                reg.USER_ID         = model.user_id         ;
                reg.INOUTFLG        = model.inoutflg        ;//0 - vnut 1 vnesh
                reg.TRDT            = model.trdt            ;
                reg.NUMBER          = model.number          ;
                reg.TIPSCHETA       = model.tipscheta       ;
                reg.TIPOBOROTA      = model.tipoborota      ;
                reg.PLATELNAME      = model.platelname      ;
                reg.PLATELINN       = model.platelinn       ;
                reg.PLATELMFO       = model.platelmfo       ;
                reg.POLUCHNAME      = model.poluchname      ;
                reg.POLUCHINN       = model.poluchinn       ;
                reg.POLUCHMFO       = model.poluchmfo       ;
                reg.PLATELRSCH      = model.platelrsch      ;
                reg.POLUCHRSCH      = model.poluchrsch      ;
                reg.SUMMA           = model.summa           ;
                reg.COMMENT         = model.comment         ;
                reg.TRREF = model.trref;
                reg.TRREFNO = model.trrefno;

                provod = db.tbbu_provodki.Add(reg);
        }

        public static void Modify(ProvodkiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbbu_provodki.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        reg.DEPARTMENT_BASE = model.department_base;
                        reg.USER_ID = model.user_id;
                        reg.INOUTFLG = model.inoutflg;
                        reg.TRDT = model.trdt;
                        reg.NUMBER = model.number;
                        reg.TIPSCHETA = model.tipscheta;
                        reg.TIPOBOROTA = model.tipoborota;
                        reg.PLATELNAME = model.platelname;
                        reg.PLATELINN = model.platelinn;
                        reg.PLATELMFO = model.platelmfo;
                        reg.POLUCHNAME = model.poluchname;
                        reg.POLUCHINN = model.poluchinn;
                        reg.POLUCHMFO = model.poluchmfo;
                        reg.PLATELRSCH = model.platelrsch;
                        reg.POLUCHRSCH = model.poluchrsch;
                        reg.SUMMA = model.summa;
                        reg.COMMENT = model.comment;
                    }
                }

                db.SaveChanges();
            }
        }

        public static void Remove(ProvodkiViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var planOblast = db.tbbu_provodki.Where(z => z.REC_ID == model.rec_id);

                if (planOblast.Any())
                {
                    var reg = planOblast.FirstOrDefault();

                    if (reg != null)
                    {
                        db.tbbu_provodki.Remove(reg);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<ProvodkiViewModel> Get(ProvodkiViewModel model)
        {
            List<ProvodkiViewModel> list = null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var query = from bank in db.tbbu_provodki
                            where bank.DEPARTMENT_BASE == model.department_base
                            select new ProvodkiViewModel
                            {
                                department_base = bank.DEPARTMENT_BASE,
                                user_id         = bank.USER_ID        , 
                                inoutflg        = bank.INOUTFLG       , 
                                trdt            = bank.TRDT           , 
                                number          = bank.NUMBER         , 
                                tipscheta       = bank.TIPSCHETA      , 
                                tipoborota      = bank.TIPOBOROTA     , 
                                platelname      = bank.PLATELNAME     , 
                                platelinn       = bank.PLATELINN      , 
                                platelmfo       = bank.PLATELMFO      , 
                                poluchname      = bank.POLUCHNAME     , 
                                poluchinn       = bank.POLUCHINN      , 
                                poluchmfo       = bank.POLUCHMFO      , 
                                platelrsch      = bank.PLATELRSCH     , 
                                poluchrsch      = bank.POLUCHRSCH     , 
                                summa           = bank.SUMMA          , 
                                comment         = bank.COMMENT         
                            };

                list = query.ToList();
            }

            return list;
        }
    }
}