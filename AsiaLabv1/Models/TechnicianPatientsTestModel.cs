using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Models
{
    public class TechnicianPatientsTestModel
    {
        public int Id { get; set; }
        public int TechnicianId { get; set; }
        public DateTime Dated { get; set; }
        public int PatientId { get; set; }
    }
}