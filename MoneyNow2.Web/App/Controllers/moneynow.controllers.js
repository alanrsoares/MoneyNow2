var app = angular.module('moneyNow2', ['ui', 'moneyNow2Services']);

app.controller('MoneyNowController', function ($scope, $http, CurrencyInfo, CurrencyConverter) {

    $scope.currencies = CurrencyInfo.query();

    $scope.convert = function () {
        CurrencyConverter.get({
            amount: $scope.amount,
            from: $scope.from,
            to: $scope.to
        }, function (data) {
            $scope.showResult = true;
            $scope.currencySymbol = data.info.currencySymbol;
            $scope.result = data.result;
        });
    };
});