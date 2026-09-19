<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true"
    CodeBehind="DayFlightNotRoute.aspx.cs" Inherits="prjApplication.Day_Flights.DayFlightNotRoute" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="prjBusinessLogic" %>
<%@ Import Namespace="prjInfo" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />

    <h3 class="page-header">Flight not permission</h3>
    <style>
        .sControl {
            height: 23px;
            width: 80px;
        }

            .sControl:focus {
                width: 200px;
            }

        #grdSource select {
            height: 23px;
            width: 80px;
        }
    </style>
    <div class="row">
        <div class="table-responsive">
            <table id="grdSource" class="table table-bordered table-responsive">
                <caption class="text-left">List resuft</caption>
                <thead>
                    <tr>
                        <th>
                            <div class='action-buttons wid_40px'><a data-toggle="tooltip" id='btnSearch' onclick='btnSearch_Onclick()'
                                title="Search"><i class="glyphicon glyphicon-search"></i></a><a id='btnClearInput'
                                    onclick='btnClearInput_Onclick();' data-toggle="tooltip" title="Clear value search">
                                    <i class="glyphicon glyphicon-trash"></i></a></div>
                        </th>
                        <th>
                            <input id="txtNBR" type="text" class="sControl" />

                        </th>
                        <th>
                            <input id="txtLETTER_TYPE" type="text" class="sControl" />

                        </th>
                        <th>
                            <input id="txtFLIGHTNBR" type="text" class="sControl" />

                        </th>
                        <th>
                            <input id="txtREGISTRATION" type="text" class="sControl" />

                        </th>
                        <th>
                            <select id="ddlFORM_AIRP">
                                <option value="">--</option>
                                <% foreach (DataRow r in new AeroDAL().GetTableObject().Rows)%>
                                <%{%>
                                <option value="<%= r["AE_CODE"] %>"><%= r["AE_CODE"] %> -- <%= r["AE_NAME"] %></option>
                                <%} %>
                            </select></th>
                        <th>
                            <select id="ddlTO_AIRP">
                                <option value="">--</option>
                                <% foreach (DataRow r in new AeroDAL().GetTableObject().Rows)%>
                                <%{%>
                                <option value="<%= r["AE_CODE"] %>"><%=r["AE_CODE"] %> -- <%= r["AE_NAME"] %></option>
                                <%} %>
                            </select></th>
                        <th>
                            <input id="txtETD" type="text" class="sControl" />

                        </th>
                        <th>
                            <input id="txtETA" type="text" class="sControl" />

                        </th>
                        <th>
                            <input id="txtATD" type="text" class="sControl" />

                        </th>
                        <th>
                            <input id="txtATA" type="text" class="sControl" />

                        </th>
                        <th>
                            <input id="txtVIA" type="text" class="sControl" />

                        </th>
                        <th>
                            <input id="txtROUTE" type="text" class="sControl" />

                        </th>
                        <th>
                            <input id="txtROUTE_TT" type="text" class="sControl" />

                        </th>
                        <th>
                            <input id="txtCNL_TYPE" type="text" class="sControl" />

                        </th>
                        <th>
                            <select id="ddlCRAFT_TYPE">
                                <option value="">---</option>
                                <%foreach (var item in new CraftTypeDAL().GetAllCraftType()) %>
                                <%{ %>
                                <option value="<%= item.MA %>"><%= item.MA %></option>
                                <%} %>
                            </select></th>
                        <th></th>
                        <th>
                            <input id="txtTEXT" type="text" class="sControl" />

                        </th>
                    </tr>
                    <tr>
                        <th>No</th>
                        <%--column 1--%>
                        <th>NBR</th>
                        <%--column 2--%>
                        <th>LETTER_TYPE</th>
                        <%--//column 3--%>
                        <th>FLIGHTNBR</th>
                        <%--//column 4--%>
                        <th>REGISTRATION</th>
                        <%--//column 5--%>
                        <th>FROM_AIRP</th>
                        <%--//column 6--%>
                        <th>TO_AIRP</th>
                        <%--//column 7--%>
                        <th>ETD</th>
                        <%--//column 8--%>
                        <th>ETA</th>
                        <%--//column 9--%>
                        <th>ATD</th>
                        <%--//column 10--%>
                        <th>ATA</th>
                        <%--//column 11--%>
                        <th>VIA</th>
                        <%--//column 12--%>
                        <th>ROUTE</th>
                        <th>ROUTE_TT</th>
                        <th>CNL_TYPE</th>
                        <%--//column 13--%>
                        <th>CRAFT_TYPE</th>
                        <%--//column 14--%>
                        <th>FLIGHTDATE</th>
                        <%--//column 15--%>
                        <th>TEXT</th>
                        <%--//column 16--%>
                    </tr>
                </thead>
                <tbody>
                    <%= grdSource_LoadFirst() %>
                </tbody>
            </table>
        </div>
    </div>

    <div id="divExcuteScript"></div>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script>
        var isSroll = false;
        var phanCach = '<%= _phanCach%>';
        var phanCachArg = '<%= _phanCachArg %>';
        function btnClearInput_Onclick() {
            $('#grdSource input').val('');
            $('#ddlTO_AIRP').prop('selectedIndex', 0);
            $('#ddlFORM_AIRP').prop('selectedIndex', 0);
        }
        function getValueSearch() {
            var obj = {
                P_NBR: $('#txtNBR').val()
                , P_LETTER_TYPE: $('#txtLETTER_TYPE').val()
                , P_FLIGHTNBR: $('#txtFLIGHTNBR').val()
                , P_REGISTRATION: $('#txtREGISTRATION').val()
                , P_FROM_AIRP: $('#ddlFORM_AIRP').val()
                , P_TO_AIRP: $('#ddlTO_AIRP').val()
                , P_ETD: $('#txtETD').val()
                , P_ETA: $('#txtETA').val()
                , P_ATD: $('#txtATD').val()
                , P_ATA: $('#txtATA').val()
                , P_VIA: $('#txtVIA').val()
                , P_ROUTE: $('#txtROUTE').val()
                , P_ROUTE_TT: $('#txtROUTE_TT').val()
                , P_CRAFT_TYPE: $('#ddlCRAFT_TYPE').val()
                , P_ROWSTART: isSroll ? (parseInt($('#grdSource tbody tr').last().attr('data-RowNumber')) + 1) : 0
                , P_ROWFINISH: isSroll ? (parseInt($('#grdSource tbody tr').last().attr('data-RowNumber')) + 20) : 20
                , P_HASPERM: 2
            }
            return obj;
        }
        function btnSearch_Onclick() {
            GetArgWithPostBack(JSON.stringify(getValueSearch()) + phanCachArg + 'btnSearch_Onclick', 'btnSearch_Onclick');
        }
        function DisplayResult(resulf, context) {
            if (context == 'btnSearch_Onclick') {
                $('#grdSource tbody>tr').remove();
                $('#grdSource tbody').append(resulf);
            }
            else if (context == 'LoadGrdSourceScroll') {
                $('#grdSource tr').last().after(resulf).fadeIn();
            }
        }
        $(window).scroll(function () {
            if ($(window).scrollTop() + $(window).height() >= $(document).height()) {
                isSroll = true;
                LoadGrdSourceScroll();
                isSroll = false;
            }
        });
        function LoadGrdSourceScroll() {
            if (isSroll)
                GetArgWithPostBack(JSON.stringify(getValueSearch()) + phanCachArg + 'LoadGrdSourceScroll', 'LoadGrdSourceScroll');
        }
    </script>
</asp:Content>
