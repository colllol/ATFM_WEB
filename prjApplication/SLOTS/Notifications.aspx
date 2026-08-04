<%@ Page Title="Thông báo" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="Notifications.aspx.cs" Inherits="prjApplication.SLOTS.Notifications" %>

<asp:Content ID="NotificationsContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/SLOTS/Notifications.css?v=20260804-1") %>" />

    <section id="notificationsPage" class="notifications-page" data-endpoint="<%= ResolveUrl("~/Handlers/Notification.ashx") %>">
        <header class="notifications-page-header">
            <div>
                <h1><i class="fa fa-bell-o" aria-hidden="true"></i> Thông báo</h1>
                <p id="notificationsSummary" aria-live="polite">Đang tải dữ liệu...</p>
            </div>
            <div class="notifications-header-actions">
                <button id="notificationsRefresh" class="notifications-icon-button" type="button" title="Làm mới danh sách" aria-label="Làm mới danh sách">
                    <i class="fa fa-refresh" aria-hidden="true"></i>
                </button>
                <button id="notificationsMarkAll" class="notifications-primary-button" type="button">
                    <i class="fa fa-check" aria-hidden="true"></i><span>Đọc tất cả</span>
                </button>
            </div>
        </header>

        <div class="notifications-toolbar">
            <div class="notifications-segmented" role="group" aria-label="Lọc trạng thái thông báo">
                <button type="button" data-status="-1" class="is-active" aria-pressed="true">Tất cả</button>
                <button type="button" data-status="0" aria-pressed="false">Chưa đọc</button>
                <button type="button" data-status="1" aria-pressed="false">Đã đọc</button>
            </div>
            <span class="notifications-unread-total"><i class="fa fa-circle" aria-hidden="true"></i><strong id="notificationsUnreadCount">0</strong> chưa đọc</span>
        </div>

        <div id="notificationsError" class="notifications-error" role="alert" hidden></div>

        <section class="notifications-table-section" aria-label="Danh sách thông báo">
            <div class="notifications-table-wrap">
                <table class="notifications-table">
                    <colgroup>
                        <col class="notifications-col-status" />
                        <col class="notifications-col-title" />
                        <col class="notifications-col-content" />
                        <col class="notifications-col-time" />
                        <col class="notifications-col-action" />
                    </colgroup>
                    <thead>
                        <tr>
                            <th scope="col">Trạng thái</th>
                            <th scope="col">Tiêu đề</th>
                            <th scope="col">Nội dung</th>
                            <th scope="col">Thời gian</th>
                            <th scope="col" class="notifications-action-heading">Thao tác</th>
                        </tr>
                    </thead>
                    <tbody id="notificationsRows">
                        <tr><td colspan="5" class="notifications-empty-cell">Đang tải dữ liệu...</td></tr>
                    </tbody>
                </table>
                <div id="notificationsLoading" class="notifications-local-loading" aria-live="polite" hidden>
                    <span class="notifications-spinner" aria-hidden="true"></span><span>Đang tải thông báo...</span>
                </div>
            </div>

            <footer class="notifications-pagination">
                <span id="notificationsPageInfo">Trang 1/1</span>
                <nav id="notificationsPager" aria-label="Phân trang thông báo"></nav>
            </footer>
        </section>
    </section>

    <script src="<%= ResolveUrl("~/SLOTS/Notifications.js?v=20260723-1") %>"></script>
</asp:Content>
