using System;
using System.Security.Cryptography;
using Itenso.TimePeriod;

namespace SampleDomain
{
    /// <summary>
    /// The sample data.
    /// </summary>
    public class SampleData : ISampleData
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the sample property.
        /// </summary>
        public virtual string SampleProperty { get; set; }
    }

    public class Patient
    {
        public virtual int patient_id { get; set; }
        public virtual string first_name { get; set; }
        public virtual string last_name { get; set; }
        public virtual long phone { get; set; }
        public virtual string email { get; set; }
        public virtual string full_name { get; set; }

        public Patient(int id, string f, string l, long phone, string email, string full_name)
        {
            patient_id = id;
            first_name = f;
            last_name = l;
            this.phone = phone;
            this.email = email;
            this.full_name = full_name;
        }

        public Patient() { }
    }

    public class Clinic
    {
        public virtual string clinic_code { get; set; }
        public virtual string desp { get; set; }

        public Clinic(String code, String desp)
        {
            this.clinic_code = code;
            this.desp = desp;
        }

        public Clinic() { }
    }

    public class Specialty
    {
        public virtual string specialty_code { get; set; }
        public virtual string desp { get; set; }

        public Specialty(String code, String desp)
        {
            this.specialty_code = code;
            this.desp = desp;
        }

        public Specialty() { }
    }

    public class ClinicSpecialty
    {
        public virtual int cs_id { get; set; }
        public virtual Clinic clinic { get; set; }
        public virtual Specialty specialty { get; set; }

        public ClinicSpecialty(int id, Clinic c, Specialty s)
        {
            this.cs_id = id;
            this.clinic = c;
            this.specialty = s;
        }

        public ClinicSpecialty() { }
    }

    public class Duration
    {
        public virtual string duration_code { get;  set; }
        public virtual string desp { get; set; }

        public Duration(String code, String desp)
        {
            this.duration_code = code;
            this.desp = desp;
        }

        public Duration() { }
    }

    public class UrgentCode
    {
        public virtual string urgent_code { get;  set; }
        public virtual string desp { get; set; }

        public UrgentCode(String code, String desp)
        {
            this.urgent_code = code;
            this.desp = desp;
        }

        public UrgentCode() { }
    }

    public class AppointmentType
    {
        public virtual string appointment_type { get;  set; }
        public virtual string desp { get; set; }

        public AppointmentType(String type, String desp)
        {
            this.appointment_type = type;
            this.desp = desp;
        }

        public AppointmentType() { }
    }

    public class Appointment
    {
        public virtual int appoint_id { get;  set; }
        public virtual Patient patient { get; set; }
        public virtual Clinic clinic { get; set; }
        public virtual Specialty specialty { get; set; }
        public virtual Duration duration { get; set; }
        public virtual UrgentCode urgent_code { get; set; }
        public virtual DateTime start_time { get; set; }
        public virtual DateTime end_time { get; set; }
        public virtual AppointmentType appoint_type { get; set; }
        public virtual int isCancel { get; set; }

        public Appointment(Patient patient, Clinic clinic, Specialty specialty, Duration duration,
          UrgentCode urgent,  DateTime startTime, DateTime endTime, AppointmentType appointmentType)
        {
            this.patient = patient;
            this.clinic = clinic;
            this.specialty = specialty;
            this.duration = duration;
            this.urgent_code = urgent;
            this.start_time = startTime;
            this.end_time = endTime;
            this.appoint_type = appointmentType;
        }

        public Appointment() { }
    }
}
