app.controller("izmParol", ["$scope", "$http", "$filter", "DataService", "CalcService", function ($scope, $http, $filter, DataService, CalcService) {
    $scope.showdata = false;
    $scope.showdata2 = false;
    $scope.TP = function () {
        $http({
            method: 'POST',
            url: "/Home/TP?p="+$scope.P0,
        }).then(function (d) {
            if (d.data == "yes") {
                $scope.P1 = "";
                $scope.P2 = "";
                $scope.showdata = true;
                alertify.success("yes");
            } else {
                $scope.showdata = false;
                alertify.error("no");
            }
            console.log(d.data);
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in TP -> ", error);
            $("#hh").modal('hide');
        });
    }
    $scope.NP=function() {
        $http({
            method: 'POST',
            url: "/Home/NP?p=" + $scope.P1,
        }).then(function (d) {
            if (d.data == "yes") {
                $scope.P0 = "";
                $scope.showdata = false;
                alertify.success("yes");
            } else {
                $scope.showdata = true;
                alertify.error("no");
            }
            console.log(d.data);
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in NP -> ", error);
            $("#hh").modal('hide');
        });
    }
}]);