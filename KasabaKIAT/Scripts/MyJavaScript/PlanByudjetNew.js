app.controller("planByudjetNew", ["$scope", "$http", "$filter", function ($scope, $http, $filter) {

    GetPravsoyuzOrg();
    $scope.selectedyear = 2018;
    $scope.selectedRegion = '';

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


    $scope.GetPlanContent = function () {

        $scope.PlanList = null;
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Buxgalter/GetRegionPlan?soato_code=' + $scope.selectedRegion.Kod + '&year=' + $scope.selectedyear
        }).then(function (response) {
            $scope.PlanList = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetPBRYears");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }


    function GetPravsoyuzOrg() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Buxgalter/SpisokPravsoyuzov'
        }).then(function (response) {
            $scope.Pravsoyuz = response.data;
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

        $http({
            method: 'get',
            url: '/Buxgalter/GetRegionListByYear?year=' + year
        }).then(function (response) {
            $scope.Regions = response.data;
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRegionListByYear");
        });
    }

    $scope.update =  function (planRow) {

        $http({
            method: 'post',
            url: '/Buxgalter/SaveRegionPlan',
            data: JSON.stringify(planRow),
            contentType: "application/json; charset=utf-8",
        }).then(function (response) {
            $scope.GetPlanContent();
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in SaveRegionPlan");
        });
    }


    $scope.delete = function () {

        $scope.Regions = null;

        $http({
            method: 'get',
            url: '/Buxgalter/DeleteRegionPlan?soato_code=' + $scope.selectedRegion.Kod + '&year=' + $scope.selectedyear
        }).then(function (response) {
            $scope.GetPlanContent();
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in SaveRegionPlan");
        });
    }

}]);