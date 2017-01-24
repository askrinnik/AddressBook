module AddressBookApp {
  "use strict";

  // from https://gist.github.com/scottmcarthur/9005953
  angular.module("AddressBook").controller("ReportController", ["ReportService",
    function (reportService: IReportResource) {
      reportService.getPhoneList()
        .$promise.then(phones => this.phones = phones);
      reportService.getPhoneCount()
        .$promise.then(phoneCount => this.phoneCount = phoneCount);
    }
  ]);
}

