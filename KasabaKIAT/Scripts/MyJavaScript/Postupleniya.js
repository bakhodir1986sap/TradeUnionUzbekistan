app.controller("Postupleniya", ["$scope", "$http", "$filter", "DataService", "CalcService", function ($scope, $http, $filter, DataService, CalcService) {
    $scope.showdata = false;
    $scope.LoadINN = function () {
        LoadDATA($scope.INN);
    }
    function LoadDATA(inn) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'POST',
            url: "/Otchot/GetPostupleniya?inn=" + inn,
        }).then(function (d) {
            if (d.data.length != 0) {
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
    $scope.exportData = function () {
        var blob = new Blob([document.getElementById('export').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Поступления.xls");
    };
}]);