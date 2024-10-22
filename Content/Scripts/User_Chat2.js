var app = angular.module("MyApp", []);
app.controller("MyCtrl", function ($scope, $compile, $http) {
    $scope.selectedId = null;
    $scope.selectFriend = function (friendId) {
        $scope.selectedId = friendId;
        var selectedId = friendId;
        //alert(friendId);
        $.ajax({
            type: "post",
            url: "/Chat2/GetMessagesById",
            data: {
                selectedId: friendId
            },
            datatype: "json",
            success: function (data) {
                //console.log(data);
                $('#friendName').text(data.friendName);

                $('#messageContainer').empty();
                data.messages.forEach(function (message) {
                    var messageHtml = '';

                    if (message.ReceiverId === friendId) {
                        messageHtml = '<div class="message m-send">' + message.MessageText + '</div>';
                    } else if (message.SenderId === friendId) {
                        messageHtml = '<div class="message m-receive">' + message.MessageText + '</div>';
                    }
                    $('#messageContainer').append(messageHtml);
                });
            }
        });
    }

    $scope.sendM = function () {
        var message = $scope.message;
        var selectedId = $scope.selectedId;
        //alert(selectedId);
        $.ajax({
            type: "post",
            url: "/Chat2/SendMessage",
            data: {
                message: $scope.message,
                selectedId: selectedId
            },
            datatype: "json",
            success: function (data) {
                var messageHtml = '<div class="message m-send">' + data.MessageText + '</div>';
                $('#messageContainer').append(messageHtml);

                $scope.$apply(function () {
                    $scope.message = '';
                });
            }
        });
    }
});