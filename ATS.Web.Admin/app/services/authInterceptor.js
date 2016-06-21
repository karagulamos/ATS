(function () {
    'use strict';

    angular.module("app.admin.portal")
           .factory('authInterceptor', authInterceptor);

    authInterceptor.$inject = ['$q', '$location', 'localStorageService'];

    function authInterceptor($q, $location, localStorageService) {
        var authInterceptorServiceFactory = {};

        authInterceptorServiceFactory.request = function (config) {
            config.headers = config.headers || {};

            var authData = localStorageService.get('authorizationData');

            if (authData) {
                config.headers.Authorization = 'Bearer ' + authData.token;
            }

            return config;
        }

        authInterceptorServiceFactory.responseError = function (rejection) {
            if (rejection.status === 401) {
                $location.path('/login');
                localStorageService.remove('authorizationData');
            }
            return $q.reject(rejection);
        }

        return authInterceptorServiceFactory;
    };
}());