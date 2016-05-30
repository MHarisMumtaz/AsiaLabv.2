using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Models
{
    public class ReferredModel
    {
        public int Id { get; set; }

        [Display(Name="Referral Name")]
        public string ReferredDoctorName { get; set; }

        [Display(Name = "Address")]
        public string ReferredDocAddress { get; set; }

        [Display(Name = "Number")]
        public string ReferredDocNumber { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Commission")]
        public Nullable<double> commission { get; set; }
    }

}