app.controller("SverkaDannix", ["$scope", "$http", "$filter", "DataService", "CalcService", function ($scope, $http, $filter, DataService, CalcService) {
    //LoadDATA();


    $scope.showdata = false;
    $scope.showdata2 = false;

    $scope.SelectedFileForUpload = null;

    $scope.UploadFiles = function (files) {
        $scope.$apply(function () {
            $scope.Message = '';
            $scope.SelectedFileForUpload = files[0];
        })
    }

    $scope.CurrentSverka = {

        id          :'',   
        year        :'',
        owner_tp    :'',
        sanat_id    :'',
        num_put     :'',
        date_san    :'',
        price       :'',
        fio_otdix   :'',
        adress      :'',
        work_place  :'',
        otrasl      :'',
        owner_code  :''

    };


    $scope.SaveDATA = function () {
        var SaveData = new FormData();
        var Savefile = $scope.SelectedFileForUpload;
        SaveData.append('UploadedFile', Savefile);

        $scope.CurrentSverka.year = $scope.SelectedYear.Id;
        $scope.CurrentSverka.owner_tp = $scope.SelectedSide.Code;
        $scope.CurrentSverka.sanat_id = $scope.SelectedSanatory.ID;
        $scope.CurrentSverka.owner_code = $scope.SelectedPravsoyuz.MATCH_DEPARTMENTBASE;

        SaveData.append('SelectedYear', $scope.CurrentSverka.year);
        SaveData.append('SelectedSide', $scope.CurrentSverka.owner_tp);
        SaveData.append('SelectedSanatory', $scope.CurrentSverka.sanat_id);
        SaveData.append('SelectedPravsoyuz', $scope.CurrentSverka.owner_code);

        alertify.success("Операция выполняется");
        $http({
            method: "post",
            withCredentials: true,
            url: '/PrixodPutevok/ReadFileSverki',
            data: SaveData,
            headers: {
                'Content-Type': undefined
            },
            transformRequest: angular.identity
        }).then(function (d) {
            $scope.showdata = false;
            if (d.data == "error") {
                alertify.error("ошибка");
            } else if (d.data == "success") {
                alertify.success("Файл удачно загружен");
                GetLoadedRows();
            }
            else {
                console.log(d.data);
            }
        }, function (err) {
            alertify.error("ошибка : " + err.data);
            console.log(err.data);
            console.log("error in parse excel");
        });
    }

    function LoadDATA() {
        $http({
            method: 'POST',
            url: "/PrixodPutevok/SD",
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
            console.log("error in SD -> ", error);
            $("#hh").modal('hide');
        });
    }

    GetYearsList();
    GetSideList();
    GetSanatories();
    GetPravList();

    $scope.SelectedYear = {
        Id: 2018,
        Year: '2018'
    };

    $scope.SelectedYear2 = {
        Id: 2018,
        Year: '2018'
    };

    $scope.SelectedSanatory2 = {

        ID: 0,
        Name_San: ''

    };

    $scope.SelectedSide = {

        Code: 0,
        Decode: 'Санаторий'

    };

    $scope.SelectedSanatory = {

        ID: 0,
        Name_San: ''

    };

    $scope.SelectedPravsoyuz = {

        MATCH_DEPARTMENTBASE: '',
        ProfsoyzNaimenovanie: ''
    };

    $scope.DownloadTemplate =  function() {
        $http({
            method: 'GET',
            url: "/PrixodPutevok/DownloadTemplate",
        }).then(function (d) {
            $scope.TemplateFile = d.data;
            console.log(d.data);
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in DownloadTemplate -> ", error);
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


    function GetLoadedRows() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetLoadedRows?year=' + $scope.SelectedYear.Id
        }).then(function (response) {
            $scope.SverkaDatas = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetLoadedRows");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function GetSideList() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Reference/GetLoadSide'
        }).then(function (response) {
            $scope.Sides = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetSideList");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }


    $scope.GetDifferByYearFiles =  function() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetDifferByYearFiles?year=' + $scope.SelectedYear.Id + '&IdSan=' + $scope.SelectedSanatory.ID + '&Obl=' + $scope.SelectedPravsoyuz.MATCH_DEPARTMENTBASE
        }).then(function (response) {
            $scope.DifferByYearFiles = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetDifferByYearFiles");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }


    function GetPravList() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Reference/GetPravsoyuzList'
        }).then(function (response) {
            $scope.Pravs = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetPravList");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

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

    $scope.exportData = function () {
        var blob = new Blob([document.getElementById('export').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Поступление Группированно По Областям.xls");
    };
}]);