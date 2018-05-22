app.controller("SverkaDannix", ["$scope", "$http", "$filter", "DataService", "CalcService", function ($scope, $http, $filter, DataService, CalcService) {
    LoadDATA();
    $scope.showdata = false;
    $scope.showdata2 = false;

    $scope.UploadFiles = function (files) {
        $scope.$apply(function () {
            $scope.Message = '';
            $scope.SelectedFileForUpload = files[0];
        })
    }

    $scope.CurrentSverka = {

        id          :'',   
        year        :'',
        owner_tp    :'',
        sanat_id    :'',
        num_put     :'',
        date_san    :'',
        price       :'',
        fio_otdix   :'',
        adress      :'',
        work_place  :'',
        otrasl      :'',
        owner_code  :''

    };


    $scope.SaveDATA = function () {
        var SaveData = new FormData();
        var Savefile = $scope.SelectedFileForUpload;
        SaveData.append('UploadedFile', Savefile);

        var postData = {
            formCollection: SaveData,
            model: $scope.CurrentSverka
        };

        alertify.success("Операция выполняется");
        $http({
            method: "post",
            withCredentials: true,
            url: '/PrixodPutevok/ReadFileSverki',
            data: postData,
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
        $http({
            method: 'POST',
            url: "/PrixodPutevok/SD",
        }).then(function (d) {
            if (d.data.length != 0) {
                $scope.showdata = true;
            } else {
                $scope.showdata = false;
            }
            $scope.DATA = d.data;
            console.log(d.data);
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in SD -> ", error);
            $("#hh").modal('hide');
        });
    }
}]);