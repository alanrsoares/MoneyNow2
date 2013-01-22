'use strict';

angular.module('moneyNow2.services', ['ngResource'])
    .factory('CurrencyInfo', function ($resource) {
        return $resource('/api/currencyinfos/:currencyId', {}, {
            query: {
                method: 'GET',
                params: { currencyId: '' },
                isArray: true
            }
        });
    })
    .factory('CurrencyConverter', function ($resource) {
        return $resource('/api/CurrencyConverter/:amount/:from/:to', {}, {
            query: {
                method: 'GET',
                params: {},
                isArray: true
            }
        });
    });