var app = angular.module("MyApp", []);
app.controller("MyCtrl", function ($scope, $compile, $http) {   

    $scope.message = "";
    $scope.messageType = "";
    $scope.Login_Users = function () {
        var PhoneNumber = $scope.PhoneNumber;
        var Password = $scope.Password;
        var RememberMe = $scope.RememberMe;
        //alert(PhoneNumber + ' ' + RememberMe + ' ' + Password);
        if (!PhoneNumber || !Password) {
            //alert("وارد کردن تمامی موارد ضروری است");
            $scope.message = "وارد کردن تمامی موارد ضروری است";
            $scope.messageType = "error";
        }
        else {
            $.ajax({
                type: "post",
                url: "/Login/LoginPost",
                data: {
                    PhoneNumber: $scope.PhoneNumber,
                    Password: $scope.Password,
                    RememberMe: $scope.RememberMe
                },
                datatype: "json",
                success: function (response) {
                    if (response == 1) {
                        $scope.message = "اطلاعات وارد شده صحیح نیست";
                        $scope.messageType = "error";
                    }
                    else if (response == 2) {
                        $scope.message = "شما با موفقیت وارد شدید.";
                        $scope.messageType = "success";
                        window.location.href = "/chat/index";
                    }
                }
            });
        }
    }

    $scope.checkEnter = function (event) {
        if (event.key === "Enter") {
            $scope.Login_Users();
        }
    };
});