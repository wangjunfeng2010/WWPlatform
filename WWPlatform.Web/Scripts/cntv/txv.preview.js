Live.txv.w = function () {
    this.config =
    {
        conid: "ul_mod_list",
        comPager: "pager",
        simPager: "pager2"
    };
    this.isBusy = false;
    this.option = {
        mi_pagesize: 6,
        mi_pagenum: 0
    };
    this.pager = {};
    this.pager2 = {};
    this.clearPageItem = false;
};

Live.txv.w.prototype = {
    init: function (opts) {
        for (var _6f in opts) {
            this.option[_6f] = opts[_6f];
        }
        this.pager = this.createPager();
        this.pager2 = this.createPager();
        this.setComplexPageTemplate(this.pager);
        this.setSimplePageTemplate(this.pager2);
        this.initParam();
    },
    initParam: function () {
        this.go(null, null);
    },

    //覆盖txv.base.js的template
    setSimplePageTemplate: function (_74) {
        _74.Templet.CONTENT = "<p class=\"mod_pagenav_main\"><span class=\"mod_pagenav_count2\">{summary}</span>{previous}{next}</p>";
        _74.Templet.SUMMARY = "<span class=\"current\">{pagenum}</span>/{totpage}";
        _74.Templet.PREVIOUS_DISABLE = "<span class=\"prev_disable prev\"><span>上一页</span></span>";
        _74.Templet.PREVIOUS_ENABLE = "<a href=\"javascript:;\" id=\"pager_previous_{id}\" class=\"prev\" title=\"上一页\"><span>上一页</span></a>\n";
        _74.Templet.NEXT_DISABLE = "<span class=\"next_disable next\"><span>下一页</span></span>\n";
        _74.Templet.NEXT_ENABLE = "<a href=\"javascript:;\" title=\"下一页\" id=\"pager_next_{id}\" class=\"next\"><span>下一页</span></a>\n";
        _74.elmId = this.config.simPager;
    },
    setComplexPageTemplate: function (_75) {
        _75.Templet.CONTENT = '<p class="mod_pagenav_main">{previous}<span class="mod_pagenav_count">{first}{prevdot}{pager}{nextdot}{last}</span>{pagecount}{summary}{next}</p><p class="mod_pagenav_option"><span class="mod_pagenav_turn">{goinput}{gobutton}</span></p>',
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
    update: function (_7d, _7e) {
        this.option[_7d] = _7e;
        this.go(_7d, _7e);
    },
    go: function (_7f, _80) {
        var _81 = this;
        var _items = $("#" + this.config.conid + " li.bbor");
        _81.renderContent(_items, _7f, _80);
    },
    renderContent: function (_82, _83, _84) {
        if (_82) {
            //_82.op = this.option;
            //txv.template.fillHtml(_82, this.config.tmpId, this.config.conid);
            var _85 = 0;
            if (_84 != null) {
                _85 = _84;
            }

            var _8a = this;
            $.each(_82, function (_86, _87) {
                if (_86 >= _85 * _8a.option.mi_pagesize && _86 < (_85 + 1) * _8a.option.mi_pagesize) {
                    $(_87).show();
                }
                else {
                    $(_87).hide();
                }
            });
            this.renderPager(this.pager, _82.length);
            this.renderPager(this.pager2, _82.length);
        }
        else {
            this.renderPager(this.pager, 0);
            this.renderPager(this.pager2, 0);
        }

        this.isBusy = false;
    },
    createPager: function () {
        var _8d = this;
        var _8e = new Live.Pager();
        _8e.offsetPage = 1;
        _8e.currentPage = 1;
        _8e.showError = function (msg) { };
        _8e.onTurnPage = function (_8f) {
            _8d.update("mi_pagenum", _8f - 1);
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

Live.txv.w_list = {
    init: function (_95) {
        //txv.common.initPage();
        var w = new txv.w();
        w.init(_95);
        var _9c = $("#" + w.config.conid + " li.bbor").length;
        if (!w.clearPageItem && _9c > w.option.mi_pagesize) {
            w.renderPager(w.pager, _9c);
            w.renderPager(w.pager2, _9c);
        }
    }
};