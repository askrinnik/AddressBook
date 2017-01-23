module AddressBookApp {
  export interface IReportResource extends ng.resource.IResourceClass<any> {
    getPhoneList(): Array<IPhoneList>;
    getPhoneCount(): Array<IPhoneCount>;
  }


}