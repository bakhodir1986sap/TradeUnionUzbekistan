using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Utils
{
    public enum ProjectTypes
    {
        DEBUG,
        PRODUCTION
    }

    public class ProjectMode
    {
        public static ProjectTypes projectMode = ProjectTypes.PRODUCTION;
    }
}