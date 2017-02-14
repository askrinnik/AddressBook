module AddressBookApp {
  "use strict";

  angular.module("AddressBook").controller("PhoneController", ["PhoneResource",
    function () {
      this.isEditing = false;
      this.editingPhone = null;

      this.editPhone = function(phone: IPhone) {
        this.isEditing = true;
        this.editingPhone = angular.copy(phone);
      }

      this.cancelEdit = function() {
        this.isEditing = false;
        this.editingPhone = null;
      }

      this.savePhone = function (phone: IPhone) {
        this.editingPhone.$save().then(() => {
          phone.phoneNumber = this.editingPhone.phoneNumber;
          this.isEditing = false;
          this.editingPhone = null;
        });
      }
    }
  ]);
}

