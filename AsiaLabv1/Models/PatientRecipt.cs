using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Drawing;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace AsiaLabv1.Models
{
    public class PatientRecipt
    {

        double NetAmount;
        double TotalCharges;
        PatientModel model;
        string LogedInUser;
        List<TestSubcategory> PatientTests;
        Branch Branch;
        Contact BranchContact;
        Gender gender;
        string path;

        public PatientRecipt(string path,string LogedInUser,Gender gender, PatientModel model, List<TestSubcategory> PatientTests,Branch branch,Contact branchcontact)
        {
            this.LogedInUser = LogedInUser;
            this.model = model;
            this.path = path;
            this.Branch = branch;
            this.BranchContact = branchcontact;
            this.PatientTests = PatientTests;
            this.gender = gender;
        }

        public PdfDocument CreateDocument()
        {
            #region old code
            // Create a new MigraDoc document
            //this.document = new Document();
            //this.document.Info.Title = "A sample invoice";
            //this.document.Info.Subject = "Demonstrates how to create an invoice.";
            //this.document.Info.Author = "Stefan Lange";

            //DefineStyles();

            //CreatePage();

            //FillContent();
            #endregion
            int ReciptSlideX = 100;
            PdfDocument pdf = new PdfDocument();
            PdfPage pdfPage = pdf.AddPage();
           
            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
            XFont font = new XFont("Arial, Helvetica, sans-serif", 12, XFontStyle.Bold);

            WriteTextOnPdf(graph, font, pdfPage, "Asia Laboratory", ReciptSlideX + 5, 30);
            DrawLineonPdf(graph, new PointF(ReciptSlideX - 10, 50), new PointF(ReciptSlideX + 170, 50), 3);
            font = new XFont("Arial, Helvetica, sans-serif", 9, XFontStyle.Regular);
            WriteTextOnPdf(graph, font, pdfPage,Branch.BranchName, ReciptSlideX + 10, 56);
            WriteTextOnPdf(graph, font, pdfPage, Branch.BranchAddress, ReciptSlideX + 10, 65);
            WriteTextOnPdf(graph, font, pdfPage, "Tel: "+BranchContact.BranchContactNo, ReciptSlideX + 10, 75);
            DrawLineonPdf(graph, new PointF(ReciptSlideX - 10, 90), new PointF(ReciptSlideX + 170, 90), 1);

            font = new XFont("Arial, Helvetica, sans-serif", 10, XFontStyle.Regular);
            WriteTextOnPdf(graph, font, pdfPage, "Statement of Asia Laboratory Charges", ReciptSlideX - 4, 98);

            font = new XFont("Arial, Helvetica, sans-serif", 9, XFontStyle.Regular);
            WriteTextOnPdf(graph, font, pdfPage, "Recipt. #", ReciptSlideX - 4, 118);
            WriteTextOnPdf(graph, font, pdfPage, ":  "+model.Id, ReciptSlideX + 35, 118);
            WriteTextOnPdf(graph, font, pdfPage, "Name", ReciptSlideX - 4, 133);
            WriteTextOnPdf(graph, font, pdfPage, ":  "+model.Name, ReciptSlideX + 35, 133);
            WriteTextOnPdf(graph, font, pdfPage, "Gender", ReciptSlideX - 4, 148);
            WriteTextOnPdf(graph, font, pdfPage, ":  " + gender.GenderDescription + " " + model.Age + "Y", ReciptSlideX + 35, 148);
            WriteTextOnPdf(graph, font, pdfPage, "Phone", ReciptSlideX - 4, 161);
            WriteTextOnPdf(graph, font, pdfPage, ":  "+model.PhoneNumber, ReciptSlideX + 35, 161);

            font = new XFont("Arial, Helvetica, sans-serif", 10, XFontStyle.Underline);
            WriteTextOnPdf(graph, font, pdfPage, "e-Reports", ReciptSlideX - 4, 180);
            WriteTextOnPdf(graph, font, pdfPage, ":  www.asialabs.com/LabReports", ReciptSlideX + 36, 180);
            font = new XFont("Arial, Helvetica, sans-serif", 10, XFontStyle.Regular);
            DrawLineonPdf(graph, new PointF(ReciptSlideX - 10, 194), new PointF(ReciptSlideX + 170, 194), 1);
            WriteTextOnPdf(graph, font, pdfPage, "Charges:", ReciptSlideX - 6, 205);
            font = new XFont("Arial, Helvetica, sans-serif", 8, XFontStyle.Underline);
            WriteTextOnPdf(graph, font, pdfPage, "Test", ReciptSlideX - 6, 217);
            WriteTextOnPdf(graph, font, pdfPage, "Amount", ReciptSlideX + 140, 217);
            font = new XFont("Arial, Helvetica, sans-serif", 9, XFontStyle.Bold);
            int X1 = ReciptSlideX - 6, X2 = ReciptSlideX + 140, Y = 230;
            foreach (var item in PatientTests)
            {
                WriteTextOnPdf(graph, font, pdfPage, item.TestSubcategoryName, X1, Y);
                WriteTextOnPdf(graph, font, pdfPage, item.Rate.ToString(), X2, Y);
                TotalCharges = TotalCharges + item.Rate;
                //NetAmount = NetAmount + item.Rate;
                Y += 13;
            }

            Y += 13;
            WriteTextOnPdf(graph, font, pdfPage, "Total Charges", X1, Y);
            WriteTextOnPdf(graph, font, pdfPage, TotalCharges.ToString(), X2, Y);
            Y += 12;
            DrawLineonPdf(graph, new PointF(ReciptSlideX - 10, Y), new PointF(ReciptSlideX + 170, Y), 1);
            Y += 13;
            WriteTextOnPdf(graph, font, pdfPage, "Payment Receivables:", X1, Y);
            font = new XFont("Arial, Helvetica, sans-serif", 9, XFontStyle.Regular);
            Y += 12;
            WriteTextOnPdf(graph, font, pdfPage, "Discount:", X1, Y);
            WriteTextOnPdf(graph, font, pdfPage, model.Discount.ToString(), X2, Y);
            Y += 12;
            WriteTextOnPdf(graph, font, pdfPage, "Net Amount:", X1, Y);
            WriteTextOnPdf(graph, font, pdfPage, (TotalCharges - model.Discount).ToString(), X2, Y);
            Y += 12;
            NetAmount = TotalCharges - model.Discount;
            WriteTextOnPdf(graph, font, pdfPage, "Paid Amount:", X1, Y);
            WriteTextOnPdf(graph, font, pdfPage, model.PaidAmount.ToString(), X2, Y);
            Y += 12;
            WriteTextOnPdf(graph, font, pdfPage, "Net Balance Due:", X1, Y);
            DrawLineonPdf(graph, new PointF(200, Y), new PointF(226, Y), 1);
            WriteTextOnPdf(graph, font, pdfPage, (NetAmount - model.PaidAmount).ToString(), X2, Y);
            Y += 12;
            DrawLineonPdf(graph, new PointF(ReciptSlideX - 10, Y), new PointF(ReciptSlideX + 170, Y), 1);
            font = new XFont("Arial, Helvetica, sans-serif", 9, XFontStyle.Bold);
            Y += 10;
            WriteTextOnPdf(graph, font, pdfPage, "Reporting Schedule", X1, Y);
            font = new XFont("Arial, Helvetica, sans-serif", 8, XFontStyle.Underline);
            Y += 12;
            WriteTextOnPdf(graph, font, pdfPage, "Reporting Date", X1, Y);
            WriteTextOnPdf(graph, font, pdfPage, "Description", X2, Y);
            Y += 12;
            font = new XFont("Arial, Helvetica, sans-serif", 8, XFontStyle.Regular);
            var ReportingDate = DateTime.Now.AddDays(1);
            foreach (var item in PatientTests)
            {
                WriteTextOnPdf(graph, font, pdfPage, ReportingDate.ToShortDateString() + "" + ReportingDate.ToShortTimeString(), X1, Y);
                WriteTextOnPdf(graph, font, pdfPage, item.TestSubcategoryName, X2, Y);
                Y += 12; 
            }
          
            Y += 18;
            DrawLineonPdf(graph, new PointF(ReciptSlideX - 10, Y), new PointF(ReciptSlideX + 170, Y), 1);
            Y += 12;
            font = new XFont("Arial, Helvetica, sans-serif", 7, XFontStyle.Regular);
            WriteTextOnPdf(graph, font, pdfPage, "Please collect your reports between 7:30PM to 8:30 PM on", X1, Y);
            Y += 12;
            WriteTextOnPdf(graph, font, pdfPage, "reporting day or any day during working hours after the", X1, Y);
            Y += 12;
            WriteTextOnPdf(graph, font, pdfPage, "report date.Please bring the original recipt to collect the report", X1, Y);
            Y += 12;
            WriteTextOnPdf(graph, font, pdfPage, "and or for refund required. Asia lab will not be responsible", X1, Y);
            Y += 12;
            WriteTextOnPdf(graph, font, pdfPage, "for report not collected within three months after the", X1, Y);
            Y += 12;
            WriteTextOnPdf(graph, font, pdfPage, "reporting date.", X1, Y);
            Y += 15;
            WriteTextOnPdf(graph, font, pdfPage, "Working Hours", X1, Y);
            WriteTextOnPdf(graph, font, pdfPage, "Mon to Sat   8:00Am to 10:30Pm", X2 - 75, Y);
            Y += 12;
            DrawLineonPdf(graph, new PointF(ReciptSlideX - 10, Y), new PointF(ReciptSlideX + 170, Y), 1);
            Y += 15;
            WriteTextOnPdf(graph, font, pdfPage, "Printed On:", X1, Y);
            WriteTextOnPdf(graph, font, pdfPage, DateTime.Now.ToShortDateString() + "" + DateTime.Now.ToShortTimeString(), X1 + 60, Y);
            Y += 12;
            WriteTextOnPdf(graph, font, pdfPage, "Printed By:", X1, Y);
            WriteTextOnPdf(graph, font, pdfPage, LogedInUser, X1 + 60, Y);
            return pdf;
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
            Pen pen = new Pen(System.Drawing.Color.Black, 2);
            pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
            graph.DrawRectangle(pen, rect);
        }

        #region old code
        ///// <summary>
        ///// Defines the styles used to format the MigraDoc document.
        ///// </summary>
        //void DefineStyles()
        //{

        //    // Get the predefined style Normal.
            
        //    Style style = this.document.Styles["Normal"];
            
        //    // Because all styles are derived from Normal, the next line changes the 
        //    // font of the whole document. Or, more exactly, it changes the font of
        //    // all styles and paragraphs that do not redefine the font.
            
        //    style.Font.Name = "Verdana";

        //    style = this.document.Styles[StyleNames.Header];
        //    style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

        //    style = this.document.Styles[StyleNames.Footer];
        //    style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

        //    // Create a new style called Table based on style Normal
        //    style = this.document.Styles.AddStyle("Table", "Normal");
        //    style.Font.Name = "Verdana";
        //    style.Font.Name = "Times New Roman";
        //    style.Font.Size = 9;

        //    // Create a new style called Reference based on style Normal
        //    style = this.document.Styles.AddStyle("Reference", "Normal");
        //    style.ParagraphFormat.SpaceBefore = "5mm";
        //    style.ParagraphFormat.SpaceAfter = "5mm";
        //    style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        //}

        ///// <summary>
        ///// Creates the static parts of the invoice.
        ///// </summary>
        //void CreatePage()
        //{
        //    // Each MigraDoc document needs at least one section.
        //    Section section = this.document.AddSection();

        //    // Put a logo in the header
        //    Image image = section.AddParagraph().AddImage(path + "Logo.jpg");
        //    image.Height = "2.5cm";
        //    image.LockAspectRatio = true;
        //   /// image.RelativeVertical = RelativeVertical.Line;
        //  ///  image.RelativeHorizontal = RelativeHorizontal.Margin;
        //    image.Top = ShapePosition.Top;
        //    image.Left = ShapePosition.Center;
        //    image.WrapFormat.Style = WrapStyle.Through;

        //    //// Create footer
        //    Paragraph paragraph = section.Footers.Primary.AddParagraph();
        //    paragraph.AddText("Powerd By: Xperium Technologies");
        //    paragraph.Format.Font.Size = 9;
        //    paragraph.Format.Alignment = ParagraphAlignment.Center;

        //    //// Create the text frame for the address
        //    //this.addressFrame = section.AddTextFrame();
        //    //this.addressFrame.Height = "3.0cm";
        //    //this.addressFrame.Width = "7.0cm";
        //    //this.addressFrame.Left = ShapePosition.Left;
        //    //this.addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
        //    //this.addressFrame.Top = "5.0cm";
        //    //this.addressFrame.RelativeVertical = RelativeVertical.Page;

        //    //// Put sender in address frame
        //    //paragraph = this.addressFrame.AddParagraph("PowerBooks Inc · Sample Street 42 · 56789 Cologne");
        //    //paragraph.Format.Font.Name = "Times New Roman";
        //    //paragraph.Format.Font.Size = 7;
        //    //paragraph.Format.SpaceAfter = 3;

        //    // Add the print date field
        //    paragraph = section.AddParagraph();
        //    paragraph.Format.SpaceBefore = "8cm";
        //    paragraph.Style = "Reference";
        //  //  paragraph.AddFormattedText("INVOICE", TextFormat.Bold);
        //    paragraph.AddTab();
        //    paragraph.AddText("Date:");
        //    paragraph.AddDateField("dd.MM.yyyy");

        //    // Create the item table
        //    this.table = section.AddTable();
        //    this.table.Style = "Table";
        //    this.table.Borders.Color = TableBorder;
        //    this.table.Borders.Width = 0.25;
        //    this.table.Borders.Left.Width = 0.5;
        //    this.table.Borders.Right.Width = 0.5;
        //    this.table.Rows.LeftIndent = 0;

        //    // Before you can add a row, you must define the columns
        //    Column column = this.table.AddColumn("1cm");
        //    column.Format.Alignment = ParagraphAlignment.Center;

        //    column = this.table.AddColumn("2.5cm");
        //    column.Format.Alignment = ParagraphAlignment.Right;

        //    column = this.table.AddColumn("3cm");
        //    column.Format.Alignment = ParagraphAlignment.Right;


        //    column = this.table.AddColumn("3.5cm");
        //    column.Format.Alignment = ParagraphAlignment.Right;

        //    column = this.table.AddColumn("2cm");
        //    column.Format.Alignment = ParagraphAlignment.Center;

        //    column = this.table.AddColumn("4cm");
        //    column.Format.Alignment = ParagraphAlignment.Right;

        //    // Create the header of the table
        //    Row row = table.AddRow();
        //    row.HeadingFormat = true;
        //    row.Format.Alignment = ParagraphAlignment.Center;
        //    row.Format.Font.Bold = true;
        //    row.Shading.Color = TableBlue;
        //    row.Cells[0].AddParagraph("Patient# ");
        //    row.Cells[0].Format.Font.Bold = false;
        //    row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
        //    row.Cells[0].VerticalAlignment = VerticalAlignment.Bottom;
        //    row.Cells[0].MergeRight = 3;
        //    row.Cells[4].AddParagraph(model.Id.ToString());
        //    row.Cells[4].Format.Alignment = ParagraphAlignment.Left;
        //    row.Cells[4].MergeRight = 1;
        //   // row.Cells[5].AddParagraph("Extended Price");
        //    //row.Cells[5].Format.Alignment = ParagraphAlignment.Left;
        //   // row.Cells[5].VerticalAlignment = VerticalAlignment.Bottom;
        //   // row.Cells[5].MergeDown = 1;

        //    row = table.AddRow();
        //    row.HeadingFormat = true;
        //    row.Format.Alignment = ParagraphAlignment.Center;
        //    row.Format.Font.Bold = false;
        //    row.Shading.Color = TableBlue;
        //    row.Cells[0].AddParagraph("Patient Name:");
        //    row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
        //    row.Cells[1].AddParagraph(model.Name);
        //    row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
        //    row.Cells[2].AddParagraph("Date of birth:");
        //    row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
        //    row.Cells[3].AddParagraph(model.DateofBirth.ToShortDateString());
        //    row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
        //    row.Cells[4].AddParagraph("Sex:");
        //    row.Cells[4].Format.Alignment = ParagraphAlignment.Left;
        //    row.Cells[5].AddParagraph(model.Genders[0].Text);
        //    row.Cells[5].Format.Alignment = ParagraphAlignment.Left;


        //    row = table.AddRow();
        //    row.HeadingFormat = true;
        //    row.Format.Alignment = ParagraphAlignment.Center;
        //    row.Format.Font.Bold = false;
        //    row.Shading.Color = TableBlue;
        //    row.Cells[0].AddParagraph("Referred By:");
        //    row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
        //    row.Cells[0].MergeRight = 2;
        //    row.Cells[3].AddParagraph(model.ReferredDoctors[0].Text);
        //    row.Cells[3].MergeRight = 2;


        //    row = table.AddRow();
        //    row.HeadingFormat = true;
        //    row.Format.Alignment = ParagraphAlignment.Center;
        //    row.Format.Font.Bold = false;
        //    row.Shading.Color = TableBlue;
        //    row.Cells[0].AddParagraph("Phone Number:");
        //    row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
        //    row.Cells[0].MergeRight = 1;
        //    row.Cells[2].AddParagraph(model.PhoneNumber);
        //    row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
        //    row.Cells[3].AddParagraph("Email:");
        //    row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
        //    row.Cells[3].MergeRight = 1;
        //    row.Cells[5].AddParagraph(model.Email);
        //    row.Cells[5].Format.Alignment = ParagraphAlignment.Left;

        //    foreach (var test in PatientTests)
        //    {
                
        //        row = table.AddRow();
        //        row.HeadingFormat = true;
        //        row.Format.Alignment = ParagraphAlignment.Center;
        //        row.Format.Font.Bold = false;
        //        row.Shading.Color = TableBlue;
        //        row.Cells[0].AddParagraph("Test Name:");
        //        row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
        //        row.Cells[0].MergeRight = 1;
        //        row.Cells[2].AddParagraph(test.TestSubcategoryName);
        //        row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
        //        row.Cells[3].AddParagraph("Rate:");
        //        row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
        //        row.Cells[3].MergeRight = 1;
        //        row.Cells[5].AddParagraph(test.Rate.ToString());
        //        row.Cells[5].Format.Alignment = ParagraphAlignment.Left;
                
        //        NetAmount = NetAmount + test.Rate;

        //    }
        //    this.table.SetEdge(0, 0, 6, 2, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);
        //}

        ///// <summary>
        ///// Creates the dynamic parts of the invoice.
        ///// </summary>
        //void FillContent()
        //{
        //    // Fill address in address text frame
        //   // XPathNavigator item = SelectItem("/invoice/to");
        //    //Paragraph paragraph = this.addressFrame.AddParagraph();
        //    //paragraph.AddText("my address");
        //    //paragraph.AddLineBreak();
        //    //paragraph.AddText("L 3/9 sector 15 A");
        //    //paragraph.AddLineBreak();
        //    //paragraph.AddText("78600" + " " + ", karachi");

        //    // Iterate the invoice items
        //   // double totalExtendedPrice = 0;
        //  //  XPathNodeIterator iter = this.navigator.Select("/invoice/items/*");
        //   // while (iter.MoveNext())
        //    //{
        //        //item = iter.Current;
        //        //double quantity = GetValueAsDouble(item, "quantity");
        //        //double price = GetValueAsDouble(item, "price");
        //        //double discount = GetValueAsDouble(item, "discount");

        //        // Each item fills two rows
        //    //    Row row1 = this.table.AddRow();
        //    //    Row row2 = this.table.AddRow();
        //    //    row1.TopPadding = 1.5;
        //    //    row1.Cells[0].Shading.Color = TableGray;
        //    //    row1.Cells[0].VerticalAlignment = VerticalAlignment.Center;
        //    //    row1.Cells[0].MergeDown = 1;
        //    //    row1.Cells[1].Format.Alignment = ParagraphAlignment.Left;
        //    //    row1.Cells[1].MergeRight = 3;
        //    //    row1.Cells[5].Shading.Color = TableGray;
        //    //    row1.Cells[5].MergeDown = 1;

        //    //    row1.Cells[0].AddParagraph("Row1");
        //    //    paragraph = row1.Cells[1].AddParagraph();
        //    //    paragraph.AddFormattedText("Row1 Title", TextFormat.Bold);
        //    //    paragraph.AddFormattedText(" by ", TextFormat.Italic);
        //    //    paragraph.AddText("author");
        //    //    row2.Cells[1].AddParagraph("quantity 12");
        //    //    row2.Cells[2].AddParagraph("12000 €");
        //    //    row2.Cells[3].AddParagraph("discount 0.0");
        //    //    row2.Cells[4].AddParagraph();
        //    //    row2.Cells[5].AddParagraph("price 0.00");
        //    //    //double extendedPrice = quantity * price;
        //    //    //extendedPrice = extendedPrice * (100 - discount) / 100;
        //    //    row1.Cells[5].AddParagraph("extend price 0.00" + " €");
        //    //    row1.Cells[5].VerticalAlignment = VerticalAlignment.Bottom;
        //    //  //  totalExtendedPrice += extendedPrice;

        //    //    this.table.SetEdge(0, this.table.Rows.Count - 2, 6, 2, Edge.Box, BorderStyle.Single, 0.75);
        //    ////}

        //    // Add an invisible row as a space line to the table
        //    Row row = this.table.AddRow();
        //    row.Borders.Visible = false;

        //    // Add the total price row
        //    row = this.table.AddRow();
        //    row.Cells[0].Borders.Visible = false;
        //    row.Cells[0].AddParagraph("Total Amount");
        //    row.Cells[0].Format.Font.Bold = true;
        //    row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
        //    row.Cells[0].MergeRight = 4;
        //    row.Cells[5].AddParagraph(NetAmount.ToString() + " Rs");

        //    // Add the VAT row
        //    row = this.table.AddRow();
        //    row.Cells[0].Borders.Visible = false;
        //    row.Cells[0].AddParagraph("Discount");
        //    row.Cells[0].Format.Font.Bold = true;
        //    row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
        //    row.Cells[0].MergeRight = 4;
        //    row.Cells[5].AddParagraph(model.Discount.ToString());

        //    // Add the additional fee row
        //    row = this.table.AddRow();
        //    row.Cells[0].Borders.Visible = false;
        //    row.Cells[0].AddParagraph("Paid Amount");
        //    row.Cells[5].AddParagraph(model.PaidAmount + " Rs");
        //    row.Cells[0].Format.Font.Bold = true;
        //    row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
        //    row.Cells[0].MergeRight = 4;

        //    // Add the total due row
        //    row = this.table.AddRow();
        //    row.Cells[0].AddParagraph("Net Amount");
        //    row.Cells[0].Borders.Visible = false;
        //    row.Cells[0].Format.Font.Bold = true;
        //    row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
        //    row.Cells[0].MergeRight = 4;
        //  //  totalExtendedPrice += 0.19 * totalExtendedPrice;
        //    row.Cells[5].AddParagraph((NetAmount - model.Discount) + " Rs");

        //    // Set the borders of the specified cell range
        //    this.table.SetEdge(5, this.table.Rows.Count - 4, 1, 4, Edge.Box, BorderStyle.Single, 0.75);

        //  //  // Add the notes paragraph
        //  //  paragraph = this.document.LastSection.AddParagraph();
        //  //  paragraph.Format.SpaceBefore = "1cm";
        //  //  paragraph.Format.Borders.Width = 0.75;
        //  //  paragraph.Format.Borders.Distance = 3;
        //  //  paragraph.Format.Borders.Color = TableBorder;
        //  //  paragraph.Format.Shading.Color = TableGray;
        //  ////  item = SelectItem("/invoice");
        //  //  paragraph.AddText("Notes");
        //}

        ///// <summary>
        ///// Selects a subtree in the XML data.
        ///// </summary>
        ////XPathNavigator SelectItem(string path)
        ////{
        ////    XPathNodeIterator iter = this.navigator.Select(path);
        ////    iter.MoveNext();
        ////    return iter.Current;
        ////}

        ///// <summary>
        ///// Gets an element value from the XML data.
        ///// </summary>
        ////static string GetValue(XPathNavigator nav, string name)
        ////{
        ////    //nav = nav.Clone();
        ////    XPathNodeIterator iter = nav.Select(name);
        ////    iter.MoveNext();
        ////    return iter.Current.Value;
        ////}

        ///// <summary>
        ///// Gets an element value as double from the XML data.
        ///// </summary>
        ////static double GetValueAsDouble(XPathNavigator nav, string name)
        ////{
        ////    try
        ////    {
        ////        string value = GetValue(nav, name);
        ////        if (value.Length == 0)
        ////            return 0;
        ////        return Double.Parse(value, CultureInfo.InvariantCulture);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        Debug.WriteLine(ex.Message);
        ////    }
        ////    return 0;
        ////}

        //// Some pre-defined colors
        //readonly static Color TableBorder = new Color(81, 125, 192);
        //readonly static Color TableBlue = new Color(235, 240, 249);
        //readonly static Color TableGray = new Color(242, 242, 242);
        #endregion
    }
}