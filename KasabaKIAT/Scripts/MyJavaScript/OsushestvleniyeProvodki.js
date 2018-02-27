app.controller("OsushestvleniyeProvodki", ["$scope", "$http", "$filter", "DataService", "CalcService", function ($scope, $http, $filter, DataService, CalcService) {
    $scope.TRDT = new Date();
    //$scope.TRDT = new Date(zb.substring(6, 10), zb.substring(3, 5) - 1, zb.substring(0, 2));
    $scope.s = "option1";
    $scope.o = "option1";
    var s = false;
    var o = false;
    $scope.changeV = function () {

    }
    $scope.changeS = function () {
        if ($scope.s == 'option1') {
            s = false;
        } else {
            if ($scope.s == 'option2') {
                s = true;
            }
        }
    }
    $scope.changeO = function () {
        if ($scope.o == 'option1') {
            o = false;
        } else {
            if ($scope.o == 'option2') {
                o = true;
            }
        }
    }
   
   
    $scope.Add1 = function () {
        var GZ = {
            INOUTFLG: false,
            TRDT: $scope.TRDT,
            NUMBER: "",
            TIPSCHETA: s,
            TIPOBOROTA: o,
            PLATELNAME: "",
            PLATELINN: "",
            PLATELMFO: "",
            POLUCHNAME: $scope.P1NAME,
            POLUCHINN: $scope.P1INN,
            POLUCHMFO: $scope.P1MFO,
            PLATELRSCH: $scope.D1,
            POLUCHRSCH: $scope.K1,
            SUMMA: $scope.SUM1,
            COMMENT: $scope.COM1,

        };
       
        $http({
            method: "post",
            url: "/Buxgalter/EnterOsushestvleniyeProvodki",
            data: JSON.stringify(GZ),
            dataType: "json"
        }).then(function (msg) {
            alertify.success(msg.data);
        }, function () {
            $("#hh").modal('hide');
            alertify.error("ошибка");
            console.log('Error in EnterOsushestvleniyeProvodki');
        });
    }

    $scope.Add2 = function () {
        var GZ = {
            INOUTFLG: true,
            TRDT: $scope.TRDT,
            NUMBER: $scope.N,
            TIPSCHETA: s,
            TIPOBOROTA: o,
            PLATELNAME: $scope.P2NAME,
            PLATELINN: $scope.P2INN,
            PLATELMFO: $scope.P2MFO,
            POLUCHNAME: $scope.P3NAME,
            POLUCHINN: $scope.P3INN,
            POLUCHMFO: $scope.P3MFO,
            PLATELRSCH: $scope.D2,
            POLUCHRSCH: $scope.K2,
            SUMMA: $scope.SUM2,
            COMMENT: $scope.COM2,

        };
        $http({
            method: "post",
            url: "/Buxgalter/EnterOsushestvleniyeProvodki",
            data: JSON.stringify(GZ),
            dataType: "json"
        }).then(function (msg) {
            alertify.success(msg.data);
        }, function () {
            $("#hh").modal('hide');
            alertify.error("ошибка");
            console.log('Error in EnterOsushestvleniyeProvodki');
        });
    }
}]);