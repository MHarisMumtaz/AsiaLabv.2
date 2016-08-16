using AsiaLabv1.Models;
using AsiaLabv1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AsiaLabv1.Controllers
{
    public class MainController : Controller
    {

        //this class is used for user/employee related queries
        //any query related to employee has been written in this class
        UserService UsersService = new UserService();
        BranchService BranchesService = new BranchService();
        PatientPaymentService PatientsPaymentService = new PatientPaymentService();
        public ActionResult MainPage()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Laboratory()
        {
            return View();
        }
        public ActionResult Ultrasound()
        {
            return View();
        }
        public ActionResult Xrays()
        {
            return View();
        }
        public ActionResult opg()
        {
            return View();
        }
        public ActionResult ecg()
        {
            return View();
        }
        public ActionResult health()
        {
            return View();
        }
        public ActionResult wecare()
        {
            return View();
        }
        public ActionResult faqs()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult SearchReport()
        {
            return View();
        }
        public ActionResult Report()
        {
            return View();
        }

        public ActionResult LoginPage()
        {
            return View("LoginPage");
        }



        public ActionResult EditProfile()
        {
            return View();
        }

        public ActionResult AdminDashboard()
        {



            if (Session["loginusername"] == null)
            {
                return RedirectToAction("LoginPage", "Main");
            }
            return View();
        }

        public ActionResult DoctorDashboard()
        {
            if (Session["loginusername"] == null)
            {
                return RedirectToAction("LoginPage", "Main");
            }
            return View();
        }
        public ActionResult ReceptionistDashboard()
        {
            if (Session["loginusername"] == null)
            {
                return RedirectToAction("LoginPage", "Main");
            }
            return View();
        }
        public ActionResult TechnicianDashboard()
        {
            if (Session["loginusername"] == null)
            {
                return RedirectToAction("LoginPage", "Main");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection LoginForm)
        {

            /*
             * for admin login and password
                username: testadmin
                password: admin2016
             * for reciptionist login and password
                username:  TestReceiptionist
                password:  TestReceiptionist2016
             */



            #region commented bekar code baad m delete krdengy ye
            //UsersService.AddUserType("Admin");
            //UsersService.AddUserType("Doctor");
            //UsersService.AddUserType("Receptionist");
            //UsersService.AddUserType("Technician");

            //BranchesService.AddBranch("branch1name", "branch 1 address", "Br Code");

            //UsersService.AddUser();
            //   var t = UsersService.GetAllUserTypes();
            //  var a = BranchesService.GetAllBranches();

            // UsersService.AddUser();
            //var t = UsersService.GetAllUserTypes();
            // var a = BranchesService.GetAllBranches();
            #endregion

            string username = LoginForm["Email"].ToString();
            string password = LoginForm["Password"].ToString();

            var model = UsersService.ValidateLogin(username, password);

            if (model == null)
            {
                TempData["Error"] = "Username or Password is Incorrect";
                return RedirectToAction("LoginPage", "Main");
            }


            if (model != null)
            {
                Session["BranchId"] = model.BranchId;
                Session["loginuser"] = model.Id;
                Session["loginusername"] = model.Name;
                Session["name"] = model.Name;
                Session["role"] = model.UserRole;

                if (model.UserRole == "Admin")
                {
                    Session["AdminLabelShow"] = model.Name;
                }

                Session["branch"] = model.BranchName;

                return View(model.UserRole + "Dashboard", model);
            }


            return RedirectToAction("LoginPage");
        }

        public ActionResult GetRevenue(FormCollection Revenue)
        {
            #region revenues

            string startDate = Revenue["Sdate"];
            string endDate = Revenue["Edate"];
            TempData["StartDate"] = startDate;
            TempData["EndDate"] = endDate;

            var PayLiaquatabadLab = PatientsPaymentService.GetTotalNetAmountByBranchId("Lab", 1, startDate, endDate);
            TempData["PayLiaquatabadLab"] = PayLiaquatabadLab;

            var PayLiaquatabadXRAY = PatientsPaymentService.GetTotalNetAmountByBranchId("X-RAY", 1, startDate, endDate);
            TempData["PayLiaquatabadXRAY"] = PayLiaquatabadXRAY;

            //var PayLiaquatabadUltra = PatientsPaymentService.GetTotalNetAmountByBranchId("Ultrasound", 1);
            //TempData["PayLiaquatabadUltra"] = PayLiaquatabadUltra;

            var PaySurjaniLab = PatientsPaymentService.GetTotalNetAmountByBranchId("Lab", 2, startDate, endDate);
            TempData["PaySurjaniLab"] = PaySurjaniLab;

            var PaySurjaniXRAY = PatientsPaymentService.GetTotalNetAmountByBranchId("X-RAY", 2, startDate, endDate);
            TempData["PaySurjaniXRAY"] = PaySurjaniXRAY;

            //var PaySurjaniUltra = PatientsPaymentService.GetTotalNetAmountByBranchId("Ultrasound", 2);
            //TempData["PaySurjaniUltra"] = PaySurjaniUltra;

            var PayGardenLab = PatientsPaymentService.GetTotalNetAmountByBranchId("Lab", 3, startDate, endDate);
            TempData["PayGardenLab"] = PayGardenLab;

            //var PayGardenXRAY = PatientsPaymentService.GetTotalNetAmountByBranchId("X-RAY", 3);
            //TempData["PayGardenXRAY"] = PayGardenXRAY;

            //var PayGardenUltra = PatientsPaymentService.GetTotalNetAmountByBranchId("Ultrasound", 3);
            //TempData["PayGardenUltra"] = PayGardenUltra;

            var PayNorthLab = PatientsPaymentService.GetTotalNetAmountByBranchId("Lab", 4, startDate, endDate);
            TempData["PayNorthLab"] = PayNorthLab;

            //var PayNorthXRAY = PatientsPaymentService.GetTotalNetAmountByBranchId("X-RAY", 4);
            //TempData["PayNorthXRAY"] = PayNorthXRAY;

            //var PayNorthUltra = PatientsPaymentService.GetTotalNetAmountByBranchId("Ultrasound", 4);
            //TempData["PayNorthUltra"] = PayNorthUltra;

            var PayJoharLab = PatientsPaymentService.GetTotalNetAmountByBranchId("Lab", 6, startDate, endDate);
            TempData["PayJoharLab"] = PayJoharLab;

            //var PayJoharXRAY = PatientsPaymentService.GetTotalNetAmountByBranchId("X-RAY", 6);
            //TempData["PayJoharXRAY"] = PayJoharXRAY;

            //var PayJoharUltra = PatientsPaymentService.GetTotalNetAmountByBranchId("Ultrasound", 6);
            //TempData["PayJoharUltra"] = PayJoharUltra;

            //=========================================////////////////////////////////////////////////////////////////==================================================================

            var TotalPayJoh = PatientsPaymentService.GetTotalPayment(6, startDate, endDate);
            TempData["TotalPayJohar"] = TotalPayJoh;

            var TotalPayLia = PatientsPaymentService.GetTotalPayment(1, startDate, endDate);
            TempData["TotalPayLiaquatabad"] = TotalPayLia;

            var TotalPayNo = PatientsPaymentService.GetTotalPayment(4, startDate, endDate);
            TempData["TotalPayNorth"] = TotalPayNo;

            var TotalPaySur = PatientsPaymentService.GetTotalPayment(2, startDate, endDate);
            TempData["TotalPaySurjani"] = TotalPaySur;

            var TotalPayGar = PatientsPaymentService.GetTotalPayment(3, startDate, endDate);
            TempData["TotalPayGarden"] = TotalPayGar;


            #endregion
            return RedirectToAction("AdminDashboard", "Main");
        }


        public ActionResult LogOut()
        {
            Session["loginuser"] = null;
            Session["loginusername"] = null;
            Session["AdminLabelShow"] = null;
            return RedirectToAction("LoginPage");
        }

    }
}
