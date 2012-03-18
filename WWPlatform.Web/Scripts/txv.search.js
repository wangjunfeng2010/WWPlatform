Live.txv.search = {
    mi_pagesize: 9,
    Init: function () {
        var _1 = 1 * $("#s_total").text();
        //        if (_1 <= 0) {
        //            this.RecommLoad();
        //        }
        //        try {
        //            $("#ad_container").show();
        //        } catch (e) { }
        //txv.common.initPage();
        g_key = $("#s_key").text();
        g_a1 = $("#s_a1").text();
        g_a2 = $("#s_a2").text();
        g_a3 = $("#s_a3").text();
        var w = g_key.trim();
        $("#iWord").val(w);
        var _2 = w + " - 搜索 文化中国 百家讲坛";
        document.title = (_2);

        var _31 = $(".mod_cont dt a");
        _31.each(function (i, el) {
            var tit = $(el).text();
            $(el).html(tit.replace(new RegExp("(" + g_key + ")"), "<em class='c_txt3'>$1</em>"));
        });

        var _32 = $(".mod_cont dd a");
        _32.each(function (i, el) {
            var tit = $(el).text();
            $(el).html(tit.replace(new RegExp("(" + g_key + ")"), "<em class='c_txt3'>$1</em>"));
        });

        var ms_type = $("#search_type").text();
        switch (ms_type) {
            case "1":
                $("#bt_albums").parents("li").addClass("current");
                break;
            case "2":
                $("#bt_videos").parents("li").addClass("current");
                break;
            case "3":
                $("#bt_fictions").parents("li").addClass("current");
                break;
        }

        var pager = txv.search.CreatePager(ms_type);
        txv.search.setPagerTemplate(pager);
        txv.search.RenderPager(pager, _1, $("#s_pagenum").text());
    },
    setPagerTemplate: function (_75) {
        _75.Templet.PREVIOUS_DISABLE = "<span class=\"prev_disable prev\"><span>上一页</span></span>";
        _75.Templet.PREVIOUS_ENABLE = "<a href=\"javascript:;\" id=\"pager_previous_{id}\" class=\"prev\" title=\"上一页\"><span>上一页</span></a>\n";
        _75.Templet.PAGE_NORMAL = "<a href=\"javascript:;\" id=\"pager_num_{id}_{pagenum}\" class=\"c_txt6\" title=\"{pagenum}\"><span>{pagenum}</span></a>\n";
        _75.Templet.LAST_ENABLE = "<a href=\"javascript:;\" title=\"{totpage}\" class=\"c_txt6\" id=\"pager_last_{id}\"><span>{totpage}</span></a>\n";
        _75.Templet.NEXT_DISABLE = "<span class=\"next_disable next\"><span>下一页</span></span>\n";
        _75.Templet.NEXT_ENABLE = "<a href=\"javascript:;\" title=\"下一页\" id=\"pager_next_{id}\" class=\"next\"><span>下一页</span></a>\n";
        _75.Templet.GO_PAGE_INPUT = "";
        _75.Templet.GO_PAGE_BUTTON = "";
        _75.Templet.PAGE_COUNT = "";
        _75.elmId = "pager";
    },
    CreatePager: function (_81) {
        var _8e = new Live.Pager();
        _8e.offsetPage = 1;
        _8e.currentPage = 1;
        _8e.showError = function (msg) {
            alert(msg);
        },
        _8e.onTurnPage = function (_8f) {
            JumpSearch(g_key, _8f - 1, _81, 1);
            return false;
        };
        return _8e;
    },
    RenderPager: function (_90, _91, _92) {
        if (_91 == 0) {
            _90.init($("#" + _90.elmId), 1);
        }
        else {
            _90.currentPage = _92 * 1 + 1;
            var _93 = Math.floor((_91 - 1) / txv.search.mi_pagesize) + 1;
            _90.init($("#" + _90.elmId), _93);
        }

        _90.fillPager();
    },
    OutputMsg: function (_19) {
        var _1a = {};
        _1a.search_info = _19;
        txv.template.fillHtml(_1a, "info_tpl", "search_body");
    },
    GetSearchBody: function () {
        var url = window.location + "";
        var pos = url.indexOf("?");
        if (pos < 0) {
            txv.search.OutputMsg("输入错误，请重新搜索");
        } else {
            url_sns = "/fcgi-bin/search" + url.substr(pos);
            $.ajax({
                type: "GET",
                url: url_sns,
                cache: false,
                dataType: "json",
                error: function () {
                    txv.search.OutputMsg("搜索失败，请稍后再试");
                },
                success: function (_2b) {
                    txv.template.fillHtml(_2b, "result_tpl", "search_body", function () {
                        txv.search.Init();
                    });
                }
            });
        }
    },
    RecommLoad: function () {
        url = "/json/recomm/search6.json";
        $j.getScript(url, function () {
            if (typeof (recommCovers) != "undefined" && recommCovers.covers.length > 0) {
                var obj = {
                    covers: []
                };
                var _1f = 6;
                if (recommCovers.covers.length < _1f) {
                    _1f = recommCovers.covers.length;
                }
                for (var _20 = 0; _20 < _1f; _20++) {
                    obj.covers.push({});
                    obj.covers[_20].url = Live.txv.common.getDetailUrl(recommCovers.covers[_20].id);
                    obj.covers[_20].pic = ["http://imgcache.qq.com/qqlive/img/jpgcache/files/qqvideo/", recommCovers.covers[_20].id.charAt(0), "/", recommCovers.covers[_20].id, "_m.jpg"].join("");
                    obj.covers[_20].tt = recommCovers.covers[_20].tt;
                    obj.covers[_20].stt = recommCovers.covers[_20].stt;
                }
                txv.template.fillHtml(obj, "recomm_tpt", "recomm_cont");
            }
        });
    }
};