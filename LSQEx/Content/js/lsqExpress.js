var clickCategory;
var newsBody;
var categoriesList;
var activeElement;
var findActiveElement;
var windowWidth;
var newsCard;
var responsiveNavbar;
var ErrorText;
//var navPlaceholder;
var highlight;
var showNews;
var modal
var highlightsTickerItem;
var highlightsTickerList;
var button;
var news;

$(function () {
    initialize();
    handleEvents();
});

function initialize() {
    clickCategory           = $(".categoryItem, .device-category, .carouselItem, .more, .pageClick");
    newsBody                = $("#target-body");
    categoriesList          = $('#categories');
    activeElement           = $(".categoryItem, #categories li");
    findActiveElement       = $(".more, .carouselItem");
    window                  = $(window).width();
    newsCard                = $(".news-item-col");
    responsiveNavbar        = $("#mobile-navbar");
    errorText               = "<p> There was a problem while retrieving the news Data. Please try again.</p> ";
    //navPlaceholder = $(".nav-placeholder");
    highlight               = $(".highlightItem");
    showNews                = $("#highlightsModal");
    modal                   = $("#modal");
    highlightsTickerItem    = $('#highlights-ticker li:first');
    highlightsTickerList    = $('#highlights-ticker');
    button                  = $("#closebtn");
    news                    = $(".news-item-col");
}

function handleEvents() {
    //Categories.js
    retrieveNews();
    if ($(window).width() >= 500) {
        dockCategories();
    }
    
    toggleActiveElement();
    dockMobileNavbar();
    wrapNewsItems();
    searchNews();


    //Highlights.js
    highlightsTicker();
    retrieveHighlights();
}