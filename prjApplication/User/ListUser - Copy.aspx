<%@ Page Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ListUser.aspx.cs" Inherits="prjApplication.User.ListUser" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Import Namespace="prjComponents" %>

<asp:Content ID="cp_ListUser" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="pnlList" runat="server">
        <span style="font-weight: bold;">+ USER LIST</span>

        <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
            <tr>
                <td style="text-align: right">
                    <div class="classSearchHeader">
                        <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                            <tr>
                                <td style="width: 40%; text-align: left;">
                                    <asp:TextBox ID="txtSearch_UserName" Width="80%" CssClass="inputtext" runat="server" placeholder="Tên tài khoản"
                                        onkeypress="return clickButton(event,'ctl00_MainContent_linkSearch');"></asp:TextBox>
                                    <asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind"
                                        Font-Bold="true" OnClick="linkSearch_Click" Text="" Width="40px" Height="38px"></asp:Button>
                                </td>

                                <td style="width: 60%; text-align: left;">

                                    <asp:LinkButton runat="server" ID="btnAddMenu" CssClass="btn btn-sm btn-primary btn-bold"
                                        OnClick="btnAddMenu_Click" CausesValidation="false">
                                                    <i class="ace-icon fa fa-pencil-square-o bigger-120 white"></i>
                                                    Thêm mới
                                    </asp:LinkButton>
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
                    <asp:GridView Visible="false" Style="border-collapse: collapse;" runat="server"
                         ID="GridView1" AutoGenerateColumns="false" DataKeyField="UserID"
                   
                    Width="100%"
                    CssClass="Grid table-condensed">
                    <Columns>
                        <asp:BoundField Visible="False" DataField="UserID">
                            <HeaderStyle Width="1%"></HeaderStyle>
                        </asp:BoundField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                UserName
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "UserName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                FullName
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "UserFullName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                UserActive
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "UserActive") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                ADDRESS
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "USERADDRESS") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                                             
                    </Columns>
                </asp:GridView>
                    <asp:DataGrid runat="server" ID="grdListUser" AutoGenerateColumns="false" DataKeyField="UserID"
                        OnEditCommand="grdListUser_EditCommand" Width="100%" OnItemDataBound="grdListUser_ItemDataBound"
                        CssClass="Grid">
                        <ItemStyle CssClass="GridItem"></ItemStyle>
                        <AlternatingItemStyle CssClass="GridAltItem" />
                        <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="UserID">
                                <HeaderStyle Width="1%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                                <HeaderTemplate>
                                    Tên tài khoản
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditUser" runat="server" CssClass="linkGridForm" Text='<%# DataBinder.Eval(Container.DataItem, "UserName") %>'
                                        ToolTip="Chỉnh sửa thông tin người dùng" Enabled='<%#_Role.R_Edit %>' CommandName="Edit"
                                        CommandArgument="EditUsers"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                <HeaderTemplate>
                                    Họ và tên
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="linkGridForm" Text='<%# DataBinder.Eval(Container.DataItem, "UserFullName") %>'
                                        ToolTip="Chỉnh sửa thông tin người dùng" Enabled='<%#_Role.R_Edit %>' CommandName="Edit"
                                        CommandArgument="EditUsers"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                                <HeaderTemplate>
                                    Thuộc nhóm
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnGroup" Width="25px" runat="server" ImageUrl="~/images/group.png"
                                        ImageAlign="AbsMiddle" ToolTip="Nhóm người dùng" CommandName="Edit" CommandArgument="Group"
                                        BorderStyle="None" Visible='<%#_Role.R_Edit %>'></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>

                            <asp:TemplateColumn>
                                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                                            <HeaderTemplate>
                                                Báo cáo
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnRoleLang" Width="25px" runat="server" ImageUrl="~/images/language.png"
                                                    ImageAlign="AbsMiddle" ToolTip="Phân quyền báo cáo" CommandName="Edit" CommandArgument="RoleLang"
                                                    BorderStyle="None" Visible='<%#_Role.R_Edit %>'></asp:ImageButton>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                                <HeaderTemplate>
                                    Chức năng
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnRole" Width="25px" runat="server" ImageUrl="~/images/function.png"
                                        ImageAlign="AbsMiddle" ToolTip="Phân quyền sử dụng chức năng" CommandName="Edit"
                                        CommandArgument="Role" BorderStyle="None" Visible='<%#_Role.R_Edit %>'></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <%--<asp:TemplateColumn>
                                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                                            <HeaderTemplate>
                                                Chuyên mục
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnRoleCategory" Width="25px" runat="server" ImageUrl="~/images/list.png"
                                                    ImageAlign="AbsMiddle" ToolTip="Phân quyền sử dụng chuyên mục" CommandName="Edit"
                                                    CommandArgument="RoleCategory" BorderStyle="None" Visible='<%#_Role.R_Edit %>'></asp:ImageButton>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>--%>

                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                                <HeaderTemplate>
                                    Trạng thái
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnIsReporter" runat="server" ImageUrl='<%#IsStatusGet(DataBinder.Eval(Container.DataItem, "UserActive").ToString())%>'
                                        ImageAlign="AbsMiddle" ToolTip="Trạng thái kích hoạt" Width="25px" CommandName="Edit" CommandArgument="IsReporter"
                                        BorderStyle="None" Visible='<%#_Role.R_Edit %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                                <HeaderTemplate>
                                    Khôi phục MK
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnResetPass" Width="25px" runat="server" ImageUrl="~/images/Restore.png"
                                        ImageAlign="AbsMiddle" ToolTip="Khôi phục về mặc khẩu mặc định" CommandName="Edit"
                                        CommandArgument="ResetPass" BorderStyle="None" Visible='<%#_Role.R_Edit %>'></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                                <HeaderTemplate>
                                    Xóa
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDelete" Width="25px" runat="server" ImageUrl="~/images/delete.png"
                                        ImageAlign="AbsMiddle" ToolTip="Xóa thông tin người dùng" CommandName="Edit"
                                        CommandArgument="Delete" BorderStyle="None" Visible='<%#_Role.R_Del %>'></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>

                </td>
            </tr>
            <tr>
                <td style="height: 10px"></td>
            </tr>
            <%--<tr>
                <td style="text-align: right" class="pageNavTotal">
                    <cc1:CurrentPage runat="server" ID="currentPage"></cc1:CurrentPage>
                    &nbsp;<cc1:Pager runat="server" ID="pages" OnIndexChanged="pages_IndexChanged" />
                </td>
            </tr>--%>
            <tr>
                <td style="text-align: right" class="pageNavTotal">
                    <cc1:CustomPaging ID="CustomPaging1" runat="server" NumberViewPage="5" PageSize="10" />
                </td>
            </tr>
        </table>

    </asp:Panel>
    <!--Phan Quyen Nguoi Dung-->
    <asp:Panel runat="server" ID="plRole" Visible="false" CssClass="TitlePanel" BackColor="white"
        BorderStyle="NotSet">
        <script language="Javascript" type="text/javascript">

            function CheckAllandUnCheckAll(objRef) {
                var GridView = document.getElementById('<%=gdListMenu.ClientID%>');
                var inputList = GridView.getElementsByTagName("input");
                for (var i = 0; i < inputList.length; i++) {
                    var row = inputList[i].parentNode.parentNode;
                    if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                        if (objRef.checked && inputList[i].disabled == false) {
                            inputList[i].checked = true;
                        }
                        else {
                            inputList[i].checked = false;
                        }
                    }
                }
            }

        </script>
        <table border="0" cellpadding="0" width="100%" cellspacing="0">
            <tr>
                <td class="datagrid_top_left"></td>
                <td class="datagrid_top_center">
                    <span class="TitlePanel">+ GÁN QUYỀN SỬ DỤNG CHỨC NĂNG CHO NGƯỜI DÙNG :
                        <asp:Label ID="roleChucNang" runat="server"></asp:Label></span>
                </td>
                <td class="datagrid_top_right"></td>
            </tr>
            <tr>
                <td class="datagrid_content_left"></td>
                <td style="text-align: center">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <%-- <tr>
                            <td>
                                <table id="Table1" style="color: #000000;" cellspacing="0" cellpadding="0" width="100%"
                                    border="0">
                                    <tr>
                                        <td align="right" width="100%" bgcolor="#ffffff">
                                            &nbsp;&nbsp; Phân quyền người dùng&nbsp;
                                            <asp:DropDownList ID="cbo_nguoidung" runat="server" Width="250px" Height="20px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-bottom: 5px;">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>--%>
                        <tr>
                            <td class="TitlePanel" style="height: 25px; text-align: right"></td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:DataGrid runat="server" ID="gdListMenu" OnItemDataBound="gdListMenu_ItemDataBound"
                                    AutoGenerateColumns="false" DataKeyField="Menu_ID" Width="100%" CssClass="Grid">
                                    <ItemStyle CssClass="GridItem"></ItemStyle>
                                    <AlternatingItemStyle CssClass="GridAltItem" />
                                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <HeaderStyle HorizontalAlign="Center" Width="3%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="3%"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAll" onclick="javascript:CheckAllandUnCheckAll(this);" runat="server"
                                                    ToolTip="Select/Deselect All"></asp:CheckBox>
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="optSelect" Enabled='<%# Eval("Role_Group") %>' runat="server" Checked='<%# Eval("Role_Menu") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
                                            <HeaderStyle Width="15%"></HeaderStyle>
                                            <ItemStyle></ItemStyle>
                                            <HeaderTemplate>
                                                Tên danh mục
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# SetNameMenu(DataBinder.Eval(Container.DataItem, "Menu_Name"), DataBinder.Eval(Container.DataItem, "Menu_ID"))%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
                                            <HeaderStyle Width="15%"></HeaderStyle>
                                            <ItemStyle></ItemStyle>
                                            <HeaderTemplate>
                                                Mô tả
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Menu_Desc")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Thêm" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="6%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkR_Add" Enabled='<%# Eval("Role_Group") %>' runat="server" Checked='<%# Eval("R_Add") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Sửa" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="6%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkR_Edit" Enabled='<%# Eval("Role_Group") %>' runat="server" Checked='<%# Eval("R_Edit") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Xóa" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="6%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkR_Del" Enabled='<%# Eval("Role_Group") %>' runat="server" Checked='<%# Eval("R_Del") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Duyệt" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="6%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkR_Pub" Enabled='<%# Eval("Role_Group") %>' runat="server" Checked='<%# Eval("R_Pub") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px"></td>
                        </tr>
                        <tr>
                            <td>

                                <asp:LinkButton runat="server" ID="linkApplly" CssClass="btn btn-sm btn-primary btn-bold btn-round"
                                    OnClick="btnApplyRole_Click">
                                                     <i class="ace-icon fa fa-floppy-o bigger-120 white"></i>
                                                        Lưu giữ
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" ID="btnThoat" CssClass="btn btn-sm btn-warning btn-round" CausesValidation="false"
                                    OnClick="btnCancelRole_Click">
                                                     <i class="ace-icon fa fa-times white"></i>
                                                        Thoát
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="datagrid_content_right"></td>
            </tr>
            <tr>
                <td class="datagrid_bottom_left"></td>
                <td class="datagrid_bottom_center"></td>
                <td class="datagrid_bottom_right"></td>
            </tr>
        </table>
    </asp:Panel>
    <!--End-->
    <!--Phan Quyen Nhom Nguoi Dung-->
    <asp:Panel runat="server" ID="plGroup" Visible="false" CssClass="TitlePanel">
        <table border="0" cellpadding="0" width="100%" cellspacing="0">
            <tr>
                <td class="datagrid_top_left"></td>
                <td class="datagrid_top_center">
                    <span class="TitlePanel">+ GÁN NHÓM CHO NGƯỜI DÙNG:
                        <asp:Label ID="lblThuocNhom" runat="server"></asp:Label></span>
                </td>
                <td class="datagrid_top_right"></td>
            </tr>
            <tr>
                <td class="datagrid_content_left"></td>
                <td style="text-align: center">
                    <table border="0" cellspacing="5" cellpadding="2" width="100%" align="center">
                        <tr>
                            <td style="width: 45%; text-align: right" valign="top">
                                <table style="width: 100%" cellspacing="2" cellpadding="2">
                                    <tr>
                                        <td style="height: 25px; background-color: #CAC9C0; font-weight: bold; font-size: 12px; font-family: Arial; text-align: left">
                                            <img src="<%=Global.ApplicationPath%>/Images/15200913357373.png" align="left" />
                                            &nbsp;DANH SÁCH NHÓM NGƯỜI DÙNG
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="height: 25px; background-color: #CAC9C0">
                                            <anthem:ListBox ID="leftPane" runat="server" CssClass="inputtext" Width="100%" Height="200px" Rows="7"
                                                DataValueField="Group_ID" AutoUpdateAfterCallBack="true" DataTextField="Group_Name"
                                                TabIndex="1">
                                            </anthem:ListBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 5%; text-align: center; vertical-align: middle">
                                <table style="width: 100%" cellspacing="2" cellpadding="2">
                                    <tr>
                                        <td align="center" valign="middle">
                                            <anthem:Button runat="server" ID="Button1" Style="cursor: hand" CssClass="myButton"
                                                Width="50" TabIndex="3" Text=">>" OnClick="btAddAll_Click"></anthem:Button>
                                            <anthem:Button runat="server" ID="btAddOne" Style="cursor: hand" CssClass="myButton"
                                                Width="50" TabIndex="3" Text=">" OnClick="btAddOne_Click"></anthem:Button>
                                            <anthem:Button runat="server" ID="btRemoveOne" Style="cursor: hand" CssClass="myButton"
                                                Width="50" TabIndex="4" Text="<" OnClick="btRemoveOne_Click"></anthem:Button>
                                            <anthem:Button runat="server" ID="Button2" Style="cursor: hand" CssClass="myButton"
                                                Width="50" TabIndex="4" Text="<<" OnClick="btRemoveAll_Click"></anthem:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>

                            <td valign="top" align="center">
                                <table style="width: 100%" cellspacing="2" cellpadding="2">
                                    <tr>
                                        <td style="height: 25px; background-color: #CAC9C0; font-weight: bold; font-size: 12px; font-family: Arial; text-align: left">
                                            <img src="<%=Global.ApplicationPath%>/Images/Group.gif" align="left" />
                                            &nbsp;Thành viên thuộc nhóm
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="height: 25px; background-color: #CAC9C0">
                                            <anthem:ListBox ID="rightPane" runat="server" Width="100%" Rows="7" Height="200px" CssClass="inputtext"
                                                DataValueField="Group_ID" AutoUpdateAfterCallBack="true" DataTextField="Group_Name"
                                                TabIndex="6">
                                            </anthem:ListBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellspacing="5" cellpadding="2" width="475" align="center">
                        <tr>
                            <td style="height: 20px; text-align: center; vertical-align: middle"></td>
                        </tr>
                        <tr>
                            <td align="center">

                                <asp:LinkButton runat="server" ID="LinkButton1" CssClass="btn btn-sm btn-primary btn-bold btn-round"
                                    OnClick="linkGroupApplly_Click">
                                                     <i class="ace-icon fa fa-floppy-o bigger-120 white"></i>
                                                        Lưu giữ
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" ID="LinkButton2" CssClass="btn btn-sm btn-warning btn-round" CausesValidation="false"
                                    OnClick="linkGroupExit_Click">
                                                     <i class="ace-icon fa fa-times white"></i>
                                                        Thoát
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="datagrid_content_right"></td>
            </tr>
            <tr>
                <td class="datagrid_bottom_left"></td>
                <td class="datagrid_bottom_center"></td>
                <td class="datagrid_bottom_right"></td>
            </tr>
        </table>
    </asp:Panel>
    <!--End-->
    <!--Phan Quyen Categorys-->
    <asp:Panel runat="server" ID="pnlRoleCategorys" Visible="false" CssClass="TitlePanel"
        BackColor="white" BorderStyle="NotSet">
        <table border="0" cellpadding="0" width="100%" cellspacing="0">
            <tr>
                <td class="datagrid_top_left"></td>
                <td class="datagrid_top_center">
                    <span class="TitlePanel">+ GÁN QUYỀN SỬ DỤNG CHUYÊN MỤC CHO NGƯỜI DÙNG:<asp:Label
                        ID="roleChuyenMuc" runat="server"></asp:Label>
                    </span>
                </td>
                <td class="datagrid_top_right"></td>
            </tr>
            <tr>
                <td class="datagrid_content_left"></td>
                <td style="text-align: center">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td align="right" style="width: 100%">
                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                    <tr>
                                        <td class="Titlelbl" style="width: 10%; text-align: right">Chuyên trang:
                                        </td>
                                        <td style="width: 90%; text-align: left">
                                            <asp:DropDownList ID="ddlLang" AutoPostBack="true" Width="200px" CssClass="inputtext"
                                                runat="server" OnSelectedIndexChanged="ddlLang_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 4px"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:DataGrid runat="server" ID="dgListCategorys" OnItemDataBound="gdListMenu_ItemDataBound"
                                    AutoGenerateColumns="false" DataKeyField="Categorys_ID" Width="100%" CssClass="Grid">
                                    <ItemStyle CssClass="GridItem"></ItemStyle>
                                    <AlternatingItemStyle CssClass="GridAltItem" />
                                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxes(this);" runat="server"
                                                    ToolTip="Select/Deselect All"></asp:CheckBox>
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="optSelect" Enabled='<%# DataBinder.Eval(Container.DataItem, "Role_Group") %>'
                                                    runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "Role_Cate") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
                                            <HeaderTemplate>
                                                Tên chuyên mục
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#DataBinder.Eval(Container.DataItem, "Category_Name")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
                                            <HeaderStyle Width="20%"></HeaderStyle>
                                            <HeaderTemplate>
                                                Thứ tự
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Category_Order")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px"></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="linkRoleCateSaves" OnClick="linkRoleCateSaves_Click"
                                    CssClass="iconLuu" Text="<%$ Resources:Strings, BUTTON_SAVES %>" />
                                <asp:Button runat="server" ID="linkRoleCateExit" OnClick="linkRoleCateExit_Click"
                                    CssClass="iconThoat" Text="<%$ Resources:Strings, BUTTON_SIGOUT %>" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="datagrid_content_right"></td>
            </tr>
            <tr>
                <td class="datagrid_bottom_left"></td>
                <td class="datagrid_bottom_center"></td>
                <td class="datagrid_bottom_right"></td>
            </tr>
        </table>
    </asp:Panel>
    <!--End-->
    <!--Phan Quyen ChuyenDe-->
    <asp:Panel runat="server" ID="pnlRolesChuyenDe" Visible="false" CssClass="TitlePanel"
        BackColor="white" BorderStyle="NotSet">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td align="left"></br>
                    <asp:Label ID="Label2" runat="server" CssClass="FieldText" Text="Chuyên trang:"></asp:Label>
                    <asp:DropDownList ID="ddlChuyenDe" AutoPostBack="true" Width="200" runat="server"
                        OnSelectedIndexChanged="ddlChuyenDe_SelectedIndexChanged" CssClass="inputtext">
                    </asp:DropDownList>
                    </br></br>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:DataGrid runat="server" ID="dgChuyenDe" OnItemDataBound="gdListMenu_ItemDataBound"
                        AutoGenerateColumns="false" DataKeyField="Categorys_ID" Width="100%" CssClass="Grid">
                        <ItemStyle CssClass="GridItem"></ItemStyle>
                        <AlternatingItemStyle CssClass="GridAltItem" />
                        <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                        <Columns>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" Width="3%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="3%"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxes(this);" runat="server"
                                        ToolTip="Select/Deselect All"></asp:CheckBox>
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="optSelect" Enabled='<%# DataBinder.Eval(Container.DataItem, "Role_Group") %>'
                                        runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "Role_Cate") %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle Width="20%"></HeaderStyle>
                                <HeaderTemplate>
                                    Tên chuyên mục
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "Category_Name")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle Width="20%"></HeaderStyle>
                                <HeaderTemplate>
                                    Thứ tự
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "Category_Order")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td style="height: 10px"></td>
            </tr>
            <tr>
                <td>
                    <br />
                    <div style="clear: both; text-align: center;">
                        <asp:Button runat="server" ID="linkRoleSavesChuyenDe" OnClick="linkRoleSavesChuyenDe_Click"
                            CssClass="iconLuu" Text="<%$ Resources:Strings, BUTTON_SAVES %>" />
                        <asp:Button runat="server" ID="linkRoleExitChuyenDe" OnClick="linkRoleExitChuyenDe_Click"
                            CssClass="iconThoat" Text="<%$ Resources:Strings, BUTTON_SIGOUT %>" />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <!--End-->
    <!--Phan Quyen Languages-->
    <asp:Panel runat="server" ID="pnlLanguages" Visible="false" CssClass="TitlePanel"
        BackColor="white" BorderStyle="NotSet">
        <table border="0" cellpadding="0" width="100%" cellspacing="0">
            <tr>
                <td class="datagrid_top_left"></td>
                <td class="datagrid_top_center">
                    <span class="TitlePanel"></span>
                </td>
                <td class="datagrid_top_right"></td>
            </tr>
            <tr>
                <td class="datagrid_content_left"></td>
                <td style="text-align: center">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td class="TitlePanel" style="height: 25px; text-align: left">
                                <asp:Label ID="lblRoleNgonNgu" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px"></td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:DataGrid runat="server" ID="dgListLanguages" OnItemDataBound="gdListMenu_ItemDataBound"
                                    AutoGenerateColumns="false" DataKeyField="ID" Width="100%" CssClass="Grid">
                                    <ItemStyle CssClass="GridItem"></ItemStyle>
                                    <AlternatingItemStyle CssClass="GridAltItem" />
                                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <HeaderStyle HorizontalAlign="Center" Width="3%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="3%"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxes(this);" runat="server"
                                                    ToolTip="Select/Deselect All"></asp:CheckBox>
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="optSelect" Enabled='<%# DataBinder.Eval(Container.DataItem, "Role_Group") %>'
                                                    runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "Role") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
                                            <HeaderStyle Width="20%"></HeaderStyle>
                                            <HeaderTemplate>
                                                Danh sách báo cáo
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#DataBinder.Eval(Container.DataItem, "NAME_REPORT")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <%--<asp:TemplateColumn>
                                            <HeaderStyle Width="10%"></HeaderStyle>
                                            <HeaderTemplate>
                                                Mô tả
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Description")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>--%>
                                        <%--<asp:TemplateColumn>
                                            <HeaderStyle Width="10%"></HeaderStyle>
                                            <HeaderTemplate>
                                                Code
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Code")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>--%>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px"></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="linkRoleSavesLanguages" OnClick="linkRoleSavesLanguages_Click"
                                    CssClass="iconLuu" Text="<%$ Resources:Strings, BUTTON_SAVES %>" />
                                <asp:Button runat="server" ID="linkRoleExitLanguages" OnClick="linkRoleExitLanguages_Click"
                                    CssClass="iconThoat" Text="<%$ Resources:Strings, BUTTON_SIGOUT %>" />
                            </td>
                        </tr>
                    </table>
                </td>

                <td class="datagrid_content_right"></td>
            </tr>
            <tr>
                <td class="datagrid_bottom_left"></td>
                <td class="datagrid_bottom_center"></td>
                <td class="datagrid_bottom_right"></td>
            </tr>
        </table>
    </asp:Panel>
    <!--End-->
    <!--Phan Quyen Loai bao-->
    <asp:Panel runat="server" ID="panelLoaibao" Visible="false" CssClass="TitlePanel"
        BackColor="white" BorderStyle="NotSet">
        <table border="0" cellpadding="0" width="100%" cellspacing="0">
            <tr>
                <td class="datagrid_top_left"></td>
                <td class="datagrid_top_center">
                    <span class="TitlePanel"></span>
                </td>
                <td class="datagrid_top_right"></td>
            </tr>
            <tr>
                <td class="datagrid_content_left"></td>
                <td style="text-align: center">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td class="TitlePanel" style="height: 25px; text-align: right">
                                <asp:Label ID="lbLoaibao" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:DataGrid runat="server" ID="drgListPaper" OnItemDataBound="gdListMenu_ItemDataBound"
                                    AutoGenerateColumns="false" DataKeyField="Paper_ID" Width="100%" CssClass="Grid">
                                    <ItemStyle CssClass="GridItem"></ItemStyle>
                                    <AlternatingItemStyle CssClass="GridAltItem" />
                                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <HeaderStyle HorizontalAlign="Center" Width="3%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="3%"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxes(this);" runat="server"
                                                    ToolTip="Select/Deselect All"></asp:CheckBox>
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="optSelect" Enabled='<%# DataBinder.Eval(Container.DataItem, "Role_Group") %>'
                                                    runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "Role_Paper") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
                                            <HeaderStyle Width="20%"></HeaderStyle>
                                            <HeaderTemplate>
                                                Loại báo
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#DataBinder.Eval(Container.DataItem, "Paper_Name")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
                                            <HeaderStyle Width="10%"></HeaderStyle>
                                            <HeaderTemplate>
                                                Mô tả
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Description")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 2px"></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="Button3" OnClick="linkRoleSavesPaper_Click" CssClass="iconSave"
                                    Text="<%$ Resources:Strings, BUTTON_SAVES %>" />
                                <asp:Button runat="server" ID="Button4" OnClick="linkRoleExitPaper_Click" CssClass="iconExit"
                                    Text="<%$ Resources:Strings, BUTTON_SIGOUT %>" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="datagrid_content_right"></td>
                <tr>
                    <td class="datagrid_bottom_left"></td>
                    <td class="datagrid_bottom_center"></td>
                    <td class="datagrid_bottom_right"></td>
                </tr>
        </table>
    </asp:Panel>
    <!--End-->
</asp:Content>
