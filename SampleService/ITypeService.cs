using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SampleDataContracts;

namespace SampleService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITypeService" in both code and config file together.
    [ServiceContract]
    public interface ITypeService
    {
        [OperationContract]
        TypeContract getTypetData(string id);

        [OperationContract]
        List<TypeContract> getTypeList();

    }
}
