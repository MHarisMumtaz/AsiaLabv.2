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
            int PatientAge = DateTime.Now.Year - model.DateofBirth.Year;
            var patient = new Patient()
            {
                PatientName = model.Name,
                PatientNumber = model.PhoneNumber,
                BranchId = model.BranchId,
                GenderId = model.GenderId,
                DateTime = DateTime.Now,
                Email = model.Email,
                Age = PatientAge.ToString(),
                Days = "", //days and months are empty becuase humam didn't created those fields in ui
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

        public List<Patient> GetPatientsByBranchId(int BranchId)
        {
            var Query = (from P in _PatientRepository.Table
                         where P.BranchId == BranchId
                         select P).ToList();
            return Query;
        }
      
        public List<Patient> SearchPatient(int BranchId,string name)
        {
            var Query = (from P in _PatientRepository.Table
                         where P.BranchId == BranchId && P.PatientName==name
                         select P).ToList();
            return Query;
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