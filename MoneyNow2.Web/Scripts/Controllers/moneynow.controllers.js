var GoogleFinanceConverterAPI = (function () {
    function GoogleFinanceConverterAPI() {
        this.serviceUrl = 'http://www.google.com/finance/converter';
    };

    GoogleFinanceConverterAPI.prototype.getConversion = function (amount, from, to, callback) {
        $.get(this.serviceUrl, { 'a': amount, 'from': from, 'to': to }, function (data) {
            var result = $(data.responseText).find('span.bld').text();
            callback(result);
        });
    };

    return GoogleFinanceConverterAPI;

})();

var MoneyNowController = function ($scope, $http) {
    $http.get('/api/currencyinfos').success(function (data) {
        $scope.currencies = data;
    });

    $scope.convert = function () {
        new GoogleFinanceConverterAPI()
            .getConversion($scope.amount, $scope.from, $scope.to, function (result) {

                var splitted = result.split(" ");

                $http.get('/api/currencyinfos/' + splitted[1]).success(function (data) {
                    $("#result")
                        .show()
                        .html("Result: <strong>" + data.currencySymbol + splitted[0] + "</strong>");
                });
                
            });
    };
};