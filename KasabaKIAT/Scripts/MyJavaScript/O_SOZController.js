app.controller("O_SOZController", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    AllRegion();
    AllOtrasl();
    $scope.showdata = false;
    $scope.ZB = new Date
    $scope.ZE = new Date();
    function GetData(Bdt, Edt, s, b) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: "post",
            url: "/Otchot/GetSvodniyOtchot?Begindt=" + Bdt.toISOString() + "&Enddt=" + Edt.toISOString() + "&Soato=" + s + "&Branch=" + b,
        }).then(function (d) {
            $scope.O_SOZ = d.data;
            if (d.data.length != 0) {
                $scope.showdata = true;
            }
            else {
                $scope.showdata = false;
            }
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function () {
            alertify.error("ошибка");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    $scope.Region = {
        id: 1726,
        value: ''
    };
    $scope.Otrasl = {
        id: 'А1',
        value: ''
    };
    function AllRegion() {
        $http({
            method: 'get',
            url: '/Otchot/GetRegion'
        }).then(function (response) {
            $scope.R = response.data;
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRegion");
        });
    }
    function AllOtrasl() {
        $http({
            method: 'get',
            url: '/Otchot/GetOtrasl'
        }).then(function (response) {
            $scope.O = response.data;
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetOtrasl");
        });
    }
    $scope.LoadDATA = function () {
        //var zb = ($scope.ZB) ? $filter('date')(parseInt($scope.ZB.substr(6)), 'dd.MM.yyyy') : '';
        //var ze = ($scope.ZE) ? $filter('date')(parseInt($scope.ZE.substr(6)), 'dd.MM.yyyy') : '';
        //$scope.ZB = new Date(zb.substring(6, 10), zb.substring(3, 5) - 1, zb.substring(0, 2));
        //$scope.ZE = new Date(ze.substring(6, 10), ze.substring(3, 5) - 1, ze.substring(0, 2));
        GetData($scope.ZB,$scope.ZE,$scope.Region.id, $scope.Otrasl.id);
    };
    $scope.Clear = function () {
        $scope.Region.id = 1726;
        $scope.Otrasl.id = 'А1';
        $scope.ZB = new Date
        $scope.ZE = new Date();
    }
}]);