<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QC07.aspx.cs" Inherits="prjApplication.ReportBTC.QC.QC07" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>+ BIÊN BẢN ĐỐI CHIẾU SỐ LIỆU ĐHB CÁC HÃNG HÀNG KHÔNG BAY QUÁ CẢNH</title>
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
                                BIÊN BẢN ĐỐI CHIẾU SỐ LIỆU ĐHB <br />
                                CÁC HÃNG HÀNG KHÔNG BAY QUÁ CẢNH<br />
                                QUÝ…/  năm 20....  
                            </td>                                                     
                        </tr>
                        <tr>
                            <td class="tg-asmn" colspan="3">(Từ ngày……/……/……đến ngày……/……/……)</td>
                        </tr>
                        <tr>
                            <td class="tg-031e" colspan="2">
                                Hôm nay, ngày.....tháng.....năm 20....., tại TT Quản lý luồng không lưu, chúng tôi gồm :<br />                                
                            </td>
                        </tr>
                        <tr>
                            <td class="tg-031e" colspan="2">
                                <b>
                                    1. Thành phần :
                                </b>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="tg-031e" colspan="2">
                                1.1. Ban tài chính Tổng công ty quản lý bay Việt Nam :
                            </td>
                            
                        </tr>
                        <tr>                            
                            <td>
                                - Ông:			    
                            </td>
                            <td>
                                Chức vụ: 
                            </td>
                        </tr>
                        <tr>
                            
                            <td>                               
                                - Bà:							
                            </td>
                            <td>
                                Chức vụ: 
                            </td>
                        </tr>
                        <tr>
                            <td class="tg-031e" colspan="2">
                                1.2. Trung tâm Quản lý luồng không lưu :
                            </td>
                        </tr>
                        <tr>
                            <td>
                                - Ông:						    
                            </td>
                            <td>
                                Chức vụ: 
                            </td>
                        </tr>
                        <tr>
                            <td>
                                - Bà:
                            </td>
                            <td>
                                Chức vụ: 
                            </td>
                        </tr>
                        <tr>
                            <td class="tg-031e" colspan="2">
                                <b>2. Nội dung :</b>
                                      Kiểm tra đối chiếu sản lượng điều hành bay quá cảnh quý… /năm 20…<br />
                                      Sau khi đối chiếu chúng tôi thống nhất sản lượng điều hành bay quá cảnh quý …../năm 20....như sau:                          
                            </td>
                        </tr>
                    </table>

                    <table class="tblNoidung" style="undefined; table-layout: fixed; width: 100%">
                          <tr>
                            <th class="tg-s6z2" rowspan="2">THÁNG</th>
                            <th class="tg-s6z2" colspan="4">FIR HÀ NỘI</th>
                            <th class="tg-s6z2" colspan="4">FIR HỒ CHÍ MINH</th>
                            <th class="tg-s6z2" rowspan="2">TỔNG CỘNG</th>
                          </tr>
                          <tr>
                            <td class="tg-s6z2">SỐ CHUYẾN THEO BÁO CÁO</td>
                            <td class="tg-s6z2">SỐ CHUYẾN TĂNG</td>
                            <td class="tg-s6z2">SỐ CHUYẾN GIẢM</td>
                            <td class="tg-s6z2">CỘNG</td>  
                            <td class="tg-s6z2">SỐ CHUYẾN THEO BÁO CÁO</td>
                            <td class="tg-s6z2">SỐ CHUYẾN TĂNG</td>
                            <td class="tg-s6z2">SỐ CHUYẾN GIẢM</td>
                            <td class="tg-s6z2">CỘNG</td>                                             
                          </tr>
                          <tr>
                            <td class="tg-s6z2">…</td>
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
                            <td class="tg-s6z2">…</td>
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
                            <td class="tg-031e"></td>
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
                            <td class="tg-031e"></td>                           
                          </tr>                        
                    </table>                   
                </td>
            </tr>           
            <tr>
                <td style="height: 10px">
                    <table class="tblNoidung" style="undefined; table-layout: fixed; width: 100%">
                        <tr>
                            <th class="tg-s6z2" colspan="3">
                                BAN TÀI CHÍNH TỔNG CÔNG TY
                            </th>
                            <th class="tg-s6z2" colspan="3">
                                TRUNG TÂM QUẢN LÝ LUỒNG KHÔNG LƯU
                            </th>
                        </tr>
                        <tr>
                            <td class="tg-s6z2">
                                NGƯỜI LẬP<br />(Ký,ghi rõ họ tên)
                            </td>
                            <td class="tg-s6z2">
                                TỔ TRƯỞNG TTT ĐHB<br />(Ký,ghi rõ họ tên)
                            </td>
                            <td class="tg-s6z2">
                                TRƯỞNG BAN TÀI CHÍNH<br />(Ký,ghi rõ họ tên,đóng dấu)
                            </td>
                            <td class="tg-s6z2">
                                NGƯỜI LẬP<br />(Ký,ghi rõ họ tên)
                            </td>
                            <td class="tg-s6z2">
                                TRƯỞNG TT HĐB&ĐPLKL<br />(Ký,ghi rõ họ tên)
                            </td>
                            <td class="tg-s6z2">
                                GIÁM ĐỐC<br />(Ký,ghi rõ họ tên,đóng dấu)
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" style="height: 80px;"></td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>
        </div>

    </form>
</body>
</html>

