<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AppList.aspx.cs" Inherits="ClinicFront.AppList" %>

<%@ Import Namespace="System.Activities.Statements" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.ServiceModel" %>
<%@ Import Namespace="ClinicFront.AppointmentReference" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Breadcrumbs-->
    <ol class="breadcrumb">
        <li class="breadcrumb-item">
            <a href="#">Appointment Lists</a>
        </li>
        <li class="breadcrumb-item active">Overview</li>
    </ol>

    <div class="container">

        <div class="card mb-3">
            <div class="card-header">
                <i class="fas fa-table"></i>

                <div class="card-body">
                    <div class="table-responsive">

                        <form runat="server">

                            <!--Sort Panel-->
                            <div class="blockquote">
                                <div>
                                    <label><b>Sort By</b>: </label>
                                </div>
                                <div class="blockquote">
                                    <asp:DropDownList ID="sortList"
                                        AutoPostBack="True"
                                        runat="server">

                                        <asp:ListItem Value="date" Selected="True"> Appointment Date </asp:ListItem>
                                        <asp:ListItem Value="pId"> Patient ID </asp:ListItem>
                                        <asp:ListItem Value="pName"> Patient Name </asp:ListItem>
                                        <asp:ListItem Value="cId"> Clinic Name </asp:ListItem>
                                        <asp:ListItem Value="sId"> Specialty Name </asp:ListItem>
                                        <asp:ListItem Value="dId"> Duration </asp:ListItem>
                                        <asp:ListItem Value="uId"> Urgent Level </asp:ListItem>
                                        <asp:ListItem Value="tId"> Appointment Type </asp:ListItem>
                                        <asp:ListItem Value="status"> Appointment Status </asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="blockquote">
                                    <asp:RadioButtonList ID="whatorder" runat="server" RepeatLayout="Flow">
                                        <asp:ListItem Value="a" Selected="True">Ascending</asp:ListItem>
                                        <asp:ListItem Value="d">Desending</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <%--    <asp:RadioButton ID="asc" runat="server" Text="Ascending" Checked="true" GroupName="order" />
                                <asp:RadioButton ID="desc" runat="server" Text="Desending" GroupName="order" />--%>
                                <div class="blockquote">
                                    <asp:Button runat="server" Text="Apply" class="btn-dark" OnClick="sort_click" ID="applying" />
                                </div>



                                <!--filter panel-->
                                <div class="blockquote">
                                    <div>
                                        <label><b>Filter By</b>: </label>
                                    </div>
                                    <div class="blockquote">
                                        <asp:DropDownList runat="server" ID="filterlist" AutoPostBack="True" OnSelectedIndexChanged="calender_change">
                                            <asp:ListItem Value="patientName" Selected="True"> Patient Name </asp:ListItem>
                                            <asp:ListItem Value="ClinicName"> Clinic Name </asp:ListItem>
                                            <asp:ListItem Value="dateRange"> Date Range</asp:ListItem>
                                        </asp:DropDownList>

                                        <input id="filterString" runat="server" />
                                        <asp:Button runat="server" Text="Apply" class="btn-dark" OnClick="filter_click" ID="filtering" />
                                    </div>
                                    <div>
                                    <asp:Calendar runat="server" ID ="startCan" style="display: none" ></asp:Calendar>
                                    <asp:Calendar runat="server" ID="endCan" style="display: none" OnSelectionChanged="check_change"></asp:Calendar>
                                    </div>
                                </div>
                            </div>

                            <asp:Repeater ID="Repeater1" runat="server">
                                <HeaderTemplate>
                                    <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
                                        <thead>
                                            <tr>
                                                <th>Appointment ID</th>
                                                <th>Patient ID</th>
                                                <th>Patient name</th>
                                                <th>Clinic</th>
                                                <th>Specialty</th>

                                                <th>Duration</th>
                                                <th>Urgent Type</th>
                                                <th>Appointment Type</th>
                                                <th>Start Time</th>
                                                <th>End Time</th>
                                                <th>Patient Phone</th>
                                                <th>Patient email</th>
                                                <th>Cancelled? </th>
                                                <th>Opertaions</th>
                                            </tr>

                                        </thead>
                                </HeaderTemplate>


                                <ItemTemplate>

                                    <tbody>
                                        <tr>
                                            <td><%#Eval("appointmentId") %></td>
                                            <td><%#Eval("patientID") %></td>
                                            <td><%#Eval("name") %></td>
                                            <td><%#Eval("clinicDesp") %></td>
                                            <td><%#Eval("specialtyDesp") %></td>

                                            <td><%#Eval("durationDesp") %></td>
                                            <td><%#Eval("urgentDesp") %></td>
                                            <td><%#Eval("appointType") %></td>
                                            <td><%#Eval("start_time") %></td>
                                            <td><%#Eval("end_time") %></td>
                                            <td><%#Eval("phone") %></td>
                                            <td><%#Eval("email") %></td>
                                            <td><%#Eval("isCancel") %></td>
                                            <td>
                                                <asp:Button ID="editing" runat="server" Text="Edit" CssClass="btn-info"
                                                    CommandArgument='<%#Eval("appointmentId") + ","+Eval("patientID") +
                                                ","  + Eval("clinicId")+ ","  + Eval("specialtyId") + "," + Eval("durationCode") + 
                                                ","  + Eval("urgentCode")+  ","  +Eval("start_time") + "," + Eval("end_time") +
                                                "," + Eval("appointTypeCode")%>'
                                                    OnCommand="edit_click" />

                                                <asp:Button runat="server" Text="Cancel" Visible='<%# Eval("isCancel").ToString() == "Active"  %>' CssClass="btn-outline-danger"
                                                    CommandArgument='<%#Eval("appointmentId") %>' OnCommand="cancel_click" OnClientClick="return confirmCancel(this)" UseSubmitBehavior="false" />
                                                <asp:Button runat="server" Text="UndoCancel" Visible='<%# Eval("isCancel").ToString() == "Cancelled" %>' CssClass="btn-outline-primary"
                                                    CommandArgument='<%#Eval("appointmentId") %>' OnCommand="undoCan_click" OnClientClick="return confirm('Are you sure to undo the cancellation?');" />


                                                <%-- <asp:Button runat="server" ID="cancelbtn" Text="Cancel" Visible='<%# Eval("isCancel").ToString() == "Active"  %>'></asp:Button> 
                                               <asp:Button runat="server" ID="undobtn" Text="UndoCancel" Visible='<%# Eval("isCancel").ToString() == "Cancelled"  %>'></asp:Button>--%>
                                            </td>
                                        </tr>
                                    </tbody>

                                </ItemTemplate>

                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>


                            <div id="dialog-confirm">
                            </div>

                        </form>
                    </div>
                </div>
            </div>
            <!-- /.container-fluid -->

        </div>

    </div>

    <script type="text/javascript">

        (function ($) {
            'use strict';
            function popup_click(id) {
                $("#dialog-confirm").text(id);
                $("#dialog-confirm").dialog({
                    resizable: false,
                    height: "auto",
                    width: 400,
                    modal: true,
                    buttons: {
                        "OK": function() {
                            $(this).dialog("close");
                            __doPostBack(control.name, ''); 
                        },
                        "Close": function() {
                            $(this).dialog("close");
                        }
                    }
                });
            }

            window.popup_click = popup_click;
        })(jQuery);

            function confirmCancel(control) {
                $("#dialog-confirm").text("Are you sure to cancel the appointment?");
                $("#dialog-confirm").dialog({
                    resizable: false,
                    height: "auto",
                    width: 400,
                    modal: true,
                    buttons: {
                        "OK": function() {
                            $(this).dialog("close");
                            __doPostBack(control.name, ''); // or __doPostBack(control.name, '')
                        },
                        "Close": function() {
                            $(this).dialog("close");
                        }
                    }
                });
            }


    </script>
    
    <script runat="server">
        
    public void undoCan_click(Object sender, CommandEventArgs e)
    {
        string appId = e.CommandArgument.ToString();
        AppointmentServiceClient appService = new AppointmentServiceClient();
        if (appId != null)
        {
            try
            {
                appService.UndoCancelAppointment(int.Parse(appId));

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage",
                    @"alert('The Appointment is active again!');window.location='AppList.aspx'", true);

            }
            catch (FaultException<DatabaseExceptionContract> exp)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "jQuery", 
                                @" $('#dialog-confirm').text('" + exp.Detail.Message + "');$('#dialog-confirm').dialog(); ", true);
            }
            catch (FaultException f)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage",
                    @"alert('Caught general fault exception: {0}',f.Message);window.location='AppList.aspx'", true);
                //Console.WriteLine("Caught general fault exception: {0}", f.Message);
            }
            catch (Exception expp)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage",
                    @"alert('Got general exception: {0}',expp.Message);window.location='AppList.aspx'", true);
                //Console.WriteLine("Got general exception: {0}", expp.Message);
            }

        }

    }

    
    </script>

</asp:Content>


