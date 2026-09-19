<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="ImportPermSC_OF.aspx.cs" Inherits="prjApplication.Tool.ImportPermSC_OF" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<%@ Register Assembly="prjComponents" Namespace="prjComponents" TagPrefix="cc1" %>
<%@ Import Namespace="prjBusinessLogic" %>
<%@ Import Namespace="prjInfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
                                            <label class="col-lg-2 control-label">EXPORT</label>
                                            <div class="col-lg-10" style="text-align: left;">
                                                <label class="textbox-inline">
                                                    <select id="SctExportType" class="form-control" runat="server" style="width: 120px;">
                                                        <option value="2">WINTER</option>
                                                        <option value="1">SUMMER</option>
                                                    </select>
                                                </label>
                                                <label class="checkbox-inline" style="width: 90px">PERMNBR</label>
                                                <label class="textbox-inline">
                                                    <input id="txtPERMNBR" type="text" runat="server" maxlength="5" data-minlenght="1" style="width: 120px" />
                                                </label>
                                                <label class="checkbox-inline" style="width: 80px">VERSION</label>
                                                <label class="textbox-inline">
                                                    <input id="txtVersion" type="text" runat="server" maxlength="1" data-minlenght="1" style="width: 50px" />
                                                </label>
                                                <label class="checkbox-inline" style="width: 50px">REG</label>
                                                <label class="textbox-inline">
                                                    <input id="txtReg" type="text" runat="server" maxlength="4000" style="width: 120px" />
                                                </label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-lg-2 control-label">OPER</label>
                                            <div class="col-lg-10" style="text-align: left;">
                                                <label class="textbox-inline">
                                                    <select id="ddlLoaiImport" class="form-control" runat="server" style="width: 85px;">
                                                        <option value="BSX">BSX</option>
                                                        <option value="LKH">LKH</option>
                                                        <option value="ETD">ETD</option>
                                                        <option value="AHK">AHK</option>
                                                        <option value="UTP">UTP</option>
                                                        <option value="CKK">CKK</option>
                                                        <option value="CCA">CCA</option>
                                                        <option value="RJA">RJA</option>
                                                        <option value="AIQ">AIQ</option>
                                                        <option value="SWM">SWM</option>
                                                        <option value="CSN">CSN</option>
                                                        <option value="CQH">CQH</option>
                                                        <option value="MXD">MXD</option>
                                                        <option value="CXA">CXA</option>
                                                        <option value="THY">THY</option>
                                                        <option value="QTR">QTR</option>
                                                        <option value="HBH">HBH</option>
                                                        <option value="MAS">MAS</option>
                                                        <option value="MAS66">MAS66</option>
                                                        <option value="CHH">CHH</option>
                                                        <option value="TGW">TGW</option>
                                                        <option value="ETH">ETH</option>
                                                        <option value="APG">APG</option>
                                                        <option value="XAX">XAX</option>
                                                        <option value="AXM">AXM</option>
                                                        <option value="DKH">DKH</option>
                                                        <option value="CSH">CSH</option>
                                                        <option value="CEB">CEB</option>
                                                        <option value="CSZ">CSZ</option>
                                                        <option value="JAI">JAI</option>
                                                        <option value="JCC">JCC</option>
                                                        <option value="AIC">AIC</option>
                                                        <option value="HXA">HXA</option>
                                                        <option value="CES">CES</option>
                                                        <option value="MKR">MKR</option>
                                                        <option value="RBA">RBA</option>
                                                        <option value="UAE">UAE</option>
                                                        <option value="CBJ">CBJ</option>
                                                        <option value="SVR">SVR</option>
                                                        <option value="LKE">LKE</option>
                                                        <option value="NCT">NCT</option>
                                                        <option value="GIA">GIA</option>
                                                        <option value="MSR">MSR</option>
                                                        <option value="CAL">CAL</option>
                                                        <option value="QFA">QFA</option>
                                                        <option value="BKP">BKP</option>
                                                        <option value="BAW">BAW</option>
                                                        <option value="EVA">EVA</option>
                                                        <option value="JNA">JNA</option>
                                                        <option value="ESR">ESR</option>
                                                        <option value="SIA">SIA</option>
                                                        <option value="ABL">ABL</option>
                                                        <option value="KAL">KAL</option>
                                                        <option value="TWB">TWB</option>
                                                        <option value="CDG">CDG</option>
                                                        <option value="AAR">AAR</option>
                                                        <option value="NCT">NCT</option>
                                                        <option value="CLU">CLU</option>
                                                        <option value="ABW">ABW</option>
                                                        <option value="NOK">NOK</option>
                                                        <option value="NOK_B">NOK_B</option>
                                                        <option value="PAL">PAL</option>
                                                        <option value="OMA">OMA</option>
                                                        <option value="VGO">VGO</option>
                                                        <option value="HKE">HKE</option>
                                                        <option value="SLK">SLK</option>
                                                        <option value="THA">THA</option>
                                                        <option value="SAA">SAA</option>
                                                        <option value="JJA">JJA</option>
                                                        <option value="JAL">JAL</option>
                                                        <option value="SVA">SVA</option>
                                                        <option value="GEC">GEC</option>
                                                        <option value="BOX">BOX</option>
                                                        <option value="THD">THD</option>
                                                        <option value="ANA">ANA</option>
                                                        <option value="KHV">KHV</option>
                                                        <option value="CLX">CLX</option>
                                                        <option value="FDX">FDX</option>
                                                        <option value="CPA">CPA</option>
                                                        <option value="MAU">MAU</option>
                                                        <option value="UAL">UAL</option>
                                                        <option value="HDA">HDA</option>
                                                        <option value="LAO">LAO</option>
                                                        <option value="ICV">ICV</option>
                                                        <option value="CRK">CRK</option>
                                                        <option value="HKC">HKC</option>
                                                        <option value="SVR">SVR</option>
                                                        <option value="VOZ">VOZ</option>
                                                        <option value="KME">KME</option>
                                                        <%--<option value="KME">KME</option>--%>
                                                        <option value="TAX">TAX</option>
                                                        <option value="TLM">TLM</option>
                                                        <option value="UPS">UPS</option>
                                                        <option value="ALL_OPER">OPER_CONVERT</option>
                                                    </select>
                                                </label>
                                                <label class="checkbox-inline" style="width: 100px">PERMDATE</label>
                                                <label class="textbox-inline">
                                                    <input id="txtENDDATE" type="text"
                                                        data-date-format="dd/mm/yyyy" class="date-picker" runat="server" style="width: 120px" data-minlenght="1" />
                                                </label>
                                                <label class="checkbox-inline" style="width: 100px">AUTHOR</label>
                                                <label class="textbox-inline">
                                                    <asp:DropDownList ID="ddlAUTHOR" runat="server" CssClass="form-control" Width="70px">
                                                    </asp:DropDownList>
                                                </label>
                                                <%--<label class="checkbox-inline"style="width:100px">PERMTYPE</label>
                                                <label class="textbox-inline">
                                                    <select id="ddlPERMTYPE" class="form-control" runat="server" style="width: 60px;">
                                                        <option value="LD">LD</option>
                                                        <option value="O/F">O/F</option>
                                                    </select>
                                                </label>--%>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-lg-2 control-label">SEASON</label>
                                            <div class="col-lg-10" style="text-align: left;">
                                                <label class="textbox-inline">
                                                    <select id="ddlSeason" class="form-control" runat="server" style="width: 50px;">
                                                        <option value="W">W</option>
                                                        <option value="S">S</option>
                                                    </select>
                                                </label>
                                                <label class="checkbox-inline" style="width: 90px">PURPOSE</label>
                                                <label class="textbox-inline">
                                                    <asp:DropDownList ID="ddlPURPOSE" runat="server" CssClass="form-control" Width="80px">
                                                    </asp:DropDownList>
                                                </label>
                                                <label class="checkbox-inline" style="width: 70px">CRAFT</label>
                                                <label class="textbox-inline">
                                                    <input id="txtCraft" type="text" runat="server" style="width: 200px" data-minlenght="1" />
                                                </label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-lg-2 control-label">P TYPE</label>
                                            <div class="col-lg-2" style="text-align: left;">
                                                <label class="textbox-inline">
                                                    <select id="ddlPERMTYPE" style="width: 70px;" >
                                                        <%--<option value="LD">LD</option>--%>
                                                        <option value="O/F">O/F</option>
                                                    </select>
                                                </label>
                                                <span style="position: relative; cursor: pointer; color: blue; font-style: italic; font-size: 20px;" onclick="checkPermNumber()">check
                                    <i style="position: absolute; top: 0px; left: 45px; width: 250px; display: none;" id="divListNumber"></i>
                                                </span>
                                            </div>
                                            <label class="col-lg-2 control-label">FLIGHT TYPE</label>
                                            <div class="col-lg-2" style="text-align: left;">
                                                <label class="textbox-inline">
                                                    <select id="ddlFLIGHTTYPE" style="width: 70px;" runat="server">
                                                        <option value="SC">SC</option>
                                                        <option value="NO">NO</option>
                                                    </select>
                                                </label>                                                
                                            </div>
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
                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <label class="col-lg-2 control-label">CONTENT</label>
                                            <div class="col-lg-10" style="text-align: left;">
                                                <textarea class="form-control" id="txtContent" runat="server" style="height: 380px;"
                                                    placeholder="Lưu ý : Khi coppy nội dung vào với hãng PIC nếu sân bay và giờ bay dự kiến liền nhau phải dùng phím space để tách.Xóa các đầu mục nếu có ."></textarea>
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

                                    <asp:LinkButton runat="server" ID="btnSave" CssClass="btn btn-sm btn-primary btn-bold"
                                        OnClick="linkSave_Click">
                                                <span class="glyphicon glyphicon-download-alt"></span>
                                                IMPORT
                                    </asp:LinkButton>

                                    <a class="btn btn-sm btn-primary">
                                        <span class="ace-icon fa fa-times"></span>
                                        EXIT
                                    </a>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-10">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <label class="col-lg-2 control-label">ERROR</label>
                                            <div class="col-lg-10" style="text-align: left; border: 1px solid #D5D5D5; color: red;">
                                                <asp:Literal ID="lblLOG" runat="server"> </asp:Literal>
                                            </div>
                                        </div>
                                    </div>
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
    <!-- PAGE CONTENT ENDS -->
    <!-- /.row -->
    <!-- basic scripts -->
    <!-- page specific plugin scripts -->

    <script src="<%=Global.ApplicationPath%>/Style/assets/js/wizard.min.js"></script>
    <script src="<%=Global.ApplicationPath%>/Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="<%=Global.ApplicationPath%>/Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="<%=Global.ApplicationPath%>/Scripts/CustomDynamic.js"></script>
    <script src="<%=Global.ApplicationPath%>/Scripts/validate.js"></script>
    <script src="<%=Global.ApplicationPath%>/Scripts/DateTimeFomat.js"></script>
    <script src="<%=Global.ApplicationPath%>/Scripts/CustumStaticdata.js"></script>
    <script src="<%=Global.ApplicationPath%>/Scripts/CustomPaging.js"></script>
    <script src="<%=Global.ApplicationPath%>/Style/assets/js/jquery-2.1.4.min.js"></script>
    <script>
       // var txtPERMNBR = $("#<%=txtPERMNBR.ClientID%>").val;
        function checkPermNumber() {
            <%--alert($("#<%=txtPERMNBR.ClientID%>").val());--%>
            $('#divListNumber').html('');
            var $request = $.ajax({
                //async: false,
                method: "PUT",
                url: urlApi + "api/ApiExtension/ExcuteTable?packageName=PERM_PKG&storeName=validFlightNbr",
                data: JSON.stringify({ P_FLIGHT_TYPE: 'SC', P_FLIGHTNBR: $("#<%=txtPERMNBR.ClientID%>").val(), P_PERMTYPE: $('#ddlPERMTYPE').val().toUpperCase() }),
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

