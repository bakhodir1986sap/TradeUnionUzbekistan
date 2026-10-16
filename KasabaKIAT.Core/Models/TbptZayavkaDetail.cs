using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbpt_zayavki_detail")]
public class TbptZayavkaDetail
{
    [Key] public long ID { get; set; }
    public long? ZAYAVKA_ID { get; set; }
    public int? ID_SANAT { get; set; }
    public int? QUANTITY { get; set; }
    public decimal? PRICE { get; set; }
}
