using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbpt_avarageprice")]
public class TbptAvaragePrice
{
    [Key] public long REC_ID { get; set; }
    public DateTime? SET_DATE { get; set; }
    public decimal? AMOUNT { get; set; }
    public long? USER_ID { get; set; }
}
