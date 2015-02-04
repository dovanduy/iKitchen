$(document).ready(function () {
    //bindEditPage(); // not in use as we use Asp.net mvc built-in feature
    bindListPage();
    //initTooltip();
    bindPostUrl();
    bindOpenUrl();
});

function initTooltip() {

    $.tools.tooltip.addEffect("myEffect",

    // show function
	function (done) {

	    // 'this' variable is a reference to the tooltip API
	    var conf = this.getConf(), tip = this.getTip();

	    // peform your effect. for example:
	    tip.css({ opacity: 1, width: '+=50' });

	    // after you have finished you must do
	    done.call();
	},

    // hide function
	function (done) {

	    // peform your effect. for example:
	    this.getTip().animate({ opacity: 0, width: '-=50' }, done);
	}
);
    $("[title]").tooltip();
}

function bindListPage() {
    $("#select_all").click(function () {
        $("input[type=checkbox][name=ids]").prop("checked", $("#select_all").prop("checked"));
    });
    $("#confirmButton").click(function () {
        var url = $('#modalPopUp').data("rel");
        var ids = $('#modalPopUp').data("ids");
        if (url && ids)
            DoPost(url, "?ids=" + ids);
    });

    $("a.delete").click(function () {
        var ids = "";
        $("input[name=ids]").each(function () {
            if ($(this).prop("checked")) {
                ids += $(this).val() + ",";
            }
        });
        if (ids == "") {
            showMessage("请选择要删除的记录");
        }
        else {
            var url = $(this).attr("rel");
            $("#modalPopUp").data("rel", url).data("ids", ids).modal({
                show: true,
                keyboard: false
            });
            //if (deleteConfirmed) {
            //    DoPost(url, "?ids=" + ids);
            //}
        }
    });

    $("select.numPerPage").change(function () {
        document.cookie = "numPerPage=" + $(this).val();
        window.location.reload();
    });

    $("a[actionurl]").click(function () {
        var ids = "";
        var targetState = $(this).attr("targetstate");

        $("input[name=ids]").each(function () {
            if ($(this).prop("checked") && (!targetState || $(this).attr("state") == targetState)) {
                ids += $(this).val() + ",";
            }
        });
        if (ids == "") {
            if (targetState)
                showMessage("请选择可执行该操作的记录！");
            else
                showMessage("请选择记录！");
        }
        else {
            // 修改id
            if ($(this).attr("idtype") == "Single") {
                ids = ids.split(',')[0];
            }
            if (!$(this).attr("confirmmessage") || confirm($(this).attr("confirmmessage"))) {
                var url = $(this).attr("actionurl");
                if ($(this).attr("IdType") == "Single")
                    DoPost(url, "?id=" + ids);
                else
                    DoPost(url, "?ids=" + ids);
            }
        }
    });

    $("a[gotourl]").click(function () {
        var ids = "";
        var targetState = $(this).attr("targetstate");

        $("input[name=ids]").each(function () {
            if ($(this).prop("checked") && (!targetState || $(this).attr("state") == targetState)) {
                ids += $(this).val() + ",";
            }
        });
        if (ids == "") {
            if (targetState)
                showMessage("请选择可执行该操作的记录！");
            else
                showMessage("请选择记录！");
        }
        else {
            // 修改id
            if ($(this).attr("idtype") == "Single") {
                ids = ids.split(',')[0];
            }
            if (!$(this).attr("confirmmessage") || confirm($(this).attr("confirmmessage"))) {
                var url = $(this).attr("gotourl") + ids;
                location.href = url;
            }
        }
        return false;
    });

}

function initDefaultText() {
    var inputs = $("[defaultText]");
    inputs.each(function () {
        if (!$(this).val()) {
            $(this).val($(this).attr("defaultText"));
            $(this).addClass("defaultText");
        }
    });

    // 楼盘输入框
    inputs.focus(function () {
        if ($(this).val() == $(this).attr("defaultText"))
            $(this).val('');
        $(this).removeClass("defaultText");
    });

}

function clearDefaultText() {
    var inputs = $("[defaultText]");
    inputs.each(function () {
        if ($(this).val() == $(this).attr("defaultText")) {
            $(this).val('');
            $(this).removeClass("defaultText");
        }
    });
}

function bindEditPage() {
    $("a.back").click(function () {
        history.back();
    });

    var eventName = "blur";
    //if ($.browser.msie) {
    //    eventName = "focusout";
    //}

    $("#editForm input.required").bind(eventName, function () {
        validate(this);
    });
    $("#editForm").bind("submit", function () {
        clearDefaultText();
        var success = true;
        $("#editForm input.required").each(function () {
            success = validate(this) && success;
        });
        if (!success && parent.showMessage) {
            parent.showMessage("请正确填写表单！错误的内容以红色标出！");
        }
        return success;
    });
}

