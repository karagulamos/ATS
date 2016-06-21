(function () {
    'use strict';
    angular.module("app.admin.portal")
           .controller('LoginController', LoginController);

    LoginController.$inject = ['$state', 'authService'];

    function LoginController($state, authService) {
        var vm = this;

        vm.loginData = { userName: "", password: "" };
        vm.message = "";
        vm.loading = false;

        vm.login = function () {
            vm.loading = true;

            authService.login(vm.loginData).then(function (response) {
                $state.go("dashboard.index").then(function() { /* success */}, function(response) {
                    vm.loading = false;
                    if (response.status === 401) {
                        vm.message = "Access Denied. Please Contact Administrator.";
                    } else {
                        vm.message = "An unexpected error occurred. Please Contact Administrator.";
                    }

                    console.log(response);
                });
            },
             function (err) {
                 vm.loading = false;

                 if (err && err.hasOwnProperty("error_description")) {
                     var description = err.error_description.toLowerCase();

                     if (description && !~description.indexOf("exception")) {
                         vm.message = err.error_description;
                     } else {
                         vm.message = "Error - Your username / password may be incorrect. Try again or contact admin.";
                     }
                 }

                 console.log(err);
             });
        };
    }
})();
