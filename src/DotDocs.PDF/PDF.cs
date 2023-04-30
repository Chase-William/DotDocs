using DotDocs.Models;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.PDF
{
    public static class PDF
    {
        public static void Make(RepositoryModel repo)
        {
            //Document document = new Document();
            //Section section = document.AddSection();

            //section.AddParagraph("Hello, World!");
            //section.AddParagraph();

            //Paragraph paragraph = section.AddParagraph();            
            //paragraph.Format.Font.Color = Color.FromCmyk(100, 30, 20, 50);            
            //paragraph.AddFormattedText("Hello, World!", TextFormat.Underline);

            //FormattedText ft = paragraph.AddFormattedText("Small text", TextFormat.Bold);            
            //ft.Font.Size = 6;

            //PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
            //pdfRenderer.Document = document;

            //pdfRenderer.RenderDocument();

            //string filename = "HelloWorld.pdf";            
            //pdfRenderer.PdfDocument.Save(filename);

            //Process.Start(filename);

            PdfDocument pdf = new PdfDocument();
            pdf.Info.Title = repo.Name;

            var page = pdf.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new("Arial", 20);

            gfx.DrawString(repo.Name, font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            const string fileName = "DotDocs Documentation.pdf";
            pdf.Save(fileName);
            Process.Start(fileName);
        }
    }
}
