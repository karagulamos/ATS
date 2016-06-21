(function () {
    'use strict';

    angular.module("app.admin.portal")
           .factory('authService', authService);

    authService.$inject = ['$http', '$q', 'localStorageService', 'appSettings'];

    function authService($http, $q, localStorageService, appSettings) {

        var serviceBase = appSettings.apiEndpoint;
        var authServiceFactory = {};

        var authentication = { isAuth: false, userName: "" };

        function logOut() {
            localStorageService.remove('authorizationData');

            authentication.isAuth = false;
            authentication.userName = "";
        };

        authServiceFactory.requestOtp = function (user) {
            var deferred = $q.defer();

            $http.get(serviceBase + '/api/admin/accounts/forgot', {params: user}).success(function (response) {
                deferred.resolve(response);
            }).error(function (err) {
                deferred.reject(err);
            });

            return deferred.promise;
        }

        authServiceFactory.resetPassword = function (otp) {
            var deferred = $q.defer();

            $http.get(serviceBase + '/api/admin/accounts/reset', {params: otp}).success(function (response) {
                deferred.resolve(response);
            }).error(function (err) {
                deferred.reject(err);
            });

            return deferred.promise;
        }

        authServiceFactory.saveRegistration = function (registration) {
            logOut();

            return $http.post(serviceBase + '/api/admins/accounts/register', registration).then(function (response) {
                return response;
            });
        };

        authServiceFactory.login = function (loginData) {
            var data = {
                "grant_type": "password",
                "userName": loginData.userName, "password": loginData.password
            };

            var deferred = $q.defer();

            $http.post(serviceBase + '/token', $.param(data), { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
                 .success(function (response) {
                     localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName });

                     authentication.isAuth = true;
                     authentication.userName = loginData.userName;

                     deferred.resolve(response);
                 }).error(function (err, status) {
                     logOut();
                     deferred.reject(err);
                 });

            return deferred.promise;

        };

        authServiceFactory.logOut = function () {
            localStorageService.remove('authorizationData');

            authentication.isAuth = false;
            authentication.userName = "";
        };

        authServiceFactory.fillAuthData = function () {
            var authData = localStorageService.get('authorizationData');

            if (authData) {
                authentication.isAuth = true;
                authentication.userName = authData.userName;
            }
        }

        authServiceFactory.authentication = authentication;

        return authServiceFactory;
    };
}());