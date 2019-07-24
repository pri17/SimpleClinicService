using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Web.ApplicationServices;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using Core;
using Itenso.TimePeriod;
using SampleBusiness;
using SampleDataContracts;
using SampleDomain;
using Duration = SampleDomain.Duration;

namespace SampleService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AppointmentService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AppointmentService.svc or AppointmentService.svc.cs at the Solution Explorer and start debugging.
    /// <summary>
    /// The appointment service.
    /// </summary>
    public class AppointmentService : IAppointmentService
    {

        private readonly Logger logger;

        public AppointmentService()
        {
            this.logger = new Logger();
        }


        /// <summary>
        /// Get a single appointment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AppointmentDataContract getAppointmentData(int id)
        {
            this.logger.Log("BEGIN - get appointment data");

            Appointment appoint;

            using (var unitOfWork = new UnitOfWork())
            {
                appoint = new AppointmentBusiness(unitOfWork).GetAppointmentData(id);
                if (appoint == null)
                {
                    var detail = new DatabaseExceptionContract
                    {
                        Result = false,
                        Message = "Cannot get the appointment data!"
                    };
                    throw new FaultException<DatabaseExceptionContract>(detail, new FaultReason(detail.Message));
                }
                unitOfWork.Close();
            }

            var mappedContract = this.mapToDC(appoint);
            return mappedContract;
        }

        private AppointmentDataContract mapToDC(Appointment app)
        {
            return new AppointmentDataContract
            {
                appointmentId = app.appoint_id,
                patientID = app.patient.patient_id,
                name = app.patient.first_name + " " + app.patient.last_name, //patient full name
                phone = app.patient.phone,  //patient phone
                email = app.patient.email, //patient email
                appointType = app.appoint_type.desp, //appintment type
                durationDesp = app.duration.desp,  // duration detail
                specialtyDesp = app.specialty.desp, // specialty detail
                urgentDesp = app.urgent_code.desp, // urgent code detail
                clinicDesp = app.clinic.desp,  // clinic detail
                start_time = app.start_time,
                end_time = app.end_time,

                specialtyId = app.specialty.specialty_code,
                clinicId = app.clinic.clinic_code,
                appointTypeCode = app.appoint_type.appointment_type,
                durationCode = app.duration.duration_code,
                urgentCode = app.urgent_code.urgent_code,

                isCancel = Convert.ToBoolean(app.isCancel) ? "Cancelled" : "Active" // need to convert isCanclel int to boolean
            };
        }

        /// <summary>
        /// Insert a new appointment
        /// </summary>
        /// <param name="patientID"></param>
        /// <param name="clinicId"></param>
        /// <param name="specialtyId"></param>
        /// <param name="durationCode"></param>
        /// <param name="urgentCode"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="type"></param>
        public void InsertAppointment(int patientID, string clinicId, string specialtyId,
            string durationCode, string urgentCode, DateTime  start, DateTime end, string type)
        {
            this.logger.Log("BEGIN - insert appointment data");

            Patient p;
            Specialty s;
            Clinic c;
            Duration d;
            AppointmentType t;
            UrgentCode u;
            Appointment app = new Appointment();
            using (var unitOfWork = new UnitOfWork())
            {
                try
                {
                    p = new PatientBusiness(unitOfWork).GetPatientData(patientID);
                    s = new SpecialtyBusiness(unitOfWork).GetSpecialtyData(specialtyId);
                    c = new ClinicBusiness(unitOfWork).GetClinicData(clinicId);
                    d = new DurationBusiness(unitOfWork).GetDurationData(durationCode);
                    t = new TypeBusiness(unitOfWork).GetTypeData(type);
                    u = new UrgentBusiness(unitOfWork).GetUrgentData(urgentCode);


                    app = new Appointment(p, c, s, d, u, start, end, t);
                }
                catch (DatabaseExcception e)
                {
                    var detail = new DatabaseExceptionContract
                    {
                        Result = false,
                        Message = e.Message,
                    };
                    throw new FaultException<DatabaseExceptionContract>(detail, new FaultReason(detail.Message));
                }

                try
                {
                    new AppointmentBusiness(unitOfWork).InsertAppointment(app);
                }
                catch (DatabaseExcception e)
                {
                    var detail = new DatabaseExceptionContract
                    {
                        Result = false,
                        Message = e.Message,
                    };
                    throw new FaultException<DatabaseExceptionContract>(detail, new FaultReason(detail.Message));

                }

                unitOfWork.Close();
            }

        }

        /// <summary>
        /// Update an appointmnet
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="patientID"></param>
        /// <param name="clinicId"></param>
        /// <param name="specialtyId"></param>
        /// <param name="durationCode"></param>
        /// <param name="urgentCode"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="type"></param>
        public Boolean UpdateAppointment(int appId, int patientID, string clinicId, string specialtyId,
            string durationCode, string urgentCode, DateTime start, DateTime end, string type)
        {
            this.logger.Log("BEGIN - update appointment");
            using (var unitOfWork = new UnitOfWork())
            {
                try
                {
                    new AppointmentBusiness(unitOfWork).UpdateApp(appId, patientID, clinicId, specialtyId,
                        durationCode, urgentCode, start, end, type);
                    unitOfWork.Close();
                    return true;
                }
                catch (DatabaseExcception e)
                {
                    var detail = new DatabaseExceptionContract
                    {
                        Result = false,
                        Message = e.Message,
                    };
                    throw new FaultException<DatabaseExceptionContract>(detail, new FaultReason(detail.Message));

                }
            }
        }

        /// <summary>
        /// Cancel an appointment
        /// </summary>
        /// <param name="appId"></param>

        public Boolean CancelAppointment(int appId)
        {
            this.logger.Log("BEGIN - cancel appointment");
            using (var unitOfWork = new UnitOfWork())
            {
                try
                {
                    if (!new AppointmentBusiness(unitOfWork).CancelAppointment(appId))
                       throw new DatabaseExcception("Sorry, cannot cancel the appointment.");
                    unitOfWork.Close();

                    return true;
                }
                catch (DatabaseExcception e)
                {
                    var detail = new DatabaseExceptionContract
                    {
                        Result = false,
                        Message = e.Message
                    };
                    throw new FaultException<DatabaseExceptionContract>(detail, new FaultReason(detail.Message));
                }
            }
           
        }


        /// <summary>
        /// Undo cancel an appointment
        /// </summary>
        /// <param name="appId"></param>
        public Boolean UndoCancelAppointment(int appId)
        {
            this.logger.Log("BEGIN - uncancel appointment");
            using (var unitOfWork = new UnitOfWork())
            {

                try
                {
                    if (new AppointmentBusiness(unitOfWork).UndoCancel(appId))
                        return true;
                   
                    unitOfWork.Close();
                    return false;
                }
                catch (DatabaseExcception e)
                {
                    var detail = new DatabaseExceptionContract
                    {
                        Result = false,
                        Message = e.Message
                    };
                    throw new FaultException<DatabaseExceptionContract>(detail);
                }
             
            }
        }

        public List<AppointmentDataContract> getAllList()
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                try
                {
                    list = new AppointmentBusiness(unitOfWork).getAllList();
                }
                catch (DatabaseExcception e)
                {
                    var detail = new DatabaseExceptionContract
                    {
                        Result = false,
                        Message = e.Message
                    };
                    throw new FaultException<DatabaseExceptionContract>(detail);
                }
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        /// <summary>
        /// get appointmnet list with clinic id and specialty id
        /// </summary>
        /// <param name="clinicId"></param>
        /// <param name="SpecialtyId"></param>
        /// <returns></returns>
        public List<AppointmentDataContract> getListWithClinicSpecialty(String clinicId, String specialtyId)
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                try
                {
                    list = new AppointmentBusiness(unitOfWork).getListWithCS(clinicId, specialtyId);
                }
                catch (DatabaseExcception e)
                {
                    var detail = new DatabaseExceptionContract
                    {
                        Result = false,
                        Message = e.Message
                    };
                    throw new FaultException<DatabaseExceptionContract>(detail, new FaultReason(detail.Message));
                }
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        /// <summary>
        /// Get free timeslots for booking new appointments
        /// </summary>
        /// <param name="clinicId"></param>
        /// <param name="specialtyId"></param>
        /// <param name="dd"></param>
        public List<TimeSlotContract> getFreePeriodsNew(String clinicId, String specialtyId, DateTime dd, String durationId)
        {
            ITimePeriodCollection freeTimeSlots = new TimePeriodCollection();
            using (var unitOfWork = new UnitOfWork())
            {
                //1. get the booked apoointment list
                var business = new AppointmentBusiness(unitOfWork);

                try
                {
                    var list = business.getListWithCSD(clinicId, specialtyId, dd);

                    //2. Consolidate all time period of the appointmetns of the day
                    TimePeriodCollection periods = new TimePeriodCollection();

                    foreach (var app in list)
                    {
                        periods.Add(new TimeRange(app.start_time, app.end_time));
                    }

                    freeTimeSlots = business.getAvailableTimeslots(periods, dd, durationId); //get available timeslots;
                }
                catch (DatabaseExcception e)
                {
                    var detail = new DatabaseExceptionContract
                    {
                        Result = false,
                        Message = e.Message
                    };
                    throw new FaultException<DatabaseExceptionContract>(detail, new FaultReason(detail.Message));
                }

                unitOfWork.Close();
            }

            List<TimeSlotContract> contractList = new List<TimeSlotContract>();

            foreach (var x in freeTimeSlots)
            {
                contractList.Add(this.mapToTimeSlotContract(x));
            }
            return contractList;

        }


        /// <summary>
        /// Get free timeslots for editing purpose 
        /// </summary>
        /// <param name="clinicId"></param>
        /// <param name="specialtyId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public List<TimeSlotContract> getFreePeriodsEdit(string clinicId, string specialtyId, DateTime start, DateTime end, String durationId)
        {

            ITimePeriodCollection freeTimeSlots = new TimePeriodCollection();

            using (var unitOfWork = new UnitOfWork())
            {
                var business = new AppointmentBusiness(unitOfWork);

                try
                {
                    var list = business.getListWithCSD(clinicId, specialtyId, start);


                    TimePeriodCollection periods = new TimePeriodCollection();

                    foreach (var app in list)
                    {
                        periods.Add(new TimeRange(app.start_time, app.end_time));
                    }

                    //Remove the original appointment time from the booked time list!
                    periods.Remove(new TimeRange(start, end));

                    freeTimeSlots = business.getAvailableTimeslots(periods, start, durationId);
                    //get available timeslots;

                }
                catch (DatabaseExcception e)
                {
                    var detail = new DatabaseExceptionContract
                    {
                        Result = false,
                        Message = e.Message
                    };
                    throw new FaultException<DatabaseExceptionContract>(detail, new FaultReason(detail.Message));
                }
                unitOfWork.Close();
            }

            List<TimeSlotContract> contractList = new List<TimeSlotContract>();

            foreach (var x in freeTimeSlots)
            {
                contractList.Add(this.mapToTimeSlotContract(x));
            }
            return contractList;

        }

        private TimeSlotContract mapToTimeSlotContract(ITimePeriod range)
        {

            return new TimeSlotContract
            {
                end = range.End,
                start = range.Start,
                time = range.Start.TimeOfDay + "-" + range.End.TimeOfDay
            };
        }

        public List<AppointmentDataContract> getListPatientAsc()
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).getListByPatientAsc();
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        public List<AppointmentDataContract> getListPatientDesc()
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).getListByPatientDesc();
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        public List<AppointmentDataContract> getListClinicAsc()
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).getListByClinicAsc();
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        public List<AppointmentDataContract> getListClinicDesc()
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).getListByClinicDesc();
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        public List<AppointmentDataContract> getListSpecialtyAsc()
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).getListBySpecialtyAsc();
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        public List<AppointmentDataContract> getListSpecialtyDesc()
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).getListBySpecialtyDesc();
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        public List<AppointmentDataContract> getListDurationAsc()
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).getListByDurationAsc();
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        public List<AppointmentDataContract> getListDurationDesc()
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).getListByDurationDesc();
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        public List<AppointmentDataContract> getListUrgentAsc()
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).getListByUrgentAsc();
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        public List<AppointmentDataContract> getListUrgentDesc()
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).getListByUrgentDesc();
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        public List<AppointmentDataContract> getListTypeAsc()
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).getListByTypeAsc();
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        public List<AppointmentDataContract> getListTypeDesc()
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).getListByTypeDesc();
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        public List<AppointmentDataContract> getListTimeAsc()
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).getListByTimeAsc();
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        public List<AppointmentDataContract> getListTimeDesc()
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).getListByTimeDesc();
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }
        
        public List<AppointmentDataContract> getListStatusAsc()
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).getListByStatusAsc();
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        public List<AppointmentDataContract> getListStatusDesc()
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).getListByStatusDesc();
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        public List<AppointmentDataContract> getListPatientNameAsc()
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).getListByPNameAsc();
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        public List<AppointmentDataContract> getListPatientNameDesc()
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).getListByPNameDesc();
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        public List<AppointmentDataContract> filterByPatientName(string name)
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).filterAppByPatientName(name);
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        public List<AppointmentDataContract> filterByClinicName(string name)
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).filterAppByClinicName(name);
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }

        public List<AppointmentDataContract> filterByDaterange(DateTime start1, DateTime end1)
        {
            var list = new List<Appointment>();
            using (var unitOfWork = new UnitOfWork())
            {
                list = new AppointmentBusiness(unitOfWork).filterAppByDaterange(start1,end1);
                unitOfWork.Close();
            }
            List<AppointmentDataContract> contractList = new List<AppointmentDataContract>();

            foreach (var x in list)
            {
                contractList.Add(this.mapToDC(x));
            }
            return contractList;
        }
    }

}
