app.controller("CampZayavki", ["$scope", "$http", "$filter", function ($scope, $http, $filter) {

    ///////////////////////////////////////////////////////////////////////////
    /// Common Part
    ///////////////////////////////////////////////////////////////////////////

    GetOtraslForOrganization();
    GetCamp();
    GetLagerZayavki();

    $scope.REC_ID           = '';
    $scope.ID_OTRAS         = '';
    $scope.OTRALSNAME       = '';
    $scope.ID_CAMP          = '';
    $scope.CAMPNAME         = '';
    $scope.QUANTUTY         = '';
    $scope.TOTALAMOUNT      = '';
    $scope.TIMESTAMP        = '';
    $scope.STATUS           = '';
    $scope.DEPARTMENT_CODE  = '';
    $scope.USERID           = '';

    $scope.FilterSelectedOtrasl = {
        Name_Ot: '',
        ID_Ot: ''
    };

    $scope.ModalOtraslSelected = {
        Name_Ot: '',
        ID_Ot: ''
    };

    $scope.FilterSelectedCamp = {
        id: '',
        value: ''
    };

    $scope.ModalCampSelected = {
        id: '',
        value: ''
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

    function GetCamp() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Home/GetCamp'
        }).then(function (response) {
            $scope.Camps = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRekvizits");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }


    /////////////////////////////////////////////////////////////////////////////////////////
    //// Spesific Business Logic
    /////////////////////////////////////////////////////////////////////////////////////////
    //Master Row Selected


    $scope.CurrentCampZayavki = {
        rec_id          :'',     
        id_otras        :'',
        otralsName      :'',
        id_camp         :'',
        campName        :'',
        quantuty        :'',
        totalamount     :'',
        timestamp       :'',
        status          :'',
        department_code :'',
        userid          :''
    };

    function GetLagerZayavki() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/CampZayavki/GetLagerNewZayavkaList'
        }).then(function (response) {
            $scope.LagerNewZayavkas = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRekvizits");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.edit = function (idSelectedVote) {

        $scope.REC_ID = idSelectedVote.rec_id;         
        $scope.ID_OTRAS = idSelectedVote.id_otras;       
        $scope.OTRALSNAME = idSelectedVote.otralsName;   

        $scope.ModalOtraslSelected.Name_Ot = idSelectedVote.otralsName;   
        $scope.ModalOtraslSelected.ID_Ot = idSelectedVote.id_otras;

        $scope.ID_CAMP = idSelectedVote.id_camp;        
        $scope.CAMPNAME = idSelectedVote.campName; 

        $scope.ModalCampSelected.id = idSelectedVote.id_camp;  
        $scope.ModalCampSelected.value = idSelectedVote.campName;

        $scope.QUANTUTY = idSelectedVote.quantuty;       
        $scope.TOTALAMOUNT = idSelectedVote.totalamount;    
        $scope.TIMESTAMP = idSelectedVote.timestamp;      
        $scope.STATUS = idSelectedVote.status;         
        $scope.DEPARTMENT_CODE = idSelectedVote.department_code;
        $scope.USERID = idSelectedVote.userid;       

        $scope.ActionType = 'Редактировать';
    }


    $scope.AddZayavkuClicked = function () {

        $scope.CurrentCampZayavki.rec_id = $scope.REC_ID;         
        $scope.CurrentCampZayavki.id_otras          =   $scope.ID_OTRAS         ;
        $scope.CurrentCampZayavki.otralsName        =   $scope.OTRALSNAME       ;
        $scope.CurrentCampZayavki.id_camp           =   $scope.ID_CAMP          ;
        $scope.CurrentCampZayavki.campName          =   $scope.CAMPNAME         ;
        $scope.CurrentCampZayavki.quantuty          =   $scope.QUANTUTY         ;
        $scope.CurrentCampZayavki.totalamount       =   $scope.TOTALAMOUNT      ;
        $scope.CurrentCampZayavki.timestamp         =   $scope.TIMESTAMP        ;
        $scope.CurrentCampZayavki.status            =   $scope.STATUS           ;
        $scope.CurrentCampZayavki.department_code   =   $scope.DEPARTMENT_CODE  ;
        $scope.CurrentCampZayavki.userid = $scope.USERID;

        $scope.CurrentCampZayavki.id_otras = $scope.ModalOtraslSelected.ID_Ot;
        $scope.CurrentCampZayavki.id_camp = $scope.ModalCampSelected.id;

        $http({
            method: "post",
            url: "/CampZayavki/AddZayavku",
            data: JSON.stringify($scope.CurrentCampZayavki),
            dataType: "json"
        }).then(function (response) {

            alertify.success(response.data);
            GetLagerZayavki();

        }, function () {
            alertify.error("ошибка");
            console.log('Error in AddNakladnoyClicked');
        });
    }


    $scope.ModifyZayavku = function () {

        $scope.CurrentCampZayavki.rec_id = $scope.REC_ID;
        $scope.CurrentCampZayavki.id_otras = $scope.ID_OTRAS;
        $scope.CurrentCampZayavki.otralsName = $scope.OTRALSNAME;
        $scope.CurrentCampZayavki.id_camp = $scope.ID_CAMP;
        $scope.CurrentCampZayavki.campName = $scope.CAMPNAME;
        $scope.CurrentCampZayavki.quantuty = $scope.QUANTUTY;
        $scope.CurrentCampZayavki.totalamount = $scope.TOTALAMOUNT;
        $scope.CurrentCampZayavki.timestamp = $scope.TIMESTAMP;
        $scope.CurrentCampZayavki.status = $scope.STATUS;
        $scope.CurrentCampZayavki.department_code = $scope.DEPARTMENT_CODE;
        $scope.CurrentCampZayavki.userid = $scope.USERID;


        $scope.CurrentCampZayavki.id_otras = $scope.ModalOtraslSelected.ID_Ot;
        $scope.CurrentCampZayavki.id_camp = $scope.ModalCampSelected.id;

        $http({
            method: "post",
            url: "/CampZayavki/ModifyZayavku",
            data: JSON.stringify($scope.CurrentCampZayavki),
            dataType: "json"
        }).then(function (response) {

            alertify.success(response.data);
            GetLagerZayavki();

        }, function () {
            alertify.error("ошибка");
            console.log('Error in AddNakladnoyClicked');
        });
    }



    $scope.DeleteZayavku = function () {

        $scope.CurrentCampZayavki.rec_id = $scope.REC_ID;
        $scope.CurrentCampZayavki.id_otras = $scope.ID_OTRAS;
        $scope.CurrentCampZayavki.otralsName = $scope.OTRALSNAME;
        $scope.CurrentCampZayavki.id_camp = $scope.ID_CAMP;
        $scope.CurrentCampZayavki.campName = $scope.CAMPNAME;
        $scope.CurrentCampZayavki.quantuty = $scope.QUANTUTY;
        $scope.CurrentCampZayavki.totalamount = $scope.TOTALAMOUNT;
        $scope.CurrentCampZayavki.timestamp = $scope.TIMESTAMP;
        $scope.CurrentCampZayavki.status = $scope.STATUS;
        $scope.CurrentCampZayavki.department_code = $scope.DEPARTMENT_CODE;
        $scope.CurrentCampZayavki.userid = $scope.USERID;


        $scope.CurrentCampZayavki.id_otras = $scope.ModalOtraslSelected.ID_Ot;
        $scope.CurrentCampZayavki.id_camp = $scope.ModalCampSelected.id;

        $http({
            method: "post",
            url: "/CampZayavki/DeleteZayavku",
            data: JSON.stringify($scope.CurrentCampZayavki),
            dataType: "json"
        }).then(function (response) {

            alertify.success(response.data);
            GetLagerZayavki();

        }, function () {
            alertify.error("ошибка");
            console.log('Error in AddNakladnoyClicked');
        });
    }

    ///////////////////////////////////////////////////////////////////
    ////Organization///////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////

    GetOtraslForOrganization();

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