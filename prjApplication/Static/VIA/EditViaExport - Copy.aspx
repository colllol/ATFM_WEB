<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="EditViaExport.aspx.cs" Inherits="prjApplication.Static.VIA.EditViaExport" %>

<%@ Register TagPrefix="nbc" Namespace="prjComponents.UI" Assembly="prjComponents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../../Style/style_input.css" rel="stylesheet" />
    <link href="../../Style/Style_List.css" rel="stylesheet" />
    <script src="../../Style/assets/js/wizard.min.js"></script>
    <script src="../../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../../Style/assets/js/bootstrap-datepicker.min.js"></script>

    <table width="100%" cellpadding="0" cellspacing="0" align="center">
        <tr align="center">
            <td align="center">

                <div id="body-wrapper">
                    <div id="main-content">
                        <div class="content-box">
                            <!-- Start Content Box -->
                            <div class="content-box-header">
                                <h3 style="cursor: s-resize; margin-top: 0px; font-size: 12px;">+ EDIT VIA</h3>

                                <div class="clear">
                                </div>
                            </div>
                            <!-- End .content-box-header -->
                            <div class="content-box-content">

                                <table class="input">
                                    <tbody>
                                        
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">FROM_AIRP(<span
                                                class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                <input id="txtFromAirp" runat="server" type="text" class="text-input" tabindex="2" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFromAirp"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập From Air" Font-Size="Small" SetFocusOnError="True"
                                                    Font-Bold="false"></asp:RequiredFieldValidator>
                                               <%-- <asp:DropDownList ID="ddlFROM_AIRP" CssClass="text-input" runat="server"
                                                    Width="40%">
                                                </asp:DropDownList>--%>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">TO_AIRP(<span
                                                class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                <input id="txtToAir" runat="server" type="text" class="text-input" tabindex="2" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtToAir"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập To Air" Font-Size="Small" SetFocusOnError="True"
                                                    Font-Bold="false"></asp:RequiredFieldValidator>
                                               <%-- <asp:DropDownList ID="ddlTO_AIRP" CssClass="text-input" runat="server"
                                                    Width="40%">
                                                </asp:DropDownList>--%>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">VIA(<span
                                                class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                <input id="txtVIA" runat="server" type="text" class="text-input" tabindex="2" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtVIA"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập VIA" Font-Size="Small" SetFocusOnError="True"
                                                    Font-Bold="false"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">TYPE:
                                            </td>
                                            <td class="tdinput">
                                                <input id="txtCraft" runat="server" type="text" class="text-input" tabindex="3" />

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


</asp:Content>
