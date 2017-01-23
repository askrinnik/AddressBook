module AddressBookApp {
  export interface IPerson extends ng.resource.IResource<IPerson> {
    name: string;
    surName: string;
    birthDay: Date;
//    $update(): ng.IPromise<IPerson>;
  }
}