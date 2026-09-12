<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="ImportsPermSC_LD.aspx.cs" Inherits="prjApplication.Tool.ImportsPermSC_LD" %>


<%@ Register Assembly="prjComponents" Namespace="prjComponents" TagPrefix="cc1" %>
<%@ Import Namespace="prjBusinessLogic" %>
<%@ Import Namespace="prjInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        input{text-transform:uppercase; }
        textarea{text-transform:uppercase; }
                 
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
                                                    <input id="txtPERMNBR" type="text" runat="server" maxlength="5"  style="width: 100px" data-minlenght="1"/>
                                                </label>
                                                <label class="checkbox-inline" style="width: 50px">
                                                    REG
                                                </label>
                                                <label class="textbox-inline">
                                                    <input id="txtReg" type="text" runat="server" maxlength="4000" style="width: 120px" />
                                                </label>
                                                <label class="checkbox-inline" style="width: 100px">
                                                    PERMDATE
                                                </label>
                                                <label class="textbox-inline">
                                                    <input id="txtENDDATE" type="text"
                                                        data-date-format="dd/mm/yyyy" class="date-picker" runat="server" style="width: 120px" data-minlenght="1"/>
                                                </label>
                                                
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-lg-2 control-label">OPER</label>
                                            <div class="col-lg-10" style="text-align: left;">
                                                <label class="textbox-inline">
                                                    <select id="ddlLoaiImport" class="form-control" runat="server" style="width: 120px;">
                                                        <option value="HVN">HVN</option>
                                                        <option value="PIC">PIC</option>
                                                        <option value="VJC">VJC</option>
                                                        <option value="VFC">VFC</option>
                                                        <option value="ABW">ABW</option>
                                                        <option value="BAV">BAV</option>
                                                        <option value="ALL_OPER">OPER_ALL_LD</option>
                                                    </select>
                                                </label>
                                                <label class="checkbox-inline" style="width: 100px">
                                                    AUTHOR
                                                </label>
                                                <label class="textbox-inline">
                                                    <asp:DropDownList ID="ddlAUTHOR" runat="server" CssClass="form-control" Width="120px">
                                                    </asp:DropDownList>
                                                </label>
                                                <label class="checkbox-inline" style="width: 80px">
                                                    VERSION
                                                </label>
                                                <label class="textbox-inline">
                                                    <input id="txtVersion" type="text" runat="server" maxlength="1" style="width: 50px" data-minlenght="1"/>
                                                </label>
                                                <%--<label class="checkbox-inline" style="width: 100px">
                                                    PERTYPE
                                                </label>
                                                <label class="textbox-inline">
                                                    <select id="ddlPERMTYPE" class="form-control" runat="server" style="width: 120px;">
                                                        <option value="LD">Landing</option>
                                                        <option value="O/F">Over Flight</option>
                                                    </select>
                                                </label>--%>
                                                <%--<label class="checkbox-inline" style="width:100px">
                                                    SEASON
                                                </label>
                                                <label class="textbox-inline">
                                                    <select id="ddlSeason" class="form-control" runat="server" style="width: 100px;">
                                                        <option value="W">Winter</option>
                                                        <option value="S">Summer</option>
                                                    </select>
                                                </label>
                                                <label class="checkbox-inline" style="width:80px">
                                                    PURPOSE
                                                </label>
                                                <label class="textbox-inline">
                                                    <asp:DropDownList ID="ddlPURPOSE" runat="server" CssClass="form-control" Width="100px">
                                                    </asp:DropDownList>
                                                </label>--%>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-lg-2 control-label">SEASON</label>
                                            <div class="col-lg-10" style="text-align: left;">
                                                <label class="textbox-inline">
                                                    <select id="ddlSeason" class="form-control" runat="server" style="width: 120px;">
                                                        <option value="W">Winter</option>
                                                        <option value="S">Summer</option>
                                                    </select>
                                                </label>
                                                <label class="checkbox-inline" style="width: 100px">
                                                    PURPOSE
                                                </label>
                                                <label class="textbox-inline">
                                                    <asp:DropDownList ID="ddlPURPOSE" runat="server" CssClass="form-control" Width="120px">
                                                    </asp:DropDownList>
                                                </label>
                                                <label class="checkbox-inline" style="width: 100px">
                                                    CRAFT
                                                </label>
                                                <label class="textbox-inline">
                                                    <input id="txtCraft" type="text" runat="server" style="width: 120px" />
                                                </label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-lg-2 control-label">TYPE</label>
                                            <div class="col-lg-2" style="text-align: left;">
                                                <label class="textbox-inline">
                                                    <select id="ddlPERMTYPE" style="width: 70px;" >
                                                        <option value="LD">LD</option>
                                                        <%--<option value="O/F">O/F</option>--%>
                                                    </select>
                                                </label>
                                                <span style="position: relative; cursor: pointer; color: blue; font-style: italic; font-size: 20px;" onclick="checkPermNumber()">check
                                    <i style="position: absolute; top: 0px; left: 45px; width: 250px; display: none;" id="divListNumber"></i>
                                                </span>
                                            </div>
                                            <label class="col-lg-2 control-label">FLIGHT TYPE</label>
                                            <div class="col-lg-2" style="text-align: left;">
                                                <label class="textbox-inline">
                                                    <select id="ddlFLIGHTTYPE" runat="server" style="width: 70px;" >
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
                                    <select runat="server" id="ddlAction" onchange="ddlAction_OnChange()">
                                        <option value="TangChuyen">Tăng chuyến</option>
                                        <option value="HuyChuyen">Hủy chuyến</option>
                                        <option value="ThayDoi">Thay đổi</option>
                                    </select>
                                    <asp:LinkButton runat="server" ID="btnSave" CssClass="btn btn-sm btn-primary btn-bold"
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
                            <div class="row">
                                <div class="col-lg-10">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <label class="col-lg-2 control-label">ERROR</label>
                                            <div class="col-lg-10" style="text-align: left;border: 1px solid #D5D5D5; color:red;">
                                                <asp:Literal id="lblLOG" runat="server"> </asp:Literal>
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
    <label id="lblAERO"></label>
    <div id="divHuyChuyen" style="display: none">
        <fieldset>
            <legend>Phép hủy</legend>
            <asp:Button ID="btnTimHuyChuyen" Text="Search" OnClick="btnTimHuyChuyen_Click" runat="server"
                CssClass="btn btn-sm btn-primary btn-bold" />
            <asp:Button ID="btnXoaHuyChuyen" Text="Delete huy chuyen" OnClick="btnXoaHuyChuyen_Click"
                runat="server" CssClass="btn btn-sm btn-primary btn-bold" />
            <asp:Button ID="btnTimThayDoi" Text="Search" OnClick="btnTimThayDoi_Click"
                runat="server" CssClass="btn btn-sm btn-primary btn-bold" />
            <asp:Button ID="btnXoaThayDoi" Text="Delete thay doi" OnClick="btnXoaThayDoi_Click"
                runat="server" CssClass="btn btn-sm btn-primary btn-bold" />
            <asp:GridView ID="grdHuyChuyen" Width="100%" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Container.DataItemIndex+1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="CallSign" DataField="CALLSIGN" />
                    <asp:BoundField HeaderText="Từ ngày" DataField="FROMDATE" />
                    <asp:BoundField HeaderText="Đến ngày" DataField="TODATE" />
                    <asp:BoundField HeaderText="Ngày bay" DataField="DAILY" />
                    <asp:BoundField HeaderText="Sân cất" DataField="FROM_AIRP" />
                    <asp:BoundField HeaderText="Sân hạ" DataField="TO_AIRP" />
                    <asp:BoundField HeaderText="Giờ cất" DataField="ETD" />
                    <asp:BoundField HeaderText="Giờ hạ" DataField="ETA" />
                    <asp:BoundField HeaderText="Số phép" DataField="PERMNBR" />
                    <asp:BoundField HeaderText="Tàu bay" DataField="CRAFT" />
 		    <asp:BoundField HeaderText="Hãng" DataField="OPER" />
                    <asp:BoundField HeaderText="Mùa" DataField="SEASON" />
                    <asp:BoundField HeaderText="Loại" DataField="PERMTYPE" />
                    <asp:BoundField HeaderText="Ghi chú" DataField="REMARK" />
                    <asp:BoundField HeaderText="Mục đích" DataField="PURPOSE" />

                </Columns>
            </asp:GridView>
        </fieldset>
        <fieldset>
            <legend>Danh sách kết quả tìm kiếm</legend>
            <%--<asp:Button ID="btnLenhHuy" runat="server" Text="Lenh Huy" OnClick="AccessHUY()"
                CssClass="btn btn-sm btn-primary btn-bold" />--%>
            <button type="button" class="btn btn-sm btn-primary btn-bold" id="btnLenhHuy" onclick="AccessHUY()">
            HỦY CHUYẾN</button>
            <asp:Button ID="btnLenhThayDoi" runat="server" Text="Lenh Thay doi" OnClick="btnLenhThayDoi_Click" CssClass="btn btn-sm btn-primary btn-bold" />
            <table id="grdKetQuaChuyenHuy_Cus">
                <caption>DANH SÁCH CHUYẾN BAY BỊ HỦY</caption>
                <thead>
                    <tr>
                        <th>STT</th>
                        <th><input id="chkAll" checked type="checkbox" onchange="chkAll_OnChange()" /></th>
                        <th>CallSign</th>
                        <th>Perm</th>
                        <th>Begin</th>
                        <th>End</th>
                        <th>From</th>
                        <th>To</th>
                        <th>Daily</th>
                        <th>ETD</th>
                        <th>ETA</th>
                        <th>OPER</th>
                        <th>SEASON</th>
                        <th>Type</th>
                        <th>Remark</th>
                        <th>Purpose</th>
                        <th>HUY</th>
                        <th>Begin Huy</th>
                        <th>End Huy</th>
                        <th></th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptKetQuaChuyenHuy" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Container.ItemIndex+1 %></td>
                                <td>
                                    <asp:CheckBox ID="chkSelectRow" data-Id='<%# Eval("IDPERMDETAIL")%>' data-fDate='<%# ((DateTime)Eval("Huy_FromDate")).ToString("dd-MM-yyyy") %>' data-etd='<%# Eval("ETD_CHANGE").ToString().Trim() %>' data-eta='<%# Eval("ETA_CHANGE").ToString().Trim() %>'
                                        data-tDate='<%# ((DateTime)Eval("Huy_ToDate")).ToString("dd-MM-yyyy") %>' data-ThamChieu='<%# Eval("THAMCHIEU") %>' data-currentId='<%# Eval("ID")%>'
                                        data-PermType='<%# Eval("PERMTYPE") %>' data-daily='<%# Eval("DAILY") %>' runat="server"
                                        Checked="true" />
                                </td>
                                <td><%# Eval("CALLSIGN") %></td>
                                <td style="white-space:nowrap; color: red;"><%# Eval("PermNbr_ID") %></td>
                                <td style="white-space:nowrap;"><%# DateTime.Parse( Eval("FROMDATE").ToString()).ToString("dd-MM-yyyy") %></td>
                                <td style="white-space:nowrap;"><%# DateTime.Parse( Eval("TODATE").ToString()).ToString("dd-MM-yyyy") %></td>
                                <td><%# Eval("FROM_AIRP") %></td>
                                <td><%# Eval("TO_AIRP") %></td>
                                <td><%# Eval("DAILY_PHEP") %></td>
                                <td><%# Eval("ETD") %></td>
                                <td><%# Eval("ETA") %></td>
                                <td><%# Eval("OPER") %></td>
                                <td><%# Eval("SEASON") %></td>
                                <td><%# Eval("PERMTYPE") %></td>
                                <td class="wid_300px"><%# Eval("Remark") %></td>
                                <td><%# Eval("Purpose") %></td>
                                <td style=" color: red;"><%# Eval("Daily") %></td>
                                <td style="white-space:nowrap; color: red;"><%# DateTime.Parse( Eval("HUY_FROMDATE").ToString()).ToString("dd-MM-yyyy") %></td>
                                <td style="white-space:nowrap; color: red;"><%# DateTime.Parse( Eval("HUY_TODATE").ToString()).ToString("dd-MM-yyyy") %></td>
                                <td></td>
                                <td></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </fieldset>
    </div>



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
    <script>
        function ddlAction_OnChange() {
            ddlAction_Display();
        }
        function Action_OnChange(b) {
            alert("Import Sucess full" + b);
        }
        function chkAll_OnChange() {
            $('#grdKetQuaChuyenHuy_Cus tbody input[type="checkbox"]').prop('checked', $('#chkAll').prop('checked'))
        }

        function ddlAction_Display() {
            var $el = $('#<%= ddlAction.ClientID%>');
            switch ($el.val()) {
                case "HuyChuyen":
                    $('#divHuyChuyen').show();
                    $('#grdKetQuaChuyenHuy_Cus').show();
                    $('#<%= btnTimHuyChuyen.ClientID%>').show();
                    $('#<%= btnXoaHuyChuyen.ClientID%>').show();
                    $('#btnLenhHuy').show();

                    $('#<%= btnTimThayDoi.ClientID%>').hide();
                    $('#<%= btnXoaThayDoi.ClientID%>').hide();
                    $('#<%= btnLenhThayDoi.ClientID%>').hide();
                    break;
                case "ThayDoi":
                    $('#divHuyChuyen').hide();

                   <%-- $('#grdKetQuaChuyenHuy_Cus').hide();
                    $('#<%= btnTimHuyChuyen.ClientID%>').hide();
                    $('#<%= btnXoaHuyChuyen.ClientID%>').hide();
                    $('#<%= btnLenhHuy.ClientID%>').hide();

                    $('#<%= btnTimThayDoi.ClientID%>').show();
                    $('#<%= btnXoaThayDoi.ClientID%>').show();
                    $('#<%= btnLenhThayDoi.ClientID%>').show();--%>

                    break;
                case "TangChuyen":
                    $('#divHuyChuyen').hide();
                    break;
            }
        }


        function AccessHUY() {
           
             var cf = confirm('Do you want cancel the flights ?');
            if (cf) {
                var $request = $.ajax({
                    async: true,
                    method: "PUT",
                    url: urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=PERM_IMP_PKG&storeName=impToPerm_Huy",
                    data: {},
                    complete: function () {                        
                        unLoadingData('loadingAccess');
                    },
                    beforeSend: function () {
                        preloadImgAfterButton('btnLenhHuy', 'loadingAccess');
                    }
                }).always(function (data) {
                    CallButtonEvent();
                    alert(data.Value == -1 ? 'Error!' : 'Sussess!');
                });
            }
        }

        function CallButtonEvent()
        {
            document.getElementById("<%=btnTimHuyChuyen.ClientID%>").click();
        }


    </script>

</asp:Content>
