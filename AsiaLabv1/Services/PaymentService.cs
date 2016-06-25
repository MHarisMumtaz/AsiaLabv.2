using AsiaLabv1.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Services
{
    public class PaymentService
    {
        Repository<Payment> _PaymentRepository = new Repository<Payment>();

        public Payment GetPaymentByPatientId(int PatientId)
        {
            var Query = (from pay in _PaymentRepository.Table
                         where pay.PatientId == PatientId
                         select pay).FirstOrDefault();
            return Query; 
        }
    }
}