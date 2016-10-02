function getNameFromPath(strFilepath) {
    var objRE = new RegExp(/([^\/\\]+)$/);
    var strName = objRE.exec(strFilepath);

    if (strName == null) {
        return null;
    }
    else {
        return strName[0];
    }
}

$(function () {
    $("#file").change(function () {
        var file = getNameFromPath($(this).val());
        if (file != null) {
            var extension = file.substr((file.lastIndexOf('.') + 1));
            switch (extension) {
                case 'jpg':
                case 'png':
                case 'jpeg':
                    flag = true;
                    break;
                default:
                    flag = false;
            }
        }
        if (flag == false) {
            $("#validationTxt").text("Please upload Your Image of type: jpg, png, jpeg");
            return false;
        }
        else {
            $("#validationTxt").text("");
        }
    });
});

$(":file").filestyle({ buttonBefore: true });
