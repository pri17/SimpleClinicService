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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ClinicService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ClinicService.svc or ClinicService.svc.cs at the Solution Explorer and start debugging.
    public class ClinicService : IClinicService
    {
        private readonly Logger logger;

        public ClinicService()
        {
            this.logger = new Logger();
        }

        public ClinicContract getclinicData(string id)
        {
            this.logger.Log("BEGIN - get clinic data");

            Clinic p;

            using (var unitOfWork = new UnitOfWork())
            {
                p = new ClinicBusiness(unitOfWork).GetClinicData(id);
                unitOfWork.Close();
            }

            var mappedContract = this.mapToDC(p);
            return mappedContract;
        }

        private ClinicContract mapToDC(Clinic p)
        {
            return new ClinicContract
            {
               desp = p.desp,
               code = p.clinic_code
            };
        }

        /// <summary>
        /// Diplay the available clinics for the specific specialty
        /// </summary>
        /// <param name="specialtyId"></param>
        public List<ClinicContract> displayClinicsWithSpecialty(String specialtyId)
        {
            List <Clinic> list = new List<Clinic>();

            using (var unitOfWork = new UnitOfWork())
            {
                list = new ClinicBusiness(unitOfWork).GetClinicListWithSpecialty(specialtyId);
                unitOfWork.Close();
            }

            List<ClinicContract> contractList = new List<ClinicContract>();

            foreach (var x in list)
            {
                var cc = this.mapToDC(x);
                contractList.Add(cc);
            }
           
            return contractList;
        }

    }
}
