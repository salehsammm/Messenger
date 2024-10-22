
var app = angular.module("MyApp", []);
app.controller("MyCtrl", function ($scope, $compile, $http) {

    $scope.friends = [];
    $scope.groups = [];
    $http.get('/Group/GetGpData')
        .then(function (response) {
            $scope.friends = response.data.friends;
            $scope.groups = response.data.groups;
            $scope.selectGp($scope.groups[0].GroupId);
        });

    $scope.getFilteredFriends = function () {
        var filteredFriends = [];
        $scope.friends.forEach(function (friend) {
            if (!$scope.groupMembers.includes(friend.FullName)) {
                filteredFriends.push(friend);
            }
        });
        return filteredFriends;
    };

    $scope.selectGp = function (groupId) {
        $scope.editingName = false;
        $scope.groupId = groupId;
        //alert(groupId);
        $.ajax({
            type: "post",
            url: "/group/GetUsersByGpId",
            data: {
                groupId: groupId
            },
            datatype: "json",
            success: function (data) {
                $scope.groupMembers = [];
                $scope.filteredFriends = [];
                $scope.groupMembers = data.nameOfUsers;
                $scope.filteredFriends = $scope.getFilteredFriends();
                $scope.IsAdmin = data.IsAdmin;
                $scope.userId = data.userId;
                $scope.groupName = data.groupName;
                $scope.Messages = data.messages;
                $scope.UserFullName = data.UserName;

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
        var groupId = $scope.groupId;
        //alert(groupId);
        if (message) {
            $.ajax({
                type: "post",
                url: "/group/SendMessage",
                data: {
                    message: $scope.message,
                    groupId: $scope.groupId
                },
                datatype: "json",
                success: function (data) {
                    $scope.selectGp($scope.groupId);
                }
            });
        }
    }

    $scope.checkEnter = function (event) {
        if (event.key === "Enter") {
            $scope.sendM();
        }
    };

    $scope.selectedFriends = [];
    $scope.selectFriend = function (friendId) {
        const index = $scope.selectedFriends.indexOf(friendId);
        if (index === -1) {
            $scope.selectedFriends.push(friendId);
            //alert('add');
        } else {
            $scope.selectedFriends.splice(index, 1);
            //alert('remove');
        }
    };

    $scope.message = "";
    $scope.messageType = "";
    $scope.GpMake = function () {
        var GpName = $scope.GpName;
        //alert(GpName);
        if (!GpName) {
            $scope.message = "لطفا نام گروه را وارد کنید";
            $scope.messageType = "error";
        }
        else {
            $.ajax({
                type: "post",
                url: "/Group/GpMakePost",
                data: {
                    GpName: $scope.GpName,
                    selectedFriends: $scope.selectedFriends
                },
                datatype: "json",
                success: function (data) {
                    if (data == 1) {
                        $scope.message = "نام گروه مورد نظر شما تکراری است";
                        $scope.messageType = "error";
                    }
                    else if (data == 2) {
                        $scope.message = "گروه شما با موفقیت ساخته شد";
                        $scope.messageType = "success";
                        window.location.href = "/group/index";
                    }
                }
            });
        }

    }

    $scope.editingName = false;
    $scope.EditGpName = function () {
        if ($scope.editingName) {
            //var groupName = $scope.groupName;
            //var groupId = $scope.groupId;
            $.ajax({
                type: "post",
                url: "/group/UpdateGroupName",
                data: {
                    groupId: $scope.groupId,
                    groupName: $scope.groupName
                },
                success: function (data) {
                    if (data == 2) {
                        $scope.editingName = false;
                        $scope.$apply();
                        alert('نام گروه شما با موفقیت تغییر کرد.')

                        $scope.friends = [];
                        $scope.groups = [];
                        $http.get('/Group/GetGpData')
                            .then(function (response) {
                                $scope.friends = response.data.friends;
                                $scope.groups = response.data.groups;
                                if ($scope.groups.length > 0) {
                                    $scope.selectGp($scope.groupId);
                                }
                            });
                    }
                }
            });
        } else {
            $scope.editingName = true;
        }
    };

    $scope.AddUserToGp = function () {
        $.ajax({
            type: "post",
            url: "/Group/AddUser",
            data: {
                groupId: $scope.groupId,
                selectedFriends: $scope.selectedFriends
            },
            datatype: "json",
            success: function (data) {
                if (data == 1) {
                    alert("Error");
                }
                else if (data == 2) {
                    alert('اعضای جدید با موفقیت به گروه اضافه شدند ');
                    $("#myModal").hide();
                    $scope.selectGp($scope.groupId);
                }
            }
        });
    }

    $scope.ShowUsers = function () {
        $("#myModal").show();
    };

    $(window).click(function (event) {
        if ($(event.target).is("#myModal")) {
            $("#myModal").hide();
        }
    });

    $scope.showDeleteModal = false;
    $scope.ShowModal = function (memberId) {
        $scope.showDeleteModal = true;
        $scope.memberId = memberId;
    };

    $scope.cancelDelete = function () {
        $scope.showDeleteModal = false;
    };

    $scope.deleteFriend = function () {
        //alert($scope.memberId);
        $.ajax({
            type: "post",
            url: "/group/DeleteMember",
            data: {
                memberId: $scope.memberId,
                groupId: $scope.groupId
            },
            datatype: "json",
            success: function (data) {
                $scope.showDeleteModal = false;
                alert("فرد مورد نظر با موفقیت از گروه حذف شد.");
                $scope.selectGp($scope.groupId);
            }
        });
    }

    $(window).click(function (event) {
        if ($(event.target).is("#myModal2")) {
            $scope.$apply(function () {
                $scope.showDeleteModal = false;
            });
        }
    });

});