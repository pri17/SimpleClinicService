using System.Collections.Generic;
using Core;
using SampleBusiness;
using SampleDataContracts;
using SampleDomain;

namespace SampleService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UrgentService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UrgentService.svc or UrgentService.svc.cs at the Solution Explorer and start debugging.
    public class UrgentService : IUrgentService
    {

        private readonly Logger logger;

        public UrgentService()
        {
            this.logger = new Logger();
        }
        public UrgentContract getUrgentData(string id)
        {
            this.logger.Log("BEGIN - get clinic data");

            UrgentCode p;

            using (var unitOfWork = new UnitOfWork())
            {
                p = new UrgentBusiness(unitOfWork).GetUrgentData(id);
                unitOfWork.Close();
            }

            var mappedContract = this.mapToDC(p);
            return mappedContract;
        }

        private UrgentContract mapToDC(UrgentCode p)
        {
            return new UrgentContract
            {
               code = p.urgent_code,
               desp = p.desp,
            };
        }

        public List<UrgentContract> getCodeList()
        {
            List<UrgentCode> p = new List<UrgentCode>();

            using (var unitOfWork = new UnitOfWork())
            {
                p = new UrgentBusiness(unitOfWork).getCodeList();
                unitOfWork.Close();
            }
            List<UrgentContract> tl = new List<UrgentContract>();
            foreach (var xx in p)
            {
                var tem = mapToDC(xx);
                tl.Add(tem);
            }
            return tl;
        }
    }
}
