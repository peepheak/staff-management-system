using API.Enum;

namespace API.Request;

public class StaffAddRequest
{
    public required string StaffId { get; set; }
    public required string FullName { get; set; }
    public DateOnly Birthday { get; set; }
    public Gender Gender { get; set; }
}