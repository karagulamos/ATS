(function () {
    angular.module("app.admin.portal").controller("DashboardController", DashboardController);

    DashboardController.$inject =
        ["$state", "authService", "metadata"];

    function DashboardController($state, authService, metadata) {
        var vm = this;
        vm.showNotification = false;
        vm.isLoading = false;

        vm.totalCandidates = metadata.totalCandidates;

        vm.thisUser = metadata.currentUser;

        vm.logOut = function () {
            authService.logOut();
            $state.go("login", {}, { reload: true });
        }
    };
}());