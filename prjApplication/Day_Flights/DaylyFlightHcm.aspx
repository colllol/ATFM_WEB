<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="DaylyFlightHcm.aspx.cs" Inherits="prjApplication.Day_Flights.DaylyFlightHcm" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="prjBusinessLogic" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/bootstrap-datepicker3.min.css" rel="stylesheet" />
    <link href="../Style/spectrum.css" rel="stylesheet" />
    <link href="../Style/assets/css/ace-skins.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/ace-rtl.min.css" rel="stylesheet" />
    <style>
        html,
        body {
            max-width: 100%;
            overflow-x: hidden;
        }

        .sInputDb {
            border: 0px !important;
            width: 100%;
            font-weight: bold;
            color: black !important;
        }

        #table-container {
            position: relative;
            display: flex;
            width: 100%;
            max-width: 100%;
            height: clamp(420px, calc(100vh - 390px), 720px);
            min-height: 0;
            flex-direction: column;
            box-sizing: border-box;
            margin: 10px 0 20px !important;
            overflow: hidden;
            border: 1px solid #c8d9e8;
            border-radius: 10px;
            background: #fff;
            box-shadow: 0 7px 22px rgba(26, 67, 105, .13);
        }

        tr {
            cursor: pointer;
            transition: all .12s ease-in-out;
        }

        .selected {
            background-color: blue;
            font-weight: bold;
            color: #ff0000;
        }



        .success {
            background-color: blue;
        }

        #header-fixed {
            position: fixed;
            top: 0px;
            display: none;
            background-color: white;
        }

        .dayly-table-shell {
            position: relative;
            width: calc(100% + 15px);
            max-width: calc(100% + 15px);
            min-width: 0;
            box-sizing: border-box;
            margin-left: -15px;
            padding-right: 24px;
        }

        #table-container > .table-responsive {
            flex: 1 1 auto;
            width: 100%;
            height: auto;
            min-height: 0;
            max-height: 100%;
            overflow: auto !important;
            overscroll-behavior: contain;
            -webkit-overflow-scrolling: touch;
            scrollbar-color: #337ab7 #e5edf4;
        }

        #table-container > .table-responsive::-webkit-scrollbar {
            width: 14px;
            height: 14px;
        }

        #table-container > .table-responsive::-webkit-scrollbar-track {
            background: #e5edf4;
            border-radius: 8px;
        }

        #table-container > .table-responsive::-webkit-scrollbar-thumb {
            min-height: 42px;
            background: #337ab7;
            border: 3px solid #e5edf4;
            border-radius: 8px;
        }

        #table-container > .table-responsive::-webkit-scrollbar-thumb:hover { background: #205b8f; }

        .dayly-scroll-controls {
            position: absolute;
            top: 50%;
            right: -11px;
            z-index: 120;
            display: flex;
            flex-direction: column;
            gap: 6px;
            transform: translateY(-50%);
        }

        .dayly-scroll-right {
            display: flex;
            width: 34px;
            height: 40px;
            align-items: center;
            justify-content: center;
            border: 1px solid rgba(255, 255, 255, .75);
            border-radius: 8px;
            background: linear-gradient(135deg, #337ab7, #185d93);
            color: #fff;
            box-shadow: 0 5px 14px rgba(20, 68, 105, .30);
            transition: background .18s ease, box-shadow .18s ease, transform .18s ease;
            touch-action: none;
            user-select: none;
        }

        .dayly-scroll-right:hover,
        .dayly-scroll-right:focus {
            outline: 0;
            background: linear-gradient(135deg, #2f8dcc, #174f7d);
            box-shadow: 0 7px 18px rgba(20, 68, 105, .40);
        }

        .dayly-scroll-right:active,
        .dayly-scroll-right.is-holding { transform: scale(.95); }
        .dayly-scroll-right .fa { font-size: 16px; }

        .dayly-table-actions,
        .dayly-top-actions {
            display: flex;
            align-items: center;
            gap: 7px;
        }

        .dayly-table-actions {
            min-width: 112px;
            padding: 2px 3px;
        }

        .dayly-table-actions a,
        .dayly-top-actions > a {
            position: relative;
            display: inline-flex;
            width: 34px;
            height: 34px;
            align-items: center;
            justify-content: center;
            border: 1px solid #acd0e7;
            border-radius: 9px;
            background: #fff;
            color: #267fb7 !important;
            box-shadow: 0 3px 9px rgba(30, 89, 128, .12);
            text-decoration: none;
            cursor: pointer;
            transition: transform .18s ease, background .18s ease, color .18s ease, box-shadow .18s ease;
        }

        .dayly-table-actions a:hover,
        .dayly-top-actions > a:hover,
        .dayly-top-actions > a:focus {
            outline: 0;
            background: #287fbd;
            color: #fff !important;
            transform: translateY(-2px);
            box-shadow: 0 7px 15px rgba(31, 105, 155, .25);
        }

        .dayly-top-actions > a.dayly-search-action {
            background: linear-gradient(135deg, #2f92cc, #1d6fa8);
            color: #fff !important;
        }

        .dayly-top-actions .badge {
            position: absolute;
            top: -6px;
            right: -6px;
        }

        #optionColor { display: none; }

        .dayly-top-actions #optionColor {
            position: relative;
            display: inline-flex;
            width: 34px;
            height: 34px;
            flex: 0 0 34px;
        }

        .dayly-top-actions #ace-settings-container {
            position: relative !important;
            top: auto !important;
            right: auto !important;
            width: 34px;
            height: 34px;
            z-index: 140;
        }

        .dayly-top-actions #ace-settings-btn {
            position: static !important;
            display: inline-flex !important;
            width: 34px !important;
            min-width: 34px !important;
            height: 34px !important;
            min-height: 34px !important;
            margin: 0 !important;
            padding: 0 !important;
            align-items: center;
            justify-content: center;
            border: 1px solid #87badd !important;
            border-radius: 9px !important;
            background: linear-gradient(145deg, #fff, #e7f4fc) !important;
            color: #267fb7 !important;
            box-shadow: 0 3px 9px rgba(30, 89, 128, .14) !important;
        }

        .dayly-top-actions #ace-settings-btn:hover,
        .dayly-top-actions #ace-settings-btn:focus {
            background: linear-gradient(135deg, #359bd2, #1d6fa8) !important;
            color: #fff !important;
            transform: translateY(-2px);
        }

        .dayly-top-actions #ace-settings-btn .fa-cog {
            font-size: 16px;
            transition: transform .28s ease;
        }

        .dayly-top-actions #ace-settings-btn:hover .fa-cog { transform: rotate(60deg); }

        .dayly-top-actions #ace-settings-box {
            position: absolute !important;
            top: 42px !important;
            right: 0 !important;
            left: auto !important;
            max-height: min(520px, calc(100vh - 180px));
            overflow-y: auto;
            border: 1px solid #b9d2e5;
            border-radius: 10px;
            box-shadow: 0 12px 28px rgba(20, 66, 101, .22);
            z-index: 1500 !important;
        }

        #grdSource {
            margin-bottom: 0;
            border-collapse: separate;
            border-spacing: 0;
            background: #fff;
            isolation: isolate;
        }

        #grdSource tbody > tr,
        #grdSource tbody > tr > td { height: auto !important; }

        #grdSource tbody > tr > td {
            padding: 4px 7px !important;
            line-height: 1.2 !important;
            vertical-align: middle !important;
        }

        #grdSource thead tr:last-child > th {
            background: #1e5f91 !important;
            color: #fff !important;
            border-color: #75a4c7 !important;
        }

        #grdSource thead tr.Spec > td {
            padding: 9px 8px !important;
            border-top: 2px solid #2c8bc2 !important;
            border-bottom: 2px solid #2c8bc2 !important;
            background: #d9effc !important;
        }

        #grdSource thead tr.Spec input,
        #grdSource thead tr.Spec select {
            border: 1px solid #68a6cf !important;
            border-radius: 6px !important;
            background: #fff !important;
        }

        #grdSource thead tr.Spec > td.dayly-action-cell {
            min-width: 145px;
            padding-left: 10px !important;
            border-right: 2px solid #2d8bc3 !important;
        }

        #grdSource thead tr > * {
            position: sticky !important;
            z-index: 20;
            background-clip: padding-box;
        }

        #grdSource tr > *.dayly-frozen-column {
            position: sticky !important;
            z-index: 50 !important;
            background-clip: padding-box;
        }

        #grdSource thead tr > *.dayly-frozen-column { z-index: 90 !important; }
        #grdSource tbody tr > td:not(.dayly-frozen-column) { position: relative; z-index: 1; }

        #grdSource tr > *.dayly-freeze-edge {
            border-right: 4px solid #006f9f !important;
            box-shadow: 8px 0 10px -7px rgba(0, 44, 70, .95) !important;
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
    </style>
    <style>
        table {
            border-collapse: separate;
            border-spacing: 0;
        }

        .cssTdCallSignAr {
            background-color: #00FF00 !important;
        }

        #infoTotal {
            position: absolute;
            width: 250px;
            font-size: 11px;
            top: -15px;
        }

        #topBar1 {
            position: absolute;
            left: 250px;
            width: 1000px;
            text-align: center;
            top: -20px;
        }

            #topBar1 span {
                background-color: blue;
            }

        #topBar2 {
            position: absolute;
            left: 250px;
            text-align: center;
            width: 1000px;
            top: 7px;
            font-size: 20px;
        }

            #topBar2 span {
                border: solid black 1px;
                width: 70px;
            }

        #topBar3 {
            position: absolute;
            bottom: 2px;
            right: 20px;
        }



        #grdSource span {
            /*border: 1px solid blue;
            border-radius: 4px;
            color: forestgreen;
            padding: 2px 3px;*/
            font-weight: 550;
        }

            #grdSource span:hover {
                cursor: pointer;
            }

        #grdSource > tr.active {
            background-color: red;
        }

        #grdSource input:focus {
        }

        #divStatusIcon {
            font-size: 13px;
            width: 28px;
        }

        #grdSource .tdIconStatus {
            white-space: nowrap;
            background-color: ghostwhite;
            color: chocolate;
            width: 28px !important;
        }

        #grdSource .iconSearch {
            cursor: pointer;
        }

        #grdSource thead tr td {
            text-transform: uppercase;
            font-weight: 700;
            color: black;
        }

        #grdSource > tbody > tr > td {
            line-height: 0.7;
        }

        #grdSource {
            font-size: 12px;
        }

        #ddlPERMTYPE {
            position: absolute;
            right: 680px;
            height: 25px;
            width: 90px;
            font-size: 12px;
            bottom: 0px;
        }

        #ddlSelect {
            position: absolute;
            right: 580px;
            height: 25px;
            width: 90px;
            font-size: 12px;
            bottom: 0px;
        }

        #lblTimeRefresh {
            position: absolute;
            right: 120px;
            bottom: 0px;
            width: 120px;
        }

        #ddlExport {
            position: absolute;
            right: 450px;
            height: 25px;
            width: 120px;
            font-size: 12px;
            bottom: 0px;
        }

        #ddlPageSize {
            position: absolute;
            right: 385px;
            height: 25px;
            width: 60px;
            font-size: 12px;
            bottom: 0px;
        }

        #btnExport {
            position: absolute;
            right: 140px;
            height: 28px;
            width: 100px;
            font-size: 12px;
            bottom: 0px;
        }


        #grdSource tr td:first-child + td + td {
            white-space: nowrap;
        }

        #grdSource tr td:last-child {
            display: none;
        }

        #grdSource tr th:last-child {
            display: none;
        }

        .divHeader {
            position: relative;
            border-bottom: dotted 1px black;
            height: 80px;
            margin-bottom: 8px;
        }

        .btn.active {
            background-color: red !important;
        }

        .table > thead > tr.Spec {
            background-position: 0% 0%;
            color: #707070;
            font-weight: 400;
            background-image: unset;
            background-color: unset;
            background-repeat: repeat-x;
            background-attachment: scroll;
        }

        .table-bordered > thead > tr > td.headSpec {
            border: unset;
        }

        .table-bordered {
            border: unset;
        }

        .tableTip {
            font-size: 12px;
            background-color: aquamarine;
            text-align: left;
        }
        /*.dControl{font-size: 12px;
            width: 50px;
            height: 20px;}*/
        .sControl {
            font-size: 10px;
            width: 50px;
            height: 25px;
            font-weight: bold;
            color: black !important;
        }

        #divSelectDate {
            position: absolute;
            left: -880px;
            top: -10px;
        }
    </style>
    <style>
        .divHeader {
            position: relative;
            display: flex;
            width: 100%;
            max-width: 100%;
            height: auto !important;
            min-height: 132px;
            flex-direction: column;
            gap: 8px;
            box-sizing: border-box;
            margin-bottom: 8px;
            padding: 10px 12px 12px;
            border: 1px solid #bfd5e6 !important;
            border-radius: 10px;
            background: linear-gradient(135deg, #f7fbff, #e7f3fc);
            box-shadow: 0 5px 14px rgba(35, 83, 120, .10);
        }

        #infoTotal { position: static !important; width: 100%; }

        #topBar1,
        #topBar2,
        #topBar3 {
            position: static !important;
            display: flex;
            width: 100% !important;
            align-items: center;
            justify-content: center;
            flex-wrap: wrap;
            text-align: center;
        }

        #topBar1 { gap: 4px; }
        #topBar2 { gap: 8px; font-size: 16px; }
        #topBar3 { min-height: 38px; gap: 8px; }

        #topBar2 span {
            width: auto !important;
            min-width: 92px;
            padding: 5px 10px;
            border: 1px solid #8eb7d7 !important;
            border-radius: 6px;
            background: #fff;
            color: #174f7c;
            white-space: nowrap;
        }

        #topBar3 > *,
        #topBar3 .checkbox,
        #divSelectDate,
        #ddlPERMTYPE,
        #ddlSelect,
        #ddlExport,
        #ddlPageSize,
        #lblTimeRefresh {
            position: static !important;
            top: auto !important;
            right: auto !important;
            bottom: auto !important;
            left: auto !important;
            width: auto !important;
            margin: 0 !important;
        }

        #topBar3 .checkbox {
            display: flex;
            min-width: 132px;
            align-items: center;
        }

        #topBar3 .checkbox label {
            display: flex;
            align-items: center;
            gap: 7px;
            padding-left: 0 !important;
            white-space: nowrap;
        }

        #topBar3 .checkbox input[type="checkbox"] {
            position: static;
            margin: 0 !important;
        }

        #topBar3 select,
        #divSelectDate select {
            min-width: 90px;
            height: 36px;
            border: 1px solid #78a9cc;
            border-radius: 6px;
            background: #fff;
            color: #164d76;
        }

        @media (max-width: 1199px) {
            #table-container { height: clamp(380px, calc(100vh - 410px), 650px); }
            .divHeader { padding-right: 8px; padding-left: 8px; }
            #topBar1 { gap: 3px; }
            #topBar1 .btn { min-width: 32px; padding-right: 8px; padding-left: 8px; }
            #topBar3 { gap: 6px; }
        }

        @media (max-width: 767px) {
            #table-container { height: max(360px, calc(100vh - 430px)); border-radius: 8px; }
            .dayly-table-shell {
                width: calc(100% + 8px);
                max-width: calc(100% + 8px);
                margin-left: -8px;
                padding-right: 22px;
            }
            .dayly-scroll-controls { right: -7px; gap: 4px; }
            .dayly-scroll-right { width: 30px; height: 36px; border-radius: 7px; }
            .divHeader { min-height: 0; }
            #topBar2, #topBar3 { justify-content: flex-start; }
            .dayly-top-actions { flex-wrap: wrap; }
        }
    </style>
    <style id="dayly-hcm-date-autocomplete">
        /* Bộ chọn ngày luôn nằm gọn trong thanh bộ lọc, không bị đẩy ra ngoài màn hình. */
        #topBar3 #divSelectDate {
            position: relative !important;
            left: auto !important;
            top: auto !important;
            right: auto !important;
            bottom: auto !important;
            display: inline-flex;
            flex: 0 0 auto;
            align-items: center;
            z-index: 80;
        }

        #topBar3 #ddlDateFlight {
            width: 112px !important;
            min-width: 112px !important;
            height: 36px !important;
            padding: 6px 26px 6px 10px !important;
            border: 1px solid #5ea1cf !important;
            border-radius: 7px !important;
            background: #fff !important;
            color: #165b89 !important;
            font-size: 13px;
            font-weight: 600;
        }

        /* Gợi ý sân bay/tàu bay phải nổi trên bảng, nhưng thấp hơn menu điều hướng. */
        body .ui-autocomplete,
        body .xdsoft_autocomplete_dropdown {
            z-index: 1040 !important;
            max-height: 230px;
            overflow-y: auto;
            border: 1px solid #76afd0 !important;
            border-radius: 0 0 6px 6px;
            background: #fff !important;
            box-shadow: 0 8px 18px rgba(26, 69, 104, .22);
        }

        body .ui-autocomplete .ui-menu-item,
        body .xdsoft_autocomplete_dropdown > div {
            padding: 7px 10px;
            color: #234f6b;
            background: #fff;
        }

        body .ui-autocomplete .ui-state-focus,
        body .xdsoft_autocomplete_dropdown > div.active {
            margin: 0;
            background: #e4f2fb !important;
            color: #145b89 !important;
        }

        /* Menu/header của master vẫn được ưu tiên khi popup chạm vùng điều hướng. */
        .navbar,
        .sidebar,
        .main-menu,
        .ace-nav {
            position: relative;
            z-index: 1100;
        }

        @media (max-width: 767px) {
            #topBar3 #ddlDateFlight {
                width: 108px !important;
                min-width: 108px !important;
            }
        }
    </style>
    <style id="styCSS">
        
    </style>
    <style>
        .icon-quay {
            -webkit-animation: spin 1000ms infinite linear;
            animation: spin 1000ms infinite linear;
        }

        @-webkit-keyframes spin {
            20% {
                -webkit-transform: rotate(45deg);
                transform: rotate(45deg);
            }

            40% {
                -webkit-transform: rotate(-45deg);
                transform: rotate(-45deg);
            }

            60% {
                -webkit-transform: rotate(45deg);
                transform: rotate(45deg);
            }

            80% {
                -webkit-transform: rotate(-45deg);
                transform: rotate(-45deg);
            }

            100% {
                -webkit-transform: rotate(45deg);
                transform: rotate(45deg);
            }
        }

        @keyframes spin {
            20% {
                -webkit-transform: rotate(45deg);
                transform: rotate(45deg);
            }

            40% {
                -webkit-transform: rotate(-45deg);
                transform: rotate(-45deg);
            }

            60% {
                -webkit-transform: rotate(45deg);
                transform: rotate(45deg);
            }

            80% {
                -webkit-transform: rotate(-45deg);
                transform: rotate(-45deg);
            }

            100% {
                -webkit-transform: rotate(45deg);
                transform: rotate(45deg);
            }
        }

        .modal {
            position: fixed;
            top: 10%;
            left: 10%;
            **overflow-y:scroll**;
        }

        .vertical-alignment-helper {
            display: table;
            height: 100%;
            width: 100%;
        }

        .vertical-align-center {
            /* To center vertically */
            display: table-cell;
            vertical-align: middle;
        }

        .modal-content {
            /* Bootstrap sets the size of the modal in the modal-dialog class, we need to inherit it */
            width: inherit;
            height: inherit;
            /* To center horizontally */
            margin: 0 auto;
        }

        dt {
            font-weight: 300;
        }
    </style>
    <div id="sound"></div>
    <div id="optionColor">
        <div class="ace-settings-container" id="ace-settings-container">
            <div class="btn btn-app btn-xs btn-warning ace-settings-btn" onclick="$('#ace-settings-box').toggleClass('open');"
                id="ace-settings-btn">
                <i class="ace-icon fa fa-cog bigger-130"></i>
            </div>
            <div class="ace-settings-box clearfix" id="ace-settings-box">
                <div class="pull-left width_50">
                    <div class="ace-settings-item">
                        <label class="lbl" for="ace-settings-navbar">
                            Sound notification
                            <input id="chkAutoSound" type="checkbox" onchange="isSound = ($('#chkAutoSound').prop('checked'));runtimeSoundNotification();" /></label>

                    </div>
                    <% foreach (DataRow r in new DayFlightSetColorDAL().GetTableColor(_user.UserID.ToString()).Rows)%>
                    <%  {%>
                    <% if (!r["CSSCLASSNAME"].ToString().Contains("FontSize")) %>
                    <%{ %>
                    <div class="ace-settings-item">
                        <label class="lbl" for="ace-settings-navbar"><%: r["ALIASNAME"] %></label>
                        <input id="txtMc<%:r["CSSCLASSNAME"] %>" type="text" data-classname="<%: r["CSSCLASSNAME"] %>"
                            data-modecolor="true" data-setcolor="<%: r["COLORCUSTOM"] %>" data-colordefault="<%: r["DEFAULTCOLOR"] %>" />
                        <a style="cursor: pointer" onclick="SetDefaultColor('<%: r["CSSCLASSNAME"] %>', <%: _user.UserID %>)">Default</a>
                    </div>
                    <%} %>
                    <% else %>
                    <%{ %>
                    <div class="ace-settings-item">
                        <label class="lbl" for="ace-settings-navbar"><%: r["ALIASNAME"] %></label>
                        <input id="txtFc<%:r["CSSCLASSNAME"] %>" type="number" data-classname="<%: r["CSSCLASSNAME"] %>"
                            data-modefontsize="true" class="wid_55px hei_25px" onblur="UpdateColorBy('<%= _user.UserID%>', $(this).val(), $(this).attr('data-className'));reStyleColor();"
                            value="<%: r["COLORCUSTOM"] %>" data-setfontsize="<%: r["COLORCUSTOM"] %>" data-fontsizedefault="<%: r["DEFAULTCOLOR"] %>" />
                        <%--<a style="cursor: pointer" onclick="SetDefaultFontSize('<%: r["CSSCLASSNAME"] %>', <%: _user.UserID %>)">
                            Default</a>--%>
                    </div>
                    <% } %>
                    <% } %>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="divHeader">
            <div id="infoTotal">
                
            </div>
            <div id="topBar1">
                <select id="ddlSelectViewColum" class="sControl wid_60px">
                    <option value="">--</option>
                    <option selected value="ETD">ETD</option>
                    <option value="ETA">ETA</option>
                    <option value="ATD">ATD</option>
                    <option value="ATA">ATA</option>
                    <option value="EOBT">EOBT</option>
                </select>
                <% for (int i = 0; i <= 23; i++)%>
                <% {%>
                <span class="btn btn-default btn-xs" onclick="$(this).toggleClass('active');">
                    <%= i %></span>
                <% } %>
            </div>
            <div id="topBar2">
                <span id="timeUS"></span>
                <span id="totql"></span>
            </div>

            <div id="topBar3">
                <div id="divSelectDate">
                </div>
                <div class="checkbox" style="font-weight: 100; width: 120px!important; position: absolute; right: 260px; bottom: -10px;">
                    <label>
                        <input type="checkbox" id="chkAutoRefresh" onchange="isRefresh = ($('#chkAutoRefresh').prop('checked')); if(isRefresh){cd= timeRefresh;runtimeCountDown();}" />Auto
                        refresh</label>
                </div>

                <select id="ddlPERMTYPE">
                    <option value="">All</option>
                    <option value="LD" selected="selected">LD</option>
                    <option value="O/F">OF</option>
                </select>
                <select id="ddlSelect">
                    <option value="0">-All-</option>
                    <option value="QN">OPER QN</option>
                    <option value="QT">OPER QT</option>
                </select>
                <select id="ddlExport">
                    <option value="2">-All-</option>
                    <option value="1">MOVED</option>
                    <option value="0">NOT MOVED</option>                   
                </select>
                <select id="ddlPageSize" class="disabled">
                    <option value="50" selected="selected">50</option>
                    <option value="100">100</option>
                    <option value="500">500</option>
                    <option value="1000">1000</option>
                    <option value="2000">2000</option>   
                    <option value="3000">3000</option>                  
                </select>
                <span id="lblTimeRefresh"></span>

                <div id="topbarNotificon"></div>
                <div class="dayly-top-actions">
                    <a id="btnSearch" class="dayly-search-action" onclick="btnSearch_Onclick()" data-toggle="tooltip" title="Tìm kiếm dữ liệu">
                        <i class="glyphicon glyphicon-search"></i>
                    </a>
                   <!--<a href="#" onclick="window.open('<%= Page.ResolveUrl("~/SendMessage/SendMessageFlight.aspx") + "?Menu_Id=" + Request.Params["Menu_ID"] %>','_blank','toolbar=yes,scrollbars=yes,resizable=yes').resizeTo(window.screen.availWidth, window.screen.availHeight).moveTo(0,0)"
                        data-toggle="tooltip" title="Send message">
                        <i class="glyphicon glyphicon-send"></i>
                    </a>
                    <a style="display: none" data-toggle="tooltip" href="#" id="urlSendMessage" title="Message arising in the day">
                        <i class="ace-icon fa fa-bell icon-quay"></i><span id="lbltopbarNotificon_TotalMessage"
                            class="badge badge-important"></span>
                    </a>
                    <a href="#" data-toggle="tooltip" title="Flight not permission" onclick="window.open('<%= Page.ResolveUrl("~/Day_Flights/DaylyFlightNotPerm.aspx") + "?Menu_Id=" + Request.Params["Menu_ID"] %>','_blank','toolbar=yes,scrollbars=yes,resizable=yes').resizeTo(window.screen.availWidth, window.screen.availHeight).moveTo(0,0)">
                        <i class="glyphicon glyphicon-remove"></i><span id="lbltopbarNotificon_TotalFlight"
                            class="badge badge-success"></span>
                    </a>
                    <a style="display: none" href="#" data-toggle="tooltip" title="Flight not complate" onclick="window.open('<%= Page.ResolveUrl("~/Day_Flights/DaylyFlightNotATD.aspx") + "?Menu_Id=" + Request.Params["Menu_ID"] %>','_blank','toolbar=yes,scrollbars=yes,resizable=yes').resizeTo(window.screen.availWidth, window.screen.availHeight).moveTo(0,0)">
                        <i class="ace-icon fa fa-fighter-jet"></i><span id="lbltopbarNotComplate_TotalFlight"
                            class="badge badge-success"></span>
                    </a>-->

                    <!--<a href="#" data-toggle="tooltip" title="Update status of message" onclick="window.open('<%= Page.ResolveUrl("~/Day_Flights/DaylyFlightCancel.aspx") + "?Menu_Id=" + Request.Params["Menu_ID"] %>','_blank','toolbar=yes,scrollbars=yes,resizable=yes').resizeTo(window.screen.availWidth, window.screen.availHeight).moveTo(0,0)">
                        <i class="glyphicon glyphicon-comment"></i><span id="lbltopbarNo_TotalFlight"
                            class="badge badge-success"></span>
                    </a>
                    <a href="#" data-toggle="tooltip" title="Export Flight Finished" onclick="LoadDataGrid_Finished()">
                        <i class="glyphicon glyphicon-cloud-download"></i><span id="lbltopbarNo_TotalFinished"
                            class="badge badge-success"></span>
                    </a>-->
                    <a href="#" data-toggle="tooltip" title="Export Exel" onclick="btnExport_Onclick()">
                        <i class="glyphicon glyphicon-file"></i>
                    </a>
                </div>
            </div>
        </div>
    </div>
    <%= RenderTopBar() %>

    <div class="dayly-table-shell">
        <div class="row" id="table-container">
            <div class="table-responsive">

            <table id="grdSource" class="table table-bordered">
                <thead>
                    <tr class="Spec" data-isinsert="true">
                        <td class="headSpec dayly-action-cell" colspan="3">
                            <div class="dayly-table-actions">
                                 <a id="btnClearInput" onclick="btnClearInput_Onclick();" title="Xóa điều kiện tìm kiếm">
                                    <i class="glyphicon glyphicon-trash"></i>
                                </a>
                            </div>
                        </td>
                        <%--<td class="headSpec">
                            <select id="ddlStatus" class="sControl">
                                <option value="0">--</option>
                                <option value="1">1</option>
                                <option value="2">2</option>
                                <option value="3">3</option>
                            </select>
                        </td>--%>
                        <td class="headSpec">
                            <input id="txtPERMNBR" type="text" class="sControl" style="width: 168px;" /></td>
                        <td class="headSpec">
                            <input type="text" class="sControl" id="txtREGISTRATION" style="width: 80px;" /></td>
                        <td class="headSpec">
                            <input id="txtFLIGHTNBR" type="text" class="sControl" style="width: 80px;" /></td>

                        <td class="headSpec">
                            <input id="ddlFROM_AIRP" type="text" data-autocomplete="AERO" class="sControl" style="width: 60px;" />
                        </td>
                        <td class="headSpec">
                            <input id="ddlTO_AIRP" type="text" data-autocomplete="AERO" class="sControl" style="width: 60px;" />
                        </td>
                        <td class="headSpec">
                            <input id="txtETD" type="text" style="display: none;" class="sControl" />
                        </td>
                        <td class="headSpec">
                            <input id="txtPtd" type="text" style="display: none;" class="sControl" /></td>
                        <td class="headSpec">
                            <input id="txtETA" type="text" style="display: none;" class="sControl" />
                        </td>

                        <td class="headSpec">
                            <input id="txtAtd" type="text" style="display: none;" class="sControl" /></td>
                        <td class="headSpec">
                            <input id="txtATA" type="text" style="display: none;" class="sControl" /></td>
                        <td class="headSpec">
                            <input id="txtROUTE" type="text" class="sControl" style="width: 120px;" /></td>
                        <td class="headSpec">
                            <input id="txtROUTE_TP" type="text" class="sControl" style="width: 90px;" /></td>
                        <td class="headSpec">
                            <input id="ddlCRAFT_T" type="text" class="sControl" data-autocomplete="CRAFT" />
                        </td>
                        <td class="headSpec">
                            <input id="ddlCRAFT_TP" type="text" class="sControl" data-autocomplete="CRAFT" />

                        </td>

                        <td class="headSpec">
                            <input id="txtPURPOSE" type="text" class="sControl" /></td>
                        <td class="headSpec">
                            <input id="txtREMARK" type="text" class="sControl" style="width: 100px;" /></td>
                        <!--<td class="headSpec">
                            <input id="txtFLIGHTDATE" type="text" data-date-format="dd/mm/yyyy" class="sControl date-picker" />
                        </td>-->

                        <td class="headSpec">
                            <input id="txtVIA" type="text" class="sControl" /></td>
                    </tr>
                    <tr>
                        <th style="width: 18px"></th>
                        <th style="width: 10px" data-sort="1" onclick="sortOnclickSpan(this);">STT</th>
                        <th>
                            <select id="ddlLetter_Type" class="sControl">
                                <option value="">--</option>
                                <option value="FPL">FPL</option>
                                <option value="DEP">DEP</option>
                                <option value="DLA">DLA</option>
                                <option value="CHG">CHG</option>
                                <option value="CNL">CNL</option>
                                <option value="ARR">ARR</option>
                                <%--<option value="NO">No Permission</option>--%>
                            </select>
                        </th>
                        <%--<th>Status</th>--%>
                        <th data-sort="1" onclick="sortOnclick(this);">PERM</th>
                        <th data-sort="1" onclick="sortOnclick(this);">REGIS</th>
                        <th data-sort="1" onclick="sortOnclick(this);">CallSign</th>

                        <th data-sort="1" onclick="sortOnclick(this);">FROM</th>
                        <th data-sort="1" onclick="sortOnclick(this);">TO</th>
                        <th data-sort="1" onclick="sortOnclick(this);">ETD</th>
                        <th data-sort="1" onclick="sortOnclick(this);">EOBT</th>
                        <th data-sort="1" onclick="sortOnclick(this);">ETA</th>

                        <th data-sort="1" onclick="sortOnclick(this);">ATD</th>
                        <th data-sort="1" onclick="sortOnclick(this);">ATA</th>
                        <th data-sort="1" onclick="sortOnclick(this);">ROUTE</th>
                        <th data-sort="1" onclick="sortOnclickSpan(this);">ROUTE_FPL</th>
                        <th data-sort="1" onclick="sortOnclickSpan(this);">CRAFT_T</th>
                        <th data-sort="1" onclick="sortOnclick(this);">CRAFT_P</th>

                        <th data-sort="1" onclick="sortOnclick(this);">PUPOSE</th>
                        <th data-sort="1" onclick="sortOnclick(this);">REMARK</th>
                        <th data-sort="1" onclick="sortOnclick(this);">VIA</th>
                    </tr>
                </thead>
                <tbody>
                   <%-- <%= grdSourceLoadFirt() %>--%>
                </tbody>

            </table>
            
            </div>
        </div>
        <div class="dayly-scroll-controls" aria-label="Điều khiển cuộn ngang">
            <button id="daylyScrollLeft" class="dayly-scroll-right" type="button" title="Nhấn để sang trái một cột, giữ để cuộn liên tục" aria-label="Cuộn bảng sang trái"><i class="fa fa-chevron-left" aria-hidden="true"></i></button>
            <button id="daylyScrollRight" class="dayly-scroll-right" type="button" title="Nhấn để sang phải một cột, giữ để cuộn liên tục" aria-label="Cuộn bảng sang phải"><i class="fa fa-chevron-right" aria-hidden="true"></i></button>
        </div>
    </div>



    <div id="popPerm" class="modal fade" role="dialog" tabindex="-1" aria-labelledby="myModal"
        data-backdrop="false" aria-hidden="true">
        <div class="modal-dialog wid_90">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">x</button>
                    <h4 class="blue bigger">Flight Permission</h4>
                </div>
                <div class="modal-body">


                    <button id="btnCancelpopPerm" type="button"
                        data-dismiss="modal" class="btn btn-sm btn-primary">
                        <i class="ace-icon fa fa-times"></i>
                        Cancel
                    </button>
                </div>
            </div>
        </div>
    </div>

    <div id="popFlightInfoExtension" class="modal fade" role="dialog" tabindex="-1"
        aria-labelledby="myModalLabel"
        data-backdrop="false"
        aria-hidden="true">
        <div class="modal-dialog wid_90">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close"
                        data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">Flight extension</h4>
                </div>
                <!--<div class="modal-body" id="txtContentExtensionInfoFlight">
                    <!--<div class="wid_50">
                        <!--<div class="form-horizontal" role="form">
                            <div class="form-group">
                                <label for="txtChange" class="mLable control-label">
                                    Change:
                                </label>
                                <label class="mLable text-info" id="txtChange"></label>
                            </div>
                        </div>
                        <div class="form-horizontal" role="form">
                            <div class="form-group">
                                <label for="txtValidateTime" class="mLable control-label">
                                    Time
    valid:
                                </label>
                                <label class="mLable text-danger" id="txtValidateTime"></label>
                            </div>
                        </div>
                        <div class="form-horizontal" role="form">
                            <div class="form-group">
                                <label for="txtHasPermission" class="mLable control-label">
                                    Has
    permission:
                                </label>
                                <label class="mLable text-primary" style="font-weight: 100"
                                    id="txtHasPermission">
                                </label>
                            </div>
                        </div>
                        <div class="form-horizontal" role="form">
                            <div class="form-group">
                                <label for="txtContentChange" class="mLable control-label">
                                    Content
    change:
                                </label>
                                <label class="mLable text-uppercase" id="txtContentChange"></label>
                            </div>
                        </div>-->

                <!--<div class="form-horizontal" role="form">
                            <div class="form-group">
                                <label for="txtChange" class="mLable control-label">
                                    Regis:
                                </label>
                                <input id="txtRegis" type="text" class="wid_100px" />
                                <label for="txtChange" class="mLable control-label">
                                    Pupose:
                                </label>
                                <input id="txtPupose" type="text" class="wid_80px" />
                                <label for="txtChange" class="mLable control-label">
                                    Remark:
                                </label>
                                <input id="txtRemark" type="text" class="wid_120px" />
                            </div>
                        </div>
                        <div class="form-horizontal" role="form">
                            <div class="form-group">
                                <label for="txtChange" class="mLable control-label">
                                    Eobt:
                                </label>
                                <input id="txtEobt" data-minlenght="1" data-control="checkAccess" type="text"
                                    placeholder="time" class="wid_50px" maxlength='4' data-number='true' />
                                <label for="txtChange" class="mLable control-label">
                                    Atd:
                                </label>
                                <input id="txtAtd2" data-minlenght="1" data-control="checkAccess" type="text"
                                    placeholder="time" class="wid_50px" maxlength='4' data-number='true' />
                                <label for="txtChange" class="mLable control-label">
                                    Ata:
                                </label>
                                <input id="txtAta" data-minlenght="1" data-control="checkAccess" type="text"
                                    placeholder="time" class="wid_50px" maxlength='4' data-number='true' />
                                <label for="txtChange" class="mLable control-label">
                                    Status   :
                                </label>

                                <select id="ddlSelectView" class="wid_60px" style="height: 30px;">
                                    <option selected value="--">--</option>
                                    <option value="FPL">FPL</option>
                                    <option value="DEP">DEP</option>
                                    <option value="DLA">DLA</option>
                                    <option value="CHG">CHG</option>
                                    <option value="CNL">CNL</option>
                                    <option value="ARR">ARR</option>
                                </select>
                                <button id="btnUpdateStatusLetter" class="btn btn-sm btn-primary" type="button">
                                    Update
                                </button>

                            </div>
                        </div>



                    </div>-->
                <textarea id="txtContentMessage" style="margin-top: -100%; margin-left: 50.5%; height: 100%; width: 50%; display: none;"
                    rows="7">                            
                        </textarea>
                <div style="border-top: 1px dotted black;"></div>
                <div>
                    <fieldset>
                        <legend>Permission</legend>
                        <table>
                            <tr>
                                <td>
                                    <dl>
                                        <dt>Author</dt>
                                        <dt>
                                            <input id="txtAuthorPerm" type="text" class="wid_60px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt>Oper</dt>
                                        <dt>
                                            <input id="txtOperPerm" type="text" class="wid_60px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt>Number</dt>
                                        <dt>
                                            <input id="txtFlightNbrPerm" type="text" class="wid_60px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt>Type</dt>
                                        <dt>
                                            <input id="txtPermTypePerm" type="text" class="wid_60px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt>Version</dt>
                                        <dt>
                                            <input id="txtVersionPerm" type="text" class="wid_60px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt>Season</dt>
                                        <dt>
                                            <input id="txtSeasonPerm" type="text" class="wid_60px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt>Date</dt>
                                        <dt>
                                            <input id="txtDatePerm" type="text" class="wid_90px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt>Hours</dt>
                                        <dt>
                                            <input id="txtHoursPerm" type="text" class="wid_60px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <label style="font-weight: 300;" id="lblLinkFile"></label>
                                </td>
                            </tr>
                        </table>
                        <table class="table table-bordered" id="tblPermission"></table>
                    </fieldset>
                </div>

                <div style="border-top: 1px dotted black;"></div>
                <div>
                    <div class="table-responsive">
                        <table id="tblDienVan" class="table table-bordered">
                            <caption class="text-left">History Message</caption>
                            <thead>
                                <tr>
                                    <th>Hour</th>
                                    <th>Date</th>
                                    <th>Type</th>
                                    <th>CallSign</th>
                                    <th>Registration</th>
                                    <th>From</th>
                                    <th>To</th>
                                    <!--<th>ETD</th>
                                        <th>ETA</th>-->
                                    <th>Via</th>
                                    <th>Content</th>
                                    <th>FROM_PL</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr></tr>
                            </tbody>
                        </table>

                    </div>
                </div>
                <div class="row" style="text-align: center;">
                    <button id="btnAcceseeInfoChange" class="btn btn-sm btn-primary"
                        style="display: none;">
                        <i class="ace-icon fa fa-times"></i>
                        Access
                    </button>
                    <button id="btnCancel" type="button"
                        data-dismiss="modal" class="btn btn-sm btn-primary">
                        <i class="ace-icon fa fa-times"></i>
                        Cancel
                    </button>

                </div>
            </div>
        </div>
    </div>
    


    <div id="popFlightInfoInsert" class="modal fade" style="width: 1300px; padding-right: 20px;" role="dialog" tabindex="-1"
        aria-labelledby="myModalLabel"
        data-backdrop="false"
        aria-hidden="true">
        <div class="modal-dialog wid_90">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close"
                        data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">Add / Update Flight Info <span id="idBnr" style="color: red;"></span></h4>
                </div>
                <div class="modal-body" id="txtContentExtensionInfoFlightInsert">


                    <fieldset>

                        <table>
                            <tr>


                                <td>
                                    <dl>
                                        <dt><b>CRAFT_T</b></dt>
                                        <dt>
                                            <input id="txt_popCRAFT_T" type="text" data-autocomplete="CRAFT" class="wid_80px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt><b>CAllSIGN</b></dt>
                                        <dt>
                                            <input id="txt_popCAllSIGN" type="text" class="wid_80px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt><b>FROM</b></dt>
                                        <dt>
                                            <input id="txt_popFROM" type="text" data-autocomplete="AERO" class="wid_80px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt><b>TO</b></dt>
                                        <dt>
                                            <input id="txt_popTO" type="text" data-autocomplete="AERO" class="wid_80px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt><b>ETD</b></dt>
                                        <dt>
                                            <input id="txt_popETD" type="text" data-number='true' maxlength='6' class="wid_80px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt><b>EOBT</b></dt>
                                        <dt>
                                            <input id="txt_popEOBT" type="text" data-number='true' maxlength='6' class="wid_80px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt><b>ETA</b></dt>
                                        <dt>
                                            <input id="txt_popETA" type="text" maxlength='7' class="wid_80px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt><b>OPER</b></dt>
                                        <dt>
                                            <input id="txt_popOPER" type="text" data-autocomplete="OPER" class="wid_80px" /></dt>
                                    </dl>
                                </td>

                                <td>
                                    <dl>
                                        <dt><b>REMARK</b></dt>
                                        <dt>
                                            <input id="txt_popREMARK" type="text" class="wid_180px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt><b>P_DATE</b></dt>
                                        <dt>
                                            <input id="txt_popPERMDATE" type="text" class="wid_180px" /></dt>
                                    </dl>
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <dl>
                                        <dt><b>P_TYPE</b></dt>
                                        <dt>
                                            <input id="txt_popPERMTYPE" type="text" data-autocomplete="PERMTYPE" class="wid_80px" /></dt>
                                    </dl>
                                </td>





                                <td>
                                    <dl>
                                        <dt><b>F_TYPE</b></dt>
                                        <dt>
                                            <input id="txt_popFLIGHTTYPE" type="text" data-autocomplete="FLIGHTTYPE" class="wid_80px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt><b>PUR</b></dt>
                                        <dt>
                                            <input id="txt_popPUR" type="text" data-autocomplete="PURPOSE" class="wid_80px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt><b>VALID</b></dt>
                                        <dt>
                                            <input id="txt_popVALID" type="text" class="wid_80px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt><b>ATD</b></dt>
                                        <dt>
                                            <input id="txt_popATD" type="text" maxlength='6' class="wid_80px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt><b>ATA</b></dt>
                                        <dt>
                                            <input id="txt_popATA" type="text" maxlength='6' class="wid_80px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt><b>CRAFT_P</b></dt>
                                        <dt>
                                            <input id="txt_popCRAFT_P" type="text" data-autocomplete="CRAFT" class="wid_80px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt><b>REGIS</b></dt>
                                        <dt>
                                            <input id="txt_popREGIS" type="text" class="wid_80px" /></dt>
                                    </dl>
                                </td>

                                <td>
                                    <dl>
                                        <dt><b>PERM</b></dt>
                                        <dt>
                                            <input id="txt_popPERMNBR" type="text" class="wid_180px" /></dt>
                                    </dl>
                                </td>
                                <td>
                                    <dl>
                                        <dt><b>ROUTE</b></dt>
                                        <dt>
                                            <input id="txt_popROUTE" type="text" class="wid_180px" /></dt>
                                    </dl>
                                </td>
                                <td style="display: none;">
                                    <dl>
                                        <dt><b>ROUTE_FPL</b></dt>
                                        <dt>
                                            <input id="txt_popROUTE_FPL" type="text" class="wid_80px" /></dt>
                                    </dl>
                                </td>

                            </tr>
                        </table>

                    </fieldset>


                    <div class="row" style="text-align: center;">
                        <select id="ddlSelectPop" class="wid_80px" style="height: 30px;display:none;">
                            <option selected value="--">--</option>
                            <option value="FPL">FPL</option>
                            <option value="DEP">DEP</option>
                            <option value="DLA">DLA</option>
                            <option value="CHG">CHG</option>
                            <option value="CNL">CNL</option>
                            <option value="ARR">ARR</option>
                            <option value="NULL">NULL</option>
                        </select>
                        <input id="txt_FID" type="text" class="wid_180px" style="display: none;" />
                        <button id="btnSaveInfoChange" onclick="btnInsert();" class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-save"></i>
                            Add New
                        </button>
                        <button id="btnUpdateInfoChange" onclick="btnUpdate();" class="btn btn-sm btn-primary" style="display:none;">
                            <i class="ace-icon fa fa-save"></i>
                            Update

                        </button>
                        <button id="btnCancelE" type="button"
                            data-dismiss="modal" class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-times"></i>
                            Close
                        </button>

                    </div>
                </div>
            </div>
        </div>
    </div>




    <div id="dhtmltooltip"></div>
    <div id="divExcuteScript"></div>
    <div id="divMessegeChange" hidden="hidden"></div>
    <div id="divAlarm" hidden="hidden"></div>
    <script src="../Style/assets/js/jquery-2.1.4.min.js"></script>
    <script src="../Style/assets/js/bootstrap.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/tableHeadFixer.js"></script>
    <script src="../Scripts/spectrum.js"></script>
    <script src="../Style/assets/js/ace-elements.min.js"></script>
    <script src="../Style/assets/js/ace.min.js"></script>
    <script src="../Scripts/tableHeadFixer.js"></script>
    <script src="../Scripts/CustumStaticdata.js"></script>
    <script>
        var d = new Date();
        $('#divSelectDate').html("<select id='ddlDateFlight' onchange='btnSearch_Onclick();'>"

            +"<option value='-4'>"
                    + dateFormat(new Date().setDate(new Date().getDate() - 4), 'dd/mm/yyyy')
        + "</option>"
        +"<option value='-3'>"
                    + dateFormat(new Date().setDate(new Date().getDate() - 3), 'dd/mm/yyyy')
        + "</option>"
        +"<option value='-2'>"
                    + dateFormat(new Date().setDate(new Date().getDate() - 2), 'dd/mm/yyyy')
        + "</option>"


        +"<option value='-1'>"
                    + dateFormat(new Date().setDate(new Date().getDate() - 1), 'dd/mm/yyyy')
        + "</option>"
                    + "<option selected value='0'>" + dateFormat(new Date(), 'dd/mm/yyyy')
    + "</option><option value='1'>" + dateFormat(new Date().setDate(new Date().getDate()
    + 1), 'dd/mm/yyyy') + "</option></select>");
        //$('#txt_popPERMDATE').multiDate();
        function SelectedDateFlight_Change() {
            btnSearch_Onclick();
        }
    </script>
    <script>
        function checkTime(m) {
            if (m < 10)
                m = "0" + m	// add zero in front of minutes < 10
            return m
        }
        function cKhungGio1(m) {
            var today = new Date();
            if (m < 10)
                m = "0" + m + "00"	// add zero in front of minutes < 10
            else m = m + '00'
            return m
        }
        function cKhungGio2(m) {
            if (m < 10)
                m = "0" + m + "59"	// add zero in front of minutes < 10
            else m = m + '59'
            return m
        }
        function start() {
            var today = new Date();
            var h = today.getHours();
            var m = today.getMinutes();
            var s = today.getSeconds();
            var today2 = new Date();
            var utc = today2.getTime() + (today2.getTimezoneOffset() * 60000);
            var nd = new Date(utc + (3600000 * 0)); /* timezone + 9*/
            var h2 = nd.getHours();
            var m2 = nd.getMinutes();
            var s2 = nd.getSeconds();
            //document.getElementById('timeVN').innerHTML = checkTime(h) + ":" + checkTime(m) + ":" + checkTime(s)
            document.getElementById('timeUS').innerHTML = checkTime(h2) + ":" + checkTime(m2) + ":" + checkTime(s2);
            var t = setTimeout(start, 1 * 1000);
        }
        var _hide_details = false
        var count = 0
        start();
    </script>
    <script>
        $('body').attr('onSubmit', 'return false;');
    </script>
    <script>
        var isSound = false;
        
        var phanCach = '<%= _phanCach%>';
        var phanCachArg = '<%= _phanCachArg %>';
        var khungGio = [0, 23];
        var loadTopbarInfo = true;
        var loadGrdSource = false;
        var loadTopbar3 = false;
        var isRefresh = false;
        var isSearch = false;
        var isSroll = false;
        var isScrollRequestPending = false;
        var hasMoreScrollData = true;
        var timeRefresh = 20;
        var cd = timeRefresh;
        var sObj = JSON.parse('<%= _ObjSearch%>');
        var btnSearch = document.getElementById('btnSearch');
        var ddlStatus = document.getElementById('ddlStatus');
        var txtPERMNBR = document.getElementById('txtPERMNBR');
        var txtFLIGHTNBR = document.getElementById('txtFLIGHTNBR');
        var txtFROM_AIRP = document.getElementById('ddlFROM_AIRP');
        var txtTO_AIRP = document.getElementById('ddlTO_AIRP');
        var txtETD = document.getElementById('txtETD');
        var txtETA = document.getElementById('txtETA ');
        var txtPtd = document.getElementById('txtPtd');
        var txtAtd = document.getElementById('txtAtd');
        var txtATA = document.getElementById('txtATA');
        var ddlCRAFT_TP = document.getElementById('ddlCRAFT_TP');
        var ddlCRAFT_T = document.getElementById('ddlCRAFT_T');
        var txtREGISTRATION = document.getElementById('txtREGISTRATION');
        var txtPURPOSE = document.getElementById('txtPURPOSE');
        var ddlMTOW = document.getElementById('ddlMTOW');
        var txtFLIGHTDATE = document.getElementById('txtFLIGHTDATE');
        var txtFLIGHT_TYPE = document.getElementById('txtFLIGHT_TYPE');
        var txtVALIDHOURS = document.getElementById('txtVALIDHOURS');
        var txtDATE_OLD = document.getElementById('txtDATE_OLD');
        var txtROUTE = document.getElementById('txtROUTE');
        var lbidBnr =document.getElementById('idBnr');


        var ddlPageSize = document.getElementById('ddlPageSize');      
        
        var appenLoad = 50;

        $("#grdSource tbody tr td").attr('valign', 'center');
        function btnClearInput_Onclick() {
            //$('#grdSource input').val('')
            $('#txtFLIGHTNBR').val('');
            $('#txtREGISTRATION').val('');

            $('#txtPERMNBR').val('');
            $('#ddlFROM_AIRP').val('');
            $('#ddlTO_AIRP').val('');
            $('#txtROUTE').val('');
            $('#txtROUTE_TP').val('');

            $('#ddlCRAFT_T').val('');
            $('#ddlCRAFT_TP').val('');

            $('#txtPURPOSE').val('');
            $('#txtREMARK').val('');

            $('#ddlLetter_Type').prop('selectedIndex', 0);
        }
        function getValueSearchDaylyFlight() {
            var khungGio = [0, 23];
            var ax = document.getElementById('topBar1').getElementsByClassName('active');
            if (ax.length == 1)
                khungGio = [ax[0].innerText, ax[0].innerText];
            else if (ax.length == 0)
            {
                //var d = new Date();
                //var n = d.getUTCHours();
                khungGio = [0, 23];
                //khungGio = [n, 23];
            }
            else khungGio = [ax[0].innerText, ax[ax.length - 1].innerText];

           
            //appenLoad = parseInt(ddlPageSize.value);

                        
            var _obj = {
                //STATUS: document.getElementById('ddlStatus').value,
                PERMNBR: $('#txtPERMNBR').val(),
                FLIGHTNBR: $('#txtFLIGHTNBR').val(),
                FROM_AIRP: $('#ddlFROM_AIRP').val(),
                TO_AIRP: $('#ddlTO_AIRP').val(),
                ETA: isRefresh ? '' : ($('#ddlSelectViewColum').val() == 'ETA' ? 'ETA'
    : ''),
                ETD: isRefresh ? '' : ($('#ddlSelectViewColum').val() == 'ETD' ? 'ETD'
    : ''),
                ATD: isRefresh ? '' : ($('#ddlSelectViewColum').val() == 'ATD' ? 'ATD'
    : ''),
                ATA: isRefresh ? '' : ($('#ddlSelectViewColum').val() == 'ATA' ? 'ATA'
    : ''),
                CRAFT_ID: $('#ddlCRAFT_T').attr('data-craftid'),
                CRAFT_TYPE: $('#ddlCRAFT_TP').val(),
                REGISTRATION: $('#txtREGISTRATION').val(),
                PURPOSE: $('#txtPURPOSE').val(),
                STT: $('#ddlExport').val(),
                REMARK: $('#txtREMARK').val(),
                FLIGHT_TYPE: $('#ddlSelect').val(),
                PERMTYPE: $('#ddlPERMTYPE').val(),
                KHUNGGIO1: cKhungGio1(khungGio[0]),
                KHUNGGIO2: cKhungGio2(khungGio[1]),
                LETTER_TYPE: $('#ddlLetter_Type').val(),
                RowStart: isSroll ? (parseInt($('#grdSource tbody tr').last().attr('data-RowNumber'))
    + 1) : 1,
                RowFinish: isSroll ? (parseInt($('#grdSource tbody tr').last().attr('data-RowNumber'))
    + appenLoad) : ddlPageSize.value,
                OptionDate: $('#ddlDateFlight').val(),
                ROUTE: $('#txtROUTE').val(),
                ROUTE_TT: $('#txtROUTE_TP').val()
                
            }
            sObj = _obj;
            return _obj;
        }
        function DisplayResult(resulf, context) {
            if (context == 'btnExport_Onclick') {
                var array = resulf.split('&&&&');
                generate_excel(resulf);
            }
            if (context == 'btnSearch_Onclick') {
                var array = resulf.split('&&&&');

                //alert(array[1])

                $("#totql").html("Total : <b>" + array[1] + "</b>");

                $('#grdSource tbody tr').remove();
                $('#grdSource tbody').append(array[0]);
                isScrollRequestPending = false;
                hasMoreScrollData = true;
                scheduleDaylyStickyFreeze();
                unPreLoadData();

                highlight_row();
            }
            if (context == 'RenderTopBarInfo') {
                $('#divExcuteScript').html(resulf);
                isFirtLoadAlarm = false;
                if ($('#divMessegeChange').html() != '')
                    notifyMe($('#divMessegeChange').html().replace(/<br>/gi, '\n'));
            }
            if (context == 'LoadGrdSourceScroll') {
                var appendedRows = $.trim(resulf || '');
                if (appendedRows) {
                    $('#grdSource tr').last().after(appendedRows).fadeIn();
                } else {
                    hasMoreScrollData = false;
                }
                isScrollRequestPending = false;
                scheduleDaylyStickyFreeze();
                unPreLoadData();

                highlight_row();
            }
            if (context == 'viewPopupInfoExtension') {
                $('#divExcuteScript').html(resulf);
                $('#txtContentMessage').val($('#txtContentMessage').val().replace(/<br>/gi,
    '\n'));
                $('#divExcuteScript').html('');
                $('#popFlightInfoExtension').modal('show');
                $('#tblPermission tbody tr td').each(function () {
                    var $el = $(this);
                    if ($($el).text() == 'null')
                        $($el).text('');
                });
            }

          if (context == 'viewPopupInfoExtensionInsert') {                
               
                $('#popFlightInfoInsert').modal('show');
               
            }
            if (context == 'btnAccessInfoChange_Onclick') {
                $('#divExcuteScript').html(resulf);
                btnSearch_Onclick();
            }
            if (context == 'UpdateColorBy') {
                alert(resulf);
            }
            if (context == 'SetDefaultColor') {
                alert(resulf);
            }
           
            if (context == 'RuntimeSoundNotification') {
                if (resulf == "1") {
                    playSound('../Sound/DayFlight/sNotifi');
                }
            }
            textColorChange();
        }
        function btnSearch_Onclick() {
            isScrollRequestPending = false;
            hasMoreScrollData = true;
            isSearch = true;
            if (isSearch) {
                preLoadData();
                GetArgWithPostBack(JSON.stringify(getValueSearchDaylyFlight()) + phanCachArg
    + 'btnSearch_Onclick', 'btnSearch_Onclick');
            }
            isSearch = false;
        }
        function btnExport_Onclick() {
             GetArgWithPostBack(JSON.stringify(GetObjectSearchExport()) + phanCachArg
    + 'btnExport_Onclick', 'btnExport_Onclick');
        }
       
        function runtimeCountDown() {
            if (!isRefresh) {
                $('#lblTimeRefresh').text(''); cd = timeRefresh; $('#ddlSelectViewColum').removeAttr('disabled');
                $('#ddlSelectViewColum').prop('selectedIndex', 1); return;
            };
            $('#topBar1 span').removeClass('active');
            $('#ddlSelectViewColum').prop('selectedIndex', 0);
            $('#ddlSelectViewColum').attr('disabled', 'disabled');
            cd--;
            if (cd > 0) {
                setTimeout(runtimeCountDown, 1000);
            } else {
                runtimeRefresh(); setTimeout(runtimeCountDown, 1000); cd = timeRefresh;
            }
            $('#lblTimeRefresh').text('Time refresh: ' + cd);
        }
        function runtimeGetTopBarInfo() {
            if (loadTopbarInfo) {
                GetArgWithPostBack('RenderTopBarInfo' + phanCachArg + 'RenderTopBarInfo',
    'RenderTopBarInfo');
                setTimeout(runtimeGetTopBarInfo, timeRefresh * 1130);
            } else return;
        }
        function runtimeMessageAlert() {
            notifyMe($('#divMessegeChange').html().replace(/<br>/gi, '\n'));
            setTimeout(runtimeMessageAlert, 13200);
        }
        function runtimeSoundNotification() {
            if (isSound) {
                GetArgWithPostBack('RuntimeSoundNotification' + phanCachArg + 'RuntimeSoundNotification',
    'RuntimeSoundNotification');
                var t = setTimeout(runtimeGetTopBarInfo, timeRefresh * 1010);
            } else return;
        }
        function runtimeGetTopbar3() {
            if (loadTopbar3) {
                GetArgWithPostBack('RenderTopBarInfo' + phanCachArg + 'RenderTopBarInfo',
    'RenderTopBarInfo');
                setTimeout(runtimeGetTopBarInfo, timeRefresh * 1020);
            } else return;
        }
        function runtimeRefresh() { //auto load 20 record and default data serach
            if (isRefresh) {
                GetArgWithPostBack(JSON.stringify(getValueSearchDaylyFlight()) + phanCachArg
    + 'btnSearch_Onclick', 'btnSearch_Onclick');
                //setTimeout(runtimeRefresh, timeRefresh * 1000);
            }
        }
        function runtimeAlarm() {
            if ($('#divAlarm').html() != '') {
                notifyMe_New($('#divAlarm').html().replace(/<br>/gi, '\n'));
            } var t = setTimeout(runtimeAlarm, isFirtLoadAlarm ? 5430 : 1800000);
        }
        function preLoadData() {
            preloadImg('grdSource', 'PreLoadData', '30px', '30px');
        }
        function unPreLoadData() {
            unLoadingData('PreLoadData');
        }
        function runtimeRefreshAll() { //auto load 20 record and default data serach
            preLoadData();
            GetArgWithPostBack(JSON.stringify(getValueSearchDaylyFlight()) + phanCachArg + 'btnSearch_Onclick',
            'btnSearch_Onclick');
            setTimeout(runtimeRefreshAll, timeRefresh * 1030);
        }
        function sortTable(iCol) {
            var table, rows, switching, i, x, y, shouldSwitch, s, indd, span;
            table = document.getElementById("grdSource");
            s = $(iCol).attr('data-sort');
            $(iCol).attr('data-sort', s == 'false' ? 'true' : 'false');
            indd = $(iCol.cellIndex)[0];
            span = s == 'true' ? '<p class="glyphicon glyphicon-triangle-bottom"></p>'
    : '<p class="glyphicon glyphicon-triangle-top"></p>';
            $('#grdSource TR').eq(1).find('p').remove();
            $(iCol).append(span);
            rows = table.getElementsByTagName("TR");
            switching = true;
            while (switching) {
                switching = false;
                for (i = 2; i < (rows.length - 1) ; i++) {
                    shouldSwitch = false;
                    x = rows[i].getElementsByTagName("TD")[indd];
                    y = rows[i + 1].getElementsByTagName("TD")[indd];
                    if (s == 'true') {
                        if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
                            shouldSwitch = true;
                            break;
                        }
                    }
                    else {
                        if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
                            shouldSwitch = true;
                            break;
                        }
                    }
                }
                if (shouldSwitch) {
                    rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
                    switching = true;
                }
            }
        }

       
        function btnInsert() {
            var $lis = $('tr[data-isInsert="true"]');
            if ($lis.length == 0) { alert('No Insert.'); return; };            
            var c = 0;
            $.each($lis, function (a, b) {
                var _ata = "", _atd = "", _eta = "", _etd = "", _eobt = "",_soper="";

                var _oper =$('#txt_popCAllSIGN').val();
                var _opertext =$('#txt_popOPER').val();
               
                var _ataorg =$('#txt_popATA').val();
                var _atdorg =$('#txt_popATD').val();

                if(parseInt(_ataorg)>0)
                {
                    if (_ataorg.length < 6)
                    {
                        alert('ATA must input 6 character !');
                        return;
                    }
                }
                if(parseInt(_atdorg)>0)
                {
                    if (_atdorg.length < 6)
                    {
                        alert('ATD must input 6 character !');
                        return;
                    }
                }

                var _etaorg =$('#txt_popETA').val();
                var _etdorg =$('#txt_popETD').val();
                var _eobtorg =$('#txt_popEOBT').val();
                var _status = '0';
                var _perm =$('#txt_popPERMNBR').val();
                if (_perm.length == 0) { _perm = '';_status=''}
                
                if (_opertext.length == 0)
                {
                    _soper = _oper.substr(0,3);                   
                }
                else 
                {
                    _soper = _opertext;                    
                }
               

                //if (_atdorg.length == 4) { _atd = _atdorg; }
                //else if (_atdorg.length == 6) { _atd = _atdorg.substr(2, 5); }
                //else if (_atdorg.length == 0) { _atd = ''; }

                if (_etaorg.length == 4) { _eta = _etaorg; }
                else if (_etaorg.length == 5) { _eta = _etaorg; }
                else if (_etaorg.length == 6) { _eta = _etaorg.substr(2, 5); }
                else if (_etaorg.length == 7) { _eta = _etaorg.substr(2, 6); }
                else if (_etaorg.length == 0) { _eta = ''; }

                if (_etdorg.length == 4) { _etd = _etdorg; }
                else if (_etdorg.length == 6) { _etd = _etdorg.substr(2, 5);}
                else if (_etdorg.length == 0) { _etd = '';}

                if (_eobtorg.length == 4) { _eobt = _eobtorg; }
                else if (_eobtorg.length == 6) { _eobt = _eobtorg.substr(2, 5); }
                else if (_eobtorg.length == 0) { _eobt = ''; }
                
                
                
                var _obj = {};
                _obj['FLIGHT_ID'] = '0';
                _obj['FLIGHTDATE'] = $('#txt_popPERMDATE').val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1');
                _obj['FLIGHTNBR'] = $('#txt_popCAllSIGN').val();
                _obj['PERMTYPE'] = $('#txt_popPERMTYPE').val();
                _obj['PURPOSE'] = $('#txt_popPUR').val();
                _obj['TO_AIRP'] = $('#txt_popTO').val();
                _obj['LASTUSER'] = '<%= _user.UserName.ToString()%>';
                _obj['REGISTRATION'] = $('#txt_popREGIS').val();
                _obj['REMARK'] = $('#txt_popREMARK').val();
                _obj['VALIDHOURS'] = $('#txt_popVALID').val();
                _obj['FLIGHT_TYPE'] = $('#txt_popFLIGHTTYPE').val();
                _obj['CRAFT_ID'] = $('#txt_popCRAFT_T').attr('data-craftid');
                _obj['CRAFT_TYPE'] = $('#txt_popCRAFT_P').val();
                _obj['ATD'] = '';
                _obj['ATA'] = '';
                _obj['VIA'] = $('#txt_popROUTE').val();
                _obj['ROUTE_TT'] = $('#txt_popROUTE_FPL').val();
                _obj['OPER_ID'] = _soper;                
                _obj['ETA'] = _eta;
                _obj['ETD'] = _etd;
                _obj['PERMNBR'] = _perm;
                _obj['FROM_AIRP'] = $('#txt_popFROM').val();
                _obj['STATUS'] = _status;
                _obj['ISACCESS'] = 1;

                _obj['EOBT'] = _eobt;
                if(_atd==''){_obj['ATDDATE'] = '';}else{_obj['ATDDATE'] = _atdorg;}
                if(_ata==''){_obj['ATADATE'] = '';}else{_obj['ATADATE'] = _ataorg;}
                if(_eobt==''){_obj['EOBTDATE'] = '';}else{_obj['EOBTDATE'] = _eobtorg;}
               
               
                var $request = $.ajax({
                    async: false,
                    method: "PUT",
                    url: urlApi + "api/DayFlights/InsertManual_GoingOn",                    
                    data: _obj,
                }).always(function (data) {
                    if (data.Code != -1) c++;
                });
            });
            alert('Insert sussess: ' + c + '/' + $lis.length);            
            btnSearch_Onclick();
        }
       function UpdateInfosFlight()
       {
            var $lis = $('tr[data-isUpdate="true"]');
            if ($lis.length == 0) { alert('No update.'); return; };
            
            var c = 0;
            $.each($lis, function (a, b) {
               
                var _ata = "", _atd = "", _eta = "", _etd = "", _eobt = "";

               
                var _ataorg =$($(b).find('[id^="txtATA"]')[0]).val();
                var _atdorg =$($(b).find('[id^="txtATD"]')[0]).val();

                if(parseInt(_ataorg)>0)
                {
                    if (_ataorg.length < 6)
                    {
                        alert('ATA must input 6 character !');
                        return;
                    }
                }
                if(parseInt(_atdorg)>0)
                {
                    if (_atdorg.length < 6)
                    {
                        alert('ATD must input 6 character !');
                        return;
                    }
                }
                

                var _etaorg =$($(b).find('[id^="txtETA"]')[0]).val();
                var _etdorg =$($(b).find('[id^="txtETD"]')[0]).val();
                var _eobtorg =$($(b).find('[id^="txtEOBT"]')[0]).val();
                                             
                
                if (_etaorg.length == 4) { _eta = _etaorg; }
                else if (_etaorg.length == 5) { _eta = _etaorg; }
                else if (_etaorg.length == 6) { _eta = _etaorg.substr(2, 5); }
                else if (_etaorg.length == 7) { _eta = _etaorg.substr(2, 6); }

                if (_etdorg.length == 4) { _etd = _etdorg; }
                else if (_etdorg.length == 6) { _etd = _etdorg.substr(2, 5);}

                if (_eobtorg.length == 4) { _eobt = _eobtorg; }
                else if (_eobtorg.length == 6) { _eobt = _eobtorg.substr(2, 5); }
                               
                var _obj = {};
                _obj['FLIGHT_ID'] = $(b).prop('id');
                _obj['FLIGHTDATE'] = '';
                _obj['FLIGHTNBR'] = $($(b).find('[id^="txtFLIGHTNBR"]')[0]).val();
                _obj['PERMTYPE'] = $('#ddlPERMTYPE option:selected').text();
                _obj['PURPOSE'] = $($(b).find('[id^="txtPURPOSE"]')[0]).val();
                _obj['TO_AIRP'] = $($(b).find('[id^="txtTO_AIRP"]')[0]).val();
                _obj['LASTUSER'] = '<%= _user.UserName.ToString()%>';
                _obj['REGISTRATION'] = $($(b).find('[id^="txtREGISTRATION"]')[0]).val();
                _obj['REMARK'] = $($(b).find('[id^="txtREMARK"]')[0]).val();
                _obj['VALIDHOURS'] = '0';
                _obj['FLIGHT_TYPE'] = $($(b).find('[id^="txtLETTER_TYPE"]')[0]).val();
                _obj['CRAFT_ID'] = '0';
                _obj['CRAFT_TYPE'] = $($(b).find('[id^="txtCRAFT_TYPE"]')[0]).val();
                _obj['ATA'] = _ataorg;
                _obj['VIA'] = $($(b).find('[id^="txtVIAGOC"]')[0]).val();
                _obj['ROUTE_TT'] = '';
                _obj['OPER_ID'] = '';
                _obj['ATD'] = _atdorg;
                _obj['ETA'] = _eta;
                _obj['ETD'] = _etd;
                _obj['PERMNBR'] = $($(b).find('[id^="txtPERMNBR"]')[0]).val();
                _obj['FROM_AIRP'] = $($(b).find('[id^="txtFROM_AIRP"]')[0]).val();
                _obj['STATUS'] = '0';
                _obj['ISACCESS'] = 1;

                _obj['EOBT'] = _eobt;
                if(_atd==''){_obj['ATDDATE'] = '';}else{_obj['ATDDATE'] = _atdorg;}
                if(_ata==''){_obj['ATADATE'] = '';}else{_obj['ATADATE'] = _ataorg;}
                if(_eobt==''){_obj['EOBTDATE'] = '';}else{_obj['EOBTDATE'] = _eobtorg;}      
                   
         
                
                var $request = $.ajax({
                    async: false,
                    method: "PUT",
                    url: urlApi + "api/DayFlights/UpdateManual_GoingOn_DB",                    
                    data: _obj,
                }).always(function (data) {
                    if (data.Code != -1) c++;                    
                });
            });            
            btnSearch_Onclick();
           
       }

        function checkIsUpdate(id) {
            if ($(id).attr('data-isInsert') != undefined) return;
            $inputs = $(id).closest('tr').find('[data-oldValue]');
            var ci = 0;
            $.each($inputs, function (a, b) {
                switch ($(b).attr('type')) {
                    case 'text':
                        if ($(b).attr('data-oldValue').toUpperCase() != $(b).val().toUpperCase()) { ci++; };
                        break;
                    case 'checkbox':
                        if ($(b).attr('data-oldValue') != ($(b).prop('checked') ? '1' : '0')) ci++;
                        break;
                }
            })
            if (ci > 0) { $(id).closest('tr').attr('data-isUpdate', 'true'); $(id).closest('tr').addClass('success'); }
            else { $(id).closest('tr').attr('data-isUpdate', 'false'); $(id).closest('tr').removeClass('success'); }
        }

        function btnUpdate() {
            var $lis = $('tr[data-isInsert="true"]');
            if ($lis.length == 0) { alert('No Insert.'); return; };
           
            var c = 0;
            $.each($lis, function (a, b) {
                var _ata = "", _atd = "", _eta = "", _etd = "", _eobt = "";

               
                var _ataorg =$('#txt_popATA').val();
                var _atdorg =$('#txt_popATD').val();
                var _etaorg =$('#txt_popETA').val();
                var _etdorg =$('#txt_popETD').val();
                var _eobtorg =$('#txt_popEOBT').val();

                
                //if (_ataorg.length == 4){_ata = _ataorg;}
                //else if (_ataorg.length == 6){_ata = _ataorg.substr(2, 5);}

                //if (_atdorg.length == 4) { _atd = _atdorg; }
                //else if (_atdorg.length == 6) { _atd = _atdorg.substr(2, 5); }
                
                if (_etaorg.length == 4) { _eta = _etaorg; }
                else if (_etaorg.length == 5) { _eta = _etaorg; }
                else if (_etaorg.length == 6) { _eta = _etaorg.substr(2, 5); }
                else if (_etaorg.length == 7) { _eta = _etaorg.substr(2, 6); }

                if (_etdorg.length == 4) { _etd = _etdorg; }
                else if (_etdorg.length == 6) { _etd = _etdorg.substr(2, 5);}

                if (_eobtorg.length == 4) { _eobt = _eobtorg; }
                else if (_eobtorg.length == 6) { _eobt = _eobtorg.substr(2, 5); }
                
                var _Fid= $('#txt_FID').val();
                
                

                var _obj = {};
                _obj['FLIGHT_ID'] =_Fid;
                _obj['FLIGHTDATE'] = $('#txt_popPERMDATE').val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1');
                _obj['FLIGHTNBR'] = $('#txt_popCAllSIGN').val();
                _obj['PERMTYPE'] = $('#txt_popPERMTYPE').val();
                _obj['PURPOSE'] = $('#txt_popPUR').val();
                _obj['TO_AIRP'] = $('#txt_popTO').val();
                _obj['LASTUSER'] = '<%= _user.UserName.ToString()%>';
                _obj['REGISTRATION'] = $('#txt_popREGIS').val();
                _obj['REMARK'] = $('#txt_popREMARK').val();
                _obj['VALIDHOURS'] = $('#txt_popVALID').val();
                _obj['FLIGHT_TYPE'] = 'SC';
                _obj['CRAFT_ID'] = $('#txt_popCRAFT_T').attr('data-craftid');
                _obj['CRAFT_TYPE'] = $('#txt_popCRAFT_P').val();
                _obj['ATA'] = _ataorg;
                _obj['VIA'] = $('#txt_popROUTE').val();
                _obj['ROUTE_TT'] = $('#txt_popROUTE_FPL').val();
                _obj['OPER_ID'] = $('#txt_popOPER').val();
                _obj['ATD'] = _atdorg;
                _obj['ETA'] = _eta;
                _obj['ETD'] = _etd;
                _obj['PERMNBR'] = $('#txt_popPERMNBR').val();
                _obj['FROM_AIRP'] = $('#txt_popFROM').val();
                _obj['STATUS'] = '0';
                _obj['ISACCESS'] = 1;

                _obj['EOBT'] = _eobt;
                if(_atd==''){_obj['ATDDATE'] = '';}else{_obj['ATDDATE'] = _atdorg;}
                if(_ata==''){_obj['ATADATE'] = '';}else{_obj['ATADATE'] = _ataorg;}
                if(_eobt==''){_obj['EOBTDATE'] = '';}else{_obj['EOBTDATE'] = _eobtorg;}
                              
                
                var $request = $.ajax({
                    async: false,
                    method: "PUT",
                    url: urlApi + "api/DayFlights/UpdateManual_GoingOn",                    
                    data: _obj,
                }).always(function (data) {
                    if (data.Code != -1) c++;
                    btnUpdateStatusLetter(_Fid);
                });
            });
            
            alert('Update sussess: ' + c + '/' + $lis.length);            
            btnSearch_Onclick();
        }

        function btnClearInput() {
            var _date = $('#ddlDateFlight option:selected').text() == '' ? new Date().format('dd-MM-yyyy') : $('#ddlDateFlight option:selected').text().replace(/\//gi, '-').replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$1/$2/$3').replace('/','-').replace('/','-');
                $('#txt_popPERMNBR').val('');
                $('#txt_popREGIS').val('');
                $('#txt_popCAllSIGN').val('');
                $('#txt_popFROM').val('');
                $('#txt_popTO').val('');
                $('#txt_popETD').val('');
                $('#txt_popEOBT').val('');
                $('#txt_popETA').val('');
                $('#txt_popPERMDATE').val(_date);
                $('#txt_popOPER').val('');

                $('#txt_popATD').val('');
                $('#txt_popATA').val('');
                $('#txt_popROUTE').val('');
                $('#txt_popROUTE_FPL').val('');
                $('#txt_popCRAFT_T').val('');
                $('#txt_popCRAFT_P').val('');

                $('#txt_popPERMTYPE').val('LD');
                $('#txt_popFLIGHTTYPE').val('NO');
                $('#txt_popPUR').val('PAX');
                $('#txt_popVALID').val('24');
                $('#txt_popREMARK').val('');
                $('#txt_FID').val('0');
            
                $('#ddlSelectPop').prop('selectedIndex', 0);
                lbidBnr.innerText = '';
        }

    </script>
    <script>
        $(function () {
            var $tableScroll = $('#table-container > .table-responsive');
            $tableScroll.off('scroll.daylyLoadMore').on('scroll.daylyLoadMore', function () {
                var distanceToBottom = this.scrollHeight - this.scrollTop - this.clientHeight;
                if (this.scrollHeight <= this.clientHeight
                    || distanceToBottom > 40
                    || isScrollRequestPending
                    || !hasMoreScrollData) return;

                isScrollRequestPending = true;
                isSroll = true;
                LoadGrdSourceScroll();
                isSroll = false;
            });
        });
        function LoadGrdSourceScroll() {
            if (isSroll) {
                GetArgWithPostBack(JSON.stringify(getValueSearchDaylyFlight()) + phanCachArg
    + 'LoadGrdSourceScroll', 'LoadGrdSourceScroll');
                preLoadData()
            }
        }
        function sortTableSpan(f, n) {
            var rows = $('#grdSource tbody  tr').get();
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
                var v = $(elm).children('td').eq(n).text().toUpperCase();
                if ($.isNumeric(v)) {
                    v = parseInt(v, 10);
                }
                return v;
            }

            $.each(rows, function (index, row) {
                $('#grdSource').children('tbody').append(row);
            });
        }
        function sortOnclickSpan(ele) {

            var s = parseInt($(ele).attr('data-sort'));
            s *= -1;
            $(ele).attr('data-sort', s);
            var span = s == '1' ? '<p class="glyphicon glyphicon-triangle-bottom"></p>'
    : '<p class="glyphicon glyphicon-triangle-top"></p>';
            $('#grdSource TR').eq(1).find('p').remove();
            $(ele).append(span);

            var n = $(ele).prevAll().length;
            sortTableSpan(s, n);
        }

        function sortOnclick(ele) {

            var s = parseInt($(ele).attr('data-sort'));
            s *= -1;
            $(ele).attr('data-sort', s);
            var span = s == '1' ? '<p class="glyphicon glyphicon-triangle-bottom"></p>'
    : '<p class="glyphicon glyphicon-triangle-top"></p>';
            $('#grdSource TR').eq(1).find('p').remove();
            $(ele).append(span);

            var n = $(ele).prevAll().length;
            sortTable(s, n);
        }
        function sortTable(f, n) {
            var rows = $('#grdSource tbody  tr').get();
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
                var v = $(elm).children('td').eq(n).find('input').val().toUpperCase();
                if ($.isNumeric(v)) {
                    v = parseInt(v, 10);
                }
                return v;
            }

            $.each(rows, function (index, row) {
                $('#grdSource').children('tbody').append(row);
            });
        }
         
    </script>
    <script>
        <%--var urlApi = '<%= ConfigurationManager.AppSettings["ApplicationPath.API"]%>';--%>
        function viewPopupInfoExtension(ele) {
            $('#txtStatusLetter').val('');
            var ax = $(ele);
            $('#grdSource tr').removeClass('active');
            ax.addClass('active');
            GetArgWithPostBack(ax.attr('data-id') + phanCach + ax.attr('data-timeM')
    + phanCach + ax.attr('data-CallSign') + phanCachArg + 'viewPopupInfoExtension',
    'viewPopupInfoExtension');

            /*
            get permission info
            */
            $('#lblLinkFile').html('');
            $('#txtAuthorPerm').val('');
            $('#txtOperPerm').val('');
            $('#txtFlightNbrPerm').val('');
            $('#txtPermTypePerm').val('');
            $('#txtVersionPerm').val('');
            $('#txtSeasonPerm').val('');
            $('#txtDatePerm').val('');
            $('#txtHoursPerm').val('');
            var $permNbr = $($(ele).find('td')[2]).text();
            //if ($permNbr == 'NoPerm') return;
            var kq = '';
            var url = urlApi + '/api/DayFlights/GetPermBy?ID=' + ax.attr('data-id');
            $.ajax({
                method: "GET",
                url: url,
            }).always(function (data) {
                $('#tblPermission tr').remove();
                if (data.ListValue == null) return;
                $('#txtAuthorPerm').val(data.ListValue[0]['AUTHOR_ID']);
                $('#txtOperPerm').val(data.ListValue[0]['OPER_ID']);
                $('#txtFlightNbrPerm').val(data.ListValue[0]['PERMNBR']);
                $('#txtPermTypePerm').val(data.ListValue[0]['PERMTYPE']);
                $('#txtVersionPerm').val(data.ListValue[0]['VERSION']);
                $('#txtSeasonPerm').val(data.ListValue[0]['SEASON']);
                $('#txtDatePerm').val(new Date(data.ListValue[0]['PERMDATE']).format('dd-mm-yyyy'));
                $('#txtHoursPerm').val(data.ListValue[0]['VALIDHOURS']);
                getLinkfile(urlApi + '/api/DayFlights/GetLinkFile', ax.attr('data-id'), 'SC');
                if (data.ListValue[0]['FLIGHTTYPE'] == 'NO')
                    kq += '<thead><tr><th>Call sign</th><th>Registration</th><th>From</th><th>To</th><th>Etd</th><th>Eta</th><th>Day flight</th><th>Craft</th><th>Purpose</th><th>Via</th><th>Remark</th><th>LastModify</th><th></th></tr></thead>';
                else kq += '<thead><tr><th>Call Sign</th><th>Registration</th><th>From</th><th>To</th><th>Etd</th><th>Eta</th><th>DAY</th><th>Craft</th><th>Begin date</th><th>End date</th><th>Purpose</th><th>Via</th><th>Remark</th></tr>';
                kq += '<tbody>';
                $.each(data.ListValue, function (a, b) {
                    kq += '<tr>';
                    kq += '<td>' + b['FLIGHTNBR'] + '</td>';
                    kq += '<td>' + b['REGISTRATION'] + '</td>';
                    kq += '<td>' + b['FROM_AIRP'] + '</td>';
                    kq += '<td>' + b['TO_AIRP'] + '</td>';
                    kq += '<td>' + b['ETD'] + '</td>';
                    kq += '<td>' + b['ETA'] + '</td>';
                    if (b['FLIGHT_TYPE'] = 'SC')
                        kq += '<td>' + (b['DAY1'] + b['DAY2'] + b['DAY3'] + b['DAY4'] + b['DAY5'] + b['DAY6'] + b['DAY7']).replace(/0/gi, '.') + '</td>';
                    else kq += '<td>' + b['DAYFLIGHT'] + '</td>';
                    kq += '<td>' + b['MA'] + '</td>';
                    if (b['FLIGHT_TYPE'] = 'SC') {
                        kq += '<td style=\"white-space: nowrap;\">' + (b['BEGINDATE'] == null ? '' : new Date(b['BEGINDATE']).format('dd-mm-yyyy')) + '</td>';
                        kq += '<td style=\"white-space: nowrap;\">' + (b['ENDDATE'] == null ? '' : new Date(b['ENDDATE']).format('dd-mm-yyyy')) + '</td>';
                    }
                    kq += '<td>' + b['PURPOSE_ID'] + '</td>';
                    kq += '<td style=\"word-break: break-all;\">' + b['VIA'] + '</td>';
                    kq += '<td style=\"word-break: break-all;\">' + b['REMARK'] + '</td>';
                    kq += '</tr>'
                })
                kq += '</tbody>';
                $('#tblPermission').append(kq);
            });
        }
        function getLinkfile(url, id, ty) {
            var url1 = url + '?id=' + id + '&permtype=' + ty;
            $.ajax({
                method: "GET",
                url: url1,
            }).always(function (ms) {
                if (ms.ListValue == null) return;
                $.each(ms.ListValue, function (a, b) {
                    $('#lblLinkFile').append('<a href="' + b['URLPATH'] + b['FILENAME'] + '">' + b['FILENAME'] + '</a></br>');

                })
            });

        }
        function btnUpdateStatusLetter_OnClick(id) {
            if ($('#txtStatusLetter').val() == '') return;
            var url = urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=FLIGHT_DAYFLIGHT&storeName=updateFlightBy_LetterType";
            $.ajax({
                async: false,
                method: "PUT",
                url: url,
                data: JSON.stringify({ P_ID: id, P_USER: '<%= _user.UserName%>', P_LETTER_TYPE: $('#ddlSelectView').val().toUpperCase(), P_REMARK: $('#txtRemark').val(), P_REGISTRATION: $('#txtRegis').val(), P_EOBT: $('#txtEobt').val(), P_ATD: $('#txtAtd2').val(), P_ATA: $('#txtAta').val(), P_PURPOSE: $('#txtPupose').val() }),
            }).always(function (data) {
                if (data.ListValue == null || data.ListValue == -1) {
                    alert('Update error!');
                } else alert('Update success!');
            })
        }
         function LoadDataGrid_Finished() {
            var cf = confirm('Do you want export finished ?');
            if (cf) {
                var url = urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=MAKE_FINISHED&storeName=make_finished_flights_news";
                $.ajax({
                    async: false,
                    method: "PUT",
                    url: url,
                    data: JSON.stringify({ P_STRING: 'TEST' }),
                }).always(function (data) {
                    if (data.ListValue == null || data.ListValue == -1) {
                        alert('Export Finished error!');
                    } alert('Success !');
                })
            }
           
        }


        function ddlExport_Change() {
            var _exp = $('#ddlExport').val();
            var _date =$('#ddlDateFlight').val();
            if ((_exp != "2")&&(_date=='-1')) {
                Move_Finished();
            } else {
                btnSearch_Onclick();
            }
        }

        function Move_Finished() {
           
            var url = urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=MAKE_FINISHED&storeName=make_finished_flights_news";
            $.ajax({
                async: false,
                method: "PUT",
                url: url,
                data: JSON.stringify({ P_STRING: 'TEST' }),
            }).always(function (data) {
                if (data.ListValue == null || data.ListValue == -1) {                       
                }
            })
            btnSearch_Onclick(); 
           
        }

        function btnDeleteBy_Onclick(idDelete) {
             var cf = confirm('Do you want delete ?');
             if (cf) {              
                    
                    var $request = $.ajax({
                        async: false,
                        method: "DELETE",
                        url: urlApi + "api/DayFlights/Delete/" + idDelete,
                    }).always(function (data) {
                        if (data.Code != -1) {
                            row_DeleteGoing(idDelete);
                            btnSearch_Onclick();  
                        }
                    });
                }
                 
           
        }
        function row_DeleteGoing(idDelete) {
               
                var url = "<%=System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]%>api/ApiExtension/ExcuteReturnInt?packageName=A_TEST_SEARCH&storeName=DELETEGOINGON";
                $.ajax({
                    async: false,
                    method: "PUT",
                    url: url,
                    data: JSON.stringify({ P_ID: idDelete}),
                }).always(function (data) {
                    if (data.ListValue == null || data.ListValue == -1) {                       
                    }
              })
        }

        function viewPopupInfoExtensionInsert(ele) {
            
            var ax = $(ele);
            var _fid=0;
            ax.addClass('active');
            GetArgWithPostBack(ax.attr('data-id') + phanCach + ax.attr('data-timeM')
    + phanCach + ax.attr('data-CallSign') + phanCachArg + 'viewPopupInfoExtensionInsert',
    'viewPopupInfoExtensionInsert');
            
            btnClearInput();
             
            if (ax.attr('data-id') == '0') {
                document.getElementById('btnUpdateInfoChange').style.display="none";
                document.getElementById('btnSaveInfoChange').style.display = "none";
                document.getElementById('txt_popATD').disabled=true;
                document.getElementById('txt_popATA').disabled=true;
                document.getElementById('txt_popPERMDATE').disabled=true;
                
               document.getElementById('txt_popPERMNBR').disabled=false;
                 document.getElementById('txt_popCRAFT_T').disabled=false;
                
            }
            else
            {
                document.getElementById('btnSaveInfoChange').style.display="none";
                document.getElementById('btnUpdateInfoChange').style.display = "none";
                document.getElementById('txt_popATD').disabled=false;
                document.getElementById('txt_popATA').disabled=false;

                document.getElementById('txt_popPERMNBR').disabled=true;
                document.getElementById('txt_popCRAFT_T').disabled=true;
            }
           
            $('#txt_FID').val(ax.attr('data-id'));

            var url = urlApi + '/api/DayFlights/GetPerm_GoingOnBy?ID=' + ax.attr('data-id');
            
            $.ajax({
                method: "GET",
                url: url,
            }).always(function (data) {                
                if (data.ListValue == null) return;
                $('#txt_popPERMNBR').val(data.ListValue[0]['PERMNBR']);
                $('#txt_popREGIS').val(data.ListValue[0]['REGISTRATION']);
                $('#txt_popCAllSIGN').val(data.ListValue[0]['FLIGHTNBR']);
                $('#txt_popFROM').val(data.ListValue[0]['FROM_AIRP']);
                $('#txt_popTO').val(data.ListValue[0]['TO_AIRP']);

                lbidBnr.innerText = data.ListValue[0]['FLIGHTNBR'];

                if(data.ListValue[0]['ETD'].length>0)
                {
                    $('#txt_popETD').val(new Date(data.ListValue[0]['FLIGHTDATE']).format('dd') + data.ListValue[0]['ETD']);
                }
                else
                {
                    $('#txt_popETD').val('');
                }
                
                $('#txt_popEOBT').val(data.ListValue[0]['EOBTDATE']);

                
                if(data.ListValue[0]['ETA']!=null)
                {
                   
                     if(data.ListValue[0]['ETA'].length >=5)
                     {
                         
                         var _date = new Date(data.ListValue[0]['FLIGHTDATE']);
                          $('#txt_popETA').val((_date.getDate()+1).format('dd') + data.ListValue[0]['ETA']);
                     }
                    else
                     {
                          $('#txt_popETA').val(new Date(data.ListValue[0]['FLIGHTDATE']).format('dd') + data.ListValue[0]['ETA']);
                     }
                    
                    
                }
                else
                {
                    $('#txt_popETA').val(data.ListValue[0]['ETA']);
                }
                
                $('#txt_popPERMDATE').val(new Date(data.ListValue[0]['FLIGHTDATE']).format('dd-mm-yyyy'));
                $('#txt_popOPER').val(data.ListValue[0]['OPER_ID']);

                $('#txt_popATD').val(data.ListValue[0]['ATD']);
                $('#txt_popATA').val(data.ListValue[0]['ATA']);
                $('#txt_popROUTE').val(data.ListValue[0]['VIA']);
                $('#txt_popROUTE_FPL').val(data.ListValue[0]['ROUTE_TT']);
                $('#txt_popCRAFT_T').val(data.ListValue[0]['MA']);
                $('#txt_popCRAFT_P').val(data.ListValue[0]['CRAFT_TYPE']);

                $('#txt_popPERMTYPE').val(data.ListValue[0]['PERMTYPE']);
                $('#txt_popFLIGHTTYPE').val(data.ListValue[0]['FLIGHT_TYPE']);
                $('#txt_popPUR').val(data.ListValue[0]['PURPOSE']);
                $('#txt_popVALID').val(data.ListValue[0]['VALIDHOURS']);
                $('#txt_popREMARK').val(data.ListValue[0]['REMARK']);
                
               
            });

        }
        
        function btnUpdateStatusLetter(id) {
            if ($('#ddlSelectPop').val() == '--') return;
            var url = urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=A_TEST_SEARCH&storeName=updateFlightBy_LetterType";
            $.ajax({
                async: false,
                method: "PUT",
                url: url,
                data: JSON.stringify({ P_ID: id, P_USER: '<%= _user.UserName%>', P_LETTER_TYPE: $('#ddlSelectPop').val().toUpperCase()}),
            }).always(function (data) {
                if (data.ListValue == null || data.ListValue == -1) {
                };
            })
        }

        function moveDate(id) {
             var cf = confirm('Do you want move date ?');
             if (cf) {                 
                 var url = urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=A_TEST_SEARCH&storeName=MOVEDATE_GOINGON";
                 
                $.ajax({
                    async: false,
                    method: "PUT",
                    url: url,
                    data: JSON.stringify({ P_ID: id, P_USER: '<%= _user.UserName%>'}),
                }).always(function (data) {
                    if (data.ListValue == null || data.ListValue == -1) {
                    };
                })
           }
           
        }
    </script>
    <script>
        function textColorChange() {
            $('#grdSource .cssTextColChange').each(function () {
                var ele = $(this);
                var span = $(ele).find('span').addClass('cssTextColChange');
            });
        }
        $('[data-ModeColor="true"]').each(function () {
            $(this).spectrum({
                color: $(this).attr('data-setcolor'),
                change: function (color) {
                    reStyleColor();
                    UpdateColorBy('<%= _user.UserID%>', color.toHexString(), $(this).attr('data-className'));
                },
                chooseText: 'Select',
                cancelText: 'Cancel',
            })
        });
        function reStyleColor() {
            $('#styCSS').html('');
            $('[data-ModeColor="true"]').each(function () {
                var cR = '.' + $(this).attr('data-className') + '{' + ($(this).attr('data-className').indexOf('cssText')
    == 0 ? 'color: ' : 'background-color: ') + $(this).spectrum('get').toHexString()
    + '!important;} ';
                $('#styCSS').append(cR);
            });
            $('[data-modefontsize="true"]').each(function () {
                var el = $(this).val() + 'px';
                $('#grdSource tbody').css('font-size', el);
            });
            textColorChange();
        }
        function UpdateColorBy(u, c, cl) {
            GetArgWithPostBack(u + phanCach + c + phanCach + cl + phanCachArg + 'UpdateColorBy',
    'UpdateColorBy');
        }
        function SetDefaultColor(cl, u) {
            GetArgWithPostBack(cl + phanCach + u + phanCachArg + 'SetDefaultColor',
    'SetDefaultColor');
            $('#txtMc' + cl).spectrum('set', $('#txtMc' + cl).attr('data-ColorDefault'));
            reStyleColor();
        }
        reStyleColor();
    </script>
    <script>
        function playSound(filename) {
            document.getElementById("sound").innerHTML = '<audio autoplay="autoplay"><source src="' + filename + '.mp3" type="audio/mpeg" /><source src="' + filename + '.ogg" type="audio/ogg" /><embed hidden="true" autostart="true" loop="false" src="' + filename
            + '.mp3" /></audio>';
        }
    </script>
    <script>
        function applyDaylyStickyFreeze() {
            var table = document.getElementById('grdSource');
            if (!table || !table.tHead || !table.tHead.rows.length) return;

            function isTransparentColor(color) {
                return !color || color === 'transparent' || color === 'rgba(0, 0, 0, 0)' ||
                    /rgba\([^)]*,\s*0(?:\.0+)?\s*\)$/.test(color);
            }

            function getEffectiveBackground(cell) {
                cell.style.removeProperty('background-color');
                var element = cell;
                while (element && element !== table.parentNode) {
                    var color = window.getComputedStyle(element).backgroundColor;
                    if (!isTransparentColor(color)) return color;
                    element = element.parentElement;
                }
                return '#ffffff';
            }

            var referenceRow = table.tHead.rows.length > 1
                ? table.tHead.rows[table.tHead.rows.length - 1]
                : table.tHead.rows[0];
            var columnWidths = [];
            for (var i = 0; i < 4 && i < referenceRow.cells.length; i++) {
                columnWidths.push(referenceRow.cells[i].getBoundingClientRect().width);
            }

            for (var rowIndex = 0; rowIndex < table.rows.length; rowIndex++) {
                var logicalColumn = 0;
                for (var cellIndex = 0; cellIndex < table.rows[rowIndex].cells.length; cellIndex++) {
                    var cell = table.rows[rowIndex].cells[cellIndex];
                    var span = parseInt(cell.getAttribute('colspan') || '1', 10);
                    if (logicalColumn < 4) {
                        var left = 0;
                        for (var col = 0; col < logicalColumn; col++) left += columnWidths[col] || 0;
                        cell.classList.add('dayly-frozen-column');
                        cell.style.left = Math.round(left) + 'px';
                        cell.style.setProperty('background-color', getEffectiveBackground(cell));
                        if (logicalColumn + span >= 4) cell.classList.add('dayly-freeze-edge');
                    }
                    logicalColumn += span;
                }
            }

            var stickyTop = 0;
            for (var headerIndex = 0; headerIndex < table.tHead.rows.length; headerIndex++) {
                var headerRow = table.tHead.rows[headerIndex];
                for (var headerCell = 0; headerCell < headerRow.cells.length; headerCell++) {
                    headerRow.cells[headerCell].style.top = Math.round(stickyTop) + 'px';
                }
                stickyTop += headerRow.getBoundingClientRect().height;
            }
        }

        function initializeDaylyHorizontalControls() {
            var container = document.getElementById('table-container');
            var mainScroll = container ? container.querySelector('.table-responsive') : null;
            var leftButton = document.getElementById('daylyScrollLeft');
            var rightButton = document.getElementById('daylyScrollRight');
            if (!mainScroll || !leftButton || !rightButton || rightButton.getAttribute('data-bound') === 'true') return;

            rightButton.setAttribute('data-bound', 'true');
            leftButton.setAttribute('data-bound', 'true');

            function getOneColumnWidth() {
                var table = document.getElementById('grdSource');
                if (!table || !table.tHead || !table.tHead.rows.length) return 120;
                var row = table.tHead.rows[table.tHead.rows.length - 1];
                var cell = row.cells.length > 4 ? row.cells[4] : row.cells[row.cells.length - 1];
                return cell ? Math.max(60, Math.round(cell.getBoundingClientRect().width)) : 120;
            }

            function bindDirectionButton(button, direction) {
                var holdTimer = null;
                var holdFrame = null;
                var isHolding = false;

                function scrollOneColumn() {
                    mainScroll.scrollBy({ left: direction * getOneColumnWidth(), behavior: 'smooth' });
                }

                function continuousScroll() {
                    mainScroll.scrollLeft += direction * 4;
                    holdFrame = window.requestAnimationFrame(continuousScroll);
                }

                function beginHold(event) {
                    if (event.button !== undefined && event.button !== 0) return;
                    event.preventDefault();
                    isHolding = false;
                    if (button.setPointerCapture && event.pointerId !== undefined) button.setPointerCapture(event.pointerId);
                    holdTimer = window.setTimeout(function () {
                        isHolding = true;
                        button.classList.add('is-holding');
                        continuousScroll();
                    }, 280);
                }

                function endHold(event) {
                    if (holdTimer === null && holdFrame === null) return;
                    window.clearTimeout(holdTimer);
                    holdTimer = null;
                    if (holdFrame !== null) window.cancelAnimationFrame(holdFrame);
                    holdFrame = null;
                    button.classList.remove('is-holding');
                    if (!isHolding) scrollOneColumn();
                    isHolding = false;
                    if (event) event.preventDefault();
                }

                button.addEventListener('pointerdown', beginHold);
                button.addEventListener('pointerup', endHold);
                button.addEventListener('pointercancel', endHold);
                button.addEventListener('lostpointercapture', endHold);
                button.addEventListener('click', function (event) {
                    if (event.detail === 0) scrollOneColumn();
                });
            }

            bindDirectionButton(leftButton, -1);
            bindDirectionButton(rightButton, 1);
        }

        function scheduleDaylyStickyFreeze() {
            window.requestAnimationFrame(function () {
                applyDaylyStickyFreeze();
                window.requestAnimationFrame(applyDaylyStickyFreeze);
            });
        }

        $(document).ready(function () {
            $('#optionColor').appendTo('.dayly-top-actions');
            initializeDaylyHorizontalControls();
            scheduleDaylyStickyFreeze();
            btnSearch_Onclick();

            var resizeTimer;
            $(window).on('resize.daylyHcmSticky', function () {
                clearTimeout(resizeTimer);
                resizeTimer = setTimeout(applyDaylyStickyFreeze, 120);
            });
        });
    </script>
    <script type="text/javascript">

        function generate_excel(html) {
            exportExel(html);
        }

        function exportExel(_html) {

            var dt = new Date();
            var day = dt.getDate();
            var month = dt.getMonth() + 1;
            var year = dt.getFullYear();
            var hour = dt.getHours();
            var mins = dt.getMinutes();
            var postfix = day + "." + month + "." + year + "_" + hour + "." + mins;

            var strHtml = "<table id='tblSourceExport' class='table table-bordered'>";
            strHtml += "<thead style='color: red'>"
                    + "<tr>"
                    + "<th></th>"
                    + "<th>PERM</th>"
                    + "<th>REGIS</th>"
                    + "<th>CallSign</th>"
                    + "<th>FROM</th>"
                    + "<th>TO</th>"
                    + "<th>ETD</th>"
                    + "<th>EOBT</th>"
                    + "<th>ETA</th>"
                    + "<th>ATD</th>"
                    + "<th>ATA</th>"
                    + "<th>ROUTE</th>"
                    + "<th>ROUTE_FPL</th>"
                    + "<th>CRAFT_T</th>"
                    + "<th>CRAFT_P</th>"
                    + "<th>PUPOSE</th>"
                    + "<th>VIA</th>"
                    + "<th>REMARK</th>"
                    + "</tr>"
                    + "</thead>"
                    + _html
                    + "<tbody>"
                    + "</tbody>"
                    + "</table>";



            var textToSave = strHtml;
            var textToSaveAsBlob = new Blob([textToSave], { type: "text/plain" });
            var textToSaveAsURL = window.URL.createObjectURL(textToSaveAsBlob);
            var fileNameToSaveAs = 'exported_dbhdb_' + postfix + '.xls';
            var downloadLink = document.createElement("a");
            downloadLink.download = fileNameToSaveAs;
            downloadLink.innerHTML = "Download File";
            downloadLink.href = textToSaveAsURL;
            downloadLink.onclick = destroyClickedElement;
            downloadLink.style.display = "none";
            document.body.appendChild(downloadLink);
            downloadLink.click();

        }
        function destroyClickedElement(event) {
            document.body.removeChild(event.target);
        }

        function GetObjectSearchExport() {
            var khungGio = [0, 23];
            var ax = document.getElementById('topBar1').getElementsByClassName('active');
            if (ax.length == 1)
                khungGio = [ax[0].innerText, ax[0].innerText];
            else if (ax.length == 0)
                khungGio = [0, 23];
            else khungGio = [ax[0].innerText, ax[ax.length - 1].innerText];



            var _obj = {
                PERMNBR: $('#txtPERMNBR').val(),
                FLIGHTNBR: $('#txtFLIGHTNBR').val(),
                FROM_AIRP: $('#ddlFROM_AIRP').val(),
                TO_AIRP: $('#ddlTO_AIRP').val(),
                ETA: isRefresh ? '' : ($('#ddlSelectViewColum').val() == 'ETA' ? 'ETA'
    : ''),
                ETD: isRefresh ? '' : ($('#ddlSelectViewColum').val() == 'ETD' ? 'ETD'
    : ''),
                ATD: isRefresh ? '' : ($('#ddlSelectViewColum').val() == 'ATD' ? 'ATD'
    : ''),
                ATA: isRefresh ? '' : ($('#ddlSelectViewColum').val() == 'ATA' ? 'ATA'
    : ''),
                CRAFT_ID: $('#ddlCRAFT_T').attr('data-craftid'),
                CRAFT_TYPE: $('#ddlCRAFT_TP').val(),
                REGISTRATION: $('#txtREGISTRATION').val(),
                PURPOSE: $('#txtPURPOSE').val(),
                REMARK: $('#txtREMARK').val(),
                FLIGHT_TYPE: $('#ddlSelect').val(),
                PERMTYPE: $('#ddlPERMTYPE').val(),
                KHUNGGIO1: cKhungGio1(khungGio[0]),
                KHUNGGIO2: cKhungGio2(khungGio[1]),
                LETTER_TYPE: $('#ddlLetter_Type').val(),
                RowStart: isSroll ? (parseInt($('#grdSource tbody tr').last().attr('data-RowNumber'))
    + 1) : 0,
                RowFinish: isSroll ? (parseInt($('#grdSource tbody tr').last().attr('data-RowNumber'))
    + appenLoad) : 6000,
                OptionDate: $('#ddlDateFlight').val(),
                FLIGHTDATE: $('#ddlDateFlight option:selected').text() == '' ? new Date().format('dd/MM/yyyy') : $('#ddlDateFlight option:selected').text().replace(/\//gi, '-').replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$1/$2/$3'),
                ROUTE: $('#txtROUTE').val(),
                ROUTE_TT: $('#txtROUTE_TP').val()
            }
            sObj = _obj;
            return _obj;
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

            }
            $(id).focus();
        }
    </script>

    <script>
        highlight_row();
        function highlight_row() {
            var table = document.getElementById('grdSource');
            var cells = table.getElementsByTagName('td');

            for (var i = 0; i < cells.length; i++) {
                // Take each cell
                var cell = cells[i];
                // do something on onclick event for cell
                cell.onclick = function () {
                    // Get the row id where the cell exists
                    var rowId = this.parentNode.rowIndex;

                    var rowsNotSelected = table.getElementsByTagName('tr');
                    for (var row = 0; row < rowsNotSelected.length; row++) {
                        rowsNotSelected[row].style.backgroundColor = "";
                        rowsNotSelected[row].classList.remove('selected');
                    }
                    var rowSelected = table.getElementsByTagName('tr')[rowId];
                    rowSelected.style.backgroundColor = "blue";
                    rowSelected.className += " selected";

                    //alert(rowId-1);
                }
            }

        }
     
       
    </script>

</asp:Content>
