jQuery(function($) {
    /* init */
    NSN.$id('composer-tabs').tabs();

    /* User post status scripts */
    var postStatusButton = 'postStatus';
    NSN.$id(postStatusButton).click(function(evt) {
        var composerForm = jQuery(this).parents('form');
        NSN.ajaxSubmit(composerForm)
            .success(function(json) {
                NSN.callJqDlg(glbDefaultDlgId, json.Message, {
                    buttons: {
                        'Close': function() {
                            jQuery(this).dialog('destroy').remove();
                            NSN.resetForm(composerForm);
                        }
                    }
                })
                .dialog('open');
            })
            .error(function(json) {
                NSN.callJqDlg(glbDefaultDlgId, json.Message).dialog('open');
            });
    });

});