using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbpt_zayavki")]
public class TbptZayavka
{
    [Key] public long ID { get; set; }
    public string? DEPARTMENT_BASE { get; set; }
    public long? ORG_ID { get; set; }
    public int? YEAR { get; set; }
    public int? KVARTAL { get; set; }
    public int? QUANTITY { get; set; }
    public string? STATUS { get; set; }
    public DateTime? CREATE_DATE { get; set; }
    public long? USER_ID { get; set; }
}
