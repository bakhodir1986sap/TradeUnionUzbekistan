app.controller("OplataViplatiZaPutyovki", ["$scope", "$http", "$filter", function ($scope, $http, $filter) {

    ///////////////////////////////////////////////////////////////////////////
    /// Common Part
    ///////////////////////////////////////////////////////////////////////////
    //Initialize Part
    GetYearsList();
    GetKvartalList();
    GetCurBusinessTime();
    GetOrganizationList();
    GetSanatories();
    GetTipOplatiList();
    GetVidOplatiList();
    GetProcentZaPutyovki();

    //Models
    $scope.SelectedYear = {
        Id: 2018,
        Year: '2018'
    };

    $scope.SelectedKvartal = {
        ID: 2,
        Name_Kv: '2-квартал'
    };

    $scope.SelectedOrganization = {

        ID: -1,
        Name_Org: ''

    };

    $scope.SelectedTipOplati = {
        code: 3,
        name_ru:''
    };

    $scope.SelectedVidOplati = {
        code: 1,
        name_ru: ''
    };

    $scope.SelectedProcent = {
        ID: 1,
        Name_Proc:'45%'
    };



    $scope.SelectedFileOplata = null; //initially make it null   

    $scope.OplataFile = function (files) {
        $scope.$apply(function () {
            $scope.Message = '';
            $scope.SelectedFileOplata = files[0];
        })
    }

    $scope.BindData = null;
    $scope.showLoader = false;
    $scope.IsVisible = false;

    $scope.ParseExcelOplata = function () {
        var formData = new FormData();
        var file = $scope.SelectedFileOplata;
        formData.append('OplataFile', file);
        $scope.showLoader = true;   //show spinner  
        $http({
            method: "post",
            withCredentials: true,
            url: '/PrixodPutevok/LoadOplataViplata',
            data: formData,
            headers: {
                'Content-Type': undefined
            },
            transformRequest: angular.identity
        }).then(function (d) {
            $scope.showdata = false;
            if (d.data == "error") {
                alertify.error("ошибка");
            }
            else {
                alertify.success("Удачно загрузилось");
                var res = d.data;
                $scope.BindData = res;
                if (d.data.length != 0) {
                    $scope.showdata = true;
                } else {
                    $scope.showdata = false;
                }
                $scope.IsVisible = true; //showing the table after databinding  
                $scope.showLoader = false; //after success hide spinner  
            }

        }, function (err) {
            alertify.error("ошибка : " + err.data);
            console.log(err.data);
            console.log("error in parse excel");
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

            GetData();

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

    function GetOrganizationList() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetOrganizationList'
        }).then(function (response) {
            $scope.OrganizationsL = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetOrganizationList");
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

    function GetTipOplatiList() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Reference/GetTipOplati'
        }).then(function (response) {

            $scope.TipOplaties = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetYearsList");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    
    function GetVidOplatiList() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Reference/GetVidOplati'
        }).then(function (response) {

            $scope.VidOplaties = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetYearsList");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function GetProcentZaPutyovki() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Reference/GetProcentZaPutyovki'
        }).then(function (response) {

            $scope.ProcentZaPutyovki = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetYearsList");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    /////////////////////////////////////////////////////////////////////////////////////////
    //// Spesific Business Logic
    /////////////////////////////////////////////////////////////////////////////////////////

    $scope.CurrentOplataMain = {

          ID                :'',       
          Data_Post         :'',
          Num_Doc           :'',
          ID_Otras:         '',
          OrgName:          '',
          Symma_DT          :'',
          Symma_KT          :'',
          Osnov             :'',
          ID_KV             :'',
          SmFil             :'',
          Name_org          :'',
          Tip_Org           :'',
          PrizOpl           :'',
          UserId            :'',
          Departmentbase    :''
    };

    $scope.CurrentOplataPutyovka = {

          ID                :'',            
          ID_Opl            :'',
          ID_Sanat          :'',
          SanatName         :'',
          Num_Put           :'',
          Symma             :'',
          ID_Pr             :'',
          Name_Proc         :'',
          Priz              :'',
          UserId            :'',
          Departmentbase    :'',
          PutyovkaChecked   :''
    };

    function GetOplataList() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetOplataMain?year=' + $scope.SelectedYear.Id + '&kvartal=' + $scope.SelectedKvartal.ID + '&tipOrg=' + $scope.SelectedVidOplati.code + '&Priz=' + $scope.SelectedTipOplati.code
        }).then(function (response) {

            $scope.OplataMains = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetOplataList");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function GetOplataMainRes() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetOplataMainRes?year='+  $scope.SelectedYear.Id + '&kvartal=' + $scope.SelectedKvartal.ID + '&tipOrg=' + $scope.SelectedVidOplati.code + '&Priz=' + $scope.SelectedTipOplati.code
        }).then(function (response) {

            $scope.TotalOplataResults = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetOplataMainRes");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function GetDetailList() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetOplataDetail?IdMain=' + $scope.CurrentOplataMain.ID
        }).then(function (response) {

            $scope.PutyovkaDtls = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetDetailList");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    
    function GetOplataDetailRes() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetOplataDetailRes?IdMain=' + $scope.CurrentOplataMain.ID
        }).then(function (response) {

            $scope.PutyovkaDtlsRes = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetOplataDetailRes");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.idSelectedVote = null;
    $scope.editSelected = function (idSelectedVote) {

        //$scope.idSelectedVote = idSelectedVote.Id;
        $scope.CurrentOplataMain = idSelectedVote;

        $scope.ActionType = 'Редактировать';
        $(".activvvv").removeClass("activvvv");
        var dataPos = (idSelectedVote.Data_Post) ? $filter('date')(parseInt(idSelectedVote.Data_Post.substr(6)), 'dd.MM.yyyy') : '';
        $scope.DataPos = new Date(dataPos.substring(6, 10), dataPos.substring(3, 5) - 1, dataPos.substring(0, 2));
        $("#"+ idSelectedVote.ID).addClass("activvvv");
        $scope.Num_Doc = $scope.CurrentOplataMain.Num_Doc;

        $scope.SelectedOrganization.ID = $scope.CurrentOplataMain.ID_Otras;
        $scope.SelectedOrganization.Name_Org = $scope.CurrentOplataMain.OrgName;

        $scope.Symma_DT = $scope.CurrentOplataMain.Symma_DT;
        $scope.Symma_KT = $scope.CurrentOplataMain.Symma_KT;
        $scope.Name_org = $scope.CurrentOplataMain.Name_org;
        $scope.Osnov = $scope.CurrentOplataMain.Osnov;

        $scope.SelectedPutyovkaRaznisa = $scope.CurrentOplataMain.Symma_KT;
        $scope.PutyovkaDtls = '';

        var tipOpl = $scope.CurrentOplataMain.PrizOpl;
        if (tipOpl == 3) {
            $scope.showSummButton = true;
        }
        else
        {
            $scope.showSummButton = false;
        }

        GetDetailList();
        GetOplataDetailRes();
    }

    function GetData() {

        if (($scope.SelectedYear.Id > -1) && ($scope.SelectedKvartal.ID > 0)) {
            GetOplataList();
            GetOplataMainRes();
        }
    }

    $scope.FiltersChanged = function () {

        if (($scope.SelectedYear.Id > -1) && ($scope.SelectedKvartal.ID > 0)) {
            GetOplataList();
            GetOplataMainRes();
            $scope.PutyovkaDtls = '';
            $scope.PutyovkaDtlsRes = '';
        }
    }

    //Organization Based Datas
    $scope.SummaOpen = function () {

        $scope.SelectedPutyovkas = [];
        $scope.SelectedPutyovka = [];
        $scope.SelectedPutyovkaP = [];
        $scope.SelectedPutyovkaLength = 0;
        $scope.SelectedPutyovkaSumma = 0;
        $scope.IsMinusBalance = true;
        GetNotCoveredPutyovka();
        GetGeneralRemainingByOrg();
        GetTotalDebtByOrg();
        GetOplataSaldo();
    }

    function GetNotCoveredPutyovka() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetNotCoveredPutyovka?IdOrg=' + $scope.CurrentOplataMain.ID_Otras
        }).then(function (response) {

            $scope.NotCoveredPutyovkas = response.data;
            $scope.NotCoveredPutyovkasCount = $scope.NotCoveredPutyovkas.length;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetTotalDebtByOrg");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
        
    function GetGeneralRemainingByOrg() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetGeneralRemainingByOrg?IdOrg=' + $scope.CurrentOplataMain.ID_Otras
        }).then(function (response) {

            $scope.GeneralRemainingByOrg = response.data;
            

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetTotalDebtByOrg");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function GetTotalDebtByOrg() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetTotalDebtByOrg?IdOrg=' + $scope.CurrentOplataMain.ID_Otras
        }).then(function (response) {

            $scope.DebtByOrg = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetTotalDebtByOrg");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }


    function GetOplataSaldo() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetOplataSaldo?IdOpl=' + $scope.CurrentOplataMain.ID
        }).then(function (response) {

            $scope.OplataSaldo = response.data;
            $scope.SelectedPutyovkaRaznisa = $scope.OplataSaldo;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetTotalDebtByOrg");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.AddOplataMain = function () {

        $scope.CurrentOplataMain = {
            ID: '',
            Data_Post: '',
            Num_Doc: '',
            ID_Otras: '',
            OrgName:'',
            Symma_DT: '',
            Symma_KT: '',
            Osnov: '',
            ID_KV: '',
            SmFil: '',
            Name_org: '',
            Tip_Org: '',
            PrizOpl: '',
            UserId: '',
            Departmentbase: ''
        };

        $scope.SelectedOrganization.ID = -1;
        $scope.SelectedOrganization.Name_Org = '';

        $scope.DataPos = '';
        $scope.Num_Doc = '';
        $scope.Symma_DT = 0;
        $scope.Symma_KT = 0;
        $scope.Osnov = '';
        $scope.Name_org = '';


        $scope.ActionType = 'Добавить';
    }

    $scope.AddOplataMainClicked = function () {

        //alertify.error("AddNakladnoy");

        $scope.CurrentOplataMain.Data_Post = $scope.DataPos;

        $scope.CurrentOplataMain.Num_Doc = $scope.Num_Doc;

        $scope.CurrentOplataMain.ID_Otras = $scope.SelectedOrganization.ID;

        $scope.CurrentOplataMain.Symma_DT = $scope.Symma_DT;
        $scope.CurrentOplataMain.Symma_KT = $scope.Symma_KT;

        $scope.CurrentOplataMain.Name_org = $scope.Name_org;

        $scope.CurrentOplataMain.Osnov = $scope.Osnov;
        $scope.CurrentOplataMain.Tip_Org = $scope.SelectedVidOplati.code;
        $scope.CurrentOplataMain.PrizOpl = $scope.SelectedTipOplati.code;

        $scope.CurrentOplataMain.ID_KV = $scope.SelectedKvartal.ID;

        $scope.CurrentOplataMain.SmFil = $scope.SelectedYear.Id;

        var addType = '';
        var tipOpl = $scope.SelectedTipOplati.code;


        if (tipOpl == 4) {
            addType = 'AddOplataMainOplataKurortniy';
        }
        else if (tipOpl == 3) {
            addType = 'AddOplataMainOplataPutyovok';
        }
        else if (tipOpl == 6) {
            addType = 'AddOplataMainOplataVozvrat';
        }
        else {
            addType = 'AddOplataMain';
        }

        $http({
            method: "post",
            url: "/PrixodPutevok/" + addType,
            data: JSON.stringify($scope.CurrentOplataMain),
            dataType: "json"
        }).then(function (response) {

            $scope.CurrentOplataMain = response.data;
            GetData();

        }, function () {
            alertify.error("ошибка");
            console.log('Error in AddOplataMainClicked');
        });
    }

    $scope.ModifyOplataMainClicked = function () {

        //alertify.error("AddNakladnoy");

        $scope.CurrentOplataMain.Data_Post = $scope.DataPos;

        $scope.CurrentOplataMain.Num_Doc = $scope.Num_Doc;

        $scope.CurrentOplataMain.ID_Otras = $scope.SelectedOrganization.ID;

        $scope.CurrentOplataMain.Symma_DT = $scope.Symma_DT;
        $scope.CurrentOplataMain.Symma_KT = $scope.Symma_KT;

        $scope.CurrentOplataMain.Name_org = $scope.Name_org;

        $scope.CurrentOplataMain.Osnov = $scope.Osnov;
        $scope.CurrentOplataMain.Tip_Org = $scope.SelectedVidOplati.code;
        $scope.CurrentOplataMain.PrizOpl = $scope.SelectedTipOplati.code;

        $scope.CurrentOplataMain.ID_KV = $scope.SelectedKvartal.ID;

        $scope.CurrentOplataMain.SmFil = $scope.SelectedYear.Id;

        var addType = '';
        var tipOpl = $scope.SelectedTipOplati.code;


        if (tipOpl == 4) {
            addType = 'ModifyOplataMainOplataKurortniy';
        }
        else if (tipOpl == 3) {
            addType = 'ModifyOplataMainOplataPutyovok';
        }
        else if (tipOpl == 6) {
            addType = 'ModifyOplataMainOplataVozvrat';
        }
        else {
            addType = 'ModifyOplataMain';
        }

        $http({
            method: "post",
            url: "/PrixodPutevok/" + addType,
            data: JSON.stringify($scope.CurrentOplataMain),
            dataType: "json"
        }).then(function (msg) {

            GetData();

        }, function () {
            alertify.error("ошибка");
            console.log('Error in AddOplataMainClicked');
        });
    }


    $scope.RemoveClicked = function () {

        $http({
            method: "post",
            url: "/PrixodPutevok/RemoveOplataMain",
            data: JSON.stringify($scope.CurrentOplataMain),
            dataType: "json"
        }).then(function (msg) {

            GetData();

        }, function () {
            alertify.error("ошибка");
            console.log('Error in AddOplataMainClicked');
        });
    }

    //Oplata List
    $scope.SelectedPutyovkas = [];

    $scope.toggleObjSelection = function ($event, dtl) {
        $event.stopPropagation();
        console.log('checkbox clicked' + dtl);

        var isDelated = false;

        for (var i = 0, l = $scope.SelectedPutyovkas.length; i < l; ++i) {
            var value = $scope.SelectedPutyovkas[i];

            if (dtl === value) {
                $scope.SelectedPutyovkas.splice($scope.SelectedPutyovkas.indexOf(value), 1);
                isDelated = true;
                break;
            }
        }

        if (!isDelated) {
            $scope.SelectedPutyovkas.push(dtl);
        }

        console.log('====================================================');

        for (var i = 0, l = $scope.SelectedPutyovkas.length; i < l; ++i) {
            var value = $scope.SelectedPutyovkas[i];
            console.log('tutuud: ' + value);
        }
    }

    $scope.UdalitPutyovku = function () {
        alertify.error("UdalitPutyovku");

        var postData = { lmodel: $scope.SelectedPutyovkas };

        $http({
            method: "post",
            url: "/PrixodPutevok/RemoveOplataRasxod",
            data: JSON.stringify(postData),
            dataType: "json"
        }).then(function (response) {

            for (var i = 0, l = $scope.SelectedPutyovkas.length; i < l; ++i) {
                var value = $scope.SelectedPutyovkas[i];

                $scope.SelectedPutyovkas.splice($scope.SelectedPutyovkas.indexOf(value), 1);
            }

            GetDetailList();

        }, function () {
            alertify.error("ошибка");
            console.log('Error in UdalitPutyovku');
        });
    }

    ////////////////////////////////////////////////////////////
    ///  Add Detail
    ////////////////////////////////////////////////////////////
    $scope.SelectedPutyovka = [];
    $scope.SelectedPutyovkaLength = 0;
    $scope.SelectedPutyovkaSumma = 0;
    $scope.IsMinusBalance = true;
        
    //For Prixod Putyovka
    $scope.toggleObjSelectionPut = function ($event, rasxodPut) {
        $event.stopPropagation();
        console.log('checkbox putyovka' + rasxodPut.ID);

        var isDelated = false;

        for (var i = 0, l = $scope.SelectedPutyovka.length; i < l; ++i) {
            var value = $scope.SelectedPutyovka[i];

            if (rasxodPut.ID === value.ID) {
                $scope.SelectedPutyovka.splice($scope.SelectedPutyovka.indexOf(rasxodPut), 1);
                isDelated = true;
                break;
            }
        }

        if (!isDelated) {

            if ($scope.SelectedPutyovkaRaznisa < rasxodPut.Symma) {
                rasxodPut.Symma = $scope.SelectedPutyovkaRaznisa;
            }

            $scope.SelectedPutyovka.push(rasxodPut);
        }

        console.log('====================================================');
        var summa = 0;

        for (var i = 0, l = $scope.SelectedPutyovka.length; i < l; ++i) {
            var value = $scope.SelectedPutyovka[i];

            if ($scope.SelectedProcent.ID === 1) {
                summa = summa + value.Symma;
            }
            else if ($scope.SelectedProcent.ID === 2) {
                summa = summa + value.Symma * 100 /45;
            } else if ($scope.SelectedProcent.ID === 3) {
                summa = summa + value.Symma * 0;
            }
            

            console.log('putyovka: ' + value.ID);
        }

        $scope.SelectedPutyovkaLength = $scope.SelectedPutyovka.length;
        $scope.SelectedPutyovkaSumma = summa;
        $scope.SelectedPutyovkaRaznisa = $scope.OplataSaldo - summa;

        if ($scope.SelectedPutyovkaRaznisa < 0)
        {
            $scope.IsMinusBalance = false;
        }
        else
        {
            $scope.IsMinusBalance = true;
        }

    }

    $scope.SelectedPutyovkaP = [];

    $scope.SavePutDtl = function () {

        for (var i = 0, l = $scope.SelectedPutyovka.length; i < l; ++i) {
            var value = $scope.SelectedPutyovka[i];
            $scope.SelectedPutyovkaP.push(value.ID);
        }

        var postData = {
            lmodel: $scope.SelectedPutyovka,
            IdOpl: $scope.CurrentOplataMain.ID,
            IdPr: $scope.SelectedProcent.ID
        };

        $http({
            method: "post",
            url: "/PrixodPutevok/AddOplataPutyovok3",
            data: JSON.stringify(postData),
            dataType: "json"
        }).then(function (response) {

            //

        }, function () {
            alertify.error("ошибка");
            console.log('Error in SavePutDtl');
        });
    }

    ///////////////////////////////////////////////////////////////////
    ////Organization///////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////

    //GetOtraslForOrganization();
    //GetRegionForOrganization();
    GetEmptyCarcas();
    //GetOrganizations();

    $scope.FilterSelectedOtrasl = {
        Name_Ot: '',
        ID_Ot: ''
    };

    $scope.FilterName = '';
    $scope.FilterInn = '';

    $scope.ModalOtraslSelected = {
        Name_Ot: '',
        ID_Ot: ''
    };

    $scope.ModalRegionSelected = {
        NameReg: '',
        ID: ''
    };

    function GetOtraslForOrganization() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetOtraslForOrganization'
        }).then(function (response) {
            $scope.Otrasl = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRekvizits");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.addOrg = function () {

        $scope.Organization = '';
        $scope.Action = "Добавить";

        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetEmptyCarcas?Id=0'
        }).then(function (response) {
            $scope.Organization = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRekvizits");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.editOrg = function (org) {

        $scope.Organization = '';
        $scope.Action = "Изменить";

        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetEmptyCarcas?Id=' + org.ID
        }).then(function (response) {
            $scope.Organization = response.data;
            $scope.ModalOtraslSelected.ID_Ot = org.ID_Ot;
            $scope.ModalOtraslSelected.Name_Ot = org.Name_Ot;

            $scope.ModalRegionSelected.ID = org.Region;
            $scope.ModalRegionSelected.NameReg = org.RegionName;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRekvizits");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function GetEmptyCarcas() {

        $scope.Organization = '';

        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetEmptyCarcas?Id=0'
        }).then(function (response) {
            $scope.Organization = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRekvizits");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function GetOrganizations() {

        $scope.Organization = '';

        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetOrganizationList'
        }).then(function (response) {
            $scope.Organizations = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRekvizits");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.GetOrganizationsFilter = function () {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetOrganizationFilter?Name=' + $scope.FilterName + '&Inn=' + $scope.FilterInn + '&OtrasId=' + $scope.FilterSelectedOtrasl.ID_Ot
        }).then(function (response) {
            $scope.Organizations = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRekvizits");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function GetRegionForOrganization() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetRegionForOrganization'
        }).then(function (response) {
            $scope.Region = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRekvizits");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.SaveOrganization = function () {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });

        $scope.Organization.ID_Ot = $scope.ModalOtraslSelected.ID_Ot;
        $scope.Organization.Region = $scope.ModalRegionSelected.ID;

        $http({
            method: 'post',
            url: '/PrixodPutevok/SaveOrganization',
            data: JSON.stringify($scope.Organization),
            contentType: "application/json; charset=utf-8",
        }).then(function (response) {
            GetOrganizations();
            alertify.info("Успешное сохранение");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in SaveRekvizit");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    
    $scope.SelectedOrganizationL = [];
    
    $scope.toggleObjSelectionOrg = function ($event, Id, Name_Org) {
        $event.stopPropagation();
        console.log('checkbox clicked' + Id);

        var isDelated = false;

        for (var i = 0, l = $scope.SelectedOrganizationL.length; i < l; ++i) {
            var value = $scope.SelectedOrganizationL[i];

            if ((Id + '_' + Name_Org) === value) {
                $scope.SelectedOrganizationL.splice($scope.SelectedOrganizationL.indexOf(Id + '_' + Name_Org), 1);
                isDelated = true;
                break;
            }
        }

        if (!isDelated) {
            $scope.SelectedOrganizationL.push(Id + '_' + Name_Org);
            $scope.SelectedOrganization.ID = Id;
            $scope.SelectedOrganization.Name_Org = Name_Org;
        }

        console.log('====================================================');

        for (var i = 0, l = $scope.SelectedOrganizationL.length; i < l; ++i) {
            var value = $scope.SelectedOrganizationL[i];
            console.log('orgorg: ' + value);
        }
    }

    //////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////

}]);