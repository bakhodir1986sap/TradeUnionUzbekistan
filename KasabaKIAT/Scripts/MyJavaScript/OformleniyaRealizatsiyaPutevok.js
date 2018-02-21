app.controller("OformleniyaRealizatsiyaPutevok", ["$scope", "$http", "$filter", "DataService", "CalcService", function ($scope, $http, $filter, DataService, CalcService) {
    //$scope.desiredLocation = DataService.get();
    $scope.result = CalcService.square2();
    LoadDATA();
    $scope.showdata = false;
    function LoadDATA() {
        console.log($scope.result);
        $.ajax({
            type: "POST",
            url: "/PrixodPutevok/ORP",
            data: JSON.stringify($scope.result),
            contentType: "application/json; charset=utf-8",
            success: function (d) {
                $scope.DATA = d.data;
                console.log(d);
                $("#hh").modal('hide');
            },
        });
    }
}]);