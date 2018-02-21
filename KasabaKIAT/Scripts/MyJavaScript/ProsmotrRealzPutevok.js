app.controller("ProsmotrRealzPutevok", ["$scope", "$http", "$filter", "DataService", "CalcService", function ($scope, $http, $filter, DataService, CalcService) {
    LoadDATA();
    $scope.showdata = false;
    function LoadDATA() {
        $http({
            method: 'POST',
            url: "/PrixodPutevok/PRP",
        }).then(function (d) {
            $scope.DATA = d.data;
            alert(d.data);
        }, function (error) {
            //alert("error in Getdictionary");
            console.log("error in Getdictionary -> ",error);
            $("#hh").modal('hide');
        });
    }
}]);