document.domain = 'localhost';
document.startTime = document.startTime || (new Date).getTime();

function f(x) {
    return function() {
        x.push(arguments);
    }
}
function ch(n) {
    return String.fromCharCode(n);
}

var s = [];
var glbDebug = true,
    glbContextPath = 'http://localhost:55555',
    glbDlgOpts, glbValidateOpts,
    glbDefaultDlgId = "ajax-response";

/* begin NSN/NSN.js */
(function(window, jQuery) {
    if (jQuery) {
        jQuery.noConflict(true);
        window.jQuery || (window.jQuery = jQuery);

        var nsn = window.NSN || {};
        nsn.versionNumber = '1.0';
        nsn.versionName = 'phoenix';
        nsn.global = this;
        nsn.DEBUG = !nsn.DEBUG ^ 1;
        nsn.LOCALE = nsn.LOCALE || 'en';
        nsn.now = Date.now || function() {
            return +new Date;
        };
        nsn._s = s;
        nsn.chars = {EOL: ch(10),SQUOTE: ch(39),DQUOTE: ch(34),BACKSLASH: ch(92),YEN: ch(165)};
        nsn.strings = {};
        nsn.nullFn = function() { };
        nsn.f = nsn.prototype = {};
        nsn.fn = nsn.prototype = {};
        nsn.agent = new function() {
            function contains() {
                var args = Array.prototype.slice.call(arguments, 0);
                return RegExp('(' + args.join('|') + ')', 'i').test(navigator.userAgent);
            }
            this.iPhone = contains('iPhone');
            this.iPad = contains('iPad');
            this.kindleFire = contains('Kindle Fire', 'Silk/');
            this.touch = this.iPhone || this.iPad || this.kindleFire;
            this.webkit = contains('WebKit');
            this.mac = contains('Macintosh');
            this.iOS = this.iPhone || this.iPad;
        };
        nsn.isIE = function(ver) {
            var yes = 'undefined' != typeof jQuery.browser.msie;
            if (ver) {
                if (nsn.isNumber(ver))
                    return yes && parseInt(jQuery.browser.version, 10) == (ver < 1 ? 6 : ver);
            } else
                return yes;
        };
        nsn.isIE6 = function() {
            return nsn.isIE(6);
        };
        nsn.isNumber = function(num) {
            return 'number' == typeof num;
        };
        nsn.isBlank = function(text) {
            return text == null ||
                   (typeof text === 'string' &&
                    /^\s*$/.test(text));
        };
        nsn.isUrl = function(url) {
            return url != null &&
				   typeof url === 'string' &&
                   /^(\/|http:\/\/|https:\/\/|ftp:\/\/)/.test(url);
        };
        nsn.byId = function(id) {
            return typeof id === 'string' ? document.getElementById(id) : id;
        };
        nsn.$id = function(id) {
            return typeof id === 'string'
                        ? jQuery(nsn.byId(id))
                        : id;
        };
        nsn.url = function(path) {
            if (typeof path === 'string')
                return glbContextPath.concat(path);
            else
                return glbContextPath;
        };
        nsn.printObj = typeof JSON != 'undefined' ? JSON.stringify : function(obj) {
            var arr = [];
            jQuery.each(obj, function(key, val) {
                var next;
                next = key + ': ' + (jQuery.isPlainObject(val) ? printObj(val) : val);
                arr.push(next);
            });
            return '{ ' + arr.join(', ') + ' }';
        };
        nsn.getJSON = function(path) {
            var json;
            jQuery.ajax({
                url: path,
                async: false,
                dataType: 'json',
                success: function(data) {
                    json = data;
                }
            });
            return json;
        };
        nsn.ajaxSubmit = function(form) {
            var jqXHR = nsn.f.ajaxSubmit(form);
            return jqXHR;
        };
        nsn.createJqDlg = function(id, data, dlgOpts) {
            var dlg = (nsn.byId(id) == null)
                      ? jQuery('<div id="' + id + '"></div>')
                      : nsn.$id(id);
            dlg.html(data);
            dlg.dialog(glbDlgOpts); // apply default dialog options
            if (typeof dlgOpts === 'object') {
                dlg.dialog(dlgOpts);
            }
            return dlg;
        };
        nsn.callJqDlg = function(id, url, settings, dlgOpts) {
            var isUrl = nsn.isUrl(url);
            if (dlgOpts == undefined) {
                dlgOpts = settings;
                settings = isUrl ? {} : undefined;
            }
            if (typeof settings === 'object') {
                settings.async = false;
            }
            var responseText = isUrl
                    ? jQuery.ajax(url, settings).responseText
                    : '<p class="nsn-popup-msg">' + url + '</p>';
            return nsn.createJqDlg(id, responseText, dlgOpts);
        };
        nsn.shakeContainer = function(container) {
            nsn.$id(container).stop(true, true).effect('shake', { times: 2, distance: 5 }, 50);
        };

        /* NSN.f prototype */
        nsn.f.ajaxSubmit = function(form) {
            var jqForm = form instanceof HTMLFormElement ? jQuery(form) : form,
                settings = {
                    async: false,
                    type: jqForm.attr('method'),
                    url: jqForm.attr('action'),
                    data: jqForm.serialize()
                };
            return jQuery.ajax(settings);
        };

        window.NSN = nsn;
    }
})(window, jQuery, undefined);

/* end NSN/NSN.js */

/* begin NSN/init.js */
(function(nsn, jQuery) {
    glbDlgOpts = {
        title: 'New Social Network',
        autoOpen: false,
        draggable: false,
        modal: true,
        resizable: false,
        position: 'center',
        minWidth: 390,
        minHeight: 190,
        width: 'auto',
        height: 'auto',
        closeOnEscape: false,
        open: function(evt, ui) {
            jQuery('.ui-dialog-titlebar-close').remove();
        },
        buttons: {
            'Close': function() {
                jQuery(this).dialog('destroy');
            }
        }
    };
    glbValidateOpts = {
        debug: glbDebug,
        errorClass: 'error', validClass: 'valid',
        onkeyup: false, onfocusout: false,
        highlight : function(el, errorClass, validClass) {
            jQuery(el).addClass(errorClass).removeClass(validClass);
            jQuery(el).next('span[class~=' + el.id + ']').addClass(errorClass);
        },
        unhighlight: function(el, errorClass, validClass) {
            jQuery(el).addClass(validClass).removeClass(errorClass);
            jQuery(el.form).find('span[class~=' + el.id + ']').removeClass(errorClass);
        }
    };
	glbDefaultDlgId = 'ajax-response';

	jQuery.ajaxSetup({
		scriptCharset: 'UTF-8',
		statusCode: {
			400: function() {
				nsn.callJqDlg(glbDefaultDlgId, 'The request cannot be fulfilled due to bad syntax.', {title: 'NSN: HTTP Response 400'}).dialog('open');
			},
			404: function() {
				nsn.callJqDlg(glbDefaultDlgId, 'The requested resource could not be found but may be available again in the future.', {title: 'NSN: HTTP Response 404'}).dialog('open');
			},
			500: function() {
				nsn.callJqDlg(glbDefaultDlgId, 'Internal Server Error. Please try again later.', {title: 'NSN: HTTP Response 500'}).dialog('open');
			}
		}
	});
	jQuery.validator.setDefaults(glbValidateOpts);
	jQuery.validator.addMethod('vietnameseDate', function(value, element) {
		return value.match(/^\d\d\d\d\/\d\d?\/\d\d?$/);
	}, 'Required yyyy/MM/dd.');
})(NSN, jQuery);

/* end NSN/init.js */
