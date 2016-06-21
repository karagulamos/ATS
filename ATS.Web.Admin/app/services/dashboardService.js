(function () {
    "use strict";

    angular.module("app.admin.portal")
           .factory("dashboardService", dashboardService);

    dashboardService.$inject = ["$resource", "appSettings"];

    function dashboardService($resource, appSettings) {

        function createResource(command) {
            var address = appSettings.apiEndpoint + "/api/identity/" + command;
            return $resource(address);
        }

        return {
            getMetadata: function () {
                return createResource("details").get().$promise;
            }
           
        }
    }
}());
