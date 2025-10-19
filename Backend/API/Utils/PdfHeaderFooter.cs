using iTextSharp.text;
using iTextSharp.text.pdf;

namespace API.Utils;

public class PdfHeaderFooter(string headerText, DateTime generatedDate) : PdfPageEventHelper
{
    private BaseFont _baseFont;
    private BaseFont _baseFontBold;

    public override void OnOpenDocument(PdfWriter writer, Document document)
    {
        base.OnOpenDocument(writer, document);
        _baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        _baseFontBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
    }

    public override void OnEndPage(PdfWriter writer, Document document)
    {
        // Header
        PdfContentByte cb = writer.DirectContent;
        cb.SaveState();
        cb.BeginText();
        cb.SetFontAndSize(_baseFontBold, 10);
        cb.SetTextMatrix(40, document.Top + 20);
        cb.ShowText(headerText);
        cb.EndText();
        cb.RestoreState();

        cb.SaveState();
        cb.BeginText();
        cb.SetFontAndSize(_baseFont, 8);
        cb.SetTextMatrix(40, 20);
        cb.ShowText($"Generated: {generatedDate:dd/MM/yyyy HH:mm:ss}");
        cb.EndText();
        cb.RestoreState();

        cb.SaveState();
        cb.BeginText();
        cb.SetFontAndSize(_baseFont, 8);
        cb.SetTextMatrix(document.Right - 80, 20);
        cb.ShowText($"Page {writer.PageNumber}");
        cb.EndText();
        cb.RestoreState();
    }
}