var app = angular.module("mvcCRUDApp",[]);
app.filter('jsonDate', ['$filter', function ($filter) {
    return function (input, format) {
        return (input)
               ? $filter('date')(parseInt(input.substr(6)), format)
               : '';
    };
}]);
app.factory('MathService', function () {
    var factory = {};

    factory.multiply = function (a, b) {
        return a * b
    }
    return factory;
});
var i=0;
app.service('CalcService', function () {
    this.square = function (inn) {
        i = inn;
        //alert("inn="+inn+" i="+i);
        return i;
    }
    this.square2 = function () {
        alert("i=" + i);
        return i;
    }
});

