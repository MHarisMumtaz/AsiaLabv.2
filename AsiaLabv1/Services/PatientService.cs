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
    }
}