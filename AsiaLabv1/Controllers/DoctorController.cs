using AsiaLabv1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AsiaLabv1.Controllers
{
    public class DoctorController : Controller
    {
        //
        // GET: /Doctor/

        PatientTestService pts = new PatientTestService();
        public static int _patienttestId, _patientId;
        public static string _deptname;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ApproveTest()
        {
            return View();
        }

        public ActionResult DeptName(string deptname)
        {
            _deptname = deptname;
            return Json("Successfull", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPatientInfo()
        {
            int SessionBrId = pts.GetBranchId(Session["branch"].ToString());

            var patientInfo = pts.GetPatientTestsDoctor(_deptname);
            List<RequiredPatient> rp = new List<RequiredPatient>();
            foreach (var item in patientInfo)
            {
                if (item.BranchId==SessionBrId)
                {
                    rp.Add(new RequiredPatient
                    {
                        Id = item.Id,
                        PatientName = item.PatientName,
                        PatientNumber = item.Id.ToString(),

                    });
                }                
            }
            return Json(rp, JsonRequestBehavior.AllowGet);

        }

        public void Temp(string patientId)
        {
            _patientId = Convert.ToInt16(patientId);
            Session["ID"] = patientId;
        }

        public ActionResult GetTests()
        {

            var patientDetails = pts.GetPatientTestsById(Convert.ToInt16(Session["ID"]));
            List<RequiredTechnicianItems> DoctorItems = new List<RequiredTechnicianItems>();
            List<RequiredTest> rt = new List<RequiredTest>();

            _patienttestId = patientDetails[0].Id;
            var results=pts.GetTestResultsById(_patienttestId);
            var pno = patientDetails[0].Patient.Id;
            var pname = patientDetails[0].Patient.PatientName;
            var ptests = patientDetails[0].Patient.PatientTests;
            int pointer = 0;

            foreach (var item2 in ptests)
            {

                rt.Add(new RequiredTest
                {

                    Id = item2.TestSubcategory.Id,
                    testName = item2.TestSubcategory.TestSubcategoryName,
                    result=results[pointer].ToString(),
                    lowerBound = item2.TestSubcategory.LowerBound,
                    upperBound = item2.TestSubcategory.UpperBound,
                    unit = item2.TestSubcategory.Unit

                });
                pointer++;

            }

            DoctorItems.Add(new RequiredTechnicianItems
            {

                PatientNumber = pno.ToString(),
                PatientName = pname,
                PatientTests = rt


            });


            return Json(DoctorItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ApprovePatientTest(string patientId, string comments)
        {
            pts.UpdateTest(Convert.ToInt16(patientId.Substring(12)));

            pts.InsertDoctorsPatientsTests(new DoctorPatientsTest
            {

                DoctorId = Convert.ToInt16(Session["loginuser"]),
                PatientId = _patientId,
                Dated = DateTime.Now

            });

            pts.InsertDoctorComments(new DoctorComment
            {

                PatientId = _patientId,
                Comments = comments,
                DoctorId = Convert.ToInt16(Session["loginuser"])

            });

            return Json("Successful");
        }


    }
}
