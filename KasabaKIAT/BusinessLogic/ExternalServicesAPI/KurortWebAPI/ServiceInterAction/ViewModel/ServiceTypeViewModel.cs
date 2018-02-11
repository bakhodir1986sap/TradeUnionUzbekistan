using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurortUrvServiceTest.ServiceInterAction.ViewModel
{
    public class ServiceTypeViewModel
    {
        public long ID { get; set; }
        public string ServiceDesc { get; set; }
        public string MethodName { get; set; }
        public bool? Direction { get; set; }
        public DateTime? CreateDt { get; set; }
    }
}
