app.controller("SPController", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    GetSP();
    $scope.showdata = false;
    $scope.showdelete = false;
    function GetSP() {
        $http({
            method: "post",
            url: "/Administirirovaniye/GetSP",
        }).then(function (d) {
            $scope.SP = d.data;
            if (d.data.length != 0) {
                $scope.showdata = true;
            }
            else {
                $scope.showdata = false;
            }
        }, function () {
            alertify.error("ошибка");
        });
    }
    $scope.editSP = function (pz) {
        $scope.showdelete = true;
        $scope.ID = pz.ID;
        $scope.INN = pz.INN;
        $scope.O = pz.O;
        $scope.D = pz.D;
        $scope.SB = pz.SB;
        $scope.SE = pz.SE;
        $scope.Action = "Изменить";
    }
    $scope.AddSPDiv = function () {
        $scope.showdelete = false;
        $scope.ID = "";
        $scope.INN = "";
        $scope.O = "";
        $scope.D = "";
        $scope.SB = "";
        $scope.SE = "";
        $scope.Action = "Добавить";
    }
    $scope.AddUpdateSP = function () {
        var getGZAction = $scope.Action;
        var PZ = {
            DEPARTMENT_BASE: $scope.INN,
            description:$scope.D,
            score_begin: $scope.SB,
            score_end: $scope.SE
        };
        if (getGZAction == "Изменить ") {
            PZ.ID = $scope.ID;
            $http({
                method: "post",
                url: "/Administirirovaniye/UpdateSP",
                data: JSON.stringify(PZ),
                dataType: "json"
            }).then(function (msg) {
                GetSP();
            }, function () {
                alertify.error("ошибка");
            });
        }
        else {
            $http({
                method: "post",
                url: "/Administirirovaniye/AddSP",
                data: JSON.stringify(PZ),
                dataType: "json"
            }).then(function (msg) {
                GetSP();
            }, function () {
                alertify.error("ошибка");
            });
        }
    }
    $scope.deleteSP = function (ID) {
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите удалить?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                $http({
                    method: "post",
                    url: "/Administirirovaniye/DeleteSP",
                    params: {
                        id: JSON.stringify(ID)
                    }
                }).then(function (msg) {
                    GetSP();
                }, function () {
                    alertify.error("ошибка");
                });
            } else {
                alertify.error("Операция отменена");
            }
        });
        return false;
    }
    $scope.LoadINN = function () {
        $scope.O = "";
        try {
            var inn = $scope.INN;
            if (inn.length == 9) {
                $http({
                    method: 'get',
                    url: '/Home/LoadINN?inn=' + inn
                }).then(function (response) {
                    console.log(response, 'res');
                    $scope.O = response.data;
                }, function (error) {
                    alertify.error("ошибка");
                    console.log(error, 'error in LoadINN');
                });
            }
        }
        catch (err) {
            $scope.O = "";
        }
    }
    $scope.exportData = function () {
        var blob = new Blob([document.getElementById('export').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Создание проводок.xls");
    };
}]);