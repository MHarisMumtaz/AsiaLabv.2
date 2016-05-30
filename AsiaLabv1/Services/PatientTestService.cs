using AsiaLabv1.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Services
{
    public class PatientTestService
    {

        Repository<PatientTest> _PatientTestRepository = new Repository<PatientTest>();
        Repository<TestSubcategory> _TestSubCategoryRepository = new Repository<TestSubcategory>();
        Repository<Patient> _PatientRepository = new Repository<Patient>();
        Repository<PatientTestResult> _PatientTestResultRepository= new Repository<PatientTestResult>();
        Repository<TechnicianPatientsTest> _TechnicianPatientTestRepository = new Repository<TechnicianPatientsTest>();
        Repository<DoctorPatientsTest> _DoctorsPatientsTestsRepository = new Repository<DoctorPatientsTest>();
        Repository<DoctorComment> _DoctorCommentsRepository = new Repository<DoctorComment>();

        public void Add(PatientTest Patienttest)
        {
            _PatientTestRepository.Insert(Patienttest);
        }

        public List<Patient> GetPatientTests()
        {
            //var query = (from pt in _PatientTestService.Table
            //             join p in _PatientRepository.Table
            //             on pt.PatientId equals p.Id
            //             where !_PatientTestResultRepository.Table.Any(ptr => ptr.PatientTestId == pt.Id)
            //             select pt).ToList<PatientTest>().GroupBy(test => test.Id).Select(grp => grp.First()).ToList();

            var check2 = (from pt in _PatientTestRepository.Table
                          join tr in _PatientTestResultRepository.Table
                          on pt.Id equals tr.PatientTestId
                          select pt.PatientId).ToList();

            //var check = (from tr in _PatientTestResultRepository.Table
            //             select tr.PatientTestId).ToList();

            var query = (from p in _PatientRepository.Table
                         join pt in _PatientTestRepository.Table
                         on p.Id equals pt.PatientId
                         //where !_PatientTestResultRepository.Table.Any(ptr => ptr.PatientTestId == pt.Id)
                         where !check2.Contains(pt.PatientId)
                         select p).ToList<Patient>().GroupBy(test => test.Id).Select(grp => grp.First()).ToList();

            
               

            return query;
        }

        public List<Patient> GetPatientTestsDoctor()
        {
            var abc=(from ptr in _PatientTestResultRepository.Table
                         where ptr.ApprovalStatus=="Approved"
                         select ptr.PatientTestId).ToList();

            var check2 = (from pt in _PatientTestRepository.Table
                          join tr in _PatientTestResultRepository.Table
                          on pt.Id equals tr.PatientTestId
                          where !abc.Contains(tr.PatientTestId)
                          select pt.PatientId).ToList();

          
            var query = (from p in _PatientRepository.Table
                         join pt in _PatientTestRepository.Table
                         on p.Id equals pt.PatientId
                         where check2.Contains(pt.PatientId)
                         select p).ToList<Patient>().GroupBy(test => test.Id).Select(grp => grp.First()).ToList();
            return query;
        }

        public List<PatientTest> GetPatientTestsById(int id)
        {
            var query = (from pt in _PatientTestRepository.Table
                         join t in _TestSubCategoryRepository.Table
                         on pt.TestSubcategoryId equals t.Id
                         //where !_PatientTestResultRepository.Table.Any(ptr => ptr.PatientTestId == pt.Id)
                         //&& pt.PatientId == id
                         where pt.PatientId == id
                         select pt).ToList<PatientTest>();
            return query;
        }
        
   
        public void InsertPatientTestResults(PatientTestResult model)
        {
            _PatientTestResultRepository.Insert(model);
        }

        public void InsertTechnicianPatientTests(TechnicianPatientsTest model)
        {
            _TechnicianPatientTestRepository.Insert(model);
        }

        public void UpdateTest(int id)
        {
            
            int iid = (from p in _PatientTestRepository.Table
                      where p.PatientId == id
                      select p.Id).FirstOrDefault();

            List<PatientTestResult> original = (from ptr in _PatientTestResultRepository.Table
                                                where ptr.PatientTestId == iid
                                                select ptr).ToList<PatientTestResult>();

            if (original != null)
            {
                foreach (var item in original)
                {
                    item.ApprovalStatus = "Approved";
                    _PatientTestResultRepository.UpdateGeneric(item);
                }
              
            }    
        }

        public void InsertDoctorsPatientsTests(DoctorPatientsTest model)
        {
            _DoctorsPatientsTestsRepository.Insert(model);
        }

        public void InsertDoctorComments(DoctorComment model)
        {
            _DoctorCommentsRepository.Insert(model);
        }

        public List<string> GetTestResultsById(int id)
        {
            var query = (from ptr in _PatientTestResultRepository.Table
                         where ptr.PatientTestId == id
                         select ptr.Result).ToList();
            return query;
        }
    }
}