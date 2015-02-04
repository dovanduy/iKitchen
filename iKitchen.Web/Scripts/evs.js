$(function () {
    // todo: initialize pages
});

function initFirstPage() {
    if (typeof (searchCondition) != "undefined")
        initSearchCondition();
    initColspanFun();
    initFavBtn();
    // 发布房源选择区域
    $("#districtSelect").bind("change", function () {
        selectDistrict();
    });
    selectDistrict();
    initAdvertisementImage();
}


var lastAutoMatch = '';
var lastAutoMatchFuntion = null;
// 初始化业务来源自动匹配
function initCustomerAutoMatch() {
    $("#CustomerName").keyup(function () {
        clearTimeout(lastAutoMatchFuntion);
        lastAutoMatchFuntion = setTimeout(function () {
            var title = $.trim($("#CustomerName").val());
            if (title && title != lastAutoMatch) {
                lastAutoMatch = title;
                $.ajax({
                    type: 'POST',
                    url: '/Customer/GetCustomerList?name=' + escape(title),
                    dataType: "json",
                    cache: false,
                    success: function (data) {
                        //alert(data);
                        $("#suggestion").hide();
                        if (data && data.length > 0) {
                            if (data.length == 1 && data[0].Title == title) {
                                // 精准匹配的业务来源
                                $("#CustomerMobile").val(data[0].Mobile);
                                $("#CustomerEmail").val(data[0].Email);
                                $("#autoMatchTip").show().fadeOut(3000);
                            }
                            else {
                                $("#customerCondidate").text('');
                                $("#suggestion").fadeIn();
                                for (var i = 0; i < data.length; i++) {
                                    appendCustomerCondidate(data[i]);
                                }
                            }
                        }
                    },
                    error: ajaxError
                });
            }
        }, 1000);
    });
}

// 添加匹配的候选楼盘
function appendCustomerCondidate(customer) {
    var html = "<a href='javascript:void(0)' class='fcbl prm' CustomerMobile='" + customer.Mobile + "' CustomerEmail='" + customer.Email + "'>" + customer.Title + "</a> ";
    $("#customerCondidate").append(html);
    $("#customerCondidate a").bind("click", function () {
        $("#CustomerName").val($(this).text());
        $("#CustomerMobile").val($(this).attr("CustomerMobile"));
        $("#CustomerEmail").val($(this).attr("CustomerEmail"));
        // 选择完毕之后隐藏
        setTimeout(function () { $("#suggestion").fadeOut(1000); }, 1000);
    });
}

function initPublishTypeTab() {
    $("a[publishType]").click(function () {
        if (searchCondition.publishType != $(this).attr("publishType")) {
            searchCondition.publishType = $(this).attr("publishType");
            doSearch();
        }
    });
}

function bindForm() {
    $("form").each(function () {
        var eventName = "blur";
        if ($.browser.msie) {
            eventName = "focusout";
        }

        $(this).find("input.required").bind(eventName, function () {
            validateInput(this);
        });
    });

    $("form").bind("submit", function () {
        clearDefaultText();
        var success = true;
        $(this).find("input.required").each(function () {
            success = validateInput(this) && success;
        });
        if (!success) {
            var alertMessage = $(this).attr("alertMessage");
            alert(alertMessage);
        }
        return success;
    });

}

