<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QN09.aspx.cs" Inherits="prjApplication.ReportBTC.QN.QN09" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>+ BÁO CÁO DOANH THU THEO HÓA ĐƠN CÁC HÃNG HÀNG KHÔNG TRONG NƯỚC</title>
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
            <table border="0" style="width: 100%; color: white;" class="table table-condensed">

            <tr>
                <td>
                    <table class="tblHeader" style="undefined; table-layout: fixed; width: 100%">

                        <tr>
                            <th class="tg-s6z2">TỔNG CÔNG TY QUẢN LÝ BAY VIỆT NAM</th>
                            <th class="tg-031e"></th>
                            <th class="tg-hgcj">CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</th>
                        </tr>
                        <tr>
                            <td class="tg-hgcj">BAN TÀI CHÍNH<br>
                                ______________</td>
                            <td class="tg-031e"></td>
                            <td class="tg-s6z2">Độc lập-Tự do- Hạnh phúc<br>
                                ______________</td>
                        </tr>
                        <tr>
                            <td class="tg-s6z2"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-s6z2">Hà nội, ngày……tháng…….năm 20…….</td>
                        </tr>
                        <tr>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                        </tr>
                        <tr>
                            <td class="tg-asmn" colspan="3">
                                BÁO CÁO DOANH THU THEO HÓA ĐƠN<br />
                                CÁC HÃNG HÀNG KHÔNG TRONG NƯỚC
                            </td>                                                     
                        </tr>
                        <tr>
                            <td class="tg-asmn" colspan="3">(Từ ngày……/……/……đến ngày……/……/……)</td>
                        </tr>                       
                    </table>

                    <table class="tblNoidung" style="undefined; table-layout: fixed; width: 100%">
                          <tr>
                            <th class="tg-s6z2" rowspan="3">HÃNG</th>
                            <th class="tg-s6z2" colspan="3">TRONG NƯỚC</th>
                            <th class="tg-s6z2" colspan="4">QUỐC TẾ</th>
                            <th class="tg-s6z2" colspan="1" rowspan="2">TỔNG CỘNG</th>
                          </tr>
                          <tr>
                            <td class="tg-s6z2">SỐ HÓA ĐƠN</td>
                            <td class="tg-s6z2">SỐ CHUYẾN </td>
                            <td class="tg-s6z2">SỐ TIỀN</td>
                            <td class="tg-s6z2">SỐ HÓA ĐƠN</td>  
                            <td class="tg-s6z2">SỐ CHUYẾN </td>
                            <td class="tg-s6z2">SỐ TIỀN</td>
                            <td class="tg-s6z2">QUY VND</td>                                       
                          </tr>
                          <tr>
                            <td class="tg-s6z2">1</td>
                            <td class="tg-s6z2">2</td>
                            <td class="tg-s6z2">3</td>
                            <td class="tg-s6z2">4</td>
                            <td class="tg-s6z2">5</td>
                            <td class="tg-s6z2">6</td>
                            <td class="tg-s6z2">7</td>
                            <td class="tg-s6z2">8=3+7</td>
                          </tr>
                          <tr>
                            <td class="tg-s6z2">HVN</td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td> 
                            <td class="tg-031e"></td> 
                            <td class="tg-031e"></td>                          
                          </tr>
                          <tr>
                            <td class="tg-s6z2">PIC</td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td> 
                            <td class="tg-031e"></td>                              
                          </tr>
                          <tr>
                            <td class="tg-s6z2">VJC</td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td> 
                            <td class="tg-031e"></td> 
                            <td class="tg-031e"></td> 
                            <td class="tg-031e"></td>                        
                          </tr>
                        <tr>
                            <td class="tg-s6z2">....</td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td> 
                            <td class="tg-031e"></td> 
                            <td class="tg-031e"></td> 
                            <td class="tg-031e"></td>                        
                          </tr>
                        <tr>
                            <td class="tg-s6z2">....</td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td> 
                            <td class="tg-031e"></td> 
                            <td class="tg-031e"></td> 
                            <td class="tg-031e"></td>                        
                          </tr>
                          <tr>
                            <td class="tg-s6z2">....</td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>>
                            <td class="tg-031e"></td> 
                            <td class="tg-031e"></td> 
                            <td class="tg-031e"></td> 
                            <td class="tg-031e"></td>                        
                          </tr>
                          <tr>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td> 
                            <td class="tg-031e"></td> 
                            <td class="tg-031e"></td> 
                            <td class="tg-031e"></td>                        
                          </tr>
                          <tr>
                            <td class="tg-s6z2">TỔNG CỘNG</td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>  
                            <td class="tg-031e"></td> 
                            <td class="tg-031e"></td> 
                            <td class="tg-031e"></td>                           
                          </tr>                        
                    </table>                   
                </td>
            </tr>           
        </table>
        </div>

    </form>
</body>
</html>

