app.controller("LoadINN", ["$scope", "$http", "$filter", "DataService", function ($scope, $http, $filter, DataService) {
    $scope.showdata = false;
    $scope.LoadDATA = function () {
        Load($scope.I, $scope.N);
        //$("#hh").modal({
        //    backdrop: 'static'
        //});
        //reset();
        //alertify.set({ buttonFocus: "cancel" });
        //alertify.confirm("Вы действительно хотите получить?", function (e) {
        //    if (e) {
        //        alertify.success("Операция выполняется");
        //        document.getElementById('connection').style.visibility = "visible";
        //        Load($scope.I, $scope.N);
        //    } else {
        //        alertify.error("Операция отменена");
        //        $("#hh").modal('hide');
        //    }
        //});
        //return false;
    }
    function Load(inn, name) {
        if (name == null) {
            name = " ";
        }
        $http({
            method: "post",
            url: "/PrixodPutevok/Getegrpo?inn=" + inn + "&name=" + name,
        }).then(function (d) {
            $scope.DATA = d.data;
            if (d.data.length !== 0) {
                $scope.showdata = true;
            }
            else {
                $scope.showdata = false;
            }
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function () {
            $("#hh").modal('hide');
            //alert("error in O_SOZ");
        });
    }
    $scope.T = function (name, inn) {
        DataService.set(inn, name);
        document.getElementById("name").value = name;
        document.getElementById("inn").value = inn;
        $scope.INN = inn;
        $scope.O = name;
    }
}]);