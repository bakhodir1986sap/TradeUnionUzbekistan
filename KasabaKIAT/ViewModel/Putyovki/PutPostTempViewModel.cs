using KasabaKIAT.ViewModel.Putyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel.Putyovki
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
        private int? _sanatoryId;
        public int? SanatoryId
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

        //long IPostupleniyaViewModel.
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }

        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        long IPostupleniyaViewModel.UserId
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        string IPostupleniyaViewModel.DepartmentBase
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        long IPostupleniyaViewModel.SanatoryId
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}