using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Enum;

namespace API.Entities;

public class Staff : BasedEntity<string>
{
    [Column(TypeName = "varchar")]
    [MaxLength(8)]
    public required string StaffId { get; set; }

    [Column(TypeName = "varchar")]
    [MaxLength(100)]
    public required string FullName { get; set; }

    public DateOnly Birthday { get; set; }
    public Gender Gender { get; set; }
}