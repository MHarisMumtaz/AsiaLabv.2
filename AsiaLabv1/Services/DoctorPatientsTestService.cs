using AsiaLabv1.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Services
{
    public class DoctorPatientsTestService
    {
        Repository<DoctorPatientsTest> _DoctorPatientsTestRepository = new Repository<DoctorPatientsTest>();
        Repository<UserEmployee> _UserRepository = new Repository<UserEmployee>();

        public UserEmployee GetDoctorByPatientId(int PatienId)
        {
            var Query = (from PatientDoctor in _DoctorPatientsTestRepository.Table
                         join user in _UserRepository.Table on PatientDoctor.DoctorId equals user.Id
                         where PatientDoctor.PatientId == PatienId
                         select user).FirstOrDefault();
            return Query;
        }
    }
}