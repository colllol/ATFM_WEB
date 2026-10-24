<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="EditReportStatic.aspx.cs" Inherits="prjApplication.Static.ReportStatic.EditReportStatic" %>
<%@ Register TagPrefix="nbc" Namespace="prjComponents.UI" Assembly="prjComponents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="http://localhost/prjApplication/Style/style_input.css" rel="stylesheet" />
    <link href="http://localhost/prjApplication/Scripts/DateTimeJQ/jquery.datetimepicker.css"
        rel="stylesheet" />
    <link href="../../Style/Style_List.css" rel="stylesheet" />
    <table width="100%" cellpadding="0" cellspacing="0" align="center">
        <tr align="center">
            <td align="center">

                <div id="body-wrapper">
                    <div id="main-content">
                        <div class="content-box">
                            <!-- Start Content Box -->
                            <div class="content-box-header">
                                <h3 style="cursor: s-resize; margin-top: 0px; font-size: 12px;">+ EDIT REPORT STATIC</h3>

                                <div class="clear">
                                </div>
                            </div>
                            <!-- End .content-box-header -->
                            <div class="content-box-content">

                                <table id="formValidate1" class="input">
                                    <tbody>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">NAME_REPORT(<span
                                                class="req_Field">*</span>):
                                                            
                                            </td>
                                            <td class="tdinput tdwidth">
                                                <asp:TextBox ID="txtNAME_REPORT" runat="server" MaxLength="50" data-minlenght="1" data-control="checkUC" CssClass="text-input" Width="40%"
                                                    TabIndex="1"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">ADDRESS_FILE(<span
                                                class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtADDRESS_FILE" runat="server" MaxLength="50" data-minlenght="1" data-control='checkUC' CssClass="text-input" Width="40%"
                                                    TabIndex="4"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">REMARK(<span
                                                class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox ID="txtREMARK" runat="server" MaxLength="200" data-minlenght="1"  CssClass="text-input" Width="40%"
                                                    TabIndex="4"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">GROUP REPORT(<span
                                                class="req_Field">*</span>):
                                            </td>
                                            <td class="tdinput">
                                                <asp:DropDownList ID="ddlReport" CssClass="wid_40" runat="server"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlReport"
                                                                Display="Dynamic" ErrorMessage="Chưa nhập country name" Font-Size="Small" SetFocusOnError="True"
                                                                Font-Bold="false"></asp:RequiredFieldValidator>
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
                                        <asp:LinkButton runat="server" OnClientClick="return checkValidCustomMinlenght('checkUC');" ID="btnSave" CssClass="btn btn-sm btn-primary btn-bold btn-round"
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

    <script src="http://localhost/prjApplication/Scripts/DateTimeJQ/jquery-1.8.3.js"></script>
    <script src="http://localhost/prjApplication/Scripts/CustomDynamic.js"></script>
</asp:Content>
