using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.Core.DataAccess
{
  public class AddressBookRepository : IAddressBookRepository
  {
    private readonly AddressBookContext _db;

    public AddressBookRepository(AddressBookContext context)
    {
      _db = context;
    }

    public Person[] GetPersons()
    {
      return _db.Persons.AsNoTracking().ToArray();
    }
    public Person[] GetPersons(string personName)
    {
      IQueryable<Person> persons = _db.Persons;
      if (!string.IsNullOrWhiteSpace(personName))
        persons = persons.Where(p => (p.Name + " " + p.SurName).Contains(personName));
      return persons.AsNoTracking().ToArray();
    }

    public Person GetPerson(int id)
    {
      return _db.Persons.Find(id);
    }

    public int CreatePerson(Person person)
    {
      _db.Persons.Add(person);
      _db.SaveChanges();
      return person.Id;
    }
    public void EditPerson(Person person)
    {
      _db.Entry(person).State = EntityState.Modified;
      _db.SaveChanges();
    }
    public void DeletePerson(int id)
    {
      var person = _db.Persons.Find(id);
      DeletePerson(person);
    }
    public void DeletePerson(Person person)
    {
      _db.Persons.Remove(person);
      _db.SaveChanges();
    }

    public async Task<IEnumerable<Phone>>  GetPersonPhones(int personId)
    {
      return await _db.Phones.Include(p => p.Person).Where(p => p.PersonId == personId).AsNoTracking().ToArrayAsync();
    }

    public void CreatePhone(Phone phone)
    {
      _db.Phones.Add(phone);
      _db.SaveChanges();
    }

    public Phone GetPhone(int id)
    {
      return _db.Phones.Find(id);
    }

    public void EditPhone(Phone phone)
    {
      _db.Entry(phone).State = EntityState.Modified;
      _db.SaveChanges();
    }

    public void DeletePhone(Phone phone)
    {
      _db.Phones.Remove(phone);
      _db.SaveChanges();
    }
  }
}
