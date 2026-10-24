<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="NotCompletedFlights.aspx.cs" Inherits="prjApplication.FinishFlights.NotCompletedFlights" %>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/bootstrap-datepicker3.min.css" rel="stylesheet" />
    
    <style type="text/css">
        #dhtmltooltip {
            position: absolute;
            width: 150px;
            border: 2px solid black;
            padding: 2px;
            background-color: lightyellow;
            visibility: hidden;
            z-index: 100;
            /*Remove below line to remove shadow. Below line should always appear last within this CSS*/
            filter: progid:DXImageTransform.Microsoft.Shadow(color=gray,direction=135);
        }
    </style>
    
    <span class="TitlePanel">+ NOT COMPLETED FLIGHTS</span>
    <div class="well well-sm">
        <div class="form-inline" role="form">
            <div class="form-group">
                <div class="form-horizontal" role="form">
                    <div class="form-group">
                        <label for="txtStartDate" class="mLable control-label">Start date</label>
                        <input id="txtStartDate" runat="server" data-date-format="dd/mm/yyyy"
                            class="inputControl date-picker mControl"
                            type="text" />
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="form-horizontal" role="form">
                    <div class="form-group">
                        <label for="txtFinishDate" class="mLable control-label">Finish date</label>
                        <input id="txtFinishDate" runat="server" data-date-format="dd/mm/yyyy"
                            class="inputControl date-picker mControl"
                            type="text" />
                    </div>
                </div>
            </div>
            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
        </div>
    </div>
    <div class="table-responsive">
        <asp:GridView ID="grdSource" runat="server" CssClass="table table-bordered table-hover"
            AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="No">
                    <ItemTemplate>
                        <%# Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="FLIGHT_ID" HeaderText="FLIGHT_ID" />
                <asp:BoundField DataField="FLIGHT_PK" HeaderText="FLIGHT_PK"/>
                <asp:BoundField DataField="PERM_ID" HeaderText="PERM_ID" />
                <asp:BoundField DataField="PERMNBR" HeaderText="PERMNBR" />                
                <asp:BoundField DataField="PERMTYPE" HeaderText="PERMTYPE" />
                <asp:BoundField DataField="FLIGHT_TYPE" HeaderText="FLIGHT_TYPE" />
                <asp:BoundField DataField="PURPOSE" HeaderText="PURPOSE" />
                <asp:BoundField DataField="CRAFT_ID" HeaderText="CRAFT_ID" />
                <asp:BoundField DataField="MTOW" HeaderText="MTOW" />
                <asp:BoundField DataField="VALIDHOURS" HeaderText="VALIDHOURS" />
                <asp:BoundField DataField="DATE_OLD" HeaderText="DATE_OLD" />
                <asp:BoundField DataField="FLIGHTDATE" HeaderText="FLIGHTDATE" />
                <asp:BoundField DataField="FLIGHTNBR" HeaderText="FLIGHTNBR" />
                <asp:BoundField DataField="REGISTRATION" HeaderText="REGISTRATION" />
                <asp:BoundField DataField="FROM_AIRP" HeaderText="FROM_AIRP"/>
                <asp:BoundField DataField="TO_AIRP" HeaderText="TO_AIRP" />
                <asp:BoundField DataField="ETD" HeaderText="ETD" />
                <asp:BoundField DataField="ETA" HeaderText="ETA" />
                <asp:BoundField DataField="ATD" HeaderText="ATD" />
                <asp:BoundField DataField="ATA" HeaderText="ATA" />
                <asp:BoundField DataField="VIA" HeaderText="VIA" />         
                <asp:BoundField DataField="LETTERNBR_PK" HeaderText="LETTERNBR_PK" />
                <asp:BoundField DataField="NBR" HeaderText="NBR" />
                <asp:BoundField DataField="OPER_ID" HeaderText="OPER_ID" />
                <asp:BoundField DataField="CRAFT_TYPE" HeaderText="CRAFT_TYPE" />
                <asp:BoundField DataField="REMARK" HeaderText="REMARK" />                
            </Columns>
        </asp:GridView>
    </div>
    <div style="text-align: right">
        <cc1:CustomPaging ID="CustomPaging1" runat="server" NumberViewPage="7" PageSize="10" />
    </div>

    <div id="popupEditFlightDetail" class="modal fade" role="dialog" tabindex="-1" data-backdrop="false"
        aria-hidden="true">
        <div class="modal-dialog wid_90">
            <div class="modal-content">
                <%--<div class="modal-header">
                    <button type="button" class="close"
                        data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">CONTENT</h4>
                </div>--%>
                <div class="modal-body">
                    <div class="row">
                        <div id="vContent"></div>
                    </div>


                    <div class="row" style="text-align: center">
                        <button id="btnCancel" type="button"
                            data-dismiss="modal" class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-times"></i>
                            Cancel
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/validate.js"></script>
    <div id="dhtmltooltip"></div>
    <script type="text/javascript">
        var offsetxpoint = -60 //Customize x offset of tooltip
        var offsetypoint = 20 //Customize y offset of tooltip
        var ie = document.all
        var ns6 = document.getElementById && !document.all
        var enabletip = false
        if (ie || ns6)
            var tipobj = document.all ? document.all["dhtmltooltip"] : document.getElementById ? document.getElementById("dhtmltooltip") : ""
        document.body.appendChild(tipobj)

        function ietruebody() {
            return (document.compatMode && document.compatMode != "BackCompat") ? document.documentElement : document.body
        }

        function ddrivetip(thetext, thecolor, thewidth) {
            if (ns6 || ie) {
                if (typeof thewidth != "undefined") tipobj.style.width = thewidth + "px"
                if (typeof thecolor != "undefined" && thecolor != "") tipobj.style.backgroundColor = thecolor
                tipobj.innerHTML = thetext
                enabletip = true
                return false
            }
        }

        function positiontip(e) {
            if (enabletip) {
                var curX = (ns6) ? e.pageX : event.clientX + ietruebody().scrollLeft;
                var curY = (ns6) ? e.pageY : event.clientY + ietruebody().scrollTop;
                //Find out how close the mouse is to the corner of the window
                var rightedge = ie && !window.opera ? ietruebody().clientWidth - event.clientX - offsetxpoint : window.innerWidth - e.clientX - offsetxpoint - 20
                var bottomedge = ie && !window.opera ? ietruebody().clientHeight - event.clientY - offsetypoint : window.innerHeight - e.clientY - offsetypoint - 20

                var leftedge = (offsetxpoint < 0) ? offsetxpoint * (-1) : -1000

                //if the horizontal distance isn't enough to accomodate the width of the context menu
                if (rightedge < tipobj.offsetWidth)
                    //move the horizontal position of the menu to the left by it's width
                    tipobj.style.left = ie ? ietruebody().scrollLeft + event.clientX - tipobj.offsetWidth + "px" : window.pageXOffset + e.clientX - tipobj.offsetWidth + "px"
                else if (curX < leftedge)
                    tipobj.style.left = "5px"
                else
                    //position the horizontal position of the menu where the mouse is positioned
                    tipobj.style.left = curX + offsetxpoint + "px"

                //same concept with the vertical position
                if (bottomedge < tipobj.offsetHeight)
                    tipobj.style.top = ie ? ietruebody().scrollTop + event.clientY - tipobj.offsetHeight - offsetypoint + "px" : window.pageYOffset + e.clientY - tipobj.offsetHeight - offsetypoint + "px"
                else
                    tipobj.style.top = curY + offsetypoint + "px"
                tipobj.style.visibility = "visible"
            }
        }

        function hideddrivetip() {
            if (ns6 || ie) {
                enabletip = false
                tipobj.style.visibility = "hidden"
                tipobj.style.left = "-1000px"
                tipobj.style.backgroundColor = ''
                tipobj.style.width = ''
            }
        }

        document.onmousemove = positiontip

    </script>
    <script>

        function LoadDataGrid() {
            GetArgWithPostBack('LoadDataGrid_____LoadDataGrid', 'LoadDataGrid');
        }
        function DisplayResult(resulf, context) {
            if (context == 'LoadDataGrid') {
                document.getElementById('<%=grdSource.ClientID%>').innerHTML = resulf;
        }
        if (context == 'GetContent') {
            document.getElementById('vContent').innerHTML = resulf;
        }
    }
    function GetContent(id) {
        GetArgWithPostBack(id + '_____GetContent', 'GetContent');
    }
    </script>
</asp:Content>
