<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="Atkl.aspx.cs" Inherits="prjApplication.Atkl.Atkl" EnableEventValidation="false" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../../Style/StyleReports.css" rel="stylesheet" />
    <link href="../Style/assets/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/bootstrap-datepicker3.min.css" rel="stylesheet" />

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

    <span class="TitlePanel">+ An toàn không lưu</span>
    <div class="classSearchHeader">
        <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
            <tr>
                <td style="width: 7%; text-align: right;">START DATE
                </td>
                <td style="width: 10%; text-align: left;">
                    <input id="txtStartDate" runat="server" data-date-format="dd/mm/yyyy"
                        class="inputControl date-picker mControl"
                        type="text" />

                </td>
                <td style="width: 7%; text-align: right;">END DATE
                </td>
                <td style="width: 10%; text-align: left;">
                    <input id="txtFinishDate" runat="server" data-date-format="dd/mm/yyyy"
                        class="inputControl date-picker mControl"
                        type="text" />

                </td>

                <td style="width: 25%; text-align: left;">

                    <asp:LinkButton runat="server" OnClientClick="return confirm('Do you want confirm');" Style="padding: 8px 24px;" ID="btnimport" OnClick="btnimport_Click" CssClass="btn btn-sm btn-primary btn-bold">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    GetData
                    </asp:LinkButton>

                    <asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind"
                        Font-Bold="true" Text="" Width="40px" Height="38px" OnClick="btnSearch_Click"></asp:Button>
                    <asp:LinkButton Visible="false" runat="server" Style="padding: 8px 24px;" ID="btnExportPdf" CssClass="btn btn-sm btn-primary btn-bold">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Pdf
                    </asp:LinkButton>

                    <asp:LinkButton runat="server" ID="btnExportExel" Style="padding: 8px 24px;" OnClick="btnExcel_Click" CssClass="btn btn-sm btn-primary btn-bold">
                                                    <span class="glyphicon glyphicon-download-alt" ></span>
                                                    Excel
                    </asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>

    <div class="table-responsive">
        <asp:GridView ID="grdSource" runat="server" CssClass="table table-bordered table-hover"
            AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <div>
                        </div>
                        <tr>
                            <th rowspan="3">STT</th>
                            <th rowspan="3">Ngày khai thác</th>
                            <th rowspan="3">Loại tàu bay</th>
                            <th rowspan="3">Đăng bạ</th>
                            <th rowspan="3">Tên gọi chuyến bay</th>
                            <th rowspan="3">Sân bay khởi hành</th>
                            <th rowspan="3">Sân bay Đến</th>
                            <th colspan="4">Giờ dự kiến theo phép bay</th>
                            <th colspan="2">Giờ theo FPL</th>
                            <th colspan="7">Khu vực tại sân</th>
                            <th colspan="5">Khu vực kiểm soát TWR</th>
                            <th colspan="6">Khu vực kiểm soát tiếp cận</th>
                            <th colspan="9">Khu vực kiểm soát đường dài</th>
                            <th colspan="6">Khu vực kiểm soát tiếp cận</th>
                            <th colspan="5">Khu vực kiểm soát TWR</th>
                            <th colspan="3">Khu vực tại sân</th>
                            <th colspan="2">Giờ khai thác thực tế theo điện văn DEP, AAR</th>
                            <th colspan="2">Đường bay</th>
                            <th rowspan="3">Ghi chú</th>
                            <th rowspan="3">Điện văn</th>
                        </tr>
                        <tr>
                            <td colspan="2">Theo phép cấp lần đầu</td>
                            <td colspan="2">Theo sửa đổi, bổ sung (nếu có)</td>
                            <td rowspan="2">Giờ khởi hành</td>
                            <td rowspan="2">Tổng thời gian đến sân bay đến</td>
                            <td rowspan="2">Vị trí đỗ tàu bay</td>
                            <td rowspan="2">Giờ dự kiến rút chèn (EOBT) do KSVKL cấp (nếu có)</td>
                            <td rowspan="2">Giờ rút chèn thực tế (AOBT)</td>
                            <td rowspan="2">Giờ nổ máy (Start-up)</td>
                            <td rowspan="2">Giờ đẩy ra (Push-back)</td>
                            <td rowspan="2">Giờ bắt đầu lăn</td>
                            <td rowspan="2">Giờ  GCU chuyển giao cho TWR</td>
                            <td rowspan="2">Giờ đến diểm chờ lên đường CHC (diểm gần đường CHC nhất)</td>
                            <td rowspan="2">Giờ đến điểm chờ nhận huấn lệnh cất cánh</td>
                            <td rowspan="2">Giờ nhận huấn lệnh cất cánh</td>
                            <td rowspan="2">Giờ cất cánh thực tế</td>
                            <td rowspan="2">Giờ chuyển giao từ TWR cho APP </td>
                            <td colspan="3">Khu vực kiểm soát tiếp cận</td>
                            <td colspan="3">Khu vực kiểm soát radar (nếu có)</td>
                            <td colspan="3">Phân khu nhận chuyển giao tàu bay từ APP</td>
                            <td colspan="3">Phân khu kế cận</td>
                            <td colspan="3">Phân khu tiếp theo</td>
                            <td colspan="3">Khu vực kiểm soát radar (nếu có)</td>
                            <td colspan="3">Khu vực kiểm soát tiếp cận (nếu có)</td>
                            <td rowspan="2">Giờ nhận chuyển giao tàu bay từ APP  (nếu không trùng với giờ chuyển giao)</td>
                            <td rowspan="2">Giờ qua điểm chuyển giao (tên điểm chuyển giao)</td>
                            <td rowspan="2">Giờ hạ cánh (Touch down)</td>
                            <td rowspan="2">Giờ tàu thoát ly khỏi đường CHC</td>
                            <td rowspan="2">Giờ chuyển giao cho GCU</td>
                            <td rowspan="2">Giờ vào đến vị trí đỗ hoặc cầu hành khách</td>
                            <td rowspan="2">Tắt máy (Shut down)</td>
                            <td rowspan="2">Giờ đóng chèn thực tế (AIBT)</td>
                            <td rowspan="2">ATD</td>
                            <td rowspan="2">ATA</td>
                            <td rowspan="2">Theo phép bay</td>
                            <td rowspan="2">Thực tế</td>
                        </tr>
                        <tr>
                            <td>ETD</td>
                            <td>ETA</td>
                            <td>ETD</td>
                            <td>ETA</td>
                            <td>Giờ nhận chuyển giao (nếu không trùng với giờ chuyển giao)</td>
                            <td>Giờ qua điểm chuyển giao (tên điểm chuyển giao)</td>
                            <td>Giờ chuyển giao cho khu vực tiếp theo </td>
                            <td>Giờ nhận chuyển giao (nếu không trùng với giờ chuyển giao)</td>
                            <td>Giờ qua điểm chuyển giao (tên điểm chuyển giao)</td>
                            <td>Giờ chuyển giao cho khu vực tiếp theo</td>
                            <td>Giờ nhận chuyển giao (nếu không trùng với giờ chuyển giao)</td>
                            <td>Giờ qua điểm chuyển giao (tên điểm chuyển giao)</td>
                            <td>Giờ chuyển giao cho khu vực tiếp theo</td>
                            <td>Giờ nhận chuyển giao (nếu không trùng với giờ chuyển giao)</td>
                            <td>Giờ qua điểm chuyển giao (tên điểm chuyển giao)</td>
                            <td>Giờ chuyển giao cho khu vực tiếp theo</td>
                            <td>Giờ nhận chuyển giao (nếu không trùng với giờ chuyển giao)</td>
                            <td>Giờ qua điểm chuyển giao (tên điểm chuyển giao)</td>
                            <td>Giờ chuyển giao cho khu vực tiếp theo</td>
                            <td>Giờ nhận chuyển giao (nếu không trùng với giờ chuyển giao)</td>
                            <td>Giờ qua điểm chuyển giao (tên điểm chuyển giao)</td>
                            <td>Giờ chuyển giao cho khu vực tiếp theo</td>
                            <td>Giờ nhận chuyển giao (nếu không trùng với giờ chuyển giao)</td>
                            <td>Giờ qua điểm chuyển giao (tên điểm chuyển giao)</td>
                            <td>Giờ chuyển giao cho khu vực tiếp theo</td>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Container.DataItemIndex +1 %>
                        <a data-toggle="tooltip" title='<%# "Edit: " + Eval("ID") %>' runat="server"
                            visible='true'>
                            <i class="ace-icon fa fa-pencil bigger-130" onclick="Edit('<%# Eval("ID") %>')"></i>
                        </a>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "DATE_EXPLOIT","{0:dd/MM/yyyy}") %>

                        </td>
                        <td>
                            <%# Eval("CRAFT_TYPE") %>
                        </td>
                        <td>
                            <%# Eval("REGISTRATION") %>
                        </td>
                        <td>
                            <%# Eval("CALLSIGN") %>
                            <%-- <asp:LinkButton ID="btnEdit" runat="server" CssClass="linkGridForm" Text='<%# Eval( "CALLSIGN") %>'
                                    Enabled='<%#_Role.R_Edit %>' CommandArgument='<%# Eval("ID") %>' CommandName="Edit"></asp:LinkButton>--%>
                        </td>
                        <td>
                            <%# Eval("FROM_AIRPORT") %>
                        </td>
                        <td>
                            <%# Eval("TO_AIRPORT") %>
                        </td>
                        <td>
                            <%# Eval("ETD_FIRST") %>
                        </td>
                        <td>
                            <%# Eval("ETA_FIRST") %>
                        </td>
                        <td>
                            <%# Eval("ETD_CHANGE") %>
                        </td>
                        <td>
                            <%# Eval("ETA_CHANGE") %>
                        </td>
                        <td>
                            <%# Eval("FPL_START") %>
                        </td>
                        <td>
                            <%# Eval("FPL_SUM").ToString() %>
                        </td>
                        <td>
                            <%# Eval("AREA_PARKING") %>
                        </td>
                        <td>
                            <%# Eval("AREA_EOBT") %>
                        </td>
                        <td>
                            <%# Eval("AREA_AOBT") %>
                        </td>
                        <td>
                            <%# Eval("AREA_START_UP") %>
                        </td>
                        <td>
                            <%# Eval("AREA_PUSH_BACK") %>
                        </td>
                        <td>
                            <%# Eval("AREA_START_RUN") %>
                        </td>
                        <td>
                            <%# Eval("AREA_TWR_APP") %>
                        </td>
                        <td>
                            <%# Eval("TWR_CHC") %>
                        </td>
                        <td>
                            <%# Eval("TWR_TOPOINT_FLY_HOUR") %>
                        </td>
                        <td>
                            <%# Eval("TWR_FLY") %>
                        </td>
                        <td>
                            <%# Eval("TWR_APP") %>
                        </td>
                        <td>
                            <%# Eval("TWR_FLY_REALY") %>
                        </td>
                        <td>
                            <%# Eval("APPROACH_TRANSFER") %>
                        </td>
                        <td>
                            <%# Eval("APPROACH_POINT") %>
                        </td>
                        <td>
                            <%# Eval("APPROACH_NEXTAREA") %>
                        </td>
                        <td>
                            <%# Eval("APPROACH_RADAR") %>
                        </td>
                        <td>
                            <%# Eval("APPROACH_RADAR_POINT") %>
                        </td>
                        <td>
                            <%# Eval("APPROACH_RADAR_NEXTAREA") %>
                        </td>
                        <td>
                            <%# Eval("DISTANCE_APP_TRANSFER") %>
                        </td>
                        <td>
                            <%# Eval("DISTANCE_APP_POINT") %>
                        </td>
                        <td>
                            <%# Eval("DISTANCE_APP_NEXTAREA") %>
                        </td>
                        <td>
                            <%# Eval("DISTANCE_ADJACENT") %>
                        </td>
                        <td>
                            <%# Eval("DISTANCE_ADJACENT_POINT") %>
                        </td>
                        <td>
                            <%# Eval("DISTANCE_ADJACENT_NEXTAREA") %>
                        </td>
                        <td>
                            <%# Eval("DISTANCE_NEXT_TRANSFER") %>
                        </td>
                        <td>
                            <%# Eval("DISTANCE_NEXT_POINT") %>
                        </td>
                        <td>
                            <%# Eval("DISTANCE_NEXT_NEXTAREA") %>
                        </td>
                        <td>
                            <%# Eval("APPROACH_TRANSFER_1") %>
                        </td>
                        <td>
                            <%# Eval("APPROACH_POINT_1") %>
                        </td>
                        <td>
                            <%# Eval("APPROACH_NEXTAREA_1") %>
                        </td>
                        <td>
                            <%# Eval("APPROACH_RADAR_1") %>
                        </td>
                        <td>
                            <%# Eval("APPROACH_RADAR_POINT_1") %>
                        </td>
                        <td>
                            <%# Eval("APPROACH_RADAR_NEXTAREA_1") %>
                        </td>
                        <td>
                            <%# Eval("TWR_APP_OVER") %>
                        </td>
                        <td>
                            <%# Eval("TWR_TRANSFER_POINT") %>
                        </td>
                        <td>
                            <%# Eval("TWR_TOUCH_DOWN") %>
                        </td>
                        <td>
                            <%# Eval("TWR_CHC_OVER") %>
                        </td>
                        <td>
                            <%# Eval("TWR_GCU") %>
                        </td>
                        <td>
                            <%# Eval("AREA_START_UP_OVER") %>
                        </td>
                        <td>
                            <%# Eval("AREA_SHUT_DOWN") %>
                        </td>
                        <td>
                            <%# Eval("AREA_AIBT") %>
                        </td>
                        <td>
                            <%# Eval("ATD") %>
                        </td>
                        <td>
                            <%# Eval("ATA") %>
                        </td>
                        <td>
                            <%# Eval("ROUTE_PERMISSION") %>
                        </td>
                        <td>
                            <%# Eval("ROUTE_REALITY") %>
                        </td>
                        <td>
                            <%# Eval("REMARK") %>
                        </td>
                        <td>
                            <%# Eval("REMARK") %>
                        </td>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <%--<div style="text-align: right">
        <cc1:CustomPaging ID="CustomPaging1" runat="server" NumberViewPage="7" PageSize="500" />
    </div>--%>
    <div style="text-align: right; float:left" >
        <cc1:PhanTrang ID="PhanTrang1" runat="server" PageSize="100" OnPaging_IndexChange="PhanTrang1_Paging_IndexChange" />
    </div>

    <div id="popupEdit1" class="modal cssHide" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="ClosePopup('popupEdit1');" class="close" data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">Edit flight</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_ID">Vị trí đỗ tàu bay</label>
                                <input id="txtAREA_PARKING" data-minlenght="1" data-number="true" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_NAME">Giờ dự kiến rút chèn (EOBT)</label>
                                <input id="txtAREA_EOBT" data-minlenght="1" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ rút chèn thực tế (AOBT)</label>
                                <input id="txtAREA_AOBT" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ nổ máy (Start-up)</label>
                                <input id="txtAREA_START_UP" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ đẩy ra (Push-back)</label>
                                <input id="txtAREA_PUSH_BACK" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ bắt đầu lăn</label>
                                <input id="txtAREA_START_RUN" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ  GCU chuyển giao cho TWR</label>
                                <input id="txtAREA_TWR_APP" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                        </div>
                    </div>
                    <div class="row" style="text-align: center">
                        <button id="btnCreate1" type="button" onclick="btnCreateOnclick();"
                            class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-check"></i>
                            Create
                        </button>
                        <button id="btnCancel" type="button" onclick="ClosePopup('popupEditFlight');" runat="server"
                            data-dismiss="modal" class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-times"></i>
                            Save
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="popupEdit2" class="modal cssHide" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="ClosePopup('popupEdit2');" class="close" data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">Edit flight</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_ID">Giờ đến diểm chờ lên đường CHC</label>
                                <input id="txtTWR_CHC" data-minlenght="1" data-number="true" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_NAME">Giờ đến điểm chờ nhận huấn lệnh cất cánh</label>
                                <input id="txtTWR_TOPOINT_FLY_HOUR" data-minlenght="1" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ nhận huấn lệnh cất cánh</label>
                                <input id="txtTWR_FLY" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ cất cánh thực tế</label>
                                <input id="txtTWR_APP" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ chuyển giao từ TWR cho APP</label>
                                <input id="txtTWR_FLY_REALY" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                        </div>
                    </div>
                    <div class="row" style="text-align: center">
                        <button id="Button1" type="button" onclick="ClosePopup('popupEditFlight');" runat="server"
                            data-dismiss="modal" class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-times"></i>
                            Save
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="popupEdit3" class="modal cssHide" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="ClosePopup('popupEdit3');" class="close" data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">Edit flight</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_ID">Giờ nhận chuyển giao(TIẾP CẬN)</label>
                                <input id="txtAPPROACH_TRANSFER" data-minlenght="1" data-number="true" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_NAME">Giờ qua điểm chuyển giao(TIẾP CẬN)</label>
                                <input id="txtAPPROACH_POINT" data-minlenght="1" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ chuyển giao cho khu vực tiếp theo(TIẾP CẬN)</label>
                                <input id="txtAPPROACH_NEXTAREA" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ nhận chuyển giao(RADAR)</label>
                                <input id="txtAPPROACH_RADAR" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ qua điểm chuyển giao(RADAR)</label>
                                <input id="txtAPPROACH_RADAR_POINT" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ chuyển giao cho khu vực tiếp theo(RADAR)</label>
                                <input id="txtAPPROACH_RADAR_NEXTAREA" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                        </div>
                    </div>
                    <div class="row" style="text-align: center">
                        <button id="Button2" type="button" onclick="ClosePopup('popupEditFlight');" runat="server"
                            data-dismiss="modal" class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-times"></i>
                            Save
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="popupEdit4" class="modal cssHide" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="ClosePopup('popupEdit4');" class="close" data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">Edit flight</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_ID">Giờ nhận chuyển giao(KHU APP)</label>
                                <input id="txtDISTANCE_APP_TRANSFER" data-minlenght="1" data-number="true" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_NAME">Giờ qua điểm chuyển giao(KHU APP)</label>
                                <input id="txtDISTANCE_APP_POINT" data-minlenght="1" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ chuyển giao cho khu vực tiếp theo(KHU APP)</label>
                                <input id="txtDISTANCE_APP_NEXTAREA" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ nhận chuyển giao(KHU KẾ CẬN)</label>
                                <input id="txtDISTANCE_ADJACENT" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ qua điểm chuyển giao(KHU KẾ CẬN)</label>
                                <input id="txtDISTANCE_ADJACENT_POINT" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ chuyển giao cho khu vực tiếp theo(KHU KẾ CẬN)</label>
                                <input id="txtDISTANCE_ADJACENT_NEXTAREA" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ nhận chuyển giao(KHU TIẾP THEO)</label>
                                <input id="txtDISTANCE_NEXT_TRANSFER" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ qua điểm chuyển giao(KHU TIẾP THEO)</label>
                                <input id="txtDISTANCE_NEXT_POINT" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ chuyển giao cho khu vực tiếp theo(KHU TIẾP THEO)</label>
                                <input id="txtDISTANCE_NEXT_NEXTAREA" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                        </div>
                    </div>
                    <div class="row" style="text-align: center">
                        <button id="Button3" type="button" onclick="ClosePopup('popupEditFlight');" runat="server"
                            data-dismiss="modal" class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-times"></i>
                            Save
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="popupEdit5" class="modal cssHide" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="ClosePopup('popupEdit5');" class="close" data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">Edit flight</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_ID">Giờ nhận chuyển giao(RADAR)</label>
                                <input id="txtAPPROACH_TRANSFER_1" data-minlenght="1" data-number="true" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_NAME">Giờ qua điểm chuyển giao(RADAR)</label>
                                <input id="txtAPPROACH_POINT_1" data-minlenght="1" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ chuyển giao cho khu vực tiếp theo(RADAR)</label>
                                <input id="txtAPPROACH_NEXTAREA_1" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ nhận chuyển giao(TIẾP CẬN)</label>
                                <input id="txtAPPROACH_RADAR_1" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ qua điểm chuyển giao(TIẾP CẬN)</label>
                                <input id="txtAPPROACH_RADAR_POINT_1" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">Giờ chuyển giao cho khu vực tiếp theo(TIẾP CẬN)</label>
                                <input id="txtAPPROACH_RADAR_NEXTAREA_1" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                        </div>
                    </div>
                    <div class="row" style="text-align: center">
                        <button id="Button4" type="button" onclick="ClosePopup('popupEditFlight');" runat="server"
                            data-dismiss="modal" class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-times"></i>
                            Save
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="popupEdit6" class="modal cssHide" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="ClosePopup('popupEdit6');" class="close" data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">Edit flight</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_ID">Giờ nhận chuyển giao tàu bay từ APP</label>
                                <input id="txtTWR_APP_OVER" data-minlenght="1" data-number="true" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_NAME">Giờ qua điểm chuyển giao</label>
                                <input id="txtTWR_TRANSFER_POINT" data-minlenght="1" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_NAME">Giờ hạ cánh</label>
                                <input id="txtTWR_TOUCH_DOWN" data-minlenght="1" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_NAME">Giờ tàu thoát ly khỏi đường CHC</label>
                                <input id="txtTWR_CHC_OVER" data-minlenght="1" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_NAME">Giờ chuyển giao cho GCU</label>
                                <input id="txtTWR_GCU" data-minlenght="1" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                        </div>
                    </div>
                    <div class="row" style="text-align: center">
                        <button id="btnCreate" type="button" style="display: none" onclick="btnCreateOnclick6();"
                            class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-check"></i>
                            Create
                        </button>
                        <button id="Button5" type="button" onclick="ClosePopup('popupEditFlight');" runat="server"
                            data-dismiss="modal" class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-times"></i>
                            Save
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="popupEdit7" class="modal cssHide" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="ClosePopup('popupEdit7');" class="close" data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">Edit flight</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_ID">Giờ vào đến vị trí đỗ hoặc cầu hành khách</label>
                                <input id="txtAREA_START_UP_OVER" data-minlenght="1" data-number="true" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_NAME">Tắt máy (Shut down)</label>
                                <input id="txtAREA_SHUT_DOWN" data-minlenght="1" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_NAME">Giờ đóng chèn thực tế (AIBT)</label>
                                <input id="txtAREA_AIBT" data-minlenght="1" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>

                        </div>
                    </div>
                    <div class="row" style="text-align: center">
                        <button id="Button6" type="button" onclick="ClosePopup('popupEditFlight');" runat="server"
                            data-dismiss="modal" class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-times"></i>
                            Save
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/validate.js"></script>
    <div id="dhtmltooltip"></div>
    <script>
        function Edit(id) {
            window.open('EditAtklFull.aspx?Menu_ID=<%=(Request["Menu_ID"]!=null)?Request["Menu_ID"].ToString():""%>&ID=' + id, '_parent');
        }
        function ReadObj() {
            var obj = {
                AREA_PARKING: txtAREA_PARKING.value,
                AREA_EOBT: txtAREA_EOBT.value,
                AREA_AOBT: txtAREA_AOBT.value,
                AREA_START_UP: txtAREA_START_UP.value,
                AREA_PUSH_BACK: txtAREA_PUSH_BACK.value,
                AREA_START_RUN: txtAREA_START_RUN.value,
                AREA_TWR_APP: txtAREA_TWR_APP.value
            };
            return obj;
        }
        function ClosePopup(elem) {
            IdSelect = '';
            document.getElementById(elem).className = "modal cssHide";

        }
        function CreateFlight(elem) {

            document.getElementById(elem).className = "modal cssShow";

        }
        function LoadDataGrid() {
            GetArgWithPostBack('LoadDataGrid_____LoadDataGrid', 'LoadDataGrid');
        }
        function DisplayResult(resulf, context) {
            if (context == 'LoadDataGrid') {
                document.getElementById('<%=grdSource.ClientID%>').innerHTML = resulf;
        }
        else if (context == 'btnCreateOnclick') {
            alert(resulf);
            LoadDataGrid();
        }
    }
    function btnCreateOnclick() {
        GetArgWithPostBack(JSON.stringify(ReadObj()) + '_____btnCreateOnclick', 'btnCreateOnclick');
    }
    function GetContent(id) {
        GetArgWithPostBack(id + '_____GetContent', 'GetContent');
    }
    </script>
</asp:Content>
