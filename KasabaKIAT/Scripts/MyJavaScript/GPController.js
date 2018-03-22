app.controller("GPController", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    GetGP_S(17203, -1);
    $scope.GPdiv = true;
    $scope.showdelete = false;
    AllRegion();
    AllPlaceType();
    AllSanatory();
    KvartalList();
    $scope.LoadGPData = function () {
        //alert($scope.SCf.id);
        if ($scope.s === "option1") {
            GetGP_S($scope.RegionFilter.id, $scope.SCFilter.id);
            //GetPP_SUM_S();
        } else {
            if ($scope.s === "option2") {
                GetGP_C($scope.RegionFilter.id, $scope.SCFilter.id);
                //GetPP_SUM_C();
            }
        }
    }
    $scope.LoadSC = function () {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        Tozala();
        AllRegion();
        AllPlaceType();
        KvartalList();
        if ($scope.s === "option1") {
            AllSanatory();
        }
        else {
            if ($scope.s === "option2") {
                AllCamp();
            }
        }
        document.getElementById('connection').style.visibility = "hidden";
        $("#hh").modal('hide');
    }
    $scope.SC = {
        id: '-1',
        value: ''
    };
    $scope.Region = {
        id: 17203,
        value: ''
    };
    $scope.PlaceType = {
        REC_ID: '1',
        NAME_UZ: ''
    };

    $scope.Kvartal = {
        id: '1',
        value: ''
    };

    $scope.SCFilter = {
        id: '-1',
        value: ''
    };
    $scope.RegionFilter = {
        id: 17203,
        value: ''
    };
    $scope.PlaceTypeFilter = {
        REC_ID: '1',
        NAME_UZ: ''
    };
    function GetGP_S(R_id,SC_id) {
        $scope.s = "option1";
        $http({
            method: "post",
            url: "GetGodovoyPlan_S?R=" + R_id + "&SC=" + SC_id,
        }).then(function (d) {
            $scope.GP = d.data;
            if (d.data.length !== 0) {
                $scope.GPdiv = true;
            }
        }, function () {
            alertify.error("ошибка");
            console.log("error in GP");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    function GetGP_C(R_id, SC_id) {
        $http({
            method: "post",
            url: "GetGodovoyPlan_C?R=" + R_id + "&SC=" + SC_id,
        }).then(function (d) {
            $scope.GP = d.data;
            if (d.data.length !== 0) {
                $scope.GPdiv = true;
            }
        }, function () {
            alertify.error("ошибка");
            console.log("error in GetGodovoyPlan_C");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    function AllPlaceType(){
     $http({
            method: 'get',
            url: '/Home/Getdictionary'
        }).then(function (response) {
            $scope.D = response.data;
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in Getdictionary");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }


    function KvartalList() {
        $http({
            method: 'get',
            url: '/GodovoyPlan/GetKvartal'
        }).then(function (response) {
            $scope.kvartal = response.data;
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetKvartal");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }


        function AllRegion(){
        $http({
            method: 'get',
            url: '/GodovoyPlan/GetRegion'
        }).then(function (response) {
            $scope.R = response.data;
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRegion");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
        }

        function AllSanatory() {
        $scope.scname = "Санаторий";
        $http({
        method: 'get',
        url: '/Home/GetSanatory'
    }).then(function (s) {
        $scope.CS = s.data;
    }, function () {
        alertify.error("ошибка");
        console.log('Error in GetSanatory');
        document.getElementById('connection').style.visibility = "hidden";
        $("#hh").modal('hide');
    });
}
function AllCamp() {
    $scope.scname = "Дет. лагер";
    $http({
        method: 'get',
        url: '/Home/GetCamp'
    }).then(function (c) {
        $scope.CS = c.data;
    }, function () {
        alertify.error("ошибка");
        console.log('Error in GetCamp');
        document.getElementById('connection').style.visibility = "hidden";
        $("#hh").modal('hide');
    });
}
$scope.AddGPDiv = function () {
    $scope.showdelete = false;
        ClearFields();
        $scope.Action = "Добавить";
    }
    function ClearFields() {
        $scope.P = "";
        $scope.Q = "";
    }
    function Tozala() {
        $scope.RegionFilter.id = 1;
        $scope.SCFilter.id = -1;
        if ($scope.s === "option1") {
            GetGP_S($scope.RegionFilter.id, $scope.SCFilter.id);
            //GetPP_SUM_S();
        } else {
            if ($scope.s === "option2") {
                GetGP_C($scope.RegionFilter.id, $scope.SCFilter.id);
                //GetPP_SUM_C();
            }
        }
        $scope.KFilter = "";
        $scope.PFilter = "";
        $scope.QFilter = "";
    }
    $scope.OF = function () {
        Tozala();
    }
    $scope.editGP = function (gp) {
        $scope.showdelete = true;
        $scope.ID = gp.ID;
        $scope.Region.id = gp.R_id;
        $scope.Region.value = gp.R;
        $scope.SC.id = gp.SC_id;
        $scope.SC.value = gp.SC;
        $scope.Kvartal.id = gp.K;
        $scope.PlaceType.NAME_UZ = gp.TM;
        $scope.PlaceType.REC_ID = gp.TM_id;
        $scope.P = gp.P;
        $scope.Q = gp.Q;
        $scope.Action = "Изменить";
    }
    $scope.AddUpdateGP = function () {
        var getGPAction = $scope.Action;
        var GP;
        if ($scope.s === "option1") {
            GP = {
                KVARTAL: $scope.Kvartal.id,
                SANATORIUM_ID: $scope.SC.id,
                TIP_MEST_ID: $scope.PlaceType.REC_ID,
                PRICE: $scope.P,
                QUANTITY: $scope.Q,
                REGION_ID: $scope.Region.id,
            };
        } else {
            if ($scope.s === "option2") {
                GP = {
                    KVARTAL: $scope.Kvartal.id,
                    CAMP_ID: $scope.SC.id,
                    TIP_MEST_ID: $scope.PlaceType.REC_ID,
                    PRICE: $scope.P,
                    QUANTITY: $scope.Q,
                    REGION_ID: $scope.Region.id,
                };
            }
        }
        if (getGPAction === "Изменить") {
            GP.REC_ID = $scope.ID;
            $http({
                method: "post",
                url: "/GodovoyPlan/UpdateGodovoyPlan",
                data: JSON.stringify(GP),
                dataType: "json"
            }).then(function (msg) {
                if ($scope.s === "option1") {
                    GetGP_S($scope.RegionFilter.id, $scope.SCFilter.id);
                } else {
                    if ($scope.s === "option2") {
                        GetGP_C($scope.RegionFilter.id, $scope.SCFilter.id);
                    }
                }
            }, function () {
                alertify.error("ошибка");
                console.log('Error in UpdateGodovoyPlan');
                document.getElementById('connection').style.visibility = "hidden";
                $("#hh").modal('hide');
            });
        }
        else {
            $http({
                method: "post",
                url: "/GodovoyPlan/AddGodovoyPlan",
                data: JSON.stringify(GP),
                dataType: "json"
            }).then(function (msg) {
                if ($scope.s === "option1") {
                    GetGP_S($scope.RegionFilter.id, $scope.SCFilter.id);
                } else {
                    if ($scope.s === "option2") {
                        GetGP_C($scope.RegionFilter.id, $scope.SCFilter.id);
                    }
                }
            }, function () {
                alertify.error("ошибка");
                console.log('Error in AddGodovoyPlan');
                document.getElementById('connection').style.visibility = "hidden";
                $("#hh").modal('hide');
            });
        }
    }
    $scope.deleteGP = function (ID) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите удалить?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                $http({
                    method: "post",
                    url: "/GodovoyPlan/DeleteGodovoyPlan",
                    params: {
                        id: JSON.stringify(ID)
                    }
                }).then(function (msg) {
                    document.getElementById('connection').style.visibility = "hidden";
                    $("#hh").modal('hide');
                    if ($scope.s === "option1") {
                        GetGP_S($scope.RegionFilter.id, $scope.SCFilter.id);
                    } else {
                        if ($scope.s === "option2") {
                            GetGP_C($scope.RegionFilter.id, $scope.SCFilter.id);
                        }
                    }
                }, function () {
                    alertify.error("ошибка");
                    console.log('Error DeleteGodovoyPlan');
                    document.getElementById('connection').style.visibility = "hidden";
                    $("#hh").modal('hide');
                });
            } else {
                document.getElementById('connection').style.visibility = "hidden";
                $("#hh").modal('hide');
                alertify.error("Операция отменена");
            }
        });
        return false;
    }
    $scope.exportData = function () {
        var blob = new Blob([document.getElementById('export').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Годовой план.xls");
    };
}]);