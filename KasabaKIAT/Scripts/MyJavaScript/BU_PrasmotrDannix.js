app.controller("BU_PrasmotrDannix", ["$scope", "$http", "$filter", "DataService", "CalcService", function ($scope, $http, $filter, DataService, CalcService) {
    LoadDATA();
    function LoadDATA() {
        $http({
            method: "post",
            url: '/Buxgalter/GetVvodDannix',
        }).then(function (d) {
            $scope.showdata = false;
            if (d.data == "error") {
                alertify.error("ошибка");
            } else {
                $scope.DATA = d.data;
                console.log(d.data);
            }
        }, function (err) {
            alertify.error("ошибка : " + err.data);
            console.log(err.data);
            console.log("error in parse excel");
        });
    }
}]);