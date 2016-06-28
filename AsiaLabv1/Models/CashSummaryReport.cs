using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Models
{
    public class CashSummaryReport
    {
        //new model
        List<CashSummaryModel> List;
        DateTime Date;
        string Branch;
        string Day;
        public CashSummaryReport(List<CashSummaryModel> list,DateTime date,string branch)
        {
            this.List = list;
            this.Date = date;
            if (date.Day == 1)
                this.Day = "Monday";
            else if (date.Day == 2)
                this.Day = "Tuesday";
            else if (date.Day == 3)
                this.Day = "Wednesday";
            else if (date.Day == 4)
                this.Day = "Thursday";
            else if (date.Day == 5)
                this.Day = "Friday";
            else if (date.Day == 6)
                this.Day = "Saturday";
            else
                this.Day = "Sunday";

            this.Branch = branch;
        }

        public PdfDocument CreateDocument()
        {
            int Y1 = 100;
            int Y = 740;
            PdfDocument pdf = new PdfDocument();
            PdfPage pdfPage = pdf.AddPage();

            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
            DrawBoxOnPdf(graph, 5, Y1, 590, 15);

            XFont font = new XFont("Arial", 14, XFontStyle.Bold);
            WriteTextOnPdf(graph, font, pdfPage, "ASIA LAB DIAGNOSTIC CENTRE", 200, 5);

            font = new XFont("Arial", 12, XFontStyle.Regular);
            WriteTextOnPdf(graph, font, pdfPage, Branch , 245, 25);

            font = new XFont("Arial", 13, XFontStyle.Bold);
            WriteTextOnPdf(graph, font, pdfPage, "Cash Summary Counter With Expense", 200, 40);

            font = new XFont("Arial", 9, XFontStyle.Bold);
            WriteTextOnPdf(graph, font, pdfPage, "Date     :", 300, 60);
            WriteTextOnPdf(graph, font, pdfPage, "DAY   :", 15, 50);
            WriteTextOnPdf(graph, font, pdfPage, "SHIFT   :", 15, 65);
            WriteTextOnPdf(graph, font, pdfPage, "TIME   :", 15, 85);
            font = new XFont("Arial", 9, XFontStyle.Regular);
            WriteTextOnPdf(graph, font, pdfPage, Date.ToShortDateString(), 335, 60);
            WriteTextOnPdf(graph, font, pdfPage, Day, 55, 50);
            WriteTextOnPdf(graph, font, pdfPage, "WHOLE", 55, 65);
            WriteTextOnPdf(graph, font, pdfPage, DateTime.Now.ToLongTimeString(), 55, 85);


            DrawBoxOnPdf(graph, 5, Y1, 20, 15);
            DrawBoxOnPdf(graph, 25, Y1, 50, 15);
            DrawBoxOnPdf(graph, 75, Y1, 50, 15);
            DrawBoxOnPdf(graph, 125, Y1, 90, 15);
            DrawBoxOnPdf(graph, 215, Y1, 90, 15);
            DrawBoxOnPdf(graph, 305, Y1, 90, 15);
            DrawBoxOnPdf(graph, 395, Y1, 40, 15);
            DrawBoxOnPdf(graph, 435, Y1, 40, 15);
            DrawBoxOnPdf(graph, 475, Y1, 40, 15);
            DrawBoxOnPdf(graph, 515, Y1, 40, 15);

            DrawVerticalLines(graph, Y1, Y, 0);
            font = new XFont("Arial", 9, XFontStyle.Bold);

            WriteTextOnPdf(graph, font, pdfPage, "S#", 5, Y1);
            WriteTextOnPdf(graph, font, pdfPage, "TIME", 25, Y1);
            WriteTextOnPdf(graph, font, pdfPage, "SLIP #.", 75, Y1);
            WriteTextOnPdf(graph, font, pdfPage, "PAITIENT", 125, Y1);
            WriteTextOnPdf(graph, font, pdfPage, "DOCTOR", 215, Y1);
            WriteTextOnPdf(graph, font, pdfPage, "CHARGES DETAIL", 305, Y1);
            WriteTextOnPdf(graph, font, pdfPage, "BILL", 397, Y1);
            WriteTextOnPdf(graph, font, pdfPage, "DIS", 437, Y1);
            WriteTextOnPdf(graph, font, pdfPage, "NET", 477, Y1);
            WriteTextOnPdf(graph, font, pdfPage, "REC", 517, Y1);
            WriteTextOnPdf(graph, font, pdfPage, "DUE", 557, Y1);

            font = new XFont("Arial", 8, XFontStyle.Regular);
            Y1 += 15;
            int i = 1;
            foreach (var item in List)
            {
                if (i % 40 == 0)
                {
                    pdfPage = pdf.AddPage();
                    graph = XGraphics.FromPdfPage(pdfPage);
                    Y1 = 40;
                    Y = 800;
                    DrawVerticalLines(graph, Y1, Y, 0);
                }
                WriteTextOnPdf(graph, font, pdfPage, i.ToString(), 6, Y1);
                WriteTextOnPdf(graph, font, pdfPage, item.Time.ToLongTimeString(), 26, Y1);
                WriteTextOnPdf(graph, font, pdfPage, item.SlipNo + "", 76, Y1);
                WriteTextOnPdf(graph, font, pdfPage, item.PatientName, 126, Y1);
                WriteTextOnPdf(graph, font, pdfPage, item.Doctor, 216, Y1);
                WriteTextOnPdf(graph, font, pdfPage, item.ChargeDetails, 306, Y1);
                WriteTextOnPdf(graph, font, pdfPage, item.Bill + "", 396, Y1);
                WriteTextOnPdf(graph, font, pdfPage, item.Dis + "", 436, Y1);
                WriteTextOnPdf(graph, font, pdfPage, item.Net + "", 476, Y1);
                WriteTextOnPdf(graph, font, pdfPage, item.Rec + "", 516, Y1);
                WriteTextOnPdf(graph, font, pdfPage, item.Due + "", 556, Y1);
                Y1 += 15;
            }
            i++;
            return pdf;
        }

        public void DrawVerticalLines(XGraphics graph,int Y1,int Y2, int stroke)
        {
            DrawLineonPdf(graph, new PointF(5, Y1), new PointF(5, Y2), stroke);
            DrawLineonPdf(graph, new PointF(515, Y1), new PointF(515, Y2), stroke);
            DrawLineonPdf(graph, new PointF(25, Y1), new PointF(25, Y2), stroke);
            DrawLineonPdf(graph, new PointF(75, Y1), new PointF(75, Y2), stroke);
            DrawLineonPdf(graph, new PointF(125, Y1), new PointF(125, Y2), stroke);
            DrawLineonPdf(graph, new PointF(215, Y1), new PointF(215, Y2), stroke);
            DrawLineonPdf(graph, new PointF(305, Y1), new PointF(305, Y2), stroke);
            DrawLineonPdf(graph, new PointF(395, Y1), new PointF(395, Y2), stroke);
            DrawLineonPdf(graph, new PointF(435, Y1), new PointF(435, Y2), stroke);
            DrawLineonPdf(graph, new PointF(475, Y1), new PointF(475, Y2), stroke);
            DrawLineonPdf(graph, new PointF(515, Y1), new PointF(515, Y2), stroke);
            DrawLineonPdf(graph, new PointF(555, Y1), new PointF(555, Y2), stroke);
            DrawLineonPdf(graph, new PointF(595, Y1), new PointF(595, Y2), stroke);
        }

        public void DrawLineonPdf(XGraphics graph, PointF P1, PointF P2, int stroke)
        {
            Pen blackPen = new Pen(System.Drawing.Color.Black, stroke);
            graph.DrawLine(blackPen, P1, P2);
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