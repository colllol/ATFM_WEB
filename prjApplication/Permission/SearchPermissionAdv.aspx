<%@ Page Title="Search Permission Adv" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master"
    AutoEventWireup="true" CodeBehind="SearchPermissionAdv.aspx.cs"
    Inherits="prjApplication.Permission.SearchPermissionAdv" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Permission/SearchPermissionAdv.css?v=20260809-7") %>" />

    <section class="spa-page">
        <header class="spa-heading">
            <div>
                <h1>Search Permission Adv</h1>
                <p>Tìm kiếm hợp nhất phép SC và NO theo ngày cấp phép (PERMDATE)</p>
            </div>
        </header>

        <div class="spa-filter-card">
            <label class="spa-field spa-field-date" for="spaPermissionDate">
                <span>Ngày cấp phép</span>
                <input id="spaPermissionDate" type="date" />
            </label>
            <label class="spa-field spa-field-time" for="spaFromTime">
                <span>Từ giờ ETD/ETA</span>
                <input id="spaFromTime" class="spa-time-input" type="text" inputmode="numeric"
                    maxlength="5" placeholder="00:00" autocomplete="off" />
            </label>
            <label class="spa-field spa-field-time" for="spaToTime">
                <span>Đến giờ ETD/ETA</span>
                <input id="spaToTime" class="spa-time-input" type="text" inputmode="numeric"
                    maxlength="5" placeholder="23:59" autocomplete="off" />
            </label>
            <label class="spa-field spa-field-criteria" for="spaFromAirp">
                <span>From</span>
                <input id="spaFromAirp" type="text" maxlength="10" autocomplete="off" />
            </label>
            <label class="spa-field spa-field-criteria" for="spaToAirp">
                <span>To</span>
                <input id="spaToAirp" type="text" maxlength="10" autocomplete="off" />
            </label>
            <label class="spa-field spa-field-via" for="spaVia">
                <span>Via</span>
                <input id="spaVia" type="text" maxlength="100" autocomplete="off" />
            </label>
            <button id="spaSearch" class="spa-button spa-button-primary" type="button">
                <i class="fa fa-search"></i> Search
            </button>
            <button id="spaClear" class="spa-button spa-button-muted" type="button">
                <i class="fa fa-eraser"></i> Clear
            </button>
            <button id="spaExportExcel" class="spa-button spa-button-excel" type="button" disabled>
                <i class="fa fa-file-excel-o"></i> Export Excel
            </button>
        </div>

        <div class="spa-result-head">
            <strong id="spaTotal">Tổng số: 0</strong>
            <span id="spaSearchCaption">Chọn ngày cấp phép và nhấn Search.</span>
        </div>

        <div class="spa-table-wrap">
            <table class="spa-table" id="spaPermissionTable">
                <thead>
                    <tr>
                        <th>NO</th>
                        <th>PERMNBR</th>
                        <th>AUTHOR</th>
                        <th>PTYPE</th>
                        <th>FTYPE</th>
                        <th>NUMBER</th>
                        <th>VERSION</th>
                        <th>DATE</th>
                        <th>OPER</th>
                    </tr>
                    <tr class="spa-column-filters">
                        <th></th>
                        <th><input type="text" data-filter-field="PermNbr" aria-label="Filter PERMNBR" autocomplete="off" /></th>
                        <th><input type="text" data-filter-field="Author" aria-label="Filter AUTHOR" autocomplete="off" /></th>
                        <th><input type="text" data-filter-field="PType" aria-label="Filter PTYPE" autocomplete="off" /></th>
                        <th><input type="text" data-filter-field="FType" aria-label="Filter FTYPE" autocomplete="off" /></th>
                        <th><input type="text" data-filter-field="Number" aria-label="Filter NUMBER" autocomplete="off" /></th>
                        <th><input type="text" data-filter-field="Version" aria-label="Filter VERSION" autocomplete="off" /></th>
                        <th><input type="text" data-filter-field="PermissionDate" aria-label="Filter DATE" placeholder="DD-MM-YYYY" autocomplete="off" /></th>
                        <th><input type="text" data-filter-field="Oper" aria-label="Filter OPER" autocomplete="off" /></th>
                    </tr>
                </thead>
                <tbody id="spaPermissionRows">
                    <tr><td colspan="9" class="spa-empty">Chưa tải dữ liệu.</td></tr>
                </tbody>
            </table>
        </div>

        <div class="spa-pager" id="spaPager" hidden>
            <button id="spaPrev" type="button">&lt;</button>
            <span id="spaPageInfo">Trang 1/1</span>
            <button id="spaNext" type="button">&gt;</button>
        </div>
    </section>

    <div id="spaDetailModal" class="spa-modal" role="dialog" aria-modal="true"
        aria-labelledby="spaDetailTitle" hidden>
        <div class="spa-modal-dialog">
            <div class="spa-modal-header">
                <div>
                    <h2 id="spaDetailTitle">Permission detail</h2>
                    <p id="spaDetailSubtitle"></p>
                </div>
                <button id="spaDetailClose" class="spa-modal-close" type="button" aria-label="Close">&times;</button>
            </div>
            <div class="spa-modal-body">
                <div class="spa-detail-table-wrap">
                    <table class="spa-table spa-detail-table">
                        <thead>
                            <tr>
                                <th>NO</th>
                                <th>CALLSIGN</th>
                                <th>REGISTRATION</th>
                                <th>FROM</th>
                                <th>TO</th>
                                <th>ETD</th>
                                <th>ETA</th>
                                <th>DAY/DATE</th>
                                <th>BEGIN DATE</th>
                                <th>END DATE</th>
                                <th>CRAFT</th>
                                <th>PURPOSE</th>
                                <th>MTOW</th>
                                <th>VIA</th>
                                <th>REMARK</th>
                                <th>STATUS</th>
                                <th>LAST USER</th>
                            </tr>
                        </thead>
                        <tbody id="spaDetailRows"></tbody>
                    </table>
                </div>
            </div>
            <div class="spa-modal-footer">
                <button id="spaDetailCancel" class="spa-button spa-button-primary" type="button">
                    <i class="fa fa-times"></i> Close
                </button>
            </div>
        </div>
    </div>

    <script src="<%= ResolveUrl("~/Permission/SearchPermissionAdv.js?v=20260809-7") %>"></script>
</asp:Content>
