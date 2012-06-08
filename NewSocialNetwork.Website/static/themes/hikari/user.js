jQuery(window).load(function () {
    setInterval(ajaxCount, 5000);
    setInterval(ajaxStream, 5000);
});

function ajaxCount() {
    jQuery.ajax({
        url: NSN.requestUrl('usercount/countpending'),
        type: 'post',
        success: function (countOf) {
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
function ajaxStream() {
    jQuery.ajax({
        url: NSN.requestUrl('feed/newestfeeds'),
        type: 'post',
        data: { feedId: lastFeedId },
        dataType: 'html',
        success: function(result) {
            if (!NSN.isBlank(result)) {
                var html = jQuery(result),
                    tags = jQuery('li.uiStreamStory', result),
                    streamContainer = jQuery('.UIStream .uiStreamHomePage');
                //lastFeedId += tags.length;
                if (streamContainer.length == 0) {
                    jQuery('.UIStream .no-posts').remove();
                    jQuery('.UIStream').prepend('<ul class="uiList uiStream uiStreamHomePage"></ul>')
                    streamContainer = jQuery('.UIStream .uiStreamHomePage');
                }
                tags.each(function(idx, val) {
                    var tagId = jQuery(this).attr('id'),
                        tagClass = jQuery(this).attr('class'),
                        feedId = tagId.slice(6);
                    streamContainer.prepend('<li id="' + tagId + '" class="' + tagClass + '">' + jQuery(this).html() + '</li>');
                    lastFeedId = feedId;
                });
            }
        }
    });
}

jQuery(function ($) {
    /* init */
    NSN.$id('composer-tabs').tabs({ disabled: [1] });
    NSN.$id('navAccount').click(navAccountClickHandler);
    NSN.$id('userNavigation').mouseleave(userNavigationMouseOutHandler);
    NSN.$id('logout').click(logOutHandler);

    /* User post status scripts */
    var postStatusButton = 'postStatus',
        postLinkButton = 'postLink';
    NSN.$id(postStatusButton).click(function (evt) {
        var composerForm = $(this).parents('form');
        NSN.ajaxSubmit(composerForm)
            .success(function (result) {
                if (result && result.length > 0) {
                    NSN.resetForm(composerForm);
                    $('.UIFeeds_Content').prepend(result);
                }
            })
            .error(function (result) {
                NSN_alertError(result.responseText);
            });
    });
    NSN.$id(postLinkButton).click(function (evt) {
        var composer = NSN.$id('composer-addlink'),
            uid = composer.find('input[name=uid]').val(),
            inputText = composer.find('textarea[name=inputText]'),
            inputLink = composer.find('input[name=inputLink]'),
            imageUrl = (thumbPagerListImages[thumbPagerCurrent - 1] ? thumbPagerListImages[thumbPagerCurrent - 1].src : ""),
            title = composer.find('.UIShareStage_Title').text(),
            description = composer.find('.UIShareStage_Summary').text();
        $.ajax({
            url: NSN.requestUrl('link/post'),
            type: 'post',
            data: { uid: uid, inputText: escape(inputText.val()), inputLink: inputLink.val(), imageUrl: imageUrl, title: escape(title), description: escape(description) },
            success: function (result) {
                if (result && result.length > 0) {
                    inputText.val('');
                    inputLink.val('');
                    $('.linkShareStage .composerCloseShare').click();
                    $('.UIFeeds_Content').prepend(result);
                }
            },
            error: function(result) {
                NSN_alertError(result.responseText);
            }
        });
    });

    $('#composer-addlink .addBtn').click(function (evt) {
        var me = $(this),
            addLinkBox = me.parents('.uiAddLink'),
            uiShareStage = addLinkBox.siblings('.linkShareStage').find('.UIShareStage'),
            inputLink = addLinkBox.find('input[name=inputLink]').val();
        if (NSN.isUrl(inputLink)) {
            $.ajax({
                url: NSN.url('/links/getremote'),
                type: 'post',
                data: { remoteUrl: inputLink },
                success: function (json) {
                    if (json != null && !NSN.isBlank(json.Url)) {
                        var thumbPager = uiShareStage.find('.UIThumbPager_Thumbs');
                        thumbPager.html('');
                        $.each(json.ImageUrls, function (idx, val) {
                            thumbPager.append($('<img class="img" style="display:none" />').attr("src", val));
                        });
                        uiShareStage.find('.UIShareStage_Title').html($('<span class="inline-edit"></span>').html(json.Title));
                        uiShareStage.find('.UIShareStage_Subtitle').html(json.Url);
                        uiShareStage.find('.UIShareStage_Summary').html($('<p class="UIShareStage_BottomMargin inline-edit"></p>').html(json.Description));
                        uiShareStage.find('.inline-edit').editInPlace({
                            callback: function (unused, enteredText) { return enteredText; }
                        });
                        addLinkBox.hide();
                        uiShareStage.parents('.linkShareStage').slideDown(100);
                        thumbPager.siblings('.UIThumbPager_Loader').hide();

                        var pageNumberControl = uiShareStage.find('.UIThumbPagerControl_PageNumber');
                        thumbPagerListImages = thumbPager.find('.img');
                        thumbPagerTotal = json.ImageUrls.length;
                        if (thumbPagerTotal > 0) {
                            $(thumbPagerListImages[0]).show();
                            thumbPagerCurrent = 1;
                        }
                        thumbPagerNextBtn = uiShareStage.find('.UIThumbPagerControl_Button_Right');
                        thumbPagerPrevBtn = uiShareStage.find('.UIThumbPagerControl_Button_Left');
                        if (thumbPagerTotal > 1) {
                            thumbPagerNextBtn.click(LinkShareStage_nextThumbPager);
                            thumbPagerPrevBtn.click(LinkShareStage_prevThumbPager);
                            LinkShareStage_displayPageCounter(thumbPagerTotal, thumbPagerCurrent);
                        }
                        else {
                            LinkShareStage_displayPageCounter(null);
                        }
                    }
                }
            });
        }
    });
    var thumbPagerTotal = 0,
        thumbPagerCurrent = 0,
        thumbPagerNextBtn = null,
        thumbPagerPrevBtn = null,
        thumbPagerListImages = null;
    function LinkShareStage_nextThumbPager(evt) {
        if (thumbPagerTotal <= 0 || thumbPagerCurrent == 0 || thumbPagerCurrent == thumbPagerTotal) {
            return;
        }
        if (thumbPagerCurrent < thumbPagerTotal) {
            thumbPagerCurrent++;
            $.each(thumbPagerListImages, function (idx, val) {
                if ((idx + 1) == thumbPagerCurrent) {
                    $(this).show();
                }
                else {
                    $(this).hide();
                }
            });
            LinkShareStage_displayPageCounter(thumbPagerTotal, thumbPagerCurrent);
        }
    }
    function LinkShareStage_prevThumbPager(evt) {
        if (thumbPagerTotal <= 0 || thumbPagerCurrent == 0 || thumbPagerCurrent == 1) {
            return;
        }
        if (thumbPagerCurrent > 1) {
            thumbPagerCurrent--;
            $.each(thumbPagerListImages, function (idx, val) {
                if ((idx + 1) == thumbPagerCurrent) {
                    $(this).show();
                }
                else {
                    $(this).hide();
                }
            });
        }
        LinkShareStage_displayPageCounter(thumbPagerTotal, thumbPagerCurrent);
    }
    function LinkShareStage_displayPageCounter(total, current) {
        var uiStageShare = $('.UIShareStage');
        if (!total || total == 0) {
            uiStageShare.find('.UIThumbPagerControl').hide();
            uiStageShare.find('.UIShareStage_ThumbPager').hide();
            return;
        }
        else {
            uiStageShare.find('.UIThumbPagerControl').show();
            uiStageShare.find('.UIShareStage_ThumbPager').show();
        }
        var uiCurrent = uiStageShare.find('.UIThumbPagerControl_PageNumber_Current'),
            uiTotal = uiStageShare.find('.UIThumbPagerControl_PageNumber_Total');
        uiCurrent.text(current);
        uiTotal.text(total);
    }
    $('.linkShareStage .composerCloseShare').click(function (evt) {
        var linkShareStage = $(this).parents('.linkShareStage'),
            addLinkBox = linkShareStage.siblings('.uiAddLink');
        linkShareStage.hide();
        addLinkBox.slideDown(100);
    });

    $('.uiFeedItem .guiButton.post').live('click', function (evtObj) {
        var feedItem = NSN_getFeedItem(this),
            feedId = NSN_getFeedId(feedItem),
            commentText = $.trim(feedItem.find('.commentEditor')[0].value);
        if (NSN.isBlank(commentText)) {
            NSN.callJqDlg(glbDefaultDlgId, 'You really are funny :). Please comment!', { hasTitle: false }).dialog('open');
            return;
        }
        NSN_postComment("on_feed", feedId, commentText, successCallback);
        function successCallback(data) {
            if (data != null && data.length > 0) {
                var editor = NSN.$id('editor-update-' + feedId),
                    cancelBtn = editor.parents('.commentBox').find('.guiButton.cancel');
                editor.val('');
                NSN.$id('update-' + feedId).find('.uiListTreeInner').append(data);
                cancelBtn.click();
            }
            else {
                NSN_alertError('Cannot process your comment. Please try again!');
            }
        }
    });

    $('.uiFeedItem .guiButton.cancel').live('click', function (evtObj) {
        var commentBox = $(this).parents('.commentBox'),
            maskCommentBox = commentBox.parent().next();
        commentBox.hide();
        maskCommentBox.show();
    });

    $('.uiFeedItem .maskText').live('mousedown', function (evtObj) {
        var maskCommentBox = $(this).parent(),
            commentBox = $(maskCommentBox.prev().children()[1]),
            commentEditor = commentBox.find('.commentEditor');
        maskCommentBox.hide();
        commentBox.show();
        commentEditor.focus();
    });

    $('.uiFeedItem .feedActionBlock .likeAction').live('click', function (evtObj) {
        var feedItem = NSN_getFeedItem(this),
            feedId = NSN_getFeedId(feedItem),
            likeButton = $(this);
        $.ajax({
            url: NSN.url('/ajax/like'),
            type: 'get',
            data: { where: "on_feed", id: feedId },
            success: function (json) {
                if (json.Status == 1) {
                    likeButton.find('.saving_message').hide();
                    likeButton.find('.default_message').show();
                    likeButton.removeClass('likeAction');
                    likeButton.addClass('unlikeAction');
                    displayTotalLiked_InFeed(likeButton, feedId);
                }
                else {
                    NSN_alertError(json.Message);
                }
            }
        });
    });
    $('.uiFeedItem .feedActionBlock .unlikeAction').live('click', function (evtObj) {
        var feedItem = NSN_getFeedItem(this),
            feedId = NSN_getFeedId(feedItem),
            likeButton = $(this);
        $.ajax({
            url: NSN.url('/ajax/unlike'),
            type: 'get',
            data: { where: "on_feed", id: feedId },
            success: function (json) {
                if (json.Status == 1) {
                    likeButton.find('.default_message').hide();
                    likeButton.find('.saving_message').show();
                    likeButton.removeClass('unlikeAction');
                    likeButton.addClass('likeAction');
                    displayTotalLiked_InFeed(likeButton, feedId);
                }
                else {
                    NSN_alertError(json.Message);
                }
            }
        });
    });
    function displayTotalLiked_InFeed(self, feedId) {
        $.ajax({
            url: NSN.requestUrl('like/totallikeonfeed'),
            data: { feedId: feedId },
            success: function (json) {
                if (json.Status == 1) {
                    var totalLikeSpan = self.siblings('span.literal'),
                        totalLike = json.Message;
                    if (!totalLike || totalLike == 0) {
                        totalLikeSpan.html('');
                    }
                    else {
                        totalLikeSpan.html('(' + totalLike + ' people liked)');
                    }
                }
            }
        });
    }
    $('.uiFeedItem .feedRemoverBtn').live('click', function (evt) {
        evt.preventDefault();
        var feedItem = NSN_getFeedItem(this);
        NSN_alertConfirmRemove('Are you sure remove this feed?', 'Remove Feed Confirmation', function () {
            var feedId = NSN_getFeedId(feedItem);
            ajaxRemoveFeed(feedId, successCallback);
            $(this).dialog('destroy').remove();
        });
        function successCallback(json) {
            if (json.Status == 1) {
                feedItem.remove();
            }
            else {
                NSN_alertError(json.Message);
            }
        }
    });
    $('.uiFeedItem .uiCommentItem .commentActions .delete').live('click', function (evt) {
        evt.preventDefault();
        var feedItem = NSN_getFeedItem(this),
            commentItem = $(this).parents('.uiCommentItem');
        NSN_alertConfirmRemove('Delete this comment? (Cannot undo)', 'Remove Comment Confirmation', function () {
            var feedId = NSN_getFeedId(feedItem),
                commentId = parseInt(commentItem.attr('id').slice(8), 10);
            ajaxRemoveComment(feedId, commentId, successCallback);
            $(this).dialog('destroy').remove();
        });
        function successCallback(json) {
            if (json.Status == 1) {
                commentItem.remove();
            }
            else {
                NSN_alertError(json.Message);
            }
        }
    });
    $('.UIGrid_Photo #addPhoto').click(function () {
        var albumId = parseInt($(this).parents('.UIGrid_Photo').attr('id').slice(12), 10);
        NSN.callJqDlg('ajax-uploader-form', NSN.url('/ajax/photouploader'),
        {
            data: { albumId: albumId }
        },
        {
            title: 'Upload Photo',
            hasTitle: false,
            width: 'auto',
            buttons: {},
            open: function (evt, ui) {
                if (!$(this).dialog("option", "hasTitle")) {
                    $('.ui-dialog-titlebar').remove();
                }
                NSN.$id('photo-fileupload').fileupload({
                    url: NSN.requestUrl('photo/uploadsave'),
                    dataType: 'json',
                    done: function (e, data) {
                        var result = data.result,
                            img = '<img src="' + NSN.staticUploadedImage('photos/' + result[0].FileName) + '" alt="" />',
                            item = '<div class="uploadedItem">' + img + '</div>';
                        $(this).parents('.uiControls').find('.uploadedQueue').append(item);
                    }
                });
                NSN.$id('photo-fileupload').fileupload('option', {
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
                NSN.$id('photo-fileupload').bind('fileuploadprogress', function (e, data) {
                    console.log(data.bitrate);
                });
            }
        }).dialog('open');
    });
    $('.UIGrid_Photo #removeAlbum').click(function () {
        var albumItem = $(this).parents('.UIGrid_Photo'),
            albumId = parseInt(albumItem.attr('id').slice(12), 10);
        function removeAlbum(albumItem) {
            albumItem.remove();
        }
        NSN_alertConfirmRemove('Delete this album? (Carefully! Cannot undo)', 'Remove Album Confirmation',
            function () {
                $.ajax({
                    url: NSN.requestUrl('photoalbum/remove'),
                    type: 'post',
                    async: false,
                    data: { albumId: albumId },
                    success: function (json) {
                        if (json.Status == 1) {
                            removeAlbum(albumItem);
                            NSN.createJqDlg(glbDefaultDlgId, json.Message, {
                                buttons: [
                                    {
                                        'text': 'Your Albums',
                                        'class': 'guiBlueButton',
                                        'click': function () {
                                            document.location = json.ReturnedPath;
                                        }
                                    }
                                ]
                            }).dialog('open');
                        }
                        else {
                            NSN_alertError(json.Message);
                        }
                    }
                });
                $(this).dialog('destroy').remove();
            });
    });
    $('.UIUploader_Photo .xdlg').live('click', function () {
        $.ajax({
            url: NSN.requestUrl('photo/cancelupload'),
            type: 'post',
            success: function (json) {
                NSN.createJqDlg(glbDefaultDlgId, json.Message, {
                    hasTitle: false,
                    width: 'auto'
                }).dialog('open');
            },
            complete: function () {
                NSN.$id('photo-fileupload').fileupload('destroy');
                NSN.$id('ajax-uploader-form').dialog('destroy').remove();
            }
        });
    });
    $('.UIUploader_Photo .upload').live('click', function () {
        $.ajax({
            url: NSN.requestUrl('photo/saveuploadedphotos'),
            type: 'post',
            data: NSN.$id('photo-uploader-form').serialize(),
            success: function (json) {
                if (json.Status == 1) {
                    NSN.$id('ajax-uploader-form').dialog('destroy').remove();
                    document.location = json.ReturnedPath;
                }
                else {
                    NSN_alertError(json.Message);
                }
            },
            error: function (data) {
                NSN_alertError(data.Message);
            }
        });
    });

    $('.UIGrid_Album #createAlbum').click(function () {
        NSN.callJqDlg('ajax-uploader-form', NSN.url('/ajax/photoalbumuploader'), {
            title: 'Upload Photo',
            hasTitle: false,
            width: 'auto',
            buttons: {},
            open: function (evt, ui) {
                if (!$(this).dialog("option", "hasTitle")) {
                    $('.ui-dialog-titlebar').remove();
                }
                NSN.$id('photoalbum-fileupload').fileupload({
                    url: NSN.requestUrl('photo/uploadsave'),
                    dataType: 'json',
                    done: function (e, data) {
                        var result = data.result,
                            img = '<img src="' + NSN.staticUploadedImage('photos/' + result[0].FileName) + '" alt="" />',
                            item = '<div class="uploadedItem">' + img + '</div>';
                        $(this).parents('.uiControls').find('.uploadedQueue').append(item);
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
    $('.UIUploader_PhotoAlbum .xdlg').live('click', function () {
        $.ajax({
            url: NSN.requestUrl('photo/cancelupload'),
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
    $('.UIUploader_PhotoAlbum .upload').live('click', function () {
        $.ajax({
            url: NSN.requestUrl('photoalbum/saveuploadedphotos'),
            type: 'post',
            data: NSN.$id('photoalbum-uploader-form').serialize(),
            success: function (json) {
                if (json.Status == 1) {
                    NSN.$id('ajax-uploader-form').dialog('destroy').remove();
                    NSN.createJqDlg(glbDefaultDlgId, json.Message, {
                        buttons: [
                            {
                                'text': 'Go To Album',
                                'class': 'guiBlueButton',
                                'click': function () {
                                    document.location = json.ReturnedPath;
                                }
                            }
                        ]
                    }).dialog('open');
                }
                else {
                    NSN_alertError(json.Message);
                }
            },
            error: function (data) {
                NSN_alertError(data.Message);
            }
        });
    });
    $('.UIGrid_Photo').on('click', '.UIPhotoItem .removePhoto', function (evt) {
        evt.preventDefault();
        var photoItem = $(this).parents('.UIPhotoItem');
        function deletePhoto(photoItem) {
            var photoId = parseInt(photoItem.attr('id').slice(6), 10),
                albumId = parseInt(photoItem.parents('.UIGrid_Photo').attr('id').slice(12), 10);
            $.ajax({
                url: NSN.requestUrl('photo/remove'),
                type: 'post',
                async: false,
                data: { albumId: albumId, photoId: photoId },
                success: function (json) {
                    if (json.Status == 1) {
                        photoItem.remove();
                    }
                }
            });
        }
        NSN_alertConfirmRemove('Delete this photo? (Cannot undo)', 'Remove Photo Confirmation', function () {
            deletePhoto(photoItem);
            $(this).dialog('destroy').remove();
        });
    });

    function disableHtmlScrollbar(ok) {
        if (ok) {
            $('body').addClass('_photoZoom');
        }
        else {
            $('body').removeClass('_photoZoom');
        }
    }

    function zoomPhotoItem(photoId) {
        $.ajax({
            url: NSN.requestUrl('photo/showzoom'),
            type: 'post',
            data: { photoId: photoId },
            success: function (result) {
                disableHtmlScrollbar(true);
                var dlg = NSN.createJqDlg('ajax-photozoom', result, {
                    hasTitle: false,
                    width: 'auto',
                    closeOnEscape: true,
                    close: function () {
                        NSN.$id('ajax-photozoom').dialog('destroy').remove();
                        disableHtmlScrollbar(false);
                    },
                    buttons: {}
                }),
                    dlgParent = dlg.parent();
                dlg.addClass('_photoZoomInner');
                dlgParent.addClass('_photoZoom');
                dlg.find('.closeTheater').click(function (evt) {
                    dlg.dialog('close');
                    evt.preventDefault();
                });
                dlg.on('keydown', 'input[name=commentText]', function (evt) {
                    if (evt.keyCode == 13) {
                        var me = $(this),
                            photoId = me.siblings('.hidden_elem').find('input[name=pid]').val(),
                            c = me.val();
                        NSN_postCommentOnPhoto(dlg, photoId, c);
                    }
                });
                dlg.delegate('.commentRemoverBtn', 'click', function (evt) {
                    evt.preventDefault();
                    var commentItem = $(this).parents('.commentItem');
                    function deleteCommentOnPhoto(commentItem, photoId) {
                        var commentId = parseInt(commentItem.attr('id').slice(8), 10);
                        $.ajax({
                            url: NSN.requestUrl('comment/remove'),
                            type: 'post',
                            async: false,
                            data: { typeId: 'photo', itemId: photoId, commentId: commentId },
                            success: function (json) {
                                if (json.Status == 1) {
                                    commentItem.remove();
                                }
                                else {
                                    NSN_alertError(json.Message);
                                }
                            }
                        });
                    }
                    NSN_alertConfirmRemove('Delete this comment? (Cannot undo)', 'Remove Comment Confirmation', function () {
                        deleteCommentOnPhoto(commentItem, photoId);
                        $(this).dialog('destroy').remove();
                    });
                });
                function likeForPhotoHandler(evt) {
                    var likeBtn = $(this),
                        photoId = parseInt(likeBtn.attr('id').slice(5), 10);
                    $.ajax({
                        url: NSN.requestUrl('ajax/like'),
                        type: 'get',
                        async: false,
                        data: { where: 'on_photo', id: photoId },
                        success: function (json) {
                            if (json.Status == 1) {
                                likeBtn.removeClass('likeBtn');
                                likeBtn.addClass('unlikeBtn');
                                likeBtn.html('<span>Unlike</span>');
                                likeBtn.unbind('click');
                                likeBtn.bind('click', unlikeForPhotoHandler);
                                displayTotalLiked_InPhotoZoom(photoId);
                            }
                            else {
                                NSN_alertError(json.Message);
                            }
                        }
                    });
                    evt.preventDefault();
                }
                function unlikeForPhotoHandler(evt) {
                    var likeBtn = $(this),
                        photoId = parseInt(likeBtn.attr('id').slice(5), 10);
                    $.ajax({
                        url: NSN.requestUrl('ajax/unlike'),
                        type: 'get',
                        async: false,
                        data: { where: 'on_photo', id: photoId },
                        success: function (json) {
                            if (json.Status == 1) {
                                likeBtn.removeClass('unlikeBtn');
                                likeBtn.addClass('likeBtn');
                                likeBtn.html('<span>Like</span>');
                                likeBtn.unbind('click');
                                likeBtn.bind('click', likeForPhotoHandler);
                                displayTotalLiked_InPhotoZoom(photoId);
                            }
                            else {
                                NSN_alertError(json.Message);
                            }
                        }
                    });
                    evt.preventDefault();
                }
                function displayTotalLiked_InPhotoZoom(photoId) {
                    $.ajax({
                        url: NSN.requestUrl('like/totallike'),
                        data: { typeId: 'photo', itemId: photoId },
                        success: function (json) {
                            if (json.Status == 1) {
                                var photoZoomItem = NSN.$id('photo-zoom-' + photoId),
                                    summaryInfo = photoZoomItem.find('.summaryInfo');
                                totalLike = json.Message;
                                if (!totalLike || totalLike == 0) {
                                    summaryInfo.find('.likeInfo').html('');
                                }
                                else {
                                    var totalLikeSpan = summaryInfo.find('.totalLike');
                                    if (totalLikeSpan && totalLikeSpan.length > 0) {
                                        totalLikeSpan.text(totalLike);
                                    }
                                    else {
                                        var likeInfoDiv = summaryInfo.find('.likeInfo');
                                        totalLikeSpan = '<span class="totalLike">' + totalLike + '</span> people liked this.';
                                        likeInfoDiv.html(totalLikeSpan);
                                    }
                                }
                            }
                        }
                    });
                }
                if (dlg.find('.likeBtn')) {
                    dlg.on('click', '.likeBtn', likeForPhotoHandler);
                }
                if (dlg.find('.unlikeBtn')) {
                    dlg.on('click', '.unlikeBtn', unlikeForPhotoHandler);
                }
                dlg.dialog('open');
                dlgParent.next().addClass('_photoZoomOverlay').click(function (evt) {
                    dlg.dialog('close');
                });
            }
        });
    };

    $('.UIGrid_Photo .UIPhotoItem').on('click', 'img.zoomFeature', function (evt) {
        var photoId = parseInt($(this).parents('.UIPhotoItem').attr('id').slice(6), 10);
        zoomPhotoItem(photoId);
        evt.preventDefault();
    });
    $('.uiFeedItem .photoUnit img.zoomFeature').live('click', function (evt) {
        var photoId = parseInt($(this).attr('id').slice(6), 10);
        zoomPhotoItem(photoId);
        evt.preventDefault();
    });

    $('.UIContent_UserProfile .profileActions').on('click', '.guiButton.addFriend', function (evtObj) {
        var profileItem = NSN_getProfileItem(this),
            profileId = NSN_getProfileId(profileItem);
        createRequestFriendDlg("_profile", profileId);
    });

    $('.UIForm_RequestFriend .requestActions .cancelBtn').live('click', function (evtObj) {
        NSN.$id('request-friend-form').dialog('destroy').remove();
    });

    $('.UIForm_RequestFriend._profile .requestActions .requestBtn').live('click', function (evtObj) {
        var friendUserId = $('.UIForm_RequestFriend input[name=friendUserId]').val(),
            message = $('.UIForm_RequestFriend textarea[name=message]').val();
        ajaxAddFriendRequest(friendUserId, message, successCallback);
        function successCallback(json) {
            if (json.Status == 1) {
                NSN.$id('request-friend-form').dialog('destroy').remove();
                NSN.createJqDlg(glbDefaultDlgId, json.Message, {
                    buttons: [
				                {
				                    'text': 'Close',
				                    'class': 'guiBlueButton',
				                    'click': function () {
				                        $('.UIContent_UserProfile .requestFriendAction')
							                .html('<div class="guiButton guiRedButton confirmingFriend">+1 Friend Request Sent</div>');
				                        $(this).dialog('destroy').remove();
				                    }
				                }
			                ]
                }).dialog('open');
            }
            else {
                NSN_alertError(json.Message);
            }
        }
    });
    $('.UIForm_RequestFriend._search .requestActions .requestBtn').live('click', function (evt) {
        var friendUserId = $('.UIForm_RequestFriend input[name=friendUserId]').val(),
            message = $('.UIForm_RequestFriend textarea[name=message]').val();
        ajaxAddFriendRequest(friendUserId, message, successCallback);
        function successCallback(json) {
            if (json.Status == 1) {
                NSN.$id('request-friend-form').dialog('destroy').remove();
                NSN.createJqDlg(glbDefaultDlgId, json.Message, {
                    buttons: [
                				{
                				    'text': 'Close',
                				    'class': 'guiBlueButton',
                				    'click': function () {
                				        $('#pagelet_search_results').find('.addFriend._' + friendUserId)
                                            .parent().html('<span>+1 Friend Request Sent</span>')
                				        $(this).dialog('destroy').remove();
                				    }
                				}
                            ]
                }).dialog('open');
            }
            else {
                NSN_alertError(json.Message);
            }
        }
    });

    NSN.$id('nsnRequestsJewel').click(function (evt) {
        $.ajax({
            url: NSN.requestUrl('jewels/showfriendrequests'),
            type: 'post',
            success: function (result) {
                var uiRequestContainer = jQuery(result).find('.uiRequestContainer');
                if (uiRequestContainer.html().trim().length > 0) {
                    NSN.createJqDlg('friend-request-stream', result, {
                        hasTitle: false,
                        width: 'auto'
                    }).dialog('open');
                }
            }
        });
        evt.preventDefault();
    });
    $('.UIStream_FriendRequests .confirmBtn').live('click', function (evt) {
        var divActions = $(this).parent();
        var requestId = parseInt(divActions.prev().attr('id').slice(15), 10);
        $.ajax({
            url: NSN.requestUrl('friendrequest/accept'),
            type: 'post',
            data: { requestId: requestId },
            success: function (json) {
                divActions.html(json.Message);
            }
        });
        evt.preventDefault();
    });
    $(document.body).delegate('.UIStream_FriendRequests .notNowBtn', 'click', function (evt) {
        evt.preventDefault();
        var requestItem = jQuery(this).parents('.uiFriendRequestItem');
        NSN_alertConfirmRemove('Do you want to cancel this request?', 'Cancel Friend Request Confirmation', function () {
            var requestId = requestItem.find('.profileItem').attr('id').slice(15);
            ajaxCancelFriendRequest(requestId, successCallback);
            jQuery(this).dialog('destroy').remove();
        });
        function successCallback(json) {
            if (json.Status == 1) {
                var divActions = requestItem.find('.actions').html(json.Message);
            }
            else {
                NSN_alertError(json.Message);
            }
        }
    });
    $('#profile-editinfo-form .submitBtn').click(function (evt) {
        var form = NSN.$id('profile-editinfo-form');
        NSN.ajaxSubmit(form)
            .success(function (json) {
                if (json.Status == 1) {
                    document.location = json.ReturnedPath;
                }
                else {
                    NSN_alertError(json.Message);
                }
            });
    });
    $('#change-avatar-btn').click(function (evt) {
        evt.preventDefault();
        $.ajax({
            url: NSN.requestUrl('profile/changeavatar'),
            async: false,
            success: dialogHandler
        });
        function dialogHandler(result) {
            NSN.createJqDlg('change-avatar-dialog', result, {
                title: 'Change Profile Photo',
                buttons: {}
            }).dialog('open');
        }
    });

    // Home Stream Page

    $('.uiStreamHomePage .UIActionLinks_bottom a.comment_action').live('click', function (evt) {
        var storyItem = HomeStream_getStreamStoryItem(this),
            uiAddComment = storyItem.find('.uiUfiAddComment');
        uiAddComment.removeClass('hidden_elem');
        uiAddComment.find('input[name=commentText]').focus();
        evt.preventDefault();
    });
    $('.uiStreamHomePage .uiUfiLike .uiUfiLikeIcon').live('click', function (evt) {
        var storyItem = HomeStream_getStreamStoryItem(this);
        $('.UIActionLinks_bottom a.like_action', storyItem).click();
    });
    $('.uiStreamHomePage .UIActionLinks_bottom a.like_action').live('click', function (evt) {
        var likeBtn = $(this),
            storyItem = HomeStream_getStreamStoryItem(likeBtn),
            feedId = HomeStream_getFeedId(storyItem);
        ajaxLikeAction("on_feed", feedId, successCallback);
        evt.preventDefault();
        function successCallback(json) {
            if (json.Status == 1) {
                likeBtn.find('.default_message').html('Unlike');
                likeBtn.find('.save_message').html('Like');
                likeBtn.removeClass('like_action').addClass('unlike_action');
                displayTotalLiked_OnStream(storyItem, feedId);
            }
        }
    });
    $('.uiStreamHomePage .UIActionLinks_bottom a.unlike_action').live('click', function (evt) {
        var unlikeBtn = $(this),
            storyItem = HomeStream_getStreamStoryItem(unlikeBtn),
            feedId = HomeStream_getFeedId(storyItem);
        ajaxUnlikeAction("on_feed", feedId, successCallback);
        evt.preventDefault();
        function successCallback(json) {
            if (json.Status == 1) {
                unlikeBtn.find('.default_message').html('Like');
                unlikeBtn.find('.save_message').html('Unlike');
                unlikeBtn.removeClass('unlike_action').addClass('like_action');
                displayTotalLiked_OnStream(storyItem, feedId);
            }
        }
    });
    $('.uiStreamHomePage .commentArea input[name=commentText]').live('keydown', function (evt) {
        var input = jQuery(this),
            storyItem = HomeStream_getStreamStoryItem(input),
            feedId = HomeStream_getFeedId(storyItem),
            commentText = input.val();
        if (evt.keyCode == 13) {
            if (NSN.isBlank(commentText)) {
                input.val('');
                return;
            }
            NSN_postComment("on_stream", feedId, commentText, successCallback);
            function successCallback(data) {
                if (data != null && data.length > 0) {
                    input.val('');
                    storyItem.find('.uiUfiComments .commentList').append(data);
                }
                else {
                    NSN_alertError('Cannot process your comment. Please try again!');
                }
            }
        }
    });
    $('.uiStreamHomePage .uiUfiComment .commentRemoverButton').live('click', function (evt) {
        evt.preventDefault();
        var removeBtn = jQuery(this),
            storyItem = HomeStream_getStreamStoryItem(removeBtn),
            commentItem = HomeStream_getCommentItem(removeBtn);
        NSN_alertConfirmRemove('Delete this comment? (Cannot undo)', 'Remove Comment Confirmation', function (evt) {
            var feedId = HomeStream_getFeedId(storyItem),
                commentId = HomeStream_getCommentId(commentItem);
            ajaxRemoveComment(feedId, commentId, successCallback);
            $(this).dialog('destroy').remove();
        });
        function successCallback(json) {
            if (json.Status == 1) {
                commentItem.remove();
            }
            else {
                NSN_alertError(json.Message);
            }
        }
    });
    $('.uiStreamHomePage .uiStreamHide').live('click', function (evt) {
        evt.preventDefault();
        var storyItem = HomeStream_getStreamStoryItem(this);
        NSN_alertConfirmRemove('Are you sure remove this feed?', 'Remove Feed Confirmation', function () {
            var feedId = HomeStream_getFeedId(storyItem);
            ajaxRemoveFeed(feedId, successCallback);
            $(this).dialog('destroy').remove();
        });
        function successCallback(json) {
            if (json.Status == 1) {
                storyItem.remove();
            }
            else {
                NSN_alertError(json.Message);
            }
        }
    });
    $('.uiStreamHomePage').delegate('.uiStreamAttachments .uiMediaThumb', 'click', function (evt) {
        evt.preventDefault();
        var thumbItem = jQuery(this),
            photoId = /photo_\d+/.exec(thumbItem.attr('class')).toString().slice(6);
        zoomPhotoItem(photoId);
    });

    $('#pagelet_search_results').on('click', '.addFriend', function (evt) {
        evt.preventDefault();
        var addFriendBtn = $(this),
            friendUserId = /_\d+/.exec(addFriendBtn.attr('class')).toString().slice(1);
        createRequestFriendDlg("_search", friendUserId);
    });

    function HomeStream_getStreamStoryItem(el) {
        return (el instanceof $) ? el.parents('.uiStreamStory') : $(el).parents('.uiStreamStory');
    }
    function HomeStream_getFeedId(el) {
        return el.attr('id').slice(6);
    }
    function HomeStream_getCommentItem(el) {
        return (el instanceof $) ? el.parents('.uiUfiComment') : $(el).parents('.uiUfiComment');
    }
    function HomeStream_getCommentId(el) {
        return /comment_\d+/.exec(el.attr('class')).toString().slice(8);
    }
    function ajaxLikeAction(where, feedId, successHandler) {
        $.ajax({
            url: NSN.url('/ajax/like'),
            type: 'get',
            data: { where: where, id: feedId },
            success: successHandler
        });
    }
    function ajaxUnlikeAction(where, feedId, successHandler) {
        $.ajax({
            url: NSN.url('/ajax/unlike'),
            type: 'get',
            data: { where: where, id: feedId },
            success: successHandler
        });
    }
    function ajaxRemoveComment(feedId, commentId, successCallback) {
        $.ajax({
            url: NSN.requestUrl('comment/removeonfeed'),
            type: 'post',
            async: false,
            data: { feedId: feedId, commentId: commentId },
            success: successCallback
        });
    }
    function ajaxRemoveFeed(feedId, successCallback) {
        $.ajax({
            url: NSN.requestUrl('feed/remove'),
            type: 'post',
            async: false,
            data: { feedId: feedId },
            success: successCallback
        });
    }
    function ajaxAddFriendRequest(friendUserId, message, successCallback) {
        $.ajax({
            url: NSN.requestUrl('friendrequest/addsave'),
            type: 'post',
            data: { friendUserId: friendUserId, message: escape(message) },
            success: successCallback
        });
    }
    function ajaxCancelFriendRequest(requestId, successCallback) {
        $.ajax({
            url: NSN.requestUrl('friendrequest/cancel'),
            type: 'post',
            data: { requestId: requestId },
            success: successCallback
        });
    }
    function ajaxLogOut(successCallback) {
        $.ajax({
            url: NSN.url('/auth/logout'),
            type: 'post',
            success: successCallback
        });
    }
    function createRequestFriendDlg(where, friendUserId) {
        NSN.callJqDlg('request-friend-form', NSN.requestUrl('friendrequest/add'),
        {
            type: 'post',
            data: { where: where, friendUserId: friendUserId }
        },
        {
            title: 'Request Friend',
            buttons: {}
        }).dialog('open');
    }
    //    function createRequestFriendDlg(where, friendUserId) {
    //        $.ajax({
    //            url: NSN.requestUrl('friendrequest/add'),
    //            type: 'post',
    //            data: { where: where, friendUserId: friendUserId },
    //            success: openDialogHandler
    //        });
    //        function openDialogHandler(result) {
    //            NSN.createJqDlg('request-friend-form', result, {
    //                title: 'Request Friend',
    //                buttons: {}
    //            }).dialog('open');
    //        }
    //    }
    function navAccountClickHandler(evt) {
        var item = $(this),
            menu = item.find('ul.navigation');
        if (!item.hasClass('openToggler')) {
            item.addClass('openToggler');
        }
        if (menu.hasClass('hidden_elem')) {
            menu.removeClass('hidden_elem');
        }
    }
    function userNavigationMouseOutHandler(evt) {
        var menu = $(this);
        if (!menu.hasClass('hidden_elem')) {
            menu.addClass('hidden_elem');
        }
        if (NSN.$id('navAccount').hasClass('openToggler')) {
            NSN.$id('navAccount').removeClass('openToggler');
        }
    }
    function logOutHandler(evt) {
        evt.preventDefault();
        ajaxLogOut(successCallback);
        function successCallback() {
            document.location = NSN.url('/');
        }
    }
    function displayTotalLiked_OnStream(storyItem, feedId) {
        $.ajax({
            url: NSN.requestUrl('like/totallikeonfeed'),
            data: { feedId: feedId },
            success: successCallback
        });
        function successCallback(json) {
            if (json.Status == 1) {
                var uiUfiLike = storyItem.find('.uiUfiLike'),
                    totalLike = json.Message;
                if (!totalLike || totalLike == 0) {
                    if (!uiUfiLike.hasClass('hidden_elem')) {
                        uiUfiLike.addClass('hidden_elem');
                    }
                }
                else {
                    uiUfiLike.find('.total_like').html(totalLike + ' people');
                    if (uiUfiLike.hasClass('hidden_elem')) {
                        uiUfiLike.removeClass('hidden_elem');
                    }
                }
            }
        }
    }

});
