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
    public class ClinicBusiness
    {
         private readonly IUnitOfWork unitOfWork;

         public ClinicBusiness(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Clinic GetClinicData(string id)
        {
            return new ClinicRepository(this.unitOfWork).GetClinicsData(id);
        }

        public List<Clinic> GetClinicListWithSpecialty(string sId)
        {
            Specialty s = new SpecialtyRepository(this.unitOfWork).GetSpecialtyData(sId);
            List< ClinicSpecialty> csList = new ClinicSpecialtyRepository(this.unitOfWork).Getlist(sId);
            List<Clinic> clinicList = new List<Clinic>();
            foreach (var cid in csList)
            {
                Clinic c = cid.clinic;
                clinicList.Add(c);
            }
            return clinicList;
        }
    }
}
