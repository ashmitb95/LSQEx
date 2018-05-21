

function retrieveHighlights() {
   
    
    highlight.click(function () {
        var ID              = $(this).attr('data-Highlight-ID');
        var Category        = $(this).attr('data-Highlight-category');
        $.ajax(
        {
            type: "GET",
            url: "../Highlights/RetrieveHighlights",
            data: {
                'ID': ID,
                'category': Category

            },
            success: function (result) {
                    showNews.html(result);
                    modal.modal('show');
            },
            error: function (xhr) {
                showNews.html("");
                showNews.prepend('<div class="modal-header">Error</div>');
                showNews.append("<p>Something went wrong.<br/> Please reload the page<p>");
                modal.modal('show');
                
            }
        });
    });

    button.click(function () {
        modal.modal('hide');
        });
}

function highlightsTicker() {
    $('#highlights-ticker li:first').slideUp(function () {
        $(this).appendTo(highlightsTickerList).slideDown();
    });
}
setInterval(function ()
{ highlightsTicker(); }, 5000);

$("#highlights-ticker").hover(function () {
    $('#highlights-ticker li:first').stop(true, true).slideUp();
}, function () {
    $(this).stop(true, true).slideUp();
});