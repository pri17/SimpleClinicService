using System;
using System.ServiceModel;
using ClinicFront.AppointmentReference;
using ClinicFront.ClinicReference;
using ClinicFront.DurationReference;
using ClinicFront.PatientReference;
using ClinicFront.SpecialtyReference;
using ClinicFront.TypeReference;
using ClinicFront.UrgentReference;
using System.Web.UI;

namespace ClinicFront
{
    public partial class Booking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetLists();
            }
        }

        private void GetLists()
        {
            PatientReference.PatientServiceClient pService = new PatientServiceClient();
            var plist = pService.getPatinentLists();
            patientList.DataSource = plist;
            patientList.DataTextField = "full_name";
            patientList.DataValueField = "id";
            patientList.DataBind();

            SpecialtyReference.SpecialtyServiceClient sService = new SpecialtyServiceClient();
            var sList = sService.getAllSpecialty();
            specialtyList.DataSource = sList;
            specialtyList.DataTextField = "desp";
            specialtyList.DataValueField = "code";
            specialtyList.DataBind();

            //ClinicReference.ClinicServiceClient cService = new ClinicServiceClient();
            //var cList = cService.displayClinicsWithSpecialty(specialtyList.SelectedValue);
            //clinicList.DataSource = cList;
            //clinicList.DataTextField = "desp";
            //clinicList.DataValueField = "code";
            //clinicList.DataBind();

            DurationReference.DurationServiceClient dService = new DurationServiceClient();
            var dList = dService.getDurationList();
            durationList.DataSource = dList;
            durationList.DataTextField = "desp";
            durationList.DataValueField = "code";
            durationList.DataBind();

            //AppointmentReference.AppointmentServiceClient appService = new AppointmentServiceClient();
            //var timeslots = appService.getFreePeriodsNew(specialtyList.SelectedValue, clinicList.SelectedValue,
            //    Calendar1.SelectedDate, durationList.SelectedValue);
            //timeList.DataSource = timeslots;
            //timeList.DataTextField = "time";
            //timeList.DataValueField = "time";
            //timeList.DataBind();

            TypeReference.TypeServiceClient tService = new TypeServiceClient();
            var types = tService.getTypeList();
            typeList.DataSource = types;
            typeList.DataTextField = "desp";
            typeList.DataValueField = "type";
            typeList.DataBind();

            UrgentReference.UrgentServiceClient uService = new UrgentServiceClient();
            var codes = uService.getCodeList();
            urgentList.DataSource = codes;
            urgentList.DataTextField = "desp";
            urgentList.DataValueField = "code";
            urgentList.DataBind();

        }

        public void clinic_change(Object sender, EventArgs e)
        {
            clinicList.Items.Clear();

            ClinicFront.ClinicReference.ClinicServiceClient cService = new ClinicServiceClient();
            var cList = cService.displayClinicsWithSpecialty(specialtyList.SelectedValue);
            clinicList.DataSource = cList;
            clinicList.DataTextField = "desp";
            clinicList.DataValueField = "code";
            clinicList.DataBind();

        }

        public void timeslots_change1(Object sender, EventArgs e)
        {
            timeList.Items.Clear();

        }

        public void timeslots_change2(Object sender, EventArgs e)
        {
            timeList.Items.Clear();

            ClinicFront.AppointmentReference.AppointmentServiceClient appService = new AppointmentServiceClient();
            var timeslots = appService.getFreePeriodsNew(specialtyList.SelectedValue, clinicList.SelectedValue,
                Calendar1.SelectedDate, durationList.SelectedValue);
            timeList.DataSource = timeslots;
            timeList.DataTextField = "time";
            timeList.DataValueField = "time";
            timeList.DataBind();
        }

        public void Booking_submit(Object sender, EventArgs e)
        {
            // here click to submit data and do the actual inserting

            string start_time, end_time;

            AppointmentServiceClient appService = new AppointmentServiceClient();
            if (timeList.SelectedIndex != null)
            {
                string[] times = timeList.SelectedValue.Split('-');

                start_time = times[0];
                end_time = times[1];
            }
            else
            {
                start_time = "";
                end_time = "";
            }

            //string path = Server.MapPath("~/log.txt"); 
            //Console.SetOut(File.CreateText(path));

            var date = Calendar1.SelectedDate;
            //Console.WriteLine("Date:" + date);


            var ss = date.ToShortDateString();
            //Console.WriteLine("Date + start_time: " + ss+" "+start_time);
            //Console.WriteLine("Date + end_time:"+ ss+" "+end_time);

            DateTime d1 = Convert.ToDateTime(ss + " " + start_time);
            DateTime d2 = Convert.ToDateTime(ss + " " + end_time);

            //Console.WriteLine(d1);
            //Console.WriteLine(d2);
            //Console.Out.Flush();

            try
            {
                appService.InsertAppointment(int.Parse(patientList.SelectedValue), clinicList.SelectedValue,
                    specialtyList.SelectedValue
                    , durationList.SelectedValue, urgentList.SelectedValue, d1, d2, typeList.SelectedValue);

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage",
                    @"alert('The Appointment is successfully booked!')", true);

            }catch(FaultException <DatabaseExceptionContract> exp)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage",
                     @"alert('" + exp.Detail.Message + "')", true);
            }

        }
    
    }
}