using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AsiaLabv1.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string UserRole { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string Qualification { get; set; }
        public string AddressDetails { get; set; }
        public string CNIC { get; set; }
        public string Gender { get; set; }

        public int GenderId { get; set; }
        public List<SelectListItem> Genders { get; set; }
        public int UserTypeId { get; set; }
        public List<SelectListItem> UserTypes { get; set; }
        public int BranchId { get; set; }
        public List<SelectListItem> Branches { get; set; }

        public UserModel()
        {
            Branches = new List<SelectListItem>();
            UserTypes = new List<SelectListItem>();
            Genders = new List<SelectListItem>();
        }

    }
}