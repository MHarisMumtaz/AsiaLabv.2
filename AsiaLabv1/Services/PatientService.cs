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
        Repository<Patient> _PatientRepository = new Repository<Patient>();
        Repository<PatientTest> _PatientTestRepository = new Repository<PatientTest>();
        Repository<PatientTestResult> _PatientTestResultRepository = new Repository<PatientTestResult>();

        Repository<PatientRefer> _PatientReferRepository = new Repository<PatientRefer>();

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
                Days = "", //days and months are empty becuase humam didn't created those fields on ui
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
    }
}