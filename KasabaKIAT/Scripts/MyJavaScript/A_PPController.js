app.controller("A_PPController", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    GetPP(14, 1726);
    AllRegion();
    AllPlaceType();
    $scope.showdelete = false;
    function GetPP(o,r) {
        $http({
            method: "post",
            url: "/Administirirovaniye/GetPP?o_id="+o+"&r_id="+r,
        }).then(function (d) {
            $scope.PP = d.data;
        }, function () {
            //alert("error in SP");
        });
    }
    $scope.PlaceType = {
        REC_ID: '14',
        NAME_UZ: ''
    };
    $scope.Region = {
        id: 1726,
        value: ''
    };
    $scope.LoadDATA = function () {
        GetPP($scope.PlaceType.REC_ID, $scope.Region.id);
    }
    function AllPlaceType() {
        $http({
            method: 'get',
            url: '/Administirirovaniye/GetSPP'
        }).then(function (response) {
            $scope.Dc = response.data;
        }, function (error) {
            //alert("error in Getdictionary");
            console.log("error in GetSPP");
        });
    }
    function AllRegion() {
        $http({
            method: 'get',
            url: '/Administirirovaniye/GetRegion'
        }).then(function (response) {
            $scope.R = response.data;
        }, function (error) {
            //alert("error in GetRegion");
            console.log("error in GetRegion");
        });
    }
    $scope.editPP = function (pz) {
        $scope.showdelete = true;
        $scope.ID = pz.ID;
        $scope.INN = pz.INN;
        $scope.Region.id = pz.R_id;
        $scope.Region.value = pz.R;
        $scope.PlaceType.REC_ID = pz.JO_id;
        $scope.PlaceType.NAME_UZ = pz.JO;
        $scope.O = pz.O;
        $scope.D = pz.D;
        $scope.SB = pz.SB;
        $scope.SE = pz.SE;
        $scope.Action = "Изменить";
    }
    $scope.AddPPDiv = function () {
        $scope.showdelete = false;
        $scope.ID = "";
        $scope.INN = "";
        $scope.O = "";
        $scope.D = "";
        $scope.SB = "";
        $scope.SE = "";
        $scope.Action = "Добавить";
    }
    $scope.AddUpdatePP = function () {
        var getGZAction = $scope.Action;
        var PZ = {
            DEPARTMENT_BASE: $scope.INN,
            description: $scope.D,
            score_begin: $scope.SB,
            score_end: $scope.SE,
            order: $scope.PlaceType.REC_ID,
            REGION_ID: $scope.Region.id
        };
        if (getGZAction == "Изменить") {
            PZ.ID = $scope.ID;
            $http({
                method: "post",
                url: "/Administirirovaniye/UpdatePP",
                data: JSON.stringify(PZ),
                dataType: "json"
            }).then(function (msg) {
                GetPP($scope.PlaceType.REC_ID, $scope.Region.id);
            }, function () {
                //alert('Error in updating');
            });
        }
        else {
            $http({
                method: "post",
                url: "/Administirirovaniye/AddPP",
                data: JSON.stringify(PZ),
                dataType: "json"
            }).then(function (msg) {
                GetPP($scope.PlaceType.REC_ID, $scope.Region.id);
            }, function () {
                //alert('Error in adding book record');
            });
        }
    }
    $scope.deletePP = function (ID) {
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите удалить?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                $http({
                    method: "post",
                    url: "/Administirirovaniye/DeletePP",
                    params: {
                        id: JSON.stringify(ID)
                    }
                }).then(function (msg) {
                    GetPP($scope.PlaceType.REC_ID, $scope.Region.id);
                }, function () {
                    //alert('Error in deleting book record');
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
                    console.log(error, 'error in LoadINN');
                });
            }
        }
        catch (err) {
            $scope.O = "";
        }
    }
}]);