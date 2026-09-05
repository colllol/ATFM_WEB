<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFMNoneMenu.Master" AutoEventWireup="true"
    CodeBehind="View_PermSC.aspx.cs" Inherits="prjApplication.Permission.View_PermSC" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<%@ Register Assembly="prjComponents" Namespace="prjComponents" TagPrefix="cc1" %>
<%@ Import Namespace="prjBusinessLogic" %>
<%@ Import Namespace="prjInfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/autocomplete.css" rel="stylesheet" />
     <link href="../Style/Style_List.css" rel="stylesheet" />
    
    <style>
        .cssHetHan {
            color: red !important;
        }

            .cssHetHan input {
                color: red !important;
            }

        #tblSource input, select {
            color: black;
        }

        .xdsoft_autocomplete_dropdown {
            min-width: 130px !important;
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

        input {
            text-transform: uppercase;
        }

        textarea {
            text-transform: uppercase;
        }

        .permission-flight-details {
            margin: 18px 0 12px;
            border: 1px solid #c5d9ea;
            border-radius: 6px;
            background: #fff;
        }

        .permission-flight-details__header {
            display: flex;
            min-height: 42px;
            padding: 9px 12px;
            align-items: center;
            justify-content: space-between;
            border-bottom: 1px solid #c5d9ea;
            background: #eef7fd;
        }

        .permission-flight-details__title {
            margin: 0;
            color: #1d5f91;
            font-size: 16px;
            font-weight: 600;
            text-transform: uppercase;
        }

        .permission-flight-details__total {
            color: #234761;
            font-size: 12px;
            font-weight: 700;
        }

        .permission-flight-details__scroll {
            width: 100%;
            max-height: 520px;
            overflow: auto;
        }

        .permission-flight-details__table {
            width: 100%;
            min-width: 1550px;
            margin: 0;
            border-collapse: collapse;
            font-size: 12px;
        }

        .permission-flight-details__table th,
        .permission-flight-details__table td {
            padding: 7px 8px;
            border: 1px solid #c5d9ea;
            vertical-align: middle;
            white-space: nowrap;
        }

        .permission-flight-details__table th {
            position: sticky;
            top: 0;
            z-index: 2;
            background: #337ab7;
            color: #fff;
            text-align: center;
        }

        .permission-flight-details__table tbody tr:nth-child(even) {
            background: #f5faff;
        }

        .permission-flight-details__message,
        .permission-flight-details__error {
            padding: 20px !important;
            text-align: center;
        }

        .permission-flight-details__error {
            color: #c0392b;
        }

        .permission-content-panel {
            margin: 12px 0 18px;
            padding: 12px;
            border: 1px solid #c5d9ea;
            border-radius: 6px;
            background: #fff;
        }

        .permission-content-panel label {
            display: block;
            margin-bottom: 6px;
            color: #234761;
            font-size: 13px;
            font-weight: 600;
            text-transform: uppercase;
        }
    </style>
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
    <div id="pathFileUpload" hidden="hidden"></div>
    <div id="perm_id" hidden="hidden"><%= Request.QueryString["ID"]==null?"": Request.QueryString["ID"]%></div>
    <div id="divExcuteScript" hidden="hidden"></div>
    <fieldset class="box-border">
        <div id="divPermMaster">         
        <label for="txtPERMNBR_ID" style="display:none;">PERMNBR</label>
                        <input id="txtPERMNBR_ID" readonly type="text" class="wid_120px" style="display:none;" />
              
        <table style="width:100%;border:0px solid black;">                                                                          
            <tr>               
                <td style="width:50%">                    
                     <fieldset class="box-border">
                    <legend class="box-border">Fly Permission Author</legend> 
                    <asp:DropDownList ID="ddlAUTHOR_ID" runat="server" CssClass="wid_565px">
                        </asp:DropDownList>   
                     </fieldset>    
                         </td>
                <td style="width:50%">   
                     <fieldset class="box-border">
                    <legend class="box-border">Operator</legend>   
                         <input id="ddlOPER_ID" class="wid_400px" type="text" data-autocomplete="OPER" data-minlenght="1" data-control="update" />
                          </fieldset>   
                         </td>
            </tr>            
            <tr  valign="top">
                <td style="width:50%">
                    <fieldset class="box-border">
                                        <legend class="box-border">Reference</legend>   
                        <table style="width:100%;border:0px solid black;">
                            <tr>
                                <td> 
                                     <label for="txtPERMNBR" style="font-size:12px;">Number</label><br />
                                    <input id="txtPERMNBR" maxlength="5" data-minlenght="1" data-control="update" type="text"
                            class="wid_80px" style="height:25px;" />
                                    <span style="position: relative; cursor: pointer; color: blue; font-style:italic;" onclick="checkPermNumber()">check
                                    <i style="position:absolute; top: 0px; left:45px; width: 250px; display: none;" id="divListNumber"></i>
                                        </span>
                                </td>
                                <td> <label for="txtPERMTYPE" style="font-size:12px;">Type</label><br />
                                    <select id="ddlPERMTYPE" onchange="ddlPERMTYPE_Change()" class="wid_80px" style="height:25px;">
                            <option value="LD">LD</option>
                            <option value="O/F">O/F</option>
                        </select></td>
                                <td>  <label for="txtVERSION" style="font-size:12px;">Version</label><br /> <input style="height:25px;" id="txtVERSION" data-minlenght="1" data-control="update" maxlength="1" type="text" class="wid_80px" /> </td>
                                <td><label for="txtSEASON">Season</label><br /> <select style="height:25px;" id="ddlSEASON" class="wid_80px">
                                <option value="W">Winter</option>
                                <option value="S">Summer</option>
                            </select></td>
                                <td>  <label for="txtPERMDATE" style="font-size:12px;">Date</label><br /> <input style="height:25px;" id="txtPERMDATE" data-minlenght="10" data-control="update" type="text" class="wid_80px" /> </td>
                                <td>  <label for="txtVALIDHOURS" style="font-size:12px;">Hours</label><br /> <input style="height:25px;" id="txtVALIDHOURS" maxlength="2" data-number="true" data-control="update"
                            data-minlenght="1" type="text"
                            class="wid_80px" /> </td>
                            </tr>
                            <tr>
                            <td colspan="6"> <label for="txtREFERENCE" style="font-size:12px;">Reference</label><br />
                        <textarea id="txtREFERENCE" maxlength="4000" data-control="update"
                            rows="3" class="wid_100" data-toggle="tooltip"  title="Limit 4000 character !"></textarea> 
<%--<button id="btnShowReference" type="button" class="btn btn-primary" onclick="getMessageFullByRefence()">Reference</button>--%>
                                
                            </td>  </tr>
                         </table>   
                    </fieldset>
                     </td>
                <td style="width:50%"> 
                    <fieldset class="box-border">
                                        <legend class="box-border">Other</legend>   
                 <table style="width:100%;border:0px solid black;">
                             <tr>
                                  <td>
                                       <label for="txtREFERENCE" style="font-size:12px;">File</label><br />
                        
                                      <asp:AsyncFileUpload ID="AsyncFileUpload1"  runat="server" OnClientUploadComplete="uploadComplete" OnClientUploadStarted="prelodingUploadFile"
                OnUploadedComplete="AsyncFileUpload1_UploadedComplete" ClientIDMode="AutoID" />
                                      <label id="lblLinkFile" style="font-weight:100"></label>
                                      
                                  </td>

                             </tr>
                             <tr>
                                 <td>
                                     <label for="txtBILLINGADDRESS" style="font-size:12px;">Billing address</label><br />
                        <textarea rows="2" id="txtBILLINGADDRESS" maxlength="4000" data-control="update"
                             class="wid_100" data-toggle="tooltip" title="Limit 4000 character !"></textarea> 

                                 </td>

                             </tr>   
                                                                                                                              
                    </table>         
                
                
                </fieldset>
                     </td>
            </tr>
        </table>
         
        <div style="display: none""><label class="col-lg-4 control-label" for="txtBEGINDATE">BEGINDATE</label>
                        <input id="txtBEGINDATE" data-date-format="dd/mm/yyyy" type="text" class="wid_120px" />  
        <label class="col-lg-4 control-label" for="txtENDDATE">ENDDATE</label>
                        <input id="txtENDDATE" data-date-format="dd/mm/yyyy" type="text" class="wid_120px" /> 

        </div>
                      
    </div>
     </fieldset>
    <!--
    <div class="row" style="text-align: center">
        <button id="btnUpdatePermMaster" class="btn btn-sm btn-primary" type="button" onclick="btnUpdateOnclick()">Update</button>
            <a style="display:none;" href="<%= Page.ResolveUrl("~/Permission/ListPermissionSC.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString()) %>"
                class="btn btn-sm btn-primary">
                <span class="ace-icon fa fa-times"></span>
                Exit
            </a>           
        </div>
        <hr />
    <div id="divPermDetail">
        <label id="lblTotalRecord"></label>
        <button type="button" id="btnUpdateList" class="btn btn-sm btn-primary" onclick="btnUpdateList_Onclick()">Update all</button>
        <button type="button" id="btnSearchBy" class="btn btn-sm btn-primary" onclick="btnSearch_Click()">Search</button>
        <button type="button" id="btnClearSearch" class="btn btn-sm btn-primary" onclick="lnkClear_Click()">Clear search</button>
        <table id="tblSource" class="">
            <thead>
                <tr>
                    <th>TT</th>
                    <th data-sort="1" onclick="sortOnclick(this);">Call Sign</th>
                    <th data-sort="1" onclick="sortOnclick(this);">Registration</th>
                    <th data-sort="1" onclick="sortOnclick(this);">From</th>
                    <th data-sort="1" onclick="sortOnclick(this);">To</th>
                    <th data-sort="1" onclick="sortOnclick(this);">Etd</th>
                    <th data-sort="1" onclick="sortOnclick(this);">Eta</th>
                    <th>All</th>
                    <th>D1</th>
                    <th>D2</th>
                    <th>D3</th>
                    <th>D4</th>
                    <th>D5</th>
                    <th>D6</th>
                    <th>D7</th>
                    <th data-sort="1" onclick="sortOnclick(this);">Craft</th>
                    <th data-sort="1" onclick="sortOnclick(this);">Begin date</th>
                    <th data-sort="1" onclick="sortOnclick(this);">End date</th>
                    <th data-sort="1" onclick="sortOnclick(this);">Purpose</th>
                    <th data-sort="1" onclick="sortOnclick(this);">Via</th> 
                    <th data-sort="1" onclick="sortOnclick(this);">Remark</th>      
                    <th>LastModify</th>  
                    <th>                        
                    </th>            
                </tr>
                <tr>
                    <th></th>
                    <th><input id="txtFLIGHTNBR" runat="server" type="text" class="wid_100px" data-control="btnUpdate" maxlength="20"
                            data-minlenght="1" /></th>
                    <th><input id="txtREGISTRATION"  runat="server" maxlength="20" data-control="btnUpdate" data-minlenght="1"
                            type="text" class="wid_120px" /></th>
                    <th style="color: black!important;">
                        <input id="ddlFROM_AIRP" type="text" style="color: black;" class="wid_70px" data-autocomplete="AERO" />
                        
                    </th>
                    <th style="color: black!important;">
                        <input runat="server" id="ddlTO_AIRP" type="text" style="color: black;" class="wid_70px" data-autocomplete="AERO" />
                    <th><input id="txtETD" class="wid_50px" data-number="true" runat="server" type="text" data-control="btnUpdate" data-minlenght="1" maxlength="4"/></th>
                    <th><input id="txtETA" class="wid_50px" data-number="true" runat="server" type="text" data-control="btnUpdate" data-minlenght="1" maxlength="4"/></th>
                    <th></th>
                    <th><asp:CheckBox ID="chkDay1" runat="server" /> </th>
                    <th><asp:CheckBox ID="chkDay2" runat="server" /></th>
                    <th><asp:CheckBox ID="chkDay3" runat="server" /></th>
                    <th><asp:CheckBox ID="chkDay4" runat="server" /></th>
                    <th><asp:CheckBox ID="chkDay5" runat="server" /></th>
                    <th><asp:CheckBox ID="chkDay6" runat="server" /></th>
                    <th><asp:CheckBox ID="chkDay7" runat="server" /></th>
                    <th style="color: black!important;">
                        <input runat="server" id="ddlCRAFT_ID" type="text" style="color: black;" class="wid_70px" data-autocomplete="CRAFT" />
                    </th>
                    <th><input id="txtBEGINDATE_SC" onblur="checkInputDate(this)" runat="server" type="text" data-minlenght="1" data-control="btnUpdate"
                            data-date-format="dd/mm/yyyy" class="wid_100px"/></th>
                    <th><input id="txtENDDATE_SC" onblur="checkInputDate(this)" runat="server" type="text" data-minlenght="1" data-control="btnUpdate"
                            data-date-format="dd/mm/yyyy" class="wid_100px"/></th>
                    <th style="color: black!important;">
                        
                        <input runat="server" id="ddlPURPOSE_ID" type="text" style="color: black;" class="wid_60px" data-autocomplete="PURPOSE" />
                    </th>
                    <th><input id="txtVIA"  runat="server" data-minlenght="1" class="wid_125px" data-control="btnUpdate" maxlength="500" type="text" /></th>    
                    <th>
                        <input id="txtREMARK" runat="server" maxlength="100" class="wid_125px" data-control="btnUpdate" data-minlenght="1" type="text" />
                    </th> 
                    <th>
                        <input id="txtLASTUSER" runat="server" class="wid_50px"  disabled="disabled" maxlength="100" type="text" />
                    </th>
                    <th style="white-space: nowrap;background-color:#fff">
                        <div class="action-buttons">
                            <a id="lnkSearch" onclick="btnSearch_Click()">
                                <i class="glyphicon glyphicon glyphicon-search"></i>
                            </a>
                            <a ID="lnkCreate" onclick="btnCreate_Details_Onclick()">
                                <i class="glyphicon glyphicon glyphicon-plus"></i>
                            </a>
                            <a ID="lnkUpdate" onclick="mbtnUpdateFlightDetailOnclick()">
                                <i class="glyphicon glyphicon glyphicon-ok"></i>
                            </a>
                            <a id="lnkCancel" onclick="lnkCancel_Click()">
                                <i class="glyphicon glyphicon glyphicon-remove"></i>
                            </a>
                            <a id="lnkClear" onclick="lnkClear_Click()">
                                <i class="glyphicon glyphicon-trash"></i>
                            </a>
                        </div>
                    </th>               
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptSource" runat="server">
                    <ItemTemplate>
                        <tr data-RowNumber='<%# Eval("rnum") %>'>
                            <td><%# Eval("rnum") %></td>
                            <td><%# Eval("FLIGHTNBR") %></td>
                            <td><%# Eval("REGISTRATION") %></td>
                            <td><%# Eval("FROM_NAME") %></td>
                            <td><%# Eval("TO_NAME") %></td>
                            <td><%# Eval("ETD") %></td>
                            <td><%# Eval("ETA") %></td>
                            <td><input disabled type="checkbox" id="chkDay1" <%# Eval("DAY1").ToString()=="0"?"":"checked" %> /></td>
                            <td><input disabled type="checkbox" id="chkDay2" <%# Eval("DAY2").ToString()=="0"?"":"checked" %> /></td>
                            <td><input disabled type="checkbox" id="chkDay3" <%# Eval("DAY3").ToString()=="0"?"":"checked" %> /></td>
                            <td><input disabled type="checkbox" id="chkDay4" <%# Eval("DAY4").ToString()=="0"?"":"checked" %> /></td>
                            <td><input disabled type="checkbox" id="chkDay5" <%# Eval("DAY5").ToString()=="0"?"":"checked" %> /></td>
                            <td><input disabled type="checkbox" id="chkDay6" <%# Eval("DAY6").ToString()=="0"?"":"checked" %> /></td>
                            <td><input disabled type="checkbox" id="chkDay7" <%# Eval("DAY7").ToString()=="0"?"":"checked" %> /></td>
                            <td><%# Eval("CRAFT_NAME") %></td>
                            <td><%# Eval("BEGINDATE", "{0:dd/MM/yyyy}") %></td>
                            <td><%# Eval("ENDDATE", "{0:dd/MM/yyyy}") %></td>
                            <td><%# Eval("PURPOSE_ID") %></td>
                            <td><%# Eval("VIA") %></td>
                            <td><%# Eval("REMARK") %></td>
                            <td><%# Eval("LASTUSER") %></td>
                            <td style="white-space: nowrap;">
                                <div class="action-buttons" style="width: 70px">
                                                        <a runat="server" data-toggle="tooltip" title="Edit">
                                                            <i class="ace-icon fa fa-pencil bigger-130" onclick="mShowDetail(<%# Eval("ID") %>);"></i>
                                                        </a>
                                                        <a runat="server" data-toggle="tooltip" title="Delete">
                                                            <i class="ace-icon fa fa-trash-o bigger-130" onclick="btnDeleteOnclick(<%# Eval("ID") %>);"></i>
                                                        </a>
                                                        <a data-toggle="tooltip" onclick="LoadShowHisEventClick(this);" class="bigger-140 show-details-btn" title="Show history">
                                                            <i class="ace-icon fa fa-angle-double-down"></i>
                                                        </a>
                                                    </div>
                            </td>
                        </tr>
                        <tr class="detail-row">
                            <td colspan="22" class="text-left">
                                <%# rListHistoryFlightDetails(new PermDetailScDAL().GetHistoryById(Eval("ID").ToString()), Eval("ID").ToString()) %>
                            </td>
                        </tr>  
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <div id="divPaging"></div>
    </div>
    <div id="divSearchExtension" class="modal fade" role="dialog" tabindex="-1" data-backdrop="false" aria-hidden="true">
         <div class="modal-content">
            <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">×</button>
                    <h4 class="blue bigger">Search extension</h4>
                </div>  
             <div class="modal-body">

             </div>  
         </div>
    </div>
    -->
    <section class="permission-flight-details" data-permission-type="SC">
        <div class="permission-flight-details__header">
            <h3 class="permission-flight-details__title">Flight details</h3>
            <span id="permissionFlightDetailsTotalSC" class="permission-flight-details__total">TOTAL: 0</span>
        </div>
        <div class="permission-flight-details__scroll">
            <table id="permissionFlightDetailsTableSC" class="permission-flight-details__table"
                data-atfm-responsive-table="off">
                <thead></thead>
                <tbody></tbody>
            </table>
        </div>
    </section>
    <section class="permission-content-panel">
        <label for="txtPERMCONTENT">Perm content</label>
        <textarea id="txtPERMCONTENT" maxlength="4000" rows="25" data-control="update"
            class="wid_100" data-toggle="tooltip" title="Limit 4000 character !"></textarea>
    </section>
    <script src="<%=Global.ApplicationPath%>/Style/assets/js/jquery-2.1.4.min.js"></script>
    <script src="<%=Global.ApplicationPath%>/Scripts/CustomDynamic.js"></script>
    <%--<script src="<%=Global.ApplicationPath%>/Scripts/CustumStaticdata.js"></script>--%>
    <script src="../Scripts/CustumStaticdata.js"></script>
    <script src="<%=Global.ApplicationPath%>/Scripts/CustomPaging.js"></script>
    <script src="<%= ResolveUrl("~/Scripts/PermissionFlightDetails.js?v=20260814-3") %>"></script>
    <script>

        function LoadShowHisEventClick(){   
            $('.detail-row').attr('style','');
        }
        function LoadShowHisEventClick(ele){
            $(ele).closest('tr').next().toggleClass('open');
            $(ele).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
                
            $('.detail-row').attr('style','');
        }
        var phanCach= '<%= _phanCach%>';
        var isCreatePermMaster=false;
        var IdSelect = <%=_ID%>;  
        if (IdSelect == 0){  
            $('#btnUpdatePermMaster').hide();                       
        }
        
        var pIdCha=0;
        var txtPERMNBR_ID = document.getElementById("txtPERMNBR_ID");
        var ddlAUTHOR_ID = document.getElementById('<%= ddlAUTHOR_ID.ClientID %>');
        var ddlPERMTYPE = document.getElementById('ddlPERMTYPE');
        var txtPERMNBR = document.getElementById('txtPERMNBR');
        var txtVERSION = document.getElementById('txtVERSION');
        var txtPERMDATE = document.getElementById('txtPERMDATE');
        var ddlOPER_ID = document.getElementById('ddlOPER_ID');
        var txtREFERENCE = document.getElementById('txtREFERENCE');
        var txtVALIDHOURS = document.getElementById('txtVALIDHOURS');
        var ddlSEASON = document.getElementById('ddlSEASON');
        var txtBEGINDATE = document.getElementById('txtBEGINDATE');
        var txtENDDATE = document.getElementById('txtENDDATE');
        var btnUpdate = document.getElementById('btnUpdate');
        var btnCreate = document.getElementById('btnCreate');
        var btnNext = document.getElementById('btnNext');
        var btnPrev = document.getElementById('btnPrev');
        var txtBILLINGADDRESS = document.getElementById('txtBILLINGADDRESS');
        var txtPERMCONTENT = document.getElementById('txtPERMCONTENT');
        var isLoadSussess = false;
        function DisplayResult(resulf, context) {
            if(context=='LoadDataGrid'){
                $('#tblSource tbody tr').remove();
                $('#tblSource tbody').append(resulf);
                LoadShowHisEventClick();
                isLoadSussess = true;
                unLoadingData();
                listfile();
            }
            if(context == 'btnSearch_Click'){
                $('#tblSource tbody tr').remove();
                $('#tblSource tbody').append(resulf);
                LoadShowHisEventClick();
                isLoadSussess = true;
                unLoadingData();
            }
            if (resulf == '') return;
            if (context == 'GetOneFlight') {
                ReadInfoPerm(resulf);               
            }           
            if (context == 'btnUpdateOnclick') {
                alert(resulf);
            }
            if (context == 'btnCreateOnclick') {
                if (resulf != '-1' && resulf != '-99') {
                    //alert('Insert sussess!');
                    txtPERMNBR_ID.value =createPermNBRID(ddlAUTHOR_ID.value, ddlPERMTYPE.value, txtPERMNBR.value, txtPERMDATE.value, ddlSEASON.value);
                    document.getElementById('perm_id').innerHTML=resulf;    
                    if(isCreatePermMaster){                        
                        btnUpdateList_Onclick();
                        //btnCreate_Details_Onclick();
                        isCreatePermMaster=false;
                    }

                } else alert('Insert error!');
                
            }
            if (context == 'btnCreate_Details_Onclick') {
                if (resulf != '-1' && resulf != '-99') {
                    alert('Insert sussess!');
                    ClearValue();
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
                    lnkCancel_Click();
                    LoadDataGrid();
                }
            }
            if (context == 'btnDeleteOnclick') {
                alert(resulf);
                LoadDataAjax();
            }
            if (context == 'RestoreHistory') {
                if (resulf != 'NOK') {
                    alert('Restore sussess!');
                    LoadDataGrid();
                } else alert('Restore error!');
            }
            if (context == 'LoadGrdSourceScroll') {
                $('#tblSource tr').last().after(resulf).fadeIn();
                LoadShowHisEventClick();
                isLoadSussess = true;
                unLoadingData();
            }
            if(context=='uploadComplete'){
                if(resulf=='Upload error!')
                    alert(resulf);
                else{
                    $('#lblLinkFile').html(resulf);
                    //unLoadingData();
                }     
            }
            if(context=='GetListFilePerm'){
                $('#lblLinkFile').html(resulf);
            }
            if(context=='deleteFile'){
                alert(resulf);
                GetListFilePerm($('#perm_id').html());
            }
        }
        function ReadInfoPerm(data) {
            var obj = JSON.parse(data.replace(/\n/gi,'<br>').replace(/\\/gi,'\\\\').replace(/\t/gi, '     '));
            //var obj = JSON.parse(data);
            txtPERMDATE.value = obj['PERMDATE'] == null ? null : obj['PERMDATE']['DateTime'].replace(/\//g,'-');
            txtPERMNBR_ID.value = obj['PERMNBR_ID'];
            txtREFERENCE.value = obj['REFERENCE']==null?null:obj['REFERENCE'].replace(/<br>/gi, '\r\n');
            txtVALIDHOURS.value = obj['VALIDHOURS'];
            txtPERMNBR.value = obj['PERMNBR'];
            txtVERSION.value = obj['VERSION'];
            setSelectedValue(ddlAUTHOR_ID.id, obj['AUTHOR_ID']);
            ddlOPER_ID.value= obj['OPER_ID'];
            setSelectedValue(ddlPERMTYPE.id, obj['PERMTYPE']);
            txtBEGINDATE.value = obj['BEGINDATE'] == null ? null : obj['BEGINDATE']['DateTime'];
            txtENDDATE.value = obj['ENDDATE'] == null ? null : obj['ENDDATE']['DateTime'];
            setSelectedValue(ddlSEASON.id, obj['SEASON']);
            txtBILLINGADDRESS.value = obj['BILLINGADDRESS']==null?null: obj['BILLINGADDRESS'].replace(/<br>/gi, '\r\n');
            txtPERMCONTENT.value = obj['PERMCONTENT'] == null ? null: obj['PERMCONTENT'].replace(/<br>/gi, '\r\n');
            $('#lblLinkFile').html(obj['LinkFiles']);
        }
        function ReadObj() {
            var obj = {
                ID: $('#perm_id').html(),
                PERMNBR_ID: createPermNBRID(ddlAUTHOR_ID.value, ddlPERMTYPE.value, txtPERMNBR.value, txtPERMDATE.value, ddlSEASON.value),
                AUTHOR_ID: ddlAUTHOR_ID.value,
                PERMTYPE: ddlPERMTYPE.value,
                PERMDATE: txtPERMDATE.value.replace(/-/g,'/'),
                PERMNBR: txtPERMNBR.value,
                VERSION: txtVERSION.value,
                OPER_ID: ddlOPER_ID.value,
                REFERENCE: txtREFERENCE.value,
                VALIDHOURS: txtVALIDHOURS.value,
                SEASON: ddlSEASON.value,
                BEGINDATE: txtBEGINDATE.value,
                ENDDATE: txtENDDATE.value,
                BILLINGADDRESS: txtBILLINGADDRESS.value,
                PERMCONTENT : txtPERMCONTENT.value
            };
            return obj;
        }
        function btnUpdateOnclick() {
            if ($('#perm_id').html()!='') {
                if (checkValidCustomMinlenght('update')) {
                    GetArgWithPostBack(JSON.stringify(ReadObj()) + '_____btnUpdateOnclick', 'btnUpdateOnclick');
                }
                else alert('Check validate!');
            }
            else {
                alert('Please select Flight');
                return;
            }
        }
        function btnCreateOnclick() {
            if($('#perm_id').html()=='')
                isCreatePermMaster=true;
            if(checkValidCustomMinlenght('update')){
                GetArgWithPostBack(JSON.stringify(ReadObj()) + '_____btnCreateOnclick', 'btnCreateOnclick');
            }
        }

        function uploadComplete(){
            if($('#perm_id').html()=='') return;
            //var result = confirm("Do you want Upload File?");
            //if(result)
            GetArgWithPostBack($('#perm_id').html() + "_____uploadComplete","uploadComplete");
            //else return;
        }
        function checkUpload(){
            return;
        }
        function createPermNBRID(au, typ, nbr, ye, ses) {
            var y = '';
            if (ye == null || ye == '')
                y = new Date().format('dd/mm/yyyy');
            return typ + ' ' + LPAD(nbr, 5, '0') + '/' + ses + '/' + au + '/' + ye.replace(/-/g,'/').split('/')[2];
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
    </script>


    <script>
        var idIndetiny = 1;
        var numberRowDelete = 0;
        var iRowAdd = 1; // 0 - no add; 1 add row defaulf
        var iRowSelect = '';
                
        var mbtnAddNewFlightDetail= document.getElementById('btnCreateSC');
        var mbtnUpdateFlightDetail = document.getElementById('btnUpdateSC');

        var IdSelectDT = 0;
        var _objRender = JSON.parse('<%= _ObjRender%>');
        var txtBEGINDATE_SC = document.getElementById('<%= txtBEGINDATE_SC.ClientID%>');
        var txtENDDATE_SC = document.getElementById('<%= txtENDDATE_SC.ClientID%>');
        //var txtPERM_ID = document.getElementById('txtPERM_ID');
        //txtPERM_ID.value = '<%= NAMECHA %>';
        
        var ddlCRAFT_ID = document.getElementById('<%=ddlCRAFT_ID.ClientID%>');
        
        var chkDAY1 = document.getElementById('<%=chkDay1.ClientID%>');
        var chkDAY2 = document.getElementById('<%=chkDay2.ClientID%>');
        var chkDAY3 = document.getElementById('<%=chkDay3.ClientID%>');
        var chkDAY4 = document.getElementById('<%=chkDay4.ClientID%>');
        var chkDAY5 = document.getElementById('<%=chkDay5.ClientID%>');
        var chkDAY6 = document.getElementById('<%=chkDay6.ClientID%>');
        var chkDAY7 = document.getElementById('<%=chkDay7.ClientID%>');
        var txtREMARK = document.getElementById('<%=txtREMARK.ClientID%>');
        var txtREGISTRATION = document.getElementById('<%=txtREGISTRATION.ClientID%>');
        var txtFLIGHTNBR = document.getElementById('<%= txtFLIGHTNBR.ClientID%>');
        var ddlTO_AIRP = document.getElementById('<%=ddlTO_AIRP.ClientID%>');
        var ddlFROM_AIRP = document.getElementById('ddlFROM_AIRP');
        var txtETD = document.getElementById('<%=txtETD.ClientID%>');
        var ddlPURPOSE_ID = document.getElementById('<%=ddlPURPOSE_ID.ClientID%>');
        var txtETA = document.getElementById('<%=txtETA.ClientID%>');
        var txtVIA = document.getElementById('<%=txtVIA.ClientID%>');
        //var txtSTATUS = document.getElementById('txtSTATUS');
        var txtLASTUSER = document.getElementById('<%=txtLASTUSER.ClientID%>');
        
        
        function ReadInfoFlightDetail(data) {
            var obj = JSON.parse(data);           
            txtBEGINDATE_SC.value = obj['BEGINDATE'] == null ? null : obj['BEGINDATE']['DateTime'];
            chkDAY1.checked = obj['DAY1'] == '0' ? false : true;
            chkDAY2.checked = obj['DAY2'] == '0' ? false : true;
            chkDAY3.checked = obj['DAY3'] == '0' ? false : true;
            chkDAY4.checked = obj['DAY4'] == '0' ? false : true;
            chkDAY5.checked = obj['DAY5'] == '0' ? false : true;
            chkDAY6.checked = obj['DAY6'] == '0' ? false : true;
            chkDAY7.checked = obj['DAY7'] == '0' ? false : true;
            txtENDDATE_SC.value = obj['ENDDATE']['DateTime'];            
            
            setSelectedValue(ddlCRAFT_ID.id, obj['CRAFT_ID']);            
            txtREMARK.value = obj['REMARK'];
            txtREGISTRATION.value = obj['REGISTRATION'];
            txtFLIGHTNBR.value = obj['FLIGHTNBR'];
            setSelectedValue(ddlTO_AIRP.id, obj['TO_AIRP']);
            setSelectedValue(ddlFROM_AIRP.id, obj['FROM_AIRP']);
            txtETD.value = obj['ETD'];
            setSelectedValue(ddlPURPOSE_ID.id, obj['PURPOSE_ID']);
            txtETA.value = obj['ETA'];
            txtVIA.value = obj['VIA'];
            txtSTATUS.value = obj['STATUS'];
            txtLASTUSER.value = obj['LASTUSER'];
            checkCustomValidate();
        }
        function GetObjectInfo() {
            var _obj = _objRender;
            _obj['PERM_ID'] = $('#perm_id').html();
            _obj['ETA'] = txtETA.value;
            _obj['ETD'] = txtETD.value;            
            _obj['CRAFT_ID'] = ddlCRAFT_ID.value;
            _obj['FLIGHTNBR'] = txtFLIGHTNBR.value;
            _obj['PURPOSE_ID'] = ddlPURPOSE_ID.value;
            _obj['DAY1'] = chkDAY1.checked == true ? '1' : '0';
            _obj['DAY2'] = chkDAY2.checked == true ? '2' : '0';
            _obj['DAY3'] = chkDAY3.checked == true ? '3' : '0';
            _obj['DAY4'] = chkDAY4.checked == true ? '4' : '0';
            _obj['DAY5'] = chkDAY5.checked == true ? '5' : '0';
            _obj['DAY6'] = chkDAY6.checked == true ? '6' : '0';
            _obj['DAY7'] = chkDAY7.checked == true ? '7' : '0';
            _obj['FROM_AIRP'] = ddlFROM_AIRP.value;
            _obj['TO_AIRP'] = ddlTO_AIRP.value;
            
            _obj['REGISTRATION'] = txtREGISTRATION.value;
            _obj['VIA'] = txtVIA.value;
            _obj['REMARK'] = txtREMARK.value;
            _obj['BEGINDATE'] = txtBEGINDATE_SC.value;
            _obj['ENDDATE'] = txtENDDATE_SC.value;
            _obj['LASTUSER'] = '<%= _user.UserName.ToString()%>';
            //        if($('#tblSource tbody tr').length<1){
            //            _obj['RSTART']=1;
            //            _obj['RFINISH']=appenLoad;
            //        }else{
            //            _obj['RSTART'] = isSroll ? (parseInt($('#tblSource tbody tr[data-RowNumber]').last().attr('data-RowNumber'))
            //+ 1) : 0;
            //            _obj['RFINISH'] = isSroll ? (parseInt($('#tblSource tbody tr[data-RowNumber]').last().attr('data-RowNumber'))
            //+ appenLoad) : appenLoad;
            //        }            
            _obj['ID'] = IdSelectDT;
            //_obj['RSTART']= 1;
            //_obj['RFINISH']=100;
            //_obj['RSTART']= (parseInt($('#divPaging').attr('data-pageIndex'))-1)*parseInt($('#divPaging').attr('data-pageSize'));
            //_obj['RFINISH']= parseInt(_obj['RSTART'])+parseInt($('#divPaging').attr('data-pageSize'));
            if($('#tblSource').attr('data-pageSize')==null)$('#tblSource').attr('data-pageSize',100);
            if($('#tblSource').attr('data-pageIndex')==null)$('#tblSource').attr('data-pageIndex',1);
            _obj['RSTART']=  (parseInt($('#tblSource').attr('data-pageIndex'))-1)*parseInt($('#tblSource').attr('data-pageSize'));
            _obj['RFINISH']= parseInt(_obj['RSTART'])+parseInt($('#tblSource').attr('data-pageSize'));
            return _obj;
        }      
        
        function deleteFile(id){
            var cf = confirm('Do you want delete?');
            if(cf)
                GetArgWithPostBack(id +'_____deleteFile','deleteFile');
        }
        function lnkClear_Click(){
            ClearValue();
        }
        function ClearValue() {
            var valueFields = [
                txtBEGINDATE_SC, txtENDDATE_SC, txtETA, txtETD,
                txtFLIGHTNBR, txtREGISTRATION, txtREMARK, txtVIA, txtLASTUSER
            ];
            $.each(valueFields, function (_, field) {
                if (field) field.value = '';
            });
            
            //ddlCRAFT_ID.value='';
            //ddlFROM_AIRP.value='';
            //ddlPURPOSE_ID.selectedIndex = 0;
            //ddlTO_AIRP.selectedIndex = 0;
            $.each([chkDAY1, chkDAY2, chkDAY3, chkDAY4, chkDAY5, chkDAY6, chkDAY7], function (_, checkbox) {
                if (checkbox) checkbox.checked = false;
            });
            IdSelectDT = '0';
        }
        function CRAFT_ID_Onchange() {
            document.getElementById('txtMTOW').value = parseInt(ddlCRAFT_ID.options[ddlCRAFT_ID.selectedIndex].getAttribute('data-taitrong'));
        }         
        function btnCreate_Details_Onclick() {
            if($('#perm_id').html()!='')
                GetArgWithPostBack(JSON.stringify(GetObjectInfo()) + '_____btnCreate_Details_Onclick',
        'btnCreate_Details_Onclick');
            else{
                GetArgWithPostBack(JSON.stringify(ReadObj()) + '_____btnCreateOnclick', 'btnCreateOnclick');
                isCreatePermMaster = true;
            }
        }
        function mReadInfoFlightDetail(data) {
            var obj = JSON.parse(data);
            txtBEGINDATE_SC.value =  obj['BEGINDATE'] == null ? null : obj['BEGINDATE']['DateTime'];
            chkDAY1.checked = obj['DAY1'] == '0' ? false : true;
            chkDAY2.checked = obj['DAY2'] == '0' ? false : true;
            chkDAY3.checked = obj['DAY3'] == '0' ? false : true;
            chkDAY4.checked = obj['DAY4'] == '0' ? false : true;
            chkDAY5.checked = obj['DAY5'] == '0' ? false : true;
            chkDAY6.checked = obj['DAY6'] == '0' ? false : true;
            chkDAY7.checked = obj['DAY7'] == '0' ? false : true;
            txtENDDATE_SC.value =  obj['ENDDATE'] == null ? null : obj['ENDDATE']['DateTime'];      
            
            setSelectedValue(ddlCRAFT_ID.id, obj['CRAFT_ID']);
            
            txtREMARK.value = obj['REMARK'];
            txtREGISTRATION.value = obj['REGISTRATION'];
            txtFLIGHTNBR.value = obj['FLIGHTNBR'];
            setSelectedValue(ddlTO_AIRP.id, obj['TO_AIRP']);
            setSelectedValue(ddlFROM_AIRP.id, obj['FROM_AIRP']);
            txtETD.value = obj['ETD'];
            setSelectedValue(ddlPURPOSE_ID.id, obj['PURPOSE_ID']);
            txtETA.value = obj['ETA'];
            txtVIA.value = obj['VIA'];
            txtLASTUSER.value=obj['LASTUSER'];
        }   
        function RestoreHistory(id, ver, UserID) {
            GetArgWithPostBack(id + phanCach + ver + phanCach + UserID + '_____RestoreHistory', 'RestoreHistory');
        }
        function mShowDetail(id) {
            IdSelectDT = id;
            iRowSelect = 'tr' + id;   
            lnkCreate.hide();
            lnkUpdate.show();
            lnkSearch.hide();
            lnkClear.hide();
            lnkCancel.show();
            //mbtnUpdateFlightDetail.removeAttribute('style');
            //mbtnAddNewFlightDetail.setAttribute('style', 'display: none');
           
            GetArgWithPostBack(id + '_____mShowDetail', 'mShowDetail');
        }
        function btnDeleteOnclick(id) {
            var result = confirm("Do you want delete?");
            if (result)
                GetArgWithPostBack(id + '_____btnDeleteOnclick', 'btnDeleteOnclick');
        }
       
        
        function LoadDataGrid() {
            GetArgWithPostBack(JSON.stringify(GetObjectInfo())+'_____LoadDataGrid', 'LoadDataGrid');
           
            loadingData();
        }
       
        function mbtnUpdateFlightDetailOnclick() {
            //if (checkValidCustomMinlenght('btnUpdate'))
            // {
            //  mbtnAddNewFlightDetail.removeAttribute('style');
            //  mbtnUpdateFlightDetail.setAttribute('style', 'display: none');
            GetArgWithPostBack(JSON.stringify(GetObjectInfo()) + '_____mbtnUpdateFlightDetailOnclick', 'mbtnUpdateFlightDetailOnclick');
            //}
               
        }

        function checkYearPermDate(id){
            //if($(id).val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3') != new Date().getFullYear()){
                
            //    alert('Check year date perm: ' + $(id).val());
            //    $(id).val('');
                
            //}
        }
        
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
        lnkCancel_Click();
        function lnkCancel_Click(){
            lnkClear_Click();
            lnkCreate.show();
            lnkClear.show()
            lnkUpdate.hide();
            lnkCancel.hide();
            lnkSearch.show();
        }
        function btnSearch_Click(){            
            //GetArgWithPostBack(JSON.stringify(GetObjectInfo()) + '_____btnSearch_Click', 'btnSearch_Click');
            $('#tblSource').attr('data-pageindex', '1');
            LoadDataAjax();
        }
        function fomatDateTimeCustom(ele){
            //$ele = $(ele);
            //if($ele.val().length<8)
            //    $ele.focus();
        }
        function checkDateOnblur(ele){
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
        //window.onload= LoadDataGrid;
    </script>
    <script>
        var isSroll=false;
        var appenLoad= 500;
        $(window).scroll(function () { //detact scroll
            if ($(window).scrollTop() + $(window).height() >= $(document).height())
            { //scrolled to bottom of the page
                isSroll = true;
                LoadGrdSourceScroll();
                isSroll = false;
            }
        });
        function LoadGrdSourceScroll() {
            if (isSroll && isLoadSussess){
                var tt = parseInt($('#lblTotalRecord').text().split(':')[1].trim());
                var cr = parseInt($('#tblSource tbody tr[data-RowNumber]').last().attr('data-RowNumber'));
                if(cr>=tt)
                    return;
                isLoadSussess=false;
                GetArgWithPostBack(JSON.stringify(GetObjectInfo()) + '_____'
                + 'LoadGrdSourceScroll', 'LoadGrdSourceScroll');
                loadingData();
            }  
        }        
    </script>
    
    <script>
        
        
        function prelodingUploadFile() {
            preloadImg('lblLinkFile', 'waitingLoadFile', '15px', '15px');
        }
        function loadingData() {
            preloadImg('tblSource', 'waitingLoadData', '30px', '30px');
        }
        function unLoadingData(){
            $('#waitingLoadData').remove();
        }
        
        function GetListFilePerm(permid){
            GetArgWithPostBack(permid+'_____GetListFilePerm','GetListFilePerm');
        }
        var listfile = function(){
            GetListFilePerm($('#perm_id').html());
        };
        $(document).ready(function(){
            //LoadDataGrid();            
        });
        
    </script>
    
    <script>
        //var _urlAPI = 'https://192.168.63.21:8082/';
        //var _urlAPI = 'http://192.168.63.21/';
        var rowId = 0;
        var inputId = '';
        var qEdit = '<%= _Role.R_Edit %>';
        var qDel = '<%= _Role.R_Del %>';
        var dt = ReadObj();
        function GetHisById(_url, _id, user){
            var rHis ='';
            try{
                $.ajax({
                    method: "GET",
                    url: _url + _id,
                }).done(function(data){
                    rHis+="<table class=\"table\">";
                    rHis += "<thead>";
                    rHis += "<tr>";
                    rHis +="<th>#</th>";                //colum 1
                    rHis +="<th>No</th>";               //colum 2
                    rHis +="<th>Last user</th>";        //colum 3
                    rHis +="<th>Last modify</th>";      //colum 4
                    rHis +="<th>Conten change</th>";    //colum 5
                    rHis +="<th>Action</th>";           //colum 6
                    rHis += "</tr>";
                    rHis += "</thead>";
                    rHis += "<tbody>";
                    $.each(data.ListValue, function(c,d){
                        rHis += "<tr>";
                        rHis += "<td>";
                        rHis += "<div class=\"action-buttons\">";
                        rHis += "<a data-toggle=\"tooltip\" title=\"Restore\">";
                        rHis += "<i class=\"glyphicon glyphicon-refresh bigger-130\"";
                        rHis +=" onclick=\"RestoreHistory(" + _id + ", " +d.NOVERSION + ",'" + user+ "');\"></i>";
                        rHis += "</a>";
                        rHis += "</div>";
                        rHis += "</td>";
                        rHis +="<td>" +"1"+ "</td>";
                        rHis +="<td>" + d.LASTUSER + "</td>";
                        rHis +="<td>" + new Date(d.LASTMODIFY).format("dd/MM/yyyy hh:mm:ss")+ "</td>";
                        rHis +="<td>" + d.CONTENT + "</td>";
                        rHis +="<td>" + d.ACTION + "</td>";
                        rHis += "</tr>";
                    })
                    rHis += "</tbody>";
                    rHis += "</table>";
                
                });
            }catch(ex){
                return '';
            }
            return rHis;
        }
        $('#tblSource>tbody input').focusout(function () {
            tickUpdate('#'+$(this).prop('id'));
        });
        function tickUpdate(id){
            var $old = $(id).attr('data-oldValue');
            if($(id).val().toUpperCase()!= $old.toUpperCase()){                
                $(id).closest('tr').attr('data-isUpdate', 'true');
            }
            $(id).val($(id).val().toUpperCase());
        }
        function loadScript(url) {
            var js = document.getElementById("sandboxScript");
            if(js !== null) {
                document.body.removeChild(js);
            }
            js = document.createElement("script");
            js.src = url;
            js.id = "sandboxScript";
            document.body.appendChild(js);
        }
        function binAutocomplete(id, v){
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
                    //case "AERO":
                    //    $(id).autocomplete({
                    //        source: [listAero],
                    //    }).on('blur', function (e, datum) {
                    //        CheckListAero($(id));
                    //    });
                    //    break;
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
        function returnEmpty(val){
            return val==null? "":val;
        }
        function reloadCheckValid(){
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
                    $(this).on('blur', function(){
                        checkInputDate1($(this));
                    } );
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
                v = $ele.val().replace(/^(\d{2})(\d{2})(\d{4})$/, '$1-$2-$3');
            if (v.length > 10 || v.length == 9) {
                $ele.css('border', '1px solid red');
                $ele.val('');
                return;
            }
            if (v.length == 10) {                
                var d = v.replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$1');
                var m = v.replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$2');
                var y = v.replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3');
                var chk = isValidDate(v);
                if (!chk) {
                    $ele.css('border', '1px solid red');
                    $ele.val('');
                }
                else {
                    $ele.css('border', '');
                    // $ele.val(new Date(y,m,d).format('dd-mm-yyyy'));
                }
            }
        }
        function checkPermNumber() {
            $('#divListNumber').html('');
            var $request = $.ajax({
                //async: false,
                method: "PUT",
                url: urlApi + "api/ApiExtension/ExcuteTable?packageName=PERM_PKG&storeName=validFlightNbr",
                data: JSON.stringify({ P_FLIGHT_TYPE: 'SC', P_FLIGHTNBR: $('#txtPERMNBR').val().toUpperCase() }),
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
        function LoadDataAjax(){
            if ($('#perm_id').html() == '') return;
            if ($('#perm_id').html() != '') $('#btnUpdatePermMaster').show();
            var kq ='';
            $.ajax({
                //async: false,
                method: "PUT",
                url: urlApi + "api/PermDetailSc/GetBySearch",                
                data: GetObjectInfo(),
                beforeSend: function(){
                    $('#tblSource tbody tr').remove();
                    $('#tblSource').attr('data-total',0);
                    loadingData();
                },
                complete: function(){
                    unLoadingData();
                    reloadCheckValid();
                    $('#tblSource').paging({onClickButton: 'LoadDataAjax'
                    });                    
                    $('#tblSource input[data-control="_updateAll"]').each(function(a, b){
                        $(b).mouseover(function(){
                            onmouseoverInput(b.id)
                        });
                        $(b).mouseout(function(){
                            onmouseoutInput(b.id)
                        });
                        $(b).ValidateTip();
                    })
                },
            }).always(function(data) {
                if(data.ListValue==null){
                    unLoadingData();
                    return;
                }
                $('#tblSource tbody tr').remove();    
                $('#tblSource').attr('data-total', data.ListValue[0]['Record_Sum']);
                var strAppend = '';
                $.each(data.ListValue, function(a,b){
                    var _sttr = "";
                    switch(b.STATUS){
                        case "1":
                            _sttr="style=\"background-color: green!important;\"";
                            break;
                        case "2":
                            _sttr="style=\"background-color: blue!important;\"";
                            break;
                        default:
                            _sttr="";
                            break;
                    }
                    strAppend = strAppend +"<tr "+ _sttr +" id='"+b.ID+"' class='"+ ((new Date().getTime() -new Date(b.ENDDATE).getTime())>0? "cssHetHan":"") +"' data-isUpdate='false' onmouseover='rowId="+b.ID+";' onmouseout='rowId=0;'>"
                    //kq += "<tr data-isUpdate='false'>"
                    +  "<td id='b_"+b.RNUM+"'>" +  b.RNUM + "</td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='4' maxlength='8' class='sInput' id='txtFLIGHTNBR"+a+"' data-oldValue='"+returnEmpty(b.FLIGHTNBR)+"' value='"+returnEmpty(b.FLIGHTNBR)+"' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtREGISTRATION"+a+"' data-oldValue='"+returnEmpty(b.REGISTRATION)+"' value='"+returnEmpty(b.REGISTRATION)+"' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='2'  class='sInput' id='txtFROM_AIRP"+a+"' data-oldValue='"+b.FROM_AIRP+"' data-autocomplete=\'AERO\' value='"+b.FROM_AIRP+"' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' id='txtTO_AIRP"+a+"' data-oldValue='"+b.TO_AIRP+"' data-autocomplete=\'AERO\' value='"+b.TO_AIRP+"' onblur='checkIsUpdate(this)'/></td>" 
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtETD"+a+"' data-oldValue='"+returnEmpty(b.ETD)+"' data-number='true' value='"+returnEmpty(b.ETD)+"' onblur='checkIsUpdate(this)'/></td>"   
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtETA"+a+"' data-oldValue='"+returnEmpty(b.ETA)+"' data-number='true' value='"+returnEmpty(b.ETA)+"' onblur='checkIsUpdate(this)'/></td>"  
                    + "<td style='background-color: antiquewhite!important;'><input id=\"chkAll"+ b.RNUM +"\" type=\"checkbox\" onchange='checkAllChange(\""+b.RNUM+"\");' /></td>"
                    + "<td><input id=\"chk"+ b.RNUM +"1\" type=\"checkbox\" " + (b.DAY1=="0"? "\"\"": "checked")+" data-oldValue='"+(b.DAY1=='0'?'0':'1')+"' onchange='checkIsUpdate(this)' /></td>"
                    + "<td><input id=\"chk"+ b.RNUM +"2\" type=\"checkbox\" " + (b.DAY2=="0"? "\"\"": "checked")+" data-oldValue='"+(b.DAY2 =='0'?'0':'1')+"' onchange='checkIsUpdate(this)' /></td>"
                    + "<td><input id=\"chk"+ b.RNUM +"3\" type=\"checkbox\" " + (b.DAY3=="0"? "\"\"": "checked")+" data-oldValue='"+(b.DAY3=='0'?'0':'1')+"' onchange='checkIsUpdate(this)' /></td>"
                    + "<td><input id=\"chk"+ b.RNUM +"4\" type=\"checkbox\" " + (b.DAY4=="0"? "\"\"": "checked")+" data-oldValue='"+(b.DAY4=='0'?'0':'1')+"' onchange='checkIsUpdate(this)' /></td>"
                    + "<td><input id=\"chk"+ b.RNUM +"5\" type=\"checkbox\" " + (b.DAY5=="0"? "\"\"": "checked")+" data-oldValue='"+(b.DAY5=='0'?'0':'1')+"' onchange='checkIsUpdate(this)' /></td>"
                    + "<td><input id=\"chk"+ b.RNUM +"6\" type=\"checkbox\" " + (b.DAY6=="0"? "\"\"": "checked")+" data-oldValue='"+(b.DAY6=='0'?'0':'1')+"' onchange='checkIsUpdate(this)' /></td>"
                    + "<td><input id=\"chk"+ b.RNUM +"7\" type=\"checkbox\" " + (b.DAY7=="0"? "\"\"": "checked")+" data-oldValue='"+(b.DAY7=='0'?'0':'1')+"' onchange='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' class='sInput' id='txtCRAFT_NAME"+a+"' data-craftid='"+b.CRAFT_ID+"' data-oldValue='"+b.CRAFT_NAME+"' onfocusin='binAutocomplete(this,\"CRAFT\")' value='"+b.CRAFT_NAME+"' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' data-CheckDate='true' class='sInput' id='txtBEGINDATE"+a+"' data-oldValue='"+new Date(b.BEGINDATE).format('dd-mm-yyyy')+"' value='"+new Date(b.BEGINDATE).format('dd-mm-yyyy') +"' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' data-CheckDate='true' class='sInput' id='txtENDDATE"+a+"' data-oldValue='"+new Date(b.ENDDATE).format('dd-mm-yyyy')+"' value='"+new Date(b.ENDDATE).format('dd-mm-yyyy') +"' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' class='sInput' id='txtPURPOSE_ID"+a+"' data-oldValue='"+b.PURPOSE_ID+"' onfocusin='binAutocomplete(this,\"PURPOSE\")' value='"+b.PURPOSE_ID+"' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtVIA"+a+"' data-oldValue='"+returnEmpty(b.VIA)+"' value='"+returnEmpty(b.VIA)+"' onblur='checkIsUpdate(this)'/></td>" 
                    + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtREMARK"+a+"' data-oldValue='"+returnEmpty(b.REMARK)+"' value='"+returnEmpty(b.REMARK)+"' onblur='checkIsUpdate(this)'/></td>"
                    + "<td>" +  b.LASTUSER + "</td>"
                    + "<td style=\"white-space: nowrap;>\"" 
                                + "<div class=\"action-buttons\">"
                                + (qEdit=="True"?"<a data-toggle=\"tooltip\" title=\"Edit\">"
                                + "<i class=\"ace-icon fa fa-pencil bigger-130\" onclick=\"mShowDetail("+ b.ID+");\"></i></a>":"")
                                + (qDel=="True"?"<a data-toggle=\"tooltip\" title=\"Delete\">"
                                + "<i id=\"btnDelete_"+b.ID+"\" class=\"ace-icon fa fa-trash-o bigger-130\" onclick=\"btnDeleteOnclick("+b.ID+");\"></i></a>":"")
                                + "<a data-toggle=\"tooltip\" onclick=\"LoadShowHisEventClick(this);\" class=\"bigger-140 show-details-btn\" title=\"Show history\"><i class=\"ace-icon fa fa-angle-double-down\"></i></a>"
                                + "</td>"
                    + "</tr>"
                   <%--+ "<tr>"
                   + "</tr>";
                   +GetHisById("<%=System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]%>api/PermHistory/GetByIdPermDetailScBk/", b.ID, 'admin')
                    +"</tr>";--%>
               });
               $('#tblSource tbody').append(strAppend);
           });   
            
        }
        
        function checkIsUpdate(id){
            if($(id).attr('data-isInsert')!= undefined)return;
            $inputs = $(id).closest('tr').find('[data-oldValue]');
            var ci=0;
            $.each($inputs, function(a,b){
                switch ($(b).attr('type')) {
                    case 'text':
                        if($(b).attr('data-oldValue')!=$(b).val()) {ci++;};
                        break;
                    case 'checkbox':
                        if($(b).attr('data-oldValue')!=($(b).prop('checked')?'1':'0')) ci++;
                        break;
                }
            })
            if(ci>0){$(id).closest('tr').attr('data-isUpdate', 'true');$(id).closest('tr').addClass('success');}
            else {$(id).closest('tr').attr('data-isUpdate', 'false');$(id).closest('tr').removeClass('success');}
            
        }
        function btnUpdateList_Onclick(){
            if($('#perm_id').html()==''){
                btnCreateOnclick();
                return;
            }
            if(!checkValidCustomMinlenght('_updateAll')){
                return;
            }
            var $lis = $('tr[data-isUpdate="true"]');
            var $lisIns = $('tr[data-isinsert="true"]');
            if($lis.length == 0 && $lisIns.length==0) {alert('No update.'); return;};
            var c=0;
            $.each($lis, function(a,b){
                var _obj={};
                _obj['PERM_ID'] = $('#perm_id').html();
                _obj['ETA'] = $($(b).find('[id^="txtETA"]')[0]).val();
                _obj['ETD'] = $($(b).find('[id^="txtETD"]')[0]).val();        
                _obj['CRAFT_ID'] = $($(b).find('[id^="txtCRAFT_NAME"]')[0]).attr('data-craftid');
                _obj['FLIGHTNBR'] =  $($(b).find('[id^="txtFLIGHTNBR"]')[0]).val();
                _obj['PURPOSE_ID'] =  $($(b).find('[id^="txtPURPOSE_ID"]')[0]).val();
                _obj['DAY1'] = $($(b).find('[id^="chk"]')[1]).prop('checked') == true ? '1' : '0';
                _obj['DAY2'] = $($(b).find('[id^="chk"]')[2]).prop('checked') == true ? '2' : '0';
                _obj['DAY3'] = $($(b).find('[id^="chk"]')[3]).prop('checked') == true ? '3' : '0';
                _obj['DAY4'] = $($(b).find('[id^="chk"]')[4]).prop('checked') == true ? '4' : '0';
                _obj['DAY5'] = $($(b).find('[id^="chk"]')[5]).prop('checked') == true ? '5' : '0';
                _obj['DAY6'] = $($(b).find('[id^="chk"]')[6]).prop('checked') == true ? '6' : '0';
                _obj['DAY7'] = $($(b).find('[id^="chk"]')[7]).prop('checked') == true ? '7' : '0';
                _obj['FROM_AIRP'] =  $($(b).find('[id^="txtFROM_AIRP"]')[0]).val();
                _obj['TO_AIRP'] =  $($(b).find('[id^="txtTO_AIRP"]')[0]).val();                
                _obj['REGISTRATION'] =  $($(b).find('[id^="txtREGISTRATION"]')[0]).val();
                _obj['VIA'] =  $($(b).find('[id^="txtVIA"]')[0]).val();
                _obj['REMARK'] =  $($(b).find('[id^="txtREMARK"]')[0]).val();
                _obj['BEGINDATE'] = new Date($($(b).find('[id^="txtBEGINDATE"]')[0]).val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1')).format('yyyy-mm-dd');
                _obj['ENDDATE'] =  new Date($($(b).find('[id^="txtENDDATE"]')[0]).val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1')).format('yyyy-mm-dd');
                _obj['LASTUSER'] = '<%= _user.UserName.ToString()%>';            
                _obj['ID'] = $(b).prop('id');
                var $request = $.ajax({
                    async: false,
                    method: "PUT",
                    url: urlApi + "api/PermDetailSc/UpdatePermDetailSc",                
                    data: _obj,                    
                }).always(function( data ) {
                    if(data.Value==null)c++;
                });
            })
            
            $.each($lisIns, function(a,b){
                var _obj={};
                _obj['PERM_ID'] = $('#perm_id').html();
                _obj['ETA'] = $($(b).find('[id^="txtETA"]')[0]).val();
                _obj['ETD'] = $($(b).find('[id^="txtETD"]')[0]).val();        
                _obj['CRAFT_ID'] = $($(b).find('[id^="txtCRAFT_NAME"]')[0]).attr('data-craftid');
                _obj['FLIGHTNBR'] =  $($(b).find('[id^="txtFLIGHTNBR"]')[0]).val();
                _obj['PURPOSE_ID'] =  $($(b).find('[id^="txtPURPOSE_ID"]')[0]).val();
                _obj['DAY1'] = $($(b).find('[id^="chk"]')[1]).prop('checked') == true ? '1' : '0';
                _obj['DAY2'] = $($(b).find('[id^="chk"]')[2]).prop('checked') == true ? '2' : '0';
                _obj['DAY3'] = $($(b).find('[id^="chk"]')[3]).prop('checked') == true ? '3' : '0';
                _obj['DAY4'] = $($(b).find('[id^="chk"]')[4]).prop('checked') == true ? '4' : '0';
                _obj['DAY5'] = $($(b).find('[id^="chk"]')[5]).prop('checked') == true ? '5' : '0';
                _obj['DAY6'] = $($(b).find('[id^="chk"]')[6]).prop('checked') == true ? '6' : '0';
                _obj['DAY7'] = $($(b).find('[id^="chk"]')[7]).prop('checked') == true ? '7' : '0';
                _obj['FROM_AIRP'] =  $($(b).find('[id^="txtFROM_AIRP"]')[0]).val();
                _obj['TO_AIRP'] =  $($(b).find('[id^="txtTO_AIRP"]')[0]).val();                
                _obj['REGISTRATION'] =  $($(b).find('[id^="txtREGISTRATION"]')[0]).val();
                _obj['VIA'] =  $($(b).find('[id^="txtVIA"]')[0]).val();
                _obj['REMARK'] =  $($(b).find('[id^="txtREMARK"]')[0]).val();
                _obj['BEGINDATE'] = new Date($($(b).find('[id^="txtBEGINDATE"]')[0]).val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1')).format('yyyy-mm-dd');
                _obj['ENDDATE'] =  new Date($($(b).find('[id^="txtENDDATE"]')[0]).val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1')).format('yyyy-mm-dd');
                _obj['LASTUSER'] = '<%= _user.UserName.ToString()%>';  
                var $request = $.ajax({
                    async: false,
                    method: "POST",
                    url: urlApi + "api/PermDetailSc/CreatePermDetailSc",                
                    data: _obj,                    
                }).always(function( data ) {
                    if(data.Value==null)c++;
                });
            })

            alert('Update susser: '+ c + 'flight permission.');
            LoadDataAjax();
        }
        window.onkeydown = function(e) {
            var charCode = (e.which) ? e.which : e.keyCode;      
            var c= $('#tblSource tr').length;
            if (charCode == 118) {                
                var _tr ="<tr id='_"+c+"' data-isInsert='true' onmouseover=\"rowId=$(this).prop('id');\" onmouseout='rowId=0;'>"
                   +  "<td id='b_"+c+"'>" +  (c-1) + "</td>"
                   + "<td><input type='text' data-control='_updateAll' data-minlenght='4' maxlength='8' class='sInput' id='txtFLIGHTNBR"+c+"' value='' onblur='checkIsUpdate(this)' /></td>"
                   + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtREGISTRATION"+c+"' value='' onblur='checkIsUpdate(this)' /></td>"
                   + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' id='txtFROM_AIRP"+c+"' onfocusin='binAutocomplete(this,\"AERO\")' value='' onblur='checkIsUpdate(this)' /></td>"
                   + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' id='txtTO_AIRP"+c+"' onfocusin='binAutocomplete(this,\"AERO\")' value='' onblur='checkIsUpdate(this)'/></td>" 
                   + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtETD"+c+"' data-number='true' value='' onblur='checkIsUpdate(this)'/></td>"   
                   + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtETA"+c+"' data-number='true' value='' onblur='checkIsUpdate(this)'/></td>"  
                   + "<td style='background-color: antiquewhite!important;'><input id=\"chkAll"+ c +"\" type=\"checkbox\" onchange='checkAllChange(\""+c+"\");' /></td>"
                   + "<td><input id=\"chk"+ c +"1\" type=\"checkbox\" onchange='checkIsUpdate(this)' /></td>"
                   + "<td><input id=\"chk"+ c +"2\" type=\"checkbox\" onchange='checkIsUpdate(this)' /></td>"
                   + "<td><input id=\"chk"+ c +"3\" type=\"checkbox\" onchange='checkIsUpdate(this)' /></td>"
                   + "<td><input id=\"chk"+ c +"4\" type=\"checkbox\" onchange='checkIsUpdate(this)' /></td>"
                   + "<td><input id=\"chk"+ c +"5\" type=\"checkbox\"  onchange='checkIsUpdate(this)' /></td>"
                   + "<td><input id=\"chk"+ c +"6\" type=\"checkbox\" onchange='checkIsUpdate(this)' /></td>"
                   + "<td><input id=\"chk"+ c +"7\" type=\"checkbox\" onchange='checkIsUpdate(this)' /></td>"
                   + "<td><input type='text' data-control='_updateAll' data-minlenght='1' class='sInput' id='txtCRAFT_NAME"+c+"' data-craftid='0' onfocusin='binAutocomplete(this,\"CRAFT\")' value='' onblur='checkIsUpdate(this)'/></td>"
                   + "<td><input type='text' data-control='_updateAll' data-minlenght='1' data-CheckDate='true' class='sInput' id='txtBEGINDATE"+c+"' value='' onblur='checkIsUpdate(this)' /></td>"
                   + "<td><input type='text' data-control='_updateAll' data-minlenght='1' data-CheckDate='true' class='sInput' id='txtENDDATE"+c+"'value='' onblur='checkIsUpdate(this)'/></td>"
                   + "<td><input type='text' data-control='_updateAll' data-minlenght='1' class='sInput' id='txtPURPOSE_ID"+c+"' onfocusin='binAutocomplete(this,\"PURPOSE\")' onblur='checkIsUpdate(this)'/></td>"
                   + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtVIA"+c+"' onblur='checkIsUpdate(this)'/></td>"
                   + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtREMARK"+c+"' onblur='checkIsUpdate(this)'/></td>"
                   + "<td></td>"
                   + "<td style=\"white-space: nowrap;>\"" 
                               + "<div class=\"action-buttons\">"                               
                               + (qDel=="True"?"<a data-toggle=\"tooltip\" title=\"Delete\">"
                               + "<i id=\"btnDelete_"+c+"\" class=\"ace-icon fa fa-trash-o bigger-130\" onclick=\"var ax = confirm('Do you want delete?'); if(ax){$('#_"+c+"').remove();}\"></i></a>":"")
                               + "<a data-toggle=\"tooltip\" onclick=\"LoadShowHisEventClick(this);\" class=\"bigger-140 show-details-btn\" title=\"Show history\"><i class=\"ace-icon fa fa-angle-double-down\"></i></a>"
                               +"</div>"
                               + "</td>"
                   + "</tr>";
                if($('#tblSource tbody tr').length==0){
                    $('#tblSource tbody').append(_tr);
                    $('#tblSource input[data-control="_updateAll"]').each(function(a, b){
                        $(b).mouseover(function(){
                            onmouseoverInput(b.id)
                        });
                        $(b).mouseout(function(){
                            onmouseoutInput(b.id)
                        });
                        $(b).ValidateTip();
                    })
                    $('#_' + c + ' input')[0].focus();
                    return;
                }
                if(rowId==0){
                    alert('Please select row!');
                    return;
                }
                var $trCurrent = $('#'+rowId),
                    //$tr = $('#'+rowId).clone();
                    $tr = $(_tr);
                $tr.removeClass('success')
                $tr.addClass('rowCreate');
                $tr.removeAttr('data-isupdate');
                $tr.prop('id', '_'+ c);
                $tr.attr('onmouseover', 'rowId=$(this).prop(\'id\')');
                $('#'+ rowId + ' input[type!=hidden][tabindex!=-1]').each(function(v,n){
                    $($tr).find('[id^=' +$(n).prop('id').replace(/[0-9]\abc/gi,'')+']').val($(n).val());
                });
                $.each($tr.find('input[data-oldValue]'), function(){
                    $(this).removeAttr('data-oldValue');
                });
                
                $tr.attr('data-isInsert', 'true');
                $($tr.find('td')[0]).text('');
                var $input1 = $($trCurrent).find('input[tabindex!=-1][type!=hidden]');
                var $input2 = $($tr).find('input[tabindex!=-1][type!=hidden]');
                for (var i = 0; i < $input2.length; i++) {
                    $($input2[i]).val($($input1[i]).val());
                    $($input2[i]).prop('checked',$($input1[i]).prop('checked'));
                    $($input2[i]).attr('data-craftid', $($input1[i]).attr('data-craftid'));
                }
                $trCurrent.after($tr);
                $($($tr).find('input[type!=hidden][tabindex!=-1]')).each(function(){
                    $(this).prop('id', $(this).prop('id').replace(/[0-9]\abc/gi,'')+$('#tblSource tr').length+'abc');
                    $(this).removeAttr('onblur')
                })
                
                $($tr).find('input')[0].focus();
                $('#tblSource input[data-control="_updateAll"]').each(function(a, b){
                    $(b).mouseover(function(){
                        onmouseoverInput(b.id)
                    });
                    $(b).mouseout(function(){
                        onmouseoutInput(b.id)
                    });
                    $(b).ValidateTip();
                })
            }
            if (charCode == 119) {
                if(inputId==''){
                    alert('Please select!');
                    return;
                }
                var $ele = $('#' + inputId),
                    $rowIndex = $ele.closest('tr').index(),
                    $columIndex = $ele.closest('td').index(),
                    $type = $ele.prop('type');
                switch($type){
                    case 'text':
                        var x = $('#tblSource tr').eq($rowIndex + 1).find('td').eq($columIndex).find('input').val();
                        if (x == undefined)
                            x = $('#tblSource tr').eq($rowIndex).find('td').eq($columIndex).find('input').val();
                        if(x==''||x==undefined)return;
                        $ele.val(x);
                        $ele.focus();
                        break;
                    case 'checkbox':
                        $('#tblSource tr').eq($rowIndex).find('td').eq($columIndex).find('input')[0].prop('checked', $ele.prop('checked'));
                        break;
                }
                    
            }
            reloadAutoComplete();
        }
        function onmouseoverInput(id){
            inputId = id;
        }
        function onmouseoutInput(id){
            inputId='';
        }
    </script>
    <script>
        PermissionFlightDetails.init({
            permissionType: 'SC',
            permissionId: '<%= _ID %>',
            flightNbr: '<%= System.Web.HttpUtility.JavaScriptStringEncode(Request.QueryString["FlightNbr"] ?? string.Empty) %>',
            apiBase: '<%= System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"] %>',
            pageMethod: '<%= ResolveUrl("~/Permission/View_PermSC.aspx/GetFlightDetails") %>',
            tableId: 'permissionFlightDetailsTableSC',
            totalId: 'permissionFlightDetailsTotalSC'
        });
    </script>
    <script>
        function getMessageFullByRefence(){
            if($('#perm_id').html().trim()=='' && $('#txtREFERENCE').val().trim()==''){
                alert('No reference');
                return;
            }
            window.open('<%= Page.ResolveUrl("~/MessManagement/ShowMessageFullContent.aspx") + "?Menu_Id="
                            + Request.Params["Menu_ID"] %>'+'&id='+$('#perm_id').html()+'&fType=SC&content='+$('#txtREFERENCE').val().replace(/\n/gi,'%0A'),'_blank','toolbar=yes,scrollbars=yes,resizable=yes').resizeTo(1200, 520);
            //resizeTo(window.screen.availWidth, window.screen.availHeight)
        }
    </script>
    <script>
        function checkAllChange(id){
            var isChecked = $('#chkAll'+ id).prop('checked');
            
            $('#chk'+id+'1').prop('checked',isChecked);
            $('#chk'+id+'2').prop('checked',isChecked);
            $('#chk'+id+'3').prop('checked',isChecked);
            $('#chk'+id+'4').prop('checked',isChecked);
            $('#chk'+id+'5').prop('checked',isChecked);
            $('#chk'+id+'6').prop('checked',isChecked);
            $('#chk'+id+'7').prop('checked',isChecked);
           
        }
    </script>
    <script>
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
                            $ele.val($ele.val() + this + ',');
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
        $('#txtPERMDATE').multiDate();
        function ddlPERMTYPE_Change(){
            $('#txtVALIDHOURS').val($('#ddlPERMTYPE').val()=='LD'?'24':'72');
        }
        if($('#perm_id').html().trim()=='')
        {ddlPERMTYPE_Change();}
    </script>
</asp:Content>
