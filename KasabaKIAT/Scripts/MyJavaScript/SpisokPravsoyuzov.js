app.controller("SpisokPravsoyuzov", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    $scope.showdata = false;
    GetData();
    function GetData() {
        $http({
            method: "post",
            url: "/Administirirovaniye/GetSpisokPravsoyuzov",
        }).then(function (d) {
            $scope.DATA = d.data;
            if (d.data.length != 0) {
                $scope.showdata = true;
            }
            else {
                $scope.showdata = false;
            }
        }, function () {
            //alert("error in O_SOZ");
        });
    }
    function Load() {
        $http({
            method: "post",
            url: "/Administirirovaniye/ServiceSpisokPravsoyuzov",
        }).then(function (d) {
            document.getElementById('connection').style.visibility = "hidden";
            GetData();
        }, function () {
            //alert("error in O_SOZ");
        });
    }
    $scope.LoadDATA = function () {
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите получить?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                document.getElementById('connection').style.visibility = "visible";
                $("#hh").modal('hide');
                Load();
            } else {
                alertify.error("Операция отменена");
                $("#hh").modal('hide');
            }
        });
        return false;
    };
    $scope.exportData = function () {
        var blob = new Blob([document.getElementById('export').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Список правсоюзов.xls");
    };
}]);