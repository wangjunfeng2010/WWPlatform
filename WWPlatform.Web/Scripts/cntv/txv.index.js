Live.txv.slide = function () {
    this.config = {
        auto: true,
        headbar: "slide_headbar",
        photo: "slide_photo",
        footbar: "slide_footbar"
    };
}

Live.txv.slide.prototype = {
    build: function () {
        var length = $("#" + this.config.photo + " a").length;
        for (var i = 0; i < length; i++) {
            $("<div class='slide-bt' index='" + (length - i) + "'></div>").appendTo("#" + this.config.footbar);
        }

        var obj = this;
        $("#" + this.config.footbar + " .slide-bt:last").addClass("bt-on");
        $("#" + this.config.footbar + " .slide-bt").mouseenter(function (e) {
            obj.playNext($(this).attr("index"));
        });

        $('#slide').mouseenter(function () {
            obj.config.auto = false;
        }).mouseleave(function () {
            obj.config.auto = true;
        });

        window.setInterval(function () {
            if (obj.config.auto) {
                obj.playNext();
            }

        }, 3000);
    },

    playNext: function (index) {
        var current = $("#" + this.config.footbar + " .slide-bt.bt-on");
        if (current.length <= 0)
            return;

        //current
        var cIndex = current.attr("index");
        var cLink = $("#" + this.config.photo + " a[index=" + cIndex + "]");

        //next
        var next, nIndex;
        if (index) {
            nIndex = index;
            next = $("#" + this.config.footbar).find("div[index=" + nIndex + "]");
        }
        else {
            //若index==undefined,则表示默认播放下一张图片
            next = current.prev().length > 0 ? current.prev() : $("#" + this.config.footbar + " .slide-bt:last");
            nIndex = $(next).attr("index");
        }

        if (nIndex == cIndex)
            return;

        var nLink = $("#" + this.config.photo + " a[index=" + nIndex + "]");

        //显示head
        var caption = nLink.find("img[alt]").attr("alt");
        $("#" + this.config.headbar).html(caption);

        //延迟加载image
        if (nLink.find("img[_src]").length > 0) {
            var img = nLink.find("img[_src]");
            img.attr("src", img.attr("_src")).removeAttr("_src");
        }

        //slide效果
        next.addClass("bt-on");
        nLink.css("z-index", 2);

        current.removeClass("bt-on");
        cLink.css("z-index", 3).fadeOut(500, function () {
            $(this).css("z-index", "1").show();
            var img = nLink.next("a").find("img[_src]");
            if (img.length > 0) {
                img.attr("src", img.attr("_src")).removeAttr("_src");
            }
        });
    }
};

Live.txv.index = {
    init: function () {
        //txv.common.initPage();
        var slide = new txv.slide();
        slide.build();
    }
}