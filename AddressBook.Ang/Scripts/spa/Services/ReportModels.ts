module AddressBookApp {
  export interface IPhoneList extends ng.resource.IResource<IPhoneList> {
    personId: number;
    personName: string;
    phoneId: number;
    personPhone: string;
  }
  export interface IPhoneCount extends ng.resource.IResource<IPhoneCount> {
    personId: number;
    personName: string;
    phones: number;
  }

}