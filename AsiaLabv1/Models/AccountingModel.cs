using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AsiaLabv1.Models
{
    public class AccountingModel
    {
        //new model
<<<<<<< HEAD
        public DateTime date { get; set; }        
=======
        public DateTime date { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
>>>>>>> b33a44e1f55a78b0a304975bf806757448dce70f
        public int BranchId { get; set; }
        public List<SelectListItem> Branches { get; set; }

        public AccountingModel()
        {
            this.Branches = new List<SelectListItem>();
        }

    }
}