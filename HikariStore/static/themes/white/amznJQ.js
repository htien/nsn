/* begin amazonJQ/amznJQ.js */
;(function(window, jQuery, undefined) {
    var $ = jQuery;
    if (!jQuery) {
        return;
    }

    var bootstrapAmznJQ = window.amznJQ;

    if (!window.goN2Debug) {
        window.goN2Debug = new function() {
            this.info = function() { };
            return this;
        }
    }

    window.amznJQ = new function() {
        ;
        var me = this;
        me.jQuery = jQuery;
        jQuery.noConflict(true);
        if (window.jQuery) {
            ;
        }
        else {
            window.jQuery = jQuery;
        }

        var _logicalToPhysical = {JQuery: {functionality: 'JQuery', urls: null}, popover: {functionality: 'popover', urls: null}};
        var _func_loaded = {};
        var _url_loaded = {};
        var _loading = {};
        
        function _loadFunctionality(functionality) {
            var urls = _logicalToPhysical[functionality].urls;
            if (urls) {
                ;
                $.each(urls, function() {
                    if (!_url_loaded[this]) {
                        _loadURL(this, functionality);
                    }
                    else {
                        ;
                    }
                });
            }
        }

        function _loadURL(url, functionality) {
            ;
            $.ajax({type: 'GET', url: url, success: _onUrlLoadedFcn(url, functionality), dataType: 'script', cache: true});
        }

        function _onUrlLoadedFcn(url, functionality) {
            return function() {
                ;
                _url_loaded[url] = true;
                var all_loaded = true;
                $.each(_logicalToPhysical[functionality].urls, function() {
                    all_loaded = all_loaded && !!_url_loaded[this];
                });
                if (all_loaded) {
                }
            };
        }

        me.addLogical = function(functionality, urls) {
            goN2Debug.info('amznJQ.addLogical for "' + functionality + '", with ' + (urls ? urls.length : 'no') + ' url(s) called');
            _logicalToPhysical[functionality] = {functionality: functionality, urls: urls};
            if (!urls) {
                me.declareAvailable(functionality);
                return;
            }
            if (_loading[functionality]) {
                _loadFunctionality(functionality);
            }
        };

        me.declareAvailable = function(functionality) {
            ;
            if (typeof _logicalToPhysical[functionality] == 'undefined') {
                _logicalToPhysical[functionality] = {functionality: functionality, urls: null};
            }
            _func_loaded[functionality] = true;
            $(document).trigger(functionality + '.loaded');
        };

        me.addStyle = function(css_url) {
            var dcss = document.styleSheets[0];
            if (dcss && dcss.addImport) {
                while (dcss.imports.length >= 31) {
                    dcss = dcss.imports[0];
                }
                dcss.addImport(css_url);
            }
            else {
                $('style[type="text/css"]:first').append('@import url("' + css_url + '");');
            }
        };

        me.addStyles = function(args) {
            var urls = args.urls || [];
            var styles = args.styles || [];
            var dcss = document.styleSheets;
            if (dcss && !dcss.length && document.createStyleSheet) {
                document.createStyleSheet();
            }
            dcss = dcss[0];
            if (dcss && dcss.addImport) {
                $.each(urls, function() {
                    while (dcss.imports.length >= 31) {
                        dcss = dcss.imports[0];
                    }
                    dcss.addImport(this);
                });
            }
            else {
                $.each(urls, function() {
                    var attrs = {type: 'text/css', rel: 'stylesheet', href: this};
                    $('head').append($('<link />').attr(attrs));
                });
            }
            var css = '';
            $.each(styles, function() {
                css += this;
            });
            if (css) {
                if (document.createStyleSheet) {
                    try {
                        var sheet = document.createStyleSheet();
                        sheet.cssText = css;
                    }
                    catch (e) {
                    }
                }
                else {
                    $('head').append($('<style />').attr({type: 'text/css'}).append(css));
                }
            }
        };

        me.available = function(functionality, eventCallbackFunction) {
            if (_func_loaded[functionality]) {
                ;
                $(document).one(functionality + '.loaded', eventCallbackFunction);
                $(document).trigger(functionality + '.loaded');
            }
            else {
                if (_loading[functionality]) {
                    ;
                    $(document).one(functionality + '.loaded', eventCallbackFunction);
                }
                else {
                    if (_logicalToPhysical[functionality]) {
                        ;
                        _loading[functionality] = true;
                        $(document).one(functionality + '.loaded', eventCallbackFunction);
                        _loadFunctionality(functionality);
                    }
                    else {
                        ;
                        _loading[functionality] = true;
                        $(document).one(functionality + '.loaded', eventCallbackFunction);
                    }
                }
            }
        };

        me.onReady = function(functionality, eventCallbackFunction) {
            var ajq = this;
            $(function() {
                ajq.available(functionality, eventCallbackFunction);
            });
        };

        var _stage_completed = {};
        var _fail_safe_stages = ['amznJQ.theFold', 'amznJQ.criticalFeature'];

        me.onCompletion = function(stage, callbackFn) {
            if (_stage_completed[stage]) {
                ;
                $(document).one(stage, callbackFn);
                $(document).trigger(stage);
            }
            else {
                ;
                $(document).one(stage, callbackFn);
            }
        };

        me.completedStage = function(stage) {
            if (!_stage_completed[stage]) {
                ;
                _stage_completed[stage] = true;
                $(document).trigger(stage);
            }
        };

        me.windowOnLoad = function() {
            ;
            $.each(_fail_safe_stages, function() {
                if (!_stage_completed[this]) {
                    ;
                    _stage_completed[this] = true;
                    $(document).trigger(this);
                }
            });
        };

        (function() {
            var plUrls = [], lowPriUrls = [], hiPriUrls = [], isLowPriEligibleYet = false, ST = setTimeout;
            var load = (function() {
                var doc = document,
                    docElem = doc.documentElement,
                    styleObj = docElem.style,
                    isWebkit = ('webKitAppearance' in styleObj),
                    isGecko = ('MozAppearance' in styleObj),
                    isSafari = (isWebkit && navigator.vendor.indexOf('Apple') === 0),
                    isIE = !isWebkit && !isGecko && (navigator.appName.indexOf('Microsoft') === 0),
                    useMultipleLoaders = isIE || isWebkit,
                    allowedLoaders = 2,
                    currentLoaders = 0;
                if (isSafari) {
                    return function() {
                    };
                }
                function setLoadState() {
                    if (hiPriUrls.length > 0) {
                        plUrls = hiPriUrls;
                    }
                    else {
                        plUrls = lowPriUrls;
                        if (plUrls.length === 0 || !isLowPriEligibleYet) {
                            return false;
                        }
                    }
                    if (useMultipleLoaders) {
                        if (currentLoaders >= allowedLoaders) {
                            return false;
                        }
                        currenLoaders++;
                    }
                    return true;
                }
                if (useMultipleLoaders) {
                    function loaderCallback() {
                        currentLoaders = currentLoaders < 1 ? 0 : currentLoaders - 1;
                        if (!isIE) {
                            load();
                        }
                        else {
                            ST(load, 0);
                        }
                    }
                    return function() {
                        if (!setLoadState()) {
                            return;
                        }
                        var url = plUrls.pop(), loader;
                        if (!isSafari) {
                            loader = new Image();
                        }
                        else {
                            loader = doc.createElement('script');
                            loader.type = 'f';
                        }
                        loader.onerror = loader.onload = loaderCallback;
                        loader.src = url;
                        if (!isIE) {
                            loader.width = loader.height = 0;
                            docElem.appendChild(loader);
                        }
                        if (currentLoaders < allowedLoaders) {
                            load();
                        }
                    };
                }
                else {
                    if (isGecko) {
                        var loaderTimeout,
                            delay = 500, maxDelay = 4000,
                            loader = doc.createElement('object');
                        loader.style.display = 'none';
                        docElem.appendChild(loader);
                        function loadTimedOut() {
                            delay = delay < maxDelay ? delay * 2 : maxDelay;
                            load();
                        }
                        return function() {
                            if (!setLoadState()) {
                                return;
                            }
                            if (loaderTimedOut) {
                                clearTimeout(loaderTimeout);
                            }
                            loader.onload = load;
                            loader.data = plUrls.pop();
                            loaderTimeout = ST(loadTimedOut, delay);
                        };
                    }
                    else {
                        return function() {
                        };
                    }
                }
            })();

            function processUrlList(urlList, target) {
                if (typeof (urlList) === 'string') {
                    urlList = [urlList];
                }
                else {
                    if (typeof (urlList) !== 'object' || urlList === null) {
                        return;
                    }
                }
                for (var i = 0; i < urlList.length; i++) {
                    if (urlList[i] && typeof (urlList[i]) !== 'string') {
                        processUrlList(urlList[i], target);
                    }
                    else {
                        if (urlList[i]) {
                            target.push(urlList[i]);
                        }
                    }
                }
            }

            me.addPL = function(urlList) {
                processUrlList(urlList, lowPriUrls);
                load();
            };

            me.getLoadState = function() {
                return {'hiPriUrls': hiPriUrls.length, 'lowPriUrls': lowPriUrls.length, 'plUrls': plUrls.length};
            };

            me.PLNow = function(urlList) {
                processUrlList(urlList, hiPriUrls);
                load();
            };

            $(window).load(function() {
                ST(function() {
                    isLowPriEligibleYet = true;
                    load();
                }, 1000);
            });
        }());

        me.strings = {};
        me.chars = {};

        if (bootstrapAmznJQ) {
            $.extend(this.strings, bootstrapAmznJQ.strings);
            $.extend(this.chars, bootstrapAmznJQ.chars);
        }
    }();

    $(window).load(function() {
        amznJQ.windowOnLoad();
    });

    if (window.ue && bootstrapAmznJQ && window.ues && window.uex) {
        ues('wb', 'jQueryActive', 1);
        uex('ld', 'jQueryActive');
    }

    amznJQ.declareAvailable('JQuery');
    amznJQ.declareAvailable('jQuery');

    if (bootstrapAmznJQ) {
        ;
        $.each(bootstrapAmznJQ._l, function() {
            amznJQ.addLogical(this[0], this[1]);
        });
        $.each(bootstrapAmznJQ._s, function() {
            amznJQ.addStyle(this[0]);
        });
        $.each(bootstrapAmznJQ._d, function() {
            amznJQ.declareAvailable(this[0], this[1]);
        });
        $.each(bootstrapAmznJQ._a, function() {
            amznJQ.available(this[0], this[1]);
        });
        $.each(bootstrapAmznJQ._o, function() {
            amznJQ.onReady(this[0], this[1]);
        });
        $.each(bootstrapAmznJQ._c, function() {
            amznJQ.onCompletion(this[0], this[1]);
        });
        $.each(bootstrapAmznJQ._cs, function() {
            amznJQ.completedStage(this[0], this[1]);
        });
        amznJQ.addPL(bootstrapAmznJQ._pl);
    }

}(window, jQuery));

