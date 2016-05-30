using AsiaLabv1.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Services
{
    public class TestDeptService
    {
        Repository<TestDepartment> _TestDeptRepository = new Repository<TestDepartment>();
        Repository<TestDepartment> _TestSubDeptRepository = new Repository<TestDepartment>();

        public void Add(TestDepartment TestDept)
        {
            _TestDeptRepository.Insert(TestDept);

        }

        public List<TestDepartment> GetAllDept()
        {
            return _TestDeptRepository.GetAll();
        }

       
        public List<TestDepartment> GetAllDeptSD()
        {
            return _TestSubDeptRepository.GetAll();
        }
    }
}