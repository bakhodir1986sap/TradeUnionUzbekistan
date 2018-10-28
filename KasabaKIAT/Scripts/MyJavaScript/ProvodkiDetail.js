app.controller("RazbivkaPostupleniya", ["$scope", "$http", "$filter", "$locale", "DataService", "CalcService", function ($scope, $http, $filter, $locale, DataService, CalcService) {
    AllRegion();
    AllOtrasl();
    AllOplataTip();
    $scope.showdata = false;
    $scope.ZB = new Date
    $scope.ZE = new Date();
    $scope.LoadDATA = function () {
        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";
        LoadDATA($scope.ZB, $scope.ZE);
    }
    $scope.LoadDATA_N = function () {
        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";
        LoadDATA_N($scope.TipOplati.id, $scope.ZB);
    }

    function LoadDATA(bg,end) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'POST',
            url: "/Otchot/GetProvodkiDetail?bg=" + bg.toISOString() + "&end=" + end.toISOString(),
        }).then(function (d) {
            if (d.data.length != 0) {
                $scope.q = d.data.length;
                $scope.showdata = true;
            } else {
                $scope.showdata = false;
            }
            $scope.DATA = d.data;
            console.log(d.data);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetPostupleniya -> ", error);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function LoadDATA_N(s, bg) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'POST',
            url: "/Otchot/GetRazbivkaPostupleniyaN?payType=" + s + "&bg=" + bg.toISOString() ,
        }).then(function (d) {
            if (d.data.length != 0) {
                $scope.q = d.data.length;
                $scope.showdata = true;
            } else {
                $scope.showdata = false;
            }
            $scope.DATA = d.data;
            console.log(d.data);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetPostupleniya -> ", error);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.Region = {
        id: 0,
        value: ''
    };
    $scope.Otrasl = {
        id: '0',
        value: ''
    };
    $scope.TipOplati = {
        id: '700',
        value: '15 %'
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

    function AllOplataTip() {
        $scope.OplataTips = [
            { id: '700', value: '15 %' },
            { id: '777', value: '25 %' },
            { id: '600', value: '15 % Фермер' }
        ];
    }


    $scope.Clear = function () {
        $scope.Region.id = 0;
        $scope.Otrasl.id = '0';
        $scope.ZB = new Date
        $scope.ZE = new Date();
        $scope.showdata = false;
    }

    $scope.CancelProvodka = function (rec_id) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'POST',
            url: "/Otchot/CancelProvodki?rec_id=" + rec_id,
        }).then(function (d) {

            alertify.success("Удачно удалена");
            $scope.LoadDATA();
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetPostupleniya -> ", error);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    };



    $scope.exportData = function () {
        var blob = new Blob([document.getElementById('export').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Разбивка поступления.xls");
    };
}]);