app.factory('DataService', ["$http", function () {
    var savedData = {
        inn:"123456789",
        name:""
    }
    function set(inn,name) {
        savedData.inn = inn;
        savedData.name= name;
    }
    function get() {
        return savedData;
    }

    return {
        set: set,
        get: get
    }
    }]);