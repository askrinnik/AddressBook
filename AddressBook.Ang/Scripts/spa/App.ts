///// <reference path="../typings/angular-ui-router/index.d.ts"/>

module AddressBookApp {
  "use strict";

  angular.module("AddressBook", [
    //--vendors
    "ui.router", // $stateProvider
    "ngResource" // $resource
    //--my dependences
  ])
    .config(["$urlRouterProvider", $urlRouterProvider => { $urlRouterProvider.otherwise("/persons"); }])
    .config([
    "$stateProvider", $stateProvider => {
        $stateProvider
          .state("persons",
          {
            url: "/persons",
            views: {
              "": {
                templateUrl: "scripts/spa/Templates/persons.tpl.html",
                controller: "PersonListController",
              }
            }
          })
          .state("reports",
          {
            url: "/reports",
            views: {
              "": {
                templateUrl: "scripts/spa/Templates/reports.tpl.html"
              }
            }
          })
          .state("phoneList",
          {
            url: "/phoneList",
            views: {
              "": {
                templateUrl: "scripts/spa/Templates/PhoneListReport.tpl.html",
                controller: "ReportController",
                controllerAs: "vm"
              }
            }
          })
          ;
      }
  ]);
}
