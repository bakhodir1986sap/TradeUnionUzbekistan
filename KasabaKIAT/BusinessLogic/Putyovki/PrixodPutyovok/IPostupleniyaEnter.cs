using KasabaKIAT.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok
{
    public interface IPostupleniyaEnter
    {
        void EnterProcess(IPostupleniyaViewModel model);

        void ModifyProcess(IPostupleniyaViewModel model);

        void DeleteProcess(IPostupleniyaViewModel model);
    }
}
