namespace AddressBook.Core.DataAccess
{
  public interface IAddressBookRepository
  {
    Person[] GetPersons();
    Person[] GetPersons(string personName);
    Person GetPerson(int id);
    int CreatePerson(Person person);
    void EditPerson(Person person);
    void DeletePerson(int id);
    void DeletePerson(Person person);
  }
}