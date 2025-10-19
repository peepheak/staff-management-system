using System.ComponentModel.DataAnnotations.Schema;
using API.Enum;

namespace API.Entities;

public abstract class BasedEntity<TId>
{
    [Column(TypeName = "varchar(100)")] public TId Id { get; set; }

    public Status Status { get; set; } = Status.Active;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}