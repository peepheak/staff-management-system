namespace API.Interfaces;

public interface IExportService
{
    Task<string> ExportAsync<TData>(IEnumerable<TData> data
        , Dictionary<string, Func<TData, object>> mappers
        , string sheetName = "Staffs");
}