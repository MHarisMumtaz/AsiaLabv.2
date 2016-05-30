﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AsiaLabv1.Repositories;


namespace AsiaLabv1.Services
{
    public class TestSubCategoryService
    {

        Repository<TestSubcategory> _TestSubCatgeroryRepository = new Repository<TestSubcategory>();
        Repository<TestCategory> _TestCatgeroryRepository = new Repository<TestCategory>();

        public void Add(TestSubcategory TestCatg)
        {
            _TestSubCatgeroryRepository.Insert(TestCatg);
        }

        public void Delete(int TestCatg)
        {
            _TestSubCatgeroryRepository.DeleteById(TestCatg);
        }
        public void Update(TestSubcategory TestCatg,int id)
        {
            _TestSubCatgeroryRepository.Update(TestCatg,id);
        }

        public TestSubcategory getById(int id)
        {
           return _TestSubCatgeroryRepository.GetById(id);
        }
        
        public List<TestSubcategory> GetSubCategTestsByTestCategoryId(int TestCategId)
        {

            var Query = (from testSub in _TestSubCatgeroryRepository.Table
                         join testCateg in _TestCatgeroryRepository.Table on testSub.TestCategoryId equals testCateg.Id
                         where testCateg.Id == TestCategId
                         select testSub).ToList<TestSubcategory>();
            return Query;
        }
       
    }
}