function validateInput(element) {
    var messageSpan = $("span[targetId='" + $(element).attr("id") + "']");
    messageSpan.removeClass("errorMsg");
    messageSpan.removeClass("notification");

    var success = false;
    if ($(element).hasClass("required")) {
        success = !!$(element).val();
    }
    if ($(element).hasClass("char")) { // 字母、数字、汉字等普通字符的组合
        success = success && isCharCn($(element).val());
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

    if ($(element).attr("id") == "ConfirmPassword") {
        success = success && $(element).val() == $("#Password").val();
    }

    if (!success) {
        messageSpan.show();
        messageSpan.addClass("errorMsg");

        return false;
    }
    else {
        messageSpan.addClass("notification");
        messageSpan.fadeOut();
        return true;
    }
}

function AddAutoRefreshTime(userIdAppendix) {
    if (!userIdAppendix) {
        userIdAppendix = "?";
    }
    else {
        userIdAppendix += "&";
    }
    if ($("#StartTime").val() && $("#EndTime").val()) {
        DoPost("/AutoRefreshTime/Add" + userIdAppendix + "StartTime=" + $("#StartTime").val() + "&EndTime=" + $("#EndTime").val());
    }
    else {
        alert("请选择开始时间和结束时间！");
    }
}

function ajaxFileUpload() {
    $("#loading")
		.ajaxStart(function () {
		    $(this).show();
		})
		.ajaxComplete(function () {
		    $(this).hide();
		});

    $.ajaxFileUpload
		(
			{
			    url: 'doajaxfileupload.php',
			    secureuri: false,
			    fileElementId: 'fileToUpload',
			    dataType: 'json',
			    data: { name: 'logan', id: 'id' },
			    success: function (data, status) {
			        if (typeof (data.error) != 'undefined') {
			            if (data.error != '') {
			                alert(data.error);
			            } else {
			                alert(data.msg);
			            }
			        }
			    },
			    error: function (data, status, e) {
			        alert(e);
			    }
			}
		)

    return false;

}

function AddImage(filename, picId, type) {
    //    console.debug(filename);
    //    console.debug(picId);
    //    console.debug(type);
    var inputName = "SaleInfoPictures";
    if (type == 1) {
        inputName = "ProjectPictures";
    }
    var picture = $("#pictureTemplate" + type).clone();
    picture.find("input[type=hidden]").val(picId).attr("name", inputName);
    picture.find("img").attr("src", "/images/upload/" + filename);
    picture.removeClass("hidden").attr("id", "");
    picture.find("a.delBtn").click(function () {
        if ($(this).attr('picType') == '0') {
            count0 = count0 - 1;
            renewPictureTip();
        }
        else if ($(this).attr('picType') == '1') {
            count1 = count1 - 1;
        }
        $(this).parent().parent().fadeOut(function () {
            $(this).remove();
        });
    });
    picture.find("a[PostUrl]").each(function () {
        $(this).attr("PostUrl", $(this).attr("PostUrl").replace("$PictureId$", picId));
    });
    // 绑定post事件
    picture.find("a[PostUrl]").click(function () {
        if ($(this).attr("ConfirmMessage") && !confirm($(this).attr("ConfirmMessage"))) {
            return;
        }
        DoPost($(this).attr("postUrl"));
    });
    // 绑定图片移动事件
    picture.find("a.setNext").click(function () {
        picture.insertAfter(picture.next().not(".clear"));
        // renewMainPicture(picture.parent());

    });
    picture.find("a.setPre").click(function () {
        picture.insertBefore(picture.prev());
        // renewMainPicture(picture.parent());
    });

    picture.insertBefore($("#pictureContainer" + type).find("li.clear"));
    parent.fixFrame(parent.document.getElementById('frame'));
}

function bindImageDelBtn() {
    $("a.delBtn").click(function () {
        if ($(this).attr('picType') == '0') {
            count0 = count0 - 1;
            renewPictureTip();
        }
        else if ($(this).attr('picType') == '1') {
            count1 = count1 - 1;
        }
        $(this).parent().parent().fadeOut(function () {
            $(this).remove();
        });
    });


}

function bindPictureContainer() {
    var picture = $("#pictureContainer0,#pictureContainer1").children();
    // 绑定图片移动事件
    picture.find("a.setNext").click(function () {
        var picCell = $(this).parent().parent();
        picCell.insertAfter(picCell.next().not(".clear"));
        // renewMainPicture(picture.parent());

    });
    picture.find("a.setPre").click(function () {
        var picCell = $(this).parent().parent();
        picCell.insertBefore(picCell.prev());
        // renewMainPicture(picture.parent());
    });
}

// 更新标题图样式
function renewMainPicture(pictureContainer) {
    pictureContainer.find("div.mainPicture").removeClass("mainPicture");
    pictureContainer.find("div.uplpadImgBox").first().addClass("mainPicture");

}

function ChangeImgCode() {
    var randomnum = Math.random();
    var getimagecode = document.getElementById("ImgVerifyCode");
    getimagecode.src = "/Home/RandomImage?r=" + randomnum;
}

function checkUserExists(username) {
    if (username && username.length >= 6) {
        $.ajax({
            beforeSend: function (XMLHttpRequest) {
                XMLHttpRequest.setRequestHeader("ajax", "true");
            },
            type: 'POST',
            url: "/User/IsExists?username=" + username,
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.message) {
                    if (confirm(data.message)) {
                        window.location.href = '/Home/FindPassword?u=' + username;
                    }
                    else {
                        alert("该用户名已注册！请使用其他用户名！");
                    }
                }
            },
            error: ajaxError
        });
    }
}


