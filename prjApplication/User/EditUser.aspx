<%@ Page Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="EditUser.aspx.cs" Inherits="prjApplication.Menu.EditUser" %>

<%@ Register TagPrefix="nbc" Namespace="prjComponents.UI" Assembly="prjComponents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link href="../Style/style_input.css" rel="stylesheet" />
    <link href="../Scripts/DateTimeJQ/jquery.datetimepicker.css" rel="stylesheet" />
    <script src="../Scripts/DateTimeJQ/jquery-1.8.3.js"></script>
    <script src="../Scripts/DateTimeJQ/jquery.datetimepicker.js"></script>
    
    <table width="100%" cellpadding="0" cellspacing="0" align="center">
        <tr align="center">
            <td align="center">

                <div id="body-wrapper">
                    <div id="main-content">
                        <div class="content-box">
                            <!-- Start Content Box -->
                            <div class="content-box-header">
                                <h3 style="cursor: s-resize; margin-top: 0px; font-size: 12px;">+CẬP NHẬT THÔNG TIN NGƯỜI DÙNG</h3>

                                <div class="clear">
                                </div>
                            </div>
                            <!-- End .content-box-header -->
                            <div class="content-box-content">

                                <table class="input">
                                    <tbody>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Tên truy cập(<span class="req_Field">*</span>):
                                                            
                                            </td>
                                            <td class="tdinput tdwidth">
                                                <asp:TextBox ID="txtUserName" runat="server" CssClass="text-input" Width="40%" TabIndex="1"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFVtxtUserName" runat="server" ControlToValidate="txtUserName"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập tài khoản" CssClass="req_Field" SetFocusOnError="True" Font-Size="Small"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <asp:Panel ID="pnlPass" runat="server">
                                            <tr>
                                                <td style="text-align: right; font-weight: bold;" class="tdinput">Mật khẩu(<span class="req_Field">*</span>):
                                                </td>
                                                <td class="tdinput">
                                                    <asp:TextBox ID="txtPass" runat="server" CssClass="inputtext" Width="40%" TextMode="Password" TabIndex="2"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPass"
                                                        Display="Dynamic" ErrorMessage="Chưa nhập mật khẩu" Font-Size="Small" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; font-weight: bold;" class="tdinput">Xác nhận mật khẩu(<span class="req_Field">*</span>):
                                                </td>
                                                <td class="tdinput">
                                                    <asp:TextBox ID="txtPassConfirm" runat="server" CssClass="inputtext" Width="40%"
                                                        TextMode="Password" TabIndex="3"></asp:TextBox>
                                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPassConfirm"
                                                        ControlToValidate="txtPass" Display="Dynamic" ErrorMessage="*" SetFocusOnError="True"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                        </asp:Panel>

                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Họ và tên(<span class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtFullName" runat="server" CssClass="text-input" Width="40%" TabIndex="4"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFVtxtFullName" runat="server" ControlToValidate="txtFullName"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập họ và tên" Font-Size="Small" SetFocusOnError="True" Font-Bold="false"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Ngày sinh:
                                            </td>
                                            <td class="tdinput">
                                                <input id="txtBirth" runat="server" class="datepicker text-input" autocomplete="off"
                                                    style="width: 200px" onkeypress='return check_num(this,14,event)' tabindex="13" />

                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Địa chỉ:
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtAddress" runat="server" CssClass="text-input medium-input" Width="60%" TextMode="MultiLine"
                                                    Rows="4" TabIndex="6"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Thư điện tử:
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="text-input" Width="40%" TabIndex="7"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Điện thoại:
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtPhoneNumber" onKeyPress='return check_num(this,12,event)' runat="server" CssClass="text-input" Width="40%" TabIndex="8"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Kích hoạt:
                                            </td>
                                            <td class="tdinput">
                                                <asp:CheckBox ID="ckActivec" CssClass="inputtext" Width="5%" runat="server" TabIndex="9"></asp:CheckBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Ghi chú:
                                            </td>
                                            <td class="tdinput">&nbsp;Các ô có đánh dấu <span class="req_Field">*</span>
                                                là trường bắt buộc phải nhập.
                                            </td>
                                        </tr>

                                    </tbody>
                                </table>
                                <div class="clear clearHeight">
                                </div>
                                <div id="khung">
                                    <div id="main">

                                        <asp:LinkButton runat="server" ID="linkApplly" CssClass="btn btn-sm btn-primary btn-bold btn-round"
                                            OnClick="linkSave_Click">
                                                     <i class="ace-icon fa fa-floppy-o bigger-120 white"></i>
                                                        Lưu giữ
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" ID="btnThoat" CssClass="btn btn-sm btn-warning btn-round" CausesValidation="false"
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

    <script language="javascript" type="text/javascript">

        $('#<%= txtBirth.ClientID %>').datetimepicker({
            mask: '39/19/9999',
            timepicker: false,
            formatTime: '',
            format: 'd/m/Y',
            formatDate: 'd/m/Y'
        });

    </script>
</asp:Content>
