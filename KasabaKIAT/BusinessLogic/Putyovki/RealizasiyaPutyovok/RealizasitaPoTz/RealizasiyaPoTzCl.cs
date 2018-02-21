using KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok;
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
        public List<RealizasiyaByTzViewModel> GetOformRealizList(List<SanAndPutViewModel> checkedList , string inn, string departmentBase, long userId)
        {
            var list = new List<RealizasiyaByTzViewModel>();
            List<long> exceptList = new List<long>();

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
                        real.to_department_code = orgDepBase.code;
                        real.department_code = departmentBase;
                        real.user_id = userId;

                        var putyovka = db.tblPos_Put.Where(f => f.Num_Put == chk.NumPut && f.ID_Sanat == chk.SanId);

                        if (putyovka.Any())
                        {
                            real.rest_begin_date = putyovka.First().DataSan;
                            real.price = putyovka.First().Symma_Put.Value;
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

                            bool freeZayavkaExists = false;
                            long zayka_rec = 0;

                            foreach (var zayka in queryZayavL)
                            {

                                var exists = exceptList.Where(ll => ll == zayka.REC_ID);

                                if (!exists.Any())
                                {
                                    freeZayavkaExists = true;

                                    zayka_rec = zayka.REC_ID;


                                    real.zayavka_id = zayka.REC_ID;
                                    real.full_name = zayka.FULL_NAME;

                                    break;
                                }
                            }

                            if (freeZayavkaExists) exceptList.Add(zayka_rec);
                            
                        }

                        RealizasiyaDataManager.Create(ref real);
                        PostPutDetailDataManager.Polzov((int)real.sanatorium_id, real.serail_number);
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