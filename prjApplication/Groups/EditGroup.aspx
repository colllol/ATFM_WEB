<%@ Page Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="EditGroup.aspx.cs" Inherits="prjApplication.Groups.EditGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/style_input.css" rel="stylesheet" />


    <table width="100%" cellpadding="0" cellspacing="0" align="center">
        <tr align="center">
            <td align="center">

                <div id="body-wrapper">
                    <div id="main-content">
                        <div class="content-box">
                            <!-- Start Content Box -->
                            <div class="content-box-header">
                                <h3 style="cursor: s-resize; margin-top: 0px; font-size: 12px;">+CẬP NHẬT THÔNG TIN NHÓM LÀM VIỆC</h3>

                                <div class="clear">
                                </div>
                            </div>
                            <!-- End .content-box-header -->
                            <div class="content-box-content">

                                <table class="input">
                                    <tbody>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Tên nhóm(<span class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput tdwidth">
                                                <asp:TextBox ID="txtName" runat="server" CssClass="text-input" Width="400" TabIndex="1"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Nhập tên nhóm"
                                                    ControlToValidate="txtName" Font-Size="Small" Display="Dynamic" SetFocusOnError="True" CssClass="req_Field"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Mô tả:
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtDesc" runat="server" CssClass="text-input medium-input" Width="60%" TextMode="MultiLine"
                                                    Rows="4" TabIndex="2"></asp:TextBox>
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

                                        <asp:LinkButton runat="server" ID="linkSave" CssClass="btn btn-sm btn-primary btn-bold btn-round"
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
