module AddressBookApp {
  "use strict";

  export interface IGetAllForPersonParameters {
    personId: number
  }

  export interface IPhoneResource extends ng.resource.IResourceClass<IPhone> {
    getAllForPerson(parameters: IGetAllForPersonParameters): Array<IPhone>;
  }

  // from https://gist.github.com/scottmcarthur/9005953
  angular.module("AddressBook").factory("PhoneResource", ["$resource", ($resource: ng.resource.IResourceService): IPhoneResource  => {

    // Define your custom actions here as IActionDescriptor
    var getAllForPerson: ng.resource.IActionDescriptor = {
      method: "GET",
      url: "api/phone/person/:personId",
      isArray: true
    };

    const actions: ng.resource.IActionHash = {
      getAllForPerson: getAllForPerson
    };

    // Return the resource, include your custom actions
    return $resource("api/phone/:id", null, actions) as IPhoneResource;

  }]);

}