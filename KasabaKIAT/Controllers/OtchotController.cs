using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KasabaKIAT.Models;
using KurortUrvServiceTest.ServiceInterAction.ActionLogic;
using log4net;

namespace KasabaKIAT.Controllers
{
    //[Authorize(Roles = "COUNCIL_REGIONAL_REGION,ADMIN")]
    public class OtchotController : Controller
    {
        ServiceActionController service = new ServiceActionController();
        ILog log = log4net.LogManager.GetLogger(typeof(AdministirirovaniyeController));
        KasabaKIATEntities KasabaDB = new KasabaKIATEntities();
        // GET: Otchot
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SvodniyOtchot()
        {
            return View();
        }
        public ActionResult Talabnomlar()
        {
            return View();
        }
        public ActionResult Forma13()
        {
            return View();
        }
        public JsonResult GetForma13(int yil, string inn)
        {
            log.Info("GetRaspredeleniyeZaezdov1");
            log.Info("yil = " + yil);
            log.Info("inn = " + inn);
            try
            {
                var data = KasabaDB.sp_rt_FormaN13(yil, inn);
                var O = from o in data
                        select new
                        {
                            d1 = o.Data_Postupleniya,
                            d2 = o.Nomer_SchetFacturi,
                            d3 = o.NameimenovaniyeOrg,
                            d4 = o.NomerPutyovki,
                            d5 = o.DataZaezda,
                            d6 = o.SenaPutyovki,
                            d7 = o.DataPokupkiPutyovki,
                            d8 = o.NomerResheniyaOVidache,
                            d9 = o.FIO,
                            d10 = o.MestoRaboti,
                            d11 = o.DataPlateja,
                            d12 = o.SymmaPlateja,
                            d13 = o.NomerPlatejnogoPorucheniya,
                            d14 = o.NomerTalona,
                            d15 = o.DataTalona
                        };

                log.Info("Count = " + O.ToList().Count);

                return Json(O.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }
            return null;
        }
        public ActionResult Forma14()
        {
            return View();
        }
        public ActionResult TablitsaOtdixayushix()
        {
            return View();
        }
        public ActionResult JurnalniyOrder()
        {
            return View();
        }
        public ActionResult FinalniyOtchot()
        {
            return View();
        }
        public ActionResult GlavnayaKniga()
        {
            return View();
        }
        public JsonResult GetSvodniyOtchot(DateTime? Begindt,DateTime? Enddt,string Soato,string Branch)
        {
            var data = KasabaDB.sp_rt_SvodniyOtchet(Begindt, Enddt, Soato, Branch);
            var list = from d in data
                       select new
                       {
                           PN = d.ProfName,
                           FN = d.FullName,
                           PP=d.PlaceAndPos,
                           MSdt=d.MedSprevDate,
                           Zdt=d.ZayavkaDate,
                           SN=d.SanName,
                           ZB=d.zaezdBegin
                       };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTalabnomlar_Korxona(int yil,string inn)
        {
            var data = KasabaDB.sp_rt_TalabnomaKorxona(yil,inn);
            var list = from d in data
                       select new
                       {
                           SN=d.sanName,
                           KV1=d.KV1,
                           KV2=d.KV2,
                           KV3=d.KV3,
                           KV4=d.KV4
                       };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTalabnomlar_Tarmoq(int yil, string soato,string branch)
        {
            var data1 = KasabaDB.sp_rt_TalabnomaTarmoq(yil, soato, branch);
            var list = from d in data1
                       select new
                       {
                           SN = d.sanName,
                           KV1 = d.KV1,
                           KV2 = d.KV2,
                           KV3 = d.KV3,
                           KV4 = d.KV4
                       };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTalabnomlar_Viloyat(int yil, string soato)
        {
            var data3 = KasabaDB.sp_rt_TalabnomaViloyat(yil, soato);
            var list = from d in data3
                       select new
                       {
                           SN = d.sanName,
                           KV1 = d.KV1,
                           KV2 = d.KV2,
                           KV3 = d.KV3,
                           KV4 = d.KV4
                       };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Postupleniya()
        {
            return View();
        }
        public JsonResult GetPostupleniya(string inn)
        {
            log.Info("GetPostupleniya");
            log.Info("inn = " + inn);
            try
            {
                var data = KasabaDB.sp_rt_Postupleniya(inn);
                var O = from o in data
                        select new
                        {
                            d1 = o.K_TYPE,
                            d2 = o.OPERDAY,
                            d3 = o.ID,
                            d4 = o.BANKID,
                            d5 = o.KLSDOX,
                            d6 = o.DOCNUMB,
                            d7 = o.DOCDATE,
                            d8 = o.DOCTYPE,
                            d9 = o.SUMPAY,
                            d10 =o.CLMFO,
                            d11 =o.CLACC,
                            d12 =o.CLINN,
                            d13 =o.CLNAME,
                            d14 =o.COMFO,
                            d15 =o.COACC,
                            d16=o.COINN,
                            d17=o.CONAME,
                            d18=o.PURPOSE,
                            d19=o.CHAPTER,
                            d20=o.OKONH,
                            d21=o.FILE_ID,//
                            d22=o.OTCHISLEN,//
                            d23=o.FileName,//
                            d24=o.CreateDate// 
                        };

                //log.Info("Count = " + O.ToList().Count);

                return Json(O.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }
            return null;
        }
        public ActionResult RazbivkaPostupleniya()
        {
            return View();
        }
        public JsonResult GetRazbivkaPostupleniya(string s,string b,DateTime? bg,DateTime? end)
        {
            log.Info("GetPostupleniya");
            log.Info("SOATO = " + s);
            log.Info("BRANCH = " + b);
            log.Info("BEGIN_DATE = " + bg);
            log.Info("END_DATE = " + end);
            try
            {
                var data = KasabaDB.sp_rt_Razbivka_Postup(s, b, bg.Value.Date, end.Value.Date);
                var O = from o in data
                        select new
                        {
                            d1 =  o.K_TYPE           ,
                            d2 =  o.OPERDAY          ,
                            d3 =  o.ID               ,
                            d4 =  o.BANKID           ,
                            d5 =  o.KLSDOX           ,
                            d6 =  o.DOCNUMB          ,
                            d7 =  o.DOCDATE          ,
                            d8 =  o.DOCTYPE          ,
                            d9 =  o.SUMPAY           ,
                            d10 = o.CLMFO           ,
                            d11 = o.CLACC           ,
                            d12 = o.CLINN           ,
                            d13 = o.CLNAME          ,
                            d14 = o.COMFO           ,
                            d15 = o.COACC           ,
                            d16 = o.COINN           ,
                            d17 = o.CONAME          ,
                            d18 = o.PURPOSE         ,
                            d19 = o.CHAPTER         ,
                            d20 = o.OKONH           ,
                            d21 = o.FILE_ID         ,//
                            d22 = o.OTCHISLEN       ,//
                            d23 = o.SUMMATOTAL      ,//
                            d24 = o.FEDERATIONSUM   ,// 
                            d25 = o.OBLASTSUM       ,
                            d26 = o.OblastName      ,
                            d27 = o.OkonxName       ,
                            d28 = o.otraslName
                        };

                //var resultList = O.ToList();

                //string csv = string.Empty;

                //csv += "K_TYPE         " + ',';
                //csv += "OPERDAY        " + ',';
                //csv += "ID             " + ',';
                //csv += "BANKID         " + ',';
                //csv += "KLSDOX         " + ',';
                //csv += "DOCNUMB        " + ',';
                //csv += "DOCDATE        " + ',';
                //csv += "DOCTYPE        " + ',';
                //csv += "SUMPAY         " + ',';
                //csv += "CLMFO          " + ',';
                //csv += "CLACC          " + ',';
                //csv += "CLINN          " + ',';
                //csv += "CLNAME         " + ',';
                //csv += "COMFO          " + ',';
                //csv += "COACC          " + ',';
                //csv += "COINN          " + ',';
                //csv += "CONAME         " + ',';
                //csv += "PURPOSE        " + ',';
                //csv += "CHAPTER        " + ',';
                //csv += "OKONH          " + ',';
                //csv += "FILE_ID        " + ',';
                //csv += "OTCHISLEN      " + ',';
                //csv += "SUMMATOTAL     " + ',';
                //csv += "FEDERATIONSUM  " + ',';
                //csv += "OBLASTSUM      " + ',';
                //csv += "OblastName     " + ',';
                //csv += "OkonxName      " + ',';
                //csv += "otraslName     " + ',';

                //foreach (var row in resultList)
                //{

                //    //Add the Data rows.
                //    //csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';

                //    csv += row.d1.ToString().Replace(",", ";") + ',';
                //    csv += row.d2.ToString().Replace(",", ";") + ',';
                //    csv += row.d3.ToString().Replace(",", ";") + ',';
                //    csv += row.d4.ToString().Replace(",", ";") + ',';
                //    csv += row.d5.ToString().Replace(",", ";") + ',';
                //    csv += row.d6.ToString().Replace(",", ";") + ',';
                //    csv += row.d7.ToString().Replace(",", ";") + ',';
                //    csv += row.d8.ToString().Replace(",", ";") + ',';
                //    csv += row.d9.ToString().Replace(",", ";") + ',';
                //    csv += row.d10.ToString().Replace(",", ";") + ',';
                //    csv += row.d11.ToString().Replace(",", ";") + ',';
                //    csv += row.d12.ToString().Replace(",", ";") + ',';
                //    csv += row.d13.ToString().Replace(",", ";") + ',';
                //    csv += row.d14.ToString().Replace(",", ";") + ',';
                //    csv += row.d15.ToString().Replace(",", ";") + ',';
                //    csv += row.d16.ToString().Replace(",", ";") + ',';
                //    csv += row.d17.ToString().Replace(",", ";") + ',';
                //    csv += row.d18.ToString().Replace(",", ";") + ',';
                //    csv += row.d19.ToString().Replace(",", ";") + ',';
                //    csv += row.d20.ToString().Replace(",", ";") + ',';
                //    csv += row.d21.ToString().Replace(",", ";") + ',';
                //    csv += row.d22.ToString().Replace(",", ";") + ',';
                //    csv += row.d23.ToString().Replace(",", ";") + ',';
                //    csv += row.d24.ToString().Replace(",", ";") + ',';
                //    csv += row.d25.ToString().Replace(",", ";") + ',';
                //    csv += row.d26.ToString().Replace(",", ";") + ',';
                //    csv += row.d27.ToString().Replace(",", ";") + ',';
                //    csv += row.d28.ToString().Replace(",", ";") + ',';


                //    //Add new line.
                //    csv += "\r\n";
                //}

                //Download the CSV file.
                //Response.Clear();
                //Response.Buffer = true;
                //Response.AddHeader("content-disposition", "attachment;filename=SqlExport.csv");
                //Response.Charset = "";
                //Response.ContentType = "application/text";
                //Response.Output.Write(csv);
                //Response.Flush();
                //Response.End();

                //return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "Report123.csv");

                var resultList = O.ToList();

                return Json(resultList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }
            return null;
        }
        public ActionResult PostupleniyaOblastyam()
        {
            return View();
        }
        public JsonResult GetPostupleniyaOblastyam(DateTime? bg, DateTime? end)
        {
            log.Info("GetPostupleniyaOblastyam");
            log.Info("BEGIN_DATE = " + bg);
            log.Info("END_DATE = " + end);
            try
            {
                var data = KasabaDB.sp_rt_Razbivka_Postup_OblastGroup(bg.Value.Date, end.Value.Date);
                var O = from o in data
                        select new
                        {
                            d1 = o.OblastName,
                            d2 = o.Kolichestvo,
                            d3 = o.TOTALSUM,
                            d4 = o.OBLASTSUM,
                        };

                var resultList = O.ToList();

                return Json(resultList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }
            return null;
        }
        public ActionResult PostupleniyaOtrasli()
        {
            return View();
        }
        public JsonResult GetPostupleniyaOtrasli(string s, DateTime? bg, DateTime? end)
        {
            log.Info("GetPostupleniyaOtrasli");
            log.Info("SOATO = " + s);
            log.Info("BEGIN_DATE = " + bg);
            log.Info("END_DATE = " + end);
            try
            {
                var data = KasabaDB.sp_rt_Razbivka_Postup_OtraslGroup(s, bg.Value.Date, end.Value.Date);
                var O = from o in data
                        select new
                        {
                            d1 = o.OblastName,
                            d2 = o.otraslName,
                            d3 = o.Kolichestvo,
                            d4 = o.TOTALSUM,
                            d5 = o.OBLASTSUM,
                        };

                var resultList = O.ToList();

                return Json(resultList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }
            return null;
        }
        public ActionResult PostupleniyaOkonxu()
        {
            return View();
        }
        public JsonResult GetPostupleniyaOkonxu(string s, DateTime? bg, DateTime? end)
        {
            log.Info("GetPostupleniyaOkonxu");
            log.Info("SOATO = " + s);
            log.Info("BEGIN_DATE = " + bg);
            log.Info("END_DATE = " + end);
            try
            {
                var data = KasabaDB.sp_rt_Razbivka_Postup_OkonxGroup(s, bg.Value.Date, end.Value.Date);
                var O = from o in data
                        select new
                        {
                            d1=o.OblastName,
                            d2 = o.OkonxName,
                            d3 = o.Kolichestvo,
                            d4 = o.TOTALSUM,
                            d5 = o.OBLASTSUM,
                        };

                var resultList = O.ToList();

                return Json(resultList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }
            return null;
        }
        public JsonResult GetOtrasl()
        {
            var O = from o in KasabaDB.sp_branch
                    select new
                    {
                        id = o.code,
                        value = o.name_uz
                    };
            var resList = O.ToList();

            resList.Insert(0, new { id = "0", value = "Все" });
            return Json(resList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRegion()
        {
            var R = (from r in KasabaDB.sp_soato
                     where r.parent_code == "17"
                     select new
                     {
                         id = r.code,
                         value = r.name_uz
                     });
            var resList = R.ToList();

            resList.Insert(0, new { id = "0", value = "Все" });
            return Json(resList, JsonRequestBehavior.AllowGet);
        }
    }
}