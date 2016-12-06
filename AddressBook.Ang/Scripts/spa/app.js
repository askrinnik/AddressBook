(function() {
  "use strict";
  angular.module("AddressBook",
    [
      // global dependencies

      //--vendors
      "ngResource",

      //--my dependences
     "AddressBook.Person"

    ]);
  angular.module("AddressBook.Person",[]);
})();