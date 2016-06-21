(function () {
    "use strict";

    angular.module("app.admin.portal")
           .directive('confirmationModal', confirmationModal);

    confirmationModal.$inject = ["$uibModal"];

    function confirmationModal($uibModal) {
        return {
            restrict: 'A',
            scope: {
                confirmClick: "&"
            },
            link: function (scope, element, attrs) {
                element.css({ 'cursor': 'pointer' });

                element.bind('click', function () {
                    var message = attrs.modalText || "Are you sure?";

                    var modalInstance = $uibModal.open({
                        templateUrl: 'app/directives/confirmationModal.html',
                        controller: function ($scope, $uibModalInstance) {
                            $scope.message = message,

                            $scope.ok = function () {
                                $uibModalInstance.close();
                            };

                            $scope.cancel = function () {
                                $uibModalInstance.dismiss('cancel');
                            };
                        }
                    });

                    modalInstance.result.then(function () {
                        scope.confirmClick();
                    });
                });

            }

        }
    }
}());