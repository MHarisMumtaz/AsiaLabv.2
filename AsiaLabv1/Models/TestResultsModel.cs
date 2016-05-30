using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Models
{
    public class TestResultsModel
    {
        public int Id { get; set; }
        public int PatientTestId { get; set; }
        public string Result { get; set; }
        public string ApprovalStatus { get; set; }
        public string Remarks { get; set; }
    }
}