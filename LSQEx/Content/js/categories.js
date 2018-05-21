function dockMobileNavbar() {
    if ($(window).width() <= 500) {
        //$(".news-item-col").wrap('<div class="mobile-padding"></div>');
        var Offset = responsiveNavbar.offset().top;
        //alert("Script is running" + Offset);
        responsiveNavbar.wrap('<div class="placeholder"></div>');
        $(".placeholder").height(responsiveNavbar.outerHeight());
        jQuery(window).scroll(function () {


            var scrollPos = jQuery(window).scrollTop();
            if (scrollPos >= Offset) {
                responsiveNavbar.addClass("fixed-nav");
            }
            else {
                responsiveNavbar.removeClass("fixed-nav");
            }

        });
    }
}
function dockCategories() {
    var navOffset                   = categoriesList.offset().top;
    categoriesList.wrap('<div class="nav-placeholder"></div>');
    $(".nav-placeholder").height(categoriesList.outerHeight());
    //navPlaceholder.height(categoriesList.outerHeight());
    $(window).scroll(function () {
        var scrollPos               = jQuery(window).scrollTop();
        if (scrollPos >= navOffset) {
            categoriesList.addClass("fixed");
        }
        else {
            categoriesList.removeClass("fixed");
        }
    });
}


function retrieveNews() {
    clickCategory.click(function () {
        newsBody.fadeOut(0.001);
        var ID  = $(this).attr('data-ID');
        var page = $(this).attr('data-page');
        var categoryname = $(this).attr('data-category');
        //alert("this is the category:"+category);
        $.ajax(
        {

            type: "GET",
            url: '../Home/GetNews',
            data: {
                'category': ID,
                'categoryName': categoryname,
                'page':page
            },
                success: function (result) {
                    $('#searchVal').attr('data-category', categoryname);
                //alert(result);
                //$("#result-container").modal(show);
                newsBody.fadeIn(1000);
                newsBody.html(result);//.fadeIn("slow");


            },
            error: function (xhr) {
                showNews.html("");
                showNews.prepend('<div class="modal-header">Error</div>');
                showNews.append("<p>Something went wrong.<br/> Please reload the page<p>");
                modal.modal('show');
            }
        });
    });
}

function searchNews() {
    //$("#searchButton").click(function () {
    //    alert("Clicked");
    //    var text = $("#searchVal").val();
    //    alert(text);
    //    newsBody.fadeOut(0.001);
    //    //alert("handle eVENTS");
    //    var ID = $(this).attr('data-ID');
    //    var category = $(this).attr('data-category');
    //    //alert("this is the category:"+category);
    //    $.ajax(
    //        {

    //            type: "GET",
    //            url: '../Home/GetNews',
    //            data: {
    //                'ID': ID,
    //                'category': category
    //            },
    //            success: function (result) {

    //                //alert(result);
    //                //$("#result-container").modal(show);
    //                newsBody.fadeIn(1000);
    //                newsBody.html(result);//.fadeIn("slow");


    //            },
    //            error: function (xhr) {
    //                showNews.html("");
    //                showNews.prepend('<div class="modal-header">Error</div>');
    //                showNews.append("<p>Something went wrong.<br/> Please reload the page<p>");
    //                modal.modal('show');
    //            }
    //        });
    }

function toggleActiveElement() {
    activeElement.click(function () {
        activeElement.removeClass("active");
        $(this).addClass("active");
    });

    findActiveElement.click(function () {
        var category = $(this).attr('data-category');
        //alert(category);
        activeElement.removeClass("active");
        $("#categories li:contains('" + category + "')").addClass("active");
    });
}



function wrapNewsItems() {
    if ($(window).width() <= 500) {
        $(".news-item-col").wrap('<div class="mobile-padding"></div>');
    }
}

//$(document).on("click", ".categoryItem, .device-category, .carouselItem, .more", function () {
//    //alert("the script is running");
//    //flag
//    var text = $(this).attr('data-category');
//    //alert("this is the category:"+text);
//    $.ajax(
//    {

//        type: "GET", //HTTP Method
//        url: '/Home/GetNews', // Controller/View
//        data: {
//            'category': text
//        },
//        success: function (result) {
//            //alert(result);
//            // $("#result-container").modal(show);

//            $("#target-body").html(result);

//        },
//        error: function () {
//            //alert('Exception Encountered');
//        }
//    });
//});

//$(function () {
//    initialise();
//    HandleEvents();
//});


//function initialise() {


//}

//function HandleEvents() {

//}
