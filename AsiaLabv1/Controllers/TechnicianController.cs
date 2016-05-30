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

        public static int _patienttestId,_patientId;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPatientInfo()
        {
 
            var patientInfo=pts.GetPatientTests();
            List<RequiredPatient> rp = new List<RequiredPatient>();
             foreach (var item in patientInfo)
                    {
                        rp.Add(new RequiredPatient
                        {
                           //Id=item.PatientId,
                           //PatientName=item.Patient.PatientName,
                           //PatientNumber=item.Patient.PatientNumber
                           Id=item.Id,
                           PatientName=item.PatientName,
                           PatientNumber=item.Id.ToString(),

                        });
                    }
                    return Json(rp, JsonRequestBehavior.AllowGet);

          }

        public ActionResult PerformTest()
        {
            return View();
        }

        public void Temp(string patientId)
        {
            _patientId = Convert.ToInt16(patientId);
            TempData["ID"] = patientId;
        }

        public ActionResult GetTests()
        {

            var patientDetails = pts.GetPatientTestsById(Convert.ToInt16(TempData["ID"]));
            List<RequiredTechnicianItems> TechnicianItems = new List<RequiredTechnicianItems>();
            List<RequiredTest> rt = new List<RequiredTest>();

                _patienttestId = patientDetails[0].Id;
                var pno = patientDetails[0].Patient.Id;
                var pname = patientDetails[0].Patient.PatientName;
                var ptests = patientDetails[0].Patient.PatientTests;

                foreach (var item2 in ptests)
                {

                    rt.Add(new RequiredTest
                    {
                        
                        Id=item2.TestSubcategory.Id,
                        testName=item2.TestSubcategory.TestSubcategoryName,
                        lowerBound=item2.TestSubcategory.LowerBound,
                        upperBound=item2.TestSubcategory.UpperBound,
                        unit=item2.TestSubcategory.Unit

                    });
                   
                }

                TechnicianItems.Add(new RequiredTechnicianItems { 
                
                PatientNumber=pno.ToString(),
                PatientName=pname,
                PatientTests=rt
                
                
                });
            

            return Json(TechnicianItems,JsonRequestBehavior.AllowGet);
        }

        public ActionResult TestResults(string[] result)
        {
            int id = _patienttestId;
            for (int i = 0; i < result.Length; i++)
            {
                pts.InsertPatientTestResults(new PatientTestResult { 
                
                PatientTestId=id,
                Result=result[i],
                ApprovalStatus="Not Approved",
                Remarks="Remarksss",

                });

            }

            pts.InsertTechnicianPatientTests(new TechnicianPatientsTest
            {

                TechnicianId = Convert.ToInt16(Session["loginuser"]),
                Dated = DateTime.Now,
                PatientId = _patientId,

            });


            return Json("Successfull");
        }
        

    }
}
