app.controller("PlatejnoyePorucheniye", ["$scope", "$http", "$filter", "DataService", "CalcService", function ($scope, $http, $filter, DataService, CalcService) {
    LoadDATA();
    $scope.showdata = false;
    $scope.showdata2 = false;
    
    function LoadDATA() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'POST',
            url: "/PlatejnoyePorucheniye/GetPlatejnoyePorucheniye",
        }).then(function (d) {
            if (d.data.length != 0) {
                $scope.showdata = true;
            } else {
                $scope.showdata = false;
            }
            $scope.DATA = d.data;
            console.log(d.data);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetPlatejnoyePorucheniye -> ", error);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }
    $scope.novoe = function () {
        //$scope.PD = $filter('date')(new Date(), 'dd.MM.yyyy');
        $scope.PD = new Date();
        $scope.N = "";
        $scope.P1NAME = "";
        $scope.P1INN = "";
        $scope.P1SCH = "";
        $scope.P1BN = "";
        $scope.P1MFO = "";
        $scope.P2NAME = "";
        $scope.P2INN = "";
        $scope.P2SCH = "";
        $scope.P2BN = "";
        $scope.P2MFO = "";
        $scope.SUM = "";
        $scope.COM = "";
        $scope.R = "";
        $scope.GB = "";
    };
    $scope.Load = function (data) {
        //var zb = (data.d1) ? $filter('date')(parseInt(data.d1.substr(6)), 'dd.MM.yyyy') : '';
        //$scope.d1 = new Date(zb.substring(6, 10), zb.substring(3, 5) - 1, zb.substring(0, 2));
        $scope.d1 = data.d1;
        $scope.d2 = data.d2;
        $scope.d3 = data.d3;
        $scope.d4 = data.d4;
        $scope.d5 = data.d5;
        $scope.d6 = data.d6;
        $scope.d7 = data.d7;
        $scope.d8 = data.d8;
        $scope.d9 = data.d9;
        $scope.d10 = data.d10;
        $scope.d11 = data.d11;
        $scope.d12 = data.d12;
        $scope.d13 = data.d13;
        $scope.d14 = data.d14;
        $scope.d15 = data.d15;
        $scope.d16 = data.d16;
    };
    $scope.Add = function () {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        var GZ = {
            plat_date: $scope.PD,
            plat_num: $scope.N,
            platelname: $scope.P1NAME,
            platelinn: $scope.P1INN,
            platelraschet: $scope.P1SCH,
            platelbankname: $scope.P1BN,
            platelbankmfo: $scope.P1MFO,
            poluchname: $scope.P2NAME,
            poluchinn: $scope.P2INN,
            poluchraschet: $scope.P2SCH,
            poluchbankname: $scope.P2BN,
            poluchbankmfo: $scope.P2MFO,
            summa: $scope.SUM,
            comment: $scope.COM,
            rukovoditel: $scope.R,
            glavbux: $scope.GB,

        };

        $http({
            method: "post",
            url: "/PlatejnoyePorucheniye/EnterPlatejnoyePorucheniye",
            data: JSON.stringify(GZ),
            dataType: "json"
        }).then(function (msg) {
            LoadDATA();
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
            //alertify.success(msg.data);
        }, function () {
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
            alertify.error("ошибка");
            console.log('Error in EnterPlatejnoyePorucheniye');
        });
    };
    $scope.downloadPDF = function () {

        var pdf = new jsPDF('p', 'pt', 'letter')

    // source can be HTML-formatted string, or a reference
    // to an actual DOM element from which the text will be scraped.
    , source = $('#fromHTMLtestdiv')[0]

    // we support special element handlers. Register them with jQuery-style
    // ID selector for either ID or node name. ("#iAmID", "div", "span" etc.)
    // There is no support for any other type of selectors
    // (class, of compound) at this time.
    , specialElementHandlers = {
        // element with id of "bypass" - jQuery style selector
        '#bypassme': function (element, renderer) {
            // true = "handled elsewhere, bypass text extraction"
            return true
        },
        '.hide': function (element, renderer) {
            // true = "handled elsewhere, bypass text extraction"
            return true
        }
    }

        margins = {
            top: 80,
            bottom: 60,
            left: 40,
            width: 522
        };
        // all coords and widths are in jsPDF instance's declared units
        // 'inches' in this case
        pdf.fromHTML(
            source // HTML string or DOM elem ref.
            , margins.left // x coord
            , margins.top // y coord
            , {
                'width': margins.width // max width of content on PDF
                , 'elementHandlers': specialElementHandlers
            },
            function (dispose) {
                // dispose: object with X, Y of the last line add to the PDF
                //          this allow the insertion of new lines after html
                pdf.save('Платежное поручение.pdf');
            },
            margins
          )
    }
    $scope.cmd1 = function () {
        document.getElementById('connection').style.visibility = "visible";
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
                        temp.style = "color: black;margin:0px;font-size:20px;";
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
                    doc.addImage(ru_text[0], 'JPEG', 10, 10, (width - 20), (height - 20), 'monkey');
                    //doc.text(0, 10, 'Non-utf-8-string');

                    doc.save('Платежное поручение.pdf');
                    document.getElementById('connection').style.visibility = "hidden";
                    $("#hh").modal('hide');
                });
    }
    function tableToJson(table) {
        var data = [];

        // first row needs to be headers
        var headers = [];
        for (var i = 0; i < table.rows[0].cells.length; i++) {
            headers[i] = table.rows[0].cells[i].innerHTML.toLowerCase().replace(/ /gi, '');
        }


        // go through cells
        for (var i = 0; i < table.rows.length; i++) {

            var tableRow = table.rows[i];
            var rowData = {};

            for (var j = 0; j < tableRow.cells.length; j++) {

                rowData[headers[j]] = tableRow.cells[j].innerHTML;

            }

            data.push(rowData);
        }

        return data;
    }
}]);