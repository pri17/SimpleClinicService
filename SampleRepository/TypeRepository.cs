using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using SampleDomain;

namespace SampleRepository
{
    public class TypeRepository
    {
        private readonly IUnitOfWork unitOfWork;

        public TypeRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

         public AppointmentType GetTypeData(String id)
        {
            return this.unitOfWork.Session.Query<AppointmentType>()
                .Where(x => x.appointment_type.Equals(id))
                .ToList()
                .FirstOrDefault();
        }

        public List<AppointmentType> getLists()
        {
            return this.unitOfWork.Session.Query<AppointmentType>()
                .ToList();
        }
    }
}
