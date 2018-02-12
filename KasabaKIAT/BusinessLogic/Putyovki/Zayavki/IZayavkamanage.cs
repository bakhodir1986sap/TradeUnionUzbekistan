using KasabaKiat.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KasabaKiat.BusinessLogic.Zayavki
{
    interface IZayavkaManage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="zayavka"></param>
        void PodachaZayavki(ZayavkiViewModel zayavka);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zayavki"></param>
        void OtpravkaZayavok();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zayavka"></param>
        void RedaktirovaniyeZayavki(ZayavkiViewModel zayavka);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Inn"></param>
        void RassmotreniyaZayavok(string Inn);
    }
}
