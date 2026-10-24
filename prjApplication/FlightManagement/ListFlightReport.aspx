<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="ListFlightReport.aspx.cs" Inherits="prjApplication.FlightManagement.ListFlightReport" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/bootstrap-datepicker3.min.css" rel="stylesheet" />
    <style>
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
    <span class="TitlePanel">+ SCHEDULED FLIGHT PLAN IN SEASON</span>
    <div class="well well-sm">
        <asp:TextBox ID="txtSearch_UserName" Width="80%" CssClass="inputtext" runat="server"
            placeholder="Search ..."
            onkeypress="return clickButton(event,'ctl00_MainContent_linkSearch');"></asp:TextBox>
        <asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind"
            Font-Bold="true" OnClick="linkSearch_Click" Text="" Width="40px" Height="38px">
        </asp:Button>
        <button type="button" onclick="CreateFlight('popupEditFlight')" id="btnCreateNew"
            class="btn btn-sm btn-primary">
            <i class="glyphicon glyphicon-plus"></i>
            Create
        </button>

        <asp:Literal ID="lit" runat="server"></asp:Literal>
    </div>
    <div class="table table-responsive">
        <asp:DataGrid runat="server" ID="grdSource" AutoGenerateColumns="false" DataKeyField="ID"
            OnEditCommand="grdSource_EditCommand" OnDeleteCommand="grdSource_DeleteCommand"
            Width="100%" OnItemCommand="grdSource_ItemCommand"
            CssClass="table table-striped table-bordered table-hover">
            <ItemStyle CssClass="GridItem"></ItemStyle>
            <AlternatingItemStyle CssClass="GridAltItem" />
            <HeaderStyle CssClass="GridHeader"></HeaderStyle>
            <Columns>
                <asp:BoundColumn Visible="False" DataField="ID">
                    <HeaderStyle Width="1%"></HeaderStyle>
                </asp:BoundColumn>                

                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                    <HeaderTemplate>
                        PERMISSION
                    </HeaderTemplate>
                    <ItemTemplate>
                        
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                    <HeaderTemplate>
                        CRAFT
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "BEGIN_DATE") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                    <HeaderTemplate>
                        CALLSIGN
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "FINISH_DATE") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                    <HeaderTemplate>
                        FROM TO
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "SEASON") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                    <HeaderTemplate>
                        ETD
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "YEAR") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                    <HeaderTemplate>
                        ROUTE
                    </HeaderTemplate>
                    <ItemTemplate>
                       
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                    <HeaderTemplate>
                        DAYS FLY
                    </HeaderTemplate>
                    <ItemTemplate>
                       
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                    <HeaderTemplate>
                        D1
                    </HeaderTemplate>
                    <ItemTemplate>
                       
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                    <HeaderTemplate>
                        D2
                    </HeaderTemplate>
                    <ItemTemplate>
                       
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                    <HeaderTemplate>
                        D3
                    </HeaderTemplate>
                    <ItemTemplate>
                       
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                    <HeaderTemplate>
                        D4
                    </HeaderTemplate>
                    <ItemTemplate>
                       
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                    <HeaderTemplate>
                        D5
                    </HeaderTemplate>
                    <ItemTemplate>
                       
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                    <HeaderTemplate>
                        D6
                    </HeaderTemplate>
                    <ItemTemplate>
                       
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                    <HeaderTemplate>
                        D7
                    </HeaderTemplate>
                    <ItemTemplate>
                       
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </div>
    <div style="text-align: right" class="pageNavTotal">
        <cc1:CustomPaging ID="CustomPaging1" runat="server" NumberViewPage="5" PageSize="10" />
    </div>
    <div id="popupEditFlight" class="modal cssHide" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="ClosePopup('popupEditFlight');" class="close" data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">Edit flight</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-horizontal">
                            <div class="form-group" style="display:none">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_ID">CALENDAR_ID</label>
                                <input id="txtCALENDAR_ID" data-minlenght="1"  value="1" data-number="true" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_NAME">CALENDAR_NAME</label>
                                <input id="txtCALENDAR_NAME" data-minlenght="1" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group" style="display:none">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">LASTUSER</label>
                                <input id="txtLASTUSER" readonly type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group" style="display:none">
                                <label class="col-lg-4 control-label" for="txtUSER_NAME">USER_NAME</label>
                                <input id="txtUSER_NAME" readonly type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group" style="display:none">
                                <label class="col-lg-4 control-label" for="txtMAKING_DATE">MAKING_DATE</label>
                                <input id="txtMAKING_DATE" readonly type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtBEGIN_DATE">BEGIN_DATE</label>
                                <input id="txtBEGIN_DATE" type="text" data-date-format="dd/mm/yyyy" class="col-xs-10 col-sm-5 col-lg-5 date-picker" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtFINISH_DATE">FINISH_DATE</label>
                                <input id="txtFINISH_DATE" type="text" data-date-format="dd/mm/yyyy" class="col-xs-10 col-sm-5 col-lg-5 date-picker" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="ddlSEASON">SEASON</label>
                                <div class="col-xs-10 col-sm-6 col-lg-6">
                                    <select id="ddlSEASON" class="form-control row wid_90">
                                        <option value="S">SUMMER</option>
                                        <option value="W">WINTER</option>
                                    </select>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtYEAR">YEAR</label>
                                <input id="txtYEAR" type="text" data-number="true" data-minlenght="1" maxlength="4"
                                    class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtTYPE">TYPE</label>
                                <div class="col-xs-10 col-sm-6 col-lg-6">
                                    <select id="ddlTypeFlight" class="form-control row wid_90">
                                        <option value="LD">Landing</option>
                                        <option value="O/F">Over Flight</option>
                                    </select>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="row" style="text-align: center">
                        <button id="btnUpdate" type="button" style="display: none" onclick="btnUpdateOnclick();"
                            class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-check"></i>
                            Update
                        </button>
                        <button id="btnCreate" type="button" style="display: none" onclick="btnCreateOnclick();"
                            class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-check"></i>
                            Create
                        </button>
                        <button id="btnCancel" type="button" onclick="ClosePopup('popupEditFlight');" runat="server"
                            data-dismiss="modal" class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-times"></i>
                            Cancel
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script>
        //GetArgWithPostBack
        var phanCach = '<%= _phanCach %>';
        var IdSelect = '0';
        var txtCALENDAR_ID = document.getElementById('txtCALENDAR_ID');
        var txtCALENDAR_NAME = document.getElementById("txtCALENDAR_NAME");
        var txtMarkingDate = document.getElementById('txtMAKING_DATE');
        var txtBeginDate = document.getElementById('txtBEGIN_DATE');
        var txtFinistDate = document.getElementById('txtFINISH_DATE');
        var ddlSEASON = document.getElementById('ddlSEASON');
        var ddlTypeFlight = document.getElementById('ddlTypeFlight');
        var txtLASTUSER = document.getElementById('txtLASTUSER');
        var txtUSER_NAME = document.getElementById('txtUSER_NAME');
        var txtYEAR = document.getElementById('txtYEAR');
        var btnCreate = document.getElementById('btnCreate');
        var btnUpdate = document.getElementById('btnUpdate');
        function ClosePopup(elem) {
            IdSelect = '';
            document.getElementById(elem).className = "modal cssHide";
            btnCreate.setAttribute('style', 'display: none');
            btnUpdate.setAttribute('style', 'display: none');
        }
        function ShowPopup(elem) {
            document.getElementById(elem).className = "modal cssShow";
        }
        function ShowpopupEditFlight(id) {
            IdSelect = id;
            GetArgWithPostBack(id + '_____GetOneFlight', 'GetOneFlight');
        }
        function ShowPopup(elem) {
            document.getElementById(elem).className = "modal cssShow";
        }
        function CreateFlight(elem) {
            document.getElementById(elem).className = "modal cssShow";
            btnCreate.removeAttribute('style');
            btnUpdate.setAttribute('style', 'display: none');
        }

        function LoadDataGrid() {
            GetArgWithPostBack('LoadDataGrid_____LoadDataGrid', 'LoadDataGrid');

        }
        function btnUpdateOnclick() {
            if (IdSelect != '0')
                GetArgWithPostBack(JSON.stringify(ReadObj()) + '_____btnUpdateOnclick', 'btnUpdateOnclick');
            else {
                alert('Please select Flight');
                return;
            }
        }
        function btnCreateOnclick() {
            GetArgWithPostBack(JSON.stringify(ReadObj()) + '_____btnCreateOnclick', 'btnCreateOnclick');
        }
        function btnDeleteOnclick(id) {
            var result = confirm("Do you want delete?");
            if (result)
                GetArgWithPostBack(id + '_____btnDeleteOnclick', 'btnDeleteOnclick');
        }
        function ReadObj() {
            var obj = {
                //CALENDAR_ID: txtCALENDAR_ID.value,
                BEGIN_DATE: txtBeginDate.value,
                MAKING_DATE: txtMarkingDate.value,
                FINISH_DATE: txtFinistDate.value,
                SEASON: ddlSEASON.value,
                CALENDAR_NAME: txtCALENDAR_NAME.value,
                TYPE: ddlTypeFlight.value,
                YEAR: txtYEAR.value,
                //CALENDAR_ID: IdSelect,
                //LASTUSER: '',
                //USER_NAME: '',
                ID: IdSelect
            };
            return obj;
        }
        function DisplayResult(resulf, context) {
            if (resulf == '') return;
            if (context == 'btnUpdateOnclick') {
                alert(resulf);
                LoadDataGrid();
            }
            if (context == 'btnCreateOnclick') {
                alert(resulf);
                LoadDataGrid();
            }
            if (context == 'GetOneFlight') {
                ReadInfoFlight(resulf);
                ShowPopup('popupEditFlight');
            }
            if (context == 'LoadDataGrid') {
                document.getElementById('<%= grdSource.ClientID%>').innerHTML = '';
                document.getElementById('<%= grdSource.ClientID%>').innerHTML = resulf;
            }
            if (context == 'btnDeleteOnclick') {
                alert(resulf);
                LoadDataGrid();
            }
        }
        function ReadInfoFlight(data) {
            var obj = JSON.parse(data);
            txtCALENDAR_ID.value = obj['CALENDAR_ID'];
            btnUpdate.removeAttribute('style');
            txtCALENDAR_NAME.value = obj['CALENDAR_NAME'];
            txtBeginDate.value = obj['BEGIN_DATE'] == null ? null : obj['BEGIN_DATE']['DateTime'];
            txtFinistDate.value = obj['FINISH_DATE'] == null ? null : obj['FINISH_DATE']['DateTime'];
            txtMarkingDate.value = obj['MAKING_DATE'] == null ? null : obj['MAKING_DATE']['DateTime'];
            setSelectedValue(ddlSEASON.id, obj['SEASON']);
            setSelectedValue(ddlTypeFlight.id, obj['TYPE']);
            txtLASTUSER.value = obj['LASTUSER'];
            txtUSER_NAME.value = obj['USER_NAME'];
            txtYEAR.value = obj['YEAR'];
        }

        function setSelectedValue(idSelected, valueToSet) {
            var selectObj = document.getElementById(idSelected);
            for (var i = 0; i < selectObj.options.length; i++) {
                if (selectObj.options[i].value == valueToSet) {
                    selectObj.options[i].selected = true;
                    return;
                }
            }
        }

    </script>





</asp:Content>

