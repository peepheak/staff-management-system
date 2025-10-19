using API.Enum;

namespace API.Request;

public class StaffUpdateRequest
{
    public required string Id { get; set; }
    public required string StaffId { get; set; }
    public required string FullName { get; set; }
    public DateOnly Birthday { get; set; }
    public Gender Gender { get; set; }
}