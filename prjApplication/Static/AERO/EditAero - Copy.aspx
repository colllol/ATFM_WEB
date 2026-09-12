<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true"
    CodeBehind="EditAero.aspx.cs" Inherits="prjApplication.Static.AERO.EditAirport" %>

<%@ Register TagPrefix="nbc" Namespace="prjComponents.UI" Assembly="prjComponents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        input, textarea {
            text-transform: uppercase;
        }
    </style>
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
                                <h3 style="cursor: s-resize; margin-top: 0px; font-size: 12px;">+ EDIT AERO</h3>

                                <div class="clear">
                                </div>
                            </div>
                            <!-- End .content-box-header -->
                            <div class="content-box-content">

                                <table class="input">
                                    <tbody>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">AERO CODE(<span
                                                class="req_Field">*</span>):
                                                            
                                            </td>
                                            <td class="tdinput tdwidth">
                                                <asp:TextBox ID="txtAE_CODE" runat="server" MaxLength="4" CssClass="text-input" Width="40%"
                                                    TabIndex="1"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFVtxtUserName" runat="server" ControlToValidate="txtAE_CODE"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập Airport code" CssClass="req_Field"
                                                    SetFocusOnError="True" Font-Size="Small"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>


                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">AERO NAME(<span
                                                class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtAE_NAME" runat="server" MaxLength="50" CssClass="text-input" Width="40%"
                                                    TabIndex="4"></asp:TextBox>
                                                <label id="lblAE_NAMEErorr"></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">ZONE(<span
                                                class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtAE_ZONE" runat="server" MaxLength="4" CssClass="text-input" Width="40%"
                                                    TabIndex="4"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAE_ZONE"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập AE_ZONE" Font-Size="Small" SetFocusOnError="True"
                                                    Font-Bold="false"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">INTER(<span
                                                class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                <asp:CheckBox ID="ckINTER" CssClass="inputtext" runat="server"></asp:CheckBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">AERO IATA:
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtAE_IATA" runat="server" MaxLength="3" CssClass="text-input" Width="40%"
                                                    TabIndex="4"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtAE_IATA"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập AE_IATA" Font-Size="Small" SetFocusOnError="True"
                                                    Font-Bold="false"></asp:RequiredFieldValidator>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">ISOVERSEA(<span
                                                class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                <asp:CheckBox ID="ckISOVERSEA" CssClass="inputtext" runat="server"></asp:CheckBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">SUMMER TIME(<span
                                                class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtSUMMER_TIME" runat="server" MaxLength="5" CssClass="text-input" Width="40%"
                                                    TabIndex="4"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtSUMMER_TIME"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập SUMMER_TIME" Font-Size="Small" SetFocusOnError="True"
                                                    Font-Bold="false"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">WINTER TIME(<span
                                                class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtWINTER_TIME" runat="server" MaxLength="5" CssClass="text-input" Width="40%"
                                                    TabIndex="4"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtWINTER_TIME"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập WINTER_TIME" Font-Size="Small" SetFocusOnError="True"
                                                    Font-Bold="false"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">LIMIT:
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtGioihan" runat="server" MaxLength="5" CssClass="text-input" Width="40%"
                                                    TabIndex="4" onKeyPress='return check_num(this,5,event)'></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">REGION:
                                            </td>
                                            <td class="tdinput">
                                                <asp:DropDownList ID="ddlVung" CssClass="text-input" runat="server"
                                                    Width="45%">
                                                    <asp:ListItem Value="" Text="-ALL-" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="MB" Text="MIỀN BẮC"></asp:ListItem>
                                                    <asp:ListItem Value="MT" Text="MIỀN TRUNG"></asp:ListItem>
                                                    <asp:ListItem Value="MN" Text="MIỀN NAM "></asp:ListItem>
                                                </asp:DropDownList>
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
