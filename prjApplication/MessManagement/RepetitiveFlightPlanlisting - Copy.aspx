<%@ Page Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="RepetitiveFlightPlanlisting.aspx.cs" Inherits="prjApplication.MessManagement.RepetitiveFlightPlanlisting" %>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../../Style/StyleReports.css" rel="stylesheet" />
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
         .cssHide {
            display: none;
        }

        .cssShow {
            display: block;
        }

        .modal {
            overflow-x: hidden;
            overflow-y: auto;
            position: fixed;
            font-family: Arial, Helvetica, sans-serif;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            background: rgba(0,0,0,0.8);
            z-index: 99999;
            opacity: 1;
            -webkit-transition: opacity 400ms ease-in;
            -moz-transition: opacity 400ms ease-in;
            transition: opacity 400ms ease-in;
            pointer-events: visible;
        }

        .table td i:hover {
            cursor: pointer;
        }
        table, th, td {
    border: 1px solid black;
}
        .txt {
            text-align:center
        }

    </style>
    
    <span class="TitlePanel">+ An toàn không lưu</span>
    <%--<div class="classSearchHeader">
            <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                <tr>
                    <td style="width: 7%; text-align: right;">START DATE
                    </td>
                    <td style="width: 10%; text-align: left;">
                        <input id="txtStartDate" runat="server" data-date-format="dd/mm/yyyy"
                            class="inputControl date-picker mControl"
                            type="text" />
                        
                    </td>
                     <td style="width: 7%; text-align: right;">END DATE
                    </td>
                    <td style="width: 10%; text-align: left;">
                       <input id="txtFinishDate" runat="server" data-date-format="dd/mm/yyyy"
                            class="inputControl date-picker mControl"
                            type="text" />
                        
                    </td>

                    <td style="width: 25%; text-align: left;">
                        
                        <asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind"
                            Font-Bold="true" Text="" Width="40px" Height="38px" OnClick="btnSearch_Click"></asp:Button>
                        <asp:LinkButton runat="server" style="padding:8px 24px;" ID="btnExportPdf"  CssClass="btn btn-sm btn-primary btn-bold">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Pdf
            </asp:LinkButton>

            <asp:LinkButton runat="server" ID="btnExportExel" style="padding:8px 24px;" CssClass="btn btn-sm btn-primary btn-bold">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Excel
            </asp:LinkButton>
                    </td>
                </tr>
            </table>
    </div>--%>
   
    <div class="table-responsive">       
                   <table>  
                            <tr>
                                <th colspan="10">A OPERATOR</th>
                                <th colspan="3">B ADDRESSEE(S)</th>
                                <th colspan="3">C DEPARTURE AEROROIVE(S)</th>
                                <th colspan="2">G SUPPLEVENTARY DATA(Item 19) AT:</th>
                            </tr>
                            <tr>
                                <td rowspan="2" class="txt">H<br />+<br />-</td>
                                <td rowspan="2" class="txt">I <br />VALID<br /> FROM<br />yymmdd</td>
                                <td rowspan="2" class="txt">J<br />VALID<br /> UNTIL<br />yymmdd</td>
                                <td colspan="7" class="txt">K<br />DAYS OF<br /> OPERATION</td>
                                <td rowspan="2" class="txt">L<br />AIRCRAFT<br />INENTIFI-CATION<br />(Item 7)</td>
                                <td rowspan="2" class="txt">M<br />TYPE OF<br />AIRCRAFT AND WAKE<br />TURBULENCE<br />CATEGORY<br />(Item 9)</td>
                                <td rowspan="2" class="txt">N<br />DEPARTURE<br />AERODROVE AND TIME<br />(Item 13)</td>
                                <td colspan="3" class="txt">O<br />ROUTE(Item 15)</td>
                                <td rowspan="2" class="txt">P<br />DESTINATION<br />AERODROIVE<br />AND TOTAL<br />ESTIMATED<br />ELAPPSED TIME<br />(Item 16)</td>
                                <td rowspan="2" class="txt">Q<br /> REMARKS</td>
                            </tr>
                       <tr>
                           <td>1</td>
                           <td>2</td>
                           <td>3</td>
                           <td>4</td>
                           <td>5</td>
                           <td>6</td>
                           <td>7</td>
                           <td>CRUISING SPEED</td>
                           <td>LEVEL</td>
                           <td>ROUTE</td>
                       </tr>
                    </table>   
    </div>    

    <script src="../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/validate.js"></script>
    <div id="dhtmltooltip"></div>
    
</asp:Content>