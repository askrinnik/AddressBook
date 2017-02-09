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
                controller: "PersonListController"
                // controllerAs: "vm" is not used for example how to use scope
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
          .state("phoneListReport",
          {
            url: "/phoneListReport",
            views: {
              "": {
                templateUrl: "scripts/spa/Templates/PhoneListReport.tpl.html",
                controller: "ReportController",
                controllerAs: "vm"
              }
            }
          })
          .state("phoneCountReport",
          {
            url: "/phoneCountReport",
            views: {
              "": {
                templateUrl: "scripts/spa/Templates/PhoneCountReport.tpl.html",
                controller: "ReportController",
                controllerAs: "vm"
              }
            }
          })
          .state("viewPerson",
          {
            url: "/viewPerson/:personId",
            views: {
              "": {
                templateUrl: "scripts/spa/Templates/ViewPerson.tpl.html",
                controller: "ViewPersonController",
                controllerAs: "vm"
              }
            },
            resolve: {
              person: [
                "$stateParams", "PersonService",
                (stateParams, personResource: IPersonResource) => personResource.get({ id: stateParams.personId }).$promise.then(data => data)
              ]
            }

          })
          .state("editPerson",
          {
            url: "/editPerson/:personId",
            views: {
              "": {
                templateUrl: "scripts/spa/Templates/EditPerson.tpl.html",
                controller: "EditPersonController",
                controllerAs: "vm"
              }
            },
            resolve: {
              person: [
                "$stateParams", "PersonService",
                (stateParams, personResource: IPersonResource) => personResource.get({ id: stateParams.personId }).$promise.then(data => data)
              ]
            }
          })
          .state("createPerson",
          {
            url: "/createPerson",
            views: {
              "": {
                templateUrl: "scripts/spa/Templates/EditPerson.tpl.html",
                controller: "EditPersonController",
                controllerAs: "vm"
              }
            },
            resolve: {
              person: [
                "PersonService",
                (personResource: IPersonResource) => {
// ReSharper disable once InconsistentNaming
                  const newPerson = new personResource();
                  newPerson.id = 0;
                  return newPerson;
                } 
              ]
            }
          })
          .state("phoneList",
          {
            url: "/phoneList/:personId",
            views: {
              "": {
                templateUrl: "scripts/spa/Templates/PhoneList.tpl.html",
                controller: "PhoneListController",
                controllerAs: "vm"
              }
            }
          })
          ;
      }
  ]);
}
