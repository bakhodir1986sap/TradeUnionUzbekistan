using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace KasabaKIAT.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        public string ChangePuppiDate(DateTime? bg)
        {
            KasabaKIATEntities KasabaDB = new KasabaKIATEntities();

            var timeIsCome = KasabaDB.TimeShedulers.Where(z => z.id == 2);

            if (timeIsCome.Any())
            {
                var res = timeIsCome.First();

                res.LastChanged = bg.Value;
            }

            KasabaDB.SaveChanges();

            KasabaDB.Dispose();

            return "ok";
        }
    }
}
