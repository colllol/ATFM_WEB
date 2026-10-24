<%@ Page Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="ChangePass.aspx.cs" Inherits="prjApplication.User.ChangePass" %>

<asp:Content ID="cp_ChangePass" ContentPlaceHolderID="MainContent" runat="server">

    <link href="../Style/style_input.css" rel="stylesheet" />


    <table width="100%" cellpadding="0" cellspacing="0" align="center">
        <tr align="center">
            <td align="center">

                <div id="body-wrapper">
                    <div id="main-content">
                        <div class="content-box">
                            <!-- Start Content Box -->
                            <div class="content-box-header">
                                <h3 style="cursor: s-resize; margin-top: 0px; font-size: 12px;">+ ĐỔI MẬT KHẨU</h3>

                                <div class="clear">
                                </div>
                            </div>
                            <!-- End .content-box-header -->
                            <div class="content-box-content">

                                <table class="input">
                                    <tbody>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Mật khẩu cũ(<span class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput tdwidth">
                                                <asp:TextBox ID="txtPassOld" runat="server"  TextMode="Password" CssClass="text-input" Width="60%" TabIndex="1"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFtxtPassOld" runat="server" ControlToValidate="txtPassOld"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập mật khẩu cũ" Font-Size="Small" Font-Bold="false" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Mật khẩu(<span class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtPass" runat="server" Width="60%" TextMode="Password" CssClass="text-input"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFtxtPass" runat="server" ControlToValidate="txtPass"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập mật khẩu mới" Font-Bold="false" Font-Size="Small" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Xác nhận mật khẩu(<span class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtPassConfirm" runat="server" Width="60%" TextMode="Password" CssClass="text-input"></asp:TextBox><br />
                                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPassConfirm"
                                                    ControlToValidate="txtPass" Display="Dynamic" ErrorMessage="Xác nhận mật khẩu sai" Font-Bold="false"
                                                    SetFocusOnError="True"></asp:CompareValidator>
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
                                            OnClick="btnBack_Click">
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
