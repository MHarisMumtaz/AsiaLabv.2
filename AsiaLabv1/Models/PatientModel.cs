using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AsiaLabv1.Models
{
    public class PatientModel
    {
        [Display(Name = "Patient Number")]
        public int Id { get; set; }

        [Display(Name = "Patient Name")]
        public string Name { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime DateofBirth { get; set; }

        [Display(Name = "Contact Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        public List<int> PatientTestIds { get; set; }

        [Display(Name = "Refer By")]
        public int ReferredId { get; set; }
        public List<SelectListItem> ReferredDoctors { get; set; }

        [Display(Name = "Gender")]
        public int GenderId { get; set; }
        public List<SelectListItem> Genders { get; set; }

        public int DeptId { get; set; }
        public List<SelectListItem> Departments { get; set; }

        public int PayId { get; set; }
        public List<SelectListItem> PayTypes { get; set; }


        [Display(Name="Discount")]
        public double Discount { get; set; }

        [Display(Name="Paid Amount")]
        public double PaidAmount { get; set; }

        public PatientModel()
        {
            this.PayTypes = new List<SelectListItem>();
            this.PatientTestIds = new List<int>();
            this.ReferredDoctors = new List<SelectListItem>();
            this.Genders = new List<SelectListItem>();
            this.Departments = new List<SelectListItem>();
        }
    }
}