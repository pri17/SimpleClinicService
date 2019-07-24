using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.ServiceModel;
using Itenso.TimePeriod;
using SampleDataContracts;
using SampleDomain;

namespace SampleService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAppointmentService" in both code and config file together.
    [ServiceContract]
    public interface IAppointmentService
    {
        /// <summary>
        /// The get appointment data.
        /// </summary>
        /// <returns>
        /// The <see cref="AppointmentDataContract"/>.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(DatabaseExceptionContract))]
        AppointmentDataContract getAppointmentData(int id);

        [OperationContract]
        [FaultContract(typeof(DatabaseExceptionContract))]
        void InsertAppointment(int patientID, string clinicId, string specialtyId,
            string durationCode, string urgentCode,  DateTime start, DateTime end, string type);

        [OperationContract]
        [FaultContract(typeof(DatabaseExceptionContract))]
        Boolean UpdateAppointment(int appId, int patientID, string clinicId, string specialtyId,
            string durationCode, string urgentCode, DateTime start, DateTime end, string type);

        [OperationContract]
        [FaultContract(typeof(DatabaseExceptionContract))]
        Boolean CancelAppointment(int appId);

        [OperationContract]
        [FaultContract(typeof(DatabaseExceptionContract))]
        Boolean UndoCancelAppointment(int appId);

        [OperationContract]
        [FaultContract(typeof(DatabaseExceptionContract))]
        List<AppointmentDataContract> getAllList();

        [OperationContract]
        [FaultContract(typeof(DatabaseExceptionContract))]
        List<AppointmentDataContract> getListWithClinicSpecialty(String clinicId, String SpecialtyId);

        [OperationContract]
        [FaultContract(typeof(DatabaseExceptionContract))]
        List<TimeSlotContract> getFreePeriodsNew(String clinicId, String specialtyId, DateTime dd, String durationId);

        [OperationContract]
        [FaultContract(typeof(DatabaseExceptionContract))]
        List<TimeSlotContract> getFreePeriodsEdit(string clinicId, string specialtyId, DateTime start, DateTime end, String durationId);

        [OperationContract]
        List<AppointmentDataContract> getListPatientAsc();

        [OperationContract]
        List<AppointmentDataContract> getListPatientDesc();

        [OperationContract]
        List<AppointmentDataContract> getListClinicAsc();

        [OperationContract]
        List<AppointmentDataContract> getListClinicDesc();

        [OperationContract]
        List<AppointmentDataContract> getListSpecialtyAsc();

        [OperationContract]
        List<AppointmentDataContract> getListSpecialtyDesc();

        [OperationContract]
        List<AppointmentDataContract> getListDurationAsc();

        [OperationContract]
        List<AppointmentDataContract> getListDurationDesc();

        [OperationContract]
        List<AppointmentDataContract> getListUrgentAsc();

        [OperationContract]
        List<AppointmentDataContract> getListUrgentDesc();

        [OperationContract]
        List<AppointmentDataContract> getListTypeAsc();

        [OperationContract]
        List<AppointmentDataContract> getListTypeDesc();

        [OperationContract]
        List<AppointmentDataContract> getListTimeAsc();

        [OperationContract]
        List<AppointmentDataContract> getListTimeDesc();

        [OperationContract]
        List<AppointmentDataContract> getListStatusAsc();

        [OperationContract]
        List<AppointmentDataContract> getListStatusDesc();

        [OperationContract]
        List<AppointmentDataContract> getListPatientNameAsc();

        [OperationContract]
        List<AppointmentDataContract> getListPatientNameDesc();

        [OperationContract]
        List<AppointmentDataContract> filterByPatientName(string name);

        [OperationContract]
        List<AppointmentDataContract> filterByClinicName(string name);

        [OperationContract]
        List<AppointmentDataContract> filterByDaterange(DateTime start1, DateTime end1);


    }
}
