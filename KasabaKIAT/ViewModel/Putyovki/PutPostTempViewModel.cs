using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Putyovki
{
    public class PutPostTempViewModel: IPostupleniyaViewModel
    {

        public long Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int BeginNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int EndNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime BeginSanDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EndSanDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private long _sanatoryId;
        public long SanatoryId
        {
            get
            {
                return _sanatoryId;
            }

            set
            {
                _sanatoryId = value;
            }
        }

        public decimal Price { get; set; }

        private long _userId;
        public long UserId
        {
            get
            {
                return _userId;
            }

            set
            {
                _userId = value;
            }
        }

        private string _departmentBase;

        public string DepartmentBase
        {
            get
            {
                return _departmentBase;
            }

            set
            {
                _departmentBase = value;
            }
        }
    }
}