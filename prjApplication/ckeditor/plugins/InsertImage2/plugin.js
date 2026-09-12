CKEDITOR.plugins.add('InsertImage2', {
    init: function(editor) {
        editor.addCommand('callcommandinsert', {
            exec: function(editor) {
                var vHeight = 650;
                var vWidth = 980;
                winDef = 'scrollbars=yes,scrolling=yes,location=no,toolbar=no,height='.concat(vHeight).concat(',').concat('width=').concat(vWidth).concat(',');
                winDef = winDef.concat('top=').concat((screen.height - vHeight) / 2).concat(',');
                winDef = winDef.concat('left=').concat((screen.width - (vWidth)) / 2);
                window.open('../ckeditor/plugins/InsertImage2/InsertImages.aspx?editorID=' + editor.name, 'InsertImage2', winDef);
            }
        });

        if (editor.addMenuItem) {
            editor.addMenuGroup('testgroup2');
            editor.addMenuItem('testitem2', {
                label: 'Chèn ảnh',
                command: 'callcommandinsert',
                icon: this.path + 'icons_imagegallery.png',
                group: 'testgroup2'

            });
        }
        if (editor.contextMenu) {
            editor.contextMenu.addListener(function(element, selection) {
                return { testitem2: CKEDITOR.TRISTATE_ON };
            });
        }

    }
});