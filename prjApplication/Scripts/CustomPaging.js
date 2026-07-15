(function ($) {
    var defaultSetting = {};
    
    defaultSetting = {
        pageSize: 600,
        numberViewPage: 7,
        cssButton: 'paginate_button',
        cssPaging: 'pagination',
        onClickButton: '',
        publics: {},
    }
    function init(that, option) {
        if ($(that).attr('data-pageSize') == null) $(that).attr('data-pageSize', option.pageSize)
        if ($(that).attr('data-pageIndex') == null) $(that).attr('data-pageIndex', 1);
        if ($(that).attr('data-total') == null || $(that).attr('data-total') == '0') {
            $('#divPaging').remove();
            $(that).attr('data-pageIndex', '1');
            $(that).after($('<div id="divPaging">0 record.</div>'));
            return;
        }
            
        $('#divPaging').remove();

        var $divPaging = $('<div id="divPaging"></div>');
        var $ul = $('<ul id="pagingCus" class="pagination">'),
            $table = $(that),
            $lis = {},
            $cssLi = 'paginate_button',
            $labelBeforePaging = '',
            $totalRecord = parseInt($table.attr('data-total')),
            $pageIndex = $($table).attr('data-pageIndex') == null ? 1 : parseInt($($table).attr('data-PageIndex')),
            $pageSixe = option.pageSize,
            $numberViewPage = option.numberViewPage,
            $TB = $numberViewPage % 2 == 0 ? $numberViewPage / 2 : ($numberViewPage - 1) / 2,
            $start = $pageIndex - $TB,
            $fnish = $pageIndex + $TB,
            $nPage = parseInt($totalRecord % $pageSixe == 0 ? $totalRecord / $pageSixe : ($totalRecord / $pageSixe) + 1);        
        while ($start < 1) { $start++; $fnish++; }
        while ($fnish > $nPage) { $fnish--; $start--; }
        $start = $start < 1 ? 1 : $start;
        $fnish = $fnish < 1 ? 1 : $fnish;
        $labelBeforePaging = $('<li style="float:left;margin-top: 15px;padding-right: 10px;">Page ' + ($pageIndex) + '/' + $nPage + '(' + $totalRecord + ' records)</li>');
        $ul.append($labelBeforePaging);
        $ul.append('<li style="cursor: pointer;"  onclick="$(\'#' + $($(that)).prop('id') + '\').attr(\'data-pageIndex\', 1); $(\'#'+$($(that)).prop('id') +'\').attr(\'data-total\',\'0\'); ' + option.onClickButton + '()' + '" data-pageIndex="1" class="paginate_button previous" aria-controls="dynamic-table"><a><<</a></li>');
        if (($fnish - $start) >= $nPage)
            $fnish = $start + $numberViewPage;
        for (var i = $start; i <= $fnish; i++) {
            if (i == $pageIndex) $cssLi = "paginate_button active";
            else $cssLi = 'paginate_button';
            var $li = $('<li style="cursor: pointer;" onclick="$(\'#' + $($(that)).prop('id') + '\').attr(\'data-pageIndex\', ' + i + '); $(\'#'+$($(that)).prop('id') +'\').attr(\'data-total\',\'0\'); ' + option.onClickButton + '()' + '" data-pageIndex="' + i + '" class="' + $cssLi + '" aria-controls="dynamic-table"><a>' + i + '</a></li>');
            $ul.append($li);
        }
        $ul.append('<li style="cursor: pointer;" onclick="$(\'#' + $($(that)).prop('id') + '\').attr(\'data-pageIndex\', ' + $nPage + '); $(\'#'+$($(that)).prop('id') +'\').attr(\'data-total\',\'0\'); ' + option.onClickButton + '()' + '" data-pageIndex="' + $nPage + '" class="paginate_button next" aria-controls="dynamic-table"><a>>></a></li>');
        $divPaging.append($ul);
        $table.after($divPaging);
        $table.attr('data-pageIndex', $pageIndex);
        var w = $(window);
        var row = $table.find('tr').eq(0);
        if (row.length) {
            w.scrollTop(row.offset().top - (w.height() / 2));
        }
    }
    publics = {

    }
    $.fn.paging = function (_options, _callback) {
        if ($.type(_options) === 'string' && publics[_options]) {
            return publics[_options].call(this, _callback);
        }
        return this.each(function () {
            var options = $.extend(true, {}, defaultSetting, _options);
            init(this, options);
        });
    };

}(jQuery));