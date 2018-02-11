using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KasabaKIAT.ViewModel.Putyovki
{
    public interface IPostupleniyaViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        long SanatoryId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        long UserId { get; set; }

        /// <summary>
        /// Which Branch entered
        /// </summary>
        string DepartmentBase { get; set; }
    }
}
