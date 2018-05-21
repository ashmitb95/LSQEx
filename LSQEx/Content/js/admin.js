$(function () {
    $("#showusers").click(function () {
        $.ajax(
       {
         
           type: "GET",
           url: '../Admin/ShowUsers',
           success: function (result) {
               $("#UserResults").html(result);
           },
           error: function (xhr) {
               alert(xhr.status);
           }
       });
    });
});