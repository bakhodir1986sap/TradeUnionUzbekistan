app.controller("Organizasii", ["$scope", "$http", "$filter", function ($scope, $http, $filter) {

    GetOtraslForOrganization();
    GetRegionForOrganization();
    GetEmptyCarcas();
    GetOrganizations();

    $scope.FilterSelectedOtrasl = {
        Name_Ot: '',
        ID_Ot:''
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

    $scope.SelectedOrganization = {

        ID: -1,
        Name_Org: ''

    };

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

            $scope.SelectedOrganization.ID = org.ParentId;
            $scope.SelectedOrganization.Name_Org = org.ParentName;

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
            url: '/PrixodPutevok/GetOrganizationFilter?Name=' + $scope.FilterName + '&Inn=' + $scope.FilterInn + '&OtrasId='+ $scope.FilterSelectedOtrasl.ID_Ot
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
        $scope.Organization.ParentId = $scope.SelectedOrganization.ID;

        $http({
            method: 'post',
            url: '/PrixodPutevok/SaveOrganization',
            data: JSON.stringify($scope.Organization),
            contentType: "application/json; charset=utf-8",
        }).then(function (response) {
            GetOrganizations();
            alertify.success("Успешное сохранение");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in SaveRekvizit");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

}]);