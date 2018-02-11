app.controller("SSSPController", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    GetData();
    function GetData() {
        $http({
            method: "post",
            url: "/Administirirovaniye/GetSSSP",
        }).then(function (d) {
            $scope.SSSP = d.data;
        }, function () {
            //alert("error in UZ");
        });
    }
    $scope.editSSSP = function (pz) {
        var zb = (pz.ST) ? $filter('date')(parseInt(pz.ST.substr(6)), 'dd.MM.yyyy') : '';
        $scope.ST = new Date(zb.substring(6, 10), zb.substring(3, 5) - 1, zb.substring(0, 2));
        $scope.ID = pz.ID;
        $scope.A = pz.A;
        $scope.Action = "Изменить";
    }
    $scope.AddSSSPDiv = function () {
        $scope.A ="";
        $scope.ST = $filter('date')(new Date(), 'dd.MM.yyyy');
        $scope.Action = "Добавить";
    }
    $scope.AddUpdateSSSP = function () {
        var getGZAction = $scope.Action;
        var PZ = {
            SET_DATE: $scope.ST,
            AMOUNT: $scope.A,
        };
        if (getGZAction == "Изменить") {
            PZ.REC_ID = $scope.ID;
            $http({
                method: "post",
                url: "/Administirirovaniye/UpdateSSSP",
                data: JSON.stringify(PZ),
                dataType: "json"
            }).then(function (msg) {
                GetData();
            }, function () {
                //alert('Error in updating');
            });
        }
        else {
            $http({
                method: "post",
                url: "/Administirirovaniye/AddSSSP",
                data: JSON.stringify(PZ),
                dataType: "json"
            }).then(function (msg) {
                GetData();
            }, function () {
                //alert('Error in adding book record');
            });
        }
    }
    $scope.deleteSSSP = function (ID) {
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите удалить?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                $http({
                    method: "post",
                    url: "/Administirirovaniye/DeleteSSSP",
                    params: {
                        id: JSON.stringify(ID)
                    }
                }).then(function (msg) {
                    GetData();
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