using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using NHibernate.Mapping;
using SampleDomain;

namespace SampleRepository
{
    public class PatientRepository
    {
        private readonly IUnitOfWork unitOfWork;

        public PatientRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Patient GetPatientsData(int id)
        {
            return this.unitOfWork.Session.Query<Patient>()
                .Where(x => x.patient_id == id)
                .ToList()
                .FirstOrDefault();
        }

        public List<Patient> getAllPatients()

        {
            return this.unitOfWork.Session.Query<Patient>()
                .ToList();
        }
    }
}
