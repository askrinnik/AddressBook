(function () {
  "use strict";

  angular.module("AddressBook.Person").factory("PersonService", ["$resource", function ($resource) {
    return $resource("/api/person/:id", null,
      {
        //getADusersByCompany: { method: "GET", url: appSettings.pathToAPI + "ldap/users" },
      });
    }]);
})();