using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleDomain;
using SampleRepository;


namespace SampleBusiness
{
    public class MessageBusiness
    {
         private readonly IUnitOfWork unitOfWork;
         private UnitOfWork unitOfWork1;

         public MessageBusiness(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<MHSMessage> Getlist()
        {
            return new MessageRepository(this.unitOfWork).getall();
        }

        public List<MHSMessage> GetById(string id)
        {
            return new MessageRepository(this.unitOfWork).getById(id);
        }

    }
}
