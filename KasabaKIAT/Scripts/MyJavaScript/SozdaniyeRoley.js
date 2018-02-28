app.controller("SozdaniyeRoley", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    $scope.showdata = false;
    GetData();
    V();
    $scope.R = {
        d1: 0,
        d2: ''
    };

    $scope.currentRoleSet = {

        roleId:0

    };

    $scope.MassiveAll =  {
        d1: 0,
        d2: ''
    };

    $scope.MassiveSelected = {
        d1: 0,
        d2: ''
    };

    function GetData() {
        $http({
            method: "post",
            url: "/Administirirovaniye/GetRoley",
        }).then(function (d) {
            $scope.DATA = d.data;
            if (d.data.length !== 0) {
                $scope.showdata = true;
            }
            else {
                $scope.showdata = false;
            }
        }, function () {
            alertify.error("ошибка");
        });
    }
    $scope.V2 = function (id) {
        $scope.currentRoleSet.roleId = id;
        $http({
            method: "post",
            url: "/Administirirovaniye/GetRoleys2?id="+id,
        }).then(function (d) {
            $scope.DATA3 = d.data;
            
        }, function () {
            alertify.error("ошибка");
        });
    }

    $scope.clickChangeTab = function () {
        angular.element('#second').trigger('click');
    };

    $scope.Print =  function Print()
    {

        console.log($scope.DATA2);
        console.log($scope.DATA3);
        console.log($scope.MassiveAll);
        console.log($scope.MassiveSelected);
    }

    function V() {
        $http({
            method: "post",
            url: "/Administirirovaniye/GetRoleys",
        }).then(function (d) {
            $scope.DATA2 = d.data;

        }, function () {
            alertify.error("ошибка");
        });
    }
}]);