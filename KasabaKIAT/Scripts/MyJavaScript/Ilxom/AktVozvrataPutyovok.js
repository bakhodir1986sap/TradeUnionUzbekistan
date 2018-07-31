app.controller("AktVozvrataPutyovok", ["$scope", "$http", "$filter", function ($scope, $http, $filter) {

    ///////////////////////////////////////////////////////////////////////////
    /// Common Part
    ///////////////////////////////////////////////////////////////////////////
    $scope.SelectedYear = {
        Id: 2018,
        Year: '2018'
    };

    $scope.SelectedKvartal = {
        ID: 2,
        Name_Kv: '2-квартал'
    };

    //Initialize Part
    GetYearsList();
    GetKvartalList();
    GetCurBusinessTime();
    GetOrganizationList();
    GetSanatories();

    //Models


    $scope.SelectedOrganization = {

        ID: -1,
        Name_Org: ''

    };

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

    /////////////////////////////////////////////////////////////////////////////////////////
    //// Spesific Business Logic
    /////////////////////////////////////////////////////////////////////////////////////////

    $scope.CurrentAkvVozv = {

        id                  :'',         
        data_post           :'',
        num_doc             :'',
        id_sanat            :'',
        sanatoriyName       :'',
        num_put             :'',
        id_ot:              '',
        orgName:            '',
        dover               :'',
        id_pol              :'',
        symmap              :'',
        id_kv               :'',
        smfil               :'',
        tip_voz             :'',
        userid              :'',
        departmentbase      :''
    };

    $scope.AkvVozvMain = {
        data_post           :'',
        num_doc             :'',
        id_ot               :'',
        dover               :'',
        symmap              :'',
        id_kv               :'',
        smfil               :'',
        tip_voz             :'',
        userid              :'',
        departmentbase      :''
    };

    $scope.CurrentRealizasiyaPutyovka = {

        id: '',
        id_vipl: '',
        id_pok: '',
        id_sanat: '',
        sanatName: '',
        num_put: '',
        num_proc: '',
        symma: '',
        id_polvoz: '',
        data_post: '',
        userid: '',
        zayavkaid: '',
        departmentbase: '',
        putyovkaChecked: '',
        primechaniye: '',
        oplata: ''
    };

    function GetAktVozvList() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetVozvratList?year=' + $scope.SelectedYear.Id + '&kvartal=' + $scope.SelectedKvartal.ID
        }).then(function (response) {

            $scope.AktVozvrats = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetNakladnoyList");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function GetVozvratRes() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetVozvratRes?year=' + $scope.SelectedYear.Id + '&kvartal=' + $scope.SelectedKvartal.ID
        }).then(function (response) {

            $scope.VozvratDtlSummar = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetVozvratRes");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    
    function GetTotalResults() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/ShowTotalForVozvrat?year=' + $scope.SelectedYear.Id + '&kvartal=' + $scope.SelectedKvartal.ID
        }).then(function (response) {

            $scope.TotalResults = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetTotalResults");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function GetData() {

        if (($scope.SelectedYear.Id > -1) && ($scope.SelectedKvartal.ID > 0)) {
            GetAktVozvList();
            GetTotalResults();
           
        }
    }

    $scope.FiltersChanged = function () {

        if (($scope.SelectedYear.Id > -1) && ($scope.SelectedKvartal.ID > 0)) {
            GetTotalResults();
            GetAktVozvList();
        }

    }

    function GetGivenPutyovkaList() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetGivenPutyovkaList?idOrg=' + $scope.SelectedOrganization.ID
        }).then(function (response) {

            $scope.PutyovkaList = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetGivenPutyovkaList");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.SelectedPutyovka = [];
    $scope.SelectedPutyovkaCount = 0;

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

        $scope.SelectedPutyovkaCount = $scope.SelectedPutyovka.length;
    }

    $scope.AppVozvrat = function () {

        $scope.AkvVozvMain = {
            data_post: '',
            num_doc: '',
            id_ot: '',
            dover: '',
            symmap: '',
            id_kv: '',
            smfil: '',
            tip_voz: '',
            userid: '',
            departmentbase: ''
        };

        $scope.SelectedOrganization.ID = -1;
        $scope.SelectedOrganization.Name_Org = '';

        $scope.PutyovkaList = '';
    }

    $scope.OrganizationChanged = function () {

        GetGivenPutyovkaList();
    }


    $scope.SaveVozvratEnter = function () {

        $scope.AkvVozvMain.id_ot = $scope.SelectedOrganization.ID;
        $scope.AkvVozvMain.id_kv = $scope.SelectedKvartal.ID;
        $scope.AkvVozvMain.smfil = $scope.SelectedYear.Id;
        $scope.AkvVozvMain.tip_voz = 0;

        var postData = { mainMdl: $scope.AkvVozvMain, lrasxm: $scope.SelectedPutyovka };

        $http({
            method: "post",
            url: "/PrixodPutevok/AddVozvtars2",
            data: JSON.stringify(postData),
            dataType: "json"
        }).then(function (response) {

            GetData();

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
            GetGivenPutyovkaList();
            $scope.OrganizationChanged();
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