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
                    hasTitle: false,
                    width: 'auto',
                    buttons: [
                        {
                            text: 'Close',
                            class: 'guiBlueButton',
                            click: function() {
                                jQuery(this).dialog('destroy').remove();
                                NSN.resetForm(composerForm);
                            }
                        }
                    ]
                }).dialog('open');
            })
            .error(function(json) {
                NSN.callJqDlg(glbDefaultDlgId, json.Message).dialog('open');
            });
    });

    jQuery('.uiFeedItem').on('click', '.guiButton.post', function(evtObj) {
        var feedItem = jQuery(this).parents('.uiFeedItem'),
            feedId = parseInt(feedItem.attr('id').slice(7), 10),
            commentText = jQuery.trim(feedItem.find('.commentEditor')[0].value);
        if (NSN.isBlank(commentText)) {
            NSN.callJqDlg(glbDefaultDlgId, 'You really are funny :). Please comment!', { hasTitle: false }).dialog('open');
            return;
        }
        NSN_postComment(feedId, commentText);
    });
});