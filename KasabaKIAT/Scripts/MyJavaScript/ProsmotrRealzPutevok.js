app.controller("ProsmotrRealzPutevok", ["$scope", "$http", "$filter", "DataService", "CalcService", function ($scope, $http, $filter, DataService, CalcService) {
    LoadDATA();
    $scope.showdata = false;
    $scope.showdata2 = false;
    function LoadDATA() {
        $http({
            method: 'POST',
            url: "/PrixodPutevok/PRP",
        }).then(function (d) {
            if (d.data.length != 0) {
                $scope.showdata = true;
            } else {
                $scope.showdata = false;
            }
            $scope.DATA = d.data;
            console.log(d.data);
            //alert(d.data);
        }, function (error) {
            //alert("error in Getdictionary");
            console.log("error in Getdictionary -> ",error);
            $("#hh").modal('hide');
        });
    }
    var ID;
    $scope.Load = function (id) {
        ID = id;
        $http({
            method: 'POST',
            url: "/PrixodPutevok/PRP2?id="+id,
        }).then(function (d) {
            $scope.DATA2 = d.data;
            if (d.data.length != 0) {
                $scope.showdata2 = true;
            } else {
                $scope.showdata2 = false;
            }
            console.log("PRP2", $scope.DATA2);
            //alert(d.data);
        }, function (error) {
            //alert("error in Getdictionary");
            console.log("error in PRP2 -> ", error);
            $("#hh").modal('hide');
        });

        $http({
            method: 'POST',
            url: "/PrixodPutevok/PRP3?id=" + id,
        }).then(function (d) {
            $scope.DATA3 = d.data;
            console.log("PRP3", $scope.DATA3);
            //alert(d.data);
        }, function (error) {
            //alert("error in Getdictionary");
            console.log("error in PRP3 -> ", error);
            $("#hh").modal('hide');
        });
    }
    $scope.vozvrat = function () {
        $http({
            method: 'POST',
            url: "/PrixodPutevok/vozvrat?id=" + ID + "&remark=" + $scope.V,
        }).then(function (d) {
            console.log(d.data);
            LoadDATA();
        }, function (error) {
            //alert("error in Getdictionary");
            console.log("error in vozvrat -> ", error);
            $("#hh").modal('hide');
        });
    }
}]);