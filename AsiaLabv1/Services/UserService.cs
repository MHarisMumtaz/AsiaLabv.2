using AsiaLabv1.Models;
using AsiaLabv1.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Services
{
    public class UserService
    {
        //these repositories are used to access database tables
        //some common CRUD of database methods have the repository are insert,update,delete,deletebyid,deleteall
        Repository<UserEmployee> UserEmp = new Repository<UserEmployee>();
        Repository<UserType> UserTypes = new Repository<UserType>();
        Repository<Address> UserAddresses = new Repository<Address>();
        Repository<Branch> Branches = new Repository<Branch>();
        Repository<Gender> Genders = new Repository<Gender>();


        public List<UserEmployee> GetAllEmployees()
        {
            return UserEmp.GetAll();
        }

        public List<Address> GetAddress()
        {
            return UserAddresses.GetAll();
        }

        public void Update(UserEmployee Emp, int id)
        {
            UserEmp.Update(Emp, id);
        }


        public List<UserEmployee> GetAllEmp()
        {
            var Query = (from user in UserEmp.Table
                         join branch in Branches.Table on user.BranchId equals branch.Id
                         join addr in UserAddresses.Table on user.Id equals addr.UserEmployeeId
                         join usertype in UserTypes.Table on addr.UserTypeId equals usertype.Id
                         select user).ToList<UserEmployee>();  
            return Query;
        }             
       
        public UserModel ValidateLogin(string UserName, string Password)
        {
            var Query = (from user in UserEmp.Table
                         join branch in Branches.Table on user.BranchId equals branch.Id
                         join addr in UserAddresses.Table on user.Id equals addr.UserEmployeeId
                         join usertype in UserTypes.Table on addr.UserTypeId equals usertype.Id
                         where (user.Username == UserName && user.Password == Password)
                         select new
                         {                             
                             UserId = user.Id,
                             UserName = user.Name,
                             BranchName = branch.BranchName,
                             BranchAddress = branch.BranchAddress,
                             UserRole = usertype.TypeDescription
                             
                             
                         }).FirstOrDefault();
            var model = new UserModel();
            if (Query != null)
            {
                model.Id = Query.UserId;
                model.Name = Query.UserName;
                model.BranchName = Query.BranchName;
                model.BranchAddress = Query.BranchAddress;
                model.UserRole = Query.UserRole;
                return model;
            }
            return null;
        }
        #region if you want to add user manually then folow these steps
        //use this register method for every user registeration
        //example
        // UserEmployee emp=new UserEmployee()
        //{
        //    Name = "Hassan Reciptionist",
        //    Username = "testtechnician",
        //    Password = "techniciantest2016",
        //    BranchId = 1                          //Note:: branchId should be get from Form Ui
        //};
        // Address addr = new Address()
        //{
        //    UserTypeId = 4,                       //Note:: userTypeId should be obtained from UI Form
        //    ContactNo = "0345-6789654",
        //    Email = "test@gmail.com",
        //    GenderId = 1,                         //Note:: GenderId should be obtained from UI Form
        //    Qualification = "MBA",
        //    AddressDetail = "H block test address karachi",
        //    CNIC = "42101-test"
        //});
        #endregion

        public void RegisterUser(UserEmployee emp,Address empAddr)
        {
            UserEmp.Insert(emp);
            empAddr.UserEmployeeId = emp.Id;
            UserAddresses.Insert(empAddr);
        }

        public void AddUser()
        {
            var useremp = new UserEmployee()
            {
                Name="Test Receiptionist",
                Username = "TestReceiptionist",
                Password = "TestReceiptionist2016",
                BranchId = 1
            };
           
            UserEmp.Insert(useremp);
            UserAddresses.Insert(new Address
            {
                UserEmployeeId = useremp.Id,
                UserTypeId = 3,
                ContactNo = "0300-689654",
                Email = "TestReceiptionist@gmail.com",
                GenderId = 1,
                Qualification = "MBA",
                AddressDetail = "H block test address karachi",
                CNIC = "42101-testreceiptionist"
            });

        }

        public void AddUserType(string UserRole)
        {
            UserTypes.Insert(new UserType
            {
                TypeDescription = UserRole
            });
        }

        public List<UserType> GetAllUserTypes()
        {
            return UserTypes.GetAll();
        }

        public Branch GetUserBranch(int UserId)
        {
            var Query = (from u in UserEmp.Table
                         join br in Branches.Table on u.BranchId equals br.Id
                         where u.Id == UserId
                         select br).FirstOrDefault();
            return Query;
        }
    }
}