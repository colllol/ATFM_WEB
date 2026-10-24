$(function() {
    var availableTags = "";
    function split(val) {
        return val.split(/,\s*/);
    }
    function extractLast(term) {
        return split(term).pop();
    }

    $(_id).bind("keydown", function(event) {
        if (event.keyCode === $.ui.keyCode.TAB &&
		        $(this).data("ui-autocomplete").menu.active) {
            event.preventDefault();
        }
    })
    .autocomplete({
        minLength: 0,
        source: function(request, response) {
            var _keyword = request.term;
            var _strkey = request.term.split(', ');
            for (var i = 0; i < _strkey.length; i++) {
                if (i + 1 == _strkey.length) {
                    _keyword = _strkey[i];
                }
                if (_strkey.length == 1) {
                    _keyword = request.term;
                }
            }
            if (_keyword.length >= 2) {
                $.ajax({
                    type: "POST",
                    url: "../Service/WebService.asmx/AutoFindKeyword",
                    data: "{key:'" + _keyword + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(msg) {
                        //alert(msg);
                        availableTags = msg; //msg.d;
                        response($.ui.autocomplete.filter(
                        availableTags, extractLast(request.term)));
                    },
                    error: function(msg) {
                        alert(msg);
                    }
                });
            }
        },
        focus: function() {
            // prevent value inserted on focus
            return false;
        },
        select: function(event, ui) {
            var terms = split(this.value);
            // remove the current input
            terms.pop();
            // add the selected item
            terms.push(ui.item.value);
            // add placeholder to get the comma-and-space at the end
            terms.push("");
            this.value = terms.join(", ");
            return false;
        }
    });
});
