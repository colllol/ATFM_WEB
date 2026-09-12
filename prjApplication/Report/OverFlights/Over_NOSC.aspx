<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Over_NOSC.aspx.cs" Inherits="prjApplication.Report.OverFlights.Over_NOSC" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NONE SCHEDULED OVER FLIGHTS</title>
    <link href="../../Style/StyleReports.css" rel="stylesheet" />
</head>
<body style="color: white;">
    <form id="form1" runat="server">
        <div class="classSearchHeader">
            <table class="tblHeader" style="border: none; text-align: right; width: 100%; color: white;">
                <tr>
                    <td style="width: 20%; text-align: center;"></td>
                    <td style="width: 30%; text-align: center;">

                        <asp:LinkButton runat="server" ID="LinkButton1" CssClass="btn btn-primary"
                            OnClick="btnPDF_Click">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Export data into Pdf
                        </asp:LinkButton>

                    </td>
                    <td style="width: 30%; text-align: center;">
                        <asp:LinkButton runat="server" ID="LinkButton2" CssClass="btn btn-primary"
                            OnClick="btnExcel_Click">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Export data into Excel
                        </asp:LinkButton>
                    </td>
                    <td style="width: 20%; text-align: center;"></td>
                </tr>
            </table>
        </div>
        <div id="divContent" runat="server">
            <table border="0" style="width: 100%; color: white;" class="table table-condensed">

                <tr>
                    <td>
                        <table class="tblHeader" style="undefined; table-layout: fixed; width: 100%">

                            <tr>
                                <th class="tg-s6z2">AIR TRAFFIC COMMAND AND<br />COORDINATION CENTER</th>
                            </tr>
                            <tr>
                                <td class="tg-asmn" colspan="6" style="text-align:center">
                                    NONE SCHEDULED OVER FLIGHTS    
                                </td>
                            </tr>
                            <tr >
                                <td class="tg-asmn" colspan="6" style="text-align:center">
                                    <asp:Label ID="lbldatekhoang" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table class="tgNoiDung" style="undefined; table-layout: fixed; width: 100%">

                            <tr>
                                <th class="tg-e3zv" style="width:50px"><b>ORDER</b></th>
                                <th class="tg-e3zv"><b>FLIGHTDATE</b></th>
                                <th class="tg-031e"><b>CALLSIGN</b></th>
                                <th class="tg-031e"><b>CRAFT</b></th>   
                                <th class="tg-031e"><b>ROUTE</b></th>            
                                <th class="tg-031e"><b>AMOUNT</b></th>                 
                            </tr>
                            <asp:Label ID="lblcontent" runat="server" style="text-align:center"></asp:Label>
                            <tr>
                                <td class="tg-e3zv" colspan="5" style="text-align:center"><b>Sum Of Flights - Fir HCM:</b></td>
                                <td style="text-align:center">
                                    <b><asp:Label ID="lblSumFirHcm" runat="server"></asp:Label></b>
                                </td>
                            </tr>  
                            <tr>
                                <td class="tg-e3zv" colspan="5" style="text-align:center"><b>Sum Of Flights - Fir HN:</b></td>
                                <td style="text-align:center">
                                <b><asp:Label ID="lblSumFirHn" runat="server"></asp:Label></b>
                                </td>
                            </tr>   
                            <tr>
                                <td class="tg-e3zv" colspan="5" style="text-align:center"><b>Sum Of Flights</b></td>
                                <td style="text-align:center">
                                <b><asp:Label ID="lblSumFlights" runat="server"></asp:Label></b>
                                </td>
                            </tr>                                                        
                        </table>                       
                    </td>
                </tr>                
            </table>
        </div>

    </form>
</body>
</html>
