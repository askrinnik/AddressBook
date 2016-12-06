module AddressBookApp {
  "use strict";

  // from https://gist.github.com/scottmcarthur/9005953

  export interface IPerson extends ng.resource.IResource<IPerson> {
    name: string;
    surName: string;
    birthDay: Date;
//    $update(): ng.IPromise<IPerson>;
  }

  export interface IPersonResource extends ng.resource.IResourceClass<IPerson> {
//    update(person): IPerson;
  }

  angular.module("AddressBook").factory("PersonService", [ "$resource", ($resource: ng.resource.IResourceService): IPersonResource => {

        // Define your custom actions here as IActionDescriptor
        //var updateAction: ng.resource.IActionDescriptor = {
        //  method: "PUT",
        //  isArray: false
        //};

        // Return the resource, include your custom actions
        return $resource("/api/person/:id", null,
        {
          //update: updateAction
        }) as IPersonResource;

  }]);

  //class PersonService {
  //  static $inject = ["$resource"];
  //  constructor(private resource: ng.resource.) {

  //  }
  //}
}