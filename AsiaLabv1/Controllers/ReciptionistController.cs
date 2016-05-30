using AsiaLabv1.Models;
using AsiaLabv1.Services;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AsiaLabv1.Controllers
{
    public class ReciptionistController : Controller
    {
        UserService UserServices = new UserService();
        PatientService PatientServices = new PatientService();
        PatientTestService PatientTestService = new PatientTestService();
        GenderService GenderServices = new GenderService();
        TestDeptService TestDeptServices = new TestDeptService();
        TestCategoryService TestCategoryServices = new TestCategoryService();
        TestSubCategoryService TestSubCategoryServices = new TestSubCategoryService();
        ReferDoctorsService ReferDoctorsServices = new ReferDoctorsService();
        PatientPaymentService PatientPaymentServices = new PatientPaymentService();
        BranchService BranchServices = new BranchService();

        public ActionResult RegisterPatient()
        {
            var model = new PatientModel();
            var Genders = GenderServices.GetAll();
            foreach (var item in Genders)
            {
                model.Genders.Add(new SelectListItem
                {
                    Text = item.GenderDescription,
                    Value = item.Id.ToString()
                });
            }
            var allDept = TestDeptServices.GetAllDept();

            foreach (var Dept in allDept)
            {
                model.Departments.Add(new SelectListItem
                {
                    Value = Dept.Id.ToString(),
                    Text = Dept.DepartmentName
                });
            }

            var allrefers = ReferDoctorsServices.GetAllReferDoctors();
            foreach (var refer in allrefers)
            {
                model.ReferredDoctors.Add(new SelectListItem
                {
                    Value = refer.Id.ToString(),
                    Text = refer.ReferredDoctorName
                });
            }

            var Paytypes = PatientPaymentServices.GetAllPayTypes();
            foreach (var Paytype in Paytypes)
            {
                model.PayTypes.Add(new SelectListItem
                {
                    Value = Paytype.Id.ToString(),
                    Text = Paytype.Description
                });
            }

            return View("RegisterPatient", model);
        }

        [HttpPost]
        public ActionResult GetTests(int Id)
        {
            var Tests = TestSubCategoryServices.GetSubCategTestsByTestCategoryId(Id);
            var TestList = new List<TestSubCategoryModel>();
            
            #region for test code delete later
            //for (int i = 0; i < 10; i++)
            //{
            //    TestList.Add(new TestSubCategoryModel
            //    {
            //        Id = i,
            //        Rate = i*5,
            //        TestSubcategoryName = "Testname"+i.ToString()
            //    });
            //}
            #endregion

            foreach (var item in Tests)
            {
                TestList.Add(new TestSubCategoryModel
                {
                    Id = item.Id,
                    Rate = item.Rate,
                    TestSubcategoryName = item.TestSubcategoryName
                });
            }

            return Json(TestList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetSubCategory(int Id)
        {
            var TestList = TestCategoryServices.GetTestCategoryByDeptId(Id);
            var TestsList = new List<Tests>();
            foreach (var item in TestList)
            {
                TestsList.Add(new Tests
                {
                    Id = item.Id,
                    Name = item.TestName
                });
            }
            return Json(TestsList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddPatient(PatientModel model)
        {

            #region temp code this must be removed
            //PatientPaymentServices.AddPayType(new PayType
            //{
            //    Description = "Card"
            //});
            //PatientPaymentServices.AddPayType(new PayType
            //{
            //    Description = "Cash"
            //});
            #endregion
            #region testing code should be delete
            //var model=new PatientModel(){
            //    BranchId=1,
            //    Name="firstTestPatient",
            //    Email="Testpatient@gmail.com",
            //    GenderId=1,
            //    DateofBirth=DateTime.Now,
            //    PhoneNumber="987987697",
            //    ReferredId=-1
            //};
            #endregion

            if (model.PatientTestIds.Count > 0)
            {
                int UserId = Convert.ToInt32(Session["loginuser"].ToString());
                var branch = UserServices.GetUserBranch(UserId);
                model.BranchId = branch.Id;
                PatientServices.Add(model);
                List<TestSubcategory> selectedTests = new List<TestSubcategory>();
                double netAmount = 0;
                foreach (var TestId in model.PatientTestIds)
                {
                    PatientTestService.Add(new PatientTest
                    {
                        PatientId = model.Id,
                        TestSubcategoryId = TestId
                    });
                    var test = TestSubCategoryServices.getById(TestId);
                    selectedTests.Add(test);
                    netAmount = netAmount + test.Rate;
                }
               
                if (model.Discount > 0)
                {
                    netAmount = netAmount - model.Discount;
                }

                PatientPaymentServices.Add(new Payment
                {
                    PatientId = model.Id,
                    PatientName = model.Name,
                    PaidAmount = model.PaidAmount,
                    Discount = model.Discount,
                    NetAmount = netAmount,
                    Balance = model.PaidAmount - netAmount,
                    PayTypeId = model.PayId
                });
                
                var gender = GenderServices.GetById(model.GenderId);
                
                model.Genders.Add(new SelectListItem
                {
                    Text = gender.GenderDescription
                });
                
                var referdoctor = ReferDoctorsServices.GetReferDoctorById(model.ReferredId);
                
                model.ReferredDoctors.Add(new SelectListItem
                {
                    Text = referdoctor.ReferredDoctorName
                });
                var branchContact = BranchServices.GetBranchContact(branch.Id);
                GeneratePdf(model, selectedTests,branch,branchContact);

                return Json("SuccessFully Added Patient", JsonRequestBehavior.AllowGet);
            }
            return Json("Please assign tests to patients", JsonRequestBehavior.AllowGet);
            
        }

        [NonAction]
        public void GeneratePdf(PatientModel model,List<TestSubcategory> tests,Branch branch,Contact branchcontact)
        {
            var path = Server.MapPath("/images/");
            // Create a invoice form with the sample invoice data
            PatientRecipt patient = new PatientRecipt(path, model, tests,branch,branchcontact);

            // Create a MigraDoc document
            Document document = patient.CreateDocument();
            document.UseCmykColor = true;


            // Create a renderer for PDF that uses Unicode font encoding
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true);

            // Set the MigraDoc document
            pdfRenderer.Document = document;

            // Create the PDF document
            pdfRenderer.RenderDocument();

            // Save the PDF document...
            string filename = "Patient-"+model.Id+".pdf";
            //#if DEBUG
            //    // I don't want to close the document constantly...
            //    filename = "Patient-" + Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
            //#endif

            pdfRenderer.Save(Server.MapPath("/PatientsReport/") + filename);
            // ...and start a viewer.
            Process.Start(Server.MapPath("/PatientsReport/") + filename);


           // PdfDocument pdf = new PdfDocument();
           // PdfPage pdfPage = pdf.AddPage();
           // XGraphics graph = XGraphics.FromPdfPage(pdfPage);
           // XFont font = new XFont("Verdana", 20, XFontStyle.Bold);
           //// graph.DrawRectangle(XBrushes.BlueViolet, new RectangleF(0, 0, 100, 50));
           // graph.DrawString("This is my first PDF document", font, XBrushes.Black,
           // new XRect(0, 0, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.Center);
           // pdf.Save("firstpage.pdf");
            
        }

       

    }
}
