<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InsertContents.aspx.cs"
    Inherits="prjApplication.ckeditor.plugins.InsertContents.InsertContents" %>

<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Register TagPrefix="cc1" Namespace="prjComponents.UI.WebControls" Assembly="prjComponents" %>
<%@ Import Namespace="prjComponents" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="REFRESH" content="1800" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="robots" content="INDEX,FOLLOW" />
    <title>Chèn nội dung tin bài</title>
    <link type="text/css" rel="Stylesheet" href="../../../Style/style.css" />

    <script type="text/javascript">
        var variable = null;
        var FCK = window.opener.CKEDITOR;
        function AjaxRequestContent(obj, url) {
            var xmlHttp;
            try {
                if (!document.all) { xmlHttp = new XMLHttpRequest() }
                else {
                    try { xmlHttp = new ActiveXObject("Msxml2.XMLHTTP") } catch (e)
                { xmlHttp = new ActiveXObject("Microsoft.XMLHTTP") }
                }
            } catch (e)
            { alert("Your browser does not support AJAX!"); return false };
            xmlHttp.onreadystatechange = function() {
                if (xmlHttp.readyState == 4) {
                    if (obj) {
                        var id = '<%=Request["editorID"] %>';
                        window.opener.InsertImage(id, xmlHttp.responseText);
                        window.close();
                    }
                }
            };
            xmlHttp.open("GET", url, true); xmlHttp.send(null)
        }
        function GetContent(_id) {
            if (_id != "") {
                var sLink = '<%=prjComponents.Global.ApplicationPath%>/Ajax/InsertContent.aspx?id=' + _id;
                AjaxRequestContent(FCK, sLink);
            }
        }
    </script>

    <script language="javascript" type="text/javascript" src="../../../Scripts/Lib.js"></script>

</head>
<body style="margin-top: 5px; margin-left: 5px; margin-right: 5px; margin-bottom: 5px">
    <form id="FormNews" method="post" runat="server">
    <div>
        <table border="0" cellpadding="0" width="100%" cellspacing="0">
            <tr>
                <td class="datagrid_top_left">
                </td>
                <td class="datagrid_top_center">
                    <span class="TitlePanel">+ CHÈN NỘI DUNG TIN VÀO BÀI VIẾT</span>
                </td>
                <td class="datagrid_top_right">
                </td>
            </tr>
            <tr>
                <td class="datagrid_content_left">
                </td>
                <td style="text-align: center">
                    <table border="0" cellpadding="1" cellspacing="1" style="width: 100%">
                        <tr>
                            <td style="width: 8%; text-align: left; padding-left: 2px;" class="Titlelbl">
                                Loại báo:
                            </td>
                            <td style="width: 80%; text-align: left">
                                <anthem:DropDownList AutoCallBack="true" ID="cboNgonNgu" runat="server" Width="150px"
                                    CssClass="inputtext" DataTextField="Languages_Name" DataValueField="Languages_ID"
                                    OnSelectedIndexChanged="cbo_lanquage_SelectedIndexChanged" TabIndex="1">
                                </anthem:DropDownList>
                                &nbsp; <span class="Titlelbl">Chuyên mục:</span>
                                <anthem:DropDownList AutoCallBack="true" ID="cbo_chuyenmuc" runat="server" Width="460px"
                                    CssClass="inputtext" DataTextField="tenchuyenmuc" DataValueField="id" TabIndex="5">
                                </anthem:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 8%; text-align: left; padding-left: 2px;" class="Titlelbl">
                                Tên bài viết:
                            </td>
                            <td style="width: 80%; text-align: left">
                                <asp:TextBox ID="txt_tieude" TabIndex="1" Width="680px" runat="server" CssClass="inputtext"
                                    onkeypress="return clickButton(event,'cmdSeek');"></asp:TextBox>
                                <asp:Button runat="server" ID="cmdSeek" CssClass="iconFind" Font-Bold="true" OnClick="cmdSeek_Click"
                                    Text="Tìm kiếm"></asp:Button>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellspacing="2" cellpadding="2" border="0">
                        <tr>
                            <td align="left" colspan="2">
                                <asp:DataGrid ID="dgr_tintuc1" runat="server" Width="100%" AutoGenerateColumns="False"
                                    OnItemDataBound="dgr_tintuc1_ItemDataBound" DataKeyField="News_ID" CssClass="Grid">
                                    <ItemStyle CssClass="GridItem"></ItemStyle>
                                    <AlternatingItemStyle CssClass="GridAltItem" />
                                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="News_ID" HeaderText="News_ID" Visible="False"></asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="<%$ Resources:Strings, BA_QLND_NEWS_NAME %>">
                                            <HeaderStyle HorizontalAlign="Left" Width="32%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left" Width="32%"></ItemStyle>
                                            <ItemTemplate>
                                                <a class="linkGridForm" href="#" onclick="GetContent('<%#Eval("News_ID")%>');">
                                                    <%#Eval("News_Tittle")%>
                                                </a>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Chuyên mục">
                                            <HeaderStyle Width="12%" HorizontalAlign="Left"></HeaderStyle>
                                            <ItemStyle Width="12%" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <%#prjBusinessLogic.UltilFunc.GetCategoryName(Eval("CAT_ID"))%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Ngày xuất bản">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.News_DatePublished")!=System.DBNull.Value?Convert.ToDateTime(DataBinder.Eval(Container, "DataItem.News_DatePublished")).ToString("dd/MM/yyyy HH:mm:ss"):"" %>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="8%"></ItemStyle>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
                                            <HeaderStyle HorizontalAlign="center" Width="6%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="center" Width="6%"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Literal runat="server" ID="Literal7" Text="<%$ Resources:Strings, BA_QLND_VIEW_PRINT %>"></asp:Literal>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <a href="Javascript:open_window_Scroll('<%=Global.ApplicationPath%>/Article/ViewAndPrint.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "News_ID") %>',50,500,100,800);" />
                                                <img src='<%= Global.ApplicationPath %>/images/view.gif' border="0" alt="Xem /In"
                                                    onmouseover="(window.status=''); return true" style="cursor: pointer;" title="Xem /In">
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <input class="iconExit" type="button" value="Đóng" onclick="window.parent.close()" />
                            </td>
                            <td style="text-align: right" class="pageNav">
                                <cc1:CurrentPage runat="server" ID="CurrentPage2" CssClass="pageNavTotal">
                                </cc1:CurrentPage>
                                <cc1:Pager runat="server" ID="pages" OnIndexChanged="pages_IndexChanged">
                                </cc1:Pager>
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
    </div>
    </form>
</body>
</html>
