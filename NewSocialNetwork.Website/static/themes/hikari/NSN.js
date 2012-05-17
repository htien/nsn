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
    glbContextPath = document.contextPath,
    glbDlgOpts, glbValidateOpts,
    glbDefaultDlgId = "ajax-response",
    glbYesLog = false;

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
        nsn._log = function(message) {
            if (glbYesLog) {
                if (typeof message == 'string') {
                    console.log(message);
                }
            }
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
                return glbContextPath == '/' ? ''.concat(path) : glbContextPath.concat(path);
            else
                return glbContextPath;
        };
        nsn.smileImage = function(category, type, name) {
            var smilesPath = nsn.url('/static/smiles/' + category + '/' + type + '/' + name);
            return smilesPath;
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
        nsn.resetForm = function(form) {
            var jqForm = nsn.$id(form);
            jqForm[0].reset();
            jqForm.find(':input[class!=noreset]:visible:enabled:first').focus();
		    return jqForm;
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
            var dlg = nsn.byId(id) == null ? jQuery('<div id="' + id + '"></div>') : nsn.$id(id);
            dlg.html(!data ? '' : data);
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
                    : url;
            return nsn.createJqDlg(id, responseText, dlgOpts);
        };
        nsn.shakeContainer = function(container) {
            nsn.$id(container).stop(true, true).effect('shake', { times: 2, distance: 5 }, 50);
        };
        nsn.fnFalse = function() {
            return false;
        };

        /*=== NSN.f prototype ===*/
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

        /** Turn off autocomplete for input text */
		nsn.f.autoCompleteOff = function() {
			nsn._log('Assigned autocomplete="off" on input[type=text].');
			jQuery('form.off').attr('autocomplete', 'off');
			jQuery('input[type=text].off').attr('autocomplete', 'off');
		};
        
        /** Prevent from dragging <link> and <image> */
        nsn.f.disableDrag = function(elements) {
			var json = jQuery.parseJSON(elements); 
			jQuery.each(json.tags, function(idx, el) {
				nsn._log('Diabled dragging on tag: "' + el + '"');
				jQuery(el).live('mousedown', nsn.fnFalse);
			});
			jQuery.each(json.classes, function(idx, el) {
				nsn._log('Diabled dragging on class: ".' + el + '"');
				jQuery('.' + el).live('mousedown', nsn.fnFalse);
			});
		};

        window.NSN = nsn;
    }
})(window, jQuery, undefined);

/* end NSN/NSN.js */

/* begin NSN/init.js */
(function(nsn, jQuery) {
    glbYesLog = glbDebug && !nsn.isIE();
    glbDlgOpts = {
        title: 'New Social Network',
        dialogClass: 'alert',
        hasTitle: true,
        autoOpen: false,
        draggable: false,
        modal: true,
        resizable: false,
        position: 'center',
        minWidth: 390,
        minHeight: 180,
        width: 450,
        height: 'auto',
        closeOnEscape: false,
        create: function(evt, ui) {
            jQuery('.ui-dialog-titlebar-close').remove();
        },
        open: function(evt, ui) {
            if (!jQuery(this).dialog("option", "hasTitle")) {
                jQuery('.ui-dialog-titlebar').remove();
            }
        },
        buttons: [
            {
                text: 'Close',
                class: 'guiBlueButton',
                click: function() {
                    jQuery(this).dialog('destroy').remove();
                }
            }
        ]
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
				nsn.callJqDlg(glbDefaultDlgId, 'The request cannot be fulfilled due to bad syntax.', {
                        title: 'NSN: HTTP Response 400',
                        width: 350
                    }).dialog('open');
			},
			404: function() {
				nsn.callJqDlg(glbDefaultDlgId, 'The requested resource could not be found but may be available again in the future.', {
                        title: 'NSN: HTTP Response 404',
                        width: 350
                    }).dialog('open');
			},
			500: function() {
				nsn.callJqDlg(glbDefaultDlgId, 'Internal Server Error. Please try again later.', {
                        title: 'NSN: HTTP Response 500',
                        width: 350
                    }).dialog('open');
			}
		}
	});
	jQuery.validator.setDefaults(glbValidateOpts);
	jQuery.validator.addMethod('vietnameseDate', function(value, element) {
		return value.match(/^\d\d\d\d\/\d\d?\/\d\d?$/);
	}, 'Required yyyy/MM/dd.');
    jQuery.validator.addMethod('gender', function(value, element) {
        return value != '0';
    }, 'Required gender.');
    jQuery.validator.addMethod('birthdayBox', function(value, element) {
        return value != '-1';
    }, 'Required.');
})(NSN, jQuery);

/* end NSN/init.js */

/* begin NSN/globals.js */
(function(nsn, jQuery) {
    buildJqConfirmDlgOpts = function(targetForm, title, callbackSuccess) {
		return {
			title: title,
			buttons: [
                {
                    text: 'OK',
                    class: 'guiBlueButton',
                    click: function(evt) {
                        buildJqConfirmDlgOpts_OK(this, targetForm, callbackSuccess);
                    }
                },
                {
                    text: 'Cancel',
                    click: function(evt) {
                        jQuery(this).dialog('destroy').remove();
                    }
                }
			]
		};
	};
    buildJqConfirmDlgOpts_OK = function(dlg, targetForm, callbackSuccess) {
	    if (!targetForm.valid()) {
		    return;
	    }
	    nsn.ajaxSubmit(targetForm)
		    .success(function(json, textCode, xhr) {
			    nsn.callJqDlg(glbDefaultDlgId, json.Message, {
				    buttons: [
                        {
                            text: 'Close',
                            class: 'guiBlueButton',
                            click: function() {
                                jQuery(dlg).dialog('destroy').remove();
                                callbackSuccess.call(this);
                            }
                        }
				    ]
			    });
			    // reset form after adding successfully
			    if (json.Action == 'add' && json.Status == '1') {
				    nsn.resetForm(targetForm);
			    }
		    })
		    .error(function(data) {
			    jQuery(this).dialog('destroy').remove();
		    });
    };
})(NSN, jQuery);

/* end NSN/globals.js */

/* begin NSN/ready.js */
jQuery(function($) {
    NSN.f.autoCompleteOff();
	NSN.f.disableDrag('{"tags":["a", "img"], "classes":["guiButton"]}');
});

/* end NSN/ready.js */