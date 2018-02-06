app.controller("GPController", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    GetGP_S(1, 1);
    $scope.GPdiv = true;
    AllRegion();
    AllPlaceType();
    AllSanatory();
    $scope.LoadGPData = function () {
        //alert($scope.SCf.id);
        if ($scope.s == "option1") {
            GetGP_S($scope.RegionFilter.id, $scope.SCFilter.id);
            //GetPP_SUM_S();
        } else {
            if ($scope.s == "option2") {
                GetGP_C($scope.RegionFilter.id, $scope.SCFilter.id);
                //GetPP_SUM_C();
            }
        }
    }
    $scope.LoadSC = function () {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $scope.GPdiv = false;
        Tozala();
        AllRegion();
        AllPlaceType();
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
        id: '1',
        value: ''
    };
    $scope.Region = {
        id: 1726,
        value: ''
    };
    $scope.PlaceType = {
        REC_ID: '1',
        NAME_UZ: ''
    };
    $scope.SCFilter = {
        id: '1',
        value: ''
    };
    $scope.RegionFilter = {
        id: 1726,
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
            if (d.data.length != 0) {
                $scope.GPdiv = true;
            }
        }, function () {
            //alert("error in GP");
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
            if (d.data.length != 0) {
                $scope.GPdiv = true;
            }
        }, function () {
            //alert("error in GetGodovoyPlan_C");
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
            //alert("error in Getdictionary");
            console.log("error in Getdictionary");
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
            //alert("error in GetRegion");
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
        //alert('Error in GetSanatory');
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
        //alert('Error in GetCamp');
        console.log('Error in GetCamp');
        document.getElementById('connection').style.visibility = "hidden";
        $("#hh").modal('hide');
    });
}
    $scope.AddGPDiv = function () {
        ClearFields();
        $scope.Action = "Добавить";
    }
    function ClearFields() {
        $scope.K ="";
        $scope.P = "";
        $scope.Q = "";
    }
    function Tozala() {
        $scope.RegionFilter.id = 1;
        $scope.SCFilter.id = 1;
        if ($scope.s == "option1") {
            GetGP_S($scope.RegionFilter.id, $scope.SCFilter.id);
            //GetPP_SUM_S();
        } else {
            if ($scope.s == "option2") {
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
        $scope.ID = gp.ID;
        $scope.Region.id = gp.R_id;
        $scope.Region.value = gp.R;
        $scope.SC.id = gp.SC_id;
        $scope.SC.value = gp.SC;
        $scope.K = gp.K;
        $scope.PlaceType.NAME_UZ = gp.TM;
        $scope.PlaceType.REC_ID = gp.TM_id;
        $scope.P = gp.P;
        $scope.Q = gp.Q;
        $scope.Action = "изменить";
    }
    $scope.AddUpdateGP = function () {
        var getGPAction = $scope.Action;
        var GP;
        if ($scope.s == "option1") {
            GP = {
                KVARTAL: $scope.K,
                SANATORIUM_ID: $scope.SC.id,
                TIP_MEST_ID: $scope.PlaceType.REC_ID,
                PRICE: $scope.P,
                QUANTITY: $scope.Q,
                REGION_ID: $scope.Region.id,
            };
        } else {
            if ($scope.s == "option2") {
                GP = {
                    KVARTAL: $scope.K,
                    CAMP_ID: $scope.SC.id,
                    TIP_MEST_ID: $scope.PlaceType.REC_ID,
                    PRICE: $scope.P,
                    QUANTITY: $scope.Q,
                    REGION_ID: $scope.Region.id,
                };
            }
        }
        if (getGPAction == "изменить") {
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
                //alert('Error in UpdateGodovoyPlan');
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
                //alert('Error in AddGodovoyPlan');
                console.log('Error in AddGodovoyPlan');
                document.getElementById('connection').style.visibility = "hidden";
                $("#hh").modal('hide');
            });
        }
    }
    $scope.deleteGP = function (ID) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Rostdan ham o`chirmoqchimisiz?", function (e) {
            if (e) {
                alertify.success("O`chirilmoqda");
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
                    //alert('Error DeleteGodovoyPlan');
                    console.log('Error DeleteGodovoyPlan');
                    document.getElementById('connection').style.visibility = "hidden";
                    $("#hh").modal('hide');
                });
            } else {
                document.getElementById('connection').style.visibility = "hidden";
                $("#hh").modal('hide');
                alertify.error("o`chirish bekor qilindi");
            }
        });
        return false;
    }
}]);