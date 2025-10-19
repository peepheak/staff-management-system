namespace API.Request;

public record StaffSearchRequest(
    string? StaffId = null,
    int? Gender = null,
    DateOnly? BirthdayFrom = null,
    DateOnly? BirthdayTo = null,
    int PageNumber = 1,
    int PageSize = 10
);