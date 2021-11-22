using KasabaKIAT.ViewModel.BankOplati;
using KaznaFilesImporter;
using KaznaFilesImporter.BankOplati;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.BankOplati.Kaznacheystvo
{
    public class KaznaFilesDataManager
    {
        public static void InsertKaznaFile(ref KaznaFiles file)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var inputFileName = file.FileName;

                var fileKazna = db.KaznaFiles.FirstOrDefault(x => x.FileName.Contains(inputFileName));

                if (fileKazna != null)
                {
                    file.Id = fileKazna.ID;
                    return;
                }

                KaznaFile kfile = new KaznaFile();

                kfile.FileName = file.FileName;
                kfile.CreateDate = file.CreateDateTime;

                var kk = db.KaznaFiles.Add(kfile);

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                file.Id = kfile.ID;
            }
        }

        public static void InsertKaznaPaymentRows(ViewModel.BankOplati.KaznaPaymentFile row)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                KaznaFilesImporter.KaznaPaymentFile file = new KaznaFilesImporter.KaznaPaymentFile();
                CultureInfo provider = CultureInfo.InvariantCulture;

                file.K_TYPE     = row.k_type    ;
                try
                {
                    file.OPERDAY = DateTime.ParseExact(row.k_operday, "ddMMyyyy", provider);
                }
                catch
                {
                    file.OPERDAY = DateTime.Now;
                }
                  
                try
                {
                    file.ID = Int64.Parse(row.k_id);
                }
                catch
                {
                    file.ID = 0;
                }

                var exists = db.KaznaPaymentFiles.Where(z => z.ID == file.ID);

                if (exists.Any())
                {
                    db.KaznaPaymentFiles.Remove(exists.First());
                }

                try
                {
                    file.BANKID = Int64.Parse(row.k_bankid);
                }
                catch
                {
                    file.BANKID = 0;
                }


                try
                {
                    file.GRPID = Int64.Parse(row.k_grpid);
                }
                catch
                {
                    file.GRPID = 0;
                }

                file.KLSDOX   =   row.k_klsdox  ;    
                file.DOCNUMB  =   row.k_docnumb ;   
                
                try
                {
                    file.DOCDATE = DateTime.ParseExact(row.k_docdate, "ddMMyyyy", provider);
                } 
                catch
                {
                    file.DOCDATE = DateTime.Now;
                }

                file.DOCTYPE  =   row.k_doctype ;    

                try
                {
                    file.SUMPAY = Int64.Parse(row.k_sumpay);
                }
                catch
                {
                    file.SUMPAY = 0;
                }

                file.CLMFO    =   row.k_clmfo   ;    
                file.CLACC    =   row.k_clacc   ;    
                file.CLINN    =   row.k_clinn   ;    
                file.CLNAME   =   row.k_clname  ;    
                file.COMFO    =   row.k_comfo   ;    
                file.COACC    =   row.k_coacc   ;    
                file.COINN    =   row.k_coinn   ;    
                file.CONAME   =   row.k_coname  ;    
                file.PURPOSE  =   row.k_purpose ;    
                file.CHAPTER  =   row.k_chapter ;
                file.OKONH = row.k_okonh;
                file.FILE_ID = row.file_id;


                db.KaznaPaymentFiles.Add(file);

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

            }
        }


        public static void InsertKaznaBankRows(BankPaymentFile row)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                KaznaFilesImporter.KaznaBankFile file = new KaznaFilesImporter.KaznaBankFile();
                CultureInfo provider = CultureInfo.InvariantCulture;

                file.K_TYPE = row.type;
                try
                {
                    file.OPERDAY = DateTime.ParseExact(row.operday, "ddMMyyyy", provider);
                }
                catch
                {
                    file.OPERDAY = DateTime.Now;
                }

                try
                {
                    file.ID = Int64.Parse(row.id);
                }
                catch
                {
                    file.ID = 0;
                }

                file.KLSDOX = row.klsdox;
                file.DOCNUMB = row.docnumb;

                try
                {
                    file.DOCDATE = DateTime.ParseExact(row.docdate, "ddMMyyyy", provider);
                }
                catch
                {
                    file.DOCDATE = DateTime.Now;
                }

                file.DOCTYPE = row.doctype;

                try
                {
                    file.SUMPAY = Int64.Parse(row.sumpay);
                }
                catch
                {
                    file.SUMPAY = 0;
                }

                file.CLMFO = row.clmfo;
                file.CLACC = row.clacc;
                file.CLINN = row.clinn;
                file.CLNAME = row.clname;
                file.COMFO = row.comfo;
                file.COACC = row.coacc;
                file.COINN = row.coinn;
                file.CONAME = row.coname;
                file.PURPOSE = row.purpose;
                file.FILE_ID = row.file_id;

                try
                {
                    file.GRPID = Int64.Parse(row.grpid);
                }
                catch
                {
                    file.GRPID = 0;
                }

                db.KaznaBankFiles.Add(file);

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

            }
        }

        public static void InsertSverkaFile(SverkaFile file)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                KaznaSverkaFile kfile = new KaznaSverkaFile();

                kfile.K_TYPE = file.k_type;


                try
                {
                    kfile.K_OPERDAY = DateTime.ParseExact(file.k_operday, "ddMMyyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    kfile.K_OPERDAY = DateTime.Now;
                }

                kfile.FILE_NAME = file.file_name;
                kfile.CREATE_DATETIME = file.create_datetime;
                kfile.FILE_ID = file.file_id;

                var kk = db.KaznaSverkaFiles.Add(kfile);

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

            }
        }

        public static bool IsAlreadySettled(string fileName, int rowsCount)
        {


            return false;
        }

        public static void EnterNotExistsOrganizations()
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                db.sp_im_KaznaNewOrg();
            }
        }
    }
}