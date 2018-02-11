app.controller("SchetFaktura", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    $scope.showdata = false;
    $scope.ZB = new Date();
    $scope.ZE = new Date();
    All();
    GetData($scope.ZB, $scope.ZE, "000000039");
    function GetData(b,e,p) {
        $http({
            method: "post",
            url: "/Administirirovaniye/GetSchetFaktura?B=" + b.toISOString() + "&E=" + e.toISOString() + "&p=" + p,
        }).then(function (d) {
            $scope.DATA = d.data;
            if (d.data.length != 0) {
                $scope.showdata = true;
            }
            else {
                $scope.showdata = false;
            }
        }, function () {
            //alert("error in O_SOZ");
        });
    }
    function Load(b, e, p) {
        $http({
            method: "post",
            url: "/Administirirovaniye/ServiceSchetFaktura?B=" + b.toISOString() + "&E=" + e.toISOString() + "&p=" + p,
        }).then(function (d) {
            document.getElementById('connection').style.visibility = "hidden";
            GetData($scope.ZB, $scope.ZE, $scope.SP.d1);
        }, function () {
            //alert("error in O_SOZ");
        });
    }
    $scope.LoadStrokaSchetFakturi = function (id) {
        $http({
            method: "post",
            url: "/Administirirovaniye/GetStrokaSchetFakturi?id=" + id,
        }).then(function (d) {
            document.getElementById('connection').style.visibility = "hidden";
            GetData($scope.ZB, $scope.ZE, $scope.SP.d1);
        }, function () {
            //alert("error in O_SOZ");
        });
    }
    $scope.LoadDATA = function () {
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите ...?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                document.getElementById('connection').style.visibility = "visible";
                Load($scope.ZB, $scope.ZE, $scope.SP.d1);
            } else {
                alertify.error("Операция отменена");
            }
        });
        return false;
    };
    $scope.SP = {
        d1: '000000039',
        d2: ''
    };
    function All() {
        $scope.s = "option1";
        $http({
            method: 'get',
            url: '/Administirirovaniye/GetSpisokPravsoyuzov'
        }).then(function (response) {
            $scope.R = response.data;
        }, function (error) {
            //alert("error in GetRegion");
            console.log("error in GetRegion");
        });
    }
}]);