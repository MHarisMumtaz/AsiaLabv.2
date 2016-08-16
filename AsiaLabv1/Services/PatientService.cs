using AsiaLabv1.Models;
using AsiaLabv1.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Services
{
    public class PatientService
    {
        Repository<TestDepartment> _TestDeptRepository = new Repository<TestDepartment>();
        Repository<Patient> _PatientRepository = new Repository<Patient>();
        Repository<PatientTest> _PatientTestRepository = new Repository<PatientTest>();
        Repository<PatientTestResult> _PatientTestResultRepository = new Repository<PatientTestResult>();
        Repository<DoctorPatientsTest> _DoctorPatientsTestRepository = new Repository<DoctorPatientsTest>();
        Repository<PatientRefer> _PatientReferRepository = new Repository<PatientRefer>();
        Repository<TestCategory> _TestCategoryRepository = new Repository<TestCategory>();
        Repository<TestSubcategory> _TestSubCategoryRepository = new Repository<TestSubcategory>();
        Repository<Payment> _PaymentRepository = new Repository<Payment>();
        Repository<Refer> _ReferRepository = new Repository<Refer>();

        public void Add(PatientModel model)
        {
            
            var patient = new Patient()
            {
                PatientName = model.Name,
                PatientNumber = model.PhoneNumber,
                BranchId = model.BranchId,
                GenderId = model.GenderId,
                DateTime = DateTime.Now,
                Email = model.Email,
                Age = model.Age,
                Days = "", //days and months are empty because humam didn't created those fields on ui
                Months = ""
            };
            
            _PatientRepository.Insert(patient);
            model.Id = patient.Id;
            if (model.ReferredId!=-1)
            {
                _PatientReferRepository.Insert(new PatientRefer
                {
                    PatientId = patient.Id,
                    ReferId = model.ReferredId
                });
            }
        }

        public Patient GetByPatientId(int Id)
        {
            var Query = (from P in _PatientRepository.Table
                         where P.Id == Id
                         select P).FirstOrDefault();
            return Query;
        }

        public List<Patient> GetPatientsByBranchId(int BranchId)
        {
            var Query = (from P in _PatientRepository.Table
                         where P.BranchId == BranchId
                         select P).ToList();
            return Query;
        }
      
        public List<PatientModel> SearchPatient(int BranchId)
        {
            var Query = (from P in _PatientRepository.Table
                         join Ptest in _PatientTestRepository.Table on P.Id equals Ptest.PatientId
                         join Presult in _PatientTestResultRepository.Table on Ptest.Id equals Presult.PatientTestId
                         where (P.BranchId == BranchId)
                         select new
                         {
                             Id = P.Id,
                             PatientName = P.PatientName,
                             Status = Presult.ApprovalStatus,
                             RegisterDate = P.DateTime
                         }).Distinct().ToList();
            var list = new List<PatientModel>();

            foreach (var item in Query)
            {
                list.Add(new PatientModel
                {
                    Id = item.Id,
                    Name = item.PatientName,
                    Status = item.Status,
                    Date = item.RegisterDate
                });
            }
            return list;
        }

        public List<Patient> SearchByStatus(int BranchId)
        {
            var Query = (from P in _PatientRepository.Table
                         join Ptest in _PatientTestRepository.Table on P.Id equals Ptest.PatientId
                         join Presult in _PatientTestResultRepository.Table on Ptest.Id equals Presult.PatientTestId
                         where (P.BranchId == BranchId && Presult.ApprovalStatus == "Approved")
                         select P).ToList();

            return Query;
        }

        public List<CashSummaryModel> GetPatientsSummary(DateTime date, int BranchId)
        {
            var Query = (from Patients in _PatientRepository.Table
                         join PT in _PatientTestRepository.Table on Patients.Id equals PT.PatientId
                         join TestSub in _TestSubCategoryRepository.Table on PT.TestSubcategoryId equals TestSub.Id
                         join TestCat in _TestCategoryRepository.Table on TestSub.TestCategoryId equals TestCat.Id
                         join Dept in _TestDeptRepository.Table on TestCat.TestDepartmentId equals Dept.Id
                         join Pay in _PaymentRepository.Table on Patients.Id equals Pay.PatientId
                         join refer in _PatientReferRepository.Table on Patients.Id equals refer.PatientId
                         join doctor in _ReferRepository.Table on refer.ReferId equals doctor.Id
                         where (Patients.BranchId == BranchId && (Patients.DateTime.Year == date.Year && Patients.DateTime.Day == date.Day && Patients.DateTime.Month == date.Month))
                         group new { Patients, Dept, Pay, doctor, TestSub } by new { Patients.Id } into items
                         select new
                         {
                             SlipNo = items.Key.Id,
                             brancid = items.Select(i => i.Patients.BranchId).FirstOrDefault(),
                             Time = items.Select(i => i.Patients.DateTime).FirstOrDefault(),
                             PatientName = items.Select(i => i.Patients.PatientName).FirstOrDefault(),
                             Doctor = items.Select(i => i.doctor.ReferredDoctorName).FirstOrDefault(),
                             ChargeDetails = items.Select(i => i.Dept.DepartmentName).FirstOrDefault(),
                             Dis = items.Select(i => i.Pay.Discount).FirstOrDefault(),
                             Net = items.Select(i => i.Pay.NetAmount).FirstOrDefault(),
                             Rec = items.Select(i => i.Pay.PaidAmount).FirstOrDefault(),
                             Due = items.Select(i => i.Pay.NetAmount).FirstOrDefault() - items.Select(i => i.Pay.PaidAmount).FirstOrDefault()
                         }).ToList();
            var List = new List<CashSummaryModel>();
            //var temp = Query.Where(i => i.Time == date).ToList();
            foreach (var item in Query)
            {
                List.Add(new CashSummaryModel
                {
                    SlipNo = item.SlipNo,
                    Time = item.Time,
                    PatientName = item.PatientName,
                    Doctor = item.Doctor,
                    ChargeDetails = item.ChargeDetails,
                    Bill = item.Net + item.Dis,
                    Dis = item.Dis,
                    Net = item.Net,
                    Rec = item.Rec,
                    Due = item.Due
                });
            }
            return List;

        }

    }
}

