using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblSprav_San")]
public partial class TblSpravSan
{
    [Key]
    public int ID { get; set; }
    public string? Name_San { get; set; }
    public string? Num_Kod { get; set; }
    public long? UserId { get; set; }
    public int? KurortSanID { get; set; }

    public virtual ICollection<TbptPutyovki> tbpt_putyovkis { get; set; } = [];
}
