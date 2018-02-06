app.controller("PZController", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    $scope.OTPdiv = false;
    AllSanatory();
    GetPZ_S(1);
    $scope.SC = {
        id: '1',
        value: ''
    };
    $scope.SCFilter = {
        id: '1',
        value: ''
    };
    function AllSanatory() {
        $scope.myVal = "Санаторий";
        $http({
            method: 'get',
            url: '/Home/GetSanatory'
        }).then(function (s) {
            $scope.CS = s.data;
        }, function () {
            //alert('Error in getting Санаторий records');
            console.log('Error in getting Санаторий records');
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    function AllCamp() {
        $scope.myVal = "Дет. лагер";
        $http({
            method: 'get',
            url: '/Home/GetCamp'
        }).then(function (c) {
            $scope.CS = c.data;
        }, function () {
            //alert('Error in getting Дет. лагер records');
            console.log('Error in getting Дет. лагер records');
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    $scope.editPZ = function (pz) {
        $scope.ID = pz.ID;
        var zb = (pz.ZB) ? $filter('date')(parseInt(pz.ZB.substr(6)), 'dd.MM.yyyy') : '';
        var ze = (pz.ZE) ? $filter('date')(parseInt(pz.ZE.substr(6)), 'dd.MM.yyyy') : '';
        $scope.ZB = new Date(zb.substring(6, 10), zb.substring(3, 5) - 1, zb.substring(0, 2));
        $scope.ZE = new Date(ze.substring(6, 10), ze.substring(3, 5) - 1, ze.substring(0, 2));;
        $scope.INN = pz.INN;
        $scope.O = pz.O;
        $scope.FN = pz.FN;
        $scope.P = pz.P;
        $scope.Q = pz.Q;
        $scope.MOE = pz.MOE;
        $scope.MO = pz.MO;
       // $scope.MO2 = pz.MO2;
        $scope.SC.id = pz.SC_id;
        $scope.SC.value = pz.SC;
        $scope.Action = "изменить";
    }
    $scope.AddPZDiv = function () {
        $scope.MOE = false;
        $scope.Q = 1;
        ClearFields();
        $scope.Action = "добавить";
    }
    function GetPZ_S(id) {
        $scope.ss = "option1s";
        $scope.myVal = "Санаторий";
        $http({
            method: "post",
            url: "GetAllPZ_S?sc_id=" + id,
        }).then(function (s) {
            $scope.PZ = s.data;
            if (s.data.length != 0) {
                $scope.PZdiv = true;
                $scope.OTPdiv = false;
            }
        }, function () {
            alert('Error in getting Санаторий records');
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    function GetPZ_C(id) {
        $http({
            method: "post",
            url: "GetAllPZ_C?sc_id="+id,
        }).then(function (c) {
            $scope.PZ = c.data;
            if (c.data.length != 0) {
                $scope.PZdiv = true;
                $scope.OTPdiv = false;
            }
        }, function () {
            //alert('Error in getting Дет. лагер records');
            console.log('Error in GetAllPZ_C');
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    $scope.LoadData= function () {
        if ($scope.ss === "option1s") {
            GetPZ_S($scope.SCFilter.id);
        }
        else {
            if ($scope.ss === "option2s") {
                GetPZ_C($scope.SCFilter.id);
            }
        }
    }
    $scope.LoadSC = function () {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $scope.SCFilter.id = 1;
        $scope.PZdiv = false;
        if ($scope.ss === "option1s") {
            AllSanatory()
            GetPZ_S($scope.SCFilter.id);
        }
        else {
            if ($scope.ss === "option2s") {
                AllCamp();
                GetPZ_C($scope.SCFilter.id);
            }
        }
        document.getElementById('connection').style.visibility = "hidden";
        $("#hh").modal('hide');
    }
    $scope.MOE_ch = function () {
        if ($scope.MOE == false) {
            $scope.MO = "";
        }
    }
    $scope.LoadINN = function () {
    $scope.O = "";
        try{
            var inn = $scope.INN;
            if (inn.length == 9) {
                $http({
                    method: 'get',
                    url: '/Home/LoadINN?inn=' + inn
                }).then(function (response) {
                    console.log(response, 'res');
                    $scope.O = response.data;
                }, function (error) {
                    console.log(error, 'error in LoadINN');
                });
            } 
        }
        catch(err){
            $scope.O="";
        }
    }
    $scope.AddUpdatePZ=function(){
        var getGZAction = $scope.Action;
        var PZ;
        if ($scope.ss == "option1s") {
            PZ = {
                ZAEZD_BEGIN: $scope.ZB,
                ZAEZD_END: $scope.ZE,
                MEDICAL_ORDER: $scope.MO,
                FULL_NAME: $scope.FN,
                DEPARTMENT_CODE: $scope.INN,
                POSITION: $scope.P,
                QUANTITY: $scope.Q,
                SANATORY_ID: $scope.SC.id,
                MEDICAL_ORDER_EXISTS:$scope.MOE
            };
        } else {
            if ($scope.ss == "option2s") {
                PZ = {
                    ZAEZD_BEGIN: $scope.ZB,
                    ZAEZD_END: $scope.ZE,
                    MEDICAL_ORDER: $scope.MO,
                    FULL_NAME: $scope.FN,
                    DEPARTMENT_CODE: $scope.INN,
                    POSITION: $scope.P,
                    QUANTITY: $scope.Q,
                    CAMP_ID: $scope.SC.id,
                    MEDICAL_ORDER_EXISTS: $scope.MOE
                };
            }
        }
        if ($scope.SC.length == 0 || $scope.INN.length == 0 || $scope.FN.length == 0 || $scope.P.length == 0 || $scope.Q.length == 0) {
            alert("Maydonlar to`ldirilmagan");
        }
        else {
            
            if (getGZAction == "изменить") {
                PZ.REC_ID = $scope.ID;
                $http({
                    method: "post",
                    url: "UpdatePodachaZayavok",
                    data: JSON.stringify(PZ),
                    dataType: "json"
                }).then(function (msg) {
                    if ($scope.ss === "option1s") {
                        GetPZ_S($scope.SCFilter.id);
                    } else {
                        if ($scope.ss === "option2s") {
                            GetPZ_C($scope.SCFilter.id);
                        }
                    }
                }, function () {
                    //alert('Error in UpdatePodachaZayavok');
                    console.log('Error in UpdatePodachaZayavok');
                });
            }
            else {
                $http({
                    method: "post",
                    url: "AddPodachaZayavok",
                    data: JSON.stringify(PZ),
                    dataType: "json"
                }).then(function (msg) {
                    if ($scope.ss === "option1s") {
                        GetPZ_S($scope.SCFilter.id);
                    } else {
                        if ($scope.ss === "option2s") {
                            GetPZ_C($scope.SCFilter.id);
                        }
                    }
                }, function () {
                    //alert('Error in AddPodachaZayavok');
                    console.log('Error in AddPodachaZayavok');
                });
            }
        }
    }
    $scope.OTPpz = function () {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Rostdan ham jo`natmoqchimisiz?", function (e) {
            if (e) {
                alertify.success("yuborilmoqda!!!");
                $http({
                    method: "post",
                    url: "OTPPodachaZayavok"
                }).then(function (msg) {
                    $scope.OTPdiv = true;
                    $scope.PZdiv = false;
                    if ($scope.ss === "option1s") {
                        GetPZ_S($scope.SCFilter.id);
                    } else {
                        if ($scope.ss === "option2s") {
                            GetPZ_C($scope.SCFilter.id);
                        }
                    }
                    document.getElementById('connection').style.visibility = "hidden";
                    $("#hh").modal('hide');
                }, function () {
                    //alert('Error in OTPPodachaZayavok');
                    console.log('Error in OTPPodachaZayavok');
                });
            } else {
                document.getElementById('connection').style.visibility = "hidden";
                $("#hh").modal('hide');
                alertify.error("yuborish bekor qilindi");
            }
        });
        return false;
    }
    $scope.deletePZ = function (ID) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Rostdan ham o`chirmoqchimisiz?", function (e) {
            if (e) {
                alertify.success("o`chirilmoqda!!!");
                $http({
                    method: "post",
                    url: "DeletePodachaZayavok",
                    params: {
                        id: JSON.stringify(ID)
                    }
                }).then(function (msg) {
                    if ($scope.ss === "option1s") {
                        GetPZ_S($scope.SCFilter.id);
                    } else {
                        if ($scope.ss === "option2s") {
                            GetPZ_C($scope.SCFilter.id);
                        }
                    }
                    document.getElementById('connection').style.visibility = "hidden";
                    $("#hh").modal('hide');
                }, function () {
                    //alert('Error in DeletePodachaZayavok');
                    console.log('Error in DeletePodachaZayavok');
                });
            } else {
                document.getElementById('connection').style.visibility = "hidden";
                $("#hh").modal('hide');
                alertify.error("o`chirish bekor qilindi");
            }
        });
        return false;
        
    }
    function ClearFields() {
        var zb = $filter('date')(new Date(), 'dd.MM.yyyy');
        var ze = $filter('date')(new Date(), 'dd.MM.yyyy');
        $scope.ZB = new Date(zb.substring(6, 10), zb.substring(3, 5) - 1, zb.substring(0, 2));
        $scope.ZE = new Date(ze.substring(6, 10), ze.substring(3, 5) - 1, ze.substring(0, 2));
        $scope.SC.id = 1;
        $scope.P = "";
        $scope.INN = "";
        $scope.FN = "";
        $scope.MO = "";
    }
    $scope.Cancel = function () {

    };
}]);