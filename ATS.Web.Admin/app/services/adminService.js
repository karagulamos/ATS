(function () {
    "use strict";

    angular.module("app.admin.portal")
           .factory("adminService", adminService);

    adminService.$inject = ["$resource", "appSettings"];

    function adminService($resource, appSettings) {

        function createResource(command) {
            var address = appSettings.apiEndpoint + "/api/admin/users/" + command;
            return $resource(address);
        }

        return {
            getAdmins: function (pageSize, pageNumber, adminType) {
                var params = { $count: true };

                switch(adminType) {
                    case "bank":
                        params["$filter"] = "RoleName eq 'bankAdmin'";
                        break;

                    case "mailpay":
                        params["$filter"] = "RoleName ne 'bankAdmin'";
                        break;

                    default:
                        params["$filter"] = "false";
                }

                if (pageSize !== undefined && pageNumber !== undefined) {
                    params["$skip"] = (pageNumber - 1) * pageSize;
                    params["$top"] = pageSize;
                }

                return createResource("query").get(params).$promise;
            },

            getAdminUser: function (adminId) {
                return createResource(":adminId").get({ adminId: adminId }).$promise;
            },

            addUser: function (admin) {
                var operation = "edit";

                if (!admin.adminId) {
                    operation = "add";
                    delete admin.adminId;
                }
                return createResource(operation).save(admin, function (data) {
                    return data;
                }).$promise;
            },

            getPendingApprovals: function (pageSize, pageNumber) {
                var params = { $count: true };

                if (pageSize !== undefined && pageNumber !== undefined) {
                    params["$skip"] = (pageNumber - 1) * pageSize;
                    params["$top"] = pageSize;
                }

                return createResource("pending/approvals").get(params).$promise;
            },

            approveAdmin: function (approvalId, stagingId, actionId) {
                return createResource("pending/approve").save({ "approvalId": approvalId, "stagingId": stagingId, "actionId": actionId }, function (data) {
                    return data;
                }).$promise;
            },

            rejectAdmin: function (approvalId, stagingId, actionId) {
                return createResource("pending/reject").save({ "approvalId": approvalId, "stagingId": stagingId, "actionId": actionId }, function (data) {
                    return data;
                }).$promise;
            }
        }
    }
}());
