using AsiaLabv1.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Services
{
    public class TestCategoryService
    {
        Repository<TestCategory> _TestCatgeroryRepository = new Repository<TestCategory>();
        Repository<TestDepartment> _TestDeptRepository = new Repository<TestDepartment>();
        Repository<TestSubcategory> _TestSubCategory = new Repository<TestSubcategory>();

        public void Add(TestCategory TestCatg)
        {
            _TestCatgeroryRepository.Insert(TestCatg);
        }
        public List<TestCategory> GetTestCategoryByDeptId(int DeptId)
        {
            var Query = (from testcat in _TestCatgeroryRepository.Table
                         join dept in _TestDeptRepository.Table on testcat.TestDepartmentId equals dept.Id
                         where dept.Id == DeptId
                         select testcat).ToList<TestCategory>();

            return Query;
        }
        public List<TestCategory> GetCatgByDeptId(int DeptId)
        {
            var chkquery =(from testDept in _TestDeptRepository.Table
                           where testDept.DepartmentName == "Testing Dept"
                           select testDept.Id).ToList();

            var check=(from testCat in _TestCatgeroryRepository.Table
                           select testCat.TestName).ToList();

            var Query = (from testCat in _TestCatgeroryRepository.Table
                         join testDept in _TestDeptRepository.Table 
                         on testCat.TestDepartmentId equals testDept.Id
                         where testDept.Id == DeptId
                         select testCat).ToList();
            return Query;
        }

        public List<TestSubcategory> GetSubCategById(int CategId)
        {

            var Query = (from testSub in _TestSubCategory.Table
                         join testCateg in _TestCatgeroryRepository.Table
                         on testSub.TestCategoryId equals testCateg.Id
                         where testCateg.Id == CategId
                         select testSub).ToList();
            return Query;
        }


    }
}