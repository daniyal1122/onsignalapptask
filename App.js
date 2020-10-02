$(document).ready(function () {
   
    loadlist();

})
$("#IDdrpmember").on("change", function () {

    var ids = $(this).val();
    ids = JSON.stringify(ids);
    $("#reportingtomultipleID").val(ids);
    var s = $("#reportingtomultipleID").val();

    // contains an array of ID
});
var Save = function () {
   
    var name = $("#name").val();
    var id = $("#ID_").val();
    if (id != null)
    {
        $.ajax({
            url: "/Apps/UpdateApp",
            type: "post",
            data: { name: name },
            success: function (res) {

                $("#name").val("");

                loadlist();

            }
        })
    }
    else
    {
        $.ajax({
            url: "/Apps/CreateApp",
            type: "post",
            data: { name: name },
            success: function (res) {

                $("#name").val("");

                loadlist();

            }
        })
    }
 
}
var loadlist = function () {
    var url = $("#hidloadMember").val();

    $.ajax({
        url: "/Apps/_LoadViewApp",
        type: 'get',

        success: function (res) {
           
            $("#divloadapp").html(res);
            $("#tblapp").DataTable();
        }




    })

}

var Edit = function (id, name) {


    $("#ID_").val(id);
    $("#name").val(name);
 
}
