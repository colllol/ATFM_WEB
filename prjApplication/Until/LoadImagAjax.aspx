<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadImagAjax.aspx.cs" Inherits="prjApplication.Until.LoadImagAjax" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc2" %>
<form runat="server" id="loadAjax">
<cc2:ToolkitScriptManager runat="Server" EnablePartialRendering="true" ID="ScriptManager1" />
<table style="width: 100%" cellpadding="2" cellspacing="2">
    <tr>
        <td>
            <span class="TitlePanel">+ ẢNH ĐÃ CẬP NHẬT</span>
        </td>
    </tr>
    <tr>
        <td style="text-align: left; width: 30%; vertical-align: top">
            <asp:updatepanel id="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:DataList ID="dlImages" DataKeyField="ID" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                            RepeatLayout="Table" Width="100%" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                            ItemStyle-BackColor="#ffffff" ItemStyle-BorderStyle="Inset" ItemStyle-Width="20%"
                                            ItemStyle-VerticalAlign="Middle" CellSpacing="2" CellPadding="2" OnEditCommand="dlImages_EditCommand">
                                            <ItemTemplate>
                                                <table border="0" cellspacing="1" cellpadding="1" width="100%" style="background-color: #f1f1f1">
                                                    <tr>
                                                        <td style="text-align: center; height: 100px; width: 100px; vertical-align: middle">
                                                            <img alt="<%#Cut_Filename(DataBinder.Eval(Container.DataItem,"ImageFileName"))%>"
                                                                src="<%#GetFileURL(DataBinder.Eval(Container.DataItem,"ImgeFilePath"))%>" border="1"
                                                                width="80px" height="60px" style="cursor: pointer;" title="<%#DataBinder.Eval(Container.DataItem,"ImageFileName")%>"
                                                                onclick="PreviewImage('<%#UrlPathImage_RemoveUpload(DataBinder.Eval(Container.DataItem,"ImgeFilePathOrizin"))%>','<%=strNumberArg%>','<%#DataBinder.Eval(Container.DataItem,"ImageFileSize")%>','<%#DataBinder.Eval(Container.DataItem,"ImageFileExtension")%>')">
                                                            <br />
                                                            <asp:Label ID="lbl_URL" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ImgeFilePath")%>'></asp:Label>
                                                            <asp:Label ID="lbl_ID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ID")%>'></asp:Label>
                                                            <asp:Label ID="lblFilename" Width="80px" runat="server" Text='<%#Cut_Filename(DataBinder.Eval(Container.DataItem,"ImageFileName"))%>'></asp:Label>
                                                            <br />
                                                            <asp:ImageButton ID="ImageButton_delete" OnClientClick="if (confirm('Bạn có chắc muốn xóa. Có thể có bài viết liên quan?')) return true; else return false;"
                                                                CommandName="edit" runat="server" ImageUrl="~/Images/cancel.gif" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </ContentTemplate>
                                </asp:updatepanel>
            <div class="classSearchHeader_Noborder" style="width: 96%; text-align: center">
                <asp:button id="cmdPrev" runat="server" text=" << " cssclass="iconNext" onclick="cmdPrev_Click">
                                    </asp:button>
                &nbsp;
                <asp:label id="lblCurrentPage" runat="server"></asp:label>
                <asp:button id="cmdNext" runat="server" text=" >> " cssclass="iconNext" onclick="cmdNext_Click">
                                    </asp:button>
            </div>
        </td>
        <td style="width: 70%; vertical-align: top; text-align: center;">
            <div id="ContainerCrop">
                <img id="imgCrop" src="" style="display: none">
            </div>
        </td>
    </tr>
</table>
</form>
