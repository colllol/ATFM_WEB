<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="ListViaReports.aspx.cs" Inherits="prjApplication.Static.VIA.ListViaReports" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Import Namespace="prjComponents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <span style="font-weight: bold;">+ VIA LIST</span>
    <link href="../../Style/Style_List.css" rel="stylesheet" />
    <link href="../../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Style/assets/css/autocomplete.css" rel="stylesheet" />

    
     <style>
                         
        .preloader {
            display: inline-block;
            padding: 0px;
            border-radius: 100%;
            border: 2px solid;
            border-top-color: rgba(0,0,0, 0.65);
            border-bottom-color: rgba(0,0,0, 0.15);
            border-left-color: rgba(0,0,0, 0.65);
            border-right-color: rgba(0,0,0, 0.15);
            -webkit-animation: preloader 0.8s linear infinite;
            animation: preloader 0.8s linear infinite;
        }

        @keyframes preloader {
            from {
                transform: rotate(0deg);
            }

            to {
                transform: rotate(360deg);
            }
        }

        @-webkit-keyframes preloader {
            from {
                -webkit-transform: rotate(0deg);
            }

            to {
                -webkit-transform: rotate(360deg);
            }
        }

    </style>
    <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
        <tr>
            <td style="text-align: right">
                <div class="classSearchHeader">
                    

                                <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                                    <tr>
                                        <td style="width: 10%;">
                                            <asp:TextBox ID="txtBEGINDATE" Width="100%" CssClass="inputtext" runat="server"
                                                placeholder="FROM DATE" class="datepicker" autocomplete="off"
                                                onkeypress='return check_num(this,14,event)'></asp:TextBox>
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:TextBox ID="txtTODATE" Width="100%" CssClass="inputtext" runat="server"
                                                placeholder="TO DATE" class="datepicker" autocomplete="off"
                                                onkeypress='return check_num(this,14,event)'></asp:TextBox>
                                        </td>
                                        <td style="width:5%;">
                                            <select id="ddlType" class="disabled" runat="server" style="width: 100%;height:34px;" onchange="ddlType_Change();">
                                                <option value="0">-All-</option>
                                                <option value="QN">QN</option>
                                                <option value="QT">QT</option>
                                            </select>
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:TextBox ID="txtOper" Width="80%" CssClass="inputtext" runat="server"
                                                placeholder="OPER" data-autocomplete="OPER"
                                                ></asp:TextBox>                                            
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:TextBox ID="txtSearch_UserName" Width="100%" CssClass="inputtext" runat="server"
                                                placeholder="FROM"
                                                onkeypress="return clickButton(event,'ctl00_MainContent_linkSearch');"></asp:TextBox>
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:TextBox ID="txtToAirp" Width="100%" CssClass="inputtext" runat="server"
                                                placeholder="TO"
                                                onkeypress="return clickButton(event,'ctl00_MainContent_linkSearch');"></asp:TextBox>
                                        </td>
                                        <td style="width: 10%;text-align:right;">
                                            <asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind"
                                                Font-Bold="true" OnClick="linkSearch_Click" Text="" Width="40px" Height="38px"></asp:Button>
                                            
                                            
                                        </td>
                                        <td style="width: 10%;">
                                            <button type="button" class="btn btn-sm btn-primary btn-bold" id="btnUpdateVia" onclick="UpdateViaToFinished()">
           Update Via All</button>
                                            </td>
                                        <td style="width: 10%;">                                          
                                             
                                            <asp:LinkButton runat="server" ID="btnAddVia" CssClass="btn btn-sm btn-primary btn-bold"
                                    OnClick="btnAddCountry_Click" OnClientClick="return checkValidCustomMinlenght(formValidate1);" CausesValidation="false">
                                                    <i class="ace-icon fa fa-pencil-square-o bigger-120 white"></i>
                                                    Add New
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
                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                FROM_AIRP
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEditUser" runat="server" CssClass="linkGridForm" Text='<%# DataBinder.Eval(Container.DataItem, "FROM_AIRP") %>'
                                    ToolTip="Edit Country" Enabled='<%#_Role.R_Edit %>' CommandName="Edit"
                                    CommandArgument='<%# Eval("ID") %>'></asp:LinkButton>

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                TO_AIRP
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEditUser2" runat="server" CssClass="linkGridForm" Text='<%# DataBinder.Eval(Container.DataItem, "TO_AIRP") %>'
                                    ToolTip="Edit Country" Enabled='<%#_Role.R_Edit %>' CommandName="Edit"
                                    CommandArgument='<%# Eval("ID") %>'></asp:LinkButton>

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="50%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="50%"></ItemStyle>
                            <HeaderTemplate>
                                VIA
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval("VIA")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                TYPE
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval("PERMTYPE")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                DELETE / UPDATE
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" Width="25px" runat="server" ImageUrl="~/images/delete.png"
                                    ImageAlign="AbsMiddle" OnClientClick="return confirm('Do you want delete?');" ToolTip="Delete" CommandName="DeleteByID"
                                    CommandArgument='<%# Eval("ID") %>' BorderStyle="None" Enabled='<%#_Role.R_Del %>'></asp:ImageButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                
                                <a data-toggle="tooltip" title="Update Via to Finished Flight !" onclick="UpdateViaFin(<%# Eval("ID") %>);"><i class="glyphicon glyphicon-arrow-right"></i></a>
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
                <cc1:CustomPaging ID="CustomPaging1" runat="server" NumberViewPage="5" PageSize="10" />
            </td>
        </tr>
    </table>
   
    <script src="../../Style/assets/js/jquery-2.1.4.min.js"></script>
    <script src="../../Style/assets/js/bootstrap.min.js"></script>
    <script src="../../Scripts/CustomDynamic.js"></script>
    <script src="../../Scripts/CustumStaticdata.js"></script>
    <script src="../../Scripts/CustomPaging.js"></script>
    <link href="../../Scripts/DateTimeJQ/jquery.datetimepicker.css" rel="stylesheet" />
    <script src="../../Scripts/DateTimeJQ/jquery.datetimepicker.js"></script>
    <script type="text/javascript">

        //var txtDate = document.getElementById('<%= txtBEGINDATE.ClientID %>');
        //var txtTDate = document.getElementById('<%= txtTODATE.ClientID %>');
 var txtDate = document.getElementById('ctl00_MainContent_txtBEGINDATE');
