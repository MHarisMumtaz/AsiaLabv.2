using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Models
{
    public class CashSummaryModel
    {
        //new model
        public int SlipNo { get; set; }
        public DateTime Time { get; set; }
        public string PatientName { get; set; }
        public string Doctor { get; set; }
        public string ChargeDetails { get; set; }
        public double Bill { get; set; }
        public double Dis { get; set; }
        public double Net { get; set; }
        public double Rec { get; set; }
        public double Due { get; set; }
    }
}