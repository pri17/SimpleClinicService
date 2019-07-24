using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using SampleDomain;
using SampleRepository;

namespace SampleBusiness
{
    public class SpecialtyBusiness
    {
        private readonly IUnitOfWork unitOfWork;

        public SpecialtyBusiness(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Specialty GetSpecialtyData(string id)
        {
            return new SpecialtyRepository(this.unitOfWork).GetSpecialtyData(id);
        }

        public List<Specialty> getAllSpecialty()
        {
            return new SpecialtyRepository(this.unitOfWork).getAllSpecialty();
        }
    }
}
