module AddressBookApp {
  "use strict";

  interface IPersonControllerScope extends ng.IScope {
    persons: Array<IPerson>;
  }

  // from https://gist.github.com/scottmcarthur/9005953
  angular.module("AddressBook").controller("PersonController", ["$scope", "PersonService", ($scope: IPersonControllerScope, personService: IPersonResource) => {

    $scope.persons = personService.query();

    // Get specific employee, and change their last name
    //let employee = personService.get({ id: 123 });
    //employee.surName = "Smith";
    //employee.$save();

    // Custom action
    //var updatedEmployee: IPerson = personService.update({ id: 100, firstName: "John" });
  }]);
}

