using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaLabv1
{
    public class RequiredItem
    {
        public int Id { get; set; }
        public string testName { get; set; }
    }

    public class RequiredTest
    {
        public int Id { get; set; }
        public string testName { get; set; }
        public double upperBound { get; set; }
        public double lowerBound { get; set; }
        public string unit { get; set; }
        public double rate { get; set; }
        public string result { get; set; }
        public string comment { get; set; }

    }

    public class RequiredPatient
    {
        public int Id { get; set; }
        public string PatientNumber { get; set; }
        public string PatientName { get; set; }

    }

    public class RequiredTechnicianItems
    {
        public string PatientNumber { get; set; }
        public string PatientName { get; set; }

        public List<RequiredTest> PatientTests { get; set; }
        public List<string> PreviousTests { get; set; }

    } 
    


    
}