using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using NHibernate.Linq;
using SampleDomain;

namespace SampleRepository
{
    public class ClinicSpecialtyRepository
    {
         private readonly IUnitOfWork unitOfWork;

         public ClinicSpecialtyRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<ClinicSpecialty> Getlist(String specialtyId)
        {

            return this.unitOfWork.Session.Query<ClinicSpecialty>()
                .Where(x => x.specialty.specialty_code.Equals(specialtyId))
                .Fetch(x => x.clinic)
                .Fetch(x =>x.specialty)
                .ToList();
        }
    }
}
