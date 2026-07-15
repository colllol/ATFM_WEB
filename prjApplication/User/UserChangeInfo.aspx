<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ITC.Master" AutoEventWireup="true"
    CodeBehind="UserChangeInfo.aspx.cs" Inherits="prjApplication.User.UserChangeInfo" %>

<%@ Register TagPrefix="nbc" Namespace="prjComponents.UI" Assembly="prjComponents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table border="0" cellpadding="0" width="100%" cellspacing="0">
        <tr>
            <td class="datagrid_top_left">
            </td>
            <td class="datagrid_top_center" style="text-align: left">
                <span class="TitlePanel">
                    <img alt="Update" style="margin-top: 8px;" src="../Images/Icons/key.gif" />
                    CẬP NHẬT THÔNG TIN</span>
            </td>
            <td class="datagrid_top_right">
            </td>
        </tr>
        <tr>
            <td class="datagrid_content_left">
            </td>
            <td style="text-align: center">
                <table cellspacing="2" cellpadding="2" width="100%" border="0">
                    <tr>
                        <td align="right" class="Titlelbl" style="width: 30%;">
                            Họ và tên: <span class="req_Field">*</span>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtFullName" runat="server" CssClass="inputtext" Width="60%"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="RFVtxtFullName" runat="server" ControlToValidate="txtFullName"
                                Display="Dynamic" ErrorMessage="Chưa nhập họ và tên" Font-Size="Small" SetFocusOnError="True"
                                Font-Bold="false"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Titlelbl">
                            Ngày sinh:
                        </td>
                        <td align="left">
                            <nbc:NetDatePicker CssClass="inputtext" ImageUrl="../Images/events.gif" ImageFolder="../scripts/DatePicker/Images"
                                Height="16px" Width="150px" ScriptSource="../scripts/datepicker.js" ID="txtBirth"
                                runat="server"></nbc:NetDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Titlelbl">
                            Địa chỉ:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="inputtext" Width="60%" TextMode="MultiLine"
                                Rows="4"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Titlelbl">
                            Thư điện tử:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="inputtext" Width="60%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Titlelbl">
                            Điện thoại:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="inputtext" Width="40%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="text-align: left" class="Titlelbl_ghichu">
                            <b>- <u>Ghi chú:</u></b> &nbsp;Các ô có đánh dấu <span class="req_Field">*</span>
                            là trường bắt buộc phải nhập.
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="text-align: left">
                            <asp:Button runat="server" ID="btnSave" CssClass="iconSave" Text="Thay đổi" OnClick="btnSave_Click" />
                            <asp:Button runat="server" CausesValidation="false" ID="btnBack" CssClass="iconExit" Text="Quay lại" OnClick="btnBack_Click" />
                            <!--<input type="button" id="btnBack" class="iconExit" value="Quay lại" onclick="history.back()" />-->
                        </td>
                    </tr>
                </table>
            </td>
            <td class="datagrid_content_right">
            </td>
        </tr>
        <tr>
            <td class="datagrid_bottom_left">
            </td>
            <td class="datagrid_bottom_center">
            </td>
            <td class="datagrid_bottom_right">
            </td>
        </tr>
    </table>
</asp:Content>
