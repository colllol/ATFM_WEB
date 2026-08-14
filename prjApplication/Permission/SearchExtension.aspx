<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master"
AutoEventWireup="true" CodeBehind="SearchExtension.aspx.cs"
Inherits="prjApplication.Permission.SearchExtension" %> <%@ Register
Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />

  <style>
    .preloader {
      display: inline-block;
      padding: 0px;
      border-radius: 100%;
      border: 2px solid;
      border-top-color: rgba(0, 0, 0, 0.65);
      border-bottom-color: rgba(0, 0, 0, 0.15);
      border-left-color: rgba(0, 0, 0, 0.65);
      border-right-color: rgba(0, 0, 0, 0.15);
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
  <style>
    input {
      text-transform: uppercase;
    }

    #tblSearch {
      width: 100%;
      border-collapse: separate;
      border-spacing: 9px 0;
    }

    #tblSearch td {
      padding: 0;
      vertical-align: top;
      white-space: nowrap;
    }

    #tblSearch tr:first-child td {
      padding: 0 2px 5px;
      color: #234761;
      font-size: 11px;
      font-weight: 600;
      line-height: 16px;
    }

    #tblSearch tr:nth-child(2) td {
      padding-bottom: 8px;
    }

    #tblSource {
      font-size: 12px !important;
      width: 100%;
      min-width: 1640px;
      margin-bottom: 0;
    }

    #tblSource th,
    #tblSource td {
      white-space: nowrap;
      vertical-align: middle;
    }

    #tblSource .search-extension-col-callsign {
      min-width: 110px;
    }

    #tblSource .search-extension-col-airport {
      min-width: 85px;
    }

    #tblSource .search-extension-col-etd,
    #tblSource .search-extension-col-type {
      min-width: 80px;
    }

    #tblSource .search-extension-col-craft {
      min-width: 100px;
    }

    .search-extension-table-shell {
      position: relative;
      margin-top: 8px;
    }

    .search-extension-table-scroll {
      width: 100%;
      overflow-x: auto;
      overflow-y: visible;
      border: 1px solid #c5d9ea;
      border-radius: 5px;
      -webkit-overflow-scrolling: touch;
    }

    .search-extension-scroll-controls {
      position: absolute;
      top: 50%;
      right: 4px;
      z-index: 5000;
      display: flex !important;
      flex-direction: column;
      gap: 5px;
      transform: translateY(-50%);
      visibility: visible !important;
    }

    .search-extension-scroll-button {
      display: flex;
      width: 34px;
      height: 40px;
      padding: 0;
      align-items: center;
      justify-content: center;
      border: 1px solid rgba(255, 255, 255, .8);
      border-radius: 8px;
      background: linear-gradient(135deg, #337ab7, #185d93);
      color: #fff;
      box-shadow: 0 4px 12px rgba(20, 68, 105, .3);
      transition: background .18s ease, box-shadow .18s ease, transform .18s ease, opacity .18s ease;
      touch-action: none;
      user-select: none;
    }

    .search-extension-scroll-button:hover,
    .search-extension-scroll-button:focus {
      outline: 0;
      background: linear-gradient(135deg, #2f8dcc, #174f7d);
      box-shadow: 0 6px 16px rgba(20, 68, 105, .4);
    }

    .search-extension-scroll-button:active,
    .search-extension-scroll-button.is-holding {
      transform: scale(.95);
    }

    .search-extension-scroll-button:disabled {
      cursor: default;
      opacity: .42;
      box-shadow: none;
    }

    .search-extension-scroll-symbol {
      font-size: 22px;
      font-weight: 700;
      line-height: 1;
    }
  </style>

  <table id="tblSearch">
    <tr>
      <td>CALLSIGN</td>
      <td>FROM</td>
      <td>TO</td>
      <td>CRAFT</td>
      <td>VIA</td>
      <td>FLIGHT DATE</td>
      <td>PERM NUMBER</td>
      <td>OPER</td>
      <td>FLIGHT TYPE</td>
      <td>TYPE</td>
      <td>ETD</td>
      <td>PURPOSE</td>
      <td>REMARK</td>
    </tr>
    <tr>
      <td>
        <input id="sCallSign" maxlength="8" class="wid_100px" type="text" />
      </td>
      <td>
        <input id="sFrom_Airp" class="wid_60px" type="text" />
      </td>
      <td>
        <input id="sTo_Airp" class="wid_60px" type="text" />
      </td>
      <td>
        <input
          id="sCraft"
          class="wid_75px"
          data-autocomplete="CRAFT"
          type="text"
        />
      </td>
      <td>
        <input id="sVia" maxlength="200" type="text" />
      </td>
      <td>
        <input
          data-checkdate="true"
          id="sFlightDate"
          class="wid_90px"
          type="text"
          maxlength="10"
          placeholder="DD-MM-YYYY"
        />
      </td>
      <td>
        <input id="sPermNbr" maxlength="8" class="wid_80px" type="text" />
      </td>
      <td>
        <input
          id="sOper"
          data-autocomplete="OPER"
          class="wid_60px"
          type="text"
        />
      </td>
      <td>
        <input
          id="sFlightType"
          data-autocomplete="FLIGHTTYPE"
          class="wid_60px"
          type="text"
        />
      </td>
      <td style="width: 90px">
        <input
          id="sPermType"
          data-autocomplete="PERMTYPE"
          class="wid_60px"
          type="text"
        />
      </td>
      <td>
        <input
          id="sEtd"
          type="text"
          data-number="true"
          maxlength="4"
          class="wid_50px"
        />
      </td>
      <td>
        <input
          id="sPurpose"
          data-autocomplete="PURPOSE"
          type="text"
          class="wid_60px"
        />
      </td>
      <td>
        <input id="sRemark" maxlength="200" type="text" class="wid_100px" />
      </td>
    </tr>
  </table>

  <button
    type="button"
    id="btnSearchExtension"
    class="btn btn-sm btn-primary"
    onclick="btnSearchExtension_Click()"
  >
    Search
  </button>
  <button
    type="button"
    id="btnClearSearchExten"
    class="btn btn-sm btn-primary"
    onclick="btnClearSearchExten_Click()"
  >
    Clear
  </button>
  <div class="search-extension-table-shell">
    <div id="searchExtensionTableScroll" class="search-extension-table-scroll">
      <table id="tblSource" class="table table-bordered" data-atfm-responsive-table="off">
        <thead>
          <tr>
            <th></th>
            <th>Permission number</th>
            <th>Permission date</th>
            <th>Daily</th>
            <th>ValidHours</th>
            <th>ValidDate</th>
            <th class="search-extension-col-callsign">Callsign</th>

            <th class="search-extension-col-airport">From</th>
            <th class="search-extension-col-airport">To</th>
            <th class="search-extension-col-etd">Etd</th>
            <th class="search-extension-col-type">Type</th>
            <th>Flight type</th>
            <th>Oper</th>
            <th>Purpose</th>
            <th class="search-extension-col-craft">Craft</th>
            <th>Via</th>
            <th>Remark</th>
          </tr>
        </thead>
        <tbody></tbody>
      </table>
    </div>
    <div class="search-extension-scroll-controls" aria-label="Horizontal table controls">
      <button id="searchExtensionScrollLeft" class="search-extension-scroll-button" type="button"
        title="Scroll table left" aria-label="Scroll table left">
        <span class="search-extension-scroll-symbol" aria-hidden="true">&#10094;</span>
      </button>
      <button id="searchExtensionScrollRight" class="search-extension-scroll-button" type="button"
        title="Scroll table right" aria-label="Scroll table right">
        <span class="search-extension-scroll-symbol" aria-hidden="true">&#10095;</span>
      </button>
    </div>
  </div>

  <script src="../Scripts/CustomDynamic.js"></script>
  <script src="../Scripts/CustumStaticdata.js"></script>
  <script src="../Scripts/CustomPaging.js"></script>
  <script>
           function btnSearchExtension_Click() {
               $('#tblSource').attr('data-pageindex', '1');
               $('#tblSource').attr('data-total', 0);
               LoadDataBySearch();
           }
           function LoadDataBySearch() {
               var $request = $.ajax({
                   method: "POST",
                   url: "SearchExtension.aspx/SearchPermissions",
                   data: JSON.stringify({ request: getObjectSearch() }),
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   beforeSend: function () {
                       $('#tblSource tbody tr').remove();
                       preloadImg('tblSource', 'loadingData', '25px', '25px');
                   },
                   complete: function () {
                       unLoadingData('loadingData');
                       $('#tblSource').paging({
                           onClickButton: 'LoadDataBySearch',
                       });
                       $(window).trigger('resize');
                   },
               }).done(function (response) {
                   var data = response && response.d ? response.d : response;
                   if (!data || data.Code !== '00') {
                       unLoadingData('loadingData');
                       $('#tblSource').attr('data-total', '0');
                       alert(data && data.Message ? data.Message : 'Không thể tải dữ liệu tìm kiếm.');
                       return;
                   }
                   $('#tblSource tbody tr').remove();
                   $('#tblSource').attr('data-total', data.Total || 0);
                   $.each(data.Items || [], function (a, b) {
                       var permId = parseInt(b.PERM_ID, 10);
                       var permLink = returnEmpty(b.PERMNBR_ID);
                       if (!isNaN(permId) && permId > 0) {
                           permLink = "<a href='#' class='js-view-permission-sc' data-perm-id='" + permId
                               + "' data-flight-nbr='" + encodeURIComponent(returnEmpty(b.FLIGHTNBR))
                               + "' title='View permission details'>" + returnEmpty(b.PERMNBR_ID) + "</a>";
                       }

                       $('#tblSource tbody').append("<tr>"
                       + "<td>" + b.RNUM + "</td>"
                       + "<td style='white-space: nowrap;'>" + permLink + "</td>"
                       + "<td>" + returnEmpty(new Date(b.PERMDATE).format('dd-mm-yyyy')) + "</td>"
                       + "<td style='white-space: nowrap;'>" + returnEmpty(b.DAYLY) + "</td>"
                       + "<td style='text-align:center; white-space: nowrap;'>" + returnEmpty(b.VALIDDATE) + "</td>"
                       + "<td style='text-align:center; white-space: nowrap;'>" + returnEmpty(b.VALIDDATEPER) + "</td>"
                       + "<td class='search-extension-col-callsign'>" + returnEmpty(b.FLIGHTNBR) + "</td>"

                       + "<td class='search-extension-col-airport'>" + returnEmpty(b.FROM_AIRP) + "</td>"
                       + "<td class='search-extension-col-airport'>" + returnEmpty(b.TO_AIRP) + "</td>"
                       + "<td class='search-extension-col-etd'>" + returnEmpty(b.ETD) + "</td>"
                       + "<td class='search-extension-col-type'>" + returnEmpty(b.PERMTYPE) + "</td>"
                       + "<td>" + returnEmpty(b.FTYPE) + "</td>"
                       + "<td>" + returnEmpty(b.OPER_ID) + "</td>"
                       + "<td>" + returnEmpty(b.PURPOSE_ID) + "</td>"
                       + "<td class='search-extension-col-craft'>" + returnEmpty(b.CRAFT) + "</td>"
                       + "<td>" + returnEmpty(b.VIA) + "</td>"
                       + "<td>" + returnEmpty(b.REMARK) + "</td>"
                       + "</tr>");
                   });
               }).fail(function (xhr) {
                   $('#tblSource tbody tr').remove();
                   $('#tblSource').attr('data-total', '0');
                   var message = 'Không thể tải dữ liệu tìm kiếm.';
                   if (xhr.responseJSON && xhr.responseJSON.Message) message += ' ' + xhr.responseJSON.Message;
                   alert(message);
               });
           }
           function getObjectSearch() {
               if ($('#tblSource').attr('data-pageSize') == null) $('#tblSource').attr('data-pageSize', 500);
               if ($('#tblSource').attr('data-pageIndex') == null) $('#tblSource').attr('data-pageIndex', 1);
               return {
                   FlightNbr: $.trim($('#sCallSign').val()),
                   FromAirp: $.trim($('#sFrom_Airp').val()),
                   ToAirp: $.trim($('#sTo_Airp').val()),
                   Craft: $.trim($('#sCraft').val()),
                   Via: $.trim($('#sVia').val()),
                   FlightDate: $.trim($('#sFlightDate').val()),
                   PermNbr: $.trim($('#sPermNbr').val()),
                   Oper: $.trim($('#sOper').val()),
                   FlightType: $.trim($('#sFlightType').val()),
                   PermType: $.trim($('#sPermType').val()),
                   Etd: $.trim($('#sEtd').val()),
                   Purpose: $.trim($('#sPurpose').val()),
                   Remark: $.trim($('#sRemark').val()),
                   PageSize: parseInt($('#tblSource').attr('data-pageSize'), 10) || 500,
                   PageIndex: Math.max(0, (parseInt($('#tblSource').attr('data-pageIndex'), 10) || 1) - 1)
               };
           }
           function returnEmpty(val) {
               return val == null ? "" : val;
           }
           function btnClearSearchExten_Click() {
               $('#sCallSign').val('');
               $('#sFrom_Airp').val('');
               $('#sTo_Airp').val('');
               $('#sCraft').val('');
               $('#sVia').val('');
               $('#sPermNbr').val('');
               $('#sFlightDate').val('');
               $('#sOper').val('');
               $('#sFlightType').val('');
               $('#sPermType').val('');
               $('#sEtd').val('');
               $('#sRemark').val('');
               $('#sPurpose').val('');
           }

           function initializeSearchExtensionScrollControls() {
               var scrollContainer = document.getElementById('searchExtensionTableScroll');
               var leftButton = document.getElementById('searchExtensionScrollLeft');
               var rightButton = document.getElementById('searchExtensionScrollRight');
               if (!scrollContainer || !leftButton || !rightButton
                   || rightButton.getAttribute('data-bound') === 'true') return;

               leftButton.setAttribute('data-bound', 'true');
               rightButton.setAttribute('data-bound', 'true');

               function updateButtonState() {
                   var maxScrollLeft = Math.max(0, scrollContainer.scrollWidth - scrollContainer.clientWidth);
                   leftButton.disabled = scrollContainer.scrollLeft <= 1;
                   rightButton.disabled = maxScrollLeft <= 1 || scrollContainer.scrollLeft >= maxScrollLeft - 1;
               }

               function getScrollStep() {
                   var header = document.querySelector('#tblSource thead .search-extension-col-callsign');
                   var columnWidth = header ? header.getBoundingClientRect().width : 110;
                   return Math.max(180, Math.round(columnWidth * 2));
               }

               function bindScrollButton(button, direction) {
                   var holdTimer = null;
                   var holdFrame = null;
                   var isHolding = false;

                   function scrollOneStep() {
                       scrollContainer.scrollBy({
                           left: direction * getScrollStep(),
                           behavior: 'smooth'
                       });
                   }

                   function scrollContinuously() {
                       scrollContainer.scrollLeft += direction * 4;
                       holdFrame = window.requestAnimationFrame(scrollContinuously);
                   }

                   function beginHold(event) {
                       if (button.disabled || (event.button !== undefined && event.button !== 0)) return;
                       event.preventDefault();
                       isHolding = false;
                       if (button.setPointerCapture && event.pointerId !== undefined) {
                           button.setPointerCapture(event.pointerId);
                       }
                       holdTimer = window.setTimeout(function () {
                           isHolding = true;
                           button.classList.add('is-holding');
                           scrollContinuously();
                       }, 280);
                   }

                   function endHold(event) {
                       if (holdTimer === null && holdFrame === null) return;
                       window.clearTimeout(holdTimer);
                       holdTimer = null;
                       if (holdFrame !== null) window.cancelAnimationFrame(holdFrame);
                       holdFrame = null;
                       button.classList.remove('is-holding');
                       if (!isHolding) scrollOneStep();
                       isHolding = false;
                       if (event) event.preventDefault();
                   }

                   button.addEventListener('pointerdown', beginHold);
                   button.addEventListener('pointerup', endHold);
                   button.addEventListener('pointercancel', endHold);
                   button.addEventListener('lostpointercapture', endHold);
                   button.addEventListener('click', function (event) {
                       if (event.detail === 0 && !button.disabled) scrollOneStep();
                   });
               }

               bindScrollButton(leftButton, -1);
               bindScrollButton(rightButton, 1);
               scrollContainer.addEventListener('scroll', updateButtonState);
               window.addEventListener('resize', updateButtonState);
               updateButtonState();
           }

           $(document).ready(function () {
               initializeSearchExtensionScrollControls();
           });

           $('#tblSearch input[type="text"]').each(function () {
               $(this).ValidateTip();
           })
           $('#tblSource').on('click', '.js-view-permission-sc', function (event) {
               event.preventDefault();
               getcontentPerm(
                   $(this).attr('data-perm-id'),
                   decodeURIComponent($(this).attr('data-flight-nbr') || '')
               );
           });

           function getcontentPerm(permId, flightNbr) {
               var normalizedPermId = parseInt(permId, 10);
               if (isNaN(normalizedPermId) || normalizedPermId <= 0) {
                   alert('Permission ID is invalid.');
                   return false;
               }

               // SearchPermExtension tra du lieu tu phep SC. FTYPE la loai chuyen
               // bay nghiep vu cua phep, khong phai dau hieu de chon bang SC/NO.
               var viewUrl = '<%= Page.ResolveUrl("~/Permission/View_PermSC.aspx") %>'
                   + '?Menu_Id=' + encodeURIComponent('<%= System.Web.HttpUtility.JavaScriptStringEncode(Request.Params["Menu_ID"] ?? string.Empty) %>')
                   + '&ID=' + encodeURIComponent(normalizedPermId)
                   + '&FlightNbr=' + encodeURIComponent($.trim(flightNbr || ''));
               var detailWindow = window.open(
                   viewUrl,
                   '_blank',
                   'toolbar=yes,scrollbars=yes,resizable=yes'
               );

               if (detailWindow) {
                   detailWindow.resizeTo(window.screen.availWidth, window.screen.availHeight);
                   detailWindow.focus();
               } else {
                   alert('Please allow pop-ups to view permission details.');
               }

               return false;
           }
  </script>
</asp:Content>
