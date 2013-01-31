'use strict';

var app = angular.module('moneyNow2', ['ui', 'moneyNow2.services']).
  config(function ($routeProvider) {
      $routeProvider.
        when('/', { controller: MoneyNowController, templateUrl: 'app/partials/converter.html' }).
        otherwise({ redirectTo: '/' });
  });