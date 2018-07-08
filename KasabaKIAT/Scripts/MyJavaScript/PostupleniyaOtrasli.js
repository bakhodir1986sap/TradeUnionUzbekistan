app.controller("PostupleniyaOtrasli", ["$scope", "$http", "$filter", "$locale", "DataService", "CalcService", function ($scope, $http, $filter, $locale,DataService, CalcService) {
    AllRegion();
    $scope.showdata = false;
    $scope.ZB = new Date
    $scope.ZE = new Date();
    $scope.LoadDATA = function () {
        LoadDATA($scope.Region.id, $scope.ZB, $scope.ZE);
    }
    function LoadDATA(s, bg, end) {

        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";

        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'POST',
            url: "/Otchot/GetPostupleniyaOtrasli?s=" + s + "&bg=" + bg.toISOString() + "&end=" + end.toISOString(),
        }).then(function (d) {
            if (d.data.length != 0) {
                $scope.showdata = true;
                $scope.q = d.data.length;
            } else {
                $scope.showdata = false;
            }
            $scope.DATA = d.data;
            console.log(d.data);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetPostupleniya -> ", error);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    $scope.Region = {
        id: 0,
        value: ''
    };
    function AllRegion() {
        $http({
            method: 'get',
            url: '/Otchot/GetRegion'
        }).then(function (response) {
            $scope.R = response.data;
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRegion");
        });
    }
    $scope.Clear = function () {
        $scope.Region.id = 0;
        $scope.ZB = new Date
        $scope.ZE = new Date();
        $scope.showdata = false;
    }
    $scope.exportData = function () {
        var blob = new Blob([document.getElementById('export').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Поступление Группированно По Отрасли.xls");
    };
}]);