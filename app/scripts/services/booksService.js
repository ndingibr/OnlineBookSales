'use strict';

angular
    .module('onlineClientApp')
    .factory('booksService',
    [
        '$http', '$q', 'ngAuthSettings',
        function ($http, $q, ngAuthSettings) {

            var serviceBase = ngAuthSettings.apiServiceBaseUri;

            var service = {
                getBooks: getBooks,
                subscribe: subscribe,
            };

            return service;

            function getBooks() {
                return $http.get(serviceBase + 'api/books')
                    .then(function (results) {
                        return results;
                    });
            };

            function subscribe(subscriptions) {
                return $http.post(serviceBase + 'api/books/subscribe', subscriptions).then(function (response) {
                    return response;
                });
            };
                      
        }
    ]);
