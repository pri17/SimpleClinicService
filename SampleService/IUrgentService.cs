using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SampleDataContracts;

namespace SampleService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUrgentService" in both code and config file together.
    [ServiceContract]
    public interface IUrgentService
    {
        [OperationContract]
        UrgentContract getUrgentData(string id);

        [OperationContract]
        List<UrgentContract> getCodeList();
    }
}
