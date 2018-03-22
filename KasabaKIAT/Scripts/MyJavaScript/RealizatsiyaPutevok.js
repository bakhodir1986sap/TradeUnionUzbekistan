app.controller("RealizatsiyaPutevok", ["$scope", "$http", "$filter", "DataService", "CalcService", function ($scope, $http, $filter, DataService, CalcService) {
    //$scope.INN = "200382518";
    var zb = new Date();// ? $filter('date')(parseInt(new Date().substr(6)), 'dd.MM.yyyy') : '';
    $scope.sana = zb.getDay();
    $scope.oy = zb.getMonth();
    $scope.yil = zb.getFullYear();
    LoadDATA();
    $scope.showdata = false;
    function GetOR() {
        $http({
            method: "post",
            url: "/PrixodPutevok/GetOR",
        }).then(function (s) {
            $scope.OR = s.data;
        }, function () {
            console.log('Error in GetSanatory');
            $("#hh").modal('hide');
        });
    }
    function LoadINN(inn) {
        Data = { b: stringArrayd, inn: inn };
        $scope.O = "";
        try {
            if (inn.length == 9) {
                $http({
                    method: 'get',
                    url: "/PrixodPutevok/Getegrpo?inn=" + inn + "&name=' '",
                }).then(function (response) {
                    if (response.data.length == 0) {
                        $scope.O = "No Data Found";
                    }
                    else {
                        $scope.O = response.data;
                    }
                    console.log($scope.O);
                    console.log(response.data);
                },function(error){
                    console.log(error, 'error in LoadINN');
                });
            }
        }
        catch (err) {
            $scope.O = "";
        }
    }
    $scope.LoadINN = function () {
        var inn = $scope.INN;
        LoadINN(inn);
    }
  
    function INNchange() {
        document.getElementById("O").innerHTML = "";
    }
    $scope.HelloFromChild1 = function () {
        $scope.ddd=CalcService.square2();
        //$scope.ddd = DataService.get();
        alert($scope.ddd);
        //$scope.INN = inn;
        //alert("inn=" + inn);
        //$('#dobavit').modal('toggle');
        //document.getElementById("O").innerHTML = name;
        //document.getElementById("inn").value = inn;
    }
    function LoadDATA() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: "post",
            url: "/PrixodPutevok/RP",
        }).then(function (d) {
            $scope.RP = d.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function () {
            //alert("error in PP_S");
            console.log("error in RP");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    $scope.data = function (id) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: "post",
            url: "/PrixodPutevok/RP2?id="+id,
        }).then(function (d) {
            $scope.RP2 = d.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function () {
            console.log("error in RP2");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    var i = 0;
    var ar1 = new Array();
    var ar2 = new Array();
    var d = {};
    var dd = {};
    var stringArrayd = new Array();
    var Data = {};
    $scope.checkboxModel = {
        value1: true,
        value2: 'YES'
    };
    $scope.invited = [];
    $scope.checkinTask = function (task) {
        //alert("task=" + task);
        if (task.Checked === true) {
            task.Lineclass = "checkedTask";
            $scope.disabled = false;
            //trash.removeClass("disabled");
        } else {
            task.Lineclass = "";
        }
        //var checkedExist = false;
        //tasks.forEach(function (v) {
        //    if (v.Checked) {
        //        checkedExist = true;
        //    }
        //});
        //if (!checkedExist) {
        //    $scope.disabled = true;
        //    trash.addClass("disabled");
        //}
    };
    $scope.BronChange = function (task) {
        //alert("BronNumber=" + task.d3 + " SanID=" + task.d1 + " task=" + task);
        if (task.Checked === true) {
            task.Lineclass = "checkedTask";
            $scope.disabled = false;
        } else {
            task.Lineclass = "";
        }
        ar1[i] = task.d3;
        ar2[i] = task.d1;
        i++;
        for (var k = 0; k < ar2.length; k++) {
            dd = {
                SanId: ar2[k],
                NumPut: ar1[k],
            };
            stringArrayd[k] = dd;
            
        }
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
    //---------------------------------------//
    $scope.Realiz = function () {
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите Реализовать?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                real();
            } else {
                $("#hh").modal('hide');
                alertify.error("Операция отменена");
            }
        });
        return false;
        //$('#dobavit2').modal('hide');
        //$('#dobavit2').modal('show');
    }
    function real() {
        if (ar1.length !== 0 || ar2.length !== 0) {
            document.getElementById('connection').style.visibility = "visible";
            $("#hh").modal({
                backdrop: 'static'
            });
            var k = 0;
            console.log(Data);
            $http({
                method: "POST",
                url: "/PrixodPutevok/GetOformleniyaRealizatsiyaPutevok",
                data: JSON.stringify(Data),
                contentType: "application/json; charset=utf-8",
            }).then(function (d) {
                k++;
                $scope.DATA = d.data;
                ar1 = new Array();
                ar2 = new Array();
                console.log(d, k);
                document.getElementById('connection').style.visibility = "hidden";
                $("#hh").modal('hide');
            }, function () {
                console.log("error in GetOformleniyaRealizatsiyaPutevok");
                document.getElementById('connection').style.visibility = "hidden";
                $("#hh").modal('hide');
                alertify.error("ошибка");
            });
        }
        else {
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
            alertify.error("Путевки не выбран");
        }
    }
    $scope.save = function (id,foiz) {
        $http({
            method: "POST",
            url: "/PrixodPutevok/Save?id=" + id + "&foiz=" + foiz,
        }).then(function (d) {
            console.log(d.data);
            LoadDATA();
            $("#hh").modal('hide');
        }, function () {
            alertify.error("ошибка");
            console.log("error in Save");
            $("#hh").modal('hide');
        });
    };
    $scope.LoadZ = function () {
        $http({
            method: "POST",
            url: "/PrixodPutevok/GetZList?inn=" + $scope.INN,
        }).then(function (d) {
            $scope.DATA2 = d.data;
            $("#hh").modal('hide');
        }, function () {
            alertify.error("ошибка");
            console.log("error in GetZList");
            $("#hh").modal('hide');
        });
    }
    //======================================//
    $scope.exportData = function () {
        var blob = new Blob([document.getElementById('export').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Реализация путевок.xls");
    };
    $scope.exportData1 = function () {
        document.getElementById('connection').style.visibility = "visible";
        document.getElementById('temp').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });

        var doc = new jsPDF();
        var width = doc.internal.pageSize.width;
        var height = doc.internal.pageSize.height;
        var utf_8_string_to_render = 'any_value_you_want';
        Promise.all(
                [
                    new Promise(function (resolve) {
                        var temp = document.createElement("div");
                        temp.id = "temp";
                        temp.style = "color:black;margin:0px;font-size:20px;";
                        //black
                        phrase = utf_8_string_to_render;
                        //temp.innerHTML = phrase;
                        //need to render element, otherwise it won't be displayed
                        document.body.appendChild(temp);

                        html2canvas($("#temp"), {
                            onrendered: function (canvas) {

                                // $("#temp").remove();
                                //width = canvas.width;
                                //height =canvas.height;
                                resolve(canvas.toDataURL('image/png'));
                            },
                        });
                    })
                ]).then(function (ru_text) {
                    doc.addImage(ru_text[0], 'JPEG', 10, 10, (width - 20), (height - 20));
                    //doc.text(0, 10, 'Non-utf-8-string');

                    doc.save('Реализация путевок.pdf');
                    document.getElementById('connection').style.visibility = "hidden";
                    document.getElementById('temp').style.visibility = "hidden";
                    $("#hh").modal('hide');
                });
    }

    $scope.LoadDATA2 = function () {
        Load($scope.I, $scope.N);
    }
    function Load(inn, name) {
        document.getElementById('connection2').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        if (name == null) {
            name = " ";
        }
        $http({
            method: "post",
            url: "/PrixodPutevok/Getegrpo?inn=" + inn + "&name=" + name,
        }).then(function (d) {
            $scope.DATA3 = d.data;
            if (d.data.length !== 0) {
                $scope.showdata = true;
            }
            else {
                $scope.showdata = false;
            }
            document.getElementById('connection2').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function () {
            document.getElementById('connection2').style.visibility = "hidden";
            $("#hh").modal('hide');
            alertify.error("ошибка");
        });
    }

    var ii = null;
    var ni = null;
    $scope.T = function (name, inn) {
        ni = name;
        ii = inn;
        document.getElementById("name").value = name;
        document.getElementById("inn1").value = inn;
    }
    $scope.Tanlash = function () {
        if (ii != null && ni != null) {
            $scope.INN = ii;
            LoadINN(ii);
        }
    }
}]);