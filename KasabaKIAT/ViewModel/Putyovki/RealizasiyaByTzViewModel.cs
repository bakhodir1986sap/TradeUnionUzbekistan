using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Putyovki
{
    public class RealizasiyaByTzViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public long rec_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string department_code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string to_department_code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long sanatorium_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long camp_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string serail_number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? rest_begin_date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? rest_end_date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string full_name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal price { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int payment_persent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string reciever { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string doverennost_number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string dover_issue_date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string dover_expire_date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int quantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long zayavka_id { get; set; }

        public string zayavitelName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? platej_date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal platej_amount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string platej_number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? talon_back_date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string org_inn { get; set; }

        public string SanatoryName { get; set; }
    }
}