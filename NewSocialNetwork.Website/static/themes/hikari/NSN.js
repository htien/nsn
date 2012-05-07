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
        nsn.byId = function(id) {
            return document.getElementById(id);
        };
        nsn.$id = function(id) {
            return jQuery(nsn.byId(id));
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
        window.NSN = nsn;
    }
})(window, jQuery, undefined);

/* end NSN/NSN.js */
