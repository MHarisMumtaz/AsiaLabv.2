using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AsiaLabv1.Models;
using AsiaLabv1.Services;

namespace AsiaLabv1.Controllers
{
    public class TechnicianController : Controller
    {
        PatientTestService pts = new PatientTestService();

        public static int _patienttestId, _patientId;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Temp2(string approvalstatus)
        {
            if (approvalstatus == "Rejected")
            {
                Session["approvalstatus"] = approvalstatus;
            }
            else
            {
                Session["approvalstatus"] = null;
            }

            return Json("Successfull", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPatientInfo()
        {
            int SessionBrId = pts.GetBranchId(Session["branch"].ToString());


            var patientInfo = pts.GetPatientTests(SessionBrId);
            if (Session["approvalstatus"] != null)
            {
                patientInfo = pts.GetPatientTestsUpdate(Session["approvalstatus"].ToString());
            }

            List<RequiredPatient> rp = new List<RequiredPatient>();
            foreach (var item in patientInfo)
            {
                if (item.BranchId == SessionBrId)
                {
                    rp.Add(new RequiredPatient
                    {
                        //Id=item.PatientId,
                        //PatientName=item.Patient.PatientName,
                        //PatientNumber=item.Patient.PatientNumber

                        Id = item.Id,
                        PatientName = item.PatientName,
                        PatientNumber = item.Id.ToString(),


                    });
                }

            }
            return Json(rp, JsonRequestBehavior.AllowGet);

        }


        public ActionResult PerformTest()
        {
            if (Session["loginusername"] == null)
            {
                return RedirectToAction("LoginPage", "Main");
            }
            return View();
        }



        public void Temp(string patientId)
        {
            _patientId = Convert.ToInt16(patientId);
            Session["ID"] = patientId;
        }

        public ActionResult GetTests()
        {

            var patientDetails = pts.GetPatientTestsByPatientId(Convert.ToInt16(Session["ID"]));
            List<RequiredTechnicianItems> TechnicianItems = new List<RequiredTechnicianItems>();
            List<RequiredTest> rt = new List<RequiredTest>();

            _patienttestId = patientDetails[0].Id;
            var pno = patientDetails[0].Patient.Id;
            var comments = pts.GetComment(pno);
            var pname = patientDetails[0].Patient.PatientName;
            var ptests = patientDetails[0].Patient.PatientTests;

            foreach (var item2 in ptests)
            {

                rt.Add(new RequiredTest
                {

                    Id = item2.TestSubcategory.Id,
                    testName = item2.TestSubcategory.TestSubcategoryName,
                    lowerBound = item2.TestSubcategory.LowerBound,
                    upperBound = item2.TestSubcategory.UpperBound,
                    unit = item2.TestSubcategory.Unit,
                    comment = comments

                });

            }

            TechnicianItems.Add(new RequiredTechnicianItems
            {

                PatientNumber = pno.ToString(),
                PatientName = pname,
                PatientTests = rt,
                PreviousTests = pts.GetTestResultsById(_patienttestId),


            });


            return Json(TechnicianItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TestResults(string[] result)
        {
            var testids = pts.GetPatientTestsByPatientId(_patientId);
            int id = _patienttestId;
            if (Session["approvalstatus"] == null)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    pts.InsertPatientTestResults(new PatientTestResult
                    {

                        PatientTestId = testids[i].Id,
                        Result = result[i],
                        ApprovalStatus = "Not Approved",
                        Remarks = "Remarksss",

                    });

                }

                pts.InsertTechnicianPatientTests(new TechnicianPatientsTest
                {

                    TechnicianId = Convert.ToInt16(Session["loginuser"]),
                    Dated = DateTime.Now,
                    PatientId = _patientId,


                });
            }
            else
            {
                pts.UpdateRejectedTest(_patienttestId, result);
                Session["approvalstatus"] = null;

            }


            return Json("Successfull");
        }


    }
}
