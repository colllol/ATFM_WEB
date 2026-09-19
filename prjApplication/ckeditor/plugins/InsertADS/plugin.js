CKEDITOR.plugins.add('InsertAds',
{
    init: function(editor) {
        var pluginName = 'InsertAds';
        editor.ui.addButton('InsertAds',
            {
                label: 'Chèn quảng cáo vào nội dung',
                icon: this.path + 'ads-16.png',
                command: pluginName,
                click: function(editor) {
                    var vHeight = 280;
                    var vWidth = 400;
                    winDef = 'scrollbars=yes,scrolling=yes,location=no,toolbar=no,height='.concat(vHeight).concat(',').concat('width=').concat(vWidth).concat(',');
                    winDef = winDef.concat('top=').concat((screen.height - vHeight) / 2).concat(',');
                    winDef = winDef.concat('left=').concat((screen.width - (vWidth)) / 2);
                    window.open('../ckeditor/plugins/InsertADS/InsertAds.aspx?editorID=' + editor.name, 'InsertAds', winDef);
                }
            });

    }
});


