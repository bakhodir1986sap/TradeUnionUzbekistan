app.controller("LoadINN", ["$scope", "$http", "$filter", "DataService", "CalcService", function ($scope, $http, $filter, DataService, CalcService) {
    $scope.showdata = false;
    $scope.LoadDATA = function () {
        Load($scope.I, $scope.N);
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
            alertify.error("ошибка");
        });
    }
   
    var i=null;
    var n=null;
    $scope.T = function (name, inn) {
        //alert(inn);
        CalcService.square(inn);
        //DataService.set(inn, name);
        $("#table tr").click(function () {
            $(this).addClass('selected').siblings().removeClass('selected');
            var value = $(this).find('td:first').html();
            //alert(value);
        });
        n = name;
        i = inn;
        document.getElementById("name").value = name;
        document.getElementById("inn").value = inn;
        $scope.INN = inn;
        $scope.O = name;
    }
    $scope.Tanlash = function () {
        if (i != null && n != null)
        {
            
            //DataService.set(i, n);
            //parent.HelloFromChild(inn, name);
        }
    }
}]);