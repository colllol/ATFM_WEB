<%@ Page Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="ListReportingRoute.aspx.cs" Inherits="prjApplication.Static.REPORTING_ROUTE.ListReportingRoute" EnableEventValidation="false"%>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc2" %>
<%@ Register TagPrefix="cc1" Namespace="prjComponents.UI.WebControls" Assembly="prjComponents" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Import Namespace="prjComponents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../../Style/Style_List.css" rel="stylesheet" />
    <span style="font-weight: bold;">+ REPORTING ROUTE LIST</span>

    <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
        <tr>
            <td style="text-align: right">
                <div class="classSearchHeader">
                    <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                        <tr>
                            <td style="width: 40%; text-align: left;">
                                <asp:TextBox ID="txtSearch_UserName" Width="80%" CssClass="inputtext" runat="server"
                                    placeholder="Search ROUTE_NAME"
                                    onkeypress="return clickButton(event,'ctl00_MainContent_linkSearch');"></asp:TextBox>
                                <asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind"
                                    Font-Bold="true" OnClick="linkSearch_Click" Text="" Width="40px" Height="38px"></asp:Button>
                            </td>
                            <td style="width: 60%; text-align: left;">
                               
                            <asp:LinkButton runat="server" ID="btnExportExel" CssClass="btn btn-sm btn-primary btn-bold"
                                OnClick="btnExcel_Click">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Export data into Excel
                            </asp:LinkButton>

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

                <asp:GridView Style="border-collapse: collapse;" runat="server" ID="grdSource" AutoGenerateColumns="false" DataKeyField="ID"
                    OnRowCommand="grdSource_RowCommand" OnRowDataBound="grdSource_RowDataBound"
                    Width="100%"
                    CssClass="Grid table-condensed">
                    <Columns>
                        <asp:BoundField Visible="False" DataField="ID">
                            <HeaderStyle Width="1%"></HeaderStyle>
                        </asp:BoundField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                TÊN ĐƯỜNG
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="linkGridForm" Text='<%# DataBinder.Eval(Container.DataItem, "ROUTE_NAME") %>'
                                    Enabled='<%# _Role.R_Edit %>' CommandArgument='<%# Eval("ROUTE_ID") %>' CommandName="Edit"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="30%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="30%"></ItemStyle>
                            <HeaderTemplate>
                                CÁC ĐIỂM BÁO CÁO
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "POINT_NAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>    
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="30%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="30%"></ItemStyle>
                            <HeaderTemplate>
                                SỐ KM
                            </HeaderTemplate>
                            <ItemTemplate>                                
                                <%# DataBinder.Eval(Container.DataItem, "SOKM") %>
                            </ItemTemplate>
                        </asp:TemplateField>                                         
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                Delete
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" Width="25px" runat="server" ImageUrl="~/images/delete.png"
                                    ImageAlign="AbsMiddle" OnClientClick="return confirm('Do you want delete?');" ToolTip="Delete ReportingPoint" CommandName="DeleteReportingRoute"
                                    CommandArgument='<%# Eval("ROUTE_ID") %>' BorderStyle="None" Visible='<%#_Role.R_Del %>'></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                Add Route
                            </HeaderTemplate>
                            <ItemTemplate>
                                <a runat="server" data-toggle="tooltip" visible='<%#_Role.R_Edit %>' title="Add">
                                    <i class="ace-icon fa fa-pencil bigger-130" data-toggle="modal" data-target="#popupAddRoute"
                                        onclick="ShowpopupAddRoute(<%# Eval("ROUTE_ID") %>);">
                                    </i>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td style="height: 10px"></td>
        </tr>
        <tr>
            <td style="text-align: right" class="pageNavTotal">
                <cc2:PhanTrang ID="PhanTrang1" runat="server" PageSize="10" NumberViewPage="7" OnPaging_IndexChange="PhanTrang1_Paging_IndexChange" />
            </td>
        </tr>
    </table>
    <div id="popupAddRoute" class="modal fade" role="dialog" tabindex="-1" data-backdrop="false"
        aria-hidden="true">
        <div class="modal-dialog ">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="ClosePopup('popupAddRoute');" class="close" data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">Add Route</h4>
                </div>
                <div class="modal-body" id="showTable">
                <asp:GridView Style="border-collapse: collapse;" runat="server" ID="GridView1" AutoGenerateColumns="false"
                    Width="100%"
                    CssClass="Grid table-condensed">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                MARK
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input runat="server" id="chkCheck" type="checkbox" onchange='chkCheck_Onchane(this)' data-pointID='<%# Eval("POINT_ID") %>' checked='<%#  Eval("MARK").ToString()=="1"?true:false %>' />                             
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                POINT_NAME
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "POINT_NAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                DESCRIPTION
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "DESCRIPTION") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                IN_FIR_VN
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnIsIN_FIR_VN" runat="server" ImageUrl='<%#IsStatusGet(DataBinder.Eval(Container.DataItem, "IN_FIR_VN").ToString())%>'
                                    ImageAlign="AbsMiddle" ToolTip="Trạng thái" Width="25px" BorderStyle="None" />
                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                FIR_HN
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnIsFIR_HN" runat="server" ImageUrl='<%#IsStatusGet(DataBinder.Eval(Container.DataItem, "FIR_HN").ToString())%>'
                                        ImageAlign="AbsMiddle" ToolTip="Trạng thái" Width="25px" BorderStyle="None" />
                                
                            </ItemTemplate>
                        </asp:TemplateField>  
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                FIR_HCM
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnIsFIR_HCM" runat="server" ImageUrl='<%#IsStatusGet(DataBinder.Eval(Container.DataItem, "FIR_HCM").ToString())%>'
                                    ImageAlign="AbsMiddle" ToolTip="Trạng thái" Width="25px" BorderStyle="None" />                               

                            </ItemTemplate>
                        </asp:TemplateField>                                                                        
                    </Columns>
                </asp:GridView>
                </div>
                
            </div>
        </div>
    </div>

   
    <script>
        var IdSelect = 0;
        var eleSelect = '';
        var element;
        function ShowpopupAddRoute(id) {
            IdSelect = id;
            GetArgWithPostBack(id + '_____GetAllReportingPoint', 'GetAllReportingPoint');
        }
        
        function DisplayResult(resulf, context) {
            if (resulf == '') return;
            if (context == 'GetAllReportingPoint') {
                document.getElementById('showTable').innerHTML = resulf;
            }
            if (context == 'chkCheck_Onchane') {
                if (resulf != 'true') {
                    $(element).prop('checked', !$(element).prop('checked'));
                    alert('Update error!');
                } else alert('Update sussess!');
                LoadDataGird();
            }
            if (context == 'LoadDataGird') {                
                document.getElementById('<%= grdSource.ClientID%>').innerHTML = resulf;
            }

            
            
        }
        function ReadInfoPerm(data) {
            var obj = JSON.parse(data);
            txtMARK.value = obj['MARK'];
            txtPOINT_NAME.value = obj['POINT_NAME'];
            txtCOMPULSORY_ON_REQUEST.value = obj['COMPULSORY_ON_REQUEST'];
            txtDESCRIPTION.value = obj['DESCRIPTION'];
            txtIN_FIR_VN.value = obj['IN_FIR_VN'];
            txtPOINT_ID.value = obj['POINT_ID'];
            txtFIR_HN.value = obj['FIR_HN'];
            txtFIR_HCM.value = obj['FIR_HCM'];
        }
        function chkCheck_Onchane(ele) {
           
            if (IdSelect != 0) {
                element = ele;
                eleSelect = $(ele).prop('id');
                var p = document.getElementById(eleSelect).getAttribute('data-pointid')
                var c = $(ele).prop('checked') ? '1' : '0'
                

                GetArgWithPostBack(IdSelect + '::::' + p + '::::' + c + '_____chkCheck_Onchane', 'chkCheck_Onchane');
            }
        }
       
        function LoadDataGird() {
            GetArgWithPostBack('LoadDataGird_____LoadDataGird', 'LoadDataGird');
        }        
       
    </script>
</asp:Content>
