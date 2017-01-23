module AddressBookApp {
  "use strict";

  interface IReportControllerScope extends ng.IScope {
    phones: Array<IPhoneList>;
  }

  // from https://gist.github.com/scottmcarthur/9005953
  angular.module("AddressBook").controller("ReportController", ["ReportService",
    function (reportService: IReportResource) {
      reportService.getPhoneList()
        .$promise.then(phones => this.phones = phones);
    }
  ]);
}

