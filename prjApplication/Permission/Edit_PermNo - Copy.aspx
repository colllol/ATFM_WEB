<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true"
    CodeBehind="Edit_PermNo.aspx.cs" Inherits="prjApplication.Permission.Edit_PermNo" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<%@ Register Assembly="prjComponents" Namespace="prjComponents" TagPrefix="cc1" %>
<%@ Import Namespace="prjBusinessLogic" %>
<%@ Import Namespace="prjInfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Style/Style_List.css" rel="stylesheet" />
    <link href="../Style/assets/css/autocomplete.css" rel="stylesheet" />
    <style>
        input {
            text-transform: uppercase;
        }

        textarea {
            text-transform: uppercase;
        }

        #tblSource input, select {
            color: black;
        }

        .sInput {
            /*border-width: 0px !important;*/
            border: 0px;
            width: 100%;
        }

        input[type=text] {
            border: 0px;
        }

        .success {
            background-color: aquamarine;
        }

        .rowCreate {
            background-color: mediumvioletred;
        }

        .cssHetHan {
            color: red;
        }

            .cssHetHan input {
                color: red !important;
            }
    </style>


    <div id="perm_id" hidden="hidden">
        <%= Request.QueryString["ID"]==null?"": Request.QueryString["ID"]%>
    </div>
    <div id="divExcuteScript" hidden="hidden"></div>





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

    <div id="divLoad" style="display: none">a</div>

    <div id="divPermMaster">

        <label for="txtPERMNBR_ID" style="display: none;">PERMNBR</label>
        <input id="txtPERMNBR_ID" style="display: none;" readonly type="text" class="wid_120px" />
        <table style="width: 100%; border: 0px solid black;">
            <tr>
                <td style="width: 50%">
                    <fieldset class="box-border">
                        <legend class="box-border">Fly Permission Author</legend>
                        <asp:DropDownList ID="ddlAUTHOR_ID" runat="server" CssClass="wid_565px">
                        </asp:DropDownList>
                    </fieldset>
                </td>
                <td style="width: 50%">
                    <fieldset class="box-border">
                        <legend class="box-border">Operator</legend>
                        <input id="ddlOPER_ID" type="text" class="wid_400px" data-autocomplete="OPER" data-minlenght="1"
                            data-control="update" />
                    </fieldset>
                </td>

            </tr>
            <tr valign="top">
                <td style="width: 50%">
                    <fieldset class="box-border">
                        <legend class="box-border">Reference</legend>
                        <table style="width: 100%; border: 0px solid black;">
                            <tr>
                                <td>
                                    <label for="txtPERMNBR" style="font-size: 12px;">Number</label><br />
                                    <input id="txtPERMNBR" maxlength="5" data-minlenght="1" data-control="update" type="text"
                                        class="wid_80px" style="height: 25px;" />
                                    <%--<span style="position: relative; cursor: pointer; color: blue; font-style: italic;"
                                        onclick="checkPermNumber()">check
                                    <i style="position: absolute; top: 0px; left: 45px; width: 250px; display: none;"
                                        id="divListNumber"></i>
                                    </span>--%>
                                </td>
                                <td>
                                    <label for="txtPERMTYPE" style="font-size: 12px;">P Type</label><br />
                                    <select id="ddlPERMTYPE" onchange="ddlPERMTYPE_Change()" class="wid_80px" style="height: 25px;">
                                        <option value="LD">LD</option>
                                        <option value="O/F">O/F</option>
                                    </select>
                                    <span style="position: relative; cursor: pointer; color: blue; font-style: italic;"
                                        onclick="checkPermNumber()">check
                                    <i style="position: absolute; top: 0px; left: 45px; width: 250px; display: none;"
                                        id="divListNumber"></i>
                                    </span>
                                </td>
                                <td><label for="txtFLIGHTTYPE" style="font-size: 12px;">F Type</label><br /> 
                                <select style="height:25px;" id="ddlFLIGHTTYPE" runat="server" class="wid_80px">
                                <option value="NO">NO</option>
                                <option value="SC">SC</option>
                                
                            </select></td>
                                <td>
                                    <label style="display: none" for="txtVERSION" style="font-size: 12px;">Version</label><br />
                                    <input style="height: 25px; display: none" id="txtVERSION" maxlength="1" type="text"
                                        class="wid_80px" />
                                </td>
                                <td>
                                    <label style="display: none" for="txtSEASON">Season</label><br />
                                    <select style="height: 25px; display: none" id="ddlSEASON" class="wid_80px">
                                        <option value="W">Winter</option>
                                        <option value="S">Summer</option>
                                    </select>

                                </td>
                                <td>
                                    <label for="txtPERMDATE" style="font-size: 12px; display: none">Date</label><br />
                                    <input style="height: 25px; display: none" id="txtPERMDATE" onblur="checkInputDate(this);checkYearPermDate(this)"
                                        data-date-format="dd/mm/yyyy"
                                        type="text" class="wid_80px" />
                                </td>
                                <td>
                                    <label for="txtVALIDHOURS" style="font-size: 12px;">Hours</label><br />
                                    <input style="height: 25px; display: block" id="txtVALIDHOURS" maxlength="2" data-number="true"
                                        data-control="update"
                                        data-minlenght="1" type="text"
                                        class="wid_80px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <label for="txtREFERENCE" style="font-size: 12px;">Reference</label><br />
                                    <textarea id="txtREFERENCE" rows="1" class="wid_80" data-control="update" data-toggle="tooltip"
                                        title="Limit 4000 character !"></textarea>
                                    <%--<input id="txtREFERENCE" maxlength="4000" style="height: 25px;" data-control="update"
                                        type="text" class="wid_560px" data-toggle="tooltip" title="Limit 4000 character !" />--%>
                                    <button id="btnShowReference" type="button" class="btn btn-primary" onclick="getMessageFullByRefence()">
                                        Reference</button>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <label for="txtBILLINGADDRESS" style="font-size: 12px;">Billing address</label><br />
                                    <textarea id="txtBILLINGADDRESS" class="wid_100" data-control="update" rows="3" data-toggle="tooltip"
                                        title="Limit 4000 character !"></textarea>
                                    <%--<input id="txtBILLINGADDRESS" maxlength="4000" style="height: 25px;" data-control="update"
                                        type="text" class="wid_600px" data-toggle="tooltip" title="Limit 4000 character !" />--%>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
                <td style="width: 50%">
                    <fieldset class="box-border">
                        <legend class="box-border">Other</legend>
                        <table style="width: 100%; border: 0px solid black;">

                            <tr>
                                <td>
                                    <label for="txtUPLOAD" style="font-size: 12px;">File upload</label>
                                    <br />
                                    <%-- <input id="txtPERMCONTENT" maxlength="4000" style="height: 25px; display: none" data-control="update"
                                        type="text" class="wid_600px" data-toggle="tooltip" title="Limit 4000 character !" />--%>
                                    <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" OnClientUploadComplete="uploadComplete"
                                        OnClientUploadStarted="prelodingUploadFile"
                                        OnUploadedComplete="AsyncFileUpload1_UploadedComplete" ClientIDMode="AutoID" />
                                    <label id="lblLinkFile" style="font-weight: 100"></label>
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <label for="txtCONTENT" style="font-size: 12px;">Perm Content</label><br />
                                    <textarea id="txtPERMCONTENT" class="wid_100" data-control="update" rows="5" data-toggle="tooltip"
                                        ></textarea>
                                </td>
                            </tr>




                        </table>


                    </fieldset>
                </td>
            </tr>
        </table>







    </div>
    <div class="row" style="text-align: center">
        <button id="btnUpdatePermMaster" class="btn btn-sm btn-primary" type="button" onclick="btnUpdateOnclick()">
            Update</button>
        
    </div>
    <hr />

    <%--<div class="table-responsive">--%>
    <button type="button" id="btnUpdateList" class="btn btn-sm btn-primary" onclick="btnUpdateList_Onclick()">
        Update all</button> 
    <button type="button" id="btnGenMess" class="btn btn-sm btn-primary"  onclick="btnGenMessage()">Export Mess Cancel</button>  
    <a  href="<%= Page.ResolveUrl("~/Permission/ListPermissionNo.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString()) %>"
            class="btn btn-sm btn-primary">
            <span class="ace-icon fa fa-times"></span>
            Exit
        </a>
    <table id="tblSource">
        <thead>
            <tr>
                <th data-sort="1" onclick="sortOnclick(this);">Call sign</th>
                <th data-sort="1" onclick="sortOnclick(this);">Registration</th>
                <th data-sort="1" onclick="sortOnclick(this);">From</th>
                <th data-sort="1" onclick="sortOnclick(this);">To</th>
                <th data-sort="1" onclick="sortOnclick(this);">Etd</th>
                <th data-sort="1" onclick="sortOnclick(this);">Eta</th>
                <th data-sort="1" onclick="sortOnclick(this);">Day flight</th>
                <th data-sort="1" onclick="sortOnclick(this);">Craft</th>
                <th data-sort="1" onclick="sortOnclick(this);">Purpose</th>
                <th data-sort="1" onclick="sortOnclick(this);">Via</th>
                <th data-sort="1" onclick="sortOnclick(this);">Remark</th>
                <%--<th>Remark send</th>--%>
                <th>Mtow</th>
                <th>LastModify</th>               
                <th></th>
            </tr>
            <tr>
                <th>
                    <input id="txtFLIGHTNBR" runat="server" type="text" class="wid_80px" data-control="btnUpdate"
                        maxlength="20"
                        data-minlenght="1" /></th>
                <th>
                    <input id="txtREGISTRATION" runat="server" maxlength="20" data-control="btnUpdate"
                        data-minlenght="1"
                        type="text" class="wid_100px" /></th>
                <th>
                    <asp:DropDownList ID="ddlFROM_AIRP" runat="server" CssClass="wid_70px"></asp:DropDownList>
                </th>
                <th>
                    <asp:DropDownList ID="ddlTO_AIRP" runat="server" CssClass="wid_70px"></asp:DropDownList>
                    <th>
                        <input id="txtETD" class="wid_50px" runat="server" type="text" data-control="btnUpdate"
                            data-minlenght="1" maxlength="4" /></th>
                    <th>
                        <input id="txtETA" class="wid_50px" runat="server" type="text" data-control="btnUpdate"
                            data-minlenght="1" maxlength="4" /></th>
                    <th>
                        <input id="txtDAYSFLIGHT" class="wid_200px" data-minlenght="1" onblur="checkInputDate(this)"
                            runat="server" data-control="btnUpdate" type="text" maxlength="200" />
                    </th>

                    <th>
                        <asp:DropDownList ID="ddlCRAFT_ID" runat="server" CssClass="wid_60px"></asp:DropDownList>
                    </th>
                    <th>
                        <asp:DropDownList ID="ddlPURPOSE_ID" runat="server" CssClass="wid_120px"></asp:DropDownList>
                    </th>
                    <th>
                        <input id="txtVIA" runat="server" data-minlenght="1" class="wid_140px" data-control="btnUpdate"
                            maxlength="500"
                            type="text" /></th>
                    <th>
                        <input id="txtREMARK" runat="server" maxlength="100" class="wid_140px" data-control="btnUpdate"
                            data-minlenght="1"
                            type="text" />
                    </th>
                    <%-- <th>
                        <input id="txtREMARK_SEND" runat="server" maxlength="4000" type="text" />
                    </th>--%>
                    <th class="wid_50px"></th>
                    <%--<th>
                        <input id="txtLASTUSER" runat="server" disabled="disabled" class="wid_50px" maxlength="100"
                            type="text" />
                    </th>--%>
                   
                    <th class="wid_15px"></th>
                    <th style="white-space: nowrap; background-color: #fff">
                        <div class="action-buttons">
                            <a id="lnkSearch" onclick="btnSearch_Click()">
                                <i class="glyphicon glyphicon glyphicon-search"></i>
                            </a>
                            <a id="lnkCreate" onclick="mbtnAddNewFlightDetailOnclick()" style="display:none;">
                                <i class="glyphicon glyphicon glyphicon-plus"></i>
                            </a>
                            <a id="lnkUpdate" onclick="mbtnUpdateFlightDetailOnclick()" style="display:none;">
                                <i class="glyphicon glyphicon glyphicon-ok"></i>
                            </a>
                            <a id="lnkCancel" onclick="lnkCancel_Click()" >
                                <i class="glyphicon glyphicon glyphicon-remove"></i>
                            </a>
                            <a id="lnkClear" onclick="lnkClear_Click()" style="display:block;">
                                <i class="glyphicon glyphicon-trash"></i>
                            </a>
                        </div>
                    </th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptSource" runat="server">
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("FLIGHTNBR") %></td>
                        <td><%# Eval("REGISTRATION") %></td>
                        <td><%# Eval("FROM_NAME") %></td>
                        <td><%# Eval("TO_NAME") %></td>
                        <td><%# Eval("ETD") %></td>
                        <td><%# Eval("ETA") %></td>
                        <td>
                            <%# Eval("DAYSFLIGHT") %>
                        </td>
                        <td><%# Eval("CRAFT_NAME") %></td>
                        <td><%# Eval("PURPOSE_ID") %></td>
                        <td><%# Eval("VIA") %></td>
                        <td><%# Eval("REMARK") %></td>
                        <%--<td><%# Eval("REMARK_SEND") %></td>--%>
                        <td><%# Eval("LASTUSER") %></td>
                        
                        <td>
                            <div class="action-buttons" style="width: 70px">
                                <a id="A1" runat="server" data-toggle="tooltip" title="Edit">
                                    <i class="ace-icon fa fa-pencil bigger-130" onclick="mShowDetail(<%# Eval("ID") %>);"></i>
                                </a>
                                <a id="A2" runat="server" data-toggle="tooltip" title="Delete">
                                    <i class="ace-icon fa fa-trash-o bigger-130 hidden" onclick="btnDeleteOnclick(<%# Eval("ID") %>);"></i>
                                </a>
                                <a data-toggle="tooltip" class="bigger-140 show-details-btn" title="Show history">
                                    <i class="ace-icon fa fa-angle-double-down"></i>
                                </a>
                                
                                
                            </div>
                        </td>
                    </tr>
                    <tr class="detail-row">
                        <td colspan="22" class="text-left">
                            <%# rListHistoryFlightDetails(new PermDetailNoDAL().GetHistoryById(Eval("ID").ToString()), Eval("ID").ToString()) %>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>

    <%--</div>--%>

    <script src="../Style/assets/js/jquery-2.1.4.min.js"></script>

    <script src="../Style/assets/js/wizard.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/CustumStaticdata.js"></script>
    <script src="../Scripts/CustomPaging.js"></script>
    <script type="text/javascript">
        jQuery(function ($) {
            var $validation = false;
            $('#fuelux-wizard-container')
            .ace_wizard({
            }).on('stepclick.fu.wizard', function (e) { });
        })
    </script>
    <script>
        var qEdit = '<%= _Role.R_Edit %>';
        var phanCach = '<%= _phanCach%>';
        var IdSelect = $('#perm_id');
        if (IdSelect == 0) {
            $('#btnUpdatePermMaster').hide();
        }
        else {
            if (qEdit != 'True') {
                document.getElementById("btnUpdatePermMaster").disabled = true;
                document.getElementById("btnUpdateList").disabled = true;
            }

        }
        var _objRef;
        var _objRender = JSON.parse('<%= _ObjRender%>');
        var txtPERMNBR_ID = document.getElementById("txtPERMNBR_ID");
        var ddlAUTHOR_ID = document.getElementById('<%= ddlAUTHOR_ID.ClientID %>');
        var ddlPERMTYPE = document.getElementById('ddlPERMTYPE');
        var ddlFLIGHTTYPE = document.getElementById('<%= ddlFLIGHTTYPE.ClientID %>');
        var txtPERMNBR = document.getElementById('txtPERMNBR');
        var txtVERSION = document.getElementById('txtVERSION');
        var txtPERMDATE = document.getElementById('txtPERMDATE');
        var ddlOPER_ID = document.getElementById('ddlOPER_ID');
        var txtREFERENCE = document.getElementById('txtREFERENCE');
        var txtVALIDHOURS = document.getElementById('txtVALIDHOURS');
        var txtBillingAddress = document.getElementById('txtBILLINGADDRESS');
        var txtPermContent = document.getElementById('txtPERMCONTENT');

        function DisplayResult(resulf, context) {
            if (context == 'GetOneFlight') {
                ReadInfoPerm(resulf);
                
            }
            if (context == 'btnUpdateOnclick') {
                alert(resulf);
            }
            if (context == 'btnSearch_Click') {
                $('#tblSource tbody tr').remove();
                $('#tblSource tbody').append(resulf);
                LoadShowHisEventClick();
            }
            if (context == 'LoadDataGrid') {
                $('#tblSource tbody tr').remove();
                $('#tblSource tbody').append(resulf);
                LoadShowHisEventClick();
               
            }
            if (resulf == '') return;
            if (context == 'btnCreateOnclick') {
                if (resulf != '-1' && resulf != '-99') {
                    txtPERMNBR_ID.value = createPermNBRID(ddlAUTHOR_ID.value, ddlPERMTYPE.value, txtPERMNBR.value);
                    document.getElementById('perm_id').innerHTML = resulf;
                    if (isCreatePermMaster) {
                        //mbtnAddNewFlightDetailOnclick();
                        btnUpdateList_Onclick();
                        isCreatePermMaster = false;
                    }

                } else alert('Insert error!');
            }
            if (context == 'mbtnAddNewFlightDetailOnclick') {
                if (resulf != '-1' && resulf != '-99') {
                    alert('Insert sussess!');
                    mClearValueControl();
                    LoadDataGrid();
                } else alert('Insert error!');
            }
            if (context == 'mShowDetail') {
                mReadInfoFlightDetail(resulf);

            }
            if (context == 'mbtnUpdateFlightDetailOnclick') {
                if (resulf != "OK") {
                    alert("Update error!");
                }
                else {
                    alert("Update sussess!");
                    mClearValueControl();
                    LoadDataGrid();
                }
            }
            if (context == 'btnDeleteOnclick') {
                alert(resulf);
                LoadDataAjax()
            }
            if (context == 'RestoreHistory') {
                if (resulf != 'NOK') {
                    alert('Restore sussess!');
                    LoadDataGrid();
                } else alert('Restore error!');
            }
            if (context == 'uploadComplete') {
                if (resulf == 'Upload error!')
                    alert(resulf);
                else
                    $('#lblLinkFile').html(resulf);
            }
            if (context == 'GetListFilePerm') {
                $('#lblLinkFile').html(resulf);
            }
            if (context == 'deleteFile') {
                alert(resulf);
                GetListFilePerm($('#perm_id').html().trim());
            }
        }
        function ReadInfoPerm(data) {
            var obj = JSON.parse(data.replace(/\n/gi, '<br>').replace(/\\/gi, '\\\\').replace(/\t/gi, '     '));
            txtPERMDATE.value = obj['PERMDATE'] == null ? null : obj['PERMDATE']['DateTime'];

            txtPERMNBR_ID.value = obj['PERMNBR_ID'];
            txtREFERENCE.value = obj['REFERENCE'] != null ? obj['REFERENCE'].replace(/<br>/gi, '\r\n') : '';

            txtVALIDHOURS.value = obj['VALIDHOURS'];
            txtPERMNBR.value = obj['PERMNBR'];
            txtVERSION.value = obj['VERSION'];
            setSelectedValue(ddlAUTHOR_ID.id, obj['AUTHOR_ID']);
            ddlOPER_ID.value = obj['OPER_ID'];
            setSelectedValue(ddlPERMTYPE.id, obj['PERMTYPE']);
            setSelectedValue(ddlFLIGHTTYPE.id, obj['FLIGHTTYPE']);            
            txtBillingAddress.value = obj['BILLINGADDRESS'].replace(/<br>/gi, '\r\n');
            txtPermContent.value = obj['PERMCONTENT'].replace(/<br>/gi, '\r\n');
        }
        function ReadObj() {
            var x = 0;
            if (txtVALIDHOURS.value.trim() == '0' || txtVALIDHOURS.value.trim() == '') {
                if (ddlPERMTYPE.value = 'LD') x = 24;
                else x = 72;
            }
            else x = txtVALIDHOURS.value.trim();
            var obj = {
                ID: $('#perm_id').html().trim() == "" ? 0 : $('#perm_id').html().trim(),
                PERMNBR_ID: createPermNBRID(ddlAUTHOR_ID.value, ddlPERMTYPE.value, txtPERMNBR.value),
                AUTHOR_ID: ddlAUTHOR_ID.value,
                PERMTYPE: ddlPERMTYPE.value,
                FLIGHTTYPE: ddlFLIGHTTYPE.value,
                PERMNBR: txtPERMNBR.value,
                VERSION: txtVERSION.value,
                OPER_ID: ddlOPER_ID.value,
                REFERENCE: txtREFERENCE.value,
                VALIDHOURS: txtVALIDHOURS.value,
                PERMDATE: txtPERMDATE.value.trim() == '' ? new Date().format('dd/mm/yyyy') : txtPERMDATE.value,
                BILLINGADDRESS: txtBillingAddress.value,
                PERMCONTENT: txtPERMCONTENT.value
            };
            return obj;
        }
        function btnUpdateOnclick() {
            if ($('#perm_id').html().trim() != '') {
                //if (checkValidCustomMinlenght('update')) {
                GetArgWithPostBack(JSON.stringify(ReadObj()) + '_____btnUpdateOnclick', 'btnUpdateOnclick');
                //}
                //else alert('Check validate!');
            }
            else {
                alert('Please select Flight');
                return;
            }
        }
        function btnCreateOnclick() {
            if (checkValidCustomMinlenght('update')) {
                isCreatePermMaster = true;
                GetArgWithPostBack(JSON.stringify(ReadObj()) + '_____btnCreateOnclick', 'btnCreateOnclick');

            }

        }

        function LoadDataGrid() {
            GetArgWithPostBack(JSON.stringify(mGetObjectInfo()) + '_____LoadDataGrid', 'LoadDataGrid');
        }

        function CRAFT_ID_Onchange() {
            document.getElementById('txtMTOW').value = parseInt(ddlCRAFT_ID.options[ddlCRAFT_ID.selectedIndex].getAttribute('data-taitrong'));
        }


        //function createPermNBRID(au, typ, nbr, ye) {
        //    var y = '';
        //    if (ye == null || ye == '')
        //        y = new Date().format('dd/mm/yyyy');
        //    return typ + ' ' + LPAD(nbr, 5, '0') + '/' + au + '/' + ye.split('/')[2];
        //}
        function createPermNBRID(au, typ, nbr) {
            var y = new Date().format('yyyy');
            var ax = typ + ' ' + LPAD(nbr, 5, '0') + '/' + au + '/' + y;
            //return typ + ' ' + LPAD(nbr, 5, '0') + '/' + au + '/' + y;
            return ax;
        }
        function LPAD(nbr, iStart, sAlias) {
            var ax = nbr;
            if (nbr.length < iStart) {
                ax = LPAD(sAlias + nbr, iStart, sAlias);
            } else {
                return ax.substring(0, iStart);
            }
            return ax;
        }

        function btnGenMessage() {
            var cf = confirm("Do you want gen message cancel flights ?");
            if (cf) {
                var url = urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=A_TEST_SEARCH&storeName=PermToCanCel_No";

                $.ajax({
                    async: false,
                    method: "PUT",
                    url: url,
                    data: JSON.stringify({ P_ID: $('#perm_id').html() }),
                }).always(function (data) {
                    alert(data.Value == -1 ? 'Error!' : 'Sussess!');
                    btnGenMessageText();
                })
            }
        }
        function btnGenMessageText() {
            var url = urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=PERMISSION2TEXT&storeName=RenderKhb_Cancel";
            $.ajax({
                async: false,
                method: "PUT",
                url: url,
                data: JSON.stringify({ TYPE_ID: 2, PERMID: $('#perm_id').html(), DATE_FLY: dateFormat(new Date().setDate(new Date().getDate()), 'dd-mm-yyyy').replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1') }),
            }).always(function (data) {
                if (data.ListValue == null || data.ListValue == -1) {

                };
            })
        }
    </script>

    <script>
        function LoadShowHisEventClick() {
            $('.show-details-btn').on('click', function (e) {
                e.preventDefault();
                $(this).closest('tr').next().toggleClass('open');
                $(this).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
            });
        };
        var isCreatePermMaster = false;
        var IdSelectDT = 0;
        var idIndetiny = 1;
        var numberRowDelete = 0;
        var iRowAdd = 1; // 0 - no add; 1 add row defaulf
        var iRowSelect = '';
        //var mPERM_ID = document.getElementById('txtPERM_ID');       
        if ($('#perm_id').html().trim() == 0) {
            $('#btnUpdatePermMaster').hide();
        }
        else {
            if (qEdit != 'True') {
                document.getElementById("btnUpdatePermMaster").disabled = true;
                document.getElementById("btnUpdateList").disabled = true;
            }
        }
        var mCRAFT_ID = document.getElementById('<%=ddlCRAFT_ID.ClientID%>');
        var mDAYSFLIGHT = document.getElementById('<%=txtDAYSFLIGHT.ClientID%>');
        var mREMARK = document.getElementById('<%= txtREMARK.ClientID%>');
        var mREGISTRATION = document.getElementById('<%= txtREGISTRATION.ClientID%>');
        var mFLIGHTNBR = document.getElementById('<%= txtFLIGHTNBR.ClientID%>');
        var mTO_AIRP = document.getElementById('<%= ddlTO_AIRP.ClientID%>');
        var mFROM_AIRP = document.getElementById('<%=ddlFROM_AIRP.ClientID%>');
        var mETD = document.getElementById('<%= txtETD.ClientID%>');
        var mPURPOSE_ID = document.getElementById('<%=ddlPURPOSE_ID.ClientID%>');
        var mETA = document.getElementById('<%= txtETA.ClientID%>');
        var mVIA = document.getElementById('<%= txtVIA.ClientID%>');
        <%--var mREMARK_SEND = document.getElementById('<%= txtREMARK_SEND.ClientID%>');--%>
        <%--var mLastUser = document.getElementById('<%= txtLASTUSER.ClientID%>');--%>
        //var mMAX_DATE = document.getElementById('txtMAX_DATE');
        function mGetObjectInfo() {
            var _obj = _objRender;
            _obj['ID'] = IdSelectDT;
            _obj['PERM_ID'] = $('#perm_id').html().trim();
            _obj['CRAFT_ID'] = mCRAFT_ID.value;
            _obj['DAYSFLIGHT'] = mDAYSFLIGHT.value;
            _obj['FLIGHTNBR'] = mFLIGHTNBR.value;
            _obj['REGISTRATION'] = mREGISTRATION.value;
            _obj['FROM_AIRP'] = mFROM_AIRP.value;
            _obj['TO_AIRP'] = mTO_AIRP.value;
            _obj['ETD'] = mETD.value;
            _obj['ETA'] = mETA.value;
            _obj['VIA'] = mVIA.value;
            _obj['LASTUSER'] = '<%= _user.UserName.ToString() %>'
            _obj['PURPOSE_ID'] = mPURPOSE_ID.value;
            //_obj['MAX_DATE'] = mMAX_DATE.value;
            _obj['REMARK'] = mREMARK.value;
            //  _obj['REMARK_SEND'] = mREMARK_SEND.value;
            if ($('#tblSource').attr('data-pageSize') == null) $('#tblSource').attr('data-pageSize', 1000);
            if ($('#tblSource').attr('data-pageIndex') == null) $('#tblSource').attr('data-pageIndex', 1);
            _obj['RSTART'] = (parseInt($('#tblSource').attr('data-pageIndex')) - 1) * parseInt($('#tblSource').attr('data-pageSize'));
            _obj['RFINISH'] = parseInt(_obj['RSTART']) + parseInt($('#tblSource').attr('data-pageSize'));
            return _obj;
        }

        function mbtnAddNewFlightDetailOnclick() {
            if ($('#perm_id').html().trim() != '')
                GetArgWithPostBack(JSON.stringify(mGetObjectInfo()) + '_____mbtnAddNewFlightDetailOnclick', 'mbtnAddNewFlightDetailOnclick');
            else {
                GetArgWithPostBack(JSON.stringify(ReadObj()) + '_____btnCreateOnclick', 'btnCreateOnclick');
                isCreatePermMaster = true;
            }
        }

        function mEditFlightDetailByID(id) {
            iRowSelect = 'tr' + id;
            GetArgWithPostBack(id + '_____mEditFlightDetailByID', 'mEditFlightDetailByID');
        }

        function mbtnUpdateFlightDetailOnclick() {
            //if (checkValidCustomMinlenght('mupdate')) {
            //    mbtnAddNewFlightDetail.removeAttribute('style');
            //    mbtnUpdateFlightDetail.setAttribute('style', 'display: none');
            GetArgWithPostBack(JSON.stringify(mGetObjectInfo()) + '_____mbtnUpdateFlightDetailOnclick', 'mbtnUpdateFlightDetailOnclick');
            //}

        }
        function RestoreHistory(id, ver, UserID) {
            GetArgWithPostBack(id + phanCach + ver + phanCach + UserID + '_____RestoreHistory', 'RestoreHistory');
        }

        function mClearValueControl() {
            //mREMARK_SEND.value = '';
            mREMARK.value = '';

            mVIA.value = '';

            mETA.value = '';
            mETD.value = '';
            mREGISTRATION.value = '';
            mFLIGHTNBR.value = '';

            mDAYSFLIGHT.value = '';

            mCRAFT_ID.selectedIndex = 0;
            mFROM_AIRP.selectedIndex = 0;
            mPURPOSE_ID.selectedIndex = 0;
            mTO_AIRP.selectedIndex = 0;
            //mLastUser.value = '';
            IdSelectDT = 0;
        }
        function mDeleteRowOnclick(id) {
            var rs = confirm('Do you want delete row?');
            if (rs) {
                numberRowDelete = id;
                GetArgWithPostBack(id + '_____mDeleteRowOnclick', 'mDeleteRowOnclick');
            }
        }
        function mReadInfoFlightDetail(data) {
            var obj = JSON.parse(data);

            setSelectedValue(mCRAFT_ID.id, obj['CRAFT_ID']);
            mDAYSFLIGHT.value = obj['DAYSFLIGHT'];
            mREGISTRATION.value = obj['REGISTRATION'];
            mFLIGHTNBR.value = obj['FLIGHTNBR'];
            setSelectedValue(mTO_AIRP.id, obj['TO_AIRP']);
            setSelectedValue(mFROM_AIRP.id, obj['FROM_AIRP']);
            mETD.value = obj['ETD'];
            mETA.value = obj['ETA'];
            mVIA.value = obj['VIA'];

            mREMARK.value = obj['REMARK'];
            //mMAX_DATE.value = obj['MAX_DATE'] == null ? null : obj['MAX_DATE']['DateTime'];
            //mREMARK_SEND.value = obj['REMARK_SEND'];
            mLastUser.value = obj['LASTUSER'];
        }
        function btnDeleteOnclick(id) {
            var result = confirm("Do you want delete?");
            if (result)
                GetArgWithPostBack(id + '_____btnDeleteOnclick', 'btnDeleteOnclick');
        }
        function mShowDetail(id) {
            IdSelectDT = id;
            iRowSelect = 'tr' + id;
            lnkCreate.hide();
            lnkUpdate.show();
            lnkSearch.show();
            lnkClear.hide();
            lnkCancel.show();
            GetArgWithPostBack(id + '_____mShowDetail', 'mShowDetail');
        }
        $('.show-details-btn').on('click', function (e) {
            e.preventDefault();
            $(this).closest('tr').next().toggleClass('open');
            $(this).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
        });
        function insertParam(key, value) {
            key = escape(key); value = escape(value);

            var kvp = document.location.search.substr(1).split('&');
            if (kvp == '') {
                document.location.search = '?' + key + '=' + value;
            }
            else {

                var i = kvp.length; var x; while (i--) {
                    x = kvp[i].split('=');

                    if (x[0] == key) {
                        x[1] = value;
                        kvp[i] = x.join('=');
                        break;
                    }
                }

                if (i < 0) { kvp[kvp.length] = [key, value].join('='); }
                document.location.search = kvp.join('&');
            }
        }
    </script>
    <script>
        var lnkCreate = $('#lnkCreate');
        var lnkClear = $('#lnkClear');
        var lnkUpdate = $('#lnkUpdate');
        var lnkSearch = $('#lnkSearch');
        var lnkCancel = $('#lnkCancel');
        function lnkClear_Click() {
            mClearValueControl();
            lnkSearch.show();
        }
        lnkCancel_Click();
        function lnkCancel_Click() {
            lnkClear_Click();
            lnkCreate.show();
            lnkClear.show()
            lnkUpdate.hide();
            lnkCancel.hide();
            lnkSearch.show();
        }
        function btnSearch_Click() {
            //GetArgWithPostBack(JSON.stringify(mGetObjectInfo()) + '_____btnSearch_Click', 'btnSearch_Click');
            $('#tblSource').attr('data-pageindex', '1');
            LoadDataAjax();            
        }
        function fomatDateTimeCustom(ele) {
            //$ele = $(ele);
            //if($ele.val().length<8)
            //    $ele.focus();
        }
        function checkDateOnblur(ele) {
            //var chkdate = $(ele).val();
            //if(chkdate == "")
            //{
            //    $(ele).focus();
            //    return false;
            //}
            //else if(!chkdate.match(/^(0[1-9]|[12][0-9]|3[01])[\- \/.](?:(0[1-9]|1[012])[\- \/.](19|20)[0-9]{2})$/))
            //{
            //    $(ele).focus();
            //    return false;
            //}
        }
    </script>

    <script>
        //$('body').attr('onSubmit', 'return false;')
    </script>
    <script>
        function GetListFilePerm(permid) {
            GetArgWithPostBack(permid + '_____GetListFilePerm', 'GetListFilePerm');
        }
        function deleteFile(id) {
            var cf = confirm('Do you want delete?');
            if (cf)
                GetArgWithPostBack(id + '_____deleteFile', 'deleteFile');
        }
        function uploadComplete() {
            if ($('#perm_id').html().trim() == '') return;
            //var result = confirm("Do you want Upload File?");
            //if(result)
            GetArgWithPostBack($('#perm_id').html().trim() + "_____uploadComplete", "uploadComplete");
            //else return;
        }
        $(document).ready(function () {
            GetListFilePerm($('#perm_id').html().trim());
        });
    </script>
    <script>
        function prelodingUploadFile() {
            preloadImg('lblLinkFile', 'waitingLoadFile', '15px', '15px');
        }
        function preLoadingData() {
            preloadImg('tblSource', 'waittingLoadGrid', '15px', '15px');
        }
        function unPreLoadingData() {
            $('#waittingLoadGrid').remove();
        }
    </script>

    <script>
        //var _urlAPI = 'https://192.168.63.21:8082/';
        //var _urlAPI = 'http://192.168.63.21/';
        var rowId = 0;
        var inputId = '';
        var qEdit = '<%= _Role.R_Edit %>';
        //var qDel = '<%= _Role.R_Del %>';
        var qDel ='True';
        var dt = ReadObj();
        function GetHisById(_url, _id, user) {
            var rHis = '';
            try {
                $.ajax({
                    async: false,
                    method: "GET",
                    url: _url + _id,
                    complete: function () {
                        //LoadShowHisEventClick();
                    }
                }).always(function (data) {
                    rHis += "<table class=\"table\">";
                    rHis += "<thead>";
                    rHis += "<tr>";
                    rHis += "<th>#</th>";                //colum 1
                    rHis += "<th>No</th>";               //colum 2
                    rHis += "<th>Last user</th>";        //colum 3
                    rHis += "<th>Last modify</th>";      //colum 4
                    rHis += "<th>Conten change</th>";    //colum 5
                    rHis += "<th>Action</th>";           //colum 6
                    rHis += "</tr>";
                    rHis += "</thead>";
                    rHis += "<tbody>";
                    if (data.ListValue == null) return '';
                    $.each(data.ListValue, function (c, d) {
                        rHis += "<tr>";
                        rHis += "<td>";
                        rHis += "<div class=\"action-buttons\">";
                        rHis += "<a data-toggle=\"tooltip\" title=\"Restore\">";
                        rHis += "<i class=\"glyphicon glyphicon-refresh bigger-130\"";
                        rHis += " onclick=\"RestoreHistory(" + _id + ", " + d.NOVERSION + ",'" + user + "');\"></i>";
                        rHis += "</a>";
                        rHis += "</div>";
                        rHis += "</td>";
                        rHis += "<td>" + "1" + "</td>";
                        rHis += "<td>" + d.LASTUSER + "</td>";
                        rHis += "<td>" + new Date(d.LASTMODIFY).format("dd/MM/yyyy hh:mm:ss") + "</td>";
                        rHis += "<td>" + d.CONTENT + "</td>";
                        rHis += "<td>" + d.ACTION + "</td>";
                        rHis += "</tr>";
                    })
                    rHis += "</tbody>";
                    rHis += "</table>";

                });
            } catch (ex) {
                throw ex;
                return '';
            }
            return rHis;
        }
        $('#tblSource>tbody input').focusout(function () {
            tickUpdate('#' + $(this).prop('id'));
        });
        function tickUpdate(id) {
            var $old = $(id).attr('data-oldValue');
            if ($(id).val().toUpperCase() != $old.toUpperCase()) {
                $(id).closest('tr').attr('data-isUpdate', 'true');
            }
            $(id).val($(id).val().toUpperCase());
        }
        function loadScript(url) {
            var js = document.getElementById("sandboxScript");
            if (js !== null) {
                document.body.removeChild(js);
            }
            js = document.createElement("script");
            js.src = url;
            js.id = "sandboxScript";
            document.body.appendChild(js);
        }
        function autoNextTabInput(evt) {
            evt.preventDefault();
            var inputs = $('input[type!=hidden][tabindex!=-1],textarea[tabindex!=-1][type!=hidden]');
            var idxx = inputs.index($('#' + document.activeElement.id));
            if (idxx >= inputs.length) return;
            if ($(inputs[idxx]).val() == '') return;
            $(inputs[idxx + 1]).select();
        }
        function checkPermNumber() {
            $('#divListNumber').html('');
            var $request = $.ajax({
                //async: false,
                method: "PUT",
                url: urlApi + "api/ApiExtension/ExcuteTable?packageName=PERM_PKG&storeName=validFlightNbr",
                data: JSON.stringify({ P_FLIGHT_TYPE: 'NO', P_FLIGHTNBR: $('#txtPERMNBR').val().toUpperCase(), P_PERMTYPE: $('#ddlPERMTYPE').val().toUpperCase() }),
                complete: function () {
                    alert($('#divListNumber').html());
                }
            }).always(function (data) {
                if (data.ListValue[0] != null) {
                    $(data.ListValue[0]).each(function (a, b) {
                        $('#divListNumber').html($('#divListNumber').html() + 'Đã có: ' + b["PERMNBR_ID"] + '\n');
                    })
                } else {
                    $('#divListNumber').html('Chưa có số phép này');
                }
            });
        }
        function binAutocomplete(id, v) {
            var $id = $(id).prop('id');
            var axx = $($id).attr('data-AutoComplete');
            switch (v) {
                case "OPER":
                    $($id).autocomplete({
                        source: [listOper],
                    }).on('blur', function (e, datum) {
                        CheckListOper($(id));
                    });
                    break;
                case "CRAFT":
                    $(id).autocomplete({
                        titleKey: 'name',
                        valueKey: 'name',
                        source: [{
                            data: listCraft,
                        }],
                    }).on('selected.xdsoft', function (e, data) {
                        $($id).val(data.name);
                    }).on('blur', function (e, datum) {
                        CheckCraft($(id));
                    });
                    break;
                case "AERO":
                    $(id).autocomplete({
                        titleKey: 'AE_REM',
                        valueKey: 'AE_REM',
                        source: [{ data: listAero, }],
                    }).on('blur', function (e, datum) {
                        CheckListAero($(id));
                    });
                    break;
                case "PURPOSE":
                    $(id).autocomplete({
                        source: [listPurpose],
                    }).on('blur', function (e, datum) {
                        CheckPurpose($(id));
                    });
                    break;
                case "OPER":
                    $(id).autocomplete({
                        source: [listOper],
                    }).on('blur', function (e, datum) {
                        CheckListOper($(id));
                    });
                    break;
            }
            $(id).focus();
        }
        function returnEmpty(val) {
            return val == null ? "" : val;
        }
        function reloadCheckValid() {
            $(document).ready(function () {
                $('[data-number="true"]').keypress(validateNumber);
                $('[data-minlenght]').keyup(validateEmty1);
                $('[data-minlenght]').each(function () {
                    var el = $(this)[0];
                    var val = el.getAttribute('data-minlenght');
                    if (el.value.length < val) {
                        el.style.border = "red solid 1px";
                    }
                    else {
                        el.style.border = "0px";
                    }
                });
            });
            try {
                $('[data-CheckDate="true"]').each(function () {
                    $(this).on('blur', function () {
                        checkInputDate1($(this));
                    });
                });
            } catch (e) {

            }
        }
        function validateEmty1(event) {
            var el = $(this)[0];
            var val = el.getAttribute('data-minlenght');
            if (el.value.length < val) {
                el.style.border = "red solid 1px";
            } else { try { el.style.border = "0px"; } catch (er) { } }
        }
        function checkInputDate1(ele) {
            var $ele = $(ele);
            var v = $(ele).val();
            if (v.length < 8) {
                $ele.css('border', '1px solid red');
                $ele.val('');
                return;
            }
            if (v.length == 8)
                v = $ele.val().replace(/^(\d{2})(\d{2})(\d{4})$/, '$1/$2/$3');
            if (v.length > 10 || v.length == 9) {
                $ele.css('border', '1px solid red');
                $ele.val('');
                return;
            }
            if (v.length == 10) {
                v = v.replace(/-/g, '/');
                var d = v.replace(/^(\d{2})\/(\d{2})\/(\d{4})$/, '$1');
                var m = v.replace(/^(\d{2})\/(\d{2})\/(\d{4})$/, '$2');
                var y = v.replace(/^(\d{2})\/(\d{2})\/(\d{4})$/, '$3');
                var chk = new Date(y + '/' + m + '/' + d);
                if (chk == 'Invalid Date') {
                    $ele.css('border', '1px solid red');
                    $ele.val('');
                }
                else {
                    $ele.css('border', '');
                    $ele.val(chk.format('dd-mm-yyyy'));
                }
            }
        }
        function LoadDataAjax() {
            if ($('#perm_id').html().trim() == '') return;
            if ($('#perm_id').html().trim() != '') {
                $('#btnUpdatePermMaster').show();
                if (qEdit != 'True') {
                    document.getElementById("btnUpdatePermMaster").disabled = true;
                    document.getElementById("btnUpdateList").disabled = true;
                }
            }
            var kq = '';
            var $request = $.ajax({
                //async: false,
                method: "PUT",
                url: urlApi + "api/PermDetailNo/GetBySearch",
                data: mGetObjectInfo(),
                beforeSend: function () {
                    $('#tblSource tbody tr').remove();
                    $('#tblSource').attr('data-total', 0);
                    preLoadingData();
                },
                complete: function () {
                    unPreLoadingData();
                    reloadCheckValid();
                    $('#tblSource').paging({
                        onClickButton: 'LoadDataAjax'
                    });
                    $('#tblSource input[data-control="_updateAll"]').each(function (a, b) {
                        $(b).mouseover(function () {
                            onmouseoverInput(b.id)
                        });
                        $(b).mouseout(function () {
                            onmouseoutInput(b.id)
                        });
                        if ($(b).prop('id').indexOf('txtDAYSFLIGHT') == -1)
                            $(b).ValidateTip();
                        else $(b).multiDate();
                    })
                },
            }).always(function (data) {
                if (data.ListValue == null) {
                    unPreLoadingData();
                    return;
                }

                $('#tblSource tbody tr').remove();
                $('#tblSource').attr('data-total', data.ListValue[0]['Record_Sum']);
                var strAppend = '';
                $.each(data.ListValue, function (a, b) {
                    strAppend = strAppend + "<tr id='" + b.ID + "' class='" + (compeValid(b.DAYSFLIGHT) ? "" : "cssHetHan") + "' data-isUpdate='false' onmouseover='rowId=" + b.ID + ";' onmouseout='rowId=0;'>"
                    //+  "<td id='b_"+b.RNUM+"'>" +  b.RNUM + "</td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' maxlength='9' class='sInput' id='txtFLIGHTNBR" + a + "' data-oldValue='" + returnEmpty(b.FLIGHTNBR) + "' value='" + returnEmpty(b.FLIGHTNBR) + "' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtREGISTRATION" + a + "' data-oldValue='" + returnEmpty(b.REGISTRATION) + "' value='" + returnEmpty(b.REGISTRATION) + "' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' id='txtFROM_AIRP" + a + "' data-oldValue='" + b.FROM_AIRP + "' onfocusin='binAutocomplete(this,\"AERO\")' value='" + b.FROM_AIRP + "' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' id='txtTO_AIRP" + a + "' data-oldValue='" + b.TO_AIRP + "' onfocusin='binAutocomplete(this,\"AERO\")' value='" + b.TO_AIRP + "' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='4' maxlength='6' class='sInput' id='txtETD" + a + "' data-oldValue='" + returnEmpty(b.ETD) + "' value='" + returnEmpty(b.ETD) + "' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtETA" + a + "' data-oldValue='" + returnEmpty(b.ETA) + "' value='" + returnEmpty(b.ETA) + "' onblur='checkIsUpdate(this)'/></td>"
                    //+ "<td><input type='text' data-control='_updateAll' data-minlenght='1' data-CheckDate='true' class='sInput' id='txtDAYSFLIGHT" + a + "' data-oldValue='" + new Date(b.DAYSFLIGHT).format('dd-mm-yyyy') + "' value='" + new Date(b.DAYSFLIGHT).format('dd-mm-yyyy') + "' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' class='sInput' id='txtDAYSFLIGHT" + a + "' data-oldValue='" + b.DAYSFLIGHT + "' value='" + b.DAYSFLIGHT + "' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' class='sInput' id='txtCRAFT_NAME" + a + "' data-craftid='" + b.CRAFT_ID + "' data-oldValue='" + b.CRAFT_NAME + "' onfocusin='binAutocomplete(this,\"CRAFT\")' value='" + b.CRAFT_NAME + "' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' class='sInput' id='txtPURPOSE_ID" + a + "' data-oldValue='" + b.PURPOSE_ID + "' onfocusin='binAutocomplete(this,\"PURPOSE\")' value='" + b.PURPOSE_ID + "' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtVIA" + a + "' data-oldValue='" + returnEmpty(b.VIA) + "' value='" + returnEmpty(b.VIA) + "' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtREMARK" + a + "' data-oldValue='" + returnEmpty(b.REMARK) + "' value='" + returnEmpty(b.REMARK) + "' onblur='checkIsUpdate(this)'/></td>"
                    + "<td>" + b.TAITRONG + "</td>"
                    + "<td>" + b.LASTUSER + "</td>"                    
                    + "<td style=\"white-space: nowrap;>\""
                                + "<div class=\"action-buttons\">"
                                + (qEdit == "True" ? "<a data-toggle=\"tooltip\" title=\"Edit\">"
                                + "<i class=\"ace-icon fa fa-pencil bigger-130\" onclick=\"mShowDetail(" + b.ID + ");\"></i></a>" : "")
                                + (qDel == "True" ? "<a data-toggle=\"tooltip\" title=\"Delete\">"
                                + "<i id=\"btnDelete_" + b.ID + "\" class=\"ace-icon fa fa-trash-o bigger-130 \" onclick=\"btnDeleteOnclick(" + b.ID + ");\"></i></a>" : "")
                                //+ "<a data-toggle=\"tooltip\" onclick=\'showHisById(this," + b.ID + ")\' class=\"bigger-140 show-details-btn\" title=\"Show history\"><i class=\"ace-icon fa fa-angle-double-down\"></i></a>"                                
                                + "</td>"
                    + "</tr>"
                    //+ "<tr id=\'his_" + a + "\'>"
                    // + GetHisById("<%=System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]%>/api/PermHistory/GetByIdPermDetailNoBk/", b.ID, 'admin')
                    //+ "</tr>";
                });
                $('#tblSource tbody').append(strAppend);
            });
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;

            
            
        }
        function showHisById(cb, id) {
            var $r = $(cb).closest('tr'),
                $h = $r.next(),
                $html = GetHisById("<%=System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]%>/api/PermHistory/GetByIdPermDetailNoBk/", id, '<%= _user.UserName%>')
            $h.children().remove();
            $h.append($html);
        }
       
        
        function checkIsUpdate(id) {
            
            if ($(id).attr('data-isInsert') != undefined) return;
            $inputs = $(id).closest('tr').find('[data-oldValue]');
            var ci = 0;
            $.each($inputs, function (a, b) {
                switch ($(b).attr('type')) {
                    case 'text':
                        if ($(b).attr('data-oldValue') != $(b).val().toUpperCase()) { ci++; };
                        break;
                    case 'checkbox':
                        if ($(b).attr('data-oldValue') != ($(b).prop('checked') ? '1' : '0')) ci++;
                        break;
                }
            })
            if (ci > 0) { $(id).closest('tr').attr('data-isUpdate', 'true'); $(id).closest('tr').addClass('success'); }
            else { $(id).closest('tr').attr('data-isUpdate', 'false'); $(id).closest('tr').removeClass('success'); }
        }
        function btnUpdateList_Onclick() {
            if ($('#perm_id').html().trim() == '') {
                btnCreateOnclick();
                return;
            }
            if (!checkValidCustomMinlenght('_updateAll')) {
                return;
            }
            var $lis = $('tr[data-isUpdate="true"]');
            var $lisIns = $('tr[data-isinsert="true"]');
            if ($lis.length == 0 && $lisIns.length == 0) { alert('No update.'); return; };
            var c = 0;
            $.each($lis, function (a, b) {
                var _obj = {};
                _obj['PERM_ID'] = $('#perm_id').html().trim();
                _obj['ETA'] = $($(b).find('[id^="txtETA"]')[0]).val();
                _obj['ETD'] = $($(b).find('[id^="txtETD"]')[0]).val();
                _obj['CRAFT_ID'] = $($(b).find('[id^="txtCRAFT_NAME"]')[0]).attr('data-craftid');
                _obj['FLIGHTNBR'] = $($(b).find('[id^="txtFLIGHTNBR"]')[0]).val();
                _obj['PURPOSE_ID'] = $($(b).find('[id^="txtPURPOSE_ID"]')[0]).val();
                _obj['FROM_AIRP'] = $($(b).find('[id^="txtFROM_AIRP"]')[0]).val();
                _obj['TO_AIRP'] = $($(b).find('[id^="txtTO_AIRP"]')[0]).val();
                _obj['REGISTRATION'] = $($(b).find('[id^="txtREGISTRATION"]')[0]).val();
                _obj['VIA'] = $($(b).find('[id^="txtVIA"]')[0]).val();
                _obj['REMARK'] = $($(b).find('[id^="txtREMARK"]')[0]).val();
                _obj['DAYSFLIGHT'] = $($(b).find('[id^="txtDAYSFLIGHT"]')[0]).val();
                _obj['LASTUSER'] = '<%= _user.UserName.ToString()%>';
                _obj['ID'] = $(b).prop('id');
                var $request = $.ajax({
                    async: false,
                    method: "PUT",
                    url: urlApi + "api/PermDetailNo/UpdatePermDetailNo",
                    data: _obj,
                }).always(function (data) {
                    if (data.Value == null) c++;
                });
            })

            $.each($lisIns, function (a, b) {
                var _obj = {};
                _obj['PERM_ID'] = $('#perm_id').html().trim();
                _obj['ETA'] = $($(b).find('[id^="txtETA"]')[0]).val();
                _obj['ETD'] = $($(b).find('[id^="txtETD"]')[0]).val();
                _obj['CRAFT_ID'] = $($(b).find('[id^="txtCRAFT_NAME"]')[0]).attr('data-craftid');
                _obj['FLIGHTNBR'] = $($(b).find('[id^="txtFLIGHTNBR"]')[0]).val();
                _obj['PURPOSE_ID'] = $($(b).find('[id^="txtPURPOSE_ID"]')[0]).val();
                _obj['FROM_AIRP'] = $($(b).find('[id^="txtFROM_AIRP"]')[0]).val();
                _obj['TO_AIRP'] = $($(b).find('[id^="txtTO_AIRP"]')[0]).val();
                _obj['REGISTRATION'] = $($(b).find('[id^="txtREGISTRATION"]')[0]).val();
                _obj['VIA'] = $($(b).find('[id^="txtVIA"]')[0]).val();
                _obj['REMARK'] = $($(b).find('[id^="txtREMARK"]')[0]).val();
                _obj['DAYSFLIGHT'] = $($(b).find('[id^="txtDAYSFLIGHT"]')[0]).val();
                _obj['LASTUSER'] = '<%= _user.UserName.ToString()%>';
                var $request = $.ajax({
                    async: false,
                    method: "POST",
                    url: urlApi + "api/PermDetailNo/CreatePermDetailNo",
                    data: _obj,
                }).always(function (data) {
                    if (data.Message == 'Cập nhật dữ liệu thành công') c++;
                });
            })

            alert('Update susser: ' + c + 'flight permission.');
            LoadDataAjax();
            //btnUpdate_GenBack_NoConfirm();
        }

        function btnUpdate_GenBack() {
            var result = confirm("Do you want gen flights to scchedule ?");
            if (result) {
                if (!checkValidCustomMinlenght('_updateAll')) {
                    return;
                }
                var $lis = $('tr[data-isUpdate="false"]');
                if ($lis.length == 0) { alert('No Flights for gen.'); return; };
                var c = 0;
                $.each($lis, function (a, b) {
                    var _sl = $($(b).find('[id^="txtSL"]')[0]).val();
                    if (_sl != '1') {
                        var _obj = {};
                        _obj['PERM_ID'] = $('#perm_id').html().trim();
                        _obj['ETA'] = $($(b).find('[id^="txtETA"]')[0]).val();
                        _obj['ETD'] = $($(b).find('[id^="txtETD"]')[0]).val();
                        _obj['CRAFT_ID'] = $($(b).find('[id^="txtCRAFT_NAME"]')[0]).attr('data-craftid');
                        _obj['FLIGHTNBR'] = $($(b).find('[id^="txtFLIGHTNBR"]')[0]).val();
                        _obj['PURPOSE_ID'] = $($(b).find('[id^="txtPURPOSE_ID"]')[0]).val();
                        _obj['FROM_AIRP'] = $($(b).find('[id^="txtFROM_AIRP"]')[0]).val();
                        _obj['TO_AIRP'] = $($(b).find('[id^="txtTO_AIRP"]')[0]).val();
                        _obj['REGISTRATION'] = $($(b).find('[id^="txtREGISTRATION"]')[0]).val();
                        _obj['VIA'] = $($(b).find('[id^="txtVIA"]')[0]).val();
                        _obj['REMARK'] = $($(b).find('[id^="txtREMARK"]')[0]).val();
                        _obj['DAYSFLIGHT'] = $($(b).find('[id^="txtDAYSFLIGHT"]')[0]).val();
                        _obj['LASTUSER'] = '<%= _user.UserName.ToString()%>';
                        _obj['ID'] = $(b).prop('id');
                        var $request = $.ajax({
                            async: false,
                            method: "PUT",
                            url: urlApi + "api/PermDetailNo/UpdatePermDetailNo_GenBack",
                            data: _obj,
                        }).always(function (data) {
                            if (data.Value == null) c++;
                        });

                    }
                })
                alert('Gen Flights : ' + c + ' to flight permission.');
                LoadDataAjax();
            }
        }

       function btnUpdate_GenBack_NoConfirm() {
            
                var $lis = $('tr[data-isUpdate="false"]');               
                var c = 0;
                $.each($lis, function (a, b) {
                    var _sl = $($(b).find('[id^="txtSL"]')[0]).val();
                    if (_sl != '1') {
                        var _obj = {};
                        _obj['PERM_ID'] = $('#perm_id').html().trim();
                        _obj['ETA'] = $($(b).find('[id^="txtETA"]')[0]).val();
                        _obj['ETD'] = $($(b).find('[id^="txtETD"]')[0]).val();
                        _obj['CRAFT_ID'] = $($(b).find('[id^="txtCRAFT_NAME"]')[0]).attr('data-craftid');
                        _obj['FLIGHTNBR'] = $($(b).find('[id^="txtFLIGHTNBR"]')[0]).val();
                        _obj['PURPOSE_ID'] = $($(b).find('[id^="txtPURPOSE_ID"]')[0]).val();
                        _obj['FROM_AIRP'] = $($(b).find('[id^="txtFROM_AIRP"]')[0]).val();
                        _obj['TO_AIRP'] = $($(b).find('[id^="txtTO_AIRP"]')[0]).val();
                        _obj['REGISTRATION'] = $($(b).find('[id^="txtREGISTRATION"]')[0]).val();
                        _obj['VIA'] = $($(b).find('[id^="txtVIA"]')[0]).val();
                        _obj['REMARK'] = $($(b).find('[id^="txtREMARK"]')[0]).val();
                        _obj['DAYSFLIGHT'] = $($(b).find('[id^="txtDAYSFLIGHT"]')[0]).val();
                        _obj['LASTUSER'] = '<%= _user.UserName.ToString()%>';
                        _obj['ID'] = $(b).prop('id');
                        var $request = $.ajax({
                            async: false,
                            method: "PUT",
                            url: urlApi + "api/PermDetailNo/UpdatePermDetailNo_GenBack",
                            data: _obj,
                        }).always(function (data) {
                            if (data.Value == null) c++;
                        });

                    }
                })               
           
           
        }



        window.onbeforeunload = function () {
            return "Leaving this page will reset the wizard";
        };
        window.onkeydown = function (e) {
            var charCode = (e.which) ? e.which : e.keyCode;
            var c = $('#tblSource tr').length;
            var _tr = $("<tr id='_" + c + "' data-isInsert='true' onmouseover=\"rowId=$(this).prop(\'id\');\" onmouseout='rowId=0;' class='rowCreate'>"
                    //+  "<td id='b_"+b.RNUM+"'>" +  b.RNUM + "</td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' maxlength='9' class='sInput' id='txtFLIGHTNBR" + c + "' value='" + $('#ddlOPER_ID').val() + "' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtREGISTRATION" + c + "' value='' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' id='txtFROM_AIRP" + c + "' onfocusin='binAutocomplete(this,\"AERO\")' value='' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' id='txtTO_AIRP" + c + "' onfocusin='binAutocomplete(this,\"AERO\")' value='' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='4' maxlength='6' class='sInput' id='txtETD" + c + "' data-number='true' value='' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtETA" + c + "' data-number='true' value='' onblur='checkIsUpdate(this)'/></td>"
                    //+ "<td><input type='text' data-control='_updateAll' data-minlenght='1' data-CheckDate='true' class='sInput' id='txtDAYSFLIGHT" + a + "' data-oldValue='" + new Date(b.DAYSFLIGHT).format('dd-mm-yyyy') + "' value='" + new Date(b.DAYSFLIGHT).format('dd-mm-yyyy') + "' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' class='sInput' id='txtDAYSFLIGHT" + c + "'  value='' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' class='sInput' id='txtCRAFT_NAME" + c + "' data-craftid='0'  onfocusin='binAutocomplete(this,\"CRAFT\")' value='' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' class='sInput' id='txtPURPOSE_ID" + c + "' onfocusin='binAutocomplete(this,\"PURPOSE\")' value='' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtVIA" + c + "' value='' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtREMARK" + c + "' value='' onblur='checkIsUpdate(this)'/></td>"
                    + "<td></td>"
                    + "<td></td>"
                    + "<td style=\"white-space: nowrap;>\""
                                + "<div class=\"action-buttons\">"
                                + (qDel == "True" ? "<a data-toggle=\"tooltip\" title=\"Delete\">"
                                + "<i id=\"btnDelete_01\" class=\"ace-icon fa fa-trash-o bigger-130\" onclick=\"var cf = confirm('Do you want detete'); if(cf) $('#_" + c + "').remove();\"></i></a>" : "")
                                //+ "<a data-toggle=\"tooltip\" class=\"bigger-140 show-details-btn\" title=\"Show history\"><i class=\"ace-icon fa fa-angle-double-down\"></i></a>"
                                + "</td>"
                    + "</tr>");
            if (charCode == 118) {
                if ($('#tblSource tbody tr').length == 0) {
                    $('#tblSource tbody').append(_tr);
                    $('#tblSource input[data-control="_updateAll"]').each(function (a, b) {
                        if ($(b).prop('id').indexOf('txtDAYSFLIGHT') == -1) {
                            $(b).mouseover(function () {
                                onmouseoverInput(b.id)
                            });
                            $(b).mouseout(function () {
                                onmouseoutInput(b.id)
                            });
                            $(b).ValidateTip();
                        } else {

                            $(b).multiDate();

                        }
                    })
                    $('#_' + c + ' input')[0].focus();
                    return;
                }
                if (rowId == 0) {
                    alert('Please select row!');
                    return;
                }
                var $trCurrent = $('#' + rowId),
                    $tr = $(_tr);
                $tr.removeClass('success')
                $tr.addClass('rowCreate');
                $tr.removeAttr('data-isupdate');
                $tr.prop('id', '_' + c);
                $tr.attr('onmouseover', 'rowId=$(this).prop(\'id\')');
                $.each($tr.find('input[data-oldValue]'), function () {
                    $(this).removeAttr('data-oldValue');
                });
                $('#' + rowId + ' input[type!=hidden][tabindex!=-1]').each(function (v, n) {
                    $($tr).find('[id^=' + $(n).prop('id').replace(/[0-9]\abc/gi, '') + ']').val($(n).val());
                });
                $tr.attr('data-isInsert', 'true');
                var $input1 = $($trCurrent).find('input[tabindex!=-1][type!=hidden]');
                var $input2 = $($tr).find('input[tabindex!=-1][type!=hidden]');
                for (var i = 0; i < $input2.length; i++) {
                    $($input2[i]).val($($input1[i]).val());
                    $($input2[i]).prop('checked', $($input1[i]).prop('checked'));
                    $($input2[i]).attr('data-craftid', $($input1[i]).attr('data-craftid'));
                }
                $($tr).find('[id^=txtFROM_AIRP]').val($($trCurrent).find('[id^=txtTO_AIRP]').val());
                $($tr).find('[id^=txtTO_AIRP]').val($($trCurrent).find('[id^=txtFROM_AIRP]').val());
                $($tr).find('[id^=txtVIA]').val(daoNguocVia(($trCurrent).find('[id^=txtVIA]').val()));
                $trCurrent.after($tr);
                $($tr).find('input')[0].focus();
                reloadCheckValid();
                $('#tblSource input[data-control="_updateAll"]').each(function (a, b) {
                    if ($(b).prop('id').indexOf('txtDAYSFLIGHT') == -1) {
                        $(b).mouseover(function () {
                            onmouseoverInput(b.id)
                        });
                        $(b).mouseout(function () {
                            onmouseoutInput(b.id)
                        });
                        $(b).ValidateTip();
                    } else {
                        $(b).multiDate();
                    }
                })

            }
            if (charCode == 119) {
                if (inputId == '') {
                    alert('Please select!');
                    return;
                }
                var $ele = $('#' + inputId),
                    $rowIndex = $ele.closest('tr').index(),
                    $columIndex = $ele.closest('td').index(),
                    $type = $ele.prop('type');
                
		 /*QUYNX2020*/
                $ele.closest('tr').attr('data-isUpdate', 'true');
                $ele.closest('tr').addClass('success');

		switch ($type) {
                    case 'text':
                        var x = $('#tblSource tr').eq($rowIndex + 1).find('td').eq($columIndex).find('input').val();
                        if (x == undefined)
                            x = $('#tblSource tr').eq($rowIndex).find('td').eq($columIndex).find('input').val();
                        //var x = inputId.indexOf('abc') != -1 ? $('#tblSource tr').eq($rowIndex + 1).find('td').eq($columIndex).find('input').val() : $('#tblSource tr').eq($rowIndex).find('td').eq($columIndex).find('input').val();
                        if (x == '' || x == undefined) return;
                        $ele.val(x);
                        $ele.focus();
                        break;
                    case 'checkbox':
                        $('#tblSource tr').eq($rowIndex).find('td').eq($columIndex).find('input')[0].prop('checked', $ele.prop('checked'));
                        break;
                }

            }
            //reloadAutoComplete();
        }
        function onmouseoverInput(id) {
            inputId = id;
        }
        function onmouseoutInput(id) {
            inputId = '';
        }
    </script>
     <script>
        LoadDataAjax();
        
    </script>
    <script>
        (function ($) {
            var $cssStyle = $('<style id="checktipCssstyle">'
                        + '.ABC{ position: relative; display: inline-block;}'
                        + '.ABCD {position: absolute; top: 0px; right: 3px; color: red; font-size: 12px; z-index: 1; white-space: nowrap; height: 10px; width: 10px; tabindex="-1" }'
                        + '</style>');
            if ($('#checktipCssstyle')[0] == undefined)
                $('body').append($cssStyle);
            function preIns(that) {
                var createTip = function (ele) {

                    var $ele = $(ele),
                        _hasDiv = $ele.parent().hasClass('ABC'),
                        _hasSpan = $ele.next().hasClass('ABCD'),
                        isNumber = false, isDate = false, isMinlength = false, isMaxlength = false;
                    var $div = _hasDiv ? $ele.parent() : $('<div class="ABC">');
                    var $span = _hasSpan ? $ele.next() : $('<span class="ABCD glyphicon glyphicon-remove">');
                    if ($ele.attr('data-contenttip') != undefined) {
                        $span.attr('data-toggle', 'tooltip');
                        $span.attr('title', $ele.attr('data-contenttip'));
                        $span.tooltip();
                    } else $span.attr('title', '');
                    if (!_hasDiv) { $ele.wrap($div); $ele.focus(); }
                    if (!_hasSpan) { $ele.after($span); $ele.focus(); }
                }
                var removeTip = function (ele) {
                    if ($(ele).next().hasClass('ABCD')) {
                        $(ele).next().remove();
                        $(ele).focus();
                    }

                }
                $(that).keyup(function (e) {
                    var $ele = $(that);
                    $ele.attr('data-contenttip', 'format dd,dd....,ddmmyy');
                    if (e.keyCode == 13) {
                        var _new = [],
                            c = 0,
                            v = $ele.val().toUpperCase();
                        v = v.replace(/JAN/gi, '01');
                        v = v.replace(/FEB/gi, '02');
                        v = v.replace(/MAR/gi, '03');
                        v = v.replace(/APR/gi, '04');
                        v = v.replace(/MAY/gi, '05');
                        v = v.replace(/JUN/gi, '06');
                        v = v.replace(/JUL/gi, '07');
                        v = v.replace(/AUG/gi, '08');
                        v = v.replace(/SEP/gi, '09');
                        v = v.replace(/OCT/gi, '10');
                        v = v.replace(/NOV/gi, '11');
                        v = v.replace(/DEC/gi, '12');
                        $ele.val(v);
                        var ds = $ele.val().trim().split(',');
                        $(ds).each(function (a, b) {
                            if (b.length == 8) {
                                b = b.replace(/^(\d{2})(\d{2})(\d{4})$/, '$1-$2-$3');
                                if (!isValidDate(b)) {
                                    c = 1;
                                } else
                                    _new.push(b);
                            }
                            else if (b.length == 6) {
                                b = b.replace(/^(\d{2})(\d{2})(\d{2})$/, '$1-$2-20$3');
                                if (!isValidDate(b)) {
                                    c = 1;
                                } else
                                    _new.push(b);
                            }
                            else if (b.length == 2) {
                                var chuan = ds[ds.findIndex(item => item.length == 6 && ds.findIndex(x=>x == item) > a)];
                                var mmyy = chuan.substring(2, 6);
                                b = (b + mmyy).replace(/^(\d{2})(\d{2})(\d{2})$/, '$1-$2-20$3');
                                if (!isValidDate(b)) {
                                    c = 1;
                                } else
                                    _new.push(b);
                            }
                            else if (b.length == 10) {
                                if (!isValidDate(b)) {
                                    c = 1;
                                }
                                else _new.push(b);
                            }
                            else {
                                c = 1;
                            }
                        });
                        if (c == 0) {
                            $ele.val('');
                            $(_new).each(function () {
                                $ele.val($ele.val() + this + ',');
                            })
                            $ele.val($ele.val().substring(0, $ele.val().length - 1));
                            $(this).attr('data-contenttip', '');
                            $ele.css({ 'color': '' })
                            removeTip($ele);
                        } else {
                            $ele.attr('data-contenttip', 'format dd,dd....,ddmmyy');
                            $ele.css({ 'color': 'red' });
                            createTip($ele);

                        }
                    }
                })
                $(that).bind('blur', function () {
                    var $ele = $(that);
                    var _new = [],
                            c = 0,
                            v = $ele.val().toUpperCase();
                    v = v.replace(/JAN/gi, '01');
                    v = v.replace(/FEB/gi, '02');
                    v = v.replace(/MAR/gi, '03');
                    v = v.replace(/APR/gi, '04');
                    v = v.replace(/MAY/gi, '05');
                    v = v.replace(/JUN/gi, '06');
                    v = v.replace(/JUL/gi, '07');
                    v = v.replace(/AUG/gi, '08');
                    v = v.replace(/SEP/gi, '09');
                    v = v.replace(/OCT/gi, '10');
                    v = v.replace(/NOV/gi, '11');
                    v = v.replace(/DEC/gi, '12');
                    $ele.val(v);
                    var ds = $ele.val().trim().split(',');
                    $(ds).each(function (a, b) {
                        if (b.length == 8) {
                            b = b.replace(/^(\d{2})(\d{2})(\d{4})$/, '$1-$2-$3');
                            if (!isValidDate(b)) {
                                c = 1;
                            } else
                                _new.push(b);
                        }
                        else if (b.length == 6) {
                            b = b.replace(/^(\d{2})(\d{2})(\d{2})$/, '$1-$2-20$3');
                            if (!isValidDate(b)) {
                                c = 1;
                            } else
                                _new.push(b);
                        }
                        else if (b.length == 2) {
                            var chuan = ds[ds.findIndex(item => item.length == 6 && ds.findIndex(x=>x == item) > a)];
                            var mmyy = chuan.substring(2, 6);
                            b = (b + mmyy).replace(/^(\d{2})(\d{2})(\d{2})$/, '$1-$2-20$3');
                            if (!isValidDate(b)) {
                                c = 1;
                            } else
                                _new.push(b);
                        }
                        else if (b.length == 10) {
                            if (!isValidDate(b)) {
                                c = 1;
                            }
                            else _new.push(b);
                        }
                        else {
                            c = 1;
                        }
                    });
                    if (c == 0) {
                        $ele.val('');
                        $(_new).each(function () {
                            $ele.val($ele.val() + new Date(this.replace(/^(\d{2})-(\d{2})-(\d{4})$/, '$3-$2-$1')).format('dd-mmm-yyyy') + ',');
                        })
                        $ele.val($ele.val().substring(0, $ele.val().length - 1));
                        $(this).attr('data-contenttip', '');
                        $ele.css({ 'color': '' })
                        removeTip($ele);
                    } else {
                        $ele.attr('data-contenttip', 'format dd,dd....,ddmmyy');
                        $ele.css({ 'color': 'red' });
                        $ele.focus();
                    }
                })
            }
            $.fn.multiDate = function () {
                if ($('#checktipCssstyle')[0] == undefined)
                    $('body').append($cssStyle);
                $(this).attr('data-contenttip', 'format dd,dd....,ddmmyy');
                preIns(this);
            };
        }(jQuery));

    </script>
    <script>
        function getMessageFullByRefence() {
            if ($('#perm_id').html().trim() == '' && $('#txtREFERENCE').val().trim() == '') {
                alert('No reference');
                return;
            }
            window.open('<%= Page.ResolveUrl("~/MessManagement/ShowMessageFullContent.aspx") + "?Menu_Id="
                            + Request.Params["Menu_ID"] %>' + '&id=' + $('#perm_id').html().trim() + '&fType=NO&content=' + $('#txtREFERENCE').val().replace(/\n/gi, '%0A'), '_blank', 'toolbar=yes,scrollbars=yes,resizable=yes').resizeTo(1200, 520);
            //resizeTo(window.screen.availWidth, window.screen.availHeight)
        }
    </script>
    <script>
        $('#ctl00_Body1').trigger({
            type: 'keydown',
            keyCode: 9
        });
    </script>
    <script>
        $('#txtPERMDATE').val(new Date().format('dd/mm/yyyy'));
        
	
	function compeValid(t) {
	    
            var dates = t.split(',');
            var bx = 0;
            var ax = 0;
	              
            
            

            var parts =t.split('-');
	    //var mydate = new Date(parts[2], parts[1], parts[0]); 
            
 	    var mydate  = new Date(parts[1] + "," + parts[0] +"," + parts[2]);
	    
	    //console.log(mydate);	
            $.each(dates, function (a, b) {
                var dx = new Date(b);
               
                if ((new Date(new Date().format('yyyy/mm/dd')) - mydate) <= 0)
                    bx++;
		/*
                if ((new Date(new Date().format('yyyy/mm/dd')).getTime() - new Date(b).getTime()) <= 0)
                    ax++;*/
            })

            

		
            if (bx > 0) return true;
            return false;
        }
        function sortOnclick(ele) {

            var s = parseInt($(ele).attr('data-sort'));
            s *= -1;
            $(ele).attr('data-sort', s);
            var span = s == '1' ? '<p class="glyphicon glyphicon-triangle-bottom"></p>'
    : '<p class="glyphicon glyphicon-triangle-top"></p>';
            $('#tblSource TR').eq(0).find('p').remove();
            $(ele).append(span);

            var n = $(ele).prevAll().length;
            sortTable(s, n);
        }
        function sortTable(f, n) {
            var rows = $('#tblSource tbody  tr').get();
            rows.sort(function (a, b) {
                var A = getVal(a);
                var B = getVal(b);
                if (A < B) {
                    return -1 * f;
                }
                if (A > B) {
                    return 1 * f;
                }
                return 0;
            });
            function getVal(elm) {
                //var v = $(elm).children('td').eq(n).text().toUpperCase();
                var v = $(elm).children('td').eq(n).find('input').val().toUpperCase();
                if ($.isNumeric(v)) {
                    v = parseInt(v, 10);
                }
                return v;
            }

            $.each(rows, function (index, row) {
                $('#tblSource').children('tbody').append(row);
            });
        }
    </script>
    <script>
        function ddlPERMTYPE_Change() {
            $('#txtVALIDHOURS').val($('#ddlPERMTYPE').val() == 'LD' ? '24' : '72');
        }
        $(document).ready(function () {
            if ($('#perm_id').html().trim() == '') {
                ddlPERMTYPE_Change();
                $('#txtREFERENCE').val('MAIL ' + new Date().format('ddmmyy'));
            }
        })
        function daoNguocVia(strA) {
            var ar = strA.split('/');
            var kq = '';
            for (var i = ar.length - 1; i >= 1; i--) {
                if (ar[i].trim() != '')
                    kq += ar[i].trim() + '/';
            }
            return (kq + ar[0]).toUpperCase();
        }
    </script>
</asp:Content>
