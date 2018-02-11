using KasabaKIAT.Models;
using KurortUrvServiceTest.ServiceInterAction.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurortUrvServiceTest.ServiceInterAction.DataManagers
{
    public class JournalDataManager
    {
        public static void Add(ref JournalViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                tbku_journal journal = new tbku_journal();

                journal.ServiceID = model.ServiceID;
                journal.CreateDT = DateTime.Now;
                journal.UserID = 1;
                journal.DepartmentBase = "173";
                journal.State = 0;
                journal.ErrorCode = model.ErrorCode;
                journal.ErrorDesc = model.ErrorDesc;

                db.tbku_journal.Add(journal);

                db.SaveChanges();

                model.ID = journal.ID;
            }
        }

        public static void Modify(JournalViewModel model)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var journal = db.tbku_journal.FirstOrDefault(z => z.ID == model.ID);

                if (journal != null)
                {
                    journal.ErrorCode = model.ErrorCode;
                    journal.ErrorDesc = model.ErrorDesc;
                    journal.State = model.State;
                }

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
    }
}
