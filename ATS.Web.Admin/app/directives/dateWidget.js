(function () {
    "use strict";

    angular.module("app.admin.portal")
           .directive('dateWidget', function () {
               return {
                   restrict: 'A',
                   link: function (scope, element) {
                       angular.element(document).ready(function () {
                           var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
                           var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];

                           var newDate = new Date();
                           newDate.setDate(newDate.getDate());
                           var formattedDate = days[newDate.getDay()] + " " + newDate.getDate() + ' ' + months[newDate.getMonth()] + ' ' + newDate.getFullYear();

                           //scope.$apply(function() {
                               $('#Date').html(formattedDate);
                           //});

                           setInterval(function () {
                               var seconds = new Date().getSeconds();
                               $("#sec").html((seconds < 10 ? "0" : "") + seconds);
                           }, 1000);

                           setInterval(function () {
                               var minutes = new Date().getMinutes();
                               $("#min").html((minutes < 10 ? "0" : "") + minutes);
                           }, 1000);

                           setInterval(function () {
                               var hours = new Date().getHours();
                               $("#hours").html((hours < 10 ? "0" : "") + hours);
                           }, 1000);
                       });
                   }
               }
           });
}());