function checkMobileExists(mobile) {
    if (mobile && mobile.length >= 6)
        DoPost("/User/IsMobileExists?mobile=" + mobile);
}

function bindComplaint() {
    $("a[ComplaintType]").click(function () {
        if (confirm("确认要投诉该房源吗？")) {
            var complaintType = $(this).attr("ComplaintType");
            var url = "/Complaint/Submit?type=" + complaintType + "&saleInfoId=" + $(this).attr("saleInfoId");
            if (complaintType == "99") {
                if ($("#ComplaintContent").val()) {
                    url = url + "&content=" + $("#ComplaintContent").val();
                }
                else {
                    alert("请填写投诉内容！");
                    $("#ComplaintContent").focus();
                    return;
                }
            }
            DoPost(url);
        }
    });
}

function initCorrectMap() {

    $("#CorrectMap").show(800);
    setTimeout(function () {
        initialize($("#Latitude").val(), $("#Longitude").val(), true, "map_canvas2");
    }, 1000);
}

function SubmitCorrectMap() {
    if (!$("#Latitude").val() || !$("#Longitude").val()) {
        alert("请标注地图！");
    }
    else {
        DoPost("/Project/Correct/?CorrectType=0&CorrectProjectId=" + $("#CorrectProjectId").val() + "&Latitude=" + $("#Latitude").val() + "&Longitude=" + $("#Longitude").val());
    }
}
function initCorrectInfo() {
    $('#CorrectInfo').show(800);
    bindForm();
}

var kCount = 0;
function validateKCount() {
    if (kCount > 500) {
        alert('房源说明不能够超过500字！');
        return false;
    }
}

function changeMenu(url) {
    if (url) {
        $(parent.document).find("[url]").removeClass("selected");
        parent.gotoFrame(url);
        $(parent.document).find("a[url='" + url.split('?')[0] + "']").addClass("selected");
    }
    return false;
    //    console.debug($(parent.document).find("a[url='" + url + "']"));
    //    $(parent.document).find("a[url='" + url + "']").click();
}

function renewPictureTip() {
    if (count0 > 0) {
        $("#picTip0").hide();
        $("#picTip1").show();
    }
    else {
        $("#picTip0").show();
        $("#picTip1").hide();
    }
}

function refreshUploadUser(elem) {
    if (elem && elem.length > 0) {
        var userDiv = $("#user" + elem.attr("userid"));
        var userName = userDiv.attr("username");
        var mobile = userDiv.attr("mobile");
        if (!userName) {
            userName = "游客";
        }
        if (!mobile) {
            mobile = "";
        }
        $("#uploadUserSpan").text(userName);
        $("#uploadUserSpan").attr("href", "/?r=v&mobile=" + mobile + "&saleInfoType=" + $("#uploadInfoSpan").attr("saleInfoType"));
    }
}

//小弹窗
function initSmallHopup() {
    if ($(".smallHopup").length > 0) {
        $(".smallHopupContainer").delay(6000).animate({
            opacity: 'show'
        }, 500);
        $(".smallHopupContainer").delay(16000).animate({
            opacity: 'hide'
        }, 500); 
    }
}
function closeSmallHopup(obj) {
    $(obj).parents(".smallHopupContainer").hide();
}

//切换城市
function initChangeCity() {
    if ($(".changeCity").length > 0) {
        $("#changeCityLink").bind("mouseenter", function () {
            $(".cityList").show();
        });
        $(".cityList").bind("mouseleave", function () {
            $(".cityList").hide();
        });
        $(".changeCity").bind("mouseleave", function () {
            $(".cityList").hide();
        });
        $(".cityList").find("a").bind("click", function () {
            changeCity(this);
        });
        $("a.city").bind("click", function () {
            changeCity(this);
        });
    }
}

function changeCity(elem) {
    if ($(elem).attr("cityId")) {
        // 设置cookie过期时间
        var date = new Date();
        var expireDays = 999;
        date.setTime(date.getTime() + expireDays * 24 * 3600 * 1000);
        document.cookie = "efecity=" + $(elem).attr("cityId") + ";path=/;expires=" + date.toGMTString();
        if (location.href.toLowerCase().indexOf('/home/myefe') > 0) {
            // 重新载入页面
            window.location.reload();
        }
        else {
            window.location.href = "/";
        }
    }
}