var txtTDate = document.getElementById('ctl00_MainContent_txtTODATE');

        var txtOper = document.getElementById('ctl00_MainContent_ddlType');
        var txtOperOne = document.getElementById('ctl00_MainContent_txtOper');

        function UpdateViaFin(id) {
            if ((txtDate.value != '') && (txtTDate.Value!='') && (txtOper.value != '0')) {
                UpdateViaTo(id, txtOper.value, txtDate.value, txtTDate.value);
            }
            else if ((txtDate.value != '') && (txtTDate.Value != '') && (txtOperOne.value != '')) {
                UpdateViaTo(id, txtOperOne.value, txtDate.value,txtTDate.value);
            }
            else {
                alert('Date and Oper not null !');
            }
        }
        function ddlType_Change() {
            if (txtOper.value != "0") {                
                document.getElementById('ctl00_MainContent_txtOper').disabled = 'true';
                document.getElementById('ctl00_MainContent_txtOper').value = '';                             
            } else {
                document.getElementById('ctl00_MainContent_txtOper').disabled = '';
                document.getElementById('ctl00_MainContent_txtOper').value = '';
            }
        }

        function UpdateViaToFinished() {
          
            var startDate = Date.parse(txtDate.value);

            var toDate = Date.parse(txtTDate.value);

            if ((txtDate.value != '') && (txtTDate.value != '')) {
                
                 UpdateViaToPreDate(txtDate.value, txtTDate.value);
                //if (startDate < toDate) {
                 //   UpdateViaToPreDate(txtDate.value, txtTDate.value);
                //}
                //else {
                 //   alert('From Date must smaller To Date !');
                //}
            }
            else {
                alert('From Date and To Date not null !');
            }

            
        }


        function UpdateViaToPreDate(date1, date2) {
            var cf = confirm('Update Via from ' + date1 + ' to ' + date2 + ' ?');
            if (cf) {
                var url = urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=A_TEST_SEARCH&storeName=UpdateVIA_ByDateTime";

                $.ajax({
                    async: false,
                    method: "PUT",
                    url: url,
                    data: JSON.stringify({ P_FDATE: new Date(date1.replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1')).format('yyyy-mm-dd'), P_DATE: new Date(date2.replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1')).format('yyyy-mm-dd'), P_USER: '<%= _user.UserName%>' }),
                    complete: function () {
                        unLoadingData('loadingAccess');
                    },
                    beforeSend: function () {
                        preloadImgAfterButton('btnUpdateVia', 'loadingAccess');
                    }
                }).always(function (data) {
                    alert(data.Value == -1 ? 'Error!' : 'Sussess!');
                })
            }

        }

        function UpdateViaTo(id, oper, date1, date2) {
            var cf = confirm('Do you want update Via from : ' + date1 + " to " + date2);
            if (cf) {
                var url = urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=A_TEST_SEARCH&storeName=UpdateViaToFinished";

                $.ajax({
                    async: false,
                    method: "PUT",
                    url: url,
                    data: JSON.stringify({ P_ID: id, P_OPER: oper, P_FDATE: date1, P_DATE:date2, P_USER: '<%= _user.UserName%>' }),
                }).always(function (data) {
                    alert(data.Value == -1 ? 'Error!' : 'Sussess!');
                })
            }

        }
        $('#<%= txtBEGINDATE.ClientID %>').datetimepicker({
            timepicker: false,
            formatTime: '',
            format: 'd-m-Y',
            formatDate: 'd-m-Y'
        });
        $('#<%= txtTODATE.ClientID %>').datetimepicker({
            timepicker: false,
            formatTime: '',
            format: 'd-m-Y',
            formatDate: 'd-m-Y'
        });
    </script>
</asp:Content>
