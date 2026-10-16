using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbpt_putyovka_ostatok_loader")]
public class TbptPutyovkaOstatokLoader
{
    [Key] public long ID { get; set; }
    public string? DEPARTMENT_BASE { get; set; }
    public int? ID_SANAT { get; set; }
    public string? NUM_PUT { get; set; }
    public decimal? PRICE { get; set; }
    public int? CUR_YEAR { get; set; }
    public int? CUR_KV { get; set; }
}
