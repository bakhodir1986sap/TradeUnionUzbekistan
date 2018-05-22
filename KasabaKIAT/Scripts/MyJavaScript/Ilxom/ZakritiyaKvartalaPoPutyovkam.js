app.controller("ZakritiyaKvartalaPoPutyovkam", ["$scope", "$http", "$filter", function ($scope, $http, $filter) {

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

    $scope.ZakritiyeKvartalaViewModelTemp = {

        year  :'',
        id_kv :'',
        IdOrg :''
    };

    function GetZakritiyaPutyovkas() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetZakritiyaPutyovkas?idOrg=' + $scope.SelectedOrganization.ID
        }).then(function (response) {

            $scope.Putyovkas = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetZakritiyaPutyovkas");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.OrgChanged = function ()
    {
        GetZakritiyaPutyovkas();
        GetZakritiyaPutyovkasRes();
    }

    function GetZakritiyaPutyovkasRes() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetZakritiyaPutyovkasRes?idOrg=' + $scope.SelectedOrganization.ID
        }).then(function (response) {

            $scope.ZakritiyeRes = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetZakritiyaPutyovkasRes");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
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

    
    $scope.SaveZakritieEnter = function () {

        $scope.ZakritiyeKvartalaViewModelTemp.IdOrg = $scope.SelectedOrganization.ID;
        $scope.ZakritiyeKvartalaViewModelTemp.id_kv = $scope.SelectedKvartal.ID;
        $scope.ZakritiyeKvartalaViewModelTemp.year = $scope.SelectedYear.Id;


        var postData = {
            mainMdl: $scope.ZakritiyeKvartalaViewModelTemp,
            lrasxm: $scope.SelectedPutyovkas
        };

        $http({
            method: "post",
            url: "/PrixodPutevok/ZakritKvartal2",
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
            GetZakritiyaPutyovkas();
            GetZakritiyaPutyovkasRes();
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