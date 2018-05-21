$(function () {
    $('#searchVal').keydown(function (event) {
        var key = event.which;
        var stringlength = $('#searchVal').val().length;

        if (key == 13 && stringlength > 3)  // the enter key code
        {
            searchNews();
            return false;
        }

    });
    function searchNews() {
        var text = $("#searchVal").val();
        var category = $('#searchVal').attr('data-category');
        if (text == "") {
            showNews.html("");
            showNews.prepend('<div class="modal-header">Alert</div>');
            showNews.append("<p>Search Query cannot be empty.<br/>");
            modal.modal('show');
        }

        else {
            activeElement.removeClass("active");
            newsBody.fadeOut(0.001);
            $.ajax(
                {
                    type: "GET",
                    url: '../Home/SearchNews',
                    data: {
                        'search': text,
                        'category': category,
                        'page': 1
                    },
                    success: function (result) {

                        //alert(result);
                        //$("#result-container").modal(show);
                        newsBody.fadeIn(1000);
                        newsBody.prepend('<p><span></span></p>')
                        newsBody.html(result);//.fadeIn("slow");


                    },
                    error: function (xhr) {
                        showNews.html("");
                        showNews.prepend('<div class="modal-header">Error</div>');
                        showNews.append("<p>Something went wrong.<br/> Please reload the page<p>");
                        modal.modal('show');
                    }

                });

        }

    }
});