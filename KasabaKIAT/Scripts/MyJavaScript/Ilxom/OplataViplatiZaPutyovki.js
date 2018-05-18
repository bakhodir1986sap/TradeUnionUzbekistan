app.controller("OplataViplatiZaPutyovki", ["$scope", "$http", "$filter", function ($scope, $http, $filter) {

    ///////////////////////////////////////////////////////////////////////////
    /// Common Part
    ///////////////////////////////////////////////////////////////////////////
    //Initialize Part
    GetYearsList();
    GetKvartalList();
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
        code: -1,
        name_ru:''
    };

    $scope.SelectedVidOplati = {
        code: -1,
        name_ru: ''
    };

    $scope.SelectedProcent = {
        ID: 0,
        Name_Proc:''
    };

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
            url: '/PrixodPutevok/GetOplataMain?year=' + $scope.SelectedYear.Id + '&kvartal=' + $scope.SelectedKvartal.ID
        }).then(function (response) {

            $scope.OplataMains = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetNakladnoyList");
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
            url: '/PrixodPutevok/GetOplataMainRes?year=' + $scope.SelectedYear.Id + '&kvartal=' + $scope.SelectedKvartal.ID
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

        var dataPos = (idSelectedVote.Data_Post) ? $filter('date')(parseInt(idSelectedVote.Data_Post.substr(6)), 'dd.MM.yyyy') : '';
        $scope.DataPos = new Date(dataPos.substring(6, 10), dataPos.substring(3, 5) - 1, dataPos.substring(0, 2));

        $scope.Num_Doc = $scope.CurrentOplataMain.Num_Doc;

        $scope.SelectedOrganization.ID = $scope.CurrentOplataMain.ID_Otras;
        $scope.SelectedOrganization.Name_Org = $scope.CurrentOplataMain.OrgName;

        $scope.Symma_DT = $scope.CurrentOplataMain.Symma_DT;
        $scope.Symma_KT = $scope.CurrentOplataMain.Symma_KT;
        $scope.Name_org = $scope.CurrentOplataMain.Name_org;
        $scope.Osnov = $scope.CurrentOplataMain.Osnov;

        $scope.PutyovkaDtls = '';

        GetDetailList();
        GetOplataDetailRes();
    }

    GetData();

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
        }
    }

    //Organization Based Datas
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
        $scope.CurrentOplataMainTip_Org = $scope.SelectedVidOplati.code;
        $scope.CurrentOplataMainPrizOpl = $scope.SelectedTipOplati.code;

        $scope.CurrentOplataMain.ID_KV = $scope.SelectedKvartal.ID;

        $scope.CurrentOplataMain.SmFil = $scope.SelectedYear.Id;

        var addType = '';

        if ($scope.SelectedTipOplati.code === 1)
        {
            addType = 'AddOplataMainOplataKurortniy';
        }
        else if ($scope.SelectedTipOplati.code === 2)
        {
            addType = 'AddOplataMainOplataPutyovok';
        }
        else if ($scope.SelectedTipOplati.code === 3)
        {
            addType = 'AddOplataMainOplataVozvrat';
        }
        else
        {
            addType = 'AddOplataMain';
        }

        $http({
            method: "post",
            url: "/PrixodPutevok/" + addType,
            data: JSON.stringify($scope.CurrentOplataMain),
            dataType: "json"
        }).then(function (response) {

            $scope.CurrentOplataMain = response.data;
            GetNakladnoyList();

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
        $scope.CurrentOplataMainTip_Org = $scope.SelectedVidOplati.code;
        $scope.CurrentOplataMainPrizOpl = $scope.SelectedTipOplati.code;

        $scope.CurrentOplataMain.ID_KV = $scope.SelectedKvartal.ID;

        $scope.CurrentOplataMain.SmFil = $scope.SelectedYear.Id;

        var addType = '';

        if ($scope.SelectedTipOplati.code === 1) {
            addType = 'ModifyOplataMainOplataKurortniy';
        }
        else if ($scope.SelectedTipOplati.code === 2) {
            addType = 'ModifyOplataMainOplataPutyovok';
        }
        else if ($scope.SelectedTipOplati.code === 3) {
            addType = 'ModifyOplataMainOplataVozvrat';
        }
        else {
            addType = 'ModifyOplataMainOplata';
        }

        $http({
            method: "post",
            url: "/PrixodPutevok/" + addType,
            data: JSON.stringify($scope.CurrentOplataMain),
            dataType: "json"
        }).then(function (msg) {

            GetNakladnoyList();

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

    //For Prixod Putyovka
    $scope.toggleObjSelectionPut = function ($event, rasxodPutId) {
        $event.stopPropagation();
        console.log('checkbox putyovka' + rasxodPutId);

        var isDelated = false;

        for (var i = 0, l = $scope.SelectedPutyovka.length; i < l; ++i) {
            var value = $scope.SelectedPutyovka[i];

            if (rasxodPutId === value) {
                $scope.SelectedPutyovka.splice($scope.SelectedPutyovka.indexOf(rasxodPutId), 1);
                isDelated = true;
                break;
            }
        }

        if (!isDelated) {
            $scope.SelectedPutyovka.push(rasxodPutId);
        }

        console.log('====================================================');

        for (var i = 0, l = $scope.SelectedPutyovka.length; i < l; ++i) {
            var value = $scope.SelectedPutyovka[i];
            console.log('putyovka: ' + value);
        }
    }

    $scope.SavePutDtl = function () {

        var postData = {
            lmodel: $scope.SelectedPutyovka,
            IdOpl: $scope.CurrentOplataMain.ID,
            IdPr: $scope.SelectedProcent.ID
        };

        $http({
            method: "post",
            url: "/PrixodPutevok/AddOplataPutyovok2",
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

    GetOtraslForOrganization();
    GetRegionForOrganization();
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