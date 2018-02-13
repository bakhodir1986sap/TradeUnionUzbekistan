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


    function Export() {
        $http({
            method: "post",
            url: "/Administirirovaniye/ExportToExcelPravsoyuz",
        }).then(function (d) {
            document.getElementById('connection').style.visibility = "hidden";
        }, function () {
            alert("error in O_SOZ");
        });
    }

    $scope.LoadDATA = function () {
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите ...?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                document.getElementById('connection').style.visibility = "visible";
                Load();
            } else {
                alertify.error("Операция отменена");
            }
        });
        return false;
    };


    $scope.ExportDATA = function () {
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите Export...?", function (e) {
            if (e) {
                alertify.success("Операция Export выполняется");
                document.getElementById('connection').style.visibility = "visible";
                Export();
            } else {
                alertify.error("Операция отменена");
            }
        });
        return false;
    };
}]);