<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Editing.aspx.cs" Inherits="ClinicFront.Editing" %>
<%@ Import Namespace="ClinicFront.AppointmentReference" %>
<%@ Import Namespace="ClinicFront.ClinicReference" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    
    <!-- Breadcrumbs-->
    <ol class="breadcrumb">
        <li class="breadcrumb-item">
            <a href="#">Appointment Lists</a>
        </li>
        <li class="breadcrumb-item active">Editing</li>
    </ol>


    <div class="container">

        <div class="card mb-3">
            <div class="card-header">
                <i class="fas fa-beer"></i>

                <div class="card-body">
                    <div class="table-responsive">

                        <form runat="server">
                            <div>
                                <label>Patients:</label>
                                <asp:DropDownList ID="patientList"
                                    AutoPostBack="True"
                                    runat="server">
                                </asp:DropDownList>

                            </div>

                            <div>
                                <label>Specialty:</label>
                                <asp:DropDownList ID="specialtyList"
                                    AutoPostBack="True"
                                    OnSelectedIndexChanged="clinic_change"
                                    runat="server" AppendDataBoundItems="false">
                                </asp:DropDownList>

                            </div>

                            <div>
                                <label>Clinics:</label>
                                <asp:DropDownList ID="clinicList"
                                    AutoPostBack="True"
                                    OnSelectedIndexChanged="timeslots_change1"
                                    runat="server" AppendDataBoundItems="false">
                                </asp:DropDownList>

                            </div>

                            <div>
                                <label>Duration:</label>
                                <asp:DropDownList ID="durationList"
                                    AutoPostBack="True"
                                    OnSelectedIndexChanged="timeslots_change2"
                                    runat="server">
                                </asp:DropDownList>

                            </div>

                            <div>
                                <label>Appointment Date:</label>
                                <asp:Calendar ID="Calendar1"
                                    ShowGridLines="True"
                                    ShowTitle="True"
                                    runat="server"
                                    SelectedDate="2019-06-17"
                                    OnSelectionChanged="timeslots_change1" />
                            </div>

                            <div>
                                <label>Available timeslots:</label>

                                <asp:DropDownList ID="timeList"
                                    AutoPostBack="True"
                                    runat="server" AppendDataBoundItems="false">
                                </asp:DropDownList>

                            </div>

                            <div>
                                <label>Appointment Types:</label>

                                <asp:DropDownList ID="typeList"
                                    AutoPostBack="True"
                                    runat="server">
                                </asp:DropDownList>

                            </div>


                            <div>
                                <label>Urgent Type:</label>

                                <asp:DropDownList ID="urgentList"
                                    AutoPostBack="True"
                                    runat="server">
                                </asp:DropDownList>

                            </div>

                            <asp:Button ID="book" runat="server" Text="Update" OnClick="Editing_submit" />

                        
                        </form>
                    </div>
                </div>
            </div>
            <!-- /.container-fluid -->

        </div>

    </div>
</asp:Content>

<script runat="server">
    



</script>
