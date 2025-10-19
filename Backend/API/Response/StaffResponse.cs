using API.Enum;

namespace API.Response;

public class StaffResponse
{
    public string? Id { get; set; }
    public string? StaffId { get; set; }
    public string? FullName { get; set; }
    public DateOnly? Birthday { get; set; }
    public Gender Gender { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}