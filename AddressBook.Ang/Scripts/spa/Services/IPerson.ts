module AddressBookApp {
  export interface IPerson extends ng.resource.IResource<IPerson> {
    id: number;
    name: string;
    surName: string;
    birthDay: Date;
//    $update(): ng.IPromise<IPerson>;
  }
}