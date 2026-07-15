<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="THSLB_MONTH.aspx.cs" Inherits="prjApplication.Report.ReportATFM.THSLB_MONTH" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TỔNG HỢP SỐ LIỆU BAY THEO THÁNG</title>
    <link href="../../Style/StyleReports.css" rel="stylesheet" />
    <style type="text/css">
        .tgTHSL {
            border-collapse: collapse;
            border-spacing: 0;
        }

            .tgTHSL td {
                font-family: Arial, sans-serif;
                font-size: 14px;
                padding: 10px 5px;
                border-style: solid;
                border-width: 1px;
                overflow: hidden;
                word-break: normal;
            }

            .tgTHSL th {
                font-family: Arial, sans-serif;
                font-size: 14px;
                font-weight: normal;
                padding: 10px 5px;
                border-style: solid;
                border-width: 1px;
                overflow: hidden;
                word-break: normal;
            }

            .tgTHSL .tg-baqh {
                text-align: center;
                vertical-align: top;
            }

            .tgTHSL .tg-hgcj {
                font-weight: bold;
                text-align: center;
            }

            .tgTHSL .tg-amwm {
                font-weight: bold;
                text-align: center;
                vertical-align: top;
            }

            .tgTHSL .tg-yw4l {
                vertical-align: top;
            }
    </style>
</head>
<body style="color: white;">
    <form id="form2" runat="server">
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
            <table border="0" style="width: 100%; color: white;" class="table table-condensed">

                <tr>
                    <td>
                        <table class="tblHeader" style="undefined; table-layout: fixed; width: 100%">

                            <tr>
                                <th class="tg-s6z2">TRUNG TÂM HĐ-ĐHB</th>
                                <th class="tg-031e"></th>
                                <th class="tg-hgcj"></th>
                            </tr>
                            <tr>
                                <td class="tg-hgcj">PHÒNG HĐ-ĐHB<br>
                                    ______________</td>
                                <td class="tg-031e"></td>
                                <td class="tg-s6z2"></td>
                            </tr>
                            <tr>
                                <td class="tg-s6z2"></td>
                                <td class="tg-031e"></td>
                                <td class="tg-s6z2"></td>
                            </tr>
                            <tr>
                                <td class="tg-031e"></td>
                                <td class="tg-031e"></td>
                                <td class="tg-031e"></td>
                            </tr>
                            <tr>
                                <td class="tg-asmn" colspan="3">TỔNG HỢP SỐ LIỆU BAY</td>
                            </tr>
                            <tr>
                                <td class="tg-yw4l" style="text-align: right;"><b>Tháng: <%=_month %></b></td>
                            </tr>
                            <tr>
                                <td class="tg-asmn" colspan="3"></td>
                            </tr>
                        </table>

                        <table class="tgTHSL" style="undefined; table-layout: fixed; width: 1088px">
                            <colgroup>
                                <col style="width: 164px">
                                <col style="width: 181px">
                                <col style="width: 91px">
                                <col style="width: 94px">
                                <col style="width: 138px">
                                <col style="width: 141px">
                                <col style="width: 130px">
                                <col style="width: 149px">
                            </colgroup>
                            <tr>
                                <th class="tg-hgcj">THÁNG</th>
                                <th class="tg-amwm">HÃNG HÀNG KHÔNG</th>
                                <th class="tg-amwm">QN</th>
                                <th class="tg-amwm">QT</th>
                                <th class="tg-amwm">FIR HAN</th>
                                <th class="tg-amwm">FIR HCM</th>
                                <th class="tg-amwm">GHI CHÚ</th>
                                <th class="tg-amwm">TỔNG</th>
                            </tr>
                            
                            <asp:Label ID="lblcontent" runat="server"></asp:Label>

                            <tr>
                                <td class="tg-amwm" colspan="7">Tổng số các chuyến bay đi đến trong tháng</td>
                                <td class="tg-yw4l">
                                    <asp:Label ID="lblTongNgay" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tg-yw4l"></td>
                                <td class="tg-yw4l" colspan="3">QUÁ CẢNH</td>
                                <td class="tg-yw4l">
                                    <asp:Label ID="lblSumFirHnMonth" runat="server"></asp:Label>
                                </td>
                                <td class="tg-yw4l">
                                    <asp:Label ID="lblSumFirHcmMonth" runat="server"></asp:Label>
                                </td>
                                <td class="tg-yw4l"></td>
                                <td class="tg-yw4l">
                                    <asp:Label ID="lblSumFir" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tg-amwm" colspan="7">Tổng số các chuyến bay trong tháng :</td>
                                <td class="tg-yw4l">
                                    <asp:Label ID="lblSumAll" runat="server"></asp:Label>
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
