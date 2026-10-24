<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true"
    CodeBehind="Imports.aspx.cs" Inherits="prjApplication.Tool.Imports" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<%@ Register Assembly="prjComponents" Namespace="prjComponents" TagPrefix="cc1" %>
<%@ Import Namespace="prjBusinessLogic" %>
<%@ Import Namespace="prjInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        input, textarea {
            text-transform: uppercase;
        }
    </style>
    <link href="../Style/Style_List.css" rel="stylesheet" />
    <!-- PAGE CONTENT BEGINS -->

    <div class="widget-box">
        <div class="widget-header widget-header-blue widget-header-flat">
            <h4 class="widget-title lighter">Import Data</h4>
        </div>

        <div class="widget-body">
            <div class="widget-main">
                <div id="fuelux-wizard-container">

                    <div class="step-content pos-rel">
                        <div class="center">

                            <div class="row">
                                <div class="col-lg-10">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <label class="col-lg-2 control-label">PERMNBR</label>
                                            <div class="col-lg-10" style="text-align: left;">
                                                <label class="textbox-inline">
                                                    <input id="txtPERMNBR" maxlength="5" data-minlenght="1" data-control="checkIns" type="text" runat="server"
                                                        class="form-control" style="width: 100px" />
                                                </label>

                                                <label class="checkbox-inline">
                                                    PERMDATE
                                                </label>
                                                <label class="textbox-inline">
                                                    <input id="txtPermDate" type="text" runat="server" data-control="checkIns"
                                                        data-date-format="dd/mm/yyyy" class="date-picker" style="width: 100px" data-minlenght="1" />
                                                </label>
                                                <label class="checkbox-inline">
                                                    PERMTYPE
                                                </label>
                                                <label class="textbox-inline">
                                                    <select id="ddlPERMTYPE" class="form-control" runat="server" style="width: 100px;">
                                                        <option value="LD">LD</option>
                                                        <option value="O/F">O/F</option>
                                                    </select>
                                                </label>
                                                <label class="checkbox-inline">
                                                    FLIGHTTYPE
                                                </label>
                                                <label class="textbox-inline">
                                                    <select id="ddlFLIGHTTYPE" class="form-control" runat="server" style="width: 100px;">
                                                       <option value="NO">NO</option>
                                                        <option value="SC">SC</option>
                                                        
                                                    </select>
                                                </label>
                                                <label class="checkbox-inline">REG</label>
                                                <label class="textbox-inline">
                                                    <input id="txtREG" type="text" runat="server" style="width: 100px" />
                                                </label>

                                            </div>
                                            
                                        </div>

                                        <div class="form-group">
                                            <label class="col-lg-2 control-label">OPER</label>
                                            <div class="col-lg-10" style="text-align: left;">
                                                <label class="textbox-inline">
                                                    <select id="ddlOPER" class="form-control" runat="server" style="width: 100px;">
                                                        <option value="ABG">ABG</option>
                                                        <option value="KAR">KAR</option>
                                                        <option value="KTK">KTK</option>
                                                        <option value="NWS">NWS</option>
                                                        <option value="NOK">NOK</option>
                                                        <option value="ALL_OPER">OPER_ALL</option>
                                                        <option value="RMY">RMY</option>
                                                    </select>
                                                </label>
                                                <label class="checkbox-inline">
                                                    AUTHOR
                                                </label>
                                                <label class="textbox-inline">
                                                    <asp:DropDownList ID="ddlAUTHOR" runat="server" CssClass="form-control" Width="100px">
                                                    </asp:DropDownList>
                                                </label>
                                                <label class="checkbox-inline" style="width: 100px">
                                                    PURPOSE
                                                </label>
                                                <label class="textbox-inline">
                                                    <asp:DropDownList ID="ddlPURPOSE" runat="server" CssClass="form-control" Width="100px">
                                                    </asp:DropDownList>
                                                </label>
                                                <label class="checkbox-inline">
                                                    CRAFT
                                                </label>
                                                <label class="textbox-inline">
                                                    <input id="txtCRAFTTYPE" type="text" runat="server" style="width: 100px" data-minlenght="1" />
                                                </label>
                                                <label class="checkbox-inline">
                                                    <span style="position: relative; cursor: pointer; color: blue; font-style: italic; font-size: 20px;" onclick="checkPermNumber()">check
                                    <i style="position: absolute; top: 0px; left: 45px; width: 250px; display: none;" id="divListNumber"></i>
                                                </span>
                                                </label>
                                            </div>

                                        </div>                                       
                                    </div>
                                    <div class="col-lg-5">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--<div class="row">
                                <div class="col-lg-10">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <label class="col-lg-2 control-label">Billing address</label>
                                            <div class="col-lg-10" style="text-align: left;">
                                                <label class="textbox-inline">
                                                    <textarea id="txtBillingAddress" runat="server" rows="3"></textarea>
                                                </label>
                                                <label class="textbox-inline">
                                                    Perm content <textarea id="txtPermContent" runat="server" rows="3"></textarea>
                                                </label>
                                                <label class="textbox-inline">
                                                    Via <textarea id="txtVia" runat="server" rows="3"></textarea>
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>--%>
                                <%--<div class="row">
                                <div class="col-lg-10">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <label class="col-lg-2 control-label">REPORT</label>
                                            <div class="col-lg-10" style="text-align: left;">
                                                <label class="form-control" id="lbLog" runat="server"></label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>--%>
                                <div class="row">
                                    <div class="col-lg-6">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <label class="col-lg-2 control-label">CONTENT</label>
                                                <div class="col-lg-10" style="text-align: left;">
                                                    <textarea class="form-control" id="txtContent" runat="server" style="height: 380px;"
                                                        placeholder="Content"></textarea>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-4">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <label class="col-lg-2 control-label">VIA</label>
                                                <div class="col-lg-10" style="text-align: left;">
                                                    <textarea class="form-control" id="txtRoutes" runat="server" style="height: 380px;"
                                                        placeholder="Via"></textarea>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-10">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <label class="col-lg-2 control-label">REMARK</label>
                                                <div class="col-lg-10" style="text-align: left;">
                                                    <textarea class="form-control" id="txtRemark" runat="server" placeholder="Remark"></textarea>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="text-align: center">
                                    <div class="col-lg-10">

                                        <asp:LinkButton runat="server" ID="btnSave" OnClientClick="return checkValidCustomMinlenght('checkIns')" CssClass="btn btn-sm btn-primary btn-bold"
                                            OnClick="linkSave_Click">
                                                <span class="glyphicon glyphicon-download-alt"></span>
                                                IMPORT
                                        </asp:LinkButton>

                                        <a href="#" class="btn btn-sm btn-primary">
                                            <span class="ace-icon fa fa-times"></span>
                                            EXIT

                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- /.widget-main -->
            </div>
            <!-- /.widget-body -->
        </div>
        <div class="row">
            <div class="col-lg-10">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="col-lg-2 control-label">ERROR</label>
                        <div class="col-lg-10" style="text-align: left; border: 1px solid #D5D5D5; color: red;">
                            <asp:Literal ID="lblLOG" runat="server"> </asp:Literal>
                            <%--<label class="form-control" id="lblLOG" runat="server" autosize = "false" style =" height:100px;"></label>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- PAGE CONTENT ENDS -->

        <!-- /.row -->
        <!-- basic scripts -->


        <!-- page specific plugin scripts -->
        <script src="../Scripts/CustomDynamic.js"></script>
        <script src="<%=Global.ApplicationPath%>/Style/assets/js/wizard.min.js"></script>
        <script src="<%=Global.ApplicationPath%>/Style/assets/js/bootstrap-datetimepicker.min.js"></script>
        <script src="<%=Global.ApplicationPath%>/Style/assets/js/bootstrap-datepicker.min.js"></script>
        <script src="<%=Global.ApplicationPath%>/Scripts/CustomDynamic.js"></script>
        <script src="<%=Global.ApplicationPath%>/Scripts/validate.js"></script>
        <script src="<%=Global.ApplicationPath%>/Scripts/DateTimeFomat.js"></script>
        <script src="<%=Global.ApplicationPath%>/Scripts/CustumStaticdata.js"></script>

        <script>
       // var txtPERMNBR = $("#<%=txtPERMNBR.ClientID%>").val;
        function checkPermNumber() {
            <%--alert($("#<%=txtPERMNBR.ClientID%>").val());--%>
            $('#divListNumber').html('');
            var $request = $.ajax({
                //async: false,
                method: "PUT",
                url: urlApi + "api/ApiExtension/ExcuteTable?packageName=PERM_PKG&storeName=validFlightNbr",
                data: JSON.stringify({ P_FLIGHT_TYPE: 'NO', P_FLIGHTNBR: $("#<%=txtPERMNBR.ClientID%>").val(), P_PERMTYPE: $("#<%=ddlPERMTYPE.ClientID%>").val() }),
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
    </script>
</asp:Content>
