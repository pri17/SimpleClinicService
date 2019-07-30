using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SampleDataContracts;
using System.ServiceModel.Web;


namespace SampleService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMessageService" in both code and config file together.
    [ServiceContract]
    public interface IMessageService
    {

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getCount", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        int getCount();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "getallLists?page={page}&pageSize={pageSize}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        List<MessageContract> getallLists(int page, int pageSize);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "getById?page={page}&pageSize={pageSize}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        List<MessageContract> getById(int page, int pageSize);
    }
}
