module AddressBookApp {
  "use strict";

  angular.module("AddressBook").controller("EditPersonController", ["$state", "person",
    function (state, person: IPerson) {
      this.person = person;

      this.updateUser = (person: IPerson) => {
        person.$save().then(() => state.go("persons"));
      }
    }
  ]);
}

