using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Core;
using NHibernate.Impl;
using NHibernate.Linq;
using SampleDomain;

namespace SampleRepository
{
    public class AppointmentRepository
    {
        private readonly IUnitOfWork unitOfWork;

        public AppointmentRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get one Appointment record
        /// </summary>
        /// <param name="id">appointment id</param>
        /// <returns></returns>
        public Appointment GetAppointmentsData(int id)
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Where(x => x.appoint_id == id)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList()
                .FirstOrDefault();
        }

        public List<Appointment> GetAppintmentList()
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }

        public List<Appointment> getListWithCS(String clincId, String SpecialtyId)
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .Where(x => x.clinic.clinic_code == clincId)
                .Where(x => x.specialty.specialty_code == SpecialtyId)
                .Where(x => x.isCancel == 0)
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }

        public List<Appointment> getListWithCSD(String clincId, String SpecialtyId, DateTime dd)
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .Where(x => x.clinic.clinic_code == clincId)
                .Where(x => x.specialty.specialty_code == SpecialtyId)
                .Where(x => x.start_time.Date == dd.Date)
                .Where(x => x.isCancel == 0)
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }

        public void InsertAppointment(Appointment app)
        {

            var appointmentToSave = new Appointment(app.patient, app.clinic, app.specialty, app.duration, app.urgent_code, app.start_time,
                app.end_time, app.appoint_type);
            this.unitOfWork.Session.Save(appointmentToSave);
            //this.unitOfWork.Session.Query<Appointment>()
            //    .InsertInto(x => new Appointment{appoint_id = app.appoint_id, patient = app.patient, clinic = app.clinic,
            //        specialty = app.specialty, duration = app.duration, urgent_code = app.urgent_code,
            //        start_time = app.start_time, end_time = app.end_time, appoint_type = app.appoint_type});

        }

        public void UpdateApp(Appointment app)
        {

            this.unitOfWork.Session.Update(app);

        }

        public void DelApp(int id)
        {

            this.unitOfWork.Session.Query<Appointment>()
                .Where(x => x.appoint_id == id)
                .Delete();

        }

        public void CancleAppointment(int id)
        {

            var app = this.unitOfWork.Session.Query<Appointment>()
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Where(x => x.appoint_id == id)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList()
                .FirstOrDefault();

            app.isCancel = 1;
            this.unitOfWork.Session.Update(app);
            this.unitOfWork.Session.Transaction.Commit();
            }

        public void UndoCancel(Appointment appointment)
        {


            appointment.isCancel = 0;
            this.unitOfWork.Session.Update(appointment);
            this.unitOfWork.Session.Transaction.Commit();

        }

        /// <summary>
        /// sort by patientID
        /// </summary>
        /// <returns></returns>
        public List<Appointment> getAppListByPatientIdAsc()
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .OrderBy(x => x.patient.patient_id)
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }

        public List<Appointment> getAppListByPatientIdDesc()
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .OrderByDescending(x => x.patient.patient_id)
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }

        /// <summary>
        /// sort by clinic name
        /// </summary>
        /// <returns></returns>
        public List<Appointment> getAppListByClinicAsc()
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .OrderBy(x => x.clinic.desp)
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }

        public List<Appointment> getAppListByClinicDesc()
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .OrderByDescending(x => x.clinic.desp)
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }

        /// <summary>
        /// sort by specialty name
        /// </summary>
        /// <returns></returns>
        public List<Appointment> getAppListBySpecialtyAsc()
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .OrderBy(x => x.specialty.desp)
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }

        public List<Appointment> getAppListBySpecialtyDesc()
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .OrderByDescending(x => x.specialty.desp)
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }

        /// <summary>
        /// sort by duration 
        /// </summary>
        /// <returns></returns>
        public List<Appointment> getAppListByDurationAsc()
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .OrderBy(x => x.duration.duration_code.Length)
                .ThenBy(x=> x.duration.duration_code)
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }

        public List<Appointment> getAppListByDurationDesc()
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .OrderByDescending(x => x.duration.duration_code.Length)
                .ThenByDescending(x=>x.duration.duration_code)
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }

        /// <summary>
        /// sort by urgent level
        /// </summary>
        /// <returns></returns>
        public List<Appointment> getAppListByUrgentAsc()
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .OrderBy(x => x.urgent_code.urgent_code)
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }

        public List<Appointment> getAppListByUrgentDesc()
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .OrderByDescending(x => x.urgent_code.urgent_code)
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }

        /// <summary>
        /// sort by appointment type
        /// </summary>
        /// <returns></returns>
        public List<Appointment> getAppListByTypeAsc()
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .OrderBy(x => x.appoint_type.appointment_type)
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }

        public List<Appointment> getAppListByTypeDesc()
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .OrderByDescending(x => x.appoint_type.appointment_type)
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }

        /// <summary>
        /// sort by timeslot 
        /// </summary>
        /// <returns></returns>
        public List<Appointment> getAppListByTimeAsc()
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .OrderBy(x => x.start_time)
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }

        public List<Appointment> getAppListByTimeDesc()
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .OrderByDescending(x => x.start_time)
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }

        /// <summary>
        /// sort by appointmetn state
        /// </summary>
        /// <returns></returns>
        public List<Appointment> getAppListByStatusAsc()
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .OrderBy(x => x.isCancel)
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }

        public List<Appointment> getAppListBystatusDesc()
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .OrderByDescending(x => x.isCancel)
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }

        /// <summary>
        /// sort by patient name
        /// </summary>
        /// <returns></returns>
        public List<Appointment> getAppListByPNameAsc()
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .OrderBy(x => x.patient.first_name)
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }

        public List<Appointment> getAppListByPNameDesc()
        {
            return this.unitOfWork.Session.Query<Appointment>()
                .OrderByDescending(x => x.patient.first_name)
                .Fetch(x => x.appoint_type)
                .Fetch(x => x.patient)
                .Fetch(x => x.clinic)
                .Fetch(x => x.specialty)
                .Fetch(x => x.urgent_code)
                .Fetch(x => x.duration)
                .ToList();
        }


        public List<Appointment> filterAppByPatient(String patientName)
        {
            return this.unitOfWork.Session.Query<Appointment>()
               .Where(x=>x.patient.full_name.ToLower().Contains(patientName.ToLower()))
               .Fetch(x => x.appoint_type)
               .Fetch(x => x.patient)
               .Fetch(x => x.clinic)
               .Fetch(x => x.specialty)
               .Fetch(x => x.urgent_code)
               .Fetch(x => x.duration)
               .ToList();
        }
        public List<Appointment> filterAppByClinic(String clinicname)
        {
            return this.unitOfWork.Session.Query<Appointment>()
               .Where(x => x.clinic.desp.ToLower().Contains(clinicname.ToLower()))// case insensitive comparison
               .Fetch(x => x.appoint_type)
               .Fetch(x => x.patient)
               .Fetch(x => x.clinic)
               .Fetch(x => x.specialty)
               .Fetch(x => x.urgent_code)
               .Fetch(x => x.duration)
               .ToList();
        }

        public List<Appointment> filterAppBydate(DateTime start1, DateTime end1)
        {
            return this.unitOfWork.Session.Query<Appointment>()
               .Fetch(x => x.appoint_type)
               .Fetch(x => x.patient)
               .Fetch(x => x.clinic)
               .Fetch(x => x.specialty)
               .Fetch(x => x.urgent_code)
               .Fetch(x => x.duration)
               .Where(x => x.start_time.CompareTo(start1) >= 0 && x.end_time.CompareTo(end1) <= 0)
               .ToList();
        }

    }
}
