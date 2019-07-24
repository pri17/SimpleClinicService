using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using SampleDomain;

namespace SampleRepository
{
    public class SpecialtyRepository
    {
        private readonly IUnitOfWork unitOfWork;

        public SpecialtyRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Specialty GetSpecialtyData(String id)
        {
            return this.unitOfWork.Session.Query<Specialty>()
                .Where(x => x.specialty_code.Equals(id))
                .ToList()
                .FirstOrDefault();
        }

        public List<Specialty> getAllSpecialty()
        {
            return this.unitOfWork.Session.Query<Specialty>()
                .ToList();

        }
    }
}
