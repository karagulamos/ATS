(function () {
    'use strict';
    angular.module("app.admin.portal")
           .controller('ForgotPasswordController', ForgotPasswordController);

    ForgotPasswordController.$inject = ['authService'];

    function ForgotPasswordController(authService) {
        var vm = this;

        vm.userData = { username: "" };
        vm.message = "";
        vm.loading = false;
        vm.Otp = { requestId: 0, code: "" };



        vm.requestOtp = function (userData) {
            vm.loadingOtp = true;

            authService.requestOtp(userData).then(function (response) {
                vm.message = "Please enter the OTP code you received on your phone.";
                vm.Otp.requestId = response;
                vm.loadingOtp = false;

                vm.summary = "success-message";

            },
            function (err) {
                vm.loadingOtp = false;

                if (err.hasOwnProperty("message")) {
                    var description = err.error_description.toLowerCase();

                    if (description && !~description.indexOf("exception")) {
                        vm.message = err.message;
                    } else {
                        vm.message = "Error - You have entered an incorrect email. Try again or contact admin.";
                    }
                }

                vm.summary = "error-message";

                console.log(err);
            });
        }

        vm.resetPassword = function (otp) {
            vm.loading = true;

            authService.resetPassword(otp).then(function (response) {
                //$state.go("dashboard.index");
                vm.message = "A new password has been sent to your email.";
                vm.loading = false;

                vm.summary = "success-message";

                vm.Otp.code = "";
                vm.Otp.requestId = 0;
            },
             function (err) {
                 vm.loading = false;

                 if (err.hasOwnProperty("message")) {
                     var description = err.message.toLowerCase();

                     if (description && !~description.indexOf("exception")) {
                         vm.message = err.message;
                     } else {
                         vm.message = "Error - You have entered an invalid email or OTP. Try again or contact admin.";
                     }
                 }

                 vm.summary = "error-message";
                 console.log(err);
             });
        };
    }
})();
