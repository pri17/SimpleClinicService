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
    public class TypeBusiness
    {
        private readonly IUnitOfWork unitOfWork;

        public TypeBusiness(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public AppointmentType GetTypeData(string id)
        {
            return new TypeRepository(this.unitOfWork).GetTypeData(id);
        }

        public List<AppointmentType> getTypeList()
        {
            return new TypeRepository(this.unitOfWork).getLists();
        }

    }
}
