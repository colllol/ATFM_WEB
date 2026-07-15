<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="FlightDeleted.aspx.cs" Inherits="prjApplication.FlightManagement.FlightDeleted" %>

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
                                                <%--<button type="button" data-toggle="modal" data-target="#MultiAdd" id="btnCreateNew"
                                                    class="btn btn-sm btn-primary">
                                                    <i class="glyphicon glyphicon-plus"></i>
                                                    Create
                                                </button>--%>
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

                                <asp:GridView runat="server" ID="grdSource" AutoGenerateColumns="false" DataKeyNames="ID"
                                    Width="100%"
                                    CssClass="table table-striped table-bordered table-hover">  
                                    <Columns>
                                        <asp:BoundField Visible="False" DataField="ID">
                                            <HeaderStyle Width="1%"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="4%"></ItemStyle>
                                            <HeaderTemplate>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div class="action-buttons" style="width: 65px">   
                                                    <a runat="server" data-toggle="tooltip" visible='<%#_Role.R_Del %>' title="Restore">
                                                        <i class="glyphicon	glyphicon-share bigger-130" onclick="btnRestoreOnclick(<%# Eval("FLIGHT_PK") %>);">
                                                        </i>
                                                    </a>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                                            <HeaderTemplate>
                                                Begin date
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("BEGINDATE", "{0:dd/MM/yyyy}") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                End date
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("ENDDATE", "{0:dd/MM/yyyy}") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                MTOW
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("MTOW") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                CARAFT_ID
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("CARAFT_ID") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                FLIGHT_PK
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("FLIGHT_PK") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                DAY1
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("DAY1") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                DAY2
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("DAY2") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                DAY3
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("DAY3") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                DAY4
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("DAY4") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                DAY5
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("DAY5") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                DAY6
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("DAY6") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                DAY7
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("DAY7") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                REMARK
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("REMARK") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                REGISTRATION
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("REGISTRATION") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                FLIGHTNBR
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("FLIGHTNBR") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                TO_AIRP
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("TO_AIRP") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                FROM_AIRP
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("FROM_AIRP") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                ETD
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("ETD") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                PURPOSE_ID
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("PURPOSE_NAME") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                ETA
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("ETA") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                VIA
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("VIA") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>                                
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <cc1:CustomPaging ID="CustomPaging1" runat="server" NumberViewPage="7" PageSize="20" />
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
    <script src="http://localhost/prjApplication/Scripts/CustomDynamic.js"></script>
    <script>
        var _objRender = JSON.parse('<%= _ObjRender%>');
        function btnRestoreOnclick(id) {
            GetArgWithPostBack(id + '_____btnRestoreOnclick', 'btnRestoreOnclick');
        }
        function LoadDataGrid() {
            GetArgWithPostBack('LoadDataGrid_____LoadDataGrid', 'LoadDataGrid');
        }
        function DisplayResult(resulf, context) {
            if(context=='btnRestoreOnclick'){
                if (resulf == 'OK') {
                    alert('Restore sussess!');
                    LoadDataGrid();
                }
            }            
            if (context == 'LoadDataGrid') {
                document.getElementById('<%= grdSource.ClientID%>').innerHTML = resulf;
            }
        }
        
    </script>
</asp:Content>
