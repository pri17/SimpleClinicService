using System;
using Itenso.TimePeriod;

namespace SampleDataContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The sample data contract.
    /// </summary>
    [DataContract]
    public class SampleDataContract
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the sample property.
        /// </summary>
        [DataMember]
        public string SampleProperty { get; set; }
    }

    [DataContract]
    public class AppointmentDataContract
    {
        [DataMember]
        public int appointmentId { get; set; }

        [DataMember]
        public int patientID { get; set; }

        [DataMember]
        public String name { get; set; }

        [DataMember]
        public long phone { get; set; }

        [DataMember]
        public String email { get; set; }

        [DataMember]
        public String clinicDesp { get; set; }

        [DataMember]
        public String clinicId { get; set; }

        [DataMember]
        public String specialtyDesp { get; set; }

        [DataMember]
        public String specialtyId { get; set; }

        [DataMember]
        public String durationDesp { get; set; }

        [DataMember]
        public String durationCode { get; set; }

        [DataMember]
        public String urgentDesp { get; set; }

        [DataMember]
        public String urgentCode { get; set; }

        [DataMember]
        public DateTime start_time { get; set; }

        [DataMember]
        public DateTime end_time { get; set; }

        [DataMember]
        public String appointType { get; set; }

        [DataMember]
        public String appointTypeCode { get; set; }

        [DataMember]
        public String isCancel { get; set; }
    }

    [DataContract]
    public class PatientContract
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string first_name { get; set; }

        [DataMember]
        public string last_name { get; set; }

        [DataMember]
        public long phone { get; set; }

        [DataMember]
        public string email { get; set; }

        [DataMember]
        public string full_name { get; set; }
    }


    [DataContract]
    public class ClinicContract
    {
        [DataMember]
        public string code { get; set; }

        [DataMember]
        public string desp { get; set; }
    }

    [DataContract]
    public class SpecialtyContract
    {
        [DataMember]
        public string code { get; set; }

        [DataMember]
        public string desp { get; set; }
    }

    [DataContract]
    public class DurationContract
    {
        [DataMember]
        public string code { get; set; }

        [DataMember]
        public string desp { get; set; }
    }

    [DataContract]
    public class TypeContract
    {
        [DataMember]
        public string type { get; set; }

        [DataMember]
        public string desp { get; set; }
    }

    [DataContract]
    public class UrgentContract
    {
        [DataMember]
        public string code { get; set; }

        [DataMember]
        public string desp { get; set; }
    }

    [DataContract]
    public class TimeSlotContract
    {
        [DataMember]
        public DateTime start { get; set; }

        [DataMember]
        public DateTime end { get; set; }

        [DataMember]
        public String time { get; set; }

    }

    [DataContract]
    public class DatabaseExceptionContract
    {
        [DataMember]
        public bool Result { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}