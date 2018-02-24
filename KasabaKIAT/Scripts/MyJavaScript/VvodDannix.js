app.controller("VvodDannix", ["$scope", "$http", "$filter", "DataService", "CalcService", function ($scope, $http, $filter, DataService, CalcService) {

    $scope.SelectedFileForUpload = null; //initially make it null   

    $scope.UploadFiles = function (files) {
        $scope.$apply(function () {
            $scope.Message = '';
            $scope.SelectedFileForUpload = files[0];
        })
    }

    $scope.BindData = null;
    $scope.showLoader = false;
    $scope.IsVisible = false;
    $scope.ParseExcel = function () {
        var formData = new FormData();
        var file = $scope.SelectedFileForUpload;
        formData.append('UploadedFile', file);
        $scope.showLoader = true;   //show spinner  
        $http({  
            method: "post",  
            withCredentials: true,  
            url: '/Buxgalter/Read',
            data: formData,  
            headers: {  
                'Content-Type': undefined  
            },  
            transformRequest: angular.identity  
        }).then(function (d) {
            var res = d.data;
            $scope.BindData = res;
            if (d.data.length != 0) {
                $scope.showdata = true;
            } else {
                $scope.showdata = false;
            }
            $scope.IsVisible = true; //showing the table after databinding  
            $scope.showLoader = false; //after success hide spinner  
        }, function (err) {
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
            console.log(d.data);
        }, function (err) {
            console.log(err.data);
            console.log("error in parse excel");
        });
    }
    function LoadDATA() {
       
    }
}]);