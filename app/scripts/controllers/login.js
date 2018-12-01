'use strict';
angular.module('onlineClientApp')
  .controller('LoginCtrl',
  [
    '$scope', '$location', 'authService',
    function ($scope, $location, authService) {

      $scope.login = {
        email: "",
        password: "",
        useRefreshTokens: false
      };

      $scope.message = "";
      $scope.userLogin = userLogin;

      function userLogin(){
        authService.logIn($scope.login)
          .then(function (saveReault) {
            $scope.message = "Login Successful";
            $location.path('/books');
          })
          .catch(function (error) {
            $scope.message = err.error_description;
          });
      };
    }
  ]);


