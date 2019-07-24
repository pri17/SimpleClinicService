using System.Security.Cryptography.X509Certificates;

namespace DomainMappings
{
    using FluentNHibernate.Mapping;

    using SampleDomain;

    /// <summary>
    /// The sample data class map. Maps the class to the database table.
    /// </summary>
    public class SampleDataClassMap : ClassMap<SampleData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleDataClassMap"/> class.
        /// </summary>
        public SampleDataClassMap()
        {
            this.Id(x => x.Id);
            this.Map(x => x.SampleProperty);
        }

    }

    public class AppointmentMap : ClassMap<Appointment>
    {
        public AppointmentMap()
        {
            this.Id(x => x.appoint_id).Column("appointment_id");
            this.References(x => x.appoint_type).Column("appointment_type");
            this.References(x => x.clinic).Column("clinic_id");
            this.References(x => x.duration).Column("duration_code");
            this.Map(x => x.end_time);
            this.Map(x => x.start_time);
            this.References(x => x.patient).Column("patient_id");
            this.References(x => x.specialty).Column("specialty_id");
            this.References(x => x.urgent_code).Column("urgent_code");
            this.Table("Appointments");
            this.Schema("dbo");
            this.Map(x => x.isCancel).Column("cancel");
        }

    }

    public class PatientMap : ClassMap<Patient>
    {
        public PatientMap()
        {
            this.Id(x => x.patient_id);
            this.Map(x => x.first_name);
            this.Map(x => x.last_name);
            this.Map(x => x.email);
            this.Map(x => x.full_name);
            this.Map(x => x.phone).Column("phone_number");
            this.Table("Patients");
            this.Schema("dbo");

        }
    }

    public class SpecialtyMap : ClassMap<Specialty>
    {
        public SpecialtyMap()
        {
            this.Id(x => x.specialty_code).GeneratedBy.Assigned();
            this.Map(x => x.desp).Column("specialty_desp");
            this.Table("Specialty");
            this.Schema("dbo");
        }
    }

    public class ClinicMap : ClassMap<Clinic>
    {
        public ClinicMap()
        {
            this.Id(x => x.clinic_code).Column("clinic_code").GeneratedBy.Assigned();
            this.Map(x => x.desp).Column("clinic_desp");
        }
    }

    public class AppointDurationMap : ClassMap<Duration>
    {
        public AppointDurationMap()
        {
            this.Id(x => x.duration_code).GeneratedBy.Assigned();
            this.Map(x => x.desp).Column("duration");
            this.Table("appointmentDuration");
            this.Schema("dbo");
        }
    }

    public class UrgentMap : ClassMap<UrgentCode>
    {
        public UrgentMap()
        {
            this.Id(x => x.urgent_code).GeneratedBy.Assigned();
            this.Map(x => x.desp).Column("level");
            this.Table("urgentLevel");
            this.Schema("dbo");
        }
    }

    public class AppointTypeMap : ClassMap<AppointmentType>
    {
        public AppointTypeMap()
        {
            this.Id(x => x.appointment_type).Column("appoint_type").GeneratedBy.Assigned();
            this.Map(x => x.desp).Column("appoint_desp");
            this.Table("appointmentType");
            this.Schema("dbo");
        }
    }

    public class ClinicSpecialtyMap : ClassMap<ClinicSpecialty>
    {
        public ClinicSpecialtyMap()
        {
            this.Id(x => x.cs_id);
            this.References(x => x.clinic).Column("clinic_id").Cascade.All();
            this.References(x => x.specialty).Column("specialty_id").Cascade.All();
            this.Table("clinicSpecialty");
            this.Schema("dbo");
        }
    }
}
