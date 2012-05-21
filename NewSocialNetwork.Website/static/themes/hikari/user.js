jQuery(function ($) {
    /* init */
    NSN.$id('composer-tabs').tabs();

    /* User post status scripts */
    var postStatusButton = 'postStatus';
    NSN.$id(postStatusButton).click(function (evt) {
        var composerForm = jQuery(this).parents('form');
        NSN.ajaxSubmit(composerForm)
            .success(function (json) {
                NSN.callJqDlg(glbDefaultDlgId, json.Message, {
                    hasTitle: false,
                    width: 'auto',
                    buttons: [
                        {
                            text: 'Close',
                            class: 'guiBlueButton',
                            click: function () {
                                jQuery(this).dialog('destroy').remove();
                                NSN.resetForm(composerForm);
                            }
                        }
                    ]
                }).dialog('open');
            })
            .error(function (json) {
                NSN.callJqDlg(glbDefaultDlgId, json.Message).dialog('open');
            });
    });

    jQuery('.uiFeedItem').on('click', '.guiButton.post', function (evtObj) {
        var feedItem = NSN_getFeedItem(this),
            feedId = NSN_getFeedId(feedItem),
            commentText = jQuery.trim(feedItem.find('.commentEditor')[0].value);
        if (NSN.isBlank(commentText)) {
            NSN.callJqDlg(glbDefaultDlgId, 'You really are funny :). Please comment!', { hasTitle: false }).dialog('open');
            return;
        }
        NSN_postComment(feedId, commentText);
    });

    jQuery('.uiFeedItem .feedActionBlock').on('click', '.likeAction', function (evtObj) {
        var feedItem = NSN_getFeedItem(this),
            feedId = NSN_getFeedId(feedItem),
            likeButton = jQuery(this);
        jQuery.ajax({
            url: NSN.url('/ajax/likeforfeed'),
            type: 'get',
            data: { feedId: feedId },
            success: function (json) {
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
    jQuery('.uiFeedItem .feedActionBlock').on('click', '.unlikeAction', function (evtObj) {
        var feedItem = NSN_getFeedItem(this),
            feedId = NSN_getFeedId(feedItem),
            likeButton = jQuery(this);
        jQuery.ajax({
            url: NSN.url('/ajax/unlikeforfeed'),
            type: 'get',
            data: { feedId: feedId },
            success: function (json) {
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
    jQuery('.UIGrid_Album #createAlbum').click(function () {
        NSN.callJqDlg('ajax-uploader-form', NSN.url('/ajax/photoalbumuploader'), {
            title: 'Upload Photo',
            hasTitle: false,
            width: 'auto',
            buttons: {},
            open: function (evt, ui) {
                if (!jQuery(this).dialog("option", "hasTitle")) {
                    jQuery('.ui-dialog-titlebar').remove();
                }
                NSN.$id('photoalbum-fileupload').fileupload({
                    url: NSN.url('/nsn/go/to/photo/uploadsave'),
                    dataType: 'json',
                    maxFileSize: 2000000, // 20MB
                    acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
                    process: [
                        {
                            action: 'load',
                            fileTypes: /^image\/(gif|jpeg|png)$/,
                            maxFileSize: 2000000
                        },
                        {
                            action: 'resize',
                            maxWidth: 1024,
                            maxHeight: 768
                        },
                        {
                            action: 'save'
                        }
                    ],
                    done: function (e, data) {
                        jQuery.each(data.result, function (index, file) {
                            alert(file.name);
                        });
                    }
                });
                NSN.$id('photoalbum-fileupload').bind('fileuploadprogress', function (e, data) {
                    console.log(data.bitrate);
                });
            }
        }).dialog('open');
    });
    jQuery('.UIUploader_PhotoAlbum .xdlg').live('click', function () {
        NSN.$id('photoalbum-fileupload').fileupload('destroy');
        NSN.$id('ajax-uploader-form').dialog('destroy').remove();
    });
    jQuery('.UIUploader_PhotoAlbum .upload').live('click', function () {

    });
});