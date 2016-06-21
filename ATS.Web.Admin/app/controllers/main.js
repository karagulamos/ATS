(function () {
    "use strict";

    angular.module("app.admin.portal")
           .controller("MainController", MainController);

    MainController.$inject = ['$state', '$location', 'authService'];

    function MainController($state, $location, authService) {
        var vm = this;

        vm.logOut = function () {
            authService.logOut();
            $state.go('dashboard.index');
        }

        vm.authentication = authService.authentication;
    }
})();
