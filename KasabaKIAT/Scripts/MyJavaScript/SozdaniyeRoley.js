app.controller("SozdaniyeRoley", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    $scope.showdata = false;
    GetData();
    V();
    $scope.R = {
        d1: 1726,
        d2: ''
    };
    function GetData() {
        $http({
            method: "post",
            url: "/Administirirovaniye/GetRoley",
        }).then(function (d) {
            $scope.DATA = d.data;
            if (d.data.length !== 0) {
                $scope.showdata = true;
            }
            else {
                $scope.showdata = false;
            }
        }, function () {
            alertify.error("ошибка");
        });
    }
    $scope.V2 = function (id) {
        $http({
            method: "post",
            url: "/Administirirovaniye/GetRoleys2?id="+id,
        }).then(function (d) {
            $scope.DATA3 = d.data;
            
        }, function () {
            alertify.error("ошибка");
        });
    }
    function V() {
        $http({
            method: "post",
            url: "/Administirirovaniye/GetRoleys",
        }).then(function (d) {
            $scope.DATA2 = d.data;

        }, function () {
            alertify.error("ошибка");
        });
    }
}]);