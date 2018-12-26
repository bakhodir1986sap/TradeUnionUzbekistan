app.controller("GlavnayaKniga", ["$scope", "$http", "$filter", "$locale", "DataService", "CalcService", function ($scope, $http, $filter, $locale ,DataService, CalcService) {
    $scope.showdata = false;
    $scope.ZB = new Date
    $scope.ZE = new Date();

    $scope.rasxod_010_3     = '';
    $scope.rasxod_010_4     = '';
    $scope.rasxod_010_5     = '';
    $scope.rasxod_010_6     = '';
    $scope.rasxod_010_7     = '';
    $scope.rasxod_010_8     = '';
    $scope.rasxod_010_9     = '';
    $scope.rasxod_010_10    = '';
    $scope.rasxod_010_11    = '';
    $scope.rasxod_010_12    = '';

    $scope.rasxod_020_3     = '';
    $scope.rasxod_020_4     = '';
    $scope.rasxod_020_5     = '';
    $scope.rasxod_020_6     = '';
    $scope.rasxod_020_7     = '';
    $scope.rasxod_020_8     = '';
    $scope.rasxod_020_9     = '';
    $scope.rasxod_020_10    = '';
    $scope.rasxod_020_11    = '';
    $scope.rasxod_020_12    = '';

    $scope.rasxod_030_3     = '';
    $scope.rasxod_030_4     = '';
    $scope.rasxod_030_5     = '';
    $scope.rasxod_030_6     = '';
    $scope.rasxod_030_7     = '';
    $scope.rasxod_030_8     = '';
    $scope.rasxod_030_9     = '';
    $scope.rasxod_030_10    = '';
    $scope.rasxod_030_11    = '';
    $scope.rasxod_030_12    = '';

    $scope.rasxod_040_3     = '';
    $scope.rasxod_040_4     = '';
    $scope.rasxod_040_5     = '';
    $scope.rasxod_040_6     = '';
    $scope.rasxod_040_7     = '';
    $scope.rasxod_040_8     = '';
    $scope.rasxod_040_9     = '';
    $scope.rasxod_040_10    = '';
    $scope.rasxod_040_11    = '';
    $scope.rasxod_040_12    = '';

    $scope.rasxod_060_3     = '';
    $scope.rasxod_060_4     = '';
    $scope.rasxod_060_5     = '';
    $scope.rasxod_060_6     = '';
    $scope.rasxod_060_7     = '';
    $scope.rasxod_060_8     = '';
    $scope.rasxod_060_9     = '';
    $scope.rasxod_060_10    = '';
    $scope.rasxod_060_11    = '';
    $scope.rasxod_060_12    = '';

    $scope.rasxod_070_3     = '';
    $scope.rasxod_070_4     = '';
    $scope.rasxod_070_5     = '';
    $scope.rasxod_070_6     = '';
    $scope.rasxod_070_7     = '';
    $scope.rasxod_070_8     = '';
    $scope.rasxod_070_9     = '';
    $scope.rasxod_070_10    = '';
    $scope.rasxod_070_11    = '';
    $scope.rasxod_070_12    = '';

    $scope.rasxod_080_3     = '';
    $scope.rasxod_080_4     = '';
    $scope.rasxod_080_5     = '';
    $scope.rasxod_080_6     = '';
    $scope.rasxod_080_7     = '';
    $scope.rasxod_080_8     = '';
    $scope.rasxod_080_9     = '';
    $scope.rasxod_080_10    = '';
    $scope.rasxod_080_11    = '';
    $scope.rasxod_080_12    = '';

    $scope.rasxod_090_3     = '';
    $scope.rasxod_090_4     = '';
    $scope.rasxod_090_5     = '';
    $scope.rasxod_090_6     = '';
    $scope.rasxod_090_7     = '';
    $scope.rasxod_090_8     = '';
    $scope.rasxod_090_9     = '';
    $scope.rasxod_090_10    = '';
    $scope.rasxod_090_11    = '';
    $scope.rasxod_090_12    = '';

    $scope.rasshifrovka_010 = '';
    $scope.rasshifrovka_020 = '';
    $scope.rasshifrovka_030 = '';
    $scope.rasshifrovka_040 = '';
    $scope.rasshifrovka_050 = '';

    $scope.drugiye_010      = '';
    $scope.drugiye_020      = '';
    $scope.drugiye_030      = '';
    $scope.drugiye_040      = '';
    $scope.drugiye_050      = '';
    $scope.drugiye_060      = '';

    $scope.T1R010C01        = '';
    $scope.T1R010C02        = '';
    $scope.T1R020C01        = '';
    $scope.T1R020C02        = '';
    $scope.T1R030C01        = '';
    $scope.T1R030C02        = '';
    $scope.T1R040C01        = '';
    $scope.T1R040C02        = '';
    $scope.T1R050C01        = '';
    $scope.T1R050C02        = '';
    $scope.T1R060C01        = '';
    $scope.T1R060C02        = '';
    $scope.T1R070C01        = '';
    $scope.T1R070C02        = '';
    $scope.T2R010C01        = '';
    $scope.T2R010C02        = '';
    $scope.T2R020C01        = '';
    $scope.T2R020C02        = '';
    $scope.T2R030C01        = '';
    $scope.T2R030C02        = '';
    $scope.T2R040C01        = '';
    $scope.T2R040C02        = '';
    $scope.T2R050C01        = '';
    $scope.T2R050C02        = '';
    $scope.T2R060C01        = '';
    $scope.T2R060C02        = '';
    $scope.T2R070C01        = '';
    $scope.T2R070C02        = '';
    $scope.T2R080C01        = '';
    $scope.T2R080C02        = '';
    $scope.T2R090C01        = '';
    $scope.T2R090C02        = '';
    $scope.T3R010C01        = '';
    $scope.T3R010C02        = '';
    $scope.T3R010C03        = '';
    $scope.T3R010C04        = '';
    $scope.T3R020C01        = '';
    $scope.T3R020C02        = '';
    $scope.T3R020C03        = '';
    $scope.T3R020C04        = '';
    $scope.T3R030C01        = '';
    $scope.T3R030C02        = '';
    $scope.T3R030C03        = '';
    $scope.T3R030C04        = '';
    $scope.T3R040C01        = '';
    $scope.T3R040C02        = '';
    $scope.T3R040C03        = '';
    $scope.T3R040C04        = '';
    $scope.T3R041C01        = '';
    $scope.T3R041C02        = '';
    $scope.T3R041C03        = '';
    $scope.T3R041C04        = '';
    $scope.T3R042C01        = '';
    $scope.T3R042C02        = '';
    $scope.T3R042C03        = '';
    $scope.T3R042C04        = '';
    $scope.T3R060C01        = '';
    $scope.T3R060C02        = '';
    $scope.T3R060C03        = '';
    $scope.T3R060C04        = '';
    $scope.T3R070C01        = '';
    $scope.T3R070C02        = '';
    $scope.T3R070C03        = '';
    $scope.T3R070C04        = '';
    $scope.T3R080C01        = '';
    $scope.T3R080C02        = '';
    $scope.T3R080C03        = '';
    $scope.T3R080C04        = '';
    $scope.T3R090C01        = '';
    $scope.T3R090C02        = '';
    $scope.T3R090C03        = '';
    $scope.T3R090C04        = '';
    $scope.T3R100C01        = '';
    $scope.T3R100C02        = '';
    $scope.T3R100C03        = '';
    $scope.T3R100C04        = '';
    $scope.T3R110C01        = '';
    $scope.T3R110C02        = '';
    $scope.T3R110C03        = '';
    $scope.T3R110C04        = '';
    $scope.T3R121C01        = '';
    $scope.T3R121C02        = '';
    $scope.T3R121C03        = '';
    $scope.T3R122C01        = '';
    $scope.T3R122C02        = '';
    $scope.T3R122C03        = '';
    $scope.T3R130C01        = '';
    $scope.T3R130C02        = '';
    $scope.T3R130C03        = '';
    $scope.T3R140C01        = '';
    $scope.T3R140C02        = '';
    $scope.T3R140C03        = '';
    $scope.T4R000C01        = '';
    $scope.T4R010C01        = '';
    $scope.T4R020C01        = '';
    $scope.T4R030C01        = '';
    $scope.T4R031C01        = '';
    $scope.T4R040C01        = '';
    $scope.T4R050C01        = '';
    $scope.T4R060C01        = '';
    $scope.T4R070C01        = '';
    $scope.T4R080C01        = '';
    $scope.T4R090C01        = '';
    $scope.T4R100C01        = '';
    $scope.T4R120C01        = '';
    $scope.T4R130C01        = '';
    $scope.T4R140C01        = '';
    $scope.T4R150C01        = '';
    $scope.T4R160C01        = '';
    $scope.T5R010C01        = '';
    $scope.T5R020C01        = '';
    $scope.T5R030C01        = '';
    $scope.T5R040C01        = '';
    $scope.T5R050C01        = '';
    $scope.T5R060C01        = '';
    $scope.T5R070C01        = '';
    $scope.T6R010C01        = '';
    $scope.T6R020C01        = '';
    $scope.T6R030C01        = '';
    $scope.T6R040C01        = '';
    $scope.T7R010C01        = '';
    $scope.T7R010C02        = '';
    $scope.T7R020C01        = '';
    $scope.T7R020C02        = '';
    $scope.T7R030C01        = '';
    $scope.T7R030C02        = '';
    $scope.T7R040C01        = '';
    $scope.T7R040C02        = '';
    $scope.T7R050C01        = '';
    $scope.T7R050C02        = '';
    $scope.T7R060C01        = '';
    $scope.T7R060C02        = '';

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
    GetRekvizits();

    function GetRekvizits() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Home/GetRekvizit'
        }).then(function (response) {
            $scope.Rekvizits = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRekvizits");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

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


    }

    ///////////////////////////////////////////////////////////////////////////////////

    $scope.LoadDATASanProf = function () {
        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";
        LoadDATASanProf($scope.SelectedYear, $scope.SelectedKvartal);
    }

    function LoadDATASanProf(bg, end) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: "/Buxgalter/GetSanatoriyProfilaktoriy?year=" + bg.Id + "&kvartal=" + end.ID,
        }).then(function (d) {
            if (d.data.length != 0) {
                $scope.q = d.data.length;
                $scope.showdata = true;
            } else {
                $scope.showdata = false;
            }

                $scope.T1R010C01 = d.data.T1R010C01;
                $scope.T1R010C02 = d.data.T1R010C02;
                $scope.T1R020C01 = d.data.T1R020C01;
                $scope.T1R020C02 = d.data.T1R020C02;
                $scope.T1R030C01 = d.data.T1R030C01;
                $scope.T1R030C02 = d.data.T1R030C02;
                $scope.T1R040C01 = d.data.T1R040C01;
                $scope.T1R040C02 = d.data.T1R040C02;
                $scope.T1R050C01 = d.data.T1R050C01;
                $scope.T1R050C02 = d.data.T1R050C02;
                $scope.T1R060C01 = d.data.T1R060C01;
                $scope.T1R060C02 = d.data.T1R060C02;
                $scope.T1R070C01 = d.data.T1R070C01;
                $scope.T1R070C02 = d.data.T1R070C02;
                $scope.T2R010C01 = d.data.T2R010C01;
                $scope.T2R010C02 = d.data.T2R010C02;
                $scope.T2R020C01 = d.data.T2R020C01;
                $scope.T2R020C02 = d.data.T2R020C02;
                $scope.T2R030C01 = d.data.T2R030C01;
                $scope.T2R030C02 = d.data.T2R030C02;
                $scope.T2R040C01 = d.data.T2R040C01;
                $scope.T2R040C02 = d.data.T2R040C02;
                $scope.T2R050C01 = d.data.T2R050C01;
                $scope.T2R050C02 = d.data.T2R050C02;
                $scope.T2R060C01 = d.data.T2R060C01;
                $scope.T2R060C02 = d.data.T2R060C02;
                $scope.T2R070C01 = d.data.T2R070C01;
                $scope.T2R070C02 = d.data.T2R070C02;
                $scope.T2R080C01 = d.data.T2R080C01;
                $scope.T2R080C02 = d.data.T2R080C02;
                $scope.T2R090C01 = d.data.T2R090C01;
                $scope.T2R090C02 = d.data.T2R090C02;
                $scope.T3R010C01 = d.data.T3R010C01;
                $scope.T3R010C02 = d.data.T3R010C02;
                $scope.T3R010C03 = d.data.T3R010C03;
                $scope.T3R010C04 = d.data.T3R010C04;
                $scope.T3R020C01 = d.data.T3R020C01;
                $scope.T3R020C02 = d.data.T3R020C02;
                $scope.T3R020C03 = d.data.T3R020C03;
                $scope.T3R020C04 = d.data.T3R020C04;
                $scope.T3R030C01 = d.data.T3R030C01;
                $scope.T3R030C02 = d.data.T3R030C02;
                $scope.T3R030C03 = d.data.T3R030C03;
                $scope.T3R030C04 = d.data.T3R030C04;
                $scope.T3R040C01 = d.data.T3R040C01;
                $scope.T3R040C02 = d.data.T3R040C02;
                $scope.T3R040C03 = d.data.T3R040C03;
                $scope.T3R040C04 = d.data.T3R040C04;
                $scope.T3R041C01 = d.data.T3R041C01;
                $scope.T3R041C02 = d.data.T3R041C02;
                $scope.T3R041C03 = d.data.T3R041C03;
                $scope.T3R041C04 = d.data.T3R041C04;
                $scope.T3R042C01 = d.data.T3R042C01;
                $scope.T3R042C02 = d.data.T3R042C02;
                $scope.T3R042C03 = d.data.T3R042C03;
                $scope.T3R042C04 = d.data.T3R042C04;
                $scope.T3R060C01 = d.data.T3R060C01;
                $scope.T3R060C02 = d.data.T3R060C02;
                $scope.T3R060C03 = d.data.T3R060C03;
                $scope.T3R060C04 = d.data.T3R060C04;
                $scope.T3R070C01 = d.data.T3R070C01;
                $scope.T3R070C02 = d.data.T3R070C02;
                $scope.T3R070C03 = d.data.T3R070C03;
                $scope.T3R070C04 = d.data.T3R070C04;
                $scope.T3R080C01 = d.data.T3R080C01;
                $scope.T3R080C02 = d.data.T3R080C02;
                $scope.T3R080C03 = d.data.T3R080C03;
                $scope.T3R080C04 = d.data.T3R080C04;
                $scope.T3R090C01 = d.data.T3R090C01;
                $scope.T3R090C02 = d.data.T3R090C02;
                $scope.T3R090C03 = d.data.T3R090C03;
                $scope.T3R090C04 = d.data.T3R090C04;
                $scope.T3R100C01 = d.data.T3R100C01;
                $scope.T3R100C02 = d.data.T3R100C02;
                $scope.T3R100C03 = d.data.T3R100C03;
                $scope.T3R100C04 = d.data.T3R100C04;
                $scope.T3R110C01 = d.data.T3R110C01;
                $scope.T3R110C02 = d.data.T3R110C02;
                $scope.T3R110C03 = d.data.T3R110C03;
                $scope.T3R110C04 = d.data.T3R110C04;
                $scope.T3R121C01 = d.data.T3R121C01;
                $scope.T3R121C02 = d.data.T3R121C02;
                $scope.T3R121C03 = d.data.T3R121C03;
                $scope.T3R122C01 = d.data.T3R122C01;
                $scope.T3R122C02 = d.data.T3R122C02;
                $scope.T3R122C03 = d.data.T3R122C03;
                $scope.T3R130C01 = d.data.T3R130C01;
                $scope.T3R130C02 = d.data.T3R130C02;
                $scope.T3R130C03 = d.data.T3R130C03;
                $scope.T3R140C01 = d.data.T3R140C01;
                $scope.T3R140C02 = d.data.T3R140C02;
                $scope.T3R140C03 = d.data.T3R140C03;
                $scope.T4R000C01 = d.data.T4R000C01;
                $scope.T4R010C01 = d.data.T4R010C01;
                $scope.T4R020C01 = d.data.T4R020C01;
                $scope.T4R030C01 = d.data.T4R030C01;
                $scope.T4R031C01 = d.data.T4R031C01;
                $scope.T4R040C01 = d.data.T4R040C01;
                $scope.T4R050C01 = d.data.T4R050C01;
                $scope.T4R060C01 = d.data.T4R060C01;
                $scope.T4R070C01 = d.data.T4R070C01;
                $scope.T4R080C01 = d.data.T4R080C01;
                $scope.T4R090C01 = d.data.T4R090C01;
                $scope.T4R100C01 = d.data.T4R100C01;
                $scope.T4R120C01 = d.data.T4R120C01;
                $scope.T4R130C01 = d.data.T4R130C01;
                $scope.T4R140C01 = d.data.T4R140C01;
                $scope.T4R150C01 = d.data.T4R150C01;
                $scope.T4R160C01 = d.data.T4R160C01;
                $scope.T5R010C01 = d.data.T5R010C01;
                $scope.T5R020C01 = d.data.T5R020C01;
                $scope.T5R030C01 = d.data.T5R030C01;
                $scope.T5R040C01 = d.data.T5R040C01;
                $scope.T5R050C01 = d.data.T5R050C01;
                $scope.T5R060C01 = d.data.T5R060C01;
                $scope.T5R070C01 = d.data.T5R070C01;
                $scope.T6R010C01 = d.data.T6R010C01;
                $scope.T6R020C01 = d.data.T6R020C01;
                $scope.T6R030C01 = d.data.T6R030C01;
                $scope.T6R040C01 = d.data.T6R040C01;
                $scope.T7R010C01 = d.data.T7R010C01;
                $scope.T7R010C02 = d.data.T7R010C02;
                $scope.T7R020C01 = d.data.T7R020C01;
                $scope.T7R020C02 = d.data.T7R020C02;
                $scope.T7R030C01 = d.data.T7R030C01;
                $scope.T7R030C02 = d.data.T7R030C02;
                $scope.T7R040C01 = d.data.T7R040C01;
                $scope.T7R040C02 = d.data.T7R040C02;
                $scope.T7R050C01 = d.data.T7R050C01;
                $scope.T7R050C02 = d.data.T7R050C02;
                $scope.T7R060C01 = d.data.T7R060C01;
                $scope.T7R060C02 = d.data.T7R060C02;


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

    $scope.SaveSanProf = function () {
        SaveSanProf();
    }

    function SaveSanProf() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });

        var postData = {
            T1R010C01 : $scope.T1R010C01,
            T1R010C02 : $scope.T1R010C02,
            T1R020C01 : $scope.T1R020C01,
            T1R020C02 : $scope.T1R020C02,
            T1R030C01 : $scope.T1R030C01,
            T1R030C02 : $scope.T1R030C02,
            T1R040C01 : $scope.T1R040C01,
            T1R040C02 : $scope.T1R040C02,
            T1R050C01 : $scope.T1R050C01,
            T1R050C02 : $scope.T1R050C02,
            T1R060C01 : $scope.T1R060C01,
            T1R060C02 : $scope.T1R060C02,
            T1R070C01 : $scope.T1R070C01,
            T1R070C02 : $scope.T1R070C02,
            T2R010C01 : $scope.T2R010C01,
            T2R010C02 : $scope.T2R010C02,
            T2R020C01 : $scope.T2R020C01,
            T2R020C02 : $scope.T2R020C02,
            T2R030C01 : $scope.T2R030C01,
            T2R030C02 : $scope.T2R030C02,
            T2R040C01 : $scope.T2R040C01,
            T2R040C02 : $scope.T2R040C02,
            T2R050C01 : $scope.T2R050C01,
            T2R050C02 : $scope.T2R050C02,
            T2R060C01 : $scope.T2R060C01,
            T2R060C02 : $scope.T2R060C02,
            T2R070C01 : $scope.T2R070C01,
            T2R070C02 : $scope.T2R070C02,
            T2R080C01 : $scope.T2R080C01,
            T2R080C02 : $scope.T2R080C02,
            T2R090C01 : $scope.T2R090C01,
            T2R090C02 : $scope.T2R090C02,
            T3R010C01 : $scope.T3R010C01,
            T3R010C02 : $scope.T3R010C02,
            T3R010C03 : $scope.T3R010C03,
            T3R010C04 : $scope.T3R010C04,
            T3R020C01 : $scope.T3R020C01,
            T3R020C02 : $scope.T3R020C02,
            T3R020C03 : $scope.T3R020C03,
            T3R020C04 : $scope.T3R020C04,
            T3R030C01 : $scope.T3R030C01,
            T3R030C02 : $scope.T3R030C02,
            T3R030C03 : $scope.T3R030C03,
            T3R030C04 : $scope.T3R030C04,
            T3R040C01 : $scope.T3R040C01,
            T3R040C02 : $scope.T3R040C02,
            T3R040C03 : $scope.T3R040C03,
            T3R040C04 : $scope.T3R040C04,
            T3R041C01 : $scope.T3R041C01,
            T3R041C02 : $scope.T3R041C02,
            T3R041C03 : $scope.T3R041C03,
            T3R041C04 : $scope.T3R041C04,
            T3R042C01 : $scope.T3R042C01,
            T3R042C02 : $scope.T3R042C02,
            T3R042C03 : $scope.T3R042C03,
            T3R042C04 : $scope.T3R042C04,
            T3R060C01 : $scope.T3R060C01,
            T3R060C02 : $scope.T3R060C02,
            T3R060C03 : $scope.T3R060C03,
            T3R060C04 : $scope.T3R060C04,
            T3R070C01 : $scope.T3R070C01,
            T3R070C02 : $scope.T3R070C02,
            T3R070C03 : $scope.T3R070C03,
            T3R070C04 : $scope.T3R070C04,
            T3R080C01 : $scope.T3R080C01,
            T3R080C02 : $scope.T3R080C02,
            T3R080C03 : $scope.T3R080C03,
            T3R080C04 : $scope.T3R080C04,
            T3R090C01 : $scope.T3R090C01,
            T3R090C02 : $scope.T3R090C02,
            T3R090C03 : $scope.T3R090C03,
            T3R090C04 : $scope.T3R090C04,
            T3R100C01 : $scope.T3R100C01,
            T3R100C02 : $scope.T3R100C02,
            T3R100C03 : $scope.T3R100C03,
            T3R100C04 : $scope.T3R100C04,
            T3R110C01 : $scope.T3R110C01,
            T3R110C02 : $scope.T3R110C02,
            T3R110C03 : $scope.T3R110C03,
            T3R110C04 : $scope.T3R110C04,
            T3R121C01 : $scope.T3R121C01,
            T3R121C02 : $scope.T3R121C02,
            T3R121C03 : $scope.T3R121C03,
            T3R122C01 : $scope.T3R122C01,
            T3R122C02 : $scope.T3R122C02,
            T3R122C03 : $scope.T3R122C03,
            T3R130C01 : $scope.T3R130C01,
            T3R130C02 : $scope.T3R130C02,
            T3R130C03 : $scope.T3R130C03,
            T3R140C01 : $scope.T3R140C01,
            T3R140C02 : $scope.T3R140C02,
            T3R140C03 : $scope.T3R140C03,
            T4R000C01 : $scope.T4R000C01,
            T4R010C01 : $scope.T4R010C01,
            T4R020C01 : $scope.T4R020C01,
            T4R030C01 : $scope.T4R030C01,
            T4R031C01 : $scope.T4R031C01,
            T4R040C01 : $scope.T4R040C01,
            T4R050C01 : $scope.T4R050C01,
            T4R060C01 : $scope.T4R060C01,
            T4R070C01 : $scope.T4R070C01,
            T4R080C01 : $scope.T4R080C01,
            T4R090C01 : $scope.T4R090C01,
            T4R100C01 : $scope.T4R100C01,
            T4R120C01 : $scope.T4R120C01,
            T4R130C01 : $scope.T4R130C01,
            T4R140C01 : $scope.T4R140C01,
            T4R150C01 : $scope.T4R150C01,
            T4R160C01 : $scope.T4R160C01,
            T5R010C01 : $scope.T5R010C01,
            T5R020C01 : $scope.T5R020C01,
            T5R030C01 : $scope.T5R030C01,
            T5R040C01 : $scope.T5R040C01,
            T5R050C01 : $scope.T5R050C01,
            T5R060C01 : $scope.T5R060C01,
            T5R070C01 : $scope.T5R070C01,
            T6R010C01 : $scope.T6R010C01,
            T6R020C01 : $scope.T6R020C01,
            T6R030C01 : $scope.T6R030C01,
            T6R040C01 : $scope.T6R040C01,
            T7R010C01 : $scope.T7R010C01,
            T7R010C02 : $scope.T7R010C02,
            T7R020C01 : $scope.T7R020C01,
            T7R020C02 : $scope.T7R020C02,
            T7R030C01 : $scope.T7R030C01,
            T7R030C02 : $scope.T7R030C02,
            T7R040C01 : $scope.T7R040C01,
            T7R040C02 : $scope.T7R040C02,
            T7R050C01 : $scope.T7R050C01,
            T7R050C02 : $scope.T7R050C02,
            T7R060C01 : $scope.T7R060C01,
            T7R060C02 : $scope.T7R060C02,
            year: $scope.SelectedYear.Id,
            kvartal: $scope.SelectedKvartal.ID
        };

        $http({
            method: "post",
            url: "/Buxgalter/SaveSanatoriyProfilaktoriy",
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


    }

}]);