using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurortUrvServiceTest.ServiceInterAction.ViewModel
{
    public class JournalViewModel
    {
        public long ID { get; set; }
        public long ServiceID { get; set; }
        public DateTime? CreateDT { get; set; }
        public long? UserID { get; set; }
        public string DepartmentBase { get; set; }
        public int? State { get; set; }
        public int? ErrorCode { get; set; }
        public string ErrorDesc { get; set; }
    }
}
