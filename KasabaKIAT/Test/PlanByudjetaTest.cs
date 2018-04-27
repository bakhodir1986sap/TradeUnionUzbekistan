using KasabaKIAT.BusinessLogic.Buxgalteriya.PlanirovaniyaBudjeta.Region;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.Test
{
    public class PlanByudjetaTest
    {
        public void Process()
        {
            var plReg = new PlanRegion();

            string soato = string.Empty;
            string department_base = string.Empty;

            #region 1706
            soato = "1706";
            department_base = "17206";

            plReg.GetRegionPlan(soato, 2018, department_base, 1);

            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "31",
                godovoy = 5151515
            });

            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "32",
                godovoy = 123123123
            });

            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "33",
                godovoy = 43423423
            });

            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "34",
                godovoy = 888888
            });


            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "34",
                godovoy = 888888
            });


            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "10",
                godovoy = 123456,
                quarter1 = 132456,
                halfyear = 99999,
                month9 = 789456123
            });

            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "16",
                godovoy = 123456
            });
            #endregion

            #region 1703
            soato = "1703";
            department_base = "17203";

            plReg.GetRegionPlan(soato, 2018, department_base, 1);

            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "31",
                godovoy = 5151515
            });

            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "32",
                godovoy = 123123123
            });

            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "33",
                godovoy = 43423423
            });

            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "34",
                godovoy = 888888
            });


            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "34",
                godovoy = 888888
            });


            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "10",
                godovoy = 123456,
                quarter1 = 132456,
                halfyear = 99999,
                month9 = 789456123
            });

            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "16",
                godovoy = 123456
            });
            #endregion

            #region 1708
            soato = "1708";
            department_base = "17208";

            plReg.GetRegionPlan(soato, 2018, department_base, 1);

            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "31",
                godovoy = 5151515
            });

            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "32",
                godovoy = 123123123
            });

            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "33",
                godovoy = 43423423
            });

            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "34",
                godovoy = 888888
            });


            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "34",
                godovoy = 888888
            });


            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "10",
                godovoy = 123456,
                quarter1 = 132456,
                halfyear = 99999,
                month9 = 789456123
            });

            plReg.SaveChanges(new ViewModel.Buxgalteriya.PlanRegionViewModel()
            {
                year = 2018,
                department_base = department_base,
                userid = 1,
                soato_code = soato,
                item_code = "16",
                godovoy = 123456
            });
            #endregion
        }
    }
}