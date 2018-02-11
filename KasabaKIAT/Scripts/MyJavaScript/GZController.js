app.controller("GZController", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    $scope.PlaceType = {
        REC_ID: '1',
        NAME_UZ: ''
    };
    $scope.PlaceTypeFilter = {
        REC_ID: '1',
        NAME_UZ: ''
    };
    $scope.SC = {
        id: '1',
        value: ''
    };
    $scope.GZdiv = true;
    Load_S();
    LoadPT();
    $scope.PlaceTypeFilter.REC_ID = 1;
    getGZ_S($scope.SC.id, $scope.PlaceTypeFilter.REC_ID);
    function LoadPT() {
        $http({
            method: 'get',
            url: '/Home/Getdictionary'
        }).then(function (response) {
            $scope.D = response.data;
        }, function (error) {
            console.log(error, 'can not get data.');
        });
    }
    $scope.LoadPlaceType = function () {
        LoadPT();
    }
    function Load_S() {
        $scope.myVar = "Санаторий";
        $scope.s = "option1";
        $http({
            method: 'get',
            url: '/Home/GetSanatory'
        }).then(function (response) {
            $scope.CS = response.data;
        }, function (error) {
            console.log(error, 'GetSanatory');
        });
    }
    function Load_C() {
        $http({
            method: 'get',
            url: '/Home/GetCamp'
        }).then(function (response) {
            $scope.CS = response.data;
        }, function (error) {
            console.log(error, 'GetCamp');
        });
    }
   //$scope.GZdiv=false;
   function getGZ_S(id,pt) {
        $http({
            method: "post",
            url: "/GrafikZaezdov/GetAllGrafikZaezdov_S?id=" + id+"&pt="+pt,
        }).then(function (gz) {
            $scope.GZ = gz.data;
            if (gz.data.length != 0) {
                $scope.GZdiv = true;
            }
        }, function () {
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
            //alert('Error in GetAllGrafikZaezdov_S');
            console.log('Error in GetAllGrafikZaezdov_S');
        });
    }
    function getGZ_C(id,pt) {
        $http({
            method: "post",
            url: "/GrafikZaezdov/GetAllGrafikZaezdov_C?id="+id+"&pt="+pt,
        }).then(function (gz) {
            $scope.GZ = gz.data;
            if (gz.data.length != 0) {
                $scope.GZdiv = true;
            }
        }, function () {
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
            //alert('Error in GetAllGrafikZaezdov_C');
            console.log('Error in GetAllGrafikZaezdov_C');
        });
    }
    //To Get all GetAllGrafikZaezdov records  
    $scope.GetCS = function () {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        //debugger;
        if ($scope.s == "option1") {
            $scope.myVar = "Санаторий";
            Load_S();
        }
        else {
            if ($scope.s == "option2") {
                $scope.myVar = "Дет/лагер";
                Load_C();
            }
        }
        document.getElementById('connection').style.visibility = "hidden";
        $("#hh").modal('hide');
    }
    $scope.GetDATA = function () {
        if ($scope.s == "option1") {
            getGZ_S($scope.SC.id,$scope.PlaceTypeFilter.REC_ID);
        }
        else
        {
            if ($scope.s == "option2") {
                getGZ_C($scope.SC.id, $scope.PlaceTypeFilter.REC_ID);
            }
        }
    }
    $scope.editGZ = function (gz) {
        $scope.ID = gz.ID;
        var zb = (gz.ZB) ? $filter('date')(parseInt(gz.ZB.substr(6)), 'dd.MM.yyyy') : '';
        var ze = (gz.ZE) ? $filter('date')(parseInt(gz.ZE.substr(6)), 'dd.MM.yyyy') : '';
        $scope.P = gz.P;
        $scope.Q = gz.Q;
        $scope.PlaceType.NAME_UZ = gz.PTI;
        $scope.PlaceType.REC_ID = gz.PTI_ID;
        $scope.Action = "Изменить";
        $scope.ZB = new Date(zb.substring(6, 10), zb.substring(3, 5) - 1, zb.substring(0, 2));
        $scope.ZE = new Date(ze.substring(6, 10), ze.substring(3, 5) - 1, ze.substring(0, 2));
    }
    $scope.AddUpdateGZ = function () {
        var GZ;
        if ($scope.s == "option1") {
            GZ = {
                ZAEZD_BEGIN: $scope.ZB,
                ZAEZD_END: $scope.ZE,
                PRICE: $scope.P,
                QUANTITY: $scope.Q,
                SANATORY_ID: $scope.SC.id,
                PLACE_TYPE_ID: $scope.PlaceType.REC_ID
            };
        } else {
            if ($scope.s == "option2") {
                GZ = {
                    ZAEZD_BEGIN: $scope.ZB,
                    ZAEZD_END: $scope.ZE,
                    PRICE: $scope.P,
                    QUANTITY: $scope.Q,
                    CAMP_ID: $scope.SC.id,
                    PLACE_TYPE_ID: $scope.PlaceType.REC_ID
                };
            }
        }
        var getGZAction = $scope.Action;

        if (getGZAction == "Изменить") {
            GZ.REC_ID = $scope.ID;
            //alert("ID : " + GZ.REC_ID + " ZAEZD_BEGIN : " + GZ.ZAEZD_BEGIN + " ZAEZD_END : " + GZ.ZAEZD_END + " PRICE : " + GZ.PRICE + " QUANTITY : " + GZ.QUANTITY + " SANATORY_ID : " + GZ.SANATORY_ID + " CAMP_ID : " + GZ.CAMP_ID + " PLACE_TYPE_ID : " + GZ.PLACE_TYPE_ID);
            $http({
                method: "post",
                url: "/GrafikZaezdov/UpdateGrafikZaezdov",
                data: JSON.stringify(GZ),
                dataType: "json"
            }).then(function (msg) {
                if ($scope.s == "option1") {
                    getGZ_S($scope.SC.id, $scope.PlaceTypeFilter.REC_ID);
                }
                else {
                    if ($scope.s == "option2") {
                        getGZ_C($scope.SC.id, $scope.PlaceTypeFilter.REC_ID);
                    }
                }
            }, function () {
                document.getElementById('connection').style.visibility = "hidden";
                $("#hh").modal('hide');
                //alert('Error in UpdateGrafikZaezdov');
                console.log('Error in UpdateGrafikZaezdov');
            });
        } else {
            //alert("ZAEZD_BEGIN : " + GZ.ZAEZD_BEGIN + " ZAEZD_END : " + GZ.ZAEZD_END + " PRICE : " + GZ.PRICE + " QUANTITY : " + GZ.QUANTITY + " SANATORY_ID : " + GZ.SANATORY_ID + " CAMP_ID : " + GZ.CAMP_ID + " PLACE_TYPE_ID : " + GZ.PLACE_TYPE_ID);
            $http({
                method: "post",
                url: "/GrafikZaezdov/AddGrafikZaezdov",
                data: JSON.stringify(GZ),
                dataType: "json"
            }).then(function (msg) {
                if ($scope.s == "option1") {
                    getGZ_S($scope.SC.id, $scope.PlaceTypeFilter.REC_ID);
                }
                else {
                    if ($scope.s == "option2") {
                        getGZ_C($scope.SC.id, $scope.PlaceTypeFilter.REC_ID);
                    }
                }
            }, function () {
                document.getElementById('connection').style.visibility = "hidden";
                $("#hh").modal('hide');
                //alert('Error in AddGrafikZaezdov');
                console.log('Error in AddGrafikZaezdov');
            });
        }
    }
    $scope.AddGZDiv = function () {
        ClearFields();
        $scope.Action = "Добавить";
    }
    $scope.deleteGZ = function (ID) {
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите удалить?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                $http({
                    method: "post",
                    url: "/GrafikZaezdov/DeleteGrafikZaezdov",
                    params: {
                        bookId: JSON.stringify(ID)
                    }
                }).then(function (msg) {
                    if ($scope.s == "option1") {
                        getGZ_S($scope.SC.id, $scope.PlaceTypeFilter.REC_ID);
                    }
                    else {
                        if ($scope.s == "option2") {
                            getGZ_C($scope.SC.id, $scope.PlaceTypeFilter.REC_ID);
                        }
                    }
                }, function () {
                    //alert('Error in DeleteGrafikZaezdov');
                    console.log('Error in DeleteGrafikZaezdov');
                });    
            } else {
                alertify.error("Операция отменена");
            }
        });
        return false;
    }
    function ClearFields() {
        $scope.ID="";
        $scope.ZB = $filter('date')(new Date(), 'dd.MM.yyyy');
        $scope.ZE = $filter('date')(new Date(), 'dd.MM.yyyy');
        $scope.P = "";
        $scope.Q = "";
        //$scope.PTI = "";
    }
    $scope.Cancel = function () {
       
    };
}]);