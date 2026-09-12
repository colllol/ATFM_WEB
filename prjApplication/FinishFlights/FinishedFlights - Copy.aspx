<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true"
    CodeBehind="FinishedFlights.aspx.cs" Inherits="prjApplication.FinishedFlights.FinishedFlights"
    EnableEventValidation="false" %>

<%@ Import Namespace="prjBusinessLogic" %>
<%@ Import Namespace="prjInfo" %>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--<link href="../Style/assets/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/bootstrap-datepicker3.min.css" rel="stylesheet" />--%>
    <link href="../Style/assets/css/autocomplete.css" rel="stylesheet" />
    <link href="../Style/Style_List.css" rel="stylesheet" />
    <script src="../Scripts/CustomDynamic.js"></script>
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
        .xdsoft_autocomplete_dropdown{min-width: 130px!important}
        .sControl {
            width: 50px;
            height: 23px;
            color: #777777;
        }

        .sInput {
            border-width: 0px !important;
            width: 100%;
        }

        #tblSource tr td:first-child + td {
            white-space: nowrap;
        }

            #tblSource tr td:first-child + td + td {
                white-space: nowrap;
            }
    </style>

    <span class="TitlePanel">+ FINISHED FILGHTS</span>
    <div class="well well-sm">

        <div class="classSearchHeader">
            <table border="0" cellpadding="1" cellspacing="1" style="width: 100%">
                <tr>
                    <td class="Titlelbl" style="text-align: right">START DATE:
                    </td>
                    <td style="text-align: left; width: 14%;">
                        <input id="txtStartDate" runat="server" data-date-format="dd/mm/yyyy"
                            class="inputtext date-picker"
                            type="text" style="width: 70%;" />
                    </td>
                    <td class="Titlelbl" style="text-align: right">END DATE:
                    </td>
                    <td style="text-align: left; width: 14%;">
                        <input id="txtFinishDate" runat="server" data-date-format="dd/mm/yyyy"
                            class="inputtext date-picker"
                            type="text" style="width: 70%;" />
                    </td>

                    <td style="text-align: left;">
                        
                    </td>
                    <td align="left">

                        <select id="ddlSelect" runat="server" style="width: 80%;">
                            <option value="0">--</option>
                            <option value="1">Nối chuyến</option>
                            <option value="2">Bay chậm</option>
                            <option value="3">Bay chuyển sân</option>
                            <option value="4">Bay khác đường bay</option>
                            <option value="5">Trùng CallSigh</option>
                            <option value="6">Gần giống CallSign</option>
                            <option value="7">Bay quốc tế</option>
                            <option value="8">Bay nội địa</option>
                            <option value="9">Hãng quốc nội</option>
                            <option value="10">Hãng quốc tế</option>
                            <option value="11">Hãng quốc nội - Bay quốc tế</option>
                            <option value="12">Hãng quốc nội - Bay nội địa</option>
                            <option value="13">Hãng quốc tế - Bay quốc tế</option>
                            <option value="14">Hãng quốc tế - Bay nội địa</option>
                        </select>
                    </td>
                    <td class="Titlelbl" style="text-align: right">
                        <asp:Literal ID="lit" runat="server"></asp:Literal>
                    </td>
                    <td align="left">

                        <div style="text-align: right; margin-right: 20px;">
                            <asp:LinkButton ID="lnkDelete" OnClick="lnkShowDelete_Click" Visible="false" data-Value="1"
                                runat="server">
               
                                 <span class="glyphicon glyphicon-th-list"></span>
                                                    LIST
                            </asp:LinkButton>

                            <asp:LinkButton ID="LinkHideDelete" OnClick="lnkShowDelete_Click" data-Value="-1"
                                runat="server">
                                <span class="glyphicon glyphicon-trash"></span>
                                                    LIST DELETE
                
                            </asp:LinkButton>

                        </div>


                    </td>

                </tr>
            </table>
        </div>

    </div>

    <button id="btnUpdateCustum" runat="server" type="button" class="btn btn-sm btn-primary" onclick="btnUpdateCustum_Onclick()">
        UpdateAll</button>
    <asp:LinkButton runat="server" ID="btnAddNew" CssClass="btn btn-sm btn-primary"
                            OnClick="btnCreate_Click">
                                                    <span class="glyphicon glyphicon-plus"></span>
                                                    Create
                        </asp:LinkButton>
    <div class="table-responsive">
        <table id="tblSource" class="table table-bordered table-responsive">
            <thead>
                <tr>
                    <th style="background-color: white;">
                        <asp:Button ID="btnSearch" runat="server" Width="40px" Height="38px" CssClass="iconFind"
                            OnClick="btnSearch_Click" /></th>
                    <th></th>
                    <th>
                        <input type="text" class="sControl" id="txtPERMNBR" runat="server" /></th>
                    <th>
                        <asp:DropDownList CssClass="sControl" runat="server" ID="ddlOPER"></asp:DropDownList>
                    </th>
                    <th>
                        <input type="text" class="sControl" id="txtFLIGHTNBR" runat="server" /></th>
                    <th>
                        <input type="text" class="sControl" id="txtREGISTRATION" runat="server" /></th>
                    <th>
                        <asp:DropDownList CssClass="sControl" ID="ddlCRAFT_TYPE" runat="server"></asp:DropDownList>
                    </th>
                    <th>
                        <asp:DropDownList CssClass="sControl" ID="ddlREAL_CRAFT_TYPE" runat="server"></asp:DropDownList>
                    </th>
                    <th>
                        <asp:DropDownList CssClass="sControl" ID="ddlFROM_AIRP" runat="server"></asp:DropDownList>
                    </th>
                    <th>
                        <asp:DropDownList CssClass="sControl" ID="ddlTO_AIRP" runat="server"></asp:DropDownList>
                    </th>
                    <th>
                        <input type="text" class="sControl" id="txtFlightDate" runat="server" /></th>
                    <th>
                        <input type="text" class="sControl" id="txtETD" runat="server" /></th>
                    <th>
                        <input type="text" class="sControl" id="txtATD" runat="server" /></th>
                    <th>
                        <input type="text" class="sControl" id="txtETA" runat="server" /></th>

                    <th>
                        <input type="text" class="sControl" id="txtATA" runat="server" /></th>
                    <th>
                        <asp:DropDownList CssClass="sControl" ID="ddlPURPOSE" runat="server"></asp:DropDownList>
                    </th>
                    <th>
                        <input type="text" class="sControl" id="txtVIA" runat="server" /></th>
                    <th>
                        <input type="text" class="sControl" id="txtFPL_VIA" runat="server" /></th>
                    <th>
                        <input type="text" class="sControl" id="txtREMARK" runat="server" /></th>
                    <th>
                        <input type="text" class="sControl" id="txtValidateHour" maxlength="2" data-number="true"
                            runat="server" /></th>
                    <th>
                        <select id="ddlFLIGHT_TYPE" runat="server" class="sControl">
                            <option value="">--</option>
                            <option value="SC">SC</option>
                            <option value="NO">NO</option>
                        </select></th>

                    <th>
                        <select id="ddlPERMTYPE" cssclass="sControl" class="sControl" runat="server">
                            <option value="">--</option>
                            <option value="LD">LD</option>
                            <option value="O/F">O/F</option>
                        </select></th>
                </tr>
                <tr>
                    <th>STT</th>
                    <th>Edit</th>
                    <th>PERMNBR</th>
                    <th>OPER</th>
                    <th>FLIGHTNBR</th>
                    <th>REGIS</th>
                    <th>CRAFT</th>
                    <th>REAL_CRAFT</th>
                    <th>FROM</th>
                    <th>TO</th>
                    <th>FLIGHTDATE</th>
                    <th>ETD</th>
                    <th>ATD</th>
                    <th>ETA</th>
                    <th>ATA</th>
                    <th>PURPOSE</th>
                    <th>VIA</th>
                    <th>FPL_VIA</th>
                    <th>REMARK</th>
                    <th>VALIDHOURS</th>
                    <th>FLIGHT_TYPE</th>
                    <th>PERM_TYPE</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="grdSource1" runat="server" OnItemDataBound="grdSource1_ItemDataBound">
                    <ItemTemplate>
                        <tr id='<%# Eval("FLIGHT_ID") %>' data-isupdate="false">
                            <td><%# Eval("RNUM") %></td>
                            <td>
                                <div class="action-buttons">
                                    <a data-toggle="tooltip" id="btnEdit"  href="<%# Page.ResolveUrl("~/FinishFlights/EditFinishFlight.aspx") + "?Menu_ID="+ Request.Params["Menu_Id"]+ "&ID=" + Eval("FLIGHT_ID") %>" title="Edit">
                                        <i class="glyphicon glyphicon-pencil"></i>
                                    </a>
                                    <asp:LinkButton runat="server" ID="lnkDelete" data-toggle="tooltip"
                                        title="Delete" CommandArgument='<%# Eval("FLIGHT_ID") %>' OnClick="lnkDelete_Click" visible='<%#_Role.R_Del %>'>
                                        <i class="glyphicon glyphicon-trash"></i>
                                    </asp:LinkButton>
                                    <a data-toggle="tooltip" class="bigger-140 show-details-btn" title="Show history" runat="server" visible='<%# _Role.R_Pub %>'>
                                        <i class="ace-icon fa fa-angle-double-down"></i>
                                    </a>
                                </div>
                            </td>

                            <td style="white-space: nowrap">
                                <input type="text" data-minlenght="5" maxlength="20" data-control="_updateAll" runat="server"
                                    id="txtPermNbr" class="sInput" value='<%# Eval("PERMNBR") %>' /></td>
                            <td>
                                <input type="text" data-control="_updateAll" data-minlenght="1" class="sInput" runat="server"
                                    id="txtOper" data-autocomplete="OPER" value='<%# Eval("OPER_ID") %>' />
                            </td>
                            <td>
                                <input type="text" data-control="_updateAll" data-minlenght="1" maxlength="8" runat="server"
                                    id="txtFlightNbr" class="sInput" value='<%# Eval("FLIGHTNBR") %>' /></td>
                            <td>
                                <input type="text" class="sInput" runat="server" id="txtRegistration" value='<%# Eval("REGISTRATION") %>' />
                            </td>
                            <td>
                                <input type="text" data-control="_updateAll" data-minlenght="1" class="sInput" runat="server"
                                    data-craftid='<%# Eval("CRAFT_ID") %>' id="txtCraftType" data-autocomplete="CRAFT"
                                    value='<%# Eval("CRAFT_TYPE") %>' /></td>
                            <td>
                                <input type="text" class="sInput" runat="server" id="txtREAL_CRAFT_TYPE" data-autocomplete="CRAFT"
                                    value='<%# Eval("REAL_CRAFT_TYPE") %>' /></td>
                            <td>
                                <input type="text" data-control="_updateAll" data-minlenght="1" class="sInput" runat="server"
                                    id="txtFrom" data-autocomplete="AERO" value='<%# Eval("FROM_AIRP") %>' />
                            </td>
                            <td>
                                <input type="text" data-control="_updateAll" data-minlenght="1" class="sInput" runat="server"
                                    id="txtTo" data-autocomplete="AERO" value='<%# Eval("TO_AIRP") %>' /></td>
                            <td>
                                <input type="text" class="sInput" data-control="_updateAll" data-minlenght="1" runat="server"
                                    id="txtFlightDate" onblur="checkInputDate(this)" value='<%# Eval("FLIGHTDATE", "{0:dd/MM/yyyy}") %>' />
                            </td>
                            <td>
                                <input type="text" data-number="true" maxlength="6" class="sInput" runat="server" id="txtEtd" value='<%# Eval("ETD") %>' />
                            </td>
                            <td>
                                <input type="text" data-number="true" maxlength="6" class="sInput" runat="server" id="txtAtd" value='<%# Eval("ATD") %>' />
                            </td>
                            <td>
                                <input type="text" data-number="true" maxlength="6" class="sInput" runat="server" id="txtEta" value='<%# Eval("ETA") %>' />
                            </td>
                            <td>
                                <input type="text" data-number="true" maxlength="6" class="sInput" runat="server" id="txtAta" value='<%# Eval("ATA") %>' />
                            </td>
                            <td>
                                <input type="text" data-control="_updateAll" data-minlenght="1" class="sInput" runat="server"
                                    id="txtPurpose" data-autocomplete="PURPOSE" value='<%# Eval("PURPOSE") %>' />
                            </td>
                            <td>
                                <input type="text" class="sInput" runat="server" id="txtVia" value='<%# Eval("VIA") %>' />
                            </td>
                            <td>
                                <input type="text" class="sInput" runat="server" id="txtFplVia" value='<%# Eval("FPL_VIA") %>' />
                            </td>
                            <td>
                                <input type="text" class="sInput" runat="server" id="txtRemark" value='<%# Eval("REMARK") %>' />
                            </td>
                            <td>
                                <input type="text" data-number="true" data-control="_updateAll" data-minlenght="1"
                                    class="sInput" runat="server" id="txtValidhorus" value='<%# Eval("VALIDHOURS") %>' />
                            </td>
                            <td>
                                <input type="text" data-control="_updateAll" data-minlenght="1" class="sInput" runat="server"
                                    id="txtFlightType" data-autocomplete="FLIGHTTYPE" value='<%# Eval("FLIGHT_TYPE") %>' />
                            </td>
                            <td>
                                <input type="text" data-control="_updateAll" data-minlenght="1" class="sInput" runat="server"
                                    id="txtPermType" data-autocomplete="PERMTYPE" value='<%# Eval("PERMTYPE") %>' />
                            </td>
                        </tr>
                        <%--<tr class="detail-row">
                            <td colspan="22" class="text-left">
                                <%# rListHistoryFlightDetails(new FinishedFlightsDAL().GetHisById(Eval("FLIGHT_ID").ToString()), Eval("FLIGHT_ID").ToString()) %>
                            </td>
                        </tr>  --%>
                        <asp:Label ID="lblId" runat="server" Text='<%# Eval("FLIGHT_ID") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblHis" runat="server" Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    <div style="text-align: right">
        <cc1:PhanTrang ID="PhanTrang1" runat="server" OnPaging_IndexChange="PhanTrang1_Paging_IndexChange"
            NumberViewPage="7" PageSize="10" />
    </div>
    <%--<div id="dhtmltooltip"></div>--%>
    <script src="../Style/assets/js/jquery-1.11.3.min.js"></script>
    <script src="../Style/assets/js/bootstrap.min.js"></script>
    <script src="../Style/assets/js/ace.min.js"></script>
    <script src="../Style/assets/js/ace.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/jquery.autocomplete.js"></script>


    <script>
        var phanCach = '<%= _phanCach%>';
        function LoadDataGrid() {
            GetArgWithPostBack('LoadDataGrid_____LoadDataGrid', 'LoadDataGrid');
        }
        function DisplayResult(resulf, context) {
            if (context == 'LoadDataGrid') {
                $('#tblSource tbody tr').remove();
                $('#tblSource tbody').append(resulf);
            }
            if (context == 'GetContent') {
                document.getElementById('vContent').innerHTML = resulf;
            }
            if (context == 'btnDelete_Onclick') {
                alert(resulf);
            }
            if (context == 'RestoreHistory') {
                if (resulf != 'NOK') {
                    alert('Restore sussess!');
                    LoadDataGrid();
                } else alert('Restore error!');
            }
            if (context == 'UpdateListObject') {
                alert(resulf);
            }
        }
        function RestoreHistory(id, ver, UserID) {
            GetArgWithPostBack(id + phanCach + ver + phanCach + UserID + '_____RestoreHistory', 'RestoreHistory');
        }
        function LoadDataGrid() {
            GetArgWithPostBack('LoadDataGrid_____LoadDataGrid', 'LoadDataGrid');
        }
        function GetContent(id) {
            GetArgWithPostBack(id + '_____GetContent', 'GetContent');
        }
        function btnDelete_Onclick(id) {
            GetArgWithPostBack(id + '_____btnDelete_Onclick', 'btnDelete_Onclick');
        }
        $('.show-details-btn').on('click', function (e) {
            e.preventDefault();
            $(this).closest('tr').next().toggleClass('open');
            $(this).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
        });
    </script>
    <script>
        var listOper = '<%= _ListOper%>'.split(',');
        var listCraft = JSON.parse('<%= _ListCraft%>');
        var listPurpose = '<%= _ListPurpose%>'.split(',');
        var listAero = '<%= _ListAero%>'.split(',');
        var listPermType = ['LD', 'O/F'];
        var listFlightType = ['SC', 'NO'];
        function CheckListSanBay(ele) {
            if (listAero.indexOf($(ele).val().toUpperCase()) == -1) {
                $(ele).val('');
                $(ele).css('border-color: red;');
            } else {
                $(ele).css('border-color: \'\'');
                $(ele).val($(ele).val().toUpperCase());
            }
        }
        function CheckListOper(ele) {
            if (listOper.indexOf($(ele).val().toUpperCase()) == -1) {
                $(ele).val('');
                $(ele).css('border-color: red;');
            } else {
                $(ele).css('border-color: \'\'');
                $(ele).val($(ele).val().toUpperCase());
            }
        }
        function CheckListAero(ele) {
            if (listAero.indexOf($(ele).val().toUpperCase()) == -1) {
                $(ele).val('');
                $(ele).css('border-color: red;');
            } else {
                $(ele).css('border-color: \'\'');
                $(ele).val($(ele).val().toUpperCase());
            }
        }
        function CheckPermType(ele) {
            if (listPermType.indexOf($(ele).val().toUpperCase()) == -1) {
                $(ele).val('');
                $(ele).css('border-color: red;');
            } else {
                $(ele).css('border-color: \'\'');
                $(ele).val($(ele).val().toUpperCase());
            }
        }
        function CheckFlightType(ele) {
            if (listFlightType.indexOf($(ele).val().toUpperCase()) == -1) {
                $(ele).val('');
                $(ele).css('border-color: red;');
            } else {
                $(ele).css('border-color: \'\'');
                $(ele).val($(ele).val().toUpperCase());
            }
        }
        function CheckCraft(ele) {
            if (listCraft.findIndex(item=>item.name == $(ele).val().toUpperCase()) == -1) {
                $(ele).val('');
                $(ele).css('border-color: red;');
            } else {
                $(ele).css('border-color: \'\'');
                $(ele).val($(ele).val().toUpperCase());
                $(ele).attr('data-craftid', listCraft[listCraft.findIndex(item=>axx = (item.name == $(ele).val().toUpperCase()))]['craftid']);
            }
        }
        function CheckPurpose(ele) {
            if (listPurpose.indexOf($(ele).val().toUpperCase()) == -1) {
                $(ele).val('');
                $(ele).css('border-color: red;');
            } else {
                $(ele).css('border-color: \'\'');
                $(ele).val($(ele).val().toUpperCase());
            }
        }

        $('[data-AutoComplete="OPER"]').each(function () {
            var $ele = $(this);
            $('#' + $ele.prop('id')).autocomplete({
                source: [listOper],
            }).on('blur', function (e, datum) {
                CheckListOper($('#' + $ele.prop('id')));
            });
        });
        $('[data-AutoComplete="PERMTYPE"]').each(function () {
            var $ele = $(this);
            $('#' + $ele.prop('id')).autocomplete({
                source: [listPermType],
            }).on('blur', function (e, datum) {
                CheckPermType($('#' + $ele.prop('id')));
            });
        });
        $('[data-AutoComplete="FLIGHTTYPE"]').each(function () {
            var $ele = $(this);
            $('#' + $ele.prop('id')).autocomplete({
                source: [listFlightType],
            }).on('blur', function (e, datum) {
                CheckFlightType($('#' + $ele.prop('id')));
            });
        });
        $('[data-AutoComplete="PURPOSE"]').each(function () {
            var $ele = $(this);
            $('#' + $ele.prop('id')).autocomplete({
                source: [listPurpose],
            }).on('blur', function (e, datum) {
                CheckPurpose($('#' + $ele.prop('id')));
            });
        });
        $('[data-AutoComplete="AERO"]').each(function () {
            var $ele = $(this);
            $('#' + $ele.prop('id')).autocomplete({
                source: [listAero],
            }).on('blur', function (e, datum) {
                CheckListAero($('#' + $ele.prop('id')));
            });
        });
        $('[data-AutoComplete="CRAFT"]').each(function () {
            var $ele = $(this);
            $('#' + $ele.prop('id')).autocomplete({
                titleKey: 'name',
                valueKey: 'name',
                source: [{
                    data: listCraft,
                }],
            }).on('selected.xdsoft', function (e, data) {
                $('#' + $ele.prop('id')).val(data.name);
            }).on('blur', function (e, datum) {
                CheckCraft($('#' + $ele.prop('id')));
            });
        });
    </script>
    <script>
        var isUpdate = false;
        var oldVal = '';
        var newVal = '';
        $('#tblSource>tbody input').focusin(function () {
            oldVal = $(this).val().toUpperCase();
        }).focusout(function () {
            newVal = $(this).val().toUpperCase();
            if (oldVal != newVal) {
                $(this).closest('tr').attr('data-isUpdate', 'true')
            }
        });
        function btnUpdateCustum_Onclick() {
            if (checkValidCustomMinlenght('_updateAll')) {
                var lis = [];
                $('[data-isUpdate="true"]').each(function () {
                    var objUpdate = {
                        FLIGHT_ID: $(this).prop('id'),
                        ATA: $($(this).find('[id^="txtAta"]')[0]).val(),
                        ATD: $($(this).find('[id^="txtAtd"]')[0]).val(),
                        ETA: $($(this).find('[id^="txtEta"]')[0]).val(),
                        ETD: $($(this).find('[id^="txtEtd"]')[0]).val(),
                        FLIGHTDATE: $($(this).find('[id^="txtFlightDate"]')[0]).val(),
                        FLIGHTNBR: $($(this).find('[id^="txtFlightNbr"]')[0]).val(),
                        FPL_VIA: $($(this).find('[id^="txtFplVia"]')[0]).val(),
                        PERMNBR: $($(this).find('[id^="txtPermNbr"]')[0]).val(),
                        REGISTRATION: $($(this).find('[id^="txtRegistration"]')[0]).val(),
                        REMARK: $($(this).find('[id^="txtRemark"]')[0]).val(),
                        VALIDHOURS: $($(this).find('[id^="txtValidhorus"]')[0]).val(),
                        VIA: $($(this).find('[id^="txtVia"]')[0]).val(),
                        CRAFT_ID: $($(this).find('[data-craftid]')[0]).attr('data-craftid'),
                        FLIGHT_TYPE: $($(this).find('[id^="txtFlightType"]')[0]).val(),
                        FROM_AIRP: $($(this).find('[id^="txtFrom"]')[0]).val(),
                        OPER_ID: $($(this).find('[id^="txtOper"]')[0]).val(),
                        PERMTYPE: $($(this).find('[id^="txtPermType"]')[0]).val(),
                        PURPOSE: $($(this).find('[id^="txtPurpose"]')[0]).val(),
                        REAL_CRAFT_TYPE: $($(this).find('[id^="txtREAL_CRAFT_TYPE"]')[0]).val(),
                        TO_AIRP: $($(this).find('[id^="txtTo"]')[0]).val(),
                        LASTUSER: '<%= _user.UserName%>',
                    }
                    lis.push(objUpdate);                    
                });
                GetArgWithPostBack(JSON.stringify(lis) + '_____UpdateListObject', 'UpdateListObject');
        }
    }
    </script>
</asp:Content>
