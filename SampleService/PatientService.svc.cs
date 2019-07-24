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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "IPatientService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PatientService.svc or PatientService.svc.cs at the Solution Explorer and start debugging.
    public class PatientService : IPatientService
    {
        private readonly Logger logger;

        public PatientService()
        {
            this.logger = new Logger();
        }

        public PatientContract getPatinentData(int id)
        {
            this.logger.Log("BEGIN - get appointment data");

            Patient p;

            using (var unitOfWork = new UnitOfWork())
            {
                p = new PatientBusiness(unitOfWork).GetPatientData(id);
                unitOfWork.Close();
            }

            var mappedContract = this.mapToDC(p);
            return mappedContract;
        }

        private PatientContract mapToDC(Patient p)
        {
            return new PatientContract
            {
                last_name = p.last_name,
                first_name = p.first_name,
                email = p.email,
                phone = p.phone,
                id = p.patient_id,
                full_name = p.first_name + " " +p.last_name
            };
        }

        public List<PatientContract> getPatinentLists()
        {
            List<Patient> plist = new List<Patient>();
            List<PatientContract> pc = new List<PatientContract>();
            using (var unitOfWork = new UnitOfWork())
            {
                plist = new PatientBusiness(unitOfWork).getAllPatients();
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