/* end amazonJQ/amznJQ.js */

/* begin amazonJQ/amzPopover.js */
(function($) {
    if (!$) {
        return;
    }

    var rootElement = function() {
        var container = $('#ap_container');
        return container.length && container || $('body');
    };

    var viewport = {
        width: function() {
            return Math.min($(window).width(), $(document).width());
        },
        height: function() {
            return $(window).height();
        }
    };

    var mouseTracker = function() {
        var regions = [], n = 3, cursor = [{x: 0, y: 0}], c = 0, scroll = [0, 0], listening = false;
        var callbackArgs = function() {
            var pCursors = [];
            for (var i = 1; i < n; i++) {
                pCursors.push(cursor[(c - i + n) % n]);
            }
            return $.extend(true, {}, {cursor: cursor[c], priorCursors: pCursors});
        };
        var check = function(immediately) {
            for (var i = 0; i < regions.length; i++) {
                var r = regions[i];
                var inside = $.grep(r.rects, function(n) {
                    return cursor[c].x >= n[0] && cursor[c].y >= n[1] && cursor[c].x < n[0] + n[2] && cursor[c].y < n[1] + n[3];
                }).length > 0;
                if (r.inside !== null && inside && !r.inside && r.mouseEnter) {
                    r.inside = r.mouseEnter(callbackArgs());
                }
                else {
                    if (r.inside !== null && !inside && r.inside && r.mouseLeave) {
                        r.inside = !r.mouseLeave(immediately, callbackArgs());
                    }
                }
            }
        };

        var startListening = function() {
            scroll = [$(window).scrollLeft(), $(window).scrollTop()];
            $(document).mousemove(function(e) {
                c = (c + 1) % n;
                cursor[c] = {x: e.pageX,y: e.pageY};
                check();
            });
            if (!isMobileAgent(true)) {
                $(document).scroll(function(e) {
                    cursor[c].x += ($(window).scrollLeft() - scroll[0]);
                    cursor[c].y += ($(window).scrollTop() - scroll[1]);
                    scroll = [$(window).scrollLeft(), $(window).scrollTop()];
                    check();
                });
            }
            listening = true;
        };

        return {
            add: function(rectsArray, options) {
                if (!listening) {
                    startListening();
                }
                var r = $.extend({rects: rectsArray}, options);
                regions.push(r);
                return r;
            },
            remove: function(region) {
                for (var i = 0; i < regions.length; i++) {
                    if (regions[i] === region) {
                        regions.splice(i, 1);
                        return;
                    }
                }
            },
            checkNow: function() {
                check(true);
            },
            getCallbackArgs: function() {
                return callbackArgs();
            }
        };
    }();

    var iframePool = function() {
        var ie6 = $.browser.msie && parseInt($.browser.version) <= 6;
        var src = ie6 ? window.AmazonPopoverImages.pixel : 'javascript:void(false)';
        var HTML = '<iframe frameborder="0" tabindex="-1" src="' + src + '" style=display:none;position:absolute;z-index:0;filter:Alpha(Opacity=\'0\');opacity:0;" />';
        var pool = [];
        var addToLib = function(n) {
            for (i = 0; i < n; i++) {
                pool.push($(HTML).prependTo(rootElement()));
            }
        };
        $(document).ready(function() {
            addToLib(3);
        });
        return {
            checkout: function(jqObj) {
                if (!pool.length) {
                    addToLib(1);
                }
                return pool.pop().css({display: "block", top: jqObj.offset().top, left: jqObj.offset().left, width: jqObj.outerWidth(), height: jqObj.outerHeight(), zIndex: Number(jqObj.css("z-index")) - 1});
            },
            checkin: function(iframe) {
                pool.push(iframe.css('display', 'none'));
            }
        };
    }();

    var elementHidingManager = function() {
        var hiddenElements = [];
        var win = /Win/.test(navigator.platform);
        var mac = /Mac/.test(navigator.platform);
        var linux = /Linux/.test(navigator.platform);
        var version = parseInt($.browser.version);
        var canOverlayWmodeWindow = false;
        var intersectingPopovers = function(obj) {
            var bounds = [obj.offset().left, obj.offset().top, obj.outerWidth(), obj.outerHeight()];
            var intersecting = [];
            for (var i = 0; i < popovers.length; i++) {
                var disparate = false;
                if (!popovers[i].settings.modal) {
                    var r = popovers[i].bounds;
                    disparate = bounds[0] > r[0] + r[2] || r[0] > bounds[0] + bounds[2] || bounds[1] > r[1] + r[3] || r[1] > bounds[1] + bounds[3];
                }
                if (!disparate) {
                    intersecting.push(popovers[i]);
                }
            }
            return intersecting;
        };
        var shouldBeVisible = function(obj) {
            if (obj.hasClass("ap_never_hide")) {
                return true;
            }
            if (intersectingPopovers(obj).length) {
                if (obj.is("object,embed")) {
                    var wmode = obj.attr("wmode") || obj.children("object,embed").attr("wmode") || obj.parent("object,embed").attr("wmode") || "window";
                    if (wmode.toLowerCase() == "window" && !canOverlayWmodeWindow) {
                        return false;
                    }
                }
                if (obj.is("iframe")) {
                    if ($.browser.safari) {
                        return false;
                    }
                }
            }
            return true;
        };
        var setVisibility = function(elementQuery, shouldBecomeVisible) {
            if (elementQuery.is("iframe[id^=DA],iframe[id^=cachebust]")) {
                elementQuery.css({display: shouldBecomeVisible ? "block" : "none"});
            } else {
                elementQuery.css({visibility: shouldBecomeVisible ? "visible" : "hidden"});
            }
        };
        return {
            update: function() {
                var HIDDEN = 0;
                var VISIBLE = 1;
                var stillHidden = [];
                for (var i = 0; i < hiddenElements.length; i++) {
                    var hiddenElement = hiddenElements[i];
                    if (!shouldBeVisible(hiddenElement)) {
                        stillHidden.push(hiddenElement);
                    }
                    else {
                        setVisibility(hiddenElement, VISIBLE);
                    }
                }
                hiddenElements = stillHidden;
                $("object:visible,embed:visible,iframe:visible").each(function() {
                    var obj = jQuery(this);
                    if (!shouldBeVisible(obj)) {
                        hiddenElements.push(obj);
                        setVisibility(obj, HIDDEN);
                    }
                });
            }
        };
    }();

    var applyBacking = function(popover, options) {
        var region = null;
        var iframe = null;
        options = options || {};
        var destroy = function() {
            if (region) {
                mouseTracker.remove(region);
                region = null;
            }
            if (iframe) {
                iframePool.checkin(iframe);
                iframe = null;
            }
            elementHidingManager.update();
        };
        var refreshBounds = function() {
            var newBounds = [popover.offset().left, popover.offset().top, popover.outerWidth(), popover.outerHeight()];
            if (region) {
                region.rects[0] = newBounds;
            }
            if (iframe) {
                iframe.css({left: newBounds[0],top: newBounds[1],width: newBounds[2],height: newBounds[3]});
            }
            elementHidingManager.update();
        };
        var reposition = function(x, y) {
            if (iframe) {
                iframe.css({left: x,top: y});
            }
            if (region) {
                region.rects[0][0] = x;
                region.rects[0][1] = y;
            }
        };
        if (options.useIFrame !== false) {
            iframe = iframePool.checkout(popover);
        }
        var bounds = [[popover.offset().left, popover.offset().top, popover.outerWidth(), popover.outerHeight()]];
        if (options.additionalCursorRects) {
            for (var i = 0; i < options.additionalCursorRects.length; i++) {
                bounds.push(options.additionalCursorRects[i]);
            }
        }
        region = mouseTracker.add(bounds, options);
        elementHidingManager.update();
        popover.backing = {destroy: destroy,refreshBounds: refreshBounds,reposition: reposition,iframe: iframe};
    };

    var defaultSettings = {width: 500, followScroll: false, locationMargin: 4, alignMargin: 0, windowMargin: 4, locationFitInWindow: true, focusOnShow: true, modal: false, draggable: false, zIndex: 200, showOnHover: false, hoverShowDelay: 400, hoverHideDelay: 200, skin: "default", useIFrame: true, clone: false, ajaxSlideDuration: 400, ajaxErrorContent: null, paddingLeft: 17, paddingRight: 17, paddingBottom: 8};
    var overlay = null;
    var popovers = [];
    var et = {
        MOUSE_ENTER: 1, MOUSE_LEAVE: 2, CLICK_TRIGGER: 4, CLICK_OUTSIDE: 8, fromStrings: function(s) {
            var flags = 0;
            var self = this;
            if (s) {
                $.each($.makeArray(s), function() {
                    flags = flags | self[this];
                });
            }
            return flags;
        }
    };
    var ajaxCache = {};
    var preparedPopover = null;
    var openGroupPopover = {};
    var skins = {"default": '<div class="ap_popover ap_popover_sprited" surround="6,16,18,16" tabindex="0">                 <div class="ap_header">                     <div class="ap_left"/>                     <div class="ap_middle"/>                     <div class="ap_right"/>                 </div>                 <div class="ap_body">                     <div class="ap_left"/>                     <div class="ap_content"><img src="' + window.AmazonPopoverImages.snake + '"/></div>                     <div class="ap_right"/>                 </div>                 <div class="ap_footer">                     <div class="ap_left"/>                     <div class="ap_middle"/>                     <div class="ap_right"/>                 </div>                 <div class="ap_titlebar">                     <div class="ap_title"/>                 </div>                 <div class="ap_close"><a href="#"><span class="ap_closetext"/><span class="ap_closebutton"><span></span></span></a></div>             </div>',"default_non_sprited": '<div class="ap_popover ap_popover_unsprited" surround="6,16,18,16" tabindex="0">                 <div class="ap_header">                     <div class="ap_left"/>                     <div class="ap_middle"/>                     <div class="ap_right"/>                 </div>                 <div class="ap_body">                     <div class="ap_left"/>                     <div class="ap_content"><img src="' + window.AmazonPopoverImages.snake + '"/></div>                     <div class="ap_right"/>                 </div>                 <div class="ap_footer">                     <div class="ap_left"/>                     <div class="ap_middle"/>                     <div class="ap_right"/>                 </div>                 <div class="ap_titlebar">                     <div class="ap_title"/>                 </div>                 <div class="ap_close"><a href="#"><span class="ap_closetext"/><img border="0" src="' + window.AmazonPopoverImages.btnClose + '"/></a></div>             </div>',"classic": '<div class="ap_classic">                 <div class="ap_titlebar">                     <div class="ap_close">                         <img width="46" height="16" border="0" alt="close" onmouseup=\'this.src="' + window.AmazonPopoverImages.closeTan + "\";' onmouseout='this.src=\"" + window.AmazonPopoverImages.closeTan + "\";' onmousedown='this.src=\"" + window.AmazonPopoverImages.closeTanDown + '";\' src="' + window.AmazonPopoverImages.closeTan + '" />                     </div>                     <span class="ap_title"></span>                 </div>                 <div class="ap_content"><img src="' + window.AmazonPopoverImages.loadingBar + '"/></div>             </div>'};

    var boundingRectangle = function(set) {
        var b = {left: Infinity, top: Infinity, right: -Infinity, bottom: -Infinity};
        set.each(function() {
            try {
                var t = $(this);
                var o = t.offset();
                var w = t.outerWidth();
                var h = t.outerHeight();
                if (t.is('are')) {
                    var ab = boundsOfAreaElement(t);
                    o = {left: ab[0],top: ab[1]};
                    w = ab[2] - ab[0];
                    h = ab[3] - ab[1];
                }
                if (o.left < b.left) {
                    b.left = o.left;
                }
                if (o.top < b.top) {
                    b.top = o.top;
                }
                if (o.left + w > b.right) {
                    b.right = o.left + w;
                }
                if (o.top + h > b.bottom) {
                    b.bottom = o.top + h;
                }
            }
            catch (e) {
            }
        });
        return b;
    };

    var bringToFront = function(popover) {
        if (popovers.length <= 1) {
            return;
        }
        var maxZ = Math.max.apply(Math, $.map(popovers, function(p) {
            return Number(p.css('z-index'));
        }));
        if (Number(popover.css("z-index")) == maxZ) {
            return;
        }
        popover.css('z-index', maxZ + 2);
        popover.backing && popover.backing.iframe.css('z-index', maxZ + 1);
    };

    $.fn.removeAmazonPopoverTrigger = function() {
        this.unbind('click.amzPopover');
        this.unbind('mouseover.amzPopover');
        this.unbind('mouseout.amzPopover');
        return this;
    };

    $.fn.amazonPopoverTrigger = function(customSettings) {
        var settings = $.extend({}, defaultSettings, customSettings);
        var triggers = this;
        var popover = null;
        if (!settings.showOnHover && settings.skin == 'default') {
            this.bind('mouseover.amzPopover', preparePopover);
        }
        if (typeof settings.showOnHover == 'string') {
            var hoverSet = triggers.filter(settings.showOnHover);
        }
        else {
            var hoverSet = settings.showOnHover ? triggers : jQuery([]);
        }
        var timerID = null;
        hoverSet.bind('mouseover.amzPopover', function(e) {
            if (!popover && !timerID) {
                timerID = setTimeout(function() {
                    if (!popover) {
                        var parent = triggers.parent(), length = parent.length, tagName = length ? parent.attr('tagName') || parent.get(0).tagName : undefined;
                        if (length && tagName) {
                            if (!settings.triggeringEnabled || settings.triggeringEnabled.call(triggers)) {
                                popover = displayPopover(settings, triggers, function() {
                                    popover = null;
                                });
                            }
                        }
                    }
                    timerID = null;
                }, settings.hoverShowDelay);
            }
            return false;
        });
        hoverSet.bind('mouseout.amzPopover', function(e) {
            if (!popover && timerID) {
                clearTimeout(timerID);
                timerID = null;
            }
        });
        triggers.bind('click.amzPopover', function(e) {
            var followLink = settings.followLink === true || typeof settings.followLink == 'function' && settings.followLink.call(triggers, popover, settings);
            if (followLink) {
                return true;
            }
            if (popover) {
                popover.triggerClicked();
            }
            else {
                if (!settings.triggeringEnabled || settings.triggeringEnabled.call(triggers)) {
                    popover = displayPopover(settings, triggers, function() {
                        popover = null;
                    });
                }
            }
            return false;
        });
        this.amznPopoverHide = function() {
            popover && popover.close();
        };
        this.amznPopoverVisible = function() {
            return !!popover;
        };
        return this;
    };

    var updateBacking = function(group) {
        if (group && openGroupPopover[group]) {
            var popover = openGroupPopover[group];
            if (popover.backing) {
                popover.backing.refreshBounds();
            }
        }
    };

    var displayPopover = function(settings, triggers, destroyFunction) {
        addAliases(settings);
        var parent = null;
        if (triggers) {
            var parents = triggers.eq(0).parents().get();
            for (var t = 0; t < parents.length && !parent; t++) {
                for (var i = 0; i < popovers.length && !parent; i++) {
                    if (popovers[i].get(0) == parents[t]) {
                        parent = popovers[i];
                    }
                }
            }
        }
        var children = [];
        children.remove = function(p) {
            for (var i = 0; i < this.length; i++) {
                if (this[i] === p) {
                    this.splice(i, 1);
                    return;
                }
            }
        };
        var interactedWith = false;
        $.each(defaultSettings, function(k, v) {
            if (typeof settings[k] == 'undefined') {
                settings[k] = v;
            }
        });
        if (!settings.location) {
            settings.location = settings.modal || !triggers ? 'centered' : 'auto';
        }
        if (settings.showCloseButton === null) {
            settings.showCloseButton = !settings.showOnHover;
        }
        $.each(popovers, function() {
            settings.zIndex = Math.max(settings.zIndex, Number(this.css('z-index')) + 2);
        });
        var closeEvent = (settings.showOnHover ? et.MOUSE_LEAVE : et.CLICK_TRIGGER) | (settings.modal ? et.CLICK_OUTSIDE : 0);
        closeEvent = (closeEvent | et.fromStrings(settings.closeEventInclude)) & ~et.fromStrings(settings.closeEventExclude);
        var clickAwayHandler;
        var close = function() {
            if (settings.group) {
                openGroupPopover[settings.group] = null;
            }
            if (original) {
                if (ballMarker && ballMarker.parents('body').length) {
                    //original.hide().insertAfter(ballMarker);
                    original.addClass('hidden').insertAfter(ballMarker);
                    ballMarker.remove();
                    ballMarker = null;
                }
                else {
                    //original.hide().appendTo(rootElement());
                    original.addClass('hidden').appendTo(rootElement());
                }
            }
            if (original != popover) {
                popover.remove();
            }
            if (parent) {
                parent.children.remove(popover);
            }
            for (var i = 0; i < popovers.length; i++) {
                if (popovers[i] === popover) {
                    popovers.splice(i, 1);
                    break;
                }
            }
            if (popover.backing) {
                popover.backing.destroy();
                popover.backing = null;
            }
            mouseTracker.checkNow();
            if (destroyFunction) {
                destroyFunction();
            }
            if (settings.onHide) {
                settings.onHide.call(triggers, popover, settings);
            }
            if (settings.modal && overlay) {
                if (overlay.fitToScreen) {
                    $(window).unbind("resize", overlay.fitToScreen);
                }
                overlay.remove();
                overlay = null;
            }
            $(document).unbind('scroll.AmazonPopover');
            $(document).unbind('click', clickAwayHandler);
            for (var i = 0; i < children.length; i++) {
                children[i].close();
            }
            children = [];
            return false;
        };
        var fill = function(content, autoshow) {
            var container = popover.find('.ap_sub_content');
            if (container.length == 0) {
                container = popover.find('.ap_content');
            }
            if (typeof content == 'string') {
                container.html(content);
            }
            else {
                container.empty().append(content);
            }
            if (typeof settings.autoshow == 'boolean' ? settings.autoshow : autoshow) {
                if ($.browser.msie) {
                    //container.children().show().hide();
                    container.children().addClass('hidden').removeClass('hidden');
                }
                //container.children(':not(style)').show();
                container.children(':not(style)').removeClass('hidden');
            }
            container.find('.ap_custom_close').click(close);
            if (settings.onFilled) {
                settings.onFilled.call(triggers, popover, settings);
            }
            return container;
        };
        if (settings.modal && !overlay) {
            overlay = showOverlay(close, settings.zIndex);
        }
        var popover = null;
        var original = null;
        var ballMarker = null;
        if (settings.skin == 'default') {
            preparePopover();
            popover = preparedPopover;
            preparedPopover = null;
        }
        else {
            var skin = $.isFunction(settings.skin) ? settings.skin() : settings.skin;
            skin = skin || '<div><div class="ap_content" /></div>';
            var skinIsHtml = /^[^<]*(<(.|\s)+>)[^>]*$/.test(skin);
            var skinHtml = (skinIsHtml ? skin : skins[skin]);
            popover = $(skinHtml);
        }
        if ($.browser.msie && parseInt($.browser.version) == 6) {
            fixPngs(popover);
        }
        if (settings.skin == 'default') {
            popover.find('.ap_content').css({paddingLeft: settings.paddingLeft,paddingRight: settings.paddingRight,paddingBottom: settings.paddingBottom});
        }
        if (settings.localContent) {
            if (settings.clone) {
                fill($(settings.localContent).clone(true), true);
            }
            else {
                original = $(settings.localContent);
                ballMarker = $('<span style="display:none" />').insertBefore(original);
                fill(original, true);
            }
        }
        else {
            if (settings.literalContent) {
                fill(settings.literalContent);
            }
        }
        if (settings.destination) {
            var destinationUrl = (typeof settings.destination == 'function') ? settings.destination() : settings.destination;
            if (settings.cacheable !== false && ajaxCache[destinationUrl]) {
                fill(ajaxCache[destinationUrl]);
            }
            else {
                $.ajax({
                    url: destinationUrl,
                    timeout: settings.ajaxTimeout,
                    success: function(data) {
                        if (settings.onAjaxSuccess) {
                            settings.onAjaxSuccess.apply(settings, arguments);
                        }
                        var contentCacheable = data.match(/^(\s|<!--[\s\S]*?-->)*<\w+[^>]*\s+cacheable="(.*?)"/i) || data.match(/^(\s|<!--[\s\S]*?-->)*<\w+[^>]*\s+cacheable='(.*?)'/i);
                        if (settings.cacheable !== false && (!contentCacheable || contentCacheable[2] !== "0")) {
                            ajaxCache[destinationUrl] = data;
                        }
                        var title = data.match(/^(\s|<!--[\s\S]*?-->)*<\w+[^>]*\s+popoverTitle="(.*?)"/i) || data.match(/^(\s|<!--[\s\S]*?-->)*<\w+[^>]*\s+popoverTitle='(.*?)'/i);
                        if (title) {
                            settings.title = title[2];
                            popover.find('.ap_title').html(settings.title);
                        }
                        if (settings.ajaxSlideDuration > 0 && !($.browser.msie && document.compatMode == 'BackCompat')) {
                            popover.find('.ap_content').hide();
                            fill(data);
                            if (!settings.width) {
                                position(popover, settings, triggers);
                            }
                            popover.find('.ap_content').slideDown(settings.ajaxSlideDuration, function() {
                                position(popover, settings, triggers);
                            });
                        }
                        else {
                            fill(data);
                            position(popover, settings, triggers);
                        }
                    },
                    error: function() {
                        var data = null;
                        if (typeof settings.ajaxErrorContent == 'function') {
                            data = settings.ajaxErrorContent.apply(settings, arguments);
                        }
                        else {
                            data = settings.ajaxErrorContent;
                        }
                        if (data !== null) {
                            var container = fill(data);
                            var title = container.children('[popoverTitle]').attr('popoverTitle');
                            if (title) {
                                popover.find('.ap_title').html(title);
                            }
                            position(popover, settings, triggers);
                        }
                    }
                });
            }
        }
        if (!settings.localContent && !settings.literalContent && !settings.destination) {
            throw ('AmazonPopover wasn\'t provided a source of content.');
        }
        if (parent) {
            parent.children.push(popover);
        }
        settings.surround = jQuery.map((popover.attr('surround') || '0,0,0,0').split(','), function(n) {
            return Number(n);
        });
        popover.css({zIndex: settings.zIndex, position: 'absolute', left: -2000, top: -2000});
        popover.click(function(e) {
            if (!e.metaKey) {
                e.stopPropagation();
            }
            interactedWith = true;
        });
        clickAwayHandler = function(e) {
            var leftButton = e.button === 0 || e.which == 1;
            if (leftButton && !e.metaKey) {
                close();
            }
        };
        if (closeEvent & et.CLICK_OUTSIDE) {
            $(document).click(clickAwayHandler);
        }
        popover.mousedown(function(e) {
            if (!children.length) {
                bringToFront(popover);
            }
        });
        var width = settings.width && (typeof settings.width == 'function' ? settings.width() : settings.width);
        if (!width) {
            width = getDynamicWidth(popover, settings) || popover.outerWidth();
        }
        if (width) {
            popover.css('width', width);
        }
        if (settings.followScroll) {
            $(document).bind('scroll.AmazonPopover', function(e) {
                followScroll(e);
            });
        }
        if (settings.title !== null && settings.title !== undefined) {
            var titleBar = popover.find('.ap_titlebar');
            if (settings.skin == "default") {
                titleBar.css({width: (width - 36)});
                titleBar.find('.ap_title').css('width', width - 70);
                popover.find('.ap_content').css({paddingTop: 18});
            }
            popover.find('.ap_title').html(settings.title);
            if (settings.draggable && !settings.modal) {
                enableDragAndDrop(titleBar, popover);
            }
            titleBar.show();
            if (settings.skin == 'default' && settings.wrapTitlebar) {
                titleBar.addClass('multiline');
                popover.find('.ap_content').css({paddingTop: titleBar.outerHeight() - 9});
            }
        }
        else {
            popover.find('.ap_titlebar').hide();
        }
        if (settings.showCloseButton !== false) {
            popover.find('.ap_close').show().click(close).mousedown(function(e) {
                e.preventDefault();
                e.stopPropagation();
                return false;
            }).css('cursor', 'default');
            if (!settings.title) {
                popover.find('.ap_content').css({paddingTop: 10});
            }
            popover.keydown(function(e) {
                if (e.keyCode == 27) {
                    close();
                }
            });
        }
        else {
            popover.find('.ap_close').css('display', 'none');
        }
        if (settings.closeText) {
            popover.find('.ap_closetext').text(settings.closeText).show();
        } else {
            popover.find('.ap_closebutton span').text('Close');
        }
        popover.appendTo(rootElement());
        position(popover, settings, triggers);
        $(document.activeElement).filter('input[type=text], select').blur();
        popover.close = close;
        if (settings.group) {
            if (openGroupPopover[settings.group]) {
                openGroupPopover[settings.group].close();
            }
            openGroupPopover[settings.group] = popover;
        }
        popover.show();
        if (settings.focusOnShow) {
            popover.get(0).hideFocus = true;
            popover.focus();
        }
        if (overlay && overlay.snapToLeft) {
            overlay.snapToLeft();
        }
        if (settings.onShow) {
            settings.onShow.call(triggers, popover, settings);
        }
        popover.bounds = [popover.offset().left, popover.offset().top, popover.outerWidth(), popover.outerHeight()];
        popovers.push(popover);
        popover.close = close;
        popover.settings = settings;
        popover.triggerClicked = function() {
            if (closeEvent & et.CLICK_TRIGGER) {
                close();
            }
        };
        popover.children = children;
        if (closeEvent & et.MOUSE_LEAVE) {
            var timerID = null;
            var triggerRects = [];
            $.each(triggers, function() {
                var n = $(this);
                if (n.is('area')) {
                    var b = boundsOfAreaElement(n);
                    triggerRects.push([b[0], b[1], b[2] - b[0], b[3] - b[1]]);
                }
                else {
                    triggerRects.push([n.offset().left, n.offset().top, n.outerWidth(), n.outerHeight()]);
                }
            });
            if (settings.additionalCursorRects) {
                $(settings.additionalCursorRects).each(function() {
                    var n = $(this);
                    triggerRects.push([n.offset().left, n.offset().top, n.outerWidth(), n.outerHeight()]);
                });
            }
            applyBacking(popover, {
                solidRectangle: settings.solidRectangle,
                useIFrame: settings.useIFrame,
                mouseEnter: function() {
                    if (timerID) {
                        clearTimeout(timerID);
                        timerID = null;
                    }
                    return true;
                },
                mouseLeave: function(immediately) {
                    if (settings.semiStatic && interactedWith) {
                        return !children.length;
                    }
                    if (timerID) {
                        clearTimeout(timerID);
                        timerID = null;
                    }
                    if (children.length == 0) {
                        if (immediately) {
                            close();
                        }
                        else {
                            timerID = setTimeout(function() {
                                close();
                                timerID = null;
                            }, settings.hoverHideDelay);
                        }
                        return true;
                    }
                    return false;
                },
                additionalCursorRects: triggerRects,
                inside: true});
        }
        else {
            applyBacking(popover, {solidRectangle: settings.solidRectangle, useIFrame: settings.useIFrame});
        }
        $(function() {
            for (var i = 0; i < popovers.length; i++) {
                if (popovers[i].settings.modal) {
                    popovers[i].backing.refreshBounds();
                }
            }
        });
        return popover;
    };

    var isMobileAgent = function(includeFire) {
        var reStr = '(iPhone|iPad' + (includeFire ? '|Silk/|Kindle Fire' : '') + ')';
        return navigator.userAgent.match(new RegExp(reStr, 'i'));
    };

    var getPageWidth = function() {
        return $.browser.msie ? $(window).width() : '100%';
    };

    var getPageHeight = function() {
        return $.browser.msie || isMobileAgent() ? $(document).height() : '100%';
    };

    var showOverlay = function(closeFunction, z) {
        var overlay = $('<div id="ap_overlay"/>');
        if ($.browser.msie) {
            overlay.fitToScreen = function(e) {
                var windowHeight = $(document).height();
                var windowWidth = $(window).width();
                var children = overlay.children();
                overlay.css({
                    width: windowWidth,
                    height: windowHeight,
                    backgroundColor: 'transparent',
                    zIndex: z
                });
                var appendElements = [];
                for (var i = 0; i < children.size() || (windowHeight - (i * 2000) > 0); i++) {
                    var paneHeight = Math.min(windowHeight - (i * 2000), 2000);
                    if (paneHeight > 0) {
                        if (i < children.size()) {
                            children.eq(i).css({
                                width: windowWidth,
                                height: paneHeight
                            });
                        }
                        else {
                            var slice = $('<div/>').css({
                                opacity: 0.4,
                                zIndex: z,
                                width: windowWidth,
                                height: paneHeight,
                                top: (i * 2000)
                            });
                            appendElements.push(slice[0]);
                        }
                    }
                    else {
                        children.eq(i).remove();
                    }
                }
                if (appendElements.length) {
                    overlay.append(appendElements);
                }
            };
            overlay.snapToLeft = function() {
                overlay.css('left', jQuery(document).scrollLeft());
            };
            $(window).bind('resize load', overlay.fitToScreen);
            $(window).scroll(overlay.snapToLeft);
            overlay.snapToLeft();
            overlay.fitToScreen();
        }
        else {
            overlay.css({
                width: getPageWidth(),
                height: getPageHeight(),
                position: ($.browser.mozilla || $.browser.safari) ? 'fixed' : '',
                opacity: 0.4,
                zIndex: z
            });
        }
        return overlay.appendTo(rootElement());
    };

    var HEADER_HEIGHT = 45;
    var FOOTER_HEIGHT = 35;
    var VERT_ARROW_OFFSET = 327;
    var LEFT_ARROW_OFFSET = 0;
    var RIGHT_ARROW_OFFSET = -51;

    var attachedPositioning = function(popover, targetY, location, position, offset) {
        if (popover.hasClass('ap_popover_sprited')) {
            var dist = targetY - location.top - offset[1];
            if (dist < HEADER_HEIGHT) {
                dist = HEADER_HEIGHT;
            }
            else {
                if (dist > popover.outerHeight() - FOOTER_HEIGHT) {
                    dist = popover.outerHeight() - FOOTER_HEIGHT;
                }
            }
            var attachingSide = position == 'left' ? 'right' : 'left';
            var elm = popover.find('.ap_body .ap_' + attachingSide);
            if (elm.length > 0) {
                elm.removeClass('ap_' + attachingSide).addClass('ap_' + attachingSide + '-arrow');
            }
            else {
                elm = popover.find('.ap_body .ap_' + attachingSide + '-arrow');
            }
            var xOffset = attachingSide == 'left' ? LEFT_ARROW_OFFSET : RIGHT_ARROW_OFFSET;
            elm.css('backgroundPosition', xOffset + 'px ' + (dist - VERT_ARROW_OFFSET) + 'px');
        }
    };

    var position = function(popover, settings, triggers) {
        if (!settings.width) {
            popover.css('width', getDynamicWidth(popover, settings));
        }
        var offset = settings.locationOffset || [0, 0];
        if (typeof settings.location == 'function') {
            var location = settings.location.call(triggers, popover, settings);
        }
        else {
            var names = $.map($.makeArray(settings.location), function(n) {
                return n == 'auto' ? ['bottom', 'left', 'right', 'top'] : n;
            });
            var set = settings.locationElement && $(settings.locationElement) || triggers;
            var b = set && boundingRectangle(set);
            var location = locationFunction[names[0]](b, popover, settings);
            var index = 0;
            for (var i = 1; i < names.length && !location.fits; i++) {
                var next = locationFunction[names[i]](b, popover, settings);
                if (next.fits) {
                    location = next;
                    index = i;
                }
            }
            if (settings.attached && (names[index] == 'left' || names[index] == 'right')) {
                attachedPositioning(popover, (b.top + b.bottom) / 2, location, names[index], offset);
            }
        }
        popover.css({
            left: location.left + offset[0],
            top: location.top + offset[1],
            margin: location.margin,
            right: location.right
        });
        if (popover.backing) {
            popover.backing.refreshBounds();
        }
    };

    var horizPosition = function(b, popover, settings) {
        var align = $.makeArray(settings.align || 'left');
        var x = {
            min: $(document).scrollLeft() + settings.windowMargin - settings.surround[3],
            max: viewport.width() + $(document).scrollLeft() - settings.windowMargin - popover.outerWidth(),
            left: b.left - settings.surround[3] - settings.alignMargin,
            right: b.right - popover.outerWidth() + settings.surround[1] + settings.alignMargin,
            center: (b.left + b.right - popover.outerWidth()) / 2
        };
        var align = $.grep($.makeArray(settings.align), function(n) {
            return x[n];
        });
        if (align.length == 0) {
            align.push('left');
        }
        for (var i = 0; i < align.length; i++) {
            if (x[align[i]] >= x.min && x[align[i]] <= x.max) {
                return x[align[i]];
            }
        }
        if (settings.forceAlignment) {
            return x[align[0]];
        }
        if (x.min > x.max) {
            return x.min;
        }
        return x[align[0]] < x.min ? x.min : x.max;
    };

    var vertPosition = function(b, popover, settings) {
        var min = $(document).scrollTop() + settings.windowMargin;
        var max = viewport.height() + $(document).scrollTop() - settings.windowMargin;
        if (settings.attached) {
            var midpoint = (b.top + b.bottom) / 2;
            if (midpoint - HEADER_HEIGHT < min) {
                min = min + HEADER_HEIGHT < b.bottom ? min : b.bottom - HEADER_HEIGHT;
            }
            if (midpoint + FOOTER_HEIGHT > max) {
                max = max - FOOTER_HEIGHT > b.top ? max : b.top + FOOTER_HEIGHT;
            }
        }
        else {
            min = Math.min(b.top - settings.alignMargin, min);
            max = Math.max(b.bottom + settings.alignMargin, max);
        }
        var y = {
            min: min - settings.surround[0],
            max: max - popover.outerHeight() + settings.surround[2],
            top: b.top - settings.surround[0] - settings.alignMargin,
            bottom: b.bottom - popover.outerHeight() + settings.alignMargin + settings.surround[2],
            middle: (b.top + b.bottom - popover.outerHeight()) / 2
        };
        var align = $.grep($.makeArray(settings.align), function(n) {
            return y[n];
        });
        if (align.length == 0) {
            align.push('top');
        }
        for (var i = 0; i < align.length; i++) {
            if (y[align[i]] >= y.min && y[align[i]] <= y.max) {
                return y[align[i]];
            }
        }
        if (settings.forceAlignment) {
            return y[align[0]];
        }
        if (y.min > y.max) {
            return y.min;
        }
        return y[align[0]] < y.min ? y.min : y.max;
    };

    var locationFunction = {
        centered: function(b, popover, settings) {
            var y = $(window).scrollTop() + 100;
            return {
                left: -(popover.outerWidth() / 2),
                right: 0,
                top: y,
                margin: "0% 50%",
                fits: true
            };
        },
        top: function(b, popover, settings) {
            var room = b.top - $(document).scrollTop() - settings.locationMargin * 2;
            var triggerInView = (b.left >= $(document).scrollLeft()) && (b.right < viewport.width() + $(document).scrollLeft());
            return {
                left: horizPosition(b, popover, settings),
                top: b.top - popover.outerHeight() - settings.locationMargin + settings.surround[2],
                fits: triggerInView && room >= popover.outerHeight() - settings.surround[0] - settings.surround[2]
            };
        },
        left: function(b, popover, settings) {
            var room = b.left - $(document).scrollLeft() - settings.locationMargin * 2;
            return {
                left: b.left - popover.outerWidth() - settings.locationMargin + settings.surround[1],
                top: vertPosition(b, popover, settings),
                fits: room >= popover.outerWidth() - settings.surround[1] - settings.surround[3]
            };
        },
        bottom: function(b, popover, settings) {
            var room = (viewport.height() + $(document).scrollTop()) - b.bottom - settings.locationMargin * 2;
            var triggerInView = (b.left >= $(document).scrollLeft()) && (b.right < viewport.width() + $(document).scrollLeft());
            return {
                left: horizPosition(b, popover, settings),
                top: b.bottom + settings.locationMargin - settings.surround[0],
                fits: triggerInView && room >= popover.outerHeight() - settings.surround[0] - settings.surround[2]
            };
        },
        right: function(b, popover, settings) {
            var room = (viewport.width() + $(document).scrollLeft()) - b.right - settings.locationMargin * 2;
            return {
                left: b.right + settings.locationMargin - settings.surround[3],
                top: vertPosition(b, popover, settings),
                fits: room >= popover.outerWidth() - settings.surround[1] - settings.surround[3]
            };
        },
        over: function(b, popover, settings) {
            var alignTo = popover.find(settings.align || '.ap_content *').offset();
            var corner = popover.offset();
            var padding = {left: alignTo.left - corner.left,top: alignTo.top - corner.top};
            var left = b.left - padding.left;
            var top = b.top - padding.top;
            var adjustedLeft = Math.min(left, viewport.width() + $(document).scrollLeft() - popover.outerWidth() - settings.windowMargin);
            adjustedLeft = Math.max(adjustedLeft, $(document).scrollLeft() - settings.surround[3] + settings.windowMargin);
            var adjustedTop = Math.min(top, viewport.height() + $(document).scrollTop() - popover.outerHeight() + settings.surround[2] - settings.windowMargin);
            adjustedTop = Math.max(adjustedTop, $(document).scrollTop() - settings.surround[0] + settings.windowMargin);
            return {
                left: settings.forceAlignment ? left : adjustedLeft,
                top: settings.forceAlignment ? top : adjustedTop,
                fits: left == adjustedLeft && top == adjustedTop
            };
        }
    };

    var addAliases = function(settings) {
        settings.align = settings.align || settings.locationAlign;
        settings.literalContent = settings.literalContent || settings.loadingContent;
    };

    var preparePopover = function() {
        if (!preparedPopover) {
            var ie6 = jQuery.browser.msie && parseInt(jQuery.browser.version) <= 6;
            preparedPopover = $(skins[ie6 ? 'default_non_sprited' : 'default']).css({left: -2000, top: -2000}).appendTo(rootElement());
        }
    };

    var fixPngs = function(obj) {
        obj.find('*').each(function() {
            var match = (jQuery(this).css('background-image') || '').match(/url\("(.*\.png)"\)/);
            if (match) {
                var png = match[1];
                jQuery(this).css('background-image', 'none');
                jQuery(this).get(0).runtimeStyle.filter = 'progid:DXImageTransform.Microsoft.AlphaImageLoader(src="' + png + '",sizingMethod="scale")';
            }
        });
    };

    var getDynamicWidth = function(popover, settings) {
        var container = popover.find('.ap_content');
        if (settings.skin == 'default' && container.length > 0) {
            var tempNode = $('<div class="ap_temp">' + container.html() + '</div>');
            tempNode.css({
                display: 'inline',
                position: 'absolute',
                top: -9999,
                left: -9999
            });
            rootElement().append(tempNode);
            var marginLeft = parseInt(container.parent().css('margin-left')) || 0;
            var marginRight = parseInt(container.parent().css('margin-right')) || 0;
            var width = tempNode.width() + marginLeft + marginRight + settings.paddingLeft + settings.paddingRight + 2;
            if (width % 2 != 0) {
                width++;
            }
            tempNode.remove();
            return Math.min(width, viewport.width());
        }
        return null;
    };

    var enableDragAndDrop = function(titlebar, popover) {
        titlebar.css('cursor', 'move');
        disableSelect(titlebar.get(0));
        titlebar.mousedown(function(e) {
            e.preventDefault();
            disableSelect(document.body);
            var offset = [e.pageX - popover.offset().left, e.pageY - popover.offset().top];
            var mousemove = function(e) {
                e.preventDefault();
                popover.css({left: e.pageX - offset[0],top: e.pageY - offset[1],margin: 0});
                if (popover.backing) {
                    popover.backing.reposition(e.pageX - offset[0], e.pageY - offset[1]);
                }
            };
            var mouseup = function(e) {
                popover.focus();
                enableSelect(document.body);
                $(document).unbind('mousemove', mousemove);
                $(document).unbind('mouseup', mouseup);
            };
            $(document).mousemove(mousemove).mouseup(mouseup);
        });
    };

    var disableSelect = function(e) {
        if (e) {
            e.onselectstart = function(e) {
                return false;
            };
            e.style.MozUserSelect = 'none';
        }
    };

    var enableSelect = function(e) {
        if (e) {
            e.onselectstart = function(e) {
                return true;
            };
            e.style.MozUserSelect = '';
        }
    };

    var boundsOfAreaElement = function(area) {
        area = jQuery(area);
        var coords = jQuery.map(area.attr('coords').split(','), function(n) {
            return Number(n);
        });
        if (area.attr('shape').match(/circle/i)) {
            coords = [coords[0] - coords[2], coords[1] - coords[2], coords[0] + coords[2], coords[1] + coords[2]];
        }
        var x = [], y = [];
        for (var i = 0; i < coords.length; i++) {
            (i % 2 == 0 ? x : y).push(coords[i]);
        }
        var min = [Math.min.apply(Math, x), Math.min.apply(Math, y)];
        var max = [Math.max.apply(Math, x), Math.max.apply(Math, y)];
        var mapName = area.parents('map').attr('name');
        var mapImg = jQuery('img[usemap=#' + mapName + ']');
        var map = mapImg.offset();
        map.left += parseInt(mapImg.css('border-left-width'));
        map.top += parseInt(mapImg.css('border-top-width'));
        return [map.left + min[0], map.top + min[1], map.left + max[0], map.top + max[1]];
    };

    $.AmazonPopover = {
        displayPopover: displayPopover,
        mouseTracker: mouseTracker,
        updateBacking: updateBacking,
        support: {
            skinCallback: true,
            controlCallbacks: true
        }
    };

    if (typeof amznJQ != 'undefined') {
        amznJQ.declareAvailable('popover');
    }
})(amznJQ.jQuery || window.jQuery);

