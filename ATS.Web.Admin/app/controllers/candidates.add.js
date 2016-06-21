(function () {
    angular.module("app.admin.portal")
        .controller("CandidateAddController", CandidateAddController);

    CandidateAddController.$inject = ["$scope", "common", "candidateService", "candidate", "states", "viewOptions"];

    function CandidateAddController($scope, common, candidateService, candidate, states, viewOptions) {
        var vm = this;
        var $timeout = common.$timeout,
                $state = common.$state;

        vm.message = [];
        vm.loading = false;
        vm.candidate = candidate;
        vm.states = states;

        vm.save = function (candidate) {
            vm.message = [];

            vm.loading = true;
            vm.showNotification = false;

            candidateService.saveCandidate(candidate).then(function (data) {
                vm.message.push("Update successful.");
                vm.class = "check green";
                vm.showNotification = true;
                vm.loading = false;

                $timeout(function () {
                    $state.go("dashboard.candidates");
                }, 1000);

            }, function (error) {

                if (error.data.modelState) {
                    for (var key in error.data.modelState) {
                        if (error.data.modelState.hasOwnProperty(key)) {
                            vm.message.push(error.data.modelState[key]);
                        }
                    }
                }

                if (error.data.exceptionMessage)
                    vm.message.push(error.data.exceptionMessage);

                if (error.data.message)
                    vm.message.push(error.data.message);

                vm.class = "bolt red";
                vm.showNotification = true;
                vm.loading = false;

            });
        }
    }
}());