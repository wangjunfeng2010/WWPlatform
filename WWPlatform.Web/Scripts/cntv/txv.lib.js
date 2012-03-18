var TrimPath;
(function () {
    if (TrimPath == null) {
        TrimPath = new Object();
    }
    if (TrimPath.evalEx == null) {
        TrimPath.evalEx = function (_d) {
            return eval(_d);
        };
    }
    var _e;
    if (Array.prototype.pop == null) {
        Array.prototype.pop = function () {
            if (this.length === 0) {
                return _e;
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
    TrimPath.parseTemplate = function (_f, _10, _11) {
        if (_11 == null) {
            _11 = TrimPath.parseTemplate_etc;
        }
        var _12 = _13(_f, _10, _11);
        var _14 = TrimPath.evalEx(_12, _10, 1);
        if (_14 != null) {
            return new _11.Template(_10, _f, _12, _14, _11);
        }
        return null;
    };
    try {
        String.prototype.process = function (_15, _16) {
            var _17 = TrimPath.parseTemplate(this, null);
            if (_17 != null) {
                return _17.process(_15, _16);
            }
            return this;
        };
    }
    catch (e) { }
    TrimPath.parseTemplate_etc = {};
    TrimPath.parseTemplate_etc.statementTag = "forelse|for|if|elseif|else|var|macro";
    TrimPath.parseTemplate_etc.statementDef =
    {
        "if":
        {
            delta: 1, prefix: "if (", suffix: ") {", paramMin: 1
        },
        "else": {
            delta: 0, prefix: "} else {"
        },
        "elseif": {
            delta: 0, prefix: "} else if (", suffix: ") {", paramDefault: "true"
        },
        "/if": {
            delta: -1, prefix: "}"
        },
        "for":
        {
            delta: 1, paramMin: 3,
            prefixFunc: function (_18, _19, _1a, etc) {
                if (_18[2] != "in") {
                    throw new etc.ParseError(_1a, _19.line, "bad for loop statement: " + _18.join(" "));
                }
                var _1b = _18[1];
                var _1c = "__LIST__" + _1b;
                return ["var ", _1c, " = ", _18[3], ";", "var __LENGTH_STACK__;", "if (typeof(__LENGTH_STACK__) == 'undefined' || !__LENGTH_STACK__.length) __LENGTH_STACK__ = new Array();",
                "__LENGTH_STACK__[__LENGTH_STACK__.length] = 0;", "if ((", _1c, ") != null) { ", "var ",
                _1b, "_ct = 0;", "for (var ", _1b, "_index in ", _1c, ") { ", _1b, "_ct++;", "if (typeof(",
                _1c, "[", _1b, "_index]) == 'function') {continue;}", "__LENGTH_STACK__[__LENGTH_STACK__.length - 1]++;",
                "var ", _1b, " = ", _1c, "[", _1b, "_index];"].join("");
            }
        },
        "forelse": {
            delta: 0, prefix: "} } if (__LENGTH_STACK__[__LENGTH_STACK__.length - 1] == 0) { if (",
            suffix: ") {", paramDefault: "true"
        },
        "/for": {
            delta: -1, prefix: "} }; delete __LENGTH_STACK__[__LENGTH_STACK__.length - 1];"
        },
        "var": {
            delta: 0, prefix: "var ", suffix: ";"
        },
        "macro":
        {
            delta: 1,
            prefixFunc: function (_1d, _1e, _1f, etc) {
                var _20 = _1d[1].split("(")[0];
                return ["var ", _20, " = function", _1d.slice(1).join(" ").substring(_20.length), "{ var _OUT_arr = []; var _OUT = { write: function(m) { if (m) _OUT_arr.push(m); } }; "].join("");
            }
        },
        "/macro": {
            delta: -1, prefix: " return _OUT_arr.join(''); };"
        }
    };
    TrimPath.parseTemplate_etc.modifierDef =
    {
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
    TrimPath.parseTemplate_etc.Template = function (_21, _22, _23, _24, etc) {
        this.process = function (_25, _26) {
            if (_25 == null) {
                _25 = {};
            }
            if (_25._MODIFIERS == null) {
                _25._MODIFIERS = {};
            }
            if (_25.defined == null) {
                _25.defined = function (str) {
                    return (_25[str] != undefined);
                };
            }
            for (var k in etc.modifierDef) {
                if (_25._MODIFIERS[k] == null) {
                    _25._MODIFIERS[k] = etc.modifierDef[k];
                }
            }
            if (_26 == null) {
                _26 = {};
            }
            var _27 = [];
            var _28 = {
                write: function (m) {
                    _27.push(m);
                }
            };
            try {
                _24(_28, _25, _26);
            }
            catch (e) {
                if (_26.throwExceptions == true) {
                    throw e;
                }
                var _29 = new String(_27.join("") + "[ERROR: " + e.toString() + (e.message ? "; " + e.message : "") + "]");
                _29["exception"] = e;
                return _29;
            }
            return _27.join("");
        };
        this.name = _21;
        this.source = _22;
        this.sourceFunc = _23;
        this.toString = function () {
            return "TrimPath.Template [" + _21 + "]";
        };
    };
    TrimPath.parseTemplate_etc.ParseError = function (_2a, _2b, _2c) {
        this.name = _2a;
        this.line = _2b;
        this.message = _2c;
    };
    TrimPath.parseTemplate_etc.ParseError.prototype.toString = function () {
        return ("TrimPath template ParseError in " + this.name + ": line " + this.line + ", " + this.message);
    };
    var _13 = function (_2d, _2e, etc) {
        _2d = _2f(_2d);
        var _30 = ["var TrimPath_Template_TEMP = function(_OUT, _CONTEXT, _FLAGS) { with (_CONTEXT) {"];
        var _31 = {
            stack: [], line: 1
        };
        var _32 = -1;
        while (_32 + 1 < _2d.length) {
            var _33 = _32;
            _33 = _2d.indexOf("{", _33 + 1);
            while (_33 >= 0) {
                var _34 = _2d.indexOf("}", _33 + 1);
                var _35 = _2d.substring(_33, _34);
                var _36 = _35.match(/^\{(cdata|minify|eval)/);
                if (_36) {
                    var _37 = _36[1];
                    var _38 = _33 + _37.length + 1;
                    var _39 = _2d.indexOf("}", _38);
                    if (_39 >= 0) {
                        var _3a;
                        if (_39 - _38 <= 0) {
                            _3a = "{/" + _37 + "}";
                        }
                        else {
                            _3a = _2d.substring(_38 + 1, _39);
                        }
                        var _3b = _2d.indexOf(_3a, _39 + 1);
                        if (_3b >= 0) {
                            _3c(_2d.substring(_32 + 1, _33), _30);
                            var _3d = _2d.substring(_39 + 1, _3b);
                            if (_37 == "cdata") {
                                _3e(_3d, _30);
                            }
                            else {
                                if (_37 == "minify") {
                                    _3e(_3f(_3d), _30);
                                }
                                else {
                                    if (_37 == "eval") {
                                        if (_3d != null && _3d.length > 0) {
                                            _30.push("_OUT.write( (function() { " + _3d + " })() );");
                                        }
                                    }
                                }
                            }
                            _33 = _32 = _3b + _3a.length - 1;
                        }
                    }
                }
                else {
                    if (_2d.charAt(_33 - 1) != "$" && _2d.charAt(_33 - 1) != "\\") {
                        var _40 = (_2d.charAt(_33 + 1) == "/" ? 2 : 1);
                        if (_2d.substring(_33 + _40, _33 + 10 + _40).search(TrimPath.parseTemplate_etc.statementTag) == 0) {
                            break;
                        }
                    }
                }
                _33 = _2d.indexOf("{", _33 + 1);
            }
            if (_33 < 0) {
                break;
            }
            var _34 = _2d.indexOf("}", _33 + 1);
            if (_34 < 0) {
                break;
            }
            _3c(_2d.substring(_32 + 1, _33), _30);
            _41(_2d.substring(_33, _34 + 1), _31, _30, _2e, etc);
            _32 = _34;
        }
        _3c(_2d.substring(_32 + 1), _30);
        if (_31.stack.length != 0) {
            throw new etc.ParseError(_2e, _31.line, "unclosed, unmatched statement(s): " + _31.stack.join(","));
        }
        _30.push("}}; TrimPath_Template_TEMP");
        return _30.join("");
    };
    var _41 = function (_42, _43, _44, _45, etc) {
        var _46 = _42.slice(1, -1).split(" ");
        var _47 = etc.statementDef[_46[0]];
        if (_47 == null) {
            _3c(_42, _44);
            return;
        }
        if (_47.delta < 0) {
            if (_43.stack.length <= 0) {
                throw new etc.ParseError(_45, _43.line, "close tag does not match any previous statement: " + _42);
            }
            _43.stack.pop();
        }
        if (_47.delta > 0) {
            _43.stack.push(_42);
        }
        if (_47.paramMin != null && _47.paramMin >= _46.length) {
            throw new etc.ParseError(_45, _43.line, "statement needs more parameters: " + _42);
        }
        if (_47.prefixFunc != null) {
            _44.push(_47.prefixFunc(_46, _43, _45, etc));
        }
        else {
            _44.push(_47.prefix);
        }
        if (_47.suffix != null) {
            if (_46.length <= 1) {
                if (_47.paramDefault != null) {
                    _44.push(_47.paramDefault);
                }
            }
            else {
                for (var i = 1; i < _46.length; i++) {
                    if (i > 1) {
                        _44.push(" ");
                    }
                    _44.push(_46[i]);
                }
            }
            _44.push(_47.suffix);
        }
    };
    var _3c = function (_48, _49) {
        if (_48.length <= 0) {
            return;
        }
        var _4a = 0;
        var _4b = _48.length - 1;
        while (_4a < _48.length && (_48.charAt(_4a) == "\n")) {
            _4a++;
        }
        while (_4b >= 0 && (_48.charAt(_4b) == " " || _48.charAt(_4b) == "\t")) {
            _4b--;
        }
        if (_4b < _4a) {
            _4b = _4a;
        }
        if (_4a > 0) {
            _49.push("if (_FLAGS.keepWhitespace == true) _OUT.write(\"");
            var s = _48.substring(0, _4a).replace("\n", "\\n");
            if (s.charAt(s.length - 1) == "\n") {
                s = s.substring(0, s.length - 1);
            }
            _49.push(s);
            _49.push("\");");
        }
        var _4c = _48.substring(_4a, _4b + 1).split("\n");
        for (var i = 0; i < _4c.length; i++) {
            _4d(_4c[i], _49);
            if (i < _4c.length - 1) {
                _49.push("_OUT.write(\"\\n\");\n");
            }
        }
        if (_4b + 1 < _48.length) {
            _49.push("if (_FLAGS.keepWhitespace == true) _OUT.write(\"");
            var s = _48.substring(_4b + 1).replace("\n", "\\n");
            if (s.charAt(s.length - 1) == "\n") {
                s = s.substring(0, s.length - 1);
            }
            _49.push(s);
            _49.push("\");");
        }
    };
    var _4d = function (_4e, _4f) {
        var _50 = "}";
        var _51 = -1;
        while (_51 + _50.length < _4e.length) {
            var _52 = "${", _53 = "}";
            var _54 = _4e.indexOf(_52, _51 + _50.length);
            if (_54 < 0) {
                break;
            }
            if (_4e.charAt(_54 + 2) == "%") {
                _52 = "${%";
                _53 = "%}";
            }
            var _55 = _4e.indexOf(_53, _54 + _52.length);
            if (_55 < 0) {
                break;
            }
            _3e(_4e.substring(_51 + _50.length, _54), _4f);
            var _56 = _4e.substring(_54 + _52.length, _55).replace(/\|\|/g, "#@@#").split("|");
            for (var k in _56) {
                if (_56[k].replace) {
                    _56[k] = _56[k].replace(/#@@#/g, "||");
                }
            }
            _4f.push("_OUT.write(");
            _57(_56, _56.length - 1, _4f);
            _4f.push(");");
            _51 = _55;
            _50 = _53;
        }
        _3e(_4e.substring(_51 + _50.length), _4f);
    };
    var _3e = function (_58, _59) {
        if (_58 == null || _58.length <= 0) {
            return;
        }
        _58 = _58.replace(/\\/g, "\\\\");
        _58 = _58.replace(/\n/g, "\\n");
        _58 = _58.replace(/"/g, "\\\"");
        _59.push("_OUT.write(\"");
        _59.push(_58);
        _59.push("\");");
    };
    var _57 = function (_5a, _5b, _5c) {
        var _5d = _5a[_5b];
        if (_5b <= 0) {
            _5c.push(_5d);
            return;
        }
        var _5e = _5d.split(":");
        _5c.push("_MODIFIERS[\"");
        _5c.push(_5e[0]);
        _5c.push("\"](");
        _57(_5a, _5b - 1, _5c);
        if (_5e.length > 1) {
            _5c.push(",");
            _5c.push(_5e[1]);
        }
        _5c.push(")");
    };
    var _2f = function (_5f) {
        _5f = _5f.replace(/\t/g, "    ");
        _5f = _5f.replace(/\r\n/g, "\n");
        _5f = _5f.replace(/\r/g, "\n");
        _5f = _5f.replace(/^(\s*\S*(\s+\S+)*)\s*$/, "$1");
        return _5f;
    };
    var _3f = function (_60) {
        _60 = _60.replace(/^\s+/g, "");
        _60 = _60.replace(/\s+$/g, "");
        _60 = _60.replace(/\s+/g, " ");
        _60 = _60.replace(/^(\s*\S*(\s+\S+)*)\s*$/, "$1");
        return _60;
    };
    TrimPath.parseDOMTemplate = function (_61, _62, _63) {
        if (_62 == null) {
            _62 = document;
        }
        var _64 = _62.getElementById(_61);
        var _65 = _64.value;
        if (_65 == null) {
            _65 = _64.innerHTML;
        }
        _65 = _65.replace(/&lt;/g, "<").replace(/&gt;/g, ">");
        return TrimPath.parseTemplate(_65, _61, _63);
    };
    TrimPath.processDOMTemplate = function (_66, _67, _68, _69, _6a) {
        return TrimPath.parseDOMTemplate(_66, _69, _6a).process(_67, _68);
    };
})();

Live.txv.lib = function () {
    this.config =
    {
        curCls: "current",
        gpCls: "_group",
        tagCls: "_gtag",
        dftCls: "_gdefault",
        gname: "gname",
        gtype: "gtype",
        tvalue: "tvalue",
        grmCls: "_grm",
        cgi: "/fcgi-bin/lib?",
        navid: "nav_option",
        conid: "content",
        detailCls: "details",
        comPager: "pager",
        simPager: "pager2",
        tmpId: "tb_tmp"
    };
    this.isBusy = false;
    this.elmGroup = {};
    this.option =
    {
        mi_cat: -1,
        mi_sub: -1,
        mi_dyn: -1,
        mi_lec: -1,
        mi_show_type: 0,
        mi_pagesize: 20,
        mi_pagenum: 0
    };
    this.pager = {};
    this.pager2 = {};
    this.beforeChange = null;
    this.afterChange = null;
    this.clearPageItem = false;
};
Live.txv.lib.prototype =
{
    init: function (op) {
        for (var _6f in op) {
            this.option[_6f] = op[_6f];
        }
        this.option.mi_pagesize = this.option.mi_show_type == 0 ? 20 : 8;
        var _70 = $("." + this.config.gpCls);

        for (var i = 0, len = _70.length; i < len; i++) {
            var obj = this.getOneGroup(_70[i]);
            this.elmGroup[obj.name] = obj;
        }
        this.pager = this.createPager();
        this.pager2 = this.createPager();
        this.setComplexPageTemplate(this.pager);
        this.setSimplatePageTemplate(this.pager2);
        this.initParam();
        this.renderNavigator();
    },
    initParam: function () {
        this.clearPageItem = true;
        this.updateTag();
        this.go(null, null);
    },
    updateTag: function () {
        for (var _73 in this.elmGroup) {
            var op = this.option[_73];
            if (op != null) {
                var obj = this.elmGroup[_73];
                var g = $(obj.group);
                g.find("." + this.config.curCls).removeClass(this.config.curCls);
                var cur = g.find("[" + this.config.tvalue + "=" + op + "]");
                if (cur.children.length) {
                    cur.addClass(this.config.curCls);
                    obj.current = cur.get(0);
                }
            }
        }
    },
    //覆盖txv.base.js中Templet
    setSimplatePageTemplate: function (_74) {
        _74.Templet.CONTENT = "<form method=\"get\" action=\"##\" id=\"pager_form_{id}\"><p class=\"mod_pagenav_main\"><span class=\"mod_pagenav_count2\">{summary}</span>{previous}{next}</p></form>";
        _74.Templet.SUMMARY = "<span class=\"current\">{pagenum}</span>/{totpage}";
        _74.Templet.PREVIOUS_DISABLE = "<span class=\"prev_disable prev\"><span>上一页</span></span>";
        _74.Templet.PREVIOUS_ENABLE = "<a href=\"javascript:;\" id=\"pager_previous_{id}\" class=\"prev\" title=\"上一页\"><span>上一页</span></a>\n";
        _74.Templet.NEXT_DISABLE = "<span class=\"next_disable next\"><span>下一页</span></span>\n";
        _74.Templet.NEXT_ENABLE = "<a href=\"javascript:;\" title=\"下一页\" id=\"pager_next_{id}\" class=\"next\"><span>下一页</span></a>\n";
        _74.elmId = this.config.simPager;
    },
    setComplexPageTemplate: function (_75) {
        _75.Templet.PREVIOUS_DISABLE = "<span class=\"prev_disable prev\"><span>上一页</span></span>";
        _75.Templet.PREVIOUS_ENABLE = "<a href=\"javascript:;\" id=\"pager_previous_{id}\" class=\"prev\" title=\"上一页\"><span>上一页</span></a>\n";
        _75.Templet.PAGE_NORMAL = "<a href=\"javascript:;\" id=\"pager_num_{id}_{pagenum}\" class=\"c_txt6\" title=\"{pagenum}\"><span>{pagenum}</span></a>\n";
        _75.Templet.LAST_ENABLE = "<a href=\"javascript:;\" title=\"{totpage}\" class=\"c_txt6\" id=\"pager_last_{id}\"><span>{totpage}</span></a>\n";
        _75.Templet.NEXT_DISABLE = "<span class=\"next_disable next\"><span>下一页</span></span>\n";
        _75.Templet.NEXT_ENABLE = "<a href=\"javascript:;\" title=\"下一页\" id=\"pager_next_{id}\" class=\"next\"><span>下一页</span></a>\n";
        _75.Templet.GO_PAGE_INPUT = "";
        _75.Templet.GO_PAGE_BUTTON = "";
        _75.Templet.PAGE_COUNT = "";
        _75.elmId = this.config.comPager;
    },
    getOneGroup: function (_76) {
        var _77 = this;
        var _78 = $("." + this.config.dftCls, _76); //.elements;
        if (_78[0]) {
            _78 = _78[0];
        }
        else {
            _78 = null;
        }
        var _79 = $("." + this.config.tagCls, _76);
        var _7a = _79.elements;
        var _7b = _76.getAttribute(this.config.gname);
        var _7c = _76.getAttribute(this.config.gtype);
        var cur = $("." + this.config.curCls, _76).get(0);
        var obj = {
            name: _7b, type: _7c, group: _76, defaultTag: _78, allTag: _7a, current: cur
        };
        obj.onChange = function (elm, lib) {
            if (lib.isBusy) {
                return;
            }
            if (elm.className.indexOf(lib.config.curCls) >= 0) {
                return;
            }
            lib.isBusy = true;
            _77.option.mi_pagenum = 0;
            this.current = elm;
            lib.update(this.name, elm.getAttribute(lib.config.tvalue));
            if (this.type == 1) {
                lib.renderNavigator();
            }
            $(this.group).find("." + lib.config.curCls).removeClass(lib.config.curCls);
            elm.className = elm.className + " " + lib.config.curCls;
        };
        obj.onRemoveTag = function () {
            obj.onChange(obj.defaultTag, _77);
        };
        _79.attr("href", "javascript:;");
        _79.bind("click", function () {
            obj.onChange(this, _77);
            return false;
        });
        return obj;
    },
    update: function (_7d, _7e) {
        this.option[_7d] = _7e;
        if (this.beforeChange) {
            this.beforeChange(_7d, _7e);
        }
        this.go(_7d, _7e);
        //this.setHash();
    },
    go: function (_7f, _80) {
        var _81 = this;
        $("#" + this.config.conid).html("正在努力获取数据..");
        var url = this.buildUrl();
        $.ajax(
        {
            url: url,
            dataType: "json",
            async: true,
            cache:false,
            beforeSend: function () { },
            success: function (data) {
                _81.renderContent(data, _7f, _80);
            },
            error: function () {
                _81.isBusy = false;
            }
        });
    },
    renderContent: function (_82, _83, _84) {
        if (_82) {
            if (!_82.features || _82.features.length == 0) {
                $("#" + this.config.conid).html("很抱歉，没有找到任何视频内容。建议您，减少一些筛选条件试试。");
                this.renderPager(this.pager, 0);
                this.renderPager(this.pager2, 0);
            }
            else {
                _82.op = this.option;
                txv.template.fillHtml(_82, this.config.tmpId, this.config.conid);
                this.renderPager(this.pager, _82.total);
                this.renderPager(this.pager2, _82.total);
                if (this.afterChange) {
                    this.afterChange(_83, _84);
                }
            }
            window.scroll(0, 0);
        }
        else {
            txv.log("Get data error:json is null!");
        }
        this.isBusy = false;
    },

    //构建url
    buildUrl: function () {
        var sb = [];
        sb.push(this.config.cgi);
        for (var _85 in this.option) {
            sb.push(_85, "=", this.option[_85], "&");
        }
        sb.pop(); //去掉最后一个&
        return sb.join("");
    },

    renderNavigator: function () {
        var nav = [];
        for (var k in this.elmGroup) {
            var elm = this.elmGroup[k].current;
            if (this.elmGroup[k].type == 1) {
                if (!elm) {
                    continue;
                }
                var _86 = elm.innerHTML;
                if (_86 != "全部") {
                    nav.push({
                        name: _86, group: this.elmGroup[k].name
                    });
                }
            }
        }
        this.setNav(nav);
    },
    setNav: function (ops) {
        var nav = $("#" + this.config.navid);
        var _89 = this;
        if (!ops || ops.length == 0) {
            nav.html("<span>-</span> <span>全部</span>");
            return;
        }

        var tpl = "{split}<span>{name}</span><a href='javascript:;' class='_grm' _group={group}>删除</a>";
        var _8a = "<em>+</em>";
        var _8b = ["<span>-</span> "];
        for (var i = 0, len = ops.length; i < len; i++) {
            _8b.push(tpl.replace(/\{name\}/g, ops[i].name).replace(/\{group\}/g, ops[i].group).replace(/\{split\}/g,
            (i ? _8a : "")));
        }
        nav.html(_8b.join(""));
        var _8c = nav.find("." + this.config.grmCls);
        _8c.each(function () {
            $(this).bind("click", function () {
                _89.elmGroup[this.getAttribute("_group")].onRemoveTag();
            });
        });
    },
    createPager: function () {
        var _8d = this;
        var _8e = new Live.Pager();
        _8e.offsetPage = 1;
        _8e.currentPage = 1;
        _8e.showError = function (msg) { };
        _8e.onTurnPage = function (_8f) {
            _8d.update("mi_pagenum", _8f - 1);
            return false;
        };
        return _8e;
    },
    renderPager: function (_90, _91) {
        if (_91 == 0) {
            _90.init($("#" + _90.elmId), 1);
        }
        else {
            _90.currentPage = this.option.mi_pagenum * 1 + 1;
            var _92 = Math.floor((_91 - 1) / this.option.mi_pagesize) + 1;
            _90.init($("#" + _90.elmId), _92);
        }
        _90.fillPager();
    }
};
Live.txv.f_list =
{
    init: function (_95, _96, _97) {
        //txv.common.initPage();
        if (_96) {
            var lib = new txv.lib();
            lib.beforeChange = function (_98, _99) {
                if (_98 == "mi_show_type") {
                    this.option.mi_pagesize = (_99 == 0 ? 20 : 8);
                }
            };
            lib.afterChange = function (_9a, _9b) {
                if (this.option.mi_show_type == 0) {
                    $("#" + this.config.conid).parent().removeClass(this.config.detailCls);
                }
                else {
                    $("#" + this.config.conid).parent().addClass(this.config.detailCls);
                }
            };
            lib.init(_95);
            //            lib.option.mi_pagesize = (lib.option.mi_show_type == 0 ? 20 : 8);
            //            var _9c = _97 * lib.option.mi_pagesize;
            //            if (!lib.clearPageItem && _9c > lib.option.mi_pagesize) {
            //                lib.renderPager(lib.pager, _9c);
            //                lib.renderPager(lib.pager2, _9c);
            //            }
        }
    }
};
