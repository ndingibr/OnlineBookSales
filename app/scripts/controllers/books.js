'use strict';

angular
    .module('onlineClientApp')
    .controller('BooksCtrl', ['$scope', '$location','booksService', 'authService', 
        function ($scope, $location, booksService, authService) { 

            $scope.subscribedBooks = [];
            $scope.unsubscribedBooks = [];


            $scope.subscription = {
                userId: "",
                bookId: ""
            };

            $scope.email = authService.authentication.email;

            booksService.getBooksSubscribed($scope.email)
                .then(function (results) {
                    $scope.subscribedBooks = results.data;
                });

            booksService.getBooksNotSubscribed($scope.email)
                .then(function (results) {
                    $scope.unsubscribedBooks = results.data[0];
                });

            $scope.subscribe = function (bookId) {

                authService.getUsersDetails($scope.email )
                    .then(function (results) {
                        $scope.subscription.userId = results.data.id;
                    });

                $scope.subscription.bookId = bookId;

                booksService.subscribe($scope.subscription)
                    .then(function (saveReault) {
                        startTimer();
                    })
            }

            $scope.unsubscribe = function (bookId) {

                authService.getUsersDetails($scope.email)
                    .then(function (results) {
                        $scope.subscription.userId = results.data.id;
                    });

                $scope.subscription.bookId = bookId;

                booksService.unsubscribe($scope.subscription)
                    .then(function (saveReault) {
                        startTimer();
                    })
            }

            $scope.filter_by = function (field) {
                console.log(field);
                console.log($scope.g[field]);
                if ($scope.g[field] === '') {
                    delete $scope.f['__' + field];
                    return;
                }
                $scope.f['__' + field] = true;
                $scope.data.forEach(function (v) {
                    console.log(v['__' + field]);
                    v['__' + field] = v[field] < $scope.g[field];
                })

            }

            var startTimer = function () {
                var timer = $timeout(function () {
                    $timeout.cancel(timer);
                    $location.path('/books');
                }, 2000);
            }

        }]);
