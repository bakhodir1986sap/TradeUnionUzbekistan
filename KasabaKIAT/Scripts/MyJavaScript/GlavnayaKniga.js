app.controller("GlavnayaKniga", ["$scope", "$http", "$filter", "$locale", "DataService", "CalcService", function ($scope, $http, $filter, $locale ,DataService, CalcService) {
    $scope.showdata = false;
    $scope.ZB = new Date
    $scope.ZE = new Date();

    $scope.rasxod_010_3 = 0;
    $scope.rasxod_010_4 = 0;
    $scope.rasxod_010_5 = 0;
    $scope.rasxod_010_6 = 0;
    $scope.rasxod_010_7 = 0;
    $scope.rasxod_010_8 = 0;
    $scope.rasxod_010_9 = 0;
    $scope.rasxod_010_10 = 0;
    $scope.rasxod_010_11 = 0;
    $scope.rasxod_010_12 = 0;

    $scope.rasxod_020_3 = 0;
    $scope.rasxod_020_4 = 0;
    $scope.rasxod_020_5 = 0;
    $scope.rasxod_020_6 = 0;
    $scope.rasxod_020_7 = 0;
    $scope.rasxod_020_8 = 0;
    $scope.rasxod_020_9 = 0;
    $scope.rasxod_020_10 = 0;
    $scope.rasxod_020_11 = 0;
    $scope.rasxod_020_12 = 0;

    $scope.rasxod_030_3 = 0;
    $scope.rasxod_030_4 = 0;
    $scope.rasxod_030_5 = 0;
    $scope.rasxod_030_6 = 0;
    $scope.rasxod_030_7 = 0;
    $scope.rasxod_030_8 = 0;
    $scope.rasxod_030_9 = 0;
    $scope.rasxod_030_10 = 0;
    $scope.rasxod_030_11 = 0;
    $scope.rasxod_030_12 = 0;

    $scope.rasxod_040_3 = 0;
    $scope.rasxod_040_4 = 0;
    $scope.rasxod_040_5 = 0;
    $scope.rasxod_040_6 = 0;
    $scope.rasxod_040_7 = 0;
    $scope.rasxod_040_8 = 0;
    $scope.rasxod_040_9 = 0;
    $scope.rasxod_040_10 = 0;
    $scope.rasxod_040_11 = 0;
    $scope.rasxod_040_12 = 0;

    $scope.rasxod_060_3 = 0;
    $scope.rasxod_060_4 = 0;
    $scope.rasxod_060_5 = 0;
    $scope.rasxod_060_6 = 0;
    $scope.rasxod_060_7 = 0;
    $scope.rasxod_060_8 = 0;
    $scope.rasxod_060_9 = 0;
    $scope.rasxod_060_10 = 0;
    $scope.rasxod_060_11 = 0;
    $scope.rasxod_060_12 = 0;

    $scope.rasxod_070_3 = 0;
    $scope.rasxod_070_4 = 0;
    $scope.rasxod_070_5 = 0;
    $scope.rasxod_070_6 = 0;
    $scope.rasxod_070_7 = 0;
    $scope.rasxod_070_8 = 0;
    $scope.rasxod_070_9 = 0;
    $scope.rasxod_070_10 = 0;
    $scope.rasxod_070_11 = 0;
    $scope.rasxod_070_12 = 0;

    $scope.rasxod_080_3 = 0;
    $scope.rasxod_080_4 = 0;
    $scope.rasxod_080_5 = 0;
    $scope.rasxod_080_6 = 0;
    $scope.rasxod_080_7 = 0;
    $scope.rasxod_080_8 = 0;
    $scope.rasxod_080_9 = 0;
    $scope.rasxod_080_10 = 0;
    $scope.rasxod_080_11 = 0;
    $scope.rasxod_080_12 = 0;

    $scope.rasxod_090_3 = 0;
    $scope.rasxod_090_4 = 0;
    $scope.rasxod_090_5 = 0;
    $scope.rasxod_090_6 = 0;
    $scope.rasxod_090_7 = 0;
    $scope.rasxod_090_8 = 0;
    $scope.rasxod_090_9 = 0;
    $scope.rasxod_090_10 = 0;
    $scope.rasxod_090_11 = 0;
    $scope.rasxod_090_12 = 0;

    $scope.rasshifrovka_010 = 0;
    $scope.rasshifrovka_020 = 0;
    $scope.rasshifrovka_030 = 0;
    $scope.rasshifrovka_040 = 0;
    $scope.rasshifrovka_050 = 0;

    $scope.drugiye_010 = 0;
    $scope.drugiye_020 = 0;
    $scope.drugiye_030 = 0;
    $scope.drugiye_040 = 0;
    $scope.drugiye_050 = 0;
    $scope.drugiye_060 = 0;

    $scope.SelectedYear = {
        Id: 2018,
        Year: '2018'
    };

    $scope.SelectedKvartal = {
        ID: 2,
        Name_Kv: '2-квартал'
    };

    $scope.ModalToDepCodeSelected = {
        Name: '',
        Kod: ''
    };

    GetYearsList();
    GetKvartalList();
    GetCurBusinessTime();
    GetToDepForOrganization();

    function GetCurBusinessTime() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Reference/GetBusinessTime'
        }).then(function (response) {

            $scope.CurBusinessTime = response.data;
            $scope.SelectedYear.Id = response.data.CurYear;
            $scope.SelectedYear.Year = response.data.CurYear;

            $scope.SelectedKvartal.ID = response.data.CurKvartal;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetCurBusinessTime");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function GetYearsList() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Reference/GetYears'
        }).then(function (response) {
            $scope.Years = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetYearsList");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function GetKvartalList() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Reference/GetKvartals'
        }).then(function (response) {
            $scope.Kvartals = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetKvartalList");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function GetToDepForOrganization() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Buxgalter/SpisokPravsoyuzovOrg'
        }).then(function (response) {
            $scope.ToDepCode = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRekvizits");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.LoadDATA = function () {
        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";
        LoadDATA($scope.SelectedYear, $scope.SelectedKvartal);
    }

    function LoadDATA(bg, end) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: "/Buxgalter/GlavnayaKniga?year=" + bg.Id + "&kvartal=" + end.ID,
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


    $scope.LoadDATAS = function () {
        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";
        LoadDATAS($scope.SelectedYear, $scope.SelectedKvartal);
    }

    function LoadDATAS(bg, end) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: "/Buxgalter/GlavnayaKnigaSvodniy?year=" + bg.Id + "&kvartal=" + end.ID,
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


    $scope.LoadDATAR = function () {
        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";
        LoadDATAR($scope.SelectedYear, $scope.SelectedKvartal, $scope.ModalToDepCodeSelected.Kod);
    }

    function LoadDATAR(bg, end , ProfCode) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: "/Buxgalter/GlavnayaKnigaVRazreze?year=" + bg.Id + "&kvartal=" + end.ID + '&departmentBase=' + ProfCode,
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


    $scope.LoadDATAF = function () {
        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";
        LoadDATAF($scope.SelectedYear, $scope.SelectedKvartal);
    }

    function LoadDATAF(bg, end) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: "/Buxgalter/FinansoviyOtchet?year=" + bg.Id + "&kvartal=" + end.ID,
        }).then(function (d) {
            if (d.data.length != 0) {
                $scope.q = d.data.length;
                $scope.showdata = true;
            } else {
                $scope.showdata = false;
            }
            $scope.DATAF = d.data;

            if ($scope.DATAF._otheroutput !== null) {
                $scope.rasxod_010_3 = $scope.DATAF._otheroutput.rasxod_010_3;
                $scope.rasxod_010_4 = $scope.DATAF._otheroutput.rasxod_010_4;
                $scope.rasxod_010_5 = $scope.DATAF._otheroutput.rasxod_010_5;
                $scope.rasxod_010_6 = $scope.DATAF._otheroutput.rasxod_010_6;
                $scope.rasxod_010_7 = $scope.DATAF._otheroutput.rasxod_010_7;
                $scope.rasxod_010_8 = $scope.DATAF._otheroutput.rasxod_010_8;
                $scope.rasxod_010_9 = $scope.DATAF._otheroutput.rasxod_010_9;
                $scope.rasxod_010_10 = $scope.DATAF._otheroutput.rasxod_010_10;
                $scope.rasxod_010_11 = $scope.DATAF._otheroutput.rasxod_010_11;
                $scope.rasxod_010_12 = $scope.DATAF._otheroutput.rasxod_010_12;
                $scope.rasxod_020_3 = $scope.DATAF._otheroutput.rasxod_020_3;
                $scope.rasxod_020_4 = $scope.DATAF._otheroutput.rasxod_020_4;
                $scope.rasxod_020_5 = $scope.DATAF._otheroutput.rasxod_020_5;
                $scope.rasxod_020_6 = $scope.DATAF._otheroutput.rasxod_020_6;
                $scope.rasxod_020_7 = $scope.DATAF._otheroutput.rasxod_020_7;
                $scope.rasxod_020_8 = $scope.DATAF._otheroutput.rasxod_020_8;
                $scope.rasxod_020_9 = $scope.DATAF._otheroutput.rasxod_020_9;
                $scope.rasxod_020_10 = $scope.DATAF._otheroutput.rasxod_020_10;
                $scope.rasxod_020_11 = $scope.DATAF._otheroutput.rasxod_020_11;
                $scope.rasxod_020_12 = $scope.DATAF._otheroutput.rasxod_020_12;
                $scope.rasxod_030_3 = $scope.DATAF._otheroutput.rasxod_030_3;
                $scope.rasxod_030_4 = $scope.DATAF._otheroutput.rasxod_030_4;
                $scope.rasxod_030_5 = $scope.DATAF._otheroutput.rasxod_030_5;
                $scope.rasxod_030_6 = $scope.DATAF._otheroutput.rasxod_030_6;
                $scope.rasxod_030_7 = $scope.DATAF._otheroutput.rasxod_030_7;
                $scope.rasxod_030_8 = $scope.DATAF._otheroutput.rasxod_030_8;
                $scope.rasxod_030_9 = $scope.DATAF._otheroutput.rasxod_030_9;
                $scope.rasxod_030_10 = $scope.DATAF._otheroutput.rasxod_030_10;
                $scope.rasxod_030_11 = $scope.DATAF._otheroutput.rasxod_030_11;
                $scope.rasxod_030_12 = $scope.DATAF._otheroutput.rasxod_030_12;
                $scope.rasxod_040_3 = $scope.DATAF._otheroutput.rasxod_040_3;
                $scope.rasxod_040_4 = $scope.DATAF._otheroutput.rasxod_040_4;
                $scope.rasxod_040_5 = $scope.DATAF._otheroutput.rasxod_040_5;
                $scope.rasxod_040_6 = $scope.DATAF._otheroutput.rasxod_040_6;
                $scope.rasxod_040_7 = $scope.DATAF._otheroutput.rasxod_040_7;
                $scope.rasxod_040_8 = $scope.DATAF._otheroutput.rasxod_040_8;
                $scope.rasxod_040_9 = $scope.DATAF._otheroutput.rasxod_040_9;
                $scope.rasxod_040_10 = $scope.DATAF._otheroutput.rasxod_040_10;
                $scope.rasxod_040_11 = $scope.DATAF._otheroutput.rasxod_040_11;
                $scope.rasxod_040_12 = $scope.DATAF._otheroutput.rasxod_040_12;
                $scope.rasxod_060_3 = $scope.DATAF._otheroutput.rasxod_060_3;
                $scope.rasxod_060_4 = $scope.DATAF._otheroutput.rasxod_060_4;
                $scope.rasxod_060_5 = $scope.DATAF._otheroutput.rasxod_060_5;
                $scope.rasxod_060_6 = $scope.DATAF._otheroutput.rasxod_060_6;
                $scope.rasxod_060_7 = $scope.DATAF._otheroutput.rasxod_060_7;
                $scope.rasxod_060_8 = $scope.DATAF._otheroutput.rasxod_060_8;
                $scope.rasxod_060_9 = $scope.DATAF._otheroutput.rasxod_060_9;
                $scope.rasxod_060_10 = $scope.DATAF._otheroutput.rasxod_060_10;
                $scope.rasxod_060_11 = $scope.DATAF._otheroutput.rasxod_060_11;
                $scope.rasxod_060_12 = $scope.DATAF._otheroutput.rasxod_060_12;
                $scope.rasxod_070_3 = $scope.DATAF._otheroutput.rasxod_070_3;
                $scope.rasxod_070_4 = $scope.DATAF._otheroutput.rasxod_070_4;
                $scope.rasxod_070_5 = $scope.DATAF._otheroutput.rasxod_070_5;
                $scope.rasxod_070_6 = $scope.DATAF._otheroutput.rasxod_070_6;
                $scope.rasxod_070_7 = $scope.DATAF._otheroutput.rasxod_070_7;
                $scope.rasxod_070_8 = $scope.DATAF._otheroutput.rasxod_070_8;
                $scope.rasxod_070_9 = $scope.DATAF._otheroutput.rasxod_070_9;
                $scope.rasxod_070_10 = $scope.DATAF._otheroutput.rasxod_070_10;
                $scope.rasxod_070_11 = $scope.DATAF._otheroutput.rasxod_070_11;
                $scope.rasxod_070_12 = $scope.DATAF._otheroutput.rasxod_070_12;
                $scope.rasxod_080_3 = $scope.DATAF._otheroutput.rasxod_080_3;
                $scope.rasxod_080_4 = $scope.DATAF._otheroutput.rasxod_080_4;
                $scope.rasxod_080_5 = $scope.DATAF._otheroutput.rasxod_080_5;
                $scope.rasxod_080_6 = $scope.DATAF._otheroutput.rasxod_080_6;
                $scope.rasxod_080_7 = $scope.DATAF._otheroutput.rasxod_080_7;
                $scope.rasxod_080_8 = $scope.DATAF._otheroutput.rasxod_080_8;
                $scope.rasxod_080_9 = $scope.DATAF._otheroutput.rasxod_080_9;
                $scope.rasxod_080_10 = $scope.DATAF._otheroutput.rasxod_080_10;
                $scope.rasxod_080_11 = $scope.DATAF._otheroutput.rasxod_080_11;
                $scope.rasxod_080_12 = $scope.DATAF._otheroutput.rasxod_080_12;
                $scope.rasxod_090_3 = $scope.DATAF._otheroutput.rasxod_090_3;
                $scope.rasxod_090_4 = $scope.DATAF._otheroutput.rasxod_090_4;
                $scope.rasxod_090_5 = $scope.DATAF._otheroutput.rasxod_090_5;
                $scope.rasxod_090_6 = $scope.DATAF._otheroutput.rasxod_090_6;
                $scope.rasxod_090_7 = $scope.DATAF._otheroutput.rasxod_090_7;
                $scope.rasxod_090_8 = $scope.DATAF._otheroutput.rasxod_090_8;
                $scope.rasxod_090_9 = $scope.DATAF._otheroutput.rasxod_090_9;
                $scope.rasxod_090_10 = $scope.DATAF._otheroutput.rasxod_090_10;
                $scope.rasxod_090_11 = $scope.DATAF._otheroutput.rasxod_090_11;
                $scope.rasxod_090_12 = $scope.DATAF._otheroutput.rasxod_090_12;
                $scope.rasshifrovka_010 = $scope.DATAF._otheroutput.rasshifrovka_010;
                $scope.rasshifrovka_020 = $scope.DATAF._otheroutput.rasshifrovka_020;
                $scope.rasshifrovka_030 = $scope.DATAF._otheroutput.rasshifrovka_030;
                $scope.rasshifrovka_040 = $scope.DATAF._otheroutput.rasshifrovka_040;
                $scope.rasshifrovka_050 = $scope.DATAF._otheroutput.rasshifrovka_050;
                $scope.drugiye_010 = $scope.DATAF._otheroutput.drugiye_010;
                $scope.drugiye_020 = $scope.DATAF._otheroutput.drugiye_020;
                $scope.drugiye_030 = $scope.DATAF._otheroutput.drugiye_030;
                $scope.drugiye_040 = $scope.DATAF._otheroutput.drugiye_040;
                $scope.drugiye_050 = $scope.DATAF._otheroutput.drugiye_050;
                $scope.drugiye_060 = $scope.DATAF._otheroutput.drugiye_060;
            }


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


    $scope.LoadDATAFS = function () {
        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";
        LoadDATAFS($scope.SelectedYear, $scope.SelectedKvartal);
    }

    function LoadDATAFS(bg, end) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: "/Buxgalter/FinansoviyOtchetSvodniy?year=" + bg.Id + "&kvartal=" + end.ID,
        }).then(function (d) {
            if (d.data.length != 0) {
                $scope.q = d.data.length;
                $scope.showdata = true;
            } else {
                $scope.showdata = false;
            }
            $scope.DATAF = d.data;

            if ($scope.DATAF._otheroutput !== null)
            {
                $scope.rasxod_010_3 = $scope.DATAF._otheroutput.rasxod_010_3;
                $scope.rasxod_010_4 = $scope.DATAF._otheroutput.rasxod_010_4;
                $scope.rasxod_010_5 = $scope.DATAF._otheroutput.rasxod_010_5;
                $scope.rasxod_010_6 = $scope.DATAF._otheroutput.rasxod_010_6;
                $scope.rasxod_010_7 = $scope.DATAF._otheroutput.rasxod_010_7;
                $scope.rasxod_010_8 = $scope.DATAF._otheroutput.rasxod_010_8;
                $scope.rasxod_010_9 = $scope.DATAF._otheroutput.rasxod_010_9;
                $scope.rasxod_010_10 = $scope.DATAF._otheroutput.rasxod_010_10;
                $scope.rasxod_010_11 = $scope.DATAF._otheroutput.rasxod_010_11;
                $scope.rasxod_010_12 = $scope.DATAF._otheroutput.rasxod_010_12;
                $scope.rasxod_020_3 = $scope.DATAF._otheroutput.rasxod_020_3;
                $scope.rasxod_020_4 = $scope.DATAF._otheroutput.rasxod_020_4;
                $scope.rasxod_020_5 = $scope.DATAF._otheroutput.rasxod_020_5;
                $scope.rasxod_020_6 = $scope.DATAF._otheroutput.rasxod_020_6;
                $scope.rasxod_020_7 = $scope.DATAF._otheroutput.rasxod_020_7;
                $scope.rasxod_020_8 = $scope.DATAF._otheroutput.rasxod_020_8;
                $scope.rasxod_020_9 = $scope.DATAF._otheroutput.rasxod_020_9;
                $scope.rasxod_020_10 = $scope.DATAF._otheroutput.rasxod_020_10;
                $scope.rasxod_020_11 = $scope.DATAF._otheroutput.rasxod_020_11;
                $scope.rasxod_020_12 = $scope.DATAF._otheroutput.rasxod_020_12;
                $scope.rasxod_030_3 = $scope.DATAF._otheroutput.rasxod_030_3;
                $scope.rasxod_030_4 = $scope.DATAF._otheroutput.rasxod_030_4;
                $scope.rasxod_030_5 = $scope.DATAF._otheroutput.rasxod_030_5;
                $scope.rasxod_030_6 = $scope.DATAF._otheroutput.rasxod_030_6;
                $scope.rasxod_030_7 = $scope.DATAF._otheroutput.rasxod_030_7;
                $scope.rasxod_030_8 = $scope.DATAF._otheroutput.rasxod_030_8;
                $scope.rasxod_030_9 = $scope.DATAF._otheroutput.rasxod_030_9;
                $scope.rasxod_030_10 = $scope.DATAF._otheroutput.rasxod_030_10;
                $scope.rasxod_030_11 = $scope.DATAF._otheroutput.rasxod_030_11;
                $scope.rasxod_030_12 = $scope.DATAF._otheroutput.rasxod_030_12;
                $scope.rasxod_040_3 = $scope.DATAF._otheroutput.rasxod_040_3;
                $scope.rasxod_040_4 = $scope.DATAF._otheroutput.rasxod_040_4;
                $scope.rasxod_040_5 = $scope.DATAF._otheroutput.rasxod_040_5;
                $scope.rasxod_040_6 = $scope.DATAF._otheroutput.rasxod_040_6;
                $scope.rasxod_040_7 = $scope.DATAF._otheroutput.rasxod_040_7;
                $scope.rasxod_040_8 = $scope.DATAF._otheroutput.rasxod_040_8;
                $scope.rasxod_040_9 = $scope.DATAF._otheroutput.rasxod_040_9;
                $scope.rasxod_040_10 = $scope.DATAF._otheroutput.rasxod_040_10;
                $scope.rasxod_040_11 = $scope.DATAF._otheroutput.rasxod_040_11;
                $scope.rasxod_040_12 = $scope.DATAF._otheroutput.rasxod_040_12;
                $scope.rasxod_060_3 = $scope.DATAF._otheroutput.rasxod_060_3;
                $scope.rasxod_060_4 = $scope.DATAF._otheroutput.rasxod_060_4;
                $scope.rasxod_060_5 = $scope.DATAF._otheroutput.rasxod_060_5;
                $scope.rasxod_060_6 = $scope.DATAF._otheroutput.rasxod_060_6;
                $scope.rasxod_060_7 = $scope.DATAF._otheroutput.rasxod_060_7;
                $scope.rasxod_060_8 = $scope.DATAF._otheroutput.rasxod_060_8;
                $scope.rasxod_060_9 = $scope.DATAF._otheroutput.rasxod_060_9;
                $scope.rasxod_060_10 = $scope.DATAF._otheroutput.rasxod_060_10;
                $scope.rasxod_060_11 = $scope.DATAF._otheroutput.rasxod_060_11;
                $scope.rasxod_060_12 = $scope.DATAF._otheroutput.rasxod_060_12;
                $scope.rasxod_070_3 = $scope.DATAF._otheroutput.rasxod_070_3;
                $scope.rasxod_070_4 = $scope.DATAF._otheroutput.rasxod_070_4;
                $scope.rasxod_070_5 = $scope.DATAF._otheroutput.rasxod_070_5;
                $scope.rasxod_070_6 = $scope.DATAF._otheroutput.rasxod_070_6;
                $scope.rasxod_070_7 = $scope.DATAF._otheroutput.rasxod_070_7;
                $scope.rasxod_070_8 = $scope.DATAF._otheroutput.rasxod_070_8;
                $scope.rasxod_070_9 = $scope.DATAF._otheroutput.rasxod_070_9;
                $scope.rasxod_070_10 = $scope.DATAF._otheroutput.rasxod_070_10;
                $scope.rasxod_070_11 = $scope.DATAF._otheroutput.rasxod_070_11;
                $scope.rasxod_070_12 = $scope.DATAF._otheroutput.rasxod_070_12;
                $scope.rasxod_080_3 = $scope.DATAF._otheroutput.rasxod_080_3;
                $scope.rasxod_080_4 = $scope.DATAF._otheroutput.rasxod_080_4;
                $scope.rasxod_080_5 = $scope.DATAF._otheroutput.rasxod_080_5;
                $scope.rasxod_080_6 = $scope.DATAF._otheroutput.rasxod_080_6;
                $scope.rasxod_080_7 = $scope.DATAF._otheroutput.rasxod_080_7;
                $scope.rasxod_080_8 = $scope.DATAF._otheroutput.rasxod_080_8;
                $scope.rasxod_080_9 = $scope.DATAF._otheroutput.rasxod_080_9;
                $scope.rasxod_080_10 = $scope.DATAF._otheroutput.rasxod_080_10;
                $scope.rasxod_080_11 = $scope.DATAF._otheroutput.rasxod_080_11;
                $scope.rasxod_080_12 = $scope.DATAF._otheroutput.rasxod_080_12;
                $scope.rasxod_090_3 = $scope.DATAF._otheroutput.rasxod_090_3;
                $scope.rasxod_090_4 = $scope.DATAF._otheroutput.rasxod_090_4;
                $scope.rasxod_090_5 = $scope.DATAF._otheroutput.rasxod_090_5;
                $scope.rasxod_090_6 = $scope.DATAF._otheroutput.rasxod_090_6;
                $scope.rasxod_090_7 = $scope.DATAF._otheroutput.rasxod_090_7;
                $scope.rasxod_090_8 = $scope.DATAF._otheroutput.rasxod_090_8;
                $scope.rasxod_090_9 = $scope.DATAF._otheroutput.rasxod_090_9;
                $scope.rasxod_090_10 = $scope.DATAF._otheroutput.rasxod_090_10;
                $scope.rasxod_090_11 = $scope.DATAF._otheroutput.rasxod_090_11;
                $scope.rasxod_090_12 = $scope.DATAF._otheroutput.rasxod_090_12;
                $scope.rasshifrovka_010 = $scope.DATAF._otheroutput.rasshifrovka_010;
                $scope.rasshifrovka_020 = $scope.DATAF._otheroutput.rasshifrovka_020;
                $scope.rasshifrovka_030 = $scope.DATAF._otheroutput.rasshifrovka_030;
                $scope.rasshifrovka_040 = $scope.DATAF._otheroutput.rasshifrovka_040;
                $scope.rasshifrovka_050 = $scope.DATAF._otheroutput.rasshifrovka_050;
                $scope.drugiye_010 = $scope.DATAF._otheroutput.drugiye_010;
                $scope.drugiye_020 = $scope.DATAF._otheroutput.drugiye_020;
                $scope.drugiye_030 = $scope.DATAF._otheroutput.drugiye_030;
                $scope.drugiye_040 = $scope.DATAF._otheroutput.drugiye_040;
                $scope.drugiye_050 = $scope.DATAF._otheroutput.drugiye_050;
                $scope.drugiye_060 = $scope.DATAF._otheroutput.drugiye_060;
            }
 
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


    $scope.LoadDATAFR = function () {
        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";
        LoadDATAFR($scope.SelectedYear, $scope.SelectedKvartal, $scope.ModalToDepCodeSelected.Kod);
    }

    function LoadDATAFR(bg, end, ProfCode) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: "/Buxgalter/FinansoviyOtchetVRazreze?year=" + bg.Id + "&kvartal=" + end.ID + '&departmentBase=' + ProfCode,
        }).then(function (d) {
            if (d.data.length != 0) {
                $scope.q = d.data.length;
                $scope.showdata = true;
            } else {
                $scope.showdata = false;
            }
            $scope.DATAF = d.data;
            if ($scope.DATAF._otheroutput !== null) {
                $scope.rasxod_010_3 = $scope.DATAF._otheroutput.rasxod_010_3;
                $scope.rasxod_010_4 = $scope.DATAF._otheroutput.rasxod_010_4;
                $scope.rasxod_010_5 = $scope.DATAF._otheroutput.rasxod_010_5;
                $scope.rasxod_010_6 = $scope.DATAF._otheroutput.rasxod_010_6;
                $scope.rasxod_010_7 = $scope.DATAF._otheroutput.rasxod_010_7;
                $scope.rasxod_010_8 = $scope.DATAF._otheroutput.rasxod_010_8;
                $scope.rasxod_010_9 = $scope.DATAF._otheroutput.rasxod_010_9;
                $scope.rasxod_010_10 = $scope.DATAF._otheroutput.rasxod_010_10;
                $scope.rasxod_010_11 = $scope.DATAF._otheroutput.rasxod_010_11;
                $scope.rasxod_010_12 = $scope.DATAF._otheroutput.rasxod_010_12;
                $scope.rasxod_020_3 = $scope.DATAF._otheroutput.rasxod_020_3;
                $scope.rasxod_020_4 = $scope.DATAF._otheroutput.rasxod_020_4;
                $scope.rasxod_020_5 = $scope.DATAF._otheroutput.rasxod_020_5;
                $scope.rasxod_020_6 = $scope.DATAF._otheroutput.rasxod_020_6;
                $scope.rasxod_020_7 = $scope.DATAF._otheroutput.rasxod_020_7;
                $scope.rasxod_020_8 = $scope.DATAF._otheroutput.rasxod_020_8;
                $scope.rasxod_020_9 = $scope.DATAF._otheroutput.rasxod_020_9;
                $scope.rasxod_020_10 = $scope.DATAF._otheroutput.rasxod_020_10;
                $scope.rasxod_020_11 = $scope.DATAF._otheroutput.rasxod_020_11;
                $scope.rasxod_020_12 = $scope.DATAF._otheroutput.rasxod_020_12;
                $scope.rasxod_030_3 = $scope.DATAF._otheroutput.rasxod_030_3;
                $scope.rasxod_030_4 = $scope.DATAF._otheroutput.rasxod_030_4;
                $scope.rasxod_030_5 = $scope.DATAF._otheroutput.rasxod_030_5;
                $scope.rasxod_030_6 = $scope.DATAF._otheroutput.rasxod_030_6;
                $scope.rasxod_030_7 = $scope.DATAF._otheroutput.rasxod_030_7;
                $scope.rasxod_030_8 = $scope.DATAF._otheroutput.rasxod_030_8;
                $scope.rasxod_030_9 = $scope.DATAF._otheroutput.rasxod_030_9;
                $scope.rasxod_030_10 = $scope.DATAF._otheroutput.rasxod_030_10;
                $scope.rasxod_030_11 = $scope.DATAF._otheroutput.rasxod_030_11;
                $scope.rasxod_030_12 = $scope.DATAF._otheroutput.rasxod_030_12;
                $scope.rasxod_040_3 = $scope.DATAF._otheroutput.rasxod_040_3;
                $scope.rasxod_040_4 = $scope.DATAF._otheroutput.rasxod_040_4;
                $scope.rasxod_040_5 = $scope.DATAF._otheroutput.rasxod_040_5;
                $scope.rasxod_040_6 = $scope.DATAF._otheroutput.rasxod_040_6;
                $scope.rasxod_040_7 = $scope.DATAF._otheroutput.rasxod_040_7;
                $scope.rasxod_040_8 = $scope.DATAF._otheroutput.rasxod_040_8;
                $scope.rasxod_040_9 = $scope.DATAF._otheroutput.rasxod_040_9;
                $scope.rasxod_040_10 = $scope.DATAF._otheroutput.rasxod_040_10;
                $scope.rasxod_040_11 = $scope.DATAF._otheroutput.rasxod_040_11;
                $scope.rasxod_040_12 = $scope.DATAF._otheroutput.rasxod_040_12;
                $scope.rasxod_060_3 = $scope.DATAF._otheroutput.rasxod_060_3;
                $scope.rasxod_060_4 = $scope.DATAF._otheroutput.rasxod_060_4;
                $scope.rasxod_060_5 = $scope.DATAF._otheroutput.rasxod_060_5;
                $scope.rasxod_060_6 = $scope.DATAF._otheroutput.rasxod_060_6;
                $scope.rasxod_060_7 = $scope.DATAF._otheroutput.rasxod_060_7;
                $scope.rasxod_060_8 = $scope.DATAF._otheroutput.rasxod_060_8;
                $scope.rasxod_060_9 = $scope.DATAF._otheroutput.rasxod_060_9;
                $scope.rasxod_060_10 = $scope.DATAF._otheroutput.rasxod_060_10;
                $scope.rasxod_060_11 = $scope.DATAF._otheroutput.rasxod_060_11;
                $scope.rasxod_060_12 = $scope.DATAF._otheroutput.rasxod_060_12;
                $scope.rasxod_070_3 = $scope.DATAF._otheroutput.rasxod_070_3;
                $scope.rasxod_070_4 = $scope.DATAF._otheroutput.rasxod_070_4;
                $scope.rasxod_070_5 = $scope.DATAF._otheroutput.rasxod_070_5;
                $scope.rasxod_070_6 = $scope.DATAF._otheroutput.rasxod_070_6;
                $scope.rasxod_070_7 = $scope.DATAF._otheroutput.rasxod_070_7;
                $scope.rasxod_070_8 = $scope.DATAF._otheroutput.rasxod_070_8;
                $scope.rasxod_070_9 = $scope.DATAF._otheroutput.rasxod_070_9;
                $scope.rasxod_070_10 = $scope.DATAF._otheroutput.rasxod_070_10;
                $scope.rasxod_070_11 = $scope.DATAF._otheroutput.rasxod_070_11;
                $scope.rasxod_070_12 = $scope.DATAF._otheroutput.rasxod_070_12;
                $scope.rasxod_080_3 = $scope.DATAF._otheroutput.rasxod_080_3;
                $scope.rasxod_080_4 = $scope.DATAF._otheroutput.rasxod_080_4;
                $scope.rasxod_080_5 = $scope.DATAF._otheroutput.rasxod_080_5;
                $scope.rasxod_080_6 = $scope.DATAF._otheroutput.rasxod_080_6;
                $scope.rasxod_080_7 = $scope.DATAF._otheroutput.rasxod_080_7;
                $scope.rasxod_080_8 = $scope.DATAF._otheroutput.rasxod_080_8;
                $scope.rasxod_080_9 = $scope.DATAF._otheroutput.rasxod_080_9;
                $scope.rasxod_080_10 = $scope.DATAF._otheroutput.rasxod_080_10;
                $scope.rasxod_080_11 = $scope.DATAF._otheroutput.rasxod_080_11;
                $scope.rasxod_080_12 = $scope.DATAF._otheroutput.rasxod_080_12;
                $scope.rasxod_090_3 = $scope.DATAF._otheroutput.rasxod_090_3;
                $scope.rasxod_090_4 = $scope.DATAF._otheroutput.rasxod_090_4;
                $scope.rasxod_090_5 = $scope.DATAF._otheroutput.rasxod_090_5;
                $scope.rasxod_090_6 = $scope.DATAF._otheroutput.rasxod_090_6;
                $scope.rasxod_090_7 = $scope.DATAF._otheroutput.rasxod_090_7;
                $scope.rasxod_090_8 = $scope.DATAF._otheroutput.rasxod_090_8;
                $scope.rasxod_090_9 = $scope.DATAF._otheroutput.rasxod_090_9;
                $scope.rasxod_090_10 = $scope.DATAF._otheroutput.rasxod_090_10;
                $scope.rasxod_090_11 = $scope.DATAF._otheroutput.rasxod_090_11;
                $scope.rasxod_090_12 = $scope.DATAF._otheroutput.rasxod_090_12;
                $scope.rasshifrovka_010 = $scope.DATAF._otheroutput.rasshifrovka_010;
                $scope.rasshifrovka_020 = $scope.DATAF._otheroutput.rasshifrovka_020;
                $scope.rasshifrovka_030 = $scope.DATAF._otheroutput.rasshifrovka_030;
                $scope.rasshifrovka_040 = $scope.DATAF._otheroutput.rasshifrovka_040;
                $scope.rasshifrovka_050 = $scope.DATAF._otheroutput.rasshifrovka_050;
                $scope.drugiye_010 = $scope.DATAF._otheroutput.drugiye_010;
                $scope.drugiye_020 = $scope.DATAF._otheroutput.drugiye_020;
                $scope.drugiye_030 = $scope.DATAF._otheroutput.drugiye_030;
                $scope.drugiye_040 = $scope.DATAF._otheroutput.drugiye_040;
                $scope.drugiye_050 = $scope.DATAF._otheroutput.drugiye_050;
                $scope.drugiye_060 = $scope.DATAF._otheroutput.drugiye_060;
            }

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

    $scope.Clear = function () {
        $scope.ZB = new Date
        $scope.ZE = new Date();
        $scope.showdata = false;
    }
    $scope.exportData = function () {
        var blob = new Blob([document.getElementById('export').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Главная книга.xls");
    };

    $scope.exportData2 = function () {
        var blob = new Blob([document.getElementById('export').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Финансовый отчет.xls");
    };
         
    $scope.SaveRasxod = function () {
        SaveRasxod();
    }

    function SaveRasxod() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });

        var postData = {
            rasxod_arr: [
                { "part": "3", "code": "010", "value": $scope.rasxod_010_3 },
                { "part": "3", "code": "020", "value": $scope.rasxod_020_3 },
                { "part": "3", "code": "030", "value": $scope.rasxod_030_3 },
                { "part": "3", "code": "040", "value": $scope.rasxod_040_3 },
                { "part": "3", "code": "060", "value": $scope.rasxod_060_3 },
                { "part": "3", "code": "070", "value": $scope.rasxod_070_3 },
                { "part": "3", "code": "080", "value": $scope.rasxod_080_3 },
                { "part": "3", "code": "090", "value": $scope.rasxod_090_3 },

                { "part": "4", "code": "010", "value": $scope.rasxod_010_4 },
                { "part": "4", "code": "020", "value": $scope.rasxod_020_4 },
                { "part": "4", "code": "030", "value": $scope.rasxod_030_4 },
                { "part": "4", "code": "040", "value": $scope.rasxod_040_4 },
                { "part": "4", "code": "060", "value": $scope.rasxod_060_4 },
                { "part": "4", "code": "070", "value": $scope.rasxod_070_4 },
                { "part": "4", "code": "080", "value": $scope.rasxod_080_4 },
                { "part": "4", "code": "090", "value": $scope.rasxod_090_4 },

                { "part": "5", "code": "010", "value": $scope.rasxod_010_5 },
                { "part": "5", "code": "020", "value": $scope.rasxod_020_5 },
                { "part": "5", "code": "030", "value": $scope.rasxod_030_5 },
                { "part": "5", "code": "040", "value": $scope.rasxod_040_5 },
                { "part": "5", "code": "060", "value": $scope.rasxod_060_5 },
                { "part": "5", "code": "070", "value": $scope.rasxod_070_5 },
                { "part": "5", "code": "080", "value": $scope.rasxod_080_5 },
                { "part": "5", "code": "090", "value": $scope.rasxod_090_5 },


                { "part": "6", "code": "010", "value": $scope.rasxod_010_6 },
                { "part": "6", "code": "020", "value": $scope.rasxod_020_6 },
                { "part": "6", "code": "030", "value": $scope.rasxod_030_6 },
                { "part": "6", "code": "040", "value": $scope.rasxod_040_6 },
                { "part": "6", "code": "060", "value": $scope.rasxod_060_6 },
                { "part": "6", "code": "070", "value": $scope.rasxod_070_6 },
                { "part": "6", "code": "080", "value": $scope.rasxod_080_6 },
                { "part": "6", "code": "090", "value": $scope.rasxod_090_6 },

                { "part": "7", "code": "010", "value": $scope.rasxod_010_7 },
                { "part": "7", "code": "020", "value": $scope.rasxod_020_7 },
                { "part": "7", "code": "030", "value": $scope.rasxod_030_7 },
                { "part": "7", "code": "040", "value": $scope.rasxod_040_7 },
                { "part": "7", "code": "060", "value": $scope.rasxod_060_7 },
                { "part": "7", "code": "070", "value": $scope.rasxod_070_7 },
                { "part": "7", "code": "080", "value": $scope.rasxod_080_7 },
                { "part": "7", "code": "090", "value": $scope.rasxod_090_7 },

                { "part": "8", "code": "010", "value": $scope.rasxod_010_8 },
                { "part": "8", "code": "020", "value": $scope.rasxod_020_8 },
                { "part": "8", "code": "030", "value": $scope.rasxod_030_8 },
                { "part": "8", "code": "040", "value": $scope.rasxod_040_8 },
                { "part": "8", "code": "060", "value": $scope.rasxod_060_8 },
                { "part": "8", "code": "070", "value": $scope.rasxod_070_8 },
                { "part": "8", "code": "080", "value": $scope.rasxod_080_8 },
                { "part": "8", "code": "090", "value": $scope.rasxod_090_8 },


                { "part": "9", "code": "010", "value": $scope.rasxod_010_9 },
                { "part": "9", "code": "020", "value": $scope.rasxod_020_9 },
                { "part": "9", "code": "030", "value": $scope.rasxod_030_9 },
                { "part": "9", "code": "040", "value": $scope.rasxod_040_9 },
                { "part": "9", "code": "060", "value": $scope.rasxod_060_9 },
                { "part": "9", "code": "070", "value": $scope.rasxod_070_9 },
                { "part": "9", "code": "080", "value": $scope.rasxod_080_9 },
                { "part": "9", "code": "090", "value": $scope.rasxod_090_9 },

                { "part": "10", "code": "010", "value": $scope.rasxod_010_10 },
                { "part": "10", "code": "020", "value": $scope.rasxod_020_10 },
                { "part": "10", "code": "030", "value": $scope.rasxod_030_10 },
                { "part": "10", "code": "040", "value": $scope.rasxod_040_10 },
                { "part": "10", "code": "060", "value": $scope.rasxod_060_10 },
                { "part": "10", "code": "070", "value": $scope.rasxod_070_10 },
                { "part": "10", "code": "080", "value": $scope.rasxod_080_10 },
                { "part": "10", "code": "090", "value": $scope.rasxod_090_10 },

                { "part": "11", "code": "010", "value": $scope.rasxod_010_11 },
                { "part": "11", "code": "020", "value": $scope.rasxod_020_11 },
                { "part": "11", "code": "030", "value": $scope.rasxod_030_11 },
                { "part": "11", "code": "040", "value": $scope.rasxod_040_11 },
                { "part": "11", "code": "060", "value": $scope.rasxod_060_11 },
                { "part": "11", "code": "070", "value": $scope.rasxod_070_11 },
                { "part": "11", "code": "080", "value": $scope.rasxod_080_11 },
                { "part": "11", "code": "090", "value": $scope.rasxod_090_11 },

                { "part": "12", "code": "010", "value": $scope.rasxod_010_12 },
                { "part": "12", "code": "020", "value": $scope.rasxod_020_12 },
                { "part": "12", "code": "030", "value": $scope.rasxod_030_12 },
                { "part": "12", "code": "040", "value": $scope.rasxod_040_12 },
                { "part": "12", "code": "060", "value": $scope.rasxod_060_12 },
                { "part": "12", "code": "070", "value": $scope.rasxod_070_12 },
                { "part": "12", "code": "080", "value": $scope.rasxod_080_12 },
                { "part": "12", "code": "090", "value": $scope.rasxod_090_12 },

                { "part": "13", "code": "010", "value": $scope.rasshifrovka_010 },
                { "part": "13", "code": "020", "value": $scope.rasshifrovka_020 },
                { "part": "13", "code": "030", "value": $scope.rasshifrovka_030 },
                { "part": "13", "code": "040", "value": $scope.rasshifrovka_040 },
                { "part": "13", "code": "050", "value": $scope.rasshifrovka_060 },

                { "part": "14", "code": "010", "value": $scope.drugiye_010 },
                { "part": "14", "code": "020", "value": $scope.drugiye_020 },
                { "part": "14", "code": "030", "value": $scope.drugiye_030 },
                { "part": "14", "code": "040", "value": $scope.drugiye_040 },
                { "part": "14", "code": "050", "value": $scope.drugiye_050 },
                { "part": "14", "code": "060", "value": $scope.drugiye_060 }

            ],
            year: $scope.SelectedYear.Id, 
            kvartal: $scope.SelectedKvartal.ID
        };

        $http({
            method: "post",
            url: "/Buxgalter/SaveRasxod",
            data: JSON.stringify(postData),
            dataType: "json"
        }).then(function (response) {

            console.log(response.data);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');

        }, function () {
            alertify.error("ошибка");
            console.log('Error in SaveRasxod');

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
        //$http({
        //    method: 'get',
        //    url: "/Buxgalter/FinansoviyOtchetSvodniy?year=" + bg.Id + "&kvartal=" + end.ID,
        //}).then(function (d) {
        //    if (d.data.length != 0) {
        //        $scope.q = d.data.length;
        //        $scope.showdata = true;
        //    } else {
        //        $scope.showdata = false;
        //    }
        //    $scope.DATAF = d.data;
        //    console.log(d.data);
        //    document.getElementById('connection').style.visibility = "hidden";
        //    $("#hh").modal('hide');
        //}, function (error) {
        //    alertify.error("ошибка");
        //    console.log("error in GetPostupleniya -> ", error);
        //    document.getElementById('connection').style.visibility = "hidden";
        //    $("#hh").modal('hide');
        //});
    }

}]);