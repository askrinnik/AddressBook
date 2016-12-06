(function () {
  "use strict";

  angular.module("AddressBook.Person").controller("PersonController", ["$scope", "PersonService", function ($scope, personService) {
    $scope.persons = personService.query();
  }]);

})();