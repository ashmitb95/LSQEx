$(function () {
        $(".pageClick").click(function () {
            newsBody.fadeOut(0.001);
            var ID = $(this).attr('data-category');
            var page = $(this).attr('data-page');
            var categoryname = $(this).attr('data-categoryName');
            //alert("this is the category:"+category);
            $.ajax(
                {

                    type: "GET",
                    url: '../Home/GetNews',
                    data: {
                        'category': ID,
                        'categoryName': categoryname,
                        'page': page
                    },
                    success: function (result) {

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
});