/* Write here your custom javascript codes */
jQuery(document).ready(function () {

    $(".btn-ok").click(function () {
        DoPost($(this).attr("postUrl"));
        return false;
    });

    // binding confirm pop up
    $('#confirm-delete').on('show.bs.modal', function (e) {
        $(this).find('.btn-ok').attr('posturl', $(e.relatedTarget).data('href'));
    });

    $("#CloseMessageButton").parent().delay(5000).hide(500);
    $('#confirm').on('show.bs.modal', function (e) {
        $(this).find('.btn-ok').attr('posturl', $(e.relatedTarget).data('href'));
        $(this).find('#ConfirmTitle').text($(e.relatedTarget).data('confirm-title'));
        $(this).find('#ConfirmMessage').text($(e.relatedTarget).data('confirm-message'));
        $(this).find('#ResultMessage').text("");
    });
});

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
        $("#ResultMessage").text(data.message);
        $("#ResultMessage").removeClass("text-success");
        $("#ResultMessage").removeClass("text-info");
        $("#ResultMessage").removeClass("text-danger");
        if (data.success) {
            $("#ResultMessage").addClass("text-success");
        }
        else {
            $("#ResultMessage").addClass("text-danger");
        }
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
    // alert('²Ù×÷Ê§°Ü£¡' + XMLHttpRequest.responseText);
    // console.debug(XMLHttpRequest.responseText);
    //alert("Failed due to unknown reason, please try later.");
}