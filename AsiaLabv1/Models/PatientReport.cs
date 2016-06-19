using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Models
{
    public class PatientReport
    {
        
        string path;
        List<PatientReportModel> model;
        Refer ReferDoc;
        Patient PatientInfo;
        string Branch;
        UserEmployee PatientDoctor;
        /// <summary>
        /// Initializes a new instance of the class BillFrom and opens the specified XML document.
        /// </summary>
        public PatientReport(string path, List<PatientReportModel> model, Refer ReferDoc, Patient PatientInfo, UserEmployee PatientDoctor, string branch)
        {
            this.model = model;
            this.path = path;
            this.ReferDoc = ReferDoc;
            this.PatientInfo = PatientInfo;
            this.Branch = branch;
            this.PatientDoctor = PatientDoctor;
        }

        /// <summary>
        /// Creates the report
        /// </summary>
        public PdfDocument CreateDocument()
        {
            PdfDocument pdf = new PdfDocument();
            PdfPage pdfPage = pdf.AddPage();

            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
            XFont font = new XFont("Arial, Helvetica, sans-serif", 10, XFontStyle.Bold);

            //left upper box
            DrawBoxOnPdf(graph, 15, 228, 320, 50);
            //right upper box
            DrawBoxOnPdf(graph, 344, 164, 250, 114);
            //table left box
            DrawBoxOnPdf(graph, 15, 292, 210, 16);
            //table center box
            DrawBoxOnPdf(graph, 225, 292, 200, 16);
            //table right box
            DrawBoxOnPdf(graph, 427, 292, 165, 16);



            XImage mg = System.Drawing.Image.FromFile(path + "Logo.jpg");
            graph.DrawImage(mg, 25, 30, 250, 50);

            WriteTextOnPdf(graph, font, pdfPage, model[0].TestCategoryName, 130, 150);

            WriteTextOnPdf(graph, font, pdfPage, Branch, 344, 150);

            WriteTextOnPdf(graph, font, pdfPage, "Test", 27, 294);

            WriteTextOnPdf(graph, font, pdfPage, "Result", 232, 294);

            WriteTextOnPdf(graph, font, pdfPage, "Normal Range", 432, 294);

            font = new XFont("Arial, Helvetica, sans-serif", 10, XFontStyle.Regular);
            WriteTextOnPdf(graph, font, pdfPage, model[0].DepartmentName, 132, 160);

            WriteTextOnPdf(graph, font, pdfPage, "Recipt #:", 350, 174);
            WriteTextOnPdf(graph, font, pdfPage, PatientInfo.Id.ToString(), 394, 174);

            WriteTextOnPdf(graph, font, pdfPage, "Patient Name:", 350, 189);
            WriteTextOnPdf(graph, font, pdfPage, PatientInfo.PatientName, 415, 189);

            WriteTextOnPdf(graph, font, pdfPage, PatientInfo.Gender.GenderDescription+" " + PatientInfo.Age + "Y", 350, 204);

            WriteTextOnPdf(graph, font, pdfPage, "Doctor Approved:", 350, 228);
            WriteTextOnPdf(graph, font, pdfPage, PatientDoctor.Name, 430, 228);

            WriteTextOnPdf(graph, font, pdfPage, "Date:", 350, 242);
            WriteTextOnPdf(graph, font, pdfPage, PatientInfo.DateTime.ToShortDateString(), 379, 242);

            font = new XFont("Arial, Helvetica, sans-serif", 10, XFontStyle.Regular);

            WriteTextOnPdf(graph, font, pdfPage, "COUNSULTING PHYSICIAN: ", 15, 177);
            WriteTextOnPdf(graph, font, pdfPage, ReferDoc.ReferredDoctorName, 148, 177);

            WriteTextOnPdf(graph, font, pdfPage, "REQUESTED BY: ", 15, 195);
            WriteTextOnPdf(graph, font, pdfPage, "CLINICAL INFORMATION / COMMENTS: ", 15, 213);
            WriteTextOnPdf(graph, font, pdfPage, "Request Slip Returned to patient", 18, 233);

            int Y = 325;
            //tests
            foreach (var item in model)
            {
                WriteTextOnPdf(graph, font, pdfPage, item.TestSubCategoryName, 32, Y);
                WriteTextOnPdf(graph, font, pdfPage, ""+item.Result+" "+item.Unit, 230, Y);
                WriteTextOnPdf(graph, font, pdfPage, "(" + item.LowerBound + "-" + item.UpperBound + ")", 430, Y);
                Y += 15;
            }

            return pdf;
        }


        public void WriteTextOnPdf(XGraphics graph, XFont font, PdfPage pdfPage, string text, int X, int Y)
        {
            graph.DrawString(text, font, XBrushes.Black,
            new XRect(X, Y, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
        }

        public void DrawBoxOnPdf(XGraphics graph, int X, int Y, float Width, float Height)
        {
            var rect = new RectangleF(X, Y, Width, Height);
            Pen pen = new Pen(System.Drawing.Color.Black, 2);
            pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
            graph.DrawRectangle(pen, rect);
        }

    }
}