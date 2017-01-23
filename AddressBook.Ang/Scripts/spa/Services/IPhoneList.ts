module AddressBookApp {
  export interface IPhoneList extends ng.resource.IResource<IPhoneList> {
    PersonId: number;
    PersonName: string;
    PhoneId: number;
    PersonPhone: string;
  }
}