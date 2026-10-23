using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblFase")]
public class TblFase
{
    [Key]
    public long ID { get; set; }
    public long? ID_Put { get; set; }
    public int? ID_Sanat { get; set; }
    public string? Num_Put { get; set; }
    public string? LastName { get; set; }
    public string? Name { get; set; }
    public string? FirstName { get; set; }
    public string? Adress { get; set; }
    public string? Rabota { get; set; }
    public string? Dolj { get; set; }
    public int? Stav_Vid { get; set; }
    public string? Pas_Ser { get; set; }
    public string? Pas_Num { get; set; }
    public string? Pas_KD { get; set; }
    public DateTime? Pas_Data { get; set; }
    public int? Gost_Pr { get; set; }
    public long? ID_Osnov { get; set; }
    public DateTime? Data_Post { get; set; }
    public string? Pol { get; set; }
    public long? UserId { get; set; }
    public string? DepartmentBase { get; set; }
    public string? INN { get; set; }
    public string? PNFL { get; set; }
}
