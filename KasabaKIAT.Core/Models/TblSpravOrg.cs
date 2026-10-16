using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblSprav_Org")]
public class TblSpravOrg
{
    [Key] public long ID { get; set; }
    public int? ID_Ot { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name_Org { get; set; } = string.Empty;

    [Required]
    [MaxLength(9)]
    public string INN { get; set; } = string.Empty;

    [MaxLength(20)]  public string? RS { get; set; }
    [MaxLength(5)]   public string? MFO { get; set; }
    [MaxLength(90)]  public string? Bank { get; set; }
    [MaxLength(5)]   public string? OKONX { get; set; }
    [MaxLength(100)] public string? Adress { get; set; }
    [MaxLength(70)]  public string? Telefon { get; set; }
    public int? Region { get; set; }
    public int? DT_OST { get; set; }
    public int? KT_OST { get; set; }
    [MaxLength(90)]  public string? E_Mail { get; set; }
    public long? UserId { get; set; }
    [MaxLength(255)] public string? DepartmentBase { get; set; }
    public bool IS_PRAVSOYUZ { get; set; }
    public long? ParentID { get; set; }
    public int? PRAVORGLEVEL { get; set; }
    [MaxLength(200)] public string? DIRECTOR { get; set; }
    [MaxLength(200)] public string? BUXGALTER { get; set; }
    [MaxLength(200)] public string? DOVERENNOE_LISO { get; set; }
    [MaxLength(20)]  public string? ORIG_INN { get; set; }
    [MaxLength(255)] public string? OWNER_CODE { get; set; }
    public DateTime? TIMESTAMP_CH { get; set; }
    public decimal? DB_BAL { get; set; }
    public decimal? CT_BAL { get; set; }
    [MaxLength(255)] public string? TO_DEPCODE { get; set; }
    public int? FACE_TYPE { get; set; }
    [MaxLength(50)]  public string? Pas_ser { get; set; }
    [MaxLength(50)]  public string? Pas_Num { get; set; }
    [MaxLength(50)]  public string? Pas_KD { get; set; }
    public DateTime? Pas_Data { get; set; }
}
