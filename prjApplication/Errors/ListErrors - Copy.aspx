<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="ListErrors.aspx.cs" Inherits="prjApplication.Errors.ListErrors" %>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <span style="font-weight: bold;">+ ERRORS</span>
    <link href="../Style/assets/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/bootstrap-datepicker3.min.css" rel="stylesheet" />
    <link href="../Style/Style_List.css" rel="stylesheet" />
    <style type="text/css">
        #dhtmltooltip {
            position: absolute;
            width: 150px;
            border: 2px solid black;
            padding: 2px;
            background-color: lightyellow;
            visibility: hidden;
            z-index: 100;
            /*Remove below line to remove shadow. Below line should always appear last within this CSS*/
            filter: progid:DXImageTransform.Microsoft.Shadow(color=gray,direction=135);
        }
    </style>
    <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
        <tr>
            <td style="text-align: right">
                <div class="classSearchHeader">
                    <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                        <tr>
                            <td style="width: 40%; text-align: left;">
                                <asp:TextBox ID="txtSearch_UserName" Width="20%" CssClass="inputtext" runat="server"
                                    placeholder="Search NBR"
                                    onkeypress="return clickButton(event,'ctl00_MainContent_linkSearch');"></asp:TextBox>
                                <asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind"
                                    Font-Bold="true" OnClick="linkSearch_Click" Text="" Width="40px" Height="38px"></asp:Button>
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
                <asp:GridView Style="border-collapse: collapse;" runat="server" ID="grdSource" AutoGenerateColumns="false"                  
                    Width="100%"
                    CssClass="Grid table-condensed">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                NBR
                            </HeaderTemplate>
                            <ItemTemplate>                               
                                    <%# DataBinder.Eval(Container.DataItem, "NBR") %>
                            </ItemTemplate>
                        </asp:TemplateField>  
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                MESSAGE
                            </HeaderTemplate>
                            <ItemTemplate>                               
                                    <%--<%# DataBinder.Eval(Container.DataItem, "MESSAGE") %>--%>
                                <span onmouseover="ddrivetip('<%# Eval("MESSAGE").ToString().Replace("\n", "</br>").Replace("'","") %>', 'yellow', 500);"
                            onmouseout="hideddrivetip();">
                            <%#  Eval("MESSAGE").ToString().Length>=60?Eval("MESSAGE").ToString().Substring(0, 60)+".....":Eval("MESSAGE").ToString() %></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                LETTERNBR_PK
                            </HeaderTemplate>
                            <ItemTemplate>
                                 <%# DataBinder.Eval(Container.DataItem, "LETTERNBR_PK") %>
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
                <cc1:PhanTrang ID="PhanTrang1" runat="server" PageSize="10" NumberViewPage="7" OnPaging_IndexChange="PhanTrang1_Paging_IndexChange"/>
            </td>
        </tr>
        
    </table>
    <div id="dhtmltooltip"></div>
    <script src="../Style/assets/js/jquery-1.11.3.min.js"></script>    
    <script src="../Scripts/CustomDynamic.js"></script>
</asp:Content>
