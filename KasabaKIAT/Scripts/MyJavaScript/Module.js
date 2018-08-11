var app = angular.module("mvcCRUDApp", ["chart.js"]);
app.filter('jsonDate', ['$filter', function ($filter) {
    return function (input, format) {
        return (input)
               ? $filter('date')(parseInt(input.substr(6)), format)
               : '';
    };
}]);
app.filter('customCurrency', function () {
    return function (input, symbol, place) {
        if (isNaN(input)) {
            return input;
        } else {
            var symbol = symbol || ' ';
            var place = place === undefined ? true : place;
            if (place === true) {
                return symbol + input;
            } else {
                var new_input = input.replace(".", " ");
                return new_input + symbol;
            }
        }
    }
});
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

