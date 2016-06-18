using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Models
{
    public class PatientSearchModel
    {
        public int PatientId { get; set; }
        public string Name { get; set; }
        public bool ShowGeneratedReportPatients { get; set; }
    }
}