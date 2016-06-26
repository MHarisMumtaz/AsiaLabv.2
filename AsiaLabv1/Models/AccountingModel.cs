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
        public DateTime date { get; set; }
        public int BranchId { get; set; }
        public List<SelectListItem> Branches { get; set; }

        public AccountingModel()
        {
            this.Branches = new List<SelectListItem>();
        }

    }
}