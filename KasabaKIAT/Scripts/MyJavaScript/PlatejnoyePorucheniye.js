app.controller("PlatejnoyePorucheniye", ["$scope", "$http", "$filter", "DataService", "CalcService", function ($scope, $http, $filter, DataService, CalcService) {
    LoadDATA();
    $scope.showdata = false;
    $scope.showdata2 = false;
    
    function LoadDATA() {
        $http({
            method: 'POST',
            url: "/PlatejnoyePorucheniye/GetPlatejnoyePorucheniye",
        }).then(function (d) {
            if (d.data.length != 0) {
                $scope.showdata = true;
            } else {
                $scope.showdata = false;
            }
            $scope.DATA = d.data;
            console.log(d.data);
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetPlatejnoyePorucheniye -> ", error);
            $("#hh").modal('hide');
        });
    }
    $scope.novoe = function () {
        //$scope.PD = $filter('date')(new Date(), 'dd.MM.yyyy');
        $scope.PD = new Date();
        $scope.N = "";
        $scope.P1NAME = "";
        $scope.P1INN = "";
        $scope.P1SCH = "";
        $scope.P1BN = "";
        $scope.P1MFO = "";
        $scope.P2NAME = "";
        $scope.P2INN = "";
        $scope.P2SCH = "";
        $scope.P2BN = "";
        $scope.P2MFO = "";
        $scope.SUM = "";
        $scope.COM = "";
        $scope.R = "";
        $scope.GB = "";
    };
    $scope.Add = function () {
        var GZ = {
            plat_date: $scope.PD,
            plat_num: $scope.N,
            platelname: $scope.P1NAME,
            platelinn: $scope.P1INN,
            platelraschet: $scope.P1SCH,
            platelbankname: $scope.P1BN,
            platelbankmfo: $scope.P1MFO,
            poluchname: $scope.P2NAME,
            poluchinn: $scope.P2INN,
            poluchraschet: $scope.P2SCH,
            poluchbankname: $scope.P2BN,
            poluchbankmfo: $scope.P2MFO,
            summa: $scope.SUM,
            comment: $scope.COM,
            rukovoditel: $scope.R,
            glavbux: $scope.GB,

        };

        $http({
            method: "post",
            url: "/PlatejnoyePorucheniye/EnterPlatejnoyePorucheniye",
            data: JSON.stringify(GZ),
            dataType: "json"
        }).then(function (msg) {
            LoadDATA();
            //alertify.success(msg.data);
        }, function () {
            $("#hh").modal('hide');
            alertify.error("ошибка");
            console.log('Error in EnterPlatejnoyePorucheniye');
        });
    };
}]);