app.controller("PZController", ["$scope", "$http", "$filter", "crudAJService", function ($scope, $http, $filter, crudAJService) {
    $scope.OTPdiv = false;
    $scope.showdelete = false;
    $scope.showdata = false;
    AllSanatory();
    GetPZ_S(-1);
    GetUpperOrganization();
    $scope.SC = {
        id: '-1',
        value: ''
    };
    $scope.SCFilter = {
        id: '-1',
        value: ''
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


    $scope.SelectedFileZayavkiLoad = null;

    $scope.UploadFilesZayavkiLoad = function (files) {
        $scope.$apply(function () {
            $scope.MessageZayavkiLoad = '';
            $scope.SelectedFileZayavkiLoad = files[0];
            $scope.SaveFilesZayavkaTempl();
        })
    }


    $scope.SaveFilesZayavkaTempl = function () {

        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });

        var SaveData = new FormData();
        var SaveZayavki = $scope.SelectedFileZayavkiLoad;
        SaveData.append('ZayavkiLoad', SaveZayavki);

        alertify.success("Операция выполняется");
        $http({
            method: "post",
            withCredentials: true,
            url: '/Home/ReadFileZayavki',
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
                GetPZ_S($scope.SCFilter.id);
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

    function AllSanatory() {
        $scope.myVal = "Санаторий";
        $http({
            method: 'get',
            url: '/Home/GetSanatory'
        }).then(function (s) {
            $scope.CS = s.data;
        }, function () {
            alertify.error("ошибка");
            console.log('Error in getting Санаторий records');
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function AllCamp() {
        $scope.myVal = "Дет. лагер";
        $http({
            method: 'get',
            url: '/Home/GetCamp'
        }).then(function (c) {
            $scope.CS = c.data;
        }, function () {
            alertify.error("ошибка");
            console.log('Error in getting Дет. лагер records');
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }



    function GetUpperOrganization() {
        $http({
            method: 'get',
            url: '/Home/GetUpperOrganization'
        }).then(function (c) {
            $scope.UpperOrganization = c.data;
        }, function () {
            alertify.error("ошибка");
            console.log('Error in GetUpperOrganization');
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.editPZ = function (pz) {
        $scope.showdelete = true;
        $scope.ID = pz.ID;
        var zb = (pz.ZB) ? $filter('date')(parseInt(pz.ZB.substr(6)), 'dd.MM.yyyy') : '';
        var ze = (pz.ZE) ? $filter('date')(parseInt(pz.ZE.substr(6)), 'dd.MM.yyyy') : '';
        $scope.ZB = new Date(zb.substring(6, 10), zb.substring(3, 5) - 1, zb.substring(0, 2));
        $scope.ZE = new Date(ze.substring(6, 10), ze.substring(3, 5) - 1, ze.substring(0, 2));;
        $scope.INN = pz.INN;
        $scope.O = pz.O;
        $scope.FN = pz.FN;
        $scope.P = pz.P;
        $scope.Q = pz.Q;
        $scope.MOE = pz.MOE;
        $scope.MO = pz.MO;
       // $scope.MO2 = pz.MO2;
        $scope.SC.id = pz.SC_id;
        $scope.SC.value = pz.SC;
        $scope.InnFiz = pz.InnFiz;
        $scope.pnfl = pz.pnfl;
        $scope.Action = "Изменить";
    }

    $scope.AddPZDiv = function () {
        $scope.showdelete = false;
        $scope.MOE = false;
        $scope.Q = 1;
        ClearFields();
        $scope.Action = "Добавить";
    }

    function GetPZ_S(id) {
        $scope.ss = "option1s";
        $scope.myVal = "Санаторий";
        $http({
            method: "post",
            url: "GetAllPZ_S?sc_id=" + id,
        }).then(function (s) {
            $scope.PZ = s.data;
            if (s.data.length != 0) {
                $scope.PZdiv = true;
                $scope.OTPdiv = false;
            }
        }, function () {
            alertify.error("ошибка");
            console.log("GetAllPZ_S");
            $("#hh").modal('hide');
        });
    }

    function GetPZ_C(id) {
        $http({
            method: "post",
            url: "GetAllPZ_C?sc_id="+id,
        }).then(function (c) {
            $scope.PZ = c.data;
            if (c.data.length != 0) {
                $scope.PZdiv = true;
                $scope.OTPdiv = false;
            }
        }, function () {
            alertify.error("ошибка");
            console.log('Error in GetAllPZ_C');
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.LoadData= function () {
        if ($scope.ss === "option1s") {
            GetPZ_S($scope.SCFilter.id);
        }
        else {
            if ($scope.ss === "option2s") {
                GetPZ_C($scope.SCFilter.id);
            }
        }
    }

    $scope.LoadSC = function () {
        $("#hh").modal({
            backdrop: 'static'
        });
        $scope.SCFilter.id = 1;
        if ($scope.ss === "option1s") {
            AllSanatory()
            GetPZ_S($scope.SCFilter.id);
        }
        else {
            if ($scope.ss === "option2s") {
                AllCamp();
                GetPZ_C($scope.SCFilter.id);
            }
        }
        $("#hh").modal('hide');
    }

    $scope.MOE_ch = function () {
        if ($scope.MOE == false) {
            $scope.MO = "";
        }
    }

    function LoadINN(inn){
        $scope.O = "";
        try {
            
            if (inn.length == 9) {
                $http({
                    method: 'get',
                    url: '/Home/LoadINN?inn=' + inn
                }).then(function (response) {
                    console.log(response, 'res');
                    $scope.O = response.data;
                }, function (error) {
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

    $scope.AddUpdatePZ=function(){
        var getGZAction = $scope.Action;
        var PZ;
        if ($scope.ss == "option1s") {
            PZ = {
                ZAEZD_BEGIN: $scope.ZB,
                ZAEZD_END: $scope.ZE,
                MEDICAL_ORDER: $scope.MO,
                FULL_NAME: $scope.FN,
                DEPARTMENT_CODE: $scope.INN,
                POSITION: $scope.P,
                QUANTITY: $scope.Q,
                SANATORY_ID: $scope.SC.id,
                MEDICAL_ORDER_EXISTS: $scope.MOE,
                SOTR_INN: $scope.InnFiz,
                PNFL : $scope.pnfl
            };
        } else {
            if ($scope.ss == "option2s") {
                PZ = {
                    ZAEZD_BEGIN: $scope.ZB,
                    ZAEZD_END: $scope.ZE,
                    MEDICAL_ORDER: $scope.MO,
                    FULL_NAME: $scope.FN,
                    DEPARTMENT_CODE: $scope.INN,
                    POSITION: $scope.P,
                    QUANTITY: $scope.Q,
                    CAMP_ID: $scope.SC.id,
                    MEDICAL_ORDER_EXISTS: $scope.MOE,
                    SOTR_INN: $scope.InnFiz,
                    PNFL: $scope.pnfl
                };
            }
        }

        if (getGZAction == "Изменить") {
            PZ.REC_ID = $scope.ID;
            $http({
                method: "post",
                url: "UpdatePodachaZayavok",
                data: JSON.stringify(PZ),
                dataType: "json"
            }).then(function (msg) {
                if ($scope.ss === "option1s") {
                    GetPZ_S($scope.SCFilter.id);
                } else {
                    if ($scope.ss === "option2s") {
                        GetPZ_C($scope.SCFilter.id);
                    }
                }
            }, function () {
                alertify.error("ошибка");
                console.log('Error in UpdatePodachaZayavok');
            });
        }
        else {
            $http({
                method: "post",
                url: "AddPodachaZayavok",
                data: JSON.stringify(PZ),
                dataType: "json"
            }).then(function (msg) {

                if (msg === "0")
                {
                    alertify.success("Удачное сохранение");
                }
                else
                {
                    alertify.error(msg.data);
                }

                if ($scope.ss === "option1s") {
                    GetPZ_S($scope.SCFilter.id);
                } else {
                    if ($scope.ss === "option2s") {
                        GetPZ_C($scope.SCFilter.id);
                    }
                }
            }, function () {
                alertify.error("ошибка");
                console.log('Error in AddPodachaZayavok');
            });
        }
    }

    $scope.OTPpz = function () {
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        reset();
        alertify.set({ buttonFocus: "cancel" });
        alertify.confirm("Вы действительно хотите отправить заявки?", function (e) {
            if (e) {
                alertify.success("Операция выполняется");
                $http({
                    method: "post",
                    url: "OTPPodachaZayavok"
                }).then(function (msg) {
                    $scope.OTPdiv = true;
                    $scope.PZdiv = false;
                    if ($scope.ss === "option1s") {
                        GetPZ_S($scope.SCFilter.id);
                    } else {
                        if ($scope.ss === "option2s") {
                            GetPZ_C($scope.SCFilter.id);
                        }
                    }
                    $("#hh").modal('hide');
                }, function () {
                    alertify.error("ошибка");
                    console.log('Error in OTPPodachaZayavok');
                });
            } else {
                $("#hh").modal('hide');
                alertify.error("Операция отменена");
            }
        });
        return false;
    }

    $scope.deletePZ = function (ID) {
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
                    url: "DeletePodachaZayavok",
                    params: {
                        id: JSON.stringify(ID)
                    }
                }).then(function (msg) {
                    if ($scope.ss === "option1s") {
                        GetPZ_S($scope.SCFilter.id);
                    } else {
                        if ($scope.ss === "option2s") {
                            GetPZ_C($scope.SCFilter.id);
                        }
                    }
                    $("#hh").modal('hide');
                }, function () {
                    alertify.error("ошибка");
                    console.log('Error in DeletePodachaZayavok');
                });
            } else {
                $("#hh").modal('hide');
                alertify.error("Операция отменена");
            }
        });
        return false;
        
    }

    function ClearFields() {
        var zb = $filter('date')(new Date(), 'dd.MM.yyyy');
        var ze = $filter('date')(new Date(), 'dd.MM.yyyy');
        $scope.ZB = new Date(zb.substring(6, 10), zb.substring(3, 5) - 1, zb.substring(0, 2));
        $scope.ZE = new Date(ze.substring(6, 10), ze.substring(3, 5) - 1, ze.substring(0, 2));
        $scope.SC.id = 1;
        $scope.P = "";
        $scope.INN = "";
        $scope.FN = "";
        $scope.MO = "";
        $scope.InnFiz = "";
        $scope.pnfl = "";
    }

    $scope.Cancel = function () {

    };

    $scope.exportData = function () {
        var blob = new Blob([document.getElementById('export').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Подача заявок.xls");
    };
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
}]);