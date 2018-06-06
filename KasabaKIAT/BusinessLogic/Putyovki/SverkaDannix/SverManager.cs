using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.SverkaDannix.Ruchnoy
{
    public enum OwnerType
    {
        Kurortniy,
        Oblastnoy
    }

    public class ResultSverka
    {
        public int      year        { get; set; }
        public int?      SanatId     { get; set; }
        public string   SanatName   { get; set; }
        //public int      owner_tp    { get; set; }
        //public string   owner_code  { get; set; }
        public string   NumPut          { get; set; }
        public string   KurortFio       { get; set; }
        public string   KurortAdress    { get; set; }
        public string   OblastFio       { get; set; }
        public string   OblastAdress    { get; set; }
    }

    public class SverManager
    {
        //Predpologaetsya chto sperva budut gruzitsya Kurortniye danniye 
        //potom dnniye oblastey. 
        //Algaritm matchinga takoy probigaemsya po dannim Kurortnogo
        //ishem po year, SanatId, NumPut
        public List<ResultSverka> MatchingEngineFiles(int year , int IdSan, string Obl)
        {
            List<ResultSverka> list = new List<ResultSverka>(); 

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                string sanatName = string.Empty;

                var sanats = db.tblSprav_San.Where(s => s.ID == IdSan);

                if (sanats.Any())
                {
                    sanatName = sanats.First().Name_San;
                }

                var total = db.tblSanatDataCompares.Where(z => z.YEAR == year && z.SANAT_ID == IdSan).ToList();

                if (!total.Any()) return list;

                var kurortList = total.
                    Where(z => z.YEAR == year
                       && z.OWNER_TP == (int)OwnerType.Oblastnoy 
                       && z.SANAT_ID == IdSan 
                       && z.OWNER_CODE == Obl);

                if (kurortList.Any())
                {
                    foreach (var putyovk in kurortList)
                    {
                        var oblastList = total.Where(
                           z => z.YEAR == year
                        && z.OWNER_TP == (int)OwnerType.Kurortniy
                        && z.SANAT_ID == IdSan 
                        //&& z.OWNER_CODE == Obl
                        && z.NUM_PUT == putyovk.NUM_PUT);

                        if (oblastList.Any())
                        {
                            var oblPut = oblastList.First();

                            if (!CompareNames(putyovk.FIO_OTDIX, oblPut.FIO_OTDIX))
                            {
                                var resSverka = new ResultSverka();
                                resSverka.year = year;
                                resSverka.SanatId = putyovk.SANAT_ID;
                                resSverka.SanatName = sanatName;
                                resSverka.NumPut = putyovk.NUM_PUT;
                                resSverka.KurortFio = oblPut.FIO_OTDIX;
                                resSverka.KurortAdress = oblPut.ADRESS ;
                                resSverka.OblastFio = putyovk.FIO_OTDIX ;
                                resSverka.OblastAdress = putyovk.ADRESS;

                                list.Add(resSverka);
                            }
                        }
                        else
                        {
                            var resSverka = new ResultSverka();
                            resSverka.year = year;
                            resSverka.SanatId = putyovk.SANAT_ID;
                            resSverka.SanatName = sanatName;
                            resSverka.NumPut = putyovk.NUM_PUT;
                            resSverka.OblastFio = putyovk.FIO_OTDIX;
                            resSverka.OblastAdress = putyovk.ADRESS;


                            list.Add(resSverka);
                        }
                    }
                }
            }

            return list;
        }

        private bool CompareNames(string firstName, string secondName)
        {
            return firstName.ToLower().Trim(' ').Equals(secondName.ToLower().Trim(' '));
        }

        /// <summary>
        /// Not finished method
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<ResultSverka> MatchingEngineExternal_Vvod(int year)
        {
            List<ResultSverka> list = new List<ResultSverka>();

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var YearBegin = new DateTime(year, 1, 1);

                var KurData = db.tbku_ZaezdyClientov.Where(z => z.DataZaezdaPlan >= YearBegin);

                if (KurData.Any())
                {
                    foreach(var putyovk in KurData)
                    {
                        var SanatoryKod = int.Parse(putyovk.SanatoryKod);

                        var oblData = from face in db.tblFase
                                      join realiz in db.tblVipl_Put on face.ID_Put equals realiz.ID
                                      join realMast in db.tblVipl on realiz.ID_Vipl equals realMast.ID
                                      where realMast.SmFil == year && face.Num_Put == putyovk.NomerPutevki
                                      && face.tblSprav_San.KurortSanID == SanatoryKod
                                      select new
                                      {
                                          SanatId   = face.ID_Sanat,

                                      };
                    }
                }
            }

            return list;
        }

        public List<ResultSverka> MatchingEngineMix(int year)
        {
            return new List<ResultSverka>();
        }
    }
}