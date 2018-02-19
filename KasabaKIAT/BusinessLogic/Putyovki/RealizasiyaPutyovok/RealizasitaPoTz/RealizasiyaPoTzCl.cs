using KasabaKIAT.Models;
using KasabaKIAT.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Putyovki.RealizasiyaPutyovok.RealizasitaPoTz
{
    public class RealizasiyaPoTzCl
    {
        public List<RealizasiyaByTzViewModel> GetOformRealizList(List<SanAndPutViewModel> checkedList , string inn)
        {
            var list = new List<RealizasiyaByTzViewModel>();

            if (checkedList == null) return null;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var orgDepBase = db.t_departmentbase.Where(d => d.le_inn == inn).FirstOrDefault();

                if (orgDepBase != null)
                {
                    foreach (var chk in checkedList)
                    {
                        var real = new RealizasiyaByTzViewModel();

                        real.sanatorium_id = chk.SanId;
                        real.serail_number = chk.NumPut;

                        var putyovka = db.tblPos_Put.Where(f => f.Num_Put == chk.NumPut && f.ID_Sanat == chk.SanId);

                        if (putyovka.Any())
                        {
                            real.rest_begin_date = putyovka.First().DataSan;
                        }
                        else
                        {
                            throw new ApplicationException("Invalid Putyovka Number");
                        }

                        var sanName = db.tblSprav_San.Where(z => z.ID == chk.SanId);

                        if (sanName.Any())
                        {
                            real.SanatoryName = sanName.First().Name_San;
                        }
                        else
                        {
                            throw new ApplicationException("Invalid Sanatory Id");
                        }

                        var queryZayav = db.tbpt_zayavki.Where(z => z.DEPARTMENT_CODE == orgDepBase.code 
                        && z.SANATORY_ID == chk.SanId);

                        if (queryZayav.Any())
                        {
                            var queryZayavL = queryZayav.ToList();

                            foreach (var zayka in queryZayavL)
                            {
                                var zayavka = queryZayavL.FirstOrDefault();

                                if (zayavka == null) break;

                                real.zayavka_id = zayavka.REC_ID;
                                real.full_name = zayavka.FULL_NAME;

                                queryZayavL.Remove(zayavka);
                            }
                        }

                        list.Add(real);
                    }
                   
                }
                else
                {
                    throw new ApplicationException("invalid departmentbase");
                }


            }


            return list;
        }
    }
}