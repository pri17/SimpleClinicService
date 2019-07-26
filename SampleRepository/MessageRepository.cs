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

        /// <summary>
        /// Get one Appointment record
        /// </summary>
        /// <param name="id">appointment id</param>
        /// <returns></returns>
        public List<MHSMessage> getall()
        {
            return this.unitOfWork.Session.Query<MHSMessage>()
                .ToList();
        }

        public MHSMessage getById(string id)
        {
            return this.unitOfWork.Session.Query<MHSMessage>()
                .Where(x => x.SequenceID == Int32.Parse(id))
                .FirstOrDefault();
        }

 
    }
    
}
