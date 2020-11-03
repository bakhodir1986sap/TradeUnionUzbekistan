using KasabaKiat.ViewModel.Putyovki;
using System;
using System.Collections.Generic;

namespace KasabaKIAT.BusinessLogic.Putyovki.VozvratPutyovki
{
    public class VozvratEnterTemp
    {
        /// <summary>
        /// Data vozvrata
        /// </summary>
        public DateTime? data_post { get; set; }

        /// <summary>
        /// Nomer nakladnoy
        /// </summary>
        public string num_doc { get; set; }

        /// <summary>
        /// Reference to 
        /// table tblSprav_Otras
        /// </summary>
        public long? id_ot { get; set; }

        /// <summary>
        /// Doverennost
        /// </summary>
        public string dover { get; set; }

        /// <summary>
        ///  Summa vozvrata
        /// </summary>
        public decimal? symmap { get; set; }

        /// <summary>
        /// Reference to 
        /// table tblSprav_Kv
        /// </summary>
        public int? id_kv { get; set; }

        /// <summary>
        /// Year YYYY
        /// </summary>
        public int? smfil { get; set; }

        /// <summary>
        /// Tip vozvrata enumaration 
        /// { 0 - Vidanniye putyovki, 
        /// 1 - Spisanniye putyovki }
        /// </summary>
        public bool tip_voz { get; set; }

        /// <summary>
        /// Reference to table users
        /// </summary>
        public long? userid { get; set; }

        /// <summary>
        /// Reference to table t_departmentbase
        /// </summary>
        public string departmentbase { get; set; }

    }


    public class VozvratEnter
    {

        /// <summary>
        /// Data vozvrata
        /// </summary>
        public DateTime?                                    data_post           { get; set; }

        /// <summary>
        /// Nomer nakladnoy
        /// </summary>
        public string                                       num_doc             { get; set; }

         /// <summary>
        /// Reference to 
        /// table tblSprav_Otras
        /// </summary>
        public long?                                        id_ot               { get; set; }

        /// <summary>
        /// Doverennost
        /// </summary>
        public string                                       dover               { get; set; }

        /// <summary>
        ///  Summa vozvrata
        /// </summary>
        public decimal?                                     symmap              { get; set; }

        /// <summary>
        /// Reference to 
        /// table tblSprav_Kv
        /// </summary>
        public int?                                         id_kv               { get; set; }

        /// <summary>
        /// Year YYYY
        /// </summary>
        public int?                                         smfil               { get; set; }

        /// <summary>
        /// Tip vozvrata enumaration 
        /// { 0 - Vidanniye putyovki, 
        /// 1 - Spisanniye putyovki }
        /// </summary>
        public bool                                         tip_voz             { get; set; }

        /// <summary>
        /// Reference to table users
        /// </summary>
        public long?                                        userid              { get; set; }

        /// <summary>
        /// Reference to table t_departmentbase
        /// </summary>
        public string                                       departmentbase      { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<RealizasiyaViplPutViewModel> detailPutyovka { get; set; }

        public VozvratEnter()
        {
            detailPutyovka = new List<RealizasiyaViplPutViewModel>();
        }
    }
}