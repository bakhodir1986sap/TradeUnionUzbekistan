using System;

namespace KasabaKIAT.BusinessLogic._3_Etap.Statistika.Stat_ViewModels
{
    public interface IBaseStatViewModel
    {
        string DepartmentBase { get; set; }
        int Year { get; set; }
        int Kvartal { get; set; }
        DateTime LastChangeDate { get; set; }
        int? AktivUsers { get; set; }
    }
}