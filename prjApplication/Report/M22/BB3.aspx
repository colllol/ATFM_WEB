<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BB3.aspx.cs" Inherits="prjApplication.Report.M22.BB3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BÁO CÁO SỐ LIỆU ĐIỀU HÀNH BAY</title>
    <link href="../../Style/StyleReports.css" rel="stylesheet" />
</head>
<body style="color: white;">
    <form id="form1" runat="server">
        <div class="classSearchHeader">
            <table class="tblHeader" style="border: none; text-align: right; width: 100%; color: white;">
                <tr>
                    <td style="width: 20%; text-align: center;"></td>
                    <td style="width: 30%; text-align: center;">

                        <asp:LinkButton runat="server" ID="btnExportPdf" CssClass="btn btn-primary"
                            OnClick="btnPDF_Click">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Export data into Pdf
                        </asp:LinkButton>

                    </td>
                    <td style="width: 30%; text-align: center;">
                        <asp:LinkButton runat="server" ID="btnExportExel" CssClass="btn btn-primary"
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
            
        </div>

    </form>
</body>
</html>
