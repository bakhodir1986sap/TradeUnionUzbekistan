app.controller("StatOblSovProf", ["$scope", "$http", "$filter", "$locale", "DataService", "CalcService", function ($scope, $http, $filter, $locale, DataService, CalcService) {

    GetStatData();

    ////////////////////////////////
    $scope.labels1 = ["Есть профком", "Нет профкома"];
    $scope.data1 = [300, 500];

    $scope.labels2 = ["Утвержденные", "Откланенные", "На рассмотрении"];
    $scope.data2 = [300, 500, 100];


    $scope.labels3 = ["Остаток", "Реализованные", "Возврат"];
    $scope.data3 = [300, 500, 100];

    /////////////////////////////////

    function GetStatData() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Home/StatOblSovProf'
        }).then(function (response) {

            $scope.ModelResult = response.data;

            $scope.data1 = [$scope.ModelResult.ProfComExists, $scope.ModelResult.ProfComNotExists];
            $scope.data2 = [$scope.ModelResult.ApprovedZayavka, $scope.ModelResult.RejectedZayavka, $scope.ModelResult.InProgressZayavka];
            $scope.data3 = [$scope.ModelResult.OstatokPutyovkas, $scope.ModelResult.RealizedPutyovkas, $scope.ModelResult.VozvratPutyovkas];

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetCurBusinessTime");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }


}]);