app.controller("StartProcess", ["$scope", "$http", "$filter", "DataService", "CalcService", function ($scope, $http, $filter, DataService, CalcService) {

    $scope.SelectedFileForUpload = null; //initially make it null   

    $scope.SelectedFilePutyovka = null; //initially make it null   
    $scope.SelectedFileKengashPutyovka = null; //initially make it null   
    $scope.SelectedFileBalance = null; //initially make it null   
    $scope.SelectedFileOrganization = null; //initially make it null   

    $scope.PutyovkaFile = function (files) {
        $scope.$apply(function () {
            $scope.Message = '';
            $scope.SelectedFilePutyovka = files[0];
        })
    }

    $scope.PutyovkaKengashFile = function (files) {
        $scope.$apply(function () {
            $scope.Message = '';
            $scope.SelectedFileKengashPutyovka = files[0];
        })
    }


    $scope.BalanceFile = function (files) {
        $scope.$apply(function () {
            $scope.Message = '';
            $scope.SelectedFileBalance = files[0];
        })
    }


    $scope.DebitCreditFile = function (files) {
        $scope.$apply(function () {
            $scope.Message = '';
            $scope.SelectedFileOrganization = files[0];
        })
    }


    $scope.BindData = null;
    $scope.showLoader = false;
    $scope.IsVisible = false;


    $scope.ParseExcelPutyovka = function () {
        var formData = new FormData();
        var file = $scope.SelectedFilePutyovka;
        formData.append('PutyovkaFile', file);
        $scope.showLoader = true;   //show spinner  
        $http({
            method: "post",
            withCredentials: true,
            url: '/PrixodPutevok/PutyovkaOstatok',
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



    $scope.ParseExcelPutyovkaKengash = function () {
        var formData = new FormData();
        var file = $scope.SelectedFileKengashPutyovka;
        formData.append('PutyovkaKengashFile', file);
        $scope.showLoader = true;   //show spinner  
        $http({
            method: "post",
            withCredentials: true,
            url: '/PrixodPutevok/PutyovkaOstatokKengash',
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

    
    $scope.ParseExcelBalance = function () {
        var formData = new FormData();
        var file = $scope.SelectedFileBalance;
        formData.append('BalanceFile', file);
        $scope.showLoader = true;   //show spinner  
        $http({
            method: "post",
            withCredentials: true,
            url: '/PrixodPutevok/BalanceSaldo',
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

    
    $scope.ParseExcelDebitCredit = function () {
        var formData = new FormData();
        var file = $scope.SelectedFileOrganization;
        formData.append('DebitCreditFile', file);
        $scope.showLoader = true;   //show spinner  
        $http({
            method: "post",
            withCredentials: true,
            url: '/PrixodPutevok/OrganizationDebitCredit',
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

    $scope.showdata = false;
    $scope.showdata2 = false;
    $scope.SaveDATA = function () {
        var SaveData = new FormData();
        var Savefile = $scope.SelectedFileForUpload;
        SaveData.append('UploadedFile', Savefile);
        alertify.success("Операция выполняется");
        $http({
            method: "post",
            withCredentials: true,
            url: '/Buxgalter/Save',
            data: SaveData,
            headers: {
                'Content-Type': undefined
            },
            transformRequest: angular.identity
        }).then(function (d) {
            $scope.showdata = false;
            if (d.data == "error") {
                alertify.error("ошибка");
            } else {
                console.log(d.data);
            }

        }, function (err) {
            alertify.error("ошибка : " + err.data);
            console.log(err.data);
            console.log("error in parse excel");
        });
    }
    function LoadDATA() {

    }
}]);