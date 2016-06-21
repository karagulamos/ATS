(function () {
    "use strict";

    angular.module("app.admin.portal")
           .directive('datePicker', datePicker);

    function datePicker() {
        return {
            restrict: 'A',
            link: function (scope, element) {
                $(element).datepicker({
                    dateFormat: "dd/mm/yy",
                    changeMonth: true,
                    changeYear: true,
                    yearRange: "1920:c+0"
                });
            }
        }
    }
}());