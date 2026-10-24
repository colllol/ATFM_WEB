<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="CalendarDayFlightDeleted.aspx.cs" Inherits="prjApplication.CalendarFlight.CalendarDayFlightDeleted" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/Style_List.css" rel="stylesheet" />
    <span class="TitlePanel">+ DAY FLIGHT DELETED</span>
    <table border="0" style="width: 100%; color: white;" class="table table-condensed">

        <tr>
            <td>
                <asp:GridView runat="server" ID="grdHis" DataKeyNames="FLIGHT_ID"
                    OnRowDataBound="grdSource_RowDataBound" AutoGenerateColumns="false"
                    CssClass="table">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <div class="action-buttons">
                                    <a data-toggle="tooltip" title="Restore">
                                        <i class="glyphicon glyphicon-refresh bigger-130"
                                            onclick="RestoreHistory(<%# Eval("FLIGHT_ID") %>, <%# Eval("NOVERSION") %>,<%# _user.UserID %>);"></i>
                                    </a>
                                    </a>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="NOVERSION" HeaderText="No" />
                        <asp:BoundField DataField="ACTION" HeaderText="Action" />
                        <asp:BoundField DataField="LASTUSER" HeaderText="Last user" />
                        <asp:BoundField DataField="LASTMODIFY" HeaderText="Last modify" />
                        <asp:BoundField DataField="CONTENT" HeaderText="Content change" />
                    </Columns>
                </asp:GridView>


            </td>
        </tr>
       
    </table>


    <script>
        var phanCach = '<%= _phanCach %>';
        var phanCachArg = '<%= _phanCachArg%>';
        function RestoreHistory(id, ver, UserID) {
            GetArgWithPostBack(id + phanCach + ver + phanCach + UserID + phanCachArg + 'RestoreHistory', 'RestoreHistory');
        }
        function DisplayResult(resulf, context) {
            if (context == 'RestoreHistory') {
                if (resulf != 'NOK') {
                    alert('Restore sussess!');
                    LoadDataGrid();
                } else alert('Restore error!');
            }
            if (context == 'LoadDataGrid') {
                document.getElementById('<%= grdHis.ClientID%>').innerHTML = resulf;
        }
    }
    function LoadDataGrid() {
        GetArgWithPostBack('LoadDataGrid' + phanCachArg + 'LoadDataGrid', 'LoadDataGrid');

    }
    $('.show-details-btn').on('click', function (e) {
        e.preventDefault();
        $(this).closest('tr').next().toggleClass('open');
        $(this).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
    });
    </script>
</asp:Content>
