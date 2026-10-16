using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbku_journal")]
public class TbkuJournal
{
    [Key] public long ID { get; set; }
    public int? ServiceID { get; set; }
    public DateTime? CreateDT { get; set; }
    public long? UserID { get; set; }
    public string? DepartmentBase { get; set; }
    public int? State { get; set; }
    public int? ErrorCode { get; set; }
    public string? ErrorDesc { get; set; }
}
