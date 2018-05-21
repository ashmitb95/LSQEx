


$(function () {
    $(".news-item-col").click(function (e) {
        if ($(e.target).is('.like-arrow')) {
            e.preventDefault();
            return;
        }
        debugger;
        var ItemID                  = $(this).attr('Data-ID');
        var categoryID              = $(this).attr('Data-category');
        $.ajax(
        {

            type: "GET",
            url: '../Home/RetrieveNews',
            data: {
                'ID': ItemID,
                'category': categoryID
            },
            success: function (result) {
                showNews.html(result);
                modal.modal('show');

            },
            error: function (xhr) {
                showNews.html("");
                showNews.prepend('<div class="modal-header">Error</div>');
                showNews.append(errorText);
                modal.modal('show');

            }
        });
    });
    $("#closebtn").click(function () {
        modal.modal('hide');
    });
});