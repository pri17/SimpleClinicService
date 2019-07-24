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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DurationService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DurationService.svc or DurationService.svc.cs at the Solution Explorer and start debugging.
    public class DurationService : IDurationService
    {
        private readonly Logger logger;

        public DurationService()
        {
            this.logger = new Logger();
        }

        public DurationContract GetDurationData(String id)
        {
            var p = new Duration();
            using (var unitOfWork = new UnitOfWork())
            {
                p = new DurationBusiness(unitOfWork).GetDurationData(id);
                unitOfWork.Close();
            }

            DurationContract dc = mapToDC(p);
            return dc;

        }

        public List<DurationContract> getDurationList( )
        {
            this.logger.Log("BEGIN - get clinic data");

            List<Duration> p = new List<Duration>();
            List<DurationContract> dl = new List<DurationContract>();

            using (var unitOfWork = new UnitOfWork())
            {
                p = new DurationBusiness(unitOfWork).geDuraltionList();
                unitOfWork.Close();
            }

            foreach (var xx in p)
            {
                var mp = mapToDC(xx);
                dl.Add(mp);
            }
            return dl;
        }

        private DurationContract mapToDC(Duration p)
        {
            return new DurationContract
            {
                code = p.duration_code,
                desp = p.desp
            };
        }
    }
}
