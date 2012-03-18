/*
Tencent ISD QQLive WebTeam. 
Copyright 1998 - 2011 
2011-3-9 14:46:23.224
*/
var TrimPath;
(function () {
    if (TrimPath == null) {
        TrimPath = new Object();
    }
    if (TrimPath.evalEx == null) {
        TrimPath.evalEx = function (_1) {
            return eval(_1);
        };
    }
    var _2;
    if (Array.prototype.pop == null) {
        Array.prototype.pop = function () {
            if (this.length === 0) {
                return _2;
            }
            return this[--this.length];
        };
    }
    if (Array.prototype.push == null) {
        Array.prototype.push = function () {
            for (var i = 0; i < arguments.length; ++i) {
                this[this.length] = arguments[i];
            }
            return this.length;
        };
    }
    TrimPath.parseTemplate = function (_3, _4, _5) {
        if (_5 == null) {
            _5 = TrimPath.parseTemplate_etc;
        }
        var _6 = _7(_3, _4, _5);
        var _8 = TrimPath.evalEx(_6, _4, 1);
        if (_8 != null) {
            return new _5.Template(_4, _3, _6, _8, _5);
        }
        return null;
    };
    try {
        String.prototype.process = function (_9, _a) {
            var _b = TrimPath.parseTemplate(this, null);
            if (_b != null) {
                return _b.process(_9, _a);
            }
            return this;
        };
    } catch (e) { }
    TrimPath.parseTemplate_etc = {};
    TrimPath.parseTemplate_etc.statementTag = "forelse|for|if|elseif|else|var|macro";
    TrimPath.parseTemplate_etc.statementDef = {
        "if": {
            delta: 1,
            prefix: "if (",
            suffix: ") {",
            paramMin: 1
        },
        "else": {
            delta: 0,
            prefix: "} else {"
        },
        "elseif": {
            delta: 0,
            prefix: "} else if (",
            suffix: ") {",
            paramDefault: "true"
        },
        "/if": {
            delta: -1,
            prefix: "}"
        },
        "for": {
            delta: 1,
            paramMin: 3,
            prefixFunc: function (_c, _d, _e, _f) {
                if (_c[2] != "in") {
                    throw new _f.ParseError(_e, _d.line, "bad for loop statement: " + _c.join(" "));
                }
                var _10 = _c[1];
                var _11 = "__LIST__" + _10;
                return ["var ", _11, " = ", _c[3], ";", "var __LENGTH_STACK__;", "if (typeof(__LENGTH_STACK__) == 'undefined' || !__LENGTH_STACK__.length) __LENGTH_STACK__ = new Array();", "__LENGTH_STACK__[__LENGTH_STACK__.length] = 0;", "if ((", _11, ") != null) { ", "var ", _10, "_ct = 0;", "for (var ", _10, "_index in ", _11, ") { ", _10, "_ct++;", "if (typeof(", _11, "[", _10, "_index]) == 'function') {continue;}", "__LENGTH_STACK__[__LENGTH_STACK__.length - 1]++;", "var ", _10, " = ", _11, "[", _10, "_index];"].join("");
            }
        },
        "forelse": {
            delta: 0,
            prefix: "} } if (__LENGTH_STACK__[__LENGTH_STACK__.length - 1] == 0) { if (",
            suffix: ") {",
            paramDefault: "true"
        },
        "/for": {
            delta: -1,
            prefix: "} }; delete __LENGTH_STACK__[__LENGTH_STACK__.length - 1];"
        },
        "var": {
            delta: 0,
            prefix: "var ",
            suffix: ";"
        },
        "macro": {
            delta: 1,
            prefixFunc: function (_12, _13, _14, etc) {
                var _15 = _12[1].split("(")[0];
                return ["var ", _15, " = function", _12.slice(1).join(" ").substring(_15.length), "{ var _OUT_arr = []; var _OUT = { write: function(m) { if (m) _OUT_arr.push(m); } }; "].join("");
            }
        },
        "/macro": {
            delta: -1,
            prefix: " return _OUT_arr.join(''); };"
        }
    };
    TrimPath.parseTemplate_etc.modifierDef = {
        "eat": function (v) {
            return "";
        },
        "escape": function (s) {
            return String(s).replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        },
        "capitalize": function (s) {
            return String(s).toUpperCase();
        },
        "default": function (s, d) {
            return s != null ? s : d;
        }
    };
    TrimPath.parseTemplate_etc.modifierDef.h = TrimPath.parseTemplate_etc.modifierDef.escape;
    TrimPath.parseTemplate_etc.Template = function (_16, _17, _18, _19, etc) {
        this.process = function (_1a, _1b) {
            if (_1a == null) {
                _1a = {};
            }
            if (_1a._MODIFIERS == null) {
                _1a._MODIFIERS = {};
            }
            if (_1a.defined == null) {
                _1a.defined = function (str) {
                    return (_1a[str] != undefined);
                };
            }
            for (var k in etc.modifierDef) {
                if (_1a._MODIFIERS[k] == null) {
                    _1a._MODIFIERS[k] = etc.modifierDef[k];
                }
            }
            if (_1b == null) {
                _1b = {};
            }
            var _1c = [];
            var _1d = {
                write: function (m) {
                    _1c.push(m);
                }
            };
            try {
                _19(_1d, _1a, _1b);
            } catch (e) {
                if (_1b.throwExceptions == true) {
                    throw e;
                }
                var _1e = new String(_1c.join("") + "[ERROR: " + e.toString() + (e.message ? "; " + e.message : "") + "]");
                _1e["exception"] = e;
                return _1e;
            }
            return _1c.join("");
        };
        this.name = _16;
        this.source = _17;
        this.sourceFunc = _18;
        this.toString = function () {
            return "TrimPath.Template [" + _16 + "]";
        };
    };
    TrimPath.parseTemplate_etc.ParseError = function (_1f, _20, _21) {
        this.name = _1f;
        this.line = _20;
        this.message = _21;
    };
    TrimPath.parseTemplate_etc.ParseError.prototype.toString = function () {
        return ("TrimPath template ParseError in " + this.name + ": line " + this.line + ", " + this.message);
    };
    var _7 = function (_22, _23, etc) {
        _22 = _24(_22);
        var _25 = ["var TrimPath_Template_TEMP = function(_OUT, _CONTEXT, _FLAGS) { with (_CONTEXT) {"];
        var _26 = {
            stack: [],
            line: 1
        };
        var _27 = -1;
        while (_27 + 1 < _22.length) {
            var _28 = _27;
            _28 = _22.indexOf("{", _28 + 1);
            while (_28 >= 0) {
                var _29 = _22.indexOf("}", _28 + 1);
                var _2a = _22.substring(_28, _29);
                var _2b = _2a.match(/^\{(cdata|minify|eval)/);
                if (_2b) {
                    var _2c = _2b[1];
                    var _2d = _28 + _2c.length + 1;
                    var _2e = _22.indexOf("}", _2d);
                    if (_2e >= 0) {
                        var _2f;
                        if (_2e - _2d <= 0) {
                            _2f = "{/" + _2c + "}";
                        } else {
                            _2f = _22.substring(_2d + 1, _2e);
                        }
                        var _30 = _22.indexOf(_2f, _2e + 1);
                        if (_30 >= 0) {
                            _31(_22.substring(_27 + 1, _28), _25);
                            var _32 = _22.substring(_2e + 1, _30);
                            if (_2c == "cdata") {
                                _33(_32, _25);
                            } else {
                                if (_2c == "minify") {
                                    _33(_34(_32), _25);
                                } else {
                                    if (_2c == "eval") {
                                        if (_32 != null && _32.length > 0) {
                                            _25.push("_OUT.write( (function() { " + _32 + " })() );");
                                        }
                                    }
                                }
                            }
                            _28 = _27 = _30 + _2f.length - 1;
                        }
                    }
                } else {
                    if (_22.charAt(_28 - 1) != "$" && _22.charAt(_28 - 1) != "\\") {
                        var _35 = (_22.charAt(_28 + 1) == "/" ? 2 : 1);
                        if (_22.substring(_28 + _35, _28 + 10 + _35).search(TrimPath.parseTemplate_etc.statementTag) == 0) {
                            break;
                        }
                    }
                }
                _28 = _22.indexOf("{", _28 + 1);
            }
            if (_28 < 0) {
                break;
            }
            var _29 = _22.indexOf("}", _28 + 1);
            if (_29 < 0) {
                break;
            }
            _31(_22.substring(_27 + 1, _28), _25);
            _36(_22.substring(_28, _29 + 1), _26, _25, _23, etc);
            _27 = _29;
        }
        _31(_22.substring(_27 + 1), _25);
        if (_26.stack.length != 0) {
            throw new etc.ParseError(_23, _26.line, "unclosed, unmatched statement(s): " + _26.stack.join(","));
        }
        _25.push("}}; TrimPath_Template_TEMP");
        return _25.join("");
    };
    var _36 = function (_37, _38, _39, _3a, etc) {
        var _3b = _37.slice(1, -1).split(" ");
        var _3c = etc.statementDef[_3b[0]];
        if (_3c == null) {
            _31(_37, _39);
            return;
        }
        if (_3c.delta < 0) {
            if (_38.stack.length <= 0) {
                throw new etc.ParseError(_3a, _38.line, "close tag does not match any previous statement: " + _37);
            }
            _38.stack.pop();
        }
        if (_3c.delta > 0) {
            _38.stack.push(_37);
        }
        if (_3c.paramMin != null && _3c.paramMin >= _3b.length) {
            throw new etc.ParseError(_3a, _38.line, "statement needs more parameters: " + _37);
        }
        if (_3c.prefixFunc != null) {
            _39.push(_3c.prefixFunc(_3b, _38, _3a, etc));
        } else {
            _39.push(_3c.prefix);
        }
        if (_3c.suffix != null) {
            if (_3b.length <= 1) {
                if (_3c.paramDefault != null) {
                    _39.push(_3c.paramDefault);
                }
            } else {
                for (var i = 1; i < _3b.length; i++) {
                    if (i > 1) {
                        _39.push(" ");
                    }
                    _39.push(_3b[i]);
                }
            }
            _39.push(_3c.suffix);
        }
    };
    var _31 = function (_3d, _3e) {
        if (_3d.length <= 0) {
            return;
        }
        var _3f = 0;
        var _40 = _3d.length - 1;
        while (_3f < _3d.length && (_3d.charAt(_3f) == "\n")) {
            _3f++;
        }
        while (_40 >= 0 && (_3d.charAt(_40) == " " || _3d.charAt(_40) == "\t")) {
            _40--;
        }
        if (_40 < _3f) {
            _40 = _3f;
        }
        if (_3f > 0) {
            _3e.push("if (_FLAGS.keepWhitespace == true) _OUT.write(\"");
            var s = _3d.substring(0, _3f).replace("\n", "\\n");
            if (s.charAt(s.length - 1) == "\n") {
                s = s.substring(0, s.length - 1);
            }
            _3e.push(s);
            _3e.push("\");");
        }
        var _41 = _3d.substring(_3f, _40 + 1).split("\n");
        for (var i = 0; i < _41.length; i++) {
            _42(_41[i], _3e);
            if (i < _41.length - 1) {
                _3e.push("_OUT.write(\"\\n\");\n");
            }
        }
        if (_40 + 1 < _3d.length) {
            _3e.push("if (_FLAGS.keepWhitespace == true) _OUT.write(\"");
            var s = _3d.substring(_40 + 1).replace("\n", "\\n");
            if (s.charAt(s.length - 1) == "\n") {
                s = s.substring(0, s.length - 1);
            }
            _3e.push(s);
            _3e.push("\");");
        }
    };
    var _42 = function (_43, _44) {
        var _45 = "}";
        var _46 = -1;
        while (_46 + _45.length < _43.length) {
            var _47 = "${",
                    _48 = "}";
            var _49 = _43.indexOf(_47, _46 + _45.length);
            if (_49 < 0) {
                break;
            }
            if (_43.charAt(_49 + 2) == "%") {
                _47 = "${%";
                _48 = "%}";
            }
            var _4a = _43.indexOf(_48, _49 + _47.length);
            if (_4a < 0) {
                break;
            }
            _33(_43.substring(_46 + _45.length, _49), _44);
            var _4b = _43.substring(_49 + _47.length, _4a).replace(/\|\|/g, "#@@#").split("|");
            for (var k in _4b) {
                if (_4b[k].replace) {
                    _4b[k] = _4b[k].replace(/#@@#/g, "||");
                }
            }
            _44.push("_OUT.write(");
            _4c(_4b, _4b.length - 1, _44);
            _44.push(");");
            _46 = _4a;
            _45 = _48;
        }
        _33(_43.substring(_46 + _45.length), _44);
    };
    var _33 = function (_4d, _4e) {
        if (_4d == null || _4d.length <= 0) {
            return;
        }
        _4d = _4d.replace(/\\/g, "\\\\");
        _4d = _4d.replace(/\n/g, "\\n");
        _4d = _4d.replace(/"/g, "\\\"");
        _4e.push("_OUT.write(\"");
        _4e.push(_4d);
        _4e.push("\");");
    };
    var _4c = function (_4f, _50, _51) {
        var _52 = _4f[_50];
        if (_50 <= 0) {
            _51.push(_52);
            return;
        }
        var _53 = _52.split(":");
        _51.push("_MODIFIERS[\"");
        _51.push(_53[0]);
        _51.push("\"](");
        _4c(_4f, _50 - 1, _51);
        if (_53.length > 1) {
            _51.push(",");
            _51.push(_53[1]);
        }
        _51.push(")");
    };
    var _24 = function (_54) {
        _54 = _54.replace(/\t/g, "    ");
        _54 = _54.replace(/\r\n/g, "\n");
        _54 = _54.replace(/\r/g, "\n");
        _54 = _54.replace(/^(\s*\S*(\s+\S+)*)\s*$/, "$1");
        return _54;
    };
    var _34 = function (_55) {
        _55 = _55.replace(/^\s+/g, "");
        _55 = _55.replace(/\s+$/g, "");
        _55 = _55.replace(/\s+/g, " ");
        _55 = _55.replace(/^(\s*\S*(\s+\S+)*)\s*$/, "$1");
        return _55;
    };
    TrimPath.parseDOMTemplate = function (_56, _57, _58) {
        if (_57 == null) {
            _57 = document;
        }
        var _59 = _57.getElementById(_56);
        var _5a = _59.value;
        if (_5a == null) {
            _5a = _59.innerHTML;
        }
        _5a = _5a.replace(/&lt;/g, "<").replace(/&gt;/g, ">");
        return TrimPath.parseTemplate(_5a, _56, _58);
    };
    TrimPath.processDOMTemplate = function (_5b, _5c, _5d, _5e, _5f) {
        return TrimPath.parseDOMTemplate(_5b, _5e, _5f).process(_5c, _5d);
    };
})(); /*  |xGv00|c4edea987b3093ece795ccf6fe9614a7 */