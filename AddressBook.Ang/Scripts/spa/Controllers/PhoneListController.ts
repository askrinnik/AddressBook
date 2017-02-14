module AddressBookApp {
  "use strict";

  angular.module("AddressBook").controller("PhoneListController", ["$stateParams", "PhoneResource",
    function (stateParams, phoneResource: IPhoneResource) {
      phoneResource.getAllForPerson({ personId: stateParams.personId }).$promise.then(phones => this.phones = phones);

      this.deletePhone = function (phone: IPhone) {
        phoneResource.remove(phone).$promise
          .then(() => {
            const index = this.phones.indexOf(phone);
            this.phones.splice(index, 1);
          });
      }
      this.newPhone = function (phoneController) {
        const newPhone = new phoneResource();
        newPhone.id = 0;
        newPhone.phoneNumber = phoneController.newPhone;
        newPhone.personId = stateParams.personId;
        newPhone.$save().then(() => {
          this.phones.push(newPhone);
          phoneController.newPhone = null;
        });
      }

    }]);
}

