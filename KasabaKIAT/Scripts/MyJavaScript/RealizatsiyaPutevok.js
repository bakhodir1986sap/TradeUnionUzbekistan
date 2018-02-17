app.controller("RealizatsiyaPutevok", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
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
            $scope.RP = d.data;
        }, function () {
            //alert("error in PP_S");
            console.log("error in RP");
            $("#hh").modal('hide');
        });
    }
    $scope.LoadDATA = function () {
        $("#hh").modal({
            backdrop: 'static'
        });
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите получить?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                document.getElementById('connection').style.visibility = "visible";
                Load($scope.I, $scope.N);
            } else {
                alertify.error("Операция отменена");
                $("#hh").modal('hide');
            }
        });
        return false;
    }
    function Load(inn,name) {
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
    $scope.Snyat = function () {
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
        alertify.confirm("Вы действительно хотите Отменить бронь?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                $.ajax({
                    type: "POST",
                    url: "/PrixodPutevok/RPSnyat",
                    data: JSON.stringify(postData),
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        LoadDATA();
                        ar1 = new Array();
                        ar2 = new Array();
                        i=0;
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