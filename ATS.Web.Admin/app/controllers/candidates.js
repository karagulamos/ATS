(function () {
    "use strict";

    angular.module("app.admin.portal")
           .controller("CandidateListController", CandidateListController)
           .controller("SearchModalController", SearchModalController);

    CandidateListController.$inject = ["$scope", "candidateService", "appSettings", "states", "$uibModal", "$http"];

    function CandidateListController($scope, candidateService, appSettings, states, $uibModal, $http) {
        var vm = this;

        var parentVm = $scope.$parent.vm;

        vm.showNotification = false;
        vm.isLoading = false;

        vm.pageSize = 10;
        vm.maxSize = 7;
        vm.currentPage = parentVm.currentPage || 1;

        vm.states = states;

        vm.previewLink = function (candidateId) {
            vm.downloadingFile = candidateId;

            candidateService.downloadCandidateResume(candidateId).then(function (response) {
                vm.message = "Candidate Resume Successfully Downloaded";
                vm.class = "check green";
                vm.downloadingFile = null;

                console.log("Candidate CV download: " + response);
            }, function (error) {
                if (error === "NOT_SUPPORTED") {
                    vm.message = "Browser Error: Please update your browser.";
                    vm.class = "bolt red";
                } else {
                    vm.message = "Download Error: Internal error occurred. Try again shortly.";
                    vm.class = "bolt red";
                }

                vm.downloadingFile = null;

                console.log("Error downloading candidate file: " + error);
            });
        }

        vm.delete = function (candidateId) {
            candidateService.deleteCandidate(candidateId).then(function (data) {
                vm.message = "Candidate sucessfully deleted.";
                vm.class = "check green";
                vm.loading = false;

                vm.refresh();

            }, function (error) {
                console.log(error);
                vm.message = "An unexpected error occurred. Please try again or contact admin";
                vm.class = "bolt red";
                vm.isLoading = false;
            });
        }

        vm.open = function () {
            var modalInstance = $uibModal.open({
                templateUrl: 'searchModal.tmpl.html',
                controller: 'SearchModalController',
                resolve: {
                    model: function () {
                        vm.search = {
                            name: '', age: null, email: '', state: '', direction: 'eq'
                        };
                        return vm;
                    }
                }
            });

            modalInstance.result.then(function (model) {
                vm.search = model;
            });
        };

        vm.refresh = function (search) {
            vm.isLoading = true;
            vm.candidates = [];

            vm.query = {};

            candidateService.getCandidates(vm.pageSize, vm.currentPage, search).then(function (data) {
                vm.candidates = data.items;

                vm.totalCount = data.count;

                if (!search) {
                    parentVm.totalCandidates = data.count;
                    parentVm.currentPage = vm.currentPage;
                }

                vm.totalPages = Math.ceil(vm.totalCount / vm.pageSize);

                vm.message = "Showing page " + (vm.totalPages > 0 ? vm.currentPage : 0) + " of " + vm.totalPages;

                vm.class = vm.candidates.length > 0 ? "check green" : "info-circle blue";
                vm.isLoading = false;
            }, function (error) {
                console.log(error);
                vm.message = "An unexpected error occurred. Please try again or contact admin";
                vm.class = "bolt red";
                vm.isLoading = false;
            });

            vm.showNotification = true;
        }

        vm.refresh();
    };

    SearchModalController.$inject = ["$scope", "$uibModalInstance", "model"];

    function SearchModalController($scope, $uibModalInstance, model) {
        $scope.vm = model;
        $scope.ok = function () {
            model.refresh($scope.vm.search);
            $uibModalInstance.close();
        };

        $scope.cancel = function () {
           // model.refresh($scope.vm.search);
            $uibModalInstance.dismiss('cancel');
        };

    }
}());