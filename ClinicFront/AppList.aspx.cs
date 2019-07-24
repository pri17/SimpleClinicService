using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClinicFront.AppointmentReference;

namespace ClinicFront
{
    public partial class AppList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
             {
                 Getlists();
             }
        }
        private void Getlists()
        {
            AppointmentReference.IAppointmentService app_service = new AppointmentServiceClient();
            var app = app_service.getAllList();
           
            Repeater1.DataSource = app;
            Repeater1.DataBind();

        }

        public void edit_click(Object sender, CommandEventArgs e)
        {

            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string appId = commandArgs[0];

            Session["appId"] = appId;
            Response.Redirect("Editing.aspx");
        }

        public void cancel_click(Object sender, CommandEventArgs e)
        {
            string appId = e.CommandArgument.ToString();
            AppointmentServiceClient appService = new AppointmentServiceClient();
            if (appId != null)
            {

                try
                {
                    appService.CancelAppointment(int.Parse(appId));
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage",
                       @"alert('The Appointment is successfully cancelled!');window.location='AppList.aspx'", true);
                }
                catch (FaultException<DatabaseExceptionContract> exp)
                {

                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage",
                        @"alert('" + exp.Detail.Message + "')", true);
                }
            }

        }

        [WebMethod]
        public String GetErrorData(HttpRequest request)
        {
            string appId = request.Params["id"];
            AppointmentServiceClient appService = new AppointmentServiceClient();
            if (appId != null)
            {
                try
                {
                    appService.UndoCancelAppointment(int.Parse(appId));

                    return "The Appointment is active again!";
                }
                catch (FaultException<DatabaseExceptionContract> exp)
                {
                    return exp.Detail.Message;   
                }
                catch (FaultException f)
                {
                  
                    return "Caught general fault exception: {0}"+ f.Message;
                }
                catch (Exception expp)
                {
                
                    return "Got general exception: {0}"+ expp.Message;
                }

            }
            return "The appointment is invaild.";
        }



        public void sort_click(Object sender, EventArgs e)
        {
            string sortstring = sortList.SelectedValue;
            AppointmentServiceClient appService = new AppointmentServiceClient();

            string ifasc = whatorder.SelectedItem.Value;

            switch (sortstring)
            {
                case "date":
                    if (ifasc == "a")
                        Repeater1.DataSource = appService.getListTimeAsc();
                    else
                        Repeater1.DataSource = appService.getListTimeDesc();
                    Repeater1.DataBind();
                    break;

                case "pId":
                    if (ifasc == "a")
                        Repeater1.DataSource = appService.getListPatientAsc();
                    else
                        Repeater1.DataSource = appService.getListPatientDesc();
                    Repeater1.DataBind();
                    break;

                case "pName":
                    if (ifasc == "a")
                        Repeater1.DataSource = appService.getListPatientNameAsc();
                    else
                        Repeater1.DataSource = appService.getListPatientNameDesc();
                    Repeater1.DataBind();
                    break;

                case "cId":
                    if (ifasc == "a")
                        Repeater1.DataSource = appService.getListClinicAsc();
                    else
                        Repeater1.DataSource = appService.getListClinicDesc();
                    Repeater1.DataBind();
                    break;

                case "sId":
                    if (ifasc == "a")
                        Repeater1.DataSource = appService.getListSpecialtyAsc();
                    else
                        Repeater1.DataSource = appService.getListSpecialtyDesc();
                    Repeater1.DataBind();
                    break;

                case "dId":
                    if (ifasc == "a")
                        Repeater1.DataSource = appService.getListDurationAsc();
                    else
                        Repeater1.DataSource = appService.getListDurationDesc();
                    Repeater1.DataBind();
                    break;

                case "uId":
                    if (ifasc == "a")
                        Repeater1.DataSource = appService.getListUrgentAsc();
                    else
                        Repeater1.DataSource = appService.getListUrgentDesc();
                    Repeater1.DataBind();
                    break;

                case "tId":
                    if (ifasc == "a")
                        Repeater1.DataSource = appService.getListTypeAsc();
                    else
                        Repeater1.DataSource = appService.getListTypeDesc();
                    Repeater1.DataBind();
                    break;

                case "status":
                    if (ifasc == "a")
                        Repeater1.DataSource = appService.getListStatusAsc();
                    else
                        Repeater1.DataSource = appService.getListStatusDesc();
                    Repeater1.DataBind();
                    break;

            }

        }

        public void calender_change(Object sender, EventArgs e)
        {
            if ((startCan.Style["display"] == "none") && (filterlist.SelectedValue.Equals("dateRange")))
            {
                startCan.Style.Remove("display");
                endCan.Style.Remove("display");
            }
            else
            {
                startCan.Style["display"] = "none";
                endCan.Style["display"] = "none";
            }
           
        }

        public void check_change(Object sender, EventArgs e)
        {
            if (endCan.SelectedDate.CompareTo(startCan.SelectedDate) < 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage",
                       @"alert('The end date must not earlier than the start date!')", true);
            }
        }

        public void filter_click(Object sender, EventArgs e)
        {
            string filterby = filterlist.SelectedValue;
            AppointmentServiceClient appService = new AppointmentServiceClient();

            string filterstring = filterString.Value.ToString();
            switch (filterby)
            {
                case "patientName":
                    Repeater1.DataSource = appService.filterByPatientName(filterstring.Trim());
                    Repeater1.DataBind();
                    break;

                case "ClinicName":
                    Repeater1.DataSource = appService.filterByClinicName(filterstring.Trim());
                    Repeater1.DataBind();
                    break;

                case "dateRange":
                    //// two Datetime
                    var startvalue = startCan.SelectedDate;
                    var endvalue = endCan.SelectedDate;
                    Repeater1.DataSource = appService.filterByDaterange(startvalue,endvalue);
                    Repeater1.DataBind();
                    break;

            }
        }

    } 
}