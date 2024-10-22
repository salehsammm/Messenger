var app = angular.module("MyApp", []);
app.controller("MyCtrl", function ($scope, $compile, $http) {

    $("#Reg2-PhoneNumber").keyup(function () {
        $("#Reg2-Username").val($("#Reg2-PhoneNumber").val());
    });

    $scope.message = "";
    $scope.messageType = "";
    $scope.Register_Users = function () {
        var Fname = $scope.Fname;
        var Lname = $scope.Lname;
        var PhoneNumber = $scope.PhoneNumber;
        var Gender = $scope.Gender;
        var Username = $scope.Username;
        var Password = $scope.Password;
        var RePassword = $scope.RePassword;
        //alert(Fname + ' ' + Lname + ' ' + PhoneNumber + ' ' + Gender + ' ' + PhoneNumber + ' ' + Password);

        if (!Fname || !Lname || !PhoneNumber || !Gender || !Password || !RePassword) {
            $scope.message = "وارد کردن تمامی موارد ضروری است";
            $scope.messageType = "error";
        }
        else if (Password != RePassword) {
            $scope.message = "رمز عبور و تکرار آن مطابقت ندارند";
            $scope.messageType = "error";
        }
        else if (PhoneNumber && PhoneNumber.length < 8) {
            $scope.message = "شماره موبایل باید حداقل 8 کاراکتر باشد";
            $scope.messageType = "error";
            return;
        }
        else if (Password.length < 6) {
            $scope.message = "رمز عبور باید حداقل 6 کاراکتر باشد";
            $scope.messageType = "error";
        }
        else {
            $.ajax({
                type: "post",
                url: "/Register/RegisterPost",
                data: {
                    Fname : $scope.Fname,
                    Lname : $scope.Lname,
                    PhoneNumber : $scope.PhoneNumber,
                    Gender : $scope.Gender,
                    Username : $scope.PhoneNumber,
                    Password : $scope.Password
                },
                datatype: "json",
                success: function (response) {
                    if (response == 1) {
                        $scope.message = "شماره موبایل مورد نظر شما تکراری است";
                        $scope.messageType = "error";
                    }
                    else if (response == 2) {
                        $scope.message = "طلاعات مورد نظر شما با موفقیت ثبت شد";
                        $scope.messageType = "success";
                        window.location.href = "/chat/index";
                    }
                }
            });
        }
    }

    $scope.checkEnter = function (event) {
        if (event.key === "Enter") {
            $scope.Register_Users();
        }
    };
});