<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Route_Month.aspx.cs" Inherits="prjApplication.Report.ReportATFM.Route_Month" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>OVER FLIGHTS BY ROUTE</title>
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
                                <td class="tg-asmn" colspan="3" style="text-align:center">
                                    OVER FLIGHTS BY ROUTE    
                                </td>
                            </tr>
                            <tr >
                                <td class="tg-asmn" colspan="3" style="text-align:center">
                                    <asp:Label ID="lbldatekhoang" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table class="tgNoiDung" style="undefined; table-layout: fixed; width: 100%">

                            <tr>
                                <th class="tg-e3zv"><b>ORDER</b></th>
                                <th class="tg-031e"><b>ROUTE</b></th>
                                <th class="tg-031e"><b>AMOUNT</b></th>                               
                            </tr>
                            <asp:Label ID="lblcontent" runat="server"></asp:Label>                                                                           
                        </table>                       
                    </td>
                </tr>                
            </table>
        </div>

    </form>
</body>
</html>
