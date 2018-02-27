app.controller("SController", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    GetSC();
    function GetSC() {
        $http({
            method: "post",
            url: "GetSC",
        }).then(function (d) {
            $scope.UZ = d.data;
        }, function () {
            alertify.error("ошибка");
        });
    }
}]);