using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using SampleDomain;

namespace SampleRepository
{
    public class DurationRepository
    {
         private readonly IUnitOfWork unitOfWork;

         public DurationRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

         public Duration GetDurationData(String id)
        {
            return this.unitOfWork.Session.Query<Duration>()
                .Where(x => x.duration_code.Equals(id))
                .ToList()
                .FirstOrDefault();
        }

        public List<Duration> getDurationList()
        {
            return this.unitOfWork.Session.Query<Duration>()
                .OrderBy(x => x.duration_code).ToList();
        }
    }
}
