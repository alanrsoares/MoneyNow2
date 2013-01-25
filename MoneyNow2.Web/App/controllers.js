var MoneyNowController = function ($scope, $http, CurrencyInfo, CurrencyConverter) {

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

            var searchTerm = query.term.toUpperCase();
            var regexp = new RegExp(eval("/" + searchTerm + "/i"));

            angular.forEach(currencyInfos, function (item) {

                var symbol = item.isoCurrencySymbol;
                var name = item.currencyEnglishName;
                var symbolSlice = symbol.substring(0, searchTerm.length).toUpperCase();
                var nameSlice = name.substring(0, searchTerm.length).toUpperCase();

                if (searchTerm === symbolSlice ||
                    searchTerm === nameSlice ||
                    regexp.test(name)) {

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

    $scope.showResult = false;

    $scope.amount = 1;

    CurrencyInfo.get({ currencyId: "USD" }, function (item1) {
        $scope.from = {
            id: item1.isoCurrencySymbol,
            text: item1.currencyEnglishName,
            url: item1.flagUrl
        };
    });

    CurrencyInfo.get({ currencyId: "BRL" }, function (item2) {
        $scope.to = {
            id: item2.isoCurrencySymbol,
            text: item2.currencyEnglishName,
            url: item2.flagUrl
        };
    });

    //#endregion

    $scope.switch = function () {
        var aux = $scope.from;
        $scope.from = $scope.to;
        $scope.to = aux;
    };

    var isValidModel = function () {
        return (
            typeof $scope.amount !== "undefined" &&
            typeof $scope.from !== "undefined" &&
            typeof $scope.to !== "undefined"
        );
    };

    $scope.convert = function () {

        if (!isValidModel()) return false;

        CurrencyConverter.get({
            amount: $scope.amount,
            from: $scope.from.id,
            to: $scope.to.id
        }, function (data) {
            $scope.result = data.result;
            $scope.showResult = true;
        });

        return true;

    };

    var convertIfModelChanged = function (newValue, oldValue) {
        if (newValue !== oldValue) $scope.convert();
    };

    $scope.$watch('amount', convertIfModelChanged);
    $scope.$watch('from', convertIfModelChanged);
    $scope.$watch('to', convertIfModelChanged);

};