Live.txv.notice = function () {
    this.auto = true;
    this.target = "b_items";
}

Live.txv.notice.prototype = {
    init: function () {
        var obj = this;
        //仅有一条公告时，不滚动！
        if ($("#" + obj.target + " li").length <= 1) {
            return;
        }
        window.setInterval(function () {
            if (obj.auto) {
                $("#" + obj.target).stop();
                $("#" + obj.target).animate({ top: '-18px' }, 500, function () {
                    $.each($("#" + obj.target + " li"), function (idx, item) {
                        $(item).hide();
                    });
                    var li = $("#" + obj.target + " li:first-child");
                    if (li.length > 0) {
                        li.show();
                        li.clone().appendTo("#" + obj.target);
                        li.remove();
                        $("#" + obj.target).css("top", "0px");
                    }
                });
            }
        }, 3000);

        $(".mod_notice").bind("mouseenter", function () {
            obj.auto = false;
        });

        $(".mod_notice").bind("mouseleave", function () {
            obj.auto = true;
        });
    }
}