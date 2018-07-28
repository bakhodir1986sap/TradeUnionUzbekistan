app.controller("PRZController", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    $scope.PRZdiv = true;
    $scope.showdata = false;
    $scope.searchD0 = new Date();
    $scope.searchD1 = new Date();
    $scope.previousID = null;
    $scope.SCf = {
        id: '-1',
        value: ''
    };
    $scope.SC = {
        id: '-1',
        value: ''
    };
    $scope.Pf = {
        id: '1',
        value: ''
    };
    getSs();
    getPs();
    Load_PRZ_S(-1);
    //--------------------------//
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
    //-------------------------//
    function Load_PRZ_S(id) {
        $http({
            method: "post",
            url: "/Home/Load_PRZ_S",
            params: {
                id: JSON.stringify(id)
            }
        }).then(function (m) {
            //alert(m.data);
            $scope.PRZ = m.data;

            
        }, function () {
            alertify.error("ошибка");
            console.log('Error in Load_PRZ_S');
        });
    }
    function Load_PRZ_C(id) {
        $http({
            method: "post",
            url: "/Home/Load_PRZ_C",
            params: {
                id: JSON.stringify(id)
            }
        }).then(function (m) {
            //alert(m.data);
            $scope.PRZ = m.data;
        }, function () {
            alertify.error("ошибка");
            console.log('Error in Load_PRZ_C');
        });
    }
    $scope.LoadCSName = function () {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $scope.searchD0 = new Date();
        $scope.searchD1 = new Date();
        $scope.PRZdiv = true;
        $scope.myVal1 = "Выбрать санаторий";
        $scope.myVal2 = "или дет. лагерь:";
        getPs();
        if ($scope.s === "option1") {
            getSs();
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }
        else {
            if ($scope.s === "option2") {
                getCs();
                document.getElementById('connection').style.visibility = "hidden";
                $("#hh").modal('hide');
            }
        }
    }
    $scope.LoadSC = function () {
        //$scope.SCf.id = 1;
        //alert($scope.SCf.id);
        if ($scope.s === "option1") {
            Load_PRZ_S($scope.SCf.id);
        } else {
            if ($scope.s === "option2") {
                Load_PRZ_C($scope.SCf.id);
            }
        }
    }
    function getSs() {
        $scope.s = "option1";
        $scope.MyVal = "Санаторий";
        $http({
            method: 'get',
            url: '/Home/GetSanatory'
        }).then(function (s) {
            $scope.CS = s.data;
        }, function () {
            alertify.error("ошибка");
            console.log('Error in GetSanatory');
        });
        //$http({
        //    method: "post",
        //    url: "/Home/GetAllPRZ_S",
        //}).then(function (s) {
        //    $scope.PRZ = s.data;
        //}, function () {
        //    alert('Error in GetAllPRZ_S');
        //});
    }
    function getCs() {
        $scope.MyVal = "Дет/лагер";
        $http({
            method: 'get',
            url: '/Home/GetCamp'
        }).then(function (c) {
            $scope.CS = c.data;
        }, function () {
            alertify.error("ошибка");
            console.log('Error in GetCamp');
        });
        //$http({
        //    method: "post",
        //    url: "/Home/GetAllPRZ_C",

        //}).then(function (c) {
        //    $scope.PRZ = c.data;
        //}, function () {
        //    alert('Error in GetAllPRZ_C');
        //});
    }
    function getPs() {
        $http({
            method: "post",
            url: "/Home/GetPosition",
        }).then(function (s) {
            $scope.Ps = s.data;
        }, function () {
            alertify.error("ошибка");
            console.log('Error in GetPosition');
        });
    }
    function Load_PRZ_D0_S() {
        var date = $scope.searchD0;
        $http({
            method: "post",
            url: "/Home/Load_PRZ_D0_S?date=" + date.toISOString(),
        }).then(function (m) {
            $scope.PRZ = m.data;
        }, function () {
            alertify.error("ошибка");
            console.log('Error in load Load_PRZ_D0_S');
        });
    }
    function Load_PRZ_D0_C() {
        var date = $scope.searchD0;
        $http({
            method: "post",
            url: "/Home/Load_PRZ_D0_C?date=" + date.toISOString(),
        }).then(function (m) {
            $scope.PRZ = m.data;
        }, function () {
            alertify.error("ошибка");
            console.log('Error in Load_PRZ_D0_C');
        });
    }
    $scope.searchD0_CH = function () {
        if ($scope.s === "option1") {
            Load_PRZ_D0_S();
        }
        else {
            if ($scope.s === "option2") {
                Load_PRZ_D0_C();
            }
        }
    }
    function Load_PRZ_D1_S() {
        var date = $scope.searchD1;
        $http({
            method: "post",
            url: "/Home/Load_PRZ_D1_S?date=" + date.toISOString(),
        }).then(function (m) {
            $scope.PRZ = m.data;
        }, function () {
            alertify.error("ошибка");
            console.log('Error in Load_PRZ_D1_S');
        });
    }
    function Load_PRZ_D1_C() {
        var date = $scope.searchD1;
        $http({
            method: "post",
            url: "/Home/Load_PRZ_D1_C?date=" + date.toISOString(),
        }).then(function (m) {
            $scope.PRZ = m.data;
        }, function () {
            alertify.error("ошибка");
            console.log('Error in Load_PRZ_D1_C');
        });
    }
    $scope.searchD1_CH = function () {
        if ($scope.s === "option1") {
            Load_PRZ_D1_S();
        }
        else {
            if ($scope.s === "option2") {
                Load_PRZ_D1_C();
            }
        }
    }
    function Load_PRZ_P_S() {
        $http({
            method: "post",
            url: "/Home/Load_PRZ_P_S",
            params: {
                id: JSON.stringify(id)
            }
        }).then(function (m) {
            $scope.PRZ = m.data;
        }, function () {
            alertify.error("ошибка");
            console.log('Error in Load_PRZ_P_S');
        });
    }
    function Load_PRZ_P_C() {
        $http({
            method: "post",
            url: "/Home/Load_PRZ_P_C",
            params: {
                id: JSON.stringify(id)
            }
        }).then(function (m) {
            $scope.PRZ = m.data;
        }, function () {
            alertify.error("ошибка");
            console.log('Error in Load_PRZ_P_C');
        });
    }
    $scope.LoadP = function () {
        if ($scope.s === "option1") {
            Load_PRZ_P_S();
        }
        else {
            if ($scope.s === "option2") {
                Load_PRZ_P_C();
            }
        }
    }
    $scope.MOE_ch = function () {
        if ($scope.MOE === false) {
            $scope.MO = "";
        }
    }
    function LoadINN(inn) {
        $scope.O = "";
        if (inn.length === 9) {
            //alert(inn);
            $http({
                method: 'get',
                url: '/Home/LoadINN?inn=' + inn
            }).then(function (response) {
                $scope.O = response.data;
            }, function (error) {
                console.log(error, 'Error in LoadINN');
            });
        }
        else {
            $scope.O = "";
        }
    }
    $scope.LoadINN = function () {
        var inn = $scope.INN;
        LoadINN(inn);
    }


    $scope.editPRZ = function (prz) {
        $scope.ID = prz.ID;

        $(".activvvv").removeClass("activvvv");
        $("#" + prz.ID).addClass("activvvv");
        var zb = (prz.ZB) ? $filter('date')(parseInt(prz.ZB.substr(6)), 'dd.MM.yyyy') : '';
        var ze = (prz.ZE) ? $filter('date')(parseInt(prz.ZE.substr(6)), 'dd.MM.yyyy') : '';
        $scope.ZB = new Date(zb.substring(6, 10), zb.substring(3, 5) - 1, zb.substring(0, 2));
        $scope.ZE = new Date(ze.substring(6, 10), ze.substring(3, 5) - 1, ze.substring(0, 2));;
        $scope.INN = prz.INN;
        $scope.O = prz.O;
        $scope.FN = prz.FN;
        $scope.P = prz.P;
        $scope.Q = prz.Q;
        $scope.MOE = prz.MOE;
        $scope.MO = prz.MO;
       // $scope.MO2 = prz.MO2;
        $scope.SC.id = prz.SC_id;
        $scope.SC.value = prz.SC;
        $scope.IsOwn = prz.isOwn;
        $scope.Status = prz.S;
        $scope.InnFiz = prz.sotrinn;
        $scope.pnfl = prz.pnfl;
        $scope.isInitial = prz.isInitial;
        $scope.Action = "Изменить";
    }
    $scope.UpdatePRZ = function () {
        var PRZ;
        if ($scope.s === "option1") {
            PRZ = {
                ZAEZD_BEGIN: $scope.ZB,
                ZAEZD_END: $scope.ZE,
                MEDICAL_ORDER: $scope.MO,
                FULL_NAME: $scope.FN,
                DEPARTMENT_CODE: $scope.INN,
                POSITION: $scope.P,
                QUANTITY: $scope.Q,
                SANATORY_ID: $scope.SC.id,
                MEDICAL_ORDER_EXISTS: $scope.MOE
            };
        } else {
            if ($scope.s === "option2") {
                PRZ = {
                    ZAEZD_BEGIN: $scope.ZB,
                    ZAEZD_END: $scope.ZE,
                    MEDICAL_ORDER: $scope.MO,
                    FULL_NAME: $scope.FN,
                    DEPARTMENT_CODE: $scope.INN,
                    POSITION: $scope.P,
                    QUANTITY: $scope.Q,
                    CAMP_ID: $scope.SC.id,
                    MEDICAL_ORDER_EXISTS: $scope.MOE
                };
            }
        }
        if ($scope.SC === null) {
            //alert("Sanatoriya yoki lager nomi tanlanmagan");
            console.log('Sanatoriya yoki lager nomi tanlanmagan');
        }
        else {
            PRZ.REC_ID = $scope.ID;
            $http({
                method: "post",
                url: "UpdatePodachaZayavok",
                data: JSON.stringify(PRZ),
                dataType: "json"
            }).then(function (msg) {
                if ($scope.s === "option1") {
                    getSs();
                    Load_PRZ_S(-1);
                }
                else {
                    if ($scope.s === "option2") {
                        getCs();
                        Load_PRZ_C($scope.SCf.id);
                    }
                }
            }, function () {
                alertify.error("ошибка");
                console.log('Error in UpdatePodachaZayavok');
            });
        }
    }
    $scope.exportData = function () {
        var blob = new Blob([document.getElementById('export').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Поиск и редактирование заявок.xls");
    };


    $scope.Passport = "Passport";
    $scope.LabourBook = "LabourBook";
    $scope.VocationOrder = "VocationOrder";
    $scope.PPO_Protokol = "PPO_Protokol";

    $scope.SelectedFilePassport = null;

    $scope.UploadFilesPassport = function (files) {
        $scope.$apply(function () {
            $scope.MessagePassport = '';
            $scope.SelectedFilePassport = files[0];
        })
    }


    $scope.DownloadFile = function (param1, fileName) {
        $http({
            method: 'GET',
            url: '/Home/DownloadZayavkaFile?zayavkaId=' + $scope.ID + '&OpName=' + param1,
            responseType: 'arraybuffer'
        }).success(function (data, status, headers) {
            headers = headers();

            var filename = fileName;
            var contentType = headers['content-type'];

            var linkElement = document.createElement('a');
            try {
                var blob = new Blob([data], { type: contentType });
                var url = window.URL.createObjectURL(blob);

                linkElement.setAttribute('href', url);
                linkElement.setAttribute("download", filename);

                var clickEvent = new MouseEvent("click", {
                    "view": window,
                    "bubbles": true,
                    "cancelable": false
                });
                linkElement.dispatchEvent(clickEvent);
            } catch (ex) {
                console.log(ex);
            }
        }).error(function (data) {
            console.log(data);
        });
    };

    $scope.SelectedFileLabourBook = null;

    $scope.UploadFilesLabourBook = function (files) {
        $scope.$apply(function () {
            $scope.MessageLabourBook = '';
            $scope.SelectedFileLabourBook = files[0];
        })
    }

    $scope.SelectedFileVocationOrder = null;

    $scope.UploadFilesVocationOrder = function (files) {
        $scope.$apply(function () {
            $scope.MessageVocationOrder = '';
            $scope.SelectedFileVocationOrder = files[0];
        })
    }

    $scope.SelectedFilePPO_Protokol = null;

    $scope.UploadFilesPPO_Protokol = function (files) {
        $scope.$apply(function () {
            $scope.MessagePPO_Protokol = '';
            $scope.SelectedFilePPO_Protokol = files[0];
        })
    }

    $scope.SaveFilesDATA = function () {

        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });

        var SaveData = new FormData();
        var SavePassport = $scope.SelectedFilePassport;
        SaveData.append('Passport', SavePassport);

        var SaveLabourBook = $scope.SelectedFileLabourBook;
        SaveData.append('LabourBook', SaveLabourBook);

        var SaveVocationOrder = $scope.SelectedFileVocationOrder;
        SaveData.append('VocationOrder', SaveVocationOrder);

        var SavePPO_Protokol = $scope.SelectedFilePPO_Protokol;
        SaveData.append('PPO_Protokol', SavePPO_Protokol);

        SaveData.append('ZayavkaID', $scope.ID);

        alertify.success("Операция выполняется");
        $http({
            method: "post",
            withCredentials: true,
            url: '/Home/UploadZayavkaFiles',
            data: SaveData,
            headers: {
                'Content-Type': undefined
            },
            transformRequest: angular.identity
        }).then(function (d) {
            $scope.showdata = false;
            if (d.data == "error") {
                alertify.error("ошибка");
            } else if (d.data == "success") {
                alertify.success("Файл удачно загружен");
            }
            else {
                console.log(d.data);
            }

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (err) {
            alertify.error("ошибка : " + err.data);
            console.log(err.data);
            console.log("error in load file");

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.GetFileInfoZayavka = function () {
        $http({
            method: 'get',
            url: '/Home/GetFileInfoZayavka?zayavkaId=' + $scope.ID
        }).then(function (c) {
            $scope.FilesOpdatas = c.data;
        }, function () {
            alertify.error("ошибка");
            console.log('Error in GetFileInfoZayavka');
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.UpdateState = function (state)
    {
        $http({
            method: 'get',
            url: '/Home/UpdateState?Id=' + $scope.ID + '&state=' + state
        }).then(function (c) {

            alertify.success("Статус удачно обнавлен");

            Load_PRZ_S(-1);

        }, function () {
            alertify.error("ошибка");
            console.log('Error in UpdateState');
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
}]);