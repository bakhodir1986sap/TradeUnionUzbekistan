app.controller("izmParol", ["$scope", "$http", "$filter", "DataService", "CalcService", function ($scope, $http, $filter, DataService, CalcService) {
    LoadDATA();
    $scope.showdata = false;
    $scope.showdata2 = false;
    $scope.TP = function () {
        $http({
            method: 'POST',
            url: "/Home/TP?p="+$scope.P0,
        }).then(function (d) {
            if (d.data == "yes") {
                $scope.showdata = true;
            } else {
                $scope.showdata = false;
            }
            console.log(d.data);
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in TP -> ", error);
            $("#hh").modal('hide');
        });
    }
    function LoadDATA() {
        $http({
            method: 'POST',
            url: "/PrixodPutevok/SD",
        }).then(function (d) {
            if (d.data.length != 0) {
                $scope.showdata = true;
            } else {
                $scope.showdata = false;
            }
            $scope.DATA = d.data;
            console.log(d.data);
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in SD -> ", error);
            $("#hh").modal('hide');
        });
    }
}]);