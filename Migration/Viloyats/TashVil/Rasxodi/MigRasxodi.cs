using KasabaKiat.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using KasabaKiat.BusinessLogic.Putyovki.RealizasiyaPutyovok;
using Migration.Viloyats.Common.Mapping.Sanatory;
using KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok;

namespace Migration.Viloyats.TashVil.Rasxodi
{
    public class MigRasxodi
    {
        public async Task Process()
        {
            using (KasabaKIAT_N1Entities db = new KasabaKIAT_N1Entities())
            {
                var rasxodi = db.tbl_mig_rasxodi.ToList().OrderBy(z=>z.POLYA02);

                string CurrentNumDoc = string.Empty;
                string ExistsCurrentNumDoc = string.Empty;
                tblVipl ViplMaint = null;

                var SanatMap = new SanatoryMapper();
                var ViplMain = new RealizasiyaViplMainViewModel();

                foreach (var rasxod in rasxodi)
                {
                    if (CurrentNumDoc.Equals(rasxod.POLYA02))
                    {
                        //Enter Next putyovkas of the same nakladnoy

                        if (!CurrentNumDoc.Equals(ExistsCurrentNumDoc))
                        {
                            Console.WriteLine("Putyovki =>" + rasxod.POLYA10);

                            var realiz = new RealizasiyaViplPutViewModel();

                            realiz.id_sanat = SanatMap.GetSanatId(rasxod.POLYA06);
                            realiz.num_put = rasxod.POLYA10;
                            realiz.num_proc = 100;
                            realiz.symma = decimal.Parse(rasxod.POLYA13);
                            realiz.id_polvoz = false;
                            realiz.data_post = DateTime.Now;
                            realiz.userid = 1;
                            realiz.zayavkaid = null;
                            realiz.departmentbase = "17227";

                            Vipl_PutDataManager.Create(realiz, db, ViplMaint);

                            PostPutDetailDataManager.Polzov(realiz.id_sanat.Value, realiz.num_put, db);
                        }
                    }
                    else
                    {


                        CurrentNumDoc = rasxod.POLYA02;

                        //Enter Nakladnoy i pervuyu putyovku

                        string naklNum = CurrentNumDoc.PadLeft(6, '0');

                        var nakExists = db.tblVipl.Where(v => v.SmFil == 2018 && v.ID_Kv == 1 && v.Num_Doc == naklNum);

                        if (nakExists.Any())
                        {
                            Console.WriteLine("Close Nakladnoy=>" + nakExists.First().Num_Doc);

                             ViplMain.id = nakExists.First().ID;

                             await ViplMainDataManager.CloseNakladnoy(ViplMain);

                            ExistsCurrentNumDoc = CurrentNumDoc;

                            continue;
                        }
                        else
                        {
                           

                            ExistsCurrentNumDoc = string.Empty;

                            ViplMain.userid = 1;
                            ViplMain.departmentbase = "17227";
                            ViplMain.smfil = 2018;
                            ViplMain.id_kv = 1;

                            var orgs = db.tblSprav_Org.Where(z => z.INN == rasxod.POLYA04);

                            if (orgs.Any())
                            {
                                ViplMain.id_org = orgs.First().ID;
                            }
                            else
                            {
                                tblSprav_Org orr = new tblSprav_Org();

                                using (KasabaKIAT_N1Entities db2 = new KasabaKIAT_N1Entities())
                                {
                                    orr.UserId = 1;
                                    orr.DepartmentBase = "17227";
                                    orr.INN = rasxod.POLYA04;
                                    orr.ORIG_INN = rasxod.POLYA04;
                                    orr.Name_Org = rasxod.POLYA05;

                                    var orgIns = db2.tblSprav_Org.Add(orr);

                                    db2.SaveChanges();
                                }

                                ViplMain.id_org = orr.ID;
                            }

                            DateTime dataPost;

                            if (DateTime.TryParseExact(rasxod.POLYA17.Substring(0 , 10), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataPost))
                            {
                                ViplMain.data_post = dataPost;
                            }
                            else
                            {
                                ViplMain.data_post = DateTime.Now;
                            }

                            ViplMain.num_doc = rasxod.POLYA02.PadLeft(6, '0');

                            Console.WriteLine("Putyovki =>" + ViplMain.num_doc);

                            ViplMain.name_dov = string.Format("№ {0} от {1} {2}", rasxod.POLYA19, rasxod.POLYA20.Substring(0, 10), rasxod.POLYA18 );
                            ViplMain.vid = false;

                            ViplMainDataManager.Create(ref ViplMain, db, out ViplMaint);

                            var realiz = new RealizasiyaViplPutViewModel();

                            realiz.id_sanat = SanatMap.GetSanatId(rasxod.POLYA06);
                            realiz.num_put = rasxod.POLYA10;
                            realiz.num_proc = 100;
                            realiz.symma = decimal.Parse(rasxod.POLYA13);
                            realiz.id_polvoz = false;
                            realiz.data_post = DateTime.Now;
                            realiz.userid = 1;
                            realiz.zayavkaid = null;
                            realiz.departmentbase = "17227";

                            Vipl_PutDataManager.Create(realiz, db, ViplMaint);

                            PostPutDetailDataManager.Polzov(realiz.id_sanat.Value, realiz.num_put, db);

                        }
                    }
                }

                db.SaveChanges();
            }
        }
    }
}
