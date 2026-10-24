//$(function() {
function autosave() {
    var t = setTimeout("autosave()", 20000);
    var _id_autosave = $(txt_id_autosave).val();
    var _lang_id = $(cbo_lang_id).val();
    var _cat_id = $(cbo_cat_id).val();
    var _sub_title = $(txt_sub_title).val().ReplaceAll("'", "’");
    if (_status == '4' || _status == '6' || _status == '82') {
        if (CKEDITOR.instances[txt_title]) {
            _title = CKEDITOR.instances[txt_title].getData().ReplaceAll("'", "’");
            _summary = CKEDITOR.instances[txt_summary].getData().ReplaceAll("'", "’");
        }
    }
    else {
        var _title = $(txt_title).val().ReplaceAll("'", "’");
        var _summary = $(txt_summary).val().ReplaceAll("'", "’");
    }
    var _images = $(txt_images).val();
    var _keywords = $(txt_keywords).val().ReplaceAll("'", "’");
    var _author_name = $(txt_author_name).val();
    var _isCategorys = 0;
    if ($(chk_isCategorys).is(':checked'))
        var _isCategorys = 1;
    var _isHomePages = 0;
    if ($(chk_isHomePages).is(':checked'))
        var _isHomePages = 1;
    var _isCategoryParrent = 0;
    if ($(chk_isCategoryParrent).is(':checked'))
        var _isCategoryParrent = 1;
    var _isImages = 0;
    if ($(chk_isImages).is(':checked'))
        var _isImages = 1;
    var _isVideo = 0;
    if ($(chk_isVideo).is(':checked'))
        var _isVideo = 1;
    var _isNewsIsHot = 0;
    if ($(chk_isNewsIsHot).is(':checked'))
        var _isNewsIsHot = 1;
    var _isNewsIsFocus = 0;
    if ($(chk_isNewsIsFocus).is(':checked'))
        var _isNewsIsFocus = 1;
    var _isHistorys = 0;
    if ($(chk_isHistorys).is(':checked'))
        var _isHistorys = 1;
    var _comment = $(txt_comment).val().ReplaceAll("'", "’");
    var _tien_NB = 0;
    if (_status == '4' || _status == '6' || _status == '82' || _status == '72' || _status == '73') {
        _tien_NB = $(txt_nhuanbut).val();
    }
    var _body = '';
    //if (typeof (FCKeditorAPI) != 'undefined')
    if (CKEDITOR.instances[txt_body]) {
        _body = CKEDITOR.instances[txt_body].getData().ReplaceAll("'", "’");
    } else {
        return;
    }
    if (_body.length > 10) {        
        //alert("tienNB:'" + _tien_NB + "'");
        $.ajax({
            type: "POST",
            url: "../Service/WebService.asmx/SaveData",
            data: "{id_autosave:'" + _id_autosave + "',UserID:'" + _UserID + "',lang_id:'" + _lang_id + "',cat_id:'" + _cat_id + "',title:'" + _title + "',sub_title:'" + _sub_title + "',images:'" + _images + "',summary:'" + _summary + "',keywords:'" + _keywords + "',author_name:'" + _author_name + "',isCategorys:'" + _isCategorys + "',isHomePages:'" + _isHomePages + "',isCategoryParrent:'" + _isCategoryParrent + "',isImages:'" + _isImages + "',isNewsIsFocus:'" + _isNewsIsFocus + "',isVideo:'" + _isVideo + "',isHistorys:'" + _isHistorys + "',body:'" + _body + "',comment:'" + _comment + "',news_id:'" + _news_id + "',status:'" + _status + "',isNewsHot:'" + _isNewsIsHot + "',tienNB:'" + _tien_NB + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function(msg) {                
                $(txt_id_autosave).val(msg.d);
            },
            error: function(msg) {
                //alert("Cập nhật không thành công !!");
            }
        });
    }
}
//});
String.prototype.ReplaceAll = function(stringToFind, stringToReplace) {
    var temp = this;
    var index = temp.indexOf(stringToFind);
    while (index != -1) {
        temp = temp.replace(stringToFind, stringToReplace);
        index = temp.indexOf(stringToFind);
    }
    return temp;
}