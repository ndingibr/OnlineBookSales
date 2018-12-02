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
                getBooksSubscribed: getBooksSubscribed,
                getBooksNotSubscribed: getBooksNotSubscribed,
                unsubscribe: unsubscribe
            };

            return service;

            function getBooks() {
                return $http.get(serviceBase + 'api/books')
                    .then(function (results) {
                        return results;
                    });
            };
            
            function getBooksSubscribed(email) {
                return $http.get(serviceBase + 'api/books/bookssubscriptionbyemail?email=' + email)
                    .then(function (results) {
                        return results;
                    });
            };

            function getBooksNotSubscribed(email) {
                return $http.get(serviceBase + 'api/books/booksnotsubscriptionbyemail?email=' + email)
                    .then(function (results) {
                        return results;
                    });
            };

            function subscribe(subscription) {
                return $http.post(serviceBase + 'api/books/subscribe', subscription).then(function (response) {
                    return response;
                });
            };

            function unsubscribe(subscription) {
                return $http.post(serviceBase + 'api/books/unsubscribe', subscription).then(function (response) {
                    return response;
                });
            };
                      
        }
    ]);
