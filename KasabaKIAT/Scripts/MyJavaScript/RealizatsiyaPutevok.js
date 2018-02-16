app.controller("RealizatsiyaPutevok", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    LoadDATA();
   
    function GetSanatory() {
        $http({
            method: "post",
            url: "/PrixodPutevok/GetSanatory",
        }).then(function (s) {
            $scope.CS = s.data;
        }, function () {
            //alert('Error in GetSanatory');
            console.log('Error in GetSanatory');
            $("#hh").modal('hide');
        });
    }
  
    
  
  
    function GetPP_S_LoadINN(inn) {
        $scope.s = "option1";
        $http({
            method: "post",
            url: "GetPrixodPutevok_S_LoadINN?inn=" + inn,
        }).then(function (d) {
            $scope.RP = d.data;
        }, function () {
            //alert("error in GetPrixodPutevok_S_LoadINN");
            console.log("error in GetPrixodPutevok_S_LoadINN");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
  
    $scope.LoadINN = function () {
        GetPP_S_LoadINN($scope.INN);
    }
    function LoadDATA() {
        $http({
            method: "post",
            url: "/PrixodPutevok/RP",
        }).then(function (d) {
            $scope.RP = d.data;
        }, function () {
            //alert("error in PP_S");
            console.log("error in RP");
            $("#hh").modal('hide');
        });
    }
    $scope.LoadDATA = function () {
        LoadDATA();
    }
    $scope.data = function (id) {
        $http({
            method: "post",
            url: "/PrixodPutevok/RP2?id="+id,
        }).then(function (d) {
            $scope.RP2 = d.data;
        }, function () {
            //alert("error in PP_S");
            console.log("error in RP2");
            $("#hh").modal('hide');
        });
        //$scope.accordion = {
        //    current: null
        //};
        //$scope.accordion.current = sb;
        //(function () {
        //    $(".panel").on("show.bs.collapse hide.bs.collapse", function (e) {
        //        $(".panel").removeClass('active');
        //        if (e.type == 'show') {
        //            $(this).addClass('active');
        //        } else {
        //            $(this).removeClass('active');
        //        }
        //    });

        //}).call(this);
        //var arr = []; var j = 0;
        //for (var i = sb; i <= se; i++) {
        //    arr[j] = i;
        //    j++;
        //}
        //$scope.rr = arr;
        //$scope.items = arr;
    }
}]);