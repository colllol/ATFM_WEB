<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="EditAlarm.aspx.cs" Inherits="prjApplication.Static.ALARM.EditAlarm" EnableEventValidation="true" %>

<%@ Register TagPrefix="nbc" Namespace="prjComponents.UI" Assembly="prjComponents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../../Style/style_input.css" rel="stylesheet" />
    <link href="../../Style/Style_List.css" rel="stylesheet" />
    <script src="../../Style/assets/js/wizard.min.js"></script>
    <script src="../../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../../Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="../../Scripts/CustomDynamic.js"></script>
    <script src="../../Scripts/validate.js"></script>
    <script src="../../Scripts/DateTimeFomat.js"></script>
    <table width="100%" cellpadding="0" cellspacing="0" align="center">
        <tr align="center">
            <td align="center">

                <div id="body-wrapper">
                    <div id="main-content">
                        <div class="content-box">
                            <!-- Start Content Box -->
                            <div class="content-box-header">
                                <h3 style="cursor: s-resize; margin-top: 0px; font-size: 12px;">+ EDIT ALARM</h3>

                                <div class="clear">
                                </div>
                            </div>
                            <!-- End .content-box-header -->
                            <div class="content-box-content">

                                <table class="input">
                                    <tbody>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">EVENT_NAME(<span
                                                class="req_Field">*</span>):
                                                            
                                            </td>
                                            <td class="tdinput tdwidth">
                                                <asp:TextBox ID="txtEVENT_NAME" runat="server" MaxLength="25" CssClass="text-input" Width="40%"
                                                    TabIndex="1"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFVtxtUserName" runat="server" ControlToValidate="txtEVENT_NAME"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập EVENT_NAME" CssClass="req_Field"
                                                    SetFocusOnError="True" Font-Size="Small"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>


                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">EVENT_DATE(<span
                                                class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                <input id="txtEVENT_DATE" runat="server" data-minlenght="1" data-date-format="dd/mm/yyyy" type="text"
                                                    class="text-input date-picker" tabindex="2" />                                                
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEVENT_DATE"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập EVENT_DATE" Font-Size="Small" SetFocusOnError="True"
                                                    Font-Bold="false"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">BEGIN_VALID:
                                            </td>
                                            <td class="tdinput">
                                                
                                                 <input id="txtBEGIN_VALID" runat="server" data-date-format="dd/mm/yyyy" type="text"
                                                    class="text-input date-picker" tabindex="3" />
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">END_VALID:
                                            </td>
                                            <td class="tdinput">
                                                
                                                 <input id="txtEND_VALID" runat="server" data-date-format="dd/mm/yyyy" type="text"
                                                    class="text-input date-picker" tabindex="4" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">STATUS:
                                            </td>
                                            <td class="tdinput">
                                                <asp:CheckBox ID="ckSTATUS" CssClass="inputtext" Width="5%" runat="server" TabIndex="5"></asp:CheckBox>
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

    <script src="../../Scripts/CustomDynamic.js"></script>
</asp:Content>
