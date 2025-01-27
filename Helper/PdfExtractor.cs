using CandidateScreeningAI.Interface;
using CandidateScreeningAI.Services;
using System.Data;
using System.Text;
using System.Text.Json.Nodes;
using UglyToad.PdfPig;
//using UglyToad.PdfPig.DocumentLayoutAnalysis.PageSegmenter;
//using UglyToad.PdfPig.DocumentLayoutAnalysis.ReadingOrderDetector;
//using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;
//using UglyToad.PdfPig.Fonts.Standard14Fonts;
//using UglyToad.PdfPig.Writer;

public class PdfExtractor
{
    public string ExtractText(string pdfFilePath)
    {
        StringBuilder extractedText = new StringBuilder();

        pdfFilePath = @"C:\\Users\\SANTHOSH VENUGOPAL\\source\\repos\\CandidateScreeningAI\\Resumes\\SANTHOSH_VENUGOPAL_RESUME_.pdf";
        // Open the PDF document
        using (var document = PdfDocument.Open(pdfFilePath))
        {
            // Iterate through all pages and extract text
            foreach (var page in document.GetPages())
            {
                extractedText.AppendLine(page.Text);
            }
        }
        return extractedText.ToString();
    }
}
