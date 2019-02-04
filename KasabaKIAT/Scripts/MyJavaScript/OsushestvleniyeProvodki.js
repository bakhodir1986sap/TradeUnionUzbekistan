app.controller("OsushestvleniyeProvodki", ["$scope", "$http", "$filter", "DataService", "CalcService", function ($scope, $http, $filter, DataService, CalcService) {
    $scope.TRDT = new Date();
    GetSchetDebit();
    GetSchetCredit();
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

    $scope.GetDescription = function GetDescription () 
    {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Buxgalter/GetSummaDescriptionFor?amount=' + $scope.SUM1 + '&debit=' + $scope.D1 + '&credit=' + $scope.K1
        }).then(function (response) {

            $scope.SUM_OPISANIYE = response.data;
            $scope.COM1 = response.data.description;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            
            console.log("error in GetCurBusinessTime");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }



    function GetSchetDebit () {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Buxgalter/GetSchetDebit'
        }).then(function (response) {

            $scope.DebitList = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {

            console.log("error in GetCurBusinessTime");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }


    function GetSchetCredit () {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Buxgalter/GetSchetCredit'
        }).then(function (response) {

            $scope.CreditList = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {

            console.log("error in GetCurBusinessTime");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
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
            alertify.success("Сохранено");

            $scope.P1NAME = '';
            $scope.P1INN = '';
            $scope.P1MFO = '';
            $scope.SUM1 = 0;
            $scope.COM1 = '';
            $scope.SUM_OPISANIYE = '';

        }, function () {
            $("#hh").modal('hide');
            alertify.error('Ошибка в счетах дебита или кредита');
            console.log('Ошибка в счетах дебита или кредита');
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
            alertify.success("Сохранено");
        }, function () {
            $("#hh").modal('hide');
            alertify.error("ошибка");
            console.log('Error in EnterOsushestvleniyeProvodki');
        });
    }
}]);