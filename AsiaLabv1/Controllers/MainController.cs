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

        public ActionResult LoginPage()
        {
            ViewBag.LoginFailed = false;
            return View("LoginPage");
        }

        public ActionResult Khaali()
        {
            return View();
        }

        public ActionResult EditProfile()
        {
            return View();
        }

        public ActionResult AdminDashboard()
        {
            return View();
        }
        public ActionResult DoctorDashboard()
        {
            return View();
        }
        public ActionResult ReceptionistDashboard()
        {
            return View();
        }
        public ActionResult TechnicianDashboard()
        {
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

            if (model==null)
            {
                ViewBag.LoginFailed = true;
                return RedirectToAction("LoginPage", "Main");

            }
            

            if (model != null)
            {
                Session["loginuser"] = model.Id;
                Session["loginusername"] = model.Name;
                Session["name"] = model.Name;
                Session["role"] = model.UserRole;

                if (model.Name == "Humam admin")
                {
                    Session["AdminLabelShow"] = model.Name;
                }
                
                Session["branch"] = model.BranchName;
                return View(model.UserRole + "Dashboard", model);
            }

            return RedirectToAction("LoginPage");
        }

        public ActionResult abc()
        {
            return View();
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
