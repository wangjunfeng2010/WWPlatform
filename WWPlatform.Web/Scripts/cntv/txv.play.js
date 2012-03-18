Live.DotShow = function () {
    this.config = {
        showCnt: 6,
        items: null,
        playRight: null,
        playLeft: null,
        pagerItems: null,
        curPage: -1,
        disCss: "over",
        pagerCss: "current",
        isLazy: true,
        getItems: null,
        pagingCallback: null
    };
};
Live.DotShow.prototype = {
    init: function (_17) {
        for (var _18 in _17) {
            this.config[_18] = _17[_18];
        }
        var _19 = this;
        this.config.playRight.bind("mouseup", function () {
            _19.moveR();
        });
        this.config.playLeft.bind("mouseup", function () {
            _19.moveL();
        });
        if (this.config.pagerItems) {
            var len = _19.config.pagerItems.length;
            for (var i = 0; i < len; i++) {
                (function (_1a) {
                    $(_19.config.pagerItems.get(_1a)).bind("mouseover", function () {
                        _19.goPage(_1a);
                    });
                })(i);
            }
        }
        if (this.config.isLazy) {
            Live.lazyLoad.lazyFunc(this.config.playLeft, function () {
                _19.showItem();
            }, -30);
        } else {
            _19.showItem();
        }
    },
    showItem: function () {
        if (this.config.getItems) {
            var _1b = this;
            this.config.getItems(function (_1c) {
                _1b.config.items = _1c;
                _1b.goPage(0);
            });
        } else {
            this.goPage(0);
        }
    },
    goPage: function (_1d) {
        if (_1d < 0 || _1d == this.config.curPage || _1d > Math.ceil(this.config.items.size() / this.config.showCnt) - 1) {
            return;
        }
        var _1e = 0,
            _1f = 0;
        _1e = _1d * this.config.showCnt;
        _1f = (_1d + 1) * this.config.showCnt - 1;
        _1e = _1e < 0 ? 0 : _1e;
        _1f = _1f > this.config.items.size() - 1 ? this.config.items.size() - 1 : _1f;
        if (_1f - _1e + 1 < this.config.showCnt) {
            _1e = _1e - (this.config.showCnt - (_1f - _1e)) + 1;
        }
        this.config.items.hide();
        for (var i = _1e; i <= _1f; i++) {
            var _20 = this.config.items.get(i);
            if (!_20) {
                return;
            }
            var _21 = $(_20).find("img");
            _21.each(function (elm) {
                var src = this.getAttribute("_src");
                if (!this.getAttribute("src") && src) {
                    this.style.visibility = "";
                    this.setAttribute("src", src);
                }
            });
            $(_20).show();
        }
        this.config.curPage = _1d;
        this.setBtn();
        if (typeof (this.config.pagingCallback) == "function") {
            this.config.pagingCallback(_1d);
        }
    },
    moveR: function () {
        this.goPage(this.config.curPage + 1);
    },
    moveL: function () {
        this.goPage(this.config.curPage - 1);
    },
    setBtn: function () {
        if (this.config.curPage <= 0) {
            this.config.playLeft.addClass(this.config.disCss);
        } else {
            this.config.playLeft.removeClass(this.config.disCss);
        }
        if (this.config.curPage == Math.ceil(this.config.items.size() / this.config.showCnt) - 1) {
            this.config.playRight.addClass(this.config.disCss);
        } else {
            this.config.playRight.removeClass(this.config.disCss);
        }
        if (this.config.pagerItems) {
            this.config.pagerItems.removeClass(this.config.pagerCss);
            $(this.config.pagerItems.get(this.config.curPage)).addClass(this.config.pagerCss);
        }
    }
};
Live.txv.play = {
    init: function () {
        //txv.common.initPage();
        if (!!$("#mod_recommend_video")) {
            var elm = $("#mod_ulike");
            if (elm.length == 0) {
                return;
            }

            var _29 = new Live.DotShow();
            _29.init({
                playRight: elm.find(".next"),
                playLeft: elm.find(".prev"),
                pagerItems: elm.find("ul li"),
                items: $("#ul_ulike_list li"),
                isLazy: false,
                showCnt: 5
            });
        }
    }
}