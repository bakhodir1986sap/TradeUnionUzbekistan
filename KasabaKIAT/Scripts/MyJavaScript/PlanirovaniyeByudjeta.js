app.controller("planByudjet", ["$scope", "$http", "$filter", function ($scope, $http, $filter) {

    GetYears();

    function GetYears() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Buxgalter/GetPBRYears'
        }).then(function (response) {
            $scope.Years = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetPBRYears");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.GetRegionListByYear = function (year) {

        $scope.Regions = null;
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });

        $http({
            method: 'get',
            url: '/Buxgalter/GetRegionListByYear?year=' + year
        }).then(function (response) {
            $scope.Regions = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRegionListByYear");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

}]);