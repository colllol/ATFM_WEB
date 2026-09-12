<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="ListFlight_Fixes.aspx.cs" Inherits="prjApplication.Flight_Fixes.ListFlight_Fixes" %>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc2" %>

<%@ Register TagPrefix="cc1" Namespace="prjComponents.UI.WebControls" Assembly="prjComponents" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Import Namespace="prjComponents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/Style_List.css" rel="stylesheet" />
    <link href="../Scripts/DateTimeJQ/jquery.datetimepicker.css" rel="stylesheet" />    
    <script src="../Scripts/DateTimeJQ/jquery.datetimepicker.js"></script>
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <span style="font-weight: bold;">+ FLIGHT FIXES LIST</span>

    <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
       
        <tr>
            <td style="text-align: right;">
                <div class="classSearchHeader">
                    <table border="0" style="text-align: right; width: 100%; color: white;">
                        <tr>
                             <td style="width: 20%; text-align: left;">
                                  <input id="txtFromDate" class="datepicker" autocomplete="off" runat="server"
                                 onkeypress='return check_num(this,14,event)' type="text" style="width:90px;" /> 
                               
                                <input id="txtFromTime" runat="server" data-minlenght="1" data-control="checkAccess" type="text"
                                    placeholder="select time" class="wid_50px" maxlength='6'  value="00:00" />
                                
                                <input id="txtToTime" runat="server" data-minlenght="1" data-control="checkAccess" type="text"
                                    placeholder="select time" class="wid_50px" maxlength='6'  value="23:59" />
                             </td>
                            <td style="width: 20%; text-align: left;">
                                <asp:TextBox ID="txtSearch_UserName" Width="80%" CssClass="inputtext" runat="server"
                                    placeholder="Search FIXNAME"
                                    onkeypress="return clickButton(event,'ctl00_MainContent_linkSearch');"></asp:TextBox>
                            </td>
                            <td style="width: 20%; text-align: left;">
                                <asp:TextBox ID="txtFixkind" Width="80%" CssClass="inputtext" runat="server"
                                    placeholder="FIXKIND"
                                    onkeypress="return clickButton(event,'ctl00_MainContent_linkSearch');"></asp:TextBox>
                            </td>
                            <td style="width: 10%; text-align: left;">
                                
                                <asp:TextBox ID="txtSearch_Sector" Width="100%" CssClass="inputtext" runat="server"
                                    placeholder="SECTOR"
                                    onkeypress="return clickButton(event,'ctl00_MainContent_linkSearch');"></asp:TextBox>
                               
                            </td>
                           
                            <td>
                                 <asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind"
                                    Font-Bold="true" OnClick="linkSearch_Click" Text="" Width="40px" Height="38px"></asp:Button>
                            </td>

                            <td style="width: 10%; text-align: left;color:red;">
                                
                                

                                <asp:Literal ID="lit" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
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
                            <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                TIMESTAMP
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "TIMESTAMP")).ToString("dd/MM/yyyy HH:mm") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                CALLSIGN
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "CALLSIGN") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                ADEP
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "ADEP") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                ADES
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "ADES") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                LATITUDE
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "LATITUDE") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                LONGITUDE
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "LONGITUDE") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                FIXKIND
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "FIXKIND") %>
                            </ItemTemplate>
                        </asp:TemplateField>



                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                FIXNAME
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "FIXNAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                DISPLAYEDETO
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "DISPLAYEDETO") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                REFERENCEETO
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "REFERENCEETO") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                CURRENTSECTOR
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "CURRENTSECTOR") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                FLIGHTRULEAFTERFIX
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "FLIGHTRULEAFTERFIX") %>
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
                <cc2:CustomPaging ID="CustomPaging1" runat="server" NumberViewPage="7" PageSize="10" />
            </td>
        </tr>
    </table>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/CustomPaging.js"></script>
    <script src="../Scripts/CustumStaticdata.js"></script>
    <script src="../Scripts/tableHeadFixer.js"></script>
   
    <script language="javascript" type="text/javascript">

        $('#<%= txtFromDate.ClientID %>').datetimepicker({
            mask: '39/19/9999',
            timepicker: false,
            formatTime: '',
            format: 'm/d/Y',
            formatDate: 'm/d/Y'
        });       
    </script>

</asp:Content>
