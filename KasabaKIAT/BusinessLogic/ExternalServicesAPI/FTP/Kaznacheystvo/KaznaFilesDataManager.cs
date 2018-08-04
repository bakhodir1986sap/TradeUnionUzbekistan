using KasabaKiat.ViewModel.BankOplati;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.BankOplati.Kaznacheystvo
{
    public class KaznaFilesDataManager
    {
        public static void InsertKaznaFile(ref KaznaFiles file)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                KasabaKIAT.Models.KaznaFiles kfile = new KasabaKIAT.Models.KaznaFiles();

                kfile.FileName = file.FileName;
                kfile.CreateDate = file.CreateDateTime;

                var kk = db.KaznaFiles.Add(kfile);

                db.SaveChanges();

                file.Id = kfile.ID;
            }
        }

        public static void InsertKaznaPaymentRows(ViewModel.BankOplati.KaznaPaymentFile row)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                KasabaKIAT.Models.KaznaPaymentFile file = new KasabaKIAT.Models.KaznaPaymentFile();
                CultureInfo provider = CultureInfo.InvariantCulture;

                file.K_TYPE     = row.k_type    ;
                try
                {
                    file.OPERDAY = DateTime.ParseExact(row.k_operday, "ddMMyyyy", provider);
                }
                catch(Exception)
                {
                    file.OPERDAY = DateTime.Now;
                }
                  
                try
                {
                    file.ID = Int64.Parse(row.k_id);
                }
                catch(Exception)
                {
                    file.ID = 0;
                }
                 
                try
                {
                    file.BANKID = Int64.Parse(row.k_bankid);
                }
                catch(Exception)
                {
                    file.BANKID = 0;
                }

                 
                file.KLSDOX   =   row.k_klsdox  ;    
                file.DOCNUMB  =   row.k_docnumb ;   
                
                try
                {
                    file.DOCDATE = DateTime.ParseExact(row.k_docdate, "ddMMyyyy", provider);
                } 
                catch(Exception)
                {
                    file.DOCDATE = DateTime.Now;
                }

                file.DOCTYPE  =   row.k_doctype ;    

                try
                {
                    file.SUMPAY = Int64.Parse(row.k_sumpay);
                }
                catch(Exception)
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
                file.OTCHISLEN = row.otchisleniye;

                db.SaveChanges();
            }
        }

        public static bool IsFileFullySettled(string fileName, int rowCount)
        {
            bool fileSettled = false;

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var fileFromDb = db.KaznaFiles.Where(f => f.FileName == fileName);

                if (fileFromDb.Any())
                {
                    var file = fileFromDb.FirstOrDefault();

                    if (file == null) return false;
                    var fileRows = db.KaznaPaymentFile.Where(r => r.FILE_ID == file.ID);

                    if (fileRows.Any())
                    {
                        if (rowCount == fileRows.Count())
                        {
                            fileSettled = true;
                        }
                    }
                }
            }

            return fileSettled;
        }
    }
}