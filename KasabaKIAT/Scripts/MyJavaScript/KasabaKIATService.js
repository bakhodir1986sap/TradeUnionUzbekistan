app.service("crudAJService", function ($http) {
    //Delete GrafikZaezdov
    this.DeleteGrafikZaezdov = function (bookId) {
        var response = $http({
            method: "post",
            url: "DeleteGrafikZaezdov",
            params: {
                bookId: JSON.stringify(bookId)
            }
        });
        return response;
    }
   
     //get All Sanatory
    this.getSs = function (v) {
        var response = $http({
            method: "post",
            url: "GetSanatory",
        });
        return response;
    };
    //get All Camp
    this.getCs = function () {
        var response = $http({
            method: "post",
            url: "GetCamp",
           
        });
        return response;
    };
     //get All PodachaZayavok
    this.getPZs = function () {
        var response = $http({
            method: "post",
            url: "GetAllPodachaZayavok",
        });
        return response;
    };
   
    // Add PrixodPutevok
    this.AddPrixodPutevok = function (pp) {
        var response = $http({
            method: "post",
            url: "AddPrixodPutevok",
            data: JSON.stringify(pp),
            dataType: "json"
        });
        return response;
    }
   
    //poisk i redaktirovat
  
   
    //  get GetAllPodachaZayavok_S_LoadINN
    this.getRZ_S_INN = function (inn, zb) {
        var response = $http({
            method: "post",
            url: "GetAllPodachaZayavok_S_LoadINN?inn=" + inn+"&zb="+zb
        });
        return response;
    }
    //  get GetAllPodachaZayavok_C_LoadINN
    this.getRZ_C_INN = function (inn, zb) {
        var response = $http({
            method: "post",
            url: "GetAllPodachaZayavok_C_LoadINN?inn=" + inn + "&zb=" + zb
        });
        return response;
    }
    //  get GroupRZ_S
    this.getGroupRZ_S = function (inn, zb) {
        var response = $http({
            method: "post",
            url: "GroupRZ_S?inn=" + inn + "&zb=" + zb
        });
        return response;
    }
    //  get GroupRZ_C
    this.getGroupRZ_C = function (inn, zb) {
        var response = $http({
            method: "post",
            url: "GroupRZ_C?inn=" + inn + "&zb=" + zb
        });
        return response;
    }
    //  get GetPrice
    this.GetPrice = function (zb) {
        var response = $http({
            method: "post",
            url: "GetAvarageprice?yil=" + zb
        });
        return response;
    }
    //  get Getbankpayments
    this.Getbankpayments = function (inn,zb) {
        var response = $http({
            method: "post",
            url: "Getbankpayments?inn=" + inn + "&zb=" + zb
        });
        return response;
    }
    //  get GetbankpaymentsYear
    this.GetbankpaymentsYear = function (inn, zb) {
        var response = $http({
            method: "post",
            url: "GetbankpaymentsYear?inn=" + inn + "&zb=" + zb
        });
        return response;
    }
    //  get AddOQ_S
    this.AddOQ_S = function (inn, zb, ZB_g, SC_g_id, OQ) {
        var response = $http({
            method: "post",
            url: "Add_RZ_S_OQ?inn=" + inn + "&zb=" + zb + "&zb_g=" + ZB_g + "&SC_id=" + SC_g_id + "&oq=" + OQ
        });
        return response;
    }
    //  get AddOQ_C
    this.AddOQ_C = function (inn, zb, ZB_g, SC_g_id, OQ) {
        var response = $http({
            method: "post",
            url: "Add_RZ_C_OQ?inn=" + inn + "&zb=" + zb + "&zb_g=" + ZB_g + "&SC_id=" + SC_g_id + "&oq=" + OQ
        });
        return response;
    }
    //  get UpdateStatusRZ
    this.UpdateStatusRZ = function (id, s) {
        var response = $http({
            method: "post",
            url: "UpdateStatusRZ?id=" + id + "&s=" + s
        });
        return response;
    }
    /* GetPrixodPutevok   */
    //get All GetPrixodPutevok
    this.GetPrixodPutevok = function () {
        var response = $http({
            method: "post",
            url: "GetPrixodPutevok",

        });
        return response;
    };
});
/*
,
            params: {
                inn: JSON.stringify(inn)
            }
*/