using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using SampleDomain;

namespace SampleRepository
{
    public class ClinicRepository
    {
        private readonly IUnitOfWork unitOfWork;

        public ClinicRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Clinic GetClinicsData(String id)
        {
            return this.unitOfWork.Session.Query<Clinic>()
                .Where(x => x.clinic_code.Equals(id))
                .ToList()
                .FirstOrDefault();
        }
    }
}
