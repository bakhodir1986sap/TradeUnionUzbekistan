app.controller("GlavnayaKniga", ["$scope", "$http", "$filter", "$locale", "DataService", "CalcService", function ($scope, $http, $filter, $locale ,DataService, CalcService) {
    $scope.showdata = false;
    $scope.ZB = new Date
    $scope.ZE = new Date();

    $scope.rasxod_010_3 = 0;
    $scope.rasxod_010_4 = 0;
    $scope.rasxod_010_5 = 0;
    $scope.rasxod_010_6 = 0;
    $scope.rasxod_010_7 = 0;
    $scope.rasxod_010_8 = 0;
    $scope.rasxod_010_9 = 0;
    $scope.rasxod_010_10 = 0;
    $scope.rasxod_010_11 = 0;
    $scope.rasxod_010_12 = 0;

    $scope.rasxod_020_3 = 0;
    $scope.rasxod_020_4 = 0;
    $scope.rasxod_020_5 = 0;
    $scope.rasxod_020_6 = 0;
    $scope.rasxod_020_7 = 0;
    $scope.rasxod_020_8 = 0;
    $scope.rasxod_020_9 = 0;
    $scope.rasxod_020_10 = 0;
    $scope.rasxod_020_11 = 0;
    $scope.rasxod_020_12 = 0;

    $scope.rasxod_030_3 = 0;
    $scope.rasxod_030_4 = 0;
    $scope.rasxod_030_5 = 0;
    $scope.rasxod_030_6 = 0;
    $scope.rasxod_030_7 = 0;
    $scope.rasxod_030_8 = 0;
    $scope.rasxod_030_9 = 0;
    $scope.rasxod_030_10 = 0;
    $scope.rasxod_030_11 = 0;
    $scope.rasxod_030_12 = 0;

    $scope.rasxod_040_3 = 0;
    $scope.rasxod_040_4 = 0;
    $scope.rasxod_040_5 = 0;
    $scope.rasxod_040_6 = 0;
    $scope.rasxod_040_7 = 0;
    $scope.rasxod_040_8 = 0;
    $scope.rasxod_040_9 = 0;
    $scope.rasxod_040_10 = 0;
    $scope.rasxod_040_11 = 0;
    $scope.rasxod_040_12 = 0;

    $scope.rasxod_060_3 = 0;
    $scope.rasxod_060_4 = 0;
    $scope.rasxod_060_5 = 0;
    $scope.rasxod_060_6 = 0;
    $scope.rasxod_060_7 = 0;
    $scope.rasxod_060_8 = 0;
    $scope.rasxod_060_9 = 0;
    $scope.rasxod_060_10 = 0;
    $scope.rasxod_060_11 = 0;
    $scope.rasxod_060_12 = 0;

    $scope.rasxod_070_3 = 0;
    $scope.rasxod_070_4 = 0;
    $scope.rasxod_070_5 = 0;
    $scope.rasxod_070_6 = 0;
    $scope.rasxod_070_7 = 0;
    $scope.rasxod_070_8 = 0;
    $scope.rasxod_070_9 = 0;
    $scope.rasxod_070_10 = 0;
    $scope.rasxod_070_11 = 0;
    $scope.rasxod_070_12 = 0;

    $scope.rasxod_080_3 = 0;
    $scope.rasxod_080_4 = 0;
    $scope.rasxod_080_5 = 0;
    $scope.rasxod_080_6 = 0;
    $scope.rasxod_080_7 = 0;
    $scope.rasxod_080_8 = 0;
    $scope.rasxod_080_9 = 0;
    $scope.rasxod_080_10 = 0;
    $scope.rasxod_080_11 = 0;
    $scope.rasxod_080_12 = 0;

    $scope.rasxod_090_3 = 0;
    $scope.rasxod_090_4 = 0;
    $scope.rasxod_090_5 = 0;
    $scope.rasxod_090_6 = 0;
    $scope.rasxod_090_7 = 0;
    $scope.rasxod_090_8 = 0;
    $scope.rasxod_090_9 = 0;
    $scope.rasxod_090_10 = 0;
    $scope.rasxod_090_11 = 0;
    $scope.rasxod_090_12 = 0;

    $scope.rasshifrovka_010 = 0;
    $scope.rasshifrovka_020 = 0;
    $scope.rasshifrovka_030 = 0;
    $scope.rasshifrovka_040 = 0;
    $scope.rasshifrovka_050 = 0;

    $scope.drugiye_010 = 0;
    $scope.drugiye_020 = 0;
    $scope.drugiye_030 = 0;
    $scope.drugiye_040 = 0;
    $scope.drugiye_050 = 0;
    $scope.drugiye_060 = 0;

    $scope.T1R010C01 = 0;
    $scope.T1R010C02 = 0;
    $scope.T1R010C03 = 0;
    $scope.T1R010C04 = 0;
    $scope.T1R010C05 = 0;
    $scope.T1R020C01 = 0;
    $scope.T1R020C02 = 0;
    $scope.T1R020C03 = 0;
    $scope.T1R020C04 = 0;
    $scope.T1R020C05 = 0;
    $scope.T1R030C01 = 0;
    $scope.T1R030C02 = 0;
    $scope.T1R030C03 = 0;
    $scope.T1R030C04 = 0;
    $scope.T1R030C05 = 0;
    $scope.T1R040C01 = 0;
    $scope.T1R040C02 = 0;
    $scope.T1R040C03 = 0;
    $scope.T1R040C04 = 0;
    $scope.T1R040C05 = 0;
    $scope.T1R050C01 = 0;
    $scope.T1R050C02 = 0;
    $scope.T1R050C03 = 0;
    $scope.T1R050C04 = 0;
    $scope.T1R050C05 = 0;
    $scope.T1R060C01 = 0;
    $scope.T1R060C02 = 0;
    $scope.T1R060C03 = 0;
    $scope.T1R060C04 = 0;
    $scope.T1R060C05 = 0;
    $scope.T1R070C01 = 0;
    $scope.T1R070C02 = 0;
    $scope.T1R070C03 = 0;
    $scope.T1R070C04 = 0;
    $scope.T1R070C05 = 0;
    $scope.T1R080C01 = 0;
    $scope.T1R080C02 = 0;
    $scope.T1R080C03 = 0;
    $scope.T1R080C04 = 0;
    $scope.T1R080C05 = 0;
    $scope.T1R090C01 = 0;
    $scope.T1R090C02 = 0;
    $scope.T1R090C03 = 0;
    $scope.T1R090C04 = 0;
    $scope.T1R090C05 = 0;
    $scope.T1R100C01 = 0;
    $scope.T1R100C02 = 0;
    $scope.T1R100C03 = 0;
    $scope.T1R100C04 = 0;
    $scope.T1R100C05 = 0;
    $scope.T1R110C01 = 0;
    $scope.T1R110C02 = 0;
    $scope.T1R110C03 = 0;
    $scope.T1R110C04 = 0;
    $scope.T1R110C05 = 0;
    $scope.T1R120C01 = 0;
    $scope.T1R120C02 = 0;
    $scope.T1R120C03 = 0;
    $scope.T1R120C04 = 0;
    $scope.T1R120C05 = 0;
    $scope.T1R130C01 = 0;
    $scope.T1R130C02 = 0;
    $scope.T1R130C03 = 0;
    $scope.T1R130C04 = 0;
    $scope.T1R130C05 = 0;
    $scope.T1R140C01 = 0;
    $scope.T1R140C02 = 0;
    $scope.T1R140C03 = 0;
    $scope.T1R140C04 = 0;
    $scope.T1R140C05 = 0;
    $scope.T1R150C01 = 0;
    $scope.T1R150C02 = 0;
    $scope.T1R150C03 = 0;
    $scope.T1R150C04 = 0;
    $scope.T1R150C05 = 0;
    $scope.T1R160C01 = 0;
    $scope.T1R160C05 = 0;
    $scope.T1R170C01 = 0;
    $scope.T1R170C05 = 0;
    $scope.T1R180C01 = 0;
    $scope.T1R180C02 = 0;
    $scope.T1R180C03 = 0;
    $scope.T1R180C04 = 0;
    $scope.T1R180C05 = 0;
    $scope.T1R190C01 = 0;
    $scope.T1R190C02 = 0;
    $scope.T1R190C03 = 0;
    $scope.T1R190C04 = 0;
    $scope.T1R190C05 = 0;
    $scope.T1R200C01 = 0;
    $scope.T1R200C02 = 0;
    $scope.T1R200C03 = 0;
    $scope.T1R200C04 = 0;
    $scope.T1R200C05 = 0;
    $scope.T1R210C01 = 0;
    $scope.T1R210C02 = 0;
    $scope.T1R210C03 = 0;
    $scope.T1R210C04 = 0;
    $scope.T1R210C05 = 0;
    $scope.T1R220C01 = 0;
    $scope.T1R220C02 = 0;
    $scope.T1R220C03 = 0;
    $scope.T1R220C04 = 0;
    $scope.T1R220C05 = 0;
    $scope.T1R230C01 = 0;
    $scope.T1R230C02 = 0;
    $scope.T1R230C03 = 0;
    $scope.T1R230C04 = 0;
    $scope.T1R230C05 = 0;
    $scope.T1R240C01 = 0;
    $scope.T1R240C05 = 0;
    $scope.T1R250C01 = 0;
    $scope.T1R250C05 = 0;
    $scope.T1R280C01 = 0;
    $scope.T1R280C02 = 0;
    $scope.T1R280C03 = 0;
    $scope.T1R280C04 = 0;
    $scope.T1R280C05 = 0;
    $scope.T2R010C01 = 0;
    $scope.T2R010C02 = 0;
    $scope.T2R020C01 = 0;
    $scope.T2R020C02 = 0;
    $scope.T2R030C01 = 0;
    $scope.T2R030C02 = 0;
    $scope.T2R040C01 = 0;
    $scope.T2R040C02 = 0;
    $scope.T2R050C01 = 0;
    $scope.T2R050C02 = 0;
    $scope.T2R060C01 = 0;
    $scope.T2R060C02 = 0;
    $scope.T2R070C01 = 0;
    $scope.T2R070C02 = 0;
    $scope.T2R080C01 = 0;
    $scope.T2R080C02 = 0;
    $scope.T2R090C01 = 0;
    $scope.T2R090C02 = 0;
    $scope.T2R110C01 = 0;
    $scope.T2R110C02 = 0;
    $scope.T2R120C01 = 0;
    $scope.T2R120C02 = 0;
    $scope.T2R130C01 = 0;
    $scope.T2R130C02 = 0;
    $scope.T2R140C01 = 0;
    $scope.T2R140C02 = 0;
    $scope.T2R150C01 = 0;
    $scope.T2R150C02 = 0;
    $scope.T2R170C01 = 0;
    $scope.T2R170C02 = 0;
    $scope.T2R180C01 = 0;
    $scope.T2R180C02 = 0;
    $scope.T2R190C01 = 0;
    $scope.T2R190C02 = 0;
    $scope.T2R200C01 = 0;
    $scope.T2R200C02 = 0;
    $scope.T2R160C01 = 0;
    $scope.T2R160C02 = 0;
    $scope.T2R100C01 = 0;
    $scope.T2R210C01 = 0;
    $scope.T2R100C02 = 0;
    $scope.T2R210C02 = 0;
    $scope.T2R230C01 = 0;
    $scope.T2R230C02 = 0;
    $scope.T2R240C01 = 0;
    $scope.T2R240C02 = 0;
    $scope.T2R010C03 = 0;
    $scope.T2R010C04 = 0;
    $scope.T2R010C05 = 0;
    $scope.T2R010C06 = 0;
    $scope.T2R010C07 = 0;
    $scope.T2R010C08 = 0;
    $scope.T2R010C09 = 0;
    $scope.T2R010C10 = 0;
    $scope.T2R010C11 = 0;
    $scope.T2R010C12 = 0;
    $scope.T2R020C03 = 0;
    $scope.T2R020C04 = 0;
    $scope.T2R020C05 = 0;
    $scope.T2R020C06 = 0;
    $scope.T2R020C07 = 0;
    $scope.T2R020C08 = 0;
    $scope.T2R020C09 = 0;
    $scope.T2R020C10 = 0;
    $scope.T2R020C11 = 0;
    $scope.T2R020C12 = 0;
    $scope.T2R030C03 = 0;
    $scope.T2R030C04 = 0;
    $scope.T2R030C05 = 0;
    $scope.T2R030C06 = 0;
    $scope.T2R030C07 = 0;
    $scope.T2R030C08 = 0;
    $scope.T2R030C09 = 0;
    $scope.T2R030C10 = 0;
    $scope.T2R030C11 = 0;
    $scope.T2R030C12 = 0;
    $scope.T3R010C01 = 0;
    $scope.T3R010C02 = 0;
    $scope.T3R010C03 = 0;
    $scope.T2R040C03 = 0;
    $scope.T2R040C04 = 0;
    $scope.T2R040C05 = 0;
    $scope.T2R040C06 = 0;
    $scope.T2R040C07 = 0;
    $scope.T2R040C08 = 0;
    $scope.T2R040C09 = 0;
    $scope.T2R040C10 = 0;
    $scope.T2R040C11 = 0;
    $scope.T2R040C12 = 0;
    $scope.T3R020C01 = 0;
    $scope.T3R020C02 = 0;
    $scope.T3R020C03 = 0;
    $scope.T2R050C03 = 0;
    $scope.T2R050C04 = 0;
    $scope.T2R050C05 = 0;
    $scope.T2R050C06 = 0;
    $scope.T2R050C07 = 0;
    $scope.T2R050C08 = 0;
    $scope.T2R050C09 = 0;
    $scope.T2R050C10 = 0;
    $scope.T2R050C11 = 0;
    $scope.T2R050C12 = 0;
    $scope.T3R030C01 = 0;
    $scope.T3R030C02 = 0;
    $scope.T3R030C03 = 0;
    $scope.T2R060C03 = 0;
    $scope.T2R060C04 = 0;
    $scope.T2R060C05 = 0;
    $scope.T2R060C06 = 0;
    $scope.T2R060C07 = 0;
    $scope.T2R060C08 = 0;
    $scope.T2R060C09 = 0;
    $scope.T2R060C10 = 0;
    $scope.T2R060C11 = 0;
    $scope.T2R060C12 = 0;
    $scope.T3R040C01 = 0;
    $scope.T3R040C02 = 0;
    $scope.T3R040C03 = 0;
    $scope.T2R070C03 = 0;
    $scope.T2R070C04 = 0;
    $scope.T2R070C05 = 0;
    $scope.T2R070C06 = 0;
    $scope.T2R070C07 = 0;
    $scope.T2R070C08 = 0;
    $scope.T2R070C09 = 0;
    $scope.T2R070C10 = 0;
    $scope.T2R070C11 = 0;
    $scope.T2R070C12 = 0;
    $scope.T3R050C01 = 0;
    $scope.T3R050C02 = 0;
    $scope.T3R050C03 = 0;
    $scope.T2R080C03 = 0;
    $scope.T2R080C04 = 0;
    $scope.T2R080C05 = 0;
    $scope.T2R080C06 = 0;
    $scope.T2R080C07 = 0;
    $scope.T2R080C08 = 0;
    $scope.T2R080C09 = 0;
    $scope.T2R080C10 = 0;
    $scope.T2R080C11 = 0;
    $scope.T2R080C12 = 0;
    $scope.T3R060C01 = 0;
    $scope.T3R060C02 = 0;
    $scope.T3R060C03 = 0;
    $scope.T2R090C03 = 0;
    $scope.T2R090C04 = 0;
    $scope.T2R090C05 = 0;
    $scope.T2R090C06 = 0;
    $scope.T2R090C07 = 0;
    $scope.T2R090C08 = 0;
    $scope.T2R090C09 = 0;
    $scope.T2R090C10 = 0;
    $scope.T2R090C11 = 0;
    $scope.T2R090C12 = 0;
    $scope.T3R070C01 = 0;
    $scope.T3R070C02 = 0;
    $scope.T3R070C03 = 0;
    $scope.T2R100C03 = 0;
    $scope.T2R100C04 = 0;
    $scope.T2R100C05 = 0;
    $scope.T2R100C06 = 0;
    $scope.T2R100C07 = 0;
    $scope.T2R100C08 = 0;
    $scope.T2R100C09 = 0;
    $scope.T2R100C10 = 0;
    $scope.T2R100C11 = 0;
    $scope.T2R100C12 = 0;
    $scope.T3R080C01 = 0;
    $scope.T3R080C02 = 0;
    $scope.T3R080C03 = 0;
    $scope.T2R110C03 = 0;
    $scope.T2R110C04 = 0;
    $scope.T2R110C05 = 0;
    $scope.T2R110C06 = 0;
    $scope.T2R110C07 = 0;
    $scope.T2R110C08 = 0;
    $scope.T2R110C09 = 0;
    $scope.T2R110C10 = 0;
    $scope.T2R110C11 = 0;
    $scope.T2R110C12 = 0;
    $scope.T3R090C01 = 0;
    $scope.T3R090C02 = 0;
    $scope.T3R090C03 = 0;
    $scope.T2R120C03 = 0;
    $scope.T2R120C04 = 0;
    $scope.T2R120C05 = 0;
    $scope.T2R120C06 = 0;
    $scope.T2R120C07 = 0;
    $scope.T2R120C08 = 0;
    $scope.T2R120C09 = 0;
    $scope.T2R120C10 = 0;
    $scope.T2R120C11 = 0;
    $scope.T2R120C12 = 0;
    $scope.T3R100C01 = 0;
    $scope.T3R100C02 = 0;
    $scope.T3R100C03 = 0;
    $scope.T2R130C03 = 0;
    $scope.T2R130C04 = 0;
    $scope.T2R130C05 = 0;
    $scope.T2R130C06 = 0;
    $scope.T2R130C07 = 0;
    $scope.T2R130C08 = 0;
    $scope.T2R130C09 = 0;
    $scope.T2R130C10 = 0;
    $scope.T2R130C11 = 0;
    $scope.T2R130C12 = 0;
    $scope.T3R110C01 = 0;
    $scope.T3R110C02 = 0;
    $scope.T3R110C03 = 0;
    $scope.T2R140C03 = 0;
    $scope.T2R140C04 = 0;
    $scope.T2R140C05 = 0;
    $scope.T2R140C06 = 0;
    $scope.T2R140C07 = 0;
    $scope.T2R140C08 = 0;
    $scope.T2R140C09 = 0;
    $scope.T2R140C10 = 0;
    $scope.T2R140C11 = 0;
    $scope.T2R140C12 = 0;
    $scope.T2R150C03 = 0;
    $scope.T2R150C04 = 0;
    $scope.T2R150C05 = 0;
    $scope.T2R150C06 = 0;
    $scope.T2R150C07 = 0;
    $scope.T2R150C08 = 0;
    $scope.T2R150C09 = 0;
    $scope.T2R150C10 = 0;
    $scope.T2R150C11 = 0;
    $scope.T2R150C12 = 0;
    $scope.T2R160C03 = 0;
    $scope.T2R160C04 = 0;
    $scope.T2R160C05 = 0;
    $scope.T2R160C06 = 0;
    $scope.T2R160C07 = 0;
    $scope.T2R160C08 = 0;
    $scope.T2R160C09 = 0;
    $scope.T2R160C10 = 0;
    $scope.T2R160C11 = 0;
    $scope.T2R160C12 = 0;
    $scope.T2R170C03 = 0;
    $scope.T2R170C04 = 0;
    $scope.T2R170C05 = 0;
    $scope.T2R170C06 = 0;
    $scope.T2R170C07 = 0;
    $scope.T2R170C08 = 0;
    $scope.T2R170C09 = 0;
    $scope.T2R170C10 = 0;
    $scope.T2R170C11 = 0;
    $scope.T2R170C12 = 0;
    $scope.T2R180C03 = 0;
    $scope.T2R180C04 = 0;
    $scope.T2R180C05 = 0;
    $scope.T2R180C06 = 0;
    $scope.T2R180C07 = 0;
    $scope.T2R180C08 = 0;
    $scope.T2R180C09 = 0;
    $scope.T2R180C10 = 0;
    $scope.T2R180C11 = 0;
    $scope.T2R180C12 = 0;
    $scope.T4R010C01 = 0;
    $scope.T4R010C02 = 0;
    $scope.T2R190C03 = 0;
    $scope.T2R190C04 = 0;
    $scope.T2R190C05 = 0;
    $scope.T2R190C06 = 0;
    $scope.T2R190C07 = 0;
    $scope.T2R190C08 = 0;
    $scope.T2R190C09 = 0;
    $scope.T2R190C10 = 0;
    $scope.T2R190C11 = 0;
    $scope.T2R190C12 = 0;
    $scope.T4R020C01 = 0;
    $scope.T4R020C02 = 0;
    $scope.T2R200C03 = 0;
    $scope.T2R200C04 = 0;
    $scope.T2R200C05 = 0;
    $scope.T2R200C06 = 0;
    $scope.T2R200C07 = 0;
    $scope.T2R200C08 = 0;
    $scope.T2R200C09 = 0;
    $scope.T2R200C10 = 0;
    $scope.T2R200C11 = 0;
    $scope.T2R200C12 = 0;
    $scope.T4R030C01 = 0;
    $scope.T4R030C02 = 0;
    $scope.T2R210C03 = 0;
    $scope.T2R210C04 = 0;
    $scope.T2R210C05 = 0;
    $scope.T2R210C06 = 0;
    $scope.T2R210C07 = 0;
    $scope.T2R210C08 = 0;
    $scope.T2R210C09 = 0;
    $scope.T2R210C10 = 0;
    $scope.T2R210C11 = 0;
    $scope.T2R210C12 = 0;
    $scope.T2R220C03 = 0;
    $scope.T2R220C04 = 0;
    $scope.T2R220C05 = 0;
    $scope.T2R220C06 = 0;
    $scope.T2R220C07 = 0;
    $scope.T2R220C08 = 0;
    $scope.T2R220C09 = 0;
    $scope.T2R220C10 = 0;
    $scope.T2R220C11 = 0;
    $scope.T2R220C12 = 0;
    $scope.T4R050C01 = 0;
    $scope.T4R050C02 = 0;
    $scope.T2R230C03 = 0;
    $scope.T2R230C04 = 0;
    $scope.T2R230C05 = 0;
    $scope.T2R230C06 = 0;
    $scope.T2R230C07 = 0;
    $scope.T2R230C08 = 0;
    $scope.T2R230C09 = 0;
    $scope.T2R230C10 = 0;
    $scope.T2R230C11 = 0;
    $scope.T2R230C12 = 0;
    $scope.T2R240C03 = 0;
    $scope.T2R240C04 = 0;
    $scope.T2R240C05 = 0;
    $scope.T2R240C06 = 0;
    $scope.T2R240C07 = 0;
    $scope.T2R240C08 = 0;
    $scope.T2R240C09 = 0;
    $scope.T2R240C10 = 0;
    $scope.T2R240C11 = 0;
    $scope.T2R240C12 = 0;
    $scope.T5R010C01 = 0;
    $scope.T5R020C01 = 0;
    $scope.T5R030C01 = 0;
    $scope.T5R040C01 = 0;
    $scope.T5R050C01 = 0;
    $scope.T5R060C01 = 0;

    $scope.SelectedYear = {
        Id: 2018,
        Year: '2018'
    };

    $scope.SelectedKvartal = {
        ID: 2,
        Name_Kv: '2-квартал'
    };

    $scope.ModalToDepCodeSelected = {
        Name: '',
        Kod: ''
    };

    GetYearsList();
    GetKvartalList();
    GetCurBusinessTime();
    GetToDepForOrganization();

    function GetCurBusinessTime() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Reference/GetBusinessTime'
        }).then(function (response) {

            $scope.CurBusinessTime = response.data;
            $scope.SelectedYear.Id = response.data.CurYear;
            $scope.SelectedYear.Year = response.data.CurYear;

            $scope.SelectedKvartal.ID = response.data.CurKvartal;

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetCurBusinessTime");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function GetYearsList() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Reference/GetYears'
        }).then(function (response) {
            $scope.Years = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetYearsList");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function GetKvartalList() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Reference/GetKvartals'
        }).then(function (response) {
            $scope.Kvartals = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetKvartalList");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    function GetToDepForOrganization() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: '/Buxgalter/SpisokPravsoyuzovOrg'
        }).then(function (response) {
            $scope.ToDepCode = response.data;
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetRekvizits");
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.LoadDATA = function () {
        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";
        LoadDATA($scope.SelectedYear, $scope.SelectedKvartal);
    }

    function LoadDATA(bg, end) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: "/Buxgalter/GlavnayaKniga?year=" + bg.Id + "&kvartal=" + end.ID,
        }).then(function (d) {
            if (d.data.length != 0) {
                $scope.q = d.data.length;
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
            console.log("error in GetPostupleniya -> ", error);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }


    $scope.LoadDATAS = function () {
        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";
        LoadDATAS($scope.SelectedYear, $scope.SelectedKvartal);
    }

    function LoadDATAS(bg, end) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: "/Buxgalter/GlavnayaKnigaSvodniy?year=" + bg.Id + "&kvartal=" + end.ID,
        }).then(function (d) {
            if (d.data.length != 0) {
                $scope.q = d.data.length;
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
            console.log("error in GetPostupleniya -> ", error);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }


    $scope.LoadDATAR = function () {
        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";
        LoadDATAR($scope.SelectedYear, $scope.SelectedKvartal, $scope.ModalToDepCodeSelected.Kod);
    }

    function LoadDATAR(bg, end , ProfCode) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: "/Buxgalter/GlavnayaKnigaVRazreze?year=" + bg.Id + "&kvartal=" + end.ID + '&departmentBase=' + ProfCode,
        }).then(function (d) {
            if (d.data.length != 0) {
                $scope.q = d.data.length;
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
            console.log("error in GetPostupleniya -> ", error);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }


    $scope.LoadDATAF = function () {
        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";
        LoadDATAF($scope.SelectedYear, $scope.SelectedKvartal);
    }

    function LoadDATAF(bg, end) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: "/Buxgalter/FinansoviyOtchet?year=" + bg.Id + "&kvartal=" + end.ID,
        }).then(function (d) {
            if (d.data.length != 0) {
                $scope.q = d.data.length;
                $scope.showdata = true;
            } else {
                $scope.showdata = false;
            }
            $scope.DATAF = d.data;

            if ($scope.DATAF._otheroutput !== null) {
                $scope.rasxod_010_3 = $scope.DATAF._otheroutput.rasxod_010_3;
                $scope.rasxod_010_4 = $scope.DATAF._otheroutput.rasxod_010_4;
                $scope.rasxod_010_5 = $scope.DATAF._otheroutput.rasxod_010_5;
                $scope.rasxod_010_6 = $scope.DATAF._otheroutput.rasxod_010_6;
                $scope.rasxod_010_7 = $scope.DATAF._otheroutput.rasxod_010_7;
                $scope.rasxod_010_8 = $scope.DATAF._otheroutput.rasxod_010_8;
                $scope.rasxod_010_9 = $scope.DATAF._otheroutput.rasxod_010_9;
                $scope.rasxod_010_10 = $scope.DATAF._otheroutput.rasxod_010_10;
                $scope.rasxod_010_11 = $scope.DATAF._otheroutput.rasxod_010_11;
                $scope.rasxod_010_12 = $scope.DATAF._otheroutput.rasxod_010_12;
                $scope.rasxod_020_3 = $scope.DATAF._otheroutput.rasxod_020_3;
                $scope.rasxod_020_4 = $scope.DATAF._otheroutput.rasxod_020_4;
                $scope.rasxod_020_5 = $scope.DATAF._otheroutput.rasxod_020_5;
                $scope.rasxod_020_6 = $scope.DATAF._otheroutput.rasxod_020_6;
                $scope.rasxod_020_7 = $scope.DATAF._otheroutput.rasxod_020_7;
                $scope.rasxod_020_8 = $scope.DATAF._otheroutput.rasxod_020_8;
                $scope.rasxod_020_9 = $scope.DATAF._otheroutput.rasxod_020_9;
                $scope.rasxod_020_10 = $scope.DATAF._otheroutput.rasxod_020_10;
                $scope.rasxod_020_11 = $scope.DATAF._otheroutput.rasxod_020_11;
                $scope.rasxod_020_12 = $scope.DATAF._otheroutput.rasxod_020_12;
                $scope.rasxod_030_3 = $scope.DATAF._otheroutput.rasxod_030_3;
                $scope.rasxod_030_4 = $scope.DATAF._otheroutput.rasxod_030_4;
                $scope.rasxod_030_5 = $scope.DATAF._otheroutput.rasxod_030_5;
                $scope.rasxod_030_6 = $scope.DATAF._otheroutput.rasxod_030_6;
                $scope.rasxod_030_7 = $scope.DATAF._otheroutput.rasxod_030_7;
                $scope.rasxod_030_8 = $scope.DATAF._otheroutput.rasxod_030_8;
                $scope.rasxod_030_9 = $scope.DATAF._otheroutput.rasxod_030_9;
                $scope.rasxod_030_10 = $scope.DATAF._otheroutput.rasxod_030_10;
                $scope.rasxod_030_11 = $scope.DATAF._otheroutput.rasxod_030_11;
                $scope.rasxod_030_12 = $scope.DATAF._otheroutput.rasxod_030_12;
                $scope.rasxod_040_3 = $scope.DATAF._otheroutput.rasxod_040_3;
                $scope.rasxod_040_4 = $scope.DATAF._otheroutput.rasxod_040_4;
                $scope.rasxod_040_5 = $scope.DATAF._otheroutput.rasxod_040_5;
                $scope.rasxod_040_6 = $scope.DATAF._otheroutput.rasxod_040_6;
                $scope.rasxod_040_7 = $scope.DATAF._otheroutput.rasxod_040_7;
                $scope.rasxod_040_8 = $scope.DATAF._otheroutput.rasxod_040_8;
                $scope.rasxod_040_9 = $scope.DATAF._otheroutput.rasxod_040_9;
                $scope.rasxod_040_10 = $scope.DATAF._otheroutput.rasxod_040_10;
                $scope.rasxod_040_11 = $scope.DATAF._otheroutput.rasxod_040_11;
                $scope.rasxod_040_12 = $scope.DATAF._otheroutput.rasxod_040_12;
                $scope.rasxod_060_3 = $scope.DATAF._otheroutput.rasxod_060_3;
                $scope.rasxod_060_4 = $scope.DATAF._otheroutput.rasxod_060_4;
                $scope.rasxod_060_5 = $scope.DATAF._otheroutput.rasxod_060_5;
                $scope.rasxod_060_6 = $scope.DATAF._otheroutput.rasxod_060_6;
                $scope.rasxod_060_7 = $scope.DATAF._otheroutput.rasxod_060_7;
                $scope.rasxod_060_8 = $scope.DATAF._otheroutput.rasxod_060_8;
                $scope.rasxod_060_9 = $scope.DATAF._otheroutput.rasxod_060_9;
                $scope.rasxod_060_10 = $scope.DATAF._otheroutput.rasxod_060_10;
                $scope.rasxod_060_11 = $scope.DATAF._otheroutput.rasxod_060_11;
                $scope.rasxod_060_12 = $scope.DATAF._otheroutput.rasxod_060_12;
                $scope.rasxod_070_3 = $scope.DATAF._otheroutput.rasxod_070_3;
                $scope.rasxod_070_4 = $scope.DATAF._otheroutput.rasxod_070_4;
                $scope.rasxod_070_5 = $scope.DATAF._otheroutput.rasxod_070_5;
                $scope.rasxod_070_6 = $scope.DATAF._otheroutput.rasxod_070_6;
                $scope.rasxod_070_7 = $scope.DATAF._otheroutput.rasxod_070_7;
                $scope.rasxod_070_8 = $scope.DATAF._otheroutput.rasxod_070_8;
                $scope.rasxod_070_9 = $scope.DATAF._otheroutput.rasxod_070_9;
                $scope.rasxod_070_10 = $scope.DATAF._otheroutput.rasxod_070_10;
                $scope.rasxod_070_11 = $scope.DATAF._otheroutput.rasxod_070_11;
                $scope.rasxod_070_12 = $scope.DATAF._otheroutput.rasxod_070_12;
                $scope.rasxod_080_3 = $scope.DATAF._otheroutput.rasxod_080_3;
                $scope.rasxod_080_4 = $scope.DATAF._otheroutput.rasxod_080_4;
                $scope.rasxod_080_5 = $scope.DATAF._otheroutput.rasxod_080_5;
                $scope.rasxod_080_6 = $scope.DATAF._otheroutput.rasxod_080_6;
                $scope.rasxod_080_7 = $scope.DATAF._otheroutput.rasxod_080_7;
                $scope.rasxod_080_8 = $scope.DATAF._otheroutput.rasxod_080_8;
                $scope.rasxod_080_9 = $scope.DATAF._otheroutput.rasxod_080_9;
                $scope.rasxod_080_10 = $scope.DATAF._otheroutput.rasxod_080_10;
                $scope.rasxod_080_11 = $scope.DATAF._otheroutput.rasxod_080_11;
                $scope.rasxod_080_12 = $scope.DATAF._otheroutput.rasxod_080_12;
                $scope.rasxod_090_3 = $scope.DATAF._otheroutput.rasxod_090_3;
                $scope.rasxod_090_4 = $scope.DATAF._otheroutput.rasxod_090_4;
                $scope.rasxod_090_5 = $scope.DATAF._otheroutput.rasxod_090_5;
                $scope.rasxod_090_6 = $scope.DATAF._otheroutput.rasxod_090_6;
                $scope.rasxod_090_7 = $scope.DATAF._otheroutput.rasxod_090_7;
                $scope.rasxod_090_8 = $scope.DATAF._otheroutput.rasxod_090_8;
                $scope.rasxod_090_9 = $scope.DATAF._otheroutput.rasxod_090_9;
                $scope.rasxod_090_10 = $scope.DATAF._otheroutput.rasxod_090_10;
                $scope.rasxod_090_11 = $scope.DATAF._otheroutput.rasxod_090_11;
                $scope.rasxod_090_12 = $scope.DATAF._otheroutput.rasxod_090_12;
                $scope.rasshifrovka_010 = $scope.DATAF._otheroutput.rasshifrovka_010;
                $scope.rasshifrovka_020 = $scope.DATAF._otheroutput.rasshifrovka_020;
                $scope.rasshifrovka_030 = $scope.DATAF._otheroutput.rasshifrovka_030;
                $scope.rasshifrovka_040 = $scope.DATAF._otheroutput.rasshifrovka_040;
                $scope.rasshifrovka_050 = $scope.DATAF._otheroutput.rasshifrovka_050;
                $scope.drugiye_010 = $scope.DATAF._otheroutput.drugiye_010;
                $scope.drugiye_020 = $scope.DATAF._otheroutput.drugiye_020;
                $scope.drugiye_030 = $scope.DATAF._otheroutput.drugiye_030;
                $scope.drugiye_040 = $scope.DATAF._otheroutput.drugiye_040;
                $scope.drugiye_050 = $scope.DATAF._otheroutput.drugiye_050;
                $scope.drugiye_060 = $scope.DATAF._otheroutput.drugiye_060;
            }


            console.log(d.data);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetPostupleniya -> ", error);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }


    $scope.LoadDATAFS = function () {
        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";
        LoadDATAFS($scope.SelectedYear, $scope.SelectedKvartal);
    }

    function LoadDATAFS(bg, end) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: "/Buxgalter/FinansoviyOtchetSvodniy?year=" + bg.Id + "&kvartal=" + end.ID,
        }).then(function (d) {
            if (d.data.length != 0) {
                $scope.q = d.data.length;
                $scope.showdata = true;
            } else {
                $scope.showdata = false;
            }
            $scope.DATAF = d.data;

            if ($scope.DATAF._otheroutput !== null)
            {
                $scope.rasxod_010_3 = $scope.DATAF._otheroutput.rasxod_010_3;
                $scope.rasxod_010_4 = $scope.DATAF._otheroutput.rasxod_010_4;
                $scope.rasxod_010_5 = $scope.DATAF._otheroutput.rasxod_010_5;
                $scope.rasxod_010_6 = $scope.DATAF._otheroutput.rasxod_010_6;
                $scope.rasxod_010_7 = $scope.DATAF._otheroutput.rasxod_010_7;
                $scope.rasxod_010_8 = $scope.DATAF._otheroutput.rasxod_010_8;
                $scope.rasxod_010_9 = $scope.DATAF._otheroutput.rasxod_010_9;
                $scope.rasxod_010_10 = $scope.DATAF._otheroutput.rasxod_010_10;
                $scope.rasxod_010_11 = $scope.DATAF._otheroutput.rasxod_010_11;
                $scope.rasxod_010_12 = $scope.DATAF._otheroutput.rasxod_010_12;
                $scope.rasxod_020_3 = $scope.DATAF._otheroutput.rasxod_020_3;
                $scope.rasxod_020_4 = $scope.DATAF._otheroutput.rasxod_020_4;
                $scope.rasxod_020_5 = $scope.DATAF._otheroutput.rasxod_020_5;
                $scope.rasxod_020_6 = $scope.DATAF._otheroutput.rasxod_020_6;
                $scope.rasxod_020_7 = $scope.DATAF._otheroutput.rasxod_020_7;
                $scope.rasxod_020_8 = $scope.DATAF._otheroutput.rasxod_020_8;
                $scope.rasxod_020_9 = $scope.DATAF._otheroutput.rasxod_020_9;
                $scope.rasxod_020_10 = $scope.DATAF._otheroutput.rasxod_020_10;
                $scope.rasxod_020_11 = $scope.DATAF._otheroutput.rasxod_020_11;
                $scope.rasxod_020_12 = $scope.DATAF._otheroutput.rasxod_020_12;
                $scope.rasxod_030_3 = $scope.DATAF._otheroutput.rasxod_030_3;
                $scope.rasxod_030_4 = $scope.DATAF._otheroutput.rasxod_030_4;
                $scope.rasxod_030_5 = $scope.DATAF._otheroutput.rasxod_030_5;
                $scope.rasxod_030_6 = $scope.DATAF._otheroutput.rasxod_030_6;
                $scope.rasxod_030_7 = $scope.DATAF._otheroutput.rasxod_030_7;
                $scope.rasxod_030_8 = $scope.DATAF._otheroutput.rasxod_030_8;
                $scope.rasxod_030_9 = $scope.DATAF._otheroutput.rasxod_030_9;
                $scope.rasxod_030_10 = $scope.DATAF._otheroutput.rasxod_030_10;
                $scope.rasxod_030_11 = $scope.DATAF._otheroutput.rasxod_030_11;
                $scope.rasxod_030_12 = $scope.DATAF._otheroutput.rasxod_030_12;
                $scope.rasxod_040_3 = $scope.DATAF._otheroutput.rasxod_040_3;
                $scope.rasxod_040_4 = $scope.DATAF._otheroutput.rasxod_040_4;
                $scope.rasxod_040_5 = $scope.DATAF._otheroutput.rasxod_040_5;
                $scope.rasxod_040_6 = $scope.DATAF._otheroutput.rasxod_040_6;
                $scope.rasxod_040_7 = $scope.DATAF._otheroutput.rasxod_040_7;
                $scope.rasxod_040_8 = $scope.DATAF._otheroutput.rasxod_040_8;
                $scope.rasxod_040_9 = $scope.DATAF._otheroutput.rasxod_040_9;
                $scope.rasxod_040_10 = $scope.DATAF._otheroutput.rasxod_040_10;
                $scope.rasxod_040_11 = $scope.DATAF._otheroutput.rasxod_040_11;
                $scope.rasxod_040_12 = $scope.DATAF._otheroutput.rasxod_040_12;
                $scope.rasxod_060_3 = $scope.DATAF._otheroutput.rasxod_060_3;
                $scope.rasxod_060_4 = $scope.DATAF._otheroutput.rasxod_060_4;
                $scope.rasxod_060_5 = $scope.DATAF._otheroutput.rasxod_060_5;
                $scope.rasxod_060_6 = $scope.DATAF._otheroutput.rasxod_060_6;
                $scope.rasxod_060_7 = $scope.DATAF._otheroutput.rasxod_060_7;
                $scope.rasxod_060_8 = $scope.DATAF._otheroutput.rasxod_060_8;
                $scope.rasxod_060_9 = $scope.DATAF._otheroutput.rasxod_060_9;
                $scope.rasxod_060_10 = $scope.DATAF._otheroutput.rasxod_060_10;
                $scope.rasxod_060_11 = $scope.DATAF._otheroutput.rasxod_060_11;
                $scope.rasxod_060_12 = $scope.DATAF._otheroutput.rasxod_060_12;
                $scope.rasxod_070_3 = $scope.DATAF._otheroutput.rasxod_070_3;
                $scope.rasxod_070_4 = $scope.DATAF._otheroutput.rasxod_070_4;
                $scope.rasxod_070_5 = $scope.DATAF._otheroutput.rasxod_070_5;
                $scope.rasxod_070_6 = $scope.DATAF._otheroutput.rasxod_070_6;
                $scope.rasxod_070_7 = $scope.DATAF._otheroutput.rasxod_070_7;
                $scope.rasxod_070_8 = $scope.DATAF._otheroutput.rasxod_070_8;
                $scope.rasxod_070_9 = $scope.DATAF._otheroutput.rasxod_070_9;
                $scope.rasxod_070_10 = $scope.DATAF._otheroutput.rasxod_070_10;
                $scope.rasxod_070_11 = $scope.DATAF._otheroutput.rasxod_070_11;
                $scope.rasxod_070_12 = $scope.DATAF._otheroutput.rasxod_070_12;
                $scope.rasxod_080_3 = $scope.DATAF._otheroutput.rasxod_080_3;
                $scope.rasxod_080_4 = $scope.DATAF._otheroutput.rasxod_080_4;
                $scope.rasxod_080_5 = $scope.DATAF._otheroutput.rasxod_080_5;
                $scope.rasxod_080_6 = $scope.DATAF._otheroutput.rasxod_080_6;
                $scope.rasxod_080_7 = $scope.DATAF._otheroutput.rasxod_080_7;
                $scope.rasxod_080_8 = $scope.DATAF._otheroutput.rasxod_080_8;
                $scope.rasxod_080_9 = $scope.DATAF._otheroutput.rasxod_080_9;
                $scope.rasxod_080_10 = $scope.DATAF._otheroutput.rasxod_080_10;
                $scope.rasxod_080_11 = $scope.DATAF._otheroutput.rasxod_080_11;
                $scope.rasxod_080_12 = $scope.DATAF._otheroutput.rasxod_080_12;
                $scope.rasxod_090_3 = $scope.DATAF._otheroutput.rasxod_090_3;
                $scope.rasxod_090_4 = $scope.DATAF._otheroutput.rasxod_090_4;
                $scope.rasxod_090_5 = $scope.DATAF._otheroutput.rasxod_090_5;
                $scope.rasxod_090_6 = $scope.DATAF._otheroutput.rasxod_090_6;
                $scope.rasxod_090_7 = $scope.DATAF._otheroutput.rasxod_090_7;
                $scope.rasxod_090_8 = $scope.DATAF._otheroutput.rasxod_090_8;
                $scope.rasxod_090_9 = $scope.DATAF._otheroutput.rasxod_090_9;
                $scope.rasxod_090_10 = $scope.DATAF._otheroutput.rasxod_090_10;
                $scope.rasxod_090_11 = $scope.DATAF._otheroutput.rasxod_090_11;
                $scope.rasxod_090_12 = $scope.DATAF._otheroutput.rasxod_090_12;
                $scope.rasshifrovka_010 = $scope.DATAF._otheroutput.rasshifrovka_010;
                $scope.rasshifrovka_020 = $scope.DATAF._otheroutput.rasshifrovka_020;
                $scope.rasshifrovka_030 = $scope.DATAF._otheroutput.rasshifrovka_030;
                $scope.rasshifrovka_040 = $scope.DATAF._otheroutput.rasshifrovka_040;
                $scope.rasshifrovka_050 = $scope.DATAF._otheroutput.rasshifrovka_050;
                $scope.drugiye_010 = $scope.DATAF._otheroutput.drugiye_010;
                $scope.drugiye_020 = $scope.DATAF._otheroutput.drugiye_020;
                $scope.drugiye_030 = $scope.DATAF._otheroutput.drugiye_030;
                $scope.drugiye_040 = $scope.DATAF._otheroutput.drugiye_040;
                $scope.drugiye_050 = $scope.DATAF._otheroutput.drugiye_050;
                $scope.drugiye_060 = $scope.DATAF._otheroutput.drugiye_060;
            }
 
            console.log(d.data);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetPostupleniya -> ", error);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }


    $scope.LoadDATAFR = function () {
        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";
        LoadDATAFR($scope.SelectedYear, $scope.SelectedKvartal, $scope.ModalToDepCodeSelected.Kod);
    }

    function LoadDATAFR(bg, end, ProfCode) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static',
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: "/Buxgalter/FinansoviyOtchetVRazreze?year=" + bg.Id + "&kvartal=" + end.ID + '&departmentBase=' + ProfCode,
        }).then(function (d) {
            if (d.data.length != 0) {
                $scope.q = d.data.length;
                $scope.showdata = true;
            } else {
                $scope.showdata = false;
            }
            $scope.DATAF = d.data;
            if ($scope.DATAF._otheroutput !== null) {
                $scope.rasxod_010_3 = $scope.DATAF._otheroutput.rasxod_010_3;
                $scope.rasxod_010_4 = $scope.DATAF._otheroutput.rasxod_010_4;
                $scope.rasxod_010_5 = $scope.DATAF._otheroutput.rasxod_010_5;
                $scope.rasxod_010_6 = $scope.DATAF._otheroutput.rasxod_010_6;
                $scope.rasxod_010_7 = $scope.DATAF._otheroutput.rasxod_010_7;
                $scope.rasxod_010_8 = $scope.DATAF._otheroutput.rasxod_010_8;
                $scope.rasxod_010_9 = $scope.DATAF._otheroutput.rasxod_010_9;
                $scope.rasxod_010_10 = $scope.DATAF._otheroutput.rasxod_010_10;
                $scope.rasxod_010_11 = $scope.DATAF._otheroutput.rasxod_010_11;
                $scope.rasxod_010_12 = $scope.DATAF._otheroutput.rasxod_010_12;
                $scope.rasxod_020_3 = $scope.DATAF._otheroutput.rasxod_020_3;
                $scope.rasxod_020_4 = $scope.DATAF._otheroutput.rasxod_020_4;
                $scope.rasxod_020_5 = $scope.DATAF._otheroutput.rasxod_020_5;
                $scope.rasxod_020_6 = $scope.DATAF._otheroutput.rasxod_020_6;
                $scope.rasxod_020_7 = $scope.DATAF._otheroutput.rasxod_020_7;
                $scope.rasxod_020_8 = $scope.DATAF._otheroutput.rasxod_020_8;
                $scope.rasxod_020_9 = $scope.DATAF._otheroutput.rasxod_020_9;
                $scope.rasxod_020_10 = $scope.DATAF._otheroutput.rasxod_020_10;
                $scope.rasxod_020_11 = $scope.DATAF._otheroutput.rasxod_020_11;
                $scope.rasxod_020_12 = $scope.DATAF._otheroutput.rasxod_020_12;
                $scope.rasxod_030_3 = $scope.DATAF._otheroutput.rasxod_030_3;
                $scope.rasxod_030_4 = $scope.DATAF._otheroutput.rasxod_030_4;
                $scope.rasxod_030_5 = $scope.DATAF._otheroutput.rasxod_030_5;
                $scope.rasxod_030_6 = $scope.DATAF._otheroutput.rasxod_030_6;
                $scope.rasxod_030_7 = $scope.DATAF._otheroutput.rasxod_030_7;
                $scope.rasxod_030_8 = $scope.DATAF._otheroutput.rasxod_030_8;
                $scope.rasxod_030_9 = $scope.DATAF._otheroutput.rasxod_030_9;
                $scope.rasxod_030_10 = $scope.DATAF._otheroutput.rasxod_030_10;
                $scope.rasxod_030_11 = $scope.DATAF._otheroutput.rasxod_030_11;
                $scope.rasxod_030_12 = $scope.DATAF._otheroutput.rasxod_030_12;
                $scope.rasxod_040_3 = $scope.DATAF._otheroutput.rasxod_040_3;
                $scope.rasxod_040_4 = $scope.DATAF._otheroutput.rasxod_040_4;
                $scope.rasxod_040_5 = $scope.DATAF._otheroutput.rasxod_040_5;
                $scope.rasxod_040_6 = $scope.DATAF._otheroutput.rasxod_040_6;
                $scope.rasxod_040_7 = $scope.DATAF._otheroutput.rasxod_040_7;
                $scope.rasxod_040_8 = $scope.DATAF._otheroutput.rasxod_040_8;
                $scope.rasxod_040_9 = $scope.DATAF._otheroutput.rasxod_040_9;
                $scope.rasxod_040_10 = $scope.DATAF._otheroutput.rasxod_040_10;
                $scope.rasxod_040_11 = $scope.DATAF._otheroutput.rasxod_040_11;
                $scope.rasxod_040_12 = $scope.DATAF._otheroutput.rasxod_040_12;
                $scope.rasxod_060_3 = $scope.DATAF._otheroutput.rasxod_060_3;
                $scope.rasxod_060_4 = $scope.DATAF._otheroutput.rasxod_060_4;
                $scope.rasxod_060_5 = $scope.DATAF._otheroutput.rasxod_060_5;
                $scope.rasxod_060_6 = $scope.DATAF._otheroutput.rasxod_060_6;
                $scope.rasxod_060_7 = $scope.DATAF._otheroutput.rasxod_060_7;
                $scope.rasxod_060_8 = $scope.DATAF._otheroutput.rasxod_060_8;
                $scope.rasxod_060_9 = $scope.DATAF._otheroutput.rasxod_060_9;
                $scope.rasxod_060_10 = $scope.DATAF._otheroutput.rasxod_060_10;
                $scope.rasxod_060_11 = $scope.DATAF._otheroutput.rasxod_060_11;
                $scope.rasxod_060_12 = $scope.DATAF._otheroutput.rasxod_060_12;
                $scope.rasxod_070_3 = $scope.DATAF._otheroutput.rasxod_070_3;
                $scope.rasxod_070_4 = $scope.DATAF._otheroutput.rasxod_070_4;
                $scope.rasxod_070_5 = $scope.DATAF._otheroutput.rasxod_070_5;
                $scope.rasxod_070_6 = $scope.DATAF._otheroutput.rasxod_070_6;
                $scope.rasxod_070_7 = $scope.DATAF._otheroutput.rasxod_070_7;
                $scope.rasxod_070_8 = $scope.DATAF._otheroutput.rasxod_070_8;
                $scope.rasxod_070_9 = $scope.DATAF._otheroutput.rasxod_070_9;
                $scope.rasxod_070_10 = $scope.DATAF._otheroutput.rasxod_070_10;
                $scope.rasxod_070_11 = $scope.DATAF._otheroutput.rasxod_070_11;
                $scope.rasxod_070_12 = $scope.DATAF._otheroutput.rasxod_070_12;
                $scope.rasxod_080_3 = $scope.DATAF._otheroutput.rasxod_080_3;
                $scope.rasxod_080_4 = $scope.DATAF._otheroutput.rasxod_080_4;
                $scope.rasxod_080_5 = $scope.DATAF._otheroutput.rasxod_080_5;
                $scope.rasxod_080_6 = $scope.DATAF._otheroutput.rasxod_080_6;
                $scope.rasxod_080_7 = $scope.DATAF._otheroutput.rasxod_080_7;
                $scope.rasxod_080_8 = $scope.DATAF._otheroutput.rasxod_080_8;
                $scope.rasxod_080_9 = $scope.DATAF._otheroutput.rasxod_080_9;
                $scope.rasxod_080_10 = $scope.DATAF._otheroutput.rasxod_080_10;
                $scope.rasxod_080_11 = $scope.DATAF._otheroutput.rasxod_080_11;
                $scope.rasxod_080_12 = $scope.DATAF._otheroutput.rasxod_080_12;
                $scope.rasxod_090_3 = $scope.DATAF._otheroutput.rasxod_090_3;
                $scope.rasxod_090_4 = $scope.DATAF._otheroutput.rasxod_090_4;
                $scope.rasxod_090_5 = $scope.DATAF._otheroutput.rasxod_090_5;
                $scope.rasxod_090_6 = $scope.DATAF._otheroutput.rasxod_090_6;
                $scope.rasxod_090_7 = $scope.DATAF._otheroutput.rasxod_090_7;
                $scope.rasxod_090_8 = $scope.DATAF._otheroutput.rasxod_090_8;
                $scope.rasxod_090_9 = $scope.DATAF._otheroutput.rasxod_090_9;
                $scope.rasxod_090_10 = $scope.DATAF._otheroutput.rasxod_090_10;
                $scope.rasxod_090_11 = $scope.DATAF._otheroutput.rasxod_090_11;
                $scope.rasxod_090_12 = $scope.DATAF._otheroutput.rasxod_090_12;
                $scope.rasshifrovka_010 = $scope.DATAF._otheroutput.rasshifrovka_010;
                $scope.rasshifrovka_020 = $scope.DATAF._otheroutput.rasshifrovka_020;
                $scope.rasshifrovka_030 = $scope.DATAF._otheroutput.rasshifrovka_030;
                $scope.rasshifrovka_040 = $scope.DATAF._otheroutput.rasshifrovka_040;
                $scope.rasshifrovka_050 = $scope.DATAF._otheroutput.rasshifrovka_050;
                $scope.drugiye_010 = $scope.DATAF._otheroutput.drugiye_010;
                $scope.drugiye_020 = $scope.DATAF._otheroutput.drugiye_020;
                $scope.drugiye_030 = $scope.DATAF._otheroutput.drugiye_030;
                $scope.drugiye_040 = $scope.DATAF._otheroutput.drugiye_040;
                $scope.drugiye_050 = $scope.DATAF._otheroutput.drugiye_050;
                $scope.drugiye_060 = $scope.DATAF._otheroutput.drugiye_060;
            }

            console.log(d.data);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetPostupleniya -> ", error);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.Clear = function () {
        $scope.ZB = new Date
        $scope.ZE = new Date();
        $scope.showdata = false;
    }
    $scope.exportData = function () {
        var blob = new Blob([document.getElementById('export').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Главная книга.xls");
    };

    $scope.exportData2 = function () {
        var blob = new Blob([document.getElementById('export').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Финансовый отчет.xls");
    };
         
    $scope.SaveRasxod = function () {
        SaveRasxod();
    }

    function SaveRasxod() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });

        var postData = {
            rasxod_arr: [
                { "part": "3", "code": "010", "value": $scope.rasxod_010_3 },
                { "part": "3", "code": "020", "value": $scope.rasxod_020_3 },
                { "part": "3", "code": "030", "value": $scope.rasxod_030_3 },
                { "part": "3", "code": "040", "value": $scope.rasxod_040_3 },
                { "part": "3", "code": "060", "value": $scope.rasxod_060_3 },
                { "part": "3", "code": "070", "value": $scope.rasxod_070_3 },
                { "part": "3", "code": "080", "value": $scope.rasxod_080_3 },
                { "part": "3", "code": "090", "value": $scope.rasxod_090_3 },

                { "part": "4", "code": "010", "value": $scope.rasxod_010_4 },
                { "part": "4", "code": "020", "value": $scope.rasxod_020_4 },
                { "part": "4", "code": "030", "value": $scope.rasxod_030_4 },
                { "part": "4", "code": "040", "value": $scope.rasxod_040_4 },
                { "part": "4", "code": "060", "value": $scope.rasxod_060_4 },
                { "part": "4", "code": "070", "value": $scope.rasxod_070_4 },
                { "part": "4", "code": "080", "value": $scope.rasxod_080_4 },
                { "part": "4", "code": "090", "value": $scope.rasxod_090_4 },

                { "part": "5", "code": "010", "value": $scope.rasxod_010_5 },
                { "part": "5", "code": "020", "value": $scope.rasxod_020_5 },
                { "part": "5", "code": "030", "value": $scope.rasxod_030_5 },
                { "part": "5", "code": "040", "value": $scope.rasxod_040_5 },
                { "part": "5", "code": "060", "value": $scope.rasxod_060_5 },
                { "part": "5", "code": "070", "value": $scope.rasxod_070_5 },
                { "part": "5", "code": "080", "value": $scope.rasxod_080_5 },
                { "part": "5", "code": "090", "value": $scope.rasxod_090_5 },


                { "part": "6", "code": "010", "value": $scope.rasxod_010_6 },
                { "part": "6", "code": "020", "value": $scope.rasxod_020_6 },
                { "part": "6", "code": "030", "value": $scope.rasxod_030_6 },
                { "part": "6", "code": "040", "value": $scope.rasxod_040_6 },
                { "part": "6", "code": "060", "value": $scope.rasxod_060_6 },
                { "part": "6", "code": "070", "value": $scope.rasxod_070_6 },
                { "part": "6", "code": "080", "value": $scope.rasxod_080_6 },
                { "part": "6", "code": "090", "value": $scope.rasxod_090_6 },

                { "part": "7", "code": "010", "value": $scope.rasxod_010_7 },
                { "part": "7", "code": "020", "value": $scope.rasxod_020_7 },
                { "part": "7", "code": "030", "value": $scope.rasxod_030_7 },
                { "part": "7", "code": "040", "value": $scope.rasxod_040_7 },
                { "part": "7", "code": "060", "value": $scope.rasxod_060_7 },
                { "part": "7", "code": "070", "value": $scope.rasxod_070_7 },
                { "part": "7", "code": "080", "value": $scope.rasxod_080_7 },
                { "part": "7", "code": "090", "value": $scope.rasxod_090_7 },

                { "part": "8", "code": "010", "value": $scope.rasxod_010_8 },
                { "part": "8", "code": "020", "value": $scope.rasxod_020_8 },
                { "part": "8", "code": "030", "value": $scope.rasxod_030_8 },
                { "part": "8", "code": "040", "value": $scope.rasxod_040_8 },
                { "part": "8", "code": "060", "value": $scope.rasxod_060_8 },
                { "part": "8", "code": "070", "value": $scope.rasxod_070_8 },
                { "part": "8", "code": "080", "value": $scope.rasxod_080_8 },
                { "part": "8", "code": "090", "value": $scope.rasxod_090_8 },


                { "part": "9", "code": "010", "value": $scope.rasxod_010_9 },
                { "part": "9", "code": "020", "value": $scope.rasxod_020_9 },
                { "part": "9", "code": "030", "value": $scope.rasxod_030_9 },
                { "part": "9", "code": "040", "value": $scope.rasxod_040_9 },
                { "part": "9", "code": "060", "value": $scope.rasxod_060_9 },
                { "part": "9", "code": "070", "value": $scope.rasxod_070_9 },
                { "part": "9", "code": "080", "value": $scope.rasxod_080_9 },
                { "part": "9", "code": "090", "value": $scope.rasxod_090_9 },

                { "part": "10", "code": "010", "value": $scope.rasxod_010_10 },
                { "part": "10", "code": "020", "value": $scope.rasxod_020_10 },
                { "part": "10", "code": "030", "value": $scope.rasxod_030_10 },
                { "part": "10", "code": "040", "value": $scope.rasxod_040_10 },
                { "part": "10", "code": "060", "value": $scope.rasxod_060_10 },
                { "part": "10", "code": "070", "value": $scope.rasxod_070_10 },
                { "part": "10", "code": "080", "value": $scope.rasxod_080_10 },
                { "part": "10", "code": "090", "value": $scope.rasxod_090_10 },

                { "part": "11", "code": "010", "value": $scope.rasxod_010_11 },
                { "part": "11", "code": "020", "value": $scope.rasxod_020_11 },
                { "part": "11", "code": "030", "value": $scope.rasxod_030_11 },
                { "part": "11", "code": "040", "value": $scope.rasxod_040_11 },
                { "part": "11", "code": "060", "value": $scope.rasxod_060_11 },
                { "part": "11", "code": "070", "value": $scope.rasxod_070_11 },
                { "part": "11", "code": "080", "value": $scope.rasxod_080_11 },
                { "part": "11", "code": "090", "value": $scope.rasxod_090_11 },

                { "part": "12", "code": "010", "value": $scope.rasxod_010_12 },
                { "part": "12", "code": "020", "value": $scope.rasxod_020_12 },
                { "part": "12", "code": "030", "value": $scope.rasxod_030_12 },
                { "part": "12", "code": "040", "value": $scope.rasxod_040_12 },
                { "part": "12", "code": "060", "value": $scope.rasxod_060_12 },
                { "part": "12", "code": "070", "value": $scope.rasxod_070_12 },
                { "part": "12", "code": "080", "value": $scope.rasxod_080_12 },
                { "part": "12", "code": "090", "value": $scope.rasxod_090_12 },

                { "part": "13", "code": "010", "value": $scope.rasshifrovka_010 },
                { "part": "13", "code": "020", "value": $scope.rasshifrovka_020 },
                { "part": "13", "code": "030", "value": $scope.rasshifrovka_030 },
                { "part": "13", "code": "040", "value": $scope.rasshifrovka_040 },
                { "part": "13", "code": "050", "value": $scope.rasshifrovka_060 },

                { "part": "14", "code": "010", "value": $scope.drugiye_010 },
                { "part": "14", "code": "020", "value": $scope.drugiye_020 },
                { "part": "14", "code": "030", "value": $scope.drugiye_030 },
                { "part": "14", "code": "040", "value": $scope.drugiye_040 },
                { "part": "14", "code": "050", "value": $scope.drugiye_050 },
                { "part": "14", "code": "060", "value": $scope.drugiye_060 }

            ],
            year: $scope.SelectedYear.Id, 
            kvartal: $scope.SelectedKvartal.ID
        };

        $http({
            method: "post",
            url: "/Buxgalter/SaveRasxod",
            data: JSON.stringify(postData),
            dataType: "json"
        }).then(function (response) {

            console.log(response.data);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');

        }, function () {
            alertify.error("ошибка");
            console.log('Error in SaveRasxod');

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });


    }

    ///////////////////////////////////////////////////////////////////////////////////

    $scope.LoadDATALager = function () {
        $locale.NUMBER_FORMATS.GROUP_SEP = " ";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ",";
        LoadDATALager($scope.SelectedYear, $scope.SelectedKvartal);
    }

    function LoadDATALager(bg, end) {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });
        $http({
            method: 'get',
            url: "/Buxgalter/GetLagerFinOtchet?year=" + bg.Id + "&kvartal=" + end.ID,
        }).then(function (d) {
            if (d.data.length != 0) {
                $scope.q = d.data.length;
                $scope.showdata = true;
            } else {
                $scope.showdata = false;
            }

            $scope.T1R010C01 = d.data.T1R010C01;
            $scope.T1R010C02 = d.data.T1R010C02;
            $scope.T1R010C03 = d.data.T1R010C03;
            $scope.T1R010C04 = d.data.T1R010C04;
            $scope.T1R010C05 = d.data.T1R010C05;
            $scope.T1R020C01 = d.data.T1R020C01;
            $scope.T1R020C02 = d.data.T1R020C02;
            $scope.T1R020C03 = d.data.T1R020C03;
            $scope.T1R020C04 = d.data.T1R020C04;
            $scope.T1R020C05 = d.data.T1R020C05;
            $scope.T1R030C01 = d.data.T1R030C01;
            $scope.T1R030C02 = d.data.T1R030C02;
            $scope.T1R030C03 = d.data.T1R030C03;
            $scope.T1R030C04 = d.data.T1R030C04;
            $scope.T1R030C05 = d.data.T1R030C05;
            $scope.T1R040C01 = d.data.T1R040C01;
            $scope.T1R040C02 = d.data.T1R040C02;
            $scope.T1R040C03 = d.data.T1R040C03;
            $scope.T1R040C04 = d.data.T1R040C04;
            $scope.T1R040C05 = d.data.T1R040C05;
            $scope.T1R050C01 = d.data.T1R050C01;
            $scope.T1R050C02 = d.data.T1R050C02;
            $scope.T1R050C03 = d.data.T1R050C03;
            $scope.T1R050C04 = d.data.T1R050C04;
            $scope.T1R050C05 = d.data.T1R050C05;
            $scope.T1R060C01 = d.data.T1R060C01;
            $scope.T1R060C02 = d.data.T1R060C02;
            $scope.T1R060C03 = d.data.T1R060C03;
            $scope.T1R060C04 = d.data.T1R060C04;
            $scope.T1R060C05 = d.data.T1R060C05;
            $scope.T1R070C01 = d.data.T1R070C01;
            $scope.T1R070C02 = d.data.T1R070C02;
            $scope.T1R070C03 = d.data.T1R070C03;
            $scope.T1R070C04 = d.data.T1R070C04;
            $scope.T1R070C05 = d.data.T1R070C05;
            $scope.T1R080C01 = d.data.T1R080C01;
            $scope.T1R080C02 = d.data.T1R080C02;
            $scope.T1R080C03 = d.data.T1R080C03;
            $scope.T1R080C04 = d.data.T1R080C04;
            $scope.T1R080C05 = d.data.T1R080C05;
            $scope.T1R090C01 = d.data.T1R090C01;
            $scope.T1R090C02 = d.data.T1R090C02;
            $scope.T1R090C03 = d.data.T1R090C03;
            $scope.T1R090C04 = d.data.T1R090C04;
            $scope.T1R090C05 = d.data.T1R090C05;
            $scope.T1R100C01 = d.data.T1R100C01;
            $scope.T1R100C02 = d.data.T1R100C02;
            $scope.T1R100C03 = d.data.T1R100C03;
            $scope.T1R100C04 = d.data.T1R100C04;
            $scope.T1R100C05 = d.data.T1R100C05;
            $scope.T1R110C01 = d.data.T1R110C01;
            $scope.T1R110C02 = d.data.T1R110C02;
            $scope.T1R110C03 = d.data.T1R110C03;
            $scope.T1R110C04 = d.data.T1R110C04;
            $scope.T1R110C05 = d.data.T1R110C05;
            $scope.T1R120C01 = d.data.T1R120C01;
            $scope.T1R120C02 = d.data.T1R120C02;
            $scope.T1R120C03 = d.data.T1R120C03;
            $scope.T1R120C04 = d.data.T1R120C04;
            $scope.T1R120C05 = d.data.T1R120C05;
            $scope.T1R130C01 = d.data.T1R130C01;
            $scope.T1R130C02 = d.data.T1R130C02;
            $scope.T1R130C03 = d.data.T1R130C03;
            $scope.T1R130C04 = d.data.T1R130C04;
            $scope.T1R130C05 = d.data.T1R130C05;
            $scope.T1R140C01 = d.data.T1R140C01;
            $scope.T1R140C02 = d.data.T1R140C02;
            $scope.T1R140C03 = d.data.T1R140C03;
            $scope.T1R140C04 = d.data.T1R140C04;
            $scope.T1R140C05 = d.data.T1R140C05;
            $scope.T1R150C01 = d.data.T1R150C01;
            $scope.T1R150C02 = d.data.T1R150C02;
            $scope.T1R150C03 = d.data.T1R150C03;
            $scope.T1R150C04 = d.data.T1R150C04;
            $scope.T1R150C05 = d.data.T1R150C05;
            $scope.T1R160C01 = d.data.T1R160C01;
            $scope.T1R160C05 = d.data.T1R160C05;
            $scope.T1R170C01 = d.data.T1R170C01;
            $scope.T1R170C05 = d.data.T1R170C05;
            $scope.T1R180C01 = d.data.T1R180C01;
            $scope.T1R180C02 = d.data.T1R180C02;
            $scope.T1R180C03 = d.data.T1R180C03;
            $scope.T1R180C04 = d.data.T1R180C04;
            $scope.T1R180C05 = d.data.T1R180C05;
            $scope.T1R190C01 = d.data.T1R190C01;
            $scope.T1R190C02 = d.data.T1R190C02;
            $scope.T1R190C03 = d.data.T1R190C03;
            $scope.T1R190C04 = d.data.T1R190C04;
            $scope.T1R190C05 = d.data.T1R190C05;
            $scope.T1R200C01 = d.data.T1R200C01;
            $scope.T1R200C02 = d.data.T1R200C02;
            $scope.T1R200C03 = d.data.T1R200C03;
            $scope.T1R200C04 = d.data.T1R200C04;
            $scope.T1R200C05 = d.data.T1R200C05;
            $scope.T1R210C01 = d.data.T1R210C01;
            $scope.T1R210C02 = d.data.T1R210C02;
            $scope.T1R210C03 = d.data.T1R210C03;
            $scope.T1R210C04 = d.data.T1R210C04;
            $scope.T1R210C05 = d.data.T1R210C05;
            $scope.T1R220C01 = d.data.T1R220C01;
            $scope.T1R220C02 = d.data.T1R220C02;
            $scope.T1R220C03 = d.data.T1R220C03;
            $scope.T1R220C04 = d.data.T1R220C04;
            $scope.T1R220C05 = d.data.T1R220C05;
            $scope.T1R230C01 = d.data.T1R230C01;
            $scope.T1R230C02 = d.data.T1R230C02;
            $scope.T1R230C03 = d.data.T1R230C03;
            $scope.T1R230C04 = d.data.T1R230C04;
            $scope.T1R230C05 = d.data.T1R230C05;
            $scope.T1R240C01 = d.data.T1R240C01;
            $scope.T1R240C05 = d.data.T1R240C05;
            $scope.T1R250C01 = d.data.T1R250C01;
            $scope.T1R250C05 = d.data.T1R250C05;
            $scope.T1R280C01 = d.data.T1R280C01;
            $scope.T1R280C02 = d.data.T1R280C02;
            $scope.T1R280C03 = d.data.T1R280C03;
            $scope.T1R280C04 = d.data.T1R280C04;
            $scope.T1R280C05 = d.data.T1R280C05;
            $scope.T2R010C01 = d.data.T2R010C01;
            $scope.T2R010C02 = d.data.T2R010C02;
            $scope.T2R020C01 = d.data.T2R020C01;
            $scope.T2R020C02 = d.data.T2R020C02;
            $scope.T2R030C01 = d.data.T2R030C01;
            $scope.T2R030C02 = d.data.T2R030C02;
            $scope.T2R040C01 = d.data.T2R040C01;
            $scope.T2R040C02 = d.data.T2R040C02;
            $scope.T2R050C01 = d.data.T2R050C01;
            $scope.T2R050C02 = d.data.T2R050C02;
            $scope.T2R060C01 = d.data.T2R060C01;
            $scope.T2R060C02 = d.data.T2R060C02;
            $scope.T2R070C01 = d.data.T2R070C01;
            $scope.T2R070C02 = d.data.T2R070C02;
            $scope.T2R080C01 = d.data.T2R080C01;
            $scope.T2R080C02 = d.data.T2R080C02;
            $scope.T2R090C01 = d.data.T2R090C01;
            $scope.T2R090C02 = d.data.T2R090C02;
            $scope.T2R110C01 = d.data.T2R110C01;
            $scope.T2R110C02 = d.data.T2R110C02;
            $scope.T2R120C01 = d.data.T2R120C01;
            $scope.T2R120C02 = d.data.T2R120C02;
            $scope.T2R130C01 = d.data.T2R130C01;
            $scope.T2R130C02 = d.data.T2R130C02;
            $scope.T2R140C01 = d.data.T2R140C01;
            $scope.T2R140C02 = d.data.T2R140C02;
            $scope.T2R150C01 = d.data.T2R150C01;
            $scope.T2R150C02 = d.data.T2R150C02;
            $scope.T2R170C01 = d.data.T2R170C01;
            $scope.T2R170C02 = d.data.T2R170C02;
            $scope.T2R180C01 = d.data.T2R180C01;
            $scope.T2R180C02 = d.data.T2R180C02;
            $scope.T2R190C01 = d.data.T2R190C01;
            $scope.T2R190C02 = d.data.T2R190C02;
            $scope.T2R200C01 = d.data.T2R200C01;
            $scope.T2R200C02 = d.data.T2R200C02;
            $scope.T2R160C01 = d.data.T2R160C01;
            $scope.T2R160C02 = d.data.T2R160C02;
            $scope.T2R100C01 = d.data.T2R100C01;
            $scope.T2R210C01 = d.data.T2R210C01;
            $scope.T2R100C02 = d.data.T2R100C02;
            $scope.T2R210C02 = d.data.T2R210C02;
            $scope.T2R230C01 = d.data.T2R230C01;
            $scope.T2R230C02 = d.data.T2R230C02;
            $scope.T2R240C01 = d.data.T2R240C01;
            $scope.T2R240C02 = d.data.T2R240C02;
            $scope.T2R010C03 = d.data.T2R010C03;
            $scope.T2R010C04 = d.data.T2R010C04;
            $scope.T2R010C05 = d.data.T2R010C05;
            $scope.T2R010C06 = d.data.T2R010C06;
            $scope.T2R010C07 = d.data.T2R010C07;
            $scope.T2R010C08 = d.data.T2R010C08;
            $scope.T2R010C09 = d.data.T2R010C09;
            $scope.T2R010C10 = d.data.T2R010C10;
            $scope.T2R010C11 = d.data.T2R010C11;
            $scope.T2R010C12 = d.data.T2R010C12;
            $scope.T2R020C03 = d.data.T2R020C03;
            $scope.T2R020C04 = d.data.T2R020C04;
            $scope.T2R020C05 = d.data.T2R020C05;
            $scope.T2R020C06 = d.data.T2R020C06;
            $scope.T2R020C07 = d.data.T2R020C07;
            $scope.T2R020C08 = d.data.T2R020C08;
            $scope.T2R020C09 = d.data.T2R020C09;
            $scope.T2R020C10 = d.data.T2R020C10;
            $scope.T2R020C11 = d.data.T2R020C11;
            $scope.T2R020C12 = d.data.T2R020C12;
            $scope.T2R030C03 = d.data.T2R030C03;
            $scope.T2R030C04 = d.data.T2R030C04;
            $scope.T2R030C05 = d.data.T2R030C05;
            $scope.T2R030C06 = d.data.T2R030C06;
            $scope.T2R030C07 = d.data.T2R030C07;
            $scope.T2R030C08 = d.data.T2R030C08;
            $scope.T2R030C09 = d.data.T2R030C09;
            $scope.T2R030C10 = d.data.T2R030C10;
            $scope.T2R030C11 = d.data.T2R030C11;
            $scope.T2R030C12 = d.data.T2R030C12;
            $scope.T3R010C01 = d.data.T3R010C01;
            $scope.T3R010C02 = d.data.T3R010C02;
            $scope.T3R010C03 = d.data.T3R010C03;
            $scope.T2R040C03 = d.data.T2R040C03;
            $scope.T2R040C04 = d.data.T2R040C04;
            $scope.T2R040C05 = d.data.T2R040C05;
            $scope.T2R040C06 = d.data.T2R040C06;
            $scope.T2R040C07 = d.data.T2R040C07;
            $scope.T2R040C08 = d.data.T2R040C08;
            $scope.T2R040C09 = d.data.T2R040C09;
            $scope.T2R040C10 = d.data.T2R040C10;
            $scope.T2R040C11 = d.data.T2R040C11;
            $scope.T2R040C12 = d.data.T2R040C12;
            $scope.T3R020C01 = d.data.T3R020C01;
            $scope.T3R020C02 = d.data.T3R020C02;
            $scope.T3R020C03 = d.data.T3R020C03;
            $scope.T2R050C03 = d.data.T2R050C03;
            $scope.T2R050C04 = d.data.T2R050C04;
            $scope.T2R050C05 = d.data.T2R050C05;
            $scope.T2R050C06 = d.data.T2R050C06;
            $scope.T2R050C07 = d.data.T2R050C07;
            $scope.T2R050C08 = d.data.T2R050C08;
            $scope.T2R050C09 = d.data.T2R050C09;
            $scope.T2R050C10 = d.data.T2R050C10;
            $scope.T2R050C11 = d.data.T2R050C11;
            $scope.T2R050C12 = d.data.T2R050C12;
            $scope.T3R030C01 = d.data.T3R030C01;
            $scope.T3R030C02 = d.data.T3R030C02;
            $scope.T3R030C03 = d.data.T3R030C03;
            $scope.T2R060C03 = d.data.T2R060C03;
            $scope.T2R060C04 = d.data.T2R060C04;
            $scope.T2R060C05 = d.data.T2R060C05;
            $scope.T2R060C06 = d.data.T2R060C06;
            $scope.T2R060C07 = d.data.T2R060C07;
            $scope.T2R060C08 = d.data.T2R060C08;
            $scope.T2R060C09 = d.data.T2R060C09;
            $scope.T2R060C10 = d.data.T2R060C10;
            $scope.T2R060C11 = d.data.T2R060C11;
            $scope.T2R060C12 = d.data.T2R060C12;
            $scope.T3R040C01 = d.data.T3R040C01;
            $scope.T3R040C02 = d.data.T3R040C02;
            $scope.T3R040C03 = d.data.T3R040C03;
            $scope.T2R070C03 = d.data.T2R070C03;
            $scope.T2R070C04 = d.data.T2R070C04;
            $scope.T2R070C05 = d.data.T2R070C05;
            $scope.T2R070C06 = d.data.T2R070C06;
            $scope.T2R070C07 = d.data.T2R070C07;
            $scope.T2R070C08 = d.data.T2R070C08;
            $scope.T2R070C09 = d.data.T2R070C09;
            $scope.T2R070C10 = d.data.T2R070C10;
            $scope.T2R070C11 = d.data.T2R070C11;
            $scope.T2R070C12 = d.data.T2R070C12;
            $scope.T3R050C01 = d.data.T3R050C01;
            $scope.T3R050C02 = d.data.T3R050C02;
            $scope.T3R050C03 = d.data.T3R050C03;
            $scope.T2R080C03 = d.data.T2R080C03;
            $scope.T2R080C04 = d.data.T2R080C04;
            $scope.T2R080C05 = d.data.T2R080C05;
            $scope.T2R080C06 = d.data.T2R080C06;
            $scope.T2R080C07 = d.data.T2R080C07;
            $scope.T2R080C08 = d.data.T2R080C08;
            $scope.T2R080C09 = d.data.T2R080C09;
            $scope.T2R080C10 = d.data.T2R080C10;
            $scope.T2R080C11 = d.data.T2R080C11;
            $scope.T2R080C12 = d.data.T2R080C12;
            $scope.T3R060C01 = d.data.T3R060C01;
            $scope.T3R060C02 = d.data.T3R060C02;
            $scope.T3R060C03 = d.data.T3R060C03;
            $scope.T2R090C03 = d.data.T2R090C03;
            $scope.T2R090C04 = d.data.T2R090C04;
            $scope.T2R090C05 = d.data.T2R090C05;
            $scope.T2R090C06 = d.data.T2R090C06;
            $scope.T2R090C07 = d.data.T2R090C07;
            $scope.T2R090C08 = d.data.T2R090C08;
            $scope.T2R090C09 = d.data.T2R090C09;
            $scope.T2R090C10 = d.data.T2R090C10;
            $scope.T2R090C11 = d.data.T2R090C11;
            $scope.T2R090C12 = d.data.T2R090C12;
            $scope.T3R070C01 = d.data.T3R070C01;
            $scope.T3R070C02 = d.data.T3R070C02;
            $scope.T3R070C03 = d.data.T3R070C03;
            $scope.T2R100C03 = d.data.T2R100C03;
            $scope.T2R100C04 = d.data.T2R100C04;
            $scope.T2R100C05 = d.data.T2R100C05;
            $scope.T2R100C06 = d.data.T2R100C06;
            $scope.T2R100C07 = d.data.T2R100C07;
            $scope.T2R100C08 = d.data.T2R100C08;
            $scope.T2R100C09 = d.data.T2R100C09;
            $scope.T2R100C10 = d.data.T2R100C10;
            $scope.T2R100C11 = d.data.T2R100C11;
            $scope.T2R100C12 = d.data.T2R100C12;
            $scope.T3R080C01 = d.data.T3R080C01;
            $scope.T3R080C02 = d.data.T3R080C02;
            $scope.T3R080C03 = d.data.T3R080C03;
            $scope.T2R110C03 = d.data.T2R110C03;
            $scope.T2R110C04 = d.data.T2R110C04;
            $scope.T2R110C05 = d.data.T2R110C05;
            $scope.T2R110C06 = d.data.T2R110C06;
            $scope.T2R110C07 = d.data.T2R110C07;
            $scope.T2R110C08 = d.data.T2R110C08;
            $scope.T2R110C09 = d.data.T2R110C09;
            $scope.T2R110C10 = d.data.T2R110C10;
            $scope.T2R110C11 = d.data.T2R110C11;
            $scope.T2R110C12 = d.data.T2R110C12;
            $scope.T3R090C01 = d.data.T3R090C01;
            $scope.T3R090C02 = d.data.T3R090C02;
            $scope.T3R090C03 = d.data.T3R090C03;
            $scope.T2R120C03 = d.data.T2R120C03;
            $scope.T2R120C04 = d.data.T2R120C04;
            $scope.T2R120C05 = d.data.T2R120C05;
            $scope.T2R120C06 = d.data.T2R120C06;
            $scope.T2R120C07 = d.data.T2R120C07;
            $scope.T2R120C08 = d.data.T2R120C08;
            $scope.T2R120C09 = d.data.T2R120C09;
            $scope.T2R120C10 = d.data.T2R120C10;
            $scope.T2R120C11 = d.data.T2R120C11;
            $scope.T2R120C12 = d.data.T2R120C12;
            $scope.T3R100C01 = d.data.T3R100C01;
            $scope.T3R100C02 = d.data.T3R100C02;
            $scope.T3R100C03 = d.data.T3R100C03;
            $scope.T2R130C03 = d.data.T2R130C03;
            $scope.T2R130C04 = d.data.T2R130C04;
            $scope.T2R130C05 = d.data.T2R130C05;
            $scope.T2R130C06 = d.data.T2R130C06;
            $scope.T2R130C07 = d.data.T2R130C07;
            $scope.T2R130C08 = d.data.T2R130C08;
            $scope.T2R130C09 = d.data.T2R130C09;
            $scope.T2R130C10 = d.data.T2R130C10;
            $scope.T2R130C11 = d.data.T2R130C11;
            $scope.T2R130C12 = d.data.T2R130C12;
            $scope.T3R110C01 = d.data.T3R110C01;
            $scope.T3R110C02 = d.data.T3R110C02;
            $scope.T3R110C03 = d.data.T3R110C03;
            $scope.T2R140C03 = d.data.T2R140C03;
            $scope.T2R140C04 = d.data.T2R140C04;
            $scope.T2R140C05 = d.data.T2R140C05;
            $scope.T2R140C06 = d.data.T2R140C06;
            $scope.T2R140C07 = d.data.T2R140C07;
            $scope.T2R140C08 = d.data.T2R140C08;
            $scope.T2R140C09 = d.data.T2R140C09;
            $scope.T2R140C10 = d.data.T2R140C10;
            $scope.T2R140C11 = d.data.T2R140C11;
            $scope.T2R140C12 = d.data.T2R140C12;
            $scope.T2R150C03 = d.data.T2R150C03;
            $scope.T2R150C04 = d.data.T2R150C04;
            $scope.T2R150C05 = d.data.T2R150C05;
            $scope.T2R150C06 = d.data.T2R150C06;
            $scope.T2R150C07 = d.data.T2R150C07;
            $scope.T2R150C08 = d.data.T2R150C08;
            $scope.T2R150C09 = d.data.T2R150C09;
            $scope.T2R150C10 = d.data.T2R150C10;
            $scope.T2R150C11 = d.data.T2R150C11;
            $scope.T2R150C12 = d.data.T2R150C12;
            $scope.T2R160C03 = d.data.T2R160C03;
            $scope.T2R160C04 = d.data.T2R160C04;
            $scope.T2R160C05 = d.data.T2R160C05;
            $scope.T2R160C06 = d.data.T2R160C06;
            $scope.T2R160C07 = d.data.T2R160C07;
            $scope.T2R160C08 = d.data.T2R160C08;
            $scope.T2R160C09 = d.data.T2R160C09;
            $scope.T2R160C10 = d.data.T2R160C10;
            $scope.T2R160C11 = d.data.T2R160C11;
            $scope.T2R160C12 = d.data.T2R160C12;
            $scope.T2R170C03 = d.data.T2R170C03;
            $scope.T2R170C04 = d.data.T2R170C04;
            $scope.T2R170C05 = d.data.T2R170C05;
            $scope.T2R170C06 = d.data.T2R170C06;
            $scope.T2R170C07 = d.data.T2R170C07;
            $scope.T2R170C08 = d.data.T2R170C08;
            $scope.T2R170C09 = d.data.T2R170C09;
            $scope.T2R170C10 = d.data.T2R170C10;
            $scope.T2R170C11 = d.data.T2R170C11;
            $scope.T2R170C12 = d.data.T2R170C12;
            $scope.T2R180C03 = d.data.T2R180C03;
            $scope.T2R180C04 = d.data.T2R180C04;
            $scope.T2R180C05 = d.data.T2R180C05;
            $scope.T2R180C06 = d.data.T2R180C06;
            $scope.T2R180C07 = d.data.T2R180C07;
            $scope.T2R180C08 = d.data.T2R180C08;
            $scope.T2R180C09 = d.data.T2R180C09;
            $scope.T2R180C10 = d.data.T2R180C10;
            $scope.T2R180C11 = d.data.T2R180C11;
            $scope.T2R180C12 = d.data.T2R180C12;
            $scope.T4R010C01 = d.data.T4R010C01;
            $scope.T4R010C02 = d.data.T4R010C02;
            $scope.T2R190C03 = d.data.T2R190C03;
            $scope.T2R190C04 = d.data.T2R190C04;
            $scope.T2R190C05 = d.data.T2R190C05;
            $scope.T2R190C06 = d.data.T2R190C06;
            $scope.T2R190C07 = d.data.T2R190C07;
            $scope.T2R190C08 = d.data.T2R190C08;
            $scope.T2R190C09 = d.data.T2R190C09;
            $scope.T2R190C10 = d.data.T2R190C10;
            $scope.T2R190C11 = d.data.T2R190C11;
            $scope.T2R190C12 = d.data.T2R190C12;
            $scope.T4R020C01 = d.data.T4R020C01;
            $scope.T4R020C02 = d.data.T4R020C02;
            $scope.T2R200C03 = d.data.T2R200C03;
            $scope.T2R200C04 = d.data.T2R200C04;
            $scope.T2R200C05 = d.data.T2R200C05;
            $scope.T2R200C06 = d.data.T2R200C06;
            $scope.T2R200C07 = d.data.T2R200C07;
            $scope.T2R200C08 = d.data.T2R200C08;
            $scope.T2R200C09 = d.data.T2R200C09;
            $scope.T2R200C10 = d.data.T2R200C10;
            $scope.T2R200C11 = d.data.T2R200C11;
            $scope.T2R200C12 = d.data.T2R200C12;
            $scope.T4R030C01 = d.data.T4R030C01;
            $scope.T4R030C02 = d.data.T4R030C02;
            $scope.T2R210C03 = d.data.T2R210C03;
            $scope.T2R210C04 = d.data.T2R210C04;
            $scope.T2R210C05 = d.data.T2R210C05;
            $scope.T2R210C06 = d.data.T2R210C06;
            $scope.T2R210C07 = d.data.T2R210C07;
            $scope.T2R210C08 = d.data.T2R210C08;
            $scope.T2R210C09 = d.data.T2R210C09;
            $scope.T2R210C10 = d.data.T2R210C10;
            $scope.T2R210C11 = d.data.T2R210C11;
            $scope.T2R210C12 = d.data.T2R210C12;
            $scope.T2R220C03 = d.data.T2R220C03;
            $scope.T2R220C04 = d.data.T2R220C04;
            $scope.T2R220C05 = d.data.T2R220C05;
            $scope.T2R220C06 = d.data.T2R220C06;
            $scope.T2R220C07 = d.data.T2R220C07;
            $scope.T2R220C08 = d.data.T2R220C08;
            $scope.T2R220C09 = d.data.T2R220C09;
            $scope.T2R220C10 = d.data.T2R220C10;
            $scope.T2R220C11 = d.data.T2R220C11;
            $scope.T2R220C12 = d.data.T2R220C12;
            $scope.T4R050C01 = d.data.T4R050C01;
            $scope.T4R050C02 = d.data.T4R050C02;
            $scope.T2R230C03 = d.data.T2R230C03;
            $scope.T2R230C04 = d.data.T2R230C04;
            $scope.T2R230C05 = d.data.T2R230C05;
            $scope.T2R230C06 = d.data.T2R230C06;
            $scope.T2R230C07 = d.data.T2R230C07;
            $scope.T2R230C08 = d.data.T2R230C08;
            $scope.T2R230C09 = d.data.T2R230C09;
            $scope.T2R230C10 = d.data.T2R230C10;
            $scope.T2R230C11 = d.data.T2R230C11;
            $scope.T2R230C12 = d.data.T2R230C12;
            $scope.T2R240C03 = d.data.T2R240C03;
            $scope.T2R240C04 = d.data.T2R240C04;
            $scope.T2R240C05 = d.data.T2R240C05;
            $scope.T2R240C06 = d.data.T2R240C06;
            $scope.T2R240C07 = d.data.T2R240C07;
            $scope.T2R240C08 = d.data.T2R240C08;
            $scope.T2R240C09 = d.data.T2R240C09;
            $scope.T2R240C10 = d.data.T2R240C10;
            $scope.T2R240C11 = d.data.T2R240C11;
            $scope.T2R240C12 = d.data.T2R240C12;
            $scope.T5R010C01 = d.data.T5R010C01;
            $scope.T5R020C01 = d.data.T5R020C01;
            $scope.T5R030C01 = d.data.T5R030C01;
            $scope.T5R040C01 = d.data.T5R040C01;
            $scope.T5R050C01 = d.data.T5R050C01;
            $scope.T5R060C01 = d.data.T5R060C01;


            console.log(d.data);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        }, function (error) {
            alertify.error("ошибка");
            console.log("error in GetPostupleniya -> ", error);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });
    }

    $scope.SaveLager = function () {
        SaveLager();
    }

    function SaveLager() {
        document.getElementById('connection').style.visibility = "visible";
        $("#hh").modal({
            backdrop: 'static'
        });

        var postData = {
            T1R010C01 : $scope.T1R010C01,
            T1R010C02 : $scope.T1R010C02,
            T1R010C03 : $scope.T1R010C03,
            T1R010C04 : $scope.T1R010C04,
            T1R010C05 : $scope.T1R010C05,
            T1R020C01 : $scope.T1R020C01,
            T1R020C02 : $scope.T1R020C02,
            T1R020C03 : $scope.T1R020C03,
            T1R020C04 : $scope.T1R020C04,
            T1R020C05 : $scope.T1R020C05,
            T1R030C01 : $scope.T1R030C01,
            T1R030C02 : $scope.T1R030C02,
            T1R030C03 : $scope.T1R030C03,
            T1R030C04 : $scope.T1R030C04,
            T1R030C05 : $scope.T1R030C05,
            T1R040C01 : $scope.T1R040C01,
            T1R040C02 : $scope.T1R040C02,
            T1R040C03 : $scope.T1R040C03,
            T1R040C04 : $scope.T1R040C04,
            T1R040C05 : $scope.T1R040C05,
            T1R050C01 : $scope.T1R050C01,
            T1R050C02 : $scope.T1R050C02,
            T1R050C03 : $scope.T1R050C03,
            T1R050C04 : $scope.T1R050C04,
            T1R050C05 : $scope.T1R050C05,
            T1R060C01 : $scope.T1R060C01,
            T1R060C02 : $scope.T1R060C02,
            T1R060C03 : $scope.T1R060C03,
            T1R060C04 : $scope.T1R060C04,
            T1R060C05 : $scope.T1R060C05,
            T1R070C01 : $scope.T1R070C01,
            T1R070C02 : $scope.T1R070C02,
            T1R070C03 : $scope.T1R070C03,
            T1R070C04 : $scope.T1R070C04,
            T1R070C05 : $scope.T1R070C05,
            T1R080C01 : $scope.T1R080C01,
            T1R080C02 : $scope.T1R080C02,
            T1R080C03 : $scope.T1R080C03,
            T1R080C04 : $scope.T1R080C04,
            T1R080C05 : $scope.T1R080C05,
            T1R090C01 : $scope.T1R090C01,
            T1R090C02 : $scope.T1R090C02,
            T1R090C03 : $scope.T1R090C03,
            T1R090C04 : $scope.T1R090C04,
            T1R090C05 : $scope.T1R090C05,
            T1R100C01 : $scope.T1R100C01,
            T1R100C02 : $scope.T1R100C02,
            T1R100C03 : $scope.T1R100C03,
            T1R100C04 : $scope.T1R100C04,
            T1R100C05 : $scope.T1R100C05,
            T1R110C01 : $scope.T1R110C01,
            T1R110C02 : $scope.T1R110C02,
            T1R110C03 : $scope.T1R110C03,
            T1R110C04 : $scope.T1R110C04,
            T1R110C05 : $scope.T1R110C05,
            T1R120C01 : $scope.T1R120C01,
            T1R120C02 : $scope.T1R120C02,
            T1R120C03 : $scope.T1R120C03,
            T1R120C04 : $scope.T1R120C04,
            T1R120C05 : $scope.T1R120C05,
            T1R130C01 : $scope.T1R130C01,
            T1R130C02 : $scope.T1R130C02,
            T1R130C03 : $scope.T1R130C03,
            T1R130C04 : $scope.T1R130C04,
            T1R130C05 : $scope.T1R130C05,
            T1R140C01 : $scope.T1R140C01,
            T1R140C02 : $scope.T1R140C02,
            T1R140C03 : $scope.T1R140C03,
            T1R140C04 : $scope.T1R140C04,
            T1R140C05 : $scope.T1R140C05,
            T1R150C01 : $scope.T1R150C01,
            T1R150C02 : $scope.T1R150C02,
            T1R150C03 : $scope.T1R150C03,
            T1R150C04 : $scope.T1R150C04,
            T1R150C05 : $scope.T1R150C05,
            T1R160C01 : $scope.T1R160C01,
            T1R160C05 : $scope.T1R160C05,
            T1R170C01 : $scope.T1R170C01,
            T1R170C05 : $scope.T1R170C05,
            T1R180C01 : $scope.T1R180C01,
            T1R180C02 : $scope.T1R180C02,
            T1R180C03 : $scope.T1R180C03,
            T1R180C04 : $scope.T1R180C04,
            T1R180C05 : $scope.T1R180C05,
            T1R190C01 : $scope.T1R190C01,
            T1R190C02 : $scope.T1R190C02,
            T1R190C03 : $scope.T1R190C03,
            T1R190C04 : $scope.T1R190C04,
            T1R190C05 : $scope.T1R190C05,
            T1R200C01 : $scope.T1R200C01,
            T1R200C02 : $scope.T1R200C02,
            T1R200C03 : $scope.T1R200C03,
            T1R200C04 : $scope.T1R200C04,
            T1R200C05 : $scope.T1R200C05,
            T1R210C01 : $scope.T1R210C01,
            T1R210C02 : $scope.T1R210C02,
            T1R210C03 : $scope.T1R210C03,
            T1R210C04 : $scope.T1R210C04,
            T1R210C05 : $scope.T1R210C05,
            T1R220C01 : $scope.T1R220C01,
            T1R220C02 : $scope.T1R220C02,
            T1R220C03 : $scope.T1R220C03,
            T1R220C04 : $scope.T1R220C04,
            T1R220C05 : $scope.T1R220C05,
            T1R230C01 : $scope.T1R230C01,
            T1R230C02 : $scope.T1R230C02,
            T1R230C03 : $scope.T1R230C03,
            T1R230C04 : $scope.T1R230C04,
            T1R230C05 : $scope.T1R230C05,
            T1R240C01 : $scope.T1R240C01,
            T1R240C05 : $scope.T1R240C05,
            T1R250C01 : $scope.T1R250C01,
            T1R250C05 : $scope.T1R250C05,
            T1R280C01 : $scope.T1R280C01,
            T1R280C02 : $scope.T1R280C02,
            T1R280C03 : $scope.T1R280C03,
            T1R280C04 : $scope.T1R280C04,
            T1R280C05 : $scope.T1R280C05,
            T2R010C01 : $scope.T2R010C01,
            T2R010C02 : $scope.T2R010C02,
            T2R020C01 : $scope.T2R020C01,
            T2R020C02 : $scope.T2R020C02,
            T2R030C01 : $scope.T2R030C01,
            T2R030C02 : $scope.T2R030C02,
            T2R040C01 : $scope.T2R040C01,
            T2R040C02 : $scope.T2R040C02,
            T2R050C01 : $scope.T2R050C01,
            T2R050C02 : $scope.T2R050C02,
            T2R060C01 : $scope.T2R060C01,
            T2R060C02 : $scope.T2R060C02,
            T2R070C01 : $scope.T2R070C01,
            T2R070C02 : $scope.T2R070C02,
            T2R080C01 : $scope.T2R080C01,
            T2R080C02 : $scope.T2R080C02,
            T2R090C01 : $scope.T2R090C01,
            T2R090C02 : $scope.T2R090C02,
            T2R110C01 : $scope.T2R110C01,
            T2R110C02 : $scope.T2R110C02,
            T2R120C01 : $scope.T2R120C01,
            T2R120C02 : $scope.T2R120C02,
            T2R130C01 : $scope.T2R130C01,
            T2R130C02 : $scope.T2R130C02,
            T2R140C01 : $scope.T2R140C01,
            T2R140C02 : $scope.T2R140C02,
            T2R150C01 : $scope.T2R150C01,
            T2R150C02 : $scope.T2R150C02,
            T2R170C01 : $scope.T2R170C01,
            T2R170C02 : $scope.T2R170C02,
            T2R180C01 : $scope.T2R180C01,
            T2R180C02 : $scope.T2R180C02,
            T2R190C01 : $scope.T2R190C01,
            T2R190C02 : $scope.T2R190C02,
            T2R200C01 : $scope.T2R200C01,
            T2R200C02 : $scope.T2R200C02,
            T2R160C01 : $scope.T2R160C01,
            T2R160C02 : $scope.T2R160C02,
            T2R100C01 : $scope.T2R100C01,
            T2R210C01 : $scope.T2R210C01,
            T2R100C02 : $scope.T2R100C02,
            T2R210C02 : $scope.T2R210C02,
            T2R230C01 : $scope.T2R230C01,
            T2R230C02 : $scope.T2R230C02,
            T2R240C01 : $scope.T2R240C01,
            T2R240C02 : $scope.T2R240C02,
            T2R010C03 : $scope.T2R010C03,
            T2R010C04 : $scope.T2R010C04,
            T2R010C05 : $scope.T2R010C05,
            T2R010C06 : $scope.T2R010C06,
            T2R010C07 : $scope.T2R010C07,
            T2R010C08 : $scope.T2R010C08,
            T2R010C09 : $scope.T2R010C09,
            T2R010C10 : $scope.T2R010C10,
            T2R010C11 : $scope.T2R010C11,
            T2R010C12 : $scope.T2R010C12,
            T2R020C03 : $scope.T2R020C03,
            T2R020C04 : $scope.T2R020C04,
            T2R020C05 : $scope.T2R020C05,
            T2R020C06 : $scope.T2R020C06,
            T2R020C07 : $scope.T2R020C07,
            T2R020C08 : $scope.T2R020C08,
            T2R020C09 : $scope.T2R020C09,
            T2R020C10 : $scope.T2R020C10,
            T2R020C11 : $scope.T2R020C11,
            T2R020C12 : $scope.T2R020C12,
            T2R030C03 : $scope.T2R030C03,
            T2R030C04 : $scope.T2R030C04,
            T2R030C05 : $scope.T2R030C05,
            T2R030C06 : $scope.T2R030C06,
            T2R030C07 : $scope.T2R030C07,
            T2R030C08 : $scope.T2R030C08,
            T2R030C09 : $scope.T2R030C09,
            T2R030C10 : $scope.T2R030C10,
            T2R030C11 : $scope.T2R030C11,
            T2R030C12 : $scope.T2R030C12,
            T3R010C01 : $scope.T3R010C01,
            T3R010C02 : $scope.T3R010C02,
            T3R010C03 : $scope.T3R010C03,
            T2R040C03 : $scope.T2R040C03,
            T2R040C04 : $scope.T2R040C04,
            T2R040C05 : $scope.T2R040C05,
            T2R040C06 : $scope.T2R040C06,
            T2R040C07 : $scope.T2R040C07,
            T2R040C08 : $scope.T2R040C08,
            T2R040C09 : $scope.T2R040C09,
            T2R040C10 : $scope.T2R040C10,
            T2R040C11 : $scope.T2R040C11,
            T2R040C12 : $scope.T2R040C12,
            T3R020C01 : $scope.T3R020C01,
            T3R020C02 : $scope.T3R020C02,
            T3R020C03 : $scope.T3R020C03,
            T2R050C03 : $scope.T2R050C03,
            T2R050C04 : $scope.T2R050C04,
            T2R050C05 : $scope.T2R050C05,
            T2R050C06 : $scope.T2R050C06,
            T2R050C07 : $scope.T2R050C07,
            T2R050C08 : $scope.T2R050C08,
            T2R050C09 : $scope.T2R050C09,
            T2R050C10 : $scope.T2R050C10,
            T2R050C11 : $scope.T2R050C11,
            T2R050C12 : $scope.T2R050C12,
            T3R030C01 : $scope.T3R030C01,
            T3R030C02 : $scope.T3R030C02,
            T3R030C03 : $scope.T3R030C03,
            T2R060C03 : $scope.T2R060C03,
            T2R060C04 : $scope.T2R060C04,
            T2R060C05 : $scope.T2R060C05,
            T2R060C06 : $scope.T2R060C06,
            T2R060C07 : $scope.T2R060C07,
            T2R060C08 : $scope.T2R060C08,
            T2R060C09 : $scope.T2R060C09,
            T2R060C10 : $scope.T2R060C10,
            T2R060C11 : $scope.T2R060C11,
            T2R060C12 : $scope.T2R060C12,
            T3R040C01 : $scope.T3R040C01,
            T3R040C02 : $scope.T3R040C02,
            T3R040C03 : $scope.T3R040C03,
            T2R070C03 : $scope.T2R070C03,
            T2R070C04 : $scope.T2R070C04,
            T2R070C05 : $scope.T2R070C05,
            T2R070C06 : $scope.T2R070C06,
            T2R070C07 : $scope.T2R070C07,
            T2R070C08 : $scope.T2R070C08,
            T2R070C09 : $scope.T2R070C09,
            T2R070C10 : $scope.T2R070C10,
            T2R070C11 : $scope.T2R070C11,
            T2R070C12 : $scope.T2R070C12,
            T3R050C01 : $scope.T3R050C01,
            T3R050C02 : $scope.T3R050C02,
            T3R050C03 : $scope.T3R050C03,
            T2R080C03 : $scope.T2R080C03,
            T2R080C04 : $scope.T2R080C04,
            T2R080C05 : $scope.T2R080C05,
            T2R080C06 : $scope.T2R080C06,
            T2R080C07 : $scope.T2R080C07,
            T2R080C08 : $scope.T2R080C08,
            T2R080C09 : $scope.T2R080C09,
            T2R080C10 : $scope.T2R080C10,
            T2R080C11 : $scope.T2R080C11,
            T2R080C12 : $scope.T2R080C12,
            T3R060C01 : $scope.T3R060C01,
            T3R060C02 : $scope.T3R060C02,
            T3R060C03 : $scope.T3R060C03,
            T2R090C03 : $scope.T2R090C03,
            T2R090C04 : $scope.T2R090C04,
            T2R090C05 : $scope.T2R090C05,
            T2R090C06 : $scope.T2R090C06,
            T2R090C07 : $scope.T2R090C07,
            T2R090C08 : $scope.T2R090C08,
            T2R090C09 : $scope.T2R090C09,
            T2R090C10 : $scope.T2R090C10,
            T2R090C11 : $scope.T2R090C11,
            T2R090C12 : $scope.T2R090C12,
            T3R070C01 : $scope.T3R070C01,
            T3R070C02 : $scope.T3R070C02,
            T3R070C03 : $scope.T3R070C03,
            T2R100C03 : $scope.T2R100C03,
            T2R100C04 : $scope.T2R100C04,
            T2R100C05 : $scope.T2R100C05,
            T2R100C06 : $scope.T2R100C06,
            T2R100C07 : $scope.T2R100C07,
            T2R100C08 : $scope.T2R100C08,
            T2R100C09 : $scope.T2R100C09,
            T2R100C10 : $scope.T2R100C10,
            T2R100C11 : $scope.T2R100C11,
            T2R100C12 : $scope.T2R100C12,
            T3R080C01 : $scope.T3R080C01,
            T3R080C02 : $scope.T3R080C02,
            T3R080C03 : $scope.T3R080C03,
            T2R110C03 : $scope.T2R110C03,
            T2R110C04 : $scope.T2R110C04,
            T2R110C05 : $scope.T2R110C05,
            T2R110C06 : $scope.T2R110C06,
            T2R110C07 : $scope.T2R110C07,
            T2R110C08 : $scope.T2R110C08,
            T2R110C09 : $scope.T2R110C09,
            T2R110C10 : $scope.T2R110C10,
            T2R110C11 : $scope.T2R110C11,
            T2R110C12 : $scope.T2R110C12,
            T3R090C01 : $scope.T3R090C01,
            T3R090C02 : $scope.T3R090C02,
            T3R090C03 : $scope.T3R090C03,
            T2R120C03 : $scope.T2R120C03,
            T2R120C04 : $scope.T2R120C04,
            T2R120C05 : $scope.T2R120C05,
            T2R120C06 : $scope.T2R120C06,
            T2R120C07 : $scope.T2R120C07,
            T2R120C08 : $scope.T2R120C08,
            T2R120C09 : $scope.T2R120C09,
            T2R120C10 : $scope.T2R120C10,
            T2R120C11 : $scope.T2R120C11,
            T2R120C12 : $scope.T2R120C12,
            T3R100C01 : $scope.T3R100C01,
            T3R100C02 : $scope.T3R100C02,
            T3R100C03 : $scope.T3R100C03,
            T2R130C03 : $scope.T2R130C03,
            T2R130C04 : $scope.T2R130C04,
            T2R130C05 : $scope.T2R130C05,
            T2R130C06 : $scope.T2R130C06,
            T2R130C07 : $scope.T2R130C07,
            T2R130C08 : $scope.T2R130C08,
            T2R130C09 : $scope.T2R130C09,
            T2R130C10 : $scope.T2R130C10,
            T2R130C11 : $scope.T2R130C11,
            T2R130C12 : $scope.T2R130C12,
            T3R110C01 : $scope.T3R110C01,
            T3R110C02 : $scope.T3R110C02,
            T3R110C03 : $scope.T3R110C03,
            T2R140C03 : $scope.T2R140C03,
            T2R140C04 : $scope.T2R140C04,
            T2R140C05 : $scope.T2R140C05,
            T2R140C06 : $scope.T2R140C06,
            T2R140C07 : $scope.T2R140C07,
            T2R140C08 : $scope.T2R140C08,
            T2R140C09 : $scope.T2R140C09,
            T2R140C10 : $scope.T2R140C10,
            T2R140C11 : $scope.T2R140C11,
            T2R140C12 : $scope.T2R140C12,
            T2R150C03 : $scope.T2R150C03,
            T2R150C04 : $scope.T2R150C04,
            T2R150C05 : $scope.T2R150C05,
            T2R150C06 : $scope.T2R150C06,
            T2R150C07 : $scope.T2R150C07,
            T2R150C08 : $scope.T2R150C08,
            T2R150C09 : $scope.T2R150C09,
            T2R150C10 : $scope.T2R150C10,
            T2R150C11 : $scope.T2R150C11,
            T2R150C12 : $scope.T2R150C12,
            T2R160C03 : $scope.T2R160C03,
            T2R160C04 : $scope.T2R160C04,
            T2R160C05 : $scope.T2R160C05,
            T2R160C06 : $scope.T2R160C06,
            T2R160C07 : $scope.T2R160C07,
            T2R160C08 : $scope.T2R160C08,
            T2R160C09 : $scope.T2R160C09,
            T2R160C10 : $scope.T2R160C10,
            T2R160C11 : $scope.T2R160C11,
            T2R160C12 : $scope.T2R160C12,
            T2R170C03 : $scope.T2R170C03,
            T2R170C04 : $scope.T2R170C04,
            T2R170C05 : $scope.T2R170C05,
            T2R170C06 : $scope.T2R170C06,
            T2R170C07 : $scope.T2R170C07,
            T2R170C08 : $scope.T2R170C08,
            T2R170C09 : $scope.T2R170C09,
            T2R170C10 : $scope.T2R170C10,
            T2R170C11 : $scope.T2R170C11,
            T2R170C12 : $scope.T2R170C12,
            T2R180C03 : $scope.T2R180C03,
            T2R180C04 : $scope.T2R180C04,
            T2R180C05 : $scope.T2R180C05,
            T2R180C06 : $scope.T2R180C06,
            T2R180C07 : $scope.T2R180C07,
            T2R180C08 : $scope.T2R180C08,
            T2R180C09 : $scope.T2R180C09,
            T2R180C10 : $scope.T2R180C10,
            T2R180C11 : $scope.T2R180C11,
            T2R180C12 : $scope.T2R180C12,
            T4R010C01 : $scope.T4R010C01,
            T4R010C02 : $scope.T4R010C02,
            T2R190C03 : $scope.T2R190C03,
            T2R190C04 : $scope.T2R190C04,
            T2R190C05 : $scope.T2R190C05,
            T2R190C06 : $scope.T2R190C06,
            T2R190C07 : $scope.T2R190C07,
            T2R190C08 : $scope.T2R190C08,
            T2R190C09 : $scope.T2R190C09,
            T2R190C10 : $scope.T2R190C10,
            T2R190C11 : $scope.T2R190C11,
            T2R190C12 : $scope.T2R190C12,
            T4R020C01 : $scope.T4R020C01,
            T4R020C02 : $scope.T4R020C02,
            T2R200C03 : $scope.T2R200C03,
            T2R200C04 : $scope.T2R200C04,
            T2R200C05 : $scope.T2R200C05,
            T2R200C06 : $scope.T2R200C06,
            T2R200C07 : $scope.T2R200C07,
            T2R200C08 : $scope.T2R200C08,
            T2R200C09 : $scope.T2R200C09,
            T2R200C10 : $scope.T2R200C10,
            T2R200C11 : $scope.T2R200C11,
            T2R200C12 : $scope.T2R200C12,
            T4R030C01 : $scope.T4R030C01,
            T4R030C02 : $scope.T4R030C02,
            T2R210C03 : $scope.T2R210C03,
            T2R210C04 : $scope.T2R210C04,
            T2R210C05 : $scope.T2R210C05,
            T2R210C06 : $scope.T2R210C06,
            T2R210C07 : $scope.T2R210C07,
            T2R210C08 : $scope.T2R210C08,
            T2R210C09 : $scope.T2R210C09,
            T2R210C10 : $scope.T2R210C10,
            T2R210C11 : $scope.T2R210C11,
            T2R210C12 : $scope.T2R210C12,
            T2R220C03 : $scope.T2R220C03,
            T2R220C04 : $scope.T2R220C04,
            T2R220C05 : $scope.T2R220C05,
            T2R220C06 : $scope.T2R220C06,
            T2R220C07 : $scope.T2R220C07,
            T2R220C08 : $scope.T2R220C08,
            T2R220C09 : $scope.T2R220C09,
            T2R220C10 : $scope.T2R220C10,
            T2R220C11 : $scope.T2R220C11,
            T2R220C12 : $scope.T2R220C12,
            T4R050C01 : $scope.T4R050C01,
            T4R050C02 : $scope.T4R050C02,
            T2R230C03 : $scope.T2R230C03,
            T2R230C04 : $scope.T2R230C04,
            T2R230C05 : $scope.T2R230C05,
            T2R230C06 : $scope.T2R230C06,
            T2R230C07 : $scope.T2R230C07,
            T2R230C08 : $scope.T2R230C08,
            T2R230C09 : $scope.T2R230C09,
            T2R230C10 : $scope.T2R230C10,
            T2R230C11 : $scope.T2R230C11,
            T2R230C12 : $scope.T2R230C12,
            T2R240C03 : $scope.T2R240C03,
            T2R240C04 : $scope.T2R240C04,
            T2R240C05 : $scope.T2R240C05,
            T2R240C06 : $scope.T2R240C06,
            T2R240C07 : $scope.T2R240C07,
            T2R240C08 : $scope.T2R240C08,
            T2R240C09 : $scope.T2R240C09,
            T2R240C10 : $scope.T2R240C10,
            T2R240C11 : $scope.T2R240C11,
            T2R240C12 : $scope.T2R240C12,
            T5R010C01 : $scope.T5R010C01,
            T5R020C01 : $scope.T5R020C01,
            T5R030C01 : $scope.T5R030C01,
            T5R040C01 : $scope.T5R040C01,
            T5R050C01 : $scope.T5R050C01,
            T5R060C01 : $scope.T5R060C01,
            year: $scope.SelectedYear.Id,
            kvartal: $scope.SelectedKvartal.ID
        };

        $http({
            method: "post",
            url: "/Buxgalter/SaveLagerFinOtchet",
            data: JSON.stringify(postData),
            dataType: "json"
        }).then(function (response) {

            console.log(response.data);
            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');

        }, function () {
            alertify.error("ошибка");
            console.log('Error in SaveRasxod');

            document.getElementById('connection').style.visibility = "hidden";
            $("#hh").modal('hide');
        });


    }
}]);