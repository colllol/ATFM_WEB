<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="DaylyFlight.aspx.cs" Inherits="prjApplication.Day_Flights.DaylyFlight" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="prjBusinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

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
            height: clamp(420px, calc(100vh - 390px), 720px);
            min-height: 0;
            flex-direction: column;
            width: 100%;
            max-width: 100%;
            box-sizing: border-box;
            margin: 10px 0 20px !important;
            overflow: hidden;
            border: 1px solid #c8d9e8;
            border-radius: 10px;
            background: #fff;
            box-shadow: 0 7px 22px rgba(26, 67, 105, .13);
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
            scrollbar-width: auto;
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

        #table-container > .table-responsive::-webkit-scrollbar-thumb:hover {
            background: #205b8f;
        }

        .dayly-top-scroll {
            flex: 0 0 18px;
            width: 100%;
            height: 18px;
            overflow-x: auto;
            overflow-y: hidden;
            border-bottom: 1px solid #c8d9e8;
            background: #e5edf4;
            scrollbar-color: #337ab7 #e5edf4;
            scrollbar-width: auto;
        }

        .dayly-top-scroll-content {
            height: 1px;
        }

        .dayly-top-scroll::-webkit-scrollbar {
            height: 14px;
        }

        .dayly-top-scroll::-webkit-scrollbar-track {
            background: #e5edf4;
        }

        .dayly-top-scroll::-webkit-scrollbar-thumb {
            background: #337ab7;
            border: 3px solid #e5edf4;
            border-radius: 8px;
        }

        .dayly-scroll-right {
            position: absolute;
            top: 50%;
            right: 3px;
            z-index: 120;
            display: flex;
            width: 38px;
            height: 54px;
            align-items: center;
            justify-content: center;
            border: 1px solid rgba(255, 255, 255, .75);
            border-radius: 10px 0 0 10px;
            background: linear-gradient(135deg, #337ab7, #185d93);
            color: #fff;
            box-shadow: 0 5px 14px rgba(20, 68, 105, .30);
            transform: translateY(-50%);
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
        .dayly-scroll-right.is-holding {
            transform: translateY(-50%) scale(.96);
        }

        .dayly-scroll-right .fa {
            font-size: 20px;
        }

        .dayly-scroll-controls {
            position: absolute;
            top: 50%;
            right: -11px;
            bottom: auto;
            z-index: 120;
            display: flex;
            flex-direction: column;
            gap: 6px;
            transform: translateY(-50%);
        }

        .dayly-scroll-controls .dayly-scroll-right {
            position: static;
            width: 34px;
            height: 40px;
            border-radius: 8px;
            transform: none;
        }

        .dayly-scroll-controls .dayly-scroll-right .fa {
            font-size: 16px;
        }

        .dayly-scroll-controls .dayly-scroll-right:active,
        .dayly-scroll-controls .dayly-scroll-right.is-holding {
            transform: scale(.95);
        }

        .dayly-table-actions {
            display: flex;
            min-width: 0;
            align-items: center;
            gap: 7px;
            padding: 2px 3px;
        }

        .dayly-table-actions a {
            display: inline-flex;
            width: 30px;
            height: 30px;
            align-items: center;
            justify-content: center;
            border: 1px solid #afd1e7;
            border-radius: 8px;
            background: #fff;
            color: #237fb9 !important;
            box-shadow: 0 3px 8px rgba(30, 89, 128, .12);
            cursor: pointer;
        }

        .dayly-table-actions a:hover {
            border-color: #2d8bc3;
            background: #2d8bc3;
            color: #fff !important;
            transform: translateY(-2px);
        }

        .dayly-table-actions .glyphicon { font-size: 15px; }

        #grdSource thead tr.Spec > td.dayly-action-cell {
            width: 145px;
            min-width: 145px;
            max-width: 145px;
            padding-left: 5px !important;
            border-right: 2px solid #2d8bc3 !important;
        }

        #grdSource thead tr:last-child > th:nth-child(2),
        #grdSource tbody > tr > td:nth-child(2) {
            width: 40px;
            min-width: 40px;
            max-width: 40px;
            padding-left: 3px !important;
            padding-right: 3px !important;
            text-align: center !important;
        }

        #grdSource tbody > tr > td:nth-child(2) > label {
            display: block;
            margin: 0;
            text-align: center;
        }

        #grdSource thead tr:last-child > th:nth-child(3),
        #grdSource tbody > tr > td:nth-child(3) {
            width: 55px;
            min-width: 55px;
            max-width: 55px;
            padding-left: 3px !important;
            padding-right: 3px !important;
            text-align: center !important;
        }

        #ddlLetter_Type {
            width: 48px !important;
            min-width: 48px !important;
            padding-left: 4px !important;
            padding-right: 2px !important;
        }

        #popFlightInfoInsert {
            width: auto !important;
            padding-right: 0 !important;
        }

        #popFlightInfoInsert .modal-dialog {
            width: calc(100% - 30px);
            max-width: 1080px;
            margin: 28px auto;
        }

        #popFlightInfoInsert .modal-header {
            padding: 14px 18px;
            border-bottom: 1px solid #d8e3ec;
        }

        #popFlightInfoInsert .modal-body {
            padding: 16px 18px 18px;
        }

        .flight-info-grid {
            display: grid;
            grid-template-columns: repeat(12, minmax(0, 1fr));
            gap: 12px 10px;
        }

        .flight-info-field {
            grid-column: span 2;
            min-width: 0;
        }

        .flight-info-field.span-1 { grid-column: span 1; }
        .flight-info-field.span-3 { grid-column: span 3; }
        .flight-info-field.span-4 { grid-column: span 4; }
        .flight-info-field.span-8 { grid-column: span 8; }

        .flight-info-field > label {
            display: block;
            margin: 0 0 4px;
            color: #35536b;
            font-size: 11px;
            font-weight: 700;
            line-height: 1.2;
            text-transform: uppercase;
        }

        .flight-info-field input,
        .flight-info-field select {
            width: 100% !important;
            min-width: 0 !important;
            height: 34px !important;
            box-sizing: border-box;
            border: 1px solid #b8ccdc;
            border-radius: 5px;
            padding: 5px 8px;
            color: #24445d;
            background: #fff;
        }

        .flight-info-field input:disabled {
            color: #77838d;
            background: #eef2f5;
        }

        .flight-info-actions {
            display: flex;
            align-items: center;
            justify-content: center;
            flex-wrap: wrap;
            gap: 8px;
            margin-top: 16px;
            padding-top: 14px;
            border-top: 1px solid #d8e3ec;
        }

        .flight-info-actions .btn {
            min-width: 94px;
            border-radius: 5px;
            text-transform: uppercase;
        }

        @media (max-width: 900px) {
            .flight-info-grid { grid-template-columns: repeat(6, minmax(0, 1fr)); }
            .flight-info-field.span-8 { grid-column: span 6; }
        }

        @media (max-width: 600px) {
            .flight-info-grid { grid-template-columns: repeat(2, minmax(0, 1fr)); }
            .flight-info-field,
            .flight-info-field.span-3,
            .flight-info-field.span-4,
            .flight-info-field.span-8 { grid-column: span 2; }
            .flight-info-field.span-1 { grid-column: span 1; }
        }

        .dayly-top-actions {
            display: flex;
            align-items: center;
            gap: 6px;
            padding: 2px 4px;
        }

        .dayly-top-actions > a {
            position: relative !important;
            display: inline-flex;
            width: 34px !important;
            height: 34px;
            align-items: center;
            justify-content: center;
            border: 1px solid #acd0e7;
            border-radius: 9px;
            background: #fff;
            color: #267fb7 !important;
            box-shadow: 0 3px 9px rgba(30, 89, 128, .12);
            text-decoration: none;
            transition: transform .18s ease, background .18s ease, color .18s ease, box-shadow .18s ease;
        }

        .dayly-top-actions > a:hover,
        .dayly-top-actions > a:focus {
            outline: 0;
            background: #287fbd;
            color: #fff !important;
            transform: translateY(-2px);
            box-shadow: 0 7px 15px rgba(31, 105, 155, .25);
        }

        .dayly-top-actions > a .glyphicon,
        .dayly-top-actions > a .fa { font-size: 15px; }

        .dayly-top-actions > a.dayly-search-action {
            background: linear-gradient(135deg, #2f92cc, #1d6fa8);
            color: #fff !important;
        }

        .dayly-top-actions .badge {
            position: absolute;
            top: -6px;
            right: -6px;
        }

        .dayly-top-actions #optionColor {
            position: relative;
            display: inline-flex;
            width: 34px;
            height: 34px;
            flex: 0 0 34px;
            align-items: center;
            justify-content: center;
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
            border: 0px solid #87badd !important;
            border-radius: 9px !important;
            background: linear-gradient(145deg, #ffffff 0%, #e7f4fc 100%) !important;
            color: #267fb7 !important;
            box-shadow: 0 3px 9px rgba(30, 89, 128, .14) !important;
            cursor: pointer;
            transition: transform .18s ease, background .18s ease, color .18s ease, box-shadow .18s ease;
        }

        .dayly-top-actions #ace-settings-btn:hover,
        .dayly-top-actions #ace-settings-btn:focus {
            outline: 0;
            background: linear-gradient(135deg, #359bd2 0%, #1d6fa8 100%) !important;
            color: #fff !important;
            transform: translateY(-2px);
            box-shadow: 0 7px 15px rgba(31, 105, 155, .26) !important;
        }

        .dayly-top-actions #ace-settings-btn .fa-cog {
            font-size: 16px;
            text-shadow: none;
            transition: transform .28s ease;
        }

        .dayly-top-actions #ace-settings-btn:hover .fa-cog,
        .dayly-top-actions #ace-settings-btn:focus .fa-cog {
            transform: rotate(60deg);
        }

        .dayly-top-actions #ace-settings-box {
            position: absolute !important;
            top: 42px !important;
            right: 0 !important;
            left: auto !important;
            max-height: min(520px, calc(100vh - 180px));
            overflow-y: auto;
            border: 0px solid #b9d2e5;
            border-radius: 10px;
            box-shadow: 0 12px 28px rgba(20, 66, 101, .22);
            z-index: 1500 !important;
        }

        #grdSource tbody > tr > td {
            height: auto !important;
            padding: 4px 7px !important;
            line-height: 1.2 !important;
            vertical-align: middle !important;
        }

        #grdSource tbody > tr { height: auto !important; }

        #grdSource {
            margin-bottom: 0;
            background: #fff;
            isolation: isolate;
        }

        #grdSource thead tr:last-child > th {
            background: #1e5f91 !important;
            color: #fff !important;
            border-color: #75a4c7 !important;
        }

        #grdSource thead tr.Spec > td {
            background: #dcecf8 !important;
            border-bottom: 2px solid #337ab7 !important;
        }

        #grdSource tbody tr > td:nth-child(-n+4) {
            border-right-color: #7da4c1 !important;
        }

        #grdSource tr > *:nth-child(4) {
            border-right: 4px solid #00a6d6 !important;
            box-shadow: 7px 0 9px -6px rgba(0, 80, 120, .9) !important;
        }

        #grdSource thead tr > *:nth-child(-n+4) {
            border-bottom-color: #00a6d6 !important;
        }

        /* Freeze bằng CSS sticky để hàng/cột đứng im ngay trong lúc cuộn. */
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

        #grdSource thead tr > *.dayly-frozen-column {
            z-index: 90 !important;
        }

        body .xdsoft_autocomplete_dropdown {
            z-index: 1040 !important;
            max-height: 230px;
            overflow-y: auto;
            border: 1px solid #76afd0 !important;
            background: #fff !important;
            box-shadow: 0 8px 18px rgba(26, 69, 104, .22);
        }

        #grdSource thead tr.Spec > td.dayly-autocomplete-open {
            position: sticky !important;
            z-index: 220 !important;
            overflow: visible !important;
        }

        #grdSource tbody tr > td:not(.dayly-frozen-column) {
            position: relative;
            z-index: 1;
        }

        #grdSource tr > *.dayly-freeze-edge {
            border-right: 4px solid #006f9f !important;
            box-shadow: 8px 0 10px -7px rgba(0, 44, 70, .95) !important;
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
		input, select, label, textarea {
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

        .cssTdBaySom {
            background-color: #f30f9b !important;
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

        /* Toolbar responsive: không để giờ/tổng số và bộ lọc chồng lên nhau. */
        .divHeader {
            display: flex;
            width: 97%;
            max-width: 100%;
            box-sizing: border-box;
            height: auto !important;
            min-height: 132px;
            flex-direction: column;
            gap: 8px;
            padding: 10px 12px 12px;
            border: 1px solid #bfd5e6 !important;
            border-radius: 10px;
            background: linear-gradient(135deg, #f7fbff 0%, #e7f3fc 100%);
            box-shadow: 0 5px 14px rgba(35, 83, 120, .10);
        }

        #topBar1 {
            position: static !important;
            display: flex;
            width: 100% !important;
            align-items: center;
            justify-content: center;
            flex-wrap: wrap;
            gap: 4px;
            text-align: center;
        }

        #topBar2 {
            position: static !important;
            display: flex;
            width: 100% !important;
            align-items: center;
            justify-content: center;
            gap: 8px;
            font-size: 16px;
        }

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

        #topBar3 {
            position: static !important;
            display: flex;
            width: 100%;
            min-height: 38px;
            align-items: center;
            justify-content: center;
            flex-wrap: wrap;
            gap: 8px;
        }

        @media (max-width: 1199px) {
            #table-container {
                height: clamp(380px, calc(100vh - 410px), 650px);
            }

            .divHeader {
                padding-right: 8px;
                padding-left: 8px;
            }

            #topBar1 {
                gap: 3px;
            }

            #topBar1 .btn {
                min-width: 32px;
                padding-right: 8px;
                padding-left: 8px;
            }

            #topBar3 {
                gap: 6px;
            }

            #topBar3 .checkbox {
                min-width: auto;
                margin-left: 0 !important;
            }
        }

        @media (max-width: 767px) {
            #table-container {
                height: max(360px, calc(100vh - 430px));
                border-radius: 8px;
            }

            .dayly-table-shell {
                width: calc(100% + 8px);
                max-width: calc(100% + 8px);
                margin-left: -8px;
                padding-right: 22px;
            }

            .dayly-scroll-controls {
                top: 50%;
                right: -7px;
                bottom: auto;
                gap: 4px;
                transform: translateY(-50%);
            }

            .dayly-scroll-controls .dayly-scroll-right {
                width: 30px;
                height: 36px;
                border-radius: 7px;
            }

            .divHeader {
                min-height: 0;
            }

            #topBar2,
            #topBar3 {
                justify-content: flex-start;
            }

            #topBar3 select,
            #divSelectDate select {
                min-width: 78px;
            }

            .dayly-top-actions {
                flex-wrap: wrap;
            }
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
            right: auto !important;
            bottom: auto !important;
            left: auto !important;
            top: auto !important;
            width: auto !important;
            margin: 0 !important;
        }

        #topBar3 .checkbox {
            display: flex;
            min-width: 132px;
            align-items: center;
            margin-left: 10px !important;
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
            background-color: #fff;
            color: #164d76;
        }

        /* Hàng tìm kiếm riêng biệt, dễ nhận biết với header dữ liệu. */
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
            box-shadow: inset 0 1px 2px rgba(31, 78, 113, .08);
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
            left: -800px;
            top: -10px;
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
    <div class="row">
        <div class="divHeader">
            <div id="infoTotal">
                <script type="text/javascript">
               
                </script>
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
                    <label style="padding-left:0px;">
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
                <select id="ddlExport" onchange="ddlExport_Change();">
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
                    <a href="#" onclick="window.open('<%= Page.ResolveUrl("~/SendMessage/SendMessageFlight.aspx") + "?Menu_Id=" + Request.Params["Menu_ID"] %>','_blank','toolbar=yes,scrollbars=yes,resizable=yes').resizeTo(window.screen.availWidth, window.screen.availHeight).moveTo(0,0)"
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
                    </a>

                    <!--<a href="#" data-toggle="tooltip" title="Update status of message" onclick="window.open('<%= Page.ResolveUrl("~/Day_Flights/DaylyFlightCancel.aspx") + "?Menu_Id=" + Request.Params["Menu_ID"] %>','_blank','toolbar=yes,scrollbars=yes,resizable=yes').resizeTo(window.screen.availWidth, window.screen.availHeight).moveTo(0,0)">
                        <i class="glyphicon glyphicon-comment"></i><span id="lbltopbarNo_TotalFlight"
                            class="badge badge-success"></span>
                    </a>-->
                    <a href="#" data-toggle="tooltip" title="Export Flight Finished" onclick="LoadDataGrid_Finished()">
                        <i class="glyphicon glyphicon-cloud-download"></i><span id="lbltopbarNo_TotalFinished"
                            class="badge badge-success"></span>
                    </a>
                    <a href="#" data-toggle="tooltip" title="Export Exel" onclick="btnExport_Onclick()">
                        <i class="glyphicon glyphicon-file"></i>
                    </a>
                    <a href="#" data-toggle="tooltip" title="Export Exel O/F" onclick="btnExportOF_Onclick()">
                        <i class="glyphicon glyphicon-save-file"></i>
                    </a>
                    <a href="#" data-toggle="tooltip" title="Export Exel Sum O/F" onclick="viewPopupInfoExtensionSelect()">
                        <i class="glyphicon glyphicon-save-file"></i>
                    </a>
                    <div id="optionColor">
                        <div class="ace-settings-container" id="ace-settings-container">
                            <div class="btn ace-settings-btn" onclick="$('#ace-settings-box').toggleClass('open');"
                                id="ace-settings-btn" role="button" tabindex="0" data-toggle="tooltip"
                                title="Tùy chỉnh màu sắc và cỡ chữ">
                                <i class="ace-icon fa fa-cog" aria-hidden="true"></i>
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
                </div>
            </div>
        </div>
    </div>


    <div class="dayly-table-shell">
        <div class="row" id="table-container">
            <div class="table-responsive">

            <table id="grdSource" class="table table-bordered">
                <thead>
                    <tr class="Spec" data-isinsert="true">
                        <td colspan="3" class="headSpec dayly-action-cell">
                            <div class="dayly-table-actions">
                                <a id="btnInput" onclick="viewPopupInfoExtensionInsert(this);" data-id="0" title="Thêm mới"><i class="glyphicon glyphicon-plus"></i></a>
                                <a id="btnSave" onclick="UpdateInfosFlight();" title="Lưu tất cả"><i class="glyphicon glyphicon-save"></i></a>
                                <a id="btnClearInput" onclick="btnClearInput_Onclick();" title="Xóa điều kiện tìm kiếm"><i class="glyphicon glyphicon-trash"></i></a>
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
                            <input id="txtETD" type="text" class="sControl" maxlength="6" inputmode="numeric" style="width: 58px;" title="Tìm theo ETD" />
                        </td>
                        <td class="headSpec">
                            <input id="txtPtd" type="text" class="sControl" maxlength="6" inputmode="numeric" style="width: 58px;" title="Tìm theo EOBT" /></td>
                        <td class="headSpec">
                            <input id="txtETA" type="text" class="sControl" maxlength="6" inputmode="numeric" style="width: 58px;" title="Tìm theo ETA" />
                        </td>

                        <td class="headSpec">
                            <input id="txtAtd" type="text" class="sControl" maxlength="6" inputmode="numeric" style="width: 58px;" title="Tìm theo ATD" /></td>
                        <td class="headSpec">
                            <input id="txtATA" type="text" class="sControl" maxlength="6" inputmode="numeric" style="width: 58px;" title="Tìm theo ATA" /></td>
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
                        <th style="width: 38px"></th>
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
                        <table id="tblFlightActionHistory" class="table table-bordered">
                            <caption class="text-left">Flight Action History</caption>
                            <thead>
                                <tr>
                                    <th>Action</th>
                                    <th>CallSign</th>
                                    <th>Flight date</th>
                                    <th>User</th>
                                    <th>Action date</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td colspan="5" class="text-muted">
                                        Chưa có lịch sử thao tác cho chuyến bay này.
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
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


    <div id="popFlightInfoView" class="modal fade" style="width: 1300px; padding-right: 20px;" role="dialog" tabindex="-1"
        aria-labelledby="myModalLabel"
        data-backdrop="false"
        aria-hidden="true">
        <div class="modal-dialog wid_90">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close"
                        data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">View Flight Info <span id="idBn1r" style="color: red;"></span></h4>
                </div>
                <div class="modal-body" id="txtContentExtensionInfoFlightSelect">
                    <fieldset>
                        <div id="txtOFcontent"></div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>

    <div id="popFlightInfoInsert" class="modal fade" role="dialog" tabindex="-1"
        aria-labelledby="flightInfoModalTitle"
        data-backdrop="false"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close"
                        data-dismiss="modal">
                        ×</button>
                    <h4 id="flightInfoModalTitle" class="blue bigger">Add / Update Flight Info <span id="idBnr" style="color: red;"></span></h4>
                </div>
                <div class="modal-body" id="txtContentExtensionInfoFlightInsert">
                    <div class="flight-info-grid">
                        <div class="flight-info-field span-1">
                            <label for="ddlSelectPop">LETTER</label>
                            <select id="ddlSelectPop">
                            <option selected value="--">--</option>
                            <option value="FPL">FPL</option>
                            <option value="DEP">DEP</option>
                            <option value="DLA">DLA</option>
                            <option value="CHG">CHG</option>
                            <option value="CNL">CNL</option>
                            <option value="ARR">ARR</option>
                            <option value="NULL">NULL</option>
                            </select>
                        </div>
                        <div class="flight-info-field span-3">
                            <label for="txt_popPERMNBR">PERM</label>
                            <input id="txt_popPERMNBR" type="text" />
                        </div>
                        <div class="flight-info-field">
                            <label for="txt_popREGIS">REGIS</label>
                            <input id="txt_popREGIS" type="text" />
                        </div>
                        <div class="flight-info-field">
                            <label for="txt_popCAllSIGN">CALLSIGN</label>
                            <input id="txt_popCAllSIGN" type="text" />
                        </div>
                        <div class="flight-info-field">
                            <label for="txt_popFROM">FROM</label>
                            <input id="txt_popFROM" type="text" data-autocomplete="AERO" />
                        </div>
                        <div class="flight-info-field">
                            <label for="txt_popTO">TO</label>
                            <input id="txt_popTO" type="text" data-autocomplete="AERO" />
                        </div>

                        <div class="flight-info-field span-1">
                            <label for="txt_popETD">ETD</label>
                            <input id="txt_popETD" type="text" data-number="true" maxlength="6" />
                        </div>
                        <div class="flight-info-field span-1">
                            <label for="txt_popEOBT">EOBT</label>
                            <input id="txt_popEOBT" type="text" data-number="true" maxlength="6" />
                        </div>
                        <div class="flight-info-field span-1">
                            <label for="txt_popETA">ETA</label>
                            <input id="txt_popETA" type="text" maxlength="7" />
                        </div>
                        <div class="flight-info-field span-1">
                            <label for="txt_popATD">ATD</label>
                            <input id="txt_popATD" type="text" maxlength="6" />
                        </div>
                        <div class="flight-info-field span-1">
                            <label for="txt_popATA">ATA</label>
                            <input id="txt_popATA" type="text" maxlength="6" />
                        </div>
                        <div class="flight-info-field span-4">
                            <label for="txt_popROUTE">ROUTE</label>
                            <input id="txt_popROUTE" type="text" />
                        </div>
                        <div class="flight-info-field span-3">
                            <label for="txt_popROUTE_FPL">ROUTE_FPL</label>
                            <input id="txt_popROUTE_FPL" type="text" />
                        </div>

                        <div class="flight-info-field">
                            <label for="txt_popCRAFT_T">CRAFT_T</label>
                            <input id="txt_popCRAFT_T" type="text" data-autocomplete="CRAFT" />
                        </div>
                        <div class="flight-info-field">
                            <label for="txt_popCRAFT_P">CRAFT_P</label>
                            <input id="txt_popCRAFT_P" type="text" data-autocomplete="CRAFT" />
                        </div>
                        <div class="flight-info-field">
                            <label for="txt_popPERMTYPE">P_TYPE</label>
                            <input id="txt_popPERMTYPE" type="text" data-autocomplete="PERMTYPE" />
                        </div>
                        <div class="flight-info-field">
                            <label for="txt_popFLIGHTTYPE">F_TYPE</label>
                            <input id="txt_popFLIGHTTYPE" type="text" data-autocomplete="FLIGHTTYPE" />
                        </div>
                        <div class="flight-info-field">
                            <label for="txt_popOPER">OPER</label>
                            <input id="txt_popOPER" type="text" data-autocomplete="OPER" />
                        </div>
                        <div class="flight-info-field">
                            <label for="txt_popPUR">PURPOSE</label>
                            <input id="txt_popPUR" type="text" data-autocomplete="PURPOSE" />
                        </div>

                        <div class="flight-info-field span-1">
                            <label for="txt_popVALID">VALID</label>
                            <input id="txt_popVALID" type="text" />
                        </div>
                        <div class="flight-info-field span-3">
                            <label for="txt_popPERMDATE">P_DATE</label>
                            <input id="txt_popPERMDATE" type="text" />
                        </div>
                        <div class="flight-info-field span-8">
                            <label for="txt_popREMARK">REMARK</label>
                            <input id="txt_popREMARK" type="text" />
                        </div>
                    </div>

                    <div class="flight-info-actions">
                        <input id="txt_FID" type="text" class="wid_180px" style="display: none;" />
                        <button id="btnSaveInfoChange" type="button" onclick="btnInsert();" class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-save"></i>
                            Add New
                        </button>
                        <button id="btnUpdateInfoChange" type="button" onclick="btnUpdate();" class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-save"></i>
                            Update

                        </button>
                        <button id="btnCancelE" type="button"
                            data-dismiss="modal" class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-times"></i>
                            Cancel
                        </button>
                        <button id="btnFinishFlight" type="button" onclick="fnFinishFlight();" class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-arrow-right"></i>
                            Move Finished
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
    <script src="../Scripts/CustumStaticdata.js"></script>
    <script>
        var d = new Date();
        $('#divSelectDate').html("<select id='ddlDateFlight' onchange='btnSearch_Onclick();'>"
            
			+"<option value='-11'>"
                    + dateFormat(new Date().setDate(new Date().getDate() - 11), 'dd/mm/yyyy')
        + "</option>"
		
			 +"<option value='-10'>"
                    + dateFormat(new Date().setDate(new Date().getDate() - 10), 'dd/mm/yyyy')
        + "</option>"
		
         +"<option value='-9'>"
                    + dateFormat(new Date().setDate(new Date().getDate() - 9), 'dd/mm/yyyy')
        + "</option>"
         +"<option value='-8'>"
                    + dateFormat(new Date().setDate(new Date().getDate() - 8), 'dd/mm/yyyy')
        + "</option>"
         +"<option value='-7'>"
                    + dateFormat(new Date().setDate(new Date().getDate() - 7), 'dd/mm/yyyy')
        + "</option>"
            +"<option value='-6'>"
                    + dateFormat(new Date().setDate(new Date().getDate() - 6), 'dd/mm/yyyy')
        + "</option>"
             +"<option value='-5'>"
                    + dateFormat(new Date().setDate(new Date().getDate() - 5), 'dd/mm/yyyy')
        + "</option>"
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
        var txtETA = document.getElementById('txtETA');
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
            $('#txtETD').val('');
            $('#txtPtd').val('');
            $('#txtETA').val('');
            $('#txtAtd').val('');
            $('#txtATA').val('');
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

                        
            var selectedTimeColumn = isRefresh ? '' : $('#ddlSelectViewColum').val();
            var etdFilter = $.trim($('#txtETD').val());
            var eobtFilter = $.trim($('#txtPtd').val());
            var etaFilter = $.trim($('#txtETA').val());
            var atdFilter = $.trim($('#txtAtd').val());
            var ataFilter = $.trim($('#txtATA').val());

            var _obj = {
                //STATUS: document.getElementById('ddlStatus').value,
                PERMNBR: $('#txtPERMNBR').val(),
                FLIGHTNBR: $('#txtFLIGHTNBR').val(),
                FROM_AIRP: $('#ddlFROM_AIRP').val(),
                TO_AIRP: $('#ddlTO_AIRP').val(),
                ETA: etaFilter || (selectedTimeColumn == 'ETA' ? 'ETA' : ''),
                ETD: etdFilter || (selectedTimeColumn == 'ETD' ? 'ETD' : ''),
                ATD: atdFilter || (selectedTimeColumn == 'ATD' ? 'ATD' : ''),
                ATA: ataFilter || (selectedTimeColumn == 'ATA' ? 'ATA' : ''),
                PTD: eobtFilter,
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
                VIA: $('#txtROUTE').val(),
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

                $("#totql").html("Total : <b>" + (array.length > 1 && array[1] ? array[1] : 0) + "</b>");

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
          console.log(getValueSearchDaylyFlight());
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
             GetArgWithPostBack(JSON.stringify(getValueSearchDaylyFlight()) + phanCachArg
    + 'btnExport_Onclick', 'btnExport_Onclick');
        }
       
        
        function btnExportSumOF_Onclick() {
            var _date = $('#ddlDateFlight option:selected').text() == '' ? new Date().format('dd-MM-yyyy') : $('#ddlDateFlight option:selected').text().replace(/\//gi, '-').replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$1/$2/$3').replace('/','-').replace('/','-');
            _date = '06-09-2021';
			var _obj = {P_USER:'<%= _user.UserName%>',P_DATE: _date};
			
			var _urlPath = "";
			_urlPath = "api/ApiExtension/ExcuteTable?packageName=A_TEST_SEARCH&storeName=GetExportMOVEFINISH_SUMOF"
            
			//console.log(_obj);
			
            //console.log(_urlPath);

			var $request = $.ajax({
                method: "PUT",
                url: urlApi + _urlPath,
                data: JSON.stringify(_obj),
                beforeSend: function () {
                },
                complete: function () {
                    unLoadingData('loaddingData');
                },
            }).always(function (data) {
				//console.log(data);
				if (data.ListValue == null) {
                    unLoadingData('loaddingData');
                    return;
                }
				
				
				var strAppend = RenderTableKhExportSum(data);
				//alert(strAppend); + "<th>No</th>"
                var strHtml = "<table id='tblSourceExport' class='table table-bordered'>";
                strHtml += "<thead style='color: red'>"
                        + "<tr>"
                        
                        + "<th>FlightDate</th>"
                        + "<th onclick='btnExportOF_FPL_Onclick()'>FPL - &nbsp;<i class='glyphicon glyphicon-save-file'></i></th>"
                        + "<th onclick='btnExportOF_DEP_Onclick()'>DEP - &nbsp;<i class='glyphicon glyphicon-save-file'></i></th>"
                        + "<th onclick='btnExportOF_ARR_Onclick()'>ARR - &nbsp;<i class='glyphicon glyphicon-save-file'></i></th>"
                       
                        + "</tr>"
                        + "</thead>"
                        + strAppend
                        + "<tbody>"
                        + "</tbody>"
                        + "</table>";

				
                //exportExelMOVEFINISH(strHtml);

                $('#txtOFcontent').html(strHtml);

            });
			
			
			
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }
        function btnExportOF_DEP_Onclick() {
            var _date = $('#ddlDateFlight option:selected').text() == '' ? new Date().format('dd-MM-yyyy') : $('#ddlDateFlight option:selected').text().replace(/\//gi, '-').replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$1/$2/$3').replace('/','-').replace('/','-');
            _date = '06-09-2021';
			var _obj = {P_USER:'<%= _user.UserName%>',P_DATE: _date};
			
			var _urlPath = "";
			_urlPath = "api/ApiExtension/ExcuteTable?packageName=A_TEST_SEARCH&storeName=GetExportMOVEFINISH_DEP_OF"
            
			//console.log(_obj);
			
		   
			var $request = $.ajax({
                method: "PUT",
                url: urlApi + _urlPath,
                data: JSON.stringify(_obj),
                beforeSend: function () {
                },
                complete: function () {
                    unLoadingData('loaddingData');
                },
            }).always(function (data) {
				//console.log(data);
				if (data.ListValue == null) {
                    unLoadingData('loaddingData');
                    return;
                }
				
				
                var strAppend = RenderTableKhExport(data);
				//alert(strAppend); + "<th>No</th>"
                var strHtml = "<table id='tblSourceExport' class='table table-bordered'>";
                strHtml += "<thead style='color: red'>"
                        + "<tr>"
                        
                        + "<th>OPER</th>"
                        + "<th>CALLSIGN</th>"
                        + "<th>REGIS</th>"
                        + "<th>R_CRAFT</th>"
                        + "<th>F_CRAFT</th>"
                        + "<th>PURPOSE</th>"
                        + "<th>P_TYPE</th>"
                        + "<th>FROM</th>"
                        + "<th>TO</th>"
                        + "<th>FLIGHTDATE</th>"
                        + "<th>ATD</th>"
                        + "<th>ATA</th>"
                        + "<th>VIA</th>"
                        + "<th>FPL_VIA</th>"
                        + "<th>REMARK</th>"
                        + "<th>LASTUSER</th>"
                        + "<th>ETD</th>"
                        + "<th>ETA</th>"
                        + "<th>EOBT</th>"
                        + "</tr>"
                        + "</thead>"
                        + strAppend
                        + "<tbody>"
                        + "</tbody>"
                        + "</table>";

				
                exportExelMOVEFINISH(strHtml);


            });
			
			
			
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }
        function btnExportOF_ARR_Onclick() {
            var _date = $('#ddlDateFlight option:selected').text() == '' ? new Date().format('dd-MM-yyyy') : $('#ddlDateFlight option:selected').text().replace(/\//gi, '-').replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$1/$2/$3').replace('/','-').replace('/','-');
            _date = '06-09-2021';
			var _obj = {P_USER:'<%= _user.UserName%>',P_DATE: _date};
			
			var _urlPath = "";
			_urlPath = "api/ApiExtension/ExcuteTable?packageName=A_TEST_SEARCH&storeName=GetExportMOVEFINISH_ARR_OF"
            
			//console.log(_obj);
			
		   
			var $request = $.ajax({
                method: "PUT",
                url: urlApi + _urlPath,
                data: JSON.stringify(_obj),
                beforeSend: function () {
                },
                complete: function () {
                    unLoadingData('loaddingData');
                },
            }).always(function (data) {
				//console.log(data);
				if (data.ListValue == null) {
                    unLoadingData('loaddingData');
                    return;
                }
				
				
                var strAppend = RenderTableKhExport(data);
				//alert(strAppend); + "<th>No</th>"
                var strHtml = "<table id='tblSourceExport' class='table table-bordered'>";
                strHtml += "<thead style='color: red'>"
                        + "<tr>"
                        
                        + "<th>OPER</th>"
                        + "<th>CALLSIGN</th>"
                        + "<th>REGIS</th>"
                        + "<th>R_CRAFT</th>"
                        + "<th>F_CRAFT</th>"
                        + "<th>PURPOSE</th>"
                        + "<th>P_TYPE</th>"
                        + "<th>FROM</th>"
                        + "<th>TO</th>"
                        + "<th>FLIGHTDATE</th>"
                        + "<th>ATD</th>"
                        + "<th>ATA</th>"
                        + "<th>VIA</th>"
                        + "<th>FPL_VIA</th>"
                        + "<th>REMARK</th>"
                        + "<th>LASTUSER</th>"
                        + "<th>ETD</th>"
                        + "<th>ETA</th>"
                        + "<th>EOBT</th>"
                        + "</tr>"
                        + "</thead>"
                        + strAppend
                        + "<tbody>"
                        + "</tbody>"
                        + "</table>";

				
                exportExelMOVEFINISH(strHtml);


            });
			
			
			
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }

        function btnExportOF_FPL_Onclick() {
            var _date = $('#ddlDateFlight option:selected').text() == '' ? new Date().format('dd-MM-yyyy') : $('#ddlDateFlight option:selected').text().replace(/\//gi, '-').replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$1/$2/$3').replace('/','-').replace('/','-');
            _date = '06-09-2021';
			var _obj = {P_USER:'<%= _user.UserName%>',P_DATE: _date};
			
			var _urlPath = "";
			_urlPath = "api/ApiExtension/ExcuteTable?packageName=A_TEST_SEARCH&storeName=GetExportMOVEFINISH_FPL_OF"
            
			//console.log(_obj);
			
		   
			var $request = $.ajax({
                method: "PUT",
                url: urlApi + _urlPath,
                data: JSON.stringify(_obj),
                beforeSend: function () {
                },
                complete: function () {
                    unLoadingData('loaddingData');
                },
            }).always(function (data) {
				//console.log(data);
				if (data.ListValue == null) {
                    unLoadingData('loaddingData');
                    return;
                }
				
				
                var strAppend = RenderTableKhExport(data);
				//alert(strAppend); + "<th>No</th>"
                var strHtml = "<table id='tblSourceExport' class='table table-bordered'>";
                strHtml += "<thead style='color: red'>"
                        + "<tr>"
                        
                        + "<th>OPER</th>"
                        + "<th>CALLSIGN</th>"
                        + "<th>REGIS</th>"
                        + "<th>R_CRAFT</th>"
                        + "<th>F_CRAFT</th>"
                        + "<th>PURPOSE</th>"
                        + "<th>P_TYPE</th>"
                        + "<th>FROM</th>"
                        + "<th>TO</th>"
                        + "<th>FLIGHTDATE</th>"
                        + "<th>ATD</th>"
                        + "<th>ATA</th>"
                        + "<th>VIA</th>"
                        + "<th>FPL_VIA</th>"
                        + "<th>REMARK</th>"
                        + "<th>LASTUSER</th>"
                        + "<th>ETD</th>"
                        + "<th>ETA</th>"
                        + "<th>EOBT</th>"
                        + "</tr>"
                        + "</thead>"
                        + strAppend
                        + "<tbody>"
                        + "</tbody>"
                        + "</table>";

				
                exportExelMOVEFINISH(strHtml);


            });
			
			
			
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }
        function btnExportOF_Onclick() {
            var _date = $('#ddlDateFlight option:selected').text() == '' ? new Date().format('dd-MM-yyyy') : $('#ddlDateFlight option:selected').text().replace(/\//gi, '-').replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$1/$2/$3').replace('/','-').replace('/','-');
            _date = '06-09-2021';
			var _obj = {P_USER:'<%= _user.UserName%>',P_DATE: _date};
			
			var _urlPath = "";
			_urlPath = "api/ApiExtension/ExcuteTable?packageName=A_TEST_SEARCH&storeName=GetExportMOVEFINISH_OF"
            
			//console.log(_obj);
			
		   
			var $request = $.ajax({
                method: "PUT",
                url: urlApi + _urlPath,
                data: JSON.stringify(_obj),

                beforeSend: function () {
                },
                complete: function () {
                    unLoadingData('loaddingData');
                },
            }).always(function (data) {
				//console.log(data);
				if (data.ListValue == null) {
                    unLoadingData('loaddingData');
                    return;
                }
				
				
                var strAppend = RenderTableKhExport(data);
				//alert(strAppend); + "<th>No</th>"
                var strHtml = "<table id='tblSourceExport' class='table table-bordered'>";
                strHtml += "<thead style='color: red'>"
                        + "<tr>"
                        
                        + "<th>OPER</th>"
                        + "<th>CALLSIGN</th>"
                        + "<th>REGIS</th>"
                        + "<th>R_CRAFT</th>"
                        + "<th>F_CRAFT</th>"
                        + "<th>PURPOSE</th>"
                        + "<th>P_TYPE</th>"
                        + "<th>FROM</th>"
                        + "<th>TO</th>"
                        + "<th>FLIGHTDATE</th>"
                        + "<th>ATD</th>"
                        + "<th>ATA</th>"
                        + "<th>VIA</th>"
                        + "<th>FPL_VIA</th>"
                        + "<th>REMARK</th>"
                        + "<th>LASTUSER</th>"
                        + "<th>ETD</th>"
                        + "<th>ETA</th>"
                        + "<th>EOBT</th>"
                        + "</tr>"
                        + "</thead>"
                        + strAppend
                        + "<tbody>"
                        + "</tbody>"
                        + "</table>";

				
                exportExelMOVEFINISH(strHtml);


            });
			
			
			
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
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
			
		//var _dateflight = $('#ddlDateFlight option:selected').text() == '' ? new Date().format('dd-MM-yyyy') : $('#ddlDateFlight option:selected').text().replace(/\//gi, '-').replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$1/$2/$3').replace('/','-').replace('/','-');
            
	    var _dateflight = $('#ddlDateFlight option:selected').text() == '' ? new Date().format('dd-MM-yyyy') : $('#ddlDateFlight option:selected').text().replace(/\//gi, '-').replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$1/$2/$3').replace('/','-').replace('/','-');
           

            var $lis = $('tr[data-isUpdate="true"]');
	   
            if ($lis.length == 0) { alert('No update.'); return; };
            
 	    //var idd =$lis.attr('data-id');
	    var idd =0;
            var c = 0;
            $.each($lis, function (a, b) {
                idd  = 	$(b).data("id");
		//console.log($(b).data("id"));
	
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
                _obj['FLIGHTDATE'] = _dateflight;
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
                _obj['ROUTE_TT'] = idd;
                _obj['OPER_ID'] = '';
                _obj['ATD'] = _atdorg;
                _obj['ETA'] = _eta;
                _obj['ETD'] = _etd;
                _obj['PERMNBR'] = $($(b).find('[id^="txtPERMNBR"]')[0]).val();
                _obj['FROM_AIRP'] = $($(b).find('[id^="txtFROM_AIRP"]')[0]).val();
                _obj['STATUS'] = '0';
                _obj['ISACCESS'] = 1;
		_obj['NBR'] = _dateflight;
                _obj['EOBT'] = _eobt;
                if(_atd==''){_obj['ATDDATE'] = '';}else{_obj['ATDDATE'] = _atdorg;}
                if(_ata==''){_obj['ATADATE'] = '';}else{_obj['ATADATE'] = _ataorg;}
                if(_eobt==''){_obj['EOBTDATE'] = '';}else{_obj['EOBTDATE'] = _eobtorg;}      
                   
         	console.log(_obj);
                
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
		
		function fnFinishFlight()
		{
			//var _F = $('#txt_FID').val(ax.attr('data-id'));
            //var url = urlApi + '/api/DayFlights/GetPerm_GoingOnBy?ID=' + ax.attr('data-id');
			var ax = $('#txt_FID').val();
			//alert(ax);
			
			
			
			
			var cf = confirm('Do you want move finished ?');
             if (cf) {                 
                 var url = urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=A_TEST_SEARCH&storeName=MOVEFINISH_GOINGON";
                 
                $.ajax({
                    async: false,
                    method: "PUT",
                    url: url,
                    data: JSON.stringify({ P_ID: ax, P_USER: '<%= _user.UserName%>'}),
                }).always(function (data) {
                if (data.ListValue == null || data.ListValue == -1) {
                    alert('Update error!');
                } else alert('Update success!');
				})
           }
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
				console.log(_etdorg);
                
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
                var scrollElement = this;
                var distanceToBottom = scrollElement.scrollHeight
                    - scrollElement.scrollTop
                    - scrollElement.clientHeight;

                if (scrollElement.scrollHeight <= scrollElement.clientHeight
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
                preLoadData();
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
            var flightId = ax.attr('data-flight-id') || ax.attr('id') || ax.attr('data-id');
            $('#grdSource tr').removeClass('active');
            ax.addClass('active');
            console.log(ax.attr('data-CallSign'));
            GetArgWithPostBack(ax.attr('data-id') + phanCach + ax.attr('data-timeM')
    + phanCach + ax.attr('data-CallSign') + phanCach + flightId
    + phanCachArg + 'viewPopupInfoExtension',
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
            $('#tblPermission').html(
                '<tbody><tr><td class="text-muted">Đang tải Permission...</td></tr></tbody>'
            );
            $('#tblFlightActionHistory tbody').html(
                '<tr><td colspan="5" class="text-muted">' +
                'Đang tải lịch sử thao tác...</td></tr>'
            );
        }

        function renderFlightActionHistory(rowsHtml) {
            $('#tblFlightActionHistory tbody').html(rowsHtml ||
                '<tr><td colspan="5" class="text-muted">' +
                'Chưa có lịch sử thao tác cho chuyến bay này.</td></tr>');
        }

        function flightPermissionEncode(value) {
            return $('<div/>').text(value == null ? '' : value).html();
        }

        function flightPermissionValue(row, name, alternateName) {
            if (!row) return '';
            if (row[name] != null) return row[name];
            return alternateName && row[alternateName] != null
                ? row[alternateName]
                : '';
        }

        function flightPermissionDate(value) {
            if (!value) return '';
            var date = new Date(value);
            return isNaN(date.getTime()) ? value : date.format('dd-mm-yyyy');
        }

        function renderFlightPermission(rows, linkFiles, errorMessage) {
            rows = $.isArray(rows) ? rows : [];
            linkFiles = $.isArray(linkFiles) ? linkFiles : [];

            $('#tblPermission').empty();
            $('#lblLinkFile').empty();

            if (errorMessage) {
                console.error('[Flight extension][Permission] ' + errorMessage);
                $('#tblPermission').html(
                    '<tbody><tr><td class="text-danger">' +
                    flightPermissionEncode(errorMessage) +
                    '</td></tr></tbody>'
                );
                return;
            }

            if (!rows.length) {
                $('#tblPermission').html(
                    '<tbody><tr><td class="text-muted">' +
                    'Không có thông tin Permission cho chuyến bay này.' +
                    '</td></tr></tbody>'
                );
                return;
            }

            var first = rows[0];
            $('#txtAuthorPerm').val(flightPermissionValue(first, 'AUTHOR_ID'));
            $('#txtOperPerm').val(flightPermissionValue(first, 'OPER_ID'));
            $('#txtFlightNbrPerm').val(flightPermissionValue(first, 'PERMNBR'));
            $('#txtPermTypePerm').val(flightPermissionValue(first, 'PERMTYPE'));
            $('#txtVersionPerm').val(flightPermissionValue(first, 'VERSION'));
            $('#txtSeasonPerm').val(flightPermissionValue(first, 'SEASON'));
            $('#txtDatePerm').val(
                flightPermissionDate(flightPermissionValue(first, 'PERMDATE'))
            );
            $('#txtHoursPerm').val(flightPermissionValue(first, 'VALIDHOURS'));

            var firstType = String(
                flightPermissionValue(first, 'FLIGHT_TYPE', 'FLIGHTTYPE')
            ).toUpperCase();
            var isNoPermissionType = firstType === 'NO';
            var html = '<thead><tr>' +
                '<th>Call sign</th><th>Registration</th><th>From</th>' +
                '<th>To</th><th>Etd</th><th>Eta</th><th>Day</th>' +
                '<th>Craft</th>';

            if (!isNoPermissionType) {
                html += '<th>Begin date</th><th>End date</th>';
            }
            html += '<th>Purpose</th><th>Via</th><th>Remark</th>' +
                '</tr></thead><tbody>';

            $.each(rows, function (_, row) {
                var rowType = String(
                    flightPermissionValue(row, 'FLIGHT_TYPE', 'FLIGHTTYPE')
                ).toUpperCase();
                var isSc = rowType === 'SC' || (!rowType && !isNoPermissionType);
                var day = flightPermissionValue(row, 'DAYFLIGHT');

                if (isSc) {
                    day = '';
                    for (var dayIndex = 1; dayIndex <= 7; dayIndex++) {
                        day += flightPermissionValue(row, 'DAY' + dayIndex);
                    }
                    day = day.replace(/0/g, '.');
                }

                html += '<tr>' +
                    '<td>' + flightPermissionEncode(
                        flightPermissionValue(row, 'FLIGHTNBR')
                    ) + '</td>' +
                    '<td>' + flightPermissionEncode(
                        flightPermissionValue(row, 'REGISTRATION')
                    ) + '</td>' +
                    '<td>' + flightPermissionEncode(
                        flightPermissionValue(row, 'FROM_AIRP')
                    ) + '</td>' +
                    '<td>' + flightPermissionEncode(
                        flightPermissionValue(row, 'TO_AIRP')
                    ) + '</td>' +
                    '<td>' + flightPermissionEncode(
                        flightPermissionValue(row, 'ETD')
                    ) + '</td>' +
                    '<td>' + flightPermissionEncode(
                        flightPermissionValue(row, 'ETA')
                    ) + '</td>' +
                    '<td>' + flightPermissionEncode(day) + '</td>' +
                    '<td>' + flightPermissionEncode(
                        flightPermissionValue(row, 'MA')
                    ) + '</td>';

                if (!isNoPermissionType) {
                    html += '<td style="white-space:nowrap;">' +
                        flightPermissionEncode(
                            flightPermissionDate(
                                flightPermissionValue(row, 'BEGINDATE')
                            )
                        ) + '</td>' +
                        '<td style="white-space:nowrap;">' +
                        flightPermissionEncode(
                            flightPermissionDate(
                                flightPermissionValue(row, 'ENDDATE')
                            )
                        ) + '</td>';
                }

                html += '<td>' + flightPermissionEncode(
                    flightPermissionValue(row, 'PURPOSE_ID')
                ) + '</td>' +
                    '<td style="word-break:break-all;">' +
                    flightPermissionEncode(flightPermissionValue(row, 'VIA')) +
                    '</td>' +
                    '<td style="word-break:break-all;">' +
                    flightPermissionEncode(flightPermissionValue(row, 'REMARK')) +
                    '</td></tr>';
            });

            $('#tblPermission').html(html + '</tbody>');

            $.each(linkFiles, function (_, file) {
                var fileName = flightPermissionValue(file, 'FILENAME');
                var fileUrl =
                    flightPermissionValue(file, 'URLPATH') + fileName;
                $('<a/>', {
                    href: fileUrl,
                    text: fileName,
                    target: '_blank',
                    rel: 'noopener noreferrer'
                }).appendTo('#lblLinkFile');
                $('#lblLinkFile').append('<br/>');
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
        function showFinishedExportLoading() {
            if (window.ATFMLoading && typeof window.ATFMLoading.show === 'function') {
                window.ATFMLoading.show('Đang xử lý và xuất chuyến bay hoàn thành...');
            }
        }

        function hideFinishedExportLoading() {
            if (window.ATFMLoading && typeof window.ATFMLoading.hideAfterRender === 'function') {
                window.ATFMLoading.hideAfterRender();
            }
        }

        function rejectFinishedExport(stage, response) {
            var deferred = $.Deferred();
            deferred.reject({ stage: stage, response: response });
            return deferred.promise();
        }

        function LoadDataGrid_Finished() {
            var cf = confirm('Do you want export finished ?');
            if (!cf) return;

            showFinishedExportLoading();

            // Nhường một nhịp render để overlay hiển thị trước khi bắt đầu xử lý.
            window.setTimeout(function () {
                var url = urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=MAKE_FINISHED&storeName=make_finished_flights_news";

                $.ajax({
                    method: "PUT",
                    url: url,
                    data: JSON.stringify({ P_STRING: '<%= _user.UserName%>' })
                }).then(function (data) {
                    if (!data || data.ListValue == null || data.ListValue == -1) {
                        return rejectFinishedExport('MAKE_FINISHED', data);
                    }

                    return LoadDataGrid_ExportFinish();
                }).done(function () {
                    // Ba file được tạo cách nhau 300 ms; thông báo sau khi đã kích hoạt đủ lượt tải.
                    window.setTimeout(function () {
                        alert('Success !');
                    }, 950);
                }).fail(function (error) {
                    console.error('[MOVEFINISH] Export Finished error:', error);
                    alert('Export Finished error!');
                }).always(function () {
                    // Giữ overlay trong lúc ba link tải LD_OF, LD và OF lần lượt được kích hoạt.
                    window.setTimeout(hideFinishedExportLoading, 900);
                });
            }, 50);
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
                    row_DeleteGoing_ByID(idDelete);
                    //btnSearch_Onclick(); 
                    //var $request = $.ajax({
                    //    async: false,
                    //    method: "DELETE",
                    //    url: urlApi + "api/DayFlights/Delete/" + idDelete,
                    //}).always(function (data) {
                    //    if (data.Code != -1) {
                    //        row_DeleteGoing(idDelete);
                    //        btnSearch_Onclick();  
                    //    }
                    //});
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

       function row_DeleteGoing_ByID(idDelete) {
               
                var url = "<%=System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]%>api/ApiExtension/ExcuteReturnInt?packageName=A_TEST_SEARCH&storeName=DELETEGOINGON_New";
                $.ajax({
                    async: false,
                    method: "PUT",
                    url: url,
                    data: JSON.stringify({ P_ID: idDelete,P_USER: '<%= _user.UserName%>'}),
                }).always(function (data) {
                    if (data.ListValue == null || data.ListValue == -1) {                       
                    }
              })
        }

        function viewPopupInfoExtensionSelect() {
            $('#popFlightInfoView').modal('show');
            btnExportSumOF_Onclick();
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
                document.getElementById('btnSaveInfoChange').style.display="";
                document.getElementById('txt_popATD').disabled=true;
                document.getElementById('txt_popATA').disabled=true;
                //document.getElementById('txt_popPERMDATE').disabled=true;
                
               document.getElementById('txt_popPERMNBR').disabled=false;
                 document.getElementById('txt_popCRAFT_T').disabled=false;
                
            }
            else
            {
                document.getElementById('btnSaveInfoChange').style.display="none";
                document.getElementById('btnUpdateInfoChange').style.display = "";
                document.getElementById('txt_popATD').disabled=false;
                document.getElementById('txt_popATA').disabled=false;

                document.getElementById('txt_popPERMNBR').disabled=true;
                document.getElementById('txt_popCRAFT_T').disabled=true;

		$('#txt_FID').val(ax.attr('data-id'));
            console.log(ax.attr('data-id'));
            var url = urlApi + 'api/DayFlights/GetPerm_GoingOnBy?ID=' + ax.attr('data-id');
            
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
                return !color || color === 'transparent' ||
                    color === 'rgba(0, 0, 0, 0)' ||
                    /rgba\([^)]*,\s*0(?:\.0+)?\s*\)$/.test(color);
            }

            function getEffectiveBackground(cell) {
                // Xóa màu đã chụp ở lần trước để đọc đúng class/màu trạng thái hiện tại.
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

            var rows = table.rows;
            for (var rowIndex = 0; rowIndex < rows.length; rowIndex++) {
                var logicalColumn = 0;
                for (var cellIndex = 0; cellIndex < rows[rowIndex].cells.length; cellIndex++) {
                    var cell = rows[rowIndex].cells[cellIndex];
                    var span = parseInt(cell.getAttribute('colspan') || '1', 10);
                    if (logicalColumn < 4) {
                        var left = 0;
                        for (var col = 0; col < logicalColumn; col++) left += columnWidths[col] || 0;
                        cell.classList.add('dayly-frozen-column');
                        cell.style.left = Math.round(left) + 'px';
                        // Sticky cell phải có nền riêng; nếu chỉ dùng nền của tr thì nền sẽ
                        // cuộn đi và dữ liệu phía sau xuyên qua vùng freeze.
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
                button.addEventListener('click', function (event) { if (event.detail === 0) scrollOneColumn(); });
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
            btnSearch_Onclick();
            initializeDaylyHorizontalControls();
            scheduleDaylyStickyFreeze();

            $(document)
                .off('focusin.daylyAutocomplete', '#grdSource thead tr.Spec input[data-autocomplete]')
                .on('focusin.daylyAutocomplete', '#grdSource thead tr.Spec input[data-autocomplete]', function () {
                    $(this).closest('td').addClass('dayly-autocomplete-open');
                })
                .off('focusout.daylyAutocomplete', '#grdSource thead tr.Spec input[data-autocomplete]')
                .on('focusout.daylyAutocomplete', '#grdSource thead tr.Spec input[data-autocomplete]', function () {
                    var $cell = $(this).closest('td');
                    window.setTimeout(function () {
                        if (!$cell.find(':focus').length) {
                            $cell.removeClass('dayly-autocomplete-open');
                        }
                    }, 180);
                });

            var resizeTimer;
            $(window).on('resize.daylySticky', function () {
                clearTimeout(resizeTimer);
                resizeTimer = setTimeout(applyDaylyStickyFreeze, 120);
            });
        });

        
    </script>
    <script type="text/javascript">

        function RenderTableKhExportSum(data) {
            var kq = '';
            //var idx = parseInt($('#tblSource').attr('data-pageindex'));
            ///var pz = parseInt(ddlPageSize.value);
            //var stt = parseInt(((idx - 1) * pz) + 1);
            if (data.ListValue == null) return '';
            console.log(data.ListValue);
            $.each(data.ListValue, function (a, b) {
                kq += "<tr>"

                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FLIGHTDATE) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.TSFPL) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.TSDEP) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.TSARR) + "</td>"
                  
                    + "</tr>";
                //stt++;
            });
            return kq;
        }

		function RenderTableKhExport(data) {
            var kq = '';
            //var idx = parseInt($('#tblSource').attr('data-pageindex'));
            ///var pz = parseInt(ddlPageSize.value);
            //var stt = parseInt(((idx - 1) * pz) + 1);
            if (data.ListValue == null) return '';
            $.each(data.ListValue, function (a, b) {
                kq += "<tr>"
                   
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.OPER_ID) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FLIGHTNBR) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.REGISTRATION) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.REAL_CRAFT_TYPE) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.CRAFT_TYPE) + "</td>"                    
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.PURPOSE) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.PERMTYPE) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FROM_AIRP) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.TO_AIRP) + "</td>"
                    + "<td style=\'text-align: left;\'>" + new Date(b.FLIGHTDATE).format('dd-mm-yyyy') + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.ATD) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.ATA) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.VIA) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FPL_VIA) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.REMARK) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.LASTUSER) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.ETD) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.ETA) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.EOBT) + "</td>"
                    + "</tr>";
                //stt++;
            });
            return kq;
        }
		function returnEmpty(val) {
            return val == null ? "" : val;
        }

		function normalizeMoveFinishPermType(value) {
            return $.trim(returnEmpty(value)).replace(/\s+/g, '').toUpperCase();
        }

		function buildMoveFinishExportTable(rows) {
            var strAppend = RenderTableKhExport({ ListValue: rows });
            var strHtml = "<table id='tblSourceExport' class='table table-bordered'>";
            strHtml += "<thead style='color: red'>"
                    + "<tr>"
                    + "<th>OPER</th>"
                    + "<th>CALLSIGN</th>"
                    + "<th>REGIS</th>"
                    + "<th>R_CRAFT</th>"
                    + "<th>F_CRAFT</th>"
                    + "<th>PURPOSE</th>"
                    + "<th>P_TYPE</th>"
                    + "<th>FROM</th>"
                    + "<th>TO</th>"
                    + "<th>FLIGHTDATE</th>"
                    + "<th>ATD</th>"
                    + "<th>ATA</th>"
                    + "<th>VIA</th>"
                    + "<th>FPL_VIA</th>"
                    + "<th>REMARK</th>"
                    + "<th>LASTUSER</th>"
                    + "<th>ETD</th>"
                    + "<th>ETA</th>"
                    + "<th>EOBT</th>"
                    + "</tr>"
                    + "</thead>"
                    + "<tbody>"
                    + strAppend
                    + "</tbody>"
                    + "</table>";

            return strHtml;
        }

		function exportMoveFinishByPermType(rows) {
            var ldOfRows = [];
            var ldRows = [];
            var ofRows = [];

            $.each(rows || [], function (_, row) {
                var permType = normalizeMoveFinishPermType(row.PERMTYPE);
                if (permType === 'LD') {
                    ldOfRows.push(row);
                    ldRows.push(row);
                } else if (permType === 'O/F') {
                    ldOfRows.push(row);
                    ofRows.push(row);
                }
            });

            var exportGroups = [
                { suffix: 'LD_OF', rows: ldOfRows },
                { suffix: 'LD', rows: ldRows },
                { suffix: 'OF', rows: ofRows }
            ];

            console.log('[MOVEFINISH] Export by PERMTYPE', {
                LD_OF: exportGroups[0].rows.length,
                LD: ldRows.length,
                OF: ofRows.length
            });

            $.each(exportGroups, function (index, group) {
                window.setTimeout(function () {
                    exportExelMOVEFINISH(
                        buildMoveFinishExportTable(group.rows),
                        group.suffix
                    );
                }, index * 300);
            });
        }

		function LoadDataGrid_ExportFinish() {
            var _date = $('#ddlDateFlight option:selected').text() == '' ? new Date().format('dd-MM-yyyy') : $('#ddlDateFlight option:selected').text().replace(/\//gi, '-').replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$1/$2/$3').replace('/','-').replace('/','-');
           			
			var _obj = {P_USER:'<%= _user.UserName%>',P_DATE: _date};
			
			var _urlPath = "";
			_urlPath= "api/ApiExtension/ExcuteTable?packageName=A_TEST_SEARCH&storeName=GetExportMOVEFINISH"
            
			//console.log(_obj);
			
		   
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + _urlPath,
                data: JSON.stringify(_obj)
            });

            return $request.then(function (data) {
                if (!data || data.ListValue == null) {
                    return rejectFinishedExport('GET_EXPORT_DATA', data);
                }

                exportMoveFinishByPermType(data.ListValue);
            });
        }
				  
        function generate_excel(html) {
           
			exportExel(html);
        }
		function exportExelMOVEFINISH(_html, fileSuffix) {
			
            var dt = new Date();
            var day = dt.getDate();
            var month = dt.getMonth() + 1;
            var year = dt.getFullYear();
            var hour = dt.getHours();
            var mins = dt.getMinutes();
            var postfix = day + "." + month + "." + year + "_" + hour + "." + mins;
			
			
            var textToSave = _html;
            var textToSaveAsBlob = new Blob([textToSave], { type: "application/vnd.ms-excel;charset=utf-8" });
            var textToSaveAsURL = window.URL.createObjectURL(textToSaveAsBlob);
            var safeSuffix = fileSuffix
                ? String(fileSuffix).replace(/[^0-9A-Za-z_-]/g, '_') + '_'
                : '';
            var fileNameToSaveAs = 'exported_MOVEFINISH_' + safeSuffix + postfix + '.xls';
            var downloadLink = document.createElement("a");
            downloadLink.download = fileNameToSaveAs;
            downloadLink.innerHTML = "Download File";
            downloadLink.href = textToSaveAsURL;
            downloadLink.onclick = destroyClickedElement;
            downloadLink.style.display = "none";
            document.body.appendChild(downloadLink);
            downloadLink.click();

            window.setTimeout(function () {
                window.URL.revokeObjectURL(textToSaveAsURL);
            }, 2000);

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
    + 1) : 1,
                RowFinish: isSroll ? (parseInt($('#grdSource tbody tr').last().attr('data-RowNumber'))
    + appenLoad) : 6000,
                OptionDate: $('#ddlDateFlight').val(),
                //FLIGHTDATE: $('#ddlDateFlight option:selected').text() == '' ? new Date().format('dd/MM/yyyy') : $('#ddlDateFlight option:selected').text().replace(/\//gi, '-').replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$1/$2/$3'),
                FLIGHTDATE:'07/09/2021',
                VIA: $('#txtROUTE').val(),
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
