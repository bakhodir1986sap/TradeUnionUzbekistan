app.controller("PostupleniyaOblastyam", ["$scope", "$http", "$filter", "$locale", "DataService", "CalcService", function ($scope, $http, $filter, $locale ,DataService, CalcService) {
    $scope.showdata = false;
    $scope.ZB = new Date
    $scope.ZE = new Date();
    $scope.LoadDATA = function () {
        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";
        LoadDATA($scope.ZB, $scope.ZE);
    }
    function LoadDATA(bg, end) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'POST',
            url: "/Otchot/GetPostupleniyaOblastyam?bg=" + bg.toISOString() + "&end=" + end.toISOString(),
        }).then(function (d) {
            if (d.data.length != 0) {
                $scope.q = d.data.length;
                $scope.showdata = true;
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
    function AllOtrasl() {
        $http({
            method: 'get',
            url: '/Otchot/GetOtrasl'
        }).then(function (response) {
            $scope.O = response.data;
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetOtrasl");
        });
    }
    $scope.Clear = function () {
        $scope.ZB = new Date
        $scope.ZE = new Date();
        $scope.showdata = false;
    }
    $scope.exportData = function () {
        var blob = new Blob([document.getElementById('export').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Поступление Группированно По Областям.xls");
    };
}]);