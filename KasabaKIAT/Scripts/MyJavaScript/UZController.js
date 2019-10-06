app.controller("UZController", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    GetUZ();
    GetRoles();
    GetRegion();
    GetOtrasl();
    GetOrg();
    function GetUZ() {
        $http({
            method: "post",
            url: "GetUZ",
        }).then(function (d) {
            $scope.UZ = d.data;
        }, function () {
            alertify.error("ошибка");
        });
    }
    function GetRoles() {
        $http({
            method: "post",
            url: "GetRoles",
        }).then(function (d) {
            $scope.Rl = d.data;
        }, function () {
            alertify.error("ошибка");
        });
    };
    function GetRegion() {
        $http({
            method: "post",
            url: "GetRegion",
        }).then(function (d) {
            $scope.Rg = d.data;
        }, function () {
            alertify.error("ошибка");
        });
    };
    function GetOtrasl() {
        $http({
            method: "post",
            url: "GetOtrasl",
        }).then(function (d) {
            $scope.Ot = d.data;
        }, function () {
            alertify.error("ошибка");
        });
    };
    function GetOrg() {
        $http({
            method: "post",
            url: "GelOrganizatsiya",
        }).then(function (d) {
            $scope.O = d.data;
        }, function () {
            alertify.error("ошибка");
        });
    };
    $scope.Roles = {
        id: '1',
        value: ''
    };
    $scope.Region = {
        id: 1726,
        value: ''
    };
    $scope.Ots = {
        id: '1',
        value: ''
    };
    $scope.Or = {
        ID: '1',
        Name_Org: ''
    };
    $scope.kurish = function () {
        $scope.parol = "text";
        if ($scope.ch == false) {
            $scope.ch = true;
            $scope.parol = "text";
        }
        else {
            $scope.ch = false;
            $scope.parol = "password";
        }
    };
    $scope.AddGPDiv = function () {
        $scope.parol = "password";
        $scope.ch = false;
        ClearFields();
        $scope.Action = "Добавить";
    }
    function ClearFields() {
        $scope.ID = 0;
        $scope.Region.id = 1726;
        $scope.Roles.id = 1;
        $scope.Ots.id = 1;
        $scope.Or.ID = 1;
        $scope.Or.Name_Org = "";
        $scope.L = "";
        $scope.P = "";
        $scope.PP = "";
        $scope.N = "";
    }
    $scope.edit = function (uz) {
        $scope.parol = "password";
        $scope.ch = false;
        $scope.ID = uz.ID;
        $scope.Region.id = uz.Rg_id;
        $scope.Region.value = uz.Rg;
        $scope.Roles.id = uz.Rl_id;
        $scope.Roles.value = uz.Rl;
        $scope.Ots.id = uz.Ot_id;
        $scope.Ots.value = uz.Ot;
        $scope.Or.ID = uz.O_id;
        $scope.Or.Name_Org = uz.O;
        $scope.L = uz.L;
        $scope.P = uz.P;
        $scope.PP = uz.P;
        $scope.N = uz.N;
        $scope.Action = "Изменить";
    }
    $scope.AddUpdateGP = function () {
        var getGPAction = $scope.Action;
        var GP={
            email: $scope.L,
            password: $scope.P,
            s_comment: $scope.N,
            bkut_id: $scope.Or.ID,
            role_id:$scope.Roles.id
            };
        if (getGPAction == "Изменить") {
            GP.id = $scope.ID;
            $http({
                method: "post",
                url: "/Administirirovaniye/UpdateUZ",
                data: JSON.stringify(GP),
                dataType: "json"
            }).then(function (msg) {
                GetUZ();
            }, function () {
                alertify.error("ошибка");
                console.log('Error in UpdateUZ');
                $("#hh").modal('hide');
            });
        }
        else {
            $http({
                method: "post",
                url: "/Administirirovaniye/AddUZ",
                data: JSON.stringify(GP),
                dataType: "json"
            }).then(function (msg) {
                GetUZ();
            }, function () {
                alertify.error("ошибка");
                console.log('Error in AddUZ');
                $("#hh").modal('hide');
            });
        }
    }
    $scope.deleteGP = function (ID) {
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите удалить?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                $http({
                    method: "post",
                    url: "/Administirirovaniye/DeleteUZ",
                    params: {
                        id: JSON.stringify(ID)
                    }
                }).then(function (msg) {
                    $("#hh").modal('hide');
                    GetUZ();
                }, function () {
                    alertify.error("ошибка");
                    console.log('Error DeleteUZ');
                    $("#hh").modal('hide');
                });
            } else {
                $("#hh").modal('hide');
                alertify.error("Операция отменена");
            }
        });
        return false;
    }


    ///////////////////////////////////////////////////////////////////
    ////Organization///////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////

    //GetOtraslForOrganization();
    //GetRegionForOrganization();
    //GetEmptyCarcas();
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
            url: '/PrixodPutevok/GetOrganizationFilterM?Name=' + $scope.FilterName + '&Inn=' + $scope.FilterInn + '&OtrasId=' + $scope.FilterSelectedOtrasl.ID_Ot
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
            $scope.Or.ID = Id;
            $scope.Or.Name_Org = Name_Org;
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