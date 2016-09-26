var url = "";
$(document).ready(function () {
    $("#alert-dialog").dialog({
        autoOpen: false,
        modal: true,
        draggable: false,
        resizable: false,
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close").hide();
        },
        buttons: {
            "OK": function () { $(this).dialog("close"); },
            "Cancel": function () { $(this).dialog("close"); }
        }
    });
    $("#details-dialog").dialog({
        autoOpen: false,
        modal: true,
        draggable: false,
        resizable: false,
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close").hide();
            $(this).load(url);
        },
        buttons: {
            "Close": function () { $(this).dialog("close"); }
        }
    });
    $("#create-dialog").dialog({
        title: "Add Book",
        autoOpen: false,
        modal: true,
        resizable: false,
        width: 400,
        draggable: true,
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close").hide();
            $(this).load(url);
        }
    });
    $("#delete-dialog").dialog({
        title: "Confirm Delete",
        autoOpen: false,
        resizable: false,
        modal: true,
        draggable: true,
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close").hide();
        },
        buttons: {
            "OK": function () {
                $(this).dialog("close");
                //to go to Delete action..
                window.location.href = url;
            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    });
    if ('@TempData["msg"]' != "") {
        $("#alert-dialog").dialog("open");
    }
    $("#lnkCreate").on("click", function (e) {
        //e.preventDefault();
        url = $(this).attr("href");
        $("#create-dialog").dialog("open");

        return false;
    });
    $(".lnkDetail").on("click", function (e) {
        url = $(this).attr("href");
        $("#details-dialog").dialog("open");
        return false;
    });
    $(".lnkEdit").on("click", function (e) {
        $(".ui-dialog-title").html("Update Book");
        url = $(this).attr("href");
        $("#create-dialog").dialog("open");
        return false;
    });
    $(".lnkDelete").on("click", function (e) {
        url = $(this).attr("href");
        $("#delete-dialog").dialog("open");
        return false;
    });
    $("#btncancel").on("click", function (e) {
        $("#create-dialog").dialog("close");
        return false;
    });
});