using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    Task<IEnumerable<Phone>> GetPersonPhones(int personId);
    void CreatePhone(Phone phone);
    Phone GetPhone(int id);
    void EditPhone(Phone phone);
    void DeletePhone(Phone phone);
    IQueryable<PhoneListModel> GetPhoneList();
    IQueryable<PhoneCountModel> GetPhoneCount();
  }
}