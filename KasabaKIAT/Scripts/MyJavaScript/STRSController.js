app.controller("STRSController", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
     $scope.PlaceType = {
        REC_ID: '1',
        NAME_UZ: ''
     };
     $scope.PlaceTypeFilter = {
         REC_ID: '1',
         NAME_UZ: ''
     };
     $scope.PlaceTypeFilter.REC_ID = 1;
     GetSTRS();
     $scope.showdelete = false;
     function GetSTRS() {
        $http({
            method: "post",
            url: "/home/Getdictionary",
        }).then(function (d) {
            $scope.STRS = d.data;
        }, function () {
            alertify.error("ошибка");
        });
    }
     $scope.editSTRS = function (pz) {
         $scope.showdelete = true;
         $scope.ID = pz.REC_ID;
        $scope.PTI= pz.NAME_UZ;
        $scope.Action = "Изменить";
    }
     $scope.AddSTRSDiv = function () {
         $scope.showdelete = false;
         $scope.PTI="";
        $scope.Action = "Добавить";
    }
     $scope.AddUpdateSTRS = function () {
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
                GetSTRS();
            }, function () {
                alertify.error("ошибка");
            });
        }
        else {
            $http({
                method: "post",
                url: "/Administirirovaniye/AddSTRS",
                data: JSON.stringify(PZ),
                dataType: "json"
            }).then(function (msg) {
                GetSTRS();
            }, function () {
                alertify.error("ошибка");
            });
        }
     }
     $scope.deleteSTRS = function (ID) {
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
                     GetSTRS();
                 }, function () {
                     alertify.error("ошибка");
                 });
             } else {
                 alertify.error("Операция отменена");
             }
         });
         return false;
     }
}]);