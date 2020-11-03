using KasabaKIAT.BusinessLogic.StartProcess.LoaderViewModels;
using KasabaKIAT.Models;
using System.Linq;

namespace KasabaKIAT.BusinessLogic.StartProcess.LoaderDataManagers
{
    public class DebitorVaCreditorDataManager
    {
        public static void Enter(DebitorVaCreditorViewModel model, KasabaKIATEntities db)
        {
            var organizations = db.tblSprav_Org.Where(z => z.DepartmentBase == model.departmentbase
                                                        && z.INN == model.inn);

            if (organizations.Any())
            {
                var organization = organizations.First();

                organization.DB_BAL = model.debit_bal;
                organization.CT_BAL = model.credit_bal;
            }
            else
            {
                var organizations1 = db.tblSprav_Org.Where(z=>z.INN == model.inn && z.DepartmentBase == model.departmentbase);

                if (organizations1.Any()) return; //Dublication not allowed

                var org = new tblSprav_Org
                {
                    INN = model.inn,
                    Name_Org = model.NameOrg,
                    DB_BAL = model.debit_bal,
                    CT_BAL = model.credit_bal
                };

                db.tblSprav_Org.Add(org);
            }
        }
    }
}