<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="EditDocument.aspx.cs" Inherits="prjApplication.Static.Document.EditDocument" %>
<%@ Register TagPrefix="nbc" Namespace="prjComponents.UI" Assembly="prjComponents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../../Style/style_input.css" rel="stylesheet" />
    <link href="../../Style/Style_List.css" rel="stylesheet" />
    <script src="../../Style/assets/js/wizard.min.js"></script>
    <script src="../../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../../Style/assets/js/bootstrap-datepicker.min.js"></script>
   
    <link href="../Style/style_input.css" rel="stylesheet" />

    <script language="Javascript" type="text/javascript">
        function f_SubmitImage() {
            SubmitImage('<%=prjComponents.Global.TinPath%>' + 'UploadMulti/FileManagerment.aspx?vType=1&vKey=1', 840, 580);
        }
        function getPath(valuePath, numArg) {
            if (parseInt(numArg) == 1) {
                document.getElementById("ctl00_MainContent_txtValue").value = valuePath;
               
            }
        }
        function uploadOnchange() {
            if (document.getElementById("ctl00_MainContent_txtValue").value != '') {
               
            }
            else {
                document.getElementById("ctl00_MainContent_ImgViews").style.display = 'none';
            }
        }
        function ClearImage() {
            document.getElementById('ctl00_MainContent_txtValue').value = "";
           
        }
    </script>
    <table width="100%" cellpadding="0" cellspacing="0" align="center">
        <tr align="center">
            <td align="center">

                <div id="body-wrapper">
                    <div id="main-content">
                        <div class="content-box">
                            <!-- Start Content Box -->
                            <div class="content-box-header">
                                <h3 style="cursor: s-resize; margin-top: 0px; font-size: 12px;">+ EDIT Document</h3>

                                <div class="clear">
                                </div>
                            </div>
                            <!-- End .content-box-header -->
                            <div class="content-box-content">

                                <table class="input">
                                    <tbody>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">NAME(<span
                                                class="req_Field">*</span>):
                                                            
                                            </td>
                                            <td class="tdinput tdwidth">
                                                <asp:TextBox ID="txt_NAME" runat="server"  CssClass="text-input" Width="60%"
                                                    TabIndex="1"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFVtxtUserName" runat="server" ControlToValidate="txt_NAME"
                                                    Display="Dynamic" ErrorMessage="Chưa nhập NAME" CssClass="req_Field"
                                                    SetFocusOnError="True" Font-Size="Small"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">PATH
                                            </td>
                                            <td class="tdinput">
                                                <input id="txtValue" runat="server"  type="text" class="text-input" tabindex="3" style="width:60%"/>
                                                <input accesskey="S" onclick="f_SubmitImage()" type="button" class="PhotoSel" value="Browse"
                                                    name="cmd_SavePath2" tabindex="5" />
                                              
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-weight: bold;" class="tdinput">NOTE
                                            </td>
                                            <td class="tdinput">
                                                <textarea id="txtNote"  rows="3" runat="server"  type="text" class="text-input" tabindex="4" style="width:60%!important"/>
                                                
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