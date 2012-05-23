jQuery(window).load(function() {
    setInterval(ajaxCount, 1000);
});

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

    jQuery('.uiFeedItem').on('click', '.guiButton.cancel', function (evtObj) {
        var commentBox = jQuery(this).parents('.commentBox'),
            maskCommentBox = commentBox.parent().next();
        commentBox.hide();
        maskCommentBox.show();
    });

    jQuery('.uiFeedItem').on('mousedown', '.maskText', function (evtObj) {
        var maskCommentBox = jQuery(this).parent(),
            commentBox = jQuery(maskCommentBox.prev().children()[1]),
            commentEditor = commentBox.find('.commentEditor');
        maskCommentBox.hide();
        commentBox.show();
        commentEditor.focus();
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
                    done: function (e, data) {
                        var result = data.result,
                            img = '<img src="' + NSN.staticUploadedImage('photos/' + result[0].FileName) + '" alt="" />',
                            item = '<div class="uploadedItem">' + img + '</div>';
                        jQuery(this).parents('.uiControls').find('.uploadedQueue').append(item);
                    }
                });
                NSN.$id('photoalbum-fileupload').fileupload('option', {
                    maxFileSize: 2000000, // 2MB
                    acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
                    process: [
                        {
                            action: 'load',
                            filetypes: /^image\/(gif|jpeg|png)$/,
                            maxfilesize: 2000000
                        },
                        {
                            action: 'resize',
                            maxwidth: 1024,
                            maxheight: 768
                        },
                        {
                            action: 'save'
                        }
                    ]
                });
                NSN.$id('photoalbum-fileupload').bind('fileuploadprogress', function (e, data) {
                    console.log(data.bitrate);
                });
            }
        }).dialog('open');
    });
    jQuery('.UIUploader_PhotoAlbum .xdlg').live('click', function () {
        jQuery.ajax({
            url: NSN.url('/nsn/go/to/photo/cancelupload'),
            type: 'post',
            success: function (json) {
                NSN.createJqDlg(glbDefaultDlgId, json.Message, {
                    hasTitle: false,
                    width: 'auto'
                }).dialog('open');
            },
            complete: function () {
                NSN.$id('photoalbum-fileupload').fileupload('destroy');
                NSN.$id('ajax-uploader-form').dialog('destroy').remove();
            }
        });
    });
    jQuery('.UIUploader_PhotoAlbum .upload').live('click', function () {
        jQuery.ajax({
            url: NSN.url('/nsn/go/to/photoalbum/saveuploadedphotos'),
            type: 'post',
            data: NSN.$id('photoalbum-uploader-form').serialize(),
            success: function (json) {
                if (json.Status == 1) {
                    NSN.$id('ajax-uploader-form').dialog('destroy').remove();
                }
                NSN.createJqDlg(glbDefaultDlgId, json.Message, {
                    buttons: [
                        {
                            text: 'Go to album',
                            class: 'guiBlueButton',
                            click: function () {
                                document.location = json.ReturnedPath;
                            }
                        }
                    ]
                }).dialog('open');
            },
            error: function (data) {
                NSN.createJqDlg(glbDefaultDlgId, data);
            }
        });
    });

    jQuery('.UIContent_UserProfile .profileActions').on('click', '.guiButton.addFriend', function (evtObj) {
        var profileItem = NSN_getProfileItem(this),
            profileId = NSN_getProfileId(profileItem);
        NSN.callJqDlg('request-friend-form', NSN.url('/nsn/go/to/friendrequest/add'),
        {
            type: 'post',
            data: { friendUserId: profileId },
        },
        {
            title: 'Request Friend',
            buttons: {}
        }).dialog('open');
    });

    jQuery('.UIForm_RequestFriend .requestActions .cancelBtn').live('click', function (evtObj) {
        NSN.$id('request-friend-form').dialog('destroy').remove();
    });

    jQuery('.UIForm_RequestFriend .requestActions .requestBtn').live('click', function (evtObj) {
        var friendUserId = jQuery('.UIForm_RequestFriend input[name=friendUserId]').val(),
            message = jQuery('.UIForm_RequestFriend textarea[name=message]').val();
        jQuery.ajax({
            url: NSN.url('/nsn/go/to/friendrequest/addsave'),
            type: 'post',
            data: { friendUserId: friendUserId, message: escape(message) },
            success: function(json) {
                if (json.Status == 1) {
                    NSN.$id('request-friend-form').dialog('destroy').remove();
                    NSN.createJqDlg(glbDefaultDlgId, json.Message, {
                        buttons: [
                            {
                                text: 'Close',
                                class: 'guiBlueButton',
                                click: function() {
                                    jQuery('.UIContent_UserProfile .requestFriendAction')
                                        .html('<div class="guiButton guiRedButton confirmingFriend">+1 Friend Request Sent</div>');
                                    jQuery(this).dialog('destroy').remove();
                                }
                            }
                        ]
                    }).dialog('open');
                }
                else {
                    NSN.createJqDlg(glbDefaultDlgId, json.Message).dialog('open');
                }
            }
        });
    });

    NSN.$id('nsnRequestsJewel').click(function(evt) {
        jQuery.ajax({
            url: NSN.url('/nsn/go/to/jewels/showfriendrequests'),
            type: 'post',
            success: function(result) {
                NSN.createJqDlg('friend-request-stream', result, {
                    hasTitle: false,
                    width: 'auto'
                }).dialog('open');
            }
        });
        evt.preventDefault();
    });

});

function ajaxCount() {
    jQuery.ajax({
        url: NSN.url('/nsn/go/to/usercount/countpending'),
        type: 'post',
        success: function(countOf) {
            var mailNew = countOf.MailNew,
                commentPedding = countOf.CommentPending,
                friendRequest = countOf.FriendRequest;
            var jewelRequests = NSN.$id('nsnRequestsJewel'),
                jewelMessages = NSN.$id('nsnMessagesJewel'),
                jewelNotifs = NSN.$id('nsnNotificationsJewel');
            jewelRequests.find('.jewelCount').html((friendRequest > 0) ? ('<span>' + friendRequest + '</span>') : (''));
            jewelMessages.find('.jewelCount').html((mailNew > 0) ? ('<span>' + mailNew + '</span>') : (''));
            jewelNotifs.find('.jewelCount').html((commentPedding > 0) ? ('<span>' + commentPedding + '</span>') : (''));
        }
    });
}