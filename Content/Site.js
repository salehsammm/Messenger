$(document).ready(function () {

    $("#reg-PhoneNumber").keyup(function () {
        var phoneNumberValue = $(this).val();
        $("#reg-UserName").val(phoneNumberValue);
    });
});