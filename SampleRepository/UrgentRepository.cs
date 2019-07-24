using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using SampleDomain;

namespace SampleRepository
{
    public class UrgentRepository
    {
        private readonly IUnitOfWork unitOfWork;

        public UrgentRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

         public UrgentCode GetUrgentData(String id)
        {
            return this.unitOfWork.Session.Query<UrgentCode>()
                .Where(x => x.urgent_code.Equals(id))
                .ToList()
                .FirstOrDefault();
        }

        public List<UrgentCode> getCodeList()
        {
            return this.unitOfWork.Session.Query<UrgentCode>()
                .ToList();
        }
    }
}
