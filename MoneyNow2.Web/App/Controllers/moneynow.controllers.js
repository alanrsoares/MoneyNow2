var app = angular.module('moneyNow2', ['ui', 'moneyNow2Services']);

app.controller('MoneyNowController', function ($scope, $http, CurrencyInfo, CurrencyConverter) {

    var currencyInfos;

    var format = function (state) {
        return "<img style='width: 25px' src='" + state.url + "'/> - " + state.text + " - (" + state.id + ")";
    };

    CurrencyInfo.query(function (data) {
        currencyInfos = data;
    });

    $scope.currencies = {
        query: function (query) {
            var data = { results: [] };
            angular.forEach(currencyInfos, function (item) {
                if (query.term.toUpperCase() === item.isoCurrencySymbol.substring(0, query.term.length).toUpperCase()) {
                    data.results.push({
                        id: item.isoCurrencySymbol,
                        text: item.currencyEnglishName,
                        url: item.flagUrl
                    });
                }
            });
            query.callback(data);
        },
        formatResult: format,
        formatSelection: format
    };


    //#region Defaults

    $scope.amount = 1;

    CurrencyInfo.get({ currencyId: "USD" }, function (item1) {
        $scope.from = {
            id: item1.isoCurrencySymbol,
            text: item1.currencyEnglishName,
            url: item1.flagUrl
        };
        CurrencyInfo.get({ currencyId: "BRL" }, function (item2) {
            $scope.to = {
                id: item2.isoCurrencySymbol,
                text: item2.currencyEnglishName,
                url: item2.flagUrl
            };

            $scope.convert();
        });
    });



    //#endregion

    $scope.convert = function () {

        CurrencyConverter.get({
            amount: $scope.amount,
            from: $scope.from.id,
            to: $scope.to.id
        }, function (data) {
            $scope.showResult = true;
            $scope.currencySymbol = data.info.currencySymbol;
            $scope.result = data.result;
        });
    };

});