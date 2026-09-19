<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BB7.aspx.cs" Inherits="prjApplication.Report.M24.BB7" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>+ BIÊN BẢN ĐỐI CHIẾU SỐ LIỆU BAY CÁC HÃNG HÀNG KHÔNG QUỐC NỘI CẤT HẠ CÁNH TẠI CÁC SÂN BAY KHU VỰC MB THÁNG</title>
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
                            <th class="tg-s6z2">TRUNG TÂM QUẢN LÝ LUỒNG KHÔNG LƯU</th>
                            <th class="tg-031e"></th>
                            <th class="tg-hgcj">CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</th>
                        </tr>
                        <tr>
                            <td class="tg-hgcj">TRUNG TÂM HĐB&amp;ĐPLKL<br>
                                ______________</td>
                            <td class="tg-031e"></td>
                            <td class="tg-s6z2">Độc lập-Tự do- Hạnh phúc<br>
                                ______________</td>
                        </tr>
                        <tr>
                            <td class="tg-s6z2">Số……./BC-ĐPL</td>
                            <td class="tg-031e"></td>
                            <td class="tg-s6z2">Hà nội, ngày <%=_date %> tháng <%=_month %> năm <%=_year %></td>
                        </tr>
                        <tr>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                        </tr>
                        <tr>
                            <td class="tg-asmn" colspan="3">
                               BIÊN BẢN ĐỐI CHIẾU SỐ LIỆU BAY CÁC HÃNG HÀNG KHÔNG QUỐC NỘI<br />
                                CẤT HẠ CÁNH TẠI CÁC SÂN BAY KHU VỰC MB THÁNG <%=_month %> /<%=_year %>
                            </td>                                                     
                        </tr>
                    </table>



                    <table class="tblNoidung" style="undefined; table-layout: fixed; width: 100%">
                          <tr>
                            <th class="tg-s6z2" rowspan="2" style="width:100px;">Hãng/<br />sân bay</th>
                            <th class="tg-s6z2" colspan="3"><%=_operid %></th>
                            
                          </tr>
                          <tr>
                            <td class="tg-s6z2">QN đi QN</td>
                            <td class="tg-s6z2">QN đi QT</td>
                            <td class="tg-s6z2">QT về</td>
                                                         
                          </tr>
                          <tr>
                            <td class="tg-s6z2">VVCI</td>
                            <td class="tg-031e"><%=_slbQN1 %></td>
                            <td class="tg-031e"><%=_slbQT1 %></td>
                            <td class="tg-031e"><%=_slbQTV1 %></td>
                                                      
                          </tr>
                          <tr>
                            <td class="tg-s6z2">VVNB</td>
                            <td class="tg-031e"><%=_slbQN2 %></td>
                            <td class="tg-031e"><%=_slbQT2 %></td>
                            <td class="tg-031e"><%=_slbQTV2 %></td>
                                                        
                          </tr>
                         
                         <tr>
                            <td class="tg-s6z2">
                                <b>1.TỔNG MB</b>

                            </td>
                            <td class="tg-031e"><%=_stongQNMB1 %></td>
                            <td class="tg-031e"><%=_stongQTMB1 %></td>
                            <td class="tg-031e"><%=_stongQTVMB1 %></td>
                            
                          </tr>
                          <tr>
                            <td class="tg-s6z2">VVCA</td>
                            <td class="tg-031e"><%=_slbQNMT1 %></td>
                            <td class="tg-031e"><%=_slbQTMT1 %></td>
                            <td class="tg-031e"><%=_slbQTVMT1 %></td>
                           
                          </tr>
                         <tr>
                            <td class="tg-s6z2">VVDN</td>
                            <td class="tg-031e"><%=_slbQNMT2 %></td>
                            <td class="tg-031e"><%=_slbQTMT2 %></td>
                            <td class="tg-031e"><%=_slbQTVMT2 %></td>
                            
                          </tr>
                          
                          <tr>
                            <td class="tg-s6z2">
                                <b>2.TỔNG MT</b>
                            </td>
                           <td class="tg-031e"><%=_stongQNMT2 %></td>
                            <td class="tg-031e"><%=_stongQTMT2 %></td>
                            <td class="tg-031e"><%=_stongQTVMT2 %></td>
                                                        
                            
                          </tr>
                          <tr>
                            <td class="tg-s6z2">VVBM</td>
                            <td class="tg-031e"><%=_slbQNMN1 %></td>
                            <td class="tg-031e"><%=_slbQTMN1 %></td>
                            <td class="tg-031e"><%=_slbQTVMN1 %></td>
                                                        
                            
                          </tr>   
                          <tr>
                            <td class="tg-s6z2">VVTS</td>
                            <td class="tg-031e"><%=_slbQNMN2 %></td>
                            <td class="tg-031e"><%=_slbQTMN2 %></td>
                            <td class="tg-031e"><%=_slbQTVMN2 %></td>
                                                      
                            
                          </tr>   
                         
                          <tr>
                            <td class="tg-s6z2"><b>3.TỔNG MN</b></td>
                            <td class="tg-031e"><%=_stongQNMN3 %></td>
                            <td class="tg-031e"><%=_stongQTMN3 %></td>
                            <td class="tg-031e"><%=_stongQTVMN3 %></td>
                            
                            
                          </tr>   
                          <tr>
                            <td class="tg-s6z2"><b>TỔNG(1+2+3)</b></td>
                            <td class="tg-031e"><%=_stongQN123 %></td>
                            <td class="tg-031e"><%=_stongQT123 %></td>
                            <td class="tg-031e"><%=_stongQTV123 %></td>
                            
                            
                            
                          </tr>   
                                             
                    </table>                   
                </td>
            </tr>           
             <tr>
                <td style="height: 10px">
                    <table class="tblNoidung" style="undefined; table-layout: fixed; width: 100%">
                        <tr>
                            <th class="tg-s6z2" colspan="2">
                                ĐẠI DIỆN TRUNG TÂM QLLKL
                            </th>
                            <th class="tg-s6z2" colspan="2">
                                ĐẠI DIỆN CÔNG TY QLBMB
                            </th>
                        </tr>
                        <tr>
                            <td class="tg-s6z2">
                                TRƯỞNG TT <br />HĐB&ĐPLKL
                            </td>
                            <td class="tg-s6z2">
                                GIÁM ĐỐC <br />TRUNG TÂM
                            </td>
                            <td class="tg-s6z2">
                                    TRƯỞNG TT <br />KS ĐƯỜNG DÀI
                            </td>
                            <td class="tg-s6z2">
                                GIÁM ĐỐC<br />CÔNG TY
                            </td>                            
                        </tr>
                        <tr>
                            <td colspan="4" style="height: 80px;"></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <b>Nơi nhận:</b>

                </td>                
            </tr>
            <tr>
                <td>
                    <b>-Như kính gửi;</b>

                </td>                
            </tr>
            <tr>
                <td>
                    <b>-Lưu VT,ĐPL</b>

                </td>                
            </tr>
        </table>
        </div>

    </form>
</body>
</html>


