'use strict';

angular
  .module('onlineClientApp')
  .controller('SignupCtrl', ['$scope', '$location', '$routeParams', '$timeout', 'authService', 'roleService',
    function ($scope, $location, $routeParams, $timeout, authService, roleService) {

      $scope.savedSuccessfully = false;
        $scope.message = "";
        $scope.errors = "";


      $scope.signup = {
        email: "",
        firstName: "",
        lastName: "",
        password: "",
        confirmPassword: "",
        phoneNumber: "",
        selectedRole: "",
        police_Station: "",
        active: false,
        adminState: false
      };


      authService.getRoles()
         .then(function (results) {
            $scope.roles = results.data;
         });

        authService.getPoliceStations()
            .then(function (results) {
                $scope.policeStations = results.data;
            });

        if ($routeParams.email !== undefined) {
            authService.getUsersDetails($routeParams.email)
                .then(function (results) {
                    $scope.signup.email = results.data[0].email;
                    $scope.signup.firstName = results.data[0].firstName;
                    $scope.signup.lastName = results.data[0].lastName;
                    $scope.signup.phoneNumber = results.data[0].phoneNumber;
                    $scope.signup.selectedRole = results.data[0].id.toString();
                    $scope.signup.police_Station = results.data[0].police_Station;
                    $scope.signup.adminState = $routeParams.isAdmin
                });
        } 

      $scope.register = function () {
          authService.saveRegistration($scope.signup).then(function success(response) {
          $scope.savedSuccessfully = true;
          $scope.message = "User has been registered successfully, please check your email to confirm your email address";
          startTimer();
        },
          function (error) {
            var errors = [];
            for (var key in error.data) {
              for (var i = 0; i < error.data.length; i++) {
                errors.push(error.data[i]);
              }
            }
            $scope.errors = errors;
          });
      };

      var startTimer = function () {
        var timer = $timeout(function () {
          $timeout.cancel(timer);
          $location.path('/login');
        }, 2000);
      }

    }]);
