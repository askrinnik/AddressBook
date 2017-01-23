module AddressBookApp {
  export interface IGetByNameParameters {
    personName: string
  }

  export interface IPersonResource extends ng.resource.IResourceClass<IPerson> {
    getByName(parameters: IGetByNameParameters): Array<IPerson>;
  }
}