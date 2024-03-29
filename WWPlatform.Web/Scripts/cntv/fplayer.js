var version = '0.171.5.8.3.1';
var adversion = 'ad0.171.5.8.3.1';
var playerW = 640;
var playerH = 480;
var bgAdW = 960;
var bgAdH = 480;
var rollAdW = 640;
var rollAdH = 30;
var html5IsDebug = false;
var html5IsShowPlayer = true;
var html5Media_events = new Array();
html5Media_events["loadstart"] = 0;
html5Media_events["progress"] = 0;
html5Media_events["suspend"] = 0;
html5Media_events["emptied"] = 0;
html5Media_events["stalled"] = 0;
html5Media_events["play"] = 0;
html5Media_events["pause"] = 0;
html5Media_events["loadedmetadata"] = 0;
html5Media_events["loadeddata"] = 0;
html5Media_events["waiting"] = 0;
html5Media_events["playing"] = 0;
html5Media_events["canplay"] = 0;
html5Media_events["canplaythrough"] = 0;
html5Media_events["seeking"] = 0;
html5Media_events["seeked"] = 0;
html5Media_events["timeupdate"] = 0;
html5Media_events["ended"] = 0;
html5Media_events["ratechange"] = 0;
html5Media_events["durationchange"] = 0;
html5Media_events["volumechange"] = 0;
html5Media_events["error"] = 0;

