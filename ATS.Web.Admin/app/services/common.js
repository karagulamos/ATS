(function () {
    "use strict";

    angular.module("app.admin.portal")
           .factory("common", common);

    common.$inject = ["$state", "$timeout"];

    function common($state, $timeout) {
        return {
            parentVm: function(scope) {
                return scope.$parent.vm;
            },
            $state: $state,
            $timeout: $timeout
        }
    }
}());
