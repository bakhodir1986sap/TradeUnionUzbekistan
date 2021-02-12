using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel.Putyovki
{
    public class PostPutyovkaViewModel
    {
        public long             Id              { get; set; }

        public long?            IdPost          { get; set; }

        public int?            IdSanat         { get; set; }

        public string           SanatName       { get; set; }

        public string           Num_Put         { get; set; }

        public DateTime?        DataSan         { get; set; }

        public DateTime?        DataSanEnd      { get; set; }

        public decimal?         Symma_Put       { get; set; }

        public bool             ID_PolVoz       { get; set; }

        public DateTime?        Data_Post       { get; set; }

        public long?            UserId          { get; set; }

        public string           Departmentbase  { get; set; }

        public bool             Checked         { get; set; }

        public string           StatusPutyovki  { get; set; }

        //For History 

        public int? Id_Kv { get; set; }

        public DateTime? Data_Post_Main { get; set; }

        public string Num_Doc { get; set; }

        public string PosName { get; set; }

        public int? SmFil { get; set; }

        public string DepartmentBase { get; set; }

        //public override string ToString()
        //{
        //    return "Счет фактура=> Дата: " + Data_Post.ToString() + " Номер: " + Num_Doc +
        //        " Организация: " + PosName + " Год: " + SmFil + " квартал: " + Id_Kv +
        //        " Санаторий: " + SanatName + " Номер путевки: " + Num_Put + " Цена путевки " +
        //        Symma_Put + " Юзер Ид: " + UserId + " Филиал: " + Departmentbase;
        //}
    }
}