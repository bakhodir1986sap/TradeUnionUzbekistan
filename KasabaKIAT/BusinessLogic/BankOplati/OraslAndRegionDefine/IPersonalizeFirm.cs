using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KasabaKIAT.BusinessLogic.BankOplati.OraslAndRegionDefine
{
    interface IPersonalizeFirm
    {
        string FindOtraslByInn(string Inn);

        byte? FindOtraslByAccount(string account);

        string FindRegionByInn(string Inn);

        byte? FindRegionByAccount(string account);
    }
}
