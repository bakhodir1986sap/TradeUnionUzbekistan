app.controller("StatPPO", ["$scope", "$http", "$filter", "$locale", "DataService", "CalcService", function ($scope, $http, $filter, $locale, DataService, CalcService) {

    GetStatData();

    ////////////////////////////////

    $scope.labels2 = ["Утвержденные", "Откланенные", "На рассмотрении"];
    $scope.data2 = [300, 500, 100];


    /////////////////////////////////

    function GetStatData() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Home/StatPPO'
        }).then(function (response) {

            $scope.ModelResult = response.data;

            $scope.data2 = [$scope.ModelResult.ApprovedZayavka, $scope.ModelResult.RejectedZayavka, $scope.ModelResult.InProgressZayavka];

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
            }, function (error) {
                GetStatData();
            alertify.error("ошибка");
            console.log("error in GetCurBusinessTime");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }


}]);