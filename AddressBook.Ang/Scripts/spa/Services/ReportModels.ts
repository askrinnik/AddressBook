module AddressBookApp {
  export interface IPhoneList extends ng.resource.IResource<IPhoneList> {
    PersonId: number;
    PersonName: string;
    PhoneId: number;
    PersonPhone: string;
  }
  export interface IPhoneCount extends ng.resource.IResource<IPhoneCount> {
    PersonId: number;
    PersonName: string;
    Phones: number;
  }

}