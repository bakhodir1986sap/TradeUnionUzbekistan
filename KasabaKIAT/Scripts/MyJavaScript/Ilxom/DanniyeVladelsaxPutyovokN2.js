app.controller("DanniyeVladelsaxPutyovok", ["$scope", "$http", "$filter", function ($scope, $http, $filter) {

    ///////////////////////////////////////////////////////////////////////////
    /// Common Part
    ///////////////////////////////////////////////////////////////////////////

    //Initialize Part
    GetYearsList();
    GetKvartalList();
    GetOrganizationList();
    GetPol();
    GetOsnovaniyaVidachi();
    GetTipVidachi();
    GetProcentZaPutyovki();
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

    $scope.SelectedPol = {
        Code: '',
        Decode:''
    };

    $scope.SelectedOsnovaniyaVidachi = {
        ID      :'',    
        Name_Os :''
    };

    $scope.SelectedTipVidachi = {
        ID: '',
        Name_Os: ''
    };

    $scope.SelectedProcentZaPutyovki = {
        ID          :'', 
        Name_Proc   :''
    };

    $scope.SelectedRecommendedSanatory = {
        ID          :'',
        Name_San    :''
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

    function GetPol() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Reference/GetPol'
        }).then(function (response) {
            $scope.Pols = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetPol");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function GetOsnovaniyaVidachi() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Reference/GetOsnovaniyaVidachi'
        }).then(function (response) {
            $scope.Osnovaniyas = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetOsnovaniyaVidachi");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function GetTipVidachi() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Reference/GetTipVidachi'
        }).then(function (response) {
            $scope.TipVidachies = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetTipVidachi");
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
            $scope.Procents = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetProcentZaPutyovki");
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
            url: '/Reference/GetRecommendedSanatory'
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

    $scope.SelectedBeginDate = new Date();
    $scope.SelectedEndDate = new Date();

    function GetDanniyVladelsaNakladnoy() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        var postData = {
            beginDt: $scope.SelectedBeginDate
            , endDt: $scope.SelectedEndDate
            , IdOrg: $scope.SelectedOrganization.ID
        };

        $http({
            method: "post",
            url: "/PrixodPutevok/GetDanniyVladelsaNakladnoy",
            data: JSON.stringify(postData),
            dataType: "json"
        }).then(function (response) {

            $scope.Nakladnoys = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');

        }, function () {
            alertify.error("ошибка");
            console.log("error in GetDanniyVladelsaNakladnoy");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    
    function GetDanniyVladelsaPutyovki() {

        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        var postData = {
            beginDt: $scope.SelectedBeginDate
            , endDt: $scope.SelectedEndDate
            , IdOrg: $scope.SelectedOrganization.ID
        };

        $http({
            method: "post",
            url: "/PrixodPutevok/GetDanniyVladelsaPutyovki",
            data: JSON.stringify(postData),
            dataType: "json"
        }).then(function (response) {

            $scope.Putyovkas = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');

        }, function () {
            alertify.error("ошибка");
            console.log("error in GetDanniyVladelsaNakladnoy");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    
    function GetDanniyTotal() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        var postData = {
            beginDt: $scope.SelectedBeginDate
            , endDt: $scope.SelectedEndDate
            , IdOrg: $scope.SelectedOrganization.ID
        };

        $http({
            method: "post",
            url: "/PrixodPutevok/GetDanniyTotal",
            data: JSON.stringify(postData),
            dataType: "json"
        }).then(function (response) {

            $scope.TotalRes = response.data;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');

        }, function () {
            alertify.error("ошибка");
            console.log("error in GetDanniyVladelsaNakladnoy");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    
    function GetFaceData(IdPut) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetEmptyForm?IdPut=' + IdPut
        }).then(function (response) {

            $scope.RaznoskaPutyovok = response.data;
            $scope.CurrentFace = $scope.RaznoskaPutyovok.faceMain;
            $scope.CurrentFaceMed = $scope.RaznoskaPutyovok.faceMedDtl;

            //$scope.CurrentRealizasiyaPutyovka.id = $scope.CurrentFace.id_put;
            //$scope.CurrentRealizasiyaPutyovka.id_sanat =$scope.CurrentFace.id_sanat;
            //$scope.CurrentRealizasiyaPutyovka.num_put = $scope.CurrentFace.num_put;
            $scope.SelectedTipVidachi.ID = $scope.CurrentFace.stav_vid;
            $scope.SelectedProcentZaPutyovki.ID = $scope.CurrentFace.gost_pr;
            $scope.SelectedOsnovaniyaVidachi.ID = $scope.CurrentFace.id_osnov;

            var data1 = ($scope.CurrentFace.pas_data) ? $filter('date')(parseInt($scope.CurrentFace.pas_data.substring(6)), 'dd.MM.yyyy') : '';
            $scope.CurrentFace.pas_data = new Date(data1.substring(6, 10), data1.substring(3, 5) - 1, data1.substring(0, 2));

            var data2 = ($scope.CurrentFaceMed.med_data) ? $filter('date')(parseInt($scope.CurrentFaceMed.med_data.substring(6)), 'dd.MM.yyyy') : '';
            $scope.CurrentFaceMed.med_data = new Date(data2.substring(6, 10), data2.substring(3, 5) - 1, data2.substring(0, 2));

            var data3 = ($scope.CurrentFaceMed.opl_data) ? $filter('date')(parseInt($scope.CurrentFaceMed.opl_data.substring(6)), 'dd.MM.yyyy') : '';
            $scope.CurrentFaceMed.opl_data = new Date(data3.substring(6, 10), data3.substring(3, 5) - 1, data3.substring(0, 2));

            var data4 = ($scope.CurrentFaceMed.tal_data) ? $filter('date')(parseInt($scope.CurrentFaceMed.tal_data.substring(6)), 'dd.MM.yyyy') : '';
            $scope.CurrentFaceMed.tal_data = new Date(data4.substring(6, 10), data4.substring(3, 5) - 1, data4.substring(0, 2));


            if ($scope.CurrentFace.pol)
            {
                $scope.SelectedPol.Code = 1;
            }
            else
            {
                $scope.SelectedPol.Code = 0;
            }

            $scope.SelectedRecommendedSanatory.ID = $scope.CurrentFaceMed.med_san;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetDanniyTotal");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.ShowClicked = function ()
    {
        $scope.Nakladnoys = '';
        $scope.Putyovkas = '';
        $scope.TotalRes = '';

        GetDanniyVladelsaNakladnoy();
        GetDanniyVladelsaPutyovki();
        GetDanniyTotal();
    }

    $scope.CurrentFace = {

        id              :'',     
        id_put          :'',
        id_sanat        :'',
        num_put         :'',
        lastname        :'',
        name            :'',
        firstname       :'',
        adress          :'',
        rabota          :'',
        dolj            :'',
        stav_vid        :'',
        stav_vidNm      :'',
        pas_ser         :'',
        pas_num         :'',
        pas_kd          :'',
        pas_data        :'',
        gost_pr         :'',
        gost_prNm       :'',
        id_osnov        :'',
        id_osnovNm      :'',
        data_post       :'',
        pol             :'',
        userid          :'',
        departmentbase  :'',
        inn             :'',
        pnfl            :''
    };

    $scope.CurrentFaceMed = {

        id                  :'',       
        id_face             :'',
        med_doc             :'',
        med_data            :'',
        med_san             :'',
        opl_doc             :'',
        opl_data            :'',
        opl_symma           :'',
        tal_doc             :'',
        tal_data            :'',
        userid              :'',
        departmentbase      :''
    };

    $scope.RowClicked = function (puyovka)
    {
        $scope.CurrentRealizasiyaPutyovka = puyovka;
        $(".activvvv").removeClass("activvvv");
        $("#"+ puyovka.id).addClass("activvvv");
        GetFaceData(puyovka.id);
    }

    $scope.SaveFaceData = function () {

        $scope.CurrentFace.id_put = $scope.CurrentRealizasiyaPutyovka.id;
        $scope.CurrentFace.id_sanat = $scope.CurrentRealizasiyaPutyovka.id_sanat;
        $scope.CurrentFace.num_put = $scope.CurrentRealizasiyaPutyovka.num_put;
        $scope.CurrentFace.stav_vid = $scope.SelectedTipVidachi.ID;
        $scope.CurrentFace.gost_pr = $scope.SelectedProcentZaPutyovki.ID;
        $scope.CurrentFace.id_osnov = $scope.SelectedOsnovaniyaVidachi.ID
        $scope.CurrentFace.pol = $scope.SelectedPol.Code;

        $scope.CurrentFaceMed.med_san = $scope.SelectedRecommendedSanatory.ID;

        //long IdViplPut , FaceViewModel faceModel, Face_MedViewModel fmModel

        var postData = {
            IdViplPut: $scope.CurrentRealizasiyaPutyovka.id
            , faceModel: $scope.CurrentFace
            , fmModel: $scope.CurrentFaceMed
        };

        $http({
            method: "post",
            url: "/PrixodPutevok/SaveChanges2",
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

        $scope.GetOrganizationsFilterF = function () {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetFizLisoFilter?Name=' + $scope.FilterName + '&Inn=' + $scope.FilterInn + '&Pas_ser=' + $scope.FilterSeria + '&Pas_Number=' + $scope.FilterNomer
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