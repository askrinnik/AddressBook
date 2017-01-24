module AddressBookApp {
  "use strict";

  angular.module("AddressBook").controller("ViewPersonController", ["person",
    function (person: IPerson) {
      this.person = person;
    }
  ]);
}

