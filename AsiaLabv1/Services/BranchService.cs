using AsiaLabv1.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Services
{
    public class BranchService
    {
        Repository<Branch> Branches = new Repository<Branch>();
        Repository<Contact> BranchesContacts = new Repository<Contact>();

        public void AddBranch(string BranchName, string BranchAddress, string BranchCode)
        {
            var branch = new Branch()
            {
                BranchName = BranchName,
                BranchAddress = BranchAddress,
                BranchCode = BranchCode
            };
            Branches.Insert(branch);
            AddBranchContact(branch.Id, "021-4567891");
        }

        //this method can be used to fill available branches dropdown on Registration form
        public List<Branch> GetAllBranches()
        {
           return Branches.GetAll();
        }

        public void AddBranchContact(int BranchId, string ContactNumber)
        {
            BranchesContacts.Insert(new Contact
            {
                BranchId = BranchId,
                BranchContactNo = ContactNumber
            });
        }

        public Branch GetById(int Id)
        {
            return Branches.GetById(Id);
        }

        public Contact GetBranchContact(int BranchId)
        {
            var Query = (from contact in BranchesContacts.Table
                         where contact.BranchId == BranchId
                         select contact).FirstOrDefault();
            return Query;
        }
    }
}