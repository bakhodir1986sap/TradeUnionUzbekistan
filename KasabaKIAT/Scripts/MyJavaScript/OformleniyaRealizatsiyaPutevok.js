app.controller("OformleniyaRealizatsiyaPutevok", ["$scope", "$http", "$filter", "DataService", function ($scope, $http, $filter, DataService) {
    $scope.desiredLocation = DataService.get();
    LoadDATA();
    $scope.showdata = false;
    function LoadDATA() {
        console.log($scope.desiredLocation);
        $.ajax({
            type: "POST",
            url: "/PrixodPutevok/ORP",
            data: JSON.stringify($scope.desiredLocation),
            contentType: "application/json; charset=utf-8",
            success: function (d) {
                $scope.DATA = d.data;
                console.log(d);
                $("#hh").modal('hide');
            },
        });
    }
}]);