using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Models
{
    public class DoctorWiseSummaryReport
    {
        List<DoctorWiseSummaryModel> List;
        string branch;
        DateTime From;
        DateTime To;
        string Day;
        public DoctorWiseSummaryReport(List<DoctorWiseSummaryModel> list, string branch,DateTime from,DateTime to)
        {
            this.List = new List<DoctorWiseSummaryModel>();
            this.branch = branch;
            this.From = from;
            this.To = to;
            if (DateTime.Now.Day == 1)
                this.Day = "Monday";
            else if (DateTime.Now.Day == 2)
                this.Day = "Tuesday";
            else if (DateTime.Now.Day == 3)
                this.Day = "Wednesday";
            else if (DateTime.Now.Day == 4)
                this.Day = "Thursday";
            else if (DateTime.Now.Day == 5)
                this.Day = "Friday";
            else if (DateTime.Now.Day == 6)
                this.Day = "Saturday";
            else
                this.Day = "Sunday";
        }
        public PdfDocument CreateDocument()
        {
            int Y1 = 100;
            int Y = 740;
            PdfDocument pdf = new PdfDocument();
            PdfPage pdfPage = pdf.AddPage();
            XGraphics graph = XGraphics.FromPdfPage(pdfPage);


            XFont font = new XFont("Arial", 14, XFontStyle.Bold);
            WriteTextOnPdf(graph, font, pdfPage, "ASIA LAB DIAGNOSTIC CENTRE", 200, 5);

            font = new XFont("Arial", 12, XFontStyle.Regular);
            WriteTextOnPdf(graph, font, pdfPage, branch, 245, 25);

            font = new XFont("Arial", 13, XFontStyle.Bold);
            WriteTextOnPdf(graph, font, pdfPage, "DOCTOR WISE SUMMARY", 230, 40);

            font = new XFont("Arial", 9, XFontStyle.Bold);

            WriteTextOnPdf(graph, font, pdfPage, "Date To   :", 15, 50);
            WriteTextOnPdf(graph, font, pdfPage, "Date From    :", 15, 65);
            WriteTextOnPdf(graph, font, pdfPage, "Print On   :", 15, 85);
            font = new XFont("Arial", 9, XFontStyle.Regular);

            WriteTextOnPdf(graph, font, pdfPage,From.ToShortDateString(), 65, 50);
            WriteTextOnPdf(graph, font, pdfPage,To.ToLongTimeString(), 75, 65);
            WriteTextOnPdf(graph, font, pdfPage, Day, 65, 85);

            DrawRow(graph, Y1);

            font = new XFont("Arial", 8, XFontStyle.Bold);
            WriteTextOnPdf(graph, font, pdfPage, "S#", 10, Y1 + 2);
            WriteTextOnPdf(graph, font, pdfPage, "Doctor Name", 56, Y1 + 2);
            WriteTextOnPdf(graph, font, pdfPage, "NO.OF PAT", 307, Y1 + 2);
            WriteTextOnPdf(graph, font, pdfPage, "DOCTOR COMM BILL", 406, Y1 + 2);
            WriteTextOnPdf(graph, font, pdfPage, "DOCTOR COMM", 507, Y1 + 2);


            font = new XFont("Arial", 8, XFontStyle.Regular);
            Y1 += 15;
            int i = 1;
            double TotalPatient = 0, TotalCommBill = 0, TotalDocComm = 0;
            foreach (var item in List)
            {
                if (i % 40 == 0)
                {
                    pdfPage = pdf.AddPage();
                    graph = XGraphics.FromPdfPage(pdfPage);
                    Y1 = 40;
                    Y = 700;
                }
                DrawRow(graph, Y1);
                
                WriteTextOnPdf(graph, font, pdfPage, i.ToString(), 10, Y1 + 2);
                WriteTextOnPdf(graph, font, pdfPage, item.DoctorName, 59, Y1 + 5);
                WriteTextOnPdf(graph, font, pdfPage, item.PatNo.ToString(), 312, Y1 + 5);
                WriteTextOnPdf(graph, font, pdfPage, item.CommBill.ToString(), 409, Y1 + 5);
                WriteTextOnPdf(graph, font, pdfPage, item.DocCommision.ToString(), 511, Y1 + 5);
                
                TotalPatient += item.PatNo;
                TotalCommBill += item.CommBill;
                TotalDocComm += item.DocCommision;
                Y1 += 15;
                i++;
            }
            Y1 += 15;
            DrawRow(graph, Y1);
            WriteTextOnPdf(graph, font, pdfPage,"Total", 59, Y1 + 5);
            WriteTextOnPdf(graph, font, pdfPage, TotalPatient.ToString(), 312, Y1 + 5);
            WriteTextOnPdf(graph, font, pdfPage, TotalCommBill.ToString(), 409, Y1 + 5);
            WriteTextOnPdf(graph, font, pdfPage, TotalDocComm.ToString(), 511, Y1 + 5);
            return pdf;
        }

        public void DrawRow(XGraphics graph,int Y1)
        {
            DrawBoxOnPdf(graph, 5, Y1, 590, 15);
            DrawBoxOnPdf(graph, 5, Y1, 50, 15);
            DrawBoxOnPdf(graph, 55, Y1, 250, 15);
            DrawBoxOnPdf(graph, 306, Y1, 100, 15);
            DrawBoxOnPdf(graph, 406, Y1, 100, 15);
        }
        public void WriteTextOnPdf(XGraphics graph, XFont font, PdfPage pdfPage, string text, int X, int Y)
        {
            graph.DrawString(text, font, XBrushes.Black,
            new XRect(X, Y, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
        }

        public void DrawBoxOnPdf(XGraphics graph, int X, int Y, float Width, float Height)
        {
            var rect = new RectangleF(X, Y, Width, Height);
            Pen pen = new Pen(System.Drawing.Color.Black, 1);
            pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
            graph.DrawRectangle(pen, rect);
        }
    }
}