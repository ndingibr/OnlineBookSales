'use strict';

angular
    .module('onlineClientApp')
    .factory('authService', ['$http', '$q', 'ngAuthSettings', 'localStorageService',
        function ($http, $q, ngAuthSettings, localStorageService) {

            var serviceBase = ngAuthSettings.apiServiceBaseUri;

            var authentication = {
                id: 0,
                isAuth: false,
                email: "",
                useRefreshTokens: false
            };

            var service = {
                saveRegistration: saveRegistration,
                logIn: logIn,
                logOut: logOut,
                authentication: authentication,
                getLoggedUserEmail: getLoggedUserEmail,
                getUsersDetails: getUsersDetails
            };

            return service;

            function saveRegistration(registration) {
                logOut();
                return $http.post(serviceBase + 'api/users/signup', registration).then(function (response) {
                    return response;
                });
            };

            function logIn(login) {
                return $http.post(serviceBase + 'api/users/login', login).then(function (response) {

                    if (login.useRefreshTokens) {
                        localStorageService.set('authorization', { token: response.access_token, email: login.email, refreshToken: response.refresh_token, useRefreshTokens: true });
                    }
                    else {
                        localStorageService.set('authorization', { token: response.access_token, email: login.email, refreshToken: "", useRefreshTokens: false });
                    }
                    authentication.isAuth = true;
                    authentication.email = login.email;
                    authentication.id = login.Id;
                    authentication.useRefreshTokens = login.useRefreshTokens;

                    return response;
                });
            };

            function getLoggedUserEmail() {
                return localStorageService.authorization.email;
            }
            
            function getUsersDetails(email) {
                return $http.get(serviceBase + 'api/users/user?email=' + email).then(function (results) {
                    return results;
                });
            }


            function logOut() {
                localStorageService.remove('authorization');
                authentication.isAuth = false;
                authentication.email = "";
                authentication.useRefreshTokens = false;
            };

        }]);
