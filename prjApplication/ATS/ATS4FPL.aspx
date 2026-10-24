<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="ATS4FPL.aspx.cs" Inherits="prjApplication.ATS.ATS4FPL" %>

<%@ Register TagPrefix="cc1" Namespace="prjComponents.UI.WebControls" Assembly="prjComponents" %>
<%@ Import Namespace="prjComponents" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <link href="../Style/StyleReports.css" rel="stylesheet" />
    <link href="../Scripts/DateTimeJQ/jquery.datetimepicker.css" rel="stylesheet" />    
    <script src="../Scripts/DateTimeJQ/jquery.datetimepicker.js"></script>
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
     <style type="text/css">
        .w3-example {
        background-color: #f1f1f1;
       
    }
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
         .cssHide {
            display: none;
        }

        .cssShow {
            display: block;
        }

        .modal {
            overflow-x: hidden;
            overflow-y: auto;
            position: fixed;
            font-family: Arial, Helvetica, sans-serif;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            background: rgba(0,0,0,0.8);
            z-index: 99999;
            opacity: 1;
            -webkit-transition: opacity 400ms ease-in;
            -moz-transition: opacity 400ms ease-in;
            transition: opacity 400ms ease-in;
            pointer-events: visible;
        }

        .table td i:hover {
            cursor: pointer;
        } 
   </style>
     <asp:Panel ID="pnlList" runat="server">
        <span style="font-weight: bold;">+ EXPORT</span>

         <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
            <tr>
                <td style="text-align: right">
                    <div class="classSearchHeader">
                        <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                            <tr>
                                <td style="width: 40%; text-align: left;">                                    
                                </td>

                                <td style="width: 60%; text-align: left;">

                                    <input id="txtBEGINDATE" class="datepicker" autocomplete="off" runat="server"
                                 onkeypress='return check_num(this,14,event)' type="text" style="width:90px;" /> 
                                    <asp:Button id="btnOption" runat="server" OnClick="btnSelect_Click" Text="Select" class="btn btn-primary btn-sm" type="button"></asp:Button>
                                  
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
                     <asp:GridView Style="border-collapse: collapse;" runat="server" ID="grdListUser" AutoGenerateColumns="false" 
                           
                            Width="100%"
                            CssClass="Grid table table-striped table-bordered">
                            <Columns>
                                <asp:TemplateField HeaderText="STT">
                                    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                    <HeaderTemplate>
                                         FlightDate
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                         <%# DataBinder.Eval(Container.DataItem, "FlightDate","{0:dd/MM/yyyy}") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                    <HeaderTemplate>
                                        CALLSIGN
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <a style="cursor: pointer" class="linkGridForm" onclick="f_CallFPL('popupEdit1','<%# DataBinder.Eval(Container.DataItem, "CALLSIGN") %>','<%# DataBinder.Eval(Container.DataItem, "FLIGHTDATE") %>'); "> <%# DataBinder.Eval(Container.DataItem, "CALLSIGN") %></a>
                                       
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                    <HeaderTemplate>
                                        REG
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "REG") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                    <HeaderTemplate>
                                         SEL
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "SEL") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                    <HeaderTemplate>
                                        FROM
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "FROM_FPL") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                    <HeaderTemplate>
                                        TO
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "TO_FPL") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                    <HeaderTemplate>
                                        ETD
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "EET") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                 <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                    <HeaderTemplate>
                                        EET
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "EEET") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                    <HeaderTemplate>
                                        FLIGHTRULES
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "FLIGHTRULES") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                    <HeaderTemplate>
                                        AIRCRAFT
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "AIRCRAFT") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                    <HeaderTemplate>
                                        WTC
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "WTC") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                 <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                    <HeaderTemplate>
                                        EQUIPMENT
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "EQUIPMENT") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                    <HeaderTemplate>
                                        CRUISINGSPEED
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "CRUISINGSPEED") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                    <HeaderTemplate>
                                        LEVEL
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "LEVEL_FPL") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" Width="15%"></ItemStyle>
                                    <HeaderTemplate>
                                        ROUTE
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "ROUTE") %>
                                    </ItemTemplate>
                                </asp:TemplateField> 
                                 <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" Width="15%"></ItemStyle>
                                    <HeaderTemplate>
                                        CONTEXT
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "CONTEXT") %>
                                    </ItemTemplate>
                                </asp:TemplateField>                                  
                            </Columns>
                        </asp:GridView>

                </td>
            </tr>            
        </table>
          <div id="popupEdit1" class="modal cssHide" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="ClosePopup('popupEdit1');" class="close" data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">View flight</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-horizontal">
                                                                           
                        </div>
                    </div>
                    
                </div>
            </div>
        </div>
    </div>
    </asp:Panel>
      <script language="javascript" type="text/javascript">

        $('#<%= txtBEGINDATE.ClientID %>').datetimepicker({
            mask: '39/19/9999',
            timepicker: false,
            formatTime: '',
            format: 'd/m/Y',
            formatDate: 'd/m/Y'
        });

          function f_CallFPL(elem, callsign,date) {              
              //document.getElementById(elem).className = "modal cssShow";
              
          }
         
          function DisplayResult(resulf, context) {
          

            }
          function ClosePopup(elem) {
              IdSelect = '';
              document.getElementById(elem).className = "modal cssHide";

          }
    </script>
</asp:Content>
