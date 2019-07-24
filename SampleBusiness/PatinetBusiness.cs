using System;
using System.Collections.Generic;

namespace SampleBusiness
{
    using Core;
    using SampleDomain;
    using SampleRepository;

    public class PatientBusiness
    {
        private readonly IUnitOfWork unitOfWork;

        public PatientBusiness(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Patient GetPatientData(int id)
        {
            return new PatientRepository(this.unitOfWork).GetPatientsData(id);
        }

        public List<Patient> getAllPatients()
        {
            return new PatientRepository(this.unitOfWork).getAllPatients();
        }
    }

}