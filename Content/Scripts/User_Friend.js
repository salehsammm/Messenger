var app = angular.module("MyApp", []);
app.controller("MyCtrl", function ($scope, $compile, $http) {

    $scope.message = "";
    $scope.messageType = "";
    $scope.AddFriend = function () {
        var PhoneNumber = $scope.PhoneNumber;
        var FullName = $scope.FullName;
        //alert(PhoneNumber + ' ' + FullName);
        if (!PhoneNumber || !FullName) {
            //alert("وارد کردن تمامی موارد ضروری است");
            $scope.message = "وارد کردن تمامی موارد ضروری است";
            $scope.messageType = "error";
        }
        else {
            $.ajax({
                type: "post",
                url: "/Friend/AddFriend",
                data: {
                    PhoneNumber: $scope.PhoneNumber,
                    FullName: $scope.FullName,
                },
                datatype: "json",
                success: function (response) {
                    console.log(response);

                    if (response == 1) {
                        $scope.message = "این شماره در برنامه ثبت نشده";
                        $scope.messageType = "error";
                    }
                    else if (response == 2) {
                        $scope.message = " قبلا این شماره در لیست مخاطبین شما ثبت شده است";
                        $scope.messageType = "error";
                    }
                    else if (response == 3) {
                        $scope.message = " این شماره خودتان است !!!!!!";
                        $scope.messageType = "error";
                    }
                    else if (response == 4) {
                        $scope.message = "مخاطب شما با موفقیت ثبت شد";
                        $scope.messageType = "success";
                        window.location.href = "/chat/index";
                    }
                }
            });
        }       
    }

    $scope.message = "";
    $scope.messageType = "";
    $scope.editFriend = function (FriendId) {
        //alert($scope.PhoneNumber);
        var PhoneNumber = $scope.PhoneNumber;
        var FullName = $scope.FullName;
        if (!PhoneNumber || !FullName) {
            //alert("وارد کردن تمامی موارد ضروری است");
            $scope.message = "وارد کردن تمامی موارد ضروری است";
            $scope.messageType = "error";
        }
        else {
            $.ajax({
                type: "post",
                url: "/Friend/postEditFriend",
                data: {
                    PhoneNumber: $scope.PhoneNumber,
                    FullName: $scope.FullName,
                },
                datatype: "json",
                success: function (response) {
                    if (response == 1) {
                        $scope.message = "این شماره در برنامه ثبت نشده";
                        $scope.messageType = "error";
                    }
                    else if (response == 2) {
                        $scope.message = "تغییرات با موفقیت ثبت شد";
                        $scope.messageType = "success";
                        window.location.href = "/chat/index";
                    }
                }
            });
        }
    }

    $scope.checkEnter = function (event) {
        if (event.key === "Enter") {
            $scope.AddFriend();
        }
    };

});