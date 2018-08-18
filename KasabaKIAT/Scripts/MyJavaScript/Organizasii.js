app.controller("Organizasii", ["$scope", "$http", "$filter", function ($scope, $http, $filter) {

    GetOtraslForOrganization();
    GetRegionForOrganization();
    GetToDepForOrganization();
    GetEmptyCarcas();
    GetOrganizations();
    GetPravLevel();

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

    $scope.SelectedFileOtrasl = null; //initially make it null   

    $scope.OtraslFile = function (files) {
        $scope.$apply(function () {
            $scope.Message = '';
            $scope.SelectedFileOtrasl = files[0];
        })
    }

    $scope.BindData = null;
    $scope.showLoader = false;
    $scope.IsVisible = false;

    $scope.ParseExcelOtrasl = function () {
        var formData = new FormData();
        var file = $scope.SelectedFileOtrasl;
        formData.append('OtraslFile', file);
        $scope.showLoader = true;   //show spinner  
        $http({
            method: "post",
            withCredentials: true,
            url: '/PrixodPutevok/OrganizationOtrasl',
            data: formData,
            headers: {
                'Content-Type': undefined
            },
            transformRequest: angular.identity
        }).then(function (d) {
            $scope.showdata = false;
            if (d.data == "error") {
                alertify.error("ошибка");
            }
            else {
                alertify.success("Удачно загрузилось");
                var res = d.data;
                $scope.BindData = res;
                if (d.data.length != 0) {
                    $scope.showdata = true;
                } else {
                    $scope.showdata = false;
                }
                $scope.IsVisible = true; //showing the table after databinding  
                $scope.showLoader = false; //after success hide spinner  
            }

        }, function (err) {
            alertify.error("ошибка : " + err.data);
            console.log(err.data);
            console.log("error in parse excel");
        });
    }


    $scope.TotalCount = 0;
    $scope.TotalCountNeot = 0;

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

    function GetToDepForOrganization() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Buxgalter/SpisokPravsoyuzovOrg'
        }).then(function (response) {
            $scope.ToDepCode = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRekvizits");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }


    function GetPravLevel() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Reference/GetPravLevel'
        }).then(function (response) {
            $scope.ProfLevels = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetPravLevel");
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
        //console.log(org.ID);
        $(".activvvv").removeClass("activvvv");
        $("#" + org.ID).addClass("activvvv");
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

            $scope.SelectedProgLevel.Code = org.PravSoyuzLevel;
            //$scope.SelectedProgLevel.Decode = org.ParentName;

            $scope.ModalToDepCodeSelected.Kod = org.to_depcode;

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
            url: '/PrixodPutevok/GetOrganizationList'
        }).then(function (response) {
            $scope.Organizations = response.data;
            TotalCountOfOrganization();
            TotalCountOfNeotOrganization();
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRekvizits");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }



    function TotalCountOfOrganization() {

        $scope.TotalCount = 0;

        $http({
            method: 'get',
            url: '/Reference/TotalCountOfOrganization'
        }).then(function (response) {
            $scope.TotalCount = response.data;
        }, function (error) {
            console.log("error in GetRekvizits");
        });
    }


    function TotalCountOfNeotOrganization() {

        $scope.TotalCountNeot = 0;

        $http({
            method: 'get',
            url: '/Reference/TotalCountOfNeotOrganization'
        }).then(function (response) {
            $scope.TotalCountNeot = response.data;
        }, function (error) {
            console.log("error in GetRekvizits");
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
            TotalCountOfOrganization();
            TotalCountOfNeotOrganization();
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
        $scope.Organization.PravSoyuzLevel = $scope.SelectedProgLevel.Code;
        $scope.Organization.to_depcode = $scope.ModalToDepCodeSelected.Kod;
        

        $http({
            method: 'post',
            url: '/PrixodPutevok/SaveOrganization',
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

            TotalCountOfOrganization();
            TotalCountOfNeotOrganization();

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