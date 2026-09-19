<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="ListPermNoIMP.aspx.cs" Inherits="prjApplication.PermIMP.ListPermNoIMP" EnableEventValidation="false" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Import Namespace="prjComponents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        
        #tblSource > tbody > tr > td, .table > tbody > tr > th, .table > tfoot > tr > td, .table > tfoot > tr > th, .table > thead > tr > td, .table > thead > tr > th {
                padding: unset;
            }

        #tblSource tr > td, tr > th {
            padding: unset;
            vertical-align: middle;
            text-align: center;
        }
        #tblSource input{border: none!important; color: black; text-transform: uppercase;}
        input{text-transform: uppercase;}
        .sInput {
            /*border-width: 0px !important;*/
            border: 0px;
            width: 100%;
        }
    </style>

    
    <asp:LinkButton runat="server" ID="btnExportExel" CssClass="btn btn-sm btn-primary btn-bold"
        OnClick="btnExcel_Click">
        <span class="glyphicon glyphicon-download-alt"></span>
        Export data into Excel
    </asp:LinkButton>
    <input runat="server" id="txtOper" placeholder="Select Oper" data-AutoComplete="OPER" />
    <asp:LinkButton ID="lnkBindToPerm" runat="server" CssClass="btn btn-primary"
        OnClick="lnkBindToPerm_Click" OnClientClick="if ($('#ctl00_MainContent_txtOper').val().trim() == '') {alert('Oper no value!'); $('#ctl00_MainContent_txtOper').focus();return false;} return confirm('Do you want render '+ $('#ctl00_MainContent_txtOper').val()+'?');"
        Text="Send to Permission"></asp:LinkButton>
    <asp:LinkButton ID="lnkDeleteBy" runat="server" CssClass="btn btn-sm btn-primary btn-bold"
        OnClick="lnkDeleteBy_Click" OnClientClick="return confirm('Do you want delete '+ $('#ctl00_MainContent_txtOper').val()+'?');"
        Text="Delete oper"></asp:LinkButton>
    <table id="tblSource" class="table table-bordered">
        <caption style="text-align: left">
            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search"
                OnClick="btnSearch_Click" />
            <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-primary" Text="Update"
                OnClick="btnUpdate_Click" />
        </caption>
        <thead>
            <tr>
                <th></th>
                <td>
                    <input id="txtSearchCALLSIGN" runat="server" data-oldvalue='<%# Eval("CALLSIGN") %>' value='<%# Eval("CALLSIGN") %>' class="sInput" /></td>
                <td>
                    <input id="txtSearchREGISTRATION" runat="server"  value='<%# Eval("REGISTRATION") %>' class="sInput"/>
                </td><!--
                <td>
                    <input id="txtSearchDAYFLY"  runat="server" data-oldvalue='<%# Eval("daily") %>' value='<%# Eval("daily") %>' class="sInput"/>
                </td>-->
                <td>
                    <input id="txtSearchCRAFT" runat="server" data-oldvalue='<%# Eval("CRAFT") %>' value='<%# Eval("CRAFT") %>' class="sInput"/>
                </td>
                <td>
                    <input id="txtSearchFROM_AIRP" runat="server" data-oldvalue='<%# Eval("FROM_AIRP") %>' value='<%# Eval("FROM_AIRP") %>' class="sInput" /></td>
                <td>
                    <input id="txtSearchTO_AIRP" runat="server" data-oldvalue='<%# Eval("TO_AIRP") %>' value='<%# Eval("TO_AIRP") %>' class="sInput"/></td>
                <td>
                    <input id="txtSearchETD" runat="server" data-oldvalue='<%# Eval("ETD") %>' value='<%# Eval("ETD") %>' class="sInput"/>
                </td>
                <td>
                    <input id="txtSearchETA" runat="server" data-oldvalue='<%# Eval("ETA") %>' value='<%# Eval("ETA") %>' class="sInput"/>
                </td>
                <td>
                    <input id="txtSearchVIA" runat="server" data-oldvalue='<%# Eval("VIA") %>' value='<%# Eval("VIA") %>' class="sInput"/>
                </td>
                <td>
                    <input id="txtSearchPERMTYPE" runat="server" data-oldvalue='<%# Eval("PERMTYPE") %>' value='<%# Eval("PERMTYPE") %>' class="sInput"/></td>
                <td>
                    <input id="txtSearchPERMNBR" runat="server" data-oldvalue='<%# Eval("PERMNBR") %>' value='<%# Eval("PERMNBR") %>' class="sInput"/></td>
                <td>
                    <input id="txtSearchREMARK" runat="server" data-oldvalue='<%# Eval("REMARK") %>' value='<%# Eval("REMARK") %>' class="sInput"/></td>
                <th></th>
            </tr>
            <tr>
                <th>No</th>
                <th>CALLSIGN</th>
                <th>REGIS</th>
                <!--<th>DAYFLY</th>-->
                <%--<th>FROMDATE</th>--%>
                <%--<th>TODATE</th>--%>
                <%--<th>DAILY</th>--%>
                <th>CRAFT</th>
                <th>FROM</th>
                <th>TO</th>
                <th>ETD</th>
                <th>ETA</th>
                <th>VIA</th>
                <th>TYPE</th>
                <th>PERMNBR</th>
                <th>REMARK</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater runat="server" ID="rptSource">
                <ItemTemplate>
                    <tr>
                        <td><%# (PhanTrang1.PageIndex*PhanTrang1.PageSize)+Container.ItemIndex+1 %></td>
                        <td>
                            <input id="txtCALLSIGN" class="wid_75px" runat="server" data-oldvalue='<%# Eval("CALLSIGN") %>' value='<%# Eval("CALLSIGN") %>' />
                        </td>
                        <td>
                            <input id="txtREGISTRATION" type="text" class="wid_100px" runat="server" data-oldvalue='<%# Eval("REGISTRATION") %>' value='<%# Eval("REGISTRATION") %>' />
                        </td><!--
                        <td>
                            <input   id="txtDAYFLY" runat="server" class="wid_100px" data-oldvalue='<%# Eval("daily") %>' value='<%# Eval("daily", "{0:dd/MM/yyyy}") %>' />
                        </td>-->
                        <td>
                            <input id="txtCRAFT" runat="server" class="wid_60px" data-oldvalue='<%# Eval("CRAFT") %>' value='<%# Eval("CRAFT") %>' />
                        </td>
                        <td>
                            <input id="txtFROM_AIRP" runat="server" class="wid_60px" data-oldvalue='<%# Eval("FROM_AIRP") %>' value='<%# Eval("FROM_AIRP") %>' /></td>
                        <td>
                            <input id="txtTO_AIRP" runat="server" class="wid_60px" data-oldvalue='<%# Eval("TO_AIRP") %>' value='<%# Eval("TO_AIRP") %>' />
                        </td>
                        <td>
                            <input id="txtETD" runat="server" class="wid_60px" data-oldvalue='<%# Eval("ETD") %>' value='<%# Eval("ETD") %>' />
                        </td>
                        <td>
                            <input id="txtETA" runat="server" class="wid_60px" data-oldvalue='<%# Eval("ETA") %>' value='<%# Eval("ETA") %>' />
                        </td>
                        <td>
                            <input id="txtVIA" class="wid_250px" runat="server" data-oldvalue='<%# Eval("VIA") %>' value='<%# Eval("VIA") %>' />
                        </td>
                        <td>
                            <input id="txtPERMTYPE" class="wid_50px" runat="server" data-oldvalue='<%# Eval("PERMTYPE") %>' value='<%# Eval("PERMTYPE") %>' />
                        </td>
                        <td>
                            <input id="txtPERMNBR" class="wid_75px" runat="server" data-oldvalue='<%# Eval("PERMNBR") %>' value='<%# Eval("PERMNBR") %>' />
                        </td>
                        <td>
                            <input id="txtREMARK" class="wid_200px" runat="server" data-oldvalue='<%# Eval("REMARK") %>' value='<%# Eval("REMARK") %>' />
                        </td>
                        <td>
                            <asp:LinkButton ID="lblDelete" OnClientClick="return confirm('Do you want delete?');" runat="server" OnClick="lblDelete_Click" Text="Delete" data-ID='<%# Eval("ID") %>'></asp:LinkButton>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>

    <div class="pageNavTotal text-left">
        <cc1:PhanTrang ID="PhanTrang1" runat="server" PageSize="50" NumberViewPage="7" OnPaging_IndexChange="PhanTrang1_Paging_IndexChange" />
    </div>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/CustumStaticdata.js"></script>
</asp:Content>
