module AddressBookApp {
  "use strict";

  // from https://gist.github.com/scottmcarthur/9005953
  angular.module("AddressBook").factory("PersonService", [ "$resource", ($resource: ng.resource.IResourceService): IPersonResource => {

    // Define your custom actions here as IActionDescriptor
    var getByName: ng.resource.IActionDescriptor = {
      method: "GET",
      url: "/api/person/GetByName/:personName",
      isArray: true
    };

    const actions: ng.resource.IActionHash = {
      getByName: getByName
    };

    // Return the resource, include your custom actions
    return $resource("/api/person/:id", null, actions) as IPersonResource;

  }]);

}