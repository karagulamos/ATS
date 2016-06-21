(function () {
    "use strict";

    var app = angular.module("app.admin.portal");

    app.constant("appSettings", {
        apiEndpoint: "http://localhost:55931"
    });

    app.config(function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptor');
    });

    app.run(['authService', '$rootScope', '$state', '$location', function (authService, $rootScope, $state, $location) {
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
            var authentication = authService.authentication;
            if (authentication.isAuth && toState.name === "login") {
                $state.go("dashboard.index");
            }

            if (!authentication.isAuth && toState.name !== "login") {
                $location.path("/");
            }
        });

        $rootScope.$on("$stateChangeSuccess", function (event, toState, toParams, fromState, fromParams) {
            if (toState.name === "dashboard.index") {
            }
        });

        authService.fillAuthData();
    }]);

    app.config(function ($provide) {
        $provide.decorator('$state', function ($delegate, $stateParams) {
            $delegate.forceReload = function () {
                return $delegate.go($delegate.current, $stateParams, {
                    reload: true,
                    inherit: false,
                    notify: true
                });
            };
            return $delegate;
        });
    });
}());