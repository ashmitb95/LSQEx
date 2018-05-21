$(function () {
    $(".like-arrow").click(function () {
        var NewsID = $(this).attr("data-NewsID");
        var CategoryID = $(this).attr("data-CategoryID");
        var toggle = $(this).attr("data-flag");
        var isUpvote = $(this).attr("data-upvote");
        $.ajax({
            type: "GET",
            url: '../Home/VoteNews',
            data: {
                'newsID': NewsID,
                'categoryID': CategoryID,
                'toggle': isUpvote
            },
            success: function (result) {
                var UpdateID = "#LikeCount".concat(NewsID);
                var UpdateButton = "#LikeButton".concat(NewsID);
                var Upvotebtn = "#Upvotebtn".concat(NewsID);
                var Downvotebtn = "#Downvotebtn".concat(NewsID);
                if (isUpvote == "true") {
                    if ($(Upvotebtn).attr('data-flag') == "false") {
                        $(Upvotebtn).attr('data-flag', 'true');
                        $(Upvotebtn).addClass('blue');
                    }
                    else {
                        $(Upvotebtn).attr('data-flag', 'false');
                        $(Upvotebtn).removeClass('blue');
                    }
                    if ($(Downvotebtn).attr('data-flag') == "true") {
                        $(Downvotebtn).attr('data-flag', 'false');
                        $(Downvotebtn).removeClass('blue');
                    }
                }
                else {
                    if (toggle == "false") {
                        $(Downvotebtn).attr('data-flag', 'true');
                        $(Downvotebtn).addClass('blue');
                    }
                    else {
                        $(Downvotebtn).attr('data-flag', 'false');
                        $(Downvotebtn).removeClass('blue');
                    }
                    if ($(Upvotebtn).attr('data-flag') == "true") {
                        $(Upvotebtn).attr('data-flag', 'false');
                        $(Upvotebtn).removeClass('blue');

                    }
                    

                }
                
                $(UpdateID).html(result);     
             },
            error: function (xhr) {
                showNews.html("");
                showNews.prepend('<div class="modal-header">Error</div>');
                showNews.append("<p>Something went wrong.<br/> Please reload the page<p>");
                modal.modal('show');
            }
        })

    })
});