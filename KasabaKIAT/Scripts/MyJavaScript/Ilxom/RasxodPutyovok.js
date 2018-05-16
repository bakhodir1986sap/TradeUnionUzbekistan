app.controller("RasxodPutyovok", ["$scope", "$http", "$filter", function ($scope, $http, $filter) {

     ///////////////////////////////////////////////////////////////////////////
    /// Common Part
    ///////////////////////////////////////////////////////////////////////////
    //Initialize Part
    GetYearsList();
    GetKvartalList();
    GetOrganizationList();
    GetSanatories();

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
    //Master Row Selected
    GetData();

    $scope.CurrentNakladnoy = {

        id: '',
        id_org: '',
        organizationName:'',
        data_post: '',
        num_doc: '',
        id_pok: '',
        num_kol: '',
        symma: '',
        name_dov: '',
        id_kv: '',
        vid: '',
        smfil: '',
        userid: '',
        departmentbase: ''
    };

    $scope.CurrentPrixodPutyovka = {

        Id: '',
        IdPost: '',
        IdSanat: '',
        SanatName: '',
        Num_Put: '',
        DataSan: '',
        DataSanEnd: '',
        Symma_Put: '',
        ID_PolVoz: '',
        Data_Post: '',
        UserId: '',
        Departmentbase: '',
        Checked: ''
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

    $scope.idSelectedVote = null;
    $scope.editSelected = function (idSelectedVote) {

        $scope.idSelectedVote = idSelectedVote.Id;
        $scope.CurrentNakladnoy = idSelectedVote;

        $scope.ActionType = 'Редактировать';

        //Data Pos
        var dataPos = (idSelectedVote.data_post) ? $filter('date')(parseInt(idSelectedVote.data_post.substr(6)), 'dd.MM.yyyy') : '';
        $scope.DataPos = new Date(dataPos.substring(6, 10), dataPos.substring(3, 5) - 1, dataPos.substring(0, 2));

        //Number Putyovka
        $scope.Num_Doc = $scope.CurrentNakladnoy.num_doc;

        //Organization
        $scope.SelectedOrganization.ID = $scope.CurrentNakladnoy.id_org;
        $scope.SelectedOrganization.Name_Org = $scope.CurrentNakladnoy.organizationName;

        //Doverennost
        $scope.Name_Dov = $scope.CurrentNakladnoy.name_dov;

        $scope.NakladnoyDtls = '';

        GetDetailList();
        GetDetailListRes();
    }

    function GetNakladnoyList() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetViplMain?year=' + $scope.SelectedYear.Id + '&kvartal=' + $scope.SelectedKvartal.ID
        }).then(function (response) {

            $scope.Nakladnoys = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetYearsList");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function GetData() {

        if (($scope.SelectedYear.Id > -1) && ($scope.SelectedKvartal.ID > 0)) {
            GetTotalResults();
            GetNakladnoyList();
        }
    }

    function GetTotalResults() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetViplTotalResults?year=' + $scope.SelectedYear.Id + '&kvartal=' + $scope.SelectedKvartal.ID
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

    function GetDetailList() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetViplDtl?IdVipl=' + $scope.CurrentNakladnoy.id
        }).then(function (response) {

            $scope.NakladnoyDtls = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetDetailList");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.FiltersChanged = function () {

        if (($scope.SelectedYear.Id > -1) && ($scope.SelectedKvartal.ID > 0)) {
            GetTotalResults();
            GetNakladnoyList();
        }

    }

    function GetDetailListRes() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetViplDetailListRes?IdVipl=' + $scope.CurrentNakladnoy.id
        }).then(function (response) {

            $scope.PutyovkaDtlSummar = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetDetailListRes");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.AddNakladnoy = function () {

        $scope.CurrentNakladnoy = {
            id: '',
            id_org: '',
            data_post: '',
            num_doc: '',
            id_pok: '',
            num_kol: '',
            symma: '',
            name_dov: '',
            id_kv: '',
            vid: '',
            smfil: '',
            userid: '',
            departmentbase: ''
        };

        $scope.SelectedOrganization.ID = -1;
        $scope.SelectedOrganization.Name_Org = '';

        $scope.DataPos = '';
        $scope.Num_Doc = '';
        $scope.Name_Dov = '';

        $scope.ActionType = 'Добавить';
    }

    $scope.AddNakladnoyClicked = function () {

        //alertify.error("AddNakladnoy");

        $scope.CurrentNakladnoy.data_post = $scope.DataPos;

        $scope.CurrentNakladnoy.num_doc = $scope.Num_Doc;

        $scope.CurrentNakladnoy.id_org = $scope.SelectedOrganization.ID;

        $scope.CurrentNakladnoy.name_dov = $scope.Name_Dov;

        $scope.CurrentNakladnoy.id_kv = $scope.SelectedKvartal.ID;

        $scope.CurrentNakladnoy.smfil = $scope.SelectedYear.Id;

        $http({
            method: "post",
            url: "/PrixodPutevok/AddViplMain",
            data: JSON.stringify($scope.CurrentNakladnoy),
            dataType: "json"
        }).then(function (msg) {

            GetNakladnoyList();

        }, function () {
            alertify.error("ошибка");
            console.log('Error in AddNakladnoyClicked');
        });
    }

    $scope.EditNakladnoy = function () {

        //alertify.error("EditNakladnoy");

        $scope.CurrentNakladnoy.data_post = $scope.DataPos;

        $scope.CurrentNakladnoy.num_doc = $scope.Num_Doc;

        $scope.CurrentNakladnoy.id_org = $scope.SelectedOrganization.ID;

        $scope.CurrentNakladnoy.name_dov = $scope.Name_Dov;

        $scope.CurrentNakladnoy.id_kv = $scope.SelectedKvartal.ID;

        $scope.CurrentNakladnoy.smfil = $scope.SelectedYear.Id;

        $http({
            method: "post",
            url: "/PrixodPutevok/ModifyViplMain",
            data: JSON.stringify($scope.CurrentNakladnoy),
            dataType: "json"
        }).then(function (msg) {

            GetNakladnoyList();

        }, function () {
            alertify.error("ошибка");
            console.log('Error in AddNakladnoyClicked');
        });
    }

    $scope.MakeProvodka = function () {

        //alertify.error("MakeProvodka");

        $scope.CurrentNakladnoy.data_post = $scope.DataPos;

        $scope.CurrentNakladnoy.num_doc = $scope.Num_Doc;

        $scope.CurrentNakladnoy.id_org = $scope.SelectedOrganization.ID;

        $scope.CurrentNakladnoy.name_dov = $scope.Name_Dov;

        $scope.CurrentNakladnoy.id_kv = $scope.SelectedKvartal.ID;

        $scope.CurrentNakladnoy.smfil = $scope.SelectedYear.Id;

        $http({
            method: "post",
            url: "/PrixodPutevok/CloseViplNakladnoy",
            data: JSON.stringify($scope.CurrentNakladnoy),
            dataType: "json"
        }).then(function (msg) {

            GetNakladnoyList();

        }, function () {
            alertify.error("ошибка");
            console.log('Error in MakeProvodka');
        });
    }

    $scope.RemoveNakladnoy = function () {

        $scope.CurrentNakladnoy.data_post = $scope.DataPos;

        $scope.CurrentNakladnoy.num_doc = $scope.Num_Doc;

        $scope.CurrentNakladnoy.id_org = $scope.SelectedOrganization.ID;

        $scope.CurrentNakladnoy.name_dov = $scope.Name_Dov;

        $scope.CurrentNakladnoy.id_kv = $scope.SelectedKvartal.ID;

        $scope.CurrentNakladnoy.smfil = $scope.SelectedYear.Id;

        $http({
            method: "post",
            url: "/PrixodPutevok/RemoveViplMain",
            data: JSON.stringify($scope.CurrentNakladnoy),
            dataType: "json"
        }).then(function (msg) {

            $scope.NakladnoyDtls = '';
            GetNakladnoyList();

        }, function () {
            alertify.error("ошибка");
            console.log('Error in RemoveNakladnoy');
        });
    }

    $scope.SelectedPutyovkas = [];

    //Rasxod List
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

        var postData = { putyovkaL: $scope.SelectedPutyovkas };

        $http({
            method: "post",
            url: "/PrixodPutevok/RemoveViplPutyovkaDetail",
            data: JSON.stringify(postData),
            dataType: "json"
        }).then(function (response) {

            for (var i = 0, l = $scope.SelectedPutyovkas.length; i < l; ++i) {
                var value = $scope.SelectedPutyovkas[i];

                $scope.SelectedPutyovkas.splice($scope.SelectedPutyovkas.indexOf(value), 1);
            }

            GetDetailList();
            GetDetailListRes();

        }, function () {
            alertify.error("ошибка");
            console.log('Error in UdalitPutyovku');
        });
    }

    ////////////////////////////////////////////////////////////
    ///  Add Detail
    ////////////////////////////////////////////////////////////
    $scope.SelectedPutyovka = [];
    $scope.SelectedPutyovkaCount = 0;

    //For Prixod Putyovka
    $scope.toggleObjSelectionPut = function ($event, prixodPutId) {
        $event.stopPropagation();
        console.log('checkbox putyovka' + prixodPutId);

        var isDelated = false;

        for (var i = 0, l = $scope.SelectedPutyovka.length; i < l; ++i) {
            var value = $scope.SelectedPutyovka[i];

            if (prixodPutId === value) {
                $scope.SelectedPutyovka.splice($scope.SelectedPutyovka.indexOf(prixodPutId), 1);
                isDelated = true;
                break;
            }
        }

        if (!isDelated) {
            $scope.SelectedPutyovka.push(prixodPutId);
        }

        $scope.SelectedPutyovkaCount = $scope.SelectedPutyovka.length;
        console.log('====================================================');

        for (var i = 0, l = $scope.SelectedPutyovka.length; i < l; ++i) {
            var value = $scope.SelectedPutyovka[i];
            console.log('putyovka: ' + value);
        }
    }

    function GetNewPutyovkaList() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetNewPutyovkaList'
        }).then(function (response) {

            $scope.PrixodPutyovkas = response.data;
            $scope.PrixodPutyovkasCount = $scope.PrixodPutyovkas.length;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetNewPutyovkaList");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.SavePutDtl = function () {

        var postData = { modModel: $scope.SelectedPutyovka, IdViplMain: $scope.CurrentNakladnoy.id };

        $http({
            method: "post",
            url: "/PrixodPutevok/AddViplPut2",
            data: JSON.stringify(postData),
            dataType: "json"
        }).then(function (response) {

            for (var i = 0, l = $scope.SelectedPutyovkas.length; i < l; ++i) {
                var value = $scope.SelectedPutyovkas[i];

                $scope.SelectedPutyovkas.splice($scope.SelectedPutyovkas.indexOf(value), 1);
            }

            $scope.SelectedPutyovkaCount = 0;
            GetDetailList();
            GetTotalResults();
            GetDetailListRes();

        }, function () {
            alertify.error("ошибка");
            console.log('Error in SavePutDtl');
        });
    }

    $scope.AddPutyovkas = function ()
    {

        for (var i = 0, l = $scope.SelectedPutyovka.length; i < l; ++i)
        {
            var value = $scope.SelectedPutyovka[i];

            $scope.SelectedPutyovkas.splice($scope.SelectedPutyovka.indexOf(value), 1);
        }

        GetNewPutyovkaList();

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