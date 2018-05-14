app.controller("PrixodPutyovokRuchnoy", ["$scope", "$http", "$filter", function ($scope, $http, $filter) {

    //Initialize Part
    GetYearsList();
    GetKvartalList();
    GetOrganizationList();
    GetSanatories();
    

    //Table Row highlight
    $scope.idSelectedVote = null;
    $scope.editSelected = function (idSelectedVote) {
        $scope.idSelectedVote = idSelectedVote.Id;
        $scope.CurrentNakladnoy = idSelectedVote;

        $scope.ActionType = 'Редактировать';

        //Data Pos
        var dataPos = (idSelectedVote.Data_Post) ? $filter('date')(parseInt(idSelectedVote.Data_Post.substr(6)), 'dd.MM.yyyy') : '';
        $scope.DataPos = new Date(dataPos.substring(6, 10), dataPos.substring(3, 5) - 1, dataPos.substring(0, 2));

        //Number Putyovka
        $scope.Num_Doc = $scope.CurrentNakladnoy.Num_Doc;

        //Organization
        $scope.SelectedOrganization.ID = $scope.CurrentNakladnoy.ID_Pos;
        $scope.SelectedOrganization.Name_Org = $scope.CurrentNakladnoy.PosName;

        //Doverennost
        $scope.Name_Dov = $scope.CurrentNakladnoy.Name_Dov;

        $scope.NakladnoyDtls = '';
        GetDetailList();
        GetDetailListRes();
    };

    //Models
    $scope.SelectedYear = {
        Id: 2018,
        Year: '2018'
    };

    $scope.SelectedKvartal = {
        ID: 2,
        Name_Kv: '2-квартал'
    };

    GetData();

    $scope.TotalResults = {

        NakladnoyCount: 0,
        PutyovkaCount: 0,
        SummaNakladnix: 0

    };

    $scope.CurrentNakladnoy = {

        Id    :'',         
        Id_Kv: '',       
        Kvartal:'',        
        Data_Post: '',      
        Num_Doc:'',        
        ID_Pos:0,         
        PosName:'',        
        Num_Kol:'',        
        Symma:'',          
        Name_Dov:'',       
        Vid:'',            
        SmFil:'',          
        UserId:'',         
        DepartmentBase:'', 
        BeginNumber:'',    
        EndNumber: ''
    };

    $scope.SelectedOrganization = {

        ID: -1,
        Name_Org: ''
        
    };

    $scope.CurrentPutyovka = {

        Id              :'',
        IdPost          :'',
        IdSanat         :'',
        SanatName       :'',
        Num_Put         :'',
        DataSan         :'',
        DataSanEnd      :'',
        Symma_Put       :'',
        ID_PolVoz       :'',
        Data_Post       :'',
        UserId          :'',
        Departmentbase  :'',
        Checked         :''
    };


    $scope.ZagatovkaPutyovka = {

        Year: '',        
        
        Kvartal: '',       
        
        SanatId: '',   

        SanatName:'',
        
        beginNumber: '',   
        
        endNumber:'',     
        
        SanatDate: '',     
        
        Price:'',         
        
        DepartmentBase:'',
        
        UserId:'',        
        
        IdPost:''        

    };

    $scope.SelectedSanatory = {

        ID: 0,
        Name_San: ''

    };


    //Get Functions
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

    function GetTotalResults() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetTotalResults?year=' + $scope.SelectedYear.Id + '&kvartal=' + $scope.SelectedKvartal.ID
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
    
    function GetNakladnoyList() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetMainList?year=' + $scope.SelectedYear.Id + '&kvartal=' + $scope.SelectedKvartal.ID
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

    function GetDetailList() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetDetailList?IdPost=' + $scope.CurrentNakladnoy.Id 
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

    function GetDetailListRes() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetDetailListRes?IdPost=' + $scope.CurrentNakladnoy.Id
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

            Id: '',
            Id_Kv: '',
            Kvartal: '',
            Data_Post: '',
            Num_Doc: '',
            ID_Pos: 0,
            PosName: '',
            Num_Kol: '',
            Symma: '',
            Name_Dov: '',
            Vid: '',
            SmFil: '',
            UserId: '',
            DepartmentBase: '',
            BeginNumber: '',
            EndNumber: ''
        };

        $scope.SelectedOrganization.ID = -1;
        $scope.SelectedOrganization.Name_Org = '';

        $scope.DataPos = '';
        $scope.Num_Doc = '';
        $scope.Name_Dov = '';

        $scope.ActionType = 'Добавить';
    }
        
    //eventsHandlers
    $scope.FiltersChanged = function () {

        if (($scope.SelectedYear.Id > -1) && ($scope.SelectedKvartal.ID > 0))
        {
            GetTotalResults();
            GetNakladnoyList();
        }

    }

    function GetData() {

        if (($scope.SelectedYear.Id > -1) && ($scope.SelectedKvartal.ID > 0)) {
            GetTotalResults();
            GetNakladnoyList();
        }

    }

    $scope.AddNakladnoyClicked = function () {

        //alertify.error("AddNakladnoy");

        $scope.CurrentNakladnoy.Data_Post = $scope.DataPos;

        $scope.CurrentNakladnoy.Num_Doc = $scope.Num_Doc;

        $scope.CurrentNakladnoy.ID_Pos = $scope.SelectedOrganization.ID;

        $scope.CurrentNakladnoy.Name_Dov = $scope.Name_Dov;

        $scope.CurrentNakladnoy.Id_Kv = $scope.SelectedKvartal.ID;

        $scope.CurrentNakladnoy.SmFil = $scope.SelectedYear.Id;

        $http({
            method: "post",
            url: "/PrixodPutevok/AddMain",
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

        //alertify.error("AddNakladnoy");

        $scope.CurrentNakladnoy.Data_Post = $scope.DataPos;

        $scope.CurrentNakladnoy.Num_Doc = $scope.Num_Doc;

        $scope.CurrentNakladnoy.ID_Pos = $scope.SelectedOrganization.ID;

        $scope.CurrentNakladnoy.Name_Dov = $scope.Name_Dov;

        $scope.CurrentNakladnoy.Id_Kv = $scope.SelectedKvartal.ID;

        $scope.CurrentNakladnoy.SmFil = $scope.SelectedYear.Id;

        $http({
            method: "post",
            url: "/PrixodPutevok/ModifyMain",
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

        $scope.CurrentNakladnoy.Data_Post = $scope.DataPos;

        $scope.CurrentNakladnoy.Num_Doc = $scope.Num_Doc;

        $scope.CurrentNakladnoy.ID_Pos = $scope.SelectedOrganization.ID;

        $scope.CurrentNakladnoy.Name_Dov = $scope.Name_Dov;

        $scope.CurrentNakladnoy.Id_Kv = $scope.SelectedKvartal.ID;

        $scope.CurrentNakladnoy.SmFil = $scope.SelectedYear.Id;

        $http({
            method: "post",
            url: "/PrixodPutevok/CloseNakladnoy",
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

        $scope.CurrentNakladnoy.Data_Post = $scope.DataPos;

        $scope.CurrentNakladnoy.Num_Doc = $scope.Num_Doc;

        $scope.CurrentNakladnoy.ID_Pos = $scope.SelectedOrganization.ID;

        $scope.CurrentNakladnoy.Name_Dov = $scope.Name_Dov;

        $scope.CurrentNakladnoy.Id_Kv = $scope.SelectedKvartal.ID;

        $scope.CurrentNakladnoy.SmFil = $scope.SelectedYear.Id;

        $http({
            method: "post",
            url: "/PrixodPutevok/RemoveAllWithLinked",
            data: JSON.stringify($scope.CurrentNakladnoy),
            dataType: "json"
        }).then(function (msg) {

            GetNakladnoyList();

        }, function () {
            alertify.error("ошибка");
            console.log('Error in AddNakladnoyClicked');
        });
    }


    $scope.PutyovkaChecked = function (dtl)
    {

    }

    $scope.SelectedPutyovkas = [];

    $scope.toggleObjSelection = function ($event, dtl) {
        $event.stopPropagation();
        console.log('checkbox clicked' + dtl);

        var isDelated = false;

        for (var i = 0, l = $scope.SelectedPutyovkas.length; i < l; ++i) {
            var value = $scope.SelectedPutyovkas[i];

            if (dtl === value)
            {
                $scope.SelectedPutyovkas.splice($scope.SelectedPutyovkas.indexOf(value), 1);
                isDelated = true;
                break;
            }
        }

        if (!isDelated)
        {
            $scope.SelectedPutyovkas.push(dtl);
        }

        console.log('====================================================');

        for (var i = 0, l = $scope.SelectedPutyovkas.length; i < l; ++i) {
            var value = $scope.SelectedPutyovkas[i];
            console.log('tutuud: ' + value);
        }
    }


    ////////////////////////////////////////////////////////////////
    // Add putyovka Details
    ///////////////////////////////////////////////////////////////

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

    $scope.PrinyatClicked = function () {

        $scope.ZagatovkaPutyovka.Kvartal = $scope.SelectedKvartal.ID;

        $scope.ZagatovkaPutyovka.Year = $scope.SelectedYear.Id;

        $scope.ZagatovkaPutyovka.SanatId = $scope.SelectedSanatory.ID;

        $scope.ZagatovkaPutyovka.SanatName = $scope.SelectedSanatory.Name_San;

        $scope.ZagatovkaPutyovka.IdPost = $scope.CurrentNakladnoy.Id;

        $http({
            method: "post",
            url: "/PrixodPutevok/ParsePrixodPutyovka",
            data: JSON.stringify($scope.ZagatovkaPutyovka),
            dataType: "json"
        }).then(function (response) {

            $scope.PutyovkaList = response.data;

        }, function () {
            alertify.error("ошибка");
            console.log('Error in PrinyatClicked');
        });
    }

    $scope.SelectedPutyovka = [];


    $scope.toggleObjSelectionPut = function ($event, Num_Put, IdSanat) {
        $event.stopPropagation();
        console.log('checkbox putyovka' + IdSanat + '-' +  Num_Put);

        var isDelated = false;

        for (var i = 0, l = $scope.SelectedPutyovka.length; i < l; ++i) {
            var value = $scope.SelectedPutyovka[i];

            if ((Num_Put + '_' + IdSanat) === value) {
                $scope.SelectedPutyovka.splice($scope.SelectedPutyovka.indexOf(Num_Put + '_' + IdSanat), 1);
                isDelated = true;
                break;
            }
        }

        if (!isDelated) {
            $scope.SelectedPutyovka.push(Num_Put + '_' + IdSanat);
        }

        console.log('====================================================');

        for (var i = 0, l = $scope.SelectedPutyovka.length; i < l; ++i) {
            var value = $scope.SelectedPutyovka[i];
            console.log('putyovka: ' + value);
        }
    }

    $scope.SavePutDtl = function ()
    {
        for (var i = 0, l = $scope.PutyovkaList.length; i < l; ++i)
        {

            var dataPos = ($scope.PutyovkaList[i].Data_Post) ? $filter('date')(parseInt($scope.PutyovkaList[i].Data_Post.substr(6)), 'dd.MM.yyyy') : '';
            var dataPosd = new Date(parseInt(dataPos.substring(6, 10)), parseInt(dataPos.substring(3, 5) - 1), parseInt(dataPos.substring(0, 2)));

            $scope.PutyovkaList[i].Data_Post = dataPosd;

            var dataSan = ($scope.PutyovkaList[i].DataSan) ? $filter('date')(parseInt($scope.PutyovkaList[i].DataSan.substr(6)), 'dd.MM.yyyy') : '';
            var dataSand = new Date(parseInt(dataSan.substring(6, 10)), parseInt(dataSan.substring(3, 5) - 1), parseInt(dataSan.substring(0, 2)));

            $scope.PutyovkaList[i].DataSan = dataSand;
        }


        var postData = { lModel: $scope.PutyovkaList, expLst: $scope.SelectedPutyovka };

        $http({
            method: "post",
            url: "/PrixodPutevok/AddDetail",
            data: JSON.stringify(postData),
            dataType: "json"
        }).then(function (response) {

            //

        }, function () {
            alertify.error("ошибка");
            console.log('Error in SavePutDtl');
        });
    }


    $scope.AddPutyovkas = function () {

        $scope.ZagatovkaPutyovka = {
            Year: '',
            Kvartal: '',
            SanatId: '',
            SanatName: '',
            beginNumber: '',
            endNumber: '',
            SanatDate: '',
            Price: '',
            DepartmentBase: '',
            UserId: '',
            IdPost: ''
        };

        $scope.PutyovkaList = '';

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