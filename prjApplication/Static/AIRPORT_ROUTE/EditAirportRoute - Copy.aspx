<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true"
    CodeBehind="EditAirportRoute.aspx.cs" Inherits="prjApplication.Static.AIRPORT_ROUTE.EditAirportRoute" %>

<%@ Register TagPrefix="nbc" Namespace="prjComponents.UI" Assembly="prjComponents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link href="http://localhost/prjApplication/Style/style_input.css" rel="stylesheet" />
    <link href="http://localhost/prjApplication/Scripts/DateTimeJQ/jquery.datetimepicker.css"
        rel="stylesheet" />
    <script src="http://localhost/prjApplication/Scripts/DateTimeJQ/jquery-1.8.3.js"></script>
    <link href="../../Style/assets/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="../../Style/assets/css/bootstrap-datepicker3.min.css" rel="stylesheet" />
    <link href="../../Style/Style_List.css" rel="stylesheet" />

    <table border="0" cellpadding="0" width="100%" cellspacing="0">
        <tr>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td style="text-align: center">
                <table width="100%" cellpadding="0" cellspacing="0" align="center">
                    <tr align="center">
                        <td align="center">

                            <div id="body-wrapper">
                                <div id="main-content">
                                    <div class="content-box">
                                        <!-- Start Content Box -->
                                        <div class="content-box-header">
                                            <h3 style="cursor: s-resize; margin-top: 0px; font-size: 12px;">+ EDIT Airport</h3>

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
                                                            <asp:TextBox ID="txtFROM_AIRP" runat="server" MaxLength="4"  CssClass="text-input" Width="40%"
                                                                TabIndex="4"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFROM_AIRP"
                                                                Display="Dynamic" ErrorMessage="Chưa nhập FROM_AIRP" Font-Size="Small" SetFocusOnError="True"
                                                                Font-Bold="false"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right; font-weight: bold;"  class="tdinput">TO_AIRP(<span
                                                            class="req_Field">*</span>):
                                                        </td>
                                                        <td class="tdinput">
                                                            <asp:TextBox ID="txtTO_AIRP" runat="server" MaxLength="4" CssClass="text-input" Width="40%"
                                                                TabIndex="4"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator011" runat="server" ControlToValidate="txtTO_AIRP"
                                                                Display="Dynamic" ErrorMessage="Chưa nhập TO_AIRP" Font-Size="Small" SetFocusOnError="True"
                                                                Font-Bold="false"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right; font-weight: bold;" class="tdinput">ROUTE(<span
                                                            class="req_Field">*</span>):
                                                            
                                                        </td>
                                                        <td class="tdinput tdwidth">
                                                            <asp:TextBox ID="txtROUTE" runat="server" MaxLength="150" CssClass="text-input" Width="40%"
                                                                TabIndex="1"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RFVtxtUserName" runat="server" ControlToValidate="txtROUTE" 
                                                                Display="Dynamic" ErrorMessage="Chưa nhập Route" CssClass="req_Field"
                                                                SetFocusOnError="True" Font-Size="Small"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td style="text-align: right; font-weight: bold;" class="tdinput">IS_OVERSEA(<span
                                                            class="req_Field">*</span>):
                                                        </td>
                                                        <td class="tdinput">
                                                            <asp:TextBox ID="txtIS_OVERSEA" runat="server" CssClass="text-input" Width="40%"
                                                                TabIndex="4"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtIS_OVERSEA"
                                                                Display="Dynamic" ErrorMessage="Chưa nhập IS_OVERSEA" Font-Size="Small" SetFocusOnError="True"
                                                                Font-Bold="false"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right; font-weight: bold;" class="tdinput">DOMESTIC(<span
                                                            class="req_Field">*</span>):
                                                        </td>
                                                        <td class="tdinput">
                                                            <asp:TextBox ID="txtDOMESTIC" runat="server" data-number="true" CssClass="text-input" Width="40%"
                                                                TabIndex="4"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDOMESTIC"
                                                                Display="Dynamic" ErrorMessage="Chưa nhập DOMESTIC" Font-Size="Small" SetFocusOnError="True"
                                                                Font-Bold="false"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right; font-weight: bold;" class="tdinput">INTERNATIONAL(<span
                                                            class="req_Field">*</span>):
                                                        </td>
                                                        <td class="tdinput">
                                                            <asp:TextBox ID="txtINTERNATIONAL" runat="server" data-number="true" CssClass="text-input" Width="40%"
                                                                TabIndex="4"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtINTERNATIONAL"
                                                                Display="Dynamic" ErrorMessage="Chưa nhập INTERNATIONAL" Font-Size="Small" SetFocusOnError="True"
                                                                Font-Bold="false"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right; font-weight: bold;" class="tdinput">IS_DOMESTIC(<span
                                                            class="req_Field">*</span>):
                                                        </td>
                                                        <td class="tdinput">
                                                            <asp:TextBox ID="txtIS_DOMESTIC" runat="server" CssClass="text-input" Width="40%"
                                                                TabIndex="4"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtIS_DOMESTIC"
                                                                Display="Dynamic" ErrorMessage="Chưa nhập IS_DOMESTIC" Font-Size="Small" SetFocusOnError="True"
                                                                Font-Bold="false"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr id="trTest" runat="server" visible="false">
                                                        <td style="text-align: right; font-weight: bold;" class="tdinput">SUMMARY(<span
                                                            class="req_Field">*</span>):
                                                        </td>
                                                        <td class="tdinput">
                                                            <asp:TextBox ID="txtSUMMARY" runat="server" CssClass="text-input" Width="40%"
                                                                TabIndex="4"></asp:TextBox>
                                                            
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
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
        </tr>
    </table>
    <script src="../../Scripts/CustomDynamic.js"></script>
</asp:Content>

