using AsiaLabv1.Models;
using AsiaLabv1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;

namespace AsiaLabv1.Controllers
{
    public class AdminController : Controller
    {

        #region Services
        UserService UserServices = new UserService();
        BranchService BranchServices = new BranchService();
        GenderService GenderServices = new GenderService();
        TestDeptService TestDeptServices = new TestDeptService();
        TestCategoryService TestCategoryServices = new TestCategoryService();
        TestSubCategoryService TestSubCategoryServices = new TestSubCategoryService();
        ReferDoctorsService ReferDoctorsServices = new ReferDoctorsService();
        #endregion

        public static int CategId = 0;
        public static int DoctorId = 0;
        public static int EmpId = 0;

        public ActionResult Masters()
        {
            return View();
        }

        public ActionResult Accounting()
        {
            return View();
        }

        public ActionResult Templates()
        {
            return View();
        }

        public ActionResult BranchReport()
        {
            return View();
        }

        #region Employee Registration

        public ActionResult Register()
        {
            UserModel model = new UserModel();
            var branches = BranchServices.GetAllBranches();

            foreach (var item in branches)
            {
                model.Branches.Add(new SelectListItem
                {
                    Text = item.BranchName,
                    Value = item.Id.ToString()
                });
            }

            var Genders = GenderServices.GetAll();
            foreach (var item in Genders)
            {
                model.Genders.Add(new SelectListItem
                {
                    Text = item.GenderDescription,
                    Value = item.Id.ToString()
                });
            }
            var UserTypes = UserServices.GetAllUserTypes();
            foreach (var item in UserTypes)
            {
                model.UserTypes.Add(new SelectListItem
                {
                    Text = item.TypeDescription,
                    Value = item.Id.ToString()
                });
            }

            return View("RegisterEmployee", model);
        }

        [HttpPost]
        public ActionResult RegisterUser(UserModel usermodel)
        {
            var Employee = new UserEmployee()
            {
                Name = usermodel.Name,
                Username = usermodel.UserName,
                Password = usermodel.Password,
                BranchId = usermodel.BranchId
            };

            var EmployeeAddress = new Address()
            {
                UserTypeId = usermodel.UserTypeId,
                ContactNo = usermodel.ContactNo,
                Email = usermodel.Email,
                GenderId = usermodel.GenderId,
                Qualification = usermodel.Qualification,
                AddressDetail = usermodel.AddressDetails,
                CNIC = usermodel.CNIC
            };

            UserServices.RegisterUser(Employee, EmployeeAddress);
            //jo view call krna hai registration k baad wo view bna kr oska name paramter m yha pass krdena
            return RedirectToAction("Register", "Admin");
        }

