app.controller("SchetFaktura", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    $scope.showdata = false;
    $scope.showdata2 = false;
    $scope.ZB = new Date();
    $scope.ZE = new Date();
    All();
    GetData($scope.ZB, $scope.ZE, "000000025");
    function GetData(b,e,p) {
        $http({
            method: "post",
            url: "/Administirirovaniye/GetSchetFaktura?B=" + b.toISOString() + "&E=" + e.toISOString() + "&p=" + p,
        }).then(function (d) {
            $scope.DATA = d.data;
            if (d.data.length !== 0) {
                $scope.showdata = true;
            }
            else {
                $scope.showdata = false;
            }
        }, function () {
            alertify.error("ошибка");
        });
    }
    function Load(b, e, p) {
        $http({
            method: "post",
            url: "/Administirirovaniye/ServiceSchetFaktura?B=" + b.toISOString() + "&E=" + e.toISOString() + "&p=" + p,
        }).then(function (d) {
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
            GetData($scope.ZB, $scope.ZE, $scope.SP.d1);
        }, function () {
            $("#hh").modal('hide');
            alertify.error("ошибка");
        });
    }
    function Oprixod(b, e, p) {
        $http({
            method: "post",
            url: "/Administirirovaniye/OprixodovaniyaSchetFaktura?B=" + b.toISOString() + "&E=" + e.toISOString() + "&p=" + p,
        }).then(function (d) {
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
            GetData($scope.ZB, $scope.ZE, $scope.SP.d1);
        }, function () {
            $("#hh").modal('hide');
            alertify.error("ошибка");
        });
    }


    var d = "";
    $scope.LoadStrokaSchetFakturi = function (id) {
        


        $scope.DATA2 = null;


        document.getElementById('connection2').style.visibility = "visible";
        $("#hh2").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        console.log(id);

        $(".activvvv").removeClass("activvvv");
        $("#" + id).addClass("activvvv");

        d = id;
        $http({
            method: "post",
            url: "/Administirirovaniye/GetStrokaSchetFakturi?id=" + id,
        }).then(function (d) {
            $scope.DATA2 = d.data;
            if (d.data.length !== 0) {
                $scope.showdata2 = true;
            }
            else {
                $scope.showdata2 = false;
            }
            document.getElementById('connection2').style.visibility = "hidden";
            $("#hh2").modal('hide');
        }, function () {
            alertify.error("ошибка");
            document.getElementById('connection2').style.visibility = "hidden";
            $("#hh2").modal('hide');
        });
    }
    $scope.LoadDATA = function () {
        $("#hh").modal({
            backdrop: 'static'
        });
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите получить?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                document.getElementById('connection').style.visibility = "visible";
                Load($scope.ZB, $scope.ZE, $scope.SP.d1);
            } else {
                alertify.error("Операция отменена");
                $("#hh").modal('hide');
            }
        });
        return false;
    };

    $scope.OPRIXDATA = function () {
        $("#hh").modal({
            backdrop: 'static'
        });
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите Оприходовать?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                document.getElementById('connection').style.visibility = "visible";
                Oprixod($scope.ZB, $scope.ZE, $scope.SP.d1);
            } else {
                alertify.error("Операция отменена");
                $("#hh").modal('hide');
            }
        });
        return false;
    };

    $scope.SP = {
        d1: '000000025',
        d2: ''
    };
    function All() {
        $http({
            method: 'get',
            url: '/Administirirovaniye/GetSpisokPravsoyuzov'
        }).then(function (response) {
            $scope.R = response.data;
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRegion");
        });
    }
    $scope.exportData1 = function () {
        var blob = new Blob([document.getElementById('export1').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Документ Счет фактуры.xls");
    };
    $scope.exportData2 = function () {
        var blob = new Blob([document.getElementById('export2').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Детали счет фактуры.xls");
    };
}]);