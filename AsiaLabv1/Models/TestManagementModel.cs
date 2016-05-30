using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AsiaLabv1.Models
{
    public class TestManagementModel 
    {
        //Adding Deartments
        public int deptId { get; set; }
        public string deptName { get; set; }
      
        public List<SelectListItem> departments { get; set; }


        public bool IsNewDepartment{get;set;}


        public string testCategoryName { get; set; } //Test SubDepartment
        public int testCategoryId { get; set; }
        public string testName { get; set; }
        public string unit { get; set; }
        public string rate { get; set; }

        public string upperBound { get; set; }
        public string lowerBound { get; set; }
       
        //[Display(Name="Test Category Code")]
        //public string CatgoryCode { get; set; }
  


        public TestManagementModel()
        {
            departments = new List<SelectListItem>();
        }


    }
}