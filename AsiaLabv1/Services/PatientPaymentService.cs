using AsiaLabv1.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Services
{
    public class PatientPaymentService
    {
        Repository<Payment> _PaymentRepository = new Repository<Payment>();
        Repository<PayType> _PaytypeRepository = new Repository<PayType>();

        public void Add(Payment PatientPaymentInfo)
        {
            _PaymentRepository.Insert(PatientPaymentInfo);
        }

        public void AddPayType(PayType Paytype)
        {
            _PaytypeRepository.Insert(Paytype);
        }

        public List<PayType> GetAllPayTypes()
        {
            return _PaytypeRepository.GetAll();
        }
    }
}