/* end amazonJQ/amzPopover.js */

/* begin amazonJQ/navbarJQ-beacon.js */
;window.navbar = {};
(function() {
    var byID = function(elemID) {
        return document.getElementById(elemID);
    },
    nullFn = function() {
    };

    var agent = (new function() {
        function contains() {
            var args = Array.prototype.slice.call(arguments, 0);
            var regex = new RegExp("(" + args.join("|") + ")", "i");
            return regex.test(navigator.userAgent);
        }
        this.iPhone = contains("iPhone");
        this.iPad = contains("iPad");
        this.kindleFire = contains("Kindle Fire", "Silk/");
        this.touch = this.iPhone || this.iPad || this.kindleFire;
        this.webkit = contains("WebKit");
        this.mac = contains("Macintosh");
        this.iOS = this.iPhone || this.iPad;
    });

    amznJQ.available('jQuery', function() {
        var $ = jQuery;
        $.fn.navOnOptionClick = function(callback) {
            this.each(function() {
                var $this = $(this);
                if ((agent.mac && agent.webkit) || agent.touch) {
                    $this.change(function() {
                        callback.apply($this);
                    });
                }
                else {
                    var time = {click: 0, change: 0};
                    function buildClickChangeHandler(primary, secondary) {
                        return function() {
                            time[primary] = new Date().getTime();
                            if (time[primary] - time[secondary] <= 100) {
                                callback.apply($this);
                            }
                        };
                    }
                    $this.click(buildClickChangeHandler('click', 'change')).change(buildClickChangeHandler('change', 'click'));
                }
            });
        };
    });

    var navInitJSInteraction = function() {
        var $ = jQuery, $id = (function(elemID) {
            return $(byID(elemID));
        });
        var isIE6 = $.browser.msie && parseInt($.browser.version) <= 6;
        var areaMapper = (function() {
            if (!agent.kindleFire) {
                return {disable: nullFn, enable: nullFn};
            }
            var disabled = $([]);
            return {
                disable: function(except) {
                    var newMaps = $('img[usemap]').filter(function() {
                        return $(this).parents(except).length == 0;
                    });
                    disabled = disabled.add(newMaps);
                    newMaps.each(function() {
                        this.disabledUseMap = $(this).attr('usemap');
                        $(this).attr('usemap', '');
                    });
                },
                enable: function() {
                    disabled.each(function() {
                        $(this).attr('usemap', this.disabledUseMap);
                    });
                    disabled = $([]);
                }
            };
        })();
        _navbar.template = (function() {
            var cache = {};
            return function(id, data) {
                if (!cache[id]) {
                    cache[id] = new Function("obj", "var p=[],print=function(){p.push.apply(p,arguments);};" + "with(obj){p.push('" + $(id).html().replace(/[\r\t\n]/g, " ").replace(/'(?=[^#]*#>)/g, "\t").split("'").join("\\'").split("\t").join("'").replace(/<#=(.+?)#>/g, "',$1,'").split("<#").join("');").split("#>").join("p.push('") + "');}return p.join('');");
                }
                try {
                    return cache[id](data);
                }
                catch (e) {
                    return '';
                }
            };
        })();
        if (_navbar.yourAccountPrimer) {
            $('#nav_prefetch_yourorders').mousedown((function() {
                var fired = false;
                return function() {
                    if (fired) {
                        return;
                    }
                    fired = true;
                    jQuery.ajax({
                        url: _navbar.yourAccountPrimer,
                        cache: false,
                        type: "GET"
                    });
                };
            })());
        }
        function NavButton(id) {
            this._jqId = '#' + id;
            var button = this;
            byID(id).className = byID(id).className.replace('nav-menu-inactive', 'nav-menu-active');
            this.onShow = function() {
                button._toggleClass(true, 'amz-navbutton-outer-open');
            };
            this.onHide = function() {
                button._toggleClass(false, 'amz-navbutton-outer-open');
            };
            this.registerTrigger = function(options) {
                var params = this._defaultTriggerParams(button);
                $().extend(params, options || {});
                $(button._jqId).amazonPopoverTrigger(params);
            };
            this.removeTrigger = function() {
                $(button._jqId).removeAmazonPopoverTrigger();
            };
            this._toggleClass = function(state, className) {
                if (state) {
                    $(button._jqId).addClass(className);
                }
                else {
                    $(button._jqId).removeClass(className);
                }
            };
            $(button._jqId).keypress(function(e) {
                if (e.which == 13 && $(button._jqId).attr('href')) {
                    window.location = $(button._jqId).attr('href');
                }
            });
            this._defaultTriggerParams = function(button) {
                return {
                    width: null,
                    location: 'bottom',
                    locationAlign: 'left',
                    locationMargin: 0,
                    hoverShowDelay: agent.touch ? 0 : 250,
                    hoverHideDelay: agent.touch ? 0 : 250,
                    showOnHover: true,
                    forceAlignment: true,
                    focusOnShow: false,
                    skin: null,
                    onShow: button.onShow,
                    onHide: button.onHide,
                    showCloseButton: false,
                    group: 'navbar'
                };
            };
        }
        var SKIN = function(jqObject, xOffset) {
            var callback = function() {
                var navWidth = $id('nav-bar-outer').width();
                var rightMargin = Math.min(30, Math.max(0, navWidth - jqObject.offset().left - jqObject.outerWidth() - xOffset)), style = rightMargin < 30 ? ' style="width: ' + (rightMargin + 15) + 'px;"' : '', ie6class = isIE6 ? 'nav_ie6' : '';
                return '<table cellspacing="0" cellpadding="0" surround="0,' + rightMargin + ',30,30" class="' + ie6class + '">' + '<tr><td class="nav_pop_tl nav_pop_h"><div class="nav_pop_lr_min"></div></td><td class="nav_pop_tc nav_pop_h"></td><td class="nav_pop_tr nav_pop_h"' + style + '><div class="nav_pop_lr_min"' + style + "></div></td></tr>" + '<tr><td class="nav_pop_cl nav_pop_v"></td><td class="nav_pop_cc ap_content"></td><td class="nav_pop_cr nav_pop_v"' + style + '></td></tr>' + '<tr><td class="nav_pop_bl nav_pop_v"></td><td class="nav_pop_bc nav_pop_h"></td><td class="nav_pop_br nav_pop_v"' + style + '></td></tr>' + '</table>';
            };
            return $.AmazonPopover.support && $.AmazonPopover.support.skinCallback ? callback : callback();
        };
        
        var initShopByDepartments = function() {
        };

        var initYourAccount = function() {
            if (!$id('hikari-nav-account').length || !$id('nav_account_flyout')) {
                return;
            }
            var id = 'hikari-nav-account', button = new NavButton(id), jQButton = $id(id);
            var onShow = function() {
                button.onShow();
                areaMapper.disable('#nav_account_flyout');
                window._navbar.loadDynamicMenusConditionally();
            }, onHide = function() {
                areaMapper.enable();
                button.onHide();
            };
            button.registerTrigger({localContent: '#nav_account_flyout', locationAlign: 'left', locationOffset: [isIE6 ? -3 : 0, 0], skin: SKIN(jQButton, 0), onShow: onShow, onHide: onHide, followLink: window._navbar.yourAccountClickable && !agent.touch});
        };
        
        var navbarAPIError = "no error";
        window.navbar.error = function() {
            return navbarAPIError;
        };

        initYourAccount();
        
        (function() {
            var mouseHookTriggered = false;
            window._navbar.loadDynamicMenusConditionally = function() {
                mouseHookTriggered = true;
                return true;
            };
        })();

        amznJQ.declareAvailable("navbarJS-beacon");
        amznJQ.declareAvailable("navbarJS-jQuery");
        amznJQ.declareAvailable("navbarJSInteraction");
    };

    amznJQ.available("popover", function() {
        amznJQ.available("navbarInline", function() {
            amznJQ.available("navbarBTF", navInitJSInteraction);
            //amznJQ.available("navbarBTFLite", navInitSearchDropdown);
        });
    });
})();

window.navbar.exposeSBD = function(expose) {
    amznJQ.available("navbarJSInteraction", function() {
        if (_navbar._initExposeSBD().ok()) {
            _navpreload._protectExposeSBD(expose);
        }
    });
};

amznJQ.declareAvailable("navbarJSLoaded");
