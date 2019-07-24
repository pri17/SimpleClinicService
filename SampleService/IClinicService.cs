using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SampleDataContracts;

namespace SampleService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IClinicService" in both code and config file together.
    [ServiceContract]
    public interface IClinicService
    {
        [OperationContract]
        ClinicContract getclinicData(string id);

        [OperationContract]
        List<ClinicContract> displayClinicsWithSpecialty(String specialtyId);

    }
}
