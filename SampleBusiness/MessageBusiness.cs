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

        public int getCount()
        {
            return new MessageRepository(this.unitOfWork).getCount();
        }

        public List<MHSMessage> Getlist(int page, int pageSize)
        {
            return new MessageRepository(this.unitOfWork).getall(page, pageSize);
        }

        public List<MHSMessage> GetById(int page, int pageSize)
        {
            return new MessageRepository(this.unitOfWork).getById(page, pageSize);
        }

    }
}
