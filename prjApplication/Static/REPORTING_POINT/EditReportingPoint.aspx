<%@ Page Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="EditReportingPoint.aspx.cs" Inherits="prjApplication.Static.REPORTING_POINT.EditReportingPoint" %>

<%@ Register TagPrefix="nbc" Namespace="prjComponents.UI" Assembly="prjComponents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../../Style/style_input.css" rel="stylesheet" />
    <link href="../../Scripts/DateTimeJQ/jquery.datetimepicker.css"
        rel="stylesheet" />
    <script src="../../Scripts/DateTimeJQ/jquery-1.8.3.js"></script>
    <link href="../../Style/Style_List.css" rel="stylesheet" />


    <table width="100%" cellpadding="0" cellspacing="0" align="center">
        <tr align="center">
            <td align="center">

                <div id="body-wrapper">
                    <div id="main-content">
                        <div class="content-box">
                            <!-- Start Content Box -->
                            <div class="content-box-header">
                                <h3 style="cursor: s-resize; margin-top: 0px; font-size: 12px;">+ EDIT REPORTING ROUTE</h3>

                                <div class="clear">
                                </div>
                            </div>
                            <!-- End .content-box-header -->
                            <div class="content-box-content">

                                <table class="input">
                                    <tbody>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">POINT_NAME(<span
                                                class="req_Field">*</span>):
                                                            
                                            </td>
                                            <td class="tdinput tdwidth">
                                                <asp:TextBox ID="txtPOINT_NAME" runat="server" MaxLength="15" CssClass="text-input" Width="40%"
                                                    TabIndex="1"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFVtxtUserName" runat="server" ControlToValidate="txtPOINT_NAME"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập POINT_NAME" CssClass="req_Field"
                                                    SetFocusOnError="True" Font-Size="Small"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>


                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">COMPULSORY_ON_REQUEST(<span
                                                class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                 <asp:CheckBox ID="ckCOMPULSORY_ON_REQUEST" CssClass="inputtext" Width="5%" runat="server" TabIndex="5"></asp:CheckBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">DESCRIPTION(<span
                                                class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtDESCRIPTION" runat="server" MaxLength="100" CssClass="text-input" Width="40%"
                                                    TabIndex="4"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator011" runat="server" ControlToValidate="txtDESCRIPTION"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập DESCRIPTION" Font-Size="Small" SetFocusOnError="True"
                                                    Font-Bold="false"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">IN_FIR_VN(<span
                                                class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                 <asp:CheckBox ID="ckIN_FIR_VN" CssClass="inputtext" Width="5%" runat="server" TabIndex="5"></asp:CheckBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">FIR_HN(<span
                                                class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                               <asp:CheckBox ID="ckFIR_HN" CssClass="inputtext" Width="5%" runat="server" TabIndex="6"></asp:CheckBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">FIR_HCM(<span
                                                class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                <asp:CheckBox ID="ckFIR_HCM" CssClass="inputtext" Width="5%" runat="server" TabIndex="7"></asp:CheckBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">Ghi chú:
                                            </td>
                                            <td class="tdinput">&nbsp;Các ô có đánh dấu <span class="req_Field">*</span>
                                                là trường bắt buộc phải nhập.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <label runat="server" id="lblResuft" style="font-weight: 700; color: red;"></label>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class="clear clearHeight">
                                </div>
                                <div id="khung">
                                    <div id="main">
                                        <asp:LinkButton runat="server" ID="btnSave" CssClass="btn btn-sm btn-primary btn-bold btn-round"
                                            OnClick="btnSave_Click">
                                                     <i class="ace-icon fa fa-floppy-o bigger-120 white"></i>
                                                        Save
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" ID="btnExit" CssClass="btn btn-sm btn-warning btn-round"
                                            CausesValidation="false"
                                            OnClick="btnExit_Click">
                                                     <i class="ace-icon fa fa-times white"></i>
                                                        Exit
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

    <script src="http://localhost/prjApplication/Scripts/CustomDynamic.js"></script>
</asp:Content>

