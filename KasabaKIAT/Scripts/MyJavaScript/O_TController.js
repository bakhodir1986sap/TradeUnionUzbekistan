app.controller("O_TController", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    AllRegion();
    AllOtrasl();
    $scope.showdata = false;
    $scope.showexp = false;
    $scope.zb = "2018";
    $scope.showINN = true;//position:absolute
    $scope.Load = function () {
        if ($scope.s == "option1") {
            document.getElementById('INN').style = "";
            document.getElementById('Region').style = "position:absolute";
            document.getElementById('Otrasl').style = "position:absolute";
            document.getElementById('INN').style.visibility = "visible";
            document.getElementById('Region').style.visibility = "hidden";
            document.getElementById('Otrasl').style.visibility = "hidden";
            $scope.showINN = true;
            $scope.showRegion = false;
            $scope.showOtrasl = false;
        } else {
            if ($scope.s == "option2") {
                document.getElementById('Region').style.visibility = "visible";
                document.getElementById('Otrasl').style.visibility = "visible";
                document.getElementById('INN').style.visibility = "hidden";
                document.getElementById('Region').style = "";
                document.getElementById('Otrasl').style = "margin-left:100px";
                document.getElementById('INN').style = "position:absolute";
                $scope.showRegion = true;
                $scope.showOtrasl = true;
                $scope.showINN = false;
            } else {
                if ($scope.s == "option3") {
                    document.getElementById('Region').style = "";
                    document.getElementById('INN').style = "margin-left:100px";
                    document.getElementById('Otrasl').style = "position:absolute";
                    document.getElementById('INN').style.visibility = "visible";
                    document.getElementById('Region').style.visibility = "visible";
                    document.getElementById('Otrasl').style.visibility = "hidden";
                    $scope.showINN = true;
                    $scope.showRegion = true;
                    $scope.showOtrasl = false;
                }
            }
        }
    };
    $scope.LoadDATA = function () {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        if ($scope.s == "option1") {
            GetData_Korxona($scope.zb, $scope.INN);
        } else {
            if ($scope.s == "option2") {
                GetData_Tarmoq($scope.zb, $scope.Region.id, $scope.Otrasl.id);
            } else {
                if ($scope.s == "option3") {
                    GetData_Viloyat($scope.zb,$scope.Region.id);
                }
            }
        }
    };
    function GetData_Korxona(yil, inn) {
        $http({
            method: "post",
            url: "/Otchot/GetTalabnomlar_Korxona?yil=" + yil + "&inn=" + inn,
        }).then(function (d) {
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
            $scope.O_T = d.data;
            if (d.data.length != 0) {
                $scope.showdata = true;
            }
            else {
                $scope.showdata = false;
            }
        }, function () {
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
            alertify.error("ошибка");
        });
    }
    function GetData_Tarmoq(yil, s, b) {
        $http({
            method: "post",
            url: "/Otchot/GetTalabnomlar_Tarmoq?yil=" + yil + "&soato=" + s + "&branch=" + b,
        }).then(function (d) {
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
            $scope.O_T = d.data;
            if (d.data.length != 0) {
                $scope.showdata = true;
            }
            else {
                $scope.showdata = false;
            }
        }, function () {
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
            alertify.error("ошибка");
        });
    }
    function GetData_Viloyat(yil, s) {
        $http({
            method: "post",
            url: "/Otchot/GetTalabnomlar_Viloyat?yil=" + yil + "&soato=" + s,
        }).then(function (d) {
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
            $scope.O_T = d.data;
            if (d.data.length != 0) {
                $scope.showdata = true;
            }
            else {
                $scope.showdata = false;
            }
        }, function () {
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
            alertify.error("ошибка");
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
        $scope.s = "option1";
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
    $scope.Clear = function () {
        $scope.Region.id = 1726;
        $scope.Otrasl.id = 'А1';
        $scope.INN = "";
        $scope.zb = "2018";
        $scope.showdata = false;
    }
    $scope.exportData = function () {
        var blob = new Blob([document.getElementById('export').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Талабномалар.xls");
    };
}]);