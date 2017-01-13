///// <reference path="../typings/angular-ui-router/index.d.ts"/>

module AddressBookApp {
  "use strict";

  angular.module("AddressBook", [
    //--vendors
//    "ui.router", // $stateProvider
    "ngResource" // $resource
    //--my dependences
  ])/*.config([
    "$stateProvider", function ($stateProvider) {
      $stateProvider
        .state("userInfo",
        {
          url: "/userInfo",
          views: {
            "": {
              templateUrl: "src/scripts/spa/userProfile/templates/userInfo.html",
              controller: "UserInfoCtrl",
              controllerAs: "vm"
            }
          }
        });
    }
  ])*/;
}
