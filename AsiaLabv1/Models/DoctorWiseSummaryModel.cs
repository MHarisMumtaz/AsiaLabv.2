using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Models
{
    public class DoctorWiseSummaryModel
    {
        public string DoctorName { get; set; }
        public int PatNo { get; set; }
        public double CommBill { get; set; }
        public double DocCommision { get; set; }
    }
}