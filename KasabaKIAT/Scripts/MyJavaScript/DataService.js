app.service('DataService', ["$http", function () {
    var savedData = {
        inn:"123456789",
        name:"aaaaaaaa"
    }
    //service
    //factory
   
    function set(inn, name) {
        savedData.inn = inn;
        savedData.name = name;
        //alert("set " + savedData.inn + " " + savedData.name);
    }
    function get() {
       //alert("get " + savedData.inn + " " + savedData.name);
        return savedData;
    }

    return {
        set: set,
        get: get
    }
}]);