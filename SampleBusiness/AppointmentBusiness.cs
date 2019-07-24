using System;
using System.Collections.Generic;
using System.Data.Common;
using Itenso.TimePeriod;

namespace SampleBusiness
{
    using Core;
    using SampleDomain;
    using SampleRepository;

    public class AppointmentBusiness
    {
        private readonly IUnitOfWork unitOfWork;

        public AppointmentBusiness(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// </summary>
        /// <param name="id">appointment id</param>
        /// <returns></returns>
        public Appointment GetAppointmentData(int id)
        {
            var app =  new AppointmentRepository(this.unitOfWork).GetAppointmentsData(id);
            if (app == null)
                throw new DatabaseExcception("The appointment id is invaild.");
            else
                return app;

        }

        public List<Appointment> getAllList()
        {
            var appList =  new AppointmentRepository(this.unitOfWork).GetAppintmentList();
            if(appList.Count==0)
                throw new DatabaseExcception("The system exists no appointment records currently.");
            else
                return appList;
          
        }

        public void InsertAppointment(Appointment app)
        {
            // check if timeslot is available
            
                if (checkAvailablity(app.clinic.clinic_code, app.specialty.specialty_code, app.start_time, app.end_time))
                {
                    try
                    {
                        new AppointmentRepository(this.unitOfWork).InsertAppointment(app);
                    }
                    catch (DatabaseExcception e)
                    {
                        throw new DatabaseExcception("Sorry, cannot book the appointment!");
                    }
                }
                else
                {
                    throw new DatabaseExcception("Sorry, cannot book the appointment beacuse of timeslots conflict!");
                }
          
         }


        public void UpdateApp(int appId, int patientID, string clinicId, string specialtyId,
            string durationCode, string urgentCode, DateTime start, DateTime end, string type)
        {

            // Also need to check the timeslot is available or not
            if (checkAvailablity(clinicId, specialtyId, start, end))
            {
                var appointment = new Appointment();
                try
                {
                    var p = new PatientBusiness(unitOfWork).GetPatientData(patientID);
                    var s = new SpecialtyBusiness(unitOfWork).GetSpecialtyData(specialtyId);
                    var c = new ClinicBusiness(unitOfWork).GetClinicData(clinicId);
                    var d = new DurationBusiness(unitOfWork).GetDurationData(durationCode);
                    var t = new TypeBusiness(unitOfWork).GetTypeData(type);
                    var u = new UrgentBusiness(unitOfWork).GetUrgentData(urgentCode);

                    appointment = new AppointmentRepository(this.unitOfWork).GetAppointmentsData(appId);
                    appointment.clinic = c;
                    appointment.specialty = s;
                    appointment.patient = p;
                    appointment.duration = d;
                    appointment.appoint_type = t;
                    appointment.urgent_code = u;
                    appointment.start_time = start;
                    appointment.end_time = end;
                }
                catch (DatabaseExcception e)
                {
                    throw new DatabaseExcception("Cannot get the appointment data!");
                }
               

                try
                {
                    new AppointmentRepository(this.unitOfWork).UpdateApp(appointment);
                }
                catch (DatabaseExcception e)
                {
                    throw new DatabaseExcception("Cannot update the appointment!");
                }

            }

        }

        public void DeleteApp(int id)
        {
            try
            {
                new AppointmentRepository(this.unitOfWork).DelApp(id);
            }
            catch (DatabaseExcception e)
            {
                throw new DatabaseExcception("Cannot delete the appointment!");
            }
        }

        public Boolean CancelAppointment(int id)
        {
            var business = new AppointmentRepository(this.unitOfWork);
            var app = business.GetAppointmentsData(id);//Will check if the appointment exists.
            if(app.isCancel==1)
                throw new DatabaseExcception("sorry, the appointment is already cancelled.");

            try
            {
                new AppointmentRepository(this.unitOfWork).CancleAppointment(id);
                return true;
            }
            catch (DatabaseExcception e)
            {
                throw new DatabaseExcception("Cannot cancel the appointment!");
            }
            
        }

        public Boolean UndoCancel(int id)
        {
            var repository = new AppointmentRepository(this.unitOfWork);
            Appointment app = GetAppointmentData(id);//Will check if the appointment exists first
            if (checkAvailablity(app.clinic.clinic_code, app.specialty.specialty_code, app.start_time, app.end_time))
            {
                try
                {
                    if(app.isCancel==0)
                        throw new DatabaseExcception("Sorry, the appointment is already re-actived!");
                    repository.UndoCancel(app);
                    return true;
                }
                catch (DatabaseExcception e)
                {
                    throw new DatabaseExcception("Cannot undo the cancellation!");
                }
            }
            else
            {
                throw new DatabaseExcception("Cannot undo the cancellation! Time Slots conflicts!");
            }
        }

        /// <summary>
        /// Check if exist timeslots conflict
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Boolean checkAvailablity(String clinicId, String specialtyId, DateTime start, DateTime end)
        {
   

            var repository = new AppointmentRepository(this.unitOfWork);
            //1. reach the appointment list with the clinic id and specialty_id which are not cancelled;
            List<Appointment> list = repository.getListWithCSD(clinicId, specialtyId, start);
        
            foreach (var ll in list)
            {
                DateTime llStart = ll.start_time;
                DateTime llEnd = ll.end_time;
                if (start.CompareTo(llStart) >= 0 && start.CompareTo(llEnd) < 0 
                    || end.CompareTo(llStart)> 0 && end.CompareTo(llEnd) <= 0)
                {
                    return false;
                }
            }
            return true;
        }

        public ITimePeriodCollection getAvailableTimeslots(TimePeriodCollection periods, DateTime dd, String durationId)
        {
            TimePeriodCombiner<TimeRange> periodCombiner = new TimePeriodCombiner<TimeRange>();
            ITimePeriodCollection combinedPeriods = periodCombiner.CombinePeriods(periods);

            //foreach (ITimePeriod combinedPeriod in combinedPeriods)
            //{
            //    Console.WriteLine("Combined Period: " + combinedPeriod);
            //}

            //3. Find te gaps among the periods, also with the gap between the start time and end time

            TimeGapCalculator<TimeRange> gapCalculator = new TimeGapCalculator<TimeRange>(new TimeCalendar());

            ITimePeriod searchLimits = new CalendarTimeRange(
                               new DateTime(dd.Year, dd.Month, dd.Day, 9, 0, 0), new DateTime(dd.Year, dd.Month, dd.Day, 16, 0, 0));
            ITimePeriodCollection freeTimes = gapCalculator.GetGaps(combinedPeriods, searchLimits);

            foreach (ITimePeriod free in freeTimes)
            {
                //Console.WriteLine("Combined Period: " + combinedPeriod);
                System.Diagnostics.Debug.WriteLine("Free Period: " + free);
            }

            ITimePeriodCollection freeTimeSlots = splitTimeRange(freeTimes, durationId);

            return freeTimeSlots;
            //System.Diagnostics.Debug.WriteLine("The int of the Relation 'After':" + (int)PeriodRelation.After);

        }

        private ITimePeriodCollection splitTimeRange(ITimePeriodCollection freetimes, string duration)
        {
            ITimePeriodCollection freeTimeslots = new TimePeriodCollection();

            foreach (var period in freetimes)
            {
                // for each free period, first determine if the period duraion >= user selected duration
                var diff = new DateDiff(period.Start,period.End).Minutes;
                int du = Int32.Parse(duration);//duration int, minutes
                if (diff >= du)
                {
                    for (int i = 0;; i=i+5) // here set the rules that difference between timeslot start time is 5 minutes
                    {
                        DateTime start = period.Start.AddMinutes(i); // free start = period.start + set timeslot difference
                        DateTime end = start.AddMinutes(du);
                        if (end.CompareTo(period.End) <= 0) // If the end of the timeslot is already later than the period end, then end.
                        {
                            ITimePeriod timeslot = new TimeRange(start,end);
                            freeTimeslots.Add(timeslot);
                        }

                        if (start.AddMinutes(du) >= period.End)
                            // if the timeslot start+duration >= period end, then finish the loop
                        {
                            break;
                        }
                    }

                }
                
            }

            return freeTimeslots;
        }

        public List<Appointment> getListWithCS(String clinicId, String specialtyId)
        { 
            var app =  new AppointmentRepository(this.unitOfWork).getListWithCS(clinicId, specialtyId);
            if(app.Count==0)
                throw new DatabaseExcception("There exist no appointments data.");

            return app;
        }

        public List<Appointment> getListWithCSD(String clinicId, String specialtyId, DateTime dd)
        {
            var app = new AppointmentRepository(this.unitOfWork).getListWithCSD(clinicId, specialtyId, dd);
            if (app.Count == 0)
                throw new DatabaseExcception("There exist no appointments data.");

            return app;
        }


        public List<Appointment> getListByPatientAsc()
        {
            return new AppointmentRepository(this.unitOfWork).getAppListByPatientIdAsc();
        }

        public List<Appointment> getListByPatientDesc()
        {
            return new AppointmentRepository(this.unitOfWork).getAppListByPatientIdDesc();
        }

        public List<Appointment> getListByClinicAsc()
        {
            return new AppointmentRepository(this.unitOfWork).getAppListByClinicAsc();
        }

        public List<Appointment> getListByClinicDesc()
        {
            return new AppointmentRepository(this.unitOfWork).getAppListByClinicDesc();
        }


        public List<Appointment> getListBySpecialtyAsc()
        {
            return new AppointmentRepository(this.unitOfWork).getAppListBySpecialtyAsc();
        }

        public List<Appointment> getListBySpecialtyDesc()
        {
            return new AppointmentRepository(this.unitOfWork).getAppListBySpecialtyDesc();
        }

        public List<Appointment> getListByDurationAsc()
        {
            return new AppointmentRepository(this.unitOfWork).getAppListByDurationAsc();
        }

        public List<Appointment> getListByDurationDesc()
        {
            return new AppointmentRepository(this.unitOfWork).getAppListByDurationDesc();
        }

        public List<Appointment> getListByUrgentAsc()
        {
            return new AppointmentRepository(this.unitOfWork).getAppListByUrgentAsc();
        }

        public List<Appointment> getListByUrgentDesc()
        {
            return new AppointmentRepository(this.unitOfWork).getAppListByUrgentDesc();
        }

        public List<Appointment> getListByTypeAsc()
        {
            return new AppointmentRepository(this.unitOfWork).getAppListByTypeAsc();
        }

        public List<Appointment> getListByTypeDesc()
        {
            return new AppointmentRepository(this.unitOfWork).getAppListByTypeDesc();
        }

        public List<Appointment> getListByTimeAsc()
        {
            return new AppointmentRepository(this.unitOfWork).getAppListByTimeAsc();
        }

        public List<Appointment> getListByTimeDesc()
        {
            return new AppointmentRepository(this.unitOfWork).getAppListByTimeDesc();
        }
        
        public List<Appointment> getListByStatusAsc()
        {
            return new AppointmentRepository(this.unitOfWork).getAppListByStatusAsc();
        }

        public List<Appointment> getListByStatusDesc()
        {
            return new AppointmentRepository(this.unitOfWork).getAppListBystatusDesc();
        }

        public List<Appointment> getListByPNameAsc()
        {
            return new AppointmentRepository(this.unitOfWork).getAppListByPNameAsc();
        }

        public List<Appointment> getListByPNameDesc()
        {
            return new AppointmentRepository(this.unitOfWork).getAppListByPNameDesc();
        }

        public List<Appointment> filterAppByPatientName(string patientname)
        {
            return new AppointmentRepository(this.unitOfWork).filterAppByPatient(patientname);
        }

        public List<Appointment> filterAppByClinicName(string clinicname)
        {
            return new AppointmentRepository(this.unitOfWork).filterAppByClinic(clinicname);
        }

        public List<Appointment> filterAppByDaterange(DateTime start1, DateTime end1)
        {
            return new AppointmentRepository(this.unitOfWork).filterAppBydate(start1, end1);
        }
    }
}
