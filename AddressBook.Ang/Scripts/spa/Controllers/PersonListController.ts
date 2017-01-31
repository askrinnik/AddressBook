module AddressBookApp {
  "use strict";

  class PersonListViewModel {
    constructor(private personService: IPersonResource) {
       this.isShowProgress = false;
    }

    persons: Array<IPerson>;
    isShowProgress: boolean;
    searchText: string;
    getAll() {
      this.isShowProgress = true;
      this.personService.query()
        .$promise.then(persons => this.persons = persons)
        .finally(() => this.isShowProgress = false);
    }
    getByName() {
      if (this.searchText === "") {
        this.getAll();
        return;
      }
      this.isShowProgress = true;
      this.personService.getByName({ personName: this.searchText })
        .$promise.then(persons => this.persons = persons)
        .catch(reason => {
          console.error(reason.data.MessageDetail);
        })
        .finally(() => this.isShowProgress = false);
    }
    deletePerson(person: IPerson) {
      this.personService.remove({id: person.Id }).$promise
//      person.$remove() // doesn't work because we have field 'Id' but 'id' is required. Couldn't figureout how to fix
        .then(() => {
        const index = this.persons.indexOf(person);
        this.persons.splice(index, 1);
      });
    }

  }
  interface IPersonListControllerScope extends ng.IScope {
    vm: PersonListViewModel;
  }

  // from https://gist.github.com/scottmcarthur/9005953
  angular.module("AddressBook").controller("PersonListController", ["$scope", "PersonService",
    ($scope: IPersonListControllerScope, personService: IPersonResource) => {
      $scope.vm = new PersonListViewModel(personService);
      $scope.vm.getAll();

    // Get specific employee, and change their last name
    //let employee = personService.get({ id: 123 });
    //employee.surName = "Smith";
    //employee.$save();

    // Custom action
    //var updatedEmployee: IPerson = personService.update({ id: 100, firstName: "John" });
  }]);
}

