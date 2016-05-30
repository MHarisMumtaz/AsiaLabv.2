using AsiaLabv1.Models;
using AsiaLabv1.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Services
{
    public class ReferDoctorsService
    {
        Repository<Refer> _ReferRepository = new Repository<Refer>();

        public void Add(Refer ReferDoctor)
        {
            _ReferRepository.Insert(ReferDoctor);
        }

        public List<Refer> GetAllReferDoctors()
        {
            return _ReferRepository.GetAll();
        }

        public Refer GetReferDoctorById(int Id)
        {
            return _ReferRepository.GetById(Id);
        }

        public void Update(Refer Doc, int id)
        {
            _ReferRepository.Update(Doc, id);
        }

        public void Delete(int Doc)
        {
            _ReferRepository.DeleteById(Doc);
        }
    }
}