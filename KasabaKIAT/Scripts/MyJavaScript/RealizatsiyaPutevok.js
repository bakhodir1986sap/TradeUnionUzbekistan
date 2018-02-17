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
    var i = 0;
    var ar1 = new Array();
    var ar2 = new Array();
    var d = {};
    $scope.BronChange=function(BronNumber,SanID){
        ar1[i] = BronNumber;
        ar2[i] = SanID;
        alert(ar1[i] + " : " + ar2[i] + " : " + i);
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
            //alert(d.ID_San + " : " + d.Num_Put);
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
                //$.ajax({
                //    type: "POST",
                //    url: "/PrixodPutevok/RPBron",
                //    data: postData,
                //    success: function (data) {
                //        alert(data.Result);
                //        LoadDATA();
                //    },
                //    dataType: "json",
                //    traditional: true
                //});
                $.ajax({
                    type: "POST",
                    url: "/PrixodPutevok/RPBron",
                    data: JSON.stringify(postData),
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                            LoadDATA();
                            console.log(data.data);
                        alert(data.Result);
                    },
                    dataType: "json",
                    traditional: true
                });
                $("#hh").modal('hide');
                //$http({
                //    method: "post",
                //    url: "/Administirirovaniye/RPBron",
                //    params: {
                //        b: JSON.stringify(d)
                //    }
                //}).then(function (msg) {
                //    LoadDATA();
                //    console.log(msg.data);
                //}, function () {
                //    //alert('Error DeleteGodovoyPlan');
                //    console.log('Error RPBron');
                //    $("#hh").modal('hide');
                //});
            } else {
                $("#hh").modal('hide');
                alertify.error("Операция отменена");
            }
        });
        return false;
    }
}]);