using AsiaLabv1.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Services
{
    public class GenderService
    {
        Repository<Gender> Genders = new Repository<Gender>();

        public List<Gender> GetAll()
        {
            return Genders.GetAll();
        }
        public void AddGender(Gender Gender)
        {
            Genders.Insert(Gender);
        }

        public Gender GetById(int Id)
        {
            return Genders.GetById(Id);
        }
    }
}