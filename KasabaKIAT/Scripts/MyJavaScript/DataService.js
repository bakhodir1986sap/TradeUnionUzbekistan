myApp.factory('DataService', ["$http",
    function ($http) {
        var GetGrafikZaezdov = function () {
            return $http.get("Home/GetGrafikZaezdov");
        };
        return {
            GetGrafikZaezdov: GetGrafikZaezdov
        };
    }]);