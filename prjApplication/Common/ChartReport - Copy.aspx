<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="ChartReport.aspx.cs" Inherits="prjApplication.Common.ChartReport" %>

<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="../Scripts/jsapi.js"></script>
   <script type="text/javascript" src="../Scripts/uds_api_contents.js"></script> 
    <script type="text/javascript" src="../Scripts/CustomDynamic.js"></script> 
     <script type="text/javascript">
         function drawChart() {
          <%=drawChart_ByDate("data","option","chart","visualization","Schedule Flight")%>          
     }

     google.setOnLoadCallback(drawChart);
    </script> 
    <span class="TitlePanel">+ Chart</span>
    <fieldset class="box-border">

        <table style="width: 80%; border: 0px solid black;">
            <tr>
                <td>
                    <label for="txtPERMDATE" style="font-size: 12px;">Date</label>
                    <input style="height: 25px;" id="txtPERMDATE" runat="server" 
                        onblur="checkInputDate(this)"
                        data-date-format="dd/mm/yyyy" type="text" class="wid_80px" />
                    &nbsp;
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPERMDATE"
                                                    Display="Dynamic" ErrorMessage="*" CssClass="req_Field" SetFocusOnError="True" Font-Size="Small"></asp:RequiredFieldValidator>
                </td>
                 <td >
                     <label for="txtREFERENCE" style="font-size: 12px;">Airport</label>
                      <asp:DropDownList ID="ddlAirport" CssClass="text-input" runat="server"
                                                            class="wid_80px">
                                                 </asp:DropDownList>  
                     
                    
                </td>
                  <td >
                       <a href="#" onclick="CallURL();" class="btn btn-primary">
                         <span class="glyphicon glyphicon-download-alt"></span>
                                                    Search
                    </a>
                      <a href="#" onclick="LineURL();" class="btn btn-primary">
                         <span class="glyphicon glyphicon-report-alt"></span>
                                                    Line Chart
                    </a>
                                        
                </td>
                <td>
                    Ngưỡng năng lực giới hạn sân bay
                    <asp:Literal runat="server" ID="litLevel"></asp:Literal>
                    chuyến/giờ
                </td>
            </tr>
            

        </table>
    </fieldset>

    <div class="table-responsive">
        <style type="text/css">
            .tg {
                border-collapse: collapse;
                border-spacing: 0;
            }

                .tg td {
                    font-family: Arial, sans-serif;
                    font-size: 14px;
                    padding: 10px 5px;
                    border-style: solid;
                    border-width: 1px;
                    overflow: hidden;
                    word-break: normal;
                    border-color: black;
                }

                .tg th {
                    font-family: Arial, sans-serif;
                    font-size: 14px;
                    font-weight: normal;
                    padding: 10px 5px;
                    border-style: solid;
                    border-width: 1px;
                    overflow: hidden;
                    word-break: normal;
                    border-color: black;
                }

                .tg .tg-9fhq {
                    background-color: #68cbd0;
                    text-align: center;
                }
        </style>
        <table class="tg" style="undefined; table-layout: fixed; width: 100%">            
            <tr>
                <td>
                    <div id="visualization" style="width: 1100px; height: 400px;"></div>
                </td>                                
            </tr>
             <tr>
             <td>
                <asp:DataGrid runat="server" ID="grdList" AutoGenerateColumns="false" 
                        Width="100%" 
                        CssClass="Grid">
                        <ItemStyle CssClass="GridItem"></ItemStyle>
                        <AlternatingItemStyle CssClass="GridAltItem" />
                        <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                        <Columns>                            
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H1
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h1").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h1").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H2
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h1").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h1").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H3
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h2").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h2").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H4
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h3").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h3").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H5
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h4").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h4").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H6
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h5").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h5").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H7
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h6").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h6").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H8
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h7").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h7").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H9
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h8").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h8").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H11
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h9").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h9").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H11
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h11").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h11").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H12
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h12").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h12").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H13
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h13").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h13").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H14
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h14").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h14").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H15
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h15").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h15").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H16
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h16").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h16").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H17
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h17").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h17").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H18
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h18").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h18").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H19
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h19").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h19").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H20
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h20").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h20").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H21
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h21").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h21").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H22
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h22").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h22").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H23
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h23").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h23").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    H24
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h24").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0]) + Convert.ToInt32(DataBinder.Eval(Container.DataItem, "h24").ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1]) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                                                                                                        
                        </Columns>
                    </asp:DataGrid>
             </td>
             </tr>          
        </table>
    </div>

    <script language="javascript" type="text/javascript">      
        $(document).ready(function ($) {

            //var currentTime = new Date();
            //var month = currentTime.getMonth() + 1;
            //var day = currentTime.getDate();
           // var year = currentTime.getFullYear();
           // var date1 = day + "/" + month + "/" + year; // output
            var _fdate = getParameterByName('fdate'); // "lorem"
            var _fairport = getParameterByName('fairport');
            $('#ctl00_MainContent_txtPERMDATE').val(_fdate);
            $('#ctl00_MainContent_ddlAirport').val(_fairport);
            //if (_fdate != null) date1 = _fdate;
            
        }
        );
            
        function CallURL() {
            
            var _fairport = $('#ctl00_MainContent_ddlAirport').val();

            window.open("ChartReport.aspx?fdate=" + $('#ctl00_MainContent_txtPERMDATE').val() + "&fairport=" + _fairport, "_self")
        }
        function LineURL() {
            var _fairport = $('#ctl00_MainContent_ddlAirport').val();
            window.open("ChartReportLine.aspx?fdate=" + $('#ctl00_MainContent_txtPERMDATE').val() + "&fairport=" + _fairport, "_self")
        }
        function getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }

    </script>
</asp:Content>
