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
            $("#validationTxt").text("You can upload only jpg,png,jpeg extension file");
            return false;
        }
        else {
            var size = GetFileSize('file');
            if (size > 3) {
                $("#validationTxt").text("You can upload file up to 1 MB");
            }
            else {
                $("#validationTxt").text("");
            }
        }
    });
});
