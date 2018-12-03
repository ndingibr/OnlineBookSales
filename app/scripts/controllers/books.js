'use strict';

angular
    .module('onlineClientApp')
    .controller('BooksCtrl', ['$scope', '$route', 'booksService', 'authService',
        function ($scope, $route, booksService, authService) {

            $scope.subscribedBooks = [];
            $scope.unsubscribedBooks = [];


            $scope.subscription = {
                userId: "",
                bookId: ""
            };

            $scope.email = authService.authentication.email;

            authService.getUsersDetails($scope.email)
                .then(function (results) {
                    $scope.subscription.userId = results.data.id;
                });

            booksService.getBooksSubscribed($scope.email)
                .then(function (results) {
                    $scope.subscribedBooks = results.data;
                });

            booksService.getBooksNotSubscribed($scope.email)
                .then(function (results) {
                    $scope.unsubscribedBooks = results.data;
                });

            $scope.subscribe = function (bookId) {
                $scope.subscription.bookId = bookId;
                booksService.subscribe($scope.subscription)
                    .then(function (saveReault) {
                        $route.reload();
                    })
            }

            $scope.unsubscribe = function (bookId) {
                $scope.subscription.bookId = bookId;
                booksService.unsubscribe($scope.subscription)
                    .then(function (saveReault) {
                        $route.reload();
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
