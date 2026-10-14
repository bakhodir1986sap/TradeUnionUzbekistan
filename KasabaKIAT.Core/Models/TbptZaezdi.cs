using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbpt_zaezdi")]
public partial class TbptZaezdi
{
    [Key]
    public long REC_ID { get; set; }
    public DateTime? ZAEZD_BEGIN { get; set; }
    public DateTime? ZAEZD_END { get; set; }
    public long? PLACE_TYPE_ID { get; set; }
    public decimal? PRICE { get; set; }
    public int? QUANTITY { get; set; }
    public int? SANATORY_ID { get; set; }
    public long? CAMP_ID { get; set; }
    public long? USER_ID { get; set; }
}
