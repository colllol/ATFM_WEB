<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="RpKhiTuong.aspx.cs" Inherits="prjApplication.Report.KhiTuong.RpKhiTuong" %>

<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
    <span class="TitlePanel">+ WMO</span>
    <fieldset class="box-border">

        <table style="width: 100%; border: 0px solid black;">
            <tr>
                <td>
                    <label for="txtPERMDATE" style="font-size: 12px;">Date</label><br />
                    <input style="height: 25px;" id="txtPERMDATE" runat="server" 
                        
                        data-date-format="dd/mm/yyyy" type="text" class="wid_80px" />
                    &nbsp;
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPERMDATE"
                                                    Display="Dynamic" ErrorMessage="*" CssClass="req_Field" SetFocusOnError="True" Font-Size="Small"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <label for="txtFromHours" style="font-size: 12px;">From Hours</label><br />
                    <input style="height: 25px;" id="txtFromHours" runat="server" maxlength="4" data-number="true" data-control="update"
                        data-minlenght="1" type="text"
                        class="wid_80px" />
                     &nbsp;
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFromHours"
                                                    Display="Dynamic" ErrorMessage="*" CssClass="req_Field" SetFocusOnError="True" Font-Size="Small"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <label for="txtToHours" style="font-size: 12px;">To Hours</label><br />
                    <input style="height: 25px;" id="txtToHours" runat="server" maxlength="4" data-number="true" data-control="update"
                        data-minlenght="1" type="text"
                        class="wid_80px" />
                     &nbsp;
                     <asp:RequiredFieldValidator ID="RFVtxtUserName" runat="server" ControlToValidate="txtToHours"
                                                    Display="Dynamic" ErrorMessage="*" CssClass="req_Field" SetFocusOnError="True" Font-Size="Small"></asp:RequiredFieldValidator>
                </td>

            </tr>
            <tr>
                <td>
                    <label for="txtREFERENCE" style="font-size: 12px;">Airport</label><br />

                    <asp:DropDownCheckBoxes ID="ddlAirport" runat="server"
                        AddJQueryReference="True" UseButtons="false" UseSelectAllNode="True">
                        <Style SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130" />
                        <Texts SelectBoxCaption="Select Airport" />
                    </asp:DropDownCheckBoxes>
                    &nbsp;
                    <asp:ExtendedRequiredFieldValidator ID="ExtendedRequiredFieldValidator1" runat="server"
                        ControlToValidate="ddlAirport" ErrorMessage="*" ForeColor="Red"></asp:ExtendedRequiredFieldValidator>
                </td>
                <td>
                    <label for="txtOper" style="font-size: 12px;">Oper</label><br />

                    <asp:DropDownCheckBoxes ID="ddlOper" runat="server"
                        AddJQueryReference="True" UseButtons="false" UseSelectAllNode="True">
                        <Style SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130" />
                        <Texts SelectBoxCaption="Select Oper" />
                    </asp:DropDownCheckBoxes>
                     &nbsp;
                    <asp:ExtendedRequiredFieldValidator ID="ExtendedRequiredFieldValidator2" runat="server"
                        ControlToValidate="ddlOper" ErrorMessage="*" ForeColor="Red"></asp:ExtendedRequiredFieldValidator>
                </td>
                <td >
                        <asp:LinkButton runat="server" ID="btnExportExel" CssClass="btn btn-sm btn-primary btn-bold"
                                        OnClick="btnExcel_Click">
                                                        <span class="glyphicon glyphicon-download-alt"></span>
                                                        Export data into Excel
                       </asp:LinkButton>
                </td>


            </tr>

        </table>
    </fieldset>

    <div class="table-responsive">
        <style type="text/css">
            .tg {
                border-collapse: collapse;
                border-spacing: 0;
            }

                .tg td {
                    font-family: Arial, sans-serif;
                    font-size: 14px;
                    padding: 10px 5px;
                    border-style: solid;
                    border-width: 1px;
                    overflow: hidden;
                    word-break: normal;
                    border-color: black;
                }

                .tg th {
                    font-family: Arial, sans-serif;
                    font-size: 14px;
                    font-weight: normal;
                    padding: 10px 5px;
                    border-style: solid;
                    border-width: 1px;
                    overflow: hidden;
                    word-break: normal;
                    border-color: black;
                }

                .tg .tg-9fhq {
                    background-color: #68cbd0;
                    text-align: center;
                }
        </style>
        <table class="tg" style="undefined; table-layout: fixed; width: 100%">
            <colgroup>
                <col style="width: 38px">
                <col style="width: 133px">
                <col style="width: 134px">
                <col style="width: 130px">
                <col style="width: 97px">
                <col style="width: 159px">
            </colgroup>
            <tr>
                <th class="tg-9fhq" rowspan="2">STT</th>
                <th class="tg-9fhq" rowspan="2">AIRPORT</th>
                <th class="tg-9fhq" rowspan="2">OPER</th>
                <th class="tg-9fhq" colspan="3">REPORT</th>
            </tr>
            <tr>
                <td class="tg-9fhq">Take Off</td>
                <td class="tg-9fhq">Landing</td>
                <td class="tg-9fhq">Total</td>
            </tr>
            <asp:Label ID="lblBindData" runat="server"></asp:Label>
        </table>
    </div>

    <script language="javascript" type="text/javascript">

        $('#<%= txtPERMDATE.ClientID %>').datetimepicker({
            mask: '39/19/9999',
            timepicker: false,
            formatTime: '',
            format: 'd/m/Y',
            formatDate: 'd/m/Y'
        });

    </script>
</asp:Content>
