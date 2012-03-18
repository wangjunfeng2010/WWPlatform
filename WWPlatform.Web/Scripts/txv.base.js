window.QZFL = window.QZFL || {};
QZFL.version = "2.0.9.2";
QZFL._qzfl = 2.092;

QZFL.lang = {
    isString: function (o) {
        return QZFL.object.getType(o) == "string";
    },
    isArray: function (o) {
        return QZFL.object.getType(o) == "array";
    },
    isFunction: function (o) {
        return QZFL.object.getType(o) == "function";
    },
    isHashMap: function (o) {
        return QZFL.object.getType(o) == "object";
    },
    isNode: function (o) {
        if (typeof (Node) == "undefined") {
            Node = null;
        }
        try {
            if (!o || !((Node != undefined && o instanceof Node) || o.nodeName)) {
                return false;
            }
        } catch (ignored) {
            return false;
        }
        return true;
    },
    isElement: function (o) {
        return o && o.nodeType == 1;
    }
};
QZFL.userAgent = (function () {
    var t, _1, _2, _3, _4, _5, _6, _7, _8, _9, _a, _b, _c, _d, _e, _f = navigator.userAgent,
        _10 = navigator.appVersion,
        _11 = /(?:Firefox|GranParadiso|Iceweasel|Minefield).(\d+\.\d+)/i,
        _12 = /AppleWebKit.(\d+\.\d+)/i,
        _13 = /Chrome.(\d+\.\d+)/i,
        _14 = /Version.(\d+\.\d+)/i,
        _15 = /Windows.+?(\d+\.\d+)/,
        _1 = _2 = _3 = _4 = _5 = _6 = _7 = _8 = NaN;
    _9 = _a = _b = _c = _e = false;
    if (window.ActiveXObject) {
        _1 = 9 - ((_10.indexOf("Trident/5.0") > -1) ? 0 : 1) - (window.XDomainRequest ? 0 : 1) - (window.XMLHttpRequest ? 0 : 1);
        t = navigator.appMinorVersion;
        if (_1 > 7 && t && t.toLowerCase().indexOf("beta") > -1) {
            _b = true;
        }
    } else {
        if (document.getBoxObjectFor || typeof (window.mozInnerScreenX) != "undefined") {
            t = _f.match(_11);
            _2 = parseFloat((t && t[1]) || "3.3", 10);
        } else {
            if (!navigator.taintEnabled) {
                t = _f.match(_12);
                _5 = (t && t.length > 1) ? parseFloat(t[1], 10) : (!!document.evaluate ? (!!document.querySelector ? 525 : 420) : 419);
                if ((t = _10.match(_13)) || window.chrome) {
                    if (!t) {
                        t = _f.match(_13);
                    }
                    _7 = parseFloat((t && t[1]) || "2.0", 10);
                }
                if ((t = _10.match(_14)) && !window.chrome) {
                    if (!t) {
                        t = _f.match(_14);
                    }
                    _4 = parseFloat((t && t[1]) || "3.3", 10);
                }
                if (_f.indexOf("AdobeAIR") > -1) {
                    _6 = 1;
                }
                if (_f.indexOf("iPad") > -1) {
                    _c = true;
                }
                if (_f.indexOf("iPhone") > -1) {
                    _d = true;
                }
            } else {
                if (window.opera) {
                    _3 = parseFloat(_10, 10);
                } else {
                    _1 = 6;
                }
            }
        }
    }
    if (_f.indexOf("Windows") > -1) {
        _9 = true;
        t = _f.match(_15);
        _8 = parseFloat((t && t[1]) || "5.1", 10);
    } else {
        if (_f.indexOf("Mac OS X") > -1) {
            _a = true;
        } else {
            _9 = true;
        }
    }

    function _16() {
        if (!_16.adjusted && _1 && _1 < 7) {
            try {
                document.execCommand("BackgroundImageCache", false, true);
            } catch (ignored) { }
            _16.adjusted = true;
        }
    };
    return {
        beta: _b,
        firefox: _2,
        ie: _1,
        opera: _3,
        air: _6,
        safari: _4,
        safariV: _4,
        webkit: _5,
        chrome: _7,
        adjustBehaviors: _16,
        windows: _8 || _9,
        isiPad: _c,
        isiPhone: _d,
        macs: _a
    };
})();
QZFL.object = {
    map: function (_17, _18) {
        _18 = _18 || window;
        QZFL.object.extend(_18, _17);
    },
    extend: function () {
        var _19 = arguments[0] || {},
            i = 1,
            _1a = arguments.length,
            _1b = false,
            _1c, _1d, src, _1e;
        if (typeof _19 === "boolean") {
            _1b = _19;
            _19 = arguments[1] || {};
            i = 2;
        }
        if (typeof _19 !== "object" && QZFL.object.getType(_19) !== "function") {
            _19 = {};
        }
        if (_1a === i) {
            _19 = QZFL;
            --i;
        }
        for (; i < _1a; i++) {
            if ((_1c = arguments[i]) != null) {
                for (_1d in _1c) {
                    src = _19[_1d];
                    _1e = _1c[_1d];
                    if (_19 === _1e) {
                        continue;
                    }
                    if (_1b && _1e && typeof _1e === "object" && !_1e.nodeType) {
                        var _1f;
                        if (src) {
                            _1f = src;
                        } else {
                            if (QZFL.lang.isArray(_1e)) {
                                _1f = [];
                            } else {
                                if (QZFL.object.getType(_1e) === "object") {
                                    _1f = {};
                                } else {
                                    _1f = _1e;
                                }
                            }
                        }
                        _19[_1d] = QZFL.object.extend(_1b, _1f, _1e);
                    } else {
                        if (_1e !== undefined) {
                            _19[_1d] = _1e;
                        }
                    }
                }
            }
        }
        return _19;
    },
    each: function (obj, _20) {
        var _21, _22, i = 0,
            _23 = obj.length,
            _24 = (_23 === undefined) || (typeof (obj) == "function");
        if (_24) {
            for (_21 in obj) {
                if (_20.call(obj[_21], obj[_21], _21, obj) === false) {
                    break;
                }
            }
        } else {
            for (_22 = obj[0]; i < _23 && _20.call(_22, _22, i, obj) !== false; _22 = obj[++i]) { }
        }
        return obj;
    },
    getType: function (obj) {
        return obj === null ? "null" : (obj === undefined ? "undefined" : Object.prototype.toString.call(obj).slice(8, -1).toLowerCase());
    },
    routeRE: /([\d\w_]+)/g,
    route: function (obj, _25) {
        obj = obj || {};
        _25 += "";
        var r = QZFL.object.routeRE,
            m;
        r.lastIndex = 0;
        while ((m = r.exec(_25)) !== null) {
            obj = obj[m[0]];
            if (obj === undefined || obj === null) {
                break;
            }
        }
        return obj;
    },
    bind: function (obj, fn) {
        var _26 = Array.prototype.slice.call(arguments, 2);
        return function () {
            var _27 = obj || this,
                _28 = _26.concat(Array.prototype.slice.call(arguments, 0));
            if (typeof (fn) == "string") {
                if (_27[fn]) {
                    return _27[fn].apply(_27, _28);
                }
            } else {
                return fn.apply(_27, _28);
            }
        };
    }
};
QZFL.config = {
    debugLevel: 0,
    defaultDataCharacterSet: "GB2312",
    //DCCookieDomain: "qzone.qq.com",
    domainPrefix: "culture-visions.com"
};
QZFL.string = {
    RegExps: {
        trim: /^\s+|\s+$/g,
        ltrim: /^\s+/,
        rtrim: /\s+$/,
        nl2br: /\n/g,
        s2nb: /[\x20]{2}/g,
        URIencode: /[\x09\x0A\x0D\x20\x21-\x29\x2B\x2C\x2F\x3A-\x3F\x5B-\x5E\x60\x7B-\x7E]/g,
        escHTML: {
            re_amp: /&/g,
            re_lt: /</g,
            re_gt: />/g,
            re_apos: /\x27/g,
            re_quot: /\x22/g
        },
        escString: {
            bsls: /\\/g,
            nl: /\n/g,
            rt: /\r/g,
            tab: /\t/g
        },
        restXHTML: {
            re_amp: /&amp;/g,
            re_lt: /&lt;/g,
            re_gt: /&gt;/g,
            re_apos: /&(?:apos|#0?39);/g,
            re_quot: /&quot;/g
        },
        write: /\{(\d{1,2})(?:\:([xodQqb]))?\}/g,
        isURL: /^(?:ht|f)tp(?:s)?\:\/\/(?:[\w\-\.]+)\.\w+/i,
        cut: /[\x00-\xFF]/,
        getRealLen: {
            r0: /[^\x00-\xFF]/g,
            r1: /[\x00-\xFF]/g
        },
        format: /\{([\d\w\.]+)\}/g
    },
    commonReplace: function (s, p, r) {
        return s.replace(p, r);
    },
    listReplace: function (s, l) {
        if (QZFL.lang.isHashMap(l)) {
            for (var i in l) {
                s = QZFL.string.commonReplace(s, l[i], i);
            }
            return s;
        } else {
            return s + "";
        }
    },
    trim: function (str) {
        return QZFL.string.commonReplace(str + "", QZFL.string.RegExps.trim, "");
    },
    ltrim: function (str) {
        return QZFL.string.commonReplace(str + "", QZFL.string.RegExps.ltrim, "");
    },
    rtrim: function (str) {
        return QZFL.string.commonReplace(str + "", QZFL.string.RegExps.rtrim, "");
    },
    nl2br: function (str) {
        return QZFL.string.commonReplace(str + "", QZFL.string.RegExps.nl2br, "<br />");
    },
    s2nb: function (str) {
        return QZFL.string.commonReplace(str + "", QZFL.string.RegExps.s2nb, "&nbsp;&nbsp;");
    },
    URIencode: function (str) {
        var cc, ccc;
        return (str + "").replace(QZFL.string.RegExps.URIencode, function (a) {
            if (a == " ") {
                return "+";
            } else {
                if (a == "\r") {
                    return "";
                }
            }
            cc = a.charCodeAt(0);
            ccc = cc.toString(16);
            return "%" + ((cc < 16) ? ("0" + ccc) : ccc);
        });
    },
    escHTML: function (str) {
        var t = QZFL.string.RegExps.escHTML;
        return QZFL.string.listReplace((str + ""), {
            "&amp;": t.re_amp,
            "&lt;": t.re_lt,
            "&gt;": t.re_gt,
            "&#039;": t.re_apos,
            "&quot;": t.re_quot
        });
    },
    escString: function (str) {
        var t = QZFL.string.RegExps.escString,
            h = QZFL.string.RegExps.escHTML;
        return QZFL.string.listReplace((str + ""), {
            "\\\\": t.bsls,
            "\\n": t.nl,
            "": t.rt,
            "\\t": t.tab,
            "\\'": h.re_apos,
            "\\\"": h.re_quot
        });
    },
    restHTML: function (str) {
        if (!QZFL.string.restHTML.__utilDiv) {
            QZFL.string.restHTML.__utilDiv = document.createElement("div");
        }
        var t = QZFL.string.restHTML.__utilDiv;
        t.innerHTML = (str + "");
        if (typeof (t.innerText) != "undefined") {
            return t.innerText;
        } else {
            if (typeof (t.textContent) != "undefined") {
                return t.textContent;
            } else {
                if (typeof (t.text) != "undefined") {
                    return t.text;
                } else {
                    return "";
                }
            }
        }
    },
    restXHTML: function (str) {
        var t = QZFL.string.RegExps.restXHTML;
        return QZFL.string.listReplace((str + ""), {
            "<": t.re_lt,
            ">": t.re_gt,
            "'": t.re_apos,
            "\"": t.re_quot,
            "&": t.re_amp
        });
    },
    write: function (_2a, _2b) {
        if (arguments.length < 1 || !QZFL.lang.isString(_2a)) {
            return "";
        }
        var _2c = QZFL.lang.arg2arr(arguments),
            _2d = _2c.shift(),
            tmp;
        return _2d.replace(QZFL.string.RegExps.write, function (a, b, c) {
            b = parseInt(b, 10);
            if (b < 0 || (typeof _2c[b] == "undefined")) {
                return "(n/a)";
            } else {
                if (!c) {
                    return _2c[b];
                } else {
                    switch (c) {
                        case "x":
                            return "0x" + _2c[b].toString(16);
                        case "o":
                            return "o" + _2c[b].toString(8);
                        case "d":
                            return _2c[b].toString(10);
                        case "Q":
                            return "\"" + _2c[b].toString(16) + "\"";
                        case "q":
                            return "`" + _2c[b].toString(16) + "'";
                        case "b":
                            return "<" + !!_2c[b] + ">";
                    }
                }
            }
        });
    },
    isURL: function (s) {
        return QZFL.string.RegExps.isURL.test(s);
    },
    customEncode: function (s, _2e) {
        var r;
        if (typeof _2e == "undefined") {
            _2e = "";
        }
        switch (_2e.toUpperCase()) {
            case "URICPT":
                r = encodeURIComponent(s);
                break;
            default:
                r = encodeURIComponent(s);
        }
        return r;
    },
    escapeURI: function (s) {
        if (window.encodeURIComponent) {
            return encodeURIComponent(s);
        }
        if (window.escape) {
            return escape(s);
        }
        return "";
    },
    fillLength: function (_2f, _30, ch, _31) {
        _2f = _2f + "";
        if (_2f.length < _30) {
            var res = (1 << (_30 - _2f.length)).toString(2).substring(1);
            if (ch) {
                res = res.replace("0", ch);
            }
            return _31 ? (_2f + res) : (res + _2f);
        } else {
            return _2f;
        }
    },
    cut: function (str, _32, _33) {
        str += "";
        _32 -= 0;
        _33 = _33 || "";
        if (isNaN(_32)) {
            return str;
        }
        var len = str.length,
            i = Math.min(Math.floor(_32 / 2), len),
            cnt = QZFL.string.getRealLen(str.slice(0, i));
        for (; i < len && cnt < _32; i++) {
            cnt += 1 + !QZFL.string.RegExps.cut.test(str.charAt(i));
        }
        return str.slice(0, cnt > _32 ? i - 1 : i) + (i < len ? _33 : "");
    },
    getRealLen: function (s, _34) {
        if (typeof (s) != "string") {
            return 0;
        }
        if (!_34) {
            return s.replace(QZFL.string.RegExps.getRealLen.r0, "**").length;
        } else {
            var cc = s.replace(QZFL.string.RegExps.getRealLen.r1, "");
            return (s.length - cc.length) + (encodeURI(cc).length / 3);
        }
    },
    format: function (str) {
        var _35 = Array.prototype.slice.call(arguments),
            v;
        str = _35.shift() + "";
        if (_35.length == 1 && typeof (_35[0]) == "object") {
            _35 = _35[0];
        }
        QZFL.string.RegExps.format.lastIndex = 0;
        return str.replace(QZFL.string.RegExps.format, function (m, n) {
            v = QZFL.object.route(_35, n);
            return v === undefined ? m : v;
        });
    }
};
QZFL.cookie = {
    set: function (name, _1b2, _1b3, path, hour) {
        if (hour) {
            var _1b4 = new Date();
            _1b4.setTime(_1b4.getTime() + 3600000 * hour);
        }
        document.cookie = name + "=" + _1b2 + "; " + (hour ? ("expires=" + _1b4.toGMTString() + "; ") : "") + (path ? ("path=" + path + "; ") : "path=/; ") + (_1b3 ? ("domain=" + _1b3 + ";") : ("domain=" + QZFL.config.domainPrefix + ";"));
        return true;
    },
    get: function (name) {
        var r = new RegExp("(?:^|;+|\\s+)" + name + "=([^;]*)"),
            m = document.cookie.match(r);
        return (!m ? "" : m[1]);
    },
    del: function (name, _1b5, path) {
        document.cookie = name + "=; expires=Mon, 26 Jul 1997 05:00:00 GMT; " + (path ? ("path=" + path + "; ") : "path=/; ") + (_1b5 ? ("domain=" + _1b5 + ";") : ("domain=" + QZFL.config.domainPrefix + ";"));
    }
};
QZFL.string.StringBuilder = function () {
    this._strList = QZFL.lang.arg2arr(arguments);
};
QZFL.string.StringBuilder.prototype = {
    append: function (str) {
        if (QZFL.lang.isString(str)) {
            this._strList.push(str.toString());
        }
    },
    insertFirst: function (str) {
        if (QZFL.lang.isString(str)) {
            this._strList.unshift(str.toString());
        }
    },
    appendArray: function (arr) {
        if (QZFL.lang.isArray(arr)) {
            this._strList = this._strList.concat(arr);
        }
    },
    toString: function (_17e) {
        return this._strList.join(!_17e ? "" : _17e);
    },
    clear: function () {
        this._strList.splice(0, this._strList.length);
    }
};
QZFL.lang.isValidXMLdom = function (o) {
    return !!(o && o.xml && /^<\?xml/.test(o.xml));
};
QZFL.lang.arg2arr = function (_17f, _180) {
    return Array.prototype.slice.apply(_17f, [_180 || 0, _17f.length]);
};
QZFL.lang.getObjByNameSpace = function (ns, _181) {
    if (typeof (ns) != "string") {
        return ns;
    }
    var l = ns.split("."),
        r = window;
    try {
        for (var i = 0, len = l.length; i < len; ++i) {
            if (typeof (r[l[i]]) == "undefined") {
                if (_181) {
                    r[l[i]] = {};
                } else {
                    return void (0);
                }
            }
            r = r[l[i]];
        }
        return r;
    } catch (ignore) {
        return void (0);
    }
};
QZFL.lang.objectClone = function (obj, _182) {
    if ((typeof obj) == "object") {
        var res = (QZFL.lang.isArray(obj)) ? [] : {};
        for (var i in obj) {
            if (i != _182) {
                res[i] = QZFL.lang.objectClone(obj[i], _182);
            }
        }
        return res;
    } else {
        if ((typeof obj) == "function") {
            return Object;
        }
    }
    return obj;
};
QZFL.lang.obj2str = function (obj) {
    var t, sw;
    if ((typeof obj) == "object") {
        if (obj === null) {
            return "null";
        }
        sw = QZFL.lang.isArray(obj);
        t = [];
        for (var i in obj) {
            t.push((sw ? "" : ("\"" + QZFL.string.escString(i) + "\":")) + obj2str(obj[i]));
        }
        t = t.join();
        return sw ? ("[" + t + "]") : ("{" + t + "}");
    } else {
        if ((typeof obj) == "function") {
            return "";
        } else {
            if ((typeof obj) == "undefined") {
                return "undefined";
            } else {
                if ((typeof obj) == "number") {
                    return obj.toString();
                }
            }
        }
    }
    return !obj ? "\"\"" : ("\"" + QZFL.string.escString(obj) + "\"");
};
QZFL.lang.propertieCopy = function (s, b, _183, _184) {
    var l = (!_183 || typeof (_183) != "object") ? b : _183;
    s = s || {};
    for (var p in l) {
        if (!_184 || !(p in s)) {
            s[p] = l[p];
        }
    }
    return s;
};
QZFL.lang.tryThese = function () {
    for (var res, i = 0, len = arguments.length; i < len; i++) {
        try {
            return arguments[i]();
        } catch (ignore) { }
    }
    return null;
};
QZFL.lang.chain = function (u, v) {
    var _185 = Array.prototype.slice.call(arguments);
    return function () {
        for (var i = 0, len = _185.length; i < len; i++) {
            if (_185[i] && _185[i].apply(null, arguments) === false) {
                return false;
            }
        }
        return true;
    };
};
QZFL.lang.uniqueArray = function (arr) {
    if (!QZFL.lang.isArray(arr)) {
        return arr;
    }
    var flag = {},
        _186 = 0;
    while (_186 < arr.length) {
        if (flag[arr[_186]] == typeof (arr[_186])) {
            arr.splice(_186, 1);
            continue;
        }
        flag[arr[_186].toString()] = typeof (arr[_186]);
        ++_186;
    }
    return arr;
};
if (typeof Live == "undefined" || !Live) {
    var Live;
    Live = QZFL;
    Live.isLoaded = false;
    Live.ver = "$Rev: 8562 $";
    Live.lastModifed = "$Date: 2011-07-01 14:50:48 +0800 (周五, 1 七月 2011) $";
}

Live.lazyLoad = (function () {
    var _209 = {};
    var _20a = [];
    var _20b = 0;
    var _20c = -1;
    var _20d;
    var _20e;
    var _20f = {};
    var _210 = 0;
    var _211 = true;

    function _212(tags, _213) {
        if (_213) {
            _20d = _213;
        } else {
            _20d = document.compatMode == "BackCompat" ? document.body : document.documentElement;
        }
        _20e = tags || ["img", "iframe"];
    };

    function _214() {
        var _215 = [];
        for (var i = 0, len = _20e.length; i < len; i++) {
            var el = $(_20e[i] + "[lz_src]"); //.elements;
            for (var j = 0, len2 = el.length; j < len2; j++) {
                _215.push(el[j]);
            }
        }
        for (var key = 0, len = _215.length; key < len; key++) {
            if (typeof (_215[key]) == "object" && _215[key].getAttribute("lz_src")) {
                _20a.push(_215[key]);
            } else { }
        }
        for (var i = 0, len = _20a.length; i < len; i++) {
            var _216 = _20a[i];
            _216.style.visibility = "hidden";
            var _217 = Math.round(QZFL.dom.getXY(_216)[1]);
            if (_209[_217]) {
                _209[_217].push(i);
            } else {
                var _218 = [];
                _218[0] = i;
                _209[_217] = _218;
                _20b++;
            }
        }
    };

    function _219() {
        var _21a = window.pageYOffset || _20d.scrollTop;
        return _20d.clientHeight + _21a;
    };

    function _21b() {
        _210++;
        if (!_20b) {
            _211 = true;
            return;
        }
        var _21c = _219();
        if (_20c >= _21c) {
            setTimeout(_21b, 200);
            return;
        }
        _20c = _21c;
        for (var key in _209) {
            if (_21c > key) {
                var t_o = _209[key];
                var _21d = t_o.length;
                for (var l = 0; l < _21d; l++) {
                    _20a[t_o[l]].src = _20a[t_o[l]].getAttribute("lz_src");
                    _20a[t_o[l]].style.visibility = "visible";
                }
                delete _209[key];
                _20b--;
            } else { }
        }
        for (var key in _20f) {
            if (_21c > key) {
                var t_o = _20f[key];
                var _21d = t_o.length;
                for (var l = 0; l < _21d; l++) {
                    t_o[l]();
                }
                delete _20f[key];
                _20b--;
            }
        }
        setTimeout(_21b, 200);
    };

    function _21e() {
        for (var i = 0, len = _20a.length; i < len; i++) {
            _20a[i].src = _20a[i].getAttribute("lz_src");
            _20a[i].style.visibility = "visible";
        }
        delete _20a;
        delete _209;
        _20b = 0;
    };

    function init(tags, _21f) {
        _212(tags, _21f);
        _214();
        _211 = false;
        setTimeout(function () {
            _21b();
        }, 0);
    };

    function _220(_221, _222, fix) {
        fix = fix || 0;
        if (!_221.getXY) {
            _221 = $e(_221);
        }
        var _223 = _221.getXY()[1] + fix;
        if (_20f[_223]) {
            _20f[_223].push(_222);
        } else {
            var _224 = [];
            _224[0] = _222;
            _20f[_223] = _224;
            _20b++;
        }
        if (_211) {
            _211 = false;
            _21b();
        }
    };
    return {
        init: init,
        lazyFunc: _220,
        getScreenShowHeight: _219
    };
})();

//Pager
Live.Pager = function () {
    this.eleContainer = null;
    this.alwayShow = false;
    this.totalPage = 0;
    this.pageSize = Live.Pager.Const.PG_DEF_PAGESIZE;
    this.previousPage = 0;
    this.currentPage = 0;
    this.offsetPage = Live.Pager.Const.PG_DEF_OFFSETSIZE;
    this.staus = Live.Pager.Const.PG_STATUS_READY;
    this.initialized = false;
    this.onTurnPage = function (page, pagesize) {
        return true;
    };
    this.totalCnt = 0;
    this.id = Live.Pager.maxId;
    this.hideEmpty = true;
    this.Templet =
    {
        CONTENT: '<form method="get" action="##" id="pager_form_{id}">{begin}<p class="mod_pagenav_main">{previous}<span class="mod_pagenav_count">{first}{prevdot}{pager}{nextdot}{last}</span>{pagecount}{summary}{next}</p><p class="mod_pagenav_option"><span class="mod_pagenav_turn">{goinput}{gobutton}</span></p></form>',
        FIRST_DISABLE: '', FIRST_ENABLE: '<a href="##" title="1" id="pager_first_{id}"><span>1</span></a>\n',
        LAST_DISABLE: '', LAST_ENABLE: '<a href="##" title="{totpage}" id="pager_last_{id}"><span>{totpage}</span></a>\n',
        PREVIOUS_DISABLE: '<span class="mod_pagenav_disable"><span>上一页</span></span>\n', PREVIOUS_ENABLE: '<a href="##" id="pager_previous_{id}" title="上一页"><span>上一页</span></a>\n',
        NEXT_DISABLE: '<span class="mod_pagenav_disable"><span>下一页</span></span>\n', NEXT_ENABLE: '<a href="##" title="下一页" id="pager_next_{id}"><span>下一页</span></a>\n',
        PAGE_NORMAL: '<a href="##" id="pager_num_{id}_{pagenum}" title="{pagenum}"><span>{pagenum}</span></a>\n',
        PAGE_CURRENT: '<span class="current"><span>{pagenum}</span></span>\n', GO_PAGE_INPUT: '<span class="mod_pagenav_turn">转到<input type="text" id="pager_input_{id}" name="PageCount" autocomplete="off"/>页\n',
        GO_PAGE_BUTTON: '<button type="submit" id="pager_btn_{id}"><span>确定</span></button>\n', PAGE_COUNT: '<span class="mod_pagenav_count2"><span class="current">{curpg}</span>/{totpage}</span>\n',
        PREVDOT_ENABLE: '<span class="mod_pagenav_more"><span>…</span></span>\n', PREVDOT_DISABLE: "",
        NEXTDOT_ENABLE: '<span class="mod_pagenav_more"><span>…</span></span>\n', NEXTDOT_DISABLE: "",
        SUMMARY: "", BEGIN: ""
    };
    Live.Pager.items[Live.Pager.maxId] = this;
    Live.Pager.maxId++;
}
Live.Pager.Const = {
    PG_STATUS_READY: 0, PG_STATUS_GO: 1, PG_DEF_OFFSETSIZE: 2, PG_DEF_PAGESIZE: 20
}
Live.Pager.maxId = 0;
Live.Pager.items = {};
Live.Pager.prototype = {
    init: function (eleContainer, totalPage) {
        if (!eleContainer || !totalPage) {
            return;
        }
        this.eleContainer = eleContainer;
        this.totalPage = totalPage;
        this.initialized = true;
    },
    fillPager: function () {
        if (!this.initialized || !this.currentPage || !this.eleContainer) {
            return;
        }
        var pageArr = [];
        var pageHTML = '';
        var start, end, i;
        var aBtn;
        this.currentPage = parseInt(this.currentPage, 10);
        this.offsetPage = parseInt(this.offsetPage);
        start = (this.currentPage - this.offsetPage > 0) ? (this.currentPage - this.offsetPage) : 1;
        end = (this.currentPage + this.offsetPage <= this.totalPage) ? (this.currentPage + this.offsetPage) : this.totalPage;
        var leftoffset, rightoffset;
        leftoffset = this.currentPage - start;
        leftoffset = leftoffset > 0 ? leftoffset : 0;
        rightoffset = end - this.currentPage;
        rightoffset = rightoffset > 0 ? rightoffset : 0;
        if (leftoffset + rightoffset < this.offsetPage * 2) {
            m = Math.abs(leftoffset - rightoffset);
            if (leftoffset < rightoffset) {
                end = end + m;
            }
            else {
                start = start - m;
            }
        }
        if (end > this.totalPage) {
            end = this.totalPage;
        }
        if (start < 1) {
            start = 1;
        }
        if (start - 1 >= 2 && this.currentPage - start >= 2) {
            start++;
        }
        if (end + 2 == this.totalPage) {
            end++;
        }
        for (i = start; i <= end && i <= this.totalPage; i++) {
            if (i == 1 && this.currentPage != 1) {
                continue;
            }
            if (i == this.totalPage && this.currentPage != this.totalPage) {
                continue;
            }
            if (i == this.currentPage) {
                pageArr.push(this.Templet.PAGE_CURRENT.replace(/\{pagenum\}/g, i));
            }
            else {
                pageArr.push(this.Templet.PAGE_NORMAL.replace(/\{pagenum\}/g, i));
            }
        }
        pageHTML = this.Templet.CONTENT.replace(/\{pager\}/g, pageArr.join(''));
        pageHTML = pageHTML.replace(/\{prevdot\}/g, start - 1 > 1 ? this.Templet.PREVDOT_ENABLE : this.Templet.PREVDOT_DISABLE);
        pageHTML = pageHTML.replace(/\{nextdot\}/g, this.totalPage - end > 1 ? this.Templet.NEXTDOT_ENABLE : this.Templet.NEXTDOT_DISABLE);
        pageHTML = pageHTML.replace(/\{first\}/g, (this.currentPage == 1) ? this.Templet.FIRST_DISABLE : this.Templet.FIRST_ENABLE);
        pageHTML = pageHTML.replace(/\{previous\}/g, (this.currentPage == 1) ? this.Templet.PREVIOUS_DISABLE : this.Templet.PREVIOUS_ENABLE);
        pageHTML = pageHTML.replace(/\{next\}/g, (this.currentPage == this.totalPage) ? this.Templet.NEXT_DISABLE : this.Templet.NEXT_ENABLE);
        pageHTML = pageHTML.replace(/\{last\}/g, (this.currentPage == this.totalPage) ? this.Templet.LAST_DISABLE : this.Templet.LAST_ENABLE);
        pageHTML = pageHTML.replace(/\{goinput\}/g, this.Templet.GO_PAGE_INPUT);
        pageHTML = pageHTML.replace(/\{gobutton\}/g, this.Templet.GO_PAGE_BUTTON);
        pageHTML = pageHTML.replace(/\{summary\}/g, this.Templet.SUMMARY);
        pageHTML = pageHTML.replace(/\{pagecount\}/g, this.Templet.PAGE_COUNT);
        pageHTML = pageHTML.replace(/\{id\}/g, this.id);
        pageHTML = pageHTML.replace(/\{begin\}/g, this.Templet.BEGIN);
        pageHTML = pageHTML.replace(/\{totpage\}/g, this.totalPage);
        pageHTML = pageHTML.replace(/\{curpg\}/g, this.currentPage);
        pageHTML = pageHTML.replace(/\{pagenum\}/g, this.currentPage);
        pageHTML = pageHTML.replace(/\{totalcnt\}/g, this.totalCnt);
        if (this.totalPage <= 1 && this.alwayShow == false) {
            this.eleContainer.hide();
        }
        else {
            this.eleContainer.show();
        }
        this.eleContainer.html(pageHTML);

        var self = this;
        for (i = start; i <= end; i++) {
            if (aBtn = $('#pager_num_' + this.id + '_' + i)) {
                aBtn.bind("click", { pageindex: i }, function (event) {
                    self.goPage(event.data.pageindex);
                });
            }
        }
        if (aBtn = $('#pager_first_' + this.id)) {
            aBtn.bind("click", function () {
                self.goPage(1);
            });
        }
        if (aBtn = $('#pager_previous_' + this.id)) {
            aBtn.bind("click", function () {
                self.goPage(self.currentPage - 1);
            });
        }
        if (aBtn = $('#pager_next_' + this.id)) {
            aBtn.bind("click", function () {
                self.goPage(self.currentPage + 1);
            });
        }
        if (aBtn = $('#pager_last_' + this.id)) {
            aBtn.bind("click", function () {
                self.goPage(self.totalPage);
            });
        }
        if (aBtn = $('#pager_form_' + this.id)) {
            $('pager_form_' + this.id).onsubmit = function () {
                if (self.checkPage()) {
                    var $ele = $('#pager_input_' + self.id);
                    var ijumppage = QZFL.string.trim($ele.getVal());
                    self.goPage(ijumppage);
                    return false;
                }
                return false;
            };
        }
    },
    goPage: function (page) {
        page = parseInt(page, 10);
        if (isNaN(page)) {
            page = 1;
        }
        if (page < 1) {
            page = 1;
        }
        else if (page > this.totalPage) {
            page = this.totalPage;
        }
        this.previousPage = this.currentPage;
        this.currentPage = page;
        if (typeof this.onTurnPage == 'function') {
            if (!this.onTurnPage(this.currentPage, this.pageSize)) {
                return false;
            }
        }
        this.fillPager();
        event.preventDefault();
        return false;
    },
    checkPage: function () {
        var $ele = $('#pager_input_' + this.id);
        var ijumppage = QZFL.string.trim($ele.getVal());
        if (ijumppage.length == 0) {
            this.showError("您未输入页码，请输入数字页码！");
            $ele.focus();
            $ele.elements[0].select();
            return false;
        }
        if (/^\d+$/.test(ijumppage) == false) {
            this.showError("您输入的页码不正确，请重新输入数字页码！");
            $ele.elements[0].focus();
            $ele.elements[0].select();
            return false;
        }
        ijumppage = parseInt(ijumppage, 10);
        if (ijumppage <= 0 || ijumppage > this.totalPage) {
            this.showError("您输入的页码不存在，请重新输入数字页码！");
            $ele.elements[0].focus();
            $ele.elements[0].select();
            return false;
        }
        return true;
    },
    showError: function (msg) {
        alert(msg);
    },
    refresh: function () {
        this.goPage(this.currentPage);
    }
}

Live = Live || {};
Live.txv = {
    isLoaded: false,
    ver: "$Rev: 8562 $",
    lastModifed: "$Date: 2011-07-01 14:50:48 +0800 (周五, 1 七月 2011) $",
    isDebug: false
};

var txv = window.txv || Live.txv;

Live.txv.path = {
    trimpath: "/scripts/txv.trimpath.js",
    search_cgi: "/search.html",
    search_suggest: "/fcgi-bin/suggest?ms_key="
};
// su->siteuser
Live.txv.su = {
    config: {
        openidcookie: "openid",
        nickcookie: "lw_nick",
        loginurl: "/qq/login",
        logouturl: "/qq/logout",
        //hook methods,用于回调
        rejectLogin: null,
        success: null,
        logout: null
    },
    init: function (_252, _253) {
        _253 = !!_253;
        Live.object.extend(this.config, _252);
        txv.su.setStatus(_253, false);
    },
    isOpened: false,
    isLogin: function () {
        return txv.su.getOpenid() != "";
    },
    setStatus: function (_254, _255) {
        if (txv.su.getOpenid() == "") {
            txv.su.logout();
        }

        if (txv.su.getNick() != "") {
            txv.su.showOnline();
            if (_254 == true && typeof txv.su.config.success == "function") {
                txv.su.config.success();
            }
        }
    },
    getOpenid: function () {
        return Live.string.trim(Live.cookie.get(txv.su.config.openidcookie));
    },
    getNick: function () {
        var n = Live.cookie.get(txv.su.config.nickcookie);
        if (n == "")
            return n;

        var arr = n.split("|");
        if (arr[1] && arr[1] == this.getOpenid()) {
            return decodeURIComponent(arr[0]);
        }

        return "";
    },
    showOnline: function (_256) {
        var _257 = $("#login_nick");
        _257.text(["欢迎你,", txv.su.getNick()].join(""));
        var _258 = $("#login_action");
        _258.text("[退出]");
        _258.bind("click", txv.su.logout);
    },
    showNotLogin: function () {
        var _259 = $("#login_nick");
        _259.text("你好，欢迎来到文化中国！请先");

        var _25a = $("#login_action");
        _25a.text("登录");
        _25a.bind("click", txv.su.openLogin);
    },
    openLogin: function (_25e) {
        txv.su.isOpened = true;
        Live.object.extend(txv.su.config, _25e);

        window.open(txv.su.config.loginurl, "用qq登录", "width=480,height=320,menubar=0,scrollbars=0, status=1,titlebar=0,toolbar=0,location=1");
    },
    logout: function () {
        //Live.cookie.set(txv.su.config.openidcookie, "", location.host, "/", -24);
        //Live.cookie.set(txv.su.config.nickcookie, "", location.host, "/", -24);
        $.get(
            txv.su.config.logouturl
        );
        txv.su.showNotLogin();
        if (typeof txv.su.config.logout == "function") {
            txv.su.config.logout();
        }
    }
}

//登录成功后的可以回调此函数，从而更新页面的登录信息
function ptlogin2_onClose(_267) {
    txv.su.isOpend = false;
    if (_267) {
        ptlogin2_onSuccess();
    } else {
        if (typeof (txv.su.config.rejectLogin) == "function") {
            txv.su.config.rejectLogin();
        }
    }
};

function ptlogin2_onSuccess() {
    txv.su.setStatus(true, true);
};

Live.txv.template =
{
    _isLoadingJS: false, timer: {
        waitJS: 0
    },
    FILL_TYPE: {
        TPL_ID: 1, TPL_STR: 2
    },
    convertTime: function (d) {
        var month = d.getMonth() + 1;
        var day = d.getDate();
        var hour = d.getHours();
        var min = d.getMinutes();
        var sec = d.getSeconds();
        function fillZero(str) {
            return str < 10 ? "0" + str : str;
        }
        return {
            "year": d.getFullYear(), "month": month, "day": day, "hour": fillZero(hour), "min": fillZero(min),
            "sec": fillZero(sec)
        }
    },
    MODIFIERS:
    {
        formatTime: function (str) {
            if (!/\d+/.test(str)) {
                return str;
            }
            var v = parseInt(str) * 1000;
            var d = new Date(v);
            var dd = txv.template.convertTime(d);
            return [[dd.year, dd.month, dd.day].join("-"), [dd.hour, dd.min, dd.sec].join(":")].join(" ");
        },
        getTimeLong: function (time) {
            var m = parseInt(time / 60);
            var s = time % 60;
            return "" + m + "分钟" + s + "秒";
        },
        getTimeLong2: function (time) {
            var m = parseInt(time / 60);
            var s = time % 60;
            return "" + m + "&#039;" + s + "&#034;";
        },
        filterComment: function (str) {
            return str.replace(/\\r\\n/ig, "<br/>");
        },
        getPlayUrl: function (cid) {
            return txv.common.getPlayUrl(cid);
        },
        getDetailUrl: function (cid, typeid) {
            return txv.common.getDetailUrl(cid, typeid || 0);
        },
        getVideoPlayUrl: function (vid) {
            return txv.common.getVideoPlayUrl(vid);
        },
        encode: function (str) {
            return encodeURIComponent(str);
        },
        searchurl: function (tag) {
            return txv.path.search_cgi + "&ms_key=" + encodeURIComponent(tag);
        },
        getVideoSnap: function (vid) {
            return txv.common.getVideoSnap(vid, 0);
        },
        filterStr: function (str) {
            return QZFL.string.filterAngAndQt(str);
        },
        getTimeDiff: function (time) {
            if (isNaN(time)) {
                return time;
            }
            if (/^\d{10}$/.test(time)) {
                time = time * 1000;
            }
            var dd = new Date(time);
            var now = new Date();
            var todaySec = now.getHours() * 3600 + now.getMinutes() * 60 + now.getSeconds();
            var t0 = new Date().valueOf();
            var sec = parseInt((t0 - time) / 1000);
            var strtm = "";
            if (sec < 60) {
                strtm = "刚刚";
            }
            else if (sec > 60 && sec < 3600) {
                strtm = Math.ceil(sec / 60) + "分钟前";
            }
            else {
                var dataObj = txv.template.convertTime(dd);
                if (sec >= 3600 && sec < todaySec) {
                    strtm = "今天 " + dataObj.hour + ":" + dataObj.min;
                }
                else if (sec > todaySec && sec < todaySec + 86400) {
                    strtm = "昨天 " + dataObj.hour + ":" + dataObj.min;
                }
                else {
                    strtm = dataObj.month + "月" + dataObj.day + "日" + " " + dataObj.hour + ":" + dataObj.min;
                }
            }
            return strtm;
        }
    },
    fillHtml: function (json, tplid, containerid, callback, isClear, modifiers, filltype, isResetLink) {
        function fill(_json, _tplid, _containerid, _isClear, _modifiers, _filltype, _callback, _isResetLink) {
            txv.template._isLoadingJS = false;
            var container = $("#" + _containerid);
            if (!container) {
                return;
            }
            if (typeof filltype == "undefined") {
                filltype = txv.template.FILL_TYPE.TPL_ID;
            }
            if ((filltype == txv.template.FILL_TYPE.TPL_ID && !$(_tplid)) || (filltype == txv.template.FILL_TYPE.TPL_STR && !_tplid)) {
                return;
            }
            if (typeof _isClear == "undefined") {
                _isClear = true;
            }
            if (typeof filltype == "undefined") {
                filltype = txv.template.FILL_TYPE.TPL_ID;
            }
            _json._MODIFIERS = _modifiers || txv.template.MODIFIERS;
            var myTemplateObj = filltype == txv.template.FILL_TYPE.TPL_ID ? TrimPath.parseDOMTemplate(_tplid) : TrimPath.parseTemplate(_tplid);
            var result = myTemplateObj.process(_json);
            container.html(result);
            if (typeof _isResetLink == "undefined") {
                _isResetLink = true;
            }
            if (typeof _callback == "function") {
                _callback();
            }
        }
        if (typeof TrimPath == "undefined") {
            if (txv.template._isLoadingJS == false) {
                txv.template._isLoadingJS = true;
                $.getScript(txv.path.trimpath, function () {
                    fill(json, tplid, containerid, isClear, modifiers, filltype, callback, isResetLink);
                });
            }
            else {
                setTimeout(function () {
                    txv.template.fillHtml(json, tplid, containerid, isClear, modifiers, filltype, callback,
                    isResetLink);
                }, 100);
            }
        }
        else {
            fill(json, tplid, containerid, isClear, modifiers, filltype, callback, isResetLink);
        }
    }
}

Live.SearchBox = function (_247) {
    this.config = {
        defaultTxt: "请输入关键词",
        inputBox: null,
        sugSelector: null,
        searchBtn: null,
        searchForm: null,
        sugCover: null
    };
    Live.object.extend(this.config, _247);
    this.selectedIndex = -1;
    this.itemsCount = 0;
    this.timer = [];
};
Live.SearchBox.prototype = {
    search_cgi: "",
    search_suggest: "",
    init: function () {
        var self = this;
        var $el = this.config.inputBox;
        $el.val(this.config.defaultTxt);
        document.sform.onsubmit = function () {
            return self.go();
        };
        this.initSmartBox();
        $el.bind("focus", function () {
            var v = QZFL.string.trim($el.val());
            if (v == self.config.defaultTxt) {
                $el.val("");
            } else {
                $el.select();
            }
            $el.addClass("onfocus");
        });
    },
    go: function () {
        //Live.tj2.button("search.btn");
        var val = QZFL.string.trim(this.config.inputBox.val());
        if (val == "" || val == this.config.defaultTxt) {
            return false;
        }
        if (document.charset != "utf-8" && Live.userAgent.ie) {
            var _248 = document.charset;
            document.charset = "utf-8";
            setTimeout(function () {
                document.charset = _248;
            }, 0);
        }
        document.sform.action = Live.SearchBox.search_cgi;
        return true;
    },
    resetInputBox: function (_249) {
        var $el = this.config.inputBox;
        var v = QZFL.string.trim($el.val());
        if (v == "" || v == this.config.defaultTxt) {
            if (_249) {
                $el.val(this.config.defaultTxt);
            }
            $el.attr("class", "mod_search_txt");
        } else {
            $el.attr("class", "mod_search_txt");
        }
    },
    initSmartBox: function () {
        var self = this;
        this.config.inputBox.bind("keyup", function (e) {
            if (e.keyCode == 38 || e.keyCode == 40) {
                clearTimeout(self.timer[0]);
                return;
            }
            self.timer[0] = setTimeout(function () {
                clearTimeout(self.timer[0]);
                self.selectedIndex = -1;
                self.getSuggest();
            }, 300);
        });
        this.config.inputBox.bind("keydown", function (e) {
            self.resetInputBox(false);
            $("#iWord").addClass("onfocus");
            if (e.keyCode == 38) {
                self.choose("up", true);
                return;
            } else {
                if (e.keyCode == 40) {
                    self.choose("down", true);
                    return;
                }
            }
        });
        this.config.inputBox.bind("blur", function () {
            self.hideSelector();
            self.resetInputBox(true);
        });
    },
    choose: function (key, fill) {
        fill = fill | false;
        if (typeof key == "string") {
            if (key == "up") {
                if (this.selectedIndex <= 0) {
                    this.selectedIndex = this.itemsCount;
                }
                this.selectedIndex--;
            } else {
                if (key == "down") {
                    if (this.selectedIndex >= this.itemsCount - 1) {
                        this.selectedIndex = -1;
                    }
                    this.selectedIndex++;
                }
            }
        } else {
            if (typeof key == "number") {
                this.selectedIndex = key;
            }
        }
        this.config.sugSelector.find("li.current").removeClass("current");
        var _24a = this.config.sugSelector.find("li:eq(" + this.selectedIndex + ")");
        _24a.addClass("current");
        if (!!fill) {
            this.config.inputBox.val(_24a.text());
        }
    },
    hideSelector: function () {
        this.config.sugSelector.html("");
        this.config.sugSelector.hide();
        this.config.sugCover.hide();
    },
    getSuggest: function () {
        var self = this;
        var val = QZFL.string.trim(this.config.inputBox.val());
        if (val == "") {
            this.hideSelector();
            return;
        }
        $.ajax({
            url: Live.SearchBox.search_suggest + encodeURIComponent(val),
            dataType: "json",
            async: true,
            beforeSend: function () { },
            success: function (_24b) {
                var sb = new Live.string.StringBuilder();
                self.itemsCount = _24b.head.num;
                if (self.itemsCount == 0) {
                    self.config.sugSelector.hide();
                    self.config.sugCover.hide();
                    return;
                }
                try {
                    for (var i = 0; i < _24b.items.length; i++) {
                        sb.appendArray(["<li><a href='#' target='_self'>", _24b.items[i].w, "</a></li>"]);
                    }
                    self.config.sugSelector.html(sb.toString());
                    self.config.sugSelector.show();
                    self.config.sugCover.show();
                    self.bindListMouseEvt();
                } catch (e) {
                    //txv.log(e);
                }
            }
        });
    },
    bindListMouseEvt: function () {
        var self = this;
        this.config.sugSelector.find("li").each(function (i, el) {
            var $el = $(el);
            $el.bind("mouseover", function () {
                self.choose(i);
                self.config.inputBox.unbind("blur");
            });
            $el.bind("mouseout", function () {
                self.config.inputBox.bind("blur", function () {
                    self.hideSelector();
                });
            });
            $el.bind("click", function () {
                self.choose(i, true);
                self.hideSelector();
                self.go();
                document.sform.submit();
            });
        });
    }
};
Live.SearchBox.defaultInit = function (_24c, _24d) {
    var _24e = false;
    this.search_cgi = _24c;
    this.search_suggest = _24d;
    var sb = new Live.SearchBox({
        inputBox: $("#iWord"),
        sugSelector: $("#sgt_list"),
        searchBtn: $("#sbutton"),
        searchForm: $("#sform"),
        sugCover: $("#mod_keywords_list")
    });
    sb.init();
};


Live.txv.common = {
    initPage: function (_268) {
        Live.SearchBox.defaultInit(txv.path.search_cgi, txv.path.search_suggest);
        txv.su.init((!!_268 && !!_268.login) ? _268.login : null, true);
    }
}