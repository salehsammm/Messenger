
var app = angular.module("MyApp", []);
app.controller("MyCtrl", function ($scope, $compile, $http) {

    $scope.friends = [];
    $http.get('/Chat/GetFriends')
        .then(function (response) {
            if (response.data.message) {
                window.location.href = "/chat/NoFriend";
            } else {
                $scope.friends = response.data;
                $scope.selectFriend($scope.friends[0].FriendId);
            }       
        });

    $scope.selectFriend = function (friendId) {
        $scope.selectedId = friendId;
        var selectedId = friendId;
        //alert(friendId);
        $.ajax({
            type: "post",
            url: "/Chat/GetMessagesById",
            data: {
                selectedId: friendId
            },
            datatype: "json",
            success: function (data) {
                $scope.friendName = data.friendName;
                $scope.Messages = data.messages;
                $scope.friendMobile = data.friendMobile;
                $scope.$apply();
                $scope.$apply(function () {
                    $scope.message = '';
                    var messageContainer = document.getElementById('messageContainer');
                    messageContainer.scrollTop = messageContainer.scrollHeight;
                });
            }
        });
    }

    $scope.sendM = function () {
        var message = $scope.message;
        var selectedId = $scope.selectedId;
        //alert(selectedId);
        if (message) {
            $.ajax({
                type: "post",
                url: "/Chat/SendMessage",
                data: {
                    message: $scope.message,
                    selectedId: selectedId
                },
                datatype: "json",
                success: function (data) {
                    $scope.selectFriend($scope.selectedId);
                }
            });
        }
    }

    $scope.checkEnter = function (event) {
        if (event.key === "Enter") {
            $scope.sendM();
        }
    };    

    $scope.editingName = false;
    $scope.editFriend = function () {        
        if ($scope.editingName) {
            //alert($scope.friendName);
            var selectedId = $scope.selectedId;
            var friendName = $scope.friendName;
            $.ajax({
                type: "post",
                url: "/friend/postEditFriend",
                data: {
                    selectedId: $scope.selectedId,
                    friendName: $scope.friendName
                },
                success: function (data) {
                    if (data == 2) {
                        $scope.editingName = false;
                        $scope.$apply();
                        alert('نام مخاطب شما با موفقیت تغییر کرد.')

                        $scope.friends = [];
                        $http.get('/Chat/GetFriends')
                            .then(function (response) {
                                if (response.data.message) {
                                    window.location.href = "/chat/NoFriend";
                                } else {
                                    $scope.friends = response.data;
                                    $scope.selectFriend($scope.selectedId);
                                }
                            });
                    }
                }
            });
        } else {
            $scope.editingName = true;
        }
    };

    $scope.deleteFriend = function (friendId) {
        $.ajax({
            type: "post",
            url: "/Chat/DeleteFriend",
            data: {
                selectedId: friendId
            },
            datatype: "json",
            success: function (data) {
                if (data == 2) {
                    $scope.showDeleteModal = false;
                    alert("مخاطب شما با موفقیت حذف شد.");
                    $scope.friends = [];
                    $http.get('/Chat/GetFriends')
                        .then(function (response) {
                            if (response.data.message) {
                                window.location.href = "/chat/NoFriend";
                            } else {
                                $scope.friends = response.data;
                                $scope.selectFriend($scope.friends[0].FriendId);
                            }
                        });
                    $scope.selectFriend($scope.friends[0].FriendId);
                }
            }
        });
    }

    $scope.showDeleteModal = false;
    $scope.ShowModal = function () {
        $scope.showDeleteModal = true;
    };

    $scope.cancelDelete = function () {
        $scope.showDeleteModal = false;
        $scope.showPhoneModal = false;
    };

    $(window).click(function (event) {
        if ($(event.target).is("#myModal")) {
            $scope.$apply(function () {
                $scope.showDeleteModal = false;
                $scope.showPhoneModal = false;
            });
        }
    });

    $scope.showPhoneModal = false;
    $scope.ShowPhoneNumber = function () {
        $scope.showPhoneModal = true;
    };

});