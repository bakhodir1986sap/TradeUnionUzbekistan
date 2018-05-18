app.controller("Rekviziti", ["$scope", "$http", "$filter", function ($scope, $http, $filter) {

    GetRekvizits();

    function GetRekvizits() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Home/GetRekvizit'
        }).then(function (response) {
            $scope.Rekvizits = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRekvizits");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.SaveRekvizit = function () {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'post',
            url: '/Home/SaveRekvizit',
            data: JSON.stringify($scope.Rekvizits),
            contentType: "application/json; charset=utf-8",
        }).then(function (response) {
            
            alertify.error("Успешное сохранение");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in SaveRekvizit");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

}]);