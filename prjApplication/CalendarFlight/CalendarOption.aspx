<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="CalendarOption.aspx.cs" Inherits="prjApplication.CalendarFlight.CalendarOption" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Select option CALENDAR FILGHT</h3>
    <div class="row">
        <div class="col-lg-6">
            <button id="btnCalendarDay" data-toggle="modal" data-target="#popCalendarDay" class="btn btn-primary">
                Calendar day</button>
        </div>
        <div class="col-lg-6">
            <button id="btnCalendarTime" data-toggle="modal" data-target="#popCalendarTime" class="btn btn-primary">Calendar time</button>
        </div>
    </div>
    <div id="popCalendarDay" class="modal fade" role="dialog" tabindex="-1" data-backdrop="false"
        aria-hidden="true">
        <div class="modal-dialog ">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">
                    ×</button>
                <h4 class="blue bigger">Select day</h4>
            </div>
            <div class="modal-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="col-lg-4 control-label" for="txtday">Select</label>
                        <input id="txtday" runat="server" data-date-format="dd/mm/yyyy" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnSelectDay" OnClientClick="checkTimeDay();" OnClick="btnSelectDay_Click" runat="server" class="btn btn-primary" Text="Save" />                    
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                </div>
        </div>
    </div>
    <div id="popCalendarTime" class="modal fade" role="dialog" tabindex="-1" data-backdrop="false"
        aria-hidden="true">
        <div class="modal-dialog ">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">
                    ×</button>
                <h4 class="blue bigger">Select time</h4>
            </div>
            <div class="modal-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="col-lg-4 control-label" for="txtDayStart">Time start</label>
                        <input id="txtDayStart" runat="server" data-date-format="dd/mm/yyyy" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                    </div>
                    <div class="form-group">
                        <label class="col-lg-4 control-label" for="txtDayFinish">Time finish</label>
                        <input id="txtDayFinish" runat="server" data-date-format="dd/mm/yyyy" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnSelectTime" OnClientClick="checkTime();" runat="server" class="btn btn-primary" Text="Save" OnClick="btnSelectTime_Click" />                     
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                </div>
        </div>
    </div>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script>
        function checkTimeDay() {
            if ($('#<%= txtday.ClientID%>').val() == '') {
                alert('Please select day');
                return false;
            }
            return true;
        }
        function checkTime() {
            if ($('#<%= txtDayFinish.ClientID%>').val() == '' && $('#<%= txtDayStart.ClientID%>').val() == '') {
                alert('Please select day');
                return false;
            }                
            return true;
        }
    </script>
</asp:Content>
