

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BÁO CÁO SỐ LIỆU BAY</title>
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Style/StyleRpDHB.css" rel="stylesheet" />

            <script src="http://192.168.62.20:2121//Style/assets/js/jquery-2.1.4.min.js"></script>
        <link href="http://192.168.62.20:2121//Style/style.css" rel="stylesheet" />
        <script src="http://192.168.62.20:2121//Scripts/Lib.js"></script>


	<link href="../Scripts/DateTimeJQ/jquery.datetimepicker.css" rel="stylesheet" />
   
    <script src="../Scripts/DateTimeJQ/jquery.datetimepicker.js"></script>

</head>
<body>
    <form id="form2" runat="server">
    
    <div id="abcxyz" class="well well-sm" style="text-align: center;">
        <b>FROM DATE :</b>
         <input id="txtFromDate" data-minlenght="1" data-control="checkAccess" type="text"
            placeholder="select date" class="wid_100px" />
        <b>TO DATE :</b>
          <input id="txtToDate" data-minlenght="1" data-control="checkAccess" type="text"
            placeholder="select date" class="wid_100px" />
        <button type="button" id="btnSearch" class="btn btn-sm btn-primary" style="width: 100px" onclick="btnSearch_OnClick()">
            Search</button>
		<button type="button" id="btnExport" class="btn btn-sm btn-primary" style="width: 135px" onclick="LoadDataGrid_Export()">
            Export Excel</button>
        
    </div>


    <div id="exportid" data-toggle="tooltip">

        <div id = "headerID" style = "text-align: center;"><table class="tgheard" style="width: 100%"><tr><td class="tg-5fpg" style ="width:48%;" colspan="3">TRUNG TÂM QLLKL<br/><span class="fontRp13">TRUNG TÂM HĐB&ĐPLKL<br/><br></th></td><td class="tg-5fpg" style ="width:4%;"></td><td class="tg-5fpg" colspan="4" style ="width:48%;"></td></tr><tr><td class="tg-5fpg fontRp14" colspan="8" style = "text-align:center;"><span style = "font-weight:bold;text-align:center;font-size:24px;"> TỔNG HỢP SỐ LIỆU BAY</span></td></tr><tr><td class="tg-mqab fontRp14"  colspan="3" style="text-align:right;font-weight:bold;">Từ ngày : <b id="itungay"></b></td><td class="tg-oqgr"></td><td class="tg-31sd fontRp14"  colspan="4" style="text-align:left;font-weight:bold;">Đến ngày: <b id="idenngay"></b></td></tr><tr><td colspan="8"></td></tr><tr><td class="tg-5fpg" colspan="8"></td></tr></table></div>
        <div id="contentid">
            <div id = "content">


        </div>
        <div id = "footer"><table class="tgft" style="width: 100%"><colgroup><col style='width:193px'><col style='width:181px'><col style='width:178px'><col style='width:143px'><col style='width:144px'><col style='width:130px'><col style='width:119px'><col style='width:130px'></colgroup><thead><tr><th class='tg-zv4m' colspan='8></th></tr></thead><tbody><tr><td class='tg-zv4m' colspan='4'>GHI CHÚ :</td><td class='tg-zv4m' rowspan='4'></td><td class='tg-zv4m'>NGÀY</td><td class='tg-zv4m'>THÁNG</td><td class='tg-zv4m'>NĂM</td></tr><tr><td class='tg-zv4m' colspan='4'></td><td class='tg-zv4m' colspan='3' rowspan='2'></td></tr><tr><td class='tg-zv4m' colspan='4'></td></tr><tr><td class='tg-zv4m' colspan='4'>NGƯỜI LẬP BÁO CÁO</td><td class='tg-8jgo' colspan='3'>TRƯỞNG TT HĐB &amp; ĐPLKL</td></tr></tbody></table></div>

    </div>

    <script src="../Scripts/CustumStaticdata.js"></script>
    <script>
      $('#txtFromDate').datetimepicker({
            timepicker: false,
            formatTime: '',
            format: 'd-m-Y',
            formatDate: 'd-m-Y'
        });
        $('#txtToDate').datetimepicker({
            timepicker: false,
            formatTime: '',
            format: 'd-m-Y',
            formatDate: 'd-m-Y'
        });
    </script>
    <script>
        
		
		//LoadDataGrid();   		
    </script>
	<script language="javascript" type="text/javascript">
		
        var idTongQT=0;
		function btnSearch_OnClick() {
			LoadSumData();
			LoadDataGrid();
        }
		function LoadSumData() {
			
			 var $request = $.ajax({
                method: "PUT",
                url: urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=REPORT_DHB&storeName=DHB_Fun_SumHQT_LD",
				 data: JSON.stringify({ P_FROMDATE: $('#txtFromDate').val(), P_TODATE: $('#txtToDate').val()}),
                
            }).always(function (data) {
                /*if (data.ListValue[0] != null) {
                    $(data.ListValue).each(function (a, b) {
                        var c = "<div onclick=\"lblNbr_OnRowClick(" + b.ID + "); $(this).addClass('select');\">" + new Date(b.LETTERNBR_PK).format('dd/mm/yyyy HH:MM:ss') + "</div>";
                        $('#lblNbr').append(c);
                    })
                }*/
				idTongQT = data.ListValue;
				//$('#idTongQT').html('');
				//$('#idTongQT').append(data.ListValue);
				//console.log(data.ListValue);
            });
			
           
        }
		function returnEmpty(val) {
            return val == null ? "" : val;
        }
		function LoadDataGrid() {
			 
			  
			 $('#itungay').html('');
			 $('#itungay').append($('#txtFromDate').val());
				
			 $('#idenngay').html('');
			 $('#idenngay').append($('#txtToDate').val());	
				
			 var kq = '';
			 var header = '';
			 var footer = '';
			 var $request = $.ajax({
                method: "PUT",
                url: urlApi + "api/ApiExtension/ExcuteTable?packageName=REPORT_DHB&storeName=DHB_Fun_QNQT_LD",
				 data: JSON.stringify({ P_FROMDATE: $('#txtFromDate').val(), P_TODATE: $('#txtToDate').val()}),
                
            }).always(function (data) {
                var stt = 1;
				var tongcong=0;
				var tong=0
				var thang='';
				if (data.ListValue == null) stt= '';
				var total = data.ListValue.length;
				$.each(data.ListValue, function (a, b) {
					tongcong = tongcong+ b.TT
					if (stt==1){
						kq += "<tr><td class=\'tg-de2y\' rowspan=\'"+returnEmpty(total)+"\'></td><td class=\'tg-de2y\'>"+ returnEmpty(b.OPER_ID) + "</td>"
							+ "<td class=\'tg-de2y\' style=\'text-align:center\'>"+returnEmpty(b.QN)+"</td>"
							+ "<td class=\'tg-de2y\' style=\'text-align:center\'>"+returnEmpty(b.QT)+"</td><td class=\'tg-de2y\'></td>"
							+ "<td class=\'tg-de2y\'style=\'text-align:center\'><b>"+returnEmpty(b.TT)+"</b></td>"
							+ "</tr>";
						}
					else
					{
						kq += "<tr><td class=\'tg-de2y\'>"+ returnEmpty(b.OPER_ID) + "</td>"
							+ "<td class=\'tg-de2y\' style=\'text-align:center\'>"+returnEmpty(b.QN)+"</td>"
							+ "<td class=\'tg-de2y\' style=\'text-align:center\'>"+returnEmpty(b.QT)+"</td><td class=\'tg-de2y\'></td>"
							+ "<td class=\'tg-de2y\'style=\'text-align:center\'><b>"+returnEmpty(b.TT)+"</b></td>"
							+ "</tr>";
					}
					stt++;
				});
				thang = $('#txtFromDate').val().substring(3,10);
				tong = tongcong + parseInt(idTongQT);
				header ="<table class=\'tgbn\' style=\'width: 100%\'><colgroup><col style = \'width:138px\'><col style = \'width:179px\'><col style = \'width:98px\'><col style = \'width:101px\'><col style = \'width:106px\'><col style = \'width:92px\'><col style = \'width:141px\'><col style = \'width: 132px\'></colgroup><thead><tr><th class=\'tg-9ydz\'>THÁNG</th><th class=\'tg-9ydz\'>HÃNG HÀNG KHÔNG</th><th class=\'tg-9ydz\'>QN</th><th class=\'tg-9ydz\'>QT</th><th class='tg-9ydz'>GHI CHÚ</th><th class='tg-9ydz'>TỔNG</th></tr></thead><tbody><tr><td class=\'tg-de2y\' style=\'text-align:center\'>"+thang+"</td><td class=\'tg-de2y\' colspan=\'3\'>QUỐC TẾ</td><td class=\'tg-de2y\'></td><td class=\'tg-de2y\'style=\'text-align:center\'><b id='idTongQT'>"+idTongQT+"</b></td></tr>"	
				
				footer = "<tr><td class=\'tg-sn4r\' colspan=\'5\'>Tổng số các chuyến bay trong tháng : </td><td class=\'tg-de2y\' style=\'text-align:center\'><b>"+ tong+"</b></td></tr><tr><td class=\'tg-sn4r\' colspan=\'5\'>Tổng cộng :</td><td class=\'tg-de2y\' style=\'text-align:center\'><b>"+tong+"</b></td></tr></tbody></table>"
				
				$('#content').html('');
				$('#content').append(header + kq + footer);
								
            });
			$request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }
		function destroyClickedElement(event) {
            document.body.removeChild(event.target);
        }
		function LoadDataGrid_Export() {
			var kq = '';
			 var header = '';
			 var footer = '';
			 var $request = $.ajax({
                method: "PUT",
                url: urlApi + "api/ApiExtension/ExcuteTable?packageName=REPORT_DHB&storeName=DHB_Fun_QNQT_LD",
				 data: JSON.stringify({ P_FROMDATE: $('#txtFromDate').val(), P_TODATE: $('#txtToDate').val()}),
                
            }).always(function (data) {
                var stt = 1;
				var tongcong=0;
				var tong=0
				var thang='';
				if (data.ListValue == null) stt= '';
				var total = data.ListValue.length;
				$.each(data.ListValue, function (a, b) {
					tongcong = tongcong+ b.TT
					if (stt==1){
						kq += "<tr><td class=\'tg-de2y\' rowspan=\'"+returnEmpty(total)+"\'></td><td class=\'tg-de2y\'>"+ returnEmpty(b.OPER_ID) + "</td>"
							+ "<td class=\'tg-de2y\' style=\'text-align:center\'>"+returnEmpty(b.QN)+"</td>"
							+ "<td class=\'tg-de2y\' style=\'text-align:center\'>"+returnEmpty(b.QT)+"</td><td class=\'tg-de2y\'></td>"
							+ "<td class=\'tg-de2y\'style=\'text-align:center\'><b>"+returnEmpty(b.TT)+"</b></td>"
							+ "</tr>";
						}
					else
					{
						kq += "<tr><td class=\'tg-de2y\'>"+ returnEmpty(b.OPER_ID) + "</td>"
							+ "<td class=\'tg-de2y\' style=\'text-align:center\'>"+returnEmpty(b.QN)+"</td>"
							+ "<td class=\'tg-de2y\' style=\'text-align:center\'>"+returnEmpty(b.QT)+"</td><td class=\'tg-de2y\'></td>"
							+ "<td class=\'tg-de2y\'style=\'text-align:center\'><b>"+returnEmpty(b.TT)+"</b></td>"
							+ "</tr>";
					}
					stt++;
				});
				thang = $('#txtFromDate').val().substring(3,10);
				tong = tongcong + parseInt(idTongQT);
				header ="<table class=\'tgbn\' style=\'width: 100%\'><colgroup><col style = \'width:138px\'><col style = \'width:179px\'><col style = \'width:98px\'><col style = \'width:101px\'><col style = \'width:106px\'><col style = \'width:92px\'><col style = \'width:141px\'><col style = \'width: 132px\'></colgroup><thead><tr><th class=\'tg-9ydz\'>THÁNG</th><th class=\'tg-9ydz\'>HÃNG HÀNG KHÔNG</th><th class=\'tg-9ydz\'>QN</th><th class=\'tg-9ydz\'>QT</th><th class='tg-9ydz'>GHI CHÚ</th><th class='tg-9ydz'>TỔNG</th></tr></thead><tbody><tr><td class=\'tg-de2y\' style=\'text-align:center\'>"+thang+"</td><td class=\'tg-de2y\' colspan=\'3\'>QUỐC TẾ</td><td class=\'tg-de2y\'></td><td class=\'tg-de2y\'style=\'text-align:center\'><b id='idTongQT'>"+idTongQT+"</b></td></tr>"	
				
				footer = "<tr><td class=\'tg-sn4r\' colspan=\'5\'>Tổng số các chuyến bay trong tháng : </td><td class=\'tg-de2y\' style=\'text-align:center\'><b>"+ tong+"</b></td></tr><tr><td class=\'tg-sn4r\' colspan=\'5\'>Tổng cộng :</td><td class=\'tg-de2y\' style=\'text-align:center\'><b>"+tong+"</b></td></tr></tbody></table>"
				
				 generate_excel(header + kq + footer);
				
								
            });
			$request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
		}
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

            var textToSave = _html;
            var textToSaveAsBlob = new Blob([textToSave], { type: "text/plain" });
            var textToSaveAsURL = window.URL.createObjectURL(textToSaveAsBlob);
            var fileNameToSaveAs = 'exported_SL_BAYNGAY_' + postfix + '.xls';
            var downloadLink = document.createElement("a");
            downloadLink.download = fileNameToSaveAs;
            downloadLink.innerHTML = "Download File";
            downloadLink.href = textToSaveAsURL;
            downloadLink.onclick = destroyClickedElement;
            downloadLink.style.display = "none";
            document.body.appendChild(downloadLink);
            downloadLink.click();

        }
    </script>
 </form>
</body>

</html>
