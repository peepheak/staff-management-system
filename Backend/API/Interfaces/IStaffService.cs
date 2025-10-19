using API.Enum;
using API.Request;
using API.Response;
using API.Wrapper;

namespace API.Interfaces;

public interface IStaffService
{
    Task<Result<string>> AddAsync(StaffAddRequest request, CancellationToken cancellationToken);
    Task<Result<string>> UpdateAsync(StaffUpdateRequest request, CancellationToken cancellationToken);
    Task<Result<string>> DeleteAsync(string id, CancellationToken cancellationToken);

    Task<PaginatedResult<StaffResponse>> GetAllAsync(string? staffId, int? gender, DateOnly? birthdayFrom,
        DateOnly? birthdayTo, int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

    Task<Result<StaffResponse>> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task<Result<byte[]>> ExportToPdfAsync(int pageNumber, int pageSize, string? staffId, int? gender,
        DateOnly? fromDate, DateOnly? toDate, CancellationToken cancellationToken);

    Task<Result<byte[]>> ExportToExcelAsync(int pageNumber, int pageSize, string? staffId, int? gender,
        DateOnly? fromDate, DateOnly? toDate, CancellationToken cancellationToken);
}