<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="ChartReportLine.aspx.cs" Inherits="prjApplication.Common.ChartReportLine" %>

<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="../Scripts/jsapi.js"></script>
   <script type="text/javascript" src="../Scripts/uds_api_contents.js"></script> 
    <script type="text/javascript" src="../Scripts/CustomDynamic.js"></script> 
     <script type="text/javascript">
         function drawChart() {
          <%=drawChart_ByDate("data","option","chart","visualization","Schedule Flight")%>          
     }
     google.setOnLoadCallback(drawChart);
    </script> 
    <span class="TitlePanel">+ Chart</span>
    <fieldset class="box-border">

        <table style="width: 50%; border: 0px solid black;">
            <tr>
                <td>
                    <label for="txtPERMDATE" style="font-size: 12px;">Date</label>
                    <input style="height: 25px;" id="txtPERMDATE" runat="server" 
                        onblur="checkInputDate(this)"
                        data-date-format="dd/mm/yyyy" type="text" class="wid_80px" />
                    &nbsp;
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPERMDATE"
                                                    Display="Dynamic" ErrorMessage="*" CssClass="req_Field" SetFocusOnError="True" Font-Size="Small"></asp:RequiredFieldValidator>
                </td>
                 <td >
                     <label for="txtREFERENCE" style="font-size: 12px;">Airport</label>
                      <asp:DropDownList ID="ddlAirport" CssClass="text-input" runat="server"
                                                            class="wid_80px">
                                                 </asp:DropDownList>  
                     
                    
                </td>
                  <td >
                       <a href="#" onclick="CallURL();" class="btn btn-primary">
                         <span class="glyphicon glyphicon-download-alt"></span>
                                                    Search
                    </a>
                      <a href="#" onclick="LineURL();" class="btn btn-primary">
                         <span class="glyphicon glyphicon-report-alt"></span>
                                                    ColumnChart Chart
                    </a>
                    
                     
                </td>

            </tr>
            

        </table>
    </fieldset>

    <div class="table-responsive">
        <style type="text/css">
            .tg {
                border-collapse: collapse;
                border-spacing: 0;
            }

                .tg td {
                    font-family: Arial, sans-serif;
                    font-size: 14px;
                    padding: 10px 5px;
                    border-style: solid;
                    border-width: 1px;
                    overflow: hidden;
                    word-break: normal;
                    border-color: black;
                }

                .tg th {
                    font-family: Arial, sans-serif;
                    font-size: 14px;
                    font-weight: normal;
                    padding: 10px 5px;
                    border-style: solid;
                    border-width: 1px;
                    overflow: hidden;
                    word-break: normal;
                    border-color: black;
                }

                .tg .tg-9fhq {
                    background-color: #68cbd0;
                    text-align: center;
                }
        </style>
        <table class="tg" style="undefined; table-layout: fixed; width: 100%">            
            <tr>
                <td>
                    <div id="visualization" style="width: 1100px; height: 400px;"></div>
                </td>                                
            </tr>          
        </table>
    </div>

    <script language="javascript" type="text/javascript">      
        $(document).ready(function ($) {

            //var currentTime = new Date();
            //var month = currentTime.getMonth() + 1;
            //var day = currentTime.getDate();
           // var year = currentTime.getFullYear();
           // var date1 = day + "/" + month + "/" + year; // output
            var _fdate = getParameterByName('fdate'); // "lorem"
            var _fairport = getParameterByName('fairport');
            $('#ctl00_MainContent_txtPERMDATE').val(_fdate);
            $('#ctl00_MainContent_ddlAirport').val(_fairport);
            //if (_fdate != null) date1 = _fdate;
            
        }
        );
            
        function CallURL() {
            
            var _fairport = $('#ctl00_MainContent_ddlAirport').val();

            window.open("ChartReportLine.aspx?fdate=" + $('#ctl00_MainContent_txtPERMDATE').val() + "&fairport=" + _fairport, "_self")
        }
        function LineURL() {
            var _fairport = $('#ctl00_MainContent_ddlAirport').val();
            window.open("ChartReport.aspx?fdate=" + $('#ctl00_MainContent_txtPERMDATE').val() + "&fairport=" + _fairport, "_self")
        }
        function getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }

    </script>
</asp:Content>
