app.controller("OformleniyaRealizatsiyaPutevok", ["$scope", "$http", "$filter", "DataService", function ($scope, $http, $filter, DataService) {
    LoadDATA();
    $scope.showdata = false;
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
    function LoadDATA() {
        $http({
            method: "post",
            url: "/PrixodPutevok/RP",
        }).then(function (d) {
            $scope.DATA = d.data;
        }, function () {
            //alert("error in PP_S");
            console.log("error in RP");
            $("#hh").modal('hide');
        });
    }
    $scope.data = function (id) {
        $http({
            method: "post",
            url: "/PrixodPutevok/RP2?id=" + id,
        }).then(function (d) {
            $scope.RP2 = d.data;
        }, function () {
            //alert("error in PP_S");
            console.log("error in RP2");
            $("#hh").modal('hide');
        });
    }
    var i = 0;
    var ar1 = new Array();
    var ar2 = new Array();
    var d = {};
    $scope.BronChange = function (BronNumber, SanID) {
        ar1[i] = BronNumber;
        ar2[i] = SanID;
        i++;
    }
    $scope.Bron = function () {
        var stringArray = new Array();
        for (var k = 0; k < ar1.length; k++) {
            d = {
                ID_San: ar2[k],
                Num_Put: ar1[k],
            };
            stringArray[k] = d;
        }

        var postData = { b: stringArray };
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите Забронировать?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                $.ajax({
                    type: "POST",
                    url: "/PrixodPutevok/RPBron",
                    data: JSON.stringify(postData),
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        LoadDATA();
                        ar1 = new Array();
                        ar2 = new Array();
                        i = 0;
                        console.log(data);
                        $("#hh").modal('hide');
                    },
                    //dataType: "json",
                    //traditional: true
                });
                $("#hh").modal('hide');
            } else {
                $("#hh").modal('hide');
                alertify.error("Операция отменена");
            }
        });
        return false;
    }
}]);