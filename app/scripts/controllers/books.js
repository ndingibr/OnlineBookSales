'use strict';

angular
    .module('onlineClientApp')
    .controller('BooksCtrl', ['$scope', 'booksService', 'authService',
        function ($scope, booksService, authService) {

            $scope.books = [];


            $scope.subscription = {
                Id: "",
                userId: "",
                bookId: ""
            };

            booksService.getBooks()
                .then(function (results) {
                    $scope.books = results.data;
                });

            $scope.subscribe = function (bookId) {

                var email = authService.authentication.email;
                    authService.getUsersDetails(email)
                    .then(function (results) {
                        $scope.subscription.userId = results.id;
                    });

                $scope.subscription.bookId = bookId;

                booksService.subscribe($scope.subscription)
                    .then(function (saveReault) {
                        $location.path('/books');
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

        }]);
