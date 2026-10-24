CKEDITOR.plugins.add('InsertAdsContent', {
    init: function(editor) {
        editor.addCommand('callcommandinsertAds', {
            exec: function(editor) {
                var vHeight = 280;
                var vWidth = 400;
                winDef = 'scrollbars=yes,scrolling=yes,location=no,toolbar=no,height='.concat(vHeight).concat(',').concat('width=').concat(vWidth).concat(',');
                winDef = winDef.concat('top=').concat((screen.height - vHeight) / 2).concat(',');
                winDef = winDef.concat('left=').concat((screen.width - (vWidth)) / 2);
                window.open('../ckeditor/plugins/InsertAdsContent/InsertAdsContent.aspx?editorID=' + editor.name, 'InsertAdsContent', winDef);
            }
        });

        if (editor.addMenuItem) {
            editor.addMenuGroup('testgroup3');
            editor.addMenuItem('testitem3', {
                label: 'Chèn quảng cáo',
                command: 'callcommandinsertAds',
                icon: this.path + 'ads-16.png',
                group: 'testgroup3'

            });
        }
        if (editor.contextMenu) {
            editor.contextMenu.addListener(function(element, selection) {
                return { testitem3: CKEDITOR.TRISTATE_ON };
            });
        }

    }
});