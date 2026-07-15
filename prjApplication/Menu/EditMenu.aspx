<%@ Page Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="EditMenu.aspx.cs" Inherits="prjApplication.Menu.EditMenu" %>

<%@ Import Namespace="prjComponents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link href="../Style/style_input.css" rel="stylesheet" />

    <script language="Javascript" type="text/javascript">
        function f_SubmitImage() {
            SubmitImage('../UploadMulti/Upload.aspx?vType=1&vKey=1', 840, 580);
        }
        function getPath(valuePath, numArg) {
            if (parseInt(numArg) == 1) {
                document.getElementById("ctl00_MainContent_txtThumbnail").value = valuePath;
                document.getElementById("ctl00_MainContent_ImgViews").src = '<%=prjComponents.Global.TinPath%>' + '<%=prjComponents.Global.UploadPath%>' + valuePath;
                document.getElementById("ctl00_MainContent_ImgViews").style.display = '';
            }
        }
        function uploadOnchange() {
            if (document.getElementById("ctl00_MainContent_txtThumbnail").value != '') {
                document.getElementById("ctl00_MainContent_ImgViews").src = '<%=prjComponents.Global.TinPath%>' + '<%=prjComponents.Global.UploadPath%>' + document.getElementById("ctl00_MainContent_txtThumbnail").value;
                document.getElementById("ctl00_MainContent_ImgViews").style.display = '';
            }
            else {
                document.getElementById("ctl00_MainContent_ImgViews").style.display = 'none';
            }
        }
        function ClearImage() {
            document.getElementById('ctl00_MainContent_txtThumbnail').value = "";
            document.getElementById("ctl00_MainContent_ImgViews").style.display = 'none';
        }
    </script>




    <table width="100%" cellpadding="0" cellspacing="0" align="center">
        <tr align="center">
            <td align="center">
                <div id="body-wrapper">
                    <div id="main-content">
                        <div class="content-box">
                            <!-- Start Content Box -->
                            <div class="content-box-header">
                                <h3 style="cursor: s-resize; margin-top: 0px; font-size: 12px;">+CẬP NHẬT THÔNG TIN CHỨC NĂNG HỆ THỐNG</h3>
                                <div class="clear">
                                </div>
                            </div>
                            <!-- End .content-box-header -->
                            <div class="content-box-content">
                                <table class="input">
                                    <tbody>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Tên chức năng(<span class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput tdwidth">
                                                <asp:TextBox ID="txtMenuName" runat="server" CssClass="text-input" Width="450px" TabIndex="1"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                                    ControlToValidate="txtMenuName" Display="Dynamic" SetFocusOnError="True" CssClass="req_Field"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Chức năng cha:
                                            </td>
                                            <td class="tdinput">
                                                <asp:DropDownList ID="ddlParrentID" runat="server" CssClass="text-input" Width="462px" TabIndex="2">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Đường dẫn:
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtMenuURL" runat="server" CssClass="text-input" Width="450px" TabIndex="3"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Icon:
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtThumbnail" runat="server" CssClass="text-input" Width="379px" TabIndex="4" onblur="uploadOnchange();"></asp:TextBox>
                                                <input accesskey="S" onclick="f_SubmitImage()" type="button" class="PhotoSel" value="Browse"
                                                    name="cmd_SavePath2" tabindex="5" />
                                                <img runat="server" id="ImgViews" onclick="ViewImages(this.src);" alt="Xem"
                                                    title="Xem" style="max-width: 40px; max-height: 40px; border: 0px; vertical-align: middle; cursor: pointer;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Mô tả:
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtMenuDesc" runat="server" CssClass="text-input medium-input" Width="450px" TextMode="MultiLine"
                                                    Rows="4" TabIndex="6"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Thứ tự:
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtMenuOrder" runat="server" CssClass="text-input" TabIndex="7" Width="10%" onKeyPress='return check_num(this,5,event)'></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Hiển thị:
                                            </td>
                                            <td class="tdinput">
                                                <asp:CheckBox ID="chkIsDisplay" CssClass="inputtext" Width="5%" runat="server" TabIndex="8" />
                                            </td>
                                        </tr>
                                        <!--<tr>
                                                        <td style="text-align: right; font-weight: bold;" class="tdinput">Đồng bộ dữ liệu:
                                                        </td>
                                                        <td class="tdinput">
                                                            <asp:CheckBox ID="chkActiveSync" CssClass="inputtext" Width="5%" runat="server" TabIndex="9" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right; font-weight: bold;" class="tdinput">Đồng bộ ảnh:
                                                        </td>
                                                        <td class="tdinput">
                                                            <asp:CheckBox ID="chkActiveSyncImage" CssClass="inputtext" Width="5%" runat="server" TabIndex="10" />
                                                        </td>
                                                    </tr>-->
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Ghi chú:
                                            </td>
                                            <td class="tdinput">&nbsp;Các ô có đánh dấu <span class="req_Field">*</span> là trường bắt buộc phải
                                                nhập.
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class="clear clearHeight">
                                </div>
                                <div id="khung">
                                    <div id="main">


                                        <asp:LinkButton runat="server" ID="btnSave" CssClass="btn btn-sm btn-primary btn-bold btn-round"
                                            OnClick="linkSave_Click">
                                                     <i class="ace-icon fa fa-floppy-o bigger-120 white"></i>
                                                        Lưu giữ
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" ID="linkExit" CssClass="btn btn-sm btn-warning btn-round" CausesValidation="false"
                                            OnClick="LinkCancel_Click">
                                                     <i class="ace-icon fa fa-times white"></i>
                                                        Thoát
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <!-- End .content-box-content -->
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>

</asp:Content>
