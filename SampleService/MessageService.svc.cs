using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Core;
using SampleBusiness;
using SampleDataContracts;
using SampleDomain;

namespace SampleService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MessageService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MessageService.svc or MessageService.svc.cs at the Solution Explorer and start debugging.
    public class MessageService : IMessageService
    {
       private readonly Logger logger;

       public MessageService()
        {
            this.logger = new Logger();
        }

       public List<MessageContract> getallLists()
       {
           List<MHSMessage> p = new List<MHSMessage>();

           using (var unitOfWork = new UnitOfWork())
           {
               p = new MessageBusiness(unitOfWork).Getlist();
               unitOfWork.Close();
           }
           List<MessageContract> tl = new List<MessageContract>();
           foreach (var xx in p)
           {
               var tem = mapToDC(xx);
               tl.Add(tem);
           }
           return tl;
       }

       public MessageContract getById(string id)
       {
           MHSMessage p ;

           using (var unitOfWork = new UnitOfWork())
           {
               p = new MessageBusiness(unitOfWork).GetById(id);
               unitOfWork.Close();
           }
           
           var tem = mapToDC(p);
          
           return tem;
       }

       private MessageContract mapToDC(MHSMessage p)
       {
           return new MessageContract
           {
               sequenceId = p.SequenceID,
               attempts = p.Attempts,
               actionAt = p.ActionAt,
               conversationID = p.ConversationID,
               createdAt = p.CreatedAt,
               destination = p.Destination,
               messageID = p.MessageID,
               messageType =p.MessageType,
               referenceID = p.ReferenceID,
               source = p.Source,
                state =p.State,
                transportMessageId =p.TransportMessageId,
                version= p.Version
           };
       }
    }
}