var html5PlayerLeft = 160;
var html5PlayerTop = 0;
var html5PlayerWidth;
var html5PlayerHeight;
var progressBarWidth;
var progressBarX = 150;
var chapterVos;
var totalLength = 1;
var currentIndex = 0;
var debugString = "debug mode";
var html5IsRtmp = false;
var rest = 0;
var isGetJsData = false;
var isGetJsDataTimeOut = false;
var timeOutTimer;
function isIPad() {
    return /(iphone|ipad)/i.test(navigator.userAgent);
}
function createPlayer(playerId, w, h) {
    html5PlayerWidth = playerW;
    html5PlayerHeight = playerH;
    var fo;
    if (IsMaxthon()) {
        fo = new SWFObject("http://player.cntv.cn/standard/cntvplayer.swf?v=" + version + "&a=" + Math.random(), playerId, playerW, playerH, "10.0.0.0", "#000000");
    } else {
        fo = new SWFObject("http://player.cntv.cn/standard/cntvplayer.swf?v=" + version, playerId, playerW, playerH, "10.0.0.0", "#000000");
    }
    fo.addVariable("languageConfig", "http://kejiao.cntv.cn/nettv/Library/discovery/player/zh_cn.xml");
    fo.addVariable("qmFrequency", "1");
    fo.addVariable("wmode", "window");
    fo.addVariable("dynamicDataPath", "http://vdd.player.cntv.cn/index.php");
    //hack cntvplayer
    fo.addVariable("widgetsConfig", "http://js.player.cntv.cn/xml/widgetsConfig/outside-error.xml");

    fo.addParam("wmode", "window");
    fo.addParam("quality", "best");
    fo.addParam("menu", "false");
    fo.addParam("allowFullScreen", "true");
    fo.addParam("allowScriptAccess", "always");
    fo.addVariable("isConviva", "false");
    return fo;
}
function writePlayer(fo, divId, bgAdDataPath, rollAdDataPath) {
    if (isIPad()) {
        var jsDataUrl = "http://vdd.player.cntv.cn/html5.php?pid=" + fo.getVariable("videoCenterId") + "&tai=ipad";
        createHtml5Player(divId, html5PlayerWidth, html5PlayerHeight, jsDataUrl);
    } else {
        doLayOut(fo, divId, bgAdDataPath, rollAdDataPath);
    }
}
function doLayOut(playerFo, baseDivId, bgAdDataPath, rollAdDataPath) {
    var baseDiv = document.getElementById(baseDivId);

    //	var bgDiv = document.createElement("div");
    //	bgDiv.setAttribute("id","bgAd_div");
    //	bgDiv.style.width = '960px';
    //	bgDiv.style.height = '480px';
    //	baseDiv.appendChild(bgDiv);

    var playerDiv = document.createElement("div");
    playerDiv.setAttribute("id", "player_div");
    //	playerDiv.style.position = "absolute";
    playerDiv.style.width = '640px';
    playerDiv.style.height = '480px';
    playerDiv.style.left = '160px';
    playerDiv.style.top = '0px';
    baseDiv.appendChild(playerDiv);
    playerFo.write("player_div");

    //	var bgfo = new SWFObject("http://player.cntv.cn/adplayer/cntvBgAdPlayer.swf?v="+adversion,"bgAdPlayer",bgAdW,bgAdH,"10.0.0.0");
    //	bgfo.addParam("flashvars","xmlPath="+bgAdDataPath);
    //	bgfo.addParam("wmode", "transparent");
    //	bgfo.addParam("quality", "best");
    //	bgfo.addParam("menu","false");
    //	bgfo.addParam("allowFullScreen", "true");
    //	bgfo.addParam("allowScriptAccess","always");
    //	bgfo.write("bgAd_div");
}
function isCntvBoxLoader() { try { var f; if (window.ActiveXObject) { f = new ActiveXObject("CNTVCBox.DispCBox.1") || false } else { netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect"); f = Components.classes["@cntv.cn/CBox/speedup;1"].createInstance().QueryInterface(Components.interfaces.ICntvSpeedup) || false } if (f) { f.RunApp(); return true; } else { return false; } } catch (e) { return false; } }
function IsMaxthon() {
    try {
        window.external.max_invoke("GetHotKey");
        return true;
    } catch (ex) {
        return false;
    }
}
function isIPad() {
    return /(iphone|ipad)/i.test(navigator.userAgent);
}
function createHtml5Player(mainDivId, _w, _h, dataUrl) {
    html5PlayerWidth = _w;
    html5PlayerHeight = _h - 30;
    progressBarWidth = html5PlayerWidth - 310;
    var baseDiv = document.getElementById(mainDivId);
    baseDiv.innerHTML = "";
    baseDiv.style.position = "relative";
    baseDiv.style.width = html5PlayerWidth + "px";
    baseDiv.style.height = _h + "px";
    document._baseDiv = baseDiv;

    var html5BgImage = createElementByType("img", "html5BgImage", "absolute", bgAdW + "px", bgAdH + "px", "0px", "0px");
    html5BgImage.src = "http://player.cntv.cn/images/ipad/bgImage/default_bg_1.jpg";
    document._baseDiv.appendChild(html5BgImage);

    var html5VideoBack = createElementByType("div", "html5VideoBack", "absolute", html5PlayerWidth + "px", html5PlayerHeight + "px", html5PlayerLeft + "px", html5PlayerTop + "px");
    html5VideoBack.style.backgroundColor = "#000000";
    document._baseDiv.appendChild(html5VideoBack);
    document._html5VideoBack = document.getElementById("html5VideoBack");

    if (html5IsShowPlayer) {
        html5LoadJsData(dataUrl);
    }
}
function createPlayerElement() {
    if (html5IsShowPlayer) {

        var playerDiv = document.createElement("video");
        playerDiv.setAttribute("id", "html5Player");
        playerDiv.style.position = "absolute";
        playerDiv.style.zIndex = "1";
        playerDiv.style.width = html5PlayerWidth + "px";
        playerDiv.style.height = html5PlayerHeight + "px";
        playerDiv.style.left = html5PlayerLeft + "px";
        playerDiv.style.top = html5PlayerTop + "px";
        playerDiv.src = chapterVos[currentIndex]["url"];
        playerDiv.controls = "controls";
        playerDiv.preload = "none";
        document._baseDiv.appendChild(playerDiv);
        document._html5Player = document.getElementById("html5Player");
        fadeTo(0);
    }
    if (html5IsDebug) {
        var debugText = document.createElement("textarea");
        debugText.setAttribute("id", "debugText");
        debugText.style.position = "absolute";
        debugText.style.width = '200px';
        debugText.style.height = '480px';
        debugText.style.left = '640px';
        debugText.style.top = '0px';
        debugText.value = debugString;
        document._baseDiv.appendChild(debugText);
        document._debugText = document.getElementById("debugText");
    }

}
function createControlBar() {

    var controlDiv = createElementByType("div", "html5ControlDiv", "absolute", html5PlayerWidth + "px", "30px", html5PlayerLeft + "px", (html5PlayerTop + html5PlayerHeight) + "px");
    document._baseDiv.appendChild(controlDiv);

    var controlBarBg = createElementByType("img", "controlBarBg", "absolute", html5PlayerWidth + "px", "30px", "0px", "0px");
    controlBarBg.src = "http://player.cntv.cn/html5Player/images/bar_bg.png";
    controlDiv.appendChild(controlBarBg);

    var playButtonBG = createElementByType("img", "playButtonBG", "absolute", "140px", "30px", "0px", "0px");
    playButtonBG.src = "http://player.cntv.cn/html5Player/images/play_button_bg.png";
    html5AddListener(playButtonBG, "click", clickPlayButton);
    controlDiv.appendChild(playButtonBG);


    var progressBG = createElementByType("img", "progressBG", "absolute", progressBarWidth + "px", "12px", progressBarX + "px", "9px");
    progressBG.src = "http://player.cntv.cn/html5Player/images/progressBG.png";
    controlDiv.appendChild(progressBG);


    var progressPlayed = createElementByType("img", "progressPlayed", "absolute", "0px", "12px", "150px", "9px");
    progressPlayed.src = "http://player.cntv.cn/html5Player/images/playedBG.png";
    controlDiv.appendChild(progressPlayed);
    document._progressPlayed = document.getElementById("progressPlayed");

    var playTip = createElementByType("img", "playTip", "absolute", "16px", "15px", (progressBarX - 8) + "px", "7px");
    playTip.src = "http://player.cntv.cn/html5Player/images/play_tip.png";
    controlDiv.appendChild(playTip);
    document._playTip = document.getElementById("playTip");

    var progressCover = createElementByType("div", "progressCover", "absolute", progressBarWidth + "px", "30px", (progressBarX) + "px", "0px");
    controlDiv.appendChild(progressCover);
    html5AddListener(progressCover, "click", onPlayTipMouseDown);
    document._progressCover = document.getElementById("progressCover");

    var playButton = createElementByType("img", "playButton", "absolute", "10px", "8px", "25px", "11px");
    playButton.src = "http://player.cntv.cn/html5Player/images/play_button.png";
    controlDiv.appendChild(playButton);
    document._playButton = document.getElementById("playButton");

    var pauseButton = createElementByType("img", "pauseButton", "absolute", "10px", "8px", "25px", "11px");
    pauseButton.src = "http://player.cntv.cn/html5Player/images/pause_button.png";
    controlDiv.appendChild(pauseButton);
    document._pauseButton = document.getElementById("pauseButton");

    var cntvLogo = createElementByType("img", "cntvLogo", "absolute", "53px", "22px", (html5PlayerWidth - 60) + "px", "4px");
    cntvLogo.src = "http://player.cntv.cn/html5Player/images/cntv_logo_player.png";
    controlDiv.appendChild(cntvLogo);

    var timePanel = createElementByType("input", "timePanel", "absolute", "80px", "22px", (html5PlayerWidth - 140) + "px", "5px");
    timePanel.style.border = "0px";
    timePanel.style.backgroundColor = "transparent";
    timePanel.style.color = "#ffffff";
    timePanel.setAttribute("type", "text");
    timePanel.value = "00:00 / 00:00";
    controlDiv.appendChild(timePanel);
    document._timePanel = document.getElementById("timePanel");

    isPlaying(false);
}
function createElementByType(type, _id, position, _w, _h, _l, _t) {
    var el = document.createElement(type);
    el.setAttribute("id", _id);
    el.style.position = position;
    el.style.width = _w;
    el.style.height = _h;
    el.style.left = _l;
    el.style.top = _t;
    return el;
}
function html5InitEvent() {
    for (key in html5Media_events) {
        html5AddListener(document._html5Player, key, capture);
    }
}
function html5PlayerNext() {
    if (currentIndex + 1 < chapterVos.length) {
        currentIndex++;
        htmlPlayAFile();
    } else {
        document._html5Player.pause();
        fadeTo(0);
        showReplay();
    }
}
function getIPadVersion() {
    var ipadVersion = 30201;
    var VersionStr = "Version/";
    var info = navigator.userAgent;
    var arr; // = []; 
    var pos = navigator.userAgent.indexOf(VersionStr);

    if (pos > 0) {
        var pStart = pos + VersionStr.length;
        var pEnd = pStart;
        while ((navigator.userAgent[pEnd] >= '0' && navigator.userAgent[pEnd] <= '9') || navigator.userAgent[pEnd] == '.') {
            pEnd++
        }

        info = info.slice(pStart, pEnd); //info = 5.0.2.1

        arr = info.split(".");

        if (arr != null)//如果数组有效
        {
            if (arr.length > 0) {
                ipadVersion = Number(arr[0]) * 10000;
            }
            if (arr.length > 1) {
                ipadVersion += Number(arr[1]) * 100;
            }
            if (arr.length > 2) {
                ipadVersion += Number(arr[2]);
            }
        }
    }

    return ipadVersion;
}

var isfirst = true;
function htmlPlayAFile() {
    document._html5Player.pause();
    document._html5Player.src = chapterVos[currentIndex]["url"];
    if (!isfirst) {
        document._html5Player.load();
        document._html5Player.play();
    }
    isfirst = false;
}
function setPlayed() {
    var base = getCurrentTimeBase();
    var playedTime = timeToString(getCurrentTimeBase() + document._html5Player.currentTime);
    var totalTime = timeToString(totalLength);
    document._timePanel.value = playedTime + " / " + totalTime;
    var setTo = ((getCurrentTimeBase() + document._html5Player.currentTime) / totalLength);
    document._progressPlayed.style.width = parseInt(setTo * progressBarWidth) + "px";
    document._playTip.style.left = parseInt(setTo * progressBarWidth + progressBarX - 8) + "px";
}
function clickPlayButton(event) {
    playOrPause();
}
function isPlaying(v) {
    if (v) {
        document._playButton.style.visibility = "hidden";
        document._pauseButton.style.visibility = "visible";
    } else {
        document._playButton.style.visibility = "visible";
        document._pauseButton.style.visibility = "hidden";
    }
}
function playOrPause() {
    if (document._html5Player.paused) {
        document._html5Player.play();
    }
    else {
        document._html5Player.pause();
    }
    isPlaying(!document._html5Player.paused);
}
function onError(event) {
    debug("error");
}
var addOnStr;
function capture(event) {
    if (event.type == "ended") {
        html5PlayerNext();
    }
    if (event.type == "timeupdate") {
        setPlayed();
    }
    if (event.type == "pause") {
        isPlaying(false);
    }
    if (event.type == "playing") {
        isPlaying(true);
        fadeTo(1);
        if (html5IsRtmp) {
            totalLength = document._html5Player.duration;
        }
        if (rest != 0) {
            setTimeout(timeToRest, 500);
        }
    }
    if (addOnStr != event.type) {
        debug(event.type);
    }
    addOnStr = event.type;
}
function timeToRest() {
    document._html5Player.currentTime = rest;
    rest = 0;
}
function debug(v) {
    if (html5IsDebug) {
        document._debugText.value = document._debugText.value + "\n" + v;
        document._debugText.scrollTop = document._debugText.scrollHeight;
    }
}

function html5AddListener(obj, type, handler) {
    if (obj.attachEvent) {
        obj.attachEvent('on' + type, handler);
    } else if (obj.addEventListener) {
        obj.addEventListener(type, handler, false);
    }
}

function html5LoadJsData(url) {
    var jsLoader = createElementByType("script", "jsLoader", "absolute", "0px", "0px", "0px", "0px");
    jsLoader.src = url;
    document._baseDiv.appendChild(jsLoader);
    //alert("aaa");
    timeOutTimer = setTimeout(loadJsDataFail, 5000);
}
function loadJsDataFail() {
    isGetJsDataTimeOut = true;
    isGetJsData = false;
    showError("未能获得视频数据");
}
function getHtml5VideoData(data) {
    if (!isGetJsDataTimeOut) {
        clearTimeout(timeOutTimer);
        var errStr = "";
        try {
            var obj = eval('(' + data + ')');
        } catch (e) {
            errStr = "数据格式错误";
            isGetJsData = false;
            showError(errStr);
            return;
        }
        if (obj["is_ipad_support"] == "true") {
            if (obj["ack"] == "yes") {
                if (obj["video"]["chapters"]) {
                    chapterVos = (obj["video"]["chapters"]);
                    totalLength = getTotalLength();
                    html5IsRtmp = false;
                    isGetJsData = true;
                } else if (obj["video"]["streams"]) {
                    var choosenUrl = "";
                    for (var i = 0; i < obj["video"]["streams"].length; i++) {
                        if (obj["video"]["streams"][i]["bitRate"] == "450") {
                            choosenUrl = 'http://v.cctv.com/rtmp' + obj["video"]["streams"][i]["streamName"].replace('mp4:v', '');
                        }
                    }
                    if (choosenUrl == "") {
                        choosenUrl = 'http://v.cctv.com/rtmp' + obj["video"]["streams"][0]["streamName"].replace('mp4:v', '');
                    }
                    var vVo = { "url": choosenUrl, "duration": "0" };
                    chapterVos = new Array();
                    chapterVos[0] = vVo;
                    html5IsRtmp = true;
                    isGetJsData = true;
                } else {
                    errStr = "视频数据格式错误";
                    isGetJsData = false;
                }
            } else {
                errStr = "未能获得视频数据";
                isGetJsData = false;
            }
        } else {
            errStr = "该视频暂没有对应的iPad播放格式，近期将全面支持iPad用户观看，感谢您的支持和关注。";
            isGetJsData = false;
        }
        if (obj["is_invalid_copyright"] != "0") {
            errStr = "We're sorry,this video can not be streamed in your region";
            isGetJsData = false;
        }
        if (isGetJsData) {
            createControlBar();
            createPlayerElement();
            html5InitEvent();
            htmlPlayAFile();
        } else {
            showError(errStr);
        }
    }
}
function showError(err) {
    var errorPanel = createElementByType("div", "errorPanel", "relative", "", "", "0px", "45%");
    errorPanel.style.color = "#FFFFFF";
    errorPanel.innerHTML = err;
    errorPanel.align = "center";
    document._html5VideoBack.appendChild(errorPanel);
}
function getTotalLength() {
    var totalLength = 0.0;
    for (var i = 0; i < chapterVos.length; i++) {
        totalLength += parseInt(chapterVos[i]["duration"]);
    }
    return totalLength;
}
function getCurrentTimeBase() {
    var r = 0;
    if (chapterVos) {
        for (var i = 0; i < currentIndex; i++) {
            if (currentIndex != 0) {
                r += parseInt(chapterVos[i]["duration"]);
            }
        }
    }
    return r;
}

function getChapterIndex(v) {
    var r = 0;
    for (var i = 0; i < chapterVos.length; i++) {
        r += parseInt(chapterVos[i]["duration"]);
        if (v < r) {
            if (i != 0) {
                rest = r - v;
            } else {
                rest = v;
            }
            return i;
        }
    }
    return 0;
}
function onPlayTipMouseDown(event) {
    getClickPos(event);
}
function getClickPos(e) {
    var xPage = (navigator.appName == 'Netscape') ? e.pageX : event.x + document.body.scrollLeft;
    var yPage = (navigator.appName == 'Netscape') ? e.pageY : event.y + document.body.scrollTop;
    identifyImage = document._progressCover;
    img_x = locationLeft(identifyImage);
    var xPos = xPage - img_x;
    if (xPos < progressBarWidth && xPos > 0) {
        var s = ((xPos) / progressBarWidth);
        seekTo(s)
    }
}

function locationLeft(element) {
    offsetTotal = element.offsetLeft;
    scrollTotal = 0;

    if (element.tagName != "BODY") {
        if (element.offsetParent != null)
            return offsetTotal + scrollTotal + locationLeft(element.offsetParent);
    }
    return offsetTotal + scrollTotal;
}
function locationTop(element) {
    offsetTotal = element.offsetTop;
    scrollTotal = 0; //element.scrollTop but we dont want to deal with scrolling - already in page coords


    if (element.tagName != "BODY") {
        if (element.offsetParent != null)
            return offsetTotal + scrollTotal + locationTop(element.offsetParent);
    }
    return offsetTotal + scrollTotal;
}
function seekTo(v) {

    var charpterTo = getChapterIndex(v * totalLength);
    if (charpterTo == currentIndex) {
        document._html5Player.currentTime = rest;
    } else {
        currentIndex = charpterTo;
        htmlPlayAFile();
    }
}
function fadeTo(v) {
    //document._html5Player.style.opacity = v;
}
function showReplay() {
    var replayButton = createElementByType("img", "replayButton", "absolute", "56px", "56px", ((html5PlayerWidth - 56) / 2) + "px", ((html5PlayerHeight - 56) / 2) + "px");
    replayButton.src = "http://player.cntv.cn/html5Player/images/playSceneButtonR.png";
    html5AddListener(replayButton, "click", onReplayClick);
    document._baseDiv.appendChild(replayButton);
    document._replayButton = document.getElementById("replayButton");
}
function onReplayClick(event) {
    debug("replay");
}
function timeToString(v) {
    var intV = parseInt(v);
    var min = parseInt(intV / 60);
    var sec = parseInt(intV % 60);
    return (fillTo2Bit(min) + ":" + fillTo2Bit(sec));
}
function fillTo2Bit(v) {
    if (v < 10) {
        return "0" + v;
    } else {
        return v;
    }
    return v;
}