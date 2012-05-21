jQuery(function($) {
    /* init */
    NSN.$id('composer-tabs').tabs();
    $('textarea.autogrow').autogrow();

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
        var feedItem = NSN_getFeedItem(this),
            feedId = NSN_getFeedId(feedItem),
            commentText = jQuery.trim(feedItem.find('.commentEditor')[0].value);
        if (NSN.isBlank(commentText)) {
            NSN.callJqDlg(glbDefaultDlgId, 'You really are funny :). Please comment!', { hasTitle: false }).dialog('open');
            return;
        }
        NSN_postComment(feedId, commentText);
    });

    jQuery('.uiFeedItem .feedActionBlock').on('click', '.likeAction', function(evtObj) {
        var feedItem = NSN_getFeedItem(this),
            feedId = NSN_getFeedId(feedItem),
            likeButton = jQuery(this);
        jQuery.ajax({
            url: NSN.url('/ajax/likeforfeed'),
            type: 'get',
            data: { feedId: feedId },
            success: function(json) {
                if (json.Status == 1) {
                    likeButton.find('.saving_message').hide();
                    likeButton.find('.default_message').show();
                    likeButton.removeClass('likeAction');
                    likeButton.addClass('unlikeAction');
                }
                else {
                    NSN.createJqDlg(glbDefaultDlgId,
                        '<div class="nsn-popup-msg ui-state-error">' + json.Message + '</div>')
                        .dialog('open');
                }
            }
        });
    });
    jQuery('.uiFeedItem .feedActionBlock').on('click', '.unlikeAction', function(evtObj) {
        var feedItem = NSN_getFeedItem(this),
            feedId = NSN_getFeedId(feedItem),
            likeButton = jQuery(this);
        jQuery.ajax({
            url: NSN.url('/ajax/unlikeforfeed'),
            type: 'get',
            data: { feedId: feedId },
            success: function(json) {
                if (json.Status == 1) {
                    likeButton.find('.default_message').hide();
                    likeButton.find('.saving_message').show();
                    likeButton.removeClass('unlikeAction');
                    likeButton.addClass('likeAction');
                }
                else {
                    NSN.createJqDlg(glbDefaultDlgId,
                        '<div class="nsn-popup-msg ui-state-error">' + json.Message + '</div>')
                        .dialog('open');
                }
            }
        });
    });
});