////////////////////////////
 //public List<CashSummaryModel> GetPatientsSummary(DateTime date, int BranchId)
 //       {
 //           var Query = (from Patients in _PatientRepository.Table
 //                        join PT in _PatientTestRepository.Table on Patients.Id equals PT.PatientId
 //                        join TestSub in _TestSubCategoryRepository.Table on PT.TestSubcategoryId equals TestSub.Id
 //                        join TestCat in _TestCategoryRepository.Table on TestSub.TestCategoryId equals TestCat.Id
 //                        join Dept in _TestDeptRepository.Table on TestCat.TestDepartmentId equals Dept.Id
 //                        join Pay in _PaymentRepository.Table on Patients.Id equals Pay.PatientId
 //                        join refer in _PatientReferRepository.Table on Patients.Id equals refer.PatientId
 //                        join doctor in _ReferRepository.Table on refer.ReferId equals doctor.Id
 //                        where (Patients.BranchId == BranchId && (Patients.DateTime.Year == date.Year && Patients.DateTime.Day == date.Day && Patients.DateTime.Month == date.Month))
 //                        group new { Patients, Dept, Pay, doctor, TestSub } by new { Patients.Id } into items
 //                        select new
 //                        {
 //                            SlipNo = items.Key.Id,
 //                            brancid = items.Select(i => i.Patients.BranchId).FirstOrDefault(),
 //                            Time = items.Select(i => i.Patients.DateTime).FirstOrDefault(),
 //                            PatientName = items.Select(i => i.Patients.PatientName).FirstOrDefault(),
 //                            Doctor = items.Select(i => i.doctor.ReferredDoctorName).FirstOrDefault(),
 //                            ChargeDetails = items.Select(i => i.Dept.DepartmentName).FirstOrDefault(),
 //                            Dis = items.Select(i => i.Pay.Discount).FirstOrDefault(),
 //                            Net = items.Select(i => i.Pay.NetAmount).FirstOrDefault(),
 //                            Rec = items.Select(i => i.Pay.PaidAmount).FirstOrDefault(),
 //                            Due = items.Select(i => i.Pay.NetAmount).FirstOrDefault() - items.Select(i => i.Pay.PaidAmount).FirstOrDefault()
 //                        }).ToList();
 //           var List = new List<CashSummaryModel>();
 //           //var temp = Query.Where(i => i.Time == date).ToList();
 //           foreach (var item in Query)
 //           {
 //               List.Add(new CashSummaryModel
 //               {
 //                   SlipNo = item.SlipNo,
 //                   Time = item.Time,
 //                   PatientName = item.PatientName,
 //                   Doctor = item.Doctor,
 //                   ChargeDetails = item.ChargeDetails,
 //                   Bill = item.Net+item.Dis,
 //                   Dis = item.Dis,
 //                   Net = item.Net,
 //                   Rec = item.Rec,
 //                   Due = item.Due
 //               });
 //           }
 //           return List;

 //       }