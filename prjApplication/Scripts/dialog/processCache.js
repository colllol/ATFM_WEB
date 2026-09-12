
var dialog = new JDialog({
    uri: 'DIVHolderID',
    title: 'Thiết lập hạn định',
    showDefaultButton: false,
    showCancelButton: true,
    enableScrollHandler: true,
    reload: true,
    loadingImageUrl: '../Scripts/dialog/activity.gif',
    customSettings:
            {
                frameWidth: 550,
                frameHeight: 300
            }
});
function ProcessCache() {
    var $loaderData = $('#bindata');
    var _linkCache = '../Ajax/Caching.aspx';
    $('#bindata').html('<img src="../Images/Icons/loading.gif" alt="" width="100px" style="margin-bottom:10px;" /><div style="text-align: center; font-weight: bold; color: #29a825; font-size: 18px;">Đang xử lý tạo Cache...</div>');
    $('.DIVC0verFrame').show();
    dialog.setTitle('Thông báo');
    dialog.setURI('DIVHolderID');
    dialog.setFrame(250, 200);
    dialog.doModal();
    $loaderData.load(_linkCache, function() {
        var _value = $('#bindata').text().trim();
        if (_value.trim() == 'OK') {
            $('.DIVC0verFrame').hide();
        }
    });
}
//$(document).ready(function() {
//    ProcessCache();
//});
