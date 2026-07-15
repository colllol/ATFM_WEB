<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="EditOper.aspx.cs" Inherits="prjApplication.Static.Oper.EditOper" %>
<%@ Register TagPrefix="nbc" Namespace="prjComponents.UI" Assembly="prjComponents" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <link href="http://localhost/prjApplication/Style/style_input.css" rel="stylesheet" />
    <link href="http://localhost/prjApplication/Scripts/DateTimeJQ/jquery.datetimepicker.css"
        rel="stylesheet" />
    <script src="http://localhost/prjApplication/Scripts/DateTimeJQ/jquery-1.8.3.js"></script>

    <link href="../../Style/Style_List.css" rel="stylesheet" />

    <table width="100%" cellpadding="0" cellspacing="0" align="center">
        <tr align="center">
            <td align="center">

                <div id="body-wrapper">
                    <div id="main-content">
                        <div class="content-box">
                            <!-- Start Content Box -->
                            <div class="content-box-header">
                                <h3 style="cursor: s-resize; margin-top: 0px; font-size: 12px;">+ EDIT ADDRESS AFTN</h3>

                                <div class="clear">
                                </div>
                            </div>
                            <!-- End .content-box-header -->
                            <div class="content-box-content">

                                <table class="input">
                                    <tbody>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">OPER_NAME(<span
                                                class="req_Field">*</span>):
                                                            
                                            </td>
                                            <td class="tdinput tdwidth">
                                                <asp:TextBox ID="txtOPER_NAME" runat="server" MaxLength="100" CssClass="text-input" Width="40%"
                                                    TabIndex="1"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFVtxtUserName" runat="server" ControlToValidate="txtOPER_NAME"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập OPER_NAME" CssClass="req_Field"
                                                    SetFocusOnError="True" Font-Size="Small"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">OPER_ADDRESS(<span
                                                class="req_Field">*</span>):
                                                            
                                            </td>
                                            <td class="tdinput tdwidth">
                                                <asp:TextBox ID="txtOPER_ADDRESS" runat="server" MaxLength="200" CssClass="text-input" Width="40%"
                                                    TabIndex="1"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtOPER_ADDRESS"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập OPER_ADDRESS" CssClass="req_Field"
                                                    SetFocusOnError="True" Font-Size="Small"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">IS_DOMESTIC(<span
                                                class="req_Field">*</span>):                                                            
                                            </td>
                                            <td class="tdinput">
                                                <asp:CheckBox ID="ckIS_DOMESTIC" CssClass="inputtext" Width="5%" runat="server" TabIndex="7"></asp:CheckBox>
                                            </td>  
                                            <%--<td class="tdinput tdwidth">
                                                <asp:TextBox ID="txtIS_DOMESTIC" runat="server" MaxLength="1" CssClass="text-input" Width="40%"
                                                    TabIndex="1"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtIS_DOMESTIC"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập IS_DOMESTIC" CssClass="req_Field"
                                                    SetFocusOnError="True" Font-Size="Small"></asp:RequiredFieldValidator>
                                            </td>--%>
                                        </tr>

                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">OPER_ICAO(<span
                                                class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtOPER_ICAO" runat="server" MaxLength="3" CssClass="text-input" Width="40%"
                                                    TabIndex="4"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtOPER_ICAO"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập OPER_ICAO" Font-Size="Small" SetFocusOnError="True"
                                                    Font-Bold="false"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">OPER_IATA:
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtOPER_IATA" runat="server" MaxLength="2"  CssClass="text-input" Width="40%"
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

    <script src="http://localhost/prjApplication/Scripts/CustomDynamic.js"></script>
</asp:Content>
