using System;
using Core;
using SampleDomain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleRepository
{
    public class MessageRepository
    {
        private readonly IUnitOfWork unitOfWork;

        public MessageRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public int getCount()
        {
            return this.unitOfWork.Session.Query<MHSMessage>()
                .Count();
        }

        public List<MHSMessage> getall(int page, int pageSize)
        {
            return this.unitOfWork.Session.Query<MHSMessage>()
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public List<MHSMessage> getById(int page, int pageSize)
        {
            return this.unitOfWork.Session.Query<MHSMessage>()
                .Where(x => x.Source == "EBS_ASID")
                .Where(x => x.Destination == "DEVTEST")
                .Skip(page*pageSize)
                .Take(pageSize)
                .ToList();
        }

 
    }
    
}
