app.controller("Postupleniya", ["$scope", "$http", "$filter", "$locale", "DataService", "CalcService", function ($scope, $http, $filter, $locale,DataService, CalcService) {
    $scope.showdata = false;
    $scope.showdata2 = false;
    $scope.LoadINN = function () {
        LoadDATA($scope.INN);
    }

    ////////////////////////////////
    $scope.labels = ['2006', '2007', '2008', '2009', '2010', '2011', '2012'];
    $scope.series = ['Series A', 'Series B'];

    $scope.data = [
        [65, 59, 80, 81, 56, 55, 40],
        [28, 48, 40, 19, 86, 27, 90]
    ];

    /////////////////////////////////

    GetSanatories();

    function GetSanatories() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Reference/GetSanatories'
        }).then(function (response) {
            $scope.Sanatories = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetSanatories");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.SelectedSanatory = {

        ID: 0,
        Name_San: ''

    };

    $scope.LoadINN_N = function () {
        LoadDATAN($scope.INN);
    }

    $scope.LoadINN_X = function () {
        LoadDATAX($scope.INN);
    }


    $scope.LoadINN_P = function () {
        LoadDATAP($scope.SelectedSanatory.ID, $scope.NumPut);
    }

    function LoadDATA(inn) {
        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";

        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'POST',
            url: "/Otchot/GetPostupleniya?inn=" + inn,
        }).then(function (d) {
            if (d.data.length != 0) {
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

    function LoadDATAN(inn) {

        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";

        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'POST',
            url: "/Otchot/GetPostupleniyaN?inn=" + inn,
        }).then(function (d) {
            if (d.data.length != 0) {
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
            console.log("error in GetPostupleniyaN -> ", error);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }


    function LoadDATAX(inn) {

        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";

        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'POST',
            url: "/Buxgalter/GetEcpTransactions?inn=" + inn,
        }).then(function (d) {
            if (d.data.length != 0) {
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
            console.log("error in GetPostupleniyaN -> ", error);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function LoadDATAP(idSanat, NumPut) {

        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";

        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'POST',
            url: "/PrixodPutevok/GetPutyovkaHistory?IdSanat=" + idSanat + '&Num_Put=' + NumPut,
        }).then(function (d) {
            if (d.data.length != 0) {
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
            console.log("error in GetPostupleniyaN -> ", error);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }


    $scope.update = function (item) {

        var postData = { model: item };

        $http({
            method: "post",
            url: "/Buxgalter/ModifyEcpTransaction",
            data: JSON.stringify(postData),
            dataType: "json"
        }).then(function (response) {

            LoadDATAX($scope.INN);

        }, function () {
            alertify.error("ошибка");
            console.log('Error in SavePutDtl');
        });
    }


    $scope.exportData = function () {
        var blob = new Blob([document.getElementById('export').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Поступления.xls");
    };
    $scope.LoadDATA2 = function () {
        Load($scope.I, $scope.N);
    }
    function Load(inn, name) {
        document.getElementById('connection2').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        if (name == null) {
            name = " ";
        }
        $http({
            method: "post",
            url: "/PrixodPutevok/Getegrpo?inn=" + inn + "&name=" + name,
        }).then(function (d) {
            $scope.DATA3 = d.data;
            if (d.data.length !== 0) {
                $scope.showdata2 = true;
            }
            else {
                $scope.showdata2 = false;
            }
            document.getElementById('connection2').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function () {
            document.getElementById('connection2').style.visibility = "hidden";
            $("#hh").modal('hide');
            alertify.error("ошибка");
        });
    }
    var ii = null;
    var ni = null;
    $scope.T = function (name, inn) {
        ni = name;
        ii = inn;
        document.getElementById("name").value = name;
        document.getElementById("inn1").value = inn;
    }
    $scope.Tanlash = function () {
        if (ii != null && ni != null) {
            $scope.INN = ii;
            LoadDATA(ii);
        }
    }
}]);