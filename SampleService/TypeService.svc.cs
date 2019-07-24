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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TypeService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TypeService.svc or TypeService.svc.cs at the Solution Explorer and start debugging.
    public class TypeService : ITypeService
    {
        private readonly Logger logger;

        public TypeService()
        {
            this.logger = new Logger();
        }

        public TypeContract getTypetData(string id)
        {
            this.logger.Log("BEGIN - get clinic data");

            AppointmentType p;

            using (var unitOfWork = new UnitOfWork())
            {
                p = new TypeBusiness(unitOfWork).GetTypeData(id);
                unitOfWork.Close();
            }

            var mappedContract = this.mapToDC(p);
            return mappedContract;
        }

        public List<TypeContract> getTypeList()
        {
            List<AppointmentType> p = new List<AppointmentType>();

            using (var unitOfWork = new UnitOfWork())
            {
                p = new TypeBusiness(unitOfWork).getTypeList();
                unitOfWork.Close();
            }
            List<TypeContract> tl = new List<TypeContract>();
            foreach (var xx in p)
            {
                var tem = mapToDC(xx);
                tl.Add(tem);
            }
            return tl;
        }

        private TypeContract mapToDC(AppointmentType p)
        {
            return new TypeContract
            {
               desp = p.desp,
               type = p.appointment_type
            };
        }
    }
}
