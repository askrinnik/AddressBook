module AddressBookApp {
  "use strict";

  // from https://gist.github.com/scottmcarthur/9005953
  angular.module("AddressBook").factory("ReportService", [ "$resource", ($resource: ng.resource.IResourceService): IReportResource => {

    // Define your custom actions here as IActionDescriptor
    var getPhoneList: ng.resource.IActionDescriptor = {
      method: "GET",
      url: "/api/person/getPhoneList",
      isArray: true
    };

    const actions: ng.resource.IActionHash = {
      getPhoneList: getPhoneList
    };

    // Return the resource, include your custom actions
    return $resource("", null, actions) as IReportResource;

  }]);
}