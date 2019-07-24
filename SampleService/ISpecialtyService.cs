using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SampleDataContracts;

namespace SampleService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISpecialtyService" in both code and config file together.
    [ServiceContract]
    public interface ISpecialtyService
    {

        [OperationContract]
        SpecialtyContract getSpecialtyData(string id);

        [OperationContract]
        List<SpecialtyContract> getAllSpecialty();
    }
}
