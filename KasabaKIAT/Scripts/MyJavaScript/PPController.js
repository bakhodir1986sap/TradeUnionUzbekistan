app.controller("PPController", ["$scope", "$http", "$filter", "crudAJService", function ( $scope, $http, $filter, crudAJService) {
    $scope.SC = {
        id: '1',
        value: ''
    };
    $scope.SCFilter = {
        id: '1',
        value: ''
    };
    GetSanatory();
    GetPP(1);
    GetPP_SUM(1);
    $scope.PPdiv = false;
    //LoadDATA_S();
    $scope.LoadPPData = function () {
        GetPP($scope.SCFilter.id);
        GetPP_SUM($scope.SCFilter.id);
    }
    function GetPP(id) {
        $http({
            method: "post",
            url: "/PrixodPutevok/GetPP?sc_id=" + id,
        }).then(function (d) {
            $scope.PP = d.data;
            if (d.data.length != 0) {
                $scope.q = d.data.length;
                $scope.PPdiv = true;
            } else {
                $scope.PPdiv = false;
            }
        }, function () {
            alertify.error("ошибка");
            console.log("error in GetPP");
            $("#hh").modal('hide');
        });
    }
    function GetPP_SUM(id) {
        $http({
            method: "post",
            url: "/PrixodPutevok/GetPP_SUM?sc_id=" + id,
        }).then(function (d) {
            $scope.PP_SUM = d.data;
        }, function () {
            alertify.error("ошибка");
            console.log("error in GetPP_SUM");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    function GetSanatory() {
        $http({
            method: "post",
            url: "/PrixodPutevok/GetSanatory",
        }).then(function (s) {
            $scope.CS = s.data;
        }, function () {
            alertify.error("ошибка");
            console.log('Error in GetSanatory');
            $("#hh").modal('hide');
        });
    }
    function GetPP_SUM_S(id) {
        $http({
            method: "post",
            url: "GetPP_SUM_S?sc_id=" + id,
        }).then(function (d) {
            $scope.PP_SUM = d.data;
            if (d.data.length != 0) {
                $scope.PPdiv = true;
            }
        }, function () {
            alertify.error("ошибка");
            console.log("error in GetPP_SUM_S");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    function GetPP_SUM_C(id) {
        $http({
            method: "post",
            url: "GetPP_SUM_C?sc_id=" + id,
        }).then(function (d) {
            $scope.PP_SUM = d.data;
            if (d.data.length != 0) {
                $scope.PPdiv = true;
            } 
        }, function () {
            alertify.error("ошибка");
            console.log("error in GetPP_SUM_C");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
  }
    function GetPP_S(id) {
      $scope.scname = "Санаторий";
      $scope.s = "option1";
        $http({
            method: "post",
            url: "GetPrixodPutevok_S?sc_id="+id,
        }).then(function (d) {
            $scope.PP = d.data;
        }, function () {
            alertify.error("ошибка");
            console.log("error in GetPrixodPutevok_S");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
  }
    function GetPP_C(id) {
      $http({
          method: "post",
          url: "GetPrixodPutevok_C?sc_id=" + id,
      }).then(function (d) {
          $scope.PP = d.data;
      }, function () {
          alertify.error("ошибка");
          console.log("error in GetPrixodPutevok_C");
          document.getElementById('connection').style.visibility = "hidden";
          $("#hh").modal('hide');
      });
  }
    function Tozala() {
      $scope.SCFilter.id = 1;
      $scope.PFilter = "";
      GetPP(1);
      GetPP_SUM(1);
  }
    $scope.ok = function () {
      Tozala();
  }
    $scope.AddPPDiv = function () {
       $scope.showdelete = false;
      $scope.SC.id = 1;
      ClearFields();
      $scope.Action = "Добавить";
  }
    function ClearFields() {
      var rbd = $filter('date')(new Date(), 'dd.MM.yyyy');
      var red = $filter('date')(new Date(), 'dd.MM.yyyy');
      $scope.RBD = new Date(rbd.substring(6, 10), rbd.substring(3, 5) - 1, rbd.substring(0, 2));
      $scope.RED = new Date(red.substring(6, 10), red.substring(3, 5) - 1, red.substring(0, 2));
      $scope.SB = "";
      $scope.SE = "";
      $scope.RB = "";
      $scope.P = "";
      $scope.Q_IN = "";
      $scope.Q_OUT = "";
  }
    $scope.editPP = function (pp) {
      $scope.showdelete = true;
        $scope.ID = pp.ID;
        var rbd = (pp.RBD) ? $filter('date')(parseInt(pp.RBD.substr(6)), 'dd.MM.yyyy') : '';
        var red = (pp.RED) ? $filter('date')(parseInt(pp.RED.substr(6)), 'dd.MM.yyyy') : '';
        $scope.RBD = new Date(rbd.substring(6, 10), rbd.substring(3, 5) - 1, rbd.substring(0, 2));
        $scope.RED = new Date(red.substring(6, 10), red.substring(3, 5) - 1, red.substring(0, 2));;
        $scope.SB = pp.SB;
        $scope.SE = pp.SE;
        $scope.RB = pp.RB;
        $scope.P = pp.P;
        $scope.Q_IN = pp.Q_IN;
        $scope.Q_OUT = pp.Q_OUT;
        $scope.SC.id = pp.SC_id;
        $scope.SC.value = pp.SC;
        $scope.Action = "Редактировать";
    }
    $scope.AddUpdatePP = function () {
        var getPPAction = $scope.Action;
        var PP;
        if ($scope.s == "option1") {
            PP = {
                SANATORIUM_ID: $scope.SC.id,
                SERIAL_BEGIN: $scope.SB,
                SERIAL_END: $scope.SE,
                REST_BEGIN_DATE: $scope.RBD,
                REST_END_DATE: $scope.RED,
                PRICE: $scope.P,
                QUANTITY_IN: $scope.Q_IN,
                QUANTITY_OUT: $scope.Q_OUT
            };
        } else {
            if ($scope.s == "option2") {
                PP = {
                    CAMP_ID: $scope.SC.id,
                    SERIAL_BEGIN: $scope.SB,
                    SERIAL_END: $scope.SE,
                    REST_BEGIN_DATE: $scope.RBD,
                    REST_END_DATE: $scope.RED,
                    PRICE: $scope.P,
                    QUANTITY_IN: $scope.Q_IN,
                    QUANTITY_OUT: $scope.Q_OUT
                };
            }
        }
        if (getPPAction == "Редактировать") {
            PP.REC_ID = $scope.ID;
            $http({
                method: "post",
                url: "UpdatePrixodPutevok",
                data: JSON.stringify(PP),
                dataType: "json"
            }).then(function (msg) {
                if ($scope.s === "option1") {
                    GetPP_S($scope.SCFilter.id);
                    GetPP_SUM_S($scope.SCFilter.id);
                } else {
                    if ($scope.s === "option2") {
                        GetPP_C($scope.SCFilter.id);
                        GetPP_SUM_C($scope.SCFilter.id);
                    }
                }
            }, function () {
                //alert('Error in UpdatePrixodPutevok');
                console.log('Error in UpdatePrixodPutevok');
                document.getElementById('connection').style.visibility = "hidden";
                $("#hh").modal('hide');
            });
        }
        else {
            $http({
                method: "post",
                url: "AddPrixodPutevok",
                data: JSON.stringify(PP),
                dataType: "json"
            }).then(function (msg) {
                if ($scope.s === "option1") {
                    GetPP_S($scope.SCFilter.id);
                    GetPP_SUM_S($scope.SCFilter.id);
                } else {
                    if ($scope.s === "option2") {
                        GetPP_C($scope.SCFilter.id);
                        GetPP_SUM_C($scope.SCFilter.id);
                    }
                }
            }, function () {
                //alert('Error in AddPrixodPutevok');
                console.log('Error in AddPrixodPutevok');
                document.getElementById('connection').style.visibility = "hidden";
                $("#hh").modal('hide');
            });
        }
    }
    $scope.deletePP = function (ID) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите удалить?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                $http({
                    method: "post",
                    url: "DeletePrixodPutevok",
                    params: {
                        id: JSON.stringify(ID)
                    }
                }).then(function (msg) {
                    document.getElementById('connection').style.visibility = "hidden";
                    $("#hh").modal('hide');
                    if ($scope.s === "option1") {
                        GetPP_S($scope.SCFilter.id);
                        GetPP_SUM_S($scope.SCFilter.id);
                    } else {
                        if ($scope.s === "option2") {
                            GetPP_C($scope.SCFilter.id);
                            GetPP_SUM_C($scope.SCFilter.id);
                        }
                    }
                }, function () {
                    //alert('Error DeletePrixodPutevok');
                    console.log('Error DeletePrixodPutevok');
                    document.getElementById('connection').style.visibility = "hidden";
                    $("#hh").modal('hide');
                });
            } else {
                document.getElementById('connection').style.visibility = "hidden";
                $("#hh").modal('hide');
                alertify.error("Операция отменена");
            }
        });
        return false;   
    }
    function GetPP_S_LoadINN(inn) {
        $scope.s = "option1";
        $http({
            method: "post",
            url: "GetPrixodPutevok_S_LoadINN?inn=" + inn,
        }).then(function (d) {
            $scope.RP = d.data;
        }, function () {
            alertify.error("ошибка");
            console.log("error in GetPrixodPutevok_S_LoadINN");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    function GetPP_C_LoadINN(inn) {
        $http({
            method: "post",
            url: "GetPrixodPutevok_C_LoadINN?inn=" + inn,
        }).then(function (d) {
            $scope.RP = d.data;
        }, function () {
            alertify.error("ошибка");
            console.log("error in GetPrixodPutevok_C_LoadINN");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    $scope.LoadINN = function () {
        if ($scope.INN != null) {
            if ($scope.s === "option1") {
                GetPP_S_LoadINN($scope.INN);
            }
            else {
                if ($scope.s === "option2") {
                    GetPP_C_LoadINN($scope.INN);
                }
            }
        }
    }
    function LoadDATA_S() {
        $scope.scname = "Санаторий";
        $scope.s = "option1";
        $http({
            method: "post",
            url: "PP_S",
        }).then(function (d) {
            $scope.RP = d.data;
        }, function () {
            alertify.error("ошибка");
            console.log("error in PP_S");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    function LoadDATA_C() {
        $scope.scname = "Дет. лагер";
        $http({
            method: "post",
            url: "PP_C",
        }).then(function (d) {
            $scope.RP = d.data;
        }, function () {
            alertify.error("ошибка");
            console.log("error in PP_C");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    $scope.LoadDATA = function () {
        if ($scope.s === "option1") {
            LoadDATA_S();
        }
        else {
            if ($scope.s === "option2") {
                LoadDATA_C();
            }
        }
    }
    $scope.data = function (sb, se) {
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
        var arr = []; var j = 0;
        for (var i = sb; i <= se; i++) {
            arr[j] = i;
            j++;
        }
        $scope.rr = arr;
        $scope.items = arr;
    }
    $scope.exportData = function () {
        var blob = new Blob([document.getElementById('export').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-16"
        });
        saveAs(blob, "Просмотр остатков путевок.xls");
    };
}]);