        public ActionResult FillKendoForEmployees(string isFill)
        {
            // EmpId = isFill == "none" ? Convert.ToInt16(empid) : EmpId;
            if (isFill == "" || isFill == null)
            {

                var subCategories = UserServices.GetAllEmp();
                var addre = UserServices.GetAddress();


                List<UserModel> test = new List<UserModel>();
                foreach (var item in subCategories.Zip(addre, Tuple.Create))
                {

                    test.Add(new UserModel
                    {
                        Id = item.Item1.Id,
                        Name = item.Item1.Name,
                        Gender = item.Item2.Gender.GenderDescription,
                        UserName = item.Item1.Username,
                        Email = item.Item2.Email,
                        UserRole = item.Item2.UserType.TypeDescription,
                        BranchName = item.Item1.Branch.BranchName,  
                        

                    });
                }
                return Json(test, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateKendoGridEmp(string models)
        {
            IList<UserModel> objName = new JavaScriptSerializer().Deserialize<IList<UserModel>>(models);
            UserEmployee uemp= new UserEmployee();
            uemp.Id = objName[0].Id;
            uemp.Username = objName[0].UserName;

            UserServices.Update(uemp,uemp.Id);           
            return Json(uemp);
        }

        //public ActionResult DeleteEmp(string models)
        //{
        //    IList<ReferredModel> objName = new JavaScriptSerializer().Deserialize<IList<ReferredModel>>(models);
        //    ReferDoctorsServices.Delete(Convert.ToInt16(objName[0].Id));
        //    return Json("Record Deleted", JsonRequestBehavior.AllowGet);
        //}


        #endregion

        #region Test Mangnment
        public ActionResult TestsManagement()
        {
            TestManagementModel tManagementModel = new TestManagementModel();
            var departments = TestDeptServices.GetAllDept();
            var subdepartments = TestDeptServices.GetAllDeptSD();

            foreach (var item in departments)
            {
                tManagementModel.departments.Add(new SelectListItem
                {
                    Text = item.DepartmentName,
                    Value = item.Id.ToString()
                });
            }

            return View("TestsManagement", tManagementModel);
        }

        public ActionResult AddTestDepartmentsAndCategories(TestManagementModel model)
        {
            if (model.IsNewDepartment)
            {
                TestDeptServices.Add(new TestDepartment
                {
                    DepartmentName = model.deptName
                });

            }
            else
            {
                TestCategoryServices.Add(new TestCategory
                {
                    TestDepartmentId = model.deptId,
                    TestCategoryCode = "null",
                    TestName = model.testCategoryName

                });
            }

            return Json("Successfully Added", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddCategories(string id)
        {
            TestManagementModel tmanagementmodel = new TestManagementModel();
            var testCategories = TestCategoryServices.GetCatgByDeptId(Convert.ToInt16(id));

            List<RequiredItem> Categ = new List<RequiredItem>();
            foreach (var item in testCategories)
            {
                Categ.Add(new RequiredItem
                {

                    Id = item.Id,
                    testName = item.TestName

                });
            }


            return Json(Categ, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AddTests(TestSubCategoryModel model)
        {

            TestSubCategoryServices.Add(new TestSubcategory
            {
                TestSubcategoryName = model.TestSubcategoryName,
                UpperBound = model.UpperBound,
                LowerBound = model.LowerBound,
                Unit = model.Unit,
                Rate = model.Rate,
                TestCategoryId = model.TestCategoryId

            });
            return Json("Successfully Added", JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillDropdown(string categId, string subCategId)
        {

            if (categId != "")
            {

                var testCategories = TestCategoryServices.GetCatgByDeptId(Convert.ToInt16(categId));
                List<RequiredItem> Categ = new List<RequiredItem>();
                foreach (var item in testCategories)
                {
                    Categ.Add(new RequiredItem
                    {

                        Id = item.Id,
                        testName = item.TestName

                    });
                }


                return Json(Categ, JsonRequestBehavior.AllowGet);
            }

            else if (subCategId != "")
            {
                var subCategories = TestCategoryServices.GetSubCategById(Convert.ToInt16(subCategId));

                List<RequiredTest> test = new List<RequiredTest>();
                foreach (var item in subCategories)
                {
                    test.Add(new RequiredTest
                    {

                        Id = item.Id,
                        testName = item.TestSubcategoryName

                    });
                }

                return Json(test, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillDropdownKendo(string isFill, string subCategId)
        {
           CategId = isFill == "none" ? Convert.ToInt16(subCategId) : CategId;
            if (isFill == "" || isFill == null)
            {

                var subCategories = TestCategoryServices.GetSubCategById(CategId);

                List<RequiredTest> test = new List<RequiredTest>();
                foreach (var item in subCategories)
                {
                    test.Add(new RequiredTest
                    {

                        Id = item.Id,
                        testName = item.TestSubcategoryName,
                        upperBound = item.UpperBound,
                        lowerBound = item.LowerBound,
                        unit = item.Unit,
                        rate = item.Rate

                    });
                }
                return Json(test, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateKendoGrid(string models)
        {
            IList<RequiredTest> objName = new JavaScriptSerializer().Deserialize<IList<RequiredTest>>(models);
            TestSubcategory tsc = new TestSubcategory();
            tsc.Id = objName[0].Id;
            tsc.TestSubcategoryName = objName[0].testName;
            tsc.UpperBound = objName[0].upperBound;
            tsc.LowerBound = objName[0].lowerBound;
            tsc.Unit = objName[0].unit;
            tsc.Rate = objName[0].rate;
            tsc.TestCategoryId = TestSubCategoryServices.getById(tsc.Id).TestCategoryId;
            TestSubCategoryServices.Update(tsc, tsc.Id);
            return Json(tsc);
        }

        public ActionResult Delete(string models)
        {
            IList<RequiredTest> objName = new JavaScriptSerializer().Deserialize<IList<RequiredTest>>(models);
            TestSubCategoryServices.Delete(Convert.ToInt16(objName[0].Id));
            return Json("Record Deleted", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Refer Doctor

        public ActionResult DoctorReferrals()
        {
            ReferredModel model = new ReferredModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddReferDoctor(ReferredModel model)
        {
            ReferDoctorsServices.Add(new Refer
            {
                ReferredDoctorName = model.ReferredDoctorName,
                ReferredDocAddress = model.ReferredDocAddress,
                ReferredDocNumber = model.ReferredDocNumber,
                Remarks = model.Remarks,
                commission = model.commission
            });
            //return new JsonResult(); 
            return RedirectToAction("DoctorReferrals", "Admin");

        }


        public ActionResult FillKendoForDoctors(string isFill)
        {
            //DoctorId = isFill == "none" ? Convert.ToInt16(DocId) : DoctorId;
            if (isFill == "" || isFill == null)
            {

                //var subCategories = TestCategoryServices.GetSubCategById(CategId);
                var doctors = ReferDoctorsServices.GetAllReferDoctors();

                List<ReferredModel> doc = new List<ReferredModel>();
                foreach (var item in doctors)
                {
                    doc.Add(new ReferredModel
                    {

                        Id = item.Id,
                        ReferredDoctorName = item.ReferredDoctorName,
                        ReferredDocAddress = item.ReferredDocAddress,
                        ReferredDocNumber = item.ReferredDocNumber,
                        Remarks = item.Remarks,
                        commission = item.commission

                    });
                }
                return Json(doc, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateKendoGridDoc(string models)
        {
            IList<ReferredModel> objName = new JavaScriptSerializer().Deserialize<IList<ReferredModel>>(models);
            Refer refDoc = new Refer();
            refDoc.Id = objName[0].Id;
            refDoc.ReferredDoctorName = objName[0].ReferredDoctorName;
            refDoc.ReferredDocAddress = objName[0].ReferredDocAddress;
            refDoc.ReferredDocNumber = objName[0].ReferredDocNumber;
            refDoc.Remarks = objName[0].Remarks;
            refDoc.commission = objName[0].commission;
            ReferDoctorsServices.Update(refDoc, refDoc.Id);
            return Json(refDoc);
        }

        public ActionResult DeleteDoc(string models)
        {
            IList<ReferredModel> objName = new JavaScriptSerializer().Deserialize<IList<ReferredModel>>(models);
            ReferDoctorsServices.Delete(Convert.ToInt16(objName[0].Id));
            return Json("Record Deleted", JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
