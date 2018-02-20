var app = angular.module("mvcCRUDApp",[]);
app.filter('jsonDate', ['$filter', function ($filter) {
    return function (input, format) {
        return (input)
               ? $filter('date')(parseInt(input.substr(6)), format)
               : '';
    };
}]);
app.factory('MathService', function () {
    //var factory = {};

    //factory.multiply = function (a, b) {
    //    return a * b
    //}
    //return factory;

    var d = {};
    var stringArray = new Array();
    var postData = {};
    postData.multiply = function (san, num, inn) {
        sss = inn;
        alert(sss);
        for (var k = 0; k < san.length; k++) {
            d = {
                SanId: san[k],
                NumPut: num[k],
            };
            stringArray[k] = d;
            postData = { b: stringArray, inn: inn };
        }
    }
    return postData;
});
var s = {};
var n = {};
var i;
app.service('CalcService', function (MathService) {
    this.square = function (san, num, inn) {
        s = san;
        n = num;
        i = inn;
        alert("inn="+i);
       // return MathService.multiply(san, num, inn);
    }
    this.square2 = function () {
        alert("i=" + i);
        return MathService.multiply(s, n, i);
    }
});

