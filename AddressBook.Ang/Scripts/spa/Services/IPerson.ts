module AddressBookApp {
  export interface IPerson extends ng.resource.IResource<IPerson> {
    Id: number;
    Name: string;
    SurName: string;
    BirthDay: Date;
//    $update(): ng.IPromise<IPerson>;
  }
}