function validate(element) {
    var messageSpan = $("span[targetId=" + $(element).attr("id") + "]");
    isErrorBefore = messageSpan.hasClass("warnMsg");
    messageSpan.removeClass("tipMsg");
    messageSpan.removeClass("successMsg");
    messageSpan.removeClass("warnMsg");
    var success = false;
    if ($(element).hasClass("required")) {
        success = !!$(element).val();
    }
    if ($(element).hasClass("char")) { // 字母、数字、下划线等普通字符的组合
        success = success && isChar($(element).val());
    }
    if ($(element).hasClass("number")) {
        var number = parseInt($(element).val());
        success = success && !isNaN(number);
        if ($(element).attr("max")) {
            success = success && number <= parseInt($(element).attr("max"));
        }
        if ($(element).attr("min")) {
            success = success && number >= parseInt($(element).attr("min"));
        }
    }
    if ($(element).hasClass("email")) {
        success = success && isEmail($(element).val());
    }
    if ($(element).hasClass("mobile")) {
        success = success && isMobile($(element).val());
    }
    if ($(element).hasClass("length")) {
        success = success && $(element).val().length >= parseInt($(element).attr("minlength"));
    }

    // messageSpan.parent().removeClass("warnBlock");
    if (!$(element).attr("tipMsg")) {
        if (!success) {
            messageSpan.addClass("warnMsg");
            // messageSpan.parent().addClass("warnBlock");
            return false;
        }
        else {
            if (isErrorBefore) {
                messageSpan.addClass("successMsg");
                messageSpan.fadeOut(2000);
            }
            else {
                messageSpan.addClass("tipMsg");
            }
            return true;
        }
    }
    else {
        // efe only
        if (!success) {
            $(element).addClass("errorInput");
            $(element).attr("title", $(element).attr("tipMsg"));
            return false;
        }
        else {
            $(element).removeClass("errorInput");
            $(element).attr("title", "");
            return true;
        }
    }
}

// 判断是否是字母、数字、下划线等普通字符的组合
function isChar(str) {
    var reg = new RegExp(/^\w+$/);
    return reg.test(str);
}

// 判断是否是字母、数字、下划线、汉字等的组合
function isCharCn(str) {
    var reg = new RegExp(/^[a-zA-Z0-9\u4e00-\u9fa5]+$/);
    return reg.test(str);
}

function isEmail(email) {
    var reg = new RegExp(/^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$/);
    //var reg = new RegExp(/^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$/);
    return reg.test(email);
}

function isMobile(mobile) {
    var regu = /^[1][3,4,5,8][0-9]{9}$/;
    var re = new RegExp(regu);
    return re.test(mobile);
}

// fix the height of iframe. from: http://hi.baidu.com/djyuning520/blog/item/8489061eb7eb18c6a786699c.html
function fixFrame(obj) {
    if (!obj)
        return;
    var cwin = obj;
    if (document.getElementById) {
        if (cwin && !window.opera) {
            if (cwin.contentDocument && cwin.contentDocument.body.offsetHeight)
                cwin.height = cwin.contentDocument.body.offsetHeight + 20; //FF　NS
            else if (cwin.Document && cwin.Document.body.scrollHeight)
                cwin.height = cwin.Document.body.scrollHeight + 10; //IE
        }
        else {
            if (cwin.contentWindow.document && cwin.contentWindow.document.body.scrollHeight)
                cwin.height = cwin.contentWindow.document.body.scrollHeight; //Opera
        }

        if (cwin.height < 400)
            cwin.height = 400;
    }
}

function bindFrameUrl() {
    $("[url]").bind("click", function () {
        var url = $(this).attr("url");
        if (url) {
            $("[url]").removeClass("selected");
            gotoFrame(url);
            $(this).addClass("selected");
        }
    });
}

function bindPostUrl() {
    $("[postUrl]").click(function () {
        if ($(this).attr("ConfirmMessage") && !confirm($(this).attr("ConfirmMessage"))) {
            return false;
        }
        DoPost($(this).attr("postUrl"));
        return false;
    });
}

function bindOpenUrl() {
    $("[openUrl]").click(function () {
        window.open($(this).attr("openUrl"));
    });
}

function gotoFrame(url) {
    $("#frame").attr("src", url);
}

//分页跳转函数
function goto_page(url) {
    var pagenum = parseInt($('#gotoPage').val());
    if (pagenum > 0) {
        window.location = url.replace('###', pagenum - 1);
    } else {
        showMessage('页码必须为正整数!');
        $('#gotoPage').focus();
    }
}

//分页跳转函数
function changePageSize() {
    var pageSize = parseInt($('#PageSize').val());
    if (pageSize) {
        document.cookie = "numPerPage=" + pageSize;
    } else {
        document.cookie = "numPerPage=; expires=Thu, 01 Jan 1970 00:00:00 GMT";
    }
}

function DoPost(url, param) {
    if (param == undefined)
        param = '';

    $.ajax({
        beforeSend: function (XMLHttpRequest) {
            XMLHttpRequest.setRequestHeader("ajax", "true");
        },
        type: 'POST',
        url: url + param,
        dataType: "json",
        cache: false,
        success: ajaxSuccess,
        error: ajaxError
    });
}

function ajaxSuccess(data) {
    if (data.message) {
        showMessage(data.message);
    }
    if (data.action == 'reload') {
        // window.location.replace(window.location);
        window.location.reload();
    }
    else if (data.action == 'redirect' && data.url) {
        location.href = data.url;
    }
}

function ajaxError(XMLHttpRequest, textStatus, errorThrown) {
    // alert('操作失败！' + XMLHttpRequest.responseText);
    // console.debug(XMLHttpRequest.responseText);
    //alert("Failed due to unknown reason, please try later.");
}

function showMessage(message) {
    //$("#messageDiv").text(message).show().delay(2000).fadeOut();
    $.gritter.add({
        title: message,
        //text: 'Fades out automatically in amount of time.',
    });
}