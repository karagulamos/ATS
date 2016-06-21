(function () {
    "use strict";

    angular.module("app.admin.portal").config([
	   "$stateProvider", "$urlRouterProvider", "$locationProvider", function ($stateProvider, $urlRouterProvider, $locationProvider) {

	       // DEFAULT

	       $urlRouterProvider.otherwise("/login");

	       // LOGIN

	       $stateProvider.state("login", {
	           url: "/login",
	           templateUrl: "app/views/login.html",
	           controller: "LoginController as vm"
	       });

	       $stateProvider.state("forgotPassword", {
	           url: "/reset",
	           templateUrl: "app/views/login.forgotpassword.html",
	           controller: "ForgotPasswordController as vm"
	       });

	       // DASHBOARD

	       $stateProvider.state("dashboard", {
	           url: "/dashboard",
	           abstract: true,
	           templateUrl: "app/views/dashboard.layout.html",
	           controller: "DashboardController as vm",
	           resolve: {
	               dashboardService: "dashboardService",
	               metadata: function (dashboardService) {
	                   return dashboardService.getMetadata();
	               }
	           }

	       }).state("dashboard.index", {
	           url: "/",
	           templateUrl: "app/views/dashboard.index.html"
	       });

	       // CUSTOMERS

	       $stateProvider.state("dashboard.candidates", {
	           url: "/candidates",
	           templateUrl: "app/views/dashboard.candidates.html",
	           controller: "CandidateListController as vm",
	           resolve: {
	               candidateService: "candidateService",

	               states: function (candidateService) {
	                   return candidateService.getStates();
	               }
	           }
	       }).state("dashboard.candidatesAdd", {
	           url: "/candidates/add",
	           templateUrl: "app/views/dashboard.candidates.add.html",
	           controller: "CandidateAddController as vm",
	           resolve: {
	               candidate: function () {
	                   return { enabled: 0, candidateId: 0 };
	               },

	               viewOptions: function () {
	                   return { title: "ADD" }
	               }
	           }
	       }).state("dashboard.candidatesEdit", {
	           url: "/candidates/edit/:candidateId",
	           templateUrl: "app/views/dashboard.candidates.add.html",
	           controller: "CandidateAddController as vm",
	           resolve: {
	               candidateService: "candidateService",

	               candidate: function (candidateService, $stateParams) {
	                   return candidateService.getCandidateById($stateParams.candidateId);
	               },

	               states: function (candidateService) {
                       return candidateService.getStates();
                   },

	               viewOptions: function () {
	                   return { title: "EDIT" }
	               }
	           }
	       });


	       // CONFIG

	       $locationProvider.html5Mode({
	           enabled: false,
	           requireBase: false
	       });
	   }]);
}());