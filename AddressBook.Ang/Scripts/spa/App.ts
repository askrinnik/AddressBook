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
                templateUrl: "scripts/spa/persons.tpl.html",
                controller: "PersonListController",
                controllerAs: "vm"
              }
            }
          });
      }
  ]);
}
