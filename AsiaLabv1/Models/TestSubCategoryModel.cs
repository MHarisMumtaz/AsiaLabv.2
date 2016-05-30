using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AsiaLabv1.Models
{
    public class TestSubCategoryModel
    {
        public int Id { get; set; }
        public string TestSubcategoryName { get; set; }
        public float UpperBound { get; set; }
        public float LowerBound { get; set; }
        public string Unit { get; set; }
        public double Rate { get; set; }
        public int TestCategoryId { get; set; }


    }

    public class Tests
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}