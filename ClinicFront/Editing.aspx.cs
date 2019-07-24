using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClinicFront.AppointmentReference;
using ClinicFront.ClinicReference;
using ClinicFront.DurationReference;
using ClinicFront.PatientReference;
using ClinicFront.SpecialtyReference;
using ClinicFront.TypeReference;
using ClinicFront.UrgentReference;

namespace ClinicFront
{
    public partial class Editing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetAppointmentData();
            }
        }

        private void GetAppointmentData()
        {
            String id = Session["appId"].ToString();
            if (id != null)
            {
                AppointmentServiceClient appService = new AppointmentServiceClient();
                AppointmentDataContract con = appService.getAppointmentData(int.Parse(id));

                PatientServiceClient pService = new PatientServiceClient();
                var plist = pService.getPatinentLists();
                patientList.DataSource = plist;
                patientList.DataTextField = "full_name" ;
                patientList.DataValueField = "id";
                patientList.DataBind();
                patientList.Items.Insert(0, con.name);

                SpecialtyServiceClient sService = new SpecialtyServiceClient();
                var sList = sService.getAllSpecialty();
                specialtyList.DataSource = sList;
                specialtyList.DataTextField = "desp";
                specialtyList.DataValueField = "code";
                specialtyList.DataBind();
                specialtyList.Items.Insert(0,con.specialtyDesp);

                DurationServiceClient dService = new DurationServiceClient();
                var dList = dService.getDurationList();
                durationList.DataSource = dList;
                durationList.DataTextField = "desp";
                durationList.DataValueField = "code";
                durationList.DataBind();
                durationList.Items.Insert(0,con.durationDesp);

                TypeServiceClient tService = new TypeServiceClient();
                var types = tService.getTypeList();
                typeList.DataSource = types;
                typeList.DataTextField = "desp";
                typeList.DataValueField = "type";
                typeList.DataBind();
                typeList.Items.Insert(0,con.appointType);

                UrgentServiceClient uService = new UrgentServiceClient();
                var codes = uService.getCodeList();
                urgentList.DataSource = codes;
                urgentList.DataTextField = "desp";
                urgentList.DataValueField = "code";
                urgentList.DataBind();
                urgentList.Items.Insert(0,con.urgentDesp);

                Calendar1.SelectedDate = con.start_time.Date;
                clinicList.Items.Insert(0,con.clinicDesp);
                timeList.Items.Insert(0,con.start_time.TimeOfDay + "-" + con.end_time.TimeOfDay);

            }
        }

        public void clinic_change(Object sender, EventArgs e)
        {
            clinicList.Items.Clear();

            ClinicServiceClient cService = new ClinicServiceClient();
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

            AppointmentServiceClient appService = new AppointmentServiceClient();
            var timeslots = appService.getFreePeriodsNew(specialtyList.SelectedValue, clinicList.SelectedValue,
                Calendar1.SelectedDate, durationList.SelectedValue);
            timeList.DataSource = timeslots;
            timeList.DataTextField = "time";
            timeList.DataValueField = "time";
            timeList.DataBind();
        }

        public void Editing_submit(Object sender, EventArgs e)
        {

            string start_time, end_time;

            AppointmentServiceClient appService = new AppointmentServiceClient();

            var appid = Session["appId"].ToString();
            if (appid != null)
            {
                AppointmentDataContract con = appService.getAppointmentData(int.Parse(appid));

                int patientId;
                if (patientList.SelectedIndex == 0)
                {
                    patientId = con.patientID;
                }
                else
                {
                    patientId = int.Parse(patientList.SelectedValue);
                }

                string cId;
                if (clinicList.SelectedIndex == 0)
                {
                    cId = con.clinicId;
                }
                else
                {
                    cId = clinicList.SelectedValue;
                }

                string sId;
                if (specialtyList.SelectedIndex == 0)
                {
                    sId = con.specialtyId;
                }
                else
                {
                    sId = specialtyList.SelectedValue;
                }


                string dId;
                if (durationList.SelectedIndex == 0)
                {
                    dId = con.durationCode;
                }
                else
                {
                    dId = durationList.SelectedValue;
                }

                string uId;
                if (urgentList.SelectedIndex == 0)
                {
                    uId = con.urgentCode;
                }
                else
                {
                    uId = urgentList.SelectedValue;
                }

                DateTime d1, d2;
                var theDate = Calendar1.SelectedDate.ToShortDateString();

                if (timeList.SelectedIndex == 0)
                {
                    d1 = con.start_time;
                    d2 = con.end_time;
                }
                else
                {

                    string[] times = timeList.SelectedValue.Split('-');

                    start_time = times[0];
                    end_time = times[1];
                    d1 = Convert.ToDateTime(theDate + " " + start_time);
                    d2 = Convert.ToDateTime(theDate + " " + end_time);
                }

                string tId;
                if (typeList.SelectedIndex == 0)
                {
                    tId = con.appointTypeCode;
                }
                else
                {
                    tId = typeList.SelectedValue;
                }


                if (appService.UpdateAppointment(int.Parse(Session["appId"].ToString()), patientId, cId,
                    sId, dId, uId, d1, d2, tId))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('The Appointment is successfully updated!');window.location='AppList.aspx'", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('
                                Sorry, the update failed!')", true);
                }

            }

        }

    }
}