﻿document.domain="localhost";document.startTime=document.startTime||(new Date).getTime();function f(b){return function(){b.push(arguments)}}function ch(b){return String.fromCharCode(b)}var s=[];
(function(b,d){if(d){d.noConflict(!0);b.jQuery||(b.jQuery=d);var a=b.NSN||{};a.versionNumber="1.0";a.versionName="phoenix";a.global=this;a.DEBUG=!a.DEBUG^1;a.LOCALE=a.LOCALE||"en";a.now=Date.now||function(){return+new Date};a._s=s;a.chars={EOL:ch(10),SQUOTE:ch(39),DQUOTE:ch(34),BACKSLASH:ch(92),YEN:ch(165)};a.strings={};a.nullFn=function(){};a.fn=a.prototype={};a.agent=new function(){function a(){var c=Array.prototype.slice.call(arguments,0);return RegExp("("+c.join("|")+")","i").test(navigator.userAgent)}
this.iPhone=a("iPhone");this.iPad=a("iPad");this.kindleFire=a("Kindle Fire","Silk/");this.touch=this.iPhone||this.iPad||this.kindleFire;this.webkit=a("WebKit");this.mac=a("Macintosh");this.iOS=this.iPhone||this.iPad};a.isIE=function(c){var b="undefined"!=typeof d.browser.msie;if(c){if(a.isNumber(c))return b&&parseInt(d.browser.version,10)<=(1>c?6:c)}else return b};a.isIE6=function(){return a.isIE(6)};a.isNumber=function(a){return"number"==typeof a};a.byId=function(a){return document.getElementById(a)};
a.$id=function(b){return d(a.byId(b))};a.printObj="undefined"!=typeof JSON?JSON.stringify:function(a){var b=[];d.each(a,function(a,c){var e;e=a+": "+(d.isPlainObject(c)?printObj(c):c);b.push(e)});return"{ "+b.join(", ")+" }"};b.NSN=a}})(window,jQuery,void 0);jQuery(function(){});