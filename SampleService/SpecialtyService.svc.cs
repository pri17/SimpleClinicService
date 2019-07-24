using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Core;
using SampleBusiness;
using SampleDataContracts;
using SampleDomain;

namespace SampleService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SpecialtyService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SpecialtyService.svc or SpecialtyService.svc.cs at the Solution Explorer and start debugging.
    public class SpecialtyService : ISpecialtyService
    {
         private readonly Logger logger;

        public SpecialtyService()
        {
            this.logger = new Logger();
        }

        public SpecialtyContract getSpecialtyData(string id)
        {
            this.logger.Log("BEGIN - get clinic data");

            Specialty p;

            using (var unitOfWork = new UnitOfWork())
            {
                p = new SpecialtyBusiness(unitOfWork).GetSpecialtyData(id);
                unitOfWork.Close();
            }

            var mappedContract = this.mapToDC(p);
            return mappedContract;
        }

        private SpecialtyContract mapToDC(Specialty p)
        {
            return new SpecialtyContract
            {
               desp = p.desp,
               code = p.specialty_code
            };
        }

        public List<SpecialtyContract> getAllSpecialty()
        {
            List<Specialty> plist = new List<Specialty>();
            List<SpecialtyContract> pc = new List<SpecialtyContract>();
            using (var unitOfWork = new UnitOfWork())
            {
                plist = new SpecialtyBusiness(unitOfWork).getAllSpecialty();
                unitOfWork.Close();
            }

            foreach (var pa in plist)
            {
                var temp = mapToDC(pa);
                pc.Add(temp);
            }
            return pc;
        }
    }
   
}
