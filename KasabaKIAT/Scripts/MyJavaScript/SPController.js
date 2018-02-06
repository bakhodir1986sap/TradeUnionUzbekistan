app.controller("SPController", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    GetSP();
    function GetSP() {
        $http({
            method: "post",
            url: "/Administirirovaniye/GetSP",
        }).then(function (d) {
            $scope.SP = d.data;
        }, function () {
            //alert("error in SP");
        });
    }
    $scope.editSP = function (pz) {
        $scope.ID = pz.ID;
        $scope.INN = pz.INN;
        $scope.O = pz.O;
        $scope.D = pz.D;
        $scope.SB = pz.SB;
        $scope.SE = pz.SE;
        $scope.Action = "изменить";
    }
    $scope.AddSPDiv = function () {
        $scope.ID = "";
        $scope.INN = "";
        $scope.O = "";
        $scope.D = "";
        $scope.SB = "";
        $scope.SE = "";
        $scope.Action = "добавить";
    }
    $scope.AddUpdateSP = function () {
        var getGZAction = $scope.Action;
        var PZ = {
            DEPARTMENT_BASE: $scope.INN,
            description:$scope.D,
            score_begin: $scope.SB,
            score_end: $scope.SE
        };
        if (getGZAction == "изменить") {
            PZ.ID = $scope.ID;
            $http({
                method: "post",
                url: "/Administirirovaniye/UpdateSP",
                data: JSON.stringify(PZ),
                dataType: "json"
            }).then(function (msg) {
                GetSP();
            }, function () {
                //alert('Error in updating');
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
                //alert('Error in adding book record');
            });
        }
    }
    $scope.deleteSP = function (ID) {
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Rostdan ham o`chirmoqchimisiz?", function (e) {
            if (e) {
                alertify.success("o`chirilmoqda!!!");
                $http({
                    method: "post",
                    url: "/Administirirovaniye/DeleteSP",
                    params: {
                        id: JSON.stringify(ID)
                    }
                }).then(function (msg) {
                    GetSP();
                }, function () {
                    //alert('Error in deleting book record');
                });
            } else {
                alertify.error("o`chirish bekor qilindi");
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