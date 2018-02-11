app.controller("SSPOController", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    $scope.PlaceType = {
        REC_ID: '1',
        NAME_UZ: ''
    };
    $scope.PlaceTypeFilter = {
        REC_ID: '1',
        NAME_UZ: ''
    };
    $scope.PlaceTypeFilter.REC_ID = 1;
    GetSSPO();
    function GetSSPO() {
        $http({
            method: "post",
            url: "/Administirirovaniye/GetSSPO",
        }).then(function (d) {
            $scope.SSPO = d.data;
        }, function () {
            //alert("error in STRS");
        });
    }
    $scope.editSSPO = function (pz) {
        $scope.ID = pz.REC_ID;
        $scope.PTI = pz.NAME_UZ;
        $scope.Action = "Изменить";
    }
    $scope.AddSSPODiv = function () {
        $scope.PTI = "";
        $scope.Action = "Добавить";
    }
    $scope.AddUpdateSSPO = function () {
        var getGZAction = $scope.Action;
        var PZ = {
            NAME_RU: $scope.PTI,
            NAME_UZ: $scope.PTI,
            NAME_LT: $scope.PTI,
        };
        if (getGZAction == "Изменить") {
            PZ.REC_ID = $scope.ID;
            $http({
                method: "post",
                url: "/Administirirovaniye/UpdateSTRS",
                data: JSON.stringify(PZ),
                dataType: "json"
            }).then(function (msg) {
                GetSSPO();
            }, function () {
                //alert('Error in updating');
            });
        }
        else {
            $http({
                method: "post",
                url: "/Administirirovaniye/AddSSPO",
                data: JSON.stringify(PZ),
                dataType: "json"
            }).then(function (msg) {
                GetSSPO();
            }, function () {
                //alert('Error in adding book record');
            });
        }
    }
    $scope.deleteSSPO = function (ID) {
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите удалить?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                $http({
                    method: "post",
                    url: "/Administirirovaniye/DeleteSTRS",
                    params: {
                        id: JSON.stringify(ID)
                    }
                }).then(function (msg) {
                    GetSSPO();
                }, function () {
                    //alert('Error in deleting book record');
                });
            } else {
                alertify.error("Операция отменена");
            }
        });
        return false;
    }
}]);