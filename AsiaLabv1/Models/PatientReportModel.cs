using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Models
{
    public class PatientReportModel
    {
        public int DepartmentId { get; set; }
        public int CategoryId { get; set; }
        public int TestSubCategoryId { get; set; }
        public string DepartmentName { get; set; }
        public string TestCategoryName { get; set; }
        public string TestSubCategoryName { get; set; }
        public double LowerBound { get; set; }
        public double UpperBound { get; set; }
        public string Result { get; set; }
        public string Unit { get; set; }
    }
}