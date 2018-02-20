app.factory('DataService', ["$http", function () {
    //var savedData = {
    //    inn:"",
    //    name:""
    //}
    //service
    //factory
    var d = {};
    var stringArray = new Array();
    var postData = {};
    var sss="00000000";
    function set(san, num, inn) {
        sss=inn;
        for (var k = 0; k < san.length; k++) {
            d = {
                SanId: san[k],
                NumPut: num[k],
            };
            stringArray[k] = d;
            postData = { b: stringArray, inn: inn };
        }
    }
    function get() {
        return postData;
    }

    return {
        set: set,
        get: get
    }
}]);