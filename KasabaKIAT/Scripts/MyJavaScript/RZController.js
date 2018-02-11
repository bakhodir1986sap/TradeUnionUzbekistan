app.controller("RZController", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    $scope.RZdiv = false;
    $scope.RZ_Gdiv = false;
    $scope.ZB = '2018';
    GetRZ_S();
    GetRZ_G_S();
    getPrice();
    GetBank();
    $scope.STATUS = {
        ID: '2',
        NAME_UZ: ''
    }
    $scope.LoadRZStatus = function () {
        $http({
            method: 'get',
            url: '/Home/Getdictionary_RZ'
        }).then(function (response) {
            $scope.S = response.data;
        }, function (error) {
            console.log(error, 'can not get data.');
        });
    }
    function AddOQ_S(SC_g_id, ZB_g, OQ) {
        var getData = crudAJService.AddOQ_S($scope.INN, $scope.ZB, ZB_g, SC_g_id, OQ);
        getData.then(function (p) {
            GetRZ_G_S();
        }, function () {
            //alert('Error in AddOQ_S');
            console.log("Error in AddOQ_S");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    function AddOQ_C(SC_g_id, ZB_g, OQ) {
        var getData = crudAJService.AddOQ_C($scope.INN, $scope.ZB, ZB_g, SC_g_id, OQ);
        getData.then(function (p) {
            GetRZ_G_C();
        }, function () {
            //alert('Error in AddOQ_C');
            console.log('Error in AddOQ_C');
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    $scope.AddOQ = function (SC_g_id, ZB_g, OQ) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите cохранить?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                if ($scope.s == "option1") {
                    AddOQ_S(SC_g_id, ZB_g, OQ);
                    $("#hh").modal('hide');
                }
                else {
                    if ($scope.s == "option2") {
                        AddOQ_C(SC_g_id, ZB_g, OQ);
                        $("#hh").modal('hide');
                    }
                }

            } else {
                document.getElementById('connection').style.visibility = "hidden";
                $("#hh").modal('hide');
                alertify.error("Операция отменена");
            }
        });
        return false;
    }
    $scope.UpdateStatusRZ = function (id, s) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите cохранить?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                var getData = crudAJService.UpdateStatusRZ(id, s);
                getData.then(function (p) {
                    if ($scope.s = "option1") {
                        GetRZ_S();
                        $("#hh").modal('hide');
                    }
                    else {
                        if ($scope.s = "option2") {
                            GetRZ_C();
                            $("#hh").modal('hide');
                        }
                    }
                }, function () {
                    document.getElementById('connection').style.visibility = "hidden";
                    $("#hh").modal('hide');
                    //alert('Error in UpdateStatusRZ');
                    console.log('Error in UpdateStatusRZ');
                });

            } else {
                document.getElementById('connection').style.visibility = "hidden";
                $("#hh").modal('hide');
                alertify.error("Операция отменена");
            }
        });
        return false;
        
    }
    function getPrice() {
        var getData = crudAJService.GetPrice($scope.ZB);
        getData.then(function (p) {
            $scope.Price = p.data;
        }, function () {
            //alert('Error in getPrice');
            console.log('Error in getPrice');
        });
    }
    function GetBank() {
        var getData = crudAJService.Getbankpayments($scope.INN, $scope.ZB);
        getData.then(function (b) {
            $scope.Bank = b.data;
        }, function () {
            //alert('Error in GetBank');
            console.log('Error in GetBank');
        });
        var d = crudAJService.GetbankpaymentsYear($scope.INN, $scope.ZB);
        d.then(function (b) {
            $scope.GroupYear = b.data;
        }, function () {
            //alert('Error in GetbankpaymentsYear');
            console.log('Error in GetbankpaymentsYear');
        });
    }
    function GetRZ_G_S(){
        var getData = crudAJService.getGroupRZ_S($scope.INN, $scope.ZB);
        getData.then(function (s) {
            $scope.RZ_G = s.data;
            if (s.data.length != 0) {
                $scope.RZ_Gdiv = true;
            }
            else {
                $scope.RZ_Gdiv = false;
            }
        }, function () {
            //alert('Error in getting GetRZ_G_S records');
            console.log('Error in getting GetRZ_G_S records');
        });
    }
    function GetRZ_G_C() {
        var getData = crudAJService.getGroupRZ_C($scope.INN, $scope.ZB);
        getData.then(function (s) {
            $scope.RZ_G = s.data;
            if (s.data.length != 0) {
                $scope.RZ_Gdiv = true;
            }
            else {
                $scope.RZ_Gdiv = false;
            }
        }, function () {
            //alert('Error in getting GetRZ_G_C records');
            console.log('Error in getting GetRZ_G_C records');
        });
    }
    function GetRZ_S() {
        $scope.s = "option1";
        $scope.myValue = "санаторий";
        var getData = crudAJService.getRZ_S_INN($scope.INN, $scope.ZB);
        getData.then(function (s) {
            $scope.RZ = s.data;
            if (s.data.length != 0) {
                $scope.RZdiv = true;
            }
            else {
                $scope.RZdiv = false;
            }
        }, function () {
            //alert('Error in getting GetRZ_S records');
            console.log('Error in getting GetRZ_S records');
        });
    }
    function GetRZ_C() {
        var getData = crudAJService.getRZ_C_INN($scope.INN,$scope.ZB);
        getData.then(function (c) {
            $scope.RZ = c.data;
            if (c.data.length != 0) {
                $scope.RZdiv = true;
            }
            else {
                $scope.RZdiv = false;
            }
        }, function () {
            //alert('Error in getting GetRZ_C records');
            console.log('Error in getting GetRZ_C records');
        });
    }
    $scope.SC = {
        id: '1',
        value: ''
    };
    $scope.editRZ = function (rz) {
     $scope.ID = rz.ID;
     var tz = (rz.TS) ? $filter('date')(parseInt(rz.TS.substr(6)), 'dd.MM.yyyy') : '';
     $scope.TZ = new Date(tz.substring(6, 10), tz.substring(3, 5) - 1, tz.substring(0, 2));
        //var zb = (rz.ZB) ? $filter('date')(parseInt(rz.ZB.substr(6)), 'dd.MM.yyyy') : '';
        //var ze = (rz.ZE) ? $filter('date')(parseInt(rz.ZE.substr(6)), 'dd.MM.yyyy') : '';
        //$scope.ZB = new Date(zb.substring(6, 10), zb.substring(3, 5) - 1, zb.substring(0, 2));
        //$scope.ZE = new Date(ze.substring(6, 10), ze.substring(3, 5) - 1, ze.substring(0, 2));;
        $scope.INN = rz.INN;
        $scope.O = rz.O;
        $scope.FN = rz.FN;
        $scope.P = rz.P;
        $scope.Q = rz.Q;
        $scope.MOE = rz.MOE;
        $scope.MO = rz.MO;
        $scope.SC.id = rz.SC_id;
        $scope.SC.value = rz.SC;
        $scope.Action = "Изменить";
    }
    $scope.LoadINN = function () {
        if ($scope.s != null && $scope.INN != null && $scope.ZB != null) {
            $("#hh").modal({
                backdrop: 'static',
                backdrop: 'static'
            });
            document.getElementById('connection').style.visibility = "visible";
            $("#hh").modal({
                backdrop: 'static'
            });
            $scope.RZdiv = false;
            $scope.RZ_Gdiv = false;
            if ($scope.s == "option1") {
                if ($scope.INN != null && $scope.ZB != null) {
                    GetRZ_S();
                    GetRZ_G_S();
                    getPrice();
                    GetBank();
                }
                document.getElementById('connection').style.visibility = "hidden";
                $("#hh").modal('hide');
            } else {
                if ($scope.s == "option2") {
                    if ($scope.INN != null && $scope.ZB != null) {
                        GetRZ_C();
                        GetRZ_G_C();
                        getPrice();
                        GetBank();
                    }
                    document.getElementById('connection').style.visibility = "hidden";
                    $("#hh").modal('hide');
                }
            }
            var z = $scope.ZB;
            $scope.Sana = {
                "SANA":
                [
                    {
                        "ZB_g": "1",
                        "oy": "YANVAR " + z
                    },
                    {
                        "ZB_g": "2",
                        "oy": "FEVRAL " + z
                    },
                    {
                        "ZB_g": "3",
                        "oy": "MART " + z
                    },
                    {
                        "ZB_g": "4",
                        "oy": "APREL " + z
                    },
                    {
                        "ZB_g": "5",
                        "oy": "MAY " + z
                    },
                    {
                        "ZB_g": "6",
                        "oy": "IYUN " + z
                    },
                    {
                        "ZB_g": "7",
                        "oy": "IYUL " + z
                    },
                    {
                        "ZB_g": "8",
                        "oy": "AVGUST " + z
                    },
                    {
                        "ZB_g": "9",
                        "oy": "SENTABR " + z
                    },
                    {
                        "ZB_g": "10",
                        "oy": "OKTABR " + z
                    },
                    {
                        "ZB_g": "11",
                        "oy": "NOYABR " + z
                    },
                    {
                        "ZB_g": "12",
                        "oy": "DEKABR " + z
                    }
                ]
            };
        }

}
 
}]);