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
    public class UrgentBusiness
    {
        private readonly IUnitOfWork unitOfWork;

        public UrgentBusiness(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public UrgentCode GetUrgentData(string id)
        {
            return new UrgentRepository(this.unitOfWork).GetUrgentData(id);
        }

        public List<UrgentCode> getCodeList()
        {
            return new UrgentRepository(this.unitOfWork).getCodeList();
        }
    }
}
