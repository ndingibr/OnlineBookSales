'use strict';

angular
  .module('onlineClientApp')
  .controller('UsersCtrl', ['$scope', '$location', '$timeout', 'authService', 'roleService',
    function ($scope, $location, $timeout, authService, roleService) {

      //TODO:Remove variables injected but not being used

      $scope.users = []

      authService.getUsers()
        .then(function (results) {
          $scope.users = results.data;
        });

      $scope.loadSignup = function (emailAddress) {
          $location.path('/signup').search({
              email: emailAddress,
              isAdmin: true });
      };
     
    }]);
