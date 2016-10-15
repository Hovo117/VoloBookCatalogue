window.addEventListener("popstate", function (e) {
    $.ajax({
        url: location.href,
        success: function (result) {
            $('#GridBooks').html(result);
        }
    });
});

function ChangeUrl(page, url) {
    if (typeof (history.pushState) != "undefined") {
        var obj = { Page: page, Url: url };
        history.pushState(null, obj.Page, obj.Url);
    } else {
        alert("Browser does not support HTML5.");
    }
}

function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

function search() {
    $.ajax({
        url: "/Books/Index?searchString=" + $('#SearchString').val(),
        success: function (result) {
            ChangeUrl("index", "/Books/Index?searchString=" + $('#SearchString').val());
            $('#GridBooks').html(result);
        }
    });
}

$(function () {
    $("#btnSearch").click(function () {
        search();
    });

    $("#SearchString").keypress(function (e) {
        if (e.keyCode == 13) {
            search();
        }
    });

    $('body').on('click', '#GridBooks .pagination a', function (event) {
        event.preventDefault();
        console.log('page');
        var searchString = $('#SearchString').val();
        if (searchString == undefined || searchString == '') {
            searchString = '';
        } else {
            searchString = '&searchString=' + searchString;
        }

        var currentSortOption;
        currentSortOption = getUrlVars()['sortOption'];

        var sort;
        switch (currentSortOption) {
            case "price_acs": sort = "&sortOption=price_acs"; break;
            case "price_desc": sort = "&sortOption=price_desc"; break;
            case "title_acs": sort = "&sortOption=title_acs"; break;
            case "title_desc": sort = "&sortOption=title_desc"; break;
            case "author_acs": sort = "&sortOption=author_acs"; break;
            case "author_desc": sort = "&sortOption=author_desc"; break;
            case "country_acs": sort = "&sortOption=country_acs"; break;
            case "country_desc": sort = "&sortOption=country_desc"; break;
            case "pages_acs": sort = "&sortOption=pages_acs"; break;
            case "pages_desc": sort = "&sortOption=pages_desc"; break;

            default: sort = ""; break;
        }

        var url = $(this).attr('href') + searchString + sort;

        console.log(url);
        $.ajax({
            url: url,
            success: function (result) {
                ChangeUrl('index', url);
                $('#GridBooks').html(result);
            }
        });


    });


    $('body').on('click', '#GridBooks table .aaa', function (event) {

        event.preventDefault();

        var searchString = $('#SearchString').val();
        if (searchString == undefined || searchString == '') {
            searchString = '';
        } else {
            searchString = '&searchString=' + searchString;
        }

        var columnToSort = $(this).text();
        var currentSortOption = getUrlVars()['sortOption'];
        console.log(currentSortOption);
        var sort;
        switch (currentSortOption) {
            case "title_acs":
                sort = 'sortOption=title_desc';
                break;
            case "title_desc":
                sort = 'sortOption=title_acs';
                break;
            case "author_acs":
                sort = 'sortOption=author_desc';
                break;
            case "author_desc":
                sort = 'sortOption=author_acs';
                break;
            case "price_acs":
                sort = 'sortOption=price_desc';
                break;
            case "price_desc":
                sort = 'sortOption=price_acs';
                break;
            case "country_acs":
                sort = 'sortOption=country_desc';
                break;
            case "country_desc":
                sort = 'sortOption=country_acs';
                break;
            case "pages_acs":
                sort = 'sortOption=pages_desc';
                break;
            case "pages_desc":
                sort = 'sortOption=pages_acs';
                break;
            default:
                sort = '';
                break;
        }


        switch (columnToSort) {
            case 'Title':
                if (currentSortOption != 'title_acs' && currentSortOption != 'title_desc') {
                    sort = 'sortOption=title_acs';
                }
                break;
            case 'Author':
                if (currentSortOption != 'author_acs' && currentSortOption != 'author_desc') {
                    sort = 'sortOption=author_acs';
                }
                break;
            case 'Price':
                if (currentSortOption != 'price_acs' && currentSortOption != 'price_desc') {
                    sort = 'sortOption=price_acs';
                }
                break;
            case 'Country':
                if (currentSortOption != 'country_acs' && currentSortOption != 'country_desc') {
                    sort = 'sortOption=country_acs';
                }
                break;
            case 'Pages':
                if (currentSortOption != 'pages_acs' && currentSortOption != 'pages_desc') {
                    sort = 'sortOption=pages_acs';
                }
                break;
            default:
                sort = '';
                break;

        }
        if (sort != '' & searchString != '') {
            sort = '&' + sort;
        }
        var url = '/Books/Index?' + searchString + sort;
        $.ajax({
            url: url,
            success: function (result) {
                ChangeUrl('index', url);
                $('#GridBooks').html(result);
            }
        });
    });

});