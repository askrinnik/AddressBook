module AddressBookApp {
  export interface IPhone extends ng.resource.IResource<IPhone> {
    id: number;
    phoneNumber: string;
    personId: number;
  }
}