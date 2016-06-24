using AsiaLabv1.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Services
{
    public class PatientPaymentService
    {
        Repository<Payment> _PaymentRepository = new Repository<Payment>();
        Repository<PayType> _PaytypeRepository = new Repository<PayType>();
        Repository<Patient> _PatientRepository = new Repository<Patient>();
        Repository<Branch> _BranchRepository = new Repository<Branch>();

        Repository<TestSubcategory> _TestSubCategoryRepository = new Repository<TestSubcategory>();
        Repository<TestCategory> _TestCategoryRepository = new Repository<TestCategory>();
        Repository<TestDepartment> _TestDeptRepository = new Repository<TestDepartment>();
        Repository<PatientTest> _PatientTestRepository = new Repository<PatientTest>();


        public void Add(Payment PatientPaymentInfo)
        {
            _PaymentRepository.Insert(PatientPaymentInfo);
        }

        public void AddPayType(PayType Paytype)
        {
            _PaytypeRepository.Insert(Paytype);
        }

        public List<PayType> GetAllPayTypes()
        {
            return _PaytypeRepository.GetAll();
        }

        public double GetTotalNetAmountByBranchId(string deptname, int branchid,string start,string end)
        {

            var query = (from p in _PatientRepository.Table
                         join br in _BranchRepository.Table on p.BranchId equals br.Id
                         join pt in _PatientTestRepository.Table on p.Id equals pt.PatientId
                         join pay in _PaymentRepository.Table on p.Id equals pay.PatientId
                         where (pt.TestSubcategory.TestCategory.TestDepartment.DepartmentName == deptname && br.Id == branchid)
                         select p.Id).ToList();

            //var query2 = (from p in _PatientRepository.Table
            //             join br in _BranchRepository.Table on p.BranchId equals br.Id
            //             join pt in _PatientTestRepository.Table on p.Id equals pt.PatientId
            //             join pay in _PaymentRepository.Table on p.Id equals pay.PatientId
            //             where (pt.TestSubcategory.TestCategory.TestDepartment.DepartmentName == deptname && br.Id == branchid)
            //             select p.DateTime).ToList();

            //           var start = new DateTime(2016, 6, 22);
            //var end = new DateTime(2016, 6, 22);

           
            var DT1 = DateTime.Parse(start);
            var DT2 = DateTime.Parse(end);
            var checkquery = (from p in _PaymentRepository.Table

                              where query.Contains(p.PatientId) && (p.Patient.DateTime >= DT1 && p.Patient.DateTime < DT2)
                              select p.NetAmount).Sum();
            return checkquery;
        }
    }
}