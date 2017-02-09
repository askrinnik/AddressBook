module AddressBookApp {
  "use strict";

  angular.module("AddressBook").controller("PhoneListController", ["$stateParams", "PhoneResource",
    function (stateParams, phoneResource: IPhoneResource) {
      phoneResource.getAllForPerson({ personId: stateParams.personId }).$promise.then(phones => this.phones = phones);
    }]);
}

