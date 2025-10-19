using System.Net;
using API.Common;
using API.Constants;
using API.Context;
using API.Entities;
using API.Enum;
using API.Extensions;
using API.Interfaces;
using API.Request;
using API.Response;
using API.Utils;
using API.Wrapper;
using AutoMapper;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class StaffService(AppDbContext context, IMapper mapper) : IStaffService, ITransientService
{
    public async Task<Result<string>> AddAsync(StaffAddRequest request, CancellationToken cancellationToken)
    {
        var existingRecord = await context.Staff
            .AsNoTracking()
            .Where(x => x.StaffId == request.StaffId)
            .AnyAsync(cancellationToken);
        if (existingRecord)
            return await Result<string>.FailAsync(ApplicationConstants.Message.Exists, HttpStatusCode.BadRequest);

        var requestAdd = mapper.Map<Staff>(request);
        requestAdd.Id = Guid.NewGuid().ToString();
        requestAdd.CreatedAt = DateTime.UtcNow;
        requestAdd.Status = Status.Active;
        await context.Staff.AddAsync(requestAdd, cancellationToken);
        var response = await context.SaveChangesAsync(cancellationToken);

        if (response > 0)
            return await Result<string>.SuccessAsync(requestAdd.Id, ApplicationConstants.Message.Saved,
                HttpStatusCode.OK);
        return await Result<string>.FailAsync(ApplicationConstants.Message.Failed, HttpStatusCode.InternalServerError);
    }

    public async Task<Result<string>> UpdateAsync(StaffUpdateRequest request, CancellationToken cancellationToken)
    {
        var requestUpdate = await context.Staff
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (requestUpdate == null)
            return await Result<string>.FailAsync($"{request.Id} not found", HttpStatusCode.NotFound);

        mapper.Map(request, requestUpdate);
        requestUpdate.UpdatedAt = DateTime.UtcNow;

        var response = await context.SaveChangesAsync(cancellationToken);
        if (response > 0)
            return await Result<string>.SuccessAsync(requestUpdate.Id, ApplicationConstants.Message.Updated,
                HttpStatusCode.OK);
        return await Result<string>.FailAsync(ApplicationConstants.Message.Failed, HttpStatusCode.InternalServerError);
    }

    public async Task<Result<string>> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var requestDelete = await context.Staff
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (requestDelete == null)
            return await Result<string>.FailAsync($"{id} not found", HttpStatusCode.NotFound);

        requestDelete.Status = Status.Deleted;
        requestDelete.UpdatedAt = DateTime.UtcNow;

        var response = await context.SaveChangesAsync(cancellationToken);
        if (response > 0)
            return await Result<string>.SuccessAsync(requestDelete.Id, ApplicationConstants.Message.Deleted,
                HttpStatusCode.OK);
        return await Result<string>.FailAsync(ApplicationConstants.Message.Failed, HttpStatusCode.InternalServerError);
    }

    public async Task<PaginatedResult<StaffResponse>> GetAllAsync(string? staffId, int? gender, DateOnly? birthdayFrom,
        DateOnly? birthdayTo, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var data = await context.Staff
            .Where(x => x.Status == Status.Active)
            .Where(x => string.IsNullOrEmpty(staffId) || x.StaffId.Contains(staffId))
            .Where(x => !gender.HasValue || x.Gender == (Gender)gender.Value)
            .Where(x => !birthdayFrom.HasValue || x.Birthday >= birthdayFrom.Value)
            .Where(x => !birthdayTo.HasValue || x.Birthday <= birthdayTo.Value)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new StaffResponse
            {
                Id = x.Id,
                StaffId = x.StaffId,
                FullName = x.FullName,
                Birthday = x.Birthday,
                Gender = x.Gender,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToPaginatedAsync(pageNumber, pageSize, HttpStatusCode.OK);

        return data;
    }

    public async Task<Result<StaffResponse>> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var data = await context.Staff
            .AsNoTracking()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (data == null)
            return await Result<StaffResponse>.FailAsync($"{id} not found", HttpStatusCode.NotFound);
        var result = mapper.Map<StaffResponse>(data);
        return await Result<StaffResponse>.SuccessAsync(result, ApplicationConstants.Message.Recieved,
            HttpStatusCode.OK);
    }

    public async Task<Result<byte[]>> ExportToPdfAsync(int pageNumber, int pageSize, string? staffId, int? gender,
        DateOnly? fromDate, DateOnly? toDate, CancellationToken cancellationToken)
    {
        try
        {
            var result = await GetAllAsync(staffId, gender, fromDate, toDate, pageNumber, pageSize, cancellationToken);

            if (result.Data == null || !result.Data.Any())
                return await Result<byte[]>.FailAsync("No data available to export", HttpStatusCode.NoContent);

            var pdfBytes = await GeneratePdfAsync(result.Data, staffId, gender, fromDate, toDate);
            return await Result<byte[]>.SuccessAsync(pdfBytes, ApplicationConstants.Message.Recieved,
                HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return await Result<byte[]>.FailAsync($"Error generating PDF: {ex.Message}",
                HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result<byte[]>> ExportToExcelAsync(int pageNumber, int pageSize, string? staffId, int? gender,
        DateOnly? fromDate, DateOnly? toDate, CancellationToken cancellationToken)
    {
        try
        {
            var result = await GetAllAsync(staffId, gender, fromDate, toDate, pageNumber, pageSize, cancellationToken);

            if (result.Data == null || !result.Data.Any())
                return await Result<byte[]>.FailAsync("No data available to export", HttpStatusCode.NoContent);

            var excelBytes = await GenerateExcelAsync(result.Data);
            return await Result<byte[]>.SuccessAsync(excelBytes, ApplicationConstants.Message.Recieved,
                HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return await Result<byte[]>.FailAsync($"Error generating Excel file: {ex.Message}",
                HttpStatusCode.InternalServerError);
        }
    }

    private static async Task<byte[]> GeneratePdfAsync(IEnumerable<StaffResponse> data, string? staffId, int? gender,
        DateOnly? fromDate, DateOnly? toDate)
    {
        return await Task.Run(() =>
        {
            using var stream = new MemoryStream();
            var pdfDoc = new Document(PageSize.A4.Rotate(), 15f, 15f, 30f, 20f);
            var writer = PdfWriter.GetInstance(pdfDoc, stream);
            writer.PageEvent = new PdfHeaderFooter("Staff Export Report", DateTime.Now);
            pdfDoc.Open();

            var title = new Paragraph("Staff Export Report", new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD))
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 10f
            };
            pdfDoc.Add(title);

            var staffResponses = data as StaffResponse[] ?? data.ToArray();
            var filterInfo = new Paragraph(
                $"Generated: {DateTime.Now:dd/MM/yyyy HH:mm:ss} | Total Records: {staffResponses.Length}",
                new Font(Font.FontFamily.HELVETICA, 9, Font.ITALIC))
            {
                Alignment = Element.ALIGN_RIGHT,
                SpacingAfter = 15f
            };
            pdfDoc.Add(filterInfo);

            // Add filter criteria if any
            if (!string.IsNullOrEmpty(staffId) || gender.HasValue || fromDate.HasValue || toDate.HasValue)
            {
                var filterCriteria = new List<string>();
                if (!string.IsNullOrEmpty(staffId)) filterCriteria.Add($"Staff ID: {staffId}");
                if (gender.HasValue) filterCriteria.Add($"Gender: {(Gender)gender.Value}");
                if (fromDate.HasValue) filterCriteria.Add($"From: {fromDate:dd/MM/yyyy}");
                if (toDate.HasValue) filterCriteria.Add($"To: {toDate:dd/MM/yyyy}");

                Paragraph criteria = new Paragraph(
                    $"Filters: {string.Join(" | ", filterCriteria)}",
                    new Font(Font.FontFamily.HELVETICA, 9))
                {
                    Alignment = Element.ALIGN_LEFT,
                    SpacingAfter = 15f
                };
                pdfDoc.Add(criteria);
            }

            // Create Table
            var table = new PdfPTable(4)
            {
                WidthPercentage = 100f,
                SpacingBefore = 10f,
                SpacingAfter = 10f
            };

            float[] widths = { 20f, 35f, 20f, 25f };
            table.SetWidths(widths);

            // Header styling
            var headerFont = new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.WHITE);
            var headerColor = new BaseColor(41, 128, 185);

            foreach (var headerText in new[] { "Staff ID", "Full Name", "Gender", "Birthday" })
            {
                var cell = new PdfPCell(new Phrase(headerText, headerFont))
                {
                    BackgroundColor = headerColor,
                    Padding = 8f,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BorderWidth = 1f,
                    BorderColor = new BaseColor(200, 200, 200)
                };
                table.AddCell(cell);
            }

            // Data rows
            var alternateColor = new BaseColor(245, 245, 245);
            var dataFont = new Font(Font.FontFamily.HELVETICA, 10);
            var rowIndex = 0;

            foreach (var staff in staffResponses)
            {
                var rowColor = (rowIndex % 2 == 0) ? BaseColor.WHITE : alternateColor;

                var cellData = new[]
                {
                    staff.StaffId,
                    staff.FullName,
                    staff.Gender.ToString(),
                    staff.Birthday.HasValue ? staff.Birthday.Value.ToString("dd/MM/yyyy") : "N/A"
                };

                foreach (var cellValue in cellData)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(cellValue ?? "", dataFont))
                    {
                        BackgroundColor = rowColor,
                        Padding = 6f,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        BorderWidth = 0.5f,
                        BorderColor = new BaseColor(220, 220, 220)
                    };
                    table.AddCell(cell);
                }

                rowIndex++;
            }

            pdfDoc.Add(table);

            var footer = new Paragraph("© 2025 Staff Management. Confidential.",
                new Font(Font.FontFamily.HELVETICA, 8, Font.ITALIC, BaseColor.GRAY))
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingBefore = 20f
            };
            pdfDoc.Add(footer);

            pdfDoc.Close();
            return stream.ToArray();
        });
    }

    private static async Task<byte[]> GenerateExcelAsync(IEnumerable<StaffResponse> data)
    {
        return await Task.Run(() =>
        {
            using var stream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Staff");

                worksheet.Cell(1, 1).Value = "Staff ID";
                worksheet.Cell(1, 2).Value = "Full Name";
                worksheet.Cell(1, 3).Value = "Gender";
                worksheet.Cell(1, 4).Value = "Birthday";

                for (var col = 1; col <= 4; col++)
                {
                    var headerCell = worksheet.Cell(1, col);
                    headerCell.Style.Font.Bold = true;
                    headerCell.Style.Fill.BackgroundColor = XLColor.FromHtml("2980B9");
                    headerCell.Style.Font.FontColor = XLColor.White;
                    headerCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                var rowIndex = 2;
                foreach (var staff in data)
                {
                    worksheet.Cell(rowIndex, 1).Value = staff.StaffId ?? "N/A";
                    worksheet.Cell(rowIndex, 2).Value = staff.FullName ?? "N/A";
                    worksheet.Cell(rowIndex, 3).Value = staff.Gender.ToString();
                    worksheet.Cell(rowIndex, 4).Value =
                        staff.Birthday.HasValue ? staff.Birthday.Value.ToString("dd/MM/yyyy") : "N/A";

                    if (rowIndex % 2 == 0)
                    {
                        for (var col = 1; col <= 4; col++)
                        {
                            worksheet.Cell(rowIndex, col).Style.Fill.BackgroundColor = XLColor.FromHtml("ECF0F1");
                        }
                    }

                    rowIndex++;
                }

                worksheet.Columns().AdjustToContents();
                workbook.SaveAs(stream);
            }

            return stream.ToArray();
        });
    }
}