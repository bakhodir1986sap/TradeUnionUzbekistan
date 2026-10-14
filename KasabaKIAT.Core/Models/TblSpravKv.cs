using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblSprav_Kv")]
public partial class TblSpravKv
{
    [Key]
    public long ID { get; set; }
    public string? Name_Kv { get; set; }
    public long? UserId { get; set; }
}
