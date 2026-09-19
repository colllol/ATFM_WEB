<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadImg.aspx.cs" Inherits="prjApplication.Until.LoadImg" %>

<asp:datalist id="dlImages" datakeyfield="ID" runat="server" repeatcolumns="3" repeatdirection="Horizontal"
    repeatlayout="Table" width="100%" itemstyle-horizontalalign="Center" itemstyle-borderwidth="0"
    itemstyle-backcolor="#ffffff" itemstyle-borderstyle="Inset" itemstyle-width="20%"
    itemstyle-verticalalign="Middle" cellspacing="2" cellpadding="2" oneditcommand="dlImages_EditCommand">
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
                                                            <!--<asp:ImageButton ID="ImageButton_delete" OnClientClick="if (confirm('Bạn có chắc muốn xóa. Có thể có bài viết liên quan?')) return true; else return false;"
                                                                CommandName="edit" runat="server" ImageUrl="~/Images/cancel.gif" />-->
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:datalist>
<div class="classSearchHeader_Noborder" style="width: 96%; text-align: center">
    <img src="btn-quaylai.png" border="0" id="imgQuayLai" alt="Quay Lai" />
    <img src="btn-xemtiep.png" border="0" id="imgXemTiep" alt="Xem Tiep" />
</div>
