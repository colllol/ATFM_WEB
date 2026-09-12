<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="ListPermNoDelete.aspx.cs" Inherits="prjApplication.Permission.ListPermNoDelete" %>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table border="0" cellpadding="0" width="100%" cellspacing="0">
        <tr>
            <td></td>
            <td style="text-align: left">
                <span class="TitlePanel">+ Flight LIST</span>
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td style="text-align: center">
                <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
                    <tr>
                        <td style="text-align: right">
                            <div class="classSearchHeader">
                                <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                                    <tr>
                                        <td style="width: 40%; text-align: left;">
                                            <asp:TextBox ID="txtSearch_UserName" Width="80%" CssClass="inputtext" runat="server"
                                                placeholder="Search ..."
                                                onkeypress="return clickButton(event,'ctl00_MainContent_linkSearch');"></asp:TextBox>
                                            <asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind"
                                                Font-Bold="true" OnClick="linkSearch_Click" Text="" Width="40px" Height="38px">
                                            </asp:Button>
                                        </td>

                                        <td style="width: 60%; text-align: left;">

                                            <%--<asp:LinkButton runat="server" ID="btnAddCountry" CssClass="btn btn-sm btn-primary btn-bold"
                                                OnClick="btnAddCountry_Click" CausesValidation="false">
                                                    <i class="ace-icon fa fa-pencil-square-o bigger-120 white"></i>
                                                    Thêm mới
                                            </asp:LinkButton>--%>
                                            <button type="button" data-toggle="modal" data-target="#MultiAdd" id="btnCreateNew"
                                                class="btn btn-sm btn-primary">
                                                <i class="glyphicon glyphicon-plus"></i>
                                                Create
                                            </button>
                                            <asp:Literal ID="lit" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px"></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView runat="server" ID="grdHis" AutoGenerateColumns="false" Visible="false" CssClass="table">
                                <Columns>
                                    <asp:TemplateField HeaderText="#">
                                        <ItemTemplate>
                                         <div class="action-buttons">
                                             <a data-toggle="tooltip" title="Restore">
                                                 <i class="glyphicon glyphicon-refresh bigger-130" 
                                                        onclick="RestoreHistory(<%# Eval("ID") %>, <%# Eval("NOVERSION") %>,<%# _user.UserID %>);"></i>
                                                </a>
                                             </a>
                                         </div>                                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="NOVERSION" HeaderText="No"/>
                                    <asp:BoundField DataField="ACTION" HeaderText="Action"/>
                                    <asp:BoundField DataField="LASTUSER" HeaderText="Last user"/>
                                    <asp:BoundField DataField="LASTMODIFY" HeaderText="Last modify"/>
                                    <asp:BoundField DataField="CONTENT" HeaderText="Content change"/>
                                </Columns>
                            </asp:GridView>                            
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px"></td>
                    </tr>
                    <tr>
                        <td style="text-align: right" class="pageNavTotal">
                            <cc1:CustomPaging ID="CustomPaging1" runat="server" NumberViewPage="5" PageSize="10" />
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
    <script>
        var phanCach = '<%= _phanCach %>';
        var phanCachArg = '<%= _phanCachArg%>';
        function RestoreHistory(id, ver, UserID) {
            GetArgWithPostBack(id + phanCach + ver + phanCach + UserID + phanCachArg+'RestoreHistory', 'RestoreHistory');
        }
        function DisplayResult(resulf, context) {
            if (context == 'RestoreHistory') {
                if (resulf != '') {
                    alert('Restore sussess!');
                } else alert('Restore error!');
            }
        }
        $('.show-details-btn').on('click', function (e) {
            e.preventDefault();
            $(this).closest('tr').next().toggleClass('open');
            $(this).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
        });
    </script>
</asp:Content>
