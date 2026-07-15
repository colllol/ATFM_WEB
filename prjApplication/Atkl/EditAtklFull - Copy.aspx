<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/ATFM.Master" CodeBehind="EditAtklFull.aspx.cs" Inherits="prjApplication.Atkl.EditAtklFull" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<%@ Register Assembly="prjComponents" Namespace="prjComponents" TagPrefix="cc1" %>
<%@ Import Namespace="prjBusinessLogic" %>
<%@ Import Namespace="prjInfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div id="pathFileUpload" hidden="hidden"></div>
    <div id="atkl_id" hidden="hidden"><%= Request.QueryString["ID"]==null?"": Request.QueryString["ID"]%></div>
    <div id="divExcuteScript" hidden="hidden"></div>
    <fieldset class="box-border">
        <div id="divPermMaster">
            <label for="txtPERMNBR_ID" style="display: none;">PERMNBR</label>
            <input id="txtPERMNBR_ID" readonly type="text" class="wid_120px" style="display: none;" />

            <table style="width: 100%; border: 0px solid black;">
                <tr>
                    <td style="width: 50%">
                        <fieldset class="box-border">
                            <legend class="box-border">Giờ theo FPL</legend>
                            <table style="width: 100%; border: 0px solid black;">
                                <tr>
                                    <td>
                                        <label for="txtFPL_START" style="font-size: 12px;">Giờ khởi hành</label><br />
                                        <input id="txtFPL_START" data-control="update" maxlength="6" type="text"
                                            class="wid_80px" style="height: 25px;" />
                                    </td>
                                    <td>
                                        <label for="txtFPL_SUM" style="font-size: 12px;">Tổng thời gian đến sân bay đến</label><br />
                                        <input style="height: 25px;" id="txtFPL_SUM" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                </tr>

                            </table>
                        </fieldset>
                    </td>
                    <td style="width: 50%">
                        <fieldset class="box-border">
                            <legend class="box-border">Khu vực tại sân</legend>
                            <table style="width: 100%; border: 0px solid black;">
                                <tr>
                                    <td>
                                        <label for="txtAREA_PARKING" style="font-size: 12px;">Vị trí đỗ tàu bay</label><br />
                                        <input id="txtAREA_PARKING" data-control="update" maxlength="6" type="text"
                                            class="wid_80px" style="height: 25px;" />
                                    </td>
                                    <td>
                                        <label for="txtAREA_EOBT" style="font-size: 12px;">Giờ dự kiến rút chèn (EOBT) </label>
                                        <br />
                                        <input style="height: 25px;" id="txtAREA_EOBT" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtAREA_AOBT" style="font-size: 12px;">Giờ rút chèn thực tế (AOBT)</label><br />
                                        <input style="height: 25px;" id="txtAREA_AOBT" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="txtAREA_START_UP" style="font-size: 12px;">Giờ nổ máy (Start-up)</label><br />
                                        <input style="height: 25px;" id="txtAREA_START_UP" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtAREA_PUSH_BACK" style="font-size: 12px;">Giờ đẩy ra (Push-back)</label><br />
                                        <input style="height: 25px;" id="txtAREA_PUSH_BACK" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtAREA_START_RUN" style="font-size: 12px;">Giờ bắt đầu lăn</label><br />
                                        <input style="height: 25px;" id="txtAREA_START_RUN" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="txtAREA_TWR_APP" style="font-size: 12px;">Giờ GCU chuyển giao cho TWR</label><br />
                                        <input style="height: 25px;" id="txtAREA_TWR_APP" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr valign="top">
                    <td style="width: 50%">
                        <fieldset class="box-border">
                            <legend class="box-border">Khu vực kiểm soát TWR</legend>
                            <table style="width: 100%; border: 0px solid black;">
                                <tr>
                                    <td>
                                        <label for="txtTWR_CHC" style="font-size: 12px;">Giờ đến diểm chờ lên đường CHC</label><br />
                                        <input id="txtTWR_CHC" data-control="update" maxlength="6" type="text"
                                            class="wid_80px" style="height: 25px;" />
                                    </td>
                                    <td>
                                        <label for="txtTWR_TOPOINT_FLY_HOUR" style="font-size: 12px;">Giờ đến điểm chờ nhận huấn lệnh cất cánh</label><br />
                                        <input style="height: 25px;" id="txtTWR_TOPOINT_FLY_HOUR" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtTWR_FLY" style="font-size: 12px;">Giờ nhận huấn lệnh cất cánh</label><br />
                                        <input style="height: 25px;" id="txtTWR_FLY" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="txtTWR_APP" style="font-size: 12px;">Giờ cất cánh thực tế</label><br />
                                        <input style="height: 25px;" id="txtTWR_APP" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtTWR_FLY_REALY" style="font-size: 12px;">Giờ chuyển giao từ TWR cho APP</label><br />
                                        <input style="height: 25px;" id="txtTWR_FLY_REALY" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td style="width: 50%">
                        <fieldset class="box-border">
                            <legend class="box-border">Khu vực kiểm soát tiếp cận</legend>
                            <table style="width: 100%; border: 0px solid black;">
                                <tr>
                                    <td>
                                        <label for="txtAPPROACH_TRANSFER" style="font-size: 12px;">Giờ nhận chuyển giao(TIẾP CẬN)</label><br />
                                        <input id="txtAPPROACH_TRANSFER" data-control="update" maxlength="6" type="text"
                                            class="wid_80px" style="height: 25px;" />
                                    </td>
                                    <td>
                                        <label for="txtAPPROACH_POINT" style="font-size: 12px;">Giờ qua điểm chuyển giao(TIẾP CẬN)</label><br />
                                        <input style="height: 25px;" id="txtAPPROACH_POINT" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtAPPROACH_NEXTAREA" style="font-size: 12px;">Giờ chuyển giao cho khu vực tiếp theo(TIẾP CẬN)</label><br />
                                        <input style="height: 25px;" id="txtAPPROACH_NEXTAREA" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="txtAPPROACH_RADAR" style="font-size: 12px;">Giờ nhận chuyển giao(RADAR)</label><br />
                                        <input id="txtAPPROACH_RADAR" data-control="update" maxlength="6" type="text"
                                            class="wid_80px" style="height: 25px;" />
                                    </td>
                                    <td>
                                        <label for="txtAPPROACH_RADAR_POINT" style="font-size: 12px;">Giờ qua điểm chuyển giao(RADAR)</label><br />
                                        <input style="height: 25px;" id="txtAPPROACH_RADAR_POINT" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtAPPROACH_RADAR_NEXTAREA" style="font-size: 12px;">Giờ chuyển giao cho khu vực tiếp theo(RADAR)</label><br />
                                        <input style="height: 25px;" id="txtAPPROACH_RADAR_NEXTAREA" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                </tr>

                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr valign="top">
                    <td style="width: 50%">
                        <fieldset class="box-border">
                            <legend class="box-border">Khu vực kiểm soát đường dài</legend>
                            <table style="width: 100%; border: 0px solid black;">
                                <tr>
                                    <td>
                                        <label for="txtDISTANCE_APP_TRANSFER" style="font-size: 12px;">Giờ nhận chuyển giao(APP)</label><br />
                                        <input id="txtDISTANCE_APP_TRANSFER" data-control="update" maxlength="6" type="text"
                                            class="wid_80px" style="height: 25px;" />
                                    </td>
                                    <td>
                                        <label for="txtDISTANCE_APP_POINT" style="font-size: 12px;">Giờ qua điểm chuyển giao(APP)</label><br />
                                        <input style="height: 25px;" id="txtDISTANCE_APP_POINT" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtDISTANCE_APP_NEXTAREA" style="font-size: 12px;">Giờ chuyển giao cho khu vực tiếp theo(APP)</label><br />
                                        <input style="height: 25px;" id="txtDISTANCE_APP_NEXTAREA" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="txtDISTANCE_ADJACENT" style="font-size: 12px;">Giờ nhận chuyển giao(KẾ CẬN)</label><br />
                                        <input id="txtDISTANCE_ADJACENT" data-control="update" maxlength="6" type="text"
                                            class="wid_80px" style="height: 25px;" />
                                    </td>
                                    <td>
                                        <label for="txtDISTANCE_ADJACENT_POINT" style="font-size: 12px;">Giờ qua điểm chuyển giao(KẾ CẬN)</label><br />
                                        <input style="height: 25px;" id="txtDISTANCE_ADJACENT_POINT" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtDISTANCE_ADJACENT_NEXTAREA" style="font-size: 12px;">Giờ chuyển giao cho khu vực tiếp theo(KẾ CẬN)</label><br />
                                        <input style="height: 25px;" id="txtDISTANCE_ADJACENT_NEXTAREA" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="txtDISTANCE_NEXT_TRANSFER" style="font-size: 12px;">Giờ nhận chuyển giao(TIẾP THEO)</label><br />
                                        <input id="txtDISTANCE_NEXT_TRANSFER" data-control="update" maxlength="6" type="text"
                                            class="wid_80px" style="height: 25px;" />
                                    </td>
                                    <td>
                                        <label for="txtDISTANCE_NEXT_POINT" style="font-size: 12px;">Giờ qua điểm chuyển giao(TIẾP THEO)</label><br />
                                        <input style="height: 25px;" id="txtDISTANCE_NEXT_POINT" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtDISTANCE_NEXT_NEXTAREA" style="font-size: 12px;">Giờ chuyển giao cho khu vực tiếp theo(TIẾP THEO)</label><br />
                                        <input style="height: 25px;" id="txtDISTANCE_NEXT_NEXTAREA" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td style="width: 50%">
                        <fieldset class="box-border">
                            <legend class="box-border">Khu vực kiểm soát tiếp cận</legend>
                            <table style="width: 100%; border: 0px solid black;">
                                <tr>
                                    <td>
                                        <label for="txtAPPROACH_TRANSFER_1" style="font-size: 12px;">Giờ nhận chuyển giao (nếu không trùng với giờ chuyển giao)(RADAR)</label><br />
                                        <input id="txtAPPROACH_TRANSFER_1" data-control="update" maxlength="6" type="text"
                                            class="wid_80px" style="height: 25px;" />
                                    </td>
                                    <td>
                                        <label for="txtAPPROACH_POINT_1" style="font-size: 12px;">Giờ qua điểm chuyển giao (tên điểm chuyển giao)(RADAR)</label><br />
                                        <input style="height: 25px;" id="txtAPPROACH_POINT_1" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtAPPROACH_NEXTAREA_1" style="font-size: 12px;">Giờ chuyển giao cho khu vực tiếp theo(RADAR)</label><br />
                                        <input style="height: 25px;" id="txtAPPROACH_NEXTAREA_1" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="txtAPPROACH_RADAR_1" style="font-size: 12px;">Giờ nhận chuyển giao (nếu không trùng với giờ chuyển giao)(TIẾP CẬN)</label><br />
                                        <input id="txtAPPROACH_RADAR_1" data-control="update" maxlength="6" type="text"
                                            class="wid_80px" style="height: 25px;" />
                                    </td>
                                    <td>
                                        <label for="txtAPPROACH_RADAR_POINT_1" style="font-size: 12px;">Giờ qua điểm chuyển giao (tên điểm chuyển giao)(TIẾP CẬN)</label><br />
                                        <input style="height: 25px;" id="txtAPPROACH_RADAR_POINT_1" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtAPPROACH_RADAR_NEXTAREA_1" style="font-size: 12px;">Giờ chuyển giao cho khu vực tiếp theo(TIẾP CẬN)</label><br />
                                        <input style="height: 25px;" id="txtAPPROACH_RADAR_NEXTAREA_1" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                </tr>

                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr valign="top">
                    <td style="width: 50%">
                        <fieldset class="box-border">
                            <legend class="box-border">Khu vực kiểm soát TWR</legend>
                            <table style="width: 100%; border: 0px solid black;">
                                <tr>
                                    <td>
                                        <label for="txtTWR_APP_OVER" style="font-size: 12px;">Giờ nhận chuyển giao tàu bay từ APP (nếu không trùng với giờ chuyển giao)</label><br />
                                        <input id="txtTWR_APP_OVER" data-control="update" maxlength="6" type="text"
                                            class="wid_80px" style="height: 25px;" />
                                    </td>
                                    <td>
                                        <label for="txtTWR_TRANSFER_POINT" style="font-size: 12px;">Giờ qua điểm chuyển giao (tên điểm chuyển giao)</label><br />
                                        <input style="height: 25px;" id="txtTWR_TRANSFER_POINT" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtTWR_TOUCH_DOWN" style="font-size: 12px;">Giờ hạ cánh (Touch down)</label><br />
                                        <input style="height: 25px;" id="txtTWR_TOUCH_DOWN" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="txtTWR_CHC_OVER" style="font-size: 12px;">Giờ tàu thoát ly khỏi đường CHC</label><br />
                                        <input style="height: 25px;" id="txtTWR_CHC_OVER" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtTWR_GCU" style="font-size: 12px;">Giờ chuyển giao cho GCU</label><br />
                                        <input style="height: 25px;" id="txtTWR_GCU" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td style="width: 50%">
                        <fieldset class="box-border">
                            <legend class="box-border">Khu vực tại sân</legend>
                            <table style="width: 100%; border: 0px solid black;">
                                <tr>
                                    <td>
                                        <label for="txtAREA_START_UP_OVER" style="font-size: 12px;">Giờ vào đến vị trí đỗ hoặc cầu hành khách</label><br />
                                        <input id="txtAREA_START_UP_OVER" data-control="update" maxlength="6" type="text"
                                            class="wid_80px" style="height: 25px;" />
                                    </td>
                                    <td>
                                        <label for="txtAREA_SHUT_DOWN" style="font-size: 12px;">Tắt máy (Shut down)</label><br />
                                        <input style="height: 25px;" id="txtAREA_SHUT_DOWN" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtAREA_AIBT" style="font-size: 12px;">Giờ đóng chèn thực tế (AIBT)</label><br />
                                        <input style="height: 25px;" id="txtAREA_AIBT" data-control="update" maxlength="6" type="text" class="wid_80px" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <div class="row" style="text-align: center">
                <button id="btnUpdatePermMaster" class="btn btn-sm btn-primary" type="button" onclick="btnUpdateOnclick()">Update</button>
                <a href="<%= Page.ResolveUrl("~/Atkl/Atkl.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString()) %>"
                    class="btn btn-sm btn-primary">
                    <span class="ace-icon fa fa-times"></span>
                    Exit
                </a>
            </div>
            <script src="<%=Global.ApplicationPath%>/Scripts/CustomDynamic.js"></script>
            <script>
                function ReadInfoPerm(data) {
                    var obj = JSON.parse(data.replace(/\n/gi, '<br>').replace(/\\/gi, '\\\\').replace(/\t/gi, '     '));

                    txtFPL_START.value = obj['FPL_START'];
                    txtFPL_SUM.value = obj['FPL_SUM'];

                    txtAREA_PARKING.value = obj['AREA_PARKING'];
                    txtAREA_EOBT.value = obj['AREA_EOBT'];
                    txtAREA_AOBT.value = obj['AREA_AOBT'];
                    txtAREA_START_UP.value = obj['AREA_PUSH_BACK'];
                    txtAREA_PUSH_BACK.value = obj['AREA_PUSH_BACK'];
                    txtAREA_START_RUN.value = obj['AREA_START_RUN'];
                    txtAREA_TWR_APP.value = obj['AREA_TWR_APP'];

                    txtTWR_CHC.value = obj['TWR_CHC'];
                    txtTWR_TOPOINT_FLY_HOUR.value = obj['TWR_TOPOINT_FLY_HOUR'];
                    txtTWR_FLY.value = obj['TWR_FLY'];
                    txtTWR_APP.value = obj['TWR_APP'];
                    txtTWR_FLY_REALY.value = obj['TWR_FLY_REALY'];

                    txtAPPROACH_TRANSFER.value = obj['APPROACH_TRANSFER'];
                    txtAPPROACH_POINT.value = obj['APPROACH_POINT'];
                    txtAPPROACH_NEXTAREA.value = obj['APPROACH_NEXTAREA'];
                    txtAPPROACH_RADAR.value = obj['APPROACH_RADAR'];
                    txtAPPROACH_RADAR_POINT.value = obj['APPROACH_RADAR_POINT'];
                    txtAPPROACH_RADAR_NEXTAREA.value = obj['APPROACH_RADAR_NEXTAREA'];

                    txtDISTANCE_APP_TRANSFER.value = obj['DISTANCE_APP_TRANSFER'];
                    txtDISTANCE_APP_POINT.value = obj['DISTANCE_APP_POINT'];
                    txtDISTANCE_APP_NEXTAREA.value = obj['DISTANCE_APP_NEXTAREA'];
                    txtDISTANCE_ADJACENT.value = obj['DISTANCE_ADJACENT'];
                    txtDISTANCE_ADJACENT_POINT.value = obj['DISTANCE_ADJACENT_POINT'];
                    txtDISTANCE_ADJACENT_NEXTAREA.value = obj['DISTANCE_ADJACENT_NEXTAREA'];
                    txtDISTANCE_NEXT_TRANSFER.value = obj['DISTANCE_NEXT_TRANSFER'];
                    txtDISTANCE_NEXT_POINT.value = obj['DISTANCE_NEXT_POINT'];
                    txtDISTANCE_NEXT_NEXTAREA.value = obj['DISTANCE_NEXT_NEXTAREA'];

                    txtAPPROACH_TRANSFER_1.value = obj['APPROACH_TRANSFER_1'];
                    txtAPPROACH_POINT_1.value = obj['APPROACH_POINT_1'];
                    txtAPPROACH_NEXTAREA_1.value = obj['APPROACH_NEXTAREA_1'];
                    txtAPPROACH_RADAR_1.value = obj['APPROACH_RADAR_1'];
                    txtAPPROACH_RADAR_POINT_1.value = obj['APPROACH_RADAR_POINT_1'];
                    txtAPPROACH_RADAR_NEXTAREA_1.value = obj['APPROACH_RADAR_NEXTAREA_1'];

                    txtTWR_APP_OVER.value = obj['TWR_APP_OVER'];
                    txtTWR_TRANSFER_POINT.value = obj['TWR_TRANSFER_POINT'];
                    txtTWR_TOUCH_DOWN.value = obj['TWR_TOUCH_DOWN'];
                    txtTWR_CHC_OVER.value = obj['TWR_CHC_OVER'];
                    txtTWR_GCU.value = obj['TWR_GCU'];

                    txtAREA_START_UP_OVER.value = obj['AREA_START_UP_OVER'];
                    txtAREA_SHUT_DOWN.value = obj['AREA_SHUT_DOWN'];
                    txtAREA_AIBT.value = obj['AREA_AIBT'];
                }
                function DisplayResult(resulf, context) {
                    if (context == 'GetOneFlight') {
                        ReadInfoPerm(resulf);
                    }
                    if (context == 'btnUpdateOnclick') {
                        alert(resulf);
                    }
                }
                function ReadObj() {
                    var obj = {
                        ID: $('#atkl_id').html(),

                        FPL_START: txtFPL_START.value,
                        FPL_SUM: txtFPL_SUM.value,

                        AREA_PARKING: txtAREA_PARKING.value,
                        AREA_EOBT: txtAREA_EOBT.value,
                        AREA_AOBT: txtAREA_AOBT.value,
                        AREA_START_UP: txtAREA_START_UP.value,
                        AREA_PUSH_BACK: txtAREA_PUSH_BACK.value,
                        AREA_START_RUN: txtAREA_START_RUN.value,
                        AREA_TWR_APP: txtAREA_TWR_APP.value,
                        //DATE_EXPLOIT: Date.now,
                        TWR_CHC: txtTWR_CHC.value,
                        TWR_TOPOINT_FLY_HOUR: txtTWR_TOPOINT_FLY_HOUR.value,
                        TWR_APP: txtTWR_APP.value,
                        TWR_FLY: txtTWR_FLY.value,
                        TWR_FLY_REALY: txtTWR_FLY_REALY.value,

                        APPROACH_TRANSFER: txtAPPROACH_TRANSFER.value,
                        APPROACH_POINT: txtAPPROACH_POINT.value,
                        APPROACH_NEXTAREA: txtAPPROACH_NEXTAREA.value,
                        APPROACH_RADAR: txtAPPROACH_RADAR.value,
                        APPROACH_RADAR_POINT: txtAPPROACH_RADAR_POINT.value,
                        APPROACH_RADAR_NEXTAREA: txtAPPROACH_RADAR_NEXTAREA.value,

                        DISTANCE_APP_TRANSFER: txtDISTANCE_APP_TRANSFER.value,
                        DISTANCE_APP_POINT: txtDISTANCE_APP_POINT.value,
                        DISTANCE_APP_NEXTAREA: txtDISTANCE_APP_NEXTAREA.value,
                        DISTANCE_ADJACENT: txtDISTANCE_ADJACENT.value,
                        DISTANCE_ADJACENT_POINT: txtDISTANCE_ADJACENT_POINT.value,
                        DISTANCE_ADJACENT_NEXTAREA: txtDISTANCE_ADJACENT_NEXTAREA.value,
                        DISTANCE_NEXT_TRANSFER: txtDISTANCE_NEXT_TRANSFER.value,
                        DISTANCE_NEXT_POINT: txtDISTANCE_NEXT_POINT.value,
                        DISTANCE_NEXT_NEXTAREA: txtDISTANCE_NEXT_NEXTAREA.value,

                        APPROACH_TRANSFER_1: txtAPPROACH_TRANSFER_1.value,
                        APPROACH_POINT_1: txtAPPROACH_POINT_1.value,
                        APPROACH_NEXTAREA_1: txtAPPROACH_NEXTAREA_1.value,
                        APPROACH_RADAR_1: txtAPPROACH_RADAR_1.value,
                        APPROACH_RADAR_POINT_1: txtAPPROACH_RADAR_POINT_1.value,
                        APPROACH_RADAR_NEXTAREA_1: txtAPPROACH_RADAR_NEXTAREA_1.value,

                        TWR_APP_OVER: txtTWR_APP_OVER.value,
                        TWR_TRANSFER_POINT: txtTWR_TRANSFER_POINT.value,
                        TWR_TOUCH_DOWN: txtTWR_TOUCH_DOWN.value,
                        TWR_CHC_OVER: txtTWR_CHC_OVER.value,
                        TWR_GCU: txtTWR_GCU.value,

                        AREA_START_UP_OVER: txtAREA_START_UP_OVER.value,
                        AREA_SHUT_DOWN: txtAREA_SHUT_DOWN.value,
                        AREA_AIBT: txtAREA_AIBT.value
                    };
                    return obj;
                }
                function btnUpdateOnclick() {
                    if ($('#atkl_id').html() != '') {
                        GetArgWithPostBack(JSON.stringify(ReadObj()) + '_____btnUpdateOnclick', 'btnUpdateOnclick');
                    }
                    else {
                        alert('Please select Flight');
                        return;
                    }
                }
            </script>
</asp:Content>
