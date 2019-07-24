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
    public class DurationBusiness
    {
        
        private readonly IUnitOfWork unitOfWork;

        public DurationBusiness(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Duration GetDurationData(string id)
        {
            return new DurationRepository(this.unitOfWork).GetDurationData(id);
        }

        public List<Duration> geDuraltionList()
        {
            return new DurationRepository(this.unitOfWork).getDurationList();
        }
    }
}
