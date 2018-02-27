app.controller("RaspredeleniyeZaezdov", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    $scope.showdata = false;
    $scope.showdata2 = false;
    $scope.showdelete = false;
    $scope.ZB = "2018";
    All("2018");
    All2();
    All3("2018", "11");
    GetData1("2018", "11");
    GetData2("2018", "11");
    function GetData1(y,k) {
        $http({
            method: "post",
            url: "/Administirirovaniye/GetRaspredeleniyeZaezdov1?yil=" + y + "&kod=" + k,
        }).then(function (d) {
            $scope.DATA = d.data;
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
    function GetData2(y, k) {
        $http({
            method: "post",
            url: "/Administirirovaniye/GetRaspredeleniyeZaezdov2?yil=" + y + "&kod=" + k,
        }).then(function (d) {
            $scope.DATA2 = d.data;
            if (d.data.length != 0) {
                $scope.showdata2 = true;
            }
            else {
                $scope.showdata2 = false;
            }
        }, function () {
            alertify.error("ошибка");
        });
    }
    $scope.Load = function (id) {
        All($scope.ZB);
    }
    $scope.LoadDATA = function () {
        GetData1($scope.ZB, $scope.SP.d1);
        GetData2($scope.ZB, $scope.SP.d1);
        All3($scope.ZB, $scope.SP.d1);
        //$("#hh").modal({
        //    backdrop: 'static',
        //    backdrop: 'static'
        //});
        //reset();
        //alertify.set({ buttonFocus: "cancel" });
        //alertify.confirm("Вы действительно хотите получить?", function (e) {
        //    if (e) {
        //        alertify.success("Операция выполняется");
        //        document.getElementById('connection').style.visibility = "visible";
        //        $("#hh").modal('hide');
        //        GetData1($scope.ZB, $scope.SP.d1);
        //        GetData2($scope.ZB, $scope.SP.d1);
        //    } else {
        //        alertify.error("Операция отменена");
        //        $("#hh").modal('hide');
        //    }
        //});
        //return false;
    };
    $scope.SP = {
        d1: '11',
        d2: ''
    };
    $scope.SP2 = {
        d1: '11',
        d2: ''
    };
    $scope.SP3 = {
        d1: '000000002',
        d2: ''
    };
    $scope.SP4 = {
        d1: '11',
        d2: ''
    };
    function All(yil) {
        $http({
            method: 'get',
            url: '/Administirirovaniye/GetSanatory?yil='+yil,
        }).then(function (response) {
            $scope.R = response.data;
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetSanatory");
        });
    }
    function All2() {
        $http({
            method: 'get',
            url: '/Administirirovaniye/GetSpisokPravsoyuzov'
        }).then(function (response) {
            $scope.Rr = response.data;
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRegion");
        });
    }
    function All3(y, k) {
        $http({
            method: 'get',
            url: "/Administirirovaniye/GetKod?yil=" + y + "&kod=" + k,
        }).then(function (response) {
            $scope.Rr2 = response.data;
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRegion");
        });
    }
    $scope.edit = function (gp) {
        $scope.showdelete = true;
        $scope.ID = gp.ID;
        $scope.ZB2 = gp.d1;
        $scope.SP2.d1 = gp.d2;
        $scope.SP3.d1 = gp.d3;
        $scope.SP4.d1 = gp.d4;
        $scope.Q1 = gp.d5;
        $scope.Q2 = gp.d6;
        $scope.Q3 = gp.d7;
        $scope.Q4 = gp.d8;
        $scope.Action = "Изменить";
    }
    $scope.AddDiv = function () {
        $scope.showdelete = false;
        ClearFields();
        $scope.Action = "Добавить";
    }
    function ClearFields() {
        $scope.ZB2 = "2018";
        $scope.Q1 = "";
        $scope.Q2 = "";
        $scope.Q3 = "";
        $scope.Q4 = "";
    }
    $scope.AddUpdate = function () {
        var getGPAction = $scope.Action;
        var GP = {
            GodZaezdov: $scope.ZB2,
            SanatoryKod: $scope.SP2.d1,
            ProfsoyzKod: $scope.SP3.d1,
            SanatoryRazmeshenieKod: $scope.SP4.d1,
            Putevok1kv: $scope.Q1,
            Putevok2kv: $scope.Q2,
            Putevok3kv: $scope.Q3,
            Putevok4kv: $scope.Q4,
        };
        if (getGPAction == "Изменить") {
            GP.ID = $scope.ID;
            $http({
                method: "post",
                url: "/Administirirovaniye/UpdateSpisokRaspredeleniya",
                data: JSON.stringify(GP),
                dataType: "json"
            }).then(function (msg) {
                All($scope.ZB2);
                GetData2($scope.ZB, $scope.SP.d1);
                All3($scope.ZB, $scope.SP.d1);
            }, function () {
                alertify.error("ошибка");
                console.log('Error in UpdateGodovoyPlan');
                $("#hh").modal('hide');
            });
        }
        else {
            $http({
                method: "post",
                url: "/Administirirovaniye/AddSpisokRaspredeleniya",
                data: JSON.stringify(GP),
                dataType: "json"
            }).then(function (msg) {
                All($scope.ZB2);
                GetData2($scope.ZB, $scope.SP.d1);
                All3($scope.ZB, $scope.SP.d1);
            }, function () {
                alertify.error("ошибка");
                console.log('Error in AddGodovoyPlan');
                $("#hh").modal('hide');
            });
        }
    }
    $scope.delete = function (ID) {
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите удалить?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                $http({
                    method: "post",
                    url: "/Administirirovaniye/DeleteSpisokRaspredeleniya",
                    params: {
                        id: JSON.stringify(ID)
                    }
                }).then(function (msg) {
                    GetData2($scope.ZB, $scope.SP.d1);
                    All3($scope.ZB, $scope.SP.d1);
                }, function () {
                    alertify.error("ошибка");
                    console.log('Error DeleteGodovoyPlan');
                    $("#hh").modal('hide');
                });
            } else {
                $("#hh").modal('hide');
                alertify.error("Операция отменена");
            }
        });
        return false;
    }
    $scope.exportData1 = function () {
        var blob = new Blob([document.getElementById('export1').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "file1.xls");
    };
    $scope.exportData2 = function () {
        var blob = new Blob([document.getElementById('export2').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "file2.xls");
    };
}]);