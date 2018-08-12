app.controller("FizLiso", ["$scope", "$http", "$filter", function ($scope, $http, $filter) {

    //GetOtraslForOrganization();
    //GetRegionForOrganization();
    //GetToDepForOrganization();
    GetEmptyCarcas();
    GetOrganizations();
    //GetPravLevel();

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

    $scope.ModalToDepCodeSelected = {
        Name: '',
        Kod: ''
    };

    $scope.ModalRegionSelected = {
        NameReg: '',
        ID: ''
    };

    $scope.SelectedOrganization = {

        ID: '',
        Name_Org: ''

    };

    $scope.SelectedProgLevel = {

        Code: 5,
        Decode: 'ППО'

    };

    $scope.TotalCount = 0;
    $scope.TotalCountNeot = 0;
    $scope.FilterSeria = '';
    $scope.FilterNomer = '';

    function GetOrganizationList() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetFizLisoList'
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

    $scope.GetOrgDataByInn = function (keyEvent, inn) {

            $http({
                method: 'get',
                url: '/Reference/GetOrgDataByInn?inn=' + inn
            }).then(function (response) {

                if ($scope.Organization.Name_Org == '') {
                    $scope.Organization.Name_Org = response.data.Name_Full;
                }

                if ($scope.Organization.OKONX == '') {
                    $scope.Organization.OKONX = response.data.Okonx;
                }

            }, function (error) {
                console.log("error in GetOrgDataByInn");
            });
    }


    $scope.GetBankNameByMFO = function (keyEvent, mfo) {

            $http({
                method: 'get',
                url: '/Reference/GetBankNameByMFO?mfo=' + mfo
            }).then(function (response) {

                $scope.Organization.Bank = response.data.BankName;

            }, function (error) {
                console.log("error in GetBankNameByMFO");
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
            url: '/PrixodPutevok/GetEmptyCarcasFizLiso?Id=0'
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
        //console.log(org.ID);
        $(".activvvv").removeClass("activvvv");
        $("#" + org.ID).addClass("activvvv");
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetEmptyCarcasFizLiso?Id=' + org.ID
        }).then(function (response) {
            $scope.Organization = response.data;

            var dataPos = (org.Pas_Data) ? $filter('date')(parseInt(org.Pas_Data.substr(6)), 'dd.MM.yyyy') : '';
            $scope.Organization.Pas_Data = new Date(dataPos.substring(6, 10), dataPos.substring(3, 5) - 1, dataPos.substring(0, 2));

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
            url: '/PrixodPutevok/GetEmptyCarcasFizLiso?Id=0'
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

    $scope.ClearSelectedValue = function ()
    {
        $scope.SelectedOrganization.ID = '';
        $scope.SelectedOrganization.Name_Org = '';
    }

    function GetOrganizations() {

        $scope.Organization = '';

        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/PrixodPutevok/GetFizLisoList'
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

    $scope.SaveOrganization = function () {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });

        //$scope.Organization.ID_Ot = $scope.ModalOtraslSelected.ID_Ot;
        //$scope.Organization.Region = $scope.ModalRegionSelected.ID;
        //$scope.Organization.ParentId = $scope.SelectedOrganization.ID;
        //$scope.Organization.PravSoyuzLevel = $scope.SelectedProgLevel.Code;
        //$scope.Organization.to_depcode = $scope.ModalToDepCodeSelected.Kod;
        

        $http({
            method: 'post',
            url: '/PrixodPutevok/SaveFizLiso',
            data: JSON.stringify($scope.Organization),
            contentType: "application/json; charset=utf-8",
        }).then(function (response) {

            if (response.data == 'success')
            {
                $scope.GetOrganizationsFilter();
                alertify.success("Успешное сохранение");
            }
            else
            {
                alertify.success("Сохранение завершилась с ошибкой");
            }

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in SaveRekvizit");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }


    $scope.exportData = function () {
        var blob = new Blob([document.getElementById('export').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Организации.xls");
    };
}]);