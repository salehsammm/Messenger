var app = angular.module("MyApp", []);
app.controller("MyCtrl", function ($scope, $compile, $http) {
    $scope.send = function () {
        var message = $scope.message;
        var selectedId = $scope.selectedId;
        alert(selectedId);
        //$.ajax({
        //    type: "post",
        //    url: "/Chat/SendMessage",
        //    data: {
        //        message = $scope.message,
        //        selectedId = $scope.selectedId
        //    },
        //    datatype: "json",
        //    success: function (response) {
        //        if (response == 1) {
        //            window.location.href = "/chat/index";
        //        }
        //        else {
        //            alert("Error");
        //        }
        //    }
        //});
    }
});