app.controller("UZController", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    GetUZ();
    GetRoles();
    GetRegion();
    GetOtrasl();
    GetOrg();
    function GetUZ() {
        $http({
            method: "post",
            url: "GetUZ",
        }).then(function (d) {
            $scope.UZ = d.data;
        }, function () {
            //alert("error in UZ");
        });
    }
    function GetRoles() {
        $http({
            method: "post",
            url: "GetRoles",
        }).then(function (d) {
            $scope.Rl = d.data;
        }, function () {
            //alert("error in UZ");
        });
    };
    function GetRegion() {
        $http({
            method: "post",
            url: "GetRegion",
        }).then(function (d) {
            $scope.Rg = d.data;
        }, function () {
            //alert("error in UZ");
        });
    };
    function GetOtrasl() {
        $http({
            method: "post",
            url: "GetOtrasl",
        }).then(function (d) {
            $scope.Ot = d.data;
        }, function () {
            //alert("error in UZ");
        });
    };
    function GetOrg() {
        $http({
            method: "post",
            url: "GelOrganizatsiya",
        }).then(function (d) {
            $scope.O = d.data;
        }, function () {
            //alert("error in UZ");
        });
    };
    $scope.Roles = {
        id: '1',
        value: ''
    };
    $scope.Region = {
        id: 1726,
        value: ''
    };
    $scope.Ots = {
        id: '1',
        value: ''
    };
    $scope.Or = {
        id: '1',
        value: ''
    };
    $scope.kurish = function () {
        $scope.parol = "text";
        if ($scope.ch == false) {
            $scope.ch = true;
            $scope.parol = "text";
        }
        else {
            $scope.ch = false;
            $scope.parol = "password";
        }
    };
    $scope.AddGPDiv = function () {
        $scope.parol = "password";
        $scope.ch = false;
        ClearFields();
        $scope.Action = "Добавить";
    }
    function ClearFields() {
        $scope.ID = 0;
        $scope.Region.id = 1726;
        $scope.Roles.id = 1;
        $scope.Ots.id = 1;
        $scope.Or.id = 1;
        $scope.L = "";
        $scope.P = "";
        $scope.PP = "";
        $scope.N = "";
    }
    $scope.edit = function (uz) {
        $scope.parol = "password";
        $scope.ch = false;
        $scope.ID = uz.ID;
        $scope.Region.id = uz.Rg_id;
        $scope.Region.value = uz.Rg;
        $scope.Roles.id = uz.Rl_id;
        $scope.Roles.value = uz.Rl;
        $scope.Ots.id = uz.Ot_id;
        $scope.Ots.value = uz.Ot;
        $scope.Or.id = uz.O_id;
        $scope.Or.value = uz.O;
        $scope.L = uz.L;
        $scope.P = uz.P;
        $scope.PP = uz.P;
        $scope.N = uz.N;
        $scope.Action = "изменить";
    }
    $scope.AddUpdateGP = function () {
        var getGPAction = $scope.Action;
        var GP={
            email: $scope.L,
            password: $scope.P,
            user_name: $scope.N,
            bkut_id: $scope.Or.id,
            role_id:$scope.Roles.id,
            REGION_ID: $scope.Region.id,
            BRANCH_ID: $scope.Ots.id,
            };
        if (getGPAction == "изменить") {
            GP.id = $scope.ID;
            $http({
                method: "post",
                url: "/Administirirovaniye/UpdateUZ",
                data: JSON.stringify(GP),
                dataType: "json"
            }).then(function (msg) {
                GetUZ();
            }, function () {
                //alert('Error in UpdateGodovoyPlan');
                console.log('Error in UpdateUZ');
                $("#hh").modal('hide');
            });
        }
        else {
            $http({
                method: "post",
                url: "/Administirirovaniye/AddUZ",
                data: JSON.stringify(GP),
                dataType: "json"
            }).then(function (msg) {
                GetUZ();
            }, function () {
                //alert('Error in AddGodovoyPlan');
                console.log('Error in AddUZ');
                $("#hh").modal('hide');
            });
        }
    }
    $scope.deleteGP = function (ID) {
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
                    url: "/Administirirovaniye/DeleteUZ",
                    params: {
                        id: JSON.stringify(ID)
                    }
                }).then(function (msg) {
                    $("#hh").modal('hide');
                    GetUZ();
                }, function () {
                    //alert('Error DeleteGodovoyPlan');
                    console.log('Error DeleteUZ');
                    $("#hh").modal('hide');
                });
            } else {
                $("#hh").modal('hide');
                alertify.error("o`chirish bekor qilindi");
            }
        });
        return false;